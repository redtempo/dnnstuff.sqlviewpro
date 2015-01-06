Imports DotNetNuke
Namespace DNNStuff.SQLViewPro
    Public Class DNNUtilities
        Public Shared Function GetSetting(ByVal settings As Hashtable, ByVal key As String, Optional ByVal [default] As String = "") As String
            Dim ret As String

            If settings.ContainsKey(key) Then
                Try
                    ret = settings(key).ToString
                Catch ex As Exception
                    ret = [default]
                End Try
            Else
                ret = [default]
            End If
            Return ret
        End Function

        Public Shared Sub SafeHashtableAdd(ByRef ht As Hashtable, key As Object, value As Object)
            ' checks for existing entry and overwrites data otherwise adding
            If ht.ContainsKey(key) Then
                ht(key) = value
            Else
                ht.Add(key, value)
            End If
        End Sub

        Public Shared Function QueryStringDefault(ByVal req As HttpRequest, ByVal parameterName As String, ByVal defaultValue As String) As String
            ' check for querystring parameter and default if not found
            If Not req.QueryString(parameterName) Is Nothing Then
                Return req.QueryString(parameterName)
            End If
            Return defaultValue
        End Function


#Region " Skinning"
        Public Shared Sub InjectCSS(ByVal pg As System.Web.UI.Page, ByVal fileName As String)

            ' page style sheet reference
            Dim objCSS As Control = pg.FindControl("CSS")
            If objCSS Is Nothing Then
                ' DNN 4 doesn't have CSS control any more, look for Head
                objCSS = pg.FindControl("Head")
            End If

            ' container stylesheet
            If Not objCSS Is Nothing Then
                Dim CSSId As String = DotNetNuke.Common.Globals.CreateValidID(fileName)

                ' container package style sheet
                If objCSS.FindControl(CSSId) Is Nothing Then

                    Dim objLink As New HtmlGenericControl("link")
                    objLink.ID = CSSId
                    objLink.Attributes("rel") = "stylesheet"
                    objLink.Attributes("type") = "text/css"
                    objLink.Attributes("href") = fileName
                    objCSS.Controls.Add(objLink)
                End If
            End If

        End Sub

#End Region

#Region "version checking"
        ''' <summary>
        ''' Returns a version-safe set of version numbers for DNN
        ''' </summary>
        ''' <param name="major">out int of the DNN Major version</param>
        ''' <param name="minor">out int of the DNN Minor version</param>
        ''' <param name="revision">out int of the DNN Revision</param>
        ''' <param name="build">out int of the DNN Build version</param>
        ''' <remarks>Dnn moved the version number during about the 4.9 version, which to me was a bit frustrating and caused the need for this reflection method call</remarks>
        ''' <returns>true if it worked.</returns>
        Public Shared Function SafeDNNVersion(major As Integer, minor As Integer, revision As Integer, build As Integer) As Boolean
            Dim ver As System.Version = System.Reflection.Assembly.GetAssembly(GetType(DotNetNuke.Common.Globals)).GetName().Version
            If ver IsNot Nothing Then
                major = ver.Major
                minor = ver.Minor
                build = ver.Build
                revision = ver.Revision
                Return True
            Else
                major = 0
                minor = 0
                build = 0
                revision = 0
                Return False
            End If
        End Function

        Public Shared Function SafeDNNVersion() As System.Version
            Dim ver As System.Version = System.Reflection.Assembly.GetAssembly(GetType(DotNetNuke.Common.Globals)).GetName().Version
            If ver IsNot Nothing Then
                Return ver
            End If
            Return New System.Version(0, 0, 0, 0)
        End Function
#End Region

    End Class
End Namespace
