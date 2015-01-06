Imports System.Text.RegularExpressions

Namespace DNNStuff.SQLViewPro

    Public Class Compatibility
        ' this module will provide compatibility between DNN versions
        Public Shared Function ReplaceGenericTokens(ByVal text As String) As String
            Dim ret As String = text
            Dim objTokenReplace As New DotNetNuke.Services.Tokens.TokenReplace()
            ret = objTokenReplace.ReplaceEnvironmentTokens(ret)
            Return ret
        End Function

        Public Shared Function ReplaceGenericTokensForTest(ByVal text As String) As String
            Dim ret As String = text

            ' replace tokens that aren't available
            ret = Regex.Replace(ret, "\[QUERYSTRING:.*?\]", "1", RegexOptions.IgnoreCase)
            ret = Regex.Replace(ret, "\[QS:.*?\]", "1", RegexOptions.IgnoreCase)
            ' replace any parameter tokens named date with dates (crude workaround for the time being)
            ret = Regex.Replace(ret, "\[PARAMETER:.*?DATE.*?\]", "1966-2-21", RegexOptions.IgnoreCase)
            ' replace rest of parameters
            ret = Regex.Replace(ret, "\[PARAMETER:.*?\]", "1", RegexOptions.IgnoreCase)

            Dim objTokenReplace As New DotNetNuke.Services.Tokens.TokenReplace()
            ret = objTokenReplace.ReplaceEnvironmentTokens(ret)

            Return ret
        End Function
    End Class

End Namespace

