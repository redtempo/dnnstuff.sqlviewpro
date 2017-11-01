using System.Data;
using DotNetNuke.Framework;
using System.Collections;

namespace DNNStuff.SQLViewPro
{
	
	public abstract class DataProvider
	{
		
#region Shared/Static Methods
		
		// singleton reference to the instantiated object
		private static DataProvider objProvider = null;
		
		// constructor
		static DataProvider()
		{
			CreateProvider();
		}
		
		// dynamically create provider
		private static void CreateProvider()
		{
			objProvider = (DataProvider) (Reflection.CreateObject("data", "DNNStuff.SQLViewPro", "DNNStuff.SQLViewPro"));
		}
		
		// return the provider
		public static new DataProvider Instance()
		{
			return objProvider;
		}
		
#endregion
		
		// all core methods defined below
		public abstract DataSet RunQuery(string queryText, string dataSetName, Hashtable parameters);
		
		// report set
		public abstract IDataReader GetReportSet(int reportSetId);
		public abstract IDataReader GetReportSetByModule(int moduleId);
		public abstract int UpdateReportSet(int moduleId, int reportSetId, string reportSetName, string reportSetTheme, int reportSetConnectionId, string reportSetHeaderText, string reportSetFooterText, bool reportSetDebug, string runCaption, string backCaption, string parameterLayout, bool alwaysShowParameters, string renderMode, bool autoRun, string reportSetConfig);
		public abstract IDataReader ListReportSet(int portalId);
		public abstract void DeleteReportSet(int moduleId);
		
		// report set collections
		public abstract IDataReader GetReportSetReport(int reportSetId);
		public abstract IDataReader GetReportSetParameter(int reportSetId);
		public abstract IDataReader ListReport(int reportSetId);
		
		// report
		public abstract IDataReader GetReport(int reportId);
		public abstract int UpdateReport(int reportSetId, int reportId, string reportTypeId, string reportName, string reportTheme, int reportConnectionId, string reportHeaderText, string reportFooterText, string reportCommand, string reportConfig, int reportOrder, int reportDrilldownReportId, string reportDrilldownFieldname, string reportNoItemsText, string reportPageTitle, int reportCommandCacheTimeout, string reportMetaDescription, string reportCommandCacheScheme);
		public abstract void DeleteReport(int reportId);
		public abstract void UpdateReportOrder(int reportId, int increment);
		
		// report collections
		public abstract IDataReader ListReportDrilldown(int reportId);
		
		// parameter
		public abstract IDataReader GetParameter(int parameterId);
		public abstract int UpdateParameter(int reportSetId, int parameterId, string parameterName, string parameterCaption, string parameterTypeId, string parameterConfig, int parameterOrder);
		public abstract IDataReader ListParameter(int reportSetId);
		public abstract void DeleteParameter(int parameterId);
		public abstract void UpdateParameterOrder(int parameterId, int increment);
		
		// report type
		public abstract IDataReader ListReportType();
		public abstract IDataReader GetReportType(string reportTypeId);
		
		// Parameter type
		public abstract IDataReader ListParameterType();
		public abstract IDataReader GetParameterType(string parameterTypeId);
		
		// Connection
		public abstract IDataReader GetConnection(int connectionId);
		public abstract int UpdateConnection(int portalId, int connectionId, string connectionName, string connectionString);
		public abstract void DeleteConnection(int connectionId);
		public abstract IDataReader ListConnection(int portalId);
		
	}
	
}
