<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="TemplateReportSettingsControl.ascx.vb" Inherits="DNNStuff.SQLViewPro.StandardReports.TemplateReportSettingsControl" EnableViewState="false" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnForm" id="panels-settings">
	<div class="dnnFormExpandContent">
		<a href="">Expand All</a></div>
	<h2 id="Template" class="dnnFormSectionHead">
		<a href="#">
			<%=Localization.GetString("lblTemplateHeader", LocalResourceFile)%></a></h2>
	<fieldset class="dnnClear">
		<div class="dnnFormItem">
            <dnn:Label ID="lblTemplateText" runat="server" ControlName="txtTemplateText"  Suffix=":" />
            <asp:TextBox ID="txtTemplateText" Runat="server" columns="80" rows="50" TextMode="MultiLine" />
		</div>
	</fieldset>
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
