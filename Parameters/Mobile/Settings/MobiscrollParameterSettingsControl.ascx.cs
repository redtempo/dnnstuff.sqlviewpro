


using System.Xml.Serialization;
using DNNStuff.SQLViewPro.Controls;

//***************************************************************************/
//* MobiscrollParameterSettings.ascx.vb
//*
//* Copyright (c) 2004 by DNNStuff.
//* All rights reserved.
//*
//* Date:        August 9, 2004
//* Author:      Richard Edwards
//* Description: Mobiscroll Parameter Settings Handler
//*************/


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
			
			MobiscrollParameterSettings obj = new MobiscrollParameterSettings();
			obj.Default = txtDefault.Text;
			obj.Preset = ddPreset.SelectedValue;
			obj.Theme = ddTheme.SelectedValue;
			obj.Mode = ddMode.SelectedValue;
			
			return Serialization.SerializeObject(obj, typeof(MobiscrollParameterSettings));
			
		}
		
		public override void LoadSettings(string settings)
		{
			MobiscrollParameterSettings obj = new MobiscrollParameterSettings();
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
		private string _DatabaseFormat = "";
		public string DatabaseFormat
		{
			get
			{
				return _DatabaseFormat;
			}
			set
			{
				_DatabaseFormat = value;
			}
		}
		private string _Preset = "date";
		public string Preset
		{
			get
			{
				return _Preset;
			}
			set
			{
				_Preset = value;
			}
		}
		private string _Theme = "default";
		public string Theme
		{
			get
			{
				return _Theme;
			}
			set
			{
				_Theme = value;
			}
		}
		private string _Mode = "Scroller";
		public string Mode
		{
			get
			{
				return _Mode;
			}
			set
			{
				_Mode = value;
			}
		}
	}
#endregion
	
}

