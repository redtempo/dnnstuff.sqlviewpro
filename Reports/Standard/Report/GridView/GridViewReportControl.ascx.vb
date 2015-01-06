'***************************************************************************/
'* GridViewReportControl.ascx.vb
'*
'* Copyright (c) 2004 by DNNStuff.
'* All rights reserved.
'*
'* Date:        August 9, 2004
'* Author:      Richard Edwards
'* Description: Grid View Report
'*************/
Imports DotNetNuke
Imports System.Configuration
Imports System.IO
Imports System.Text.RegularExpressions
Imports DNNStuff.SQLViewPro.Controls
Imports DNNStuff.SQLViewPro.Serialization

Namespace DNNStuff.SQLViewPro.StandardReports

    Public Class GridViewReportControl
        Inherits DNNStuff.SQLViewPro.Controls.ReportControlBase

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()

            LoadReportExtra()
            RenderHeader()
            FormatGrid()
            RenderFooter()

            Try
                BindGrid()
                ' miscellaneous
                cmdExportExcel.Visible = ReportExtra.EnableExcelExport
                cmdExportExcel.Text = ReportExtra.ExcelExportButtonCaption
            Catch ex As Exception
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Protected WithEvents phHeader As System.Web.UI.WebControls.PlaceHolder
        Protected WithEvents phFooter As System.Web.UI.WebControls.PlaceHolder
        Protected WithEvents gvGrid As System.Web.UI.WebControls.GridView
        Protected WithEvents cmdExportExcel As System.Web.UI.WebControls.LinkButton

#End Region

#Region " Page"

        Private Property ReportExtra As GridReportSettings = New GridReportSettings

        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load


        End Sub

#End Region

#Region " Data"
        Private Sub LoadReportExtra()

            ' load from database
            If Not Report Is Nothing Then
                ReportExtra = DirectCast(DeserializeObject(Report.ReportConfig, GetType(GridReportSettings)), GridReportSettings)
            End If

        End Sub
#End Region

#Region " Debug"
        Sub RenderDebug(ByVal Query As String)
            If Query.Length > 0 Then
                ' debug
                Dim ctrl As New HtmlControls.HtmlGenericControl("div")
                With ctrl
                    .InnerText = Query
                    .Attributes.Add("class", Report.ReportTheme & "_Debug")
                End With
                phHeader.Controls.Add(ctrl)
            End If
        End Sub
#End Region

