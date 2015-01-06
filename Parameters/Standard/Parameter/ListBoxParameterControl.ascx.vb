'***************************************************************************/
'* ListBoxParameter.ascx.vb
'*
'* Copyright (c) 2004 by DNNStuff.
'* All rights reserved.
'*
'* Date:        August 9, 2004
'* Author:      Richard Edwards
'* Description: ListBox Parameter Handler
'*************/

Imports System.Configuration
Imports System.Xml.Serialization
Imports DNNStuff.SQLViewPro.Controls
Imports System.Web.UI.WebControls
Imports System.Collections.Generic

Namespace DNNStuff.SQLViewPro.StandardParameters

    Partial Class ListBoxParameterControl
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
        Dim _multiValued As Boolean = False

#Region " Base Method Implementations"
        Public Overrides Property Values() As List(Of String)
            Get
                Dim selected As List(Of String) = New List(Of String)
                For Each li As Web.UI.WebControls.ListItem In lbParameter.Items
                    If li.Selected Then
                        selected.Add(li.Value.ToString)
                    End If
                Next
                Return selected
            End Get

            Set(ByVal Value As List(Of String))
                If Value.Count > 0 Then lbParameter.SelectedValue = Value(0) Else lbParameter.SelectedValue = ""
            End Set
        End Property
        Public Overrides ReadOnly Property MultiValued() As Boolean
            Get
                Return _multiValued
            End Get
        End Property
        Public Overrides Sub LoadRuntimeSettings()
            Dim obj As ListBoxParameterSettings = DirectCast(Serialization.DeserializeObject(Settings.ParameterConfig, GetType(ListBoxParameterSettings)), ListBoxParameterSettings)
            _multiValued = obj.MultiSelect
            AddOptions(lbParameter, obj, Settings)
            If obj.MultiSelect Then
                lbParameter.SelectionMode = ListSelectionMode.Multiple
                lbParameter.Rows = obj.MultiSelectSize
            End If
            SelectDefaults(lbParameter, obj, obj.MultiSelect)
        End Sub

#End Region

    End Class

End Namespace
