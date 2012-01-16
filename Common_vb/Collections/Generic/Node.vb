Public Class Node(Of T)

    Private _item As T
    Private _count As Integer

    Public Property Item As T
        Get
            Return _item
        End Get
        Protected Set(ByVal value As T)
            _item = value
        End Set
    End Property

    Public Property Count As Integer
        Get
            Return _count
        End Get
        Set(ByVal value As Integer)
            _count = value
        End Set
    End Property

    Public Sub New(ByVal item As T, Optional ByVal initialCount As Integer = 0)
        _item = item
        _count = initialCount
    End Sub

    Public Sub Increment(Optional ByVal amount As Integer = 1)
        _count += amount
    End Sub

    Public Sub Decrement(Optional ByVal amount As Integer = 1)
        _count -= amount
    End Sub

End Class
