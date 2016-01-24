


using DotNetNuke.Security.Permissions;
	
	
	namespace DNNStuff.SQLViewPro
	{
		
		public partial class Report : System.Web.UI.Page
		{
			
			private int _ModuleId = -1;
		private int _TabId = -1;
		public int ModuleId
		{
			get
			{
				if (Request.QueryString["ModuleId"] != null)
				{
					_ModuleId = System.Convert.ToInt32(Request.QueryString["ModuleId"]);
				}
				return _ModuleId;
			}
		}
		public int TabId
		{
			get
			{
				if (Request.QueryString["TabId"] != null)
				{
					_TabId = System.Convert.ToInt32(Request.QueryString["TabId"]);
				}
				return _TabId;
			}
		}
		
		private void Report_Init(object sender, System.EventArgs e)
		{
			if (HasViewPermissions())
			{
				DotNetNuke.Entities.Modules.PortalModuleBase ctrl = default(DotNetNuke.Entities.Modules.PortalModuleBase);
				ctrl = (DotNetNuke.Entities.Modules.PortalModuleBase) (LoadControl(ResolveUrl("SQLViewPro.ascx")));
				phInject.Controls.Add(ctrl);
			}
			else
			{
				Response.Redirect((string) (DotNetNuke.Common.Globals.AccessDeniedURL()), true);
			}
		}
		
		private bool HasViewPermissions()
		{
			DotNetNuke.Entities.Modules.ModuleInfo mi = default(DotNetNuke.Entities.Modules.ModuleInfo);
			DotNetNuke.Entities.Modules.ModuleController mc = new DotNetNuke.Entities.Modules.ModuleController();
			mi = mc.GetModule(ModuleId, TabId);
			return ModulePermissionController.CanViewModule(mi);
		}
		
		private void form1_Load(object sender, System.EventArgs e)
		{
			DotNetNuke.Framework.jQuery.RegisterJQuery(Page);
		}
	}
}

