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

    Partial Class TextBoxParameterSettingsControl
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
                Return ResolveUrl("App_LocalResources/TextBoxParameterSettingsControl")
            End Get
        End Property

        Public Overrides Function UpdateSettings() As String

            Dim obj As TextBoxParameterSettings = New TextBoxParameterSettings
            With obj
                .Default = txtDefault.Text
                .Rows = StringHelpers.DefaultInt32FromString(txtRows.Text, 1)
                .Columns = StringHelpers.DefaultInt32FromString(txtColumns.Text, 0)
            End With

            Return SerializeObject(obj, GetType(TextBoxParameterSettings))

        End Function

        Public Overrides Sub LoadSettings(ByVal settings As String)
            Dim obj As New TextBoxParameterSettings
            If settings IsNot Nothing Then
                obj = DirectCast(DeserializeObject(settings, GetType(TextBoxParameterSettings)), TextBoxParameterSettings)
            End If

            With obj
                txtDefault.Text = .Default
                txtRows.Text = .Rows
                txtColumns.Text = .Columns
            End With
        End Sub

#End Region


    End Class

#Region " Settings"
    <XmlRootAttribute(ElementName:="Settings", IsNullable:=False)> _
    Public Class TextBoxParameterSettings
        Public Property [Default] As String
        Public Property Rows As Integer = 1
        Public Property Columns As Integer = 0
    End Class
#End Region

End Namespace
