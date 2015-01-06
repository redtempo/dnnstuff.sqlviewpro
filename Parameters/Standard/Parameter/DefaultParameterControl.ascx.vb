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

    Partial Class DefaultParameterControl
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
                Return New List(Of String)(New String() {txtParameter.Text})
            End Get
            Set(ByVal Value As List(Of String))
                If Value.Count > 0 Then txtParameter.Text = Value(0) Else txtParameter.Text = ""
            End Set
        End Property

        Public Overrides Sub LoadRuntimeSettings()
            Dim obj As DefaultParameterSettings = DirectCast(DeserializeObject(Settings.ParameterConfig, GetType(DefaultParameterSettings)), DefaultParameterSettings)
            With obj
                txtParameter.Text = TokenReplacement.ReplaceTokens(.Default)
            End With
        End Sub
#End Region

    End Class

End Namespace
