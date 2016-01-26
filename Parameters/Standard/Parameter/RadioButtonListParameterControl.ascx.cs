


using System.Collections.Generic;

//***************************************************************************/
//* RadioButtonParameter.ascx.vb
//*
//* Copyright (c) 2004 by DNNStuff.
//* All rights reserved.
//*
//* Date:        August 9, 2004
//* Author:      Richard Edwards
//* Description: RadioButtonList Parameter Handler
//*************/


namespace DNNStuff.SQLViewPro.StandardParameters
{
	
	public partial class RadioButtonListParameterControl : ListParameterControlBase
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
				return new List<string>(new string[] {rblParameter.SelectedValue});
			}
			set
			{
				if (value.Count > 0)
				{
					rblParameter.SelectedValue = value[0].ToString();
				}
				else
				{
					rblParameter.SelectedValue = "";
				}
			}
		}
		
		public override void LoadRuntimeSettings()
		{
			var obj = (FlowListParameterSettings) (Serialization.DeserializeObject(Settings.ParameterConfig, typeof(FlowListParameterSettings)));
			rblParameter.RepeatColumns = obj.RepeatColumns;
			rblParameter.RepeatDirection = obj.RepeatDirection;
			rblParameter.RepeatLayout = obj.RepeatLayout;
			AddOptions(rblParameter, obj, Settings);
			SelectDefaults(rblParameter, obj, false);
		}
		
#endregion
		
	}
	
}

