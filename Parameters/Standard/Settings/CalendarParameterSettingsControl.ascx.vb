'***************************************************************************/
'* CalendarParameterSettings.ascx.vb
'*
'* Copyright (c) 2004 by DNNStuff.
'* All rights reserved.
'*
'* Date:        August 9, 2004
'* Author:      Richard Edwards
'* Description: Calendar Parameter Settings Handler
'*************/

Imports System.Configuration
Imports System.Xml.Serialization
Imports DNNStuff.SQLViewPro.Controls
Imports DNNStuff.SQLViewPro.Serialization

Namespace DNNStuff.SQLViewPro.StandardParameters

    Partial Class CalendarParameterSettingsControl
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
                Return ResolveUrl("App_LocalResources/CalendarParameterSettingsControl")
            End Get
        End Property

        Public Overrides Function UpdateSettings() As String

            Dim obj As CalendarParameterSettings = New CalendarParameterSettings
            With obj
                .Default = txtDefault.Text
                .DatabaseDateFormat = txtDatabaseDateFormat.Text
            End With

            Return SerializeObject(obj, GetType(CalendarParameterSettings))

        End Function

        Public Overrides Sub LoadSettings(ByVal settings As String)
            Dim obj As New CalendarParameterSettings
            If settings IsNot Nothing Then
                obj = DirectCast(DeserializeObject(settings, GetType(CalendarParameterSettings)), CalendarParameterSettings)
            End If
            With obj
                txtDefault.Text = .Default
                txtDatabaseDateFormat.Text = .DatabaseDateFormat
            End With
        End Sub

#End Region

    End Class

#Region " Settings"
    <XmlRootAttribute(ElementName:="Settings", IsNullable:=False)> _
    Public Class CalendarParameterSettings
        Public Property [Default]() As String
        Public Property DatabaseDateFormat() As String = "yyyy-M-d"
    End Class
#End Region

End Namespace
