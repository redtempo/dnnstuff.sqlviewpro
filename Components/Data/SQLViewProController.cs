using System;
using System.Web;
using System.Collections;
using System.Xml;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Common.Utilities;

namespace DNNStuff.SQLViewPro
{
	public class SQLViewProController : IPortable
	{
		
#region Export
		public string ExportModule(int ModuleID)
		{
			var ctrl = new ReportSetController();
			
			var strXML = "";
			
			strXML += "<sqlviewpro>";
			
			var info = ctrl.GetReportSetByModule(ModuleID);
			strXML += ExportReportSet(info);
			
			strXML += ExportReport(info.ReportSetId);
			
			strXML += ExportParameter(info.ReportSetId);
			
			// close it off
			strXML += "</sqlviewpro>";
			
			return strXML;
			
		}
		
		private string ExportConnection(PortalSettings portalSettings)
		{
			var ctrl = new ConnectionController();
			var al = ctrl.ListConnection(Convert.ToInt32(portalSettings.PortalId), false, false);
			var info = default(ConnectionInfo);
			var s = "<connections>";
			for (var i = 0; i <= al.Count - 1; i++)
			{
				info = (ConnectionInfo) (al[i]);
				s += "<connection>";
				s += "<id>" + XmlUtils.XMLEncode(info.ConnectionId.ToString()) + "</id>";
				s += "<name>" + XmlUtils.XMLEncode(info.ConnectionName) + "</name>";
				s += "<string>" + XmlUtils.XMLEncode(info.ConnectionString) + "</string>";
				s += "</connection>";
			}
			s += "</connections>";
			
			return s;
		}
		
		private string ExportReportSet(ReportSetInfo info)
		{
			var s = "";
			s += "<reportset>";
			s += "<id>" + XmlUtils.XMLEncode(info.ReportSetId.ToString()) + "</id>";
			s += "<name>" + XmlUtils.XMLEncode(info.ReportSetName) + "</name>";
			s += "<theme>" + XmlUtils.XMLEncode(info.ReportSetTheme) + "</theme>";
			s += "<debug>" + XmlUtils.XMLEncode(info.ReportSetDebug.ToString()) + "</debug>";
			s += "<runcaption>" + XmlUtils.XMLEncode(info.RunCaption) + "</runcaption>";
			s += "<backcaption>" + XmlUtils.XMLEncode(info.BackCaption) + "</backcaption>";
			s += "<connectionid>" + XmlUtils.XMLEncode(info.ReportSetConnectionId.ToString()) + "</connectionid>";
			s += "<connectionstring>" + XmlUtils.XMLEncode(info.ReportSetConnectionString.ToString()) + "</connectionstring>";
			s += "<footertext>" + XmlUtils.XMLEncode(info.ReportSetFooterText) + "</footertext>";
			s += "<headertext>" + XmlUtils.XMLEncode(info.ReportSetHeaderText) + "</headertext>";
			s += "<parameterlayout>" + XmlUtils.XMLEncode(info.ParameterLayout) + "</parameterlayout>";
			s += "<alwaysshowparameters>" + XmlUtils.XMLEncode(info.AlwaysShowParameters.ToString()) + "</alwaysshowparameters>";
			s += "<rendermode>" + XmlUtils.XMLEncode(info.RenderMode) + "</rendermode>";
			s += "<reportsetconfig>" + XmlUtils.XMLEncode(info.ReportSetConfig) + "</reportsetconfig>";
			s += "</reportset>";
			return s;
		}
		
