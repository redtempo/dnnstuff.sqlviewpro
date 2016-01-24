using System.Xml.Serialization;
using DNNStuff.SQLViewPro.Controls;

namespace DNNStuff.SQLViewPro.StandardReports
{
	
	public partial class TemplateReportSettingsControl : ReportSettingsControlBase
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
				return ResolveUrl("App_LocalResources/TemplateReportSettingsControl");
			}
		}
		
		public override string UpdateSettings()
		{
			int PageSize = 5;
			TemplateReportSettings obj = new TemplateReportSettings();
			obj.TemplateText = txtTemplateText.Text;
			obj.AllowPaging = chkAllowPaging.Checked;
			obj.PagingType = ddPagingType.SelectedValue;
			if (int.TryParse(txtPageSize.Text, out PageSize))
			{
				obj.PageSize = PageSize;
			}
			else
			{
				obj.PageSize = 5;
			}
			obj.PrevPageText = txtPrevPageText.Text;
			obj.NextPageText = txtNextPageText.Text;
			obj.FirstPageText = txtFirstPageText.Text;
			obj.LastPageText = txtLastPageText.Text;
			obj.PageTemplate = txtPageTemplate.Text;
			
			return Serialization.SerializeObject(obj, typeof(TemplateReportSettings));
			
		}
		
		public override void LoadSettings(string settings)
		{
			TemplateReportSettings obj = new TemplateReportSettings();
			if (!string.IsNullOrEmpty(settings))
			{
				obj = (TemplateReportSettings) (Serialization.DeserializeObject(settings, typeof(TemplateReportSettings)));
			}
			
			txtTemplateText.Text = obj.TemplateText;
			chkAllowPaging.Checked = obj.AllowPaging;
			ddPagingType.SelectedValue = obj.PagingType;
			txtPageSize.Text = obj.PageSize.ToString();
			txtPrevPageText.Text = obj.PrevPageText;
			txtNextPageText.Text = obj.NextPageText;
			txtFirstPageText.Text = obj.FirstPageText;
			txtLastPageText.Text = obj.LastPageText;
			txtPageTemplate.Text = obj.PageTemplate;
		}
		
#endregion
		
	}
	
#region  Settings
	[XmlRootAttribute(ElementName = "Settings", IsNullable = false)]public class TemplateReportSettings
	{
		private string _TemplateText = "[EACHROW][StringCol][/EACHROW]";
		public string TemplateText
		{
			get
			{
				return _TemplateText;
			}
			set
			{
				_TemplateText = value;
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
		private string _PagingType = "Internal";
		public string PagingType
		{
			get
			{
				return _PagingType;
			}
			set
			{
				_PagingType = value;
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
		private string _FirstPageText = "First";
		public string FirstPageText
		{
			get
			{
				return _FirstPageText;
			}
			set
			{
				_FirstPageText = value;
			}
		}
		private string _LastPageText = "Last";
		public string LastPageText
		{
			get
			{
				return _LastPageText;
			}
			set
			{
				_LastPageText = value;
			}
		}
		private string _PageTemplate = "<a href=\"[URL]\">[TEXT]</a>";
		public string PageTemplate
		{
			get
			{
				return _PageTemplate;
			}
			set
			{
				_PageTemplate = value;
			}
		}
	}
#endregion
	
}

