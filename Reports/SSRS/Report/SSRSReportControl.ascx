<%@ Control Language="C#" Inherits="DNNStuff.SQLViewPro.SSRSReports.SSRSReportControl" CodeBehind="SSRSReportControl.ascx.cs" AutoEventWireup="true" Explicit="True" targetSchema="http://schemas.microsoft.com/intellisense/ie5" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<rsweb:ReportViewer ID="ReportViewer1" runat="server" ShowReportBody="False"  OnReportRefresh="ReportViewer_ReportRefresh"/>

