'***************************************************************************/
'* DropDownListParameterControl.ascx.vb
'*
'* Copyright (c) 2004 by DNNStuff.
'* All rights reserved.
'*
'* Date:        August 9, 2004
'* Author:      Richard Edwards
'* Description: DropdownList Parameter Control
'*************/

Imports System.Configuration
Imports System.Xml.Serialization
Imports DNNStuff.SQLViewPro.Controls
Imports System.Web.UI.WebControls
Imports System.Collections.Generic

Namespace DNNStuff.SQLViewPro.StandardParameters

    Partial Class DropDownListParameterControl
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
                Return New List(Of String)(New String() {ddlParameter.SelectedValue})
            End Get
            Set(ByVal Value As List(Of String))
                If Value.Count > 0 Then ddlParameter.SelectedValue = Value(0) Else ddlParameter.SelectedValue = ""
            End Set
        End Property

        Public Overrides Sub LoadRuntimeSettings()
            Dim obj As DropDownListParameterSettings = DirectCast(Serialization.DeserializeObject(Settings.ParameterConfig, GetType(DropDownListParameterSettings)), DropDownListParameterSettings)
            AddOptions(ddlParameter, obj, Settings)
            SelectDefaults(ddlParameter, obj, False)
            ddlParameter.AutoPostBack = obj.AutoPostback
            AddHandler ddlParameter.SelectedIndexChanged, AddressOf ddlParameter_SelectedIndexChanged
        End Sub

#End Region

        Private Sub ddlParameter_SelectedIndexChanged(sender As Object, e As System.EventArgs)
            MyBase.Run(Me)
        End Sub

    End Class

End Namespace
