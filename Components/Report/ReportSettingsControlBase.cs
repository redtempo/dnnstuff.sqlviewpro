using System.Web.UI;

namespace DNNStuff.SQLViewPro.Controls
{
	public abstract class ReportSettingsControlBase : UserControl
	{
		
#region Abstract Methods
		public abstract void LoadSettings(string settings);
		public abstract string UpdateSettings();
		protected abstract string LocalResourceFile {get;}
		
#endregion
		protected void Page_Load(object sender, System.EventArgs e)
		{
			LoadLabelResources(this);
		}
		
		private void LoadLabelResources(Control root)
		{
			foreach (Control c in root.Controls)
			{
				if ((c) is DotNetNuke.UI.UserControls.LabelControl)
				{
					DotNetNuke.UI.UserControls.LabelControl label = (DotNetNuke.UI.UserControls.LabelControl) c;
					string labelText = (string) (DotNetNuke.Services.Localization.Localization.GetString(label.ID + ".Text", LocalResourceFile));
					if (labelText == null)
					{
						labelText = label.ID.Replace("lbl", "");
					}
					string helpText = (string) (DotNetNuke.Services.Localization.Localization.GetString(label.ID + ".Help", LocalResourceFile));
					if (helpText == null)
					{
						helpText = "Help not available for " + labelText;
					}
					label.Text = labelText;
					label.HelpText = helpText;
				}
				if (c.HasControls())
				{
					LoadLabelResources(c);
				}
			}
		}
	}
}
