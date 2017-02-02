<%@ Control Language="C#" Inherits="DNNStuff.SQLViewPro.StandardReports.HtmlChartReportSettingsControl" CodeBehind="HtmlChartReportSettingsControl.ascx.cs" AutoEventWireup="true" Explicit="True" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" EnableViewState="false" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnnstuff" TagName="CustomPropertiesViewer" Src="Properties/CustomPropertiesViewer.ascx" %>
<div class="dnnForm" id="panels-settings">
    <div class="dnnFormExpandContent">
        <a href="">Expand All</a></div>
    <h2 id="ChartType" class="dnnFormSectionHead">
        <a href="#">
            <%=Localization.GetString("lblChartTypeHeader", LocalResourceFile)%></a></h2>
    <fieldset class="dnnClear">
        <div class="dnnFormItem">
            <dnn:Label ID="lblChartType" runat="server" CssClass="SubHead" ControlName="ddlChartType" Suffix=":" />
            <asp:DropDownList ID="ddlChartType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlChartType_SelectedIndexChanged">
                <asp:ListItem Value="Area2D" Text="Area 2D" />
                <asp:ListItem Value="Bar2D" Text="Bar 2D" />
                <asp:ListItem Value="Column2D" Text="Column 2D" />
                <asp:ListItem Value="Column3D" Text="Column 3D" />
                <asp:ListItem Value="Doughnut2D" Text="Doughnut 2D" />
                <asp:ListItem Value="Funnel" Text="Funnel" />
                <asp:ListItem Value="Line" Text="Line" />
                <asp:ListItem Value="Pie2D" Text="Pie 2D" />
                <asp:ListItem Value="Pie3D" Text="Pie 3D" />
                <asp:ListItem Value="StackedArea2D" Text="Stacked Area 2D" />
                <asp:ListItem Value="StackedBar2D" Text="Stacked Bar 2D" />
                <asp:ListItem Value="StackedColumn2D" Text="Stacked Column 2D" />
                <asp:ListItem Value="StackedColumn3D" Text="Stacked Column 3D" />
                <asp:ListItem Value="MSColumn2D" Text="Multi Series Column 2D" />
                <asp:ListItem Value="MSColumn3D" Text="Multi Series Column 3D" />
                <asp:ListItem Value="MSLine" Text="Multi Series Line 2D" />
                <asp:ListItem Value="MSArea2D" Text="Multi Series Area 2D" />
                <asp:ListItem Value="MSBar2D" Text="Multi Series Bar 2D" />
            </asp:DropDownList>
        </div>
    </fieldset>
    <h2 id="H2" class="dnnFormSectionHead">
        <a href="#">
            <%=Localization.GetString("lblChartSpecificHeader", LocalResourceFile)%></a></h2>
    <fieldset class="dnnClear">
        <div class="dnnFormItem">
            <dnnstuff:CustomPropertiesViewer ID="cpvMain" runat="server" />
            <div class="normal">
                The Fusion Chart report is based on the free charting component &copy; 2011 <a href="http://www.HtmlCharts.com/free/">HtmlCharts Technologies LLP.</a> used under the <a href="http://www.gnu.org/copyleft/gpl.html">MIT license</a></div>
        </div>
    </fieldset>
</div>
<script type="text/javascript">
	jQuery(function ($) {
			var setupModule = function () {
				$('#panels-settings').dnnPanels();
				$('#panels-settings .dnnFormExpandContent a').dnnExpandAll({
					targetArea: '#panels-settings'
				});
			};
			setupModule();
			Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
				// note that this will fire when _any_ UpdatePanel is triggered,
				// which may or may not cause an issue
				setupModule();
			});
	});
</script>

