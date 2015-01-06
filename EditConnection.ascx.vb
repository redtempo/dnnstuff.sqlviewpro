'***************************************************************************/
'* EditConnection.ascx.vb
'*
'* Copyright (c) 2004 by DNNStuff.
'* All rights reserved.
'*
'* Date:        August 9, 2004
'* Author:      Richard Edwards
'* Description: Portal Module for editing setting for a Connection grid
'*************/

Imports DotNetNuke
Imports System.Configuration
Imports System.IO

Namespace DNNStuff.SQLViewPro

    Partial Class EditConnection
        Inherits Entities.Modules.PortalModuleBase


        'standard

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()

            ' initialize
            If Not Request.QueryString("ConnectionId") Is Nothing Then
                ConnectionId = Int32.Parse(Request.QueryString("ConnectionId").ToString)
            Else
                ConnectionId = -1
            End If

            InitConnection()

        End Sub

#End Region

#Region " Page"

        Private _ConnectionId As Integer
        Public Property ConnectionId() As Integer
            Get
                Return _ConnectionId
            End Get
            Set(ByVal Value As Integer)
                _ConnectionId = Value
            End Set
        End Property
        Private _Connection As ConnectionInfo
        Public Property Connection() As ConnectionInfo
            Get
                Return _Connection
            End Get
            Set(ByVal Value As ConnectionInfo)
                _Connection = Value
            End Set
        End Property
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
                DNNUtilities.InjectCSS(Me.Page, ResolveUrl("Resources/Support/edit.css"))



            If Not Page.IsPostBack Then

                BindConnection()

            End If

        End Sub

#End Region

#Region " Data"
        Private Sub InitConnection()
            Dim objConnection As ConnectionInfo = ConnectionController.GetConnection(ConnectionId)

            ' load from database
            Connection = objConnection
        End Sub
        Private Sub BindConnection()
            If Not Connection Is Nothing Then
                With Connection
                    txtName.Text = .ConnectionName
                    txtConnectionString.Text = .ConnectionString
                End With
            End If


        End Sub

        Private Sub SaveConnection()
            Dim objConnectionController As ConnectionController = New ConnectionController
            ConnectionId = objConnectionController.UpdateConnection(PortalId, ConnectionId, txtName.Text, txtConnectionString.Text)
        End Sub

#End Region

#Region " Navigation"
        Private Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdUpdate.Click

            If Page.IsValid Then
                SaveConnection()

                ' Redirect back to the Connection set
                Response.Redirect(NavigateConnections())
            End If

        End Sub

        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
            Response.Redirect(NavigateConnections())
        End Sub

        Private Function NavigateConnections() As String
            Return EditUrl("EditConnections")
        End Function
#End Region

        Private Sub vldConnectionStringValid_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles vldConnectionStringValid.ServerValidate
            Try
                Services.Data.Query.TestConnection(args.Value)
                args.IsValid = True
            Catch ex As System.Security.SecurityException
                vldConnectionStringValid.ErrorMessage = "Insufficient trust level to create OLEDB connections. You must configure DNN to use a lower trust level or add OLEDBPermission to the current trust level<br />" & "Connection string error: " & ex.Message
                args.IsValid = False
            Catch ex As Exception
                vldConnectionStringValid.ErrorMessage = "Connection string error: " & ex.Message
                args.IsValid = False
            End Try
        End Sub

    End Class

End Namespace
