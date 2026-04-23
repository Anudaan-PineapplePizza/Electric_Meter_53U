Option Strict On
Imports System
Imports System.Windows.Forms
Imports System.Net.Sockets
Imports System.Net
Imports System.Threading.Thread

Public Delegate Sub InternalUseOnly(ByVal s As String)

' Version information
' 3.0.0 (16/04/2010) : Independant Modbus dll 
' 3.1.0 (10/05/2010) : Modbus dll VB2008
' 3.2.0 (24/01/2011) : Remove threads
' 3.3.0 (13/01/2012) : Use Serial port object of framework 2.0
' 3.4.0 (23/11/2012) : Implement 3 retries on any communication error (timeout, checksum error...)
' 3.4.1 (16/07/2015) : Built for 32 and 64 Bits (plateform x86)

' 3.5.0 (01/01/2013) : Include TCP Modbus driver, Add Input registers functions (Be careful-> built for 32 bits only!!!)
' 4.0.0 (13/03/2015) : Include FTDI driver, Built for 32 and 64 Bits (plateform x86)

Public Class Class_ModbusTCP

    '=========================================================================
    ' Public variables
    '=========================================================================
    Public LastErrorMsg As String
    Public DebugMsg As String
    Public IsOpen As Boolean
    Public TimeOut As Integer = 500
    Public NbRetry As Integer = 3

    Public Enum E_MODBUS_TYPE As Integer
        Modbus = 0
        TCP_Modbus = 1
        FTDI_Modbus = 2
    End Enum

    '=========================================================================
    ' Private variables
    '=========================================================================
    Private Const prv_Version As String = "V 4.0"
    Private prv_Callback As Object
    Private SerialPort As New IO.Ports.SerialPort
    Private prv_WaitTimer As Timer
    Private prv_Initialized As Boolean
    Private prv_DebugMode As Boolean
    Private prv_DemoMode As Boolean
    Private prv_ActualRetry As Integer = 0
    Private prv_ConnectionType As E_MODBUS_TYPE
    Private NumberOfBytesReceived As Integer

    Dim sck As Socket
    Dim EndPoint As IPEndPoint
    Dim sendbuf As Byte()

    '=========================================================================
    ' General purpose functions
    '=========================================================================
    Public Property DebugMode() As Boolean
        Get
            Return prv_DebugMode
        End Get
        Set(ByVal Value As Boolean)
            prv_DebugMode = Value
        End Set
    End Property

    Public Property DemoMode() As Boolean
        Get
            Return prv_DemoMode
        End Get
        Set(ByVal Value As Boolean)
            prv_DemoMode = Value
        End Set
    End Property

    Public Property ConnectionType() As E_MODBUS_TYPE
        Get
            Return prv_ConnectionType
        End Get
        Set(ByVal Value As E_MODBUS_TYPE)
            prv_ConnectionType = Value
        End Set
    End Property

    Public Function GetDLLVersionInfo() As String
        If prv_DemoMode Then
            Return prv_Version & "Demo mode"
        End If
        Return prv_Version
    End Function

    Public Function Init_InternalUseOnly(ByVal f As Object) As Boolean
        prv_Callback = f
        prv_WriteDebug("Init_DebugCallback successed")
        Return True
    End Function

    'Public Function Open(ByVal PortNumber As Integer, ByVal PortSpeed As Integer) As Boolean
    '    Dim DeviceCount As Integer
    '    Dim DeviceIndex As Integer
    '    Dim TempDevString As String
    '    Dim MyFlags As Integer

    '    Try
    '        If prv_ConnectionType = E_MODBUS_TYPE.TCP_Modbus Then Return False

    '        If IsOpen Then
    '            If prv_ConnectionType = E_MODBUS_TYPE.Modbus Then
    '                SerialPort.Close()
    '            End If
    '            IsOpen = False
    '        End If

    '        '----------------------------------------------------------------
    '        'DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO 
    '        If prv_DemoMode Then
    '            IsOpen = True
    '            Return True
    '        End If
    '        'DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO 
    '        '----------------------------------------------------------------

    '        If prv_ConnectionType = E_MODBUS_TYPE.Modbus Then
    '            '----------------------------------------------------------------
    '            ' COM PORT DRIVER
    '            '----------------------------------------------------------------
    '            ' Attempt to open the port.
    '            SerialPort.StopBits = IO.Ports.StopBits.One
    '            SerialPort.BaudRate = PortSpeed
    '            SerialPort.DataBits = 8
    '            SerialPort.Parity = IO.Ports.Parity.None
    '            SerialPort.ReadBufferSize = 4096
    '            SerialPort.WriteBufferSize = 4096
    '            SerialPort.PortName = "COM" & PortNumber.ToString
    '            SerialPort.Open()


    '        End If

    '        IsOpen = True
    '        Return IsOpen
    '    Catch ex As Exception
    '        LastErrorMsg = "MB_OpenPort() failed : " & ex.Message
    '        Return False
    '    End Try
    'End Function

    Public Function Open(ByVal IP_Address As String,
                        ByVal TCP_Port As Integer) As Boolean
        Try

            If prv_ConnectionType = E_MODBUS_TYPE.Modbus Then Return False

            If Not (sck Is Nothing) Then
                sck.Close()
            End If
            IsOpen = False

            '----------------------------------------------------------------
            'DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO
            If prv_DemoMode Then
                IsOpen = True
                Return True
            End If
            'DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO 
            '----------------------------------------------------------------

            sck = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)

            sck.SendTimeout = 180
            sck.ReceiveTimeout = 180

            Dim result As IAsyncResult = sck.BeginConnect(IPAddress.Parse(IP_Address), TCP_Port, Nothing, Nothing)
            Dim success As Boolean = result.AsyncWaitHandle.WaitOne(200, True)

            If Not success Then
                sck.Close()
                Throw New ApplicationException("Failed to connect.")
            End If

            IsOpen = sck.Connected
            Return IsOpen

        Catch ex As Exception
            LastErrorMsg = "MB_Open() failed : " & ex.Message
            Return False
        End Try
    End Function


    Public Function Close() As Boolean
        Try
            If Not (prv_TransactionThread Is Nothing) Then
                If prv_TransactionThread.ThreadState <> Threading.ThreadState.Stopped Then
                    prv_TransactionThread.Abort()
                End If
            End If

            prv_WaitTimer.Enabled = False
            prv_WaitTimer.Stop()
            prv_WaitTimer.Dispose()

            '----------------------------------------------------------------
            'DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO
            If prv_DemoMode Then
                IsOpen = False
                Return True
            End If
            'DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO 
            '----------------------------------------------------------------

            If prv_ConnectionType = E_MODBUS_TYPE.Modbus Then
                SerialPort.Close()
            ElseIf prv_ConnectionType = E_MODBUS_TYPE.TCP_Modbus Then
                If Not sck Is Nothing Then sck.Close()
            End If

            IsOpen = False
            Return True
        Catch ex As Exception
            LastErrorMsg = "MB_Close() failed : " & ex.Message
            Return False
        End Try
    End Function

    Public Sub SetTimeout(ByVal value As Integer)
        TimeOut = value
        If prv_ConnectionType = E_MODBUS_TYPE.Modbus Then
            SerialPort.ReadTimeout = TimeOut
            SerialPort.WriteTimeout = TimeOut
        End If
    End Sub

    Public Function GetTimeOut() As Integer
        Return TimeOut
    End Function

    Public Sub SetMaxRetry(ByVal value As Integer)
        NbRetry = value
    End Sub

    Public Function GetMaxRetry() As Integer
        Return NbRetry
    End Function

    Private Const MB_READ_HOLDING_REGISTERS As Byte = 3
    Private Const MB_READ_INPUT_REGISTERS As Byte = 4
    Private Const MB_FORCE_SINGLE_COIL As Byte = 5
    Private Const MB_PRESET_SINGLE_REGISTER As Byte = 6
    Private Const MB_PRESET_MULTIPLE_REGISTERS As Byte = 16

    Public Function ReadInteger(
                        ByVal device_address As Integer,
                        ByVal register_address As Integer,
                        ByRef Value As Integer) As Boolean
        Dim pChar() As Byte = {0, 0}

        LastErrorMsg = ""
        Try

            If Not Read_NChar(device_address, register_address, pChar, 2) Then
                LastErrorMsg = "ReadInteger() failed : " & LastErrorMsg
                Return False
            End If


            Value = makeuint(pChar(0), pChar(1))
            Return True

        Catch ex As Exception
            LastErrorMsg = "ReadInteger() failed to execute : " & ex.Message
            Return False
        End Try

    End Function

    Public Function ReadIntegerInput(
                ByVal device_address As Integer,
                ByVal register_address As Integer,
                ByRef Value As Integer) As Boolean
        Dim pChar() As Byte = {0, 0}

        LastErrorMsg = ""
        Try
            If Not Read_NCharInput(device_address, register_address, pChar, 2) Then
                LastErrorMsg = "ReadIntegerInput() failed : " & LastErrorMsg
                Return False
            End If

            Value = makeuint(pChar(0), pChar(1))
            Return True

        Catch ex As Exception
            LastErrorMsg = "ReadIntegerInput() failed to execute : " & ex.Message
            Return False
        End Try

    End Function


    Public Function ReadString(
                        ByVal device_address As Integer,
                        ByVal register_address As Integer,
                        ByRef Value As String, ByVal Size As Integer) As Boolean
        Dim i As Integer
        Dim pChar() As Byte

        LastErrorMsg = ""
        Try
            Value = ""
            ReDim pChar(Size)

            If Not Read_NChar(device_address, register_address, pChar, Size) Then
                LastErrorMsg = "ReadString() failed : " & LastErrorMsg
                Return False
            End If

            For i = 0 To Size - 1
                If pChar(i) = 0 Then Exit For
                Value &= Chr(pChar(i))
            Next
            Return True

        Catch ex As Exception
            LastErrorMsg = "ReadString() failed to execute : " & ex.Message
            Return False
        End Try

    End Function

    Public Function WriteString(
                        ByVal device_address As Integer,
                        ByVal register_address As Integer,
                        ByVal Value As String,
                        ByVal Size As Integer) As Boolean
        Dim i As Integer
        Dim pChar() As Byte

        LastErrorMsg = ""
        Try
            ReDim pChar(Math.Max(Size, Value.Length))
            For i = 0 To Math.Min(Size, Value.Length) - 1
                pChar(i) = CByte(Asc(Value.Chars(i)))
            Next

            Return Write_NChar(device_address, register_address, pChar, Size)

        Catch ex As Exception
            LastErrorMsg = "Write_String() failed to execute : " & ex.Message
            Return False
        End Try

    End Function

    Public Function ReadFloat(
                        ByVal device_address As Integer,
                        ByVal register_address As Integer,
                        ByRef Value As Single) As Boolean
        Dim pChar() As Byte = {0, 0, 0, 0}

        LastErrorMsg = ""
        Try

            If Not Read_NChar(device_address, register_address, pChar, 4) Then
                LastErrorMsg = "ReadFloat() failed : " & LastErrorMsg
                Return False
            End If

            Value = makesingle(pChar(0), pChar(1), pChar(2), pChar(3))
            Return True

        Catch ex As Exception
            LastErrorMsg = "ReadFloat() failed to execute : " & ex.Message
            Return False
        End Try

    End Function

    Public Function ReadFloatInput(
                 ByVal device_address As Integer,
                 ByVal register_address As Integer,
                 ByRef Value As Single) As Boolean
        Dim pChar() As Byte = {0, 0, 0, 0}

        LastErrorMsg = ""
        Try
            If Not Read_NCharInput(device_address, register_address, pChar, 4) Then
                LastErrorMsg = "ReadFloatInput() failed : " & LastErrorMsg
                Return False
            End If

            Value = makesingle(pChar(0), pChar(1), pChar(2), pChar(3))
            Return True

        Catch ex As Exception
            LastErrorMsg = "ReadFloatInput() failed to execute : " & ex.Message
            Return False
        End Try

    End Function


    Public Function WriteInteger(
                        ByVal device_address As Integer,
                        ByVal register_address As Integer,
                        ByVal Value As Integer) As Boolean

        Dim pChar() As Byte = {0, 0}

        LastErrorMsg = ""
        Try

            'pChar(0) = WordHighByte(Value)
            'pChar(1) = WordLowByte(Value)

            If Not Write_NChar(device_address, register_address, pChar, 2) Then
                LastErrorMsg = "WriteInteger() failed : " & LastErrorMsg
                Return False
            End If

            Return True

        Catch ex As Exception
            LastErrorMsg = "WriteInteger() failed to execute : " & ex.Message
            Return False
        End Try

    End Function

    Public Function WriteFloat(
                        ByVal device_address As Integer,
                        ByVal register_address As Integer,
                        ByVal Value As Single) As Boolean
        Dim pChar() As Byte = {0, 0, 0, 0}

        LastErrorMsg = ""
        Try
            expandsingle(Value, pChar(0), pChar(1), pChar(2), pChar(3))
            If Not Write_NChar(device_address, register_address, pChar, 4) Then
                LastErrorMsg = "WriteFloat() failed : " & LastErrorMsg
                Return False
            End If

            Return True

        Catch ex As Exception
            LastErrorMsg = "WriteFloat() failed to execute : " & ex.Message
            Return False
        End Try

    End Function

    Public Function WriteULONG(
                    ByVal device_address As Integer,
                    ByVal register_address As Integer,
                    ByVal Value As Long) As Boolean
        Dim pChar() As Byte = {0, 0, 0, 0}

        LastErrorMsg = ""
        Try
            'ExpandULONG(Value, pChar(0), pChar(1), pChar(2), pChar(3))
            'If Not Write_NChar(device_address, register_address, pChar, 4) Then
            '    LastErrorMsg = "WriteULONG() failed" & LastErrorMsg
            '    Return False
            'End If

            Return True

        Catch ex As Exception
            LastErrorMsg = "WriteULONG() failed to execute : " & ex.Message
            Return False
        End Try

    End Function

    Public Function ReadULONG(
                        ByVal device_address As Integer,
                        ByVal register_address As Integer,
                        ByRef Value As Long) As Boolean
        Dim pChar() As Byte = {0, 0, 0, 0}

        LastErrorMsg = ""
        Try

            If Not Read_NChar(device_address, register_address, pChar, 4) Then
                LastErrorMsg = "ReadULONG() failed : " & LastErrorMsg
                Return False
            End If

            'Value = MakeULONG(pChar(0), pChar(1), pChar(2), pChar(3))
            Return True

        Catch ex As Exception
            LastErrorMsg = "ReadULONG() failed to execute : " & ex.Message
            Return False
        End Try

    End Function

    Public Function ReadULONGInput(
                    ByVal device_address As Integer,
                    ByVal register_address As Integer,
                    ByRef Value As Long) As Boolean
        Dim pChar() As Byte = {0, 0, 0, 0}

        LastErrorMsg = ""
        Try
            If Not Read_NCharInput(device_address, register_address, pChar, 4) Then
                LastErrorMsg = "ReadULONGInput() failed : " & LastErrorMsg
                Return False
            End If

            'Value = MakeULONG(pChar(0), pChar(1), pChar(2), pChar(3))
            Return True

        Catch ex As Exception
            LastErrorMsg = "ReadULONGInput() failed to execute : " & ex.Message
            Return False
        End Try

    End Function

    Public Function Write_NChar(
                        ByVal device_address As Integer,
                        ByVal registers_address As Integer,
                        ByRef bytes() As Byte,
                        ByVal bytes_count As Integer) As Boolean
        Dim i As Integer
        Dim crc As Integer
        Dim out_buffer() As Byte = {}
        Dim in_buffer() As Byte = {}
        Dim out_buffer_size As Integer

        LastErrorMsg = ""

        Try
            '=================================================================
            ' Construct and send the message
            '=================================================================
            Select Case prv_ConnectionType
                Case E_MODBUS_TYPE.Modbus, E_MODBUS_TYPE.FTDI_Modbus
                    out_buffer_size = bytes_count + 9
                    ReDim out_buffer(out_buffer_size - 1)
                    ReDim in_buffer(7)

                    out_buffer(0) = CByte(device_address)
                    out_buffer(1) = MB_PRESET_MULTIPLE_REGISTERS
                    out_buffer(2) = prv_RegisterHighByte(registers_address)
                    out_buffer(3) = prv_RegisterLowByte(registers_address)
                    out_buffer(4) = 0
                    out_buffer(5) = CByte(bytes_count / 2)
                    out_buffer(6) = CByte(bytes_count)
                    For i = 0 To bytes_count - 1
                        out_buffer(7 + i) = CByte(bytes(i))
                    Next

                    crc = prv_crc(out_buffer, 7 + bytes_count)
                    out_buffer(7 + bytes_count) = prv_RegisterLowByte(crc)
                    out_buffer(8 + bytes_count) = prv_RegisterHighByte(crc)

                Case E_MODBUS_TYPE.TCP_Modbus
                    out_buffer_size = bytes_count + 13
                    ReDim out_buffer(out_buffer_size - 1)
                    ReDim in_buffer(255)

                    out_buffer(0) = 0
                    out_buffer(1) = 0
                    out_buffer(2) = 0
                    out_buffer(3) = 0
                    out_buffer(4) = 0
                    out_buffer(5) = CByte(CByte(bytes_count) + 7)
                    out_buffer(6) = CByte(device_address)
                    out_buffer(7) = MB_PRESET_MULTIPLE_REGISTERS
                    out_buffer(8) = prv_RegisterHighByte(registers_address)
                    out_buffer(9) = prv_RegisterLowByte(registers_address)
                    out_buffer(10) = 0
                    out_buffer(11) = CByte(bytes_count / 2)
                    out_buffer(12) = CByte(bytes_count)
                    For i = 0 To bytes_count - 1
                        out_buffer(13 + i) = CByte(bytes(i))
                    Next

            End Select

