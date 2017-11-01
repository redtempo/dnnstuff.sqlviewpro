

using System;
using System.Web.UI.WebControls;
using System.Data;




namespace DNNStuff.SQLViewPro
{
	public class SQLUtil
	{
		
		public static void AddOptionsFromQuery(ListControl list, string queryText, string connectionString, string defaultValue, int cacheTimeout)
		{
			
			var ds = Services.Data.Query.RetrieveData(queryText, connectionString, cacheTimeout, "Absolute", null);
			
			if (ds.Tables.Count > 0)
			{
				if (ds.Tables[0].Columns.Count == 1)
				{
					var with_1 = list;
					with_1.DataValueField = ds.Tables[0].Columns[0].ColumnName;
					with_1.DataTextField = ds.Tables[0].Columns[0].ColumnName;
					with_1.DataSource = ds.Tables[0].DefaultView;
					with_1.DataBind();
				}
				else if (ds.Tables[0].Columns.Count > 1)
				{
					var with_2 = list;
					with_2.DataValueField = ds.Tables[0].Columns[0].ColumnName;
					with_2.DataTextField = ds.Tables[0].Columns[1].ColumnName;
					with_2.DataSource = ds.Tables[0].DefaultView;
					with_2.DataBind();
				}
			}
		}
		public static void AddOptionsFromList(ListControl list, string options, string defaultValue = "")
		{
			// delims
			const string VALUE_DELIM = "|";
			const string FIELD_DELIM = "\n";
			
			var li = default(ListItem);
			var optionArray = options.Replace(Environment.NewLine, FIELD_DELIM).Split(FIELD_DELIM[0]);
			
			var insertPosition = 0;
			foreach (var o in optionArray)
			{
				li = new ListItem();
				li.Value = (string) (o.Split(VALUE_DELIM[0])[0]);
				if (o.Split(VALUE_DELIM[0]).GetUpperBound(0) > 0)
				{
					li.Text = (string) (o.Split(VALUE_DELIM[0])[1]);
				}
				else
				{
					li.Text = li.Value;
				}
				
				if (li.Text == defaultValue)
				{
					li.Selected = true;
				}
				
				// items are inserted to appear ahead of any databound options
				list.Items.Insert(insertPosition, li);
				insertPosition++;
			}
			
		}
		
		public static bool ContainsCatchWords(string queryText)
		{
			// valid query so that it doesn't contain malicious code
			var CatchWords = new string[] {" INSERT ", " UPDATE ", " DELETE ", " DROP ", " SELECT INTO "};
			var upperQuery = " " + queryText.ToUpper() + " ";
			var DisableCatchWords = false;
			var IsValid = true;
			
			if (System.Configuration.ConfigurationManager.AppSettings["DNNStuff:SQLViewPro:DisableCatchWords"] == null)
			{
				DisableCatchWords = false;
			}
			else
			{
				DisableCatchWords = System.Configuration.ConfigurationManager.AppSettings["DNNStuff:SQLViewPro:DisableCatchWords"].ToString().ToUpper() == "TRUE";
			}
			
			if (!DisableCatchWords)
			{
				foreach (var w in CatchWords)
				{
					if (upperQuery.IndexOf(w) > 0)
					{
						IsValid = false;
						break;
					}
				}
			}
			return IsValid;
			
		}
		
	}
}
