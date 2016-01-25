using System.Xml.Serialization;
using DNNStuff.SQLViewPro.Controls;

namespace DNNStuff.SQLViewPro.StandardReports
{
	
	public partial class NoReportSettingsControl : ReportSettingsControlBase
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
		
	
#region  Base Method Implementations
		protected override string LocalResourceFile
		{
			get
			{
				return ResolveUrl("App_LocalResources/NoReportSettingsControl");
			}
		}
		
		public override string UpdateSettings()
		{
			var obj = new NoReportSettings();
			return Serialization.SerializeObject(obj, typeof(NoReportSettings));
		}
		
		public override void LoadSettings(string settings)
		{
		}
		
#endregion
		
		
	}
	
#region  Settings
	[XmlRootAttribute(ElementName = "Settings", IsNullable = false)]public class NoReportSettings
	{
	}
#endregion
	
}

