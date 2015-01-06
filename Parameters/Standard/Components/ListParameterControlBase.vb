'***************************************************************************/
'* ListParameterControlBase.vb
'*
'* COPYRIGHT (c) 2004-2005 by DNNStuff
'* ALL RIGHTS RESERVED.
'*
'* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
'* TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
'* THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
'* CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
'* DEALINGS IN THE SOFTWARE.
'*************/

Imports DNNStuff.SQLViewPro.Controls
Imports System.Web.UI.WebControls
Imports DNNStuff.SQLViewPro.Services.Data

Namespace DNNStuff.SQLViewPro.StandardParameters
    Public MustInherit Class ListParameterControlBase
        Inherits ParameterControlBase

#Region "Option Loading"
        Protected Sub AddOptions(ByVal list As ListControl, ByVal customParameterSettings As ListParameterSettings, ByVal parameterSettings As ParameterInfo)
            ' add options from both if necessary
            If customParameterSettings.Command.Length > 0 Then
                Dim connectionString As String
                If customParameterSettings.ConnectionId < 0 Then
                    ' get report set connection
                    Dim objReportSetController As New ReportSetController
                    Dim objReportSetInfo As ReportSetInfo = objReportSetController.GetReportSet(ParameterSettings.ReportSetId)
                    connectionString = objReportSetInfo.ReportSetConnectionString
                Else
                    Dim objConnectionInfo As ConnectionInfo = ConnectionController.GetConnection(customParameterSettings.ConnectionId)
                    connectionString = objConnectionInfo.ConnectionString
                End If
                SQLUtil.AddOptionsFromQuery(list, ReplaceOptionTokens(customParameterSettings.Command), connectionString, customParameterSettings.Default, customParameterSettings.CommandCacheTimeout)
            End If
            If customParameterSettings.List.Length > 0 Then
                SQLUtil.AddOptionsFromList(list, customParameterSettings.List, customParameterSettings.Default)
            End If
        End Sub

        Private Function ReplaceOptionTokens(s As String) As String
            Return TokenReplacement.ReplaceTokens(s)
        End Function

        Protected Sub SelectDefaults(ByVal list As ListControl, ByVal customParameterSettings As ListParameterSettings, ByVal multiAllowed As Boolean)
            Dim defaultValues() As String = customParameterSettings.Default.Split(",")

            If defaultValues.Length > 0 Then
                For Each defaultValue As String In defaultValues
                    Dim li As ListItem = list.Items.FindByValue(defaultValue)
                    If li IsNot Nothing Then
                        li.Selected = True
                    Else
                        ' try text
                        li = list.Items.FindByText(defaultValue)
                        If li IsNot Nothing Then
                            li.Selected = True
                        End If
                    End If
                    If Not multiAllowed Then Exit For ' first one only
                Next
            End If

        End Sub
#End Region


    End Class
End Namespace
