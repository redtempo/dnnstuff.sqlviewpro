
using DotNetNuke.Services.Localization;
using System;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.UI;

using System.Text;
using System.Collections.Generic;
using DNNStuff.SQLViewPro.Controls;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;



namespace DNNStuff.SQLViewPro
{
	
	public partial class SQLViewPro : PortalModuleBase, IActionable, IPortable
	{
		
		private const string CTRL_ACTION_BACK = "cmdBack";
		
		private const string RenderMode_Default = "Default";
		private const string RenderMode_Popup = "Popup";
		private const string RenderMode_NewWindow = "NewWindow";
		
		
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

			try
			{
				
				// PortalModuleControl base class settings for this module
				HelpURL = "http://www.dnnstuff.com"; // a URL for support on the module
				
				InitReportSet();
				
				if (ReportSet != null)
				{
					// initialize report sets and parameters
					InitParameters();
					RenderParameterArea();
				}
				else
				{
					HideAllAreas();
					Controls.Add(new LiteralControl("Module is not configured. Please create a new reportset or import an existing one from the repository."));
				}
				
			}
			catch (Exception ex)
			{
				DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
			}
			
		}


        protected LinkButton cmdAction;
		protected LinkButton cmdBack;
        #endregion

        #region  DNN Menus
        
        ModuleActionCollection IActionable.ModuleActions
        {
			get
			{
                var addedActions = new ModuleActionCollection();
				var action = default(ModuleAction);
				
				// add/edit report set
				if (ReportSet == null)
				{
					// new report set
					action = new ModuleAction(GetNextActionID());
					action.Title = Localization.GetString("Menu.NewReportSet", LocalResourceFile);
					action.CommandName = ModuleActionType.ContentOptions;
					action.Url = EditUrl("ReportSetId", "-1", "EditReportSet");
					action.Secure = SecurityAccessLevel.Edit;
					addedActions.Add(action);
				}
				else
				{
					action = new ModuleAction(GetNextActionID());
					action.Title = string.Format(Localization.GetString("Menu.EditCurrentReportSet", LocalResourceFile), ReportSet.ReportSetName);
					action.CommandName = ModuleActionType.ContentOptions;
					action.Url = EditUrl("ReportSetId", ReportSet.ReportSetId.ToString(), "EditReportSet");
					action.Secure = SecurityAccessLevel.Edit;
					addedActions.Add(action);
					
					foreach (ReportInfo r in Reports)
					{
						action = new ModuleAction(GetNextActionID());
						action.Title = string.Format(Localization.GetString("Menu.EditCurrentReport", LocalResourceFile), r.ReportName);
						action.CommandName = ModuleActionType.ContentOptions;
						action.Url = EditUrl("ReportSetId", ReportSet.ReportSetId.ToString(), "EditReport", "ReportId=" + r.ReportId.ToString());
						action.Secure = SecurityAccessLevel.Edit;
						addedActions.Add(action);
						
					}
					
					foreach (ParameterInfo p in Parameters)
					{
						action = new ModuleAction(GetNextActionID());
						action.Title = string.Format(Localization.GetString("Menu.EditCurrentParameter", LocalResourceFile), p.ParameterName);
						action.CommandName = ModuleActionType.ContentOptions;
						action.Url = EditUrl("ParameterId", p.ParameterId.ToString(), "EditParameter", string.Format("ReportSetId={0}", ReportSet.ReportSetId.ToString()));
						action.Secure = SecurityAccessLevel.Edit;
						addedActions.Add(action);
					}
					
				}
				
				// add break
				addedActions.Add(new ModuleAction(GetNextActionID(), "~", ""));
				
				// edit connections
				action = new ModuleAction(GetNextActionID());
				action.Title = Localization.GetString("Menu.EditConnections", LocalResourceFile);
				action.CommandName = ModuleActionType.ContentOptions;
				action.Url = EditUrl("EditConnections");
				action.Secure = SecurityAccessLevel.Edit;
				addedActions.Add(action);
				
				// repository
				action = new ModuleAction(GetNextActionID());
				action.Title = Localization.GetString("Menu.BrowseRepository", LocalResourceFile);
				action.CommandName = ModuleActionType.ContentOptions;
				action.Url = EditUrl("BrowseRepository");
				action.Secure = SecurityAccessLevel.Edit;
				addedActions.Add(action);
				
				// add break
				addedActions.Add(new ModuleAction(GetNextActionID(), "~", ""));
				
				return addedActions;
			}
		}
		
