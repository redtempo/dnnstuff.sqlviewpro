using System;
using System.Data;
using System.Web.UI;

using System.Reflection;
using System.Web;
using Newtonsoft.Json;

namespace DNNStuff.SQLViewPro.StandardReports
{

    public partial class Html5ChartReportControl : Controls.ReportControlBase
    {

        #region  Web Form Designer Generated Code

        //This call is required by the Web Form Designer.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {

        }

        private void Page_Init(Object sender, EventArgs e)
        {
            base.Page_Init(sender, e);

            //CODEGEN: This method call is required by the Web Form Designer
            //Do not modify it using the code editor.
            InitializeComponent();

            RenderChart();

        }

        #endregion

        #region  Page

        private Html5ChartReportSettings ReportExtra { get; set; } = new Html5ChartReportSettings();

        private string _reportScript;

        #endregion

        #region  Base Method Implementations

        public override void LoadRuntimeSettings(ReportInfo settings)
        {
            ReportExtra =
                (Html5ChartReportSettings)
                    (Serialization.DeserializeObject(settings.ReportConfig, typeof (Html5ChartReportSettings)));
        }

        #endregion

        #region  Chart

        private void RenderChart()
        {
            Page.ClientScript.RegisterClientScriptInclude(GetType(), "Kendo_All_Javascript",
                ResolveUrl("KendoUI/js/kendo.all.min.js"));
            //Page.ClientScript.RegisterClientScriptInclude(GetType(), "Html5Chart_JavaScript", ResolveUrl("Resources/Html5Charts.js"));
            var ds = ReportData();
            var dsJson = JsonConvert.SerializeObject(ds, State.ReportSet.ReportSetDebug?Formatting.Indented:Formatting.None);

            var data = new
            {
                id = Unique("chartdiv"),
                x = ReportExtra,
                data = dsJson
            };

            // debug for query
            if (State.ReportSet.ReportSetDebug)
            {
                DebugInfo.Append(QueryText);
            }

            // control injection
            string ctrl =
                @"<div id='{{id}}' style='width:{{x.ChartWidth}}px; height:{{x.ChartHeight}}px'></div>";
            var ctrlTemplate = HandlebarsDotNet.Handlebars.Compile(ctrl);
            Controls.Add(new LiteralControl(ctrlTemplate(data)));

            // script injection
            string js = System.IO.File.ReadAllText(Server.MapPath(ResolveUrl("Resources/Html5Charts.template")));

            var jsTemplate = HandlebarsDotNet.Handlebars.Compile(js);

            _reportScript = jsTemplate(data);


            // debug for script
            if (State.ReportSet.ReportSetDebug)
            {
                DebugInfo.Append("<hr />");
                DebugInfo.Append(HttpUtility.HtmlEncode(_reportScript));
                DebugInfo.Append("<hr />");
                DebugInfo.Append(data);
            }

        }


        #endregion

        private void Page_PreRender(object sender, EventArgs e)
		{
            base.Page_PreRender(sender, e);

			Page.ClientScript.RegisterClientScriptBlock(GetType(), Unique("RenderHtml5Chart"), _reportScript, true);
		}
	}
	
}

