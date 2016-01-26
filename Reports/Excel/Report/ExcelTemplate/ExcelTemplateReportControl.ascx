<%@ Control Language="C#" Inherits="DNNStuff.SQLViewPro.ExcelReports.ExcelTemplateReportControl" CodeBehind="ExcelTemplateReportControl.ascx.cs" AutoEventWireup="true" Explicit="True" targetSchema="http://schemas.microsoft.com/intellisense/ie5" %>

<asp:label ID="lblChoose" runat="server" Visible="false">Please choose your file preference:</asp:label>
<asp:radiobuttonlist ID="rdoExcelType" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" Visible="false" OnSelectedIndexChanged="rdoExcelType_SelectedIndexChanged">
<asp:ListItem Value="xls" Text="xls" Selected="False" />
<asp:ListItem Value="xlsx" Text="xlsx" Selected="False" />
</asp:radiobuttonlist>

