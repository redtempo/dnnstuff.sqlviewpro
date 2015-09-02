<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="TemplateReportSettingsControl.ascx.vb" Inherits="DNNStuff.SQLViewPro.StandardReports.TemplateReportSettingsControl" EnableViewState="false" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnForm" id="panels-settings">
	<div class="dnnFormExpandContent">
		<a href="">Expand All</a></div>
	<h2 id="Template" class="dnnFormSectionHead">
		<a href="#">
			<%=Localization.GetString("lblTemplateHeader", LocalResourceFile)%></a></h2>
	<fieldset class="dnnClear">
		<div class="dnnFormItem">
            <dnn:Label ID="lblTemplateText" runat="server" ControlName="txtTemplateText"  Suffix=":" />
            <asp:TextBox ID="txtTemplateText" Runat="server" columns="80" rows="50" TextMode="MultiLine" />
		</div>
	</fieldset>
	<h2 id="Paging" class="dnnFormSectionHead">
		<a href="#">
			<%=Localization.GetString("lblPagingHeader",LocalResourceFile)%></a></h2>
	<fieldset class="dnnClear">
		<div class="dnnFormItem">
			<dnn:Label ID="lblAllowPaging" runat="server" ControlName="chkAllowPaging" Suffix=":" />
			<asp:CheckBox ID="chkAllowPaging" runat="server" CssClass="NormalTextBox" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="lblPagingType" runat="server" ControlName="ddPagingType" Suffix=":" />
			<asp:DropDownList ID="ddPagingType" runat="server" CssClass="NormalTextBox">
				<asp:ListItem Value="Internal">Internal</asp:ListItem>
				<asp:ListItem Value="Querystring">Querystring</asp:ListItem>
			</asp:DropDownList>
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="lblPageSize" runat="server" ControlName="txtPageSize" Suffix=":" />
			<asp:TextBox ID="txtPageSize" runat="server" CssClass="NormalTextBox" Rows="1" Columns="4" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="lblPrevNext" runat="server" ControlName="txtPrevPageText" Suffix=":" />
			<asp:TextBox ID="txtPrevPageText" runat="server" CssClass="NormalTextBox" Rows="1" Columns="10" />
			<asp:TextBox ID="txtNextPageText" runat="server" CssClass="NormalTextBox" Rows="1" Columns="10" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="lblFirstLast" runat="server" ControlName="txtFirstLastPageText" Suffix=":" />
			<asp:TextBox ID="txtFirstPageText" runat="server" CssClass="NormalTextBox" Rows="1" Columns="10" />
			<asp:TextBox ID="txtLastPageText" runat="server" CssClass="NormalTextBox" Rows="1" Columns="10" />
		</div>
		<div class="dnnFormItem">
            <dnn:Label ID="lblPageTemplate" runat="server" ControlName="txtPageTemplate"  Suffix=":" />
            <asp:TextBox ID="txtPageTemplate" Runat="server" CssClass="NormalTextBox" Rows="1" Columns="100" />
		</div>
        <div class="dnnFormMessage dnnFormInfo">Pager tokens include:<br/>
            <strong>[PAGER]</strong> - inserts the pager (if it isn't found it will be added to the end)<br/>
            <strong>[PAGER:FIRST]</strong> - inserts the first page link<br/>
            <strong>[PAGER:LAST]</strong> - inserts the last page link<br/>
            <strong>[PAGER:PREVIOUS]</strong> - inserts the previous page link<br/>
            <strong>[PAGER:NEXT]</strong> - inserts the next page link<br/>
            <strong>[PAGER:PAGES]</strong> - inserts the pages link list (up to 10)<br/>
            <strong>[PAGER:NUMBER]</strong> - the current page number<br/>
            <strong>[PAGER:COUNT]</strong> - the number of pages available
        </div>
        <div class="dnnFormMessage dnnFormInfo">Page Link tokens include:<br/>
            <strong>[URL]</strong> - inserts the url for the page<br/>
            <strong>[TEXT]</strong> - inserts the text for the page link<br/>
            <strong>[TYPE]</strong> - inserts the link type (ie. first, last, previous, next or page (for numeric pages)) - usefull in a classname to destinquish different link types
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
