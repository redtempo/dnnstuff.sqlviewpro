



using System.Text.RegularExpressions;


namespace DNNStuff.SQLViewPro
{
	
	public class Compatibility
	{
		// this module will provide compatibility between DNN versions
		public static string ReplaceGenericTokens(string text)
		{
			string ret = text;
			DotNetNuke.Services.Tokens.TokenReplace objTokenReplace = new DotNetNuke.Services.Tokens.TokenReplace();
			ret = (string) (objTokenReplace.ReplaceEnvironmentTokens(ret));
			return ret;
		}
		
		public static string ReplaceGenericTokensForTest(string text)
		{
			string ret = text;
			
			// replace tokens that aren't available
			ret = Regex.Replace(ret, "\\[QUERYSTRING:.*?\\]", "1", RegexOptions.IgnoreCase);
			ret = Regex.Replace(ret, "\\[QS:.*?\\]", "1", RegexOptions.IgnoreCase);
			// replace any parameter tokens named date with dates (crude workaround for the time being)
			ret = Regex.Replace(ret, "\\[PARAMETER:.*?DATE.*?\\]", "1966-2-21", RegexOptions.IgnoreCase);
			// replace rest of parameters
			ret = Regex.Replace(ret, "\\[PARAMETER:.*?\\]", "1", RegexOptions.IgnoreCase);
			
			DotNetNuke.Services.Tokens.TokenReplace objTokenReplace = new DotNetNuke.Services.Tokens.TokenReplace();
			ret = (string) (objTokenReplace.ReplaceEnvironmentTokens(ret));
			
			return ret;
		}
	}
	
}


