Imports DotNetNuke

Namespace DNNStuff.SQLViewPro

    Public MustInherit Class DataProvider

#Region "Shared/Static Methods"

        ' singleton reference to the instantiated object 
        Private Shared objProvider As DataProvider = Nothing

        ' constructor
        Shared Sub New()
            CreateProvider()
        End Sub

        ' dynamically create provider
        Private Shared Sub CreateProvider()
            objProvider = CType(Framework.Reflection.CreateObject("data", "DNNStuff.SQLViewPro", "DNNStuff.SQLViewPro"), DataProvider)
        End Sub

        ' return the provider
        Public Shared Shadows Function Instance() As DataProvider
            Return objProvider
        End Function

#End Region

        ' all core methods defined below
        Public MustOverride Function RunQuery(ByVal queryText As String, ByVal dataSetName As String) As DataSet

        ' report set
        Public MustOverride Function GetReportSet(ByVal ReportSetId As Integer) As IDataReader
        Public MustOverride Function GetReportSetByModule(ByVal ModuleId As Integer) As IDataReader
        Public MustOverride Function UpdateReportSet(ByVal ModuleId As Integer, ByVal ReportSetId As Integer, ByVal ReportSetName As String, ByVal ReportSetTheme As String, ByVal ReportSetConnectionId As Integer, ByVal ReportSetHeaderText As String, ByVal ReportSetFooterText As String, _
            ByVal ReportSetDebug As Boolean, ByVal RunCaption As String, ByVal BackCaption As String, ByVal ParameterLayout As String, ByVal AlwaysShowParameters As Boolean, ByVal RenderMode As String, ByVal AutoRun As Boolean, ByVal ReportSetConfig As String) As Integer
        Public MustOverride Function ListReportSet(ByVal PortalId As Integer) As IDataReader
        Public MustOverride Sub DeleteReportSet(ByVal ModuleId As Integer)

        ' report set collections
        Public MustOverride Function GetReportSetReport(ByVal ReportSetId As Integer) As IDataReader
        Public MustOverride Function GetReportSetParameter(ByVal ReportSetId As Integer) As IDataReader
        Public MustOverride Function ListReport(ByVal ReportSetId As Integer) As IDataReader

        ' report
        Public MustOverride Function GetReport(ByVal ReportId As Integer) As IDataReader
        Public MustOverride Function UpdateReport(ByVal ReportSetId As Integer, ByVal ReportId As Integer, ByVal ReportTypeId As String, ByVal ReportName As String, ByVal ReportTheme As String, ByVal ReportConnectionId As Integer, ByVal ReportHeaderText As String, ByVal ReportFooterText As String, _
            ByVal ReportCommand As String, ByVal ReportConfig As String, ByVal ReportOrder As Integer, ByVal ReportDrilldownReportId As Integer, ByVal ReportDrilldownFieldname As String, ByVal ReportNoItemsText As String, ByVal ReportPageTitle As String, ByVal ReportCommandCacheTimeout As Integer, ByVal ReportMetaDescription As String) As Integer
        Public MustOverride Sub DeleteReport(ByVal ReportId As Integer)
        Public MustOverride Sub UpdateReportOrder(ByVal ReportId As Integer, ByVal Increment As Integer)

        ' report collections
        Public MustOverride Function ListReportDrilldown(ByVal ReportId As Integer) As IDataReader

        ' parameter
        Public MustOverride Function GetParameter(ByVal ParameterId As Integer) As IDataReader
        Public MustOverride Function UpdateParameter(ByVal ReportSetId As Integer, ByVal ParameterId As Integer, ByVal ParameterName As String, ByVal ParameterCaption As String, _
            ByVal ParameterTypeId As String, ByVal ParameterConfig As String, ByVal ParameterOrder As Integer) As Integer
        Public MustOverride Function ListParameter(ByVal ReportSetId As Integer) As IDataReader
        Public MustOverride Sub DeleteParameter(ByVal ParameterId As Integer)
        Public MustOverride Sub UpdateParameterOrder(ByVal ParameterId As Integer, ByVal Increment As Integer)

        ' report type
        Public MustOverride Function ListReportType() As IDataReader
        Public MustOverride Function GetReportType(ByVal ReportTypeId As String) As IDataReader

        ' Parameter type
        Public MustOverride Function ListParameterType() As IDataReader
        Public MustOverride Function GetParameterType(ByVal ParameterTypeId As String) As IDataReader

        ' Connection
        Public MustOverride Function GetConnection(ByVal ConnectionId As Integer) As IDataReader
        Public MustOverride Function UpdateConnection(ByVal PortalId As Integer, ByVal ConnectionId As Integer, ByVal ConnectionName As String, ByVal ConnectionString As String) As Integer
        Public MustOverride Sub DeleteConnection(ByVal ConnectionId As Integer)
        Public MustOverride Function ListConnection(ByVal PortalId As Integer) As IDataReader

    End Class

End Namespace