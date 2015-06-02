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
                    Dim pagerPrevious As String = ""
                    Dim pagerNext As String = ""
                    Dim pagerFirst As String = ""
                    Dim pagerLast As String = ""
                    Dim pagerPages As String = ""

                    ' previous
                    Dim prevPage As Int32 = PageNumber - 1
                    If prevPage > 0 Then
                        ' insert previous page item
                        If ReportExtra.PagingType = PAGINGTYPE_INTERNAL Then
                            pagerPrevious = "<asp:LinkButton id=""cmdPreviousPage"" runat=""server"" CommandArgument=""" & prevPage & """>" & ReportExtra.PrevPageText & "</asp:LinkButton>"
                        Else
                            pagerPrevious = PageTemplate(prevPage, ReportExtra.PrevPageText, "prev")
                        End If
                    End If
                    ' next
                    Dim nextPage As Int32 = PageNumber + 1
                    If PageNumber < maxPage Then
                        ' insert next page
                        If ReportExtra.PagingType = PAGINGTYPE_INTERNAL Then
                            pagerNext = "<asp:LinkButton id=""cmdNextPage"" runat=""server"" CommandArgument=""" & nextPage & """>" & ReportExtra.NextPageText & "</asp:LinkButton>"
                        Else
                            pagerNext = PageTemplate(nextPage, ReportExtra.NextPageText, "next")
                        End If
                    End If
                    ' first
                    If PageNumber > 1 Then
                        If ReportExtra.PagingType = PAGINGTYPE_INTERNAL Then
                            pagerFirst = "<asp:LinkButton id=""cmdFirstPage"" runat=""server"" CommandArgument=""" & 1 & """>" & ReportExtra.FirstPageText & "</asp:LinkButton>"
                        Else
                            pagerFirst = PageTemplate(1, ReportExtra.FirstPageText, "first")
                        End If
                    End If
                    ' last
                    If PageNumber < maxPage Then
                        If ReportExtra.PagingType = PAGINGTYPE_INTERNAL Then
                            pagerLast = "<asp:LinkButton id=""cmdLastPage"" runat=""server"" CommandArgument=""" & maxPage & """>" & ReportExtra.LastPageText & "</asp:LinkButton>"
                        Else
                            pagerLast = PageTemplate(maxPage, ReportExtra.LastPageText, "last")
                        End If
                    End If
                    ' pages
                    If ReportExtra.PagingType = PAGINGTYPE_QUERYSTRING Then
                        For i As Integer = 1 To Math.Min(10, maxPage)
                            pagerPages = pagerPages & PageTemplate(i, i, "page")
                        Next
                    End If

                    ' check if no [PAGER*] tokens present, otherwise use a default
                    If Not result.Contains("[PAGER") Then
                        result = result & "[PAGER]"
                    End If

                    ' inject pager controls
                    result = result.Replace("[PAGER]", "[PAGER:FIRST][PAGER:PREVIOUS][PAGER:PAGES][PAGER:NEXT][PAGER:LAST]")
                    result = result.Replace("[PAGER:FIRST]", pagerFirst)
                    result = result.Replace("[PAGER:LAST]", pagerLast)
                    result = result.Replace("[PAGER:PREVIOUS]", pagerPrevious)
                    result = result.Replace("[PAGER:NEXT]", pagerNext)
                    result = result.Replace("[PAGER:PAGES]", pagerPages)
                    result = result.Replace("[PAGER:NUMBER]", PageNumber.ToString())
                    result = result.Replace("[PAGER:COUNT]", maxPage.ToString())
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
                    Dim cmdPreviousPage As LinkButton
                    cmdPreviousPage = DirectCast(phContent.FindControl("cmdPreviousPage"), LinkButton)
                    If cmdPreviousPage IsNot Nothing Then
                        AddHandler cmdPreviousPage.Command, AddressOf Pager_Click
                    End If
                    Dim cmdNextPage As LinkButton
                    cmdNextPage = DirectCast(phContent.FindControl("cmdNextPage"), LinkButton)
                    If cmdNextPage IsNot Nothing Then
                        AddHandler cmdNextPage.Command, AddressOf Pager_Click
                    End If
                    Dim cmdFirstPage As LinkButton
                    cmdFirstPage = DirectCast(phContent.FindControl("cmdFirstPage"), LinkButton)
                    If cmdFirstPage IsNot Nothing Then
                        AddHandler cmdFirstPage.Command, AddressOf Pager_Click
                    End If
                    Dim cmdLastPage As LinkButton
                    cmdLastPage = DirectCast(phContent.FindControl("cmdLastPage"), LinkButton)
                    If cmdLastPage IsNot Nothing Then
                        AddHandler cmdLastPage.Command, AddressOf Pager_Click
                    End If
                End If
            End If
        End Sub
        Private Function PageTemplate(page As Integer, text As String, type As String) As String
            Return ReportExtra.PageTemplate.Replace("[URL]", Url.ReplaceQueryStringParam(Request.Url.AbsoluteUri, "pg", page)).Replace("[TEXT]", text).Replace("[TYPE]", type)
        End Function
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
