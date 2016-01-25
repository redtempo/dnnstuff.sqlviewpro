

using System.Web.UI.WebControls;

using System.Xml.Serialization;
using DNNStuff.SQLViewPro.Controls;
using System.Collections.Generic;
using System.Linq;
	
namespace DNNStuff.SQLViewPro.SSRSReports
	{
		
		public partial class SSRSReportSettingsControl : ReportSettingsControlBase
		{
			
#region  Web Form Designer Generated Code
			
			//This call is required by the Web Form Designer.
			[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent()
			{
				
			}
		
		private void Page_Init(System.Object sender, System.EventArgs e)
		{
			//CODEGEN: This method call is required by the Web Form Designer
			//Do not modify it using the code editor.
			InitializeComponent();
			
		}
		
#endregion
		
#region  Page
		
		
#endregion
		
#region  Base Method Implementations
		protected override string LocalResourceFile => ResolveUrl("App_LocalResources/SSRSReportSettingsControl");

		    public override string UpdateSettings()
		{
			
			var obj = new SSRSReportSettings();
			obj.ProcessingMode = ddlProcessingMode.SelectedValue;
			
			// remote
			obj.ReportServerUrl = txtReportServerUrl.Text;
			obj.ReportServerReportPath = txtReportServerReportPath.Text;
			obj.ReportServerUsername = txtReportServerUsername.Text;
			obj.ReportServerPassword = txtReportServerPassword.Text;
			obj.ReportServerDomain = txtReportServerDomain.Text;
			
			// local
			obj.LocalReportPath = "FileID=" + Request.Form[urlLocalReportPath.UniqueID + "$cboFiles"];
			
			// parameters
			obj.AdditionalParameters = txtAdditionalParameters.Text;
			
			// viewer
			obj.ViewerHeight = txtViewerHeight.Text;
			obj.ViewerWidth = txtViewerWidth.Text;
			
			// toolbar
			var options = new Dictionary<string, string>();
			foreach (ListItem li in lstReportOptions.Items)
			{
				if (li.Selected)
				{
					options.Add(li.Value, "1");
				}
			}
			obj.ToolbarOptions = string.Join(",", options.Select(p => p.Key + '=' + p.Value).ToArray());
			
			return Serialization.SerializeObject(obj, typeof(SSRSReportSettings));
			
		}
		
		public override void LoadSettings(string settings)
		{
			var obj = new SSRSReportSettings();
			if (!string.IsNullOrEmpty(settings))
			{
				obj = (SSRSReportSettings) (Serialization.DeserializeObject(settings, typeof(SSRSReportSettings)));
			}
			
			ControlHelpers.InitDropDownByValue(ddlProcessingMode, obj.ProcessingMode);
			
			// remote
			txtReportServerUrl.Text = obj.ReportServerUrl;
			txtReportServerReportPath.Text = obj.ReportServerReportPath;
			txtReportServerUsername.Text = obj.ReportServerUsername;
			txtReportServerPassword.Attributes.Add("value", obj.ReportServerPassword); // because it's a password field
			txtReportServerDomain.Text = obj.ReportServerDomain;
			
			// local
			urlLocalReportPath.Url = obj.LocalReportPath;
			
			// parameters
			txtAdditionalParameters.Text = obj.AdditionalParameters;
			
			// viewer
			txtViewerHeight.Text = obj.ViewerHeight;
			txtViewerWidth.Text = obj.ViewerWidth;
			
			// toolbar
			var options = StringHelpers.ToDictionary(obj.ToolbarOptions, ',');
			foreach (ListItem li in lstReportOptions.Items)
			{
				li.Selected = false;
				if (options.ContainsKey(li.Value))
				{
					if (options[li.Value] == "1")
					{
						li.Selected = true;
					}
				}
			}
			RefreshVisibility();
		}
		
		public void RefreshVisibility()
		{
			var showRemote = ddlProcessingMode.SelectedValue == Common.ProcessingModeRemote;
			pnlRemote.Visible = showRemote;
			pnlLocal.Visible = !(showRemote);
		}
		
#endregion
		
		protected void ddlProcessingMode_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			RefreshVisibility();
		}
		
	}
	
#region  Settings
	[XmlRootAttribute(ElementName = "Settings", IsNullable = false)]public class SSRSReportSettings
	{
	    public string ProcessingMode { get; set; } = "Remote";

	    public string ReportServerUrl { get; set; } = "http://myserver/reportserver";

	    public string ReportServerReportPath { get; set; } = "/myreport";

	    public string ReportServerUsername { get; set; } = "";

	    public string ReportServerPassword { get; set; } = "";

	    public string ReportServerDomain { get; set; } = "";

	    public string LocalReportPath { get; set; } = "";

	    public string AdditionalParameters { get; set; } = "";

	    public string ViewerHeight { get; set; } = "80%";

	    public string ViewerWidth { get; set; } = "100%";

	    public string ToolbarOptions { get; set; } = "ShowBackButton=1,ShowDocumentMapButton=1,ShowExportControls=1,ShowFindControls=1,ShowPageNavigationControls=1,ShowParameterPrompts=1,ShowPrintButton=1,ShowPromptAreaButton=1,ShowRefreshButton=1,ShowToolBar=1,ShowZoomControl=1,ShowWaitControlCancelLink=1";
	}
#endregion
	
}

