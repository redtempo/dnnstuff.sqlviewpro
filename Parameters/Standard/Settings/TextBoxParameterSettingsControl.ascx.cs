using System.Xml.Serialization;
using DNNStuff.SQLViewPro.Controls;

namespace DNNStuff.SQLViewPro.StandardParameters
{
	
	public partial class TextBoxParameterSettingsControl : ParameterSettingsControlBase
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
		protected override string LocalResourceFile => ResolveUrl("App_LocalResources/TextBoxParameterSettingsControl");

	    public override string UpdateSettings()
		{
			
			var obj = new TextBoxParameterSettings();
			obj.Default = txtDefault.Text;
			obj.Rows = StringHelpers.DefaultInt32FromString(txtRows.Text, 1);
			obj.Columns = StringHelpers.DefaultInt32FromString(txtColumns.Text, 0);
			
			return Serialization.SerializeObject(obj, typeof(TextBoxParameterSettings));
			
		}
		
		public override void LoadSettings(string settings)
		{
			var obj = new TextBoxParameterSettings();
			if (settings != null)
			{
				obj = (TextBoxParameterSettings) (Serialization.DeserializeObject(settings, typeof(TextBoxParameterSettings)));
			}
			
			txtDefault.Text = obj.Default;
			txtRows.Text = obj.Rows.ToString();
			txtColumns.Text = obj.Columns.ToString();
		}
		
#endregion
		
		
	}
	
#region  Settings
	[XmlRootAttribute(ElementName = "Settings", IsNullable = false)]public class TextBoxParameterSettings
	{
		public string Default {get; set;}
	    public int Rows { get; set; } = 1;

	    public int Columns { get; set; } = 0;
	}
#endregion
	
}

