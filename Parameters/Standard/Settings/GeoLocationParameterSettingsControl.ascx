<%@ Control language="vb" Inherits="DNNStuff.SQLViewPro.StandardParameters.GeoLocationParameterSettingsControl" CodeBehind="GeoLocationParameterSettingsControl.ascx.vb" AutoEventWireup="false" Explicit="True" targetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnForm" id="panel-settings">
	<div class="dnnFormMessage dnnFormInfo">
		<%=Localization.GetString("lblParameterNote", LocalResourceFile)%></div>
	<div class="dnnFormExpandContent">
		<a href="">Expand All</a></div>
	<h2 id="Common" class="dnnFormSectionHead">
		<a href="#">
			<%=Localization.GetString("lblCommonHeader", LocalResourceFile)%></a></h2>
	<fieldset class="dnnClear">
		<div class="dnnFormItem">
			<dnn:Label ID="lblEnableHighAccuracy" Runat="server" controlname="chkEnableHighAccuracy" suffix=":"></dnn:Label>
			<asp:Checkbox id="chkEnableHighAccuracy" runat="server"></asp:Checkbox>
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="lblTimeout" Runat="server" controlname="txtTimeout" suffix=":" />
			<asp:textbox id="txtTimeout" runat="server" />
			<asp:RequiredFieldValidator ID="txtTimeoutRequired" runat="server" ControlToValidate="txtTimeout" ErrorMessage="Required" />
			<asp:CompareValidator ID="txtTimeoutInteger" runat="server" ControlToValidate="txtTimeout" ValueToCompare="0" Operator="GreaterThanEqual" Type="Integer" ErrorMessage="Must be an integer, 0 or greater" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="lblMaximumAge" Runat="server" controlname="txtMaximumAge" suffix=":" />
			<asp:textbox id="txtMaximumAge" runat="server" />
			<asp:RequiredFieldValidator ID="txtMaximumAgeRequired" runat="server" ControlToValidate="txtMaximumAge" ErrorMessage="Required" />
			<asp:CompareValidator ID="txtMaximumAgeInteger" runat="server" ControlToValidate="txtMaximumAge" ValueToCompare="0" Operator="GreaterThanEqual" Type="Integer" ErrorMessage="Must be an integer, 0 or greater" />
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
