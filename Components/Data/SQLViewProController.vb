Imports System.Xml
Imports DotNetNuke.Common.Utilities.XmlUtils
Imports DotNetNuke.Entities.Portals
Imports System.Collections.Generic

Namespace DNNStuff.SQLViewPro
    Public Class SQLViewProController
        Implements DotNetNuke.Entities.Modules.IPortable

#Region "Export"
        Public Function ExportModule(ByVal ModuleID As Integer) As String Implements DotNetNuke.Entities.Modules.IPortable.ExportModule
            Dim ctrl As New ReportSetController

            Dim strXML As String = ""

            strXML += "<sqlviewpro>"

            Dim info As ReportSetInfo = ctrl.GetReportSetByModule(ModuleID)
            strXML += ExportReportSet(info)

            strXML += ExportReport(info.ReportSetId)

            strXML += ExportParameter(info.ReportSetId)

            ' close it off
            strXML += "</sqlviewpro>"

            Return strXML

        End Function

        Private Function ExportConnection(ByVal portalSettings As PortalSettings) As String
            Dim ctrl As New ConnectionController
            Dim al As ArrayList = ctrl.ListConnection(portalSettings.PortalId, False, False)
            Dim info As ConnectionInfo
            Dim s As String = "<connections>"
            For i As Integer = 0 To al.Count - 1
                info = DirectCast(al.Item(i), ConnectionInfo)
                s += "<connection>"
                s += "<id>" & XMLEncode(info.ConnectionId.ToString) & "</id>"
                s += "<name>" & XMLEncode(info.ConnectionName) & "</name>"
                s += "<string>" & XMLEncode(info.ConnectionString) & "</string>"
                s += "</connection>"
            Next
            s += "</connections>"

            Return s
        End Function

        Private Function ExportReportSet(ByVal info As ReportSetInfo) As String
            Dim s As String = ""
            s += "<reportset>"
            s += "<id>" & XMLEncode(info.ReportSetId.ToString) & "</id>"
            s += "<name>" & XMLEncode(info.ReportSetName) & "</name>"
            s += "<theme>" & XMLEncode(info.ReportSetTheme) & "</theme>"
            s += "<debug>" & XMLEncode(info.ReportSetDebug.ToString) & "</debug>"
            s += "<runcaption>" & XMLEncode(info.RunCaption) & "</runcaption>"
            s += "<backcaption>" & XMLEncode(info.BackCaption) & "</backcaption>"
            s += "<connectionid>" & XMLEncode(info.ReportSetConnectionId.ToString) & "</connectionid>"
            s += "<connectionstring>" & XMLEncode(info.ReportSetConnectionString.ToString) & "</connectionstring>"
            s += "<footertext>" & XMLEncode(info.ReportSetFooterText) & "</footertext>"
            s += "<headertext>" & XMLEncode(info.ReportSetHeaderText) & "</headertext>"
            s += "<parameterlayout>" & XMLEncode(info.ParameterLayout) & "</parameterlayout>"
            s += "<alwaysshowparameters>" & XMLEncode(info.AlwaysShowParameters.ToString) & "</alwaysshowparameters>"
            s += "<rendermode>" & XMLEncode(info.RenderMode) & "</rendermode>"
            s += "<reportsetconfig>" & XMLEncode(info.ReportSetConfig) & "</reportsetconfig>"
            s += "</reportset>"
            Return s
        End Function

        Private Function ExportReport(ByVal ReportSetId As Integer) As String

            Dim ctrl As New ReportController
            Dim al As ArrayList = ctrl.ListReport(ReportSetId)
            Dim info As ReportInfo
            Dim s As String = "<reports>"
            For i As Integer = 0 To al.Count - 1
                info = DirectCast(al.Item(i), ReportInfo)
                s += "<report>"
                s += "<id>" & XMLEncode(info.ReportId.ToString) & "</id>"
                s += "<reportsetid>" & XMLEncode(info.ReportSetId.ToString) & "</reportsetid>"
                s += "<name>" & XMLEncode(info.ReportName) & "</name>"
                s += "<theme>" & XMLEncode(info.ReportTheme) & "</theme>"
                s += "<command>" & XMLEncode(info.ReportCommand) & "</command>"
                s += "<reporttype>" & XMLEncode(info.ReportTypeId.ToString) & "</reporttype>"
                s += "<config>" & XMLEncode(info.ReportConfig) & "</config>"
                s += "<connectionid>" & XMLEncode(info.ReportConnectionId.ToString) & "</connectionid>"
                s += "<connectionstring>" & XMLEncode(info.ReportConnectionString.ToString) & "</connectionstring>"
                s += "<footertext>" & XMLEncode(info.ReportFooterText) & "</footertext>"
                s += "<headertext>" & XMLEncode(info.ReportHeaderText) & "</headertext>"
                s += "<order>" & XMLEncode(info.ReportOrder.ToString) & "</order>"
                s += "<drilldownreportid>" & XMLEncode(info.ReportDrilldownReportId.ToString) & "</drilldownreportid>"
                s += "<drilldownfieldname>" & XMLEncode(info.ReportDrilldownFieldname) & "</drilldownfieldname>"
                s += "<noitemstext>" & XMLEncode(info.ReportNoItemsText) & "</noitemstext>"
                s += "<pagetitle>" & XMLEncode(info.ReportPageTitle) & "</pagetitle>"
                s += "<cachetimeout>" & XMLEncode(info.ReportCommandCacheTimeout.ToString()) & "</cachetimeout>"
                s += "<metadescription>" & XMLEncode(info.ReportMetaDescription) & "</metadescription>"

                s += "</report>"
            Next
            s += "</reports>"

            Return s
        End Function

        Private Function ExportParameter(ByVal ReportSetId As Integer) As String

            Dim ctrl As New ParameterController
            Dim al As List(Of ParameterInfo) = ctrl.ListParameter(ReportSetId)
            Dim info As ParameterInfo
            Dim s As String = "<parameters>"
            For i As Integer = 0 To al.Count - 1
                info = al.Item(i)
                s += "<parameter>"
                s += "<id>" & XMLEncode(info.ParameterId.ToString) & "</id>"
                s += "<reportsetid>" & XMLEncode(info.ReportSetId.ToString) & "</reportsetid>"
                s += "<name>" & XMLEncode(info.ParameterName) & "</name>"
                s += "<parametertype>" & XMLEncode(info.ParameterTypeId.ToString) & "</parametertype>"
                s += "<config>" & XMLEncode(info.ParameterConfig) & "</config>"
                s += "<caption>" & XMLEncode(info.ParameterCaption) & "</caption>"
                s += "<order>" & XMLEncode(info.ParameterOrder.ToString) & "</order>"
                s += "</parameter>"
            Next
            s += "</parameters>"

            Return s
        End Function
