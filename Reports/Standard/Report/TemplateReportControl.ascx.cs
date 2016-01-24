

using System;
using System.Data;
using System.Web.UI.WebControls;

using System.Text.RegularExpressions;
using DNNStuff.SQLViewPro.Controls;
using System.Linq;
using DotNetNuke.Common;

//***************************************************************************/
	//* XmlReportControl.ascx.vb
	//*
	//* Copyright (c) 2004 by DNNStuff.
	//* All rights reserved.
	//*
	//* Date:        August 9, 2004
	//* Author:      Richard Edwards
	//* Description: Xml Report
	//*************/
	
	namespace DNNStuff.SQLViewPro.StandardReports
	{
		
		public partial class TemplateReportControl : ReportControlBase
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
			
		}
		
		
#endregion
		
#region  Page
		private const string PAGINGTYPE_INTERNAL = "Internal";
		private const string PAGINGTYPE_QUERYSTRING = "Querystring";
		
		private TemplateReportSettings _ReportExtra = new TemplateReportSettings();
		private TemplateReportSettings ReportExtra
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
		
		private int PageNumber
		{
			get
			{
				int value = 1;
				if (ReportExtra.PagingType == PAGINGTYPE_INTERNAL)
				{
					if (ViewState["pageNumber"] != null)
					{
						value = Convert.ToInt32(ViewState["pageNumber"]);
					}
				}
				else
				{
					if (Request.QueryString["pg"] != null)
					{
						value = Convert.ToInt32(Request.QueryString["pg"]);
					}
				}
				return value;
			}
			set
			{
				if (ReportExtra.PagingType == PAGINGTYPE_INTERNAL)
				{
					ViewState["pageNumber"] = value;
				}
			}
		}
		
		private void Page_Load(object sender, EventArgs e)
		{
			try
			{
				BindTemplateToData();
				
			}
			catch (Exception ex)
			{
				DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
			}
		}
#endregion
		
#region  Base Method Implementations
		public override void LoadRuntimeSettings(ReportInfo settings)
		{
			_ReportExtra = (TemplateReportSettings) (Serialization.DeserializeObject(settings.ReportConfig, typeof(TemplateReportSettings)));
		}
#endregion
		
