'***************************************************************************/
'* ChartReportSettings.ascx.vb
'*
'* Copyright (c) 2004 by DNNStuff.
'* All rights reserved.
'*
'* Date:        August 9, 2004
'* Author:      Richard Edwards
'* Description: Grid Report Settings Handler
'*************/

Imports System.Configuration
Imports System.Xml.Serialization
Imports DNNStuff.SQLViewPro.Controls
Imports DNNStuff.SQLViewPro.Serialization
Imports System.Collections.Generic

Namespace DNNStuff.SQLViewPro.StandardReports

    Partial Class FusionChartReportSettingsControl
        Inherits ReportSettingsControlBase

        Protected Overrides ReadOnly Property LocalResourceFile() As String
            Get
                Return ResolveUrl("App_LocalResources/FusionChartReportSettingsControl")
            End Get
        End Property

        Private Property ReportSettings As FusionChartReportSettings = New FusionChartReportSettings

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()


            InitSettings()

        End Sub

#End Region

#Region " Base Method Implementations"
        Private Sub InitSettings()

            ddlChartType.SelectedValue = reportSettings.ChartType

            ' Load custom settings (if any)
            cpvMain.Settings = FusionChartSettings()
            cpvMain.LocalResourceFile = LocalResourceFile
            cpvMain.InitialValues = reportSettings

            If Page.IsPostBack Then
                Dim postbackControl As Control = ControlHelpers.GetPostBackControl(Page)
                If postbackControl IsNot Nothing Then
                    If Not postbackControl.ID = "cmdUpdate" Then
                        cpvMain.Filter = ReportSettings.ChartType
                        cpvMain.InitializeValues()
                    End If
                End If
            End If

            cpvMain.RenderProperties()


        End Sub

        Public Overrides Function UpdateSettings() As String

            Dim obj As FusionChartReportSettings = New FusionChartReportSettings
            With obj
                .ChartType = ddlChartType.SelectedValue
            End With

            ' update properties
            cpvMain.SetProperties(obj)

            Return SerializeObject(obj, GetType(FusionChartReportSettings))

        End Function

        Public Overrides Sub LoadSettings(ByVal settings As String)
            ReportSettings = New FusionChartReportSettings()
            If Not String.IsNullOrEmpty(settings) Then
                ReportSettings = DirectCast(DeserializeObject(settings, GetType(FusionChartReportSettings)), FusionChartReportSettings)
            End If
        End Sub

        Private Function FusionChartSettings() As Settings
            If IO.File.Exists(SettingsFilename) Then
                Dim settings As Settings = settings.Load(SettingsFilename)
                Return settings
            End If
            Return New Settings
        End Function

        Private Function SettingsFilename() As String
            Dim propertiesFolder As String = ResolveUrl("Properties")
            Return IO.Path.Combine(MapPath(propertiesFolder), "FusionChartReport.xml")
        End Function

#End Region

        Private Sub ddlChartType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlChartType.SelectedIndexChanged
            cpvMain.Filter = ddlChartType.SelectedValue
            cpvMain.RenderProperties()
        End Sub

    End Class

