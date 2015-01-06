Imports DotNetNuke
Imports DotNetNuke.Security.Permissions

Namespace DNNStuff.SQLViewPro

    Public Class Export
        Inherits System.Web.UI.Page

        Public Const EXPORT_KEY As String = "sqlviewpro_export"
        Private _ModuleId As Integer = -1
        Private _TabId As Integer = -1

        Public ReadOnly Property ModuleId() As Integer
            Get
                If Request.QueryString("ModuleId") IsNot Nothing Then
                    _ModuleId = CInt(Request.QueryString("ModuleId"))
                End If
                Return _ModuleId
            End Get
        End Property

        Public ReadOnly Property TabId() As Integer
            Get
                If Request.QueryString("TabId") IsNot Nothing Then
                    _TabId = CInt(Request.QueryString("TabId"))
                End If
                Return _TabId
            End Get
        End Property

        Private Sub Report_Init(sender As Object, e As System.EventArgs) Handles Me.Init
            If HasViewPermissions() Then
                Dim details As ExportDetails = DirectCast(Session(EXPORT_KEY), ExportDetails)
                If details IsNot Nothing Then
                    If details.Dataset IsNot Nothing Then
                        Dim ms As IO.StringWriter = DataTableToExcel(details.Dataset.Tables(0))
                        ExportToExcel(ms, details)
                    ElseIf details.Binary IsNot Nothing Then
                        ExportToExcel(details.Binary, details)
                    ElseIf details.BinaryFilename.Length > 0 Then
                        Using fs As IO.FileStream = IO.File.OpenRead(details.BinaryFilename)
                            Dim bytes As Byte() = New Byte(CInt(fs.Length - 1)) {}
                            fs.Read(bytes, 0, Convert.ToInt32(fs.Length))
                            fs.Close()
                            ExportToExcel(bytes, details)
                        End Using
                        Try
                            IO.File.Delete(details.BinaryFilename)
                        Catch ex As Exception

                        End Try
                    End If
                    Session.Remove(EXPORT_KEY)
                End If
            Else
                Session.Remove(EXPORT_KEY)
                Response.Redirect(DotNetNuke.Common.AccessDeniedURL(), True)
            End If
        End Sub

        Private Function HasViewPermissions() As Boolean
            Dim mi As DotNetNuke.Entities.Modules.ModuleInfo
            Dim mc As New DotNetNuke.Entities.Modules.ModuleController
            mi = mc.GetModule(ModuleId, TabId)
            Return ModulePermissionController.CanViewModule(mi)
        End Function

        Private Function DataTableToExcel(ByVal dt As System.Data.DataTable) As IO.StringWriter
            Dim ms As New IO.StringWriter

            'header/footer to support UTF-8 characters
            Dim header As String = "<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">" & vbLf & "<html xmlns=""http://www.w3.org/1999/xhtml"">" & vbLf & "<head>" & vbLf & "<title></title>" & vbLf & "<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />" & vbLf & "<style>" & vbLf & "</style>" & vbLf & "</head>" & vbLf & "<body>" & vbLf
            Dim footer As String = vbLf & "</body>" & vbLf & "</html>"

            ms.Write(header)
            'create an htmltextwriter which uses the stringwriter
            Dim htmlWrite As New System.Web.UI.HtmlTextWriter(ms)
            'instantiate a datagrid
            Dim dg As New System.Web.UI.WebControls.DataGrid
            'set the datagrid datasource to the dataset passed in
            dg.DataSource = dt
            'bind the datagrid
            dg.DataBind()
            'tell the datagrid to render itself to our htmltextwriter
            dg.RenderControl(htmlWrite)

            ms.Write(footer)

            Return ms
        End Function

        Private Sub ExportInit(ByVal ba As Byte(), ByVal details As ExportDetails)
            'first let's clean up the response.object
            Response.ClearHeaders()
            Response.Clear()
            Response.Buffer = True
            Response.ContentEncoding = System.Text.Encoding.UTF8
            Response.Charset = "utf-8"
            Response.AddHeader("Content-Disposition", String.Format("{0}; filename=""{1}""", details.Disposition, details.Filename))

            ' turn off cacheing
            'Response.CacheControl = "no-cache"
            'Response.AddHeader("Pragma", "no-cache")
            Response.Expires = -1

        End Sub

        Private Sub ExportToExcel(ByVal ba As Byte(), ByVal details As ExportDetails)
            ' common initialize
            ExportInit(ba, details)

            'set the response mime type for excel
            If details.Filename.EndsWith(".xlsx") Then
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Else
                Response.ContentType = "application/vnd.ms-excel"
            End If

            ' now the data
            Response.BinaryWrite(ba)

            'all that's left is to output the html
            Response.End()

        End Sub

        Private Sub ExportToExcel(ByVal ms As IO.StringWriter, ByVal details As ExportDetails)
            ExportToExcel(System.Text.Encoding.UTF8.GetBytes(ms.ToString), details)
        End Sub

    End Class

    <Serializable()>
    Public Class ExportDetails
        Public Property Filename() As String
        Public Property ContentType() As String
        Public Property Disposition() As String = "inline"
        Public Property Dataset() As DataSet = Nothing
        Public Property Binary() As Byte() = Nothing
        Public Property BinaryFilename() As String = ""
    End Class
End Namespace
