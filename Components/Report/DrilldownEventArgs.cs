using System;
using System.Data;

namespace DNNStuff.SQLViewPro.Controls
{
	public class DrilldownEventArgs : EventArgs
	{
		
		public DrilldownEventArgs(int reportId, string name, DataRow value)
		{
			_reportId = reportId;
			_name = name;
			_value = value;
		}
		
#region Properties
		private int _reportId;
		
		public int ReportId
		{
			get
			{
				return _reportId;
			}
			set
			{
				_reportId = value;
			}
		}
		
		private DataRow _value;
		public DataRow Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
			}
		}
		
		private string _name;
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}
#endregion
		
	}
}

