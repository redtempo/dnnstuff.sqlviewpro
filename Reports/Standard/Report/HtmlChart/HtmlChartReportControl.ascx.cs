using System;
using System.Data;
using System.Web.UI;

using System.Reflection;

namespace DNNStuff.SQLViewPro.StandardReports
{
	
	public partial class HtmlChartReportControl : Controls.ReportControlBase
	{
		
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
			
			RenderChart();
			
		}
		
		protected System.Web.UI.WebControls.PlaceHolder phChart;
		
#endregion
		
#region  Page

	    private HtmlChartReportSettings ReportExtra { get; set; } = new HtmlChartReportSettings();

	    private string _reportScript;
		
#endregion
		
#region  Base Method Implementations
		public override void LoadRuntimeSettings(ReportInfo settings)
		{
			ReportExtra = (HtmlChartReportSettings) (Serialization.DeserializeObject(settings.ReportConfig, typeof(HtmlChartReportSettings)));
		}
#endregion
		
#region  Chart
		private string GenerateColourSet(int numColors)
		{
			var random = new Random();
			var s = "";
			
			for (var i = 0; i <= numColors - 1; i++)
			{
				s = s + string.Format("{0:X6}", random.Next(0x1000000)) + ",";
			}
			s = s + string.Format("{0:X6}", random.Next(0x1000000));
			
			return s;
		}
		
		private string ReportColorSet(int numColors)
		{
			var colorSet = ReportExtra.ColorSet;
			
			if (colorSet == "Custom")
			{
				colorSet = ReportExtra.CustomColorSet.Replace(" ", "");
			}
			
			if (string.IsNullOrEmpty(colorSet))
			{
				colorSet = GenerateColourSet(numColors);
			}
			return colorSet;
		}
		
		private string GetChartProperties()
		{
			var s = new System.Text.StringBuilder();
			
			foreach (var prop in ReportExtra.GetType().GetProperties())
			{
				
				var expectedValue = "";
				
				var value = prop.GetValue(ReportExtra, null);
				if (value != null)
				{
					switch (prop.PropertyType.Name)
					{
						case "Boolean":
							expectedValue = ConvertBooleanToBit(Convert.ToBoolean(value)).ToString();
							break;
						default:
							expectedValue = value.ToString();
							break;
					}
					
					// only append if there is a value
					if (expectedValue.Length > 0)
					{
						s.AppendFormat("{0}=\'{1}\' ", prop.Name, StringHelpers.XmlEncode(ReplaceReportTokens(expectedValue)));
					}
					
				}
				
			}
			return s.ToString();
		}
		
		private int ConvertBooleanToBit(bool b)
		{
			// returns 0 for false, 1 for true
			return Convert.ToByte(b);
		}
		
		private void RenderChart()
		{
			Page.ClientScript.RegisterClientScriptInclude(GetType(), "HtmlChart_JavaScript", ResolveUrl("Resources/HtmlCharts.js"));
			
			Controls.Add(new LiteralControl($"<canvas id=\"{Unique("chartdiv")}\" width=\"{ReportExtra.ChartWidth}\" height=\"{ReportExtra.ChartHeight}\"></canvas>"));

            // debug
            if (State.ReportSet.ReportSetDebug)
			{
				DebugInfo.Append(QueryText);
			}
			
			var ds = ReportData();
			
			var data = new System.Text.StringBuilder();
            RenderSingleSeriesChart(ds, data);

            //data.AppendFormat("<graph {0} >", GetChartProperties());

            //if (ds.Tables.Count > 1 && (ReportExtra.ChartType.StartsWith("MS") || ReportExtra.ChartType.StartsWith("Stacked")))
            //{
            //	RenderMultiSeriesChartMultipleTable(ds, data);
            //}
            //else if (ds.Tables[0].Columns.Count > 1 && (ReportExtra.ChartType.StartsWith("MS") || ReportExtra.ChartType.StartsWith("Stacked")))
            //{
            //	RenderMultiSeriesChartSingleTable(ds, data);
            //}
            //else
            //{
            //	RenderSingleSeriesChart(ds, data);
            //}

            //data.Append("</graph>");

            var sb = new System.Text.StringBuilder();
			sb.Append("<script type=\"text/javascript\">");
			sb.Append("jQuery(document).ready(function () {");
            sb.Append($"var ctx = document.getElementById(\"{Unique("chartdiv")}\");");
            sb.Append("var chart = new Chart(ctx, {");
		    sb.Append("type: 'bar',");
		    sb.Append($"data: {data}");
            sb.Append("});");
            sb.Append("});");
			sb.Append("</script>");
			
			_reportScript = sb.ToString();
			
		}
		
