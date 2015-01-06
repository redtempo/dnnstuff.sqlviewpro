Public Class ValidationHelpers
    Public Shared Function CommonValidator(ByVal key As String) As String
        Select Case key
            Case "Integer"
                Return "[-|+]?\b\d+\b"
            Case "PositiveInteger"
                Return "\b\d+\b"
            Case "Color", "Colour"
                Return "^([a-fA-F0-9]{6}|[a-fA-F0-9]{3})$"
        End Select
        Return key
    End Function
End Class