		public string ExportModule(int ModuleID)
		{
			// included as a stub only so that the core knows this module Implements Entities.Modules.IPortable
			return "";
		}
		
		public void ImportModule(int ModuleID, string Content, string Version, int UserId)
		{
			// included as a stub only so that the core knows this module Implements Entities.Modules.IPortable
		}
#endregion
		
#region  Page Properties
		// will have to set this dynamically
		private int _ReportSetId = -1;
		private ArrayList _Parameters;
		private ReportSetInfo _ReportSet;
		private ArrayList _Reports;
		private ArrayListStack _DrilldownStack = new ArrayListStack();
		
		// token settings
		private Hashtable _sqlviewproTokens;
		
		// effective module id
		private int _EffectiveModuleId = -1;
		public int EffectiveModuleId
		{
			get
			{
				if (_EffectiveModuleId == -1)
				{
					if (Request.QueryString["ModuleId"] != null)
					{
						_EffectiveModuleId = Convert.ToInt32(Request.QueryString["ModuleId"]);
					}
					else
					{
						_EffectiveModuleId = Convert.ToInt32(ModuleId);
					}
				}
				return _EffectiveModuleId;
			}
		}
		private int _EffectiveTabId = -1;
		public int EffectiveTabId
		{
			get
			{
				if (_EffectiveTabId == -1)
				{
					if (Request.QueryString["TabId"] != null)
					{
						_EffectiveTabId = Convert.ToInt32(Request.QueryString["TabId"]);
					}
					else
					{
						_EffectiveTabId = Convert.ToInt32(TabId);
					}
				}
				return _EffectiveTabId;
			}
		}
		
		public string DrilldownStackKey => "Drilldown$" + EffectiveModuleId.ToString();

	    public ReportSetInfo ReportSet
		{
			get
			{
				return _ReportSet;
			}
			set
			{
				_ReportSet = value;
			}
		}
		
		public ArrayList Reports
		{
			get
			{
				return _Reports;
			}
			set
			{
				_Reports = value;
			}
		}
		
		public ArrayList Parameters
		{
			get
			{
				return _Parameters;
			}
			set
			{
				_Parameters = value;
			}
		}



        #endregion

        #region  Page Load
        private void Page_Load(Object sender, EventArgs e)
		{
			try
			{
				DotNetNuke.Framework.jQuery.RequestRegistration();
				if (ReportSet != null)
				{
					if (ReportSet.RenderMode == RenderMode_Popup || ReportSet.RenderMode == RenderMode_NewWindow)
					{
						Page.ClientScript.RegisterClientScriptInclude(GetType(), "jquery.fancybox", ResolveUrl("resources/jQuery/FancyBox/jquery.fancybox-1.3.4.pack.js"));
						Page.ClientScript.RegisterClientScriptInclude(GetType(), "jquery.sqlviewpro.fullscreen", ResolveUrl("resources/jQuery/jquery.sqlviewpro.fullscreen.js"));
						Page.ClientScript.RegisterClientScriptBlock(GetType(), "sqlviewpro.object", string.Format("<script type=\"text/javascript\">sqlviewpro.fullScreenUrl = \'{0}\';</script>", FullscreenUrl()));
						DNNUtilities.InjectCSS(Page, ResolveUrl("resources/jQuery/fancybox/jquery.fancybox-1.3.4.css"));
					}
					RenderModule();
				}
			}
			catch (Exception ex)
			{
				DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
			}

            // event handlers
            if (cmdAction != null) cmdAction.Click += cmdAction_Click;
            if (cmdBack != null) cmdBack.Click += cmdBack_Click;
		}

        private void Page_Unload(object sender, EventArgs e)
		{
			if (ReportSet != null)
			{
				if (Session[DrilldownStackKey] == null)
				{
					Session.Add(DrilldownStackKey, _DrilldownStack);
				}
				else
				{
					Session[DrilldownStackKey] = _DrilldownStack;
				}
			}
		}
		
#endregion
		
#region  Action
		
