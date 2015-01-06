<%@ Control Language="vb" CodeBehind="BrowseRepository.ascx.vb" AutoEventWireup="false" Explicit="True" Inherits="DNNStuff.SQLViewPro.BrowseRepository" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div>
    <%=Localization.GetString("DocumentationHelp.Text", LocalResourceFile)%></div>
<div class="dnnForm dnnClear">
    <div class="dnnFormItem">
        <dnn:Label ID="lblRepository" CssClass="SubHead" runat="server" ControlName="cboRepository" Suffix=":" />
        <asp:DropDownList ID="cboRepository" runat="server" AutoPostBack="False" />
    </div>
    <div class="dnnFormItem">
        <asp:PlaceHolder ID="phResults" runat="server" />
    </div>
    <ul class="dnnActions dnnClear">
        <li>
            <asp:LinkButton ID="cmdImport" runat="server" Text="Import" ResourceKey="cmdImport" CssClass="dnnPrimaryAction" CausesValidation="True" /></li>
        <li>
            <asp:LinkButton ID="cmdCancel" Text="Cancel" resourcekey="cmdCancel" CausesValidation="False" runat="server" CssClass="dnnSecondaryAction" /></li>
    </ul>
</div>
