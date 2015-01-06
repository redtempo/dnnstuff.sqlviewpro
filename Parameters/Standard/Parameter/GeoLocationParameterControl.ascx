<%@ Control language="vb" Inherits="DNNStuff.SQLViewPro.StandardParameters.GeoLocationParameterControl" CodeBehind="GeoLocationParameterControl.ascx.vb" AutoEventWireup="false" Explicit="True" targetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:HiddenField ID="geolocation" runat="server" Value="" />
<script type="text/javascript">
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(

		function (position) {
		    var g1 = document.getElementById('<%=geolocation.ClientId() %>');
		    g1.value = position.coords.latitude + ',' + position.coords.longitude;
		},
        // next function is the error callback
		function (error) {
		    var g1 = document.getElementById('<%=geolocation.ClientId() %>');
		    g1.value = '';
		},
		{
            enableHighAccuracy: <%=GeoLocationSettings.EnableHighAccuracy.ToString().ToLower() %>,
		    maximumAge: <%=GeoLocationSettings.MaximumAge %>,
		    timeout: <%=GeoLocationSettings.Timeout %>
		}

	);
}
</script>