		public void RefreshReport()
		{
			// going forward from parameters to first level report
			var state = new DrilldownState((ArrayList) (Parameters.Clone()));
			// clear panel
			pnlReportSet.Controls.Clear();
			_DrilldownStack.Clear();
			_DrilldownStack.Push(state);
			RenderPage();
		}
		
		private void PreviousReport()
		{
			// clear panel
			pnlReportSet.Controls.Clear();
			// going back a report
			_DrilldownStack.Pop();
			RenderPage();
		}
		
		protected void cmdAction_Click(Object sender, EventArgs e)
		{
			RefreshReport();
		}
		
		protected void cmdBack_Click(Object sender, EventArgs e)
		{
			PreviousReport();
		}
		
		private bool UrlBasedPagingActive()
		{
			return Request.QueryString["pg"] != null;
		}
		
		private void RenderModule()
		{
			if (ReportSet == null)
			{
				return;
			}
			
			try
			{
				RetrieveParameterValuesFromControls();
				
				if (!(Page.IsPostBack || UrlBasedPagingActive()))
				{
					if (CurrentlyInFullscreen())
					{
						RetrieveParameterValuesFromQueryString();
					}
					
					// first time shown
					_DrilldownStack.Clear();
					if ((NoParameters() && ReportSet.RenderMode != RenderMode_Popup) || (NoParameters() && ReportSet.RenderMode != RenderMode_NewWindow) || CurrentlyInFullscreen() || (ReportSet.AutoRun && !(ReportSet.RenderMode == RenderMode_Popup || ReportSet.RenderMode == RenderMode_NewWindow)))
					{
						var state = new DrilldownState((ArrayList) (Parameters.Clone()));
						_DrilldownStack.Push(state); // initial reports that aren't drilldowns
					}
				}
				
				RenderPage();
				
			}
			catch (Exception ex)
			{
				DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
			}
		}
		
		private void RenderPage()
		{
			if (_DrilldownStack.IsEmpty())
			{
				ShowParameterArea();
			}
			else
			{
				RenderReportSetArea();
				ShowReportSetArea();
			}
		}
		
#endregion
		
#region  ReportSet
		private void InitReportSet()
		{
			
			// init stack for drilldown
			if (Session[DrilldownStackKey] == null)
			{
				_DrilldownStack = new ArrayListStack();
			}
			else
			{
				_DrilldownStack = (ArrayListStack) (Session[DrilldownStackKey]);
			}
			
			var objReportSetController = new ReportSetController();
			_ReportSet = objReportSetController.GetReportSetByModule(EffectiveModuleId);
			if (_ReportSet != null)
			{
				if (_ReportSet.ReportSetTheme != "None")
				{
					DNNUtilities.InjectCSS(Page, ResolveUrl(string.Format("skins/{0}/styles.css", _ReportSet.ReportSetTheme)));
				}
				// get reports
				_Reports = objReportSetController.GetReportSetReport(ReportSet.ReportSetId);
			}
			
			
		}
		
		private void ShowReportSetArea()
		{
			if ((NoParameters() && FirstLevel()) || (ReportSet.AlwaysShowParameters && FirstLevel()))
			{
				if (cmdBack != null)
				{
					cmdBack.Visible = false;
				}
			}
			else
			{
				if (cmdBack != null)
				{
					cmdBack.Visible = true;
				}
			}
			pnlParameter.Visible = ReportSet.AlwaysShowParameters && !NoParameters();
			pnlReportSet.Visible = true;
		}
		
		private void RenderReportSetArea()
		{
			// set class of report set panel
			pnlReportSet.CssClass = _ReportSet.ReportSetTheme + "_ReportSet";
			
			Control c = null;
			
			c = ApplyReportLayout(BuildDefaultReportSetLayout());
			pnlReportSet.Controls.Add(c);
			
			// set action buttons
			var obj = FindControlRecursive(pnlReportSet, CTRL_ACTION_BACK);
			
			if (obj != null)
			{
				cmdBack = (LinkButton) obj;
			}
			
		}
		
