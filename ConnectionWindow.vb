Option Strict On
Option Explicit On

Imports System.IO
Imports System.IO.Ports
Imports System.Windows.Forms

Public Class ConnectionWindow

    Private Class ConfigItem
        Public Property DisplayName As String
        Public Property FilePath As String
        Public Overrides Function ToString() As String
            Return DisplayName
        End Function
    End Class

    Private Const CONFIG_BROWSE As String = "Browse for file..."

    Private Sub ConnectionWindow_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        With ComboBox_Baudrate.Items
            .Clear()
            .Add("9600") : .Add("19200") : .Add("38400")
        End With
        ComboBox_Baudrate.SelectedIndex = 2
        ComboBox_Parity.Items.Clear()
        ComboBox_Parity.Items.Add(New ComboItem(Of IO.Ports.Parity) With {.Text = "None", .Value = IO.Ports.Parity.None})
        ComboBox_Parity.Items.Add(New ComboItem(Of IO.Ports.Parity) With {.Text = "Odd", .Value = IO.Ports.Parity.Odd})
        ComboBox_Parity.Items.Add(New ComboItem(Of IO.Ports.Parity) With {.Text = "Even", .Value = IO.Ports.Parity.Even})
        ComboBox_Parity.SelectedIndex = 1
        ComboBox_StopBit.Items.Clear()
        ComboBox_StopBit.Items.Add(New ComboItem(Of IO.Ports.StopBits) With {.Text = "1 bit", .Value = IO.Ports.StopBits.One})
        ComboBox_StopBit.Items.Add(New ComboItem(Of IO.Ports.StopBits) With {.Text = "2 bit", .Value = IO.Ports.StopBits.Two})
        ComboBox_StopBit.SelectedIndex = 0
        If ComPort.IsOpen Then ComPort.Close()
        ComPortMaster.RefreshComPorts()
        TextBox_EpackIP.Text = UserPreferences.Instance.EpackIP
        NumericUpDown_EpackPort.Value = Math.Min(65535, Math.Max(1, UserPreferences.Instance.EpackPort))
        RefreshConfigList()
        CheckBox_KeepConfig.Checked = True
        ComboBox_Configuration.Enabled = False
        Timer_Connection.Interval = 500
        Timer_Connection.Stop()
        DisconnectUI()
    End Sub

    Private Sub Button_Connect_Click(sender As Object, e As EventArgs) Handles Button_Connect.Click
        If ModbusRsMaster.IsConnected() Then
            DisconnectUI()
            ModbusRsMaster.Disconnect()
            UpdateStartButton()
            Return
        End If
        Dim selected As ComPortInfo = TryCast(ComboBox_COMPort.SelectedItem, ComPortInfo)
        Dim COMport As String = If(selected IsNot Nothing, selected.Port, Nothing)
        If String.IsNullOrWhiteSpace(COMport) Then
            MessageBox.Show("No COM port selected.") : Return
        End If
        Dim baud As Integer
        If Not Integer.TryParse(ComboBox_Baudrate.Text, baud) Then
            MessageBox.Show("Invalid baudrate.") : Return
        End If
        Dim deviceAddressInt As Integer
        If String.IsNullOrWhiteSpace(TextBox_DeviceAddress.Text) OrElse
           Not Integer.TryParse(TextBox_DeviceAddress.Text, deviceAddressInt) OrElse
           deviceAddressInt < 1 OrElse deviceAddressInt > 247 Then
            MessageBox.Show("Invalid Modbus address (1-247).") : Return
        End If
        Dim deviceAddress As Byte = CByte(deviceAddressInt)
        Dim parityEnum As IO.Ports.Parity =
            CType(ComboBox_Parity.SelectedItem, ComboItem(Of IO.Ports.Parity)).Value
        Dim stopBitsEnum As IO.Ports.StopBits =
            CType(ComboBox_StopBit.SelectedItem, ComboItem(Of IO.Ports.StopBits)).Value
        ModbusRsMaster.Config.Baudrate = baud
        ModbusRsMaster.Config.Parity = parityEnum
        ModbusRsMaster.Config.StopBit = stopBitsEnum
        ModbusRsMaster.Config.DeviceAddress = deviceAddress
        ModbusRsMaster.Config.ConnectString =
            $"{COMport},{baud},{parityEnum},{stopBitsEnum},ID={deviceAddress}"
        If ModbusRsMaster.Connect(COMport, baud, parityEnum, stopBitsEnum, deviceAddress) Then
            ConnectUI()
        Else
            MessageBox.Show("Cannot open COM port.")
        End If
        UpdateStartButton()
    End Sub

    Public Sub ConnectUI()
        Timer_Connection.Start()
        ModbusRsMaster.Connected = True
        Label_ConnectionStatus.BackColor = Drawing.Color.FromArgb(0, 200, 100)
        Button_Connect.Text = "Disconnect Modbus"
        Button_Connect.BackColor = Drawing.Color.FromArgb(100, 28, 28)
        MeterWindow.Label_StatusText.Text = "ONLINE"
        TextBox_DeviceAddress.Enabled = False
        ComboBox_Baudrate.Enabled = False
        ComboBox_Parity.Enabled = False
        ComboBox_StopBit.Enabled = False
        ComboBox_COMPort.Enabled = False
        UpdateStartButton()
    End Sub

    Public Sub DisconnectUI()
        Timer_Connection.Stop()
        ModbusRsMaster.Connected = False
        Label_ConnectionStatus.BackColor = Drawing.Color.FromArgb(100, 28, 28)
        MeterWindow.Label_ConnectionStatus2.BackColor = Drawing.Color.FromArgb(100, 28, 28)
        Button_Connect.Text = "Connect Modbus"
        Button_Connect.BackColor = Drawing.Color.FromArgb(28, 44, 70)
        MeterWindow.Label_StatusText.Text = "OFFLINE"
        TextBox_DeviceAddress.Enabled = True
        ComboBox_Baudrate.Enabled = True
        ComboBox_Parity.Enabled = True
        ComboBox_StopBit.Enabled = True
        ComboBox_COMPort.Enabled = True
        UpdateStartButton()
    End Sub

    ' Start actif uniquement si TCP ou RS connecte
    Private Sub UpdateStartButton()
        Dim active As Boolean = ModbusRsMaster.IsConnected() OrElse
                                ModbusTcpMaster.modbustcpisconnected
        Button_Start.Enabled = active
        If active Then
            Button_Start.BackColor = Drawing.Color.FromArgb(30, 180, 100)
            Button_Start.Text = "Start"
        Else
            Button_Start.BackColor = Drawing.Color.FromArgb(130, 140, 150)
            Button_Start.Text = "Start"
        End If
    End Sub

    Private Sub Button_Connect_TCP_Click(sender As Object, e As EventArgs) Handles Button_Connect_TCP.Click
        Dim p As UserPreferences = UserPreferences.Instance
        p.EpackIP = TextBox_EpackIP.Text.Trim()
        p.EpackPort = CInt(NumericUpDown_EpackPort.Value)
        If ModbusTcpMaster.modbustcpisconnected Then
            ModbusTcpMaster.CloseTCP()
            RegisterMap.Refresh()
            p.EpackEnabled = False
            p.Save()
            Button_Connect_TCP.BackColor = Drawing.Color.FromArgb(28, 44, 70)
            Button_Connect_TCP.ForeColor = Drawing.Color.White
            Button_Connect_TCP.Text = "Connect TCP"
        Else
            ModbusTcpMaster.OpenTCP(p.EpackIP, p.EpackPort)
            If ModbusTcpMaster.modbustcpisconnected Then
                RegisterMap.Refresh()
                p.EpackEnabled = True
                For Each r As RegisterDef In RegisterMap.GetRealTimeSignals()
                    If r.Group = SignalGroup.ePack Then
                        p.SetPanelVisible(r.ID, True)
                        p.SetCsvEnabled(r.ID, True)
                        p.SetEnabled(r.ID, True)
                    End If
                Next
                p.Save()
                Button_Connect_TCP.BackColor = Drawing.Color.FromArgb(100, 28, 28)
                Button_Connect_TCP.ForeColor = Drawing.Color.FromArgb(255, 200, 200)
                Button_Connect_TCP.Text = "Disconnect TCP"
                If MeterWindow.Visible Then MeterWindow.OnPreferencesApplied()
            Else
                Button_Connect_TCP.BackColor = Drawing.Color.FromArgb(28, 44, 70)
                Button_Connect_TCP.ForeColor = Drawing.Color.White
                Button_Connect_TCP.Text = "Connect TCP"
                MessageBox.Show("TCP connection failed. Check IP and port.",
                    "TCP Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        End If
        UpdateStartButton()
    End Sub

    Private Sub Button_ReadValues_Click(sender As Object, e As EventArgs) Handles Button_ReadValues.Click
        If Not ModbusTcpMaster.modbustcpisconnected Then
            MessageBox.Show("TCP not connected.") : Return
        End If
        Read_epackTCP(True)
    End Sub

    Private Sub Button_ReadConfig_Click(sender As Object, e As EventArgs) Handles Button_ReadConfig.Click
        If Not ModbusTcpMaster.modbustcpisconnected Then
            MessageBox.Show("TCP not connected.") : Return
        End If
        ModbusTcpMaster.Read_Config_epackTCP(True)
    End Sub

    Private Sub Timer_Connection_Tick(sender As Object, e As EventArgs) Handles Timer_Connection.Tick
        If Not ModbusRsMaster.IsConnected() Then
            DisconnectUI()
            Return
        End If
        Select Case ModbusRsMaster._blinkState
            Case 0
                Label_ConnectionStatus.BackColor = Drawing.Color.FromArgb(0, 200, 100)
                MeterWindow.Label_ConnectionStatus2.BackColor = Drawing.Color.FromArgb(0, 200, 100)
                ModbusRsMaster._blinkState = 1
            Case 1
                Label_ConnectionStatus.BackColor = Drawing.Color.FromArgb(40, 80, 40)
                MeterWindow.Label_ConnectionStatus2.BackColor = Drawing.Color.FromArgb(40, 80, 40)
                ModbusRsMaster._blinkState = 0
        End Select
        If ModbusTcpMaster.modbustcpisconnected Then
            Button_Connect_TCP.BackColor = Drawing.Color.FromArgb(100, 28, 28)
            Button_Connect_TCP.ForeColor = Drawing.Color.FromArgb(255, 200, 200)
            Button_Connect_TCP.Text = "Disconnect TCP"
        Else
            Button_Connect_TCP.BackColor = Drawing.Color.FromArgb(28, 44, 70)
            Button_Connect_TCP.ForeColor = Drawing.Color.White
            Button_Connect_TCP.Text = "Connect TCP"
        End If
    End Sub

    Private Sub ComboBox_COMPort_MouseClick(sender As Object, e As MouseEventArgs) _
            Handles ComboBox_COMPort.MouseClick
        RefreshComPorts()
    End Sub

    Private Sub TextBox_DeviceAddress_KeyPress(sender As Object, e As KeyPressEventArgs) _
            Handles TextBox_DeviceAddress.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub TextBox_DeviceAddress_TextChanged(sender As Object, e As EventArgs) _
            Handles TextBox_DeviceAddress.TextChanged

        If (TextBox_DeviceAddress.Text.Trim() = "67" Or TextBox_DeviceAddress.Text.Trim() = "99") Then
            Button_Start.Enabled = True
            Button_Start.BackColor = Drawing.Color.FromArgb(130, 85, 0)
            Button_Start.Text = "Start (DEMO)"
        Else
            UpdateStartButton()
        End If
    End Sub

    Private Sub Button_Start_Click(sender As Object, e As EventArgs) Handles Button_Start.Click
        If (TextBox_DeviceAddress.Text.Trim() = "67" Or TextBox_DeviceAddress.Text.Trim() = "99") Then
            MeterWindow.IsDemoMode = True
            ApplySelectedConfig()
            Me.Hide()
            MeterWindow.Show()
            Return
        End If
        MeterWindow.IsDemoMode = False
        If Not ModbusRsMaster.IsConnected() AndAlso Not ModbusTcpMaster.modbustcpisconnected Then
            MessageBox.Show("Not connected.") : Return
        End If
        ApplySelectedConfig()
        Me.Hide()
        MeterWindow.Show()
    End Sub

    Private Sub CheckBox_KeepConfig_CheckedChanged(sender As Object, e As EventArgs) _
            Handles CheckBox_KeepConfig.CheckedChanged
        ComboBox_Configuration.Enabled = Not CheckBox_KeepConfig.Checked
        If Not CheckBox_KeepConfig.Checked Then RefreshConfigList()
    End Sub

    Private Sub ComboBox_Configuration_SelectedIndexChanged(sender As Object, e As EventArgs) _
            Handles ComboBox_Configuration.SelectedIndexChanged
        Dim item As ConfigItem = TryCast(ComboBox_Configuration.SelectedItem, ConfigItem)
        If item Is Nothing OrElse item.DisplayName <> CONFIG_BROWSE Then Return
        Dim dlg As New OpenFileDialog()
        dlg.Title = "Select a JSON configuration file"
        dlg.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*"
        dlg.InitialDirectory = AppFolder("config")
        If dlg.ShowDialog() = DialogResult.OK Then
            Dim newItem As New ConfigItem With {
                .DisplayName = Path.GetFileNameWithoutExtension(dlg.FileName),
                .FilePath = dlg.FileName
            }
            ComboBox_Configuration.Items.Insert(ComboBox_Configuration.Items.Count - 1, newItem)
            ComboBox_Configuration.SelectedItem = newItem
        ElseIf ComboBox_Configuration.Items.Count > 1 Then
            ComboBox_Configuration.SelectedIndex = 0
        End If
    End Sub

    Private Sub RefreshConfigList()
        ComboBox_Configuration.Items.Clear()
        Dim configDir As String = AppFolder("config")
        If Directory.Exists(configDir) Then
            For Each f As String In Directory.GetFiles(configDir, "*.json")
                ComboBox_Configuration.Items.Add(New ConfigItem With {
                    .DisplayName = Path.GetFileNameWithoutExtension(f),
                    .FilePath = f
                })
            Next
        End If
        ComboBox_Configuration.Items.Add(New ConfigItem With {
            .DisplayName = CONFIG_BROWSE, .FilePath = ""})
        If ComboBox_Configuration.Items.Count > 1 Then ComboBox_Configuration.SelectedIndex = 0
    End Sub

    Private Sub ApplySelectedConfig()
        If CheckBox_KeepConfig.Checked Then Return
        Dim item As ConfigItem = TryCast(ComboBox_Configuration.SelectedItem, ConfigItem)
        If item Is Nothing OrElse item.DisplayName = CONFIG_BROWSE Then Return
        If Not File.Exists(item.FilePath) Then
            MessageBox.Show("Config file not found:" & Environment.NewLine & item.FilePath,
                "Config error", MessageBoxButtons.OK, MessageBoxIcon.Warning) : Return
        End If
        Try
            UserPreferences.Instance.LoadFromFile(item.FilePath)
        Catch ex As Exception
            MessageBox.Show("Error loading config:" & Environment.NewLine & ex.Message,
                "Config error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Shared Function AppFolder(subFolder As String) As String
        Dim dir As String = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Electric_Meter_53U", subFolder)
        If Not Directory.Exists(dir) Then Directory.CreateDirectory(dir)
        Return dir
    End Function

End Class