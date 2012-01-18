Namespace My.Collections

    Public Class Node

        Public Property Count As Integer
        Public Property Generation As Integer

        Public Sub New(Optional ByVal initialCount As Integer = 0)
            _Count = initialCount
        End Sub

        Public Sub Increment(Optional ByVal amount As Integer = 1)
            _count += amount
        End Sub

        Public Sub Decrement(Optional ByVal amount As Integer = 1)
            _count -= amount
        End Sub

    End Class

End Namespace
