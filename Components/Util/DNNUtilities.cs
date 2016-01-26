

using System;
using System.Web;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI;


namespace DNNStuff.SQLViewPro
{
	public class DNNUtilities
	{
		public static string GetSetting(Hashtable settings, string key, string @default = "")
		{
			var ret = default(string);
			
			if (settings.ContainsKey(key))
			{
				try
				{
					ret = settings[key].ToString();
				}
				catch (Exception)
				{
					ret = @default;
				}
			}
			else
			{
				ret = @default;
			}
			return ret;
		}
		
		public static void SafeHashtableAdd(ref Hashtable ht, object key, object value)
		{
			// checks for existing entry and overwrites data otherwise adding
			if (ht.ContainsKey(key))
			{
				ht[key] = (Hashtable) value;
			}
			else
			{
				ht.Add(key, value);
			}
		}
		
		public static string QueryStringDefault(HttpRequest req, string parameterName, string defaultValue)
		{
			// check for querystring parameter and default if not found
			if (!(req.QueryString[parameterName] == null))
			{
				return req.QueryString[parameterName];
			}
			return defaultValue;
		}
		
		
#region  Skinning
		public static void InjectCSS(Page pg, string fileName)
		{
			
			// page style sheet reference
			var objCSS = pg.FindControl("CSS");
			if (objCSS == null)
			{
				// DNN 4 doesn't have CSS control any more, look for Head
				objCSS = pg.FindControl("Head");
			}
			
			// container stylesheet
			if (objCSS != null)
			{
				var CSSId = (string) (DotNetNuke.Common.Globals.CreateValidID(fileName));
				
				// container package style sheet
				if (objCSS.FindControl(CSSId) == null)
				{
					
					var objLink = new HtmlGenericControl("link");
					objLink.ID = CSSId;
					objLink.Attributes["rel"] = "stylesheet";
					objLink.Attributes["type"] = "text/css";
					objLink.Attributes["href"] = fileName;
					objCSS.Controls.Add(objLink);
				}
			}
			
		}
		
#endregion
		
#region version checking
		/// <summary>
		/// Returns a version-safe set of version numbers for DNN
		/// </summary>
		/// <param name="major">out int of the DNN Major version</param>
		/// <param name="minor">out int of the DNN Minor version</param>
		/// <param name="revision">out int of the DNN Revision</param>
		/// <param name="build">out int of the DNN Build version</param>
		/// <remarks>Dnn moved the version number during about the 4.9 version, which to me was a bit frustrating and caused the need for this reflection method call</remarks>
		/// <returns>true if it worked.</returns>
		public static bool SafeDNNVersion(int major, int minor, int revision, int build)
		{
			var ver = System.Reflection.Assembly.GetAssembly(typeof(DotNetNuke.Common.Globals)).GetName().Version;
			if (ver != null)
			{
				major = ver.Major;
				minor = ver.Minor;
				build = ver.Build;
				revision = ver.Revision;
				return true;
			}
			else
			{
				major = 0;
				minor = 0;
				build = 0;
				revision = 0;
				return false;
			}
		}
		
		public static Version SafeDNNVersion()
		{
			var ver = System.Reflection.Assembly.GetAssembly(typeof(DotNetNuke.Common.Globals)).GetName().Version;
			if (ver != null)
			{
				return ver;
			}
			return new Version(0, 0, 0, 0);
		}
#endregion
		
	}
}

