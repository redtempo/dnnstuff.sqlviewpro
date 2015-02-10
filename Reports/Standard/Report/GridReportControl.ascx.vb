'***************************************************************************/
'* GridReportControl.ascx.vb
'*
'* Copyright (c) 2004 by DNNStuff.
'* All rights reserved.
'*
'* Date:        August 9, 2004
'* Author:      Richard Edwards
'* Description: Grid Report
'*************/
Imports DotNetNuke
Imports DotNetNuke.Common
Imports System.Configuration
Imports System.IO
Imports System.Text.RegularExpressions
Imports DNNStuff.SQLViewPro.Controls
Imports DNNStuff.SQLViewPro.Serialization

Namespace DNNStuff.SQLViewPro.StandardReports

    Partial Class GridReportControl
        Inherits DNNStuff.SQLViewPro.Controls.ReportControlBase

        Protected WithEvents cmdExportExcel As System.Web.UI.WebControls.LinkButton

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()

            ApplyGridFormatting()

        End Sub


#End Region

#Region " Page"

        Private Property ReportExtra() As GridReportSettings = New GridReportSettings()

        Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
            Try
                BindGridToData() ' needs to be in page load otherwise paging past first set of pages doesn't work

                ' miscellaneous
                If ReportExtra.EnableExcelExport Then
                    RenderExcelButton()
                End If
            Catch ex As Exception
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(Me, ex)
            End Try

        End Sub

#End Region

#Region " Base Method Implementations"
        Public Overrides Sub LoadRuntimeSettings(ByVal settings As ReportInfo)
            ReportExtra = DirectCast(DeserializeObject(settings.ReportConfig, GetType(GridReportSettings)), GridReportSettings)
        End Sub
#End Region
#Region "Export"
        Private Function RenderExcelExportButtonAsText() As String
            Return String.Format("<asp:LinkButton id=""{1}"" runat=""server"" CommandName=""Goto"" CommandArgument=""1"" OnClick=""{1}_Click"" cssClass=""CommandButton SQLViewProButton"">{0}</asp:LinkButton>", ReportExtra.ExcelExportButtonCaption, "cmdExportExcel")
        End Function

        Private Sub RenderExcelButton()
            Dim c As Control = Nothing
            Dim pnl As Control = Nothing

            c = ParseControl(RenderExcelExportButtonAsText())

            If ReportExtra.ExcelExportPosition = "Top" Then
                pnl = pnlHeader
            Else
                pnl = pnlFooter
            End If
            pnl.Controls.Add(c)

            ' set action buttons
            Dim obj As Control = FindControlRecursive(pnl, "cmdExportExcel")

            If obj IsNot Nothing Then
                cmdExportExcel = DirectCast(obj, LinkButton)
            End If

        End Sub

        Private Sub cmdExportExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdExportExcel.Click
            Dim ds As DataSet = ReportData()
            Dim details As New ExportDetails
            With details
                .Dataset = ds
                .Filename = CleanFileName(Report.ReportName & ".xls")
            End With
            Session(Export.EXPORT_KEY) = details
            Controls.Add(New LiteralControl(String.Format("<iframe style='display:none' scrolling='auto' src='{0}?ModuleId={1}&TabId={2}'></iframe>", ResolveUrl("~/DesktopModules/DNNStuff - SQLViewPro/Export.aspx"), State.ModuleId, State.TabId)))
        End Sub

