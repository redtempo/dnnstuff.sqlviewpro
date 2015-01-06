'***************************************************************************/
'* ParameterSettingsBase.vb
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
Namespace DNNStuff.SQLViewPro.Controls
    Public MustInherit Class ParameterSettingsControlBase
        Inherits System.Web.UI.UserControl

#Region "Abstract Methods"
        Public MustOverride Sub LoadSettings(ByVal settings As String)
        Public MustOverride Function UpdateSettings() As String
        Protected MustOverride ReadOnly Property LocalResourceFile() As String
        Public Overridable ReadOnly Property CaptionRequired() As Boolean
            Get
                Return True
            End Get
        End Property
#End Region

        Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
            LoadLabelResources(Me)
        End Sub

        Private Sub LoadLabelResources(root As Control)
            For Each c As Control In root.Controls
                If TypeOf (c) Is DotNetNuke.UI.UserControls.LabelControl Then
                    Dim label As DotNetNuke.UI.UserControls.LabelControl = DirectCast(c, DotNetNuke.UI.UserControls.LabelControl)
                    Dim labelText As String = DotNetNuke.Services.Localization.Localization.GetString(label.ID & ".Text", LocalResourceFile)
                    If labelText Is Nothing Then labelText = label.ID.Replace("lbl", "")
                    Dim helpText As String = DotNetNuke.Services.Localization.Localization.GetString(label.ID & ".Help", LocalResourceFile)
                    If helpText Is Nothing Then helpText = "Help not available for " & labelText
                    label.Text = labelText
                    label.HelpText = helpText
                End If
                If c.HasControls() Then
                    LoadLabelResources(c)
                End If
            Next
        End Sub
    End Class
End Namespace