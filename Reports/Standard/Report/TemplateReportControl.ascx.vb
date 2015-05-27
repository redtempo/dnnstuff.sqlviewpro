'***************************************************************************/
'* XmlReportControl.ascx.vb
'*
'* Copyright (c) 2004 by DNNStuff.
'* All rights reserved.
'*
'* Date:        August 9, 2004
'* Author:      Richard Edwards
'* Description: Xml Report
'*************/
Imports DotNetNuke
Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports System.Configuration
Imports System.IO
Imports System.Text.RegularExpressions
Imports DNNStuff.SQLViewPro.Controls
Imports DNNStuff.SQLViewPro.Serialization
Imports DNNStuff.Utilities
Imports System.Collections.Generic

Namespace DNNStuff.SQLViewPro.StandardReports

    Partial Class TemplateReportControl
        Inherits DNNStuff.SQLViewPro.Controls.ReportControlBase

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()

        End Sub


#End Region

#Region " Page"
        Private Const PAGINGTYPE_INTERNAL As String = "Internal"
        Private Const PAGINGTYPE_QUERYSTRING As String = "Querystring"

        Private Property ReportExtra As TemplateReportSettings = New TemplateReportSettings()

        Private Property PageNumber() As Int32
            Get
                Dim value As Int32 = 1
                If ReportExtra.PagingType = PAGINGTYPE_INTERNAL Then
                    If ViewState("pageNumber") IsNot Nothing Then
                        value = CInt(ViewState("pageNumber"))
                    End If
                Else
                    If Request.QueryString("pg") IsNot Nothing Then
                        value = CInt(Request.QueryString("pg"))
                    End If
                End If
                Return value
            End Get
            Set(value As Int32)
                If ReportExtra.PagingType = PAGINGTYPE_INTERNAL Then
                    ViewState("pageNumber") = value
                End If
            End Set
        End Property

        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                BindTemplateToData()

            Catch ex As Exception
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
#End Region

#Region " Base Method Implementations"
        Public Overrides Sub LoadRuntimeSettings(ByVal settings As ReportInfo)
            _ReportExtra = DirectCast(DeserializeObject(settings.ReportConfig, GetType(TemplateReportSettings)), TemplateReportSettings)
        End Sub
#End Region

