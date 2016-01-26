<%@ Control Language="C#" Inherits="DNNStuff.SQLViewPro.EditConnections" CodeBehind="EditConnections.ascx.cs" AutoEventWireup="true" Explicit="True" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnForm dnnClear">
    <div class="dnnFormItem">
        <dnn:label id="lblConnections" runat="server" controlname="dgConnection" suffix=":" />
        <asp:LinkButton ID="cmdAddConnection" runat="server" CssClass="CommandButton" resourcekey="cmdAddConnection" OnClick="cmdAddConnection_Click">Add Connection</asp:LinkButton>
    </div>
    <div class="dnnFormItem">
        <asp:DataGrid ID="dgConnection" CssClass="Grid" runat="server" AutoGenerateColumns="False" Width="100%" DataKeyField="ConnectionId" OnItemCommand="dgConnection_ItemCommand" OnItemDataBound="dgConnection_ItemDataBound">
            <HeaderStyle CssClass="GridHeader" />
            <ItemStyle CssClass="GridItem" />
            <AlternatingItemStyle CssClass="GridAltItem" />
            <FooterStyle CssClass="GridFooter" />
            <PagerStyle CssClass="PagerStyle" />
            <Columns>
                <asp:TemplateColumn ItemStyle-Width="30px">
                    <ItemTemplate>
                        <asp:LinkButton ID="cmdDeleteConnection" runat="server" Text="Del" ResourceKey="cmdDelete" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ConnectionId") %>' Enabled='<%# DataBinder.Eval(Container.DataItem, "CanDelete") %>' />
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn ItemStyle-Width="30px">
                    <ItemTemplate>
                        <asp:LinkButton ID="cmdEditConnection" runat="server" Text="Edit" ResourceKey="cmdEdit" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ConnectionId") %>' />
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="ConnectionName" HeaderText="ConnectionName"></asp:BoundColumn>
                <asp:BoundColumn DataField="UsedInReportSetCount" HeaderText="UsedInReportSets"></asp:BoundColumn>
                <asp:BoundColumn DataField="UsedInReportCount" HeaderText="UsedInReports"></asp:BoundColumn>
            </Columns>
        </asp:DataGrid>
    </div>
</div>
<ul class="dnnActions dnnClear">
    <li>
        <asp:LinkButton class="dnnPrimaryAction" ID="cmdCloseBottom" Text="Close" runat="server" CausesValidation="False" ResourceKey="Close"  OnClick="cmdClose_Click"/>
    </li>
</ul>

