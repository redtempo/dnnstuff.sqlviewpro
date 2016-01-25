using System.Collections.Generic;
using DotNetNuke.Entities.Portals;

namespace DNNStuff.SQLViewPro.Controls
{
	public abstract class ParameterControlBase : System.Web.UI.UserControl
	{
		
#region Public Methods
		public ParameterInfo Settings {get; set;}
		public PortalSettings PortalSettings {get; set;}

        public string Unique(string key)
		{
			return key + "_" + Settings.ParameterId.ToString();
		}
#endregion
		
#region Abstract Methods
		
		public abstract List<string> Values {get; set;}
		public virtual bool MultiValued => false;
	    public abstract void LoadRuntimeSettings();
		public virtual System.Collections.Specialized.StringDictionary ExtraValues => null;

	    #endregion

        #region Events
        // events
        public event OnRunEventHandler OnRun;
        public delegate void OnRunEventHandler(object o);

        protected void Run(ParameterControlBase o)
        {
            var onRunEventHandler = this.OnRun;
            if (onRunEventHandler != null)
            {
                onRunEventHandler(o);
            }
        }
        #endregion

    }
}

