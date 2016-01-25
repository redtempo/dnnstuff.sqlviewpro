using System;
using System.Data;
using System.Web.UI;
using System.IO;
using DotNetNuke.Common;
using FlexCel.Core;
using FlexCel.XlsAdapter;
	
		
		namespace DNNStuff.SQLViewPro.ExcelReports
		{
			
			public partial class ExcelTemplateReportControl : Controls.ReportControlBase
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
			
			try
			{
				if (Globals.IsEditMode())
				{
					Controls.Add(new LiteralControl("<strong>Please switch to view mode to generate Excel Template file</strong>"));
				}
				else
				{
					rdoExcelType.Visible = false;
					lblChoose.Visible = false;
					if (ReportExtra.XlsFileName.Length > 0 & ReportExtra.XlsxFileName.Length > 0)
					{
						rdoExcelType.Visible = true;
						lblChoose.Visible = true;
					}
					else if (ReportExtra.XlsFileName.Length > 0)
					{
						ProcessExcelTemplate(ReportExtra.XlsFileName);
					}
					else if (ReportExtra.XlsxFileName.Length > 0)
					{
						ProcessExcelTemplate(ReportExtra.XlsxFileName);
					}
				}
			}
			catch (Exception ex)
			{
				DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
			}
		}
		
		
#endregion
		
#region  Page

			    private ExcelTemplateReportSettings ReportExtra { get; set; } = new ExcelTemplateReportSettings();

			    #endregion
		
#region  Base Method Implementations
		public override void LoadRuntimeSettings(ReportInfo Settings)
		{
			ReportExtra = (ExcelTemplateReportSettings) (Serialization.DeserializeObject(Settings.ReportConfig, typeof(ExcelTemplateReportSettings)));
		}
#endregion
		
#region  Excel Template
		private void ProcessExcelTemplate(string fileId)
		{
			
			var ds = ReportData();
			
			// add debug info
			if (State.ReportSet.ReportSetDebug)
			{
				DebugInfo.Append(QueryText);
			}
			
			if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
			{
				RenderNoItems();
			}
			else
			{
				RenderExcelTemplate(ds.Tables[0], fileId);
			}
		}
		
		private void RenderExcelTemplate(DataTable dt, string fileId)
		{
			var dataPath = Server.MapPath(ResolveUrl("Templates"));
			var fileName = (string) (Services.File.GetFilenameFromFileId(fileId));
			
			var xls = new XlsFile(fileName);
			
			// save current sheet reference for later
			var activeSheetSaved = (string) xls.ActiveSheetByName;
			
			xls.ActiveSheetByName = ReportExtra.DataSheetName;
			// handle header row
			if (ReportExtra.ContainsHeaderRow)
			{
				// keep header row
				xls.DeleteRange(new TXlsCellRange(2, 1, FlxConsts.Max_Rows + 1, FlxConsts.Max_Columns + 1), TFlxInsertMode.NoneDown);
			}
			else
			{
				// clear sheet
				xls.ClearSheet();
				// headings
				for (var col = 0; col <= dt.Columns.Count - 1; col++)
				{
					xls.SetCellFromString(1, col + 1, dt.Columns[col].ColumnName);
				}
			}
			
			// update data
			for (var row = 0; row <= dt.Rows.Count - 1; row++)
			{
				for (var col = 0; col <= dt.Columns.Count - 1; col++)
				{
					xls.SetCellValue(row + 2, col + 1, dt.Rows[row][col]);
				}
			}
			
			xls.ActiveSheetByName = activeSheetSaved;
			
			// update range
			//Dim range As TXlsNamedRange = New TXlsNamedRange()
			//range.Name = "DEPTDATA"
			//range.RangeFormula = String.Format("='{0}'!$A$1:$H${1}", xls.ActiveSheetByName, dt.Rows.Count + 1)
			//xls.SetNamedRange(range)
			
			// determine file extension
			var fileExtension = "xls";
			if (fileName.EndsWith("xlsx"))
			{
				fileExtension = "xlsx";
			}
			
			// stream to user
			using (var ms = new MemoryStream())
			{
				xls.Save(ms);
				
				var details = new ExportDetails();
				details.Dataset = null;
				details.Filename = ReportExtra.OutputFileName.Replace("[TICKS]", DateTime.Now.Ticks.ToString()) + "." + fileExtension;
				details.Disposition = ReportExtra.DispositionType;
				
				// write tmp file
				var filePath = Server.MapPath(ResolveUrl(string.Format("{0}.dat", Guid.NewGuid().ToString())));
				var fs = File.OpenWrite(filePath);
				fs.Write(ms.GetBuffer(), 0, Convert.ToInt32(ms.Length));
				fs.Close();
				details.BinaryFilename = filePath;
				
				Session[Export.EXPORT_KEY] = details;
				
				if (Request.ServerVariables["HTTP_USER_AGENT"].Contains("ipad") || Request.ServerVariables["HTTP_USER_AGENT"].Contains("iphone"))
				{
					//' no iframe for iphone, ipad
					Response.Redirect(string.Format("{0}?ModuleId={1}&TabId={2}", ResolveUrl("~/DesktopModules/DNNStuff - SQLViewPro/Export.aspx"), State.ModuleId, State.TabId));
				}
				else
				{
					Controls.Add(new LiteralControl(string.Format("<iframe style=\'display:none\' scrolling=\'auto\' src=\'{0}?ModuleId={1}&TabId={2}\'></iframe>", ResolveUrl("~/DesktopModules/DNNStuff - SQLViewPro/Export.aspx"), State.ModuleId, State.TabId)));
				}
				
				
			}
			
			
		}
#endregion
		
		protected void rdoExcelType_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (rdoExcelType.SelectedValue)
			{
				case "xls":
					ProcessExcelTemplate(ReportExtra.XlsFileName);
					break;
				case "xlsx":
					ProcessExcelTemplate(ReportExtra.XlsxFileName);
					break;
			}
		}
	}
	
}

