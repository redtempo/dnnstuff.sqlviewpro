'***************************************************************************/
'* XmlReportControl.ascx.vb
'*
'* Copyright (c) 2004 by DNNStuff.
'* All rights reserved.
'*
'* Date:        August 9, 2004
'* Author:      Richard Edwards
'* Description: Xml Report
'*************/
Imports DotNetNuke
Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports System.Configuration
Imports System.IO
Imports System.Text.RegularExpressions
Imports DNNStuff.SQLViewPro.Controls
Imports DNNStuff.SQLViewPro.Serialization
Imports System.Xml

Namespace DNNStuff.SQLViewPro.StandardReports

    Partial Class XmlReportControl
        Inherits DNNStuff.SQLViewPro.Controls.ReportControlBase

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()

            Try
                BindXMLToData()
            Catch ex As Exception
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(Me, ex)
            End Try
        End Sub


#End Region

#Region " Page"

        Private Property ReportExtra As XmlReportSettings = New XmlReportSettings

#End Region

#Region " Base Method Implementations"
        Public Overrides Sub LoadRuntimeSettings(ByVal Settings As ReportInfo)
            _ReportExtra = DirectCast(DeserializeObject(Settings.ReportConfig, GetType(XmlReportSettings)), XmlReportSettings)
        End Sub
#End Region

#Region " XML"
        Private Sub BindXMLToData()

            Dim ds As DataSet = ReportData()

            ' add debug info
            If State.ReportSet.ReportSetDebug Then
                DebugInfo.Append(QueryText)
                Dim sw As New System.IO.StringWriter
                ds.WriteXml(sw)
                DebugInfo.AppendFormat("<pre>{0}</pre>", Server.HtmlEncode(sw.ToString))
            End If

            If ds.Tables.Count = 0 OrElse ds.Tables(0).Rows.Count = 0 Then
                xmlContent.Visible = (Report.ReportNoItemsText.Length = 0)
                RenderNoItems()
            Else
                Dim swData As New System.IO.StringWriter
                ds.WriteXml(swData)

                Dim xmlData As New XmlDocument
                xmlData.LoadXml(swData.ToString)

                ' transform data using xsl
                Dim xslTransform As Xsl.XslCompiledTransform = GetXslTransform(ReportExtra.XslSrc)
                Dim swOutput As New System.IO.StringWriter

                Dim xmltwOutput As New XmlTextWriter(swOutput)
                If Not xslTransform Is Nothing And Not xmlData Is Nothing Then
                    xslTransform.Transform(xmlData, xmltwOutput)
                End If

                xmlContent.Text = swOutput.ToString
            End If
        End Sub



#End Region

#Region "xsl functions"
        Private Function GetXSLContent(ByVal ContentURL As String) As Xsl.XslCompiledTransform

            GetXSLContent = New Xsl.XslCompiledTransform
            Dim req As System.Net.WebRequest = GetExternalRequest(ContentURL)
            Dim result As System.Net.WebResponse = req.GetResponse()
            Dim objXSLTransform As XmlReader = New XmlTextReader(result.GetResponseStream)
            GetXSLContent.Load(objXSLTransform, Nothing, Nothing)

        End Function

        Private Function GetXslTransform(ByVal XslDoc As String) As Xsl.XslCompiledTransform
            If XslDoc <> "" Then
                If GetURLType(XslDoc) = Entities.Tabs.TabType.Url Then
                    If XslDoc.ToLower.StartsWith("http") Then
                        Return GetXSLContent(XslDoc)
                    ElseIf XslDoc.StartsWith("~") Or XslDoc.StartsWith("/") Then
                        Dim trans As New Xsl.XslCompiledTransform
                        trans.Load(Me.Context.Server.MapPath(XslDoc))
                        Return trans
                    ElseIf XslDoc.Contains(":\") Then
                        Dim trans As New Xsl.XslCompiledTransform
                        trans.Load(XslDoc)
                        Return trans
                    End If
                End If
            End If
            Return Nothing
        End Function
#End Region

    End Class

End Namespace
