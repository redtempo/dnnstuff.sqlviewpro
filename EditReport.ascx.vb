'***************************************************************************/
'* EditReport.ascx.vb
'*
'* Copyright (c) 2004 by DNNStuff.
'* All rights reserved.
'*
'* Date:        August 9, 2004
'* Author:      Richard Edwards
'* Description: Portal Module for editing setting for a report grid
'*************/

Imports DotNetNuke
Imports DotNetNuke.Common
Imports System.Configuration
Imports System.IO

Namespace DNNStuff.SQLViewPro

    Partial Class EditReport
        Inherits Entities.Modules.PortalModuleBase
        Private Const STR_ReferringUrl As String = "EditReport_ReferringUrl"

        'standard
        Protected WithEvents cpConnection As Controls.ConnectionPickerControl

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()

            ' initialize
            ReportId = Int32.Parse(DNNUtilities.QueryStringDefault(Request, "ReportId", "-1"))
            ReportSetId = Int32.Parse(DNNUtilities.QueryStringDefault(Request, "ReportSetId", "-1"))

            InitReport()

        End Sub

#End Region

#Region " Page"

        Public Property ReportSetId() As Integer
        Public Property ReportId() As Integer
        Public Property Report() As ReportInfo
        Public Property ReportConfig() As String

        Private Sub SaveReferringPage()
            ' save referring page
            If Request.UrlReferrer Is Nothing Then
                Session(STR_ReferringUrl) = NavigateURL()
            Else
                Session(STR_ReferringUrl) = Request.UrlReferrer.AbsoluteUri
            End If
        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            DNNUtilities.InjectCSS(Me.Page, ResolveUrl("Resources/Support/edit.css"))
            Page.ClientScript.RegisterClientScriptInclude(Me.GetType, "yeti", ResolveUrl("resources/support/yetii-min.js"))

            If Page.IsPostBack Then
                RenderReportSettings()
            Else
                SaveReferringPage()

                ' drop down report type
                BindReportType()
                BindDrilldownReport()
                BindSkinFolder(cboSkin)
                BindReport()

                ' do report type
                RenderReportSettings()
                RetrieveReportSettings()
            End If

        End Sub

#End Region

#Region " Data"
        Private Sub InitReport()
            Dim objReportController As ReportController = New ReportController
            Dim objReport As ReportInfo = objReportController.GetReport(ReportId)

            ' load from database
            If objReport Is Nothing Then
                Report = New ReportInfo()
            Else
                Report = objReport
            End If
        End Sub
        Private Sub BindReport()

            With Report
                txtName.Text = .ReportName

                Dim li As ListItem = cboSkin.Items.FindByValue(.ReportTheme)
                If Not li Is Nothing Then li.Selected = True Else cboSkin.Items(0).Selected = True

                cpConnection.ConnectionId = .ReportConnectionId
                txtHeader.Text = .ReportHeaderText
                txtFooter.Text = .ReportFooterText
                txtQuery.Text = .ReportCommand
                txtCommandCacheTimeout.Text = .ReportCommandCacheTimeout.ToString()

                ' cache scheme
                li = ddCommandCacheScheme.Items.FindByValue(.ReportCommandCacheScheme)
                If Not li Is Nothing Then li.Selected = True Else ddCommandCacheScheme.Items.FindByValue("Sliding").Selected = True
                ddCommandCacheScheme.SelectedValue = .ReportCommandCacheScheme

                ' report type - default to GRID if not selected
                li = ddReportType.Items.FindByValue(.ReportTypeId.ToString)
                If Not li Is Nothing Then li.Selected = True Else ddReportType.Items.FindByValue("GRID").Selected = True
                ddReportType.SelectedValue = .ReportTypeId.ToString

                ' drilldown
                txtDrilldownFieldname.Text = .ReportDrilldownFieldname
                li = ddDrilldownReportId.Items.FindByValue(.ReportDrilldownReportId.ToString)
                If Not li Is Nothing Then li.Selected = True Else ddDrilldownReportId.Items.FindByValue("-1").Selected = True

                txtNoItems.Text = .ReportNoItemsText
                txtPageTitle.Text = .ReportPageTitle
                txtMetaDescription.Text = .ReportMetaDescription

            End With

        End Sub
        Private Sub BindSkinFolder(ByVal o As ListControl)
            Dim skinFolder As New IO.DirectoryInfo(Server.MapPath(ResolveUrl("Skins")))
            o.Items.Clear()
            o.Items.Add(New ListItem("Report Set Default", ""))
            o.Items.Add(New ListItem("None", "None"))
            For Each folder As IO.DirectoryInfo In skinFolder.GetDirectories()
                o.Items.Add(folder.Name)
            Next
        End Sub

        Private Sub SaveReport()
            RetrieveReportSettings()

            Dim objReportController As ReportController = New ReportController
            ReportId = objReportController.UpdateReport(ReportSetId, ReportId, ddReportType.SelectedValue, txtName.Text, cboSkin.SelectedItem.Value, cpConnection.ConnectionId, txtHeader.Text, txtFooter.Text, txtQuery.Text, ReportConfig, -1, Convert.ToInt32(ddDrilldownReportId.SelectedValue), txtDrilldownFieldname.Text, txtNoItems.Text, txtPageTitle.Text, Convert.ToInt32(txtCommandCacheTimeout.Text), txtMetaDescription.Text, ddCommandCacheScheme.SelectedValue)
        End Sub

        Private Sub BindReportType()
            Dim objReportTypeController As ReportTypeController = New ReportTypeController
            With ddReportType
                .DataTextField = "ReportTypeName"
                .DataValueField = "ReportTypeId"
                .DataSource = objReportTypeController.ListReportType()
                .DataBind()
            End With
        End Sub

        Private Sub BindDrilldownReport()
            Dim objReportList As ArrayList
            Dim objReportSetController As ReportSetController = New ReportSetController
            objReportList = objReportSetController.GetReportSetReport(ReportSetId)

            With ddDrilldownReportId
                .DataValueField = "ReportId"
                .DataTextField = "ReportName"
                .DataSource = objReportList
                .DataBind()
            End With

            ' add the default to the start of the list
            Dim li As New ListItem(Localization.GetString("NoDrilldown.Text", LocalResourceFile), "-1")
            ddDrilldownReportId.Items.Insert(0, li)

            ' remove this report
            If ReportId > -1 Then
                li = ddDrilldownReportId.Items.FindByValue(ReportId.ToString)
                If Not li Is Nothing Then ddDrilldownReportId.Items.Remove(li)
            End If

        End Sub

