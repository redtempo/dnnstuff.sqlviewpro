using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;
using Microsoft.ApplicationBlocks.Data;

namespace DNNStuff.SQLViewPro
{
    public class SqlDataProvider : DataProvider
    {
        #region Constructors

        public SqlDataProvider()
        {
            // Read the configuration specific information for this provider
            var providerConfiguration = ProviderConfiguration.GetProviderConfiguration("data");
            var objProvider = (Provider) (providerConfiguration.Providers[providerConfiguration.DefaultProvider]);

            // Read the attributes for this provider
            ConnectionString = Config.GetConnectionString();

            ProviderPath = objProvider.Attributes["providerPath"];

            ObjectQualifier = objProvider.Attributes["objectQualifier"];
            if (ObjectQualifier != "" && ObjectQualifier.EndsWith("_") == false)
            {
                ObjectQualifier += "_";
            }

            DatabaseOwner = objProvider.Attributes["databaseOwner"];
            if (DatabaseOwner != "" && DatabaseOwner.EndsWith(".") == false)
            {
                DatabaseOwner += ".";
            }
        }

        public string ConnectionString { get; }

        public string ProviderPath { get; }

        public string ObjectQualifier { get; }

        public string DatabaseOwner { get; }

        public string CmdPrefix => "DNNStuff_SQLViewPro_";

        #endregion

        public override DataSet RunQuery(string queryText, string dataSetName)
        {
            // make replacements for objectQualifier and databaseOwner
            queryText = Regex.Replace(queryText, "{oQ}|{objectQualifier}", ObjectQualifier, RegexOptions.IgnoreCase);
            queryText = Regex.Replace(queryText, "{dO}|{databaseOwner}", DatabaseOwner, RegexOptions.IgnoreCase);
            using (var cn = new SqlConnection(ConnectionString))
            {
                using (var cmd = SqlHelper.CreateCommand(cn, queryText, null))
                {
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.Text;
                    var adapter = new SqlDataAdapter(cmd);
                    cn.Open();
                    var ds = new DataSet(dataSetName);
                    adapter.Fill(ds);
                    return ds;
                }
            }
        }

        // report set
        public override IDataReader GetReportSet(int reportSetId)
        {
            return SqlHelper.ExecuteReader(ConnectionString,
                DatabaseOwner + ObjectQualifier + CmdPrefix + "GetReportSet", reportSetId);
        }

        public override IDataReader GetReportSetByModule(int moduleId)
        {
            return SqlHelper.ExecuteReader(ConnectionString,
                DatabaseOwner + ObjectQualifier + CmdPrefix + "GetReportSetByModule", moduleId);
        }

