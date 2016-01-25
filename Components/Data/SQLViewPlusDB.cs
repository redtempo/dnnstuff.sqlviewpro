

using System;
using System.Collections;
using System.Data;

using DotNetNuke.Common.Utilities;
using System.Collections.Generic;


namespace DNNStuff.SQLViewPro
{
	
#region  ReportSet
	[Serializable()]public class ReportSetInfo
	{
		
		// initialization
		public ReportSetInfo()
		{
		}
		
		// public properties
		private int _ReportSetId = -1;
		public int ReportSetId
		{
			get
			{
				return _ReportSetId;
			}
			set
			{
				_ReportSetId = value;
			}
		}
		public string ReportSetName {get; set;}
		public string ReportSetTheme {get; set;}
		public int ReportSetConnectionId {get; set;}
		public string ReportSetConnectionString {get; set;}
		public string ReportSetHeaderText {get; set;}
		public string ReportSetFooterText {get; set;}
		public bool ReportSetDebug {get; set;}
		public string RunCaption {get; set;}
		public string BackCaption {get; set;}
		public string ParameterLayout {get; set;}
		public bool AlwaysShowParameters {get; set;}
		public int ReportCount {get; set;}
		public int ParameterCount {get; set;}
		public bool AutoRun {get; set;}
		public string RenderMode {get; set;}
		
		public string ReportSetConfig {get; set;}
		
		private ReportSetConfig _Config = null;
		public ReportSetConfig Config
		{
			get
			{
				if (_Config == null)
				{
					if (ReportSetConfig == "")
					{
						_Config = new ReportSetConfig();
					}
					else
					{
						_Config = (ReportSetConfig) (Serialization.DeserializeObject(ReportSetConfig, typeof(ReportSetConfig)));
					}
				}
				return _Config;
			}
		}
	}
	
	[Serializable()]public class ReportSetConfig
	{
		
	}
	
	public class ReportSetController
	{
		
		public ReportSetInfo GetReportSet(int ReportSetId)
		{
			return ((ReportSetInfo) (CBO.FillObject(DataProvider.Instance().GetReportSet(ReportSetId), typeof(ReportSetInfo))));
		}
		public ReportSetInfo GetReportSetByModule(int ModuleId)
		{
			return ((ReportSetInfo) (CBO.FillObject(DataProvider.Instance().GetReportSetByModule(ModuleId), typeof(ReportSetInfo))));
		}
		
		public int UpdateReportSet(int ModuleId, int ReportSetId, string ReportSetName, string ReportSetTheme, int ReportSetConnectionId, string ReportSetHeaderText, string ReportSetFooterText, bool ReportSetDebug, string RunCaption, string BackCaption, string ParameterLayout, bool AlwaysShowParameters, string RenderMode, bool AutoRun, string ReportSetConfig)
		{
			return DataProvider.Instance().UpdateReportSet(ModuleId, ReportSetId, ReportSetName, ReportSetTheme, ReportSetConnectionId, ReportSetHeaderText, ReportSetFooterText, ReportSetDebug, RunCaption, BackCaption, ParameterLayout, AlwaysShowParameters, RenderMode, AutoRun, ReportSetConfig);
		}
		
		public void DeleteReportSet(int ModuleId)
		{
			DataProvider.Instance().DeleteReportSet(ModuleId);
		}
		
		public ArrayList ListReportSet(int PortalId)
		{
			return CBO.FillCollection(DataProvider.Instance().ListReportSet(PortalId), typeof(ReportSetInfo));
		}
		
		public ArrayList GetReportSetReport(int ReportSetId)
		{
			var al = default(ArrayList);
			al = (ArrayList) (CBO.FillCollection(DataProvider.Instance().GetReportSetReport(ReportSetId), typeof(ReportInfo)));
			foreach (var obj in al)
			{
				var ri = (ReportInfo) obj;
				ri.ReportDrillDowns = CBO.FillCollection(DataProvider.Instance().ListReportDrilldown(ri.ReportId), typeof(ReportInfo));
			}
			return al;
		}
		
