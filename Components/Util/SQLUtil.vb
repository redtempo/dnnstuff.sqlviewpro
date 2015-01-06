
Namespace DNNStuff.SQLViewPro
    Public Class SQLUtil

        Public Shared Sub AddOptionsFromQuery(ByVal list As ListControl, ByVal queryText As String, ByVal connectionString As String, ByVal defaultValue As String, ByVal cacheTimeout As Integer)

            Dim ds As DataSet = Services.Data.Query.RetrieveData(queryText, connectionString, cacheTimeout)

            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Columns.Count = 1 Then
                    With list
                        .DataValueField = ds.Tables(0).Columns(0).ColumnName
                        .DataTextField = ds.Tables(0).Columns(0).ColumnName
                        .DataSource = ds.Tables(0).DefaultView
                        .DataBind()
                    End With
                ElseIf ds.Tables(0).Columns.Count > 1 Then
                    With list
                        .DataValueField = ds.Tables(0).Columns(0).ColumnName
                        .DataTextField = ds.Tables(0).Columns(1).ColumnName
                        .DataSource = ds.Tables(0).DefaultView
                        .DataBind()
                    End With
                End If
            End If
        End Sub
        Public Shared Sub AddOptionsFromList(ByVal list As ListControl, ByVal options As String, Optional ByVal defaultValue As String = "")
            ' delims
            Const VALUE_DELIM As Char = "|"c
            Const FIELD_DELIM As Char = Chr(10)

            Dim li As ListItem
            Dim optionArray() As String = options.Replace(Environment.NewLine, FIELD_DELIM).Split(FIELD_DELIM)

            Dim insertPosition As Integer = 0
            For Each o As String In optionArray
                li = New ListItem
                li.Value = o.Split(VALUE_DELIM)(0)
                If o.Split(VALUE_DELIM).GetUpperBound(0) > 0 Then
                    li.Text = o.Split(VALUE_DELIM)(1)
                Else
                    li.Text = li.Value
                End If

                If li.Text = defaultValue Then li.Selected = True

                ' items are inserted to appear ahead of any databound options
                list.Items.Insert(insertPosition, li)
                insertPosition += 1
            Next

        End Sub

        Public Shared Function ContainsCatchWords(ByVal queryText As String) As Boolean
            ' valid query so that it doesn't contain malicious code
            Dim CatchWords() As String = {" INSERT ", " UPDATE ", " DELETE ", " DROP ", " SELECT INTO "}
            Dim upperQuery As String = " " & queryText.ToUpper & " "
            Dim DisableCatchWords As Boolean = False
            Dim IsValid As Boolean = True

            If System.Configuration.ConfigurationManager.AppSettings("DNNStuff:SQLViewPro:DisableCatchWords") Is Nothing Then
                DisableCatchWords = False
            Else
                DisableCatchWords = (System.Configuration.ConfigurationManager.AppSettings("DNNStuff:SQLViewPro:DisableCatchWords").ToString.ToUpper = "TRUE")
            End If

            If Not DisableCatchWords Then
                For Each w As String In CatchWords
                    If upperQuery.IndexOf(w) > 0 Then
                        IsValid = False
                        Exit For
                    End If
                Next
            End If
            Return IsValid

        End Function

    End Class
End Namespace