#End Region

#Region " Navigation"
        Private Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdUpdate.Click

            If Page.IsValid Then
                SaveReport()

                NavigateBack()
            End If

        End Sub

        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
            NavigateBack()
        End Sub

        Private Sub NavigateBack()
            If Session(STR_ReferringUrl) IsNot Nothing Then
                Response.Redirect(Session(STR_ReferringUrl).ToString)
            End If
        End Sub

#End Region

        Private Sub RetrieveReportSettings()
            Dim objReportSettings As Controls.ReportSettingsControlBase = DirectCast(phReportSettings.FindControl("ReportSettings"), Controls.ReportSettingsControlBase)

            ReportConfig = objReportSettings.UpdateSettings

        End Sub
        Private Sub RenderReportSettings()
            Dim reportTypeId As String = ddReportType.SelectedValue

            Dim objReportTypeController As ReportTypeController = New ReportTypeController

            Dim objReportType As ReportTypeInfo = objReportTypeController.GetReportType(reportTypeId)

            Dim objReportSettingsBase As Controls.ReportSettingsControlBase
            objReportSettingsBase = CType(LoadControl(Me.ResolveUrl(objReportType.ReportTypeSettingsControlSrc)), Controls.ReportSettingsControlBase)

            With objReportSettingsBase
                .ID = "ReportSettings"
                If Not Report Is Nothing Then
                    .LoadSettings(Report.ReportConfig)
                Else
                    .LoadSettings("")
                End If
            End With
            phReportSettings.Controls.Add(New LiteralControl(String.Format("<h3>{0} Settings</h3>", objReportType.ReportTypeName)))
            phReportSettings.Controls.Add(objReportSettingsBase)

        End Sub
        Private Sub ddReportType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddReportType.SelectedIndexChanged
            If Not Report Is Nothing Then
                RetrieveReportSettings()
            End If
        End Sub

#Region " Validation"
        Private Sub vldQuery_ServerValidate(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles vldQuery.ServerValidate
            Dim msg As String = ""
            args.IsValid = Services.Data.Query.IsQueryValid(txtQuery.Text, ConnectionController.GetConnectionString(cpConnection.ConnectionId, ReportSetId), msg)
            vldQuery.ErrorMessage = msg
        End Sub

        Private Sub cmdQueryTest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdQueryTest.Click
            Dim msg As String = ""
            Dim isValid As Boolean = Services.Data.Query.IsQueryValid(txtQuery.Text, ConnectionController.GetConnectionString(cpConnection.ConnectionId, ReportSetId), msg)

            lblQueryTestResults.Text = msg
            lblQueryTestResults.CssClass = "NormalGreen"
            If Not isValid Then lblQueryTestResults.CssClass = "NormalRed"

        End Sub

#End Region
    End Class

End Namespace
