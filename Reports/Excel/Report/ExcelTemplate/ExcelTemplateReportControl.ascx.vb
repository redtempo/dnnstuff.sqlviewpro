'***************************************************************************/
'* ExcelTemplateReportControl.ascx.vb
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
Imports FlexCel.Core
Imports FlexCel.XlsAdapter


Namespace DNNStuff.SQLViewPro.ExcelReports

    Partial Class ExcelTemplateReportControl
        Inherits DNNStuff.SQLViewPro.Controls.ReportControlBase

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()

            Try
                If IsEditMode() Then
                    Controls.Add(New LiteralControl("<strong>Please switch to view mode to generate Excel Template file</strong>"))
                Else
                    rdoExcelType.Visible = False
                    lblChoose.Visible = False
                    If ReportExtra.XlsFileName.Length > 0 And ReportExtra.XlsxFileName.Length > 0 Then
                        rdoExcelType.Visible = True
                        lblChoose.Visible = True
                    ElseIf ReportExtra.XlsFileName.Length > 0 Then
                        ProcessExcelTemplate(ReportExtra.XlsFileName)
                    ElseIf ReportExtra.XlsxFileName.Length > 0 Then
                        ProcessExcelTemplate(ReportExtra.XlsxFileName)
                    End If
                End If
            Catch ex As Exception
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(Me, ex)
            End Try
        End Sub


#End Region

#Region " Page"

        Private _ReportExtra As ExcelTemplateReportSettings = New ExcelTemplateReportSettings
        Private ReadOnly Property ReportExtra() As ExcelTemplateReportSettings
            Get
                Return _ReportExtra
            End Get
        End Property

#End Region

#Region " Base Method Implementations"
        Public Overrides Sub LoadRuntimeSettings(ByVal Settings As ReportInfo)
            _ReportExtra = DirectCast(DeserializeObject(Settings.ReportConfig, GetType(ExcelTemplateReportSettings)), ExcelTemplateReportSettings)
        End Sub
#End Region

#Region " Excel Template"
        Private Sub ProcessExcelTemplate(ByVal fileId As String)

            Dim ds As DataSet = ReportData()

            ' add debug info
            If State.ReportSet.ReportSetDebug Then
                DebugInfo.Append(QueryText)
            End If

            If ds.Tables.Count = 0 OrElse ds.Tables(0).Rows.Count = 0 Then
                RenderNoItems()
            Else
                RenderExcelTemplate(ds.Tables(0), fileId)
            End If
        End Sub

        Private Sub RenderExcelTemplate(ByVal dt As DataTable, ByVal fileId As String)
            Dim dataPath As String = Server.MapPath(ResolveUrl("Templates"))
            Dim fileName As String = Services.File.GetFilenameFromFileId(fileId)

            Dim xls As XlsFile = New XlsFile(fileName)

            ' save current sheet reference for later
            Dim activeSheetSaved As String = xls.ActiveSheetByName

            xls.ActiveSheetByName = ReportExtra.DataSheetName
            ' handle header row
            If ReportExtra.ContainsHeaderRow Then
                ' keep header row
                xls.DeleteRange(New TXlsCellRange(2, 1, FlxConsts.Max_Rows + 1, FlxConsts.Max_Columns + 1), TFlxInsertMode.NoneDown)
            Else
                ' clear sheet
                xls.ClearSheet()
                ' headings
                For col As Integer = 0 To dt.Columns.Count - 1
                    xls.SetCellFromString(1, col + 1, dt.Columns(col).ColumnName)
                Next col
            End If

            ' update data
            For row As Integer = 0 To dt.Rows.Count - 1
                For col As Integer = 0 To dt.Columns.Count - 1
                    xls.SetCellValue(row + 2, col + 1, dt.Rows(row).Item(col))
                Next
            Next

            xls.ActiveSheetByName = activeSheetSaved

            ' update range
            'Dim range As TXlsNamedRange = New TXlsNamedRange()
            'range.Name = "DEPTDATA"
            'range.RangeFormula = String.Format("='{0}'!$A$1:$H${1}", xls.ActiveSheetByName, dt.Rows.Count + 1)
            'xls.SetNamedRange(range)

            ' determine file extension
            Dim fileExtension As String = "xls"
            If fileName.EndsWith("xlsx") Then fileExtension = "xlsx"

            ' stream to user
            Using ms As MemoryStream = New MemoryStream()
                xls.Save(ms)

                Dim details As New ExportDetails
                With details
                    .Dataset = Nothing
                    .Filename = ReportExtra.OutputFileName.Replace("[TICKS]", Now.Ticks.ToString) & "." & fileExtension
                    .Disposition = ReportExtra.DispositionType
                End With

                ' write tmp file
                Dim filePath As String = Server.MapPath(ResolveUrl(String.Format("{0}.dat", System.Guid.NewGuid().ToString())))
                Dim fs As FileStream = File.OpenWrite(filePath)
                fs.Write(ms.GetBuffer(), 0, Convert.ToInt32(ms.Length))
                fs.Close()
                details.BinaryFilename = filePath

                Session(Export.EXPORT_KEY) = details

                If Request.ServerVariables("HTTP_USER_AGENT").Contains("ipad") Or Request.ServerVariables("HTTP_USER_AGENT").Contains("iphone") Then
                    '' no iframe for iphone, ipad
                    Response.Redirect(String.Format("{0}?ModuleId={1}&TabId={2}", ResolveUrl("~/DesktopModules/DNNStuff - SQLViewPro/Export.aspx"), State.ModuleId, State.TabId))
                Else
                    Controls.Add(New LiteralControl(String.Format("<iframe style='display:none' scrolling='auto' src='{0}?ModuleId={1}&TabId={2}'></iframe>", ResolveUrl("~/DesktopModules/DNNStuff - SQLViewPro/Export.aspx"), State.ModuleId, State.TabId)))
                End If


            End Using

        End Sub
#End Region

        Private Sub rdoExcelType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles rdoExcelType.SelectedIndexChanged
            Select Case rdoExcelType.SelectedValue
                Case "xls"
                    ProcessExcelTemplate(ReportExtra.XlsFileName)
                Case "xlsx"
                    ProcessExcelTemplate(ReportExtra.XlsxFileName)
            End Select
        End Sub
    End Class

End Namespace
