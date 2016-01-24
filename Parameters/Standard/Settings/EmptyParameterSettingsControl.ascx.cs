


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
	
	public partial class EmptyParameterSettingsControl : ParameterSettingsControlBase
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
				return ResolveUrl("App_LocalResources/EmptyParameterSettingsControl");
			}
		}
		
		public override string UpdateSettings()
		{
			
			EmptyParameterSettings obj = new EmptyParameterSettings();
			return Serialization.SerializeObject(obj, typeof(EmptyParameterSettings));
			
		}
		
		public override void LoadSettings(string settings)
		{
			
		}
		
#endregion
		
		
	}
	
#region  Settings
	[XmlRootAttribute(ElementName = "Settings", IsNullable = false)]public class EmptyParameterSettings
	{
	}
#endregion
	
}

