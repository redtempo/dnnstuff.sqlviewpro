


using System.Xml.Serialization;
using DNNStuff.SQLViewPro.Controls;

//***************************************************************************/
//* DefaultParameterSettings.ascx.vb
//*
//* Copyright (c) 2004 by DNNStuff.
//* All rights reserved.
//*
//* Date:        August 9, 2004
//* Author:      Richard Edwards
//* Description: Default Parameter Settings Handler
//*************/


namespace DNNStuff.SQLViewPro.StandardParameters
{
	
	public partial class CheckBoxParameterSettingsControl : ParameterSettingsControlBase
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
		protected override string LocalResourceFile => ResolveUrl("App_LocalResources/CheckBoxParameterSettingsControl");

	    public override string UpdateSettings()
		{
			
			var obj = new CheckBoxParameterSettings();
			obj.DefaultChecked = chkDefault.Checked;
			
			return Serialization.SerializeObject(obj, typeof(CheckBoxParameterSettings));
			
		}
		
		public override void LoadSettings(string settings)
		{
			var obj = new CheckBoxParameterSettings();
			if (settings != null)
			{
				obj = (CheckBoxParameterSettings) (Serialization.DeserializeObject(settings, typeof(CheckBoxParameterSettings)));
			}
			
			chkDefault.Checked = obj.DefaultChecked;
		}
		
#endregion
		
	}
	
#region  Settings
	[XmlRootAttribute(ElementName = "Settings", IsNullable = false)]public class CheckBoxParameterSettings
	{
	    public bool DefaultChecked { get; set; }
	}
#endregion
	
}

