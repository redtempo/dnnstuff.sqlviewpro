<%@ Control Language="C#" Inherits="DNNStuff.SQLViewPro.EditConnection" CodeBehind="EditConnection.ascx.cs" AutoEventWireup="true" Explicit="True" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnForm dnnClear">
    <div class="dnnFormItem">
        <dnn:Label ID="lblConnectionName" runat="server" CssClass="SubHead" ControlName="txtName" Suffix=":"></dnn:Label>
        <asp:TextBox ID="txtName" runat="server" CssClass="NormalTextBox" TextMode="SingleLine" Columns="50"></asp:TextBox>
    </div>
    <div class="dnnFormItem">
        <dnn:Label ID="lblConnectionString" runat="server" CssClass="SubHead" ControlName="txtConnectionString" Suffix=":"></dnn:Label>
        <asp:TextBox ID="txtConnectionString" runat="server" CssClass="NormalTextBox" TextMode="MultiLine" Rows="2" Columns="70" Width="100%"></asp:TextBox>
        <asp:CustomValidator ID="vldConnectionStringValid" runat="server" ControlToValidate="txtConnectionString" EnableClientScript="False" Display="Dynamic" ErrorMessage="Connection String is invalid"  OnServerValidate="vldConnectionStringValid_ServerValidate"/>
    </div>
    <ul class="dnnActions dnnClear">
        <li>
            <asp:LinkButton ID="cmdUpdate" Text="Update" resourcekey="cmdUpdate" CausesValidation="True" runat="server" CssClass="dnnPrimaryAction"  OnClick="cmdUpdate_Click"/></li>
        <li>
            <asp:LinkButton ID="cmdCancel" Text="Cancel" resourcekey="cmdCancel" CausesValidation="False" runat="server" CssClass="dnnSecondaryAction"  OnClick="cmdCancel_Click"/></li>
    </ul>
</div>

