<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="DNNStuff.SQLViewPro.Report" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="head" runat="server">
    <title></title>
    <asp:PlaceHolder ID="SCRIPTS" runat="server" />
</head>
<body>
   
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="300" />
        <asp:PlaceHolder runat="server" ID="phInject" />
    </form>
</body>
</html>