#Region " Grid"
        Sub FormatGrid()
            ' format grid
            gvGrid.CssClass = Report.ReportTheme & "_Grid"
            gvGrid.HeaderStyle.CssClass = Report.ReportTheme & "_GridHeader"
            gvGrid.RowStyle.CssClass = Report.ReportTheme & "_GridItem"
            gvGrid.AlternatingRowStyle.CssClass = Report.ReportTheme & "_GridAlternatingItem"

            ' sorting
            gvGrid.AllowSorting = ReportExtra.AllowSorting

            gvGrid.AllowPaging = ReportExtra.AllowPaging
            If gvGrid.AllowPaging Then
                gvGrid.PageSize = ReportExtra.PageSize
                gvGrid.PagerSettings.Mode = DirectCast(System.Enum.Parse(GetType(PagerMode), ReportExtra.PagerMode), PagerMode)
                gvGrid.PagerSettings.NextPageText = ReportExtra.NextPageText
                gvGrid.PagerSettings.PreviousPageText = ReportExtra.PrevPageText
                gvGrid.PagerStyle.CssClass = Report.ReportTheme & "_Pager"
                gvGrid.PagerSettings.Position = DirectCast(System.Enum.Parse(GetType(PagerPosition), ReportExtra.PagerPosition), PagerPosition)
            End If
        End Sub

        Sub RenderHeader()
            If Report.ReportHeaderText.Length > 0 Then
                ' header
                Dim ctrl As New HtmlControls.HtmlGenericControl("div")
                With ctrl

                    .InnerHtml = ReplaceTokens(Report.ReportHeaderText)
                    .Attributes.Add("class", Report.ReportTheme & "_Header")
                End With
                phHeader.Controls.Add(ctrl)
            End If
        End Sub

        Sub RenderFooter()
            If Report.ReportFooterText.Length > 0 Then
                ' Footer
                Dim ctrl As New HtmlControls.HtmlGenericControl("div")
                With ctrl
                    .InnerHtml = ReplaceTokens(Report.ReportFooterText)
                    .Attributes.Add("class", Report.ReportTheme & "_Footer")
                End With
                phFooter.Controls.Add(ctrl)
            End If
        End Sub

        Sub RenderNoItems()
            If Report.ReportNoItemsText.Length > 0 Then
                ' No Items
                Dim ctrl As New HtmlControls.HtmlGenericControl("div")
                With ctrl
                    .InnerHtml = ReplaceTokens(Report.ReportNoItemsText)
                    .Attributes.Add("class", Report.ReportTheme & "_NoItems")
                End With
                phFooter.Controls.AddAt(0, ctrl)
            End If
        End Sub

        Sub BindGrid()
            Dim QueryText As String = GetQueryText()
            ' debug ?
            If State.ReportSet.ReportSetDebug Then
                RenderDebug(QueryText)
            End If

            Dim ds As DataSet = Query.RetrieveData(QueryText, Report.ReportConnectionString)

            If gvGrid.Columns.Count = 0 And gvGrid.AutoGenerateColumns = False Then
                AddGridColumns(ds)
            End If

            If ds.Tables(0).Rows.Count = 0 Then
                gvGrid.Visible = (Report.ReportNoItemsText.Length = 0)
                RenderNoItems()
            Else
                gvGrid.DataSource = Nothing
                gvGrid.DataSource = ds
                gvGrid.DataBind()
            End If

        End Sub

        Private Sub AddGridColumns(ByVal ds As DataSet)
            Dim hiddenColumns As String() = ReportExtra.HideColumns.Split(","c)

            For Each c As DataColumn In ds.Tables(0).Columns
                If Array.IndexOf(hiddenColumns, c.ColumnName) < 0 Then

                    If IsDrilldown() And IsDrilldownColumn(c.ColumnName) Then
                        Dim dgt As TemplateField = New TemplateField
                        With dgt
                            .ItemTemplate = New DrilldownTemplate(c.ColumnName)
                            .HeaderText = c.Caption
                            .SortExpression = c.ColumnName
                        End With
                        gvGrid.Columns.Add(dgt)
                    Else
                        Dim dgc As BoundField = New BoundField
                        With dgc
                            .DataField = c.ColumnName
                            .HeaderText = c.Caption
                            .SortExpression = c.ColumnName
                        End With
                        gvGrid.Columns.Add(dgc)
                    End If
                End If

            Next
        End Sub

        Private Function GetQueryText() As String
            Dim QueryText As String = Report.ReportCommand
            Dim OrderByText As String = ReportExtra.OrderBy

            Dim SortExpression As String = ""
            Dim SortDirection As String = "ASC"

            ' retrieve viewstate
            SortExpression = CStr(ViewState("SortExpression"))
            SortDirection = CStr(ViewState("SortDirection"))

            ' bind data
            If QueryText <> "" Then
                QueryText = ReplaceTokens(QueryText)
                If SortExpression <> "" And gvGrid.AllowSorting Then
                    QueryText = QueryText & " ORDER BY [" & SortExpression & "] " & SortDirection
                ElseIf OrderByText.Length > 0 Then
                    QueryText = QueryText & " ORDER BY " & OrderByText
                End If
            End If
            Return QueryText
        End Function


        Sub SortCommand_OnClick(ByVal Source As Object, ByVal E As GridViewSortEventArgs) Handles gvGrid.Sorting

            Dim objGrid As GridView = DirectCast(Source, GridView)
            Dim SortExpression As String = ""
            Dim SortDirection As String = "ASC"

            ' retrieve viewstate
            SortExpression = CStr(ViewState("SortExpression"))
            SortDirection = CStr(ViewState("SortDirection"))

            If SortExpression = E.SortExpression Then
                If SortDirection = "ASC" Then
                    SortDirection = "DESC"
                Else
                    SortDirection = "ASC"
                End If
            Else
                SortExpression = E.SortExpression
                SortDirection = "ASC"
            End If

            ' save to view state
            ViewState("SortExpression") = SortExpression
            ViewState("SortDirection") = SortDirection

            BindGrid()
        End Sub

        Private Sub PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvGrid.PageIndexChanged
            gvGrid.PageIndex = e.NewPageIndex
            BindGrid()
        End Sub

        Private Sub gvGrid_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvGrid.RowCreated
            ' hide columns

            If e.Row.RowType = ListItemType.Header Then
                e.Row.Visible = Not ReportExtra.HideColumnHeaders
            End If
        End Sub

