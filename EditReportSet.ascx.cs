using DotNetNuke.Services.Localization;
using System.Web.UI.WebControls;
using System.Collections;
using DotNetNuke.Common;
using DotNetNuke.Entities.Modules;

namespace DNNStuff.SQLViewPro
{
	
	public partial class EditReportSet : PortalModuleBase
	{
		private const string STR_ReferringUrl = "EditReportSet_ReferringUrl";
		
		protected DropDownList cboTheme;
		protected Controls.ConnectionPickerControl cpConnection;
		
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
		
		// collection counts for use with move up/down binding
		private int _ParameterCount;
		private int _ReportCount;
		
		private int _ReportSetId;
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
		
		private void SaveReferringPage()
		{
			// save referring page
			if (Request.UrlReferrer == null)
			{
				Session[STR_ReferringUrl] = Globals.NavigateURL();
			}
			else
			{
				// don't set if coming from a later page such as EditReport or EditParameter
				if (!(Request.UrlReferrer.AbsoluteUri.ToString().Contains("EditReport/") || Request.UrlReferrer.AbsoluteUri.ToString().Contains("EditParameter/")))
				{
					Session[STR_ReferringUrl] = Globals.NavigateURL();
				}
			}
		}
		
		private void Page_Load(System.Object sender, System.EventArgs e)
		{
			DNNUtilities.InjectCSS(Page, ResolveUrl("Resources/Support/edit.css"));
			Page.ClientScript.RegisterClientScriptInclude(GetType(), "yeti", ResolveUrl("resources/support/yetii-min.js"));
			
			if (Request.QueryString["ReportSetId"] != null)
			{
				ReportSetId = int.Parse(Request.QueryString["ReportSetId"].ToString());
			}
			else
			{
				ReportSetId = -1;
			}
			
			if (!Page.IsPostBack)
			{
				BindSkinFolder(cboSkin);
				LoadReportSet();
				
				SaveReferringPage();
			}
			
		}
		
#endregion
		
#region  Data ReportSet
		private void LoadReportSet()
		{
			ReportSetController objReportSetController = new ReportSetController();
			ReportSetInfo objReportSet = objReportSetController.GetReportSet(ReportSetId);
			
			if (objReportSet == null)
			{
				// create new
				txtName.Text = "New Report Set";
				SaveReportSet();
				Response.Redirect(NavigateReportSet(ReportSetId));
			}
			
			txtName.Text = objReportSet.ReportSetName;
			
			ListItem item = cboSkin.Items.FindByValue(objReportSet.ReportSetTheme);
			if (item != null)
			{
				item.Selected = true;
			}
			else
			{
				cboSkin.Items[0].Selected = true;
			}
			
			cpConnection.ConnectionId = objReportSet.ReportSetConnectionId;
			txtHeader.Text = objReportSet.ReportSetHeaderText;
			txtFooter.Text = objReportSet.ReportSetFooterText;
			chkDebug.Checked = objReportSet.ReportSetDebug;
			txtRunCaption.Text = objReportSet.RunCaption;
			txtBackCaption.Text = objReportSet.BackCaption;
			txtParameterLayout.Text = objReportSet.ParameterLayout;
			chkAlwaysShowParameters.Checked = objReportSet.AlwaysShowParameters;
			
			ControlHelpers.InitDropDownByValue(ddlRenderMode, objReportSet.RenderMode);
			chkAutoRun.Checked = objReportSet.AutoRun;
			
			
			// report grid
			BindReport();
			// parameter grid
			BindParameter();
		}
		
		private void SaveReportSet()
		{
			ReportSetController objReportSetController = new ReportSetController();
			ReportSetConfig obj = new ReportSetConfig();
			ReportSetId = objReportSetController.UpdateReportSet(ModuleId, ReportSetId, txtName.Text, cboSkin.SelectedItem.Value, System.Convert.ToInt32(cpConnection.ConnectionId), txtHeader.Text, txtFooter.Text, chkDebug.Checked, txtRunCaption.Text, txtBackCaption.Text, txtParameterLayout.Text, chkAlwaysShowParameters.Checked, ddlRenderMode.SelectedValue, chkAutoRun.Checked, Serialization.SerializeObject(obj, typeof(ReportSetConfig)));
		}
		
