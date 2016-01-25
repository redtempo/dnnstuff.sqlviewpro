

using System.Web.UI.WebControls;

using System.Xml.Serialization;
using DNNStuff.SQLViewPro.Controls;
using System.Collections.Generic;
using System.Linq;
	
	//***************************************************************************/
	//* XmlReportSettings.ascx.vb
	//*
	//* Copyright (c) 2004 by DNNStuff.
	//* All rights reserved.
	//*
	//* Date:        August 9, 2004
	//* Author:      Richard Edwards
	//* Description: Template Report Settings Handler
	//*************/
	
	
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
		protected override string LocalResourceFile
		{
			get
			{
				return ResolveUrl("App_LocalResources/SSRSReportSettingsControl");
			}
		}
		
		public override string UpdateSettings()
		{
			
			SSRSReportSettings obj = new SSRSReportSettings();
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
			Dictionary<string, string> options = new Dictionary<string, string>();
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
			SSRSReportSettings obj = new SSRSReportSettings();
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
			Dictionary<string, string> options = StringHelpers.ToDictionary(obj.ToolbarOptions, ',');
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
			bool showRemote = ddlProcessingMode.SelectedValue == Common.ProcessingModeRemote;
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
		private string _ProcessingMode = "Remote";
		public string ProcessingMode
		{
			get
			{
				return _ProcessingMode;
			}
			set
			{
				_ProcessingMode = value;
			}
		}
		private string _ReportServerUrl = "http://myserver/reportserver";
		public string ReportServerUrl
		{
			get
			{
				return _ReportServerUrl;
			}
			set
			{
				_ReportServerUrl = value;
			}
		}
		private string _ReportServerReportPath = "/myreport";
		public string ReportServerReportPath
		{
			get
			{
				return _ReportServerReportPath;
			}
			set
			{
				_ReportServerReportPath = value;
			}
		}
		private string _ReportServerUsername = "";
		public string ReportServerUsername
		{
			get
			{
				return _ReportServerUsername;
			}
			set
			{
				_ReportServerUsername = value;
			}
		}
		private string _ReportServerPassword = "";
		public string ReportServerPassword
		{
			get
			{
				return _ReportServerPassword;
			}
			set
			{
				_ReportServerPassword = value;
			}
		}
		private string _ReportServerDomain = "";
		public string ReportServerDomain
		{
			get
			{
				return _ReportServerDomain;
			}
			set
			{
				_ReportServerDomain = value;
			}
		}
		
		private string _LocalReportPath = "";
		public string LocalReportPath
		{
			get
			{
				return _LocalReportPath;
			}
			set
			{
				_LocalReportPath = value;
			}
		}
		
		private string _AdditionalParameters = "";
		public string AdditionalParameters
		{
			get
			{
				return _AdditionalParameters;
			}
			set
			{
				_AdditionalParameters = value;
			}
		}
		
		private string _ViewerHeight = "80%";
		public string ViewerHeight
		{
			get
			{
				return _ViewerHeight;
			}
			set
			{
				_ViewerHeight = value;
			}
		}
		private string _ViewerWidth = "100%";
		public string ViewerWidth
		{
			get
			{
				return _ViewerWidth;
			}
			set
			{
				_ViewerWidth = value;
			}
		}
		
		private string _ToolbarOptions = "ShowBackButton=1,ShowDocumentMapButton=1,ShowExportControls=1,ShowFindControls=1,ShowPageNavigationControls=1,ShowParameterPrompts=1,ShowPrintButton=1,ShowPromptAreaButton=1,ShowRefreshButton=1,ShowToolBar=1,ShowZoomControl=1,ShowWaitControlCancelLink=1";
		public string ToolbarOptions
		{
			get
			{
				return _ToolbarOptions;
			}
			set
			{
				_ToolbarOptions = value;
			}
		}
	}
#endregion
	
}

