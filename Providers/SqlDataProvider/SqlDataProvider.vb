Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.ApplicationBlocks.Data
Imports DotNetNuke

Namespace DNNStuff.SQLViewPro

    Public Class SqlDataProvider
        Inherits DataProvider

#Region "Private Members"
        Private Const ProviderType As String = "data"

        Private _providerConfiguration As Framework.Providers.ProviderConfiguration = Framework.Providers.ProviderConfiguration.GetProviderConfiguration(ProviderType)
        Private _connectionString As String
        Private _providerPath As String
        Private _objectQualifier As String
        Private _databaseOwner As String
#End Region

#Region "Constructors"

        Public Sub New()

            ' Read the configuration specific information for this provider
            Dim objProvider As Framework.Providers.Provider = CType(_providerConfiguration.Providers(_providerConfiguration.DefaultProvider), Framework.Providers.Provider)

            ' Read the attributes for this provider
            _connectionString = DotNetNuke.Common.Utilities.Config.GetConnectionString()

            _providerPath = objProvider.Attributes("providerPath")

            _objectQualifier = objProvider.Attributes("objectQualifier")
            If _objectQualifier <> "" And _objectQualifier.EndsWith("_") = False Then
                _objectQualifier += "_"
            End If

            _databaseOwner = objProvider.Attributes("databaseOwner")
            If _databaseOwner <> "" And _databaseOwner.EndsWith(".") = False Then
                _databaseOwner += "."
            End If

        End Sub

        Public ReadOnly Property ConnectionString() As String
            Get
                Return _connectionString
            End Get
        End Property

        Public ReadOnly Property ProviderPath() As String
            Get
                Return _providerPath
            End Get
        End Property

        Public ReadOnly Property ObjectQualifier() As String
            Get
                Return _objectQualifier
            End Get
        End Property

        Public ReadOnly Property DatabaseOwner() As String
            Get
                Return _databaseOwner
            End Get
        End Property
        Public ReadOnly Property CmdPrefix() As String
            Get
                Return "DNNStuff_SQLViewPro_"
            End Get
        End Property
