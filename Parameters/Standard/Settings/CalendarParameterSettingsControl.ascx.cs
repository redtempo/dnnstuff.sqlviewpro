using System.Xml.Serialization;
using DNNStuff.SQLViewPro.Controls;

namespace DNNStuff.SQLViewPro.StandardParameters
{
    public partial class CalendarParameterSettingsControl : ParameterSettingsControlBase
    {
        #region  Web Form Designer Generated Code

        //This call is required by the Web Form Designer.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
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

        protected override string LocalResourceFile => ResolveUrl("App_LocalResources/CalendarParameterSettingsControl")
            ;

        public override string UpdateSettings()
        {
            var obj = new CalendarParameterSettings
                      {
                          Default = txtDefault.Text,
                          DatabaseDateFormat = txtDatabaseDateFormat.Text
                      };

            return Serialization.SerializeObject(obj, typeof (CalendarParameterSettings));
        }

        public override void LoadSettings(string settings)
        {
            var obj = new CalendarParameterSettings();
            if (settings != null)
            {
                obj =
                    (CalendarParameterSettings)
                        (Serialization.DeserializeObject(settings, typeof (CalendarParameterSettings)));
            }
            txtDefault.Text = obj.Default;
            txtDatabaseDateFormat.Text = obj.DatabaseDateFormat;
        }

        #endregion
    }

    #region  Settings

    [XmlRootAttribute(ElementName = "Settings", IsNullable = false)]
    public class CalendarParameterSettings
    {
        public string Default { get; set; }
        public string DatabaseDateFormat { get; set; } = "yyyy-M-d";
    }

    #endregion
}