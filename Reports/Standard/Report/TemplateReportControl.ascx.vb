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

        Private Property ReportExtra As TemplateReportSettings = New TemplateReportSettings()

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
            _ReportExtra = DirectCast(DeserializeObject(Settings.ReportConfig, GetType(TemplateReportSettings)), TemplateReportSettings)
        End Sub
#End Region

#Region " Template"

        Private Sub BindTemplateToData()

            ' debug ?
            If State.ReportSet.ReportSetDebug Then
                DebugInfo.Append(QueryText)
            End If

            Dim ds As DataSet = ReportData()

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

                result = System.Text.RegularExpressions.Regex.Replace(result, "<drilldown name=\""(?<name>.*)\"">(?<inner>.*)</drilldown>", "<asp:LinkButton id=""cmdDrilldown[#RowNumber]"" runat=""server"" CommandName=""Drilldown|${name}"" CommandArgument=""[#RowNumber]"">${inner}</asp:LinkButton>", RegexOptions.IgnoreCase)
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
