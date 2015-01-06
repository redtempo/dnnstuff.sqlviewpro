'***************************************************************************/
'* ExcelTemplateReportSettings.ascx.vb
'*
'* Copyright (c) 2004 by DNNStuff.
'* All rights reserved.
'*
'* Date:        August 9, 2004
'* Author:      Richard Edwards
'* Description: Xml Report Settings Handler
'*************/

Imports System.Configuration
Imports System.Xml.Serialization
Imports DNNStuff.SQLViewPro.Controls
Imports DNNStuff.SQLViewPro.Serialization

Namespace DNNStuff.SQLViewPro.ExcelReports

    Partial Public Class ExcelTemplateReportSettingsControl
        Inherits ReportSettingsControlBase

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
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        End Sub

#End Region

#Region " Base Method Implementations"
        Protected Overrides ReadOnly Property LocalResourceFile() As String
            Get
                Return ResolveUrl("App_LocalResources/ExcelTemplateReportSettingsControl")
            End Get
        End Property

        Public Overrides Function UpdateSettings() As String

            Dim obj As ExcelTemplateReportSettings = New ExcelTemplateReportSettings
            With obj
                .DataSheetName = txtDataSheetName.Text
                .ContainsHeaderRow = chkContainsHeaderRow.Checked
                .XlsFileName = ctlXlsFileName.Url
                .XlsxFileName = ctlXlsxFileName.Url
                .OutputFileName = txtOutputFileName.Text
                .DispositionType = ddDispositionType.SelectedValue
            End With

            Return SerializeObject(obj, GetType(ExcelTemplateReportSettings))

        End Function

        Public Overrides Sub LoadSettings(ByVal settings As String)
            Dim obj As ExcelTemplateReportSettings = New ExcelTemplateReportSettings()
            If Not String.IsNullOrEmpty(settings) Then
                obj = DirectCast(DeserializeObject(settings, GetType(ExcelTemplateReportSettings)), ExcelTemplateReportSettings)
            End If
            With obj
                txtDataSheetName.Text = .DataSheetName
                chkContainsHeaderRow.Checked = .ContainsHeaderRow
                ctlXlsFileName.Url = .XlsFileName
                ctlXlsxFileName.Url = .XlsxFileName
                txtOutputFileName.Text = .OutputFileName

                ControlHelpers.InitDropDownByValue(ddDispositionType, .DispositionType)
            End With
        End Sub

#End Region

    End Class

#Region " Settings"
    <XmlRootAttribute(ElementName:="Settings", IsNullable:=False)> _
    Public Class ExcelTemplateReportSettings
        Public Property DataSheetName() As String
        Public Property OutputFileName() As String
        Public Property XlsFileName() As String
        Public Property XlsxFileName() As String
        Public Property ContainsHeaderRow() As Boolean
        Public Property DispositionType() As String = "attachment"
    End Class
#End Region

End Namespace
