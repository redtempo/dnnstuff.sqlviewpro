using System;
using System.Web.UI.WebControls;
using System.Xml;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Common;

namespace DNNStuff.SQLViewPro
{
	
	public partial class BrowseRepository : PortalModuleBase
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
			
			HelpURL = "http://www.dnnstuff.com/";
		}
		
#endregion
		
#region  Page Level
		
		
		private void Page_Load(Object sender, EventArgs e)
		{
			DNNUtilities.InjectCSS(Page, ResolveUrl("Resources/Support/edit.css"));
			
			
			try
			{
				cmdImport.Attributes.Add("onclick", "return confirm(\'Importing will overwrite the current reportset associated with this module. Are you sure?\');");
				
				if (Page.IsPostBack == false)
				{
					LoadSettings();
				}
				
			}
			catch (Exception exc) //Module failed to load
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}

		    cmdCancel.Click += cmdCancel_Click;
		    cmdImport.Click += cmdImport_Click;

		}
		
		protected void cmdCancel_Click(object sender, EventArgs e)
		{
			try
			{
				ReturnToPage();
			}
			catch (Exception exc) //Module failed to load
			{
                Exceptions.ProcessModuleLoadException(this, exc);
			}
		}
		
		protected void cmdImport_Click(object sender, EventArgs e)
		{
			try
			{
				if (Page.IsValid)
				{
					ImportTemplate();
					ReturnToPage();
				}
			}
			catch (Exception exc) //Module failed to load
			{
                Exceptions.ProcessModuleLoadException(this, exc);
			}
		}
		
		private void ReturnToPage()
		{
			
			// Redirect back to the portal home page
			Response.Redirect((string) (Globals.NavigateURL()), true);
			
		}
#endregion
		
#region  Process
		private void ImportTemplate()
		{
			ImportTemplate(cboRepository.SelectedValue);
		}
		
		private void ImportTemplate(string TemplateName)
		{
			
			var templateFile = new System.IO.FileInfo(System.IO.Path.Combine((string) (Server.MapPath(ResolveUrl("Repository"))), TemplateName));
			if (templateFile != null)
			{
				var xmlData = new XmlDocument();
				xmlData.Load(templateFile.FullName);
				var strType = xmlData.DocumentElement.GetAttribute("type").ToString();
				if (strType == StringHelpers.CleanName((string) ModuleConfiguration.DesktopModule.ModuleName) || strType == StringHelpers.CleanName((string) ModuleConfiguration.DesktopModule.FriendlyName))
				{
					var strVersion = xmlData.DocumentElement.GetAttribute("version").ToString();
					var ctrl = new SQLViewProController();
					ctrl.ImportModule(ModuleId, xmlData.DocumentElement.InnerXml, strVersion, UserId);
				}
			}
		}
		
#endregion
		
#region  Settings
		private void LoadSettings()
		{
			// repository
			BindRepository(cboRepository);
		}
		
		private void BindRepository(ListControl o)
		{
			var repositoryFolder = new System.IO.DirectoryInfo((string) (Server.MapPath(ResolveUrl("Repository"))));
			o.Items.Clear();
			foreach (var fi in repositoryFolder.GetFiles("content.*.xml"))
			{
				o.Items.Add(fi.Name);
			}
		}
		
#endregion
		
	}
	
	
}
