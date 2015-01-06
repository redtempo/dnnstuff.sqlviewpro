<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SSRSReportSettingsControl.ascx.vb" Inherits="DNNStuff.SQLViewPro.SSRSReports.SSRSReportSettingsControl" EnableViewState="false" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="URL" Src="~/controls/URLControl.ascx" %>
<div class="dnnForm dnnClear" id="panels-settings">
    <h2 id="ReportConfiguration" class="dnnFormSectionHead">
        <a href="#">
            <%=Localization.GetString("lblReportConfigurationHeader", LocalResourceFile)%></a></h2>
    <div class="dnnClear">
        <fieldset>
            <div class="dnnFormItem">
                <dnn:Label ID="lblProcessingMode" runat="server" ControlName="ddlProcessingMode"
                    Suffix=":" />
                <asp:DropDownList runat="server" ID="ddlProcessingMode" AutoPostBack="true">
                    <asp:ListItem Value="Remote">Remote</asp:ListItem>
                    <asp:ListItem Value="Local">Local</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div id="pnlRemote" runat="server">
                <div class="dnnFormItem">
                    <dnn:Label ID="lblReportServerUrl" runat="server" ControlName="txtReportServerUrl"
                        Suffix=":" Text="Report Server Url" />
                    <asp:TextBox ID="txtReportServerUrl" runat="server" Columns="80" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblReportServerReportPath" runat="server" ControlName="txtReportServerReportPath"
                        Suffix=":" />
                    <asp:TextBox ID="txtReportServerReportPath" runat="server" Columns="80" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblReportServerUsername" runat="server" ControlName="txtReportServerUsername"
                        Suffix=":" />
                    <asp:TextBox ID="txtReportServerUsername" runat="server" Columns="80" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblReportServerPassword" runat="server" ControlName="txtReportServerPassword"
                        Suffix=":" />
                    <asp:TextBox ID="txtReportServerPassword" runat="server" Columns="80" TextMode="Password" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblReportServerDomain" runat="server" ControlName="txtReportServerDomain"
                        Suffix=":" />
                    <asp:TextBox ID="txtReportServerDomain" runat="server" Columns="80" />
                </div>
            </div>
            <div id="pnlLocal" runat="server">
                <div class="dnnFormItem">
                    <dnn:Label ID="lblLocalReportPath" runat="server" ControlName="urlLocalReportPath"
                        Suffix=":" />
                    <div class="dnnLeft">
                        <dnn:URL ID="urlLocalReportPath" runat="server" Width="275" Required="False" ShowTrack="False"
                            ShowNewWindow="False" ShowLog="False" UrlType="F" ShowUrls="False" ShowFiles="True"
                            ShowTabs="False"></dnn:URL>
                    </div>
                </div>
            </div>
            <div class="dnnFormItem">
                <dnn:Label ID="lblAdditionalParameters" runat="server" ControlName="txtAdditionalParameters"
                    Suffix=":" />
                <asp:TextBox ID="txtAdditionalParameters" runat="server" Columns="80" Rows="2" TextMode="MultiLine" />
            </div>
        </fieldset>
        <h2 id="ReportViewer" class="dnnFormSectionHead">
            <a href="#">
                <%=Localization.GetString("lblReportViewerHeader", LocalResourceFile)%></a></h2>
        <div class="dnnClear">
            <fieldset class="dnnClear">
                <div class="dnnFormItem">
                    <dnn:Label ID="lblViewerWidth" runat="server" ControlName="txtViewerWidth" Suffix=":" />
                    <asp:TextBox ID="txtViewerWidth" runat="server" Columns="5" CssClass="dnnNoMinWidth" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblViewerHeight" runat="server" ControlName="txtViewerHeight" Suffix=":" />
                    <asp:TextBox ID="txtViewerHeight" runat="server" Columns="5" CssClass="dnnNoMinWidth" />
                </div>
                <div class="dnnFormItem">
                    <div class="dnnFormMessage dnnFormInfo">Some options are only available when using the report viewer ActiveX control with IE only</div>
                    <dnn:Label ID="lblReportOptions" runat="server" ControlName="lstReportOptions" Suffix=":" />
                    <asp:CheckBoxList runat="server" ID="lstReportOptions" RepeatColumns="1" RepeatDirection="Horizontal"
                        CssClass="dnnCheckBoxes dnnLeft">
                        <asp:ListItem Value="ShowBackButton">Show Back Button</asp:ListItem>
                        <asp:ListItem Value="ShowDocumentMapButton">Show Document Map Button</asp:ListItem>
                        <asp:ListItem Value="ShowExportControls">Show Export Controls</asp:ListItem>
                        <asp:ListItem Value="ShowFindControls">Show Find Controls</asp:ListItem>
                        <asp:ListItem Value="ShowPageNavigationControls">Show Page Navigation Controls</asp:ListItem>
                        <asp:ListItem Value="ShowParameterPrompts">Show Parameter Prompts</asp:ListItem>
                        <asp:ListItem Value="ShowPrintButton">Show Print Button</asp:ListItem>
                        <asp:ListItem Value="ShowPromptAreaButton">Show Prompt Area Button</asp:ListItem>
                        <asp:ListItem Value="ShowRefreshButton">Show Refresh Button</asp:ListItem>
                        <asp:ListItem Value="ShowToolBar">Show ToolBar</asp:ListItem>
                        <asp:ListItem Value="ShowZoomControl">Show Zoom Control</asp:ListItem>
                        <asp:ListItem Value="ShowWaitControlCancelLink">Show Wait Control Cancel Link</asp:ListItem>
                    </asp:CheckBoxList>
                </div>
             </fieldset>
         </div>
    </div>
</div>
<script type="text/javascript">
	jQuery(function ($) {
		var setupModule = function () {
			$('#panels-settings').dnnPanels();
			$('#panels-settings .dnnFormExpandContent a').dnnExpandAll({
				targetArea: '#panels-settings'
			});
		};
		setupModule();
		Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
			// note that this will fire when _any_ UpdatePanel is triggered,
			// which may or may not cause an issue
			setupModule();
		});
	});
</script>