		private string BuildDefaultReportSetLayout()
		{
			// build default reportset layout
			var sb = new StringBuilder();
			sb.Append(RenderBackButtonAsText());
			sb.Append(RenderReportContainerAsText());
			
			return sb.ToString();
		}
		
		private string RenderReportSetHeaderAsText()
		{
			var s = new StringBuilder();
			if (_ReportSet.ReportSetHeaderText.Length > 0)
			{
				s.AppendFormat("<div class=\"{0}_Header\">{1}</div>", _ReportSet.ReportSetTheme, ReplaceTokens(_ReportSet.ReportSetHeaderText));
			}
			return s.ToString();
		}
		
		private string RenderReportSetFooterAsText()
		{
			var s = new StringBuilder();
			if (_ReportSet.ReportSetFooterText.Length > 0)
			{
				s.AppendFormat("<div class=\"{0}_Footer\">{1}</div>", _ReportSet.ReportSetTheme, ReplaceTokens(_ReportSet.ReportSetFooterText));
			}
			return s.ToString();
		}
		
		private string RenderReportContainerAsText()
		{
			var s = new StringBuilder();
			s.AppendFormat("<div id=\"pnlReportContainer\" runat=\"server\" />");
			return s.ToString();
		}
		
		private string RenderBackButtonAsText()
		{
			// button
			if (HideBackButton() && FirstLevel())
			{
				return "";
			}
			return string.Format("<asp:LinkButton id=\"{1}\" runat=\"server\" CommandName=\"Goto\" cssClass=\"CommandButton SQLViewProButton\">{0}</asp:LinkButton>", BackCaption(), CTRL_ACTION_BACK);
		}
		
	
#endregion
		
#region  Report
		private Control ApplyReportLayout(string reportLayout)
		{
			
			var reportControls = ParseControl(reportLayout);
			
			var reportContainer = FindControlRecursive(reportControls, "pnlReportContainer");
			
			// state
			var state = (DrilldownState) (_DrilldownStack.Peek());
			state.PortalId = PortalId;
			state.ModuleId = EffectiveModuleId;
			state.TabId = TabId;
			state.UserId = UserId;
			state.ReportSet = ReportSet;

		    foreach (ReportInfo objReport in Reports)
			{
				
				if (objReport.ReportDrilldownReportId == state.FromReportId && objReport.ReportDrilldownFieldname == state.FromReportColumn) // show only if proper drilldown level
				{
					
					// override theme if nothing
					if (string.IsNullOrEmpty(objReport.ReportTheme))
					{
						objReport.ReportTheme = ReportSet.ReportSetTheme;
					}
					
					if (objReport.ReportTheme != "None")
					{
						DNNUtilities.InjectCSS(Page, ResolveUrl(string.Format("skins/{0}/styles.css", objReport.ReportTheme)));
					}
					
					// override connection string if necessary
					if (objReport.ReportConnectionId == (int) ConnectionType.ReportSetDefault)
					{
						objReport.ReportConnectionId = ReportSet.ReportSetConnectionId;
						objReport.ReportConnectionString = ReportSet.ReportSetConnectionString;
					}
					
					// add to page
					var objReportBase = (ReportControlBase) (LoadControl(ResolveUrl(objReport.ReportTypeControlSrc)));
					objReportBase.ID = "ReportBase" + objReport.ReportId.ToString();
					objReportBase.FullScreenUrl = FullscreenUrl();
					objReportBase.LoadRuntimeSettings(objReport);
					
					// handlers
                    objReportBase.OnDrillDown += OnDrilldown;

                    objReportBase.State = state;
					objReportBase.Report = objReport;
					
					// add report wrapper and then add this to report set wrapper
					var ctrl = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
					ctrl.Attributes.Add("class", objReport.ReportTheme + "_Report");
					
					var headerMarkup = objReportBase.RenderHeaderAsText();
					var footerMarkup = objReportBase.RenderFooterAsText();
					
					// header
					if (headerMarkup != "")
					{
						ctrl.Controls.Add(ParseControl(headerMarkup));
					}
					// report
					ctrl.Controls.Add(objReportBase);
					// footer
					if (footerMarkup != "")
					{
						ctrl.Controls.Add(ParseControl(footerMarkup));
					}
					
					
					reportContainer.Controls.Add(ctrl);
				}
				
			}
			
			return reportControls;
		}
		
#endregion
		
#region  Convenience
		private bool NoParameters()
		{
			return _Parameters.Count == 0;
		}

