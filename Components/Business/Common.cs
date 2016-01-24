

using System.Web.UI;





namespace DNNStuff.SQLViewPro
{
	public class Common
	{
		// constants
		public const string CompanyName = "DNNStuff";
		public const string ProductName = "SQLViewPro";
		public const string CompanyUrl = "http://www.dnnstuff.com";
		public const string TrialStyle = "display:block;visibility:visible;color:black;position:relative;left:0;top:0;margin:0;padding:0;font:1.0em;line-height:1;";
		
		// standard menus
		public const string ViewOptions = "ViewOptions";
		
		/// <summary>
		/// TrialWarning - builds up the Trial warning inserted when using the Trial version of the module
		/// </summary>
		public static string TrialWarning()
		{
			
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.AppendFormat("<p>Thank you for evaluating <a style=\"text-decoration:underline\" target=\"_blank\" ", null);
			sb.AppendFormat("title=\"{0}\" ", ProductName);
			sb.AppendFormat("href=\"{0}/{2}.aspx?utm_source={1}&utm_medium=trial&utm_campaign={1}\">{2}</a>. ", CompanyUrl, CompanyName, ProductName);
			sb.AppendFormat("If after your evaluation you wish to support great DotNetNuke software and obtain a licensed copy of all DNNStuff modules, ", null);
			sb.AppendFormat("please visit the store to <a style=\"text-decoration:underline\" target=\"_blank\" ", null);
			sb.AppendFormat("title=\"{0}\" ", CompanyName);
			sb.AppendFormat("href=\"{0}/store.aspx?utm_source={1}&utm_medium=trial&utm_campaign={2}", CompanyUrl, CompanyName, ProductName);
			sb.AppendFormat("\">purchase a membership</a>. Use discount code <strong>\'TRIAL\'</strong> at checkout for 10% ", null);
			sb.AppendFormat("off!</p><hr />", null);
			
			return sb.ToString();
		}
		
		/// <summary>
		/// AddTrialNotice - returns a control containing the trial warning
		/// </summary>
		public static void AddTrialNotice(Control ParentControl)
		{
			
			System.Web.UI.HtmlControls.HtmlGenericControl ctrl = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
			ctrl.InnerHtml = TrialWarning();
			ctrl.Attributes.Add("style", TrialStyle);
			
			ParentControl.Controls.Add(ctrl);
			
		}
	}
	
}

