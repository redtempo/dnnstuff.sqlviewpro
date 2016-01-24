

using System;
using System.Web;




namespace DNNStuff
{
	public class Url
	{
		// <summary>
		// This method returns a fully qualified absolute server Url which includes
		// the protocol, server, port in addition to the server relative Url.
		//
		// Works like Control.ResolveUrl including support for ~ syntax
		// but returns an absolute URL.
		// </summary>
		// <param name="ServerUrl">Any Url, either App relative or fully qualified</param>
		// <param name="forceHttps">if true forces the url to use https</param>
		// <returns></returns>
		
		public static string ResolveServerUrl(string serverUrl, bool forceHttps)
		{
			
			// *** Is it already an absolute Url
			if (serverUrl.IndexOf("://") > -1)
			{
				return serverUrl;
			}
			
			// *** Start by fixing up the Url an Application relative Url
			string newUrl = ResolveUrl(serverUrl);
			
			Uri originalUri = HttpContext.Current.Request.Url;
			newUrl = ((forceHttps ? "https" : originalUri.Scheme) ) + "://" + originalUri.Authority + newUrl;
			return newUrl;
			
		}
		
		// <summary>
		// This method returns a fully qualified absolute server Url which includes
		// the protocol, server, port in addition to the server relative Url.
		//
		// It work like Page.ResolveUrl, but adds these to the beginning.
		// This method is useful for generating Urls for AJAX methods
		// </summary>
		// <param name="ServerUrl">Any Url, either App relative or fully qualified</param>
		// <returns></returns>
		
		public static string ResolveServerUrl(string serverUrl)
		{
			return ResolveServerUrl(serverUrl, false);
		}
		
		// <summary>
		// Returns a site relative HTTP path from a partial path starting out with a ~.
		// Same syntax that ASP.Net internally supports but this method can be used
		// outside of the Page framework.
		//
		// Works like Control.ResolveUrl including support for ~ syntax
		// but returns an absolute URL.
		// </summary>
		// <param name="originalUrl">Any Url including those starting with ~</param>
		// <returns>relative url</returns>
		
		public static string ResolveUrl(string originalUrl)
		{
			
			if (originalUrl == null)
			{
				
				return null;
			}
			// *** Absolute path - just return
			if (originalUrl.IndexOf("://") != -1)
			{
				return originalUrl;
			}
			
			// *** Fix up image path for ~ root app dir directory
			if (originalUrl.StartsWith("~"))
			{
				string newUrl = "";
				
				if (HttpContext.Current != null)
				{
					newUrl = HttpContext.Current.Request.ApplicationPath + originalUrl.Substring(1).Replace("//", "/");
				}
				else
				{
					
					// *** Not context: assume current directory is the base directory
					throw (new ArgumentException("Invalid URL: Relative URL not allowed."));
				}
				
				// *** Just to be sure fix up any double slashes
				return newUrl;
			}
			
			return originalUrl;
			
		}
		
		public static string ReplaceQueryStringParam(string currentPageUrl, string paramToReplace, string newValue)
		{
			string urlWithoutQuery = Convert.ToString((currentPageUrl.IndexOf('?') >= 0) ? (currentPageUrl.Substring(0, currentPageUrl.IndexOf('?'))) : currentPageUrl);
			
			string queryString = Convert.ToString((currentPageUrl.IndexOf('?') >= 0) ? (currentPageUrl.Substring(currentPageUrl.IndexOf('?'))) : null);
			
			System.Collections.Specialized.NameValueCollection queryParamList = queryString != null ? (HttpUtility.ParseQueryString(queryString)) : (HttpUtility.ParseQueryString(string.Empty));
			
			if (queryParamList[paramToReplace] != null)
			{
				queryParamList[paramToReplace] = newValue;
			}
			else
			{
				queryParamList.Add(paramToReplace, newValue);
			}
			return string.Format("{0}?{1}", urlWithoutQuery, queryParamList);
		}
		
	}
}

