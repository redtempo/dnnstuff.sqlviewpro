using System;
using System.Collections;

namespace DNNStuff.SQLViewPro
{
	
	[Serializable()]public class DrilldownState
	{
	    public int FromReportId { get; set; } = -1;
	    public string FromReportColumn { get; set; } = "";
        public ArrayList Parameters { get; set; } = new ArrayList();
		public ReportSetInfo ReportSet {get; set;}
		public int PortalId {get; set;}
		public int ModuleId {get; set;}
		public int TabId {get; set;}
		public int UserId {get; set;}
		
		public DrilldownState(int fromReportId, string fromReportColumn, ArrayList parameters)
		{
			FromReportId = fromReportId;
			FromReportColumn = fromReportColumn;
			Parameters = parameters;
		}
		public DrilldownState(ArrayList parameters)
		{
			Parameters = parameters;
		}
	}
	
}

