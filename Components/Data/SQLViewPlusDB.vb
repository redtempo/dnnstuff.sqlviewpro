Imports System
Imports System.Data
Imports DotNetNuke
Imports DotNetNuke.Common.Utilities
Imports System.Xml.Serialization
Imports System.Collections.Generic

Namespace DNNStuff.SQLViewPro

#Region " ReportSet"
    <Serializable()> _
    Public Class ReportSetInfo

        ' initialization
        Public Sub New()
        End Sub

        ' public properties
        Public Property ReportSetId() As Integer = -1
        Public Property ReportSetName() As String
        Public Property ReportSetTheme() As String
        Public Property ReportSetConnectionId() As Integer
        Public Property ReportSetConnectionString() As String
        Public Property ReportSetHeaderText() As String
        Public Property ReportSetFooterText() As String
        Public Property ReportSetDebug() As Boolean
        Public Property RunCaption() As String
        Public Property BackCaption() As String
        Public Property ParameterLayout() As String
        Public Property AlwaysShowParameters() As Boolean
        Public Property ReportCount() As Integer
        Public Property ParameterCount() As Integer
        Public Property AutoRun() As Boolean
        Public Property RenderMode() As String

        Public Property ReportSetConfig() As String

        Private _Config As ReportSetConfig = Nothing
        Public ReadOnly Property Config() As ReportSetConfig
            Get
                If _Config Is Nothing Then
                    If _ReportSetConfig = "" Then
                        _Config = New ReportSetConfig()
                    Else
                        _Config = DirectCast(Serialization.DeserializeObject(_ReportSetConfig, GetType(ReportSetConfig)), ReportSetConfig)
                    End If
                End If
                Return _Config
            End Get
        End Property
    End Class

    <Serializable()> _
    Public Class ReportSetConfig

    End Class

    Public Class ReportSetController

        Public Function GetReportSet(ByVal ReportSetId As Integer) As ReportSetInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetReportSet(ReportSetId), GetType(ReportSetInfo)), ReportSetInfo)
        End Function
        Public Function GetReportSetByModule(ByVal ModuleId As Integer) As ReportSetInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetReportSetByModule(ModuleId), GetType(ReportSetInfo)), ReportSetInfo)
        End Function

        Public Function UpdateReportSet(ByVal ModuleId As Integer, ByVal ReportSetId As Integer, ByVal ReportSetName As String, ByVal ReportSetTheme As String, ByVal ReportSetConnectionId As Integer, ByVal ReportSetHeaderText As String, ByVal ReportSetFooterText As String, ByVal ReportSetDebug As Boolean, ByVal RunCaption As String, ByVal BackCaption As String, ByVal ParameterLayout As String, ByVal AlwaysShowParameters As Boolean, ByVal RenderMode As String, ByVal AutoRun As Boolean, ByVal ReportSetConfig As String) As Integer
            Return DataProvider.Instance().UpdateReportSet(ModuleId, ReportSetId, ReportSetName, ReportSetTheme, ReportSetConnectionId, ReportSetHeaderText, ReportSetFooterText, ReportSetDebug, RunCaption, BackCaption, ParameterLayout, AlwaysShowParameters, RenderMode, AutoRun, ReportSetConfig)
        End Function

        Public Sub DeleteReportSet(ByVal ModuleId As Integer)
            DataProvider.Instance().DeleteReportSet(ModuleId)
        End Sub

        Public Function ListReportSet(ByVal PortalId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().ListReportSet(PortalId), GetType(ReportSetInfo))
        End Function

        Public Function GetReportSetReport(ByVal ReportSetId As Integer) As ArrayList
            Dim al As ArrayList
            al = CBO.FillCollection(DataProvider.Instance().GetReportSetReport(ReportSetId), GetType(ReportInfo))
            For Each obj As Object In al
                Dim ri As ReportInfo = DirectCast(obj, ReportInfo)
                ri.ReportDrillDowns = CBO.FillCollection(DataProvider.Instance().ListReportDrilldown(ri.ReportId), GetType(ReportInfo))
            Next
            Return al
        End Function

        Public Function GetReportSetParameter(ByVal ReportSetId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetReportSetParameter(ReportSetId), GetType(ParameterInfo))
        End Function
    End Class

#End Region

