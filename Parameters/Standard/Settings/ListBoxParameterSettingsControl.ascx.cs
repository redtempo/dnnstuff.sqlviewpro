

using System;

using DNNStuff.SQLViewPro.Controls;

//***************************************************************************/
//* ListBoxParameterSettings.ascx.vb
//*
//* Copyright (c) 2004 by DNNStuff.
//* All rights reserved.
//*
//* Date:        August 9, 2004
//* Author:      Richard Edwards
//* Description: ListBox Parameter Settings Handler
//*************/


namespace DNNStuff.SQLViewPro.StandardParameters
{
	
	public partial class ListBoxParameterSettingsControl : ParameterSettingsControlBase
	{
		
		protected ConnectionPickerControl cpConnection;
		
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
			
			QueryStringInitialize();
		}
		
#endregion
		
#region  Page

	    public int ReportSetId { get; set; } = -1;

	    public int ParameterId { get; set; } = -1;

	    private void QueryStringInitialize()
		{
			// initialize
			if (!(Request.QueryString["ReportSetId"] == null))
			{
				ReportSetId = int.Parse(Request.QueryString["ReportSetId"].ToString());
			}
			else
			{
				ReportSetId = -1;
			}
			
			if (!(Request.QueryString["ParameterId"] == null))
			{
				ParameterId = int.Parse(Request.QueryString["ParameterId"].ToString());
			}
			else
			{
				ParameterId = -1;
			}
			
		}
#endregion
		
#region  Base Method Implementations
		protected override string LocalResourceFile => ResolveUrl("App_LocalResources/ListBoxParameterSettingsControl");

	    public override string UpdateSettings()
		{
			
			var obj = new ListBoxParameterSettings();
			obj.Default = txtDefault.Text;
			obj.List = txtList.Text;
			obj.Command = txtCommand.Text;
			obj.CommandCacheTimeout = Convert.ToInt32(txtCommandCacheTimeout.Text);
			obj.ConnectionId = Convert.ToInt32(cpConnection.ConnectionId);
			obj.AutoPostback = chkAutoPostback.Checked;
			obj.MultiSelect = chkMultiSelect.Checked;
			var temp_result = obj.MultiSelectSize;
			if (!int.TryParse(txtListBoxSize.Text, out temp_result))
			{
				obj.MultiSelectSize = temp_result;
				obj.MultiSelectSize = 5;
			}
			
			return Serialization.SerializeObject(obj, typeof(ListBoxParameterSettings));
			
		}
		
		public override void LoadSettings(string settings)
		{
			var obj = new ListBoxParameterSettings();
			if (settings != null)
			{
				obj = (ListBoxParameterSettings) (Serialization.DeserializeObject(settings, typeof(ListBoxParameterSettings)));
			}
			txtDefault.Text = obj.Default;
			txtList.Text = obj.List;
			txtCommand.Text = obj.Command;
			txtCommandCacheTimeout.Text = obj.CommandCacheTimeout.ToString();
			cpConnection.ConnectionId = obj.ConnectionId;
			chkAutoPostback.Checked = obj.AutoPostback;
			chkMultiSelect.Checked = obj.MultiSelect;
			txtListBoxSize.Text = obj.MultiSelectSize.ToString();
		}
		
#endregion
		
#region  Validation
		protected void vldCommand_ServerValidate(Object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
		{
			
			var msg = "";
			args.IsValid = Convert.ToBoolean(Services.Data.Query.IsQueryValid(txtCommand.Text, ConnectionController.GetConnectionString(Convert.ToInt32(cpConnection.ConnectionId), ReportSetId), ref msg));
			vldCommand.ErrorMessage = msg;
			
		}
		
		
		protected void cmdQueryTest_Click(object sender, EventArgs e)
		{
			var msg = "";
			var isValid = Convert.ToBoolean(Services.Data.Query.IsQueryValid(txtCommand.Text, ConnectionController.GetConnectionString(Convert.ToInt32(cpConnection.ConnectionId), ReportSetId), ref msg));
			
			lblQueryTestResults.Text = msg;
			lblQueryTestResults.CssClass = "NormalGreen";
			if (!isValid)
			{
				lblQueryTestResults.CssClass = "NormalRed";
			}
			
		}
#endregion
		
	}
	
}