		public ArrayList GetReportSetParameter(int ReportSetId)
		{
			return CBO.FillCollection(DataProvider.Instance().GetReportSetParameter(ReportSetId), typeof(ParameterInfo));
		}
	}
	
#endregion
	
#region  Report
	public class ReportInfo
	{
		
		// initialization
		public ReportInfo()
		{
		}
		
		// public properties
		public int ReportId {get; set;}
		public int ReportSetId {get; set;}
		public string ReportName {get; set;}
		public string ReportTheme {get; set;}
		public int ReportConnectionId {get; set;}
		public string ReportConnectionString {get; set;}
		public string ReportHeaderText {get; set;}
		public string ReportFooterText {get; set;}
		public string ReportNoItemsText {get; set;}
		public string ReportCommand {get; set;}
		private int _ReportCommandCacheTimeout = 60;
		public int ReportCommandCacheTimeout
		{
			get
			{
				return _ReportCommandCacheTimeout;
			}
			set
			{
				_ReportCommandCacheTimeout = value;
			}
		}
		public string ReportCommandCacheScheme {get; set;}
		public string ReportConfig {get; set;}
		public string ReportTypeName {get; set;}
		private string _ReportTypeId = "GRID";
		public string ReportTypeId
		{
			get
			{
				return _ReportTypeId;
			}
			set
			{
				_ReportTypeId = value;
			}
		}
		public string ReportTypeControlSrc {get; set;}
		public string ReportTypeSettingsControlSrc {get; set;}
		public int ReportOrder {get; set;}
		private string _ReportDrilldownFieldname = "";
		public string ReportDrilldownFieldname
		{
			get
			{
				return _ReportDrilldownFieldname;
			}
			set
			{
				_ReportDrilldownFieldname = value;
			}
		}
		private int _ReportDrilldownReportId = -1;
		public int ReportDrilldownReportId
		{
			get
			{
				return _ReportDrilldownReportId;
			}
			set
			{
				_ReportDrilldownReportId = value;
			}
		}
		public ArrayList ReportDrillDowns {get; set;}
		public string ReportPageTitle {get; set;}
		public string ReportMetaDescription {get; set;}
		
		// calced
		public string ReportIdentifier => ReportName.Replace(" ", "_");

	    public string ReportIdentifierQuoteStartCharacter
		{
			get
			{
				if (ReportConnectionString.ToLower().Contains("mysql"))
				{
					return "`";
				}
				return "[";
			}
		}
		public string ReportIdentifierQuoteEndCharacter
		{
			get
			{
				if (ReportConnectionString.ToLower().Contains("mysql"))
				{
					return "`";
				}
				return "]";
			}
		}
	}
	
	public class ReportController
	{
		public ReportInfo GetReport(int reportId)
		{
			var ri = default(ReportInfo);
			
			ri = (ReportInfo) (CBO.FillObject(DataProvider.Instance().GetReport(reportId), typeof(ReportInfo)));
			
			if (ri != null)
			{
				ri.ReportDrillDowns = CBO.FillCollection(DataProvider.Instance().ListReportDrilldown(reportId), typeof(ReportInfo));
			}
			
			return ri;
		}
		
		public int UpdateReport(int ReportSetId, int ReportId, string ReportTypeId, string ReportName, string ReportTheme, int ReportConnectionId, string ReportHeaderText, string ReportFooterText, string ReportCommand, string ReportConfig, int ReportOrder, int ReportDrilldownReportId, string ReportDrilldownFieldname, string ReportNoItemsText, string ReportPageTitle, int ReportCommandCacheTimeout, string ReportMetaDescription, string ReportCommandCacheScheme)
		{
			
			return DataProvider.Instance().UpdateReport(ReportSetId, ReportId, ReportTypeId, ReportName, ReportTheme, ReportConnectionId, ReportHeaderText, ReportFooterText, ReportCommand, ReportConfig, ReportOrder, ReportDrilldownReportId, ReportDrilldownFieldname, ReportNoItemsText, ReportPageTitle, ReportCommandCacheTimeout, ReportMetaDescription, ReportCommandCacheScheme);
		}
		public ArrayList ListReport(int ReportSetId)
		{
			return CBO.FillCollection(DataProvider.Instance().ListReport(ReportSetId), typeof(ReportInfo));
		}
		
