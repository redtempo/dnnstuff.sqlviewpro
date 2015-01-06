Imports System.Collections

<Serializable()>
Public Class ArrayListStack

    Private _list As New ArrayList

    Public Sub Push(item As Object)
        _list.Add(item)
    End Sub

    Public Sub Pop()
        If Not IsEmpty() Then
            _list.RemoveAt(_list.Count - 1)
        End If
    End Sub

    Public Function Peek() As Object
        If Not IsEmpty() Then
            Return _list(_list.Count - 1)
        End If
        Return Nothing
    End Function

    Public Sub Clear()
        _list.Clear()
    End Sub

    Public Function IsEmpty() As Boolean
        Return _list.Count = 0
    End Function

    Public Function Count() As Integer
        Return _list.Count
    End Function
End Class
