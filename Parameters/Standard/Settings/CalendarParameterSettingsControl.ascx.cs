


using System.Xml.Serialization;
using DNNStuff.SQLViewPro.Controls;

//***************************************************************************/
//* CalendarParameterSettings.ascx.vb
//*
//* Copyright (c) 2004 by DNNStuff.
//* All rights reserved.
//*
//* Date:        August 9, 2004
//* Author:      Richard Edwards
//* Description: Calendar Parameter Settings Handler
//*************/


namespace DNNStuff.SQLViewPro.StandardParameters
{
	
	public partial class CalendarParameterSettingsControl : ParameterSettingsControlBase
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
				return ResolveUrl("App_LocalResources/CalendarParameterSettingsControl");
			}
		}
		
		public override string UpdateSettings()
		{
			
			CalendarParameterSettings obj = new CalendarParameterSettings();
			obj.Default = txtDefault.Text;
			obj.DatabaseDateFormat = txtDatabaseDateFormat.Text;
			
			return Serialization.SerializeObject(obj, typeof(CalendarParameterSettings));
			
		}
		
		public override void LoadSettings(string settings)
		{
			CalendarParameterSettings obj = new CalendarParameterSettings();
			if (settings != null)
			{
				obj = (CalendarParameterSettings) (Serialization.DeserializeObject(settings, typeof(CalendarParameterSettings)));
			}
			txtDefault.Text = obj.Default;
			txtDatabaseDateFormat.Text = obj.DatabaseDateFormat;
		}
		
#endregion
		
	}
	
#region  Settings
	[XmlRootAttribute(ElementName = "Settings", IsNullable = false)]public class CalendarParameterSettings
	{
		public string Default {get; set;}
		private string _DatabaseDateFormat = "yyyy-M-d";
		public string DatabaseDateFormat
		{
			get
			{
				return _DatabaseDateFormat;
			}
			set
			{
				_DatabaseDateFormat = value;
			}
		}
	}
#endregion
	
}

