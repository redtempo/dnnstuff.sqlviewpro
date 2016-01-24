using System.Web.UI;




//***************************************************************************/
//* ParameterSettingsBase.vb
//*
//* COPYRIGHT (c) 2004-2005 by DNNStuff
//* ALL RIGHTS RESERVED.
//*
//* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//* TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//* THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
//* CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
//* DEALINGS IN THE SOFTWARE.
//*************/

namespace DNNStuff.SQLViewPro.Controls
{
	public abstract class ReportSettingsControlBase : UserControl
	{
		
#region Abstract Methods
		public abstract void LoadSettings(string settings);
		public abstract string UpdateSettings();
		protected abstract string LocalResourceFile {get;}
		
#endregion
		private void Page_Load(object sender, System.EventArgs e)
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
