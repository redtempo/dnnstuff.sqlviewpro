Namespace DNNStuff
    Public Class Url
        ' <summary>
        ' This method returns a fully qualified absolute server Url which includes
        ' the protocol, server, port in addition to the server relative Url.
        ' 
        ' Works like Control.ResolveUrl including support for ~ syntax
        ' but returns an absolute URL.
        ' </summary>
        ' <param name="ServerUrl">Any Url, either App relative or fully qualified</param>
        ' <param name="forceHttps">if true forces the url to use https</param>
        ' <returns></returns>

        Public Shared Function ResolveServerUrl(serverUrl As String, forceHttps As Boolean) As String

            ' *** Is it already an absolute Url?
            If serverUrl.IndexOf("://") > -1 Then
                Return serverUrl
            End If

            ' *** Start by fixing up the Url an Application relative Url
            Dim newUrl As String = ResolveUrl(serverUrl)

            Dim originalUri As Uri = HttpContext.Current.Request.Url
            newUrl = (If(forceHttps, "https", originalUri.Scheme)) + "://" + originalUri.Authority + newUrl
            Return newUrl

        End Function

        ' <summary>
        ' This method returns a fully qualified absolute server Url which includes
        ' the protocol, server, port in addition to the server relative Url.
        ' 
        ' It work like Page.ResolveUrl, but adds these to the beginning.
        ' This method is useful for generating Urls for AJAX methods
        ' </summary>
        ' <param name="ServerUrl">Any Url, either App relative or fully qualified</param>
        ' <returns></returns>

        Public Shared Function ResolveServerUrl(serverUrl As String) As String
            Return ResolveServerUrl(serverUrl, False)
        End Function

        ' <summary>
        ' Returns a site relative HTTP path from a partial path starting out with a ~.
        ' Same syntax that ASP.Net internally supports but this method can be used
        ' outside of the Page framework.
        ' 
        ' Works like Control.ResolveUrl including support for ~ syntax
        ' but returns an absolute URL.
        ' </summary>
        ' <param name="originalUrl">Any Url including those starting with ~</param>
        ' <returns>relative url</returns>

        Public Shared Function ResolveUrl(originalUrl As String) As String

            If originalUrl Is Nothing Then

                Return Nothing
            End If
            ' *** Absolute path - just return
            If originalUrl.IndexOf("://") <> -1 Then
                Return originalUrl
            End If

            ' *** Fix up image path for ~ root app dir directory
            If originalUrl.StartsWith("~") Then
                Dim newUrl As String = ""

                If HttpContext.Current IsNot Nothing Then
                    newUrl = HttpContext.Current.Request.ApplicationPath + originalUrl.Substring(1).Replace("//", "/")
                Else

                    ' *** Not context: assume current directory is the base directory
                    Throw New ArgumentException("Invalid URL: Relative URL not allowed.")
                End If

                ' *** Just to be sure fix up any double slashes
                Return newUrl
            End If

            Return originalUrl

        End Function


    End Class
End Namespace
