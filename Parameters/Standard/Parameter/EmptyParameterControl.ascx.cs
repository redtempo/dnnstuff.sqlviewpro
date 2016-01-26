using System.Collections.Generic;

namespace DNNStuff.SQLViewPro.StandardParameters
{
	
	public partial class EmptyParameterControl : Controls.ParameterControlBase
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
		
#region  Base Method Implementations
		public override List<string> Values
		{
			get
			{
				return new List<string>(new string[] {""});
			}
			set
			{
			}
		}
		
		public override void LoadRuntimeSettings()
		{
			Visible = false;
		}
#endregion
		
	}
	
}