		private void BindSkinFolder(ListControl o)
		{
			System.IO.DirectoryInfo skinFolder = new System.IO.DirectoryInfo((string) (Server.MapPath(ResolveUrl("Skins"))));
			o.Items.Clear();
			o.Items.Add(new ListItem("None", "None"));
			foreach (System.IO.DirectoryInfo folder in skinFolder.GetDirectories())
			{
				o.Items.Add(folder.Name);
			}
		}
		
#endregion
		
#region  Data Report
		// report
		private void DeleteReport(int ReportId)
		{
			ReportController objReportController = new ReportController();
			objReportController.DeleteReport(ReportId);
		}
		
		private void MoveReport(int ReportId, int Increment)
		{
			ReportController objReportController = new ReportController();
			objReportController.UpdateReportOrder(ReportId, Increment);
		}
		
		private void CopyReport(int ReportId)
		{
			ReportController objReportController = new ReportController();
			ReportInfo objReport = objReportController.GetReport(ReportId);
			objReportController.UpdateReport(ReportSetId, -1, objReport.ReportTypeId, "Copy of " + objReport.ReportName, objReport.ReportTheme, objReport.ReportConnectionId, objReport.ReportHeaderText, objReport.ReportFooterText, objReport.ReportCommand, objReport.ReportConfig, -1, objReport.ReportDrilldownReportId, objReport.ReportDrilldownFieldname, objReport.ReportNoItemsText, objReport.ReportPageTitle, objReport.ReportCommandCacheTimeout, objReport.ReportMetaDescription, objReport.ReportCommandCacheScheme);
			
		}
		
		private void BindReport()
		{
			
			Localization.LocalizeDataGrid(ref dgReport, LocalResourceFile);
			
			ArrayList objReportList = default(ArrayList);
			ReportSetController objReportSetController = new ReportSetController();
			
			objReportList = objReportSetController.GetReportSetReport(ReportSetId);
			// save Report count
			_ReportCount = objReportList.Count;
			
			// bind
			dgReport.DataSource = objReportList;
			dgReport.DataBind();
			
			// commands
			cmdAddReport.Visible = ReportSetId > -1;
		}
		
#endregion
		
#region  Data Parameter
		private void DeleteParameter(int ParameterId)
		{
			ParameterController objParameterController = new ParameterController();
			objParameterController.DeleteParameter(ParameterId);
		}
		
		private void MoveParameter(int ParameterId, int Increment)
		{
			ParameterController objParameterController = new ParameterController();
			objParameterController.UpdateParameterOrder(ParameterId, Increment);
		}
		
		private void CopyParameter(int ParameterId)
		{
			ParameterController objParameterController = new ParameterController();
			ParameterInfo objParameter = objParameterController.GetParameter(ParameterId);
			int NewParameterId = 0;
			NewParameterId = objParameterController.UpdateParameter(ReportSetId, -1, "Copy of " + objParameter.ParameterName, objParameter.ParameterCaption, objParameter.ParameterTypeId, objParameter.ParameterConfig, -1);
			
		}
		private void BindParameter()
		{
			
			Localization.LocalizeDataGrid(ref dgParameter, LocalResourceFile);
			
			ArrayList objParameterList = default(ArrayList);
			ReportSetController objReportSetController = new ReportSetController();
			
			objParameterList = objReportSetController.GetReportSetParameter(ReportSetId);
			// save parameter count
			_ParameterCount = objParameterList.Count;
			
			// bind
			dgParameter.DataSource = objParameterList;
			dgParameter.DataBind();
			
			// commands
			cmdAddParameter.Visible = ReportSetId > -1;
			
		}
#endregion
		
#region  Navigation
		protected void cmdUpdate_Click(System.Object sender, System.EventArgs e)
		{
			if (Page.IsValid)
			{
				
				SaveReportSet();
				
				NavigateBack();
			}
			
		}
		
		protected void cmdCancel_Click(System.Object sender, System.EventArgs e)
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
		
		private string NavigateReportSet(int ReportSetId)
		{
			return EditUrl("ReportSetId", ReportSetId.ToString(), "EditReportSet");
		}
		
		private string NavigateReport(int ReportSetId, int ReportId)
		{
			return EditUrl("ReportId", ReportId.ToString(), "EditReport", string.Format("ReportSetId={0}", ReportSetId));
		}
		