		private string ExportReport(int ReportSetId)
		{
			
			var ctrl = new ReportController();
			var al = ctrl.ListReport(ReportSetId);
			var info = default(ReportInfo);
			var s = "<reports>";
			for (var i = 0; i <= al.Count - 1; i++)
			{
				info = (ReportInfo) (al[i]);
				s += "<report>";
				s += "<id>" + XmlUtils.XMLEncode(info.ReportId.ToString()) + "</id>";
				s += "<reportsetid>" + XmlUtils.XMLEncode(info.ReportSetId.ToString()) + "</reportsetid>";
				s += "<name>" + XmlUtils.XMLEncode(info.ReportName) + "</name>";
				s += "<theme>" + XmlUtils.XMLEncode(info.ReportTheme) + "</theme>";
				s += "<command>" + XmlUtils.XMLEncode(info.ReportCommand) + "</command>";
				s += "<reporttype>" + XmlUtils.XMLEncode(info.ReportTypeId.ToString()) + "</reporttype>";
				s += "<config>" + XmlUtils.XMLEncode(info.ReportConfig) + "</config>";
				s += "<connectionid>" + XmlUtils.XMLEncode(info.ReportConnectionId.ToString()) + "</connectionid>";
				s += "<connectionstring>" + XmlUtils.XMLEncode(info.ReportConnectionString.ToString()) + "</connectionstring>";
				s += "<footertext>" + XmlUtils.XMLEncode(info.ReportFooterText) + "</footertext>";
				s += "<headertext>" + XmlUtils.XMLEncode(info.ReportHeaderText) + "</headertext>";
				s += "<order>" + XmlUtils.XMLEncode(info.ReportOrder.ToString()) + "</order>";
				s += "<drilldownreportid>" + XmlUtils.XMLEncode(info.ReportDrilldownReportId.ToString()) + "</drilldownreportid>";
				s += "<drilldownfieldname>" + XmlUtils.XMLEncode(info.ReportDrilldownFieldname) + "</drilldownfieldname>";
				s += "<noitemstext>" + XmlUtils.XMLEncode(info.ReportNoItemsText) + "</noitemstext>";
				s += "<pagetitle>" + XmlUtils.XMLEncode(info.ReportPageTitle) + "</pagetitle>";
				s += "<cachetimeout>" + XmlUtils.XMLEncode(info.ReportCommandCacheTimeout.ToString()) + "</cachetimeout>";
				s += "<metadescription>" + XmlUtils.XMLEncode(info.ReportMetaDescription) + "</metadescription>";
				s += "<cachescheme>" + XmlUtils.XMLEncode(info.ReportCommandCacheScheme.ToString()) + "</cachescheme>";
				
				s += "</report>";
			}
			s += "</reports>";
			
			return s;
		}
		
		private string ExportParameter(int ReportSetId)
		{
			
			var ctrl = new ParameterController();
			var al = ctrl.ListParameter(ReportSetId);
			var info = default(ParameterInfo);
			var s = "<parameters>";
			for (var i = 0; i <= al.Count - 1; i++)
			{
				info = al[i];
				s += "<parameter>";
				s += "<id>" + XmlUtils.XMLEncode(info.ParameterId.ToString()) + "</id>";
				s += "<reportsetid>" + XmlUtils.XMLEncode(info.ReportSetId.ToString()) + "</reportsetid>";
				s += "<name>" + XmlUtils.XMLEncode(info.ParameterName) + "</name>";
				s += "<parametertype>" + XmlUtils.XMLEncode(info.ParameterTypeId.ToString()) + "</parametertype>";
				s += "<config>" + XmlUtils.XMLEncode(info.ParameterConfig) + "</config>";
				s += "<caption>" + XmlUtils.XMLEncode(info.ParameterCaption) + "</caption>";
				s += "<order>" + XmlUtils.XMLEncode(info.ParameterOrder.ToString()) + "</order>";
				s += "</parameter>";
			}
			s += "</parameters>";
			
			return s;
		}
#endregion
		
#region Import
		
		public void ImportModule(int ModuleID, string Content, string Version, int UserID)
		{
			
			// remove current reportset
			var ctrl = new ReportSetController();
			ctrl.DeleteReportSet(ModuleID);
			
			var xmlSqlViewPro = DotNetNuke.Common.Globals.GetContent(Content, "sqlviewpro");
			
			var ReportSetId = ImportReportSet(ModuleID, xmlSqlViewPro);
			ImportReport(ReportSetId, xmlSqlViewPro);
			ImportParameter(ReportSetId, xmlSqlViewPro);
			
		}
		
