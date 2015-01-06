Namespace DNNStuff

    Public Class ControlHelpers
        Public Shared Function GetPostBackControl(page As Page) As Control


            Dim postbackControlInstance As Control = Nothing
            Dim postbackControlName As String = page.Request.Params.[Get]("__EVENTTARGET")

            If postbackControlName IsNot Nothing AndAlso postbackControlName <> String.Empty Then
                postbackControlInstance = FindControlRecursive(page, postbackControlName)
            Else
                ' handle the Button control postbacks
                For i As Integer = 0 To page.Request.Form.Keys.Count - 1
                    postbackControlInstance = FindControlRecursive(page, page.Request.Form.Keys(i))
                    If TypeOf postbackControlInstance Is System.Web.UI.WebControls.Button Then
                        Return postbackControlInstance
                    End If
                Next
            End If

            ' handle the ImageButton postbacks
            If postbackControlInstance Is Nothing Then
                For i As Integer = 0 To page.Request.Form.Count - 1
                    If (page.Request.Form.Keys(i).EndsWith(".x")) OrElse (page.Request.Form.Keys(i).EndsWith(".y")) Then
                        postbackControlInstance = FindControlRecursive(page, page.Request.Form.Keys(i).Substring(0, page.Request.Form.Keys(i).Length - 2))
                        Return postbackControlInstance
                    End If
                Next
            End If

            Return postbackControlInstance
        End Function

        Public Shared Function GetPostBackControlName(page As Page) As String


            Dim postbackControlName As String = page.Request.Params.[Get]("__EVENTTARGET")
            If String.IsNullOrEmpty(postbackControlName) Then Return ""

            Return StringHelpers.FindLastField(postbackControlName, "$"c)

        End Function

        Public Shared Function FindControlRecursive(root As Control, id As String) As Control
            Return root.FindControl(id)
        End Function

        Public Shared Sub InitDropDownByValue(c As DropDownList, value As String)
            Dim li As ListItem = c.Items.FindByValue(value)
            If li IsNot Nothing Then li.Selected = True
        End Sub
    End Class
End Namespace
