'***************************************************************************/
'* EmptyParameterSettings.ascx.vb
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

    Partial Class GeoLocationParameterSettingsControl
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
                Return ResolveUrl("App_LocalResources/GeoLocationParameterSettingsControl")
            End Get
        End Property

        Public Overrides Function UpdateSettings() As String

            Dim obj As GeoLocationParameterSettings = New GeoLocationParameterSettings
            With obj
                .EnableHighAccuracy = chkEnableHighAccuracy.Checked
                .Timeout = StringHelpers.DefaultInt32FromString(txtTimeout.Text, 5000)
                .MaximumAge = StringHelpers.DefaultInt32FromString(txtMaximumAge.Text, 60000)
            End With
            Return SerializeObject(obj, GetType(GeoLocationParameterSettings))

        End Function

        Public Overrides Sub LoadSettings(ByVal settings As String)
            Dim obj As GeoLocationParameterSettings = New GeoLocationParameterSettings

            If settings IsNot Nothing Then
                obj = DirectCast(DeserializeObject(settings, GetType(GeoLocationParameterSettings)), GeoLocationParameterSettings)
            End If

            With obj
                chkEnableHighAccuracy.Checked = .EnableHighAccuracy
                txtTimeout.Text = .Timeout.ToString()
                txtMaximumAge.Text = .MaximumAge.ToString()
            End With

        End Sub

        Public Overrides ReadOnly Property CaptionRequired As Boolean
            Get
                Return False
            End Get
        End Property
#End Region



    End Class

#Region " Settings"
    <XmlRootAttribute(ElementName:="Settings", IsNullable:=False)> _
    Public Class GeoLocationParameterSettings
        Public Property EnableHighAccuracy As Boolean = True
        Public Property Timeout As Integer = 5000
        Public Property MaximumAge As Integer = 60000
    End Class
#End Region

End Namespace
