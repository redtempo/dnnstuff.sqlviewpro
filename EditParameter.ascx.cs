

using System;
using DotNetNuke.Common;
using DotNetNuke.Entities.Modules;



//***************************************************************************/
//* EditSQLViewPro.ascx.vb
//*
//* Copyright (c) 2004 by DNNStuff.
//* All rights reserved.
//*
//* Date:        August 9, 2004
//* Author:      Richard Edwards
//* Description: Portal Module for editing setting for a report grid
//*************/


namespace DNNStuff.SQLViewPro
{
	
	public partial class EditParameter : PortalModuleBase
	{
		private const string STR_ReferringUrl = "EditParameter_ReferringUrl";
		
		
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
			
			// initialize
			ParameterId = int.Parse(DNNUtilities.QueryStringDefault(Request, "ParameterId", "-1"));
			ReportSetId = int.Parse(DNNUtilities.QueryStringDefault(Request, "ReportSetId", "-1"));
			
			InitParameter();
			
		}
		
#endregion
		
#region  Page
		
		public int ReportSetId {get; set;}
		public int ParameterId {get; set;}
		public ParameterInfo Parameter {get; set;}
		public string ParameterConfig {get; set;}
		
		private void SaveReferringPage()
		{
			// save referring page
			if (Request.UrlReferrer == null)
			{
				Session[STR_ReferringUrl] = Globals.NavigateURL();
			}
			else
			{
				Session[STR_ReferringUrl] = Request.UrlReferrer.AbsoluteUri;
			}
		}

		private void Page_Load(Object sender, EventArgs e)
		{
			DNNUtilities.InjectCSS(Page, ResolveUrl("Resources/Support/edit.css"));
			
			
			Page.ClientScript.RegisterClientScriptInclude(GetType(), "yeti", ResolveUrl("resources/support/yetii-min.js"));
			
			
			if (Page.IsPostBack)
			{
				RenderParameterSettings();
			}
			else
			{
				SaveReferringPage();
				
				// drop down parameter type
				BindParameterType();
				BindParameter();
				
				// do parameter type
				RenderParameterSettings();
				RetrieveParameterSettings();
			}
			
		}
		
#endregion
		
#region  Data
		private void InitParameter()
		{
			ParameterController objParameterController = new ParameterController();
			ParameterInfo objParameter = objParameterController.GetParameter(ParameterId);
			
			// load from database
			if (objParameter == null)
			{
				Parameter = new ParameterInfo();
			}
			else
			{
				Parameter = objParameter;
			}
		}
		
		private void BindParameter()
		{
			txtName.Text = Parameter.ParameterName;
			txtCaption.Text = Parameter.ParameterCaption;
			ddParameterType.SelectedValue = Parameter.ParameterTypeId;
		}
		
		private void SaveParameter()
		{
			RetrieveParameterSettings();
			
			ParameterController objParameterController = new ParameterController();
			ParameterId = objParameterController.UpdateParameter(ReportSetId, ParameterId, txtName.Text, txtCaption.Text, ddParameterType.SelectedValue, ParameterConfig, -1);
		}
		
		private void BindParameterType()
		{
			ParameterTypeController objParameterTypeController = new ParameterTypeController();
			ddParameterType.DataTextField = "ParameterTypeName";
			ddParameterType.DataValueField = "ParameterTypeId";
			ddParameterType.DataSource = objParameterTypeController.ListParameterType();
			ddParameterType.DataBind();
		}
		
#endregion
		
#region  Navigation
		protected void cmdUpdate_Click(object sender, EventArgs e)
		{
			
			if (Page.IsValid)
			{
				SaveParameter();
				
				// Redirect back to the Parameter set
				NavigateBack();
				
			}
			
		}
		
		protected void cmdCancel_Click(object sender, EventArgs e)
		{
			NavigateBack();
		}
		
		private void NavigateBack()
		{
			if (Session[STR_ReferringUrl] != null)
			{
				Response.Redirect(Session[STR_ReferringUrl].ToString());
			}
		}
		
#endregion
		
		private void RetrieveParameterSettings()
		{
			Controls.ParameterSettingsControlBase objParameterSettings = (Controls.ParameterSettingsControlBase) (phParameterSettings.FindControl("ParameterSettings"));
			
			ParameterConfig = (string) objParameterSettings.UpdateSettings();
			
		}
		
		private void RenderParameterSettings()
		{
			string parameterTypeId = ddParameterType.SelectedValue;
			
			ParameterTypeController objParameterTypeController = new ParameterTypeController();
			
			ParameterTypeInfo objParameterType = objParameterTypeController.GetParameterType(parameterTypeId);
			
			Controls.ParameterSettingsControlBase objParameterSettingsBase = default(Controls.ParameterSettingsControlBase);
			objParameterSettingsBase = (Controls.ParameterSettingsControlBase) (LoadControl(ResolveUrl(objParameterType.ParameterTypeSettingsControlSrc)));
			
			objParameterSettingsBase.ID = "ParameterSettings";
			if (Parameter != null)
			{
				objParameterSettingsBase.LoadSettings(Parameter.ParameterConfig);
			}
			else
			{
				objParameterSettingsBase.LoadSettings(null);
			}
			phParameterSettings.Controls.Add(objParameterSettingsBase);
			
			// update settings requirements
			txtCaption_Required.Enabled = Convert.ToBoolean(objParameterSettingsBase.CaptionRequired);
			
		}
		private void ddParameterType_SelectedIndexChanged(object sender, EventArgs e)
		{
			RetrieveParameterSettings();
		}
	}
	
}