#End Region

#Region " Drilldown"

        Private Class DrilldownTemplate : Implements ITemplate
            Private _fieldName As String

            Sub New(ByVal fieldName As String)
                _fieldName = fieldName
            End Sub

            Sub InstantiateIn(ByVal container As Control) Implements ITemplate.InstantiateIn
                Dim linkbtn As LinkButton = New LinkButton
                linkbtn.ID = "lbDrilldown" & _fieldName
                AddHandler linkbtn.DataBinding, AddressOf BindHyperLinkColumn
                container.Controls.Add(linkbtn)
            End Sub

            Public Sub BindHyperLinkColumn(ByVal sender As Object, ByVal e As EventArgs)
                Dim linkbtn As LinkButton = CType(sender, LinkButton)
                Dim container As GridViewRow = CType(linkbtn.NamingContainer, GridViewRow)

                With linkbtn
                    .CommandName = "Drilldown|" & _fieldName
                    .CommandArgument = Convert.ToString(DataBinder.Eval((CType(container, GridViewRow)).DataItem, _fieldName))
                    .Text = Convert.ToString(DataBinder.Eval((CType(container, GridViewRow)).DataItem, _fieldName))
                End With

            End Sub
        End Class

        Private Sub gvGrid_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvGrid.RowCommand
            If e.CommandName.StartsWith("Drilldown|") Then
                Dim QueryText As String = GetQueryText()
                Dim ds As DataSet = Query.RetrieveData(QueryText, Report.ReportConnectionString)
                'TODO: fix this up
                Dim dr As DataRow = ds.Tables(0).Rows(0)

                MyBase.DrillDown(Me, New DrilldownEventArgs(Report.ReportId, e.CommandName.Replace("Drilldown|", ""), dr))
            End If
        End Sub
        Private Function IsDrilldown() As Boolean
            Return Report.ReportDrillDowns.Count > 0
        End Function

        Private Function IsDrilldownColumn(ByVal colName As String) As Boolean
            For Each obj As Object In Report.ReportDrillDowns
                Dim ri As ReportInfo = DirectCast(obj, ReportInfo)
                If ri.ReportDrilldownFieldname = colName Then Return True
            Next
            Return False
        End Function

        Private Function DrilldownReportByColumn(ByVal colName As String) As ReportInfo
            For Each obj As Object In Report.ReportDrillDowns
                Dim ri As ReportInfo = DirectCast(obj, ReportInfo)
                If ri.ReportDrilldownFieldname = colName Then Return ri
            Next
            Return Nothing
        End Function
#End Region

#Region " Export"
        Private Sub cmdExportExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdExportExcel.Click
            Dim QueryText As String = GetQueryText()
            Dim ds As DataSet = Query.RetrieveData(QueryText, Report.ReportConnectionString)
            DataSetToExcel.Convert(ds.Tables(0), Response, "excelexport.xls")
        End Sub

#End Region

    End Class

End Namespace
