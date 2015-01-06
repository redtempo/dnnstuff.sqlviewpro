<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ExcelTemplateReportSettingsControl.ascx.vb" Inherits="DNNStuff.SQLViewPro.ExcelReports.ExcelTemplateReportSettingsControl" EnableViewState="true" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="URL" Src="~/controls/URLControl.ascx" %>

<div class="dnnForm dnnClear" id="panels-settings">
	<div class="dnnClear">
	    <fieldset>
		    <div class="dnnFormItem">
            	<dnn:Label ID="lblOutputFileName" runat="server" ControlName="txtOutputFileName"  Suffix=":" />
                <asp:TextBox ID="txtOutputFileName" Runat="server" columns="100 " />
            </div>
            <div class="dnnFormItem">
	            <dnn:Label ID="lblContainsHeaderRow" runat="server" ControlName="chkContainsHeaderRow"  Suffix=":" />
                <asp:Checkbox ID="chkContainsHeaderRow" Runat="server"  />
            </div>
            <div class="dnnFormItem">
	            <dnn:Label ID="lblDataSheetName" runat="server" ControlName="txtDataSheetName"  Suffix=":" />
                <asp:TextBox ID="txtDataSheetName" Runat="server" columns="100 " />
            </div>
            <div class="dnnFormItem">
	            <dnn:Label ID="lblXlsFileName" runat="server" ControlName="ctlXlsFileName"  Suffix=":" />
                <div class="dnnLeft"><dnn:url id="ctlXlsFileName" runat="server" width="275" Required="False" showtrack="False" shownewwindow="False" showlog="False" urltype="F" showUrls="False" showfiles="True" showtabs="False"></dnn:url></div>
            </div>
            <div class="dnnFormItem">
	            <dnn:Label ID="lblXlsxFileName" runat="server" ControlName="ctrlXlsxFileName"  Suffix=":" />
                <div class="dnnLeft"><dnn:url id="ctlXlsxFileName" runat="server" width="275" Required="False" showtrack="False" shownewwindow="False" showlog="False" urltype="F" showUrls="False" showfiles="True" showtabs="False"></dnn:url></div>
            </div>
            <div class="dnnFormItem">
	            <dnn:Label ID="lblDispositionType" runat="server" ControlName="ddDispositionType"  Suffix=":" />
                <asp:DropDownList ID="ddDispositionType" runat="server">
                    <asp:ListItem Value="inline">inline</asp:ListItem>
                    <asp:ListItem Value="attachment">attachment</asp:ListItem>
                </asp:DropDownList>
            </div>
        </fieldset>
    </div>
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
