'***************************************************************************/
'* CalendarParameter.ascx.vb
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
Imports DotNetNuke
Imports System.Globalization
Imports System.Collections.Generic

Namespace DNNStuff.SQLViewPro.StandardParameters

    Partial Class CalendarParameterControl
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
            cmdStartCalendar.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtCalendar)
        End Sub

#End Region

#Region " Base Method Implementations"

        Public Overrides Property Values() As List(Of String)
            Get
                Dim ret As String = txtCalendar.Text
                Dim dt As Date
                If Date.TryParse(txtCalendar.Text, dt) Then
                    If CalendarSettings.DatabaseDateFormat <> "" Then
                        ret = dt.ToString(CalendarSettings.DatabaseDateFormat)
                    Else
                        ret = dt.ToString("M/d/yyyy")
                    End If
                End If
                Return New List(Of String)(New String() {ret})
            End Get

            Set(ByVal Value As List(Of String))

                Dim provider As CultureInfo = CultureInfo.GetCultureInfo(PortalSettings.DefaultLanguage)
                Dim dt As Date
                If Value.Count > 0 Then
                    If Date.TryParseExact(Value(0), "M/d/yyyy", provider, DateTimeStyles.None, dt) Then
                        txtCalendar.Text = dt.ToShortDateString
                    End If
                Else
                    txtCalendar.Text = ""
                End If

            End Set
        End Property

        Public Overrides Sub LoadRuntimeSettings()
            Values = New List(Of String)(New String() {TokenReplacement.ReplaceTokens(CalendarSettings.Default)})
        End Sub

#End Region
        Private Function CalendarSettings() As CalendarParameterSettings
            Return DirectCast(DeserializeObject(Settings.ParameterConfig, GetType(CalendarParameterSettings)), CalendarParameterSettings)
        End Function
    End Class

End Namespace
