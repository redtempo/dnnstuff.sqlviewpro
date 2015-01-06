'***************************************************************************/
'* FusionChartReportControl.ascx.vb
'*
'* Copyright (c) 2004 by DNNStuff.
'* All rights reserved.
'*
'* Date:        Sep/27/2008
'* Author:      Richard Edwards
'* Description: FusionChart Report
'*************/
Imports DotNetNuke
Imports System.Configuration
Imports System.IO
Imports System.Text.RegularExpressions
Imports DNNStuff.SQLViewPro.Controls
Imports DNNStuff.SQLViewPro.Serialization
Imports DNNStuff.Utilities
Imports System.Reflection
Imports System.Collections.Generic
Imports System.Text

Namespace DNNStuff.SQLViewPro.StandardReports

    Partial Class FusionChartReportControl
        Inherits DNNStuff.SQLViewPro.Controls.ReportControlBase

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()

            RenderChart()

        End Sub

        Protected WithEvents phChart As System.Web.UI.WebControls.PlaceHolder

#End Region

#Region " Page"

        Private Property ReportExtra() As FusionChartReportSettings = New FusionChartReportSettings()
        Private _reportScript As String

#End Region

#Region " Base Method Implementations"
        Public Overrides Sub LoadRuntimeSettings(ByVal settings As ReportInfo)
            _ReportExtra = DirectCast(DeserializeObject(Settings.ReportConfig, GetType(FusionChartReportSettings)), FusionChartReportSettings)
        End Sub
#End Region

