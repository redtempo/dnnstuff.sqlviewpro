

using System;

using DNNStuff.SQLViewPro.Controls;
using System.Web.UI.WebControls;

//***************************************************************************/
//* FlowListParameterSettings.ascx.vb
//*
//* Copyright (c) 2004 by DNNStuff.
//* All rights reserved.
//*
//* Date:        August 9, 2004
//* Author:      Richard Edwards
//* Description: FlowList Parameter Settings Handler
//*************/


namespace DNNStuff.SQLViewPro.StandardParameters
{
	
	public partial class FlowListParameterSettingsControl : ParameterSettingsControlBase
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
		private int _ReportSetId = -1;
		public int ReportSetId
		{
			get
			{
				return _ReportSetId;
			}
			set
			{
				_ReportSetId = value;
			}
		}
		private int _ParameterId = -1;
		public int ParameterId
		{
			get
			{
				return _ParameterId;
			}
			set
			{
				_ParameterId = value;
			}
		}
		
		
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
		protected override string LocalResourceFile
		{
			get
			{
				return ResolveUrl("App_LocalResources/FlowListParameterSettingsControl");
			}
		}
		
		public override string UpdateSettings()
		{
			
			FlowListParameterSettings obj = new FlowListParameterSettings();
			obj.Default = txtDefault.Text;
			obj.List = txtList.Text;
			obj.Command = txtCommand.Text;
			obj.ConnectionId = Convert.ToInt32(cpConnection.ConnectionId);
			obj.CommandCacheTimeout = Convert.ToInt32(txtCommandCacheTimeout.Text);
			
			obj.RepeatColumns = StringHelpers.DefaultInt32FromString(txtRepeatColumns.Text, 1);
			obj.RepeatDirection = (RepeatDirection) (Enum.Parse(typeof(RepeatDirection), ddlRepeatDirection.SelectedValue));
			obj.RepeatLayout = (RepeatLayout) (Enum.Parse(typeof(RepeatLayout), ddlRepeatLayout.SelectedValue));
			
			
			return Serialization.SerializeObject(obj, typeof(FlowListParameterSettings));
		}
		
		public override void LoadSettings(string settings)
		{
			FlowListParameterSettings obj = new FlowListParameterSettings();
			if (settings != null)
			{
				obj = (FlowListParameterSettings) (Serialization.DeserializeObject(settings, typeof(FlowListParameterSettings)));
			}
			
			txtDefault.Text = obj.Default;
			txtList.Text = obj.List;
			txtCommand.Text = obj.Command;
			txtCommandCacheTimeout.Text = obj.CommandCacheTimeout.ToString();
			txtRepeatColumns.Text = obj.RepeatColumns.ToString();
			ddlRepeatDirection.Items.FindByValue(obj.RepeatDirection.ToString()).Selected = true;
			ddlRepeatLayout.Items.FindByValue(obj.RepeatLayout.ToString()).Selected = true;
			cpConnection.ConnectionId = obj.ConnectionId;
			
		}
		
#endregion
		
#region  Validation
		private void vldCommand_ServerValidate(Object source, ServerValidateEventArgs args)
		{
			
			string msg = "";
			args.IsValid = Convert.ToBoolean(Services.Data.Query.IsQueryValid(txtCommand.Text, ConnectionController.GetConnectionString(Convert.ToInt32(cpConnection.ConnectionId), ReportSetId), ref msg));
			vldCommand.ErrorMessage = msg;
			
		}
		
		
		protected void cmdQueryTest_Click(object sender, EventArgs e)
		{
			string msg = "";
			bool isValid = Convert.ToBoolean(Services.Data.Query.IsQueryValid(txtCommand.Text, ConnectionController.GetConnectionString(Convert.ToInt32(cpConnection.ConnectionId), ReportSetId), ref msg));
			
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