#End Region
#Region " Grid"
        Private Sub ApplyGridFormatting()
            ' format grid
            dgCommand.CssClass = Report.ReportTheme & "_Grid"
            dgCommand.HeaderStyle.CssClass = Report.ReportTheme & "_GridHeader"
            dgCommand.ItemStyle.CssClass = Report.ReportTheme & "_GridItem"
            dgCommand.AlternatingItemStyle.CssClass = Report.ReportTheme & "_GridAlternatingItem"

            ' sorting
            dgCommand.AllowSorting = ReportExtra.AllowSorting

            dgCommand.AllowPaging = ReportExtra.AllowPaging
            If dgCommand.AllowPaging Then
                dgCommand.PageSize = ReportExtra.PageSize
                dgCommand.PagerStyle.Mode = DirectCast(System.Enum.Parse(GetType(PagerMode), ReportExtra.PagerMode), PagerMode)
                dgCommand.PagerStyle.NextPageText = ReportExtra.NextPageText
                dgCommand.PagerStyle.PrevPageText = ReportExtra.PrevPageText
                dgCommand.PagerStyle.CssClass = Report.ReportTheme & "_Pager"
                dgCommand.PagerStyle.Position = DirectCast(System.Enum.Parse(GetType(PagerPosition), ReportExtra.PagerPosition), PagerPosition)
            End If
        End Sub

        Private Sub BindGridToData()
            Dim gridQuery As String = GetQueryText()

            ' debug ?
            If State.ReportSet.ReportSetDebug Then
                DebugInfo.Append(gridQuery)
            End If

            Dim ds As DataSet = ReportData(gridQuery)

            If dgCommand.Columns.Count = 0 And dgCommand.AutoGenerateColumns = False Then
                If ds.Tables.Count > 0 Then AddGridColumns(ds)
            End If

            If ds.Tables.Count = 0 OrElse ds.Tables(0).Rows.Count = 0 Then
                dgCommand.Visible = (Report.ReportNoItemsText.Length = 0)
                RenderNoItems()
            Else
                dgCommand.DataSource = Nothing
                dgCommand.DataSource = ds
                dgCommand.DataBind()
            End If

        End Sub

        Private Sub AddGridColumns(ByVal ds As DataSet)
            Dim hiddenColumns As String() = ReportExtra.HideColumns.Split(","c)

            For Each c As DataColumn In ds.Tables(0).Columns
                If Array.IndexOf(hiddenColumns, c.ColumnName) < 0 Then

                    If IsDrilldown() And IsDrilldownColumn(c.ColumnName) Then
                        Dim dgt As TemplateColumn = New TemplateColumn
                        With dgt
                            .ItemTemplate = New DrilldownTemplate(c.ColumnName)
                            .HeaderText = c.Caption
                            .SortExpression = c.ColumnName
                        End With
                        dgCommand.Columns.Add(dgt)
                    Else
                        Dim dgc As BoundColumn = New BoundColumn
                        With dgc
                            .DataField = c.ColumnName
                            .HeaderText = c.Caption
                            .SortExpression = c.ColumnName
                        End With
                        dgCommand.Columns.Add(dgc)
                    End If
                End If

            Next
        End Sub
        Private Property SortExpression() As String
            Get
                Dim value As String = CStr(ViewState("SortExpression"))
                If value = "" Then value = ReportExtra.OrderBy.Replace(" DESC", "").Replace(" ASC", "")
                Return value
            End Get
            Set(value As String)
                ViewState("SortExpression") = value
            End Set
        End Property
        Private Property SortDirection() As String
            Get
                Dim value As String = CStr(ViewState("SortDirection"))
                If value = "" Then
                    value = "ASC"
                    If ReportExtra.OrderBy.Contains(" ASC") Then
                        value = "ASC" ' NOTE: doing this step in case someone sorts by DESCRIPTION or something else with DESC in it and wants to specifically sort ascending
                    ElseIf ReportExtra.OrderBy.Contains(" DESC") Then
                        value = "DESC"
                    End If
                End If
                Return value
            End Get
            Set(value As String)
                ViewState("SortDirection") = value
            End Set
        End Property

        Private Function GetQueryText() As String
            Dim query As String = QueryText

            ' bind data
            If query <> "" Then
                If dgCommand.AllowSorting Then
                    If query.Contains("[SORTEXPRESSION]") Or query.Contains("[SORTDIRECTION]") Then ' handling in stored proc likely
                        query = query.Replace("[SORTEXPRESSION]", SortExpression).Replace("[SORTDIRECTION]", SortDirection)
                    Else
                        If SortExpression <> "" Then
                            If Not query.Contains("ORDER BY") Then
                                query = query & String.Format(" ORDER BY {0}{1}{2} {3}", Report.ReportIdentifierQuoteStartCharacter, SortExpression, Report.ReportIdentifierQuoteEndCharacter, SortDirection)
                            End If
                        End If
                    End If
                End If
            End If
            Return query
        End Function

        Private Sub ToggleSort(ByVal newSortExpression As String)

            If SortExpression.Equals(newSortExpression, StringComparison.InvariantCultureIgnoreCase) Then
                If SortDirection = "ASC" Then
                    SortDirection = "DESC"
                Else
                    SortDirection = "ASC"
                End If
            Else
                SortExpression = newSortExpression
                SortDirection = "ASC"
            End If
        End Sub

        Private Sub SortCommand_OnClick(ByVal Source As Object, ByVal E As DataGridSortCommandEventArgs) Handles dgCommand.SortCommand
            ToggleSort(E.SortExpression)
            BindGridToData()
        End Sub

        Private Sub PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgCommand.PageIndexChanged
            dgCommand.CurrentPageIndex = e.NewPageIndex
            BindGridToData()
        End Sub

        Private Sub dgCommand_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgCommand.ItemCreated
            ' hide columns
            If e.Item.ItemType = ListItemType.Header Then
                e.Item.Visible = Not ReportExtra.HideColumnHeaders
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

            Private Sub BindHyperLinkColumn(ByVal sender As Object, ByVal e As EventArgs)
                Dim linkbtn As LinkButton = CType(sender, LinkButton)
                Dim container As DataGridItem = CType(linkbtn.NamingContainer, DataGridItem)

                With linkbtn
                    .CommandName = "Drilldown|" & _fieldName
                    .CommandArgument = Convert.ToString(DataBinder.Eval(container.DataItem, _fieldName))
                    .Text = Convert.ToString(DataBinder.Eval(container.DataItem, _fieldName))
                End With
            End Sub
        End Class

        Private Sub dgCommand_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgCommand.ItemCommand
            If e.CommandName.StartsWith("Drilldown|") Then
                Dim query As String = GetQueryText()
                Dim ds As DataSet = ReportData(query)
                Dim dr As DataRow = ds.Tables(0).Rows(e.Item.DataSetIndex)

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


    End Class

End Namespace