#Region " Chart"
        Private Function GenerateColourSet(ByVal numColors As Integer) As String
            Dim random As New Random()
            Dim s As String = ""

            For i As Integer = 0 To numColors - 1
                s = s & String.Format("{0:X6}", random.Next(&H1000000)) & ","
            Next
            s = s & String.Format("{0:X6}", random.Next(&H1000000))

            Return s
        End Function

        Private Function ReportColorSet(ByVal numColors As Integer) As String
            Dim colorSet As String = ReportExtra.ColorSet

            If colorSet = "Custom" Then colorSet = ReportExtra.CustomColorSet.Replace(" ", "")

            If colorSet = "" Then colorSet = GenerateColourSet(numColors)
            Return colorSet
        End Function

        Private Function GetChartProperties() As String
            Dim s As New Text.StringBuilder

            For Each prop As PropertyInfo In ReportExtra.GetType.GetProperties()

                Dim expectedValue As String = ""

                Dim value As Object = prop.GetValue(ReportExtra, Nothing)
                If value IsNot Nothing Then
                    Select Case prop.PropertyType.Name
                        Case "Boolean"
                            expectedValue = ConvertBooleanToBit(Convert.ToBoolean(value)).ToString
                        Case Else
                            expectedValue = value.ToString
                    End Select

                    ' only append if there is a value
                    If expectedValue.Length > 0 Then
                        s.AppendFormat("{0}='{1}' ", prop.Name, StringHelpers.XmlEncode(ReplaceReportTokens(expectedValue)))
                    End If

                End If

            Next
            Return s.ToString
        End Function

        Private Function ConvertBooleanToBit(ByVal b As Boolean) As Integer
            ' returns 0 for false, 1 for true
            Return Convert.ToByte(b)
        End Function

        Private Sub RenderChart()
            Page.ClientScript.RegisterClientScriptInclude(Me.GetType(), "FusionChart_JavaScript", ResolveUrl("Resources/FusionCharts.js"))

            Me.Controls.Add(New LiteralControl(String.Format("<div id=""{0}""></div>", Unique("chartdiv"))))

            ' debug ?
            If State.ReportSet.ReportSetDebug Then
                DebugInfo.Append(QueryText)
            End If

            Dim ds As DataSet = ReportData()

            Dim data As New Text.StringBuilder
            data.AppendFormat("<graph {0} >", GetChartProperties())

            If ds.Tables.Count > 1 And (ReportExtra.ChartType.StartsWith("MS") Or ReportExtra.ChartType.StartsWith("Stacked")) Then
                RenderMultiSeriesChartMultipleTable(ds, data)
            ElseIf ds.Tables(0).Columns.Count > 1 And (ReportExtra.ChartType.StartsWith("MS") Or ReportExtra.ChartType.StartsWith("Stacked")) Then
                RenderMultiSeriesChartSingleTable(ds, data)
            Else
                RenderSingleSeriesChart(ds, data)
            End If

            data.Append("</graph>")

            Dim sb As New Text.StringBuilder
            sb.Append("<script type=""text/javascript"">")
            sb.Append("jQuery(document).ready(function () {")
            sb.AppendFormat("   var chart = new FusionCharts(""{0}"", ""ChartId"", ""{1}"", ""{2}"");", ResolveUrl("Charts/FCF_" & ReportExtra.ChartType & ".swf"), ReportExtra.ChartWidth, ReportExtra.ChartHeight)
            sb.AppendFormat("   chart.setDataXML(""{0}"");", data.ToString())
            sb.AppendFormat("   chart.setTransparent(""{0}"");", "false")
            sb.AppendFormat("chart.render(""{0}"");", Unique("chartdiv"))
            sb.Append("});")
            sb.Append("</script>")

            _ReportScript = sb.ToString

        End Sub

        Private Sub RenderMultiSeriesChartSingleTable(ByVal ds As DataSet, ByVal data As Text.StringBuilder)
            ' multi series
            data.AppendFormat("<categories>")
            For Each dr As DataRow In ds.Tables(0).Rows
                data.AppendFormat("<category name='{0}' />", StringHelpers.XmlEncode(dr(0).ToString()))
            Next
            data.AppendFormat("</categories>")

            Dim colors As String()
            Dim colorIndex As Integer = 0
            colors = ReportColorSet(ds.Tables(0).Columns.Count).Split(","c)
            For columnIndex As Integer = 1 To ds.Tables(0).Columns.Count - 1
                data.AppendFormat("<dataset seriesname='{0}' color='{1}'>", StringHelpers.XmlEncode(ds.Tables(0).Columns(columnIndex).ColumnName), colors(colorIndex))
                For Each dr As DataRow In ds.Tables(0).Rows
                    data.AppendFormat("<set value='{0}' />", StringHelpers.XmlEncode(dr(columnIndex).ToString()))
                Next
                data.AppendFormat("</dataset>")
                colorIndex += 1
                If colorIndex >= colors.Length Then colorIndex = 0
            Next
        End Sub

        Private Sub RenderMultiSeriesChartMultipleTable(ByVal ds As DataSet, ByVal data As Text.StringBuilder)
            ' multi series - get categories off first table
            data.AppendFormat("<categories>")
            For Each dr As DataRow In ds.Tables(0).Rows
                data.AppendFormat("<category name='{0}' />", dr(0).ToString())
            Next
            data.AppendFormat("</categories>")

            Dim colors As String()
            Dim colorIndex As Integer = 0
            colors = ReportColorSet(ds.Tables(0).Columns.Count).Split(","c)
            For tableIndex As Integer = 0 To ds.Tables.Count - 1
                data.AppendFormat("<dataset seriesname='{0}' color='{1}'>", StringHelpers.XmlEncode(ds.Tables(tableIndex).Columns(1).ColumnName), colors(colorIndex))
                For Each dr As DataRow In ds.Tables(tableIndex).Rows
                    data.AppendFormat("<set value='{0}' />", StringHelpers.XmlEncode(dr(1).ToString()))
                Next
                data.AppendFormat("</dataset>")
                colorIndex += 1
                If colorIndex >= colors.Length Then colorIndex = 0
            Next
        End Sub

        Private Sub RenderSingleSeriesChart(ByVal ds As DataSet, ByVal data As Text.StringBuilder)
            Dim colors As String()
            Dim colorIndex As Integer = 0
            colors = ReportColorSet(ds.Tables(0).Rows.Count).Split(","c)

            ' single series
            For Each dr As DataRow In ds.Tables(0).Rows
                data.AppendFormat("<set name='{0}' value='{1}' color='{2}' />", StringHelpers.XmlEncode(dr(0).ToString()), StringHelpers.XmlEncode(dr(1).ToString()), colors(colorIndex))
                colorIndex += 1
                If colorIndex >= colors.Length Then colorIndex = 0
            Next
        End Sub

#End Region

        Private Sub Page_PreRender1(sender As Object, e As System.EventArgs) Handles Me.PreRender
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), Unique("RenderFusionChart"), _ReportScript)
        End Sub
    End Class

End Namespace
