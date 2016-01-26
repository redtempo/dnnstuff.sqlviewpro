<%@ Control Language="C#" Inherits="DNNStuff.SQLViewPro.EditParameter" CodeBehind="EditParameter.ascx.cs" AutoEventWireup="true" Explicit="True" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnForm dnnClear">
    <div id="editsettings" class="tabslayout">
        <ul id="editsettings-nav" class="tabslayout">
            <li><a href="#tab1"><span>
                <%=Localization.GetString("TabCaption_Tab1", LocalResourceFile)%></span></a></li>
        </ul>
        <div class="tabs-container">
            <div class="tab" id="tab1">
                <div class="dnnFormItem">
                    <dnn:Label ID="lblParameterType" runat="server" CssClass="SubHead" ControlName="ddParameterType" Suffix=":" />
                    <asp:DropDownList ID="ddParameterType" runat="server" CssClass="NormalTextBox" AutoPostBack="True"  OnSelectedIndexChanged="ddParameterType_SelectedIndexChanged"/>
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblName" runat="server" CssClass="SubHead" ControlName="txtName" Suffix=":" />
                    <asp:TextBox ID="txtName" runat="server" CssClass="NormalTextBox" TextMode="SingleLine" Columns="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="txtName_Required" runat="server" ControlToValidate="txtName" Display="Dynamic" ErrorMessage="<br />Name is required" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblCaption" runat="server" CssClass="SubHead" ControlName="txtCaption" Suffix=":"></dnn:Label>
                    <asp:TextBox ID="txtCaption" runat="server" CssClass="NormalTextBox" TextMode="SingleLine" Columns="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="txtCaption_Required" runat="server" ControlToValidate="txtCaption" Display="Dynamic" ErrorMessage="<br />Caption is required" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblParameterTypeSettingsHeader" runat="server" />
                    <asp:PlaceHolder ID="phParameterSettings" runat="server" />
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