		private int ImportConnection(string connectionName, string connectionString)
		{
			var portalSettings = (PortalSettings) (HttpContext.Current.Items["PortalSettings"]);
			
			var ctrl = new ConnectionController();
			
			// see if we can find the connection string and return the id
			var connections = ctrl.ListConnection(Convert.ToInt32(portalSettings.PortalId), false, false);
			foreach (ConnectionInfo info in connections)
			{
				if (info.ConnectionString == connectionString)
				{
					return info.ConnectionId;
				}
			}
			
			// we'll have to make a new one
			var connectionId = default(int);
			connectionId = ctrl.UpdateConnection(Convert.ToInt32(portalSettings.PortalId), -1, connectionName, connectionString);
			
			return connectionId;
			
		}
		
		private int ImportReportSet(int ModuleId, XmlNode root)
		{
			XmlNode xmlReportSet = root["reportset"];
			
			var ctrl = new ReportSetController();
			
			var reportSetId = default(int);
			var reportSetName = default(string);
			var reportSetTheme = default(string);
			var reportSetConnectionId = default(int);
			var reportSetConnectionString = default(string);
			var reportSetFooterText = default(string);
			var reportSetHeaderText = default(string);
			var reportSetDebug = default(bool);
			var runCaption = default(string);
			var backCaption = default(string);
			var parameterLayout = default(string);
			var alwaysShowParameters = default(bool);
			var renderMode = default(string);
			var autoRun = default(bool);
			
			var reportSetConfig = default(string);
			
			reportSetId = -1;
			reportSetName = xmlReportSet["name"].InnerText;
			reportSetTheme = xmlReportSet["theme"].InnerText;
			reportSetConnectionId = Convert.ToInt32(xmlReportSet["connectionid"].InnerText);
			reportSetConnectionString = xmlReportSet["connectionstring"].InnerText;
			if (reportSetConnectionId != (int) ConnectionType.PortalDefault)
			{
				reportSetConnectionId = ImportConnection("Imported " + DateTime.Now.ToShortDateString(), reportSetConnectionString);
			}
			reportSetFooterText = xmlReportSet["footertext"].InnerText;
			reportSetHeaderText = xmlReportSet["headertext"].InnerText;
			reportSetDebug = Convert.ToBoolean(xmlReportSet["debug"].InnerText);
			runCaption = GetNodeInnerText(xmlReportSet, "runcaption");
			backCaption = GetNodeInnerText(xmlReportSet, "backcaption");
			parameterLayout = GetNodeInnerText(xmlReportSet, "parameterlayout");
			alwaysShowParameters = Convert.ToBoolean(GetNodeInnerText(xmlReportSet, "alwaysshowparameters", "false"));
			renderMode = GetNodeInnerText(xmlReportSet, "rendermode", "default");
			autoRun = Convert.ToBoolean(GetNodeInnerText(xmlReportSet, "autoRun", "false"));
			
			reportSetConfig = GetNodeInnerText(xmlReportSet, "reportsetconfig", "");
			
			// check if id is already present
			reportSetId = ctrl.UpdateReportSet(ModuleId, reportSetId, reportSetName, reportSetTheme, reportSetConnectionId, reportSetHeaderText, reportSetFooterText, reportSetDebug, runCaption, backCaption, parameterLayout, alwaysShowParameters, renderMode, autoRun, reportSetConfig);
			return reportSetId;
			
		}
		
