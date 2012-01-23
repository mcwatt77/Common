Imports System.Collections.Generic
Imports System.IO

Namespace Collections

    Public Class CountingDictionary

        Protected _innerDict As Dictionary(Of String, Node)

        Public Sub New()
            _innerDict = New Dictionary(Of String, Node)
        End Sub

        Public Sub New(ByVal capacity As Integer)
            _innerDict = New Dictionary(Of String, Node)(capacity)
        End Sub

        Public Property Generation As Integer

        Public Property ResetGenerations As Boolean

        ''' <summary>
        ''' Control whether any more new keys may be added.
        ''' When False existing keys will continue to be counted, but keys not already
        ''' in the table will not be addeded.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AllowNewKeys As Boolean = True

        Public ReadOnly Property Keys As Dictionary(Of String, Node).KeyCollection
            Get
                Return _innerDict.Keys
            End Get
        End Property

        Public ReadOnly Property Values As Dictionary(Of String, Node).ValueCollection
            Get
                Return _innerDict.Values
            End Get
        End Property

        Public Function Add(ByVal key As String) As Boolean
            Dim result As Boolean = False
            Dim node As Node = Nothing
            If _innerDict.TryGetValue(key, node) Then
                node.Increment()
                result = True
            Else
                If AllowNewKeys Then
                    node = New Node(1)
                    node.Generation = Generation + 1
                    _innerDict.Add(key, node)
                    result = True
                End If
            End If
            Return result
        End Function

        Public Sub Remove(ByVal key As String)
            Dim node As Node = Nothing
            If _innerDict.TryGetValue(key, node) Then
                If node.Count > 0 Then node.Decrement()
            End If
        End Sub

        Public Sub RemoveAll()
            _innerDict.Clear()
        End Sub

        Public Function Count(ByVal key As String) As Integer
            Dim result As Integer = 0
            Dim node As Node = Nothing
            If _innerDict.TryGetValue(key, node) Then
                result = node.Count
            End If
            Return result
        End Function

        Public Function CountAll() As Integer
            Dim result As Integer = 0
            For Each value As Node In _innerDict.Values
                result += value.Count
            Next
            Return result
        End Function

        Public Overridable Sub Load(ByVal path As String)
            If File.Exists(path) Then
                Using file As New StreamReader(path)
                    Dim line As String
                    While Not file.EndOfStream()
                        line = file.ReadLine()
                        Dim field As String() = line.Split(";")
                        If field.Length >= 2 Then
                            Dim node = New Node(field(1))
                            If field.Length = 3 Then
                                node.Generation = field(2)
                                Generation = Math.Max(Generation, node.Generation)
                            End If
                            _innerDict.Add(field(0), node)
                        End If
                    End While
                End Using
            End If
        End Sub

        Public Overridable Sub Save(ByVal path As String)
            Using outFile As New StreamWriter(path)
                For Each key In Keys
                    Dim node As Node = _innerDict(key)
                    If ResetGenerations Then node.Generation = 0
                    outFile.WriteLine( _
                        String.Format("{0};{1};{2}", key, node.Count, node.Generation))
                Next
            End Using
        End Sub

    End Class

End Namespace
