'***************************************************************************/
'* FlowListParameterSettings.ascx.vb
'*
'* Copyright (c) 2004 by DNNStuff.
'* All rights reserved.
'*
'* Date:        August 9, 2004
'* Author:      Richard Edwards
'* Description: FlowList Parameter Settings Handler
'*************/

Imports System.Configuration
Imports System.Xml.Serialization
Imports DNNStuff.SQLViewPro.Controls
Imports DNNStuff.SQLViewPro.Serialization
Imports System.Web.UI.WebControls

Namespace DNNStuff.SQLViewPro.StandardParameters

    Partial Class FlowListParameterSettingsControl
        Inherits ParameterSettingsControlBase

        Protected WithEvents cpConnection As Controls.ConnectionPickerControl

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()

            QueryStringInitialize()
        End Sub

#End Region

#Region " Page"
        Public Property ReportSetId() As Integer = -1
        Public Property ParameterId() As Integer = -1


        Private Sub QueryStringInitialize()
            ' initialize
            If Not Request.QueryString("ReportSetId") Is Nothing Then
                ReportSetId = Int32.Parse(Request.QueryString("ReportSetId").ToString)
            Else
                ReportSetId = -1
            End If

            If Not Request.QueryString("ParameterId") Is Nothing Then
                ParameterId = Int32.Parse(Request.QueryString("ParameterId").ToString)
            Else
                ParameterId = -1
            End If

        End Sub
#End Region

#Region " Base Method Implementations"
        Protected Overrides ReadOnly Property LocalResourceFile() As String
            Get
                Return ResolveUrl("App_LocalResources/FlowListParameterSettingsControl")
            End Get
        End Property

        Public Overrides Function UpdateSettings() As String

            Dim obj As FlowListParameterSettings = New FlowListParameterSettings
            With obj
                .Default = txtDefault.Text
                .List = txtList.Text
                .Command = txtCommand.Text
                .ConnectionId = cpConnection.ConnectionId
                .CommandCacheTimeout = Convert.ToInt32(txtCommandCacheTimeout.Text)

                .RepeatColumns = StringHelpers.DefaultInt32FromString(txtRepeatColumns.Text, 1)
                .RepeatDirection = DirectCast(System.Enum.Parse(GetType(RepeatDirection), ddlRepeatDirection.SelectedValue), RepeatDirection)
                .RepeatLayout = DirectCast(System.Enum.Parse(GetType(RepeatLayout), ddlRepeatLayout.SelectedValue), RepeatLayout)

            End With

            Return SerializeObject(obj, GetType(FlowListParameterSettings))
        End Function

        Public Overrides Sub LoadSettings(ByVal settings As String)
            Dim obj As New FlowListParameterSettings
            If settings IsNot Nothing Then
                obj = DirectCast(DeserializeObject(settings, GetType(FlowListParameterSettings)), FlowListParameterSettings)
            End If

            With obj
                txtDefault.Text = .Default
                txtList.Text = .List
                txtCommand.Text = .Command
                txtCommandCacheTimeout.Text = .CommandCacheTimeout.ToString()
                txtRepeatColumns.Text = .RepeatColumns.ToString
                ddlRepeatDirection.Items.FindByValue(.RepeatDirection).Selected = True
                ddlRepeatLayout.Items.FindByValue(.RepeatLayout).Selected = True
                cpConnection.ConnectionId = .ConnectionId

            End With
        End Sub

#End Region

#Region " Validation"
        Private Sub vldCommand_ServerValidate(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles vldCommand.ServerValidate

            Dim msg As String = ""
            args.IsValid = Services.Data.Query.IsQueryValid(txtCommand.Text, ConnectionController.GetConnectionString(cpConnection.ConnectionId, ReportSetId), msg)
            vldCommand.ErrorMessage = msg

        End Sub


        Private Sub cmdQueryTest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdQueryTest.Click
            Dim msg As String = ""
            Dim isValid As Boolean = Services.Data.Query.IsQueryValid(txtCommand.Text, ConnectionController.GetConnectionString(cpConnection.ConnectionId, ReportSetId), msg)

            lblQueryTestResults.Text = msg
            lblQueryTestResults.CssClass = "NormalGreen"
            If Not isValid Then lblQueryTestResults.CssClass = "NormalRed"

        End Sub
#End Region

    End Class

End Namespace
