<%@ Register TagPrefix="dnnstuff" TagName="ConnectionPicker" Src="../../../controls/ConnectionPicker/ConnectionPickerControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control Language="C#" Inherits="DNNStuff.SQLViewPro.StandardParameters.FlowListParameterSettingsControl"
    CodeBehind="FlowListParameterSettingsControl.ascx.cs" AutoEventWireup="true"
    Explicit="True" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<div class="dnnForm" id="panels-settings">
    <div class="dnnFormExpandContent">
        <a href="">Expand All</a></div>
    <h2 id="Common" class="dnnFormSectionHead">
        <a href="#">
            <%=Localization.GetString("lblCommonHeader", LocalResourceFile)%></a></h2>
    <fieldset class="dnnClear">
        <div class="dnnFormItem">
            <dnn:Label ID="lblDefault" runat="server" CssClass="SubHead" ControlName="txtDefault"
                Suffix=":"></dnn:Label>
            <asp:TextBox ID="txtDefault" runat="server" CssClass="NormalTextBox" TextMode="SingleLine"
                Columns="50"></asp:TextBox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblRepeatColumns" runat="server" CssClass="SubHead" ControlName="txtRepeatColumns"
                Suffix=":"></dnn:Label>
            <asp:TextBox ID="txtRepeatColumns" runat="server" CssClass="NormalTextBox" TextMode="SingleLine"
                Columns="5"></asp:TextBox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblRepeatDirection" runat="server" CssClass="SubHead" ControlName="ddlRepeatDirection"
                Suffix=":"></dnn:Label>
            <asp:DropDownList ID="ddlRepeatDirection" runat="server" CssClass="NormalTextBox">
                <asp:ListItem Value="0">Horizontal</asp:ListItem>
                <asp:ListItem Value="1">Vertical</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblRepeatLayout" runat="server" CssClass="SubHead" ControlName="ddlRepeatLayout"
                Suffix=":"></dnn:Label>
            <asp:DropDownList ID="ddlRepeatLayout" runat="server" CssClass="NormalTextBox">
                <asp:ListItem Value="0">Flow</asp:ListItem>
                <asp:ListItem Value="1">Table</asp:ListItem>
            </asp:DropDownList>
        </div>
    </fieldset>
    <h2 id="H2" class="dnnFormSectionHead">
        <a href="#">
            <%=Localization.GetString("lblListValues", LocalResourceFile)%></a></h2>
    <div class="dnnFormMessage dnnFormInfo">
        <%=Localization.GetString("lblListValuesNote", LocalResourceFile)%></div>
    <fieldset class="dnnClear">
        <div class="dnnFormItem">
            <dnn:Label ID="lblList" runat="server" CssClass="SubHead" ControlName="txtList" Suffix=":">
            </dnn:Label>
            <asp:TextBox ID="txtList" Columns="50" TextMode="MultiLine" CssClass="NormalTextBox"
                runat="server" Rows="10"></asp:TextBox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblConnection" runat="server" CssClass="SubHead" ControlName="cpConnection"
                Suffix=":"></dnn:Label>
            <dnnstuff:ConnectionPicker ID="cpConnection" IncludePortalDefault="True" IncludeReportSetDefault="True"
                runat="server" CssClass="NormalTextBox"></dnnstuff:ConnectionPicker>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblCommand" runat="server" CssClass="SubHead" ControlName="txtCommand"
                Suffix=":"></dnn:Label>
            <asp:TextBox ID="txtCommand" Columns="70" TextMode="MultiLine" CssClass="NormalTextBox"
                         runat="server" Rows="10"></asp:TextBox><br />
            <asp:CustomValidator ID="vldCommand" CssClass="NormalRed" runat="server" ErrorMessage=""
                Display="Dynamic" ControlToValidate="txtCommand" OnServerValidate="vldCommand_ServerValidate"></asp:CustomValidator>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblCommandCacheTimeout" runat="server" ControlName="txtCommandCacheTimeout" />
            <asp:TextBox ID="txtCommandCacheTimeout" runat="server" CssClass="dnnNoMinWidth dnnFormRequired" Columns="10" />
            <asp:RequiredFieldValidator runat="server" ID="reqCommandCacheTimeout" CssClass="dnnFormMessage dnnFormError" ControlToValidate="txtCommandCacheTimeout" Display="Dynamic" ErrorMessage="Cache timeout is required"></asp:RequiredFieldValidator>
            <asp:CompareValidator runat="server" ID="cmpCommandCacheTimeout" CssClass="dnnFormMessage dnnFormError" ControlToValidate="txtCommandCacheTimeout" Display="Dynamic" ErrorMessage="Cache timeout must be zero or greater" ValueToCompare="0" Type="Integer" Operator="GreaterThanEqual"></asp:CompareValidator>
        </div>
        <div class="dnnFormItem">
            <asp:LinkButton ID="cmdQueryTest" runat="server" CssClass="Normal" CausesValidation="false" OnClick="cmdQueryTest_Click"><%=Localization.GetString("cmdQueryTest.Text", LocalResourceFile)%></asp:LinkButton>
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

