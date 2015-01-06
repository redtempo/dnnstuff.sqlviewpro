'***************************************************************************/
'* ReportControlBase.vb
'*
'* COPYRIGHT (c) 2004-2011 by DNNStuff
'* ALL RIGHTS RESERVED.
'*
'* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
'* TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
'* THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
'* CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
'* DEALINGS IN THE SOFTWARE.
'*************/

Imports System.Text
Imports DNNStuff.SQLViewPro.Services.Data

Namespace DNNStuff.SQLViewPro.Controls
    Public MustInherit Class ReportControlBase
        Inherits UserControl

        Public Event OnDrillDown(ByVal o As Object, ByVal e As DrilldownEventArgs)

#Region "Public Properties"
        Public Property FullScreenUrl As String
        Public Property State As DrilldownState
        Public Property Report As ReportInfo
        Public Property DebugInfo As StringBuilder = New StringBuilder

        Public ReadOnly Property QueryText() As String
            Get
                Dim s As String = Report.ReportCommand
                If s <> "" Then
                    s = ReplaceReportTokens(s)
                End If
                Return s
            End Get
        End Property
#End Region

#Region " Public Methods"
        ''' <summary>
        ''' Retrieve data for the given query
        ''' </summary>
        ''' <param name="query">The query to execute</param>
        ''' <returns>DataSet containing all data</returns>
        ''' <remarks></remarks>
        Public Function ReportData(ByVal query As String) As DataSet
            Return Services.Data.Query.RetrieveData(query, Report.ReportConnectionString, Report.ReportCommandCacheTimeout)
        End Function

        ''' <summary>
        ''' Retrieves data for the report query
        ''' </summary>
        ''' <returns>DataSet containing all data</returns>
        ''' <remarks></remarks>
        Public Function ReportData() As DataSet
            Return Services.Data.Query.RetrieveData(QueryText, Report.ReportConnectionString, Report.ReportCommandCacheTimeout)
        End Function

        ''' <summary>
        ''' Retrieves a unique string based on the given key
        ''' </summary>
        ''' <param name="key"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Unique(ByVal key As String) As String
            Return key & "_" & Report.ReportId
        End Function

#End Region

#Region "Abstract Methods"
        Public MustOverride Sub LoadRuntimeSettings(ByVal settings As ReportInfo)
#End Region

        Public Overridable Function RenderHeaderAsText() As String
            Dim s As New StringBuilder
            If Report.ReportHeaderText.Length > 0 Then
                s.AppendFormat("<div class=""{0}_Header"">{1}</div>", Report.ReportTheme, ReplaceReportTokens(Report.ReportHeaderText))
            End If
            Return s.ToString
        End Function

        Public Overridable Function RenderFooterAsText() As String
            Dim s As New StringBuilder
            If Report.ReportFooterText.Length > 0 Then
                s.AppendFormat("<div class=""{0}_Footer"">{1}</div>", Report.ReportTheme, ReplaceReportTokens(Report.ReportFooterText))
            End If
            Return s.ToString
        End Function

        Public Overridable Sub RenderNoItems()
            If Report.ReportNoItemsText.Length > 0 Then
                ' No Items
                Dim ctrl As New HtmlControls.HtmlGenericControl("div")
                With ctrl
                    .InnerHtml = ReplaceReportTokens(Report.ReportNoItemsText)
                    .Attributes.Add("class", Report.ReportTheme & "_NoItems")
                End With
                Controls.Add(ctrl)
            End If
        End Sub

        Public Overridable Function RenderPageTitleAsText() As String
            Return ReplaceReportTokens(Report.ReportPageTitle)
        End Function

        Public Overridable Function RenderMetaDescriptionAsText() As String
            Return ReplaceReportTokens(Report.ReportMetaDescription)
        End Function

#Region "Events"
        Protected Sub DrillDown(ByVal o As ReportControlBase, ByVal e As DrilldownEventArgs)
            RaiseEvent OnDrillDown(o, e)
        End Sub

        Private Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
            Dim postbackControlId As String = ""
            If Page.IsPostBack Then
                Dim postbackControl As Control = ControlHelpers.GetPostBackControl(Page)
                If postbackControl IsNot Nothing Then postbackControlId = postbackControl.ID
            End If

            If postbackControlId <> "cmdBack" Then
                If Report.ReportPageTitle <> "" Then
                    Dim thisPage As DotNetNuke.Framework.CDefault = DirectCast(Me.Page, DotNetNuke.Framework.CDefault)
                    thisPage.Title = RenderPageTitleAsText()
                End If
                If Report.ReportMetaDescription <> "" Then
                    Dim thisPage As DotNetNuke.Framework.CDefault = DirectCast(Me.Page, DotNetNuke.Framework.CDefault)
                    Dim meta As String = RenderMetaDescriptionAsText()
                    If meta.Contains("<meta") Then
                        thisPage.Header.Controls.Add(New LiteralControl(meta))
                    Else
                        thisPage.Description = meta
                    End If
                End If

            End If
        End Sub

        Private Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
            If State.ReportSet.ReportSetDebug Then
                Me.Controls.Add(RenderDebug(DebugInfo.ToString()))
            End If
        End Sub
#End Region

#Region "Token Replacement"
        Protected Function ReplaceReportTokens(ByVal text As String, Optional ByVal ds As DataSet = Nothing) As String
            Return TokenReplacement.ReplaceTokens(text, ReportTokenSettings, ds)
        End Function

        Private _reportTokens As Hashtable
        Private Function ReportTokenSettings() As Hashtable

            If _reportTokens Is Nothing Then
                _reportTokens = New Hashtable
                Dim fullScreenParameters As String = ""
                ' now do parameters
                For Each param As ParameterInfo In State.Parameters
                    Dim tokenValue As String = ""
                    If param.Values IsNot Nothing Then
                        If param.MultiValued Then
                            tokenValue = "'" & String.Join("','", param.Values.ToArray()) & "'"
                        Else
                            tokenValue = param.Values(0).Replace("'", "''")
                        End If
                    End If
                    DNNUtilities.SafeHashtableAdd(_reportTokens, "PARAMETER:" & param.ParameterIdentifier.ToUpper(), tokenValue)
                    fullScreenParameters = fullScreenParameters & String.Format("&{0}={1}", param.ParameterIdentifier, tokenValue)

                    If param.ExtraValues IsNot Nothing Then
                        For Each key As String In param.ExtraValues.Keys
                            DNNUtilities.SafeHashtableAdd(_reportTokens, "PARAMETER:" & param.ParameterIdentifier.ToUpper() & ":" & key.ToUpper(), param.ExtraValues(key))
                            fullScreenParameters = fullScreenParameters & String.Format("&{0}:{1}={2}", param.ParameterIdentifier, key, param.Values)
                        Next
                    End If
                Next
                DNNUtilities.SafeHashtableAdd(_reportTokens, "REPORTNAME", Report.ReportName)
                DNNUtilities.SafeHashtableAdd(_reportTokens, "REPORTTYPE", Report.ReportTypeId)
                DNNUtilities.SafeHashtableAdd(_reportTokens, "REPORTTYPENAME", Report.ReportTypeName)
                If Request IsNot Nothing Then DNNUtilities.SafeHashtableAdd(_reportTokens, "PAGEURL", Request.Url.AbsoluteUri)
                DNNUtilities.SafeHashtableAdd(_reportTokens, "IMAGEURL", ResolveUrl("~/images"))
                DNNUtilities.SafeHashtableAdd(_reportTokens, "FULLSCREENURL", String.Format("{0}{1}&hidebackbutton=1", FullScreenUrl, fullScreenParameters))
            End If

            Return _reportTokens
        End Function


#End Region

#Region "Debug"
        Private Function RenderDebug(info As String) As Control
            Return ParseControl(RenderDebugAsText(info))
        End Function

        Private Function RenderDebugAsText(info As String) As String
            Dim s As New StringBuilder
            If info.Length > 0 Then
                s.AppendFormat("<div class=""{0}_Debug"">{1}</div>", Report.ReportTheme, info)
            End If
            Return s.ToString
        End Function
#End Region


    End Class
End Namespace
