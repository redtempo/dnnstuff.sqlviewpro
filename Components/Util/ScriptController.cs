

using System;


using System.Reflection;

#region licensing - please include
// Bruce Chapman Copyright (c) 2011
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions
// of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
#endregion

#region change control
// name    date     comment
// * brc     dec 11   Created
//

#endregion


namespace DNNStuff.SQLViewPro
{
	/// <summary>
	/// Used to inject script into pages independent of DotNetNuke run-time version
	/// </summary>
	public sealed class ScriptController
	{
		private ScriptController()
		{
		}
		/// <summary>
		/// This enum provides a location for where in the output page a script should be located.
		/// </summary>
		public enum ScriptInjectOrder
		{
			a_BeforejQuery,
			b_BeforeDnnXml,
			c_BeforeDomPositioning,
			d_BeforeDnnControls,
			e_Default
		}
		/// <summary>
		/// This enum provides a location for where in the output page a css file should be referenced.
		/// </summary>
		public enum CssInjectOrder
		{
			a_BeforeDefault,
			b_BeforeModule,
			c_BeforeSkin,
			d_BeforeContainer,
			e_BeforePortal,
			f_Last
		}
#region jquery/css injection
		/// <summary>
		/// Includes the jQuery libraries onto the page
		/// </summary>
		/// <param name="page">Page object from calling page/control</param>
		/// <param name="includejQueryUI">if true, includes the jQuery UI libraries</param>
		/// <param name="debug">if true, includes the uncompressed libraries</param>
		public static void InjectjQueryLibary(System.Web.UI.Page page, bool includejQueryUI, bool debug)
		{
			var major = default(int);
			var minor = default(int);
			var build = default(int);
			var revision = default(int);
			var injectjQueryLib = false;
			var injectjQueryUiLib = false;
			if (DNNUtilities.SafeDNNVersion(major, minor, revision, build))
			{
				switch (major)
				{
					case 4:
						injectjQueryLib = true;
						injectjQueryUiLib = true;
						break;
					case 5:
						injectjQueryLib = false;
						injectjQueryUiLib = true;
						break;
					default:
						//6.0 and above
						injectjQueryLib = false;
						injectjQueryUiLib = false;
						break;
				}
			}
			else
			{
				injectjQueryLib = true;
			}
			
			if (injectjQueryLib)
			{
				//no in-built jQuery libraries into the framework, so include the google version
				string lib = null;
				if (debug)
				{
					lib = "http://ajax.googleapis.com/ajax/libs/jquery/1/jquery.js";
				}
				else
				{
					lib = "http://ajax.googleapis.com/ajax/libs/jquery/1/jquery.min.js";
				}
				
				if (page.Header.FindControl("jquery") == null)
				{
					var jQueryLib = new System.Web.UI.HtmlControls.HtmlGenericControl("script");
					jQueryLib.Attributes.Add("src", lib);
					jQueryLib.Attributes.Add("type", "text/javascript");
					jQueryLib.ID = "jquery";
					page.Header.Controls.Add(jQueryLib);
					
					// use the noConflict (stops use of $) due to the use of prototype with a standard DNN distro
					var noConflictScript = new System.Web.UI.HtmlControls.HtmlGenericControl("script");
					noConflictScript.InnerText = " jQuery.noConflict(); ";
					noConflictScript.Attributes.Add("type", "text/javascript");
					page.Header.Controls.Add(noConflictScript);
				}
			}
			else
			{
				//call DotNetNuke.Framework.jQuery.RequestRegistration();
				var jQueryType = Type.GetType("DotNetNuke.Framework.jQuery, DotNetNuke");
				if (jQueryType != null)
				{
					//run the DNN 5.0 specific jQuery registration code
					jQueryType.InvokeMember("RequestRegistration", (BindingFlags) (BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static), null, jQueryType, null);
				}
			}
			
			//include the UI libraries?
			if (includejQueryUI)
			{
				if (injectjQueryUiLib)
				{
					// if you want to use a local version (ie, not CDN) here's where you would change it)
					
					string lib = null;
					if (debug)
					{
						lib = "http://ajax.googleapis.com/ajax/libs/jqueryui/1.7.2/jquery-ui.js";
					}
					else
					{
						
						lib = "http://ajax.googleapis.com/ajax/libs/jqueryui/1.7.2/jquery-ui.min.js";
					}
					page.ClientScript.RegisterClientScriptInclude("jqueryUI", lib);
				}
				else
				{
					//use late bound call to request registration of jquery
					var jQueryType = Type.GetType("DotNetNuke.Framework.jQuery, DotNetNuke");
					if (jQueryType != null)
					{
						//dnn 6.0 and later, allow jquery ui to be loaded from the settings.
						
						jQueryType.InvokeMember("RequestUIRegistration", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, jQueryType, null);
					}
				}
			}
		}
		/// <summary>
		/// Inject a js Library reference into the page
		/// </summary>
		/// <param name="page">The page object of the page to add the script reference to</param>
		/// <param name="name">Unique name for the script</param>
		/// <param name="lib">Url to the script library (can be relative/absolute)</param>
		/// <param name="inHeader">True if to go in the page header, false if to go into the page body</param>
		/// <param name="scriptPosition">Enumerated position for calculating where to place the script.  Works for DNN 6.1 and later only, ignored in earlier versions</param>
		public static void InjectJsLibrary(System.Web.UI.Page page, string name, string lib, bool inHeader, ScriptInjectOrder scriptPosition)
		{
			var major = default(int);
			var minor = default(int);
			var build = default(int);
			var revision = default(int);
			var allowInHeader = false;
			var useDotNetNukeWebClient = false;
			var dnnWebClientOk = false;
			if (DNNUtilities.SafeDNNVersion(major, minor, revision, build))
			{
				switch (major)
				{
					case 4:
					case 5:
						allowInHeader = true;
						break;
					default:
						//6.0 and above
						if (minor >= 1)
						{
							//6.1 and abpve
							if (revision < 1)
							{
								//6.1.0 - work with change in order that means no placement of scripts in header
								allowInHeader = false;
								useDotNetNukeWebClient = true;
							}
							else
							{
								//6.1.1 and above - use client dependency framework
								useDotNetNukeWebClient = true;
							}
						}
						else
						{
							//6.0
							allowInHeader = true;
						}
						break;
				}
			}
			
			if (useDotNetNukeWebClient)
			{
				//use the dotnetnuke web client methods
				var priority = GetScriptPriority(scriptPosition);
				//get the imbibe type
				var imbibe = Type.GetType("DotNetNuke.Web.Client.ClientResourceManagement.ClientResourceManager, DotNetNuke.Web.Client");
				if (imbibe != null)
				{
					//create arrays of both types and values for the parameters, in readiness for the reflection call
					var paramTypes = new Type[4];
					var paramValues = new object[4];
					paramTypes[0] = typeof(System.Web.UI.Page);
					paramValues[0] = page;
					paramTypes[1] = typeof(string);
					paramValues[1] = lib;
					paramTypes[2] = typeof(int);
					paramValues[2] = priority;
					paramTypes[3] = typeof(string);
					if (inHeader && allowInHeader)
					{
						paramValues[3] = "PageHeaderProvider";
					}
					else
					{
						paramValues[3] = "DnnBodyProvider";
					}
					//call the method to register the script via reflection
					var registerScriptMethod = imbibe.GetMethod("RegisterScript", paramTypes);
					if (registerScriptMethod != null)
					{
						registerScriptMethod.Invoke(null, paramValues);
						//worked OK
						dnnWebClientOk = true;
					}
				}
			}
			
			if (!useDotNetNukeWebClient || dnnWebClientOk == false)
			{
				//earlier versions or failed with reflection call, inject manually
				if (inHeader && allowInHeader)
				{
					if (page.Header.FindControl(name) == null)
					{
						var jsLib = new System.Web.UI.HtmlControls.HtmlGenericControl("script");
						jsLib.Attributes.Add("src", lib);
						jsLib.Attributes.Add("type", "text/javascript");
						jsLib.ID = name;
						page.Header.Controls.Add(jsLib);
					}
				}
				else
				{
					//register a script block - doesn't go in the header
					if (page.ClientScript != null)
					{
						page.ClientScript.RegisterClientScriptInclude(name, lib);
					}
				}
			}
			
		}
		
