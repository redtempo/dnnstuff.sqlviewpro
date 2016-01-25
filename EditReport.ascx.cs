using DotNetNuke.Services.Localization;
using System;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.UI;
using DotNetNuke.Common;
using DotNetNuke.Entities.Modules;


namespace DNNStuff.SQLViewPro
{
	
	public partial class EditReport : PortalModuleBase
	{
		private const string STR_ReferringUrl = "EditReport_ReferringUrl";
		
		//standard
		protected Controls.ConnectionPickerControl cpConnection;
		
#region  Web Form Designer Generated Code
		
		//This call is required by the Web Form Designer.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent()
		{
			
		}
		
		private void Page_Init(Object sender, EventArgs e)
		{
			//CODEGEN: This method call is required by the Web Form Designer
			//Do not modify it using the code editor.
			InitializeComponent();
			
			// initialize
			ReportId = int.Parse(DNNUtilities.QueryStringDefault(Request, "ReportId", "-1"));
			ReportSetId = int.Parse(DNNUtilities.QueryStringDefault(Request, "ReportSetId", "-1"));
			
			InitReport();
			
		}
		
#endregion
		
#region  Page
		
		public int ReportSetId {get; set;}
		public int ReportId {get; set;}
		public ReportInfo Report {get; set;}
		public string ReportConfig {get; set;}
		
		private void SaveReferringPage()
		{
			// save referring page
			if (Request.UrlReferrer == null)
			{
				Session[STR_ReferringUrl] = Globals.NavigateURL();
			}
			else
			{
				Session[STR_ReferringUrl] = Request.UrlReferrer.AbsoluteUri;
			}
		}
		
		private void Page_Load(Object sender, EventArgs e)
		{
			DNNUtilities.InjectCSS(Page, ResolveUrl("Resources/Support/edit.css"));
			Page.ClientScript.RegisterClientScriptInclude(GetType(), "yeti", ResolveUrl("resources/support/yetii-min.js"));
			
			if (Page.IsPostBack)
			{
				RenderReportSettings();
			}
			else
			{
				SaveReferringPage();
				
				// drop down report type
				BindReportType();
				BindDrilldownReport();
				BindSkinFolder(cboSkin);
				BindReport();
				
				// do report type
				RenderReportSettings();
				RetrieveReportSettings();
			}
			
		}
		
#endregion
		
#region  Data
		private void InitReport()
		{
			ReportController objReportController = new ReportController();
			ReportInfo objReport = objReportController.GetReport(ReportId);
			
			// load from database
			if (objReport == null)
			{
				Report = new ReportInfo();
			}
			else
			{
				Report = objReport;
			}
		}
		private void BindReport()
		{
			
			txtName.Text = Report.ReportName;
			
			ListItem li = cboSkin.Items.FindByValue(Report.ReportTheme);
			if (li != null)
			{
				li.Selected = true;
			}
			else
			{
				cboSkin.Items[0].Selected = true;
			}
			
			cpConnection.ConnectionId = Report.ReportConnectionId;
			txtHeader.Text = Report.ReportHeaderText;
			txtFooter.Text = Report.ReportFooterText;
			txtQuery.Text = Report.ReportCommand;
			txtCommandCacheTimeout.Text = Report.ReportCommandCacheTimeout.ToString();
			
			// cache scheme
			li = ddCommandCacheScheme.Items.FindByValue(Report.ReportCommandCacheScheme);
			if (li != null)
			{
				li.Selected = true;
			}
			else
			{
				ddCommandCacheScheme.Items.FindByValue("Sliding").Selected = true;
			}
			ddCommandCacheScheme.SelectedValue = Report.ReportCommandCacheScheme;
			
			// report type - default to GRID if not selected
			li = ddReportType.Items.FindByValue(Report.ReportTypeId.ToString());
			if (li != null)
			{
				li.Selected = true;
			}
			else
			{
				ddReportType.Items.FindByValue("GRID").Selected = true;
			}
			ddReportType.SelectedValue = Report.ReportTypeId.ToString();
			
			// drilldown
			txtDrilldownFieldname.Text = Report.ReportDrilldownFieldname;
			li = ddDrilldownReportId.Items.FindByValue(Report.ReportDrilldownReportId.ToString());
			if (li != null)
			{
				li.Selected = true;
			}
			else
			{
				ddDrilldownReportId.Items.FindByValue("-1").Selected = true;
			}
			
			txtNoItems.Text = Report.ReportNoItemsText;
			txtPageTitle.Text = Report.ReportPageTitle;
			txtMetaDescription.Text = Report.ReportMetaDescription;
			
			
		}
		private void BindSkinFolder(ListControl o)
		{
			System.IO.DirectoryInfo skinFolder = new System.IO.DirectoryInfo((string) (Server.MapPath(ResolveUrl("Skins"))));
			o.Items.Clear();
			o.Items.Add(new ListItem("Report Set Default", ""));
			o.Items.Add(new ListItem("None", "None"));
			foreach (System.IO.DirectoryInfo folder in skinFolder.GetDirectories())
			{
				o.Items.Add(folder.Name);
			}
		}
		
