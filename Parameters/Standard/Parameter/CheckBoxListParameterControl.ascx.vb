'***************************************************************************/
'* CheckListBoxParameterControl.ascx.vb
'*
'* Copyright (c) 2004 by DNNStuff.
'* All rights reserved.
'*
'* Date:        August 9, 2004
'* Author:      Richard Edwards
'* Description: CheckBoxList Parameter Handler
'*************/

Imports System.Configuration
Imports System.Xml.Serialization
Imports DNNStuff.SQLViewPro.Controls
Imports System.Web.UI.WebControls
Imports System.Collections.Generic

Namespace DNNStuff.SQLViewPro.StandardParameters

    Partial Class CheckBoxListParameterControl
        Inherits ListParameterControlBase

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
                Dim selected As List(Of String) = New List(Of String)
                For Each li As Web.UI.WebControls.ListItem In cblParameter.Items
                    If li.Selected Then
                        selected.Add(li.Value.ToString)
                    End If
                Next
                Return selected
            End Get

            Set(ByVal Value As List(Of String))
                If Value.Count > 0 Then cblParameter.SelectedValue = Value(0) Else cblParameter.SelectedValue = ""
            End Set
        End Property
        Public Overrides ReadOnly Property MultiValued() As Boolean
            Get
                Return True
            End Get
        End Property
        Public Overrides Sub LoadRuntimeSettings()
            Dim obj As FlowListParameterSettings = DirectCast(Serialization.DeserializeObject(Settings.ParameterConfig, GetType(FlowListParameterSettings)), FlowListParameterSettings)
            With obj
                cblParameter.RepeatColumns = .RepeatColumns
                cblParameter.RepeatDirection = .RepeatDirection
                cblParameter.RepeatLayout = .RepeatLayout
            End With
            AddOptions(cblParameter, obj, Settings)
            SelectDefaults(cblParameter, obj, True)
        End Sub

#End Region

    End Class

End Namespace
