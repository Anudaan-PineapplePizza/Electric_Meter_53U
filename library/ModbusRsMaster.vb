Option Strict On
Option Explicit On

Imports System.IO.Ports
Imports System.Collections.Generic
Imports EasyModbus

'=========================================================================
' STRUCTURE CONFIG MODBUS
'=========================================================================
Public Structure ModbusConfigStruct
    Public DeviceAddress As Integer
    Public Baudrate As Integer
    Public Parity As Parity
    Public StopBit As StopBits
    Public ConnectString As String
End Structure

'=========================================================================
' MODULE MODBUS MASTER
'=========================================================================
Public Module ModbusRsMaster

    Public Config As ModbusConfigStruct
    Private _modbusClient As ModbusClient = Nothing

    'UI
    Public _blinkState As Integer = 0
    Public State_Connected_Button As Integer = 0
    Public Connected As Boolean = False

    '=========================================================================
    ' FUNCTION CONNECT
    '=========================================================================
    Public Function Connect(port As String,
                            baudrate As Integer,
                            parity As Parity,
                            stopBits As StopBits,
                            deviceAddress As Byte) As Boolean
        Try
            _modbusClient = New ModbusClient(port)
            _modbusClient.Baudrate = baudrate
            _modbusClient.Parity = parity
            _modbusClient.StopBits = stopBits
            _modbusClient.ConnectionTimeout = 1500
            _modbusClient.UnitIdentifier = deviceAddress
            _modbusClient.Connect()
        Catch ex As Exception
            _modbusClient = Nothing
            MessageBox.Show(
                "Impossible d'ouvrir le port " & port & " :" & Environment.NewLine & ex.Message,
                "Erreur port COM", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try

        Try
            _modbusClient.ReadHoldingRegisters(0, 1)    'Test de lecture de registre avant de valider la connection
            Return True
        Catch ex As Exception
            Try : _modbusClient.Disconnect() : Catch : End Try
            _modbusClient = Nothing
            Connected = False
            Dim msg As String
            If ex.Message.ToLower().Contains("timeout") OrElse
               ex.Message.ToLower().Contains("no response") OrElse
               ex.Message.ToLower().Contains("crc") Then
                msg = "Le port s'est ouvert mais l'appareil ne repond pas."
            Else
                msg = "Erreur de communication : " & Environment.NewLine & ex.Message
            End If
            MessageBox.Show(msg, "Slave introuvable", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End Try
    End Function

    '=========================================================================
    ' FUNCTION DISCONNECT
    '=========================================================================
    Public Sub Disconnect()
        Try
            If _modbusClient IsNot Nothing AndAlso _modbusClient.Connected Then
                _modbusClient.Disconnect()
            End If
        Catch ex As Exception
            MessageBox.Show("Disconnect error: " & ex.Message)
        Finally
            _modbusClient = Nothing
            Connected = False
        End Try
    End Sub

    '=========================================================================
    ' IS CONNECTED ?
    '=========================================================================
    Public Function IsConnected() As Boolean
        Return _modbusClient IsNot Nothing AndAlso _modbusClient.Connected
    End Function

    '=========================================================================
    ' FUNCTION CONVERT REGISTERS TO DOUBLE
    '
    ' wordCount = 1 : 16 bits unsigned (for HD registers)
    ' wordCount = 2 : 32 bits little-endian (low word = index, high word = index+1)
    '                 (signe pour puissances P, Q negatifs)
    '=========================================================================
    Public Function ConvertRegistersToDouble(regs() As Integer, index As Integer, wordCount As Integer) As Double

        Select Case wordCount
            Case 1
                Return CDbl(regs(index) And &HFFFF)

            Case 2
                ' Assembler en UInt32 puis reinterpreter en Int32 signe 
                Dim uraw As UInt32 = (CUInt(regs(index + 1) And &HFFFF) << 16) Or CUInt(regs(index) And &HFFFF)

                ' BitConverter reinterpret : evite l'overflow de CInt sur valeurs > Int32.MaxValue
                Dim sraw As Int32 = BitConverter.ToInt32(BitConverter.GetBytes(uraw), 0)
                Return CDbl(sraw)

            Case Else
                Throw New ArgumentException("Wrong wordCount (1 - 2)")
        End Select
    End Function

    '=========================================================================
    ' READ REGISTERS 2W — lecture generique registres 32 bits
    '=========================================================================
    Public Function ReadRegisters2W(address As Integer, size As Integer) As Double()
        Try
            If _modbusClient Is Nothing OrElse Not _modbusClient.Connected Then Return Nothing
            Dim regs As Integer() = _modbusClient.ReadHoldingRegisters(address, size) 'lecture de registre de taille 'size'
            Dim valueCount As Integer = size \ 2
            Dim result(valueCount - 1) As Double
            For i As Integer = 0 To valueCount - 1
                result(i) = ConvertRegistersToDouble(regs, i * 2, 2)
            Next
            Return result
        Catch ex As Exception
            Return Nothing  ' erreur silencieuse
        End Try
    End Function

    '=========================================================================
    ' READ REGISTERS 1W — lecture generique registres 16 bits (pour HD)
    '=========================================================================
    Public Function ReadRegisters1W(address As Integer, size As Integer) As Double()
        Try
            If _modbusClient Is Nothing OrElse Not _modbusClient.Connected Then Return Nothing
            Dim regs As Integer() = _modbusClient.ReadHoldingRegisters(address, size)
            Dim result(size - 1) As Double
            For i As Integer = 0 To size - 1
                result(i) = ConvertRegistersToDouble(regs, i, 1)
            Next
            Return result
        Catch ex As Exception
            Return Nothing  ' erreur silencieuse
        End Try
    End Function

    '=========================================================================
    ' READ ALL VALUES — lecture optimisee de toutes les valeurs en 2 appels
    '
    ' Appel 1 : addr 0 a 87 (88 registres) → valeurs totales + par phase + angles
    ' Appel 2 : addr 1280 a 1299 (20 regs)  → 10 THDi et THDu
    '
    ' Retourne un Dictionary(SignalID → valeur physique convertie).
    ' Les signaux absents (erreur de lecture) ne sont pas dans le dictionnaire.
    '=========================================================================
    Public Function ReadAllValues() As Dictionary(Of SignalID, Double)
        Dim result As New Dictionary(Of SignalID, Double)
        If _modbusClient Is Nothing OrElse Not _modbusClient.Connected Then Return result

        ' ── Bloc 1 : registres 0-87 ───────────────────────────────────────────
        Try
            Dim r As Integer() = _modbusClient.ReadHoldingRegisters(0, 88)

            ' Totaux (addr 0-15, offset 0-15)
            result(SignalID.I) = ConvertRegistersToDouble(r, 0, 2) / 1000.0     ' mA → A
            result(SignalID.U) = ConvertRegistersToDouble(r, 2, 2) / 100.0      ' V/100 → V
            result(SignalID.P) = ConvertRegistersToDouble(r, 4, 2)              ' W
            result(SignalID.Q) = ConvertRegistersToDouble(r, 6, 2)              ' VAR
            result(SignalID.S) = ConvertRegistersToDouble(r, 8, 2)              ' VA
            result(SignalID.PF) = ConvertRegistersToDouble(r, 10, 2) / 10000.0  ' /100 → 0 a 1
            result(SignalID.F) = ConvertRegistersToDouble(r, 12, 2) / 100.0     ' Hz/100 → Hz

            result(SignalID.Phase_Dir) = If(ConvertRegistersToDouble(r, 14, 2) = 0.0, -1.0, 1.0) ' Phase_Dir : 0 = LAG  return -1  (inductif)
            '             1 = LEAD return +1  (capacitif)
            ' Phase currents (addr 32-39)
            result(SignalID.I1) = ConvertRegistersToDouble(r, 32, 2) / 1000.0
            result(SignalID.I2) = ConvertRegistersToDouble(r, 34, 2) / 1000.0
            result(SignalID.I3) = ConvertRegistersToDouble(r, 36, 2) / 1000.0
            result(SignalID.I_Neutral) = ConvertRegistersToDouble(r, 38, 2) / 1000.0

            ' Delta voltages (addr 40-45)
            result(SignalID.U12) = ConvertRegistersToDouble(r, 40, 2) / 100.0
            result(SignalID.U23) = ConvertRegistersToDouble(r, 42, 2) / 100.0
            result(SignalID.U31) = ConvertRegistersToDouble(r, 44, 2) / 100.0

            ' Phase voltages (addr 46-51)
            result(SignalID.U1N) = ConvertRegistersToDouble(r, 46, 2) / 100.0
            result(SignalID.U2N) = ConvertRegistersToDouble(r, 48, 2) / 100.0
            result(SignalID.U3N) = ConvertRegistersToDouble(r, 50, 2) / 100.0

            ' Per-phase power (addr 52-75)
            result(SignalID.P1) = ConvertRegistersToDouble(r, 52, 2)
            result(SignalID.P2) = ConvertRegistersToDouble(r, 54, 2)
            result(SignalID.P3) = ConvertRegistersToDouble(r, 56, 2)
            result(SignalID.Q1) = ConvertRegistersToDouble(r, 58, 2)
            result(SignalID.Q2) = ConvertRegistersToDouble(r, 60, 2)
            result(SignalID.Q3) = ConvertRegistersToDouble(r, 62, 2)
            result(SignalID.S1) = ConvertRegistersToDouble(r, 64, 2)
            result(SignalID.S2) = ConvertRegistersToDouble(r, 66, 2)
            result(SignalID.S3) = ConvertRegistersToDouble(r, 68, 2)
            result(SignalID.PF1) = ConvertRegistersToDouble(r, 70, 2) / 10000.0
            result(SignalID.PF2) = ConvertRegistersToDouble(r, 72, 2) / 10000.0
            result(SignalID.PF3) = ConvertRegistersToDouble(r, 74, 2) / 10000.0

            ' Direction par phase + angles (addr 76-87)
            result(SignalID.DIR1) = If(ConvertRegistersToDouble(r, 76, 2) = 0.0, -1.0, 1.0) ' Phase_Dir : 0 = LAG  return -1  (inductif)
            result(SignalID.DIR2) = If(ConvertRegistersToDouble(r, 78, 2) = 0.0, -1.0, 1.0)             ' 1 = LEAD return +1  (capacitif)
            result(SignalID.DIR3) = If(ConvertRegistersToDouble(r, 80, 2) = 0.0, -1.0, 1.0)
            result(SignalID.UT12) = ConvertRegistersToDouble(r, 82, 2)
            result(SignalID.UT23) = ConvertRegistersToDouble(r, 84, 2)
            result(SignalID.UT31) = ConvertRegistersToDouble(r, 86, 2)

        Catch
            Return Nothing  ' erreur lecture -> signale au timer
        End Try

        ' ── Bloc 2 : THD addr 1280-1299 ───────────────────────────────────────
        Try
            Dim t As Integer() = _modbusClient.ReadHoldingRegisters(1280, 20)
            result(SignalID.THDi1) = ConvertRegistersToDouble(t, 0, 2) / 10.0
            result(SignalID.THDi2) = ConvertRegistersToDouble(t, 2, 2) / 10.0
            result(SignalID.THDi3) = ConvertRegistersToDouble(t, 4, 2) / 10.0
            result(SignalID.THDiN) = ConvertRegistersToDouble(t, 6, 2) / 10.0
            result(SignalID.THDu12) = ConvertRegistersToDouble(t, 8, 2) / 10.0
            result(SignalID.THDu23) = ConvertRegistersToDouble(t, 10, 2) / 10.0
            result(SignalID.THDu31) = ConvertRegistersToDouble(t, 12, 2) / 10.0
            result(SignalID.THDu1N) = ConvertRegistersToDouble(t, 14, 2) / 10.0
            result(SignalID.THDu2N) = ConvertRegistersToDouble(t, 16, 2) / 10.0
            result(SignalID.THDu3N) = ConvertRegistersToDouble(t, 18, 2) / 10.0
        Catch
            ' THD non bloquant
        End Try

        Return result
    End Function

    '=========================================================================
    ' READ HARMONICS HD — harmoniques individuels 2eme a 31eme d'un groupe
    '
    ' Retourne Double(29) avec les valeurs en % (index 0 = 2eme harmonique).
    '=========================================================================

    ' Enum HDGroup : HDI1=0 a HDU3N=9 (correspond aux 10 groupes de HD du 53U)
    Public Enum HDGroup As Integer
        HDI1 = 0   ' Current harmonic Line 1   — addr base 1537
        HDI2 = 1   ' Current harmonic Line 2   — addr base 1601
        HDI3 = 2   ' Current harmonic Line 3   — addr base 1665
        HDIN = 3   ' Neutral current harmonic  — addr base 1729
        HDU12 = 4   ' Delta voltage 1-2         — addr base 1793
        HDU23 = 5   ' Delta voltage 2-3         — addr base 1857
        HDU31 = 6   ' Delta voltage 3-1         — addr base 1921
        HDU1N = 7   ' Phase voltage Phase 1     — addr base 1985
        HDU2N = 8   ' Phase voltage Phase 2     — addr base 2049
        HDU3N = 9   ' Phase voltage Phase 3     — addr base 2113
    End Enum

    ' Adresses de base (HD2) pour chaque Enum HDGroup
    Private ReadOnly HD_BASE_ADDR() As Integer = {
        1536, 1600, 1664, 1728, 1792, 1856, 1920, 1984, 2048, 2112
    }

    '=========================================================================
    ' READ HARMONICS (HD)
    ' Lit les 30 harmoniques (2eme a 31eme) d'un groupe HD.

    ' index 0 = H2, index 1 = H3, ..., index 5 = H7
    ' donc result(0) = 2eme harmonique, result(29) = 31eme harmonique.

    ' Retourne Nothing si erreur.
    '=========================================================================

    Public Const HD_HARMONICS_COUNT As Integer = 30   ' default  H2 to H31 (30 harmonic orders per group)

    Public Function ReadHarmonics(group As HDGroup) As Double()
        If Not IsConnected() Then Return Nothing
        Try
            Dim baseAddr As Integer = HD_BASE_ADDR(CInt(group))
            Dim regs As Integer() = _modbusClient.ReadHoldingRegisters(baseAddr, HD_HARMONICS_COUNT)

            If regs Is Nothing OrElse regs.Length < HD_HARMONICS_COUNT Then Return Nothing
            Dim result(HD_HARMONICS_COUNT - 1) As Double
            For i As Integer = 0 To HD_HARMONICS_COUNT - 1
                result(i) = ConvertRegistersToDouble(regs, i, 1) / 10.0   ' %/10 -> %
            Next
            Return result
        Catch
            Return Nothing
        End Try
    End Function

    '=========================================================================
    ' FUNCTION WRITE — Protection d'ecriture 53U                         (BUG)
    '                - passcode configure = (0001)
    '=========================================================================
    Public Sub DisableWritingProtection53U(passcode As Integer)
        If _modbusClient Is Nothing OrElse Not _modbusClient.Connected Then
            MessageBox.Show("DisableWritingProtection53U : non connecte.")
            Return
        End If
        Try
            Dim lowWord As Integer = passcode And &HFFFF
            Dim highWord As Integer = CInt(CUInt(passcode) >> 16)
            _modbusClient.WriteSingleRegister(4942, lowWord)
            _modbusClient.WriteSingleRegister(4943, highWord)
            _modbusClient.WriteSingleRegister(4944, 1)
        Catch ex As Exception
            MessageBox.Show("Erreur DisableWritingProtection53U : " & ex.Message)
        End Try
    End Sub

    Public Sub EnableWritingProtection53U()
        If _modbusClient Is Nothing OrElse Not _modbusClient.Connected Then Return
        Try
            _modbusClient.WriteSingleRegister(4944, 0)
            _modbusClient.WriteSingleRegister(4942, 0)
            _modbusClient.WriteSingleRegister(4943, 0)
        Catch ex As Exception
            MessageBox.Show("Erreur EnableWritingProtection53U : " & ex.Message)
        End Try
    End Sub

    '=========================================================================
    ' FUNCTION WRITE — Retroeclairage LCD 53U (1 a 3)                    (BUG)
    '=========================================================================
    Public Sub Backlight53U_Change(level As Integer, passcode As Integer)
        If level < 1 Or level > 3 Then
            MessageBox.Show("Backlight53U_Change : niveau invalide (1 a 3).")
            Return
        End If
        If _modbusClient Is Nothing OrElse Not _modbusClient.Connected Then
            MessageBox.Show("Backlight53U_Change : non connecte.")
            Return
        End If
        Try
            DisableWritingProtection53U(passcode)
            _modbusClient.WriteSingleRegister(6626, level)
            EnableWritingProtection53U()
        Catch ex As Exception
            MessageBox.Show("Erreur Backlight53U_Change : " & ex.Message)
            EnableWritingProtection53U()
        End Try
    End Sub

End Module