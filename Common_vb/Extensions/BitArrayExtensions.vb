Imports System.Runtime.CompilerServices
Imports System.Text

Public Module BitArrayExtensions

    <Extension()>
    Public Function RLEncode(ByVal this As BitArray) As String
        Dim result As String = String.Empty
        Dim runStart As Integer = 0
        Dim bitPos As Integer = 1
        While (bitPos < this.Length)
            If this(bitPos) <> this(runStart) Then
                Dim runLength = bitPos - runStart
                If runLength >= 5 Then
                    result &= EncodeRun(this(runStart), runLength)
                    runStart = bitPos
                Else
                    Dim nibble = this.Nibble(runStart)
                    result &= String.Format("{0:x}", nibble)
                    runStart = runStart + 4
                End If
                bitPos = runStart
            End If
            bitPos += 1
        End While
        If (runStart < this.Length) Then
            result &= EncodeRun(this(runStart), this.Length - runStart)
        End If
        Return result
    End Function

    <Extension()>
    Public Function RLDecode(ByVal this As BitArray, ByVal encoded As String) As BitArray
        Dim result = New BitArray(this.Length)
        If String.IsNullOrEmpty(encoded) Then Return result

        Dim chars As Char() = encoded.ToCharArray
        Dim runStart = 0
        Dim currentClass = GetCharClass(chars(runStart))

        Dim bitPos = 0
        Dim charPos = 1
        While charPos < chars.Length
            If GetCharClass(chars(charPos)) <> currentClass Then
                Dim runLength = charPos - runStart
                If currentClass = CharClass.HexDigit Then
                    ' Decode Hex Run
                    Dim nibble As Byte
                    For i = runStart To charPos - 1
                        nibble = Byte.Parse(chars(i), System.Globalization.NumberStyles.HexNumber)
                        result.SetNibble(bitPos, nibble)
                        bitPos += 4
                    Next
                Else
                    Dim runCount As Integer = DecodeRunValue(currentClass, chars, runStart, runLength)
                    result.SetRun(bitPos, currentClass = CharClass.OneRun, runCount)
                    bitPos += runCount
                End If
                ' Start a new run
                runStart = charPos
                If runStart < chars.Length Then currentClass = GetCharClass(chars(runStart))
            End If
            charPos += 1
        End While
        If runStart < this.Length Then
            ' Hit the end of the array while still in a run.
            If currentClass = CharClass.HexDigit Then
                Dim nibble As Byte
                For i = runStart To chars.Length - 1
                    nibble = Byte.Parse(chars(i), System.Globalization.NumberStyles.HexNumber)
                    result.SetNibble(bitPos, nibble)
                    bitPos += 4
                Next
            Else
                Dim runLength = chars.Length - runStart
                Dim runCount As Integer = DecodeRunValue(currentClass, chars, runStart, runLength)
                result.SetRun(bitPos, currentClass = CharClass.OneRun, runCount)
                bitPos += runCount
            End If
        End If

        Return result
    End Function

    Private Enum CharClass
        ZeroRun = 0
        OneRun = 1
        HexDigit = 2
    End Enum

    Private Function GetCharClass(ByVal ch As Char) As CharClass
        If System.Uri.IsHexDigit(ch) Then Return CharClass.HexDigit
        Return If(System.Char.IsUpper(ch), CharClass.OneRun, CharClass.ZeroRun)
    End Function

    ''' <summary>
    ''' Encodes a run length as a hex literal using digits g..v for a run of zeros,
    ''' and G..V as a run of ones
    ''' </summary>
    ''' <param name="bit"></param>
    ''' <param name="runLength"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function EncodeRun(ByVal bit As Boolean, ByVal runLength As Integer) As String
        If runLength = 0 Then Return String.Empty
        Dim result As New StringBuilder(32)
        Dim hexrun As String = String.Format("{0:x}", runLength)
        Dim baseValue As Integer = If(bit, Asc("G"), Asc("g"))
        For Each digit As Char In hexrun
            Dim val As Integer
            If (digit >= "0" AndAlso digit <= "9") Then
                val = Asc(digit) - Asc("0")
            End If
            If (digit >= "a" AndAlso digit <= "f") Then
                val = 10 + Asc(digit) - Asc("a")
            End If
            result.Append(Chr(baseValue + val))
        Next
        Return result.ToString()
    End Function

    Private Function DecodeRunValue(ByVal charClass As CharClass, ByVal chars As Char(),
                                    ByVal runStart As Integer, ByVal runLength As Integer) As Integer
        Dim result As Integer = 0
        Dim baseValue As Integer = If(charClass = charClass.OneRun, Asc("G"), Asc("g"))
        For i = runStart To runStart + runLength - 1
            result = result * 16 + Asc(chars(i)) - baseValue
        Next
        Return result
    End Function

    <Extension()>
    Public Function Nibble(ByVal this As BitArray, ByVal index As Integer) As Byte
        Dim result As Byte = 0
        Dim bit As Byte = 1
        For i = 0 To 3
            If index + i >= this.Length Then Exit For
            result = If(this(index + i), result Or bit, result)
            bit <<= 1
        Next
        Return result
    End Function

    <Extension()>
    Public Sub SetNibble(ByVal this As BitArray, ByVal index As Integer, ByVal nibble As Byte)
        Dim offset As Integer = 0
        While offset < 4 And index + offset < this.Length
            this(index + offset) = (nibble And &H1) = 1
            nibble >>= 1
            offset += 1
        End While
    End Sub

    <Extension()>
    Public Sub SetRun(ByVal this As BitArray, ByVal index As Integer, ByVal value As Boolean, ByVal runCount As Integer)
        For i = index To index + runCount - 1
            this.Set(i, value)
        Next
    End Sub

    <Extension()>
    Public Function CreateMask(ByVal this As BitArray, ByVal index As Integer, ByVal bits As Integer) As BitArray
        If (bits < 1 OrElse bits > 64) Then
            Throw New ArgumentOutOfRangeException("CreateMask: bits must be between 1 and 64")
        End If
        If (index < 0 OrElse index >= this.Length) Then
            Throw New ArgumentOutOfRangeException("CreateMask: index out of range")
        End If
        If index + bits > this.Length Then bits = this.Length - index

        Dim result As New BitArray(this.Length)
        For i = index To index + bits - 1
            result.Set(i, True)
        Next
        Return result
    End Function

    <Extension()>
    Public Function ToBinary(ByVal this As BitArray)
        Dim sb As New StringBuilder(this.Length)
        For i = 0 To this.Length - 1
            If (i > 0 And i Mod 4 = 0) Then sb.Insert(0, "_")
            sb.Insert(0, If(this(i), "1", "0"))
        Next
        Return sb.ToString
    End Function

    <Extension()>
    Public Function ToHex(ByVal this As BitArray)
        Dim sb As New StringBuilder(CInt(this.Length / 4))
        For index = 0 To this.Length Step 4
            Dim nibble = this.Nibble(index)
            sb.Insert(0, String.Format("{0:x}", nibble))
        Next
        Return sb.ToString
    End Function

End Module