		private void SaveReport()
		{
			RetrieveReportSettings();
			
			ReportController objReportController = new ReportController();
			ReportId = objReportController.UpdateReport(ReportSetId, ReportId, ddReportType.SelectedValue, txtName.Text, cboSkin.SelectedItem.Value, Convert.ToInt32(cpConnection.ConnectionId), txtHeader.Text, txtFooter.Text, txtQuery.Text, ReportConfig, -1, Convert.ToInt32(ddDrilldownReportId.SelectedValue), txtDrilldownFieldname.Text, txtNoItems.Text, txtPageTitle.Text, Convert.ToInt32(txtCommandCacheTimeout.Text), txtMetaDescription.Text, ddCommandCacheScheme.SelectedValue);
		}
		
		private void BindReportType()
		{
			ReportTypeController objReportTypeController = new ReportTypeController();
			ddReportType.DataTextField = "ReportTypeName";
			ddReportType.DataValueField = "ReportTypeId";
			ddReportType.DataSource = objReportTypeController.ListReportType();
			ddReportType.DataBind();
		}
		
		private void BindDrilldownReport()
		{
			ArrayList objReportList = default(ArrayList);
			ReportSetController objReportSetController = new ReportSetController();
			objReportList = objReportSetController.GetReportSetReport(ReportSetId);
			
			ddDrilldownReportId.DataValueField = "ReportId";
			ddDrilldownReportId.DataTextField = "ReportName";
			ddDrilldownReportId.DataSource = objReportList;
			ddDrilldownReportId.DataBind();
			
			// add the default to the start of the list
			ListItem li = new ListItem((string) (Localization.GetString("NoDrilldown.Text", LocalResourceFile)), "-1");
			ddDrilldownReportId.Items.Insert(0, li);
			
			// remove this report
			if (ReportId > -1)
			{
				li = ddDrilldownReportId.Items.FindByValue(ReportId.ToString());
				if (li != null)
				{
					ddDrilldownReportId.Items.Remove(li);
				}
			}
			
		}
		
#endregion
		
#region  Navigation
		protected void cmdUpdate_Click(object sender, EventArgs e)
		{
			
			if (Page.IsValid)
			{
				SaveReport();
				
				NavigateBack();
			}
			
		}
		
		protected void cmdCancel_Click(object sender, EventArgs e)
		{
			NavigateBack();
		}
		
		private void NavigateBack()
		{
			if (Session[STR_ReferringUrl] != null)
			{
				Response.Redirect(Session[STR_ReferringUrl].ToString());
			}
		}
		
#endregion
		
		private void RetrieveReportSettings()
		{
			Controls.ReportSettingsControlBase objReportSettings = (Controls.ReportSettingsControlBase) (phReportSettings.FindControl("ReportSettings"));
			
			ReportConfig = (string) objReportSettings.UpdateSettings();
			
		}
		private void RenderReportSettings()
		{
			string reportTypeId = ddReportType.SelectedValue;
			
			ReportTypeController objReportTypeController = new ReportTypeController();
			
			ReportTypeInfo objReportType = objReportTypeController.GetReportType(reportTypeId);
			
			Controls.ReportSettingsControlBase objReportSettingsBase = default(Controls.ReportSettingsControlBase);
			objReportSettingsBase = (Controls.ReportSettingsControlBase) (LoadControl(ResolveUrl(objReportType.ReportTypeSettingsControlSrc)));
			
			objReportSettingsBase.ID = "ReportSettings";
			if (Report != null)
			{
				objReportSettingsBase.LoadSettings(Report.ReportConfig);
			}
			else
			{
				objReportSettingsBase.LoadSettings("");
			}
			phReportSettings.Controls.Add(new LiteralControl(string.Format("<h3>{0} Settings</h3>", objReportType.ReportTypeName)));
			phReportSettings.Controls.Add(objReportSettingsBase);
			
		}
		protected void ddReportType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (Report != null)
			{
				RetrieveReportSettings();
			}
		}
		
#region  Validation
		protected void vldQuery_ServerValidate(Object source, ServerValidateEventArgs args)
		{
			string msg = "";
			args.IsValid = Convert.ToBoolean(Services.Data.Query.IsQueryValid(txtQuery.Text, ConnectionController.GetConnectionString(Convert.ToInt32(cpConnection.ConnectionId), ReportSetId), ref msg));
			vldQuery.ErrorMessage = msg;
		}
		
		protected void cmdQueryTest_Click(object sender, EventArgs e)
		{
			string msg = "";
			bool isValid = Convert.ToBoolean(Services.Data.Query.IsQueryValid(txtQuery.Text, ConnectionController.GetConnectionString(Convert.ToInt32(cpConnection.ConnectionId), ReportSetId), ref msg));
			
			lblQueryTestResults.Text = msg;
			lblQueryTestResults.CssClass = "NormalGreen";
			if (!isValid)
			{
				lblQueryTestResults.CssClass = "NormalRed";
			}
			
		}
		
#endregion
	}
	
}