RetryNow:
            If Not prv_MakeTransaction(out_buffer, out_buffer.Length,
                                       in_buffer, in_buffer.Length) Then
                ' Retry
                If prv_ActualRetry < NbRetry Then
                    prv_ActualRetry += 1
                    GoTo RetryNow
                End If
                LastErrorMsg = "Write_NChar() failed : " & LastErrorMsg
                Return False
            End If

            '=================================================================
            ' Check checksum
            '=================================================================
            If prv_ConnectionType = E_MODBUS_TYPE.Modbus Or
               prv_ConnectionType = E_MODBUS_TYPE.FTDI_Modbus Then
                crc = prv_crc(in_buffer, 6)

                If (in_buffer(6) <> prv_RegisterLowByte(crc)) Or
                   (in_buffer(7) <> prv_RegisterHighByte(crc)) Then
                    If prv_DemoMode Then
                        Return True
                    End If
                    ' Retry
                    If prv_ActualRetry < NbRetry Then
                        prv_ActualRetry += 1
                        GoTo RetryNow
                    End If
                    prv_WriteOutcomingMessage(out_buffer)
                    prv_WriteIncomingMessage(in_buffer)
                    LastErrorMsg = "Write_NChar() failed : " & "Checksum error"
                    Return False
                End If
            End If

            'Clear retry number
            prv_ActualRetry = 0

            Return True

        Catch ex As Exception
            LastErrorMsg = "Write_NChar() failed : " & ex.Message
            Return False
        End Try
    End Function

    Public Function Read_NChar(
                        ByVal device_address As Integer,
                        ByVal registers_address As Integer,
                        ByRef bytes() As Byte,
                        ByVal bytes_count As Integer) As Boolean
        Dim i As Integer
        Dim crc As Integer
        Dim out_buffer() As Byte = {}
        Dim in_buffer() As Byte = {}
        Dim in_buffer_size As Integer

        LastErrorMsg = ""

        Try
            '=================================================================
            ' Construct and send the message
            '=================================================================
            Select Case prv_ConnectionType
                Case E_MODBUS_TYPE.Modbus, E_MODBUS_TYPE.FTDI_Modbus
                    in_buffer_size = bytes_count + 5
                    ReDim out_buffer(7)
                    ReDim in_buffer(in_buffer_size - 1)

                    out_buffer(0) = CByte(device_address)
                    out_buffer(1) = MB_READ_HOLDING_REGISTERS
                    out_buffer(2) = prv_RegisterHighByte(registers_address)
                    out_buffer(3) = prv_RegisterLowByte(registers_address)
                    out_buffer(4) = 0
                    out_buffer(5) = CByte(bytes_count / 2)
                    crc = prv_crc(out_buffer, 6)
                    out_buffer(6) = prv_RegisterLowByte(crc)
                    out_buffer(7) = prv_RegisterHighByte(crc)
                Case E_MODBUS_TYPE.TCP_Modbus
                    in_buffer_size = 2048
                    ReDim out_buffer(11)
                    ReDim in_buffer(in_buffer_size - 1)

                    out_buffer(0) = 0
                    out_buffer(1) = 0
                    out_buffer(2) = 0
                    out_buffer(3) = 0
                    out_buffer(4) = 0
                    out_buffer(5) = 6

                    out_buffer(6) = CByte(device_address)
                    out_buffer(7) = MB_READ_HOLDING_REGISTERS
                    out_buffer(8) = prv_RegisterHighByte(registers_address)
                    out_buffer(9) = prv_RegisterLowByte(registers_address)
                    out_buffer(10) = 0
                    out_buffer(11) = CByte(bytes_count / 2)
            End Select