		private string NavigateParameter(int ReportSetId, int ParameterId)
		{
			return EditUrl("ParameterId", ParameterId.ToString(), "EditParameter", string.Format("ReportSetId={0}", ReportSetId));
		}
#endregion
		
#region  Report Grid
		protected void cmdAddReport_Click(object sender, System.EventArgs e)
		{
			Response.Redirect(NavigateReport(ReportSetId, -1));
		}
		
		private void dgReport_ItemCommand(object source, DataGridCommandEventArgs e)
		{
			int ReportId = int.Parse(dgReport.DataKeys[e.Item.ItemIndex].ToString());
			switch (e.CommandName.ToLower())
			{
				case "edit":
					Response.Redirect(NavigateReport(ReportSetId, ReportId));
					break;
				case "delete":
					DeleteReport(ReportId);
					BindReport();
					break;
				case "up":
					MoveReport(ReportId, -1);
					BindReport();
					break;
				case "down":
					MoveReport(ReportId, 1);
					BindReport();
					break;
				case "copy":
					CopyReport(ReportId);
					BindReport();
					break;
			}
		}
		
		private void dgReport_ItemCreated(object sender, DataGridItemEventArgs e)
		{
			// process data rows only (skip the header, footer etc.)
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				// get a reference to the LinkButton of this row,
				//  and add the javascript confirmation
				LinkButton lnkDelete = (LinkButton) (e.Item.FindControl("cmdDeleteReport"));
				if (lnkDelete != null)
				{
					lnkDelete.Attributes.Add("onclick", "return confirm(\'Are you sure you want to delete this report?\');");
				}
			}
		}
		
		private void dgReport_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			ImageButton ib = default(ImageButton);
			if (e.Item.ItemType == ListItemType.Item | e.Item.ItemType == ListItemType.AlternatingItem)
			{
				if (e.Item.ItemIndex == 0)
				{
					ib = (ImageButton) (e.Item.FindControl("cmdMoveReportUp"));
					if (ib != null)
					{
						ib.Visible = false;
					}
				}
				else
				{
					if (e.Item.ItemIndex == _ReportCount - 1)
					{
						ib = (ImageButton) (e.Item.FindControl("cmdMoveReportDown"));
						if (ib != null)
						{
							ib.Visible = false;
						}
					}
					
				}
			}
		}
		
#endregion
		
#region  Parameter Grid
		protected void cmdAddParameter_Click(object sender, System.EventArgs e)
		{
			Response.Redirect(NavigateParameter(ReportSetId, -1));
		}
		
		private void dgParameter_ItemCommand(object source, DataGridCommandEventArgs e)
		{
			int ParameterId = int.Parse(dgParameter.DataKeys[e.Item.ItemIndex].ToString());
			switch (e.CommandName.ToLower())
			{
				case "edit":
					Response.Redirect(NavigateParameter(ReportSetId, ParameterId));
					break;
				case "delete":
					DeleteParameter(ParameterId);
					BindParameter();
					break;
				case "up":
					MoveParameter(ParameterId, -1);
					BindParameter();
					break;
				case "down":
					MoveParameter(ParameterId, 1);
					BindParameter();
					break;
				case "copy":
					CopyParameter(ParameterId);
					BindParameter();
					break;
					
			}
		}
		private void dgParameter_ItemCreated(object sender, DataGridItemEventArgs e)
		{
			// process data rows only (skip the header, footer etc.)
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				// get a reference to the LinkButton of this row,
				//  and add the javascript confirmation
				LinkButton lnkDelete = (LinkButton) (e.Item.FindControl("cmdDeleteParameter"));
				if (lnkDelete != null)
				{
					lnkDelete.Attributes.Add("onclick", "return confirm(\'Are you sure you want to delete this parameter?\');");
				}
			}
			
		}
		
		private void dgParameter_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			ImageButton ib = default(ImageButton);
			if (e.Item.ItemType == ListItemType.Item | e.Item.ItemType == ListItemType.AlternatingItem)
			{
				if (e.Item.ItemIndex == 0)
				{
					ib = (ImageButton) (e.Item.FindControl("cmdMoveParameterUp"));
					if (ib != null)
					{
						ib.Visible = false;
					}
				}
				else
				{
					if (e.Item.ItemIndex == _ParameterCount - 1)
					{
						ib = (ImageButton) (e.Item.FindControl("cmdMoveParameterDown"));
						if (ib != null)
						{
							ib.Visible = false;
						}
					}
					
				}
			}
		}
		
#endregion
		
	}
	
	
	
}

