using DotNetNuke.Services.Localization;
using System.Web.UI.WebControls;
using System.Collections;
using DotNetNuke.Common;
using DotNetNuke.Entities.Modules;

namespace DNNStuff.SQLViewPro
{
	
	public partial class EditConnections : PortalModuleBase
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
			DNNUtilities.InjectCSS(Page, ResolveUrl("Resources/Support/edit.css"));
			
			
			
			if (!Page.IsPostBack)
			{
				BindConnection();
			}
			
		}
		
#endregion
		
#region  Data Connection
		// connection
		private void DeleteConnection(int ConnectionId)
		{
			ConnectionController objConnectionController = new ConnectionController();
			objConnectionController.DeleteConnection(ConnectionId);
		}
		
		private void BindConnection()
		{
			Localization.LocalizeDataGrid(ref dgConnection, LocalResourceFile);
			
			ArrayList objConnectionList = default(ArrayList);
			ConnectionController objConnectionController = new ConnectionController();
			
			objConnectionList = objConnectionController.ListConnection(PortalId, false, false);
			
			// bind
			dgConnection.DataSource = objConnectionList;
			dgConnection.DataBind();
			
		}
		
#endregion
		
		
#region  Navigation
		protected void cmdClose_Click(System.Object sender, System.EventArgs e)
		{
			Response.Redirect((string) (Globals.NavigateURL()));
		}
		
		private string NavigateConnection(int ConnectionId)
		{
			return EditUrl("ConnectionId", ConnectionId.ToString(), "EditConnection");
		}
		
#endregion
		
#region  Connection Grid
		protected void cmdAddConnection_Click(object sender, System.EventArgs e)
		{
			Response.Redirect(NavigateConnection(-1));
		}
		
		protected void dgConnection_ItemCommand(object source, DataGridCommandEventArgs e)
		{
			int ConnectionId = int.Parse(dgConnection.DataKeys[e.Item.ItemIndex].ToString());
			switch (e.CommandName.ToLower())
			{
				case "edit":
					Response.Redirect(NavigateConnection(ConnectionId));
					break;
				case "delete":
					DeleteConnection(ConnectionId);
					BindConnection();
					break;
			}
		}
		
		protected void dgConnection_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			// process data rows only (skip the header, footer etc.)
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				// get a reference to the LinkButton of this row,
				//  and add the javascript confirmation
				LinkButton lnkDelete = (LinkButton) (e.Item.FindControl("cmdDeleteConnection"));
				if (lnkDelete != null)
				{
					if (lnkDelete.Enabled)
					{
						lnkDelete.Attributes.Add("onclick", "return confirm(\'Are you sure you want to delete this connection?\');");
					}
					else
					{
						lnkDelete.Attributes.Add("title", "This connection is used by other objects. It cannot be deleted.");
						
					}
					
				}
			}
		}
#endregion
		
	}
	
}