		private bool HasExcelReport()
		{
			foreach (ReportInfo objReport in Reports)
			{
				if (objReport.ReportTypeId == "EXCEL")
				{
					return true;
				}
			}
			return false;
		}
		private bool FirstLevel()
		{
			return _DrilldownStack.Count() == 1;
		}
		private string ActionCaption()
		{
			var caption = ReportSet.RunCaption;
			if (string.IsNullOrEmpty(caption))
			{
				caption = (string) (Localization.GetString("ActionRun.Text", LocalResourceFile));
			}
			if (string.IsNullOrEmpty(caption))
			{
				caption = "Run";
			}
			return caption;
		}
		private string BackCaption()
		{
			var caption = ReportSet.BackCaption;
			if (string.IsNullOrEmpty(caption))
			{
				caption = (string) (Localization.GetString("ActionBack.Text", LocalResourceFile));
			}
			if (string.IsNullOrEmpty(caption))
			{
				caption = "<- Back";
			}
			return caption;
		}
		
		private bool CurrentlyInFullscreen()
		{
			return Request.Url.AbsolutePath.EndsWith("Report.aspx");
		}
		
		private bool HideBackButton()
		{
			var value = (string) (Request.QueryString["hidebackbutton"]);
			if (value != null)
			{
				if (value == "0")
				{
					return false;
				}
				else
				{
					return true;
				}
			}
			else
			{
				return false;
			}
		}
		
		private string FullscreenUrl()
		{
			return ResolveUrl("Report.aspx") + "?moduleid=" + EffectiveModuleId.ToString() + "&tabid=" + EffectiveTabId.ToString();
		}
		
#endregion
		
#region  Parameter
		private void InitParameters()
		{
			// get report set
			var objReportSetController = new ReportSetController();
			// store parameters
			_Parameters = objReportSetController.GetReportSetParameter(ReportSet.ReportSetId);
			
		}
		
		private void HideAllAreas()
		{
			// used when report not configured at all
			pnlParameter.Visible = false;
			pnlReportSet.Visible = false;
		}
		
		private void ShowParameterArea()
		{
			pnlParameter.Visible = true;
			pnlReportSet.Visible = false;
		}
		
		private void RetrieveParameterValuesFromControls()
		{
			var ctrl = default(ParameterControlBase);
			foreach (ParameterInfo objParameter in Parameters)
			{
				
				ctrl = (ParameterControlBase) (pnlParameter.FindControl(objParameter.ParameterIdentifier));
				if (ctrl != null)
				{
					objParameter.Values = ctrl.Values;
					objParameter.ExtraValues = ctrl.ExtraValues;
					objParameter.MultiValued = Convert.ToBoolean(ctrl.MultiValued);
				}
			}
		}
		
		private void RetrieveParameterValuesFromQueryString()
		{
			var ctrl = default(ParameterControlBase);
			foreach (ParameterInfo objParameter in Parameters)
			{
				if (Request.QueryString[objParameter.ParameterIdentifier] != null)
				{
					ctrl = (ParameterControlBase) (pnlParameter.FindControl(objParameter.ParameterIdentifier));
					if (ctrl != null)
					{
						var val = (string) (Request.QueryString[objParameter.ParameterIdentifier]);
						var values = new List<string>();
						if (val.StartsWith("\'")) //multivalue list
						{
							// remove opening and closing single quote, convert ',' to ^, then split on ^
							values.AddRange(val.TrimStart(new char[] {'\''}).TrimEnd(new char[] {'\''}).Replace("\',\'", "^").Split(new char[] {'^'}, StringSplitOptions.None));
						}
						else
						{
							values.Add(val);
						}
						ctrl.Values = values;
						objParameter.Values = values;
					}
				}
			}
			
		}
		
