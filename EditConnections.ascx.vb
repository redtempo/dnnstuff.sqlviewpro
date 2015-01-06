'***************************************************************************/
'* EditConnections.ascx.vb
'*
'* Copyright (c) 2004 by DNNStuff.
'* All rights reserved.
'*
'* Date:        August 9, 2004
'* Author:      Richard Edwards
'* Description: Portal Module for editing SQLViewPro Connections
'*************/
Imports DotNetNuke
Imports DotNetNuke.Common

Namespace DNNStuff.SQLViewPro

    Partial Class EditConnections
        Inherits Entities.Modules.PortalModuleBase


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
                DNNUtilities.InjectCSS(Me.Page, ResolveUrl("Resources/Support/edit.css"))



            If Not Page.IsPostBack Then
                BindConnection()
            End If

        End Sub

#End Region

#Region " Data Connection"
        ' connection
        Private Sub DeleteConnection(ByVal ConnectionId As Integer)
            Dim objConnectionController As ConnectionController = New ConnectionController
            objConnectionController.DeleteConnection(ConnectionId)
        End Sub

        Private Sub BindConnection()
            Localization.LocalizeDataGrid(dgConnection, LocalResourceFile)

            Dim objConnectionList As ArrayList
            Dim objConnectionController As ConnectionController = New ConnectionController

            objConnectionList = objConnectionController.ListConnection(PortalId, False, False)

            ' bind
            dgConnection.DataSource = objConnectionList
            dgConnection.DataBind()

        End Sub

#End Region


#Region " Navigation"
        Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCloseBottom.Click
            Response.Redirect(NavigateUrl())
        End Sub

        Private Function NavigateConnection(ByVal ConnectionId As Integer) As String
            Return EditUrl("ConnectionId", ConnectionId.ToString, "EditConnection")
        End Function

#End Region

#Region " Connection Grid"
        Private Sub cmdAddConnection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddConnection.Click
            Response.Redirect(NavigateConnection(-1))
        End Sub

        Private Sub dgConnection_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgConnection.ItemCommand
            Dim ConnectionId As Integer = Int32.Parse(dgConnection.DataKeys(e.Item.ItemIndex).ToString)
            Select Case e.CommandName.ToLower
                Case "edit"
                    Response.Redirect(NavigateConnection(ConnectionId))
                Case "delete"
                    DeleteConnection(ConnectionId)
                    BindConnection()
            End Select
        End Sub

        Private Sub dgConnection_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgConnection.ItemDataBound
            ' process data rows only (skip the header, footer etc.)
            If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = _
                ListItemType.AlternatingItem Then
                ' get a reference to the LinkButton of this row,
                '  and add the javascript confirmation
                Dim lnkDelete As LinkButton = CType(e.Item.FindControl("cmdDeleteConnection"), _
                    LinkButton)
                If Not lnkDelete Is Nothing Then
                    If lnkDelete.Enabled Then
                        lnkDelete.Attributes.Add("onclick", _
                            "return confirm('Are you sure you want to delete this connection?');")
                    Else
                        lnkDelete.Attributes.Add("title", _
                            "This connection is used by other objects. It cannot be deleted.")

                    End If

                End If
            End If
        End Sub
#End Region

    End Class

End Namespace
