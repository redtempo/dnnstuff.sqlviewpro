<%@ Control Language="C#" Inherits="DNNStuff.SQLViewPro.StandardReports.GridReportSettingsControl" CodeBehind="GridReportSettingsControl.ascx.cs" AutoEventWireup="true" Explicit="True" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" EnableViewState="false" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnForm" id="panels-settings">
	<div class="dnnFormExpandContent">
		<a href="">Expand All</a></div>
	<h2 id="Sorting" class="dnnFormSectionHead">
		<a href="#">
			<%=Localization.GetString("lblSortingHeader", LocalResourceFile)%></a></h2>
	<fieldset class="dnnClear">
		<div class="dnnFormItem">
			<dnn:Label ID="lblAllowSorting" runat="server" ControlName="chkAllowSorting" Suffix=":" />
			<asp:CheckBox ID="chkAllowSorting" runat="server" CssClass="NormalTextBox" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="lblOrderBy" runat="server" ControlName="txtOrderBy" Suffix=":" />
			<asp:TextBox ID="txtOrderBy" runat="server" CssClass="NormalTextBox" TextMode="SingleLine"  Width="65%" />
		</div>
	</fieldset>
	<h2 id="Paging" class="dnnFormSectionHead">
		<a href="#">
			<%=Localization.GetString("lblPagingHeader", LocalResourceFile)%></a></h2>
	<fieldset class="dnnClear">
		<div class="dnnFormItem">
			<dnn:Label ID="lblAllowPaging" runat="server" ControlName="chkAllowPaging" Suffix=":" />
			<asp:CheckBox ID="chkAllowPaging" runat="server" CssClass="NormalTextBox" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="lblPageSize" runat="server" ControlName="txtPageSize" Suffix=":" />
			<asp:TextBox ID="txtPageSize" runat="server" CssClass="NormalTextBox" Rows="1" Columns="4" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="lblPagerMode" runat="server" ControlName="ddPagerMode" Suffix=":" />
			<asp:DropDownList ID="ddPagerMode" runat="server" CssClass="NormalTextBox">
				<asp:ListItem Value="NumericPages">Numeric</asp:ListItem>
				<asp:ListItem Value="NextPrev">Next+Prev</asp:ListItem>
			</asp:DropDownList>
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="lblPrevNext" runat="server" ControlName="txtPrevPageText" Suffix=":" />
			<asp:TextBox ID="txtPrevPageText" runat="server" CssClass="NormalTextBox" Rows="1" Columns="10" />
			<asp:TextBox ID="txtNextPageText" runat="server" CssClass="NormalTextBox" Rows="1" Columns="10" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="lblPagerPosition" runat="server" ControlName="ddPagerPosition" Suffix=":" />
			<asp:DropDownList ID="ddPagerPosition" runat="server" CssClass="NormalTextBox">
				<asp:ListItem Value="Top">Top</asp:ListItem>
				<asp:ListItem Value="Bottom">Bottom</asp:ListItem>
				<asp:ListItem Value="TopAndBottom">Top+Bottom</asp:ListItem>
			</asp:DropDownList>
		</div>
	</fieldset>
	<h2 id="Misc" class="dnnFormSectionHead">
		<a href="#">
			<%=Localization.GetString("lblMiscHeader", LocalResourceFile)%></a></h2>
	<fieldset class="dnnClear">
		<div class="dnnFormItem">
			<dnn:Label ID="lblEnableExcelExport" runat="server" ControlName="chkEnableExcelExport" Suffix=":" />
			<asp:CheckBox ID="chkEnableExcelExport" runat="server" CssClass="NormalTextBox" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="lblExcelExportCaption" runat="server" ControlName="txtExcelExportButtonCaption" Suffix=":" />
			<asp:TextBox ID="txtExcelExportButtonCaption" runat="server" CssClass="NormalTextBox" Rows="1" Columns="40" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="lblExcelExportPosition" runat="server" ControlName="ddExcelExportPosition" Suffix=":" />
			<asp:DropDownList ID="ddExcelExportPosition" runat="server" CssClass="NormalTextBox">
				<asp:ListItem Value="Top">Top</asp:ListItem>
				<asp:ListItem Value="Bottom">Bottom</asp:ListItem>
			</asp:DropDownList>
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="lblHideColumnHeaders" runat="server" ControlName="chkHideColumnHeaders" Suffix=":" />
			<asp:CheckBox ID="chkHideColumnHeaders" runat="server" CssClass="NormalTextBox" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="lblHideColumns" runat="server" ControlName="txtHideColumns" Suffix=":" />
			<asp:TextBox ID="txtHideColumns" runat="server" CssClass="NormalTextBox" Width="65%" />
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

