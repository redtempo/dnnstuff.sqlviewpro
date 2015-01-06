'***************************************************************************/
'* MobiscrollParameter.ascx.vb
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

Namespace DNNStuff.SQLViewPro.MobileParameters

    Partial Class MobiscrollParameterControl
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

            ScriptController.InjectjQueryLibary(Me.Page, False, False)
            ScriptController.InjectCssReference(Me.Page, "mobiscroll", ResolveUrl("Resources/mobiscroll-1.6.min.css"), True, ScriptController.CssInjectOrder.f_Last)
            ScriptController.InjectJsLibrary(Me.Page, "mobiscroll_js", ResolveUrl("Resources/mobiscroll-1.6.min.js"), False, ScriptController.ScriptInjectOrder.e_Default)

            Dim sb As New Text.StringBuilder()
            sb.AppendLine("<script type=""text/javascript"">")
            sb.AppendLine("$(document).ready(function () {")
            sb.AppendLine(String.Format("$('#{0}').scroller({{ preset: '{1}' , theme: '{2}', mode: '{3}' }});", txtMobiscroll.ClientID, MobiscrollSettings.Preset, MobiscrollSettings.Theme, MobiscrollSettings.Mode))
            sb.AppendLine("});")
            sb.AppendLine("</script>")

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), Unique("Mobiscroll"), sb.ToString())

        End Sub

#End Region

#Region " Base Method Implementations"

        Public Overrides Property Values() As List(Of String)
            Get

                Return New List(Of String)(New String() {txtMobiscroll.Text})
            End Get

            Set(ByVal Value As List(Of String))
                If Value.Count > 0 Then txtMobiscroll.Text = Value(0) Else txtMobiscroll.Text = ""
            End Set
        End Property

        Public Overrides Sub LoadRuntimeSettings()
            txtMobiscroll.Text = TokenReplacement.ReplaceTokens(MobiscrollSettings.Default)
        End Sub

#End Region

        Private Function MobiscrollSettings() As MobiscrollParameterSettings
            Return DirectCast(DeserializeObject(Settings.ParameterConfig, GetType(MobiscrollParameterSettings)), MobiscrollParameterSettings)
        End Function
    End Class

End Namespace
