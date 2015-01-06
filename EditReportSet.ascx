<%@ Control Language="vb" Inherits="DNNStuff.SQLViewPro.EditReportSet" CodeBehind="EditReportSet.ascx.vb" AutoEventWireup="false" Explicit="True" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="dnnstuff" TagName="ConnectionPicker" Src="controls/ConnectionPicker/ConnectionPickerControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<div class="dnnForm dnnClear">
    <div id="editsettings" class="tabslayout">
        <ul id="editsettings-nav" class="tabslayout">
            <li><a href="#tab1"><span>
                <%=Localization.GetString("TabCaption_Tab1", LocalResourceFile)%></span></a></li>
            <li><a href="#tab2"><span>
                <%=Localization.GetString("TabCaption_Tab2", LocalResourceFile)%></span></a></li>
            <li><a href="#help"><span>
                <%=Localization.GetString("TabCaption_Help", LocalResourceFile)%></span></a></li>
        </ul>
        <div class="tabs-container">
            <div class="tab" id="tab1">
                <div class="dnnFormItem">
                    <dnn:Label ID="lblName" runat="server" ControlName="txtName" Suffix=":" />
                    <asp:TextBox ID="txtName" runat="server" CssClass="NormalTextBox" TextMode="SingleLine" Columns="100"></asp:TextBox>
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblSkin" runat="server" ControlName="cboSkin" Suffix=":" />
                    <asp:DropDownList ID="cboSkin" runat="server" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblConnection" runat="server" ControlName="cpConnection" Suffix=":" />
                    <dnnstuff:ConnectionPicker ID="cpConnection" IncludePortalDefault="True" runat="server" CssClass="NormalTextBox"></dnnstuff:ConnectionPicker>
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblHeader" runat="server" ControlName="txtHeader" Suffix=":" />
                    <asp:TextBox ID="txtHeader" runat="server" CssClass="NormalTextBox" TextMode="MultiLine" Columns="70" Rows="2" Width="100%"></asp:TextBox>
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblFooter" runat="server" ControlName="txtFooter" Suffix=":" />
                    <asp:TextBox ID="txtFooter" runat="server" CssClass="NormalTextBox" TextMode="MultiLine" Columns="70" Rows="2" Width="100%"></asp:TextBox>
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblReports" runat="server" ControlName="dgReport" Suffix=":" />
                </div>
                <div class="dnnFormItem">
                    <asp:LinkButton ID="cmdAddReport" runat="server" CssClass="CommandButton" ResourceKey="cmdAddReport">Add Report</asp:LinkButton>
                </div>
                <div class="dnnFormItem">
                    <asp:DataGrid ID="dgReport" runat="server" DataKeyField="ReportId" AutoGenerateColumns="False" CssClass="Grid" Width="100%">
                        <HeaderStyle CssClass="GridHeader" />
                        <ItemStyle CssClass="GridItem" />
                        <AlternatingItemStyle CssClass="GridAltItem" />
                        <FooterStyle CssClass="GridFooter" />
                        <PagerStyle CssClass="PagerStyle" />
                        <Columns>
                            <asp:TemplateColumn ItemStyle-Width="30px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="cmdDeleteReport" runat="server" Text="Del" ResourceKey="cmdDelete" CommandName="Delete" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn ItemStyle-Width="30px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="cmdEditReport" runat="server" Text="Edit" ResourceKey="cmdEdit" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ReportTypeId") %>' />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="ReportName" HeaderText="ReportName"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ReportTypeName" HeaderText="ReportType"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ReportTheme" HeaderText="ReportTheme"></asp:BoundColumn>
                            <asp:TemplateColumn>
                                <ItemTemplate>
                                    <asp:ImageButton ID="cmdCopyReport" runat="server" CausesValidation="false" CommandName="Copy" ImageUrl="~/images/copy.gif" AlternateText="Copy Report"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <ItemTemplate>
                                    <asp:ImageButton ID="cmdMoveReportUp" runat="server" CausesValidation="false" CommandName="Up" ImageUrl="~/images/up.gif" AlternateText="Move Report Up"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <ItemTemplate>
                                    <asp:ImageButton ID="cmdMoveReportDown" runat="server" CausesValidation="false" CommandName="Down" ImageUrl="~/images/dn.gif" AlternateText="Move Report Down"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                    <div class="dnnFormItem">
                        <dnn:Label ID="lblParameters" runat="server" CssClass="SubHead" ControlName="" Suffix=":" />
                    </div>
					<div class="dnnFormItem">
                        <asp:LinkButton ID="cmdAddParameter" runat="server" CssClass="CommandButton" ResourceKey="cmdAddParameter">Add Parameter</asp:LinkButton>
					</div>
                    <div class="dnnFormItem">
                        <asp:DataGrid ID="dgParameter" runat="server" DataKeyField="ParameterId" AutoGenerateColumns="False" CssClass="Grid" Width="100%">
                            <HeaderStyle CssClass="GridHeader" />
                            <ItemStyle CssClass="GridItem" />
                            <AlternatingItemStyle CssClass="GridAltItem" />
                            <FooterStyle CssClass="GridFooter" />
                            <PagerStyle CssClass="PagerStyle" />
                            <Columns>
                                <asp:TemplateColumn ItemStyle-Width="30px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="cmdDeleteParameter" runat="server" Text="Del" ResourceKey="cmdDelete" CommandName="Delete" />
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn ItemStyle-Width="30px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbEditParameter" runat="server" Text="Edit" ResourceKey="cmdEdit" CommandName="Edit" />
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="ParameterName" HeaderText="ParameterName"></asp:BoundColumn>
                                <asp:BoundColumn DataField="ParameterCaption" HeaderText="ParameterCaption"></asp:BoundColumn>
                                <asp:BoundColumn DataField="ParameterTypeName" HeaderText="ParameterType"></asp:BoundColumn>
                                <asp:TemplateColumn>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="cmdCopyParameter" runat="server" CausesValidation="false" CommandName="Copy" ImageUrl="~/images/copy.gif" AlternateText="Copy Parameter"></asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="cmdMoveParameterUp" runat="server" CausesValidation="false" CommandName="Up" ImageUrl="~/images/up.gif" AlternateText="Move Parameter Up"></asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="cmdMoveParameterDown" runat="server" CausesValidation="false" CommandName="Down" ImageUrl="~/images/dn.gif" AlternateText="Move Parameter Down"></asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                    </div>
                </div>
            </div>
            <div class="tab" id="tab2">
                <div class="dnnFormItem">
                    <dnn:Label ID="lblDebug" runat="server" ControlName="chkDebug" Suffix=":" />
                    <asp:CheckBox ID="chkDebug" runat="server" TextAlign="Left" CssClass="NormalTextBox" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblRunCaption" runat="server" ControlName="txtRunCaption" Suffix=":" Text="Run" />
                    <asp:TextBox ID="txtRunCaption" runat="server" Columns="40" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblBackCaption" runat="server" ControlName="txtBackCaption" Suffix=":" Text="<-- Back" />
                    <asp:TextBox ID="txtBackCaption" runat="server" Columns="40" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblAutoRun" runat="server" ControlName="chkAutoRun" Suffix=":" />
                    <asp:CheckBox ID="chkAutoRun" runat="server" TextAlign="Left" CssClass="NormalTextBox" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblAlwaysShowParameters" runat="server" ControlName="chkAlwaysShowParameters" Suffix=":" />
                    <asp:CheckBox ID="chkAlwaysShowParameters" runat="server" TextAlign="Left" CssClass="NormalTextBox" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblRenderMode" runat="server" ControlName="ddlRenderMode" Suffix=":" />
                    <asp:DropDownList runat="server" ID="ddlRenderMode">
                        <asp:ListItem Value="Default">Default</asp:ListItem>
                        <asp:ListItem Value="Popup">Popup</asp:ListItem>
                        <asp:ListItem Value="NewWindow">New Window</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblParameterLayout" runat="server" ControlName="txtParameterLayout" Suffix=":" />
                    <asp:TextBox ID="txtParameterLayout" runat="server" CssClass="NormalTextBox" TextMode="MultiLine" Columns="70" Rows="10" Width="100%"></asp:TextBox>
                </div>
            </div>
            <div class="tab" id="help">
                <div class="dnnFormItem">
                    <%=Localization.GetString("DocumentationHelp.Text", LocalResourceFile)%></div>
            </div>
        </div>
    </div>
    <ul class="dnnActions dnnClear">
        <li>
            <asp:LinkButton ID="cmdUpdate" Text="Update" resourcekey="cmdUpdate" CausesValidation="True" runat="server" CssClass="dnnPrimaryAction" /></li>
        <li>
            <asp:LinkButton ID="cmdCancel" Text="Cancel" resourcekey="cmdCancel" CausesValidation="False" runat="server" CssClass="dnnSecondaryAction" /></li>
    </ul>
</div>
<script type="text/javascript">
    var tabber1 = new Yetii({
        id: 'editsettings',
        persist: true
    });
</script>
