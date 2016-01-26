using System;
using System.Data;
using System.Xml;
using DotNetNuke.Common;
namespace DNNStuff.SQLViewPro.StandardReports
{
	
	public partial class XmlReportControl : Controls.ReportControlBase
	{
		
#region  Web Form Designer Generated Code
		
		//This call is required by the Web Form Designer.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent()
		{
			
		}
		
		private void Page_Init(Object sender, EventArgs e)
		{
			//CODEGEN: This method call is required by the Web Form Designer
			//Do not modify it using the code editor.
			InitializeComponent();
			
			try
			{
				BindXMLToData();
			}
			catch (Exception ex)
			{
				DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
			}
		}
		
		
#endregion
		
#region  Page

	    private XmlReportSettings ReportExtra { get; set; } = new XmlReportSettings();

	    #endregion
		
#region  Base Method Implementations
		public override void LoadRuntimeSettings(ReportInfo Settings)
		{
			ReportExtra = (XmlReportSettings) (Serialization.DeserializeObject(Settings.ReportConfig, typeof(XmlReportSettings)));
		}
#endregion
		
#region  XML
		private void BindXMLToData()
		{
			
			var ds = ReportData();
			
			// add debug info
			if (State.ReportSet.ReportSetDebug)
			{
				DebugInfo.Append(QueryText);
				var sw = new System.IO.StringWriter();
				ds.WriteXml(sw);
				DebugInfo.AppendFormat("<pre>{0}</pre>", Server.HtmlEncode(sw.ToString()));
			}
			
			if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
			{
				xmlContent.Visible = Report.ReportNoItemsText.Length == 0;
				RenderNoItems();
			}
			else
			{
				var swData = new System.IO.StringWriter();
				ds.WriteXml(swData);
				
				var xmlData = new XmlDocument();
				xmlData.LoadXml(swData.ToString());
				
				// transform data using xsl
				var xslTransform = GetXslTransform(ReportExtra.XslSrc);
				var swOutput = new System.IO.StringWriter();
				
				var xmltwOutput = new XmlTextWriter(swOutput);
				if (xslTransform != null&& xmlData != null)
				{
					xslTransform.Transform(xmlData, xmltwOutput);
				}
				
				xmlContent.Text = swOutput.ToString();
			}
		}
		
		
		
#endregion
		
#region xsl functions
		private System.Xml.Xsl.XslCompiledTransform GetXSLContent(string ContentURL)
		{
			var returnValue = default(System.Xml.Xsl.XslCompiledTransform);
			
			returnValue = new System.Xml.Xsl.XslCompiledTransform();
			System.Net.WebRequest req = Globals.GetExternalRequest(ContentURL);
			var result = req.GetResponse();
			XmlReader objXSLTransform = new XmlTextReader(result.GetResponseStream());
			returnValue.Load(objXSLTransform, null, null);
			
			return returnValue;
		}
		
		private System.Xml.Xsl.XslCompiledTransform GetXslTransform(string XslDoc)
		{
			if (XslDoc != "")
			{
				if (Globals.GetURLType(XslDoc) == DotNetNuke.Entities.Tabs.TabType.Url)
				{
					if (XslDoc.ToLower().StartsWith("http"))
					{
						return GetXSLContent(XslDoc);
					}
					else if (XslDoc.StartsWith("~") || XslDoc.StartsWith("/"))
					{
						var trans = new System.Xml.Xsl.XslCompiledTransform();
						trans.Load(Context.Server.MapPath(XslDoc));
						return trans;
					}
					else if (XslDoc.Contains(":\\"))
					{
						var trans = new System.Xml.Xsl.XslCompiledTransform();
						trans.Load(XslDoc);
						return trans;
					}
				}
			}
			return null;
		}
#endregion
		
	}
	
}

