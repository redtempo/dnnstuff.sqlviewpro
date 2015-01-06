'***************************************************************************/
'* ParameterBase.vb
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
Imports System.Collections.Generic

Namespace DNNStuff.SQLViewPro.Controls
    Public MustInherit Class ParameterControlBase
        Inherits System.Web.UI.UserControl

#Region "Public Methods"
        Public Property Settings As ParameterInfo
        Public Property PortalSettings As DotNetNuke.Entities.Portals.PortalSettings
        ''' <summary>
        ''' Retrieves a unique string based on the given key
        ''' </summary>
        ''' <param name="key"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Unique(ByVal key As String) As String
            Return key & "_" & Settings.ParameterId
        End Function
#End Region

#Region "Abstract Methods"

        Public MustOverride Property Values() As List(Of String)
        Public Overridable ReadOnly Property MultiValued() As Boolean
            Get
                Return False
            End Get
        End Property
        Public MustOverride Sub LoadRuntimeSettings()
        Public Overridable ReadOnly Property ExtraValues() As Collections.Specialized.StringDictionary
            Get
                Return Nothing
            End Get
        End Property

#End Region

#Region "Events"
        ' events
        Public Event OnRun(ByVal o As Object)

        Protected Sub Run(ByVal o As ParameterControlBase)
            RaiseEvent OnRun(o)
        End Sub

#End Region

    End Class
End Namespace
