Imports System.Collections.Generic
Imports System.IO

Namespace My.Collections

    Public Class CountingDictionary

        Protected _innerDict As Dictionary(Of String, StringNode)

        Public Sub New()
            _innerDict = New Dictionary(Of String, StringNode)
        End Sub

        Public Sub New(ByVal capacity As Integer)
            _innerDict = New Dictionary(Of String, StringNode)(capacity)
        End Sub

        ''' <summary>
        ''' Control whether any more new keys may be added.
        ''' When False existing keys will continue to be counted, but keys not already
        ''' in the table will not be addeded.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AllowNewKeys As Boolean = True

        Public ReadOnly Property Keys As Dictionary(Of String, StringNode).KeyCollection
            Get
                Return _innerDict.Keys
            End Get
        End Property

        Public ReadOnly Property Values As Dictionary(Of String, StringNode).ValueCollection
            Get
                Return _innerDict.Values
            End Get
        End Property

        Public Function Add(ByVal key As String) As Boolean
            Dim result As Boolean = False
            Dim node As StringNode = Nothing
            If _innerDict.TryGetValue(key, node) Then
                node.Increment()
                result = True
            Else
                If AllowNewKeys Then
                    node = New StringNode(key, 1)
                    _innerDict.Add(key, node)
                    result = True
                End If
            End If
            Return result
        End Function

        Public Sub Remove(ByVal key As String)
            Dim node As StringNode = Nothing
            If _innerDict.TryGetValue(key, node) Then
                If node.Count > 0 Then node.Decrement()
            End If
        End Sub

        Public Sub RemoveAll()
            _innerDict.Clear()
        End Sub

        Public Function Count(ByVal key As String) As Integer
            Dim result As Integer = 0
            Dim node As StringNode = Nothing
            If _innerDict.TryGetValue(key, node) Then
                result = node.Count
            End If
            Return result
        End Function

        Public Function CountAll() As Integer
            Dim result As Integer = 0
            For Each value As StringNode In _innerDict.Values
                result += value.Count
            Next
            Return result
        End Function

        Public Overridable Sub Load(ByVal path As String)
            Using file As New StreamReader(path)
                Dim line As String
                While Not file.EndOfStream()
                    line = file.ReadLine()
                    Dim field As String() = line.Split(";")
                    If field.Length = 2 Then
                        Dim node = New StringNode(field(0), field(1))
                        _innerDict.Add(field(0), node)
                    End If
                End While
            End Using
        End Sub

        Public Overridable Sub Save(ByVal path As String)
            Using outFile As New StreamWriter(path)
                For Each value In Values
                    outFile.WriteLine("{0};[1}", value.Key, value.Count)
                Next
            End Using
        End Sub

    End Class

End Namespace