#End Region

#Region "Import"

        Public Sub ImportModule(ByVal ModuleID As Integer, ByVal Content As String, ByVal Version As String, ByVal UserID As Integer) Implements DotNetNuke.Entities.Modules.IPortable.ImportModule

            ' remove current reportset
            Dim ctrl As New ReportSetController
            ctrl.DeleteReportSet(ModuleID)

            Dim xmlSqlViewPro As XmlNode = DotNetNuke.Common.GetContent(Content, "sqlviewpro")

            Dim ReportSetId As Integer = ImportReportSet(ModuleID, xmlSqlViewPro)
            ImportReport(ReportSetId, xmlSqlViewPro)
            ImportParameter(ReportSetId, xmlSqlViewPro)

        End Sub

        Private Function ImportConnection(ByVal connectionName As String, ByVal connectionString As String) As Integer
            Dim portalSettings As PortalSettings = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)

            Dim ctrl As New ConnectionController
            Dim info As ConnectionInfo

            ' see if we can find the connection string and return the id
            Dim connections As ArrayList = ctrl.ListConnection(portalSettings.PortalId, False, False)
            For Each info In connections
                If info.ConnectionString = connectionString Then
                    Return info.ConnectionId
                End If
            Next

            ' we'll have to make a new one
            Dim connectionId As Integer
            connectionId = ctrl.UpdateConnection(portalSettings.PortalId, -1, connectionName, connectionString)

            Return connectionId

        End Function

        Private Function ImportReportSet(ByVal ModuleId As Integer, ByVal root As XmlNode) As Integer
            Dim xmlReportSet As XmlNode = root.Item("reportset")

            Dim ctrl As New ReportSetController

            Dim reportSetId As Integer
            Dim reportSetName As String
            Dim reportSetTheme As String
            Dim reportSetConnectionId As Integer
            Dim reportSetConnectionString As String
            Dim reportSetFooterText As String
            Dim reportSetHeaderText As String
            Dim reportSetDebug As Boolean
            Dim runCaption As String
            Dim backCaption As String
            Dim parameterLayout As String
            Dim alwaysShowParameters As Boolean
            Dim renderMode As String
            Dim autoRun As Boolean

            Dim reportSetConfig As String

            reportSetId = -1
            reportSetName = xmlReportSet.Item("name").InnerText
            reportSetTheme = xmlReportSet.Item("theme").InnerText
            reportSetConnectionId = Convert.ToInt32(xmlReportSet.Item("connectionid").InnerText)
            reportSetConnectionString = xmlReportSet.Item("connectionstring").InnerText
            If reportSetConnectionId <> ConnectionType.PortalDefault Then
                reportSetConnectionId = ImportConnection("Imported " & Date.Now.ToShortDateString, reportSetConnectionString)
            End If
            reportSetFooterText = xmlReportSet.Item("footertext").InnerText
            reportSetHeaderText = xmlReportSet.Item("headertext").InnerText
            reportSetDebug = Convert.ToBoolean(xmlReportSet.Item("debug").InnerText)
            runCaption = GetNodeInnerText(xmlReportSet, "runcaption")
            backCaption = GetNodeInnerText(xmlReportSet, "backcaption")
            parameterLayout = GetNodeInnerText(xmlReportSet, "parameterlayout")
            alwaysShowParameters = Convert.ToBoolean(GetNodeInnerText(xmlReportSet, "alwaysshowparameters", "false"))
            renderMode = GetNodeInnerText(xmlReportSet, "rendermode", "default")
            autoRun = Convert.ToBoolean(GetNodeInnerText(xmlReportSet, "autoRun", "false"))

            reportSetConfig = GetNodeInnerText(xmlReportSet, "reportsetconfig", "")

            ' check if id is already present
            reportSetId = ctrl.UpdateReportSet(ModuleId, reportSetId, reportSetName, reportSetTheme, reportSetConnectionId, reportSetHeaderText, reportSetFooterText, reportSetDebug, runCaption, backCaption, parameterLayout, alwaysShowParameters, renderMode, autoRun, reportSetConfig)
            Return reportSetId

        End Function

        Private Sub ImportReport(ByVal ReportSetId As Integer, ByVal root As XmlNode)
            Dim xmlReports As XmlNode = root.Item("reports")

            Dim ctrl As New ReportController

            Dim reportId As Integer
            Dim reportTypeId As String
            Dim reportName As String
            Dim reportTheme As String
            Dim reportConnectionId As Integer
            Dim reportConnectionString As String
            Dim reportFooterText As String
            Dim reportHeaderText As String
            Dim reportCommand As String
            Dim reportConfig As String
            Dim reportOrder As Integer
            Dim reportDrilldownReportId As Integer
            Dim reportDrilldownFieldname As String
            Dim reportNoItemsText As String
            Dim reportPageTitle As String
            Dim reportCommandCacheTimeout As Integer

            Dim reportMapping As New Hashtable  'map old reportid to new ones for drilldown reference

            For Each xmlReport As XmlNode In xmlReports.ChildNodes
                reportId = Convert.ToInt32(xmlReport.Item("id").InnerText)
                reportTypeId = xmlReport.Item("reporttype").InnerText
                reportName = xmlReport.Item("name").InnerText
                reportTheme = xmlReport.Item("theme").InnerText
                reportConnectionId = Convert.ToInt32(xmlReport.Item("connectionid").InnerText)
                reportConnectionString = xmlReport.Item("connectionstring").InnerText
                If reportConnectionId <> ConnectionType.PortalDefault And reportConnectionId <> ConnectionType.ReportSetDefault Then
                    reportConnectionId = ImportConnection("Imported " & Date.Now.ToShortDateString, reportConnectionString)
                End If
                reportFooterText = xmlReport.Item("footertext").InnerText
                reportHeaderText = xmlReport.Item("headertext").InnerText
                reportCommand = xmlReport.Item("command").InnerText
                reportConfig = xmlReport.Item("config").InnerText
                reportOrder = Convert.ToInt32(xmlReport.Item("order").InnerText)
                reportDrilldownReportId = Convert.ToInt32(xmlReport.Item("drilldownreportid").InnerText)
                If reportDrilldownReportId > -1 Then
                    If reportMapping.ContainsKey(reportDrilldownReportId) Then
                        reportDrilldownReportId = Convert.ToInt32(reportMapping(reportDrilldownReportId))
                    End If
                End If
                reportDrilldownFieldname = xmlReport.Item("drilldownfieldname").InnerText
                reportNoItemsText = GetNodeInnerText(xmlReport, "noitemstext")
                reportPageTitle = GetNodeInnerText(xmlReport, "pagetitle")
                reportCommandCacheTimeout = CInt(GetNodeInnerText(xmlReport, "cachetimeout", "0"))
                Dim reportMetaDescription As String = GetNodeInnerText(xmlReport, "metadescription")

                ' check if id is already present
                Dim newid As Integer = ctrl.UpdateReport(ReportSetId, -1, reportTypeId, reportName, reportTheme, reportConnectionId, reportHeaderText, reportFooterText, reportCommand, reportConfig, reportOrder, reportDrilldownReportId, reportDrilldownFieldname, reportNoItemsText, reportPageTitle, reportCommandCacheTimeout, reportMetaDescription)
                reportMapping.Add(reportId, newid)
            Next

        End Sub

        Private Sub ImportParameter(ByVal ReportSetId As Integer, ByVal root As XmlNode)
            Dim xmlParameters As XmlNode = root.Item("parameters")

            Dim ctrl As New ParameterController

            Dim parameterId As Integer
            Dim parameterTypeId As String
            Dim parameterName As String
            Dim parameterCaption As String
            Dim parameterConfig As String
            Dim parameterOrder As Integer

            For Each xmlParameter As XmlNode In xmlParameters.ChildNodes
                parameterId = -1

                parameterTypeId = xmlParameter.Item("parametertype").InnerText
                parameterName = xmlParameter.Item("name").InnerText
                parameterCaption = xmlParameter.Item("caption").InnerText
                parameterConfig = xmlParameter.Item("config").InnerText
                parameterOrder = Convert.ToInt32(xmlParameter.Item("order").InnerText)

                ' check if id is already present
                ctrl.UpdateParameter(ReportSetId, parameterId, parameterName, parameterCaption, parameterTypeId, parameterConfig, parameterOrder)
            Next

        End Sub

        Private Function GetNodeInnerText(ByVal node As XmlNode, ByVal key As String, Optional ByVal [default] As String = "") As String
            Dim xe As XmlElement = node.Item(key)
            If xe Is Nothing Then Return [default]
            Return xe.InnerText
        End Function

#End Region

    End Class

End Namespace