        public override int UpdateReportSet(int moduleId, int reportSetId, string reportSetName, string reportSetTheme,
            int reportSetConnectionId, string reportSetHeaderText, string reportSetFooterText, bool reportSetDebug,
            string runCaption, string backCaption, string parameterLayout, bool alwaysShowParameters, string renderMode,
            bool autoRun, string reportSetConfig)
        {
            return
                Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString,
                    DatabaseOwner + ObjectQualifier + CmdPrefix + "UpdateReportSet", moduleId, reportSetId,
                    reportSetName, reportSetTheme, reportSetConnectionId, reportSetHeaderText, reportSetFooterText,
                    reportSetDebug, runCaption, backCaption, parameterLayout, alwaysShowParameters, renderMode, autoRun,
                    reportSetConfig));
        }

        public override IDataReader ListReportSet(int portalId)
        {
            return SqlHelper.ExecuteReader(ConnectionString,
                DatabaseOwner + ObjectQualifier + CmdPrefix + "ListReportSet", portalId);
        }

        public override void DeleteReportSet(int moduleId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + CmdPrefix + "DeleteReportSet",
                moduleId);
        }

        public override IDataReader GetReportSetReport(int reportSetId)
        {
            return SqlHelper.ExecuteReader(ConnectionString,
                DatabaseOwner + ObjectQualifier + CmdPrefix + "GetReportSetReport", reportSetId);
        }

        public override IDataReader GetReportSetParameter(int reportSetId)
        {
            return SqlHelper.ExecuteReader(ConnectionString,
                DatabaseOwner + ObjectQualifier + CmdPrefix + "GetReportSetParameter", reportSetId);
        }

        // report
        public override IDataReader GetReport(int reportId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + CmdPrefix + "GetReport",
                reportId);
        }

        public override int UpdateReport(int reportSetId, int reportId, string reportTypeId, string reportName,
            string reportTheme, int reportConnectionId, string reportHeaderText, string reportFooterText,
            string reportCommand, string reportConfig, int reportOrder, int reportDrilldownReportId,
            string reportDrilldownFieldname, string reportNoItemsText, string reportPageTitle,
            int reportCommandCacheTimeout, string reportMetaDescription, string reportCommandCacheScheme)
        {
            return
                Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString,
                    DatabaseOwner + ObjectQualifier + CmdPrefix + "UpdateReport", reportSetId, reportId, reportTypeId,
                    reportName, reportTheme, reportConnectionId, reportHeaderText, reportFooterText, reportCommand,
                    reportConfig, reportOrder, reportDrilldownReportId, reportDrilldownFieldname, reportNoItemsText,
                    reportPageTitle, reportCommandCacheTimeout, reportMetaDescription, reportCommandCacheScheme));
        }

        public override IDataReader ListReport(int reportSetId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + CmdPrefix + "ListReport",
                reportSetId);
        }

        public override IDataReader ListReportDrilldown(int reportId)
        {
            return SqlHelper.ExecuteReader(ConnectionString,
                DatabaseOwner + ObjectQualifier + CmdPrefix + "ListReportDrilldown", reportId);
        }

        public override void UpdateReportOrder(int reportId, int increment)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString,
                DatabaseOwner + ObjectQualifier + CmdPrefix + "UpdateReportOrder", reportId, increment);
        }

        public override void DeleteReport(int reportId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + CmdPrefix + "DeleteReport",
                reportId);
        }

        // Parameter
        public override IDataReader GetParameter(int parameterId)
        {
            return SqlHelper.ExecuteReader(ConnectionString,
                DatabaseOwner + ObjectQualifier + CmdPrefix + "GetParameter", parameterId);
        }

        public override int UpdateParameter(int reportSetId, int parameterId, string parameterName,
            string parameterCaption, string parameterTypeId, string parameterConfig, int parameterOrder)
        {
            return
                Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString,
                    DatabaseOwner + ObjectQualifier + CmdPrefix + "UpdateParameter", reportSetId, parameterId,
                    parameterName, parameterCaption, parameterTypeId, parameterConfig, parameterOrder));
        }

        public override IDataReader ListParameter(int reportSetId)
        {
            return SqlHelper.ExecuteReader(ConnectionString,
                DatabaseOwner + ObjectQualifier + CmdPrefix + "ListParameter", reportSetId);
        }

        public override void UpdateParameterOrder(int parameterId, int increment)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString,
                DatabaseOwner + ObjectQualifier + CmdPrefix + "UpdateParameterOrder", parameterId, increment);
        }

        public override void DeleteParameter(int parameterId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + CmdPrefix + "DeleteParameter",
                parameterId);
        }

        // report type
        public override IDataReader ListReportType()
        {
            return SqlHelper.ExecuteReader(ConnectionString,
                DatabaseOwner + ObjectQualifier + CmdPrefix + "ListReportType");
        }

        public override IDataReader GetReportType(string reportTypeId)
        {
            return SqlHelper.ExecuteReader(ConnectionString,
                DatabaseOwner + ObjectQualifier + CmdPrefix + "GetReportType", reportTypeId);
        }

        // Parameter type
        public override IDataReader ListParameterType()
        {
            return SqlHelper.ExecuteReader(ConnectionString,
                DatabaseOwner + ObjectQualifier + CmdPrefix + "ListParameterType");
        }

        public override IDataReader GetParameterType(string parameterTypeId)
        {
            return SqlHelper.ExecuteReader(ConnectionString,
                DatabaseOwner + ObjectQualifier + CmdPrefix + "GetParameterType", parameterTypeId);
        }

        // Connection
        public override IDataReader GetConnection(int connectionId)
        {
            return SqlHelper.ExecuteReader(ConnectionString,
                DatabaseOwner + ObjectQualifier + CmdPrefix + "GetConnection", connectionId);
        }

        public override int UpdateConnection(int portalId, int connectionId, string connectionName,
            string connectionString)
        {
            return
                Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString,
                    DatabaseOwner + ObjectQualifier + CmdPrefix + "UpdateConnection", portalId, connectionId,
                    connectionName, connectionString));
        }

        public override void DeleteConnection(int connectionId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + CmdPrefix + "DeleteConnection",
                connectionId);
        }

        public override IDataReader ListConnection(int portalId)
        {
            return SqlHelper.ExecuteReader(ConnectionString,
                DatabaseOwner + ObjectQualifier + CmdPrefix + "ListConnection", portalId);
        }
    }
}


