'***************************************************************************/
'* EditSQLViewPro.ascx.vb
'*
'* Copyright (c) 2004 by DNNStuff.
'* All rights reserved.
'*
'* Date:        August 9, 2004
'* Author:      Richard Edwards
'* Description: Portal Module for editing SQLViewPro Report Set
'*************/

Imports DotNetNuke
Imports DotNetNuke.Common
Imports System.Configuration
Imports System.IO

Namespace DNNStuff.SQLViewPro

    Partial Class EditReportSet
        Inherits Entities.Modules.PortalModuleBase
        Private Const STR_ReferringUrl As String = "EditReportSet_ReferringUrl"

        Protected WithEvents cboTheme As System.Web.UI.WebControls.DropDownList
        Protected WithEvents cpConnection As Controls.ConnectionPickerControl

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

        ' collection counts for use with move up/down binding
        Private _ParameterCount As Integer
        Private _ReportCount As Integer

        Private _ReportSetId As Integer
        Public Property ReportSetId() As Integer
            Get
                Return _ReportSetId
            End Get
            Set(ByVal Value As Integer)
                _ReportSetId = Value
            End Set
        End Property

        Private Sub SaveReferringPage()
            ' save referring page
            If Request.UrlReferrer Is Nothing Then
                Session(STR_ReferringUrl) = NavigateURL()
            Else
                ' don't set if coming from a later page such as EditReport or EditParameter
                If Not (Request.UrlReferrer.AbsoluteUri.ToString.Contains("EditReport/") Or Request.UrlReferrer.AbsoluteUri.ToString.Contains("EditParameter/")) Then
                    Session(STR_ReferringUrl) = NavigateURL()
                End If
            End If
        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            DNNUtilities.InjectCSS(Me.Page, ResolveUrl("Resources/Support/edit.css"))
            Page.ClientScript.RegisterClientScriptInclude(Me.GetType, "yeti", ResolveUrl("resources/support/yetii-min.js"))

            If Not Request.QueryString("ReportSetId") Is Nothing Then
                ReportSetId = Int32.Parse(Request.QueryString("ReportSetId").ToString)
            Else
                ReportSetId = -1
            End If

            If Not Page.IsPostBack Then
                BindSkinFolder(cboSkin)
                LoadReportSet()

                SaveReferringPage()
            End If

        End Sub

#End Region

#Region " Data ReportSet"
        Private Sub LoadReportSet()
            Dim objReportSetController As ReportSetController = New ReportSetController
            Dim objReportSet As ReportSetInfo = objReportSetController.GetReportSet(ReportSetId)

            If objReportSet Is Nothing Then
                ' create new
                txtName.Text = "New Report Set"
                SaveReportSet()
                Response.Redirect(NavigateReportSet(ReportSetId))
            End If

            With objReportSet
                txtName.Text = .ReportSetName

                Dim item As ListItem = cboSkin.Items.FindByValue(.ReportSetTheme)
                If Not item Is Nothing Then item.Selected = True Else cboSkin.Items(0).Selected = True

                cpConnection.ConnectionId = .ReportSetConnectionId
                txtHeader.Text = .ReportSetHeaderText
                txtFooter.Text = .ReportSetFooterText
                chkDebug.Checked = .ReportSetDebug
                txtRunCaption.Text = .RunCaption
                txtBackCaption.Text = .BackCaption
                txtParameterLayout.Text = .ParameterLayout
                chkAlwaysShowParameters.Checked = .AlwaysShowParameters

                ControlHelpers.InitDropDownByValue(ddlRenderMode, .RenderMode)
                chkAutoRun.Checked = .AutoRun

            End With

            ' report grid
            BindReport()
            ' parameter grid
            BindParameter()
        End Sub

        Private Sub SaveReportSet()
            Dim objReportSetController As ReportSetController = New ReportSetController
            Dim obj As ReportSetConfig = New ReportSetConfig
            ReportSetId = objReportSetController.UpdateReportSet(ModuleId, ReportSetId, txtName.Text, cboSkin.SelectedItem.Value, cpConnection.ConnectionId, txtHeader.Text, txtFooter.Text, chkDebug.Checked, txtRunCaption.Text, txtBackCaption.Text, txtParameterLayout.Text, chkAlwaysShowParameters.Checked, ddlRenderMode.SelectedValue, chkAutoRun.Checked, Serialization.SerializeObject(obj, GetType(ReportSetConfig)))
        End Sub

        Private Sub BindSkinFolder(ByVal o As ListControl)
            Dim skinFolder As New IO.DirectoryInfo(Server.MapPath(ResolveUrl("Skins")))
            o.Items.Clear()
            o.Items.Add(New ListItem("None", "None"))
            For Each folder As IO.DirectoryInfo In skinFolder.GetDirectories()
                o.Items.Add(folder.Name)
            Next
        End Sub

