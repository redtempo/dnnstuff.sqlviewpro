Imports DotNetNuke
Imports DotNetNuke.Security.Permissions

Namespace DNNStuff.SQLViewPro

    Public Class Report
        Inherits System.Web.UI.Page

        Private _ModuleId As Integer = -1
        Private _TabId As Integer = -1
        Public ReadOnly Property ModuleId() As Integer
            Get
                If Request.QueryString("ModuleId") IsNot Nothing Then
                    _ModuleId = CInt(Request.QueryString("ModuleId"))
                End If
                Return _ModuleId
            End Get
        End Property
        Public ReadOnly Property TabId() As Integer
            Get
                If Request.QueryString("TabId") IsNot Nothing Then
                    _TabId = CInt(Request.QueryString("TabId"))
                End If
                Return _TabId
            End Get
        End Property

        Private Sub Report_Init(sender As Object, e As System.EventArgs) Handles Me.Init
            If HasViewPermissions() Then
                Dim ctrl As DotNetNuke.Entities.Modules.PortalModuleBase
                ctrl = CType(LoadControl(ResolveUrl("SQLViewPro.ascx")), DotNetNuke.Entities.Modules.PortalModuleBase)
                phInject.Controls.Add(ctrl)
            Else
                Response.Redirect(DotNetNuke.Common.AccessDeniedURL(), True)
            End If
        End Sub

        Private Function HasViewPermissions() As Boolean
            Dim mi As DotNetNuke.Entities.Modules.ModuleInfo
            Dim mc As New DotNetNuke.Entities.Modules.ModuleController
            mi = mc.GetModule(ModuleId, TabId)
            Return ModulePermissionController.CanViewModule(mi)
        End Function

        Private Sub form1_Load(sender As Object, e As System.EventArgs) Handles form1.Load
            DotNetNuke.Framework.jQuery.RegisterJQuery(Me.Page)
        End Sub
    End Class
End Namespace
