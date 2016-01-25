using System;
using System.Data;
using System.Web.UI;
using DotNetNuke.Common;

namespace DNNStuff.SQLViewPro.StandardReports
{
	
	public partial class ExcelReportControl : Controls.ReportControlBase
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
			
			try
			{
				ExportReport();
			}
			catch (Exception ex)
			{
				DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
			}
		}
		
#endregion
		
#region  Page

	    private NoReportSettings ReportExtra { get; set; } = new NoReportSettings();

	    #endregion
		
#region  Base Method Implementations
		
		public override void LoadRuntimeSettings(ReportInfo settings)
		{
			ReportExtra = (NoReportSettings) (Serialization.DeserializeObject(settings.ReportConfig, typeof(NoReportSettings)));
		}
		
#endregion
		
#region  Excel
		
		private void ExportReport()
		{
			var ds = ReportData();
			var details = new ExportDetails();
			details.Dataset = ds;
			details.Filename = (string) (Globals.CleanFileName(Report.ReportName + ".xls"));
			Session[Export.EXPORT_KEY] = details;
			Controls.Add(new LiteralControl(string.Format("<iframe style=\'display:none\' scrolling=\'auto\' src=\'{0}?ModuleId={1}&TabId={2}\'></iframe>", ResolveUrl("~/DesktopModules/DNNStuff - SQLViewPro/Export.aspx"), State.ModuleId, State.TabId)));
		}
		
#endregion
		
	}
	
}