RetryNow:
            If Not prv_MakeTransaction(out_buffer, out_buffer.Length,
                                       in_buffer, in_buffer.Length) Then
                ' Retry
                If prv_ActualRetry < NbRetry Then
                    prv_ActualRetry += 1
                    GoTo RetryNow
                End If
                LastErrorMsg = "Read_NChar() failed : " & LastErrorMsg
                Return False
            End If

            '=================================================================
            ' Check checksum and get response
            '=================================================================
            Select Case prv_ConnectionType
                Case E_MODBUS_TYPE.Modbus, E_MODBUS_TYPE.FTDI_Modbus
                    crc = prv_crc(in_buffer, in_buffer_size - 2)

                    If (in_buffer(in_buffer_size - 2) <> prv_RegisterLowByte(crc)) Or
                       (in_buffer(in_buffer_size - 1) <> prv_RegisterHighByte(crc)) Then
                        If prv_DemoMode Then
                            Return True
                        End If
                        prv_WriteOutcomingMessage(out_buffer)
                        prv_WriteIncomingMessage(in_buffer)
                        ' Retry
                        If prv_ActualRetry < NbRetry Then
                            prv_ActualRetry += 1
                            GoTo RetryNow
                        End If
                        LastErrorMsg = "Read_NChar() failed : Checksum error"
                        Return False
                    End If

                    For i = 0 To bytes_count - 1
                        bytes(i) = in_buffer(i + 3)
                    Next

                Case E_MODBUS_TYPE.TCP_Modbus
                    For i = 9 To bytes_count - 1
                        ReDim Preserve bytes(i - 9)
                        bytes(i - 9) = in_buffer(i)
                    Next
            End Select

            'Clear retry number
            prv_ActualRetry = 0
            Return True

        Catch ex As Exception
            LastErrorMsg = "Read_NChar() failed : " & ex.Message
            Return False
        End Try
    End Function

    Public Function Read_NCharInput(
                       ByVal device_address As Integer,
                       ByVal registers_address As Integer,
                       ByRef bytes() As Byte,
                       ByVal bytes_count As Integer) As Boolean
        Dim i As Integer
        Dim crc As Integer
        Dim out_buffer() As Byte = {}
        Dim in_buffer() As Byte = {}
        Dim in_buffer_size As Integer

        LastErrorMsg = ""
        Try
            '=================================================================
            ' Construct and send the message
            '=================================================================
            Select Case prv_ConnectionType
                Case E_MODBUS_TYPE.Modbus, E_MODBUS_TYPE.FTDI_Modbus
                    in_buffer_size = bytes_count + 5
                    ReDim out_buffer(7)
                    ReDim in_buffer(in_buffer_size - 1)

                    out_buffer(0) = CByte(device_address)
                    out_buffer(1) = MB_READ_INPUT_REGISTERS
                    out_buffer(2) = prv_RegisterHighByte(registers_address)
                    out_buffer(3) = prv_RegisterLowByte(registers_address)
                    out_buffer(4) = 0
                    out_buffer(5) = CByte(bytes_count / 2)

                    crc = prv_crc(out_buffer, 6)
                    out_buffer(6) = prv_RegisterLowByte(crc)
                    out_buffer(7) = prv_RegisterHighByte(crc)
                Case E_MODBUS_TYPE.TCP_Modbus
                    in_buffer_size = 256
                    ReDim out_buffer(11)
                    ReDim in_buffer(in_buffer_size - 1)

                    out_buffer(0) = 0
                    out_buffer(1) = 0
                    out_buffer(2) = 0
                    out_buffer(3) = 0
                    out_buffer(4) = 0
                    out_buffer(5) = 6

                    out_buffer(6) = CByte(device_address)
                    out_buffer(7) = MB_READ_INPUT_REGISTERS
                    out_buffer(8) = prv_RegisterHighByte(registers_address)
                    out_buffer(9) = prv_RegisterLowByte(registers_address)
                    out_buffer(10) = 0
                    out_buffer(11) = CByte(bytes_count / 2)
            End Select

