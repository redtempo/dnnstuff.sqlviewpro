using DNNStuff.SQLViewPro.Controls;
using System.Web.UI.WebControls;
using DNNStuff.SQLViewPro.Services.Data;

namespace DNNStuff.SQLViewPro.StandardParameters
{
	public abstract class ListParameterControlBase : ParameterControlBase
	{
		
#region Option Loading
		protected void AddOptions(ListControl list, ListParameterSettings customParameterSettings, ParameterInfo parameterSettings)
		{
			// add options from both if necessary
			if (customParameterSettings.Command.Length > 0)
			{
				var connectionString = default(string);
				if (customParameterSettings.ConnectionId < 0)
				{
					// get report set connection
					var objReportSetController = new ReportSetController();
					var objReportSetInfo = objReportSetController.GetReportSet(parameterSettings.ReportSetId);
					connectionString = objReportSetInfo.ReportSetConnectionString;
				}
				else
				{
					var objConnectionInfo = ConnectionController.GetConnection(customParameterSettings.ConnectionId);
					connectionString = objConnectionInfo.ConnectionString;
				}
				SQLUtil.AddOptionsFromQuery(list, ReplaceOptionTokens(customParameterSettings.Command), connectionString, customParameterSettings.Default, customParameterSettings.CommandCacheTimeout);
			}
			if (customParameterSettings.List.Length > 0)
			{
				SQLUtil.AddOptionsFromList(list, customParameterSettings.List, customParameterSettings.Default);
			}
		}
		
		private string ReplaceOptionTokens(string s)
		{
			return TokenReplacement.ReplaceTokens(s, null, null);
		}
		
		protected void SelectDefaults(ListControl list, ListParameterSettings customParameterSettings, bool multiAllowed)
		{
			var defaultValues = customParameterSettings.Default.Split(',');
			
			if (defaultValues.Length > 0)
			{
				foreach (var defaultValue in defaultValues)
				{
					var li = list.Items.FindByValue(defaultValue);
					if (li != null)
					{
						li.Selected = true;
					}
					else
					{
						// try text
						li = list.Items.FindByText(defaultValue);
						if (li != null)
						{
							li.Selected = true;
						}
					}
					if (!multiAllowed)
					{
						break; // first one only
					}
				}
			}
			
		}
#endregion
		
		
	}
}

