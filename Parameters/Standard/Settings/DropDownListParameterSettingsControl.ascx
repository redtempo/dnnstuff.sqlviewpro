<%@ Control Language="vb" Inherits="DNNStuff.SQLViewPro.StandardParameters.DropDownListParameterSettingsControl" CodeBehind="DropDownListParameterSettingsControl.ascx.vb" AutoEventWireup="false" Explicit="True" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnnstuff" TagName="ConnectionPicker" Src="../../../controls/ConnectionPicker/ConnectionPickerControl.ascx" %>
<div class="dnnForm" id="panels-settings">
	<div class="dnnFormExpandContent">
		<a href="">Expand All</a></div>
	<h2 id="Common" class="dnnFormSectionHead">
		<a href="#">
			<%=Localization.GetString("lblCommonHeader", LocalResourceFile)%></a></h2>
	<fieldset class="dnnClear">
		<div class="dnnFormItem">
			<dnn:Label ID="lblDefault" runat="server" ControlName="txtDefault" Suffix=":"></dnn:Label>
			<asp:TextBox ID="txtDefault" runat="server" TextMode="SingleLine" Columns="50"></asp:TextBox>
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="lblAutoPostback" runat="server" ControlName="chkAutoPostback" Suffix=":"></dnn:Label>
			<asp:CheckBox ID="chkAutoPostback" runat="server" />
		</div>
    </fieldset>
    <h2 id="H2" class="dnnFormSectionHead">
        <a href="#">
            <%=Localization.GetString("lblListValues", LocalResourceFile)%></a></h2>
    <div class="dnnFormMessage dnnFormInfo">
        <%=Localization.GetString("lblListValuesNote", LocalResourceFile)%></div>
    <fieldset class="dnnClear">
		<div class="dnnFormItem">
			<dnn:Label ID="lblList" runat="server" ControlName="txtList" Suffix=":"></dnn:Label>
			<asp:TextBox ID="txtList" Columns="50" TextMode="MultiLine" runat="server" Rows="10"></asp:TextBox>
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="lblConnection" runat="server" ControlName="cpConnection" Suffix=":"></dnn:Label>
			<dnnstuff:ConnectionPicker ID="cpConnection" IncludePortalDefault="True" IncludeReportSetDefault="True" runat="server"></dnnstuff:ConnectionPicker>
		</div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblCommand" runat="server" ControlName="txtCommand"
                Suffix=":"></dnn:Label>
            <asp:TextBox ID="txtCommand" Columns="70" TextMode="MultiLine"
                         runat="server" Rows="10"></asp:TextBox><br />
            <asp:CustomValidator ID="vldCommand" CssClass="NormalRed" runat="server" ErrorMessage=""
                Display="Dynamic" ControlToValidate="txtCommand"></asp:CustomValidator>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblCommandCacheTimeout" runat="server" ControlName="txtCommandCacheTimeout" />
            <asp:TextBox ID="txtCommandCacheTimeout" runat="server" CssClass="dnnNoMinWidth dnnFormRequired" Columns="10" />
            <asp:RequiredFieldValidator runat="server" ID="reqCommandCacheTimeout" CssClass="dnnFormMessage dnnFormError" ControlToValidate="txtCommandCacheTimeout" Display="Dynamic" ErrorMessage="Cache timeout is required"></asp:RequiredFieldValidator>
            <asp:CompareValidator runat="server" ID="cmpCommandCacheTimeout" CssClass="dnnFormMessage dnnFormError" ControlToValidate="txtCommandCacheTimeout" Display="Dynamic" ErrorMessage="Cache timeout must be zero or greater" ValueToCompare="0" Type="Integer" Operator="GreaterThanEqual"></asp:CompareValidator>
        </div>
        <div class="dnnFormItem">
            <asp:LinkButton ID="cmdQueryTest" runat="server" CssClass="Normal" CausesValidation="false"><%=Localization.GetString("cmdQueryTest.Text", LocalResourceFile)%></asp:LinkButton>
            <asp:Label ID="lblQueryTestResults" runat="server" EnableViewState="False" CssClass="NormalText"
                Text="" />
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

