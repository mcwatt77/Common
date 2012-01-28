Imports System.Runtime.CompilerServices

Module StringExtensions

    ''' <summary>
    ''' Returns an elided version the given string truncated to the specified length.
    ''' The elision occurs in the middle of the string.
    ''' </summary>
    ''' <param name="this">The string to elide.</param>
    ''' <param name="len">The desired length of the elided string (including the ...)</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function Elide(ByVal this As String, ByVal len As Integer) As String
        len = len - 3
        If len <= 0 Then Return "..."
        Dim left, right As Integer
        left = len / 2
        right = left + len Mod 2
        Return this.Elide(left, right)
    End Function

    ''' <summary>
    ''' Returns an elided version the given string truncated to the specified length.
    ''' The elision occurs at start the start of the string.
    ''' </summary>
    ''' <param name="this">The string to elide.</param>
    ''' <param name="len">The desired length of the elided string (including the ...)</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function ElideLeft(ByVal this As String, ByVal Len As Integer) As String
        Return this.Elide(0, Len)
    End Function

    ''' <summary>
    ''' Returns an elided version the given string truncated to the specified length.
    ''' The elision occurs at end the start of the string.
    ''' </summary>
    ''' <param name="this">The string to elide.</param>
    ''' <param name="len">The desired length of the elided string (including the ...)</param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    <Extension()>
    Public Function ElideRight(ByVal this As String, ByVal Len As Integer) As String
        Return this.Elide(Len, 0)
    End Function

    ''' <summary>
    ''' Returns an elided version of the given string.
    ''' </summary>
    ''' <param name="this">The string to elide.</param>
    ''' <param name="left">The number of characters to take from the start of the string.</param>
    ''' <param name="right">The number of characters to take from the left of the string.</param>
    ''' <returns></returns>
    ''' <remarks>If left + right is greater than this.Length, the original string is returned.</remarks>
    <Extension()>
    Public Function Elide(ByVal this As String, ByVal left As Integer, right As Integer) As String
        If left < 0 Or right < 0 Then Throw New ArgumentException("Arguments can not be negative.")
        If this.Length <= left + right Then Return this

        Dim lstr As String = this.Substring(0, left)
        Dim rstr As String = String.Empty
        If right > 0 Then
            rstr = this.Substring(this.Length - right)
        End If

        Return lstr & "..." & rstr
    End Function



End Module