		public ArrayList ListReportDrilldown(int ReportId)
		{
			return CBO.FillCollection(DataProvider.Instance().ListReportDrilldown(ReportId), typeof(ReportInfo));
		}
		
		public void UpdateReportOrder(int ReportId, int Increment)
		{
			DataProvider.Instance().UpdateReportOrder(ReportId, Increment);
		}
		
		public void DeleteReport(int ReportId)
		{
			DataProvider.Instance().DeleteReport(ReportId);
		}
	}
#endregion
	
#region  Parameter
	[Serializable()]public class ParameterInfo
	{
		
		// initialization
		public ParameterInfo()
		{
		}
		
		// public properties
		private int _ParameterId = -1;
		public int ParameterId
		{
			get
			{
				return _ParameterId;
			}
			set
			{
				_ParameterId = value;
			}
		}
		public int ReportSetId {get; set;}
		private string _ParameterName = "";
		public string ParameterName
		{
			get
			{
				return _ParameterName;
			}
			set
			{
				_ParameterName = value;
			}
		}
		private string _ParameterCaption = "";
		public string ParameterCaption
		{
			get
			{
				return _ParameterCaption;
			}
			set
			{
				_ParameterCaption = value;
			}
		}
		private string _ParameterTypeId = "TEXTBOX";
		public string ParameterTypeId
		{
			get
			{
				return _ParameterTypeId;
			}
			set
			{
				_ParameterTypeId = value;
			}
		}
		public string ParameterTypeName {get; set;}
		public string ParameterTypeSettingsControlSrc {get; set;}
		public string ParameterTypeControlSrc {get; set;}
		public int ParameterOrder {get; set;}
		public string ParameterConfig {get; set;}
		
		// provided by main routine, captured from gui parameters
		public List<string> Values {get; set;}
		public System.Collections.Specialized.StringDictionary ExtraValues {get; set;}
		private bool _MultiValued = false;
		public bool MultiValued
		{
			get
			{
				return _MultiValued;
			}
			set
			{
				_MultiValued = value;
			}
		}
		
		// calculated
		public string ParameterIdentifier => ParameterName.Replace(" ", "_");
	}
	
	public class ParameterController
	{
		public ParameterInfo GetParameter(int ParameterId)
		{
			return ((ParameterInfo) (CBO.FillObject(DataProvider.Instance().GetParameter(ParameterId), typeof(ParameterInfo))));
		}
		
		public int UpdateParameter(int ReportSetId, int ParameterId, string ParameterName, string ParameterCaption, string ParameterTypeId, string ParameterConfig, int ParameterOrder)
		{
			
			return DataProvider.Instance().UpdateParameter(ReportSetId, ParameterId, ParameterName, ParameterCaption, ParameterTypeId, ParameterConfig, ParameterOrder);
		}
		
		public List<ParameterInfo> ListParameter(int reportSetId)
		{
			return CBO.FillCollection<ParameterInfo>(DataProvider.Instance().ListParameter(reportSetId));
		}
		
		public void UpdateParameterOrder(int ParameterId, int Increment)
		{
			DataProvider.Instance().UpdateParameterOrder(ParameterId, Increment);
		}
		
		public void DeleteParameter(int ParameterId)
		{
			DataProvider.Instance().DeleteParameter(ParameterId);
		}
	}
#endregion
	
#region  Report Type
	public class ReportTypeInfo
	{
		public string ReportTypeId {get; set;}
		public string ReportTypeName {get; set;}
		public string ReportTypeControlSrc {get; set;}
		public string ReportTypeSettingsControlSrc {get; set;}
		public bool ReportTypeSupportsDrilldown {get; set;}
	}
	
