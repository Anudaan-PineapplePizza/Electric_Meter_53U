<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ConnectionWindow
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Panel_Header = New System.Windows.Forms.Panel()
        Me.Label_FormTitle = New System.Windows.Forms.Label()
        Me.Label_ConnectionStatus_Title = New System.Windows.Forms.Label()
        Me.Label_ConnectionStatus = New System.Windows.Forms.Panel()
        Me.Panel_Body = New System.Windows.Forms.Panel()
        Me.GroupBox_ComParams = New System.Windows.Forms.GroupBox()
        Me.TableLayout_ComParams = New System.Windows.Forms.TableLayoutPanel()
        Me.Label_DeviceAddress_Title = New System.Windows.Forms.Label()
        Me.TextBox_DeviceAddress = New System.Windows.Forms.TextBox()
        Me.Label_Baudrate_Title = New System.Windows.Forms.Label()
        Me.ComboBox_Baudrate = New System.Windows.Forms.ComboBox()
        Me.Label_Parity_Title = New System.Windows.Forms.Label()
        Me.ComboBox_Parity = New System.Windows.Forms.ComboBox()
        Me.Label_StopBit_Title = New System.Windows.Forms.Label()
        Me.ComboBox_StopBit = New System.Windows.Forms.ComboBox()
        Me.Label_COMPort_Title = New System.Windows.Forms.Label()
        Me.ComboBox_COMPort = New System.Windows.Forms.ComboBox()
        Me.Button_Connect = New System.Windows.Forms.Button()
        Me.GroupBox_TcpEpack = New System.Windows.Forms.GroupBox()
        Me.TableLayout_Tcp = New System.Windows.Forms.TableLayoutPanel()
        Me.Label_IP_Title = New System.Windows.Forms.Label()
        Me.TextBox_EpackIP = New System.Windows.Forms.TextBox()
        Me.Label_Port_Title = New System.Windows.Forms.Label()
        Me.NumericUpDown_EpackPort = New System.Windows.Forms.NumericUpDown()
        Me.Button_ReadValues = New System.Windows.Forms.Button()
        Me.Button_ReadConfig = New System.Windows.Forms.Button()
        Me.Button_Connect_TCP = New System.Windows.Forms.Button()
        Me.GroupBox_Config = New System.Windows.Forms.GroupBox()
        Me.CheckBox_KeepConfig = New System.Windows.Forms.CheckBox()
        Me.ComboBox_Configuration = New System.Windows.Forms.ComboBox()
        Me.Button_Start = New System.Windows.Forms.Button()
        Me.Timer_Connection = New System.Windows.Forms.Timer(Me.components)
        Me.ComPort = New System.IO.Ports.SerialPort(Me.components)
        Me.Panel_Header.SuspendLayout()
        Me.Panel_Body.SuspendLayout()
        Me.GroupBox_ComParams.SuspendLayout()
        Me.TableLayout_ComParams.SuspendLayout()
        Me.GroupBox_TcpEpack.SuspendLayout()
        Me.TableLayout_Tcp.SuspendLayout()
        CType(Me.NumericUpDown_EpackPort, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_Config.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel_Header
        '
        Me.Panel_Header.BackColor = System.Drawing.Color.FromArgb(CType(CType(36, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.Panel_Header.Controls.Add(Me.Label_FormTitle)
        Me.Panel_Header.Controls.Add(Me.Label_ConnectionStatus_Title)
        Me.Panel_Header.Controls.Add(Me.Label_ConnectionStatus)
        Me.Panel_Header.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel_Header.Location = New System.Drawing.Point(0, 0)
        Me.Panel_Header.Name = "Panel_Header"
        Me.Panel_Header.Size = New System.Drawing.Size(444, 42)
        Me.Panel_Header.TabIndex = 0
        '
        'Label_FormTitle
        '
        Me.Label_FormTitle.AutoSize = True
        Me.Label_FormTitle.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Bold)
        Me.Label_FormTitle.ForeColor = System.Drawing.Color.White
        Me.Label_FormTitle.Location = New System.Drawing.Point(14, 11)
        Me.Label_FormTitle.Name = "Label_FormTitle"
        Me.Label_FormTitle.Size = New System.Drawing.Size(176, 20)
        Me.Label_FormTitle.TabIndex = 0
        Me.Label_FormTitle.Text = "MODBUS CONNECTION"
        '
        'Label_ConnectionStatus_Title
        '
        Me.Label_ConnectionStatus_Title.AutoSize = True
        Me.Label_ConnectionStatus_Title.Font = New System.Drawing.Font("Segoe UI", 7.5!)
        Me.Label_ConnectionStatus_Title.ForeColor = System.Drawing.Color.FromArgb(CType(CType(180, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(215, Byte), Integer))
        Me.Label_ConnectionStatus_Title.Location = New System.Drawing.Point(358, 16)
        Me.Label_ConnectionStatus_Title.Name = "Label_ConnectionStatus_Title"
        Me.Label_ConnectionStatus_Title.Size = New System.Drawing.Size(36, 12)
        Me.Label_ConnectionStatus_Title.TabIndex = 1
        Me.Label_ConnectionStatus_Title.Text = "STATUS"
        '
        'Label_ConnectionStatus
        '
        Me.Label_ConnectionStatus.BackColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(28, Byte), Integer), CType(CType(28, Byte), Integer))
        Me.Label_ConnectionStatus.Location = New System.Drawing.Point(408, 15)
        Me.Label_ConnectionStatus.Name = "Label_ConnectionStatus"
        Me.Label_ConnectionStatus.Size = New System.Drawing.Size(14, 14)
        Me.Label_ConnectionStatus.TabIndex = 2
        '
        'Panel_Body
        '
        Me.Panel_Body.BackColor = System.Drawing.Color.GhostWhite
        Me.Panel_Body.Controls.Add(Me.GroupBox_ComParams)
        Me.Panel_Body.Controls.Add(Me.GroupBox_TcpEpack)
        Me.Panel_Body.Controls.Add(Me.GroupBox_Config)
        Me.Panel_Body.Location = New System.Drawing.Point(0, 42)
        Me.Panel_Body.Name = "Panel_Body"
        Me.Panel_Body.Size = New System.Drawing.Size(444, 286)
        Me.Panel_Body.TabIndex = 1
        '
        'GroupBox_ComParams
        '
        Me.GroupBox_ComParams.BackColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.GroupBox_ComParams.Controls.Add(Me.TableLayout_ComParams)
        Me.GroupBox_ComParams.Controls.Add(Me.Button_Connect)
        Me.GroupBox_ComParams.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.GroupBox_ComParams.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold)
        Me.GroupBox_ComParams.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(122, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.GroupBox_ComParams.Location = New System.Drawing.Point(10, 8)
        Me.GroupBox_ComParams.Name = "GroupBox_ComParams"
        Me.GroupBox_ComParams.Size = New System.Drawing.Size(206, 180)
        Me.GroupBox_ComParams.TabIndex = 0
        Me.GroupBox_ComParams.TabStop = False
        Me.GroupBox_ComParams.Text = "  COM 53U"
        '
        'TableLayout_ComParams
        '
        Me.TableLayout_ComParams.BackColor = System.Drawing.Color.Transparent
        Me.TableLayout_ComParams.ColumnCount = 2
        Me.TableLayout_ComParams.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44.0!))
        Me.TableLayout_ComParams.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 56.0!))
        Me.TableLayout_ComParams.Controls.Add(Me.Label_DeviceAddress_Title, 0, 0)
        Me.TableLayout_ComParams.Controls.Add(Me.TextBox_DeviceAddress, 1, 0)
        Me.TableLayout_ComParams.Controls.Add(Me.Label_Baudrate_Title, 0, 1)
        Me.TableLayout_ComParams.Controls.Add(Me.ComboBox_Baudrate, 1, 1)
        Me.TableLayout_ComParams.Controls.Add(Me.Label_Parity_Title, 0, 2)
        Me.TableLayout_ComParams.Controls.Add(Me.ComboBox_Parity, 1, 2)
        Me.TableLayout_ComParams.Controls.Add(Me.Label_StopBit_Title, 0, 3)
        Me.TableLayout_ComParams.Controls.Add(Me.ComboBox_StopBit, 1, 3)
        Me.TableLayout_ComParams.Controls.Add(Me.Label_COMPort_Title, 0, 4)
        Me.TableLayout_ComParams.Controls.Add(Me.ComboBox_COMPort, 1, 4)
        Me.TableLayout_ComParams.Location = New System.Drawing.Point(4, 18)
        Me.TableLayout_ComParams.Name = "TableLayout_ComParams"
        Me.TableLayout_ComParams.RowCount = 5
        Me.TableLayout_ComParams.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
        Me.TableLayout_ComParams.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
        Me.TableLayout_ComParams.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
        Me.TableLayout_ComParams.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
        Me.TableLayout_ComParams.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
        Me.TableLayout_ComParams.Size = New System.Drawing.Size(196, 122)
        Me.TableLayout_ComParams.TabIndex = 0
        '
        'Label_DeviceAddress_Title
        '
        Me.Label_DeviceAddress_Title.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(238, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.Label_DeviceAddress_Title.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label_DeviceAddress_Title.Font = New System.Drawing.Font("Segoe UI", 8.5!)
        Me.Label_DeviceAddress_Title.ForeColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(116, Byte), Integer), CType(CType(140, Byte), Integer))
        Me.Label_DeviceAddress_Title.Location = New System.Drawing.Point(3, 0)
        Me.Label_DeviceAddress_Title.Name = "Label_DeviceAddress_Title"
        Me.Label_DeviceAddress_Title.Size = New System.Drawing.Size(80, 24)
        Me.Label_DeviceAddress_Title.TabIndex = 0
        Me.Label_DeviceAddress_Title.Text = " Device"
        Me.Label_DeviceAddress_Title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TextBox_DeviceAddress
        '
        Me.TextBox_DeviceAddress.BackColor = System.Drawing.Color.FromArgb(CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.TextBox_DeviceAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox_DeviceAddress.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox_DeviceAddress.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.TextBox_DeviceAddress.Location = New System.Drawing.Point(88, 3)
        Me.TextBox_DeviceAddress.Margin = New System.Windows.Forms.Padding(2, 3, 3, 3)
        Me.TextBox_DeviceAddress.MaxLength = 3
        Me.TextBox_DeviceAddress.Name = "TextBox_DeviceAddress"
        Me.TextBox_DeviceAddress.Size = New System.Drawing.Size(105, 23)
        Me.TextBox_DeviceAddress.TabIndex = 1
        Me.TextBox_DeviceAddress.Text = "1"
        Me.TextBox_DeviceAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label_Baudrate_Title
        '
        Me.Label_Baudrate_Title.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(238, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.Label_Baudrate_Title.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label_Baudrate_Title.Font = New System.Drawing.Font("Segoe UI", 8.5!)
        Me.Label_Baudrate_Title.ForeColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(116, Byte), Integer), CType(CType(140, Byte), Integer))
        Me.Label_Baudrate_Title.Location = New System.Drawing.Point(3, 24)
        Me.Label_Baudrate_Title.Name = "Label_Baudrate_Title"
        Me.Label_Baudrate_Title.Size = New System.Drawing.Size(80, 24)
        Me.Label_Baudrate_Title.TabIndex = 0
        Me.Label_Baudrate_Title.Text = " Baudrate"
        Me.Label_Baudrate_Title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox_Baudrate
        '
        Me.ComboBox_Baudrate.BackColor = System.Drawing.Color.FromArgb(CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ComboBox_Baudrate.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ComboBox_Baudrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Baudrate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ComboBox_Baudrate.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ComboBox_Baudrate.Location = New System.Drawing.Point(88, 27)
        Me.ComboBox_Baudrate.Margin = New System.Windows.Forms.Padding(2, 3, 3, 3)
        Me.ComboBox_Baudrate.Name = "ComboBox_Baudrate"
        Me.ComboBox_Baudrate.Size = New System.Drawing.Size(105, 23)
        Me.ComboBox_Baudrate.TabIndex = 2
        '
        'Label_Parity_Title
        '
        Me.Label_Parity_Title.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(238, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.Label_Parity_Title.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label_Parity_Title.Font = New System.Drawing.Font("Segoe UI", 8.5!)
        Me.Label_Parity_Title.ForeColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(116, Byte), Integer), CType(CType(140, Byte), Integer))
        Me.Label_Parity_Title.Location = New System.Drawing.Point(3, 48)
        Me.Label_Parity_Title.Name = "Label_Parity_Title"
        Me.Label_Parity_Title.Size = New System.Drawing.Size(80, 24)
        Me.Label_Parity_Title.TabIndex = 0
        Me.Label_Parity_Title.Text = " Parity"
        Me.Label_Parity_Title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox_Parity
        '
        Me.ComboBox_Parity.BackColor = System.Drawing.Color.FromArgb(CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ComboBox_Parity.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ComboBox_Parity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Parity.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ComboBox_Parity.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ComboBox_Parity.Location = New System.Drawing.Point(88, 51)
        Me.ComboBox_Parity.Margin = New System.Windows.Forms.Padding(2, 3, 3, 3)
        Me.ComboBox_Parity.Name = "ComboBox_Parity"
        Me.ComboBox_Parity.Size = New System.Drawing.Size(105, 23)
        Me.ComboBox_Parity.TabIndex = 3
        '
        'Label_StopBit_Title
        '
        Me.Label_StopBit_Title.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(238, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.Label_StopBit_Title.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label_StopBit_Title.Font = New System.Drawing.Font("Segoe UI", 8.5!)
        Me.Label_StopBit_Title.ForeColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(116, Byte), Integer), CType(CType(140, Byte), Integer))
        Me.Label_StopBit_Title.Location = New System.Drawing.Point(3, 72)
        Me.Label_StopBit_Title.Name = "Label_StopBit_Title"
        Me.Label_StopBit_Title.Size = New System.Drawing.Size(80, 24)
        Me.Label_StopBit_Title.TabIndex = 0
        Me.Label_StopBit_Title.Text = " Stop Bit"
        Me.Label_StopBit_Title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox_StopBit
        '
        Me.ComboBox_StopBit.BackColor = System.Drawing.Color.FromArgb(CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ComboBox_StopBit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ComboBox_StopBit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_StopBit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ComboBox_StopBit.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ComboBox_StopBit.Location = New System.Drawing.Point(88, 75)
        Me.ComboBox_StopBit.Margin = New System.Windows.Forms.Padding(2, 3, 3, 3)
        Me.ComboBox_StopBit.Name = "ComboBox_StopBit"
        Me.ComboBox_StopBit.Size = New System.Drawing.Size(105, 23)
        Me.ComboBox_StopBit.TabIndex = 4
        '
        'Label_COMPort_Title
        '
        Me.Label_COMPort_Title.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(238, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.Label_COMPort_Title.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label_COMPort_Title.Font = New System.Drawing.Font("Segoe UI", 8.5!)
        Me.Label_COMPort_Title.ForeColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(116, Byte), Integer), CType(CType(140, Byte), Integer))
        Me.Label_COMPort_Title.Location = New System.Drawing.Point(3, 96)
        Me.Label_COMPort_Title.Name = "Label_COMPort_Title"
        Me.Label_COMPort_Title.Size = New System.Drawing.Size(80, 26)
        Me.Label_COMPort_Title.TabIndex = 0
        Me.Label_COMPort_Title.Text = " COM Port"
        Me.Label_COMPort_Title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox_COMPort
        '
        Me.ComboBox_COMPort.BackColor = System.Drawing.Color.FromArgb(CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ComboBox_COMPort.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ComboBox_COMPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_COMPort.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ComboBox_COMPort.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ComboBox_COMPort.Location = New System.Drawing.Point(88, 99)
        Me.ComboBox_COMPort.Margin = New System.Windows.Forms.Padding(2, 3, 3, 3)
        Me.ComboBox_COMPort.Name = "ComboBox_COMPort"
        Me.ComboBox_COMPort.Size = New System.Drawing.Size(105, 23)
        Me.ComboBox_COMPort.TabIndex = 5
        '
        'Button_Connect
        '
        Me.Button_Connect.BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(44, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.Button_Connect.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button_Connect.FlatAppearance.BorderSize = 0
        Me.Button_Connect.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button_Connect.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold)
        Me.Button_Connect.ForeColor = System.Drawing.Color.White
        Me.Button_Connect.Location = New System.Drawing.Point(6, 146)
        Me.Button_Connect.Name = "Button_Connect"
        Me.Button_Connect.Size = New System.Drawing.Size(194, 28)
        Me.Button_Connect.TabIndex = 1
        Me.Button_Connect.Text = "Connect Modbus"
        Me.Button_Connect.UseVisualStyleBackColor = False
        '
        'GroupBox_TcpEpack
        '
        Me.GroupBox_TcpEpack.BackColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.GroupBox_TcpEpack.Controls.Add(Me.TableLayout_Tcp)
        Me.GroupBox_TcpEpack.Controls.Add(Me.Button_ReadValues)
        Me.GroupBox_TcpEpack.Controls.Add(Me.Button_ReadConfig)
        Me.GroupBox_TcpEpack.Controls.Add(Me.Button_Connect_TCP)
        Me.GroupBox_TcpEpack.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.GroupBox_TcpEpack.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold)
        Me.GroupBox_TcpEpack.ForeColor = System.Drawing.Color.LimeGreen
        Me.GroupBox_TcpEpack.Location = New System.Drawing.Point(224, 8)
        Me.GroupBox_TcpEpack.Name = "GroupBox_TcpEpack"
        Me.GroupBox_TcpEpack.Size = New System.Drawing.Size(210, 180)
        Me.GroupBox_TcpEpack.TabIndex = 1
        Me.GroupBox_TcpEpack.TabStop = False
        Me.GroupBox_TcpEpack.Text = "  TCP ePack"
        '
        'TableLayout_Tcp
        '
        Me.TableLayout_Tcp.BackColor = System.Drawing.Color.Transparent
        Me.TableLayout_Tcp.ColumnCount = 2
        Me.TableLayout_Tcp.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.0!))
        Me.TableLayout_Tcp.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 64.0!))
        Me.TableLayout_Tcp.Controls.Add(Me.Label_IP_Title, 0, 0)
        Me.TableLayout_Tcp.Controls.Add(Me.TextBox_EpackIP, 1, 0)
        Me.TableLayout_Tcp.Controls.Add(Me.Label_Port_Title, 0, 1)
        Me.TableLayout_Tcp.Controls.Add(Me.NumericUpDown_EpackPort, 1, 1)
        Me.TableLayout_Tcp.Location = New System.Drawing.Point(4, 18)
        Me.TableLayout_Tcp.Name = "TableLayout_Tcp"
        Me.TableLayout_Tcp.RowCount = 2
        Me.TableLayout_Tcp.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        Me.TableLayout_Tcp.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        Me.TableLayout_Tcp.Size = New System.Drawing.Size(200, 52)
        Me.TableLayout_Tcp.TabIndex = 0
        '
        'Label_IP_Title
        '
        Me.Label_IP_Title.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(238, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.Label_IP_Title.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label_IP_Title.Font = New System.Drawing.Font("Segoe UI", 8.5!)
        Me.Label_IP_Title.ForeColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(116, Byte), Integer), CType(CType(140, Byte), Integer))
        Me.Label_IP_Title.Location = New System.Drawing.Point(3, 0)
        Me.Label_IP_Title.Name = "Label_IP_Title"
        Me.Label_IP_Title.Size = New System.Drawing.Size(66, 26)
        Me.Label_IP_Title.TabIndex = 0
        Me.Label_IP_Title.Text = " IP"
        Me.Label_IP_Title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TextBox_EpackIP
        '
        Me.TextBox_EpackIP.BackColor = System.Drawing.Color.FromArgb(CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.TextBox_EpackIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox_EpackIP.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox_EpackIP.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.TextBox_EpackIP.Location = New System.Drawing.Point(74, 3)
        Me.TextBox_EpackIP.Margin = New System.Windows.Forms.Padding(2, 3, 3, 3)
        Me.TextBox_EpackIP.Name = "TextBox_EpackIP"
        Me.TextBox_EpackIP.Size = New System.Drawing.Size(123, 23)
        Me.TextBox_EpackIP.TabIndex = 1
        '
        'Label_Port_Title
        '
        Me.Label_Port_Title.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(238, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.Label_Port_Title.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label_Port_Title.Font = New System.Drawing.Font("Segoe UI", 8.5!)
        Me.Label_Port_Title.ForeColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(116, Byte), Integer), CType(CType(140, Byte), Integer))
        Me.Label_Port_Title.Location = New System.Drawing.Point(3, 26)
        Me.Label_Port_Title.Name = "Label_Port_Title"
        Me.Label_Port_Title.Size = New System.Drawing.Size(66, 26)
        Me.Label_Port_Title.TabIndex = 0
        Me.Label_Port_Title.Text = " Port"
        Me.Label_Port_Title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'NumericUpDown_EpackPort
        '
        Me.NumericUpDown_EpackPort.BackColor = System.Drawing.Color.FromArgb(CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.NumericUpDown_EpackPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.NumericUpDown_EpackPort.Dock = System.Windows.Forms.DockStyle.Fill
        Me.NumericUpDown_EpackPort.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.NumericUpDown_EpackPort.Location = New System.Drawing.Point(74, 29)
        Me.NumericUpDown_EpackPort.Margin = New System.Windows.Forms.Padding(2, 3, 3, 3)
        Me.NumericUpDown_EpackPort.Maximum = New Decimal(New Integer() {65535, 0, 0, 0})
        Me.NumericUpDown_EpackPort.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.NumericUpDown_EpackPort.Name = "NumericUpDown_EpackPort"
        Me.NumericUpDown_EpackPort.Size = New System.Drawing.Size(123, 23)
        Me.NumericUpDown_EpackPort.TabIndex = 2
        Me.NumericUpDown_EpackPort.Value = New Decimal(New Integer() {502, 0, 0, 0})
        '
        'Button_ReadValues
        '
        Me.Button_ReadValues.BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(44, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.Button_ReadValues.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button_ReadValues.FlatAppearance.BorderSize = 0
        Me.Button_ReadValues.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button_ReadValues.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold)
        Me.Button_ReadValues.ForeColor = System.Drawing.Color.FromArgb(CType(CType(140, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.Button_ReadValues.Location = New System.Drawing.Point(6, 114)
        Me.Button_ReadValues.Name = "Button_ReadValues"
        Me.Button_ReadValues.Size = New System.Drawing.Size(96, 24)
        Me.Button_ReadValues.TabIndex = 3
        Me.Button_ReadValues.Text = "Read Values"
        Me.Button_ReadValues.UseVisualStyleBackColor = False
        '
        'Button_ReadConfig
        '
        Me.Button_ReadConfig.BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(44, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.Button_ReadConfig.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button_ReadConfig.FlatAppearance.BorderSize = 0
        Me.Button_ReadConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button_ReadConfig.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold)
        Me.Button_ReadConfig.ForeColor = System.Drawing.Color.FromArgb(CType(CType(140, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.Button_ReadConfig.Location = New System.Drawing.Point(108, 114)
        Me.Button_ReadConfig.Name = "Button_ReadConfig"
        Me.Button_ReadConfig.Size = New System.Drawing.Size(96, 24)
        Me.Button_ReadConfig.TabIndex = 4
        Me.Button_ReadConfig.Text = "Read Config"
        Me.Button_ReadConfig.UseVisualStyleBackColor = False
        '
        'Button_Connect_TCP
        '
        Me.Button_Connect_TCP.BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(44, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.Button_Connect_TCP.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button_Connect_TCP.FlatAppearance.BorderSize = 0
        Me.Button_Connect_TCP.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button_Connect_TCP.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold)
        Me.Button_Connect_TCP.ForeColor = System.Drawing.Color.White
        Me.Button_Connect_TCP.Location = New System.Drawing.Point(6, 146)
        Me.Button_Connect_TCP.Name = "Button_Connect_TCP"
        Me.Button_Connect_TCP.Size = New System.Drawing.Size(198, 28)
        Me.Button_Connect_TCP.TabIndex = 5
        Me.Button_Connect_TCP.Text = "Connect TCP"
        Me.Button_Connect_TCP.UseVisualStyleBackColor = False
        '
        'GroupBox_Config
        '
        Me.GroupBox_Config.BackColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.GroupBox_Config.Controls.Add(Me.CheckBox_KeepConfig)
        Me.GroupBox_Config.Controls.Add(Me.ComboBox_Configuration)
        Me.GroupBox_Config.Controls.Add(Me.Button_Start)
        Me.GroupBox_Config.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.GroupBox_Config.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold)
        Me.GroupBox_Config.ForeColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.GroupBox_Config.Location = New System.Drawing.Point(10, 196)
        Me.GroupBox_Config.Name = "GroupBox_Config"
        Me.GroupBox_Config.Size = New System.Drawing.Size(424, 83)
        Me.GroupBox_Config.TabIndex = 2
        Me.GroupBox_Config.TabStop = False
        Me.GroupBox_Config.Text = "  CONFIGURATION"
        '
        'CheckBox_KeepConfig
        '
        Me.CheckBox_KeepConfig.AutoSize = True
        Me.CheckBox_KeepConfig.Checked = True
        Me.CheckBox_KeepConfig.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox_KeepConfig.Font = New System.Drawing.Font("Segoe UI", 8.5!)
        Me.CheckBox_KeepConfig.ForeColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.CheckBox_KeepConfig.Location = New System.Drawing.Point(14, 25)
        Me.CheckBox_KeepConfig.Name = "CheckBox_KeepConfig"
        Me.CheckBox_KeepConfig.Size = New System.Drawing.Size(175, 19)
        Me.CheckBox_KeepConfig.TabIndex = 0
        Me.CheckBox_KeepConfig.Text = "Keep previous configuration"
        Me.CheckBox_KeepConfig.UseVisualStyleBackColor = True
        '
        'ComboBox_Configuration
        '
        Me.ComboBox_Configuration.BackColor = System.Drawing.Color.FromArgb(CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ComboBox_Configuration.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Configuration.Enabled = False
        Me.ComboBox_Configuration.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ComboBox_Configuration.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ComboBox_Configuration.Location = New System.Drawing.Point(14, 50)
        Me.ComboBox_Configuration.Name = "ComboBox_Configuration"
        Me.ComboBox_Configuration.Size = New System.Drawing.Size(192, 23)
        Me.ComboBox_Configuration.TabIndex = 1
        '
        'Button_Start
        '
        Me.Button_Start.BackColor = System.Drawing.Color.FromArgb(CType(CType(150, Byte), Integer), CType(CType(160, Byte), Integer), CType(CType(170, Byte), Integer))
        Me.Button_Start.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button_Start.Enabled = False
        Me.Button_Start.FlatAppearance.BorderSize = 0
        Me.Button_Start.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button_Start.Font = New System.Drawing.Font("Segoe UI Semibold", 10.0!, System.Drawing.FontStyle.Bold)
        Me.Button_Start.ForeColor = System.Drawing.Color.White
        Me.Button_Start.Location = New System.Drawing.Point(220, 22)
        Me.Button_Start.Name = "Button_Start"
        Me.Button_Start.Size = New System.Drawing.Size(198, 51)
        Me.Button_Start.TabIndex = 2
        Me.Button_Start.Text = "Start"
        Me.Button_Start.UseVisualStyleBackColor = False
        '
        'Timer_Connection
        '
        Me.Timer_Connection.Interval = 500
        '
        'ConnectionWindow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(444, 328)
        Me.Controls.Add(Me.Panel_Header)
        Me.Controls.Add(Me.Panel_Body)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "ConnectionWindow"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "53U — Modbus Connection"
        Me.Panel_Header.ResumeLayout(False)
        Me.Panel_Header.PerformLayout()
        Me.Panel_Body.ResumeLayout(False)
        Me.GroupBox_ComParams.ResumeLayout(False)
        Me.TableLayout_ComParams.ResumeLayout(False)
        Me.TableLayout_ComParams.PerformLayout()
        Me.GroupBox_TcpEpack.ResumeLayout(False)
        Me.TableLayout_Tcp.ResumeLayout(False)
        Me.TableLayout_Tcp.PerformLayout()
        CType(Me.NumericUpDown_EpackPort, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_Config.ResumeLayout(False)
        Me.GroupBox_Config.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel_Header As System.Windows.Forms.Panel
    Friend WithEvents Label_FormTitle As System.Windows.Forms.Label
    Friend WithEvents Label_ConnectionStatus_Title As System.Windows.Forms.Label
    Friend WithEvents Label_ConnectionStatus As System.Windows.Forms.Panel
    Friend WithEvents Panel_Body As System.Windows.Forms.Panel
    Friend WithEvents GroupBox_ComParams As System.Windows.Forms.GroupBox
    Friend WithEvents TableLayout_ComParams As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label_DeviceAddress_Title As System.Windows.Forms.Label
    Friend WithEvents TextBox_DeviceAddress As System.Windows.Forms.TextBox
    Friend WithEvents Label_Baudrate_Title As System.Windows.Forms.Label
    Friend WithEvents ComboBox_Baudrate As System.Windows.Forms.ComboBox
    Friend WithEvents Label_Parity_Title As System.Windows.Forms.Label
    Friend WithEvents ComboBox_Parity As System.Windows.Forms.ComboBox
    Friend WithEvents Label_StopBit_Title As System.Windows.Forms.Label
    Friend WithEvents ComboBox_StopBit As System.Windows.Forms.ComboBox
    Friend WithEvents Label_COMPort_Title As System.Windows.Forms.Label
    Friend WithEvents ComboBox_COMPort As System.Windows.Forms.ComboBox
    Friend WithEvents Button_Connect As System.Windows.Forms.Button
    Friend WithEvents GroupBox_TcpEpack As System.Windows.Forms.GroupBox
    Friend WithEvents TableLayout_Tcp As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label_IP_Title As System.Windows.Forms.Label
    Friend WithEvents TextBox_EpackIP As System.Windows.Forms.TextBox
    Friend WithEvents Label_Port_Title As System.Windows.Forms.Label
    Friend WithEvents NumericUpDown_EpackPort As System.Windows.Forms.NumericUpDown
    Friend WithEvents Button_ReadValues As System.Windows.Forms.Button
    Friend WithEvents Button_ReadConfig As System.Windows.Forms.Button
    Friend WithEvents Button_Connect_TCP As System.Windows.Forms.Button
    Friend WithEvents GroupBox_Config As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBox_KeepConfig As System.Windows.Forms.CheckBox
    Friend WithEvents ComboBox_Configuration As System.Windows.Forms.ComboBox
    Friend WithEvents Button_Start As System.Windows.Forms.Button
    Friend WithEvents Timer_Connection As System.Windows.Forms.Timer
    Friend WithEvents ComPort As System.IO.Ports.SerialPort

End Class