using System.Xml.Serialization;
using DNNStuff.SQLViewPro.Controls;

namespace DNNStuff.SQLViewPro.StandardReports
{
	
	public partial class GridReportSettingsControl : ReportSettingsControlBase
	{
		
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
		
	
#region  Base Method Implementations
		protected override string LocalResourceFile
		{
			get
			{
				return ResolveUrl("App_LocalResources/GridReportSettingsControl");
			}
		}
		
		public override string UpdateSettings()
		{
			var PageSize = 5;
			
			var obj = new GridReportSettings();
			obj.OrderBy = txtOrderBy.Text;
			obj.AllowSorting = chkAllowSorting.Checked;
			obj.AllowPaging = chkAllowPaging.Checked;
			if (int.TryParse(txtPageSize.Text, out PageSize))
			{
				obj.PageSize = PageSize;
			}
			else
			{
				obj.PageSize = 5;
			}
			obj.PagerMode = ddPagerMode.SelectedValue;
			obj.PagerPosition = ddPagerPosition.SelectedValue;
			obj.PrevPageText = txtPrevPageText.Text;
			obj.NextPageText = txtNextPageText.Text;
			obj.EnableExcelExport = chkEnableExcelExport.Checked;
			obj.ExcelExportButtonCaption = txtExcelExportButtonCaption.Text;
			obj.ExcelExportPosition = ddExcelExportPosition.SelectedValue;
			obj.HideColumnHeaders = chkHideColumnHeaders.Checked;
			obj.HideColumns = txtHideColumns.Text;
			
			return Serialization.SerializeObject(obj, typeof(GridReportSettings));
			
		}
		
		public override void LoadSettings(string settings)
		{
			var obj = new GridReportSettings();
			if (!string.IsNullOrEmpty(settings))
			{
				obj = (GridReportSettings) (Serialization.DeserializeObject(settings, typeof(GridReportSettings)));
			}
			
			txtOrderBy.Text = obj.OrderBy;
			chkAllowSorting.Checked = obj.AllowSorting;
			chkAllowPaging.Checked = obj.AllowPaging;
			txtPageSize.Text = obj.PageSize.ToString();
			ddPagerMode.SelectedValue = obj.PagerMode;
			ddPagerPosition.SelectedValue = obj.PagerPosition;
			txtPrevPageText.Text = obj.PrevPageText;
			txtNextPageText.Text = obj.NextPageText;
			chkEnableExcelExport.Checked = obj.EnableExcelExport;
			txtExcelExportButtonCaption.Text = obj.ExcelExportButtonCaption;
			ddExcelExportPosition.SelectedValue = obj.ExcelExportPosition;
			chkHideColumnHeaders.Checked = obj.HideColumnHeaders;
			txtHideColumns.Text = obj.HideColumns;
		}
		
		
#endregion
		
	}
	
#region  Settings
	[XmlRootAttribute(ElementName = "Settings", IsNullable = false)]public class GridReportSettings
	{
	    public bool AllowSorting { get; set; } = false;

	    public string OrderBy { get; set; } = "";

	    public bool AllowPaging { get; set; } = false;

	    public int PageSize { get; set; } = 10;

	    public string PagerMode { get; set; } = "";

	    public string PrevPageText { get; set; } = "Prev";

	    public string NextPageText { get; set; } = "Next";

	    public string PagerPosition { get; set; } = "Bottom";

	    public bool EnableExcelExport { get; set; } = false;

	    public string ExcelExportButtonCaption { get; set; } = "Excel";

	    public string ExcelExportPosition { get; set; } = "Bottom";

	    public bool HideColumnHeaders { get; set; } = false;

	    public string HideColumns { get; set; } = "";
	}
#endregion
	
}

