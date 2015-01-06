'***************************************************************************/
'* DefaultParameter.ascx.vb
'*
'* Copyright (c) 2004 by DNNStuff.
'* All rights reserved.
'*
'* Date:        August 9, 2004
'* Author:      Richard Edwards
'* Description: Default Parameter Handler
'*************/

Imports System.Configuration
Imports System.Xml.Serialization
Imports DNNStuff.SQLViewPro.Controls
Imports DNNStuff.SQLViewPro.Serialization
Imports DNNStuff.SQLViewPro.Services.Data
Imports System.Collections.Generic

Namespace DNNStuff.SQLViewPro.StandardParameters

    Partial Class GeoLocationParameterControl
        Inherits DNNStuff.SQLViewPro.Controls.ParameterControlBase

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
        Public Overrides Property Values() As List(Of String)
            Get
                Return New List(Of String)(New String() {geolocation.Value})
            End Get
            Set(ByVal Value As List(Of String))
                If Value.Count > 0 Then geolocation.Value = Value(0) Else geolocation.Value = ""
            End Set
        End Property

        Public Overrides ReadOnly Property ExtraValues As Collections.Specialized.StringDictionary
            Get
                Dim location As String = geolocation.Value
                Dim vals As New Collections.Specialized.StringDictionary()
                If location.Length > 0 Then
                    vals.Add("Latitude", location.Split(","c)(0))
                    vals.Add("Longitude", location.Split(","c)(1))
                Else
                    vals.Add("Latitude", "")
                    vals.Add("Longitude", "")
                End If
                Return vals
            End Get
        End Property

        Public Overrides Sub LoadRuntimeSettings()
        End Sub
#End Region

        Public Function GeoLocationSettings() As GeoLocationParameterSettings
            Return DirectCast(DeserializeObject(Settings.ParameterConfig, GetType(GeoLocationParameterSettings)), GeoLocationParameterSettings)
        End Function

    End Class

End Namespace
