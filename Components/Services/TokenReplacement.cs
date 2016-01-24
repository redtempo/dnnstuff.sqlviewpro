

using System.Web;
using System.Collections;
using System.Data;


using System.Text.RegularExpressions;


namespace DNNStuff.SQLViewPro.Services.Data
{
	public class TokenReplacement
	{
		
		public static string ReplaceTokens(string text, Hashtable settings = null, DataSet ds = null)
		{
			Hashtable sharedSettings = default(Hashtable);
			
			// make a copy of settings
			if (settings == null)
			{
				sharedSettings = (Hashtable) (new Hashtable());
			}
			else
			{
				sharedSettings = (Hashtable) (settings.Clone());
			}
			
			// add querystring values
			System.Collections.Specialized.NameValueCollection qs = new System.Collections.Specialized.NameValueCollection(HttpContext.Current.Request.QueryString); // create a copy, some weird errors happening with url rewriters
			object keyval = default(object);
			foreach (string key in qs.Keys)
			{
				keyval = qs[key];
				if (key != null && keyval != null)
				{
					sharedSettings.Add("QS:" + key.ToUpper(), keyval.ToString().Replace("\'", "\'\'"));
					sharedSettings.Add("QUERYSTRING:" + key.ToUpper(), keyval.ToString().Replace("\'", "\'\'"));
				}
			}
			
			// add server variables
			System.Collections.Specialized.NameValueCollection sv = new System.Collections.Specialized.NameValueCollection(HttpContext.Current.Request.ServerVariables); // create a copy
			foreach (string key in sv.Keys)
			{
				keyval = sv[key];
				if (key != null && keyval != null)
				{
					sharedSettings.Add("SV:" + key.ToUpper(), keyval.ToString().Replace("\'", "\'\'"));
					sharedSettings.Add("SERVERVAR:" + key.ToUpper(), keyval.ToString().Replace("\'", "\'\'"));
				}
			}
			
			// add form variables
			System.Collections.Specialized.NameValueCollection fv = new System.Collections.Specialized.NameValueCollection(HttpContext.Current.Request.Form); // create a copy
			foreach (string key in fv.Keys)
			{
				keyval = fv[key];
				if (key != null && keyval != null)
				{
					sharedSettings.Add("FV:" + key.ToUpper(), keyval.ToString().Replace("\'", "\'\'"));
					sharedSettings.Add("FORMVAR:" + key.ToUpper(), keyval.ToString().Replace("\'", "\'\'"));
				}
			}
			
			// do dataset replacements (if necessary)
			if (ds != null)
			{
				Utilities.RegularExpression.DataSetTokenReplacement dataReplacer = new Utilities.RegularExpression.DataSetTokenReplacement(ds);
				text = (string) (dataReplacer.Replace(text));
			}
			
			// do logic replacements
			Utilities.RegularExpression.IfDefinedTokenReplacement logicReplacer = new Utilities.RegularExpression.IfDefinedTokenReplacement(sharedSettings);
			text = (string) (logicReplacer.Replace(text));
			
			// do settings replacements
			Utilities.RegularExpression.TokenReplacement replacer = new Utilities.RegularExpression.TokenReplacement(sharedSettings);
			replacer.ReplaceIfNotFound = false;
			text = (string) (replacer.Replace(text));
			
			// do generic replacements - DNN tokens
			text = Compatibility.ReplaceGenericTokens(text);
			
			// replace escaped characters
			text = ReplaceEscapedCharacters(text);
			
			return text;
			
		}
		
		public static string ReplaceEscapedCharacters(string text)
		{
			text = Regex.Replace(text, "0x5B", "[", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, "0x5D", "]", RegexOptions.IgnoreCase);
			
			return text;
		}
	}
	
}

