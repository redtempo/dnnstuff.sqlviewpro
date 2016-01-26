
using DotNetNuke.Services.Localization;
	using System;
using System.Data;


using DotNetNuke.Common.Utilities;
	
	
	
	namespace DNNStuff.SQLViewPro.Services.Data
	{
#region  Report Query
		public class Query
		{
			
			public static DataSet RetrieveData(string queryText, string connectionString, int cacheTimeout, string cacheScheme)
			{
				return RetrieveData(queryText, connectionString, "SQLData", "Table", cacheTimeout, cacheScheme);
		}
		
		public static DataSet RetrieveData(string queryText, string connectionString, string dataSetName, string srcTable, int cacheTimeout, string cacheScheme)
		{
			var results = default(DataSet);
			
			// try cache first
			var cacheKey = (string) ("SQLViewPro_Data_" + (queryText.GetHashCode()).ToString());
			if (cacheTimeout > 0)
			{
				results = DataCache.GetCache(cacheKey) as DataSet;
				if (results != null)
				{
					return results;
				}
			}
			
			// if no query just return empty dataset
			if (queryText == "")
			{
				return new DataSet();
			}
			
			// not cached, grab live data
			if (connectionString == "")
			{
				results = DataProvider.Instance().RunQuery(queryText, dataSetName);
			}
			else
			{
				using (var cn = new System.Data.OleDb.OleDbConnection(connectionString))
				{
					using (var cmd = new System.Data.OleDb.OleDbCommand(queryText, cn))
					{
						cmd.CommandTimeout = 0;
						var da = new System.Data.OleDb.OleDbDataAdapter(cmd);
						cn.Open();
						results = new DataSet(dataSetName);
						da.Fill(results, srcTable);
					}
					
				}
				
			}
			
			// update cache
			if (cacheTimeout > 0)
			{
				if (cacheScheme == "Sliding")
				{
					DataCache.SetCache(cacheKey, results, TimeSpan.FromSeconds(cacheTimeout));
				}
				else
				{
					DataCache.SetCache(cacheKey, results, DateTime.Now.AddSeconds(cacheTimeout));
				}
			}
			return results;
			
		}
		
		public static void TestConnection(string connectionString)
		{
			var cn = new System.Data.OleDb.OleDbConnection(connectionString);
			cn.Open();
		}
		
		public static bool IsQueryValid(string queryText, string connectionString, ref string errorMessage)
		{
			var sharedResourceFile = DotNetNuke.Common.Globals.ApplicationPath + "/DesktopModules/DNNStuff - SQLViewPro/App_LocalResources/SharedResources.resx";
			
			// valid query and return appropriate error message
			var msg = "";
			var queryValid = true;
			var catchwordsPassed = true;
			
			// check for valid query
			try
			{
				RetrieveData(Compatibility.ReplaceGenericTokensForTest(queryText), connectionString, 0, "Sliding");
			}
			catch (Exception ex)
			{
				queryValid = false;
				msg = "<br>" + Localization.GetString("QueryTestError", sharedResourceFile) + " : " + ex.Message;
			}
			
			// check for catch words
			if (!SQLUtil.ContainsCatchWords(queryText))
			{
				catchwordsPassed = false;
				msg = msg + "<br>" + Localization.GetString("CatchWordsError", sharedResourceFile);
			}
			
			if (!(queryValid && catchwordsPassed))
			{
				errorMessage = msg;
				return false;
			}
			
			errorMessage = "<br>" + Localization.GetString("QueryTestOK", sharedResourceFile);
			return true;
		}
	}
	
#endregion
}

