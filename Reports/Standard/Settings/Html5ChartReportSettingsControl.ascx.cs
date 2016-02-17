using System.Xml.Serialization;
using DNNStuff.SQLViewPro.Controls;

namespace DNNStuff.SQLViewPro.StandardReports
{
	
	public partial class Html5ChartReportSettingsControl : ReportSettingsControlBase
	{
		
		protected override string LocalResourceFile
		{
			get
			{
				return ResolveUrl("App_LocalResources/Html5ChartReportSettingsControl");
			}
		}

	    private Html5ChartReportSettings ReportSettings { get; set; } = new Html5ChartReportSettings();

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
			
			
			InitSettings();
			
		}
		
#endregion
		
#region  Base Method Implementations
		private void InitSettings()
		{
			
			ddlChartType.SelectedValue = ReportSettings.ChartType;
			
			// Load custom settings (if any)
			cpvMain.Settings = Html5ChartSettings();
			cpvMain.LocalResourceFile = LocalResourceFile;
			cpvMain.InitialValues = ReportSettings;
			
			if (Page.IsPostBack)
			{
				var postbackControl = ControlHelpers.GetPostBackControl(Page);
				if (postbackControl != null)
				{
					if (postbackControl.ID != "cmdUpdate")
					{
						cpvMain.Filter = ReportSettings.ChartType;
						cpvMain.InitializeValues();
					}
				}
			}
			
			cpvMain.RenderProperties();
			
			
		}
		
		public override string UpdateSettings()
		{
			
			var obj = new Html5ChartReportSettings();
			obj.ChartType = ddlChartType.SelectedValue;
			
			// update properties
			cpvMain.SetProperties(obj);
			
			return Serialization.SerializeObject(obj, typeof(Html5ChartReportSettings));
			
		}
		
		public override void LoadSettings(string settings)
		{
			ReportSettings = new Html5ChartReportSettings();
			if (!string.IsNullOrEmpty(settings))
			{
				ReportSettings = (Html5ChartReportSettings) (Serialization.DeserializeObject(settings, typeof(Html5ChartReportSettings)));
			}
		}
		
		private Settings Html5ChartSettings()
		{
			if (System.IO.File.Exists(SettingsFilename()))
			{
				var settings = Settings.Load(SettingsFilename());
				return settings;
			}
			return new Settings();
		}
		
		private string SettingsFilename()
		{
			var propertiesFolder = ResolveUrl("Properties");
			return System.IO.Path.Combine(MapPath(propertiesFolder), "Html5ChartReport.xml");
		}
		
#endregion
		
		protected void ddlChartType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			cpvMain.Filter = ddlChartType.SelectedValue;
			cpvMain.RenderProperties();
		}
		
	}
	
