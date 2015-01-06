Imports System.IO
Imports DotNetNuke
Imports DotNetNuke.Entities.Portals

Namespace DNNStuff.SQLViewPro.Controls
    Partial Class ConnectionPickerControl
        Inherits System.Web.UI.UserControl


        ' public properties
        Private _IncludePortalDefault As Boolean = False
        Private _IncludeReportSetDefault As Boolean = False

        Public Property ConnectionId() As Integer
            Get
                If ddlConnectionPicker.SelectedIndex > -1 Then
                    Return CType(ddlConnectionPicker.SelectedValue, Integer)
                End If
            End Get
            Set(ByVal Value As Integer)
                If ddlConnectionPicker.Items.Count = 0 Then
                    Data_Init()
                End If
                If Not ddlConnectionPicker.Items.FindByValue(Value.ToString) Is Nothing Then
                    ddlConnectionPicker.Items.FindByValue(Value.ToString).Selected = True
                End If

            End Set
        End Property

        Public Property IncludePortalDefault() As Boolean
            Get
                Return _IncludePortalDefault
            End Get
            Set(ByVal Value As Boolean)
                _IncludePortalDefault = Value
            End Set
        End Property
        Public Property IncludeReportSetDefault() As Boolean
            Get
                Return _IncludeReportSetDefault
            End Get
            Set(ByVal Value As Boolean)
                _IncludeReportSetDefault = Value
            End Set
        End Property

#Region " Web Form Designer Generated Code "


        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()

            If ddlConnectionPicker.Items.Count = 0 Then
                Data_Init()
            End If
        End Sub

#End Region

        Private Sub Data_Init()
            Try
                ' Obtain PortalSettings from Current Context
                Dim _portalSettings As PortalSettings = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)

                Dim ConnectionController As New ConnectionController
                Dim ConnectionList As ArrayList = ConnectionController.ListConnection(_portalSettings.PortalId, IncludePortalDefault, IncludeReportSetDefault)

                With ddlConnectionPicker
                    .DataTextField = "ConnectionName"
                    .DataValueField = "ConnectionId"
                    .DataSource = ConnectionList
                    .DataBind()
                End With
            Catch ex As Exception

            End Try

        End Sub


    End Class

End Namespace
