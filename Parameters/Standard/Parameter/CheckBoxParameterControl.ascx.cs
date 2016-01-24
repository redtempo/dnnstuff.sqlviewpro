


using System.Collections.Generic;

//***************************************************************************/
//* CheckBoxParameter.ascx.vb
//*
//* Copyright (c) 2004 by DNNStuff.
//* All rights reserved.
//*
//* Date:        August 9, 2004
//* Author:      Richard Edwards
//* Description: CheckBox Parameter Handler
//*************/


namespace DNNStuff.SQLViewPro.StandardParameters
{
	
	public partial class CheckBoxParameterControl : Controls.ParameterControlBase
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
		public override List<string> Values
		{
			get
			{
				return new List<string>(new string[] {chkParameter.Checked.ToString()});
			}
			set
			{
				if (value.Count > 0)
				{
					chkParameter.Checked = bool.Parse(value[0].ToString());
				}
				else
				{
					chkParameter.Checked = false;
				}
			}
		}
		
		public override void LoadRuntimeSettings()
		{
			CheckBoxParameterSettings obj = (CheckBoxParameterSettings) (Serialization.DeserializeObject(Settings.ParameterConfig, typeof(CheckBoxParameterSettings)));
			chkParameter.Checked = obj.DefaultChecked;
		}
		
#endregion
		
	}
	
}

