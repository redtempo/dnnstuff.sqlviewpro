


using System.Collections.Generic;

//***************************************************************************/
//* CheckListBoxParameterControl.ascx.vb
//*
//* Copyright (c) 2004 by DNNStuff.
//* All rights reserved.
//*
//* Date:        August 9, 2004
//* Author:      Richard Edwards
//* Description: CheckBoxList Parameter Handler
//*************/


namespace DNNStuff.SQLViewPro.StandardParameters
{
	
	public partial class CheckBoxListParameterControl : ListParameterControlBase
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
				var selected = new List<string>();
				foreach (System.Web.UI.WebControls.ListItem li in cblParameter.Items)
				{
					if (li.Selected)
					{
						selected.Add(li.Value.ToString());
					}
				}
				return selected;
			}
			
			set
			{
				if (value.Count > 0)
				{
					cblParameter.SelectedValue = value[0].ToString();
				}
				else
				{
					cblParameter.SelectedValue = "";
				}
			}
		}
		public override bool MultiValued => true;

	    public override void LoadRuntimeSettings()
		{
			var obj = (FlowListParameterSettings) (Serialization.DeserializeObject(Settings.ParameterConfig, typeof(FlowListParameterSettings)));
			cblParameter.RepeatColumns = obj.RepeatColumns;
			cblParameter.RepeatDirection = obj.RepeatDirection;
			cblParameter.RepeatLayout = obj.RepeatLayout;
			AddOptions(cblParameter, obj, Settings);
			SelectDefaults(cblParameter, obj, true);
		}
		
#endregion
		
	}
	
}