		private void RenderMultiSeriesChartSingleTable(DataSet ds, System.Text.StringBuilder data)
		{
			// multi series
			data.AppendFormat("<categories>");
			foreach (DataRow dr in ds.Tables[0].Rows)
			{
				data.AppendFormat("<category name=\'{0}\' />", StringHelpers.XmlEncode((string) (dr[0].ToString())));
			}
			data.AppendFormat("</categories>");
			
			string[] colors = null;
			var colorIndex = 0;
			colors = ReportColorSet(ds.Tables[0].Columns.Count).Split(',');
			for (var columnIndex = 1; columnIndex <= ds.Tables[0].Columns.Count - 1; columnIndex++)
			{
				data.AppendFormat("<dataset seriesname=\'{0}\' color=\'{1}\'>", StringHelpers.XmlEncode(ds.Tables[0].Columns[columnIndex].ColumnName), colors[colorIndex]);
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					data.AppendFormat("<set value=\'{0}\' />", StringHelpers.XmlEncode((string) (dr[columnIndex].ToString())));
				}
				data.AppendFormat("</dataset>");
				colorIndex++;
				if (colorIndex >= colors.Length)
				{
					colorIndex = 0;
				}
			}
		}
		
		private void RenderMultiSeriesChartMultipleTable(DataSet ds, System.Text.StringBuilder data)
		{
			// multi series - get categories off first table
			data.AppendFormat("<categories>");
			foreach (DataRow dr in ds.Tables[0].Rows)
			{
				data.AppendFormat("<category name=\'{0}\' />", dr[0].ToString());
			}
			data.AppendFormat("</categories>");
			
			string[] colors = null;
			var colorIndex = 0;
			colors = ReportColorSet(ds.Tables[0].Columns.Count).Split(',');
			for (var tableIndex = 0; tableIndex <= ds.Tables.Count - 1; tableIndex++)
			{
				data.AppendFormat("<dataset seriesname=\'{0}\' color=\'{1}\'>", StringHelpers.XmlEncode(ds.Tables[tableIndex].Columns[1].ColumnName), colors[colorIndex]);
				foreach (DataRow dr in ds.Tables[tableIndex].Rows)
				{
					data.AppendFormat("<set value=\'{0}\' />", StringHelpers.XmlEncode((string) (dr[1].ToString())));
				}
				data.AppendFormat("</dataset>");
				colorIndex++;
				if (colorIndex >= colors.Length)
				{
					colorIndex = 0;
				}
			}
		}
		
		private void RenderSingleSeriesChart(DataSet ds, System.Text.StringBuilder data)
		{
			string[] colors = null;
			var colorIndex = 0;
			colors = ReportColorSet(ds.Tables[0].Rows.Count).Split(',');
			
			// single series
		    var dataLabels = "labels: [";
		    var dataPoints = "data: [";
		    var backgroundColors = "backgroundColor: [";
			foreach (DataRow dr in ds.Tables[0].Rows)
			{
			    dataLabels += "\"" + dr[0] + "\" ,";
			    dataPoints += dr[1] + ",";
			    backgroundColors += "'#" + colors[colorIndex] + "',";
				colorIndex++;
				if (colorIndex >= colors.Length)
				{
					colorIndex = 0;
				}
			}
		    dataLabels = dataLabels.TrimEnd(',') + "]";
            dataPoints = dataPoints.TrimEnd(',') + "]";
            backgroundColors = backgroundColors.TrimEnd(',') + "]";

		    data.Append("{");
		    data.Append(dataLabels + ",");
		    data.Append("datasets: [{");
		    data.Append("label: '# of Votes',");
		    data.Append(dataPoints + ",");
		    data.Append(backgroundColors);
		    data.Append("}]");
		    data.Append("}");
		}

        #endregion

        private void Page_PreRender(object sender, EventArgs e)
		{
			Page.ClientScript.RegisterClientScriptBlock(GetType(), Unique("RenderHtmlChart"), _reportScript);
		}
	}
	
}