#Region " Settings"

    <XmlRootAttribute(ElementName:="Settings", IsNullable:=False)> _
    Public Class FusionChartReportSettings
        Public Sub New()
            CanvasBorderColor = "000000"
            CanvasBorderThickness = "1"
            ChartTopMargin = "10"
            ChartRightMargin = "10"
            ChartBottomMargin = "10"
            ChartLeftMargin = "10"
            ShowNames = True
            ShowValues = True
            ShowLimits = True
            RotateNames = False
            Animation = True
            ShowColumnShadow = True
        End Sub

        Public Property ChartType() As String = "Column2D"

        ' colorset
        Public Property ColorSet() As String
        Public Property CustomColorSet() As String

        ' chart settings
        Public Property ChartHeight() As Integer = 400
        Public Property ChartWidth() As Integer = 600

        'Background Properties
        Public Property BgColor As String
        Public Property BgAlpha As String
        Public Property BgSWF As String
        'Canvas Properties
        Public Property CanvasBgColor As String
        Public Property CanvasBgAlpha As String
        Public Property CanvasBorderColor As String
        Public Property CanvasBorderThickness As String
        Public Property CanvasBaseColor As String
        Public Property CanvasBaseDepth As String
        Public Property CanvasBgDepth As String
        Public Property ShowCanvasBg As Boolean
        Public Property ShowCanvasBase As Boolean
        'Chart and Axis Titles
        Public Property Caption As String
        Public Property SubCaption As String
        Public Property XAxisName As String
        Public Property YAxisName As String
        'Chart Numerical Limits
        Public Property YAxisMinValue As String
        Public Property YAxisMaxValue As String
        'Generic Properties
        Public Property ShowNames As Boolean
        Public Property ShowValues As Boolean
        Public Property ShowLimits As Boolean
        Public Property RotateNames As Boolean
        Public Property Animation As Boolean
        Public Property ShowColumnShadow As Boolean
        Public Property ShowPercentageValues As Boolean
        Public Property ShowPercentageInLabel As Boolean
        Public Property ShowBarShadow As Boolean
        Public Property ShowLegend As Boolean
        'Font Properties
        Public Property BaseFont As String
        Public Property BaseFontSize As String
        Public Property BaseFontColor As String
        Public Property OutCnvBaseFont As String
        Public Property OutCnvBaseFontSize As String
        Public Property OutCnvBaseFontColor As String
        'Number Formatting Options
        Public Property NumberPrefix As String
        Public Property NumberSuffix As String
        Public Property FormatNumber As Boolean
        Public Property FormatNumberScale As String
        Public Property DecimalSeparator As String
        Public Property ThousandSeparator As String
        Public Property DecimalPrecision As String
        Public Property DivLineDecimalPrecision As String
        Public Property LimitsDecimalPrecision As String
        'Zero Plane
        Public Property ZeroPlaneThickness As String
        Public Property ZeroPlaneColor As String
        Public Property ZeroPlaneAlpha As String
        Public Property ZeroPlaneShowBorder As Boolean
        Public Property ZeroPlaneBorderColor As String
        'Divisional Lines
        Public Property NumDivLines As String
        Public Property DivLineColor As String
        Public Property DivLineThickness As String
        Public Property DivLineAlpha As String
        Public Property ShowDivLineValue As Boolean
        'Divisional Lines (Horizontal)
        Public Property ShowAlternateHGridColor As Boolean
        Public Property AlternateHGridColor As String
        Public Property AlternateHGridAlpha As String
        Public Property NumHDivLines As String
        Public Property HDivLineColor As String
        Public Property HDivLineThickness As String
        Public Property HDivLineAlpha As String
        'Divisional Lines (Vertical)
        Public Property ShowAlternateVGridColor As Boolean
        Public Property AlternateVGridColor As String
        Public Property AlternateVGridAlpha As String
        Public Property NumVDivLines As String
        Public Property VDivLineColor As String
        Public Property VDivLineThickness As String
        Public Property VDivLineAlpha As String
        'Hover Caption Properties
        Public Property ShowHoverCap As Boolean
        Public Property HoverCapBgColor As String
        Public Property HoverCapBorderColor As String
        Public Property HoverCapSepChar As String
        'Chart Margins
        Public Property ChartLeftMargin As String
        Public Property ChartRightMargin As String
        Public Property ChartTopMargin As String
        Public Property ChartBottomMargin As String
        'Pie Properties
        Public Property PieRadius As String
        Public Property PieBorderThickness As String
        Public Property PieBorderAlpha As String
        Public Property PieFillAlpha As String
        Public Property PieSliceDepth As String
        Public Property PieYScale As String
        'Name/Value display distance control
        Public Property SlicingDistance As String
        Public Property NameTBDistance As String
        'Line Properties
        Public Property LineColor As String
        Public Property LineThickness As String
        Public Property LineAlpha As String
        'Shadow Properties
        Public Property ShowShadow As Boolean
        Public Property ShadowColor As String
        Public Property ShadowAlpha As String
        Public Property ShadowXShift As String
        Public Property ShadowYShift As String
        Public Property ShadowThickness As String
        'Anchor properties
        Public Property ShowAnchors As Boolean
        Public Property AnchorSides As String
        Public Property AnchorRadius As String
        Public Property AnchorBorderColor As String
        Public Property AnchorBorderThickness As String
        Public Property AnchorBgColor As String
        Public Property AnchorBgAlpha As String
        Public Property AnchorAlpha As String
        'Area Properties
        Public Property ShowAreaBorder As Boolean
        Public Property AreaBorderThickness As String
        Public Property AreaBorderColor As String
        Public Property AreaBgColor As String
        Public Property AreaAlpha As String

    End Class

#End Region

End Namespace
