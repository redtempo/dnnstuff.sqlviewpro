using System;
using System.Web;
using System.Collections;
using DotNetNuke.Entities.Portals;


namespace DNNStuff.SQLViewPro.Controls
{
    public partial class ConnectionPickerControl : System.Web.UI.UserControl
    {
        // public properties

        public int ConnectionId
        {
            get
            {
                if (ddlConnectionPicker.SelectedIndex > -1)
                {
                    return Convert.ToInt32(ddlConnectionPicker.SelectedValue);
                }
                return -1;
            }
            set
            {
                if (ddlConnectionPicker.Items.Count == 0)
                {
                    Data_Init();
                }
                if (ddlConnectionPicker.Items.FindByValue(value.ToString()) != null)
                {
                    ddlConnectionPicker.Items.FindByValue(value.ToString()).Selected = true;
                }
            }
        }

        public bool IncludePortalDefault { get; set; } = false;

        public bool IncludeReportSetDefault { get; set; } = false;

        #region  Web Form Designer Generated Code

        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
        }

        private void Page_Init(Object sender, EventArgs e)
        {
            //CODEGEN: This method call is required by the Web Form Designer
            //Do not modify it using the code editor.
            InitializeComponent();

            if (ddlConnectionPicker.Items.Count == 0)
            {
                Data_Init();
            }
        }

        #endregion

        private void Data_Init()
        {
            try
            {
                // Obtain PortalSettings from Current Context
                var portalSettings = (PortalSettings) (HttpContext.Current.Items["PortalSettings"]);

                var connectionController = new ConnectionController();
                var connectionList = connectionController.ListConnection(Convert.ToInt32(portalSettings.PortalId),
                    IncludePortalDefault, IncludeReportSetDefault);

                ddlConnectionPicker.DataTextField = "ConnectionName";
                ddlConnectionPicker.DataValueField = "ConnectionId";
                ddlConnectionPicker.DataSource = connectionList;
                ddlConnectionPicker.DataBind();
            }
            catch (Exception)
            {
            }
        }
    }
}