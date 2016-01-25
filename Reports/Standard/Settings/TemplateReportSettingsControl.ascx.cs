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
		
	
#region  Base Method Implementations
		protected override string LocalResourceFile => ResolveUrl("App_LocalResources/TemplateReportSettingsControl");

	    public override string UpdateSettings()
		{
			var PageSize = 5;
			var obj = new TemplateReportSettings();
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
			var obj = new TemplateReportSettings();
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
	    public string TemplateText { get; set; } = "[EACHROW][StringCol][/EACHROW]";

	    public bool AllowPaging { get; set; } = false;

	    public string PagingType { get; set; } = "Internal";

	    public int PageSize { get; set; } = 10;

	    public string PrevPageText { get; set; } = "Prev";

	    public string NextPageText { get; set; } = "Next";

	    public string FirstPageText { get; set; } = "First";

	    public string LastPageText { get; set; } = "Last";

	    public string PageTemplate { get; set; } = "<a href=\"[URL]\">[TEXT]</a>";
	}
#endregion
	
}