#End Region

        Public Overrides Function RunQuery(ByVal queryText As String, ByVal dataSetName As String) As DataSet
            ' make replacements for objectQualifier and databaseOwner
            queryText = Text.RegularExpressions.Regex.Replace(queryText, "{oQ}|{objectQualifier}", ObjectQualifier, Text.RegularExpressions.RegexOptions.IgnoreCase)
            queryText = Text.RegularExpressions.Regex.Replace(queryText, "{dO}|{databaseOwner}", DatabaseOwner, Text.RegularExpressions.RegexOptions.IgnoreCase)
            Using cn As SqlConnection = New SqlConnection(ConnectionString)
                Using cmd As SqlCommand = SqlHelper.CreateCommand(cn, queryText, Nothing)
                    cmd.CommandTimeout = 0
                    cmd.CommandType = CommandType.Text
                    Dim adapter As SqlDataAdapter = New SqlDataAdapter(cmd)
                    cn.Open()
                    Dim ds As DataSet = New DataSet(dataSetName)
                    adapter.Fill(ds)
                    Return ds
                End Using
            End Using
        End Function

        ' report set
        Public Overrides Function GetReportSet(ByVal ReportSetId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & CmdPrefix & "GetReportSet", ReportSetId), IDataReader)
        End Function
        Public Overrides Function GetReportSetByModule(ByVal ModuleId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & CmdPrefix & "GetReportSetByModule", ModuleId), IDataReader)
        End Function
        Public Overrides Function UpdateReportSet(ByVal ModuleId As Integer, ByVal ReportSetId As Integer, ByVal ReportSetName As String, ByVal ReportSetTheme As String, ByVal ReportSetConnectionId As Integer, ByVal ReportSetHeaderText As String, ByVal ReportSetFooterText As String, ByVal ReportSetDebug As Boolean, ByVal RunCaption As String, ByVal BackCaption As String, ByVal ParameterLayout As String, ByVal AlwaysShowParameters As Boolean, ByVal RenderMode As String, ByVal AutoRun As Boolean, ByVal ReportSetConfig As String) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & CmdPrefix & "UpdateReportSet", ModuleId, ReportSetId, ReportSetName, ReportSetTheme, ReportSetConnectionId, ReportSetHeaderText, ReportSetFooterText, ReportSetDebug, RunCaption, BackCaption, ParameterLayout, AlwaysShowParameters, RenderMode, AutoRun, ReportSetConfig), Integer)
        End Function

        Public Overrides Function ListReportSet(ByVal PortalId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & CmdPrefix & "ListReportSet", PortalId), IDataReader)
        End Function

        Public Overrides Sub DeleteReportSet(ByVal ModuleId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & CmdPrefix & "DeleteReportSet", ModuleId)
        End Sub

        Public Overrides Function GetReportSetReport(ByVal ReportSetId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & CmdPrefix & "GetReportSetReport", ReportSetId), IDataReader)
        End Function

        Public Overrides Function GetReportSetParameter(ByVal ReportSetId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & CmdPrefix & "GetReportSetParameter", ReportSetId), IDataReader)
        End Function

        ' report
        Public Overrides Function GetReport(ByVal ReportId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & CmdPrefix & "GetReport", ReportId), IDataReader)
        End Function

        Public Overrides Function UpdateReport(ByVal ReportSetId As Integer, ByVal ReportId As Integer, ByVal ReportTypeId As String, ByVal ReportName As String, ByVal ReportTheme As String, ByVal ReportConnectionId As Integer, ByVal ReportHeaderText As String, ByVal ReportFooterText As String, _
                ByVal ReportCommand As String, ByVal ReportConfig As String, ByVal ReportOrder As Integer, ByVal ReportDrilldownReportId As Integer, ByVal ReportDrilldownFieldname As String, ByVal ReportNoItemsText As String, ByVal ReportPageTitle As String, ByVal ReportCommandCacheTimeout As Integer, ByVal ReportMetaDescription As String) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & CmdPrefix & "UpdateReport", ReportSetId, ReportId, ReportTypeId, ReportName, ReportTheme, ReportConnectionId, ReportHeaderText, ReportFooterText, _
                ReportCommand, ReportConfig, ReportOrder, ReportDrilldownReportId, ReportDrilldownFieldname, ReportNoItemsText, ReportPageTitle, ReportCommandCacheTimeout, ReportMetaDescription), Integer)
        End Function

        Public Overrides Function ListReport(ByVal ReportSetId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & CmdPrefix & "ListReport", ReportSetId), IDataReader)
        End Function

        Public Overrides Function ListReportDrilldown(ByVal ReportId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & CmdPrefix & "ListReportDrilldown", ReportId), IDataReader)
        End Function

        Public Overrides Sub UpdateReportOrder(ByVal ReportId As Integer, ByVal Increment As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & CmdPrefix & "UpdateReportOrder", ReportId, Increment)
        End Sub
        Public Overrides Sub DeleteReport(ByVal ReportId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & CmdPrefix & "DeleteReport", ReportId)
        End Sub

        ' Parameter
        Public Overrides Function GetParameter(ByVal ParameterId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & CmdPrefix & "GetParameter", ParameterId), IDataReader)
        End Function

        Public Overrides Function UpdateParameter(ByVal ReportSetId As Integer, ByVal ParameterId As Integer, ByVal ParameterName As String, ByVal ParameterCaption As String, _
            ByVal ParameterTypeId As String, ByVal ParameterConfig As String, ByVal ParameterOrder As Integer) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & CmdPrefix & "UpdateParameter", ReportSetId, ParameterId, ParameterName, ParameterCaption, _
                ParameterTypeId, ParameterConfig, ParameterOrder), Integer)
        End Function
        Public Overrides Function ListParameter(ByVal ReportSetId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & CmdPrefix & "ListParameter", ReportSetId), IDataReader)
        End Function

        Public Overrides Sub UpdateParameterOrder(ByVal ParameterId As Integer, ByVal Increment As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & CmdPrefix & "UpdateParameterOrder", ParameterId, Increment)
        End Sub
        Public Overrides Sub DeleteParameter(ByVal ParameterId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & CmdPrefix & "DeleteParameter", ParameterId)
        End Sub

        ' report type
        Public Overrides Function ListReportType() As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & CmdPrefix & "ListReportType"), IDataReader)
        End Function

        Public Overrides Function GetReportType(ByVal ReportTypeId As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & CmdPrefix & "GetReportType", ReportTypeId), IDataReader)
        End Function

        ' Parameter type
        Public Overrides Function ListParameterType() As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & CmdPrefix & "ListParameterType"), IDataReader)
        End Function

        Public Overrides Function GetParameterType(ByVal ParameterTypeId As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & CmdPrefix & "GetParameterType", ParameterTypeId), IDataReader)
        End Function

        ' Connection
        Public Overrides Function GetConnection(ByVal ConnectionId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(Me.ConnectionString, DatabaseOwner & ObjectQualifier & CmdPrefix & "GetConnection", ConnectionId), IDataReader)
        End Function

        Public Overrides Function UpdateConnection(ByVal PortalId As Integer, ByVal ConnectionId As Integer, ByVal ConnectionName As String, ByVal ConnectionString As String) As Integer
            Return CType(SqlHelper.ExecuteScalar(Me.ConnectionString, DatabaseOwner & ObjectQualifier & CmdPrefix & "UpdateConnection", PortalId, ConnectionId, ConnectionName, ConnectionString), Integer)
        End Function
        Public Overrides Sub DeleteConnection(ByVal ConnectionId As Integer)
            SqlHelper.ExecuteNonQuery(Me.ConnectionString, DatabaseOwner & ObjectQualifier & CmdPrefix & "DeleteConnection", ConnectionId)
        End Sub
        Public Overrides Function ListConnection(ByVal PortalId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(Me.ConnectionString, DatabaseOwner & ObjectQualifier & CmdPrefix & "ListConnection", PortalId), IDataReader)
        End Function
    End Class

End Namespace
