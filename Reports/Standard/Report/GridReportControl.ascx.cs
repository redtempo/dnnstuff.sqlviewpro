using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using DNNStuff.SQLViewPro.Controls;
using DotNetNuke.Common;

namespace DNNStuff.SQLViewPro.StandardReports
{
	
	public partial class GridReportControl : ReportControlBase
	{
		
		protected LinkButton cmdExportExcel;
		
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
			
			ApplyGridFormatting();
			
		}
		
		
#endregion
		
#region  Page

	    private GridReportSettings ReportExtra { get; set; } = new GridReportSettings();

	    private void Page_Load(object sender, EventArgs e)
		{
			try
			{
				BindGridToData(); // needs to be in page load otherwise paging past first set of pages doesn't work
				
				// miscellaneous
				if (ReportExtra.EnableExcelExport)
				{
					RenderExcelButton();
				}
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
			ReportExtra = (GridReportSettings) (Serialization.DeserializeObject(settings.ReportConfig, typeof(GridReportSettings)));
		}
#endregion
#region Export
		private string RenderExcelExportButtonAsText()
		{
			return string.Format("<asp:LinkButton id=\"{1}\" runat=\"server\" CommandName=\"Goto\" CommandArgument=\"1\"  CssClass=\"CommandButton SQLViewProButton\">{0}</asp:LinkButton>", ReportExtra.ExcelExportButtonCaption, "cmdExportExcel");
		}
		
		private void RenderExcelButton()
		{
			Control c = null;
			Control pnl = null;
			
			c = ParseControl(RenderExcelExportButtonAsText());
			
			if (ReportExtra.ExcelExportPosition == "Top")
			{
				pnl = pnlHeader;
			}
			else
			{
				pnl = pnlFooter;
			}
			pnl.Controls.Add(c);
			
			// set action buttons
			var obj = Globals.FindControlRecursive(pnl, "cmdExportExcel");
			
			if (obj != null)
			{
				cmdExportExcel = (LinkButton) obj;
			    cmdExportExcel.Click += cmdExportExcel_Click;
			}
			
		}
		
		protected void cmdExportExcel_Click(object sender, EventArgs e)
		{
			var ds = ReportData();
			var details = new ExportDetails();
			details.Data = ds;
			details.Filename = (string) (Globals.CleanFileName(Report.ReportName + ".xls"));
			Session[Export.EXPORT_KEY] = details;
			Controls.Add(new LiteralControl(string.Format("<iframe style=\'display:none\' scrolling=\'auto\' src=\'{0}?ModuleId={1}&TabId={2}\'></iframe>", ResolveUrl("~/DesktopModules/DNNStuff - SQLViewPro/Export.aspx"), State.ModuleId, State.TabId)));
		}
		
#endregion
#region  Grid
		private void ApplyGridFormatting()
		{
			// format grid
			dgCommand.CssClass = Report.ReportTheme + "_Grid";
			dgCommand.HeaderStyle.CssClass = Report.ReportTheme + "_GridHeader";
			dgCommand.ItemStyle.CssClass = Report.ReportTheme + "_GridItem";
			dgCommand.AlternatingItemStyle.CssClass = Report.ReportTheme + "_GridAlternatingItem";
			
			// sorting
			dgCommand.AllowSorting = ReportExtra.AllowSorting;

            dgCommand.AllowPaging = ReportExtra.AllowPaging;
			if (dgCommand.AllowPaging)
			{
				dgCommand.PageSize = ReportExtra.PageSize;
				dgCommand.PagerStyle.Mode = (PagerMode) (Enum.Parse(typeof(PagerMode), ReportExtra.PagerMode));
				dgCommand.PagerStyle.NextPageText = ReportExtra.NextPageText;
				dgCommand.PagerStyle.PrevPageText = ReportExtra.PrevPageText;
				dgCommand.PagerStyle.CssClass = Report.ReportTheme + "_Pager";
				dgCommand.PagerStyle.Position = (PagerPosition) (Enum.Parse(typeof(PagerPosition), ReportExtra.PagerPosition));
			}
		}
		
		private void BindGridToData()
		{
			var gridQuery = GetQueryText();
			
			// debug
			if (State.ReportSet.ReportSetDebug)
			{
				DebugInfo.Append(gridQuery);
			}
			
			var ds = ReportData(gridQuery);
			
			if (dgCommand.Columns.Count == 0 && dgCommand.AutoGenerateColumns == false)
			{
				if (ds.Tables.Count > 0)
				{
					AddGridColumns(ds);
				}
			}
			
			if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
			{
				dgCommand.Visible = Report.ReportNoItemsText.Length == 0;
				RenderNoItems();
			}
			else
			{
				dgCommand.DataSource = null;
				dgCommand.DataSource = ds;
				dgCommand.DataBind();
			}
			
		}
		
		private void AddGridColumns(DataSet ds)
		{
			var hiddenColumns = ReportExtra.HideColumns.Split(',');
			
			foreach (DataColumn c in ds.Tables[0].Columns)
			{
				if (Array.IndexOf(hiddenColumns, c.ColumnName) < 0)
				{
					
					if (IsDrilldown() && IsDrilldownColumn(c.ColumnName))
					{
						var dgt = new TemplateColumn();
						dgt.ItemTemplate = new DrilldownTemplate(c.ColumnName);
						dgt.HeaderText = c.Caption;
						dgt.SortExpression = c.ColumnName;
						dgCommand.Columns.Add(dgt);
					}
					else
					{
						var dgc = new BoundColumn();
						dgc.DataField = c.ColumnName;
						dgc.HeaderText = c.Caption;
						dgc.SortExpression = c.ColumnName;
						dgCommand.Columns.Add(dgc);
					}
				}
				
			}
		}
		private string SortExpression
		{
			get
			{
			    var value = "";
			    if (ViewState["SortExpression"] != null)
			    {
                    value = ViewState["SortExpression"].ToString();
                    if (value == "")
                    {
                        value = (string)(ReportExtra.OrderBy.Replace(" DESC", "").Replace(" ASC", ""));
                    }

                }
                return value;
			}
			set
			{
				ViewState["SortExpression"] = value;
			}
		}
		private string SortDirection
		{
			get
			{
			    var value = "";
			    if (ViewState["SortDirection"] != null)
			    {
                    value = ViewState["SortDirection"].ToString();
                    if (value == "")
                    {
                        value = "ASC";
                        if (ReportExtra.OrderBy.Contains(" ASC"))
                        {
                            value = "ASC"; // NOTE: doing this step in case someone sorts by DESCRIPTION or something else with DESC in it and wants to specifically sort ascending
                        }
                        else if (ReportExtra.OrderBy.Contains(" DESC"))
                        {
                            value = "DESC";
                        }
                    }

                }
                return value;
			}
			set
			{
				ViewState["SortDirection"] = value;
			}
		}
		
		private string GetQueryText()
		{
			var query = QueryText;
			
			// bind data
			if (query != "")
			{
				if (dgCommand.AllowSorting)
				{
					if (query.Contains("[SORTEXPRESSION]") || query.Contains("[SORTDIRECTION]")) // handling in stored proc likely
					{
						query = (string) (query.Replace("[SORTEXPRESSION]", SortExpression).Replace("[SORTDIRECTION]", SortDirection));
					}
					else
					{
						if (SortExpression != "")
						{
							if (!query.Contains("ORDER BY"))
							{
								query = query + string.Format(" ORDER BY {0}{1}{2} {3}", Report.ReportIdentifierQuoteStartCharacter, SortExpression, Report.ReportIdentifierQuoteEndCharacter, SortDirection);
							}
						}
					}
				}
			}
			return query;
		}
		
		private void ToggleSort(string newSortExpression)
		{
			
			if (SortExpression.Equals(newSortExpression, StringComparison.InvariantCultureIgnoreCase))
			{
				if (SortDirection == "ASC")
				{
					SortDirection = "DESC";
				}
				else
				{
					SortDirection = "ASC";
				}
			}
			else
			{
				SortExpression = newSortExpression;
				SortDirection = "ASC";
			}
		}

        protected void SortCommand_OnClick(object source, DataGridSortCommandEventArgs e)
		{
			ToggleSort(e.SortExpression);
			BindGridToData();
		}

        protected void PageIndexChanged(object source, DataGridPageChangedEventArgs e)
		{
			dgCommand.CurrentPageIndex = e.NewPageIndex;
			BindGridToData();
		}
		
		protected void dgCommand_ItemCreated(object sender, DataGridItemEventArgs e)
		{
			// hide columns
			if (e.Item.ItemType == ListItemType.Header)
			{
				e.Item.Visible = !ReportExtra.HideColumnHeaders;
			}
		}
		
#endregion
		
#region  Drilldown
		
		private class DrilldownTemplate : ITemplate
		{
			private string _fieldName;
			
			public DrilldownTemplate(string fieldName)
			{
				_fieldName = fieldName;
			}
			
			public void InstantiateIn(Control container)
			{
				var linkbtn = new LinkButton();
				linkbtn.ID = "lbDrilldown" + _fieldName;
				linkbtn.DataBinding += new EventHandler(BindHyperLinkColumn);
				container.Controls.Add(linkbtn);
			}
			
			private void BindHyperLinkColumn(object sender, EventArgs e)
			{
				var linkbtn = (LinkButton) sender;
				var container = (DataGridItem) linkbtn.NamingContainer;
				
				linkbtn.CommandName = "Drilldown|" + _fieldName;
				linkbtn.CommandArgument = Convert.ToString(DataBinder.Eval(container.DataItem, _fieldName));
				linkbtn.Text = Convert.ToString(DataBinder.Eval(container.DataItem, _fieldName));
			}
		}
		
		protected void dgCommand_ItemCommand(object source, DataGridCommandEventArgs e)
		{
			if (e.CommandName.StartsWith("Drilldown|"))
			{
				var query = GetQueryText();
				var ds = ReportData(query);
				var dr = ds.Tables[0].Rows[e.Item.DataSetIndex];
				
				DrillDown(this, new DrilldownEventArgs(Report.ReportId, e.CommandName.Replace("Drilldown|", ""), dr));
			}
		}
		
		private bool IsDrilldown()
		{
			return Report.ReportDrillDowns.Count > 0;
		}
		
		private bool IsDrilldownColumn(string colName)
		{
			foreach (var obj in Report.ReportDrillDowns)
			{
				var ri = (ReportInfo) obj;
				if (ri.ReportDrilldownFieldname == colName)
				{
					return true;
				}
			}
			return false;
		}
		
		protected ReportInfo DrilldownReportByColumn(string colName)
		{
			foreach (var obj in Report.ReportDrillDowns)
			{
				var ri = (ReportInfo) obj;
				if (ri.ReportDrilldownFieldname == colName)
				{
					return ri;
				}
			}
			return null;
		}
#endregion
		
		
	}
	
}

