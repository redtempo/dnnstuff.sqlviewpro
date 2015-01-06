Namespace DNNStuff.SQLViewPro

    <Serializable()> _
    Public Class DrilldownState
        Public Property FromReportId() As Integer = -1
        Public Property FromReportColumn() As String = ""
        Public Property Parameters() As ArrayList
        Public Property ReportSet() As ReportSetInfo
        Public Property PortalId() As Integer
        Public Property ModuleId() As Integer
        Public Property TabId() As Integer
        Public Property UserId() As Integer

        Public Sub New(ByVal FromReportId As Integer, ByVal FromReportColumn As String, ByVal Parameters As ArrayList)
            _FromReportId = FromReportId
            _FromReportColumn = FromReportColumn
            _Parameters = Parameters
        End Sub
        Public Sub New(ByVal Parameters As ArrayList)
            _Parameters = Parameters
        End Sub
    End Class

End Namespace
