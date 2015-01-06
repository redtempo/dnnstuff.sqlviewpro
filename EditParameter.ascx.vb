'***************************************************************************/
'* EditSQLViewPro.ascx.vb
'*
'* Copyright (c) 2004 by DNNStuff.
'* All rights reserved.
'*
'* Date:        August 9, 2004
'* Author:      Richard Edwards
'* Description: Portal Module for editing setting for a report grid
'*************/

Imports DotNetNuke
Imports DotNetNuke.Common
Imports System.Configuration
Imports System.IO

Namespace DNNStuff.SQLViewPro

    Partial Class EditParameter
        Inherits Entities.Modules.PortalModuleBase
        Private Const STR_ReferringUrl As String = "EditParameter_ReferringUrl"


#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()

            ' initialize
            ParameterId = Int32.Parse(DNNUtilities.QueryStringDefault(Request, "ParameterId", "-1"))
            ReportSetId = Int32.Parse(DNNUtilities.QueryStringDefault(Request, "ReportSetId", "-1"))

            InitParameter()

        End Sub

#End Region

#Region " Page"

        Public Property ReportSetId() As Integer
        Public Property ParameterId() As Integer
        Public Property Parameter() As ParameterInfo
        Public Property ParameterConfig() As String

        Private Sub SaveReferringPage()
            ' save referring page
            If Request.UrlReferrer Is Nothing Then
                Session(STR_ReferringUrl) = NavigateURL()
            Else
                Session(STR_ReferringUrl) = Request.UrlReferrer.AbsoluteUri
            End If
        End Sub
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
                DNNUtilities.InjectCSS(Me.Page, ResolveUrl("Resources/Support/edit.css"))


            Page.ClientScript.RegisterClientScriptInclude(Me.GetType, "yeti", ResolveUrl("resources/support/yetii-min.js"))


            If Page.IsPostBack Then
                RenderParameterSettings()
            Else
                SaveReferringPage()

                ' drop down parameter type
                BindParameterType()
                BindParameter()

                ' do parameter type
                RenderParameterSettings()
                RetrieveParameterSettings()
            End If

        End Sub

#End Region

#Region " Data"
        Private Sub InitParameter()
            Dim objParameterController As ParameterController = New ParameterController
            Dim objParameter As ParameterInfo = objParameterController.GetParameter(ParameterId)

            ' load from database
            If objParameter Is Nothing Then
                Parameter = New ParameterInfo()
            Else
                Parameter = objParameter
            End If
        End Sub

        Private Sub BindParameter()
            With Parameter
                txtName.Text = .ParameterName
                txtCaption.Text = .ParameterCaption
                ddParameterType.SelectedValue = .ParameterTypeId
            End With
        End Sub

        Private Sub SaveParameter()
            RetrieveParameterSettings()

            Dim objParameterController As ParameterController = New ParameterController
            ParameterId = objParameterController.UpdateParameter(ReportSetId, ParameterId, txtName.Text, txtCaption.Text, ddParameterType.SelectedValue, ParameterConfig, -1)
        End Sub

        Private Sub BindParameterType()
            Dim objParameterTypeController As ParameterTypeController = New ParameterTypeController
            With ddParameterType
                .DataTextField = "ParameterTypeName"
                .DataValueField = "ParameterTypeId"
                .DataSource = objParameterTypeController.ListParameterType()
                .DataBind()
            End With
        End Sub

#End Region

#Region " Navigation"
        Private Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdUpdate.Click

            If Page.IsValid Then
                SaveParameter()

                ' Redirect back to the Parameter set
                NavigateBack()

            End If

        End Sub

        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
            NavigateBack()
        End Sub

        Private Sub NavigateBack()
            If Session(STR_ReferringUrl) IsNot Nothing Then
                Response.Redirect(Session(STR_ReferringUrl).ToString)
            End If
        End Sub

#End Region

        Private Sub RetrieveParameterSettings()
            Dim objParameterSettings As Controls.ParameterSettingsControlBase = DirectCast(phParameterSettings.FindControl("ParameterSettings"), Controls.ParameterSettingsControlBase)

            ParameterConfig = objParameterSettings.UpdateSettings

        End Sub

        Private Sub RenderParameterSettings()
            Dim parameterTypeId As String = ddParameterType.SelectedValue

            Dim objParameterTypeController As ParameterTypeController = New ParameterTypeController

            Dim objParameterType As ParameterTypeInfo = objParameterTypeController.GetParameterType(parameterTypeId)

            Dim objParameterSettingsBase As Controls.ParameterSettingsControlBase
            objParameterSettingsBase = CType(LoadControl(Me.ResolveUrl(objParameterType.ParameterTypeSettingsControlSrc)), Controls.ParameterSettingsControlBase)

            With objParameterSettingsBase
                .ID = "ParameterSettings"
                If Not Parameter Is Nothing Then
                    .LoadSettings(Parameter.ParameterConfig)
                Else
                    .LoadSettings(Nothing)
                End If
            End With
            phParameterSettings.Controls.Add(objParameterSettingsBase)

            ' update settings requirements
            txtCaption_Required.Enabled = objParameterSettingsBase.CaptionRequired

        End Sub
        Private Sub ddParameterType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddParameterType.SelectedIndexChanged
            RetrieveParameterSettings()
        End Sub
    End Class

End Namespace