RetryNow:
            If Not prv_MakeTransaction(out_buffer, out_buffer.Length,
                                       in_buffer, in_buffer.Length) Then
                ' Retry
                If prv_ActualRetry < NbRetry Then
                    prv_ActualRetry += 1
                    GoTo RetryNow
                End If
                LastErrorMsg = "Read_NCharInput() failed : " & LastErrorMsg
                Return False
            End If

            '=================================================================
            ' Check checksum and get response
            '=================================================================
            Select Case prv_ConnectionType
                Case E_MODBUS_TYPE.Modbus, E_MODBUS_TYPE.FTDI_Modbus
                    crc = prv_crc(in_buffer, in_buffer_size - 2)

                    If (in_buffer(in_buffer_size - 2) <> prv_RegisterLowByte(crc)) Or
                       (in_buffer(in_buffer_size - 1) <> prv_RegisterHighByte(crc)) Then
                        If prv_DemoMode Then
                            Return True
                        End If
                        prv_WriteOutcomingMessage(out_buffer)
                        prv_WriteIncomingMessage(in_buffer)
                        ' Retry
                        If prv_ActualRetry < NbRetry Then
                            prv_ActualRetry += 1
                            GoTo RetryNow
                        End If
                        LastErrorMsg = "Read_NCharInput() failed : Checksum error"
                        Return False
                    End If
                    For i = 0 To bytes_count - 1
                        bytes(i) = in_buffer(i + 3)
                    Next
                Case E_MODBUS_TYPE.TCP_Modbus
                    For i = 9 To NumberOfBytesReceived - 1
                        ReDim Preserve bytes(i - 9)
                        bytes(i - 9) = in_buffer(i)
                    Next
            End Select

            'Clear retry number
            prv_ActualRetry = 0
            Return True

        Catch ex As Exception
            LastErrorMsg = "Read_NCharInput() failed : " & ex.Message
            Return False
        End Try
    End Function

    Public Function SetSingleCoil(
                        ByVal device_address As Integer,
                        ByVal coil_address As Integer,
                        ByRef coil As Boolean) As Boolean
        Dim crc As Integer
        Dim out_buffer() As Byte = {}
        Dim in_buffer() As Byte = {}

        LastErrorMsg = ""
        Try
            '=================================================================
            ' Construct and send the message
            '=================================================================
            Select Case prv_ConnectionType
                Case E_MODBUS_TYPE.Modbus, E_MODBUS_TYPE.FTDI_Modbus
                    ReDim out_buffer(7)
                    ReDim in_buffer(7)

                    out_buffer(0) = CByte(device_address)
                    out_buffer(1) = MB_FORCE_SINGLE_COIL
                    out_buffer(2) = prv_RegisterHighByte(coil_address)
                    out_buffer(3) = prv_RegisterLowByte(coil_address)
                    out_buffer(4) = 0
                    out_buffer(5) = CByte(IIf(coil, &HFF, 0))
                    crc = prv_crc(out_buffer, 6)
                    out_buffer(6) = prv_RegisterLowByte(crc)
                    out_buffer(7) = prv_RegisterHighByte(crc)

                Case E_MODBUS_TYPE.TCP_Modbus
                    ReDim out_buffer(11)
                    ReDim in_buffer(255)

                    out_buffer(0) = 0
                    out_buffer(1) = 0
                    out_buffer(2) = 0
                    out_buffer(3) = 0
                    out_buffer(4) = 0
                    out_buffer(5) = 6
                    out_buffer(6) = CByte(device_address)
                    out_buffer(7) = MB_FORCE_SINGLE_COIL
                    out_buffer(8) = prv_RegisterHighByte(coil_address)
                    out_buffer(9) = prv_RegisterLowByte(coil_address)
                    out_buffer(10) = 0
                    out_buffer(11) = CByte(IIf(coil, &HFF, 0))
            End Select

