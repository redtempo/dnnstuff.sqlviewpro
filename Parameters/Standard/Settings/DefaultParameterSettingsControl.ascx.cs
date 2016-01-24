


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
		
		protected override string LocalResourceFile
		{
			get
			{
				return ResolveUrl("App_LocalResources/DefaultParameterSettingsControl");
			}
		}
		
		public override string UpdateSettings()
		{
			
			DefaultParameterSettings obj = new DefaultParameterSettings();
			obj.Default = txtDefault.Text;
			
			return Serialization.SerializeObject(obj, typeof(DefaultParameterSettings));
			
		}
		
		public override void LoadSettings(string settings)
		{
			DefaultParameterSettings obj = new DefaultParameterSettings();
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
		private string _Default;
		public string Default
		{
			get
			{
				return _Default;
			}
			set
			{
				_Default = value;
			}
		}
	}
#endregion
	
}

