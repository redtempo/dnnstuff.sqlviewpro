Imports System.Text.RegularExpressions
Imports System.Collections.Generic
Imports System.Text

Public Class StringHelpers
#Region " Strings"
    Public Shared Function Wordify(pascalCaseString As String) As String
        Dim r As New Regex("(?<=[a-z])(?<x>[A-Z])|(?<=.)(?<x>[A-Z])(?=[a-z])")
        Return r.Replace(pascalCaseString, " ${x}")
    End Function

    Public Shared Function EscapeSingleQuotes(value As String) As String
        Return value.Replace("'", "''")
    End Function

    Public Shared Function XmlEncode(ByVal buf As String) As String
        Dim textOut As New StringBuilder
        Dim c As Char
        If buf.Trim Is Nothing OrElse buf = String.Empty Then Return String.Empty
        For i As Integer = 0 To buf.Length - 1
            c = buf(i)
            If Entities.ContainsKey(c) Then
                textOut.Append(Entities.Item(c))
            ElseIf (AscW(c) = &H9 OrElse AscW(c) = &HA OrElse AscW(c) = &HD) OrElse ((AscW(c) >= &H20) AndAlso (AscW(c) <= &HD7FF)) _
                OrElse ((AscW(c) >= &HE000) AndAlso (AscW(c) <= &HFFFD)) OrElse ((AscW(c) >= &H10000) AndAlso (AscW(c) <= &H10FFFF)) Then
                textOut.Append(c)
            End If
        Next
        Return textOut.ToString

    End Function

    Shared ReadOnly Entities As New Dictionary(Of Char, String)() From {{""""c, "&quot;"}, {"&"c, "&amp;"}, {"'"c, "&apos;"}, {"<"c, "&lt;"}, {">"c, "&gt;"}}

    Public Shared Function RemoveLastCharacter(value As String) As String
        If value.Length < 1 Then Return value
        Return value.Substring(0, value.Length - 1)
    End Function

    Public Shared Function DefaultInt32FromString(ByVal s As String, ByVal [default] As Int32) As Int32
        Dim result As Int32
        If Integer.TryParse(s, result) Then Return result
        Return [default]
    End Function

    Public Shared Function CleanName(ByVal name As String) As String

        Const strBadChars As String = ". ~`!@#$%^&*()-_+={[}]|\:;<,>?/" & Chr(34) & Chr(39)

        Dim intCounter As Integer
        For intCounter = 0 To Len(strBadChars) - 1
            name = name.Replace(strBadChars.Substring(intCounter, 1), "")
        Next intCounter

        Return name

    End Function

    Public Shared Function FindNthField(ByVal s As String, ByVal separator As Char, ByVal position As Integer) As String
        Dim splits As [String]() = s.Split(separator)
        If splits.Length < position Then
            Return Nothing
        End If
        Return splits(position - 1)
    End Function

    Public Shared Function FindLastField(ByVal s As String, ByVal separator As Char) As String
        Dim splits As [String]() = s.Split(separator)
        Return FindNthField(s, separator, splits.Length)
    End Function

    Public Shared Function ToDictionary(ByVal s As String, ByVal separator As Char) As Dictionary(Of String, String)
        Dim d As New Dictionary(Of String, String)
        For Each temp As String In s.Split(separator)
            Dim index As Int32 = temp.IndexOf("="c)
            If index > -1 Then
                d.Add(temp.Substring(0, index), temp.Substring(index + 1))
            End If
        Next
        Return d
    End Function
#End Region
End Class