#Region " Report"
    Public Class ReportInfo

        ' initialization
        Public Sub New()
        End Sub

        ' public properties
        Public Property ReportId() As Integer
        Public Property ReportSetId() As Integer
        Public Property ReportName() As String
        Public Property ReportTheme() As String
        Public Property ReportConnectionId() As Integer
        Public Property ReportConnectionString() As String
        Public Property ReportHeaderText() As String
        Public Property ReportFooterText() As String
        Public Property ReportNoItemsText() As String
        Public Property ReportCommand() As String
        Public Property ReportCommandCacheTimeout() As Integer = 60
        Public Property ReportConfig() As String
        Public Property ReportTypeName() As String
        Public Property ReportTypeId() As String = "GRID"
        Public Property ReportTypeControlSrc() As String
        Public Property ReportTypeSettingsControlSrc() As String
        Public Property ReportOrder() As Integer
        Public Property ReportDrilldownFieldname() As String = ""
        Public Property ReportDrilldownReportId() As Integer = -1
        Public Property ReportDrillDowns() As ArrayList
        Public Property ReportPageTitle As String
        Public Property ReportMetaDescription As String

        ' calced
        Public ReadOnly Property ReportIdentifier() As String
            Get
                Return ReportName.Replace(" ", "_")
            End Get
        End Property

    End Class

    Public Class ReportController
        Public Function GetReport(ByVal reportId As Integer) As ReportInfo
            Dim ri As ReportInfo

            ri = CType(CBO.FillObject(DataProvider.Instance().GetReport(reportId), GetType(ReportInfo)), ReportInfo)

            If Not ri Is Nothing Then
                With ri
                    .ReportDrillDowns = CBO.FillCollection(DataProvider.Instance().ListReportDrilldown(reportId), GetType(ReportInfo))
                End With
            End If

            Return ri
        End Function

        Public Function UpdateReport(ByVal ReportSetId As Integer, ByVal ReportId As Integer, ByVal ReportTypeId As String, ByVal ReportName As String, ByVal ReportTheme As String, ByVal ReportConnectionId As Integer, ByVal ReportHeaderText As String, ByVal ReportFooterText As String, _
            ByVal ReportCommand As String, ByVal ReportConfig As String, ByVal ReportOrder As Integer, ByVal ReportDrilldownReportId As Integer, ByVal ReportDrilldownFieldname As String, ByVal ReportNoItemsText As String, ByVal ReportPageTitle As String, ByVal ReportCommandCacheTimeout As Integer, ByVal ReportMetaDescription As String) As Integer

            Return DataProvider.Instance().UpdateReport(ReportSetId, ReportId, ReportTypeId, ReportName, ReportTheme, ReportConnectionId, ReportHeaderText, ReportFooterText, _
                ReportCommand, ReportConfig, ReportOrder, ReportDrilldownReportId, ReportDrilldownFieldname, ReportNoItemsText, ReportPageTitle, ReportCommandCacheTimeout, ReportMetaDescription)
        End Function
        Public Function ListReport(ByVal ReportSetId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().ListReport(ReportSetId), GetType(ReportInfo))
        End Function

        Public Function ListReportDrilldown(ByVal ReportId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().ListReportDrilldown(ReportId), GetType(ReportInfo))
        End Function

        Public Sub UpdateReportOrder(ByVal ReportId As Integer, ByVal Increment As Integer)
            DataProvider.Instance().UpdateReportOrder(ReportId, Increment)
        End Sub

        Public Sub DeleteReport(ByVal ReportId As Integer)
            DataProvider.Instance().DeleteReport(ReportId)
        End Sub
    End Class
#End Region

#Region " Parameter"
    <Serializable()> _
    Public Class ParameterInfo

        ' initialization
        Public Sub New()
        End Sub

        ' public properties
        Public Property ParameterId() As Integer = -1
        Public Property ReportSetId() As Integer
        Public Property ParameterName() As String = ""
        Public Property ParameterCaption() As String = ""
        Public Property ParameterTypeId() As String = "TEXTBOX"
        Public Property ParameterTypeName() As String
        Public Property ParameterTypeSettingsControlSrc() As String
        Public Property ParameterTypeControlSrc() As String
        Public Property ParameterOrder() As Integer
        Public Property ParameterConfig() As String

        ' provided by main routine, captured from gui parameters
        Public Property Values() As List(Of String)
        Public Property ExtraValues() As System.Collections.Specialized.StringDictionary
        Public Property MultiValued() As Boolean = False

        ' calculated
        Public ReadOnly Property ParameterIdentifier() As String
            Get
                Return ParameterName.Replace(" ", "_")
            End Get
        End Property

    End Class

    Public Class ParameterController
        Public Function GetParameter(ByVal ParameterId As Integer) As ParameterInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetParameter(ParameterId), GetType(ParameterInfo)), ParameterInfo)
        End Function

        Public Function UpdateParameter(ByVal ReportSetId As Integer, ByVal ParameterId As Integer, ByVal ParameterName As String, ByVal ParameterCaption As String, _
            ByVal ParameterTypeId As String, ByVal ParameterConfig As String, ByVal ParameterOrder As Integer) As Integer

            Return DataProvider.Instance().UpdateParameter(ReportSetId, ParameterId, ParameterName, ParameterCaption, _
                ParameterTypeId, ParameterConfig, ParameterOrder)
        End Function

        Public Function ListParameter(ByVal reportSetId As Integer) As List(Of ParameterInfo)
            Return DotNetNuke.Common.Utilities.CBO.FillCollection(Of ParameterInfo)(DataProvider.Instance().ListParameter(reportSetId))
        End Function

        Public Sub UpdateParameterOrder(ByVal ParameterId As Integer, ByVal Increment As Integer)
            DataProvider.Instance().UpdateParameterOrder(ParameterId, Increment)
        End Sub

        Public Sub DeleteParameter(ByVal ParameterId As Integer)
            DataProvider.Instance().DeleteParameter(ParameterId)
        End Sub
    End Class
