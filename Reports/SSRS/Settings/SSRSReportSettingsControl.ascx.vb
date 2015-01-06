'***************************************************************************/
'* XmlReportSettings.ascx.vb
'*
'* Copyright (c) 2004 by DNNStuff.
'* All rights reserved.
'*
'* Date:        August 9, 2004
'* Author:      Richard Edwards
'* Description: Template Report Settings Handler
'*************/

Imports System.Configuration
Imports System.Xml.Serialization
Imports DNNStuff.SQLViewPro.Controls
Imports DNNStuff.SQLViewPro.Serialization
Imports System.Collections.Generic
Imports System.Linq

Namespace DNNStuff.SQLViewPro.SSRSReports

    Public Class SSRSReportSettingsControl
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


#End Region

#Region " Base Method Implementations"
        Protected Overrides ReadOnly Property LocalResourceFile() As String
            Get
                Return ResolveUrl("App_LocalResources/SSRSReportSettingsControl")
            End Get
        End Property

        Public Overrides Function UpdateSettings() As String

            Dim obj As SSRSReportSettings = New SSRSReportSettings
            With obj
                .ProcessingMode = ddlProcessingMode.SelectedValue

                ' remote
                .ReportServerUrl = txtReportServerUrl.Text
                .ReportServerReportPath = txtReportServerReportPath.Text
                .ReportServerUsername = txtReportServerUsername.Text
                .ReportServerPassword = txtReportServerPassword.Text
                .ReportServerDomain = txtReportServerDomain.Text

                ' local
                .LocalReportPath = "FileID=" & Request.Form(urlLocalReportPath.UniqueID & "$cboFiles")

                ' parameters
                .AdditionalParameters = txtAdditionalParameters.Text

                ' viewer
                .ViewerHeight = txtViewerHeight.Text
                .ViewerWidth = txtViewerWidth.Text

                ' toolbar
                Dim options As New Dictionary(Of String, String)
                For Each li As ListItem In lstReportOptions.Items
                    If li.Selected Then options.Add(li.Value, "1")
                Next
                .ToolbarOptions = String.Join(",", options.Select(Function(p) p.Key + "="c + p.Value).ToArray())
            End With

            Return SerializeObject(obj, GetType(SSRSReportSettings))

        End Function

        Public Overrides Sub LoadSettings(ByVal settings As String)
            Dim obj As SSRSReportSettings = New SSRSReportSettings()
            If Not String.IsNullOrEmpty(settings) Then
                obj = DirectCast(DeserializeObject(settings, GetType(SSRSReportSettings)), SSRSReportSettings)
            End If

            With obj
                ControlHelpers.InitDropDownByValue(ddlProcessingMode, .ProcessingMode)

                ' remote
                txtReportServerUrl.Text = .ReportServerUrl
                txtReportServerReportPath.Text = .ReportServerReportPath
                txtReportServerUsername.Text = .ReportServerUsername
                txtReportServerPassword.Attributes.Add("value", .ReportServerPassword) ' because it's a password field
                txtReportServerDomain.Text = .ReportServerDomain

                ' local
                urlLocalReportPath.Url = .LocalReportPath

                ' parameters
                txtAdditionalParameters.Text = .AdditionalParameters

                ' viewer
                txtViewerHeight.Text = .ViewerHeight
                txtViewerWidth.Text = .ViewerWidth

                ' toolbar
                Dim options As Dictionary(Of String, String) = StringHelpers.ToDictionary(.ToolbarOptions, ","c)
                For Each li As ListItem In lstReportOptions.Items
                    li.Selected = False
                    If options.ContainsKey(li.Value) Then
                        If options(li.Value) = "1" Then li.Selected = True
                    End If
                Next
            End With
            RefreshVisibility()
        End Sub

        Public Sub RefreshVisibility()
            Dim showRemote As Boolean = (ddlProcessingMode.SelectedValue = Common.ProcessingModeRemote)
            pnlRemote.Visible = showRemote
            pnlLocal.Visible = Not (showRemote)
        End Sub

#End Region

        Private Sub ddlProcessingMode_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlProcessingMode.SelectedIndexChanged
            RefreshVisibility()
        End Sub

    End Class

#Region " Settings"
    <XmlRootAttribute(ElementName:="Settings", IsNullable:=False)> _
    Public Class SSRSReportSettings
        Public Property ProcessingMode() As String = Common.ProcessingModeRemote
        Public Property ReportServerUrl() As String = "http://myserver/reportserver"
        Public Property ReportServerReportPath() As String = "/myreport"
        Public Property ReportServerUsername() As String = ""
        Public Property ReportServerPassword() As String = ""
        Public Property ReportServerDomain() As String = ""

        Public Property LocalReportPath() As String = ""

        Public Property AdditionalParameters() As String = ""

        Public Property ViewerHeight() As String = "80%"
        Public Property ViewerWidth() As String = "100%"

        Public Property ToolbarOptions() As String = "ShowBackButton=1,ShowDocumentMapButton=1,ShowExportControls=1,ShowFindControls=1,ShowPageNavigationControls=1,ShowParameterPrompts=1,ShowPrintButton=1,ShowPromptAreaButton=1,ShowRefreshButton=1,ShowToolBar=1,ShowZoomControl=1,ShowWaitControlCancelLink=1"
    End Class
#End Region

End Namespace
