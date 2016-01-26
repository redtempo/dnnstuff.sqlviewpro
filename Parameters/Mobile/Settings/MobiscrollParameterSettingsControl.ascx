<%@ Control Language="C#" Inherits="DNNStuff.SQLViewPro.MobileParameters.MobiscrollParameterSettingsControl" CodeBehind="MobiscrollParameterSettingsControl.ascx.cs" AutoEventWireup="true" Explicit="True" targetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table cellSpacing="2" cellPadding="2" width="100%" summary="List options table" border="0">
	<tr vAlign="top">
		<td vAlign="top">
            <dnn:Label ID="lblDefault" Runat="server" CssClass="SubHead" controlname="txtDefault" suffix=":"></dnn:Label>
			<asp:textbox id="txtDefault" runat="server" CssClass="NormalTextBox" TextMode="SingleLine" Columns="50"></asp:textbox>
		</td>
	</tr>
	<tr vAlign="top">
		<td vAlign="top">
            <dnn:Label ID="lblPreset" Runat="server" CssClass="SubHead" controlname="ddPreset" suffix=":"></dnn:Label>
			<asp:dropdownlist id="ddPreset" runat="server" CssClass="NormalTextBox">
                <asp:ListItem Value="date">Date</asp:ListItem>
                <asp:ListItem Value="datetime">DateTime</asp:ListItem>
                <asp:ListItem Value="time">Time</asp:ListItem>
            </asp:dropdownlist>
		</td>
	</tr>
	<tr vAlign="top">
		<td vAlign="top">
            <dnn:Label ID="lblTheme" Runat="server" CssClass="SubHead" controlname="ddTheme" suffix=":"></dnn:Label>
			<asp:dropdownlist id="ddTheme" runat="server" CssClass="NormalTextBox">
                <asp:ListItem value="default">Default</asp:ListItem>
                <asp:ListItem value="android-ics">Android ICS</asp:ListItem>
                <asp:ListItem value="android-ics light">Android ICS Light</asp:ListItem>
                <asp:ListItem value="android">Android</asp:ListItem>
                <asp:ListItem value="sense-ui">Sense UI</asp:ListItem>
                <asp:ListItem value="ios">iOS</asp:ListItem>
            </asp:dropdownlist>
		</td>
	</tr>
	<tr vAlign="top">
		<td vAlign="top">
            <dnn:Label ID="lblMode" Runat="server" CssClass="SubHead" controlname="ddMode" suffix=":"></dnn:Label>
			<asp:dropdownlist id="ddMode" runat="server" CssClass="NormalTextBox">
                <asp:ListItem Value="scroller">Scroller</asp:ListItem>
                <asp:ListItem Value="clickpick">ClickPick</asp:ListItem>
                <asp:ListItem Value="mixed">Mixed</asp:ListItem>
            </asp:dropdownlist>
		</td>
	</tr>

</table>

