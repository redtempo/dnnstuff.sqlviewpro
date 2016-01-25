


using System.Xml.Serialization;
using DNNStuff.SQLViewPro.Controls;

//***************************************************************************/
//* EmptyParameterSettings.ascx.vb
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
	
	public partial class GeoLocationParameterSettingsControl : ParameterSettingsControlBase
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
		protected override string LocalResourceFile => ResolveUrl("App_LocalResources/GeoLocationParameterSettingsControl");

	    public override string UpdateSettings()
		{
			
			var obj = new GeoLocationParameterSettings();
			obj.EnableHighAccuracy = chkEnableHighAccuracy.Checked;
			obj.Timeout = StringHelpers.DefaultInt32FromString(txtTimeout.Text, 5000);
			obj.MaximumAge = StringHelpers.DefaultInt32FromString(txtMaximumAge.Text, 60000);
			return Serialization.SerializeObject(obj, typeof(GeoLocationParameterSettings));
			
		}
		
		public override void LoadSettings(string settings)
		{
			var obj = new GeoLocationParameterSettings();
			
			if (settings != null)
			{
				obj = (GeoLocationParameterSettings) (Serialization.DeserializeObject(settings, typeof(GeoLocationParameterSettings)));
			}
			
			chkEnableHighAccuracy.Checked = obj.EnableHighAccuracy;
			txtTimeout.Text = obj.Timeout.ToString();
			txtMaximumAge.Text = obj.MaximumAge.ToString();
			
		}
		
		public override bool CaptionRequired => false;

	    #endregion
		
		
		
	}
	
#region  Settings
	[XmlRootAttribute(ElementName = "Settings", IsNullable = false)]public class GeoLocationParameterSettings
	{
	    public bool EnableHighAccuracy { get; set; } = true;

	    public int Timeout { get; set; } = 5000;

	    public int MaximumAge { get; set; } = 60000;
	}
#endregion
	
}