#region  Template
		
		private void BindTemplateToData()
		{
			phContent.Controls.Clear();
			
			// debug
			if (State.ReportSet.ReportSetDebug)
			{
				DebugInfo.Append(QueryText);
			}
			
			DataSet ds = ReportData();
			
			int maxPage = 1;
			if (ReportExtra.AllowPaging)
			{
				maxPage = Convert.ToInt32((ds.Tables[0].Rows.Count + ReportExtra.PageSize - 1) / ReportExtra.PageSize);
				System.Collections.Generic.List<DataRow> pageData = new System.Collections.Generic.List<DataRow>();
				for (int i = ((PageNumber - 1) * ReportExtra.PageSize + 1); i <= ((PageNumber) * ReportExtra.PageSize); i++)
				{
					if (i > ds.Tables[0].Rows.Count)
					{
						break;
					}
					pageData.Add(ds.Tables[0].Rows[i - 1]);
				}
				DataTable pageTable = default(DataTable);
				if (pageData.Any()) // check for data as CopyToDataTable errors if empty list
				{
					pageTable = pageData.CopyToDataTable();
				}
				else
				{
					pageTable = ds.Tables[0].Clone(); // just copy table structure
				}
				ds = new DataSet();
				ds.Tables.Add(pageTable);
			}
			
			if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
			{
				phContent.Visible = Report.ReportNoItemsText.Length == 0;
				RenderNoItems();
			}
			else
			{
				string result = ReportExtra.TemplateText;
				
				// TEMPORARY: until export services changed
				if (result.Contains("[EXPORT_EXCEL]"))
				{
					result = result.Replace("[EXPORT_EXCEL]", "");
					cmdExportExcel.Visible = true;
				}
				
				// paging support
				if (ReportExtra.AllowPaging)
				{
					string pagerPrevious = "";
					string pagerNext = "";
					string pagerFirst = "";
					string pagerLast = "";
					string pagerPages = "";
					
					// previous
					int prevPage = PageNumber - 1;
					if (prevPage > 0)
					{
						// insert previous page item
						if (ReportExtra.PagingType == PAGINGTYPE_INTERNAL)
						{
							pagerPrevious = "<asp:LinkButton id=\"cmdPreviousPage\" runat=\"server\" CommandArgument=\"" + prevPage.ToString() + "\">" + ReportExtra.PrevPageText + "</asp:LinkButton>";
						}
						else
						{
							pagerPrevious = PageTemplate(prevPage, ReportExtra.PrevPageText, "prev");
						}
					}
					// next
					int nextPage = PageNumber + 1;
					if (PageNumber < maxPage)
					{
						// insert next page
						if (ReportExtra.PagingType == PAGINGTYPE_INTERNAL)
						{
							pagerNext = "<asp:LinkButton id=\"cmdNextPage\" runat=\"server\" CommandArgument=\"" + nextPage.ToString() + "\">" + ReportExtra.NextPageText + "</asp:LinkButton>";
						}
						else
						{
							pagerNext = PageTemplate(nextPage, ReportExtra.NextPageText, "next");
						}
					}
					// first
					if (PageNumber > 1)
					{
						if (ReportExtra.PagingType == PAGINGTYPE_INTERNAL)
						{
							pagerFirst = "<asp:LinkButton id=\"cmdFirstPage\" runat=\"server\" CommandArgument=\"" + 1 + "\">" + ReportExtra.FirstPageText + "</asp:LinkButton>";
						}
						else
						{
							pagerFirst = PageTemplate(1, ReportExtra.FirstPageText, "first");
						}
					}
					// last
					if (PageNumber < maxPage)
					{
						if (ReportExtra.PagingType == PAGINGTYPE_INTERNAL)
						{
							pagerLast = "<asp:LinkButton id=\"cmdLastPage\" runat=\"server\" CommandArgument=\"" + maxPage.ToString() + "\">" + ReportExtra.LastPageText + "</asp:LinkButton>";
						}
						else
						{
							pagerLast = PageTemplate(maxPage, ReportExtra.LastPageText, "last");
						}
					}
					// pages
					if (ReportExtra.PagingType == PAGINGTYPE_QUERYSTRING)
					{
						for (int i = 1; i <= Math.Min(10, maxPage); i++)
						{
							pagerPages = pagerPages + PageTemplate(i, i.ToString(), "page");
						}
					}
					
					// check if no [PAGER*] tokens present, otherwise use a default
					if (!result.Contains("[PAGER"))
					{
						result = result + "[PAGER]";
					}
					
					// inject pager controls
					result = result.Replace("[PAGER]", "[PAGER:FIRST][PAGER:PREVIOUS][PAGER:PAGES][PAGER:NEXT][PAGER:LAST]");
					result = result.Replace("[PAGER:FIRST]", pagerFirst);
					result = result.Replace("[PAGER:LAST]", pagerLast);
					result = result.Replace("[PAGER:PREVIOUS]", pagerPrevious);
					result = result.Replace("[PAGER:NEXT]", pagerNext);
					result = result.Replace("[PAGER:PAGES]", pagerPages);
					result = result.Replace("[PAGER:NUMBER]", PageNumber.ToString());
					result = result.Replace("[PAGER:COUNT]", maxPage.ToString());
				}
				
				
				result = Regex.Replace(result, "<drilldown name=\\\"(?<name>.*)\\\">(?<inner>.*)</drilldown>", "<asp:LinkButton id=\"cmdDrilldown[#RowNumber]\" runat=\"server\" CommandName=\"Drilldown|${name}\" CommandArgument=\"[#RowNumber]\">${inner}</asp:LinkButton>", RegexOptions.IgnoreCase);
				result = ReplaceReportTokens(result, ds);
				phContent.Controls.Add(ParseControl(result));
				
				// hook up command for drilldown links
				LinkButton drilldownCtrl = default(LinkButton);
				int drilldownIndex = 1;
				while (true)
				{
					drilldownCtrl = (LinkButton) (phContent.FindControl("cmdDrillDown" + drilldownIndex.ToString()));
					if (drilldownCtrl != null)
					{
						drilldownCtrl.Command += new CommandEventHandler(cmdDrilldown);
						drilldownIndex++;
					}
					else
					{
						break;
					}
				}
				
				// hookup command for pager links for internal
				if (ReportExtra.PagingType == PAGINGTYPE_INTERNAL)
				{
					LinkButton cmdPreviousPage = default(LinkButton);
					cmdPreviousPage = (LinkButton) (phContent.FindControl("cmdPreviousPage"));
					if (cmdPreviousPage != null)
					{
						cmdPreviousPage.Command += new CommandEventHandler(Pager_Click);
					}
					LinkButton cmdNextPage = default(LinkButton);
					cmdNextPage = (LinkButton) (phContent.FindControl("cmdNextPage"));
					if (cmdNextPage != null)
					{
						cmdNextPage.Command += new CommandEventHandler(Pager_Click);
					}
					LinkButton cmdFirstPage = default(LinkButton);
					cmdFirstPage = (LinkButton) (phContent.FindControl("cmdFirstPage"));
					if (cmdFirstPage != null)
					{
						cmdFirstPage.Command += new CommandEventHandler(Pager_Click);
					}
					LinkButton cmdLastPage = default(LinkButton);
					cmdLastPage = (LinkButton) (phContent.FindControl("cmdLastPage"));
					if (cmdLastPage != null)
					{
						cmdLastPage.Command += new CommandEventHandler(Pager_Click);
					}
				}
			}
		}
		private string PageTemplate(int page, string text, string type)
		{
			return ReportExtra.PageTemplate.Replace("[URL]", (string) (Url.ReplaceQueryStringParam(Request.Url.AbsoluteUri, "pg", page.ToString()))).Replace("[TEXT]", text).Replace("[TYPE]", type);
		}
#endregion
		
#region Paging
		private void Pager_Click(object sender, EventArgs e)
		{
			LinkButton btn = (LinkButton) sender;
			if (btn != null)
			{
				PageNumber = int.Parse(btn.CommandArgument);
				BindTemplateToData();
			}
		}
#endregion
		
#region  Export
		protected void cmdExportExcel_Click(object sender, EventArgs e)
		{
			DataSet ds = ReportData();
			Services.Export.Excel.Export(ds.Tables[0], Response, Globals.CleanFileName(Report.ReportName + ".xls"));
		}
#endregion
		
		
#region  Drilldown
		private bool IsDrilldown()
		{
			return Report.ReportDrillDowns.Count > 0;
		}
		
		protected void cmdDrilldown(object source, CommandEventArgs e)
		{
			if (e.CommandName.StartsWith("Drilldown|"))
			{
				int row = int.Parse(e.CommandArgument.ToString());
				DataSet ds = ReportData();
				DataRow dr = ds.Tables[0].Rows[row - 1];
				
				DrillDown(this, new DrilldownEventArgs(Report.ReportId, e.CommandName.Replace("Drilldown|", ""), dr));
			}
		}
#endregion
		
	}
	
}

