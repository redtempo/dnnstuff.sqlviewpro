'***************************************************************************/
'* ExcelReportControl.ascx.vb
'*
'* Copyright (c) 2004 by DNNStuff.
'* All rights reserved.
'*
'* Date:        August 9, 2004
'* Author:      Richard Edwards
'* Description: Excel Report
'*************/
Imports DotNetNuke
Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports System.Configuration
Imports System.IO
Imports System.Text.RegularExpressions
Imports DNNStuff.SQLViewPro.Controls
Imports DNNStuff.SQLViewPro.Serialization

Namespace DNNStuff.SQLViewPro.StandardReports

    Partial Class ExcelReportControl
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
                ExportReport()
            Catch ex As Exception
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

#End Region

#Region " Page"

        Private Property ReportExtra() As NoReportSettings = New NoReportSettings()

#End Region

#Region " Base Method Implementations"

        Public Overrides Sub LoadRuntimeSettings(ByVal settings As ReportInfo)
            ReportExtra = DirectCast(DeserializeObject(settings.ReportConfig, GetType(NoReportSettings)), NoReportSettings)
        End Sub

#End Region

#Region " Excel"

        Private Sub ExportReport()
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

    End Class

End Namespace
