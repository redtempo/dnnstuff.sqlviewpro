


using System.Collections.Generic;

//***************************************************************************/
//* DefaultParameter.ascx.vb
//*
//* Copyright (c) 2004 by DNNStuff.
//* All rights reserved.
//*
//* Date:        August 9, 2004
//* Author:      Richard Edwards
//* Description: Default Parameter Handler
//*************/


namespace DNNStuff.SQLViewPro.StandardParameters
{
	
	public partial class GeoLocationParameterControl : Controls.ParameterControlBase
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
		public override List<string> Values
		{
			get
			{
				return new List<string>(new string[] {geolocation.Value});
			}
			set
			{
				if (value.Count > 0)
				{
					geolocation.Value = value[0].ToString();
				}
				else
				{
					geolocation.Value = "";
				}
			}
		}
		
		public override System.Collections.Specialized.StringDictionary ExtraValues
		{
			get
			{
				string location = geolocation.Value;
				System.Collections.Specialized.StringDictionary vals = new System.Collections.Specialized.StringDictionary();
				if (location.Length > 0)
				{
					vals.Add("Latitude", (string) (location.Split(',')[0]));
					vals.Add("Longitude", (string) (location.Split(',')[1]));
				}
				else
				{
					vals.Add("Latitude", "");
					vals.Add("Longitude", "");
				}
				return vals;
			}
		}
		
		public override void LoadRuntimeSettings()
		{
		}
#endregion
		
		public GeoLocationParameterSettings GeoLocationSettings()
		{
			return ((GeoLocationParameterSettings) (Serialization.DeserializeObject(Settings.ParameterConfig, typeof(GeoLocationParameterSettings))));
		}
		
	}
	
}

