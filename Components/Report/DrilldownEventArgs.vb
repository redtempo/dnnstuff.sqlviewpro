'***************************************************************************/
'* DrilldownEventArgs.vb
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

Namespace DNNStuff.SQLViewPro.Controls
    Public Class DrilldownEventArgs
        Inherits EventArgs

        Public Sub New(ByVal reportId As Integer, ByVal name As String, ByVal value As DataRow)
            _reportId = reportId
            _name = name
            _value = value
        End Sub

#Region "Properties"
        Private _reportId As Integer

        Public Property ReportId() As Integer
            Get
                Return _reportId
            End Get
            Set(ByVal Value As Integer)
                _reportId = Value
            End Set
        End Property

        Private _value As DataRow
        Public Property Value() As DataRow
            Get
                Return _value
            End Get
            Set(ByVal Value As DataRow)
                _value = Value
            End Set
        End Property

        Private _name As String
        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal Value As String)
                _name = Value
            End Set
        End Property
#End Region

    End Class
End Namespace