#End Region

#Region " Data Report"
        ' report
        Private Sub DeleteReport(ByVal ReportId As Integer)
            Dim objReportController As ReportController = New ReportController
            objReportController.DeleteReport(ReportId)
        End Sub

        Private Sub MoveReport(ByVal ReportId As Integer, ByVal Increment As Integer)
            Dim objReportController As ReportController = New ReportController
            objReportController.UpdateReportOrder(ReportId, Increment)
        End Sub

        Private Sub CopyReport(ByVal ReportId As Integer)
            Dim objReportController As ReportController = New ReportController
            Dim objReport As ReportInfo = objReportController.GetReport(ReportId)
            With objReport
                objReportController.UpdateReport(ReportSetId, -1, .ReportTypeId, "Copy of " & .ReportName, .ReportTheme, .ReportConnectionId, .ReportHeaderText, .ReportFooterText, .ReportCommand, .ReportConfig, -1, .ReportDrilldownReportId, .ReportDrilldownFieldname, .ReportNoItemsText, .ReportPageTitle, .ReportCommandCacheTimeout, .ReportMetaDescription)
            End With

        End Sub

        Private Sub BindReport()

            Localization.LocalizeDataGrid(dgReport, LocalResourceFile)

            Dim objReportList As ArrayList
            Dim objReportSetController As ReportSetController = New ReportSetController

            objReportList = objReportSetController.GetReportSetReport(ReportSetId)
            ' save Report count
            _ReportCount = objReportList.Count

            ' bind
            dgReport.DataSource = objReportList
            dgReport.DataBind()

            ' commands
            cmdAddReport.Visible = (ReportSetId > -1)
        End Sub

#End Region

#Region " Data Parameter"
        Private Sub DeleteParameter(ByVal ParameterId As Integer)
            Dim objParameterController As ParameterController = New ParameterController
            objParameterController.DeleteParameter(ParameterId)
        End Sub

        Private Sub MoveParameter(ByVal ParameterId As Integer, ByVal Increment As Integer)
            Dim objParameterController As ParameterController = New ParameterController
            objParameterController.UpdateParameterOrder(ParameterId, Increment)
        End Sub

        Private Sub CopyParameter(ByVal ParameterId As Integer)
            Dim objParameterController As ParameterController = New ParameterController
            Dim objParameter As ParameterInfo = objParameterController.GetParameter(ParameterId)
            Dim NewParameterId As Integer = 0
            With objParameter
                NewParameterId = objParameterController.UpdateParameter(ReportSetId, -1, "Copy of " & .ParameterName, .ParameterCaption, .ParameterTypeId, .ParameterConfig, -1)
            End With

        End Sub
        Private Sub BindParameter()

            Localization.LocalizeDataGrid(dgParameter, LocalResourceFile)

            Dim objParameterList As ArrayList
            Dim objReportSetController As ReportSetController = New ReportSetController

            objParameterList = objReportSetController.GetReportSetParameter(ReportSetId)
            ' save parameter count
            _ParameterCount = objParameterList.Count

            ' bind
            dgParameter.DataSource = objParameterList
            dgParameter.DataBind()

            ' commands
            cmdAddParameter.Visible = (ReportSetId > -1)

        End Sub
#End Region

#Region " Navigation"
        Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
            If Page.IsValid Then

                SaveReportSet()

                NavigateBack()
            End If

        End Sub

        Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
            NavigateBack()
        End Sub

        Private Sub NavigateBack()
            If Session(STR_ReferringUrl) IsNot Nothing Then
                Response.Redirect(Session(STR_ReferringUrl).ToString)
            End If
        End Sub

        Private Function NavigateReportSet(ByVal ReportSetId As Integer) As String
            Return EditUrl("ReportSetId", ReportSetId.ToString, "EditReportSet")
        End Function

        Private Function NavigateReport(ByVal ReportSetId As Integer, ByVal ReportId As Integer) As String
            Return EditUrl("ReportId", ReportId.ToString, "EditReport", String.Format("ReportSetId={0}", ReportSetId))
        End Function

        Private Function NavigateParameter(ByVal ReportSetId As Integer, ByVal ParameterId As Integer) As String
            Return EditUrl("ParameterId", ParameterId.ToString, "EditParameter", String.Format("ReportSetId={0}", ReportSetId))
        End Function
