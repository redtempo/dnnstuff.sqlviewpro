


using System.Xml.Serialization;
using DNNStuff.SQLViewPro.Controls;

//***************************************************************************/
//* XmlReportSettings.ascx.vb
//*
//* Copyright (c) 2004 by DNNStuff.
//* All rights reserved.
//*
//* Date:        August 9, 2004
//* Author:      Richard Edwards
//* Description: Xml Report Settings Handler
//*************/


namespace DNNStuff.SQLViewPro.StandardReports
{
	
	public partial class XmlReportSettingsControl : ReportSettingsControlBase
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
		
#region  Page
		private void Page_Load(System.Object sender, System.EventArgs e)
		{
		}
		
#endregion
		
#region  Base Method Implementations
		protected override string LocalResourceFile
		{
			get
			{
				return ResolveUrl("App_LocalResources/XmlReportSettingsControl");
			}
		}
		
		public override string UpdateSettings()
		{
			
			XmlReportSettings obj = new XmlReportSettings();
			obj.XslSrc = txtXslSrc.Text;
			
			return Serialization.SerializeObject(obj, typeof(XmlReportSettings));
			
		}
		
		public override void LoadSettings(string settings)
		{
			XmlReportSettings obj = new XmlReportSettings();
			if (!string.IsNullOrEmpty(settings))
			{
				obj = (XmlReportSettings) (Serialization.DeserializeObject(settings, typeof(XmlReportSettings)));
			}
			txtXslSrc.Text = obj.XslSrc;
		}
		
#endregion
		
	}
	
#region  Settings
	[XmlRootAttribute(ElementName = "Settings", IsNullable = false)]public class XmlReportSettings
	{
		private string _XslSrc;
		public string XslSrc
		{
			get
			{
				return _XslSrc;
			}
			set
			{
				_XslSrc = value;
			}
		}
	}
#endregion
	
}

