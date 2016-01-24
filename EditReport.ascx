<%@ Register TagPrefix="dnnstuff" TagName="ConnectionPicker" Src="controls/ConnectionPicker/ConnectionPickerControl.ascx" %>
<%@ Control Language="C#" Inherits="DNNStuff.SQLViewPro.EditReport" CodeBehind="EditReport.ascx.cs" AutoEventWireup="true" Explicit="True" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnForm dnnClear">
    <div id="editsettings" class="tabslayout">
        <ul id="editsettings-nav" class="tabslayout">
            <li><a href="#tab1"><span>
                <%=Localization.GetString("TabCaption_Tab1", LocalResourceFile)%></span></a></li>
            <li><a href="#tab2"><span>
                <%=Localization.GetString("TabCaption_Tab2", LocalResourceFile)%></span></a></li>
        </ul>
        <div class="tabs-container">
            <div class="tab" id="tab1">
                <div class="dnnFormItem">
                    <dnn:Label ID="lblName" runat="server" ControlName="txtName" Suffix=":" />
                    <asp:TextBox ID="txtName" runat="server" TextMode="SingleLine" Columns="50" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblSkin" runat="server" ControlName="cboSkin" Suffix=":" />
                    <asp:DropDownList ID="cboSkin" runat="server" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblQuery" runat="server" ControlName="txtQuery" Suffix=":" />
                    <asp:TextBox ID="txtQuery" runat="server" TextMode="MultiLine" Rows="10" Columns="70" Width="100%"></asp:TextBox><br />
                    <asp:CustomValidator ID="vldQuery" runat="server" CssClass="NormalRed" Display="Dynamic" ControlToValidate="txtQuery" ResourceKey="vldQuery.ErrorText" Enabled="false" OnServerValidate="vldQuery_ServerValidate"></asp:CustomValidator>
                    <div class="dnnLabel"></div>
                    <asp:LinkButton ID="cmdQueryTest" runat="server" Text="Test Query" ResourceKey="cmdQueryTest" CssClass="dnnPrimaryAction"  OnClick="cmdQueryTest_Click"/><br />
                    <asp:Label ID="lblQueryTestResults" runat="server" EnableViewState="False" CssClass="NormalText" Text="" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblConnection" runat="server" ControlName="cpConnection" Suffix=":" />
                    <dnnstuff:ConnectionPicker ID="cpConnection" IncludePortalDefault="True" IncludeReportSetDefault="True" runat="server" CssClass="NormalTextBox"></dnnstuff:ConnectionPicker>
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblCommandCacheTimeout" runat="server" ControlName="txtCommandCacheTimeout" />
                    <asp:TextBox ID="txtCommandCacheTimeout" runat="server" CssClass="dnnNoMinWidth dnnFormRequired" Columns="10" />
                    <asp:RequiredFieldValidator runat="server" ID="reqCommandCacheTimeout" CssClass="dnnFormMessage dnnFormError" ControlToValidate="txtCommandCacheTimeout" Display="Dynamic" ErrorMessage="Cache timeout is required"></asp:RequiredFieldValidator>
                    <asp:CompareValidator runat="server" ID="cmpCommandCacheTimeout" CssClass="dnnFormMessage dnnFormError" ControlToValidate="txtCommandCacheTimeout" Display="Dynamic" ErrorMessage="Cache timeout must be zero or greater" ValueToCompare="0" Type="Integer" Operator="GreaterThanEqual"></asp:CompareValidator>
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblCommandCacheScheme" runat="server" ControlName="ddCommandCacheScheme" Suffix=":" />
                    <asp:DropDownList runat="server" ID="ddCommandCacheScheme">
                        <asp:ListItem Value="Sliding">Sliding</asp:ListItem>
                        <asp:ListItem Value="Absolute">Absolute</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblNoItems" runat="server" ControlName="txtNoItems" Suffix=":" />
                    <asp:TextBox ID="txtNoItems" runat="server" TextMode="MultiLine" Rows="2" Columns="70" Width="100%"></asp:TextBox>
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblHeader" runat="server" ControlName="txtHeader" Suffix=":" />
                    <asp:TextBox ID="txtHeader" runat="server" TextMode="MultiLine" Rows="2" Columns="70" Width="100%"></asp:TextBox>
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblFooter" runat="server" ControlName="txtFooter" Suffix=":" />
                    <asp:TextBox ID="txtFooter" runat="server" TextMode="MultiLine" Rows="2" Columns="70" Width="100%"></asp:TextBox>
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblPageTitle" runat="server" ControlName="txtPageTitle" Suffix=":" />
                    <asp:TextBox ID="txtPageTitle" runat="server" TextMode="MultiLine" Rows="2" Columns="70" Width="100%"></asp:TextBox>
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblMetaDescription" runat="server" ControlName="txtMetaDescription" Suffix=":" />
                    <asp:TextBox ID="txtMetaDescription" runat="server" TextMode="MultiLine" Rows="2" Columns="70" Width="100%"></asp:TextBox>
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblDrilldownReport" runat="server" ControlName="ddDrilldownReportId" Suffix=":" />
                    <asp:DropDownList ID="ddDrilldownReportId" runat="server" CssClass="NormalTextBox">
                    </asp:DropDownList>
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblDrilldownFieldname" runat="server" ControlName="txtDrilldownFieldname" Suffix=":" />
                    <asp:TextBox ID="txtDrilldownFieldname" runat="server" Rows="1" Columns="20"></asp:TextBox>
                </div>
            </div>
            <div class="tab" id="tab2">
                <div class="dnnFormItem">
                    <dnn:Label ID="lblReportType" runat="server" ControlName="ddReportType" Suffix=":" />
                    <asp:DropDownList ID="ddReportType" runat="server" AutoPostBack="True"  OnSelectedIndexChanged="ddReportType_SelectedIndexChanged"/>
                    <asp:PlaceHolder ID="phReportSettings" runat="server" />
                </div>
            </div>
        </div>
    </div>
    <ul class="dnnActions dnnClear">
        <li>
            <asp:LinkButton ID="cmdUpdate" Text="Update" resourcekey="cmdUpdate" CausesValidation="True" runat="server" CssClass="dnnPrimaryAction"  OnClick="cmdUpdate_Click"/></li>
        <li>
            <asp:LinkButton ID="cmdCancel" Text="Cancel" resourcekey="cmdCancel" CausesValidation="False" runat="server" CssClass="dnnSecondaryAction"  OnClick="cmdCancel_Click"/></li>
    </ul>
</div>
<script type="text/javascript">
    var tabber1 = new Yetii({
        id: 'editsettings',
        persist: true
    });
</script>

