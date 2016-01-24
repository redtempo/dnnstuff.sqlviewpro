

using System;

using DotNetNuke.Entities.Modules;

//***************************************************************************/
//* EditConnection.ascx.vb
//*
//* Copyright (c) 2004 by DNNStuff.
//* All rights reserved.
//*
//* Date:        August 9, 2004
//* Author:      Richard Edwards
//* Description: Portal Module for editing setting for a Connection grid
//*************/


namespace DNNStuff.SQLViewPro
{
	
	public partial class EditConnection : PortalModuleBase
	{
		
		
		//standard
		
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
			
			// initialize
			if (Request.QueryString["ConnectionId"] != null)
			{
				ConnectionId = int.Parse(Request.QueryString["ConnectionId"].ToString());
			}
			else
			{
				ConnectionId = -1;
			}
			
			InitConnection();
			
		}
		
#endregion
		
#region  Page
		
		private int _ConnectionId;
		public int ConnectionId
		{
			get
			{
				return _ConnectionId;
			}
			set
			{
				_ConnectionId = value;
			}
		}
		private ConnectionInfo _Connection;
		public ConnectionInfo Connection
		{
			get
			{
				return _Connection;
			}
			set
			{
				_Connection = value;
			}
		}
		private void Page_Load(Object sender, EventArgs e)
		{
			DNNUtilities.InjectCSS(Page, ResolveUrl("Resources/Support/edit.css"));
			
			
			
			if (!Page.IsPostBack)
			{
				
				BindConnection();
				
			}
			
		}
		
#endregion
		
#region  Data
		private void InitConnection()
		{
			ConnectionInfo objConnection = ConnectionController.GetConnection(ConnectionId);
			
			// load from database
			Connection = objConnection;
		}
		private void BindConnection()
		{
			if (Connection != null)
			{
				txtName.Text = Connection.ConnectionName;
				txtConnectionString.Text = Connection.ConnectionString;
			}
			
			
		}
		
		private void SaveConnection()
		{
			ConnectionController objConnectionController = new ConnectionController();
			ConnectionId = objConnectionController.UpdateConnection(PortalId, ConnectionId, txtName.Text, txtConnectionString.Text);
		}
		
#endregion
		
#region  Navigation
		protected void cmdUpdate_Click(object sender, EventArgs e)
		{
			
			if (Page.IsValid)
			{
				SaveConnection();
				
				// Redirect back to the Connection set
				Response.Redirect(NavigateConnections());
			}
			
		}

        protected void cmdCancel_Click(object sender, EventArgs e)
		{
			Response.Redirect(NavigateConnections());
		}
		
		private string NavigateConnections()
		{
			return EditUrl("EditConnections");
		}
#endregion
		
		protected void vldConnectionStringValid_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
		{
			try
			{
				Services.Data.Query.TestConnection(args.Value);
				args.IsValid = true;
			}
			catch (System.Security.SecurityException ex)
			{
				vldConnectionStringValid.ErrorMessage = "Insufficient trust level to create OLEDB connections. You must configure DNN to use a lower trust level or add OLEDBPermission to the current trust level<br />" + "Connection string error: " + ex.Message;
				args.IsValid = false;
			}
			catch (Exception ex)
			{
				vldConnectionStringValid.ErrorMessage = "Connection string error: " + ex.Message;
				args.IsValid = false;
			}
		}
		
	}
	
}

