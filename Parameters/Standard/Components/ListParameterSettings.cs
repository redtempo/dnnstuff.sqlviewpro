



using System.Xml.Serialization;
using System.Web.UI.WebControls;


namespace DNNStuff.SQLViewPro.StandardParameters
{
	
	[XmlRootAttribute(ElementName = "Settings", IsNullable = false)]public class ListParameterSettings
	{
		public int ConnectionId {get; set;}
		public string Command {get; set;}
		public string List {get; set;}
		public string Default {get; set;}
		private int _CommandCacheTimeout = 60;
		public int CommandCacheTimeout
		{
			get
			{
				return _CommandCacheTimeout;
			}
			set
			{
				_CommandCacheTimeout = value;
			}
		}
	}
	
	[XmlRootAttribute(ElementName = "Settings", IsNullable = false)]public class DropDownListParameterSettings : ListParameterSettings
	{
		private bool _AutoPostback = false;
		public bool AutoPostback
		{
			get
			{
				return _AutoPostback;
			}
			set
			{
				_AutoPostback = value;
			}
		}
	}
	
	[XmlRootAttribute(ElementName = "Settings", IsNullable = false)]public class ListBoxParameterSettings : ListParameterSettings
	{
		private bool _AutoPostback = false;
		public bool AutoPostback
		{
			get
			{
				return _AutoPostback;
			}
			set
			{
				_AutoPostback = value;
			}
		}
		private bool _MultiSelect = false;
		public bool MultiSelect
		{
			get
			{
				return _MultiSelect;
			}
			set
			{
				_MultiSelect = value;
			}
		}
		private int _MultiSelectSize = 5;
		public int MultiSelectSize
		{
			get
			{
				return _MultiSelectSize;
			}
			set
			{
				_MultiSelectSize = value;
			}
		}
	}
	
	[XmlRootAttribute(ElementName = "Settings", IsNullable = false)]public class FlowListParameterSettings : ListParameterSettings
	{
		
		public RepeatLayout RepeatLayout {get; set;}
		public RepeatDirection RepeatDirection {get; set;}
		private int _RepeatColumns = 2;
		public int RepeatColumns
		{
			get
			{
				return _RepeatColumns;
			}
			set
			{
				_RepeatColumns = value;
			}
		}
	}
	
	
}

