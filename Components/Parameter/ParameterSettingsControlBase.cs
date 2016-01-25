using System.Web.UI;

namespace DNNStuff.SQLViewPro.Controls
{
	public abstract class ParameterSettingsControlBase : UserControl
	{
		
#region Abstract Methods
		public abstract void LoadSettings(string settings);
		public abstract string UpdateSettings();
		protected abstract string LocalResourceFile {get;}
		public virtual bool CaptionRequired => true;

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
					var label = (DotNetNuke.UI.UserControls.LabelControl) c;
					var labelText = (string) (DotNetNuke.Services.Localization.Localization.GetString(label.ID + ".Text", LocalResourceFile));
					if (labelText == null)
					{
						labelText = label.ID.Replace("lbl", "");
					}
					var helpText = (string) (DotNetNuke.Services.Localization.Localization.GetString(label.ID + ".Help", LocalResourceFile));
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
