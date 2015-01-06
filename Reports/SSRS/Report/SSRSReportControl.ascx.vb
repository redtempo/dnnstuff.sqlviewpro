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
Imports System.Net
Imports System.Runtime.InteropServices
Imports System.Collections.Generic
Imports Microsoft.Reporting.WebForms
Imports System.Security.Principal

Namespace DNNStuff.SQLViewPro.SSRSReports

    Partial Class SSRSReportControl
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

        Private Property ReportExtra As SSRSReportSettings = New SSRSReportSettings()

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
            _ReportExtra = DirectCast(DeserializeObject(settings.ReportConfig, GetType(SSRSReportSettings)), SSRSReportSettings)
        End Sub
#End Region

#Region " Template"

        Private Sub BindTemplateToData()

            ' debug ?
            If State.ReportSet.ReportSetDebug And ReportExtra.ProcessingMode = Common.ProcessingModeLocal Then
                DebugInfo.Append(QueryText)
            End If

            If Not ReportViewer1.ShowReportBody Then
                ' If you do not use this condition, the page will auto refresh and all parameters are reset to default or null values. 
                ReportViewer1.ShowReportBody = True
                RefreshReport()
            End If
        End Sub

        Private Sub RefreshReport()
            If ReportExtra.ViewerWidth <> "" Then
                ReportViewer1.Width = Unit.Parse(ReportExtra.ViewerWidth)
            Else
                ReportViewer1.Width = Unit.Percentage(100)
            End If
            If ReportExtra.ViewerHeight <> "" Then
                ReportViewer1.Height = Unit.Parse(ReportExtra.ViewerHeight)
            Else
                ReportViewer1.Height = Unit.Percentage(100)
            End If

            Dim options As Dictionary(Of String, String) = StringHelpers.ToDictionary(ReportExtra.ToolbarOptions, ","c)
            With ReportViewer1
                .ShowBackButton = options.ContainsKey("ShowBackButton")
                .ShowDocumentMapButton = options.ContainsKey("ShowDocumentMapButton")
                .ShowExportControls = options.ContainsKey("ShowExportControls")
                .ShowFindControls = options.ContainsKey("ShowFindControls")
                .ShowPageNavigationControls = options.ContainsKey("ShowPageNavigationControls")
                .ShowParameterPrompts = options.ContainsKey("ShowParameterPrompts")
                .ShowPrintButton = options.ContainsKey("ShowPrintButton")
                .ShowPromptAreaButton = options.ContainsKey("ShowPromptAreaButton")
                .ShowRefreshButton = options.ContainsKey("ShowRefreshButton")
                .ShowToolBar = options.ContainsKey("ShowToolBar")
                .ShowZoomControl = options.ContainsKey("ShowZoomControl")
                .ShowWaitControlCancelLink = options.ContainsKey("ShowWaitControlCancelLink")
            End With

            If ReportExtra.ProcessingMode = Common.ProcessingModeRemote Then
                RenderRemoteReport()
            Else
                RenderLocalReport()
            End If

        End Sub

        Private Sub RenderLocalReport()

            Dim ds As DataSet = ReportData()

            ReportViewer1.ProcessingMode = ProcessingMode.Local
            ReportViewer1.LocalReport.ReportPath = Services.File.GetFilenameFromFileId(ReportExtra.LocalReportPath)
            ReportViewer1.LocalReport.DisplayName = Report.ReportName

            Dim dsNames As IList(Of String) = ReportViewer1.LocalReport.GetDataSourceNames()
            ReportViewer1.LocalReport.DataSources.Clear()
            For dsIndex As Integer = 0 To dsNames.Count - 1
                ReportViewer1.LocalReport.DataSources.Add(New Microsoft.Reporting.WebForms.ReportDataSource(dsNames(dsIndex), ds.Tables(dsIndex)))
            Next

            ReportViewer1.LocalReport.SetParameters(SetReportParameters(ReportViewer1.LocalReport.GetParameters()))
            ReportViewer1.LocalReport.Refresh()
        End Sub

        Private Sub RenderRemoteReport()

            ReportViewer1.ProcessingMode = ProcessingMode.Remote
            ReportViewer1.ServerReport.Timeout = -1
            ReportViewer1.ServerReport.DisplayName = Report.ReportName
            ReportViewer1.ServerReport.ReportServerUrl = New Uri(ReportExtra.ReportServerUrl)
            ReportViewer1.ServerReport.ReportPath = ReportExtra.ReportServerReportPath
            ReportViewer1.ServerReport.ReportServerCredentials = New CustomReportCredentials(ReportExtra.ReportServerUsername, ReportExtra.ReportServerPassword, ReportExtra.ReportServerDomain)

            ReportViewer1.ServerReport.SetParameters(SetReportParameters(ReportViewer1.ServerReport.GetParameters()))
            ReportViewer1.ServerReport.Refresh()
        End Sub

        Private Function SetReportParameters(ByVal reportParameters As ReportParameterInfoCollection) As List(Of ReportParameter)

            Dim parameters As New List(Of ReportParameter)
            Dim additional As Dictionary(Of String, String) = StringHelpers.ToDictionary(ReportExtra.AdditionalParameters, ","c)
            ' report parameters
            For Each parameterInfo As ReportParameterInfo In reportParameters
                Dim p As New ReportParameter(parameterInfo.Name)
                For Each parameter As ParameterInfo In State.Parameters
                    If parameter.ParameterName = p.Name Then
                        If parameter.Values IsNot Nothing Then
                            For Each val As String In parameter.Values
                                p.Values.Add(val)
                            Next
                        End If
                        Exit For
                    End If
                Next
                If additional.ContainsKey(p.Name) Then
                    p.Values.Add(ReplaceReportTokens(additional(p.Name)))
                End If
                parameters.Add(p)
            Next

            Return parameters
        End Function

#End Region
#Region "ReportViewer"
        Private Sub ReportViewer1_ReportRefresh(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ReportViewer1.ReportRefresh
            RefreshReport()
        End Sub
#End Region

    End Class

    <Serializable()>
    Public Class CustomReportCredentials
        Implements IReportServerCredentials
        Private ReadOnly _username As String
        Private ReadOnly _password As String
        Private ReadOnly _domain As String

        Public Sub New(username As String, password As String, domain As String)
            Me._username = username
            Me._password = password
            Me._domain = domain
        End Sub

        Public Function IReportServerCredentials_GetFormsCredentials(<Out()> ByRef authCookie As Cookie, <Out()> ByRef userName As String, <Out()> ByRef password As String, <Out()> ByRef authority As String) As Boolean Implements IReportServerCredentials.GetFormsCredentials
            authCookie = Nothing
            userName = InlineAssignHelper(password, InlineAssignHelper(authority, Nothing))
            Return False
        End Function

        Public ReadOnly Property IReportServerCredentials_NetworkCredentials() As ICredentials Implements IReportServerCredentials.NetworkCredentials
            Get
                Return New NetworkCredential(_username, _password, _domain)
            End Get
        End Property

        Public ReadOnly Property IReportServerCredentials_ImpersonationUser() As WindowsIdentity Implements IReportServerCredentials.ImpersonationUser
            Get
                Return Nothing
            End Get
        End Property

        Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
            target = value
            Return value
        End Function
    End Class

End Namespace
