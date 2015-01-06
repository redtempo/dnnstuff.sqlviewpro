<%@ Control language="vb" Inherits="DNNStuff.SQLViewPro.StandardParameters.CalendarParameterControl" CodeBehind="CalendarParameterControl.ascx.vb" AutoEventWireup="false" Explicit="True" targetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:textbox id="txtCalendar" runat="server" CssClass="NormalTextBox" TextMode="SingleLine"></asp:textbox>
<asp:CompareValidator ID="valCalendar" CssClass="NormalRed" runat="server" ControlToValidate="txtCalendar" ErrorMessage="<br>Invalid date!" Operator="DataTypeCheck" Type="Date" Display="Dynamic" />
&nbsp;<asp:hyperlink id="cmdStartCalendar" cssclass="CommandButton" runat="server" resourcekey="Calendar" ImageUrl="~/images/calendar.png">Calendar</asp:hyperlink>
