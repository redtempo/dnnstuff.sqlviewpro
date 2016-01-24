


using System.Xml.Serialization;
using DNNStuff.SQLViewPro.Controls;

//***************************************************************************/
//* GridReportSettings.ascx.vb
//*
//* Copyright (c) 2004 by DNNStuff.
//* All rights reserved.
//*
//* Date:        August 9, 2004
//* Author:      Richard Edwards
//* Description: Grid Report Settings Handler
//*************/


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
		
#region  Page
		private void Page_Load(System.Object sender, System.EventArgs e)
		{
			
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
			int PageSize = 5;
			
			GridReportSettings obj = new GridReportSettings();
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
			GridReportSettings obj = new GridReportSettings();
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
		private bool _AllowSorting = false;
		public bool AllowSorting
		{
			get
			{
				return _AllowSorting;
			}
			set
			{
				_AllowSorting = value;
			}
		}
		private string _OrderBy = "";
		public string OrderBy
		{
			get
			{
				return _OrderBy;
			}
			set
			{
				_OrderBy = value;
			}
		}
		private bool _AllowPaging = false;
		public bool AllowPaging
		{
			get
			{
				return _AllowPaging;
			}
			set
			{
				_AllowPaging = value;
			}
		}
		private int _PageSize = 10;
		public int PageSize
		{
			get
			{
				return _PageSize;
			}
			set
			{
				_PageSize = value;
			}
		}
		private string _PagerMode = "";
		public string PagerMode
		{
			get
			{
				return _PagerMode;
			}
			set
			{
				_PagerMode = value;
			}
		}
		private string _PrevPageText = "Prev";
		public string PrevPageText
		{
			get
			{
				return _PrevPageText;
			}
			set
			{
				_PrevPageText = value;
			}
		}
		private string _NextPageText = "Next";
		public string NextPageText
		{
			get
			{
				return _NextPageText;
			}
			set
			{
				_NextPageText = value;
			}
		}
		private string _PagerPosition = "Bottom";
		public string PagerPosition
		{
			get
			{
				return _PagerPosition;
			}
			set
			{
				_PagerPosition = value;
			}
		}
		private bool _EnableExcelExport = false;
		public bool EnableExcelExport
		{
			get
			{
				return _EnableExcelExport;
			}
			set
			{
				_EnableExcelExport = value;
			}
		}
		private string _ExcelExportButtonCaption = "Excel";
		public string ExcelExportButtonCaption
		{
			get
			{
				return _ExcelExportButtonCaption;
			}
			set
			{
				_ExcelExportButtonCaption = value;
			}
		}
		private string _ExcelExportPosition = "Bottom";
		public string ExcelExportPosition
		{
			get
			{
				return _ExcelExportPosition;
			}
			set
			{
				_ExcelExportPosition = value;
			}
		}
		private bool _HideColumnHeaders = false;
		public bool HideColumnHeaders
		{
			get
			{
				return _HideColumnHeaders;
			}
			set
			{
				_HideColumnHeaders = value;
			}
		}
		private string _HideColumns = "";
		public string HideColumns
		{
			get
			{
				return _HideColumns;
			}
			set
			{
				_HideColumns = value;
			}
		}
	}
#endregion
	
}