#End Region

#Region " Report Grid"
        Private Sub cmdAddReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddReport.Click
            Response.Redirect(NavigateReport(ReportSetId, -1))
        End Sub

        Private Sub dgReport_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgReport.ItemCommand
            Dim ReportId As Integer = Int32.Parse(dgReport.DataKeys(e.Item.ItemIndex).ToString)
            Select Case e.CommandName.ToLower
                Case "edit"
                    Response.Redirect(NavigateReport(ReportSetId, ReportId))
                Case "delete"
                    DeleteReport(ReportId)
                    BindReport()
                Case "up"
                    MoveReport(ReportId, -1)
                    BindReport()
                Case "down"
                    MoveReport(ReportId, 1)
                    BindReport()
                Case "copy"
                    CopyReport(ReportId)
                    BindReport()
            End Select
        End Sub

        Private Sub dgReport_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgReport.ItemCreated
            ' process data rows only (skip the header, footer etc.)
            If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = _
                ListItemType.AlternatingItem Then
                ' get a reference to the LinkButton of this row,
                '  and add the javascript confirmation
                Dim lnkDelete As LinkButton = CType(e.Item.FindControl("cmdDeleteReport"), _
                    LinkButton)
                If Not lnkDelete Is Nothing Then
                    lnkDelete.Attributes.Add("onclick", _
                        "return confirm('Are you sure you want to delete this report?');")
                End If
            End If
        End Sub

        Private Sub dgReport_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgReport.ItemDataBound
            Dim ib As ImageButton
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                If e.Item.ItemIndex = 0 Then
                    ib = CType(e.Item.FindControl("cmdMoveReportUp"), ImageButton)
                    If Not ib Is Nothing Then
                        ib.Visible = False
                    End If
                Else
                    If e.Item.ItemIndex = _ReportCount - 1 Then
                        ib = CType(e.Item.FindControl("cmdMoveReportDown"), ImageButton)
                        If Not ib Is Nothing Then
                            ib.Visible = False
                        End If
                    End If

                End If
            End If
        End Sub

#End Region

#Region " Parameter Grid"
        Private Sub cmdAddParameter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddParameter.Click
            Response.Redirect(NavigateParameter(ReportSetId, -1))
        End Sub

        Private Sub dgParameter_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgParameter.ItemCommand
            Dim ParameterId As Integer = Int32.Parse(dgParameter.DataKeys(e.Item.ItemIndex).ToString)
            Select Case e.CommandName.ToLower
                Case "edit"
                    Response.Redirect(NavigateParameter(ReportSetId, ParameterId))
                Case "delete"
                    DeleteParameter(ParameterId)
                    BindParameter()
                Case "up"
                    MoveParameter(ParameterId, -1)
                    BindParameter()
                Case "down"
                    MoveParameter(ParameterId, 1)
                    BindParameter()
                Case "copy"
                    CopyParameter(ParameterId)
                    BindParameter()

            End Select
        End Sub
        Private Sub dgParameter_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgParameter.ItemCreated
            ' process data rows only (skip the header, footer etc.)
            If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = _
                ListItemType.AlternatingItem Then
                ' get a reference to the LinkButton of this row,
                '  and add the javascript confirmation
                Dim lnkDelete As LinkButton = CType(e.Item.FindControl("cmdDeleteParameter"), _
                    LinkButton)
                If Not lnkDelete Is Nothing Then
                    lnkDelete.Attributes.Add("onclick", _
                        "return confirm('Are you sure you want to delete this parameter?');")
                End If
            End If

        End Sub

        Private Sub dgParameter_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgParameter.ItemDataBound
            Dim ib As ImageButton
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                If e.Item.ItemIndex = 0 Then
                    ib = CType(e.Item.FindControl("cmdMoveParameterUp"), ImageButton)
                    If Not ib Is Nothing Then
                        ib.Visible = False
                    End If
                Else
                    If e.Item.ItemIndex = _ParameterCount - 1 Then
                        ib = CType(e.Item.FindControl("cmdMoveParameterDown"), ImageButton)
                        If Not ib Is Nothing Then
                            ib.Visible = False
                        End If
                    End If

                End If
            End If
        End Sub

#End Region

    End Class



End Namespace
