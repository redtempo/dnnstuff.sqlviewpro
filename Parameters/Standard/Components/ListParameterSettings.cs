



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
	    public int CommandCacheTimeout { get; set; } = 60;
	}
	
	[XmlRootAttribute(ElementName = "Settings", IsNullable = false)]public class DropDownListParameterSettings : ListParameterSettings
	{
	    public bool AutoPostback { get; set; } = false;
	}
	
	[XmlRootAttribute(ElementName = "Settings", IsNullable = false)]public class ListBoxParameterSettings : ListParameterSettings
	{
	    public bool AutoPostback { get; set; } = false;

	    public bool MultiSelect { get; set; } = false;

	    public int MultiSelectSize { get; set; } = 5;
	}
	
	[XmlRootAttribute(ElementName = "Settings", IsNullable = false)]public class FlowListParameterSettings : ListParameterSettings
	{
		
		public RepeatLayout RepeatLayout {get; set;}
		public RepeatDirection RepeatDirection {get; set;}
	    public int RepeatColumns { get; set; } = 2;
	}
	
	
}