		private void ImportReport(int ReportSetId, XmlNode root)
		{
			XmlNode xmlReports = root["reports"];
			
			var ctrl = new ReportController();
			
			var reportId = default(int);
			var reportTypeId = default(string);
			var reportName = default(string);
			var reportTheme = default(string);
			var reportConnectionId = default(int);
			var reportConnectionString = default(string);
			var reportFooterText = default(string);
			var reportHeaderText = default(string);
			var reportCommand = default(string);
			var reportConfig = default(string);
			var reportOrder = default(int);
			var reportDrilldownReportId = default(int);
			var reportDrilldownFieldname = default(string);
			var reportNoItemsText = default(string);
			var reportPageTitle = default(string);
			var reportCommandCacheTimeout = default(int);
			
			var reportMapping = new Hashtable(); //map old reportid to new ones for drilldown reference
			
			foreach (XmlNode xmlReport in xmlReports.ChildNodes)
			{
				reportId = Convert.ToInt32(xmlReport["id"].InnerText);
				reportTypeId = xmlReport["reporttype"].InnerText;
				reportName = xmlReport["name"].InnerText;
				reportTheme = xmlReport["theme"].InnerText;
				reportConnectionId = Convert.ToInt32(xmlReport["connectionid"].InnerText);
				reportConnectionString = xmlReport["connectionstring"].InnerText;
				if (reportConnectionId != (int) ConnectionType.PortalDefault & reportConnectionId != (int) ConnectionType.ReportSetDefault)
				{
					reportConnectionId = ImportConnection("Imported " + DateTime.Now.ToShortDateString(), reportConnectionString);
				}
				reportFooterText = xmlReport["footertext"].InnerText;
				reportHeaderText = xmlReport["headertext"].InnerText;
				reportCommand = xmlReport["command"].InnerText;
				reportConfig = xmlReport["config"].InnerText;
				reportOrder = Convert.ToInt32(xmlReport["order"].InnerText);
				reportDrilldownReportId = Convert.ToInt32(xmlReport["drilldownreportid"].InnerText);
				if (reportDrilldownReportId > -1)
				{
					if (reportMapping.ContainsKey(reportDrilldownReportId))
					{
						reportDrilldownReportId = Convert.ToInt32(reportMapping[reportDrilldownReportId]);
					}
				}
				reportDrilldownFieldname = xmlReport["drilldownfieldname"].InnerText;
				reportNoItemsText = GetNodeInnerText(xmlReport, "noitemstext");
				reportPageTitle = GetNodeInnerText(xmlReport, "pagetitle");
				reportCommandCacheTimeout = int.Parse(GetNodeInnerText(xmlReport, "cachetimeout", "0"));
				var reportMetaDescription = GetNodeInnerText(xmlReport, "metadescription");
				var reportCommandCacheScheme = GetNodeInnerText(xmlReport, "cachescheme", "Sliding");
				
				// check if id is already present
				var newid = ctrl.UpdateReport(ReportSetId, -1, reportTypeId, reportName, reportTheme, reportConnectionId, reportHeaderText, reportFooterText, reportCommand, reportConfig, reportOrder, reportDrilldownReportId, reportDrilldownFieldname, reportNoItemsText, reportPageTitle, reportCommandCacheTimeout, reportMetaDescription, reportCommandCacheScheme);
				reportMapping.Add(reportId, newid);
			}
			
		}
		
		private void ImportParameter(int ReportSetId, XmlNode root)
		{
			XmlNode xmlParameters = root["parameters"];
			
			var ctrl = new ParameterController();
			
			var parameterId = default(int);
			var parameterTypeId = default(string);
			var parameterName = default(string);
			var parameterCaption = default(string);
			var parameterConfig = default(string);
			var parameterOrder = default(int);
			
			foreach (XmlNode xmlParameter in xmlParameters.ChildNodes)
			{
				parameterId = -1;
				
				parameterTypeId = xmlParameter["parametertype"].InnerText;
				parameterName = xmlParameter["name"].InnerText;
				parameterCaption = xmlParameter["caption"].InnerText;
				parameterConfig = xmlParameter["config"].InnerText;
				parameterOrder = Convert.ToInt32(xmlParameter["order"].InnerText);
				
				// check if id is already present
				ctrl.UpdateParameter(ReportSetId, parameterId, parameterName, parameterCaption, parameterTypeId, parameterConfig, parameterOrder);
			}
			
		}
		
		private string GetNodeInnerText(XmlNode node, string key, string @default = "")
		{
			var xe = node[key];
			if (xe == null)
			{
				return @default;
			}
			return xe.InnerText;
		}
		
#endregion
		
	}
	
}