#region  Settings
	
	[XmlRootAttribute(ElementName = "Settings", IsNullable = false)]public class Html5ChartReportSettings
	{
		public Html5ChartReportSettings()
		{
			CanvasBorderColor = "000000";
			CanvasBorderThickness = "1";
			ChartTopMargin = "10";
			ChartRightMargin = "10";
			ChartBottomMargin = "10";
			ChartLeftMargin = "10";
			ShowNames = true;
			ShowValues = true;
			ShowLimits = true;
			RotateNames = false;
			Animation = true;
			ShowColumnShadow = true;
		}

	    public string ChartType { get; set; } = "Column2D";

	    // colorset
		public string ColorSet {get; set;}
		public string CustomColorSet {get; set;}
		
		// chart settings
	    public int ChartHeight { get; set; } = 400;

	    public int ChartWidth { get; set; } = 600;

	    //Background Properties
		public string BgColor {get; set;}
		public string BgAlpha {get; set;}
		public string BgSWF {get; set;}
		//Canvas Properties
		public string CanvasBgColor {get; set;}
		public string CanvasBgAlpha {get; set;}
		public string CanvasBorderColor {get; set;}
		public string CanvasBorderThickness {get; set;}
		public string CanvasBaseColor {get; set;}
		public string CanvasBaseDepth {get; set;}
		public string CanvasBgDepth {get; set;}
		public bool ShowCanvasBg {get; set;}
		public bool ShowCanvasBase {get; set;}
		//Chart and Axis Titles
		public string Caption {get; set;}
		public string SubCaption {get; set;}
		public string XAxisName {get; set;}
		public string YAxisName {get; set;}
		//Chart Numerical Limits
		public string YAxisMinValue {get; set;}
		public string YAxisMaxValue {get; set;}
		//Generic Properties
		public bool ShowNames {get; set;}
		public bool ShowValues {get; set;}
		public bool ShowLimits {get; set;}
		public bool RotateNames {get; set;}
		public bool Animation {get; set;}
		public bool ShowColumnShadow {get; set;}
		public bool ShowPercentageValues {get; set;}
		public bool ShowPercentageInLabel {get; set;}
		public bool ShowBarShadow {get; set;}
		public bool ShowLegend {get; set;}
		//Font Properties
		public string BaseFont {get; set;}
		public string BaseFontSize {get; set;}
		public string BaseFontColor {get; set;}
		public string OutCnvBaseFont {get; set;}
		public string OutCnvBaseFontSize {get; set;}
		public string OutCnvBaseFontColor {get; set;}
		//Number Formatting Options
		public string NumberPrefix {get; set;}
		public string NumberSuffix {get; set;}
		public bool FormatNumber {get; set;}
		public string FormatNumberScale {get; set;}
		public string DecimalSeparator {get; set;}
		public string ThousandSeparator {get; set;}
		public string DecimalPrecision {get; set;}
		public string DivLineDecimalPrecision {get; set;}
		public string LimitsDecimalPrecision {get; set;}
		//Zero Plane
		public string ZeroPlaneThickness {get; set;}
		public string ZeroPlaneColor {get; set;}
		public string ZeroPlaneAlpha {get; set;}
		public bool ZeroPlaneShowBorder {get; set;}
		public string ZeroPlaneBorderColor {get; set;}
		//Divisional Lines
		public string NumDivLines {get; set;}
		public string DivLineColor {get; set;}
		public string DivLineThickness {get; set;}
		public string DivLineAlpha {get; set;}
		public bool ShowDivLineValue {get; set;}
		//Divisional Lines (Horizontal)
		public bool ShowAlternateHGridColor {get; set;}
		public string AlternateHGridColor {get; set;}
		public string AlternateHGridAlpha {get; set;}
		public string NumHDivLines {get; set;}
		public string HDivLineColor {get; set;}
		public string HDivLineThickness {get; set;}
		public string HDivLineAlpha {get; set;}
		//Divisional Lines (Vertical)
		public bool ShowAlternateVGridColor {get; set;}
		public string AlternateVGridColor {get; set;}
		public string AlternateVGridAlpha {get; set;}
		public string NumVDivLines {get; set;}
		public string VDivLineColor {get; set;}
		public string VDivLineThickness {get; set;}
		public string VDivLineAlpha {get; set;}
		//Hover Caption Properties
		public bool ShowHoverCap {get; set;}
		public string HoverCapBgColor {get; set;}
		public string HoverCapBorderColor {get; set;}
		public string HoverCapSepChar {get; set;}
		//Chart Margins
		public string ChartLeftMargin {get; set;}
		public string ChartRightMargin {get; set;}
		public string ChartTopMargin {get; set;}
		public string ChartBottomMargin {get; set;}
		//Pie Properties
		public string PieRadius {get; set;}
		public string PieBorderThickness {get; set;}
		public string PieBorderAlpha {get; set;}
		public string PieFillAlpha {get; set;}
		public string PieSliceDepth {get; set;}
		public string PieYScale {get; set;}
		//Name/Value display distance control
		public string SlicingDistance {get; set;}
		public string NameTBDistance {get; set;}
		//Line Properties
		public string LineColor {get; set;}
		public string LineThickness {get; set;}
		public string LineAlpha {get; set;}
		//Shadow Properties
		public bool ShowShadow {get; set;}
		public string ShadowColor {get; set;}
		public string ShadowAlpha {get; set;}
		public string ShadowXShift {get; set;}
		public string ShadowYShift {get; set;}
		public string ShadowThickness {get; set;}
		//Anchor properties
		public bool ShowAnchors {get; set;}
		public string AnchorSides {get; set;}
		public string AnchorRadius {get; set;}
		public string AnchorBorderColor {get; set;}
		public string AnchorBorderThickness {get; set;}
		public string AnchorBgColor {get; set;}
		public string AnchorBgAlpha {get; set;}
		public string AnchorAlpha {get; set;}
		//Area Properties
		public bool ShowAreaBorder {get; set;}
		public string AreaBorderThickness {get; set;}
		public string AreaBorderColor {get; set;}
		public string AreaBgColor {get; set;}
		public string AreaAlpha {get; set;}
		
	}
	
#endregion
	
}

