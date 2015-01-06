'***************************************************************************/
'* DefaultParameterSettings.ascx.vb
'*
'* Copyright (c) 2004 by DNNStuff.
'* All rights reserved.
'*
'* Date:        August 9, 2004
'* Author:      Richard Edwards
'* Description: Default Parameter Settings Handler
'*************/

Imports System.Configuration
Imports System.Xml.Serialization
Imports DNNStuff.SQLViewPro.Controls
Imports DNNStuff.SQLViewPro.Serialization

Namespace DNNStuff.SQLViewPro.StandardParameters

    Partial Class CheckBoxParameterSettingsControl
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
                Return ResolveUrl("App_LocalResources/CheckBoxParameterSettingsControl")
            End Get
        End Property

        Public Overrides Function UpdateSettings() As String

            Dim obj As CheckBoxParameterSettings = New CheckBoxParameterSettings
            With obj
                .DefaultChecked = chkDefault.Checked
            End With

            Return SerializeObject(obj, GetType(CheckBoxParameterSettings))

        End Function

        Public Overrides Sub LoadSettings(ByVal settings As String)
            Dim obj As New CheckBoxParameterSettings
            If settings IsNot Nothing Then
                obj = DirectCast(DeserializeObject(settings, GetType(CheckBoxParameterSettings)), CheckBoxParameterSettings)
            End If

            With obj
                chkDefault.Checked = .DefaultChecked
            End With
        End Sub

#End Region

    End Class

#Region " Settings"
    <XmlRootAttribute(ElementName:="Settings", IsNullable:=False)> _
    Public Class CheckBoxParameterSettings
        Private _DefaultChecked As Boolean
        Public Property [DefaultChecked]() As Boolean
            Get
                Return _DefaultChecked
            End Get
            Set(ByVal Value As Boolean)
                _DefaultChecked = Value
            End Set
        End Property
    End Class
#End Region

End Namespace
