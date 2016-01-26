using System.Xml.Serialization;
using DNNStuff.SQLViewPro.Controls;

namespace DNNStuff.SQLViewPro.StandardParameters
{
	
	public partial class DefaultParameterSettingsControl : ParameterSettingsControlBase
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
		
		protected override string LocalResourceFile => ResolveUrl("App_LocalResources/DefaultParameterSettingsControl");

	    public override string UpdateSettings()
		{
			
			var obj = new DefaultParameterSettings();
			obj.Default = txtDefault.Text;
			
			return Serialization.SerializeObject(obj, typeof(DefaultParameterSettings));
			
		}
		
		public override void LoadSettings(string settings)
		{
			var obj = new DefaultParameterSettings();
			if (settings != null)
			{
				obj = (DefaultParameterSettings) (Serialization.DeserializeObject(settings, typeof(DefaultParameterSettings)));
			}
			
			txtDefault.Text = obj.Default;
		}
		
#endregion
		
		
	}
	
#region  Settings
	[XmlRootAttribute(ElementName = "Settings", IsNullable = false)]public class DefaultParameterSettings
	{
	    public string Default { get; set; }
	}
#endregion
	
}

