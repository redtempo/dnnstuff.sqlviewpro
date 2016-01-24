

using System;
using System.Data;
using System.Web.UI;

using System.Reflection;

//***************************************************************************/
//* FusionChartReportControl.ascx.vb
//*
//* Copyright (c) 2004 by DNNStuff.
//* All rights reserved.
//*
//* Date:        Sep/27/2008
//* Author:      Richard Edwards
//* Description: FusionChart Report
//*************/

namespace DNNStuff.SQLViewPro.StandardReports
{
	
	public partial class FusionChartReportControl : Controls.ReportControlBase
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
		
		private FusionChartReportSettings _ReportExtra = new FusionChartReportSettings();
		private FusionChartReportSettings ReportExtra
		{
			get
			{
				return _ReportExtra;
			}
			set
			{
				_ReportExtra = value;
			}
		}
		private string _reportScript;
		
#endregion
		
#region  Base Method Implementations
		public override void LoadRuntimeSettings(ReportInfo settings)
		{
			_ReportExtra = (FusionChartReportSettings) (Serialization.DeserializeObject(settings.ReportConfig, typeof(FusionChartReportSettings)));
		}
#endregion
		
#region  Chart
		private string GenerateColourSet(int numColors)
		{
			Random random = new Random();
			string s = "";
			
			for (int i = 0; i <= numColors - 1; i++)
			{
				s = s + string.Format("{0:X6}", random.Next(0x1000000)) + ",";
			}
			s = s + string.Format("{0:X6}", random.Next(0x1000000));
			
			return s;
		}
		
		private string ReportColorSet(int numColors)
		{
			string colorSet = ReportExtra.ColorSet;
			
			if (colorSet == "Custom")
			{
				colorSet = ReportExtra.CustomColorSet.Replace(" ", "");
			}
			
			if (colorSet == "")
			{
				colorSet = GenerateColourSet(numColors);
			}
			return colorSet;
		}
		
		private string GetChartProperties()
		{
			System.Text.StringBuilder s = new System.Text.StringBuilder();
			
			foreach (PropertyInfo prop in ReportExtra.GetType().GetProperties())
			{
				
				string expectedValue = "";
				
				object value = prop.GetValue(ReportExtra, null);
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
			Page.ClientScript.RegisterClientScriptInclude(GetType(), "FusionChart_JavaScript", ResolveUrl("Resources/FusionCharts.js"));
			
			Controls.Add(new LiteralControl(string.Format("<div id=\"{0}\"></div>", Unique("chartdiv"))));
			
			// debug
			if (State.ReportSet.ReportSetDebug)
			{
				DebugInfo.Append(QueryText);
			}
			
			DataSet ds = ReportData();
			
			System.Text.StringBuilder data = new System.Text.StringBuilder();
			data.AppendFormat("<graph {0} >", GetChartProperties());
			
			if (ds.Tables.Count > 1 && (ReportExtra.ChartType.StartsWith("MS") || ReportExtra.ChartType.StartsWith("Stacked")))
			{
				RenderMultiSeriesChartMultipleTable(ds, data);
			}
			else if (ds.Tables[0].Columns.Count > 1 && (ReportExtra.ChartType.StartsWith("MS") || ReportExtra.ChartType.StartsWith("Stacked")))
			{
				RenderMultiSeriesChartSingleTable(ds, data);
			}
			else
			{
				RenderSingleSeriesChart(ds, data);
			}
			
			data.Append("</graph>");
			
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("<script type=\"text/javascript\">");
			sb.Append("jQuery(document).ready(function () {");
			sb.AppendFormat("   var chart = new FusionCharts(\"{0}\", \"ChartId\", \"{1}\", \"{2}\");", ResolveUrl("Charts/FCF_" + ReportExtra.ChartType + ".swf"), ReportExtra.ChartWidth, ReportExtra.ChartHeight);
			sb.AppendFormat("   chart.setDataXML(\"{0}\");", data.ToString());
			sb.AppendFormat("   chart.setTransparent(\"{0}\");", "false");
			sb.AppendFormat("chart.render(\"{0}\");", Unique("chartdiv"));
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
			int colorIndex = 0;
			colors = ReportColorSet(ds.Tables[0].Columns.Count).Split(',');
			for (int columnIndex = 1; columnIndex <= ds.Tables[0].Columns.Count - 1; columnIndex++)
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
			int colorIndex = 0;
			colors = ReportColorSet(ds.Tables[0].Columns.Count).Split(',');
			for (int tableIndex = 0; tableIndex <= ds.Tables.Count - 1; tableIndex++)
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
			int colorIndex = 0;
			colors = ReportColorSet(ds.Tables[0].Rows.Count).Split(',');
			
			// single series
			foreach (DataRow dr in ds.Tables[0].Rows)
			{
				data.AppendFormat("<set name=\'{0}\' value=\'{1}\' color=\'{2}\' />", StringHelpers.XmlEncode((string) (dr[0].ToString())), StringHelpers.XmlEncode((string) (dr[1].ToString())), colors[colorIndex]);
				colorIndex++;
				if (colorIndex >= colors.Length)
				{
					colorIndex = 0;
				}
			}
		}
		
#endregion
		
		private void Page_PreRender1(object sender, EventArgs e)
		{
			Page.ClientScript.RegisterClientScriptBlock(GetType(), Unique("RenderFusionChart"), _reportScript);
		}
	}
	
}

