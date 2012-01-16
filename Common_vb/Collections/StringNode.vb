Namespace My.Collections

    Public Class StringNode

        Private _key As String
        Private _count As String

        Public ReadOnly Property Key As String
            Get
                Return _key
            End Get
        End Property

        Public Property Count As Integer
            Get
                Return _count
            End Get
            Set(ByVal value As Integer)
                _count = value
            End Set
        End Property

        Public Sub New(ByVal key As String, Optional ByVal initialCount As Integer = 0)
            If key Is Nothing Then key = String.Empty
            _count = initialCount
        End Sub

        Public Sub Increment(Optional ByVal amount As Integer = 1)
            _count += amount
        End Sub

        Public Sub Decrement(Optional ByVal amount As Integer = 1)
            _count -= amount
        End Sub

    End Class

End Namespace
