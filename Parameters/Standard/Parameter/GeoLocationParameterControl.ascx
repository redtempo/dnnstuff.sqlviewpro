<%@ Control Language="C#" Inherits="DNNStuff.SQLViewPro.StandardParameters.GeoLocationParameterControl" CodeBehind="GeoLocationParameterControl.ascx.cs" AutoEventWireup="true" Explicit="True" targetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:HiddenField ID="geolocation" runat="server" Value="" />
<script type="text/javascript">
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(
            function(position) {
                var g1 = document.getElementById('<%=geolocation.ClientID %>');
                g1.value = position.coords.latitude + ',' + position.coords.longitude;
            },
            // next function is the error callback
            function(error) {
                var g1 = document.getElementById('<%=geolocation.ClientID %>');
                g1.value = '';
            },
            {
                enableHighAccuracy: <%=GeoLocationSettings().EnableHighAccuracy.ToString().ToLower()%>,
                maximumAge: <%=GeoLocationSettings().MaximumAge%>,
                timeout: <%=GeoLocationSettings().Timeout%>
            }

    );
}
</script>
