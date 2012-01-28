Imports System.Runtime.CompilerServices

Module DateExtensions

    ''' <summary>
    ''' Computes current age in year for the given date.
    ''' </summary>
    ''' <param name="birthdate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function CurrentAge(ByVal birthdate As DateTime) As Integer
        Return DateTime.Now.YearsSince(birthdate)
    End Function

    ''' <summary>
    ''' For the given date compute the years since the start date.
    ''' </summary>
    ''' <param name="EndDate"></param>
    ''' <param name="StartDate"></param>
    ''' <returns></returns>
    ''' <remarks>Returns a negative number when StartDate is after EndDate</remarks>
    <Extension()>
    Public Function YearsSince(ByVal EndDate As DateTime, ByVal StartDate As DateTime) As Integer
        ' Returns the number of years between the passed dates
        If StartDate > EndDate Then Return -StartDate.YearsSince(EndDate)

        If Month(EndDate) < Month(StartDate) Or _
                (Month(EndDate) = Month(StartDate) And _
                (EndDate.Day) < (StartDate.Day)) Then
            Return Year(EndDate) - Year(StartDate) - 1
        Else
            Return Year(EndDate) - Year(StartDate)
        End If
    End Function

End Module
