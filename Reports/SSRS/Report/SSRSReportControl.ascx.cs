using System;
using System.Web.UI.WebControls;

using System.Net;
using System.Collections.Generic;
using Microsoft.Reporting.WebForms;
using System.Security.Principal;

namespace DNNStuff.SQLViewPro.SSRSReports
{
	
	public partial class SSRSReportControl : Controls.ReportControlBase
	{
		
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
			
		}
		
		
#endregion
		
#region  Page

	    private SSRSReportSettings ReportExtra { get; set; } = new SSRSReportSettings();

	    private void Page_Load(object sender, EventArgs e)
		{
			try
			{
				BindTemplateToData();
			}
			catch (Exception ex)
			{
				DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
			}
		}
#endregion
		
#region  Base Method Implementations
		public override void LoadRuntimeSettings(ReportInfo settings)
		{
			ReportExtra = (SSRSReportSettings) (Serialization.DeserializeObject(settings.ReportConfig, typeof(SSRSReportSettings)));
		}
#endregion
		
#region  Template
		
		private void BindTemplateToData()
		{
			
			// debug
			if (State.ReportSet.ReportSetDebug && ReportExtra.ProcessingMode == Common.ProcessingModeLocal)
			{
				DebugInfo.Append(QueryText);
			}
			
			if (!ReportViewer1.ShowReportBody)
			{
				// If you do not use this condition, the page will auto refresh and all parameters are reset to default or null values.
				ReportViewer1.ShowReportBody = true;
				RefreshReport();
			}
		}
		
		private void RefreshReport()
		{
			if (ReportExtra.ViewerWidth != "")
			{
				ReportViewer1.Width = Unit.Parse(ReportExtra.ViewerWidth);
			}
			else
			{
				ReportViewer1.Width = Unit.Percentage(100);
			}
			if (ReportExtra.ViewerHeight != "")
			{
				ReportViewer1.Height = Unit.Parse(ReportExtra.ViewerHeight);
			}
			else
			{
				ReportViewer1.Height = Unit.Percentage(100);
			}
			
			var options = StringHelpers.ToDictionary(ReportExtra.ToolbarOptions, ',');
			ReportViewer1.ShowBackButton = options.ContainsKey("ShowBackButton");
			ReportViewer1.ShowDocumentMapButton = options.ContainsKey("ShowDocumentMapButton");
			ReportViewer1.ShowExportControls = options.ContainsKey("ShowExportControls");
			ReportViewer1.ShowFindControls = options.ContainsKey("ShowFindControls");
			ReportViewer1.ShowPageNavigationControls = options.ContainsKey("ShowPageNavigationControls");
			ReportViewer1.ShowParameterPrompts = options.ContainsKey("ShowParameterPrompts");
			ReportViewer1.ShowPrintButton = options.ContainsKey("ShowPrintButton");
			ReportViewer1.ShowPromptAreaButton = options.ContainsKey("ShowPromptAreaButton");
			ReportViewer1.ShowRefreshButton = options.ContainsKey("ShowRefreshButton");
			ReportViewer1.ShowToolBar = options.ContainsKey("ShowToolBar");
			ReportViewer1.ShowZoomControl = options.ContainsKey("ShowZoomControl");
			ReportViewer1.ShowWaitControlCancelLink = options.ContainsKey("ShowWaitControlCancelLink");
			
			if (ReportExtra.ProcessingMode == Common.ProcessingModeRemote)
			{
				RenderRemoteReport();
			}
			else
			{
				RenderLocalReport();
			}
			
		}
		
		private void RenderLocalReport()
		{
			
			var ds = ReportData();
			
			ReportViewer1.ProcessingMode = ProcessingMode.Local;
			ReportViewer1.LocalReport.ReportPath = (string) (Services.File.GetFilenameFromFileId(ReportExtra.LocalReportPath));
			ReportViewer1.LocalReport.DisplayName = Report.ReportName;
			
			var dsNames = ReportViewer1.LocalReport.GetDataSourceNames();
			ReportViewer1.LocalReport.DataSources.Clear();
			for (var dsIndex = 0; dsIndex <= dsNames.Count - 1; dsIndex++)
			{
				ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource(dsNames[dsIndex], ds.Tables[dsIndex]));
			}
			
			ReportViewer1.LocalReport.SetParameters(SetReportParameters(ReportViewer1.LocalReport.GetParameters()));
			ReportViewer1.LocalReport.Refresh();
		}
		
		private void RenderRemoteReport()
		{
			
			ReportViewer1.ProcessingMode = ProcessingMode.Remote;
			ReportViewer1.ServerReport.Timeout = -1;
			ReportViewer1.ServerReport.DisplayName = Report.ReportName;
			ReportViewer1.ServerReport.ReportServerUrl = new Uri(ReportExtra.ReportServerUrl);
			ReportViewer1.ServerReport.ReportPath = ReportExtra.ReportServerReportPath;
			ReportViewer1.ServerReport.ReportServerCredentials = new CustomReportCredentials(ReportExtra.ReportServerUsername, ReportExtra.ReportServerPassword, ReportExtra.ReportServerDomain);
			
			ReportViewer1.ServerReport.SetParameters(SetReportParameters(ReportViewer1.ServerReport.GetParameters()));
			ReportViewer1.ServerReport.Refresh();
		}
		
		private List<ReportParameter> SetReportParameters(ReportParameterInfoCollection reportParameters)
		{
			
			var parameters = new List<ReportParameter>();
			var additional = StringHelpers.ToDictionary(ReportExtra.AdditionalParameters, ',');
			// report parameters
			foreach (var parameterInfo in reportParameters)
			{
				var p = new ReportParameter(parameterInfo.Name);
				foreach (ParameterInfo parameter in State.Parameters)
				{
					if (parameter.ParameterName == p.Name)
					{
						if (parameter.Values != null)
						{
							foreach (var val in parameter.Values)
							{
								p.Values.Add(val);
							}
						}
						break;
					}
				}
				if (additional.ContainsKey(p.Name))
				{
					p.Values.Add(ReplaceReportTokens(additional[p.Name], null));
				}
				parameters.Add(p);
			}
			
			return parameters;
		}
		
#endregion

#region ReportViewer
		protected void ReportViewer_ReportRefresh(object sender, System.ComponentModel.CancelEventArgs e)
		{
			RefreshReport();
		}
#endregion
		
	}
	
	[Serializable()]
    public class CustomReportCredentials : IReportServerCredentials
	{

        private readonly string _username;

        private readonly string _password;

        private readonly string _domain;

        public WindowsIdentity ImpersonationUser => null;

	    public ICredentials NetworkCredentials => new NetworkCredential(_username, _password, _domain);

	    public CustomReportCredentials(string username, string password, string domain)
        {
            _username = username;
            _password = password;
            _domain = domain;
        }

        private static T InlineAssignHelper<T>(ref T target, T value)
        {
            target = value;
            return value;
        }

        public bool GetFormsCredentials(out Cookie authCookie, out string userName, out string password, out string authority)
        {
            password = null;
            authority = null;
            authCookie = null;
            userName = InlineAssignHelper<string>(ref password, InlineAssignHelper<string>(ref authority, null));
            return false;
        }
    }
	
}