		private void RenderParameterArea()
		{
			pnlParameter.CssClass = ReportSet.ReportSetTheme + "_Parameter";
			
			Control c = null;
			
			if (ReportSet.ParameterLayout != "")
			{
				c = ApplyParameterLayout(ReportSet.ParameterLayout);
			}
			else
			{
				c = ApplyParameterLayout(BuildDefaultParameterLayout());
			}
			
			pnlParameter.Controls.Add(c);
			
			// set action buttons (can be nothing if using full screen mode)
			cmdAction = (LinkButton) (FindControlRecursive(pnlParameter, "cmdAction"));
			
		}
		
		private string BuildDefaultParameterLayout()
		{
			// build default parameter layout
			var sb = new StringBuilder();
			foreach (ParameterInfo objParameter in Parameters)
			{
				sb.Append(RenderParameterAsText(objParameter));
			}
			return sb.ToString();
		}
		
		private Control ApplyParameterLayout(string parameterLayout)
		{
			
			// add reportset header
			parameterLayout = RenderReportSetHeaderAsText() + parameterLayout;
			
			// add action button if user hasn't
			if (!parameterLayout.Contains("[ACTIONBUTTON]"))
			{
				parameterLayout = parameterLayout + "[ACTIONBUTTON]";
			}
			
			parameterLayout = parameterLayout + RenderReportSetFooterAsText();
			
			// build parameter controls
			foreach (ParameterInfo objParameter in Parameters)
			{
				// caption
				if (parameterLayout.Contains("[" + objParameter.ParameterIdentifier + "_Caption]"))
				{
					parameterLayout = parameterLayout.Replace("[" + objParameter.ParameterIdentifier + "_Caption]", RenderLabelAsText(objParameter.ParameterCaption, ""));
				}
				
				// prompt
				if (parameterLayout.Contains("[" + objParameter.ParameterIdentifier + "_Prompt]"))
				{
					parameterLayout = parameterLayout.Replace("[" + objParameter.ParameterIdentifier + "_Prompt]", RenderPromptAsText(objParameter));
				}
				
				// caption & prompt
				if (parameterLayout.Contains("[" + objParameter.ParameterIdentifier + "]"))
				{
					parameterLayout = parameterLayout.Replace("[" + objParameter.ParameterIdentifier + "]", RenderParameterAsText(objParameter));
				}
				
			}
			// action button
			parameterLayout = parameterLayout.Replace("[ACTIONBUTTON]", RenderActionButtonAsText());
			
			// other tokens
			parameterLayout = ReplaceTokens(parameterLayout);
			
			// build parameter registration types
			var parameterRegistrations = "";
			foreach (string registrationValue in registeredParameterTypes.Values)
			{
				parameterRegistrations = parameterRegistrations + registrationValue;
			}
			// create controls
			var parameterControls = ParseControl(parameterRegistrations + parameterLayout);
			
			// load settings for each parameter
			foreach (ParameterInfo objParameter in Parameters)
			{
				var c = default(Control);
				c = FindControlRecursive(parameterControls, objParameter.ParameterIdentifier);
				if (c != null)
				{
					var baseParameter = (ParameterControlBase) c;
					baseParameter.Settings = objParameter;
					baseParameter.PortalSettings = PortalSettings;
					baseParameter.LoadRuntimeSettings();
					
					// handlers
				    baseParameter.OnRun += OnRun;

				}
			}
			
			return parameterControls;
		}
		
		System.Collections.Specialized.StringDictionary registeredParameterTypes = new System.Collections.Specialized.StringDictionary();
		private string RenderPromptAsText(ParameterInfo objParameter)
		{
			var registerControl = "";
			var parameterControl = string.Format("<svp:{0} id=\"{1}\" runat=\"server\" />", objParameter.ParameterTypeId.ToLower(), objParameter.ParameterIdentifier);
			if (!registeredParameterTypes.ContainsKey(objParameter.ParameterTypeName))
			{
				registerControl = string.Format("<%@ register tagprefix=\"svp\" tagname=\"{0}\" src=\"{1}\" %>", objParameter.ParameterTypeId.ToLower(), ResolveUrl(objParameter.ParameterTypeControlSrc));
				registeredParameterTypes.Add(objParameter.ParameterTypeName, registerControl);
			}
			return parameterControl;
		}
		
