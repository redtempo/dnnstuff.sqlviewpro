using System;
using Microsoft.VisualBasic;
using System.Data;
using DotNetNuke.Security.Permissions;


namespace DNNStuff.SQLViewPro
{
    public partial class Export : System.Web.UI.Page
    {
        public const string EXPORT_KEY = "sqlviewpro_export";
        private int _ModuleId = -1;
        private int _TabId = -1;

        public int ModuleId
        {
            get
            {
                if (Request.QueryString["ModuleId"] != null)
                {
                    _ModuleId = Convert.ToInt32(Request.QueryString["ModuleId"]);
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
                    _TabId = Convert.ToInt32(Request.QueryString["TabId"]);
                }
                return _TabId;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (HasViewPermissions())
            {
                var details = (ExportDetails) (Session[EXPORT_KEY]);
                if (details != null)
                {
                    if (details.Data != null)
                    {
                        var ms = DataTableToExcel(details.Data.Tables[0]);
                        ExportToExcel(ms, details);
                    }
                    else if (details.Binary != null)
                    {
                        ExportToExcel(details.Binary, details);
                    }
                    else if (details.BinaryFilename.Length > 0)
                    {
                        using (var fs = System.IO.File.OpenRead(details.BinaryFilename))
                        {
                            var bytes = new byte[((int) (fs.Length - 1)) + 1];
                            fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                            fs.Close();
                            ExportToExcel(bytes, details);
                        }

                        try
                        {
                            System.IO.File.Delete(details.BinaryFilename);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    Session.Remove(EXPORT_KEY);
                }
            }
            else
            {
                Session.Remove(EXPORT_KEY);
                Response.Redirect((string) (DotNetNuke.Common.Globals.AccessDeniedURL()), true);
            }
        }

        private bool HasViewPermissions()
        {
            var mi = default(DotNetNuke.Entities.Modules.ModuleInfo);
            var mc = new DotNetNuke.Entities.Modules.ModuleController();
            mi = mc.GetModule(ModuleId, TabId);
            return ModulePermissionController.CanViewModule(mi);
        }

        private System.IO.StringWriter DataTableToExcel(DataTable dt)
        {
            var ms = new System.IO.StringWriter();

            //header/footer to support UTF-8 characters
            var header =
                "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">" +
                Constants.vbLf + "<html xmlns=\"http://www.w3.org/1999/xhtml\">" + Constants.vbLf + "<head>" +
                Constants.vbLf + "<title></title>" + Constants.vbLf +
                "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />" + Constants.vbLf + "<style>" +
                Constants.vbLf + "</style>" + Constants.vbLf + "</head>" + Constants.vbLf + "<body>" + Constants.vbLf;
            var footer = Constants.vbLf + "</body>" + Constants.vbLf + "</html>";

            ms.Write(header);
            //create an htmltextwriter which uses the stringwriter
            var htmlWrite = new System.Web.UI.HtmlTextWriter(ms);
            //instantiate a datagrid
            var dg = new System.Web.UI.WebControls.DataGrid();
            //set the datagrid datasource to the dataset passed in
            dg.DataSource = dt;
            //bind the datagrid
            dg.DataBind();
            //tell the datagrid to render itself to our htmltextwriter
            dg.RenderControl(htmlWrite);

            ms.Write(footer);

            return ms;
        }

        private void ExportInit(byte[] ba, ExportDetails details)
        {
            //first let's clean up the response.object
            Response.ClearHeaders();
            Response.Clear();
            Response.Buffer = true;
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Charset = "utf-8";
            Response.AddHeader("Content-Disposition",
                string.Format("{0}; filename=\"{1}\"", details.Disposition, details.Filename));

            // turn off cacheing
            //Response.CacheControl = "no-cache"
            //Response.AddHeader("Pragma", "no-cache")
            Response.Expires = -1;
        }

        private void ExportToExcel(byte[] ba, ExportDetails details)
        {
            // common initialize
            ExportInit(ba, details);

            //set the response mime type for excel
            if (details.Filename.EndsWith(".xlsx"))
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
            else
            {
                Response.ContentType = "application/vnd.ms-excel";
            }

            // now the data
            Response.BinaryWrite(ba);

            //all that's left is to output the html
            Response.End();
        }

        private void ExportToExcel(System.IO.StringWriter ms, ExportDetails details)
        {
            ExportToExcel(System.Text.Encoding.UTF8.GetBytes(ms.ToString()), details);
        }
    }

    [Serializable()]
    public class ExportDetails
    {
        public string Filename { get; set; }
        public string ContentType { get; set; }

        public string Disposition { get; set; } = "inline";
        public DataSet Data { get; set; } = null;
        public byte[] Binary { get; set; } = null;
        public string BinaryFilename { get; set; } = "";
    }
}