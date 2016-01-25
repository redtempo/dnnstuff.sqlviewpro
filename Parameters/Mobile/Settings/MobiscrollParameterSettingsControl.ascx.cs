using System.Xml.Serialization;
using DNNStuff.SQLViewPro.Controls;

namespace DNNStuff.SQLViewPro.MobileParameters
{
	
	public partial class MobiscrollParameterSettingsControl : ParameterSettingsControlBase
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
				return ResolveUrl("App_LocalResources/MobiscrollParameterSettingsControl");
			}
		}
		
		public override string UpdateSettings()
		{
			
			var obj = new MobiscrollParameterSettings();
			obj.Default = txtDefault.Text;
			obj.Preset = ddPreset.SelectedValue;
			obj.Theme = ddTheme.SelectedValue;
			obj.Mode = ddMode.SelectedValue;
			
			return Serialization.SerializeObject(obj, typeof(MobiscrollParameterSettings));
			
		}
		
		public override void LoadSettings(string settings)
		{
			var obj = new MobiscrollParameterSettings();
			if (settings != null)
			{
				obj = (MobiscrollParameterSettings) (Serialization.DeserializeObject(settings, typeof(MobiscrollParameterSettings)));
			}
			
			txtDefault.Text = obj.Default;
			ddPreset.SelectedValue = obj.Preset;
			ddTheme.SelectedValue = obj.Theme;
			ddMode.SelectedValue = obj.Mode;
		}
		
#endregion
		
	}
	
#region  Settings
	[XmlRootAttribute(ElementName = "Settings", IsNullable = false)]public class MobiscrollParameterSettings
	{
		public string Default {get; set;}
	    public string DatabaseFormat { get; set; } = "";

	    public string Preset { get; set; } = "date";

	    public string Theme { get; set; } = "default";

	    public string Mode { get; set; } = "Scroller";
	}
#endregion
	
}

