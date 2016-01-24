

using System.Web.UI.WebControls;
using System.Web.UI;




namespace DNNStuff
{
	
	public class ControlHelpers
	{
		public static Control GetPostBackControl(Page page)
		{
			
			
			Control postbackControlInstance = null;
			string postbackControlName = page.Request.Params.Get("__EVENTTARGET");
			
			if (postbackControlName != null && postbackControlName != string.Empty)
			{
				postbackControlInstance = FindControlRecursive(page, postbackControlName);
			}
			else
			{
				// handle the Button control postbacks
				for (int i = 0; i <= page.Request.Form.Keys.Count - 1; i++)
				{
					postbackControlInstance = FindControlRecursive(page, page.Request.Form.Keys[i]);
					if (postbackControlInstance is Button)
					{
						return postbackControlInstance;
					}
				}
			}
			
			// handle the ImageButton postbacks
			if (postbackControlInstance == null)
			{
				for (int i = 0; i <= page.Request.Form.Count - 1; i++)
				{
					if ((page.Request.Form.Keys[i].EndsWith(".x")) || (page.Request.Form.Keys[i].EndsWith(".y")))
					{
						postbackControlInstance = FindControlRecursive(page, (string) (page.Request.Form.Keys[i].Substring(0, System.Convert.ToInt32(page.Request.Form.Keys[i].Length - 2))));
						return postbackControlInstance;
					}
				}
			}
			
			return postbackControlInstance;
		}
		
		public static string GetPostBackControlName(Page page)
		{
			
			
			string postbackControlName = page.Request.Params.Get("__EVENTTARGET");
			if (string.IsNullOrEmpty(postbackControlName))
			{
				return "";
			}
			
			return StringHelpers.FindLastField(postbackControlName, '$');
			
		}
		
		public static Control FindControlRecursive(Control root, string id)
		{
			return root.FindControl(id);
		}
		
		public static void InitDropDownByValue(DropDownList c, string value)
		{
			ListItem li = c.Items.FindByValue(value);
			if (li != null)
			{
				li.Selected = true;
			}
		}
	}
}

