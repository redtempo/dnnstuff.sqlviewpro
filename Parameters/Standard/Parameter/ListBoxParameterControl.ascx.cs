using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace DNNStuff.SQLViewPro.StandardParameters
{
	
	public partial class ListBoxParameterControl : ListParameterControlBase
	{
		
#region  Web Form Designer Generated Code
		
		//This call is required by the Web Form Designer.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent()
		{
			
		}
		
		private void Page_Init(System.Object sender, System.EventArgs e)
		{
			//CODEGEN: This method call is required by the Web Form Designer
			//Do not modify it using the code editor.
			InitializeComponent();
		}
		
#endregion
		
#region  Page
		private void Page_Load(System.Object sender, System.EventArgs e)
		{
			
		}
		
#endregion
		bool _multiValued = false;
		
#region  Base Method Implementations
		public override List<string> Values
		{
			get
			{
				var selected = new List<string>();
				foreach (ListItem li in lbParameter.Items)
				{
					if (li.Selected)
					{
						selected.Add(li.Value.ToString());
					}
				}
				return selected;
			}
			
			set
			{
				if (value.Count > 0)
				{
					lbParameter.SelectedValue = value[0].ToString();
				}
				else
				{
					lbParameter.SelectedValue = "";
				}
			}
		}
		public override bool MultiValued => _multiValued;

	    public override void LoadRuntimeSettings()
		{
			var obj = (ListBoxParameterSettings) (Serialization.DeserializeObject(Settings.ParameterConfig, typeof(ListBoxParameterSettings)));
			_multiValued = obj.MultiSelect;
			AddOptions(lbParameter, obj, Settings);
			if (obj.MultiSelect)
			{
				lbParameter.SelectionMode = ListSelectionMode.Multiple;
				lbParameter.Rows = obj.MultiSelectSize;
			}
			SelectDefaults(lbParameter, obj, obj.MultiSelect);
		}
		
#endregion
		
	}
	
}