RetryNow:
            If Not prv_MakeTransaction(out_buffer, out_buffer.Length,
                                       in_buffer, in_buffer.Length) Then
                ' Retry
                If prv_ActualRetry < NbRetry Then
                    prv_ActualRetry += 1
                    GoTo RetryNow
                End If
                LastErrorMsg = "SetSingleCoil() failed : " & LastErrorMsg
                Return False
            End If

            '=================================================================
            ' Message back is the echo of the message, checksum should be the same
            '=================================================================
            ' RS485 Only
            If prv_ConnectionType = E_MODBUS_TYPE.Modbus Or
                prv_ConnectionType = E_MODBUS_TYPE.FTDI_Modbus Then
                If (out_buffer(6) <> in_buffer(6)) Or
                   (out_buffer(7) <> in_buffer(7)) Then
                    If prv_DemoMode Then
                        Return True
                    End If
                    prv_WriteOutcomingMessage(out_buffer)
                    prv_WriteIncomingMessage(in_buffer)
                    ' Retry
                    If prv_ActualRetry < NbRetry Then
                        prv_ActualRetry += 1
                        GoTo RetryNow
                    End If
                    LastErrorMsg = "SetSingleCoil() failed Checksum error"
                    Return False
                End If
            End If

            'Clear retry number
            prv_ActualRetry = 0

            Return True

        Catch ex As Exception
            LastErrorMsg = "SetSingleCoil() failed : " & ex.Message
            Return False
        End Try
    End Function

    '=========================================================================
    ' PRIVATE FUNCTIONS
    '=========================================================================
    Private Sub prv_WaitTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        CType(sender, Timer).Enabled = False
    End Sub

    Private Function prv_MakeTransaction(
                        ByRef o_buffer() As Byte,
                        ByVal o_count As Integer,
                        ByRef i_buffer() As Byte,
                        ByVal i_count As Integer) As Boolean


        Try
            prv_WaitTimer.Interval = 5000
            prv_WaitTimer.Enabled = True
            While prv_TransactionThreadInProgess
                'System.Threading.Thread.Sleep(10)
                Application.DoEvents()
                If prv_WaitTimer.Enabled = False Then
                    Throw New ApplicationException("TRANSACTION TIMEDOUT")
                End If
            End While

            If Not IsOpen Then
                If prv_DemoMode Then Return True
                Throw New ApplicationException("Port is not open")
            End If

            prv_BufferOut = o_buffer
            prv_BufferIn = i_buffer
            prv_BufferInCount = prv_BufferIn.Length

            '=================================================================
            ' Start the thread
            '=================================================================
            LastErrorMsg = ""
            prv_fTransactionThread()

            If prv_DebugMode Then
                prv_WriteOutcomingMessage(o_buffer)
            End If

            If prv_DebugMode Then
                If prv_DemoMode Then
                    DebugMsg = "No message back in demo"
                    prv_WriteDebug("No message back in demo")
                Else
                    prv_WriteIncomingMessage(i_buffer)
                End If
            End If

            If Not prv_TransactionThreadResult Then
                Throw New ApplicationException(LastErrorMsg)
            End If

            Return True
        Catch ex As Exception
            LastErrorMsg = "prv_MakeTransaction() failed : " & ex.Message
            prv_WriteDebug(LastErrorMsg)
            DebugMsg = LastErrorMsg
            Return False
        Finally
            prv_WaitTimer.Enabled = False
        End Try
    End Function

    Private prv_TransactionThread As System.Threading.Thread
    Private prv_TransactionThreadResult As Boolean
    Private prv_TransactionThreadInProgess As Boolean
    Private prv_BufferIn(2048) As Byte
    Private prv_BufferInCount As Integer
    Private prv_BufferOut() As Byte

    Private Sub prv_fTransactionThread()
        Dim res As Integer
        Dim time As Date

        Try
            Select Case prv_ConnectionType
                '=======================================================================================================
                ' RS485 Modbus Standard
                '=======================================================================================================
                Case E_MODBUS_TYPE.Modbus
                    SerialPort.DiscardInBuffer()
                    SerialPort.DiscardOutBuffer()
                    If prv_DemoMode Then
                        DebugMsg = "Thread"
                        prv_WriteDebug("Thread")
                        'Threading.Thread.Sleep(10)
                        prv_TransactionThreadResult = True
                        Return
                    End If

                    '=====================================================================
                    ' Send frame 
                    '=====================================================================
                    SerialPort.Write(prv_BufferOut, 0, prv_BufferOut.Length)

                    Dim savecount As Integer = 0
                    time = Now.AddMilliseconds(TimeOut)
                    While savecount < prv_BufferInCount
                        Try
                            res = SerialPort.Read(prv_BufferIn, savecount, 1)
                        Catch ex As Exception
                            ' An exception is raised when there is no information to read.
                            If Now > time Then
                                Throw New ApplicationException("Timeout 1")
                            End If
                        End Try

                        Select Case res
                            Case -1
                                If Now > time Then
                                    Throw New ApplicationException("Timeout 2")
                                End If
                            Case 0
                                If Now > time Then
                                    Throw New ApplicationException("Zero characters returned")
                                End If
                            Case 1
                                If savecount = 1 Then
                                    If prv_BufferIn(savecount) >= &H80 Then
                                        Throw New ApplicationException("Exception")
                                    End If
                                End If
                                savecount = savecount + 1
                            Case Else
                                Throw New ApplicationException("Unexpected number of characters")
                        End Select

                    End While
                    '=======================================================================================================
                    ' RS485 Modbus FTDI
                    '=======================================================================================================

                    '=======================================================================================================
                    ' Modbus TCP
                    '=======================================================================================================
                Case E_MODBUS_TYPE.TCP_Modbus
                    sck.Send(prv_BufferOut, 0, prv_BufferOut.Length, SocketFlags.None)
                    ' We need to wait under Windows 10 otherwise communication doesn't work...
                    'Sleep(10)
                    NumberOfBytesReceived = sck.Receive(prv_BufferIn, SocketFlags.None)

                    Dim a As Integer
                    a = sck.ReceiveBufferSize
                    ' Check head's frame input is the same of head's output frame
                    If Not (prv_BufferOut(0) = prv_BufferIn(0)) Or
                         Not (prv_BufferOut(1) = prv_BufferIn(1)) Or
                         Not (prv_BufferOut(2) = prv_BufferIn(2)) Or
                         Not (prv_BufferOut(3) = prv_BufferIn(3)) Or
                         Not (prv_BufferOut(4) = prv_BufferIn(4)) Then
                        Throw New ApplicationException("Frame header Response is not correct")
                    End If

                    ' Check exception
                    If prv_BufferIn(7) >= &H80 Then
                        Throw New ApplicationException("Exception")
                    End If

                    ' Check length response frame
                    If Not (NumberOfBytesReceived = prv_BufferIn(5) + 6) Then
                        Throw New ApplicationException("Unexpected number of characters")
                    End If
            End Select

            prv_TransactionThreadResult = True

        Catch ex As Exception
            LastErrorMsg = "prv_fTransactionThread() failed : " & ex.Message
            prv_TransactionThreadResult = False
            Return
        Finally
            prv_TransactionThreadInProgess = False
        End Try
    End Sub

    Private Function prv_crc(
                        ByRef buffer() As Byte,
                        ByVal buffer_len As Integer) As Integer

        Dim crc As Integer = &HFFFF
        Dim count As Integer = 0

        Try
            'Do CRC16 calculation for each byte in caller's buffer
            While count < buffer_len
                crc = prv_do_crc_16(crc, buffer(count))
                count = count + 1
            End While

            'Return the result
            Return (crc)
        Catch ex As Exception
            Throw New ApplicationException("prv_crc() failed : " & ex.Message)
        End Try
    End Function

    Private Function prv_do_crc_16(
                        ByVal crc As Integer,
                        ByVal value As Integer) As Integer

        Const POLY16 As Integer = &HA001
        Dim n As Integer

        Try
            'First, XOR the data byte with the CRC word
            crc = crc Xor value
            'Repeat CRC loop 8 times
            For n = 0 To 7
                'If LSb is 1, shift CRC right 1 bit and XOR with the polynomial
                If (crc And 1) = 1 Then
                    crc = (crc >> 1)
                    crc = crc Xor POLY16
                Else
                    crc >>= 1 'Otherwise, just shift CRC right 1 bit
                End If
            Next n
            Return crc
        Catch ex As Exception
            Throw New ApplicationException("prv_do_crc_16() failed : " & ex.Message)
        End Try
    End Function

    Private Function prv_RegisterHighByte(ByVal v As Integer) As Byte
        ' NOTE : the passed value must be of type Integer so it can hold a value up to 65535.
        '        int16 is not appropriate since it can hold from -32,768 through 32,767 only.
        Return CByte(v >> 8)
    End Function

    Private Function prv_RegisterLowByte(ByVal v As Integer) As Byte
        ' NOTE : the passed value must be of type Integer so it can hold a value up to 65535.
        '        int16 is not appropriate since it can hold from -32,768 through 32,767 only.
        Return CByte(v Mod 256)
    End Function

    Public Sub New()
        prv_Init()
    End Sub

    Private Function prv_Init() As Boolean
        If prv_Initialized Then Return True

        prv_Initialized = True
        Try
            prv_WaitTimer = New Timer
            AddHandler prv_WaitTimer.Tick, AddressOf prv_WaitTimer_Tick
            Return True
        Catch ex As Exception
            LastErrorMsg = "Init() failed : " & ex.Message
            Return False
        End Try
    End Function

    Private Sub prv_WriteDebug(ByVal s As String)
        If Not prv_Callback Is Nothing Then
            Call CType(prv_Callback, InternalUseOnly)(s)
        End If
    End Sub

    Private Function prv_Hexa(ByVal b As Byte) As String
        Return b.ToString("X2") & " "
    End Function

    Private Sub prv_WriteIncomingMessage(ByRef bytes() As Byte)
        Dim i As Integer
        Dim s As String

        s = "< "
        For i = 0 To bytes.Length - 1
            s &= prv_Hexa(bytes(i))
        Next
        prv_WriteDebug(s)
        DebugMsg = s
    End Sub

    Private Sub prv_WriteOutcomingMessage(ByRef bytes() As Byte)
        Dim i As Integer
        Dim s As String

        s = "> "
        For i = 0 To bytes.Length - 1
            s &= prv_Hexa(bytes(i))
        Next
        prv_WriteDebug(s)
        DebugMsg = s
    End Sub

End Class
