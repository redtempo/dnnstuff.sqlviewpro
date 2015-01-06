<%@ Control language="vb" Inherits="DNNStuff.SQLViewPro.StandardReports.GridViewReportControl" CodeBehind="GridViewReportControl.ascx.vb" AutoEventWireup="false" Explicit="True" targetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:PlaceHolder id="phHeader" runat="server" />
<asp:GridView id="gvGrid" runat="server" Width="100%" EnableViewState="true" AutoGenerateColumns="false"></asp:GridView>
<asp:LinkButton id="cmdExportExcel" cssClass="CommandButton" runat="server">Excel</asp:LinkButton><br />
<asp:PlaceHolder id="phFooter" runat="server" />
