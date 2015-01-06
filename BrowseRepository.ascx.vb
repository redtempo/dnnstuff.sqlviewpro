'***************************************************************************/
'* BrowseRepository.ascx.vb
'*
'* COPYRIGHT (c) 2004 by DNNStuff
'* ALL RIGHTS RESERVED.
'*
'* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
'* TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
'* THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
'* CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
'* DEALINGS IN THE SOFTWARE.
'*************/
Option Strict On
Option Explicit On 

Imports DotNetNuke
Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Services.Exceptions
Imports Dotnetnuke.Services.Localization
Imports System.Collections.Generic
Imports System.Xml

Namespace DNNStuff.SQLViewPro

    Partial Class BrowseRepository
        Inherits Entities.Modules.PortalModuleBase

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()

            MyBase.HelpURL = "http://www.dnnstuff.com/"
        End Sub

#End Region

#Region " Page Level"


        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
                DNNUtilities.InjectCSS(Me.Page, ResolveUrl("Resources/Support/edit.css"))


            Try
                cmdImport.Attributes.Add("onclick", _
                    "return confirm('Importing will overwrite the current reportset associated with this module. Are you sure?');")

                If Page.IsPostBack = False Then
                    LoadSettings()
                End If

            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
            Try
                ReturnToPage()
            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cmdImport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdImport.Click
            Try
                If Page.IsValid Then
                    ImportTemplate()
                    ReturnToPage()
                End If
            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub ReturnToPage()

            ' Redirect back to the portal home page
            Response.Redirect(NavigateURL(), True)

        End Sub
#End Region

#Region " Process"
        Private Sub ImportTemplate()
            ImportTemplate(cboRepository.SelectedValue)
        End Sub

        Private Sub ImportTemplate(ByVal TemplateName As String)

            Dim templateFile As IO.FileInfo = New IO.FileInfo(IO.Path.Combine(Server.MapPath(ResolveUrl("Repository")), TemplateName))
            If templateFile IsNot Nothing Then
                Dim xmlData As New XmlDocument
                xmlData.Load(templateFile.FullName)
                Dim strType As String = xmlData.DocumentElement.GetAttribute("type").ToString
                If strType = StringHelpers.CleanName(ModuleConfiguration.DesktopModule.ModuleName) Or strType = StringHelpers.CleanName(ModuleConfiguration.DesktopModule.FriendlyName) Then
                    Dim strVersion As String = xmlData.DocumentElement.GetAttribute("version").ToString
                    Dim ctrl As New SQLViewProController
                    ctrl.ImportModule(ModuleId, xmlData.DocumentElement.InnerXml, strVersion, UserId)
                End If
            End If
        End Sub

#End Region

#Region " Settings"
        Private Sub LoadSettings()
            ' repository
            BindRepository(cboRepository)
        End Sub

        Private Sub BindRepository(ByVal o As ListControl)
            Dim repositoryFolder As New IO.DirectoryInfo(Server.MapPath(ResolveUrl("Repository")))
            o.Items.Clear()
            For Each fi As IO.FileInfo In repositoryFolder.GetFiles("content.*.xml")
                o.Items.Add(fi.Name)
            Next
        End Sub

#End Region

    End Class


End Namespace