	public class ReportTypeController
	{
		public IDataReader ListReportType()
		{
			return DataProvider.Instance().ListReportType();
		}
		
		public ReportTypeInfo GetReportType(string ReportTypeId)
		{
			return ((ReportTypeInfo) (CBO.FillObject(DataProvider.Instance().GetReportType(ReportTypeId), typeof(ReportTypeInfo))));
		}
	}
#endregion
	
#region  Parameter Type
	public class ParameterTypeInfo
	{
		public string ParameterTypeName {get; set;}
		public string ParameterTypeControlSrc {get; set;}
		public string ParameterTypeSettingsControlSrc {get; set;}
		private string _ParameterTypeId = "TEXTBOX";
		public string ParameterTypeId
		{
			get
			{
				return _ParameterTypeId;
			}
			set
			{
				_ParameterTypeId = value;
			}
		}
	}
	
	public class ParameterTypeController
	{
		public IDataReader ListParameterType()
		{
			return DataProvider.Instance().ListParameterType();
		}
		
		public ParameterTypeInfo GetParameterType(string ParameterTypeId)
		{
			return ((ParameterTypeInfo) (CBO.FillObject(DataProvider.Instance().GetParameterType(ParameterTypeId), typeof(ParameterTypeInfo))));
		}
	}
#endregion
	
#region  Connection
	public enum ConnectionType
	{
		PortalDefault = -1,
		ReportSetDefault = -2
	}
	public class ConnectionInfo
	{
		public string ConnectionName {get; set;}
		public string ConnectionString {get; set;}
		public int ConnectionId {get; set;}
		public int PortalId {get; set;}
		public int UsedInParameterCount {get; set;}
		public int UsedInReportCount {get; set;}
		public int UsedInReportSetCount {get; set;}
		public bool CanDelete => (UsedInParameterCount + UsedInReportCount + UsedInReportSetCount) == 0;
	}
	
	public class ConnectionController
	{
		public static ConnectionInfo GetConnection(int ConnectionId)
		{
			return ((ConnectionInfo) (CBO.FillObject(DataProvider.Instance().GetConnection(ConnectionId), typeof(ConnectionInfo))));
		}
		
		public static string GetConnectionString(int ConnectionId, int ReportSetId)
		{
			switch (ConnectionId)
			{
				case (int) ConnectionType.PortalDefault:
					return "";
				case (int) ConnectionType.ReportSetDefault:
					var rsc = new ReportSetController();
					var rsi = rsc.GetReportSet(ReportSetId);
					return rsi.ReportSetConnectionString;
				default:
					var csi = GetConnection(ConnectionId);
					return csi.ConnectionString;
			}
		}
		
		public int UpdateConnection(int PortalId, int ConnectionId, string ConnectionName, string ConnectionString)
		{
			return DataProvider.Instance().UpdateConnection(PortalId, ConnectionId, ConnectionName, ConnectionString);
		}
		
		public void DeleteConnection(int ConnectionId)
		{
			DataProvider.Instance().DeleteConnection(ConnectionId);
		}
		public ArrayList ListConnection(int PortalId, bool IncludePortalDefault, bool IncludeReportSetDefault)
		{
			var al = (ArrayList) (CBO.FillCollection(DataProvider.Instance().ListConnection(PortalId), typeof(ConnectionInfo)));
			
			var ci = default(ConnectionInfo);
			
			if (IncludePortalDefault)
			{
				// add portal default option
				ci = new ConnectionInfo();
				ci.ConnectionId = (int) ConnectionType.PortalDefault;
				ci.ConnectionName = "Portal Default";
				ci.ConnectionString = "";
				al.Insert(0, ci);
			}
			
			if (IncludeReportSetDefault)
			{
				// add report set default option
				ci = new ConnectionInfo();
				ci.ConnectionId = (int) ConnectionType.ReportSetDefault;
				ci.ConnectionName = "ReportSet Default";
				ci.ConnectionString = "";
				al.Insert(0, ci);
			}
			
			return al;
		}
	}
#endregion
	
}

