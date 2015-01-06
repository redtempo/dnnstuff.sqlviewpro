Imports System.Text.RegularExpressions

Namespace DNNStuff.SQLViewPro.Services.Data
    Public Class TokenReplacement

        Public Shared Function ReplaceTokens(ByVal text As String, Optional ByVal settings As Hashtable = Nothing, Optional ByVal ds As DataSet = Nothing) As String
            Dim sharedSettings As Hashtable

            ' make a copy of settings
            If settings Is Nothing Then
                sharedSettings = New Hashtable
            Else
                sharedSettings = DirectCast(settings.Clone(), Hashtable)
            End If

            ' add querystring values
            Dim qs As New Specialized.NameValueCollection(HttpContext.Current.Request.QueryString) ' create a copy, some weird errors happening with url rewriters
            Dim keyval As Object
            For Each key As String In qs.Keys
                keyval = qs(key)
                If key IsNot Nothing And keyval IsNot Nothing Then
                    sharedSettings.Add("QS:" & key.ToUpper(), keyval.ToString().Replace("'", "''"))
                    sharedSettings.Add("QUERYSTRING:" & key.ToUpper(), keyval.ToString().Replace("'", "''"))
                End If
            Next

            ' add server variables
            Dim sv As New Specialized.NameValueCollection(HttpContext.Current.Request.ServerVariables) ' create a copy
            For Each key As String In sv.Keys
                keyval = sv(key)
                If key IsNot Nothing And keyval IsNot Nothing Then
                    sharedSettings.Add("SV:" & key.ToUpper(), keyval.ToString().Replace("'", "''"))
                    sharedSettings.Add("SERVERVAR:" & key.ToUpper(), keyval.ToString().Replace("'", "''"))
                End If
            Next

            ' add form variables
            Dim fv As New Specialized.NameValueCollection(HttpContext.Current.Request.Form) ' create a copy
            For Each key As String In fv.Keys
                keyval = fv(key)
                If key IsNot Nothing And keyval IsNot Nothing Then
                    sharedSettings.Add("FV:" & key.ToUpper(), keyval.ToString().Replace("'", "''"))
                    sharedSettings.Add("FORMVAR:" & key.ToUpper(), keyval.ToString().Replace("'", "''"))
                End If
            Next

            ' do dataset replacements (if necessary)
            If ds IsNot Nothing Then
                Dim dataReplacer As DNNStuff.Utilities.RegularExpression.DataSetTokenReplacement = New DNNStuff.Utilities.RegularExpression.DataSetTokenReplacement(ds)
                text = dataReplacer.Replace(text)
            End If

            ' do logic replacements
            Dim logicReplacer As New DNNStuff.Utilities.RegularExpression.IfDefinedTokenReplacement(sharedSettings)
            text = logicReplacer.Replace(text)

            ' do settings replacements
            Dim replacer As New DNNStuff.Utilities.RegularExpression.TokenReplacement(sharedSettings)
            replacer.ReplaceIfNotFound = False
            text = replacer.Replace(text)

            ' do generic replacements - DNN tokens
            text = Compatibility.ReplaceGenericTokens(text)

            ' replace escaped characters
            text = ReplaceEscapedCharacters(text)

            Return text

        End Function

        Public Shared Function ReplaceEscapedCharacters(ByVal text As String) As String
            text = Regex.Replace(text, "0x5B", "[", RegexOptions.IgnoreCase)
            text = Regex.Replace(text, "0x5D", "]", RegexOptions.IgnoreCase)

            Return text
        End Function
    End Class

End Namespace