		/// <summary>
		/// Inject a reference to a CSS file into the page
		/// </summary>
		/// <param name="page">The current page object</param>
		/// <param name="name">the name of the css file - should be unique</param>
		/// <param name="file">the css file location - can be absolute or relative.</param>
		/// <param name="inHeader">true if css to be included in header, false if not</param>
		/// <param name="cssOrder">Where to include the css file in relation to the DNN css files - applies to DNN 6.1 installs only</param>
		public static void InjectCssReference(System.Web.UI.Page page, string name, string file, bool inHeader, CssInjectOrder cssOrder)
		{
			var major = default(int);
			var minor = default(int);
			var build = default(int);
			var revision = default(int);
			var useDotNetNukeWebClient = false;
			var dnnWebClientOk = false;
			if (DNNUtilities.SafeDNNVersion(major, minor, revision, build))
			{
				if (major >= 6)
				{
					if (major == 6 && minor < 1)
					{
						useDotNetNukeWebClient = false;
					}
					else
					{
						useDotNetNukeWebClient = true;
					}
				}
			}
			if (useDotNetNukeWebClient)
			{
				//use reflection to inject the css reference
				var priority = GetCssPriority(cssOrder);
				//get the imbibe type
				var imbibe = Type.GetType("DotNetNuke.Web.Client.ClientResourceManagement.ClientResourceManager, DotNetNuke.Web.Client");
				if (imbibe != null)
				{
					//reflection call
					//ClientResourceManager.RegisterScript(Page page, string filePath, int priority) // default provider
					var paramTypes = new Type[4];
					var paramValues = new object[4];
					paramTypes[0] = typeof(System.Web.UI.Page);
					paramValues[0] = page;
					paramTypes[1] = typeof(string);
					paramValues[1] = file;
					paramTypes[2] = typeof(int);
					paramValues[2] = priority;
					paramTypes[3] = typeof(string);
					if (inHeader && inHeader)
					{
						paramValues[3] = "PageHeaderProvider";
					}
					else
					{
						paramValues[3] = "DnnBodyProvider";
					}
					//call the method to register the script via reflection
					var registerStyleSheetMethod = imbibe.GetMethod("RegisterStyleSheet", paramTypes);
					if (registerStyleSheetMethod != null)
					{
						registerStyleSheetMethod.Invoke(null, paramValues);
						//worked OK
						dnnWebClientOk = true;
					}
				}
			}
			//not on DNN 6.1, so use direct method to inject the header / body.
			//note that outcome position is pot luck based on calling code.
			if (!useDotNetNukeWebClient || dnnWebClientOk == false)
			{
				if (page.Header.FindControl(name) == null)
				{
					//764 : xhtml compliance by using html link control which closes tag without separate closing tag
					var cssFile = new System.Web.UI.HtmlControls.HtmlLink();
					cssFile.Attributes.Add("rel", "stylesheet");
					cssFile.Attributes.Add("href", file);
					cssFile.Attributes.Add("type", "text/css");
					cssFile.ID = name;
					page.Header.Controls.Add(cssFile);
				}
				else
				{
					if (page.FindControl(name) == null)
					{
						var cssFile = new System.Web.UI.HtmlControls.HtmlLink();
						cssFile.Attributes.Add("rel", "stylesheet");
						cssFile.Attributes.Add("href", file);
						cssFile.Attributes.Add("type", "text/css");
						cssFile.ID = name;
						page.Controls.Add(cssFile);
					}
				}
			}
		}
		/// <summary>
		/// Injects Css for jQuery tabs Ui
		/// </summary>
		/// <param name="page"></param>
		/// <param name="preDnn6CssFile"></param>
		/// <param name="postDnn6CssFile"></param>
		/// <remarks>This method only gets run in pre-dnn6 installations.  Otherwise it uses the pre-defined DNN 6 Css declarations to style
		/// the UI Tabs</remarks>
		public static void InjectjQueryTabsCss(System.Web.UI.Page page, string preDnn6CssFile, string postDnn6CssFile)
		{
			var major = default(int);
			var minor = default(int);
			var build = default(int);
			var revision = default(int);
			DNNUtilities.SafeDNNVersion(major, minor, revision, build);
			if (major < 6)
			{
				if (preDnn6CssFile != null && preDnn6CssFile != "")
				{
					InjectCssReference(page, "moduleJqCss", preDnn6CssFile, true, CssInjectOrder.f_Last);
				}
				InjectCssReference(page, "jqueryUiTheme", "http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css", true, CssInjectOrder.f_Last);
			}
			if (major >= 6 && postDnn6CssFile != null && postDnn6CssFile != "")
			{
				InjectCssReference(page, "moduleJqCss", postDnn6CssFile, true, CssInjectOrder.c_BeforeSkin);
			}
			
		}
#endregion
		
#region calculate priority from enum types
		private static int GetScriptPriority(ScriptInjectOrder scriptPosition)
		{
			switch (scriptPosition)
			{
				case ScriptInjectOrder.a_BeforejQuery:
					return 4;
				case ScriptInjectOrder.b_BeforeDnnXml:
					return 14;
				case ScriptInjectOrder.c_BeforeDomPositioning:
					return 29;
				case ScriptInjectOrder.d_BeforeDnnControls:
					return 39;
				default:
					return 100;
			}
		}
		private static int GetCssPriority(CssInjectOrder cssPosition)
		{
			switch (cssPosition)
			{
				case CssInjectOrder.a_BeforeDefault:
					return 4;
				case CssInjectOrder.b_BeforeModule:
					return 9;
				case CssInjectOrder.c_BeforeSkin:
					return 14;
				case CssInjectOrder.d_BeforeContainer:
					return 24;
				case CssInjectOrder.e_BeforePortal:
					return 34;
				default:
					return 50;
			}
		}
#endregion
		
	}
}