#End Region

#Region " Report Type"
    Public Class ReportTypeInfo
        Public Property ReportTypeId() As String
        Public Property ReportTypeName() As String
        Public Property ReportTypeControlSrc() As String
        Public Property ReportTypeSettingsControlSrc() As String
        Public Property ReportTypeSupportsDrilldown() As Boolean
    End Class

    Public Class ReportTypeController
        Public Function ListReportType() As IDataReader
            Return DataProvider.Instance().ListReportType()
        End Function

        Public Function GetReportType(ByVal ReportTypeId As String) As ReportTypeInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetReportType(ReportTypeId), GetType(ReportTypeInfo)), ReportTypeInfo)
        End Function
    End Class
#End Region

#Region " Parameter Type"
    Public Class ParameterTypeInfo
        Public Property ParameterTypeName() As String
        Public Property ParameterTypeControlSrc() As String
        Public Property ParameterTypeSettingsControlSrc() As String
        Public Property ParameterTypeId() As String = "TEXTBOX"
    End Class

    Public Class ParameterTypeController
        Public Function ListParameterType() As IDataReader
            Return DataProvider.Instance().ListParameterType()
        End Function

        Public Function GetParameterType(ByVal ParameterTypeId As String) As ParameterTypeInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetParameterType(ParameterTypeId), GetType(ParameterTypeInfo)), ParameterTypeInfo)
        End Function
    End Class
#End Region

#Region " Connection"
    Public Enum ConnectionType As Integer
        PortalDefault = -1
        ReportSetDefault = -2
    End Enum
    Public Class ConnectionInfo
        Public Property ConnectionName() As String
        Public Property ConnectionString() As String
        Public Property ConnectionId() As Integer
        Public Property PortalId() As Integer
        Public Property UsedInParameterCount() As Integer
        Public Property UsedInReportCount() As Integer
        Public Property UsedInReportSetCount() As Integer
        Public ReadOnly Property CanDelete() As Boolean
            Get
                Return (_UsedInParameterCount + _UsedInReportCount + _UsedInReportSetCount) = 0
            End Get
        End Property
    End Class

    Public Class ConnectionController
        Public Shared Function GetConnection(ByVal ConnectionId As Integer) As ConnectionInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetConnection(ConnectionId), GetType(ConnectionInfo)), ConnectionInfo)
        End Function

        Public Shared Function GetConnectionString(ByVal ConnectionId As Integer, ByVal ReportSetId As Integer) As String
            Select Case ConnectionId
                Case ConnectionType.PortalDefault
                    Return ""
                Case ConnectionType.ReportSetDefault
                    Dim rsc As New ReportSetController
                    Dim rsi As ReportSetInfo = rsc.GetReportSet(ReportSetId)
                    Return rsi.ReportSetConnectionString
                Case Else
                    Dim csi As ConnectionInfo = GetConnection(ConnectionId)
                    Return csi.ConnectionString
            End Select
        End Function

        Public Function UpdateConnection(ByVal PortalId As Integer, ByVal ConnectionId As Integer, ByVal ConnectionName As String, ByVal ConnectionString As String) As Integer
            Return DataProvider.Instance().UpdateConnection(PortalId, ConnectionId, ConnectionName, ConnectionString)
        End Function

        Public Sub DeleteConnection(ByVal ConnectionId As Integer)
            DataProvider.Instance().DeleteConnection(ConnectionId)
        End Sub
        Public Function ListConnection(ByVal PortalId As Integer, ByVal IncludePortalDefault As Boolean, ByVal IncludeReportSetDefault As Boolean) As ArrayList
            Dim al As ArrayList = CBO.FillCollection(DataProvider.Instance().ListConnection(PortalId), GetType(ConnectionInfo))

            Dim ci As ConnectionInfo

            If IncludePortalDefault Then
                ' add portal default option
                ci = New ConnectionInfo
                With ci
                    .ConnectionId = ConnectionType.PortalDefault
                    .ConnectionName = "Portal Default"
                    .ConnectionString = ""
                End With
                al.Insert(0, ci)
            End If

            If IncludeReportSetDefault Then
                ' add report set default option
                ci = New ConnectionInfo
                With ci
                    .ConnectionId = ConnectionType.ReportSetDefault
                    .ConnectionName = "ReportSet Default"
                    .ConnectionString = ""
                End With
                al.Insert(0, ci)
            End If

            Return al
        End Function
    End Class
#End Region

End Namespace