#Region " Template"

        Private Sub BindTemplateToData()
            phContent.Controls.Clear()

            ' debug ?
            If State.ReportSet.ReportSetDebug Then
                DebugInfo.Append(QueryText)
            End If

            Dim ds As DataSet = ReportData()

            Dim maxPage As Int32 = 1
            If ReportExtra.AllowPaging Then
                maxPage = (ds.Tables(0).Rows.Count + ReportExtra.PageSize - 1) \ ReportExtra.PageSize
                Dim pageData As New List(Of DataRow)
                For i As Integer = ((PageNumber - 1) * ReportExtra.PageSize + 1) To ((PageNumber) * ReportExtra.PageSize)
                    If i > ds.Tables(0).Rows.Count Then Exit For
                    pageData.Add(ds.Tables(0).Rows(i - 1))
                Next
                Dim pageTable As DataTable = pageData.CopyToDataTable()
                ds = New DataSet()
                ds.Tables.Add(pageTable)
            End If

            If ds.Tables.Count = 0 OrElse ds.Tables(0).Rows.Count = 0 Then
                phContent.Visible = (Report.ReportNoItemsText.Length = 0)
                RenderNoItems()
            Else
                Dim result As String = ReportExtra.TemplateText

                ' TEMPORARY: until export services changed
                If result.Contains("[EXPORT_EXCEL]") Then
                    result = result.Replace("[EXPORT_EXCEL]", "")
                    cmdExportExcel.Visible = True
                End If

                ' paging support
                If ReportExtra.AllowPaging Then
                    Dim pagerControls As String = ""

                    Dim prevPage As Int32 = PageNumber - 1
                    If prevPage > 0 Then
                        ' insert previous page item
                        If ReportExtra.PagingType = PAGINGTYPE_INTERNAL Then
                            pagerControls = pagerControls & "<asp:LinkButton id=""cmdPrevPage"" runat=""server"" CommandName=""PrevPage"" CommandArgument=""" & prevPage & """>" & ReportExtra.PrevPageText & "</asp:LinkButton>"
                        Else
                            pagerControls = pagerControls & String.Format("<a href=""{0}"">{1}</a>", Url.ReplaceQueryStringParam(Request.Url.AbsoluteUri, "pg", prevPage), ReportExtra.PrevPageText)
                        End If

                    End If

                        Dim nextPage As Int32 = PageNumber + 1
                        If PageNumber < maxPage Then
                            ' insert next page
                        If ReportExtra.PagingType = PAGINGTYPE_INTERNAL Then
                            pagerControls = pagerControls & "<asp:LinkButton id=""cmdNextPage"" runat=""server"" CommandName=""NextPage"" CommandArgument=""" & nextPage & """>" & ReportExtra.NextPageText & "</asp:LinkButton>"
                        Else
                            pagerControls = pagerControls & String.Format("<a href=""{0}"">{1}</a>", Url.ReplaceQueryStringParam(Request.Url.AbsoluteUri, "pg", nextPage), ReportExtra.NextPageText)
                        End If

                    End If

                    ' make sure pager tag is present, if not add it
                    If Not result.Contains("[PAGER]") Then
                        result = result & "[PAGER]"
                    End If

                    ' inject pager controls
                    result = result.Replace("[PAGER]", pagerControls)
                    result = result.Replace("[PAGENUMBER]", PageNumber.ToString())
                    result = result.Replace("[PAGECOUNT]", maxPage.ToString())
                End If


                result = Text.RegularExpressions.Regex.Replace(result, "<drilldown name=\""(?<name>.*)\"">(?<inner>.*)</drilldown>", "<asp:LinkButton id=""cmdDrilldown[#RowNumber]"" runat=""server"" CommandName=""Drilldown|${name}"" CommandArgument=""[#RowNumber]"">${inner}</asp:LinkButton>", RegexOptions.IgnoreCase)
                result = ReplaceReportTokens(result, ds)
                phContent.Controls.Add(ParseControl(result))

                ' hook up command for drilldown links
                Dim drilldownCtrl As LinkButton
                Dim drilldownIndex As Integer = 1
                While True
                    drilldownCtrl = DirectCast(phContent.FindControl("cmdDrillDown" & drilldownIndex.ToString()), LinkButton)
                    If drilldownCtrl IsNot Nothing Then
                        AddHandler drilldownCtrl.Command, AddressOf CmdDrilldown
                        drilldownIndex += 1
                    Else
                        Exit While
                    End If
                End While

                ' hookup command for pager links for internal
                If ReportExtra.PagingType = PAGINGTYPE_INTERNAL Then
                    Dim cmdPrevPage As LinkButton
                    cmdPrevPage = DirectCast(phContent.FindControl("cmdPrevPage"), LinkButton)
                    If cmdPrevPage IsNot Nothing Then
                        AddHandler cmdPrevPage.Command, AddressOf Pager_Click
                    End If
                    Dim cmdNextPage As LinkButton
                    cmdNextPage = DirectCast(phContent.FindControl("cmdNextPage"), LinkButton)
                    If cmdNextPage IsNot Nothing Then
                        AddHandler cmdNextPage.Command, AddressOf Pager_Click
                    End If
                End If
            End If
        End Sub
#End Region

#Region "Paging"
        Private Sub Pager_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim btn As LinkButton = DirectCast(sender, LinkButton)
            If btn IsNot Nothing Then
                PageNumber = CInt(btn.CommandArgument)
                BindTemplateToData()
            End If
        End Sub
#End Region

#Region " Export"
        Private Sub cmdExportExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdExportExcel.Click
            Dim ds As DataSet = ReportData()
            Services.Export.Excel.Export(ds.Tables(0), Response, CleanFileName(Report.ReportName & ".xls"))
        End Sub
#End Region


#Region " Drilldown"
        Private Function IsDrilldown() As Boolean
            Return Report.ReportDrillDowns.Count > 0
        End Function

        Private Sub CmdDrilldown(ByVal source As Object, ByVal e As CommandEventArgs)
            If e.CommandName.StartsWith("Drilldown|") Then
                Dim row As Integer = Int32.Parse(e.CommandArgument.ToString())
                Dim ds As DataSet = ReportData()
                Dim dr As DataRow = ds.Tables(0).Rows(row - 1)

                MyBase.DrillDown(Me, New DrilldownEventArgs(Report.ReportId, e.CommandName.Replace("Drilldown|", ""), dr))
            End If
        End Sub
#End Region

    End Class

End Namespace
