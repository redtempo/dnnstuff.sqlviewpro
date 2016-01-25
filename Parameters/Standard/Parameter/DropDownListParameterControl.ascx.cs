


using System.Collections.Generic;

//***************************************************************************/
//* DropDownListParameterControl.ascx.vb
//*
//* Copyright (c) 2004 by DNNStuff.
//* All rights reserved.
//*
//* Date:        August 9, 2004
//* Author:      Richard Edwards
//* Description: DropdownList Parameter Control
//*************/


namespace DNNStuff.SQLViewPro.StandardParameters
{
	
	public partial class DropDownListParameterControl : ListParameterControlBase
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
				return new List<string>(new string[] {ddlParameter.SelectedValue});
			}
			set
			{
				if (value.Count > 0)
				{
					ddlParameter.SelectedValue = value[0].ToString();
				}
				else
				{
					ddlParameter.SelectedValue = "";
				}
			}
		}
		
		public override void LoadRuntimeSettings()
		{
			DropDownListParameterSettings obj = (DropDownListParameterSettings) (Serialization.DeserializeObject(Settings.ParameterConfig, typeof(DropDownListParameterSettings)));
			AddOptions(ddlParameter, obj, Settings);
			SelectDefaults(ddlParameter, obj, false);
			ddlParameter.AutoPostBack = obj.AutoPostback;
			ddlParameter.SelectedIndexChanged += new System.EventHandler(ddlParameter_SelectedIndexChanged);
		}
		
#endregion
		
		protected void ddlParameter_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Run(this);
		}
		
	}
	
}

