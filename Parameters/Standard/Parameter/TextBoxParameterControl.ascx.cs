


using DNNStuff.SQLViewPro.Services.Data;
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
	
	public partial class TextBoxParameterControl : Controls.ParameterControlBase
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
				return new List<string>(new string[] {txtParameter.Text});
			}
			set
			{
				if (value.Count > 0)
				{
					txtParameter.Text = value[0].ToString();
				}
				else
				{
					txtParameter.Text = "";
				}
			}
		}
		
		public override void LoadRuntimeSettings()
		{
			TextBoxParameterSettings obj = (TextBoxParameterSettings) (Serialization.DeserializeObject(Settings.ParameterConfig, typeof(TextBoxParameterSettings)));
			txtParameter.Text = TokenReplacement.ReplaceTokens(obj.Default, null, null);
			if (obj.Rows > 1)
			{
				txtParameter.TextMode = System.Web.UI.WebControls.TextBoxMode.MultiLine;
				txtParameter.Columns = obj.Columns;
				txtParameter.Rows = obj.Rows;
			}
			//Dim requiredControl As String = String.Format("<asp:RequiredFieldValidator id=""{0}_Required"" runat=""server"" ControlToValidate=""{0}"" Display=""Dynamic"" ErrorMessage=""<br />{1} is required"" />", "txtParameter", Settings.ParameterName)
			//Controls.Add(ParseControl(requiredControl))
		}
#endregion
		
	}
	
}

