Option Explicit On
Option Strict On

Friend Class Class_INI

    '=========================================================================
    ' API
    '=========================================================================
    Private Declare Function GetPrivateProfileInt Lib "kernel32.dll" _
                Alias "GetPrivateProfileIntA" (
                ByVal lpApplicationName As String,
                ByVal lpKeyName As String,
                ByVal nDefault As Integer,
                ByVal lpFileName As String
                ) As Integer

    Private Declare Function GetPrivateProfileString Lib "kernel32.dll" _
                Alias "GetPrivateProfileStringA" (
                ByVal lpApplicationName As String,
                ByVal lpKeyName As String,
                ByVal lpDefault As String,
                ByVal lpReturnedString As String,
                ByVal nSize As Integer,
                ByVal lpFileName As String
                ) As Integer

    Private Declare Function WritePrivateProfileString Lib "kernel32.dll" _
                Alias "WritePrivateProfileStringA" (
                ByVal lpApplicationName As String,
                ByVal lpKeyName As String,
                ByVal lpString As String,
                ByVal lpFileName As String
                ) As Integer

    '=========================================================================
    ' Privates
    '=========================================================================
    Private LastErrorMsg As String

    Private Function ReadKeyAsString(ByVal FileName As String,
                                     ByVal SectionName As String,
                                     ByVal KeyName As String,
                                     ByRef Value As String) As Boolean
        ' DOLATER : add default value 

        Dim returnedValue As String = " "
        Dim nSize As Integer
        Dim res As Integer

        Value = ""
        nSize = 32768
        returnedValue = returnedValue.PadLeft(nSize, " "c)

        Try
            res = GetPrivateProfileString(SectionName, KeyName, "", returnedValue, nSize, FileName)
            If res = 0 Then
                LastErrorMsg = "ReadKeyAsString() : GetPrivateProfileString() failed"
                Return False
            End If

            Value = returnedValue.Substring(0, res)
            LastErrorMsg = ""
            Return True
        Catch ex As Exception
            LastErrorMsg = "ReadKeyAsString() failed. "
            Return False
        End Try

    End Function

    '=========================================================================
    ' Publics
    '=========================================================================
    Public Function ReadStr(ByVal FileName As String,
                            ByVal SectionName As String,
                            ByVal KeyName As String,
                            ByRef ReturnedValue As String) As Boolean

        Return ReadKeyAsString(FileName, SectionName, KeyName, ReturnedValue)

    End Function

    Public Function ReadInt(ByVal FileName As String,
                            ByVal SectionName As String,
                            ByVal KeyName As String,
                            ByRef ReturnedValue As Integer) As Boolean
        Dim str As String

        If Not ReadKeyAsString(FileName, SectionName, KeyName, str) Then
            ReturnedValue = 0
            Return False
        End If

        Try
            ReturnedValue = CInt(Val(str))
            Return True
        Catch ex As Exception
            ReturnedValue = 0
            LastErrorMsg = "Value is not an integer. "
            Return False
        End Try
    End Function

    Public Function ReadFlt(ByVal FileName As String,
                            ByVal SectionName As String,
                            ByVal KeyName As String,
                            ByRef ReturnedValue As Single) As Boolean
        Dim str As String

        If Not ReadKeyAsString(FileName, SectionName, KeyName, str) Then
            ReturnedValue = 0
            Return False
        End If

        Try
            ReturnedValue = CSng(str)
            Return True
        Catch ex As Exception
            ReturnedValue = Single.NaN
            LastErrorMsg = "Value is not a float. "
            Return False
        End Try
    End Function

    Public Function ReadDbl(ByVal FileName As String,
                            ByVal SectionName As String,
                            ByVal KeyName As String,
                            ByRef ReturnedValue As Double) As Boolean
        Dim str As String

        If Not ReadKeyAsString(FileName, SectionName, KeyName, str) Then
            ReturnedValue = 0
            Return False
        End If

        Try
            ReturnedValue = CDbl(str)
            Return True
        Catch ex As Exception
            ReturnedValue = Double.NaN
            LastErrorMsg = "Value is not a double. "
            Return False
        End Try
    End Function

    Public Function ReadBln(ByVal FileName As String,
                            ByVal SectionName As String,
                            ByVal KeyName As String,
                            ByRef ReturnedValue As Boolean) As Boolean
        Dim str As String

        If Not ReadKeyAsString(FileName, SectionName, KeyName, str) Then
            ReturnedValue = False
            Return False
        End If

        ReturnedValue = CBool(IIf(str.ToUpper = "TRUE", True, False))
        Return True
    End Function

    Public Function WriteStr(ByVal FileName As String,
                             ByVal SectionName As String,
                             ByVal KeyName As String,
                             ByVal Value As String) As Boolean

        Dim res As Integer

        Try
            res = WritePrivateProfileString(SectionName, KeyName, Value, FileName)
            If res = 0 Then
                LastErrorMsg = "WriteStr() : WritePrivateProfileString() failed. "
                Return False
            End If
            Return True

        Catch ex As Exception
            LastErrorMsg = "WriteStr() failed. "
            Return False
        End Try


    End Function

    Public Function WriteInt(ByVal FileName As String,
                             ByVal SectionName As String,
                             ByVal KeyName As String,
                             ByVal Value As Integer) As Boolean

        Return WriteStr(FileName, SectionName, KeyName, Value.ToString)

    End Function

    Public Function WriteFlt(ByVal FileName As String,
                             ByVal SectionName As String,
                             ByVal KeyName As String,
                             ByVal Value As Single) As Boolean

        Return WriteStr(FileName, SectionName, KeyName, Value.ToString)

    End Function

    Public Function WriteDbl(ByVal FileName As String,
                             ByVal SectionName As String,
                             ByVal KeyName As String,
                             ByVal Value As Double) As Boolean

        Return WriteStr(FileName, SectionName, KeyName, Value.ToString)

    End Function

    Public Function WriteBln(ByVal FileName As String,
                             ByVal SectionName As String,
                             ByVal KeyName As String,
                             ByVal Value As Boolean) As Boolean

        Dim res As Boolean
        res = WriteStr(FileName, SectionName, KeyName, CStr(IIf(Value, "True", "False")))
        Return res

    End Function

    Public Function GetLastError() As String
        Return LastErrorMsg
    End Function

End Class
