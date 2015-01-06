'***************************************************************************/
'* MobiscrollParameterSettings.ascx.vb
'*
'* Copyright (c) 2004 by DNNStuff.
'* All rights reserved.
'*
'* Date:        August 9, 2004
'* Author:      Richard Edwards
'* Description: Mobiscroll Parameter Settings Handler
'*************/

Imports System.Configuration
Imports System.Xml.Serialization
Imports DNNStuff.SQLViewPro.Controls
Imports DNNStuff.SQLViewPro.Serialization

Namespace DNNStuff.SQLViewPro.MobileParameters

    Partial Class MobiscrollParameterSettingsControl
        Inherits ParameterSettingsControlBase

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

#Region " Base Method Implementations"
        Protected Overrides ReadOnly Property LocalResourceFile() As String
            Get
                Return ResolveUrl("App_LocalResources/MobiscrollParameterSettingsControl")
            End Get
        End Property

        Public Overrides Function UpdateSettings() As String

            Dim obj As MobiscrollParameterSettings = New MobiscrollParameterSettings
            With obj
                .Default = txtDefault.Text
                .Preset = ddPreset.SelectedValue
                .Theme = ddTheme.SelectedValue
                .Mode = ddMode.SelectedValue
            End With

            Return SerializeObject(obj, GetType(MobiscrollParameterSettings))

        End Function

        Public Overrides Sub LoadSettings(ByVal settings As String)
            Dim obj As New MobiscrollParameterSettings
            If settings IsNot Nothing Then
                obj = DirectCast(DeserializeObject(settings, GetType(MobiscrollParameterSettings)), MobiscrollParameterSettings)
            End If

            With obj
                txtDefault.Text = .Default
                ddPreset.SelectedValue = .Preset
                ddTheme.SelectedValue = .Theme
                ddMode.SelectedValue = .Mode
            End With
        End Sub

#End Region

    End Class

#Region " Settings"
    <XmlRootAttribute(ElementName:="Settings", IsNullable:=False)> _
    Public Class MobiscrollParameterSettings
        Public Property [Default]() As String
        Public Property DatabaseFormat() As String = ""
        Public Property Preset() As String = "date"
        Public Property Theme() As String = "default"
        Public Property Mode() As String = "Scroller"
    End Class
#End Region

End Namespace
