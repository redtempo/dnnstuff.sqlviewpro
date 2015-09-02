
Imports DotNetNuke.Common.Utilities

Namespace DNNStuff.SQLViewPro.Services.Data
#Region " Report Query"
    Public Class Query

        Public Shared Function RetrieveData(ByVal queryText As String, ByVal connectionString As String, ByVal cacheTimeout As Integer, ByVal cacheScheme As String) As DataSet
            Return RetrieveData(queryText, connectionString, "SQLData", "Table", cacheTimeout, cacheScheme)
        End Function

        Public Shared Function RetrieveData(ByVal queryText As String, ByVal connectionString As String, ByVal dataSetName As String, ByVal srcTable As String, ByVal cacheTimeout As Integer, byval cacheScheme as String) As DataSet
            Dim results As DataSet

            ' try cache first
            Dim cacheKey As String = "SQLViewPro_Data_" & CStr(queryText.GetHashCode())
            If cacheTimeout > 0 Then
                results = TryCast(DataCache.GetCache(cacheKey), DataSet)
                If results IsNot Nothing Then Return results
            End If

            ' if no query just return empty dataset
            If queryText = "" Then Return New DataSet()

            ' not cached, grab live data
            If connectionString = "" Then
                results = DataProvider.Instance.RunQuery(queryText, dataSetName)
            Else
                Using cn As OleDb.OleDbConnection = New OleDb.OleDbConnection(connectionString)
                    Using cmd As OleDb.OleDbCommand = New OleDb.OleDbCommand(queryText, cn)
                        cmd.CommandTimeout = 0
                        Dim da As OleDb.OleDbDataAdapter = New OleDb.OleDbDataAdapter(cmd)
                        cn.Open()
                        results = New DataSet(dataSetName)
                        da.Fill(results, srcTable)
                    End Using
                End Using
            End If

            ' update cache
            If cacheTimeout > 0 Then
                If cacheScheme = "Sliding" Then
                    DataCache.SetCache(cacheKey, results, TimeSpan.FromSeconds(cacheTimeout))
                Else
                    DataCache.SetCache(cacheKey, results, Date.Now.AddSeconds(cacheTimeout))
                End If
            End If
            Return results

        End Function

        Public Shared Sub TestConnection(ByVal connectionString As String)
            Dim cn As OleDb.OleDbConnection = New OleDb.OleDbConnection(connectionString)
            cn.Open()
        End Sub

        Public Shared Function IsQueryValid(ByVal queryText As String, ByVal connectionString As String, ByRef errorMessage As String) As Boolean
            Dim sharedResourceFile As String = DotNetNuke.Common.Globals.ApplicationPath & "/DesktopModules/DNNStuff - SQLViewPro/App_LocalResources/SharedResources.resx"

            ' valid query and return appropriate error message
            Dim msg As String = ""
            Dim queryValid As Boolean = True
            Dim catchwordsPassed As Boolean = True

            ' check for valid query
            Try
                RetrieveData(Compatibility.ReplaceGenericTokensForTest(queryText), connectionString, 0, "Sliding")
            Catch ex As Exception
                queryValid = False
                msg = "<br>" & Localization.GetString("QueryTestError", sharedResourceFile) & " : " & ex.Message
            End Try

            ' check for catch words
            If Not SQLUtil.ContainsCatchWords(queryText) Then
                catchwordsPassed = False
                msg = msg & "<br>" & Localization.GetString("CatchWordsError", sharedResourceFile)
            End If

            If Not (queryValid And catchwordsPassed) Then
                errorMessage = msg
                Return False
            End If

            errorMessage = "<br>" & Localization.GetString("QueryTestOK", sharedResourceFile)
            Return True
        End Function
    End Class

#End Region
End Namespace
