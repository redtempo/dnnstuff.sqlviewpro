using System.Collections.Generic;
using DNNStuff.SQLViewPro.Services.Data;

namespace DNNStuff.SQLViewPro.MobileParameters
{
	
	public partial class MobiscrollParameterControl : Controls.ParameterControlBase
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
			
			ScriptController.InjectjQueryLibary(Page, false, false);
			ScriptController.InjectCssReference(Page, "mobiscroll", ResolveUrl("Resources/mobiscroll-1.6.min.css"), true, ScriptController.CssInjectOrder.f_Last);
			ScriptController.InjectJsLibrary(Page, "mobiscroll_js", ResolveUrl("Resources/mobiscroll-1.6.min.js"), false, ScriptController.ScriptInjectOrder.e_Default);
			
			var sb = new System.Text.StringBuilder();
			sb.AppendLine("<script type=\"text/javascript\">");
			sb.AppendLine("$(document).ready(function () {");
			sb.AppendLine(string.Format("$(\'#{0}\').scroller({{ preset: \'{1}\' , theme: \'{2}\', mode: \'{3}\' }});", txtMobiscroll.ClientID, MobiscrollSettings().Preset, MobiscrollSettings().Theme, MobiscrollSettings().Mode));
			sb.AppendLine("});");
			sb.AppendLine("</script>");
			
			Page.ClientScript.RegisterClientScriptBlock(GetType(), Unique("Mobiscroll"), sb.ToString());
			
		}
		
#endregion
		
#region  Base Method Implementations
		
		public override List<string> Values
		{
			get
			{
				
				return new List<string>(new string[] {txtMobiscroll.Text});
			}
			
			set
			{
				if (value.Count > 0)
				{
					txtMobiscroll.Text = value[0].ToString();
				}
				else
				{
					txtMobiscroll.Text = "";
				}
			}
		}
		
		public override void LoadRuntimeSettings()
		{
			txtMobiscroll.Text = TokenReplacement.ReplaceTokens(MobiscrollSettings().Default, null, null);
		}
		
#endregion
		
		private MobiscrollParameterSettings MobiscrollSettings()
		{
			return ((MobiscrollParameterSettings) (Serialization.DeserializeObject(Settings.ParameterConfig, typeof(MobiscrollParameterSettings))));
		}
	}
	
}