		private string RenderParameterAsText(ParameterInfo objParameter)
		{
			
			var sb = new StringBuilder();
			
			// label
			if (objParameter.ParameterCaption.Length > 0)
			{
				var lbl = RenderLabelAsText(objParameter.ParameterCaption);
				sb.Append(lbl);
				sb.Append("<br />");
			}
			
			// input
			var prompt = RenderPromptAsText(objParameter);
			sb.Append(prompt);
			sb.Append("<br />");
			
			return sb.ToString();
			
		}
		
		private string RenderLabelAsText(string caption, string suffix = " :")
		{
			// label
			return "<asp:label CssClass=\"Caption\">" + caption + suffix + "</asp:label>";
		}
		
		private string RenderActionButtonAsText()
		{
			if (ReportSet.RenderMode == RenderMode_Popup && !CurrentlyInFullscreen())
			{
				return RenderFullScreenActionButtonAsText();
			}
			if (ReportSet.RenderMode == RenderMode_NewWindow && !CurrentlyInFullscreen())
			{
				return RenderNewWindowActionButtonAsText();
			}
			return string.Format("<asp:LinkButton id=\"cmdAction\" runat=\"server\" cssClass=\"CommandButton SQLViewProButton\">{0}</asp:LinkButton>", ActionCaption());
		}
		
		private string RenderFullScreenActionButtonAsText()
		{
			return string.Format("<a class=\"commandbutton SQLViewProButton fullscreen\" href=\"{1}\">{0}</a>", ActionCaption(), FullscreenUrl());
		}
		
		private string RenderNewWindowActionButtonAsText()
		{
			return string.Format("<a class=\"commandbutton SQLViewProButton newwindow\" href=\"{1}\">{0}</a>", ActionCaption(), FullscreenUrl());
		}
		
		private Control FindControlRecursive(Control root, string searchId)
		{
			
			if (root.ID == searchId)
			{
				return root;
			}
			
			foreach (Control ctl in root.Controls)
			{
				var foundCtl = FindControlRecursive(ctl, searchId);
				
				if (foundCtl != null)
				{
					return foundCtl;
				}
			}
			return null;
		}
		
#endregion
		
#region  Parameter Events
		private void OnRun(Object sender)
		{
			RefreshReport();
		}
#endregion
		
#region  Drilldown
		private void OnDrilldown(Object sender, DrilldownEventArgs e)
		{
			var state = new DrilldownState(Convert.ToInt32(e.ReportId), (string) e.Name, (ArrayList) (Parameters.Clone()));
			
			// add datarow as parameters
			var columnIndex = 0;
			for (columnIndex = 0; columnIndex <= e.Value.ItemArray.Length - 1; columnIndex++)
			{
				var pi = new ParameterInfo();
				pi.ParameterName = (string) (e.Value.Table.Columns[columnIndex].ColumnName);
				pi.Values = new List<string>(new string[] {e.Value[columnIndex].ToString()});
				
				state.Parameters.Add(pi);
				
			}
			_DrilldownStack.Push(state);
			// clear panel
			pnlReportSet.Controls.Clear();
			// render again
			RenderPage();
		}
		
#endregion
		
#region  Token Replacement
		private string ReplaceTokens(string text)
		{
			
			return Services.Data.TokenReplacement.ReplaceTokens(text, TokenSettings());
			
		}
		
		private Hashtable TokenSettings()
		{
			
			if (_sqlviewproTokens != null)
			{
				return _sqlviewproTokens;
			}

		    var tokens = new Hashtable
		                 {
		                     {"MODULEID", EffectiveModuleId.ToString()},
		                     {"MODULEFOLDER", ControlPath.Remove(ControlPath.Length - 1)},  // remove last / character to be consistent with resolveurl
                             {"TABMODULEID", TabModuleId.ToString()},
		                     {"PAGEURL", Request.Url.AbsoluteUri},
		                     {"IMAGEURL", ResolveUrl("~/images")},
		                     {"CDATASTART", "<![CDATA["},
		                     {"CDATAEND", "]]>"}
		                 };

		    _sqlviewproTokens = tokens;
			return _sqlviewproTokens;
		}
		
#endregion
		
		
	}
	
}

