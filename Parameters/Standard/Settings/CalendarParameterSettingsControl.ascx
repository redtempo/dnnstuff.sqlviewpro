<%@ Control language="vb" Inherits="DNNStuff.SQLViewPro.StandardParameters.CalendarParameterSettingsControl" CodeBehind="CalendarParameterSettingsControl.ascx.vb" AutoEventWireup="false" Explicit="True" targetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnForm" id="panels-settings">
	<div class="dnnFormExpandContent">
		<a href="">Expand All</a></div>
	<h2 id="Common" class="dnnFormSectionHead">
		<a href="#">
			<%=Localization.GetString("lblCommonHeader", LocalResourceFile)%></a></h2>
	<fieldset class="dnnClear">
		<div class="dnnFormItem">
            <dnn:Label ID="lblDefault" Runat="server" CssClass="SubHead" controlname="txtDefault" suffix=":"></dnn:Label>
			<asp:textbox id="txtDefault" runat="server" CssClass="NormalTextBox" TextMode="SingleLine" Columns="50"></asp:textbox>
		</div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblDatabaseDateFormat" Runat="server" CssClass="SubHead" controlname="txtDatabaseDateFormat" suffix=":"></dnn:Label>
			<asp:textbox id="txtDatabaseDateFormat" runat="server" CssClass="NormalTextBox" TextMode="SingleLine" Columns="30"></asp:textbox>
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
