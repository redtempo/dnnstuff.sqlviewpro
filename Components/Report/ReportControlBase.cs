using System.Collections;
using System.Web.UI;
using System.Data;
using System.Text;
using System.Web;

namespace DNNStuff.SQLViewPro.Controls
{
	public abstract class ReportControlBase : UserControl
	{
        public event OnDrillDownEventHandler OnDrillDown;
        public delegate void OnDrillDownEventHandler(object o, DrilldownEventArgs e);
		
#region Public Properties
		public string FullScreenUrl {get; set;}
		public DrilldownState State {get; set;}
		public ReportInfo Report {get; set;}
	    public StringBuilder DebugInfo { get; set; } = new StringBuilder();

	    public string QueryText
		{
			get
			{
				var s = Report.ReportCommand;
				if (s != "")
				{
					s = ReplaceReportTokens(s);
				}
				return s;
			}
		}
#endregion
		
#region  Public Methods
		/// <summary>
		/// Retrieve data for the given query
		/// </summary>
		/// <param name="query">The query to execute</param>
		/// <returns>DataSet containing all data</returns>
		/// <remarks></remarks>
		public DataSet ReportData(string query)
		{
			return Services.Data.Query.RetrieveData(query, Report.ReportConnectionString, Report.ReportCommandCacheTimeout, Report.ReportCommandCacheScheme);
		}
		
		/// <summary>
		/// Retrieves data for the report query
		/// </summary>
		/// <returns>DataSet containing all data</returns>
		/// <remarks></remarks>
		public DataSet ReportData()
		{
			return Services.Data.Query.RetrieveData(QueryText, Report.ReportConnectionString, Report.ReportCommandCacheTimeout, Report.ReportCommandCacheScheme);
		}
		
		/// <summary>
		/// Retrieves a unique string based on the given key
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		/// <remarks></remarks>
		public string Unique(string key)
		{
			return key + "_" + Report.ReportId.ToString();
		}
		
#endregion
		
#region Abstract Methods
		public abstract void LoadRuntimeSettings(ReportInfo settings);
#endregion
		
		public virtual string RenderHeaderAsText()
		{
			var s = new StringBuilder();
			if (Report.ReportHeaderText.Length > 0)
			{
				s.AppendFormat("<div class=\"{0}_Header\">{1}</div>", Report.ReportTheme, ReplaceReportTokens(Report.ReportHeaderText));
			}
			return s.ToString();
		}
		
		public virtual string RenderFooterAsText()
		{
			var s = new StringBuilder();
			if (Report.ReportFooterText.Length > 0)
			{
				s.AppendFormat("<div class=\"{0}_Footer\">{1}</div>", Report.ReportTheme, ReplaceReportTokens(Report.ReportFooterText));
			}
			return s.ToString();
		}
		
		public virtual void RenderNoItems()
		{
			if (Report.ReportNoItemsText.Length > 0)
			{
				// No Items
				var ctrl = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
				ctrl.InnerHtml = ReplaceReportTokens(Report.ReportNoItemsText);
				ctrl.Attributes.Add("class", Report.ReportTheme + "_NoItems");
				Controls.Add(ctrl);
			}
		}
		
		public virtual string RenderPageTitleAsText()
		{
			return ReplaceReportTokens(Report.ReportPageTitle);
		}
		
		public virtual string RenderMetaDescriptionAsText()
		{
			return ReplaceReportTokens(Report.ReportMetaDescription);
		}
		
#region Events
        protected void DrillDown(ReportControlBase o, DrilldownEventArgs e)
        {
            var onDrillDownEventHandler = OnDrillDown;
            if (onDrillDownEventHandler != null)
            {
                onDrillDownEventHandler(o, e);
            }
        }

