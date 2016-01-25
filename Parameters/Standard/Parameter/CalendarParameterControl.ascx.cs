

using System;

using DNNStuff.SQLViewPro.Services.Data;
using System.Globalization;
using System.Collections.Generic;

//***************************************************************************/
//* CalendarParameter.ascx.vb
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
	
	public partial class CalendarParameterControl : Controls.ParameterControlBase
	{
		
#region  Web Form Designer Generated Code
		
		//This call is required by the Web Form Designer.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent()
		{
			
		}
		
		private void Page_Init(Object sender, EventArgs e)
		{
			//CODEGEN: This method call is required by the Web Form Designer
			//Do not modify it using the code editor.
			InitializeComponent();
		}
		
#endregion
		
#region  Page
		private void Page_Load(Object sender, EventArgs e)
		{
			cmdStartCalendar.NavigateUrl = (string) (DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtCalendar));
		}
		
#endregion
		
#region  Base Method Implementations
		
		public override List<string> Values
		{
			get
			{
				var ret = txtCalendar.Text;
				var dt = default(DateTime);
				if (DateTime.TryParse(txtCalendar.Text, out dt))
				{
					if (CalendarSettings().DatabaseDateFormat != "")
					{
						ret = dt.ToString(CalendarSettings().DatabaseDateFormat);
					}
					else
					{
						ret = dt.ToString("M/d/yyyy");
					}
				}
				return new List<string>(new string[] {ret});
			}
			
			set
			{
				
				var provider = CultureInfo.GetCultureInfo(PortalSettings.DefaultLanguage);
				var dt = default(DateTime);
				if (value.Count > 0)
				{
					if (DateTime.TryParseExact(value[0].ToString(), "M/d/yyyy", provider, DateTimeStyles.None, out dt))
					{
						txtCalendar.Text = dt.ToShortDateString();
					}
				}
				else
				{
					txtCalendar.Text = "";
				}
				
			}
		}
		
		public override void LoadRuntimeSettings()
		{
			Values = new List<string>(new string[] {TokenReplacement.ReplaceTokens(CalendarSettings().Default, null, null)});
		}
		
#endregion
		private CalendarParameterSettings CalendarSettings()
		{
			return ((CalendarParameterSettings) (Serialization.DeserializeObject(Settings.ParameterConfig, typeof(CalendarParameterSettings))));
		}
	}
	
}

