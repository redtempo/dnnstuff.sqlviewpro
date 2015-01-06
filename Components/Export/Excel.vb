'Class to convert a dataset to an html stream which can be used to display the dataset
'in MS Excel
'The Convert method is overloaded three times as follows
' 1)  Default to 1st table in dataset
' 2)  Pass an index to tell us which table in the dataset to use
' 3)  Pass a table name to tell us which table in the dataset to use

Namespace DNNStuff.SQLViewPro.Services.Export
    Public Class Excel
        Public Shared Sub Export(ByVal dt As System.Data.DataTable, ByVal response As System.Web.HttpResponse, ByVal filename As String)
            'first let's clean up the response.object
            response.Clear()
            response.ClearHeaders()
            response.Buffer = True
            response.ContentEncoding = System.Text.Encoding.UTF8
            response.Charset = "utf-8"
            response.AddHeader("Content-Disposition", "attachment; filename=""" & filename & """")
            'set the response mime type for excel
            response.ContentType = "application/vnd.ms-excel"

            ' added to help issue with IE problems with Cache-Control: no-cache
            response.ExpiresAbsolute = Date.Now.AddYears(-1)

            'header/footer to support UTF-8 characters
            Dim header As String = "<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">" & vbLf & "<html xmlns=""http://www.w3.org/1999/xhtml"">" & vbLf & "<head>" & vbLf & "<title></title>" & vbLf & "<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />" & vbLf & "<style>" & vbLf & "</style>" & vbLf & "</head>" & vbLf & "<body>" & vbLf
            Dim footer As String = vbLf & "</body>" & vbLf & "</html>"

            response.Write(header)

            'create an htmltextwriter which uses the stringwriter
            Dim htmlWrite As New System.Web.UI.HtmlTextWriter(response.Output)
            'instantiate a datagrid
            Dim dg As New System.Web.UI.WebControls.DataGrid
            'set the datagrid datasource to the dataset passed in
            dg.DataSource = dt
            'bind the datagrid
            dg.DataBind()
            'tell the datagrid to render itself to our htmltextwriter
            dg.RenderControl(htmlWrite)

            response.Write(footer)

            'all that's left is to output the html
            response.End()

        End Sub

        'Public Shared Sub Export(ByVal ds As System.Data.DataSet, ByVal response As System.Web.HttpResponse)
        '    Export(ds.Tables(0), response, "default.xls")
        'End Sub
        'Public Shared Sub Export(ByVal ds As System.Data.DataSet, ByVal TableIndex As Integer, ByVal response As System.Web.HttpResponse)
        '    'lets make sure a table actually exists at the passed in value
        '    'if it is not call the base method
        '    If TableIndex > ds.Tables.Count - 1 Then
        '        Export(ds, response)
        '    End If
        '    Export(ds.Tables(TableIndex), response, "default.xls")
        'End Sub

        'Public Shared Sub Export(ByVal ds As System.Data.DataSet, ByVal TableName As String, ByVal response As System.Web.HttpResponse)
        '    'let's make sure the table name exists
        '    'if it does not then call the default method
        '    If ds.Tables(TableName) Is Nothing Then
        '        Export(ds, response)
        '    End If
        '    Export(ds.Tables(TableName), response, "default.xls")
        'End Sub

    End Class
End Namespace