        protected void Page_Init(object sender, System.EventArgs e)
		{
			var postbackControlId = "";
			if (Page.IsPostBack)
			{
				var postbackControl = ControlHelpers.GetPostBackControl(Page);
				if (postbackControl != null)
				{
					postbackControlId = postbackControl.ID;
				}
			}
			
			if (postbackControlId != "cmdBack")
			{
				if (Report.ReportPageTitle != "")
				{
					var thisPage = (DotNetNuke.Framework.CDefault) Page;
					thisPage.Title = RenderPageTitleAsText();
				}
				if (Report.ReportMetaDescription != "")
				{
					var thisPage = (DotNetNuke.Framework.CDefault) Page;
					var meta = RenderMetaDescriptionAsText();
					if (meta.Contains("<meta"))
					{
						thisPage.Header.Controls.Add(new LiteralControl(meta));
					}
					else
					{
						thisPage.Description = meta;
					}
				}
				
			}
		}

        protected void Page_PreRender(object sender, System.EventArgs e)
		{
			if (State.ReportSet.ReportSetDebug)
			{
				Controls.Add(RenderDebug(DebugInfo.ToString()));
			}
		}
#endregion
		
#region Token Replacement
		protected string ReplaceReportTokens(string text, DataSet ds = null)
		{
			return Services.Data.TokenReplacement.ReplaceTokens(text, ReportTokenSettings(), ds);
		}
		
		private Hashtable _reportTokens;
		private Hashtable ReportTokenSettings()
		{
			
			if (_reportTokens == null)
			{
				_reportTokens = (Hashtable) (new Hashtable());
				var fullScreenParameters = "";
				// now do parameters
				foreach (ParameterInfo param in State.Parameters)
				{
					var tokenValue = "";
					if (param.Values != null)
					{
						if (param.MultiValued)
						{
							tokenValue = "\'" + string.Join("\',\'", param.Values.ToArray()) + "\'";
						}
						else
						{
							tokenValue = (string) (param.Values[0].Replace("\'", "\'\'"));
						}
					}
					DNNUtilities.SafeHashtableAdd(ref _reportTokens, "PARAMETER:" + param.ParameterIdentifier.ToUpper(), tokenValue);
					fullScreenParameters = fullScreenParameters + string.Format("&{0}={1}", param.ParameterIdentifier, tokenValue);
					
					if (param.ExtraValues != null)
					{
						foreach (string key in param.ExtraValues.Keys)
						{
							DNNUtilities.SafeHashtableAdd(ref _reportTokens, "PARAMETER:" + param.ParameterIdentifier.ToUpper() + ":" + key.ToUpper(), param.ExtraValues[key]);
							fullScreenParameters = fullScreenParameters + string.Format("&{0}:{1}={2}", param.ParameterIdentifier, key, param.Values);
						}
					}
				}
				DNNUtilities.SafeHashtableAdd(ref _reportTokens, "REPORTNAME", Report.ReportName);
				DNNUtilities.SafeHashtableAdd(ref _reportTokens, "REPORTTYPE", Report.ReportTypeId);
				DNNUtilities.SafeHashtableAdd(ref _reportTokens, "REPORTTYPENAME", Report.ReportTypeName);
				if (Request != null)
				{
					DNNUtilities.SafeHashtableAdd(ref _reportTokens, "PAGEURL", Request.Url.AbsoluteUri);
				}
				DNNUtilities.SafeHashtableAdd(ref _reportTokens, "IMAGEURL", ResolveUrl("~/images"));
				DNNUtilities.SafeHashtableAdd(ref _reportTokens, "FULLSCREENURL", string.Format("{0}{1}&hidebackbutton=1", FullScreenUrl, fullScreenParameters));
			}
			
			return _reportTokens;
		}
		
		
#endregion
		
#region Debug
		private Control RenderDebug(string info)
		{
			return ParseControl(RenderDebugAsText(info));
		}
		
		private string RenderDebugAsText(string info)
		{
			var s = new StringBuilder();
			if (info.Length > 0)
			{
				s.AppendFormat("<div class=\"{0}_Debug\"><pre>{1}</pre></div>", Report.ReportTheme, info);
			}
			return s.ToString();
		}
#endregion
		
		
	}
}

