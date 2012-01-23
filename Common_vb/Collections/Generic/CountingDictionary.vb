Imports System.Collections.Generic
Imports System.IO

Namespace Collections.Generic

    Public Class CountingDictionary(Of T)

        Protected _innerDict As Dictionary(Of String, Node(Of T))
        Protected _keyOf As Func(Of T, String)

        Public Sub New()
            _innerDict = New Dictionary(Of String, Node(Of T))
            _keyOf = AddressOf DefaultKey
        End Sub

        Public Sub New(ByVal capacity As Integer)
            _innerDict = New Dictionary(Of String, Node(Of T))(capacity)
            _keyOf = AddressOf DefaultKey
        End Sub

        Public Sub New(ByVal keyOf As Func(Of T, String))
            _innerDict = New Dictionary(Of String, Node(Of T))
            _keyOf = keyOf
        End Sub

        Public Sub New(ByVal keyOf As Func(Of T, String), ByVal capacity As Integer)
            _innerDict = New Dictionary(Of String, Node(Of T))(capacity)
            _keyOf = keyOf
        End Sub

        Private Function DefaultKey(ByVal item As T) As String
            Return item.GetHashCode.ToString()
        End Function

        ''' <summary>
        ''' Control whether any more new keys may be added.
        ''' When False existing keys will continue to be counted, but keys not already
        ''' in the table will not be addeded.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AllowNewKeys As Boolean = True

        Public ReadOnly Property Keys As Dictionary(Of String, Node(Of T)).KeyCollection
            Get
                Return _innerDict.Keys
            End Get
        End Property

        Public ReadOnly Property Values As Dictionary(Of String, Node(Of T)).ValueCollection
            Get
                Return _innerDict.Values
            End Get
        End Property

        Public Function Add(ByVal item As T) As Boolean
            Dim result As Boolean = False
            Dim node As Node(Of T) = Nothing
            Dim key As String = _keyOf(item)
            If _innerDict.TryGetValue(key, node) Then
                node.Increment()
                result = True
            Else
                If AllowNewKeys Then
                    node = New Node(Of T)(item, 1)
                    _innerDict.Add(key, node)
                    result = True
                End If
            End If
            Return result
        End Function

        Public Sub Remove(ByVal item As T)
            Dim node As Node(Of T) = Nothing
            If _innerDict.TryGetValue(_keyOf(item), node) Then
                If node.Count > 0 Then node.Decrement()
            End If
        End Sub

        Public Sub RemoveAll()
            _innerDict.Clear()
        End Sub

        Public Function Count(ByVal item As T) As Integer
            Dim result As Integer = 0
            Dim node As Node(Of T) = Nothing
            If _innerDict.TryGetValue(_keyOf(item), node) Then
                result = node.Count
            End If
            Return result
        End Function

        Public Function CountAll() As Integer
            Dim result As Integer = 0
            For Each value As Node(Of T) In _innerDict.Values
                result += value.Count
            Next
            Return result
        End Function


    End Class
End Namespace

