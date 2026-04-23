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
        Me.Button_Connect = New System.Windows.Forms.Button()
        Me.Label_DeviceAddress_Title = New System.Windows.Forms.Label()
        Me.Panel_Communication = New System.Windows.Forms.Panel()
        Me.Panel_Header = New System.Windows.Forms.Panel()
        Me.Label_FormTitle = New System.Windows.Forms.Label()
        Me.Label_ConnectionStatus_Title = New System.Windows.Forms.Label()
        Me.Label_ConnectionStatus = New System.Windows.Forms.Panel()
        Me.Panel_Body = New System.Windows.Forms.Panel()
        Me.GroupBox_ComParams = New System.Windows.Forms.GroupBox()
        Me.TableLayout_ComParams = New System.Windows.Forms.TableLayoutPanel()
        Me.TextBox_DeviceAddress = New System.Windows.Forms.TextBox()
        Me.Label_Baudrate_Title = New System.Windows.Forms.Label()
        Me.ComboBox_Baudrate = New System.Windows.Forms.ComboBox()
        Me.Label_Parity_Title = New System.Windows.Forms.Label()
        Me.ComboBox_Parity = New System.Windows.Forms.ComboBox()
        Me.Label_StopBit_Title = New System.Windows.Forms.Label()
        Me.ComboBox_StopBit = New System.Windows.Forms.ComboBox()
        Me.Label_COMPort_Title = New System.Windows.Forms.Label()
        Me.ComboBox_COMPort = New System.Windows.Forms.ComboBox()
        Me.Panel_Divider = New System.Windows.Forms.Panel()
        Me.GroupBox_Config = New System.Windows.Forms.GroupBox()
        Me.Button_Connect_TCP = New System.Windows.Forms.Button()
        Me.Button_ReadTCP = New System.Windows.Forms.Button()
        Me.CheckBox_KeepConfig = New System.Windows.Forms.CheckBox()
        Me.ComboBox_Configuration = New System.Windows.Forms.ComboBox()
        Me.Button_Start = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Splitter1 = New System.Windows.Forms.Splitter()
        Me.Splitter2 = New System.Windows.Forms.Splitter()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel_Com_Parameters = New System.Windows.Forms.Panel()
        Me.Panel_Baudrate = New System.Windows.Forms.Panel()
        Me.Panel_DeviceAddress = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.Timer_Connection = New System.Windows.Forms.Timer(Me.components)
        Me.ComPort = New System.IO.Ports.SerialPort(Me.components)
        Me.SerialPort1 = New System.IO.Ports.SerialPort(Me.components)
        Me.Label_Epack_ = New System.Windows.Forms.Label()
        Me.Panel_Communication.SuspendLayout()
        Me.Panel_Header.SuspendLayout()
        Me.Panel_Body.SuspendLayout()
        Me.GroupBox_ComParams.SuspendLayout()
        Me.TableLayout_ComParams.SuspendLayout()
        Me.GroupBox_Config.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button_Connect
        '
        Me.Button_Connect.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(122, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.Button_Connect.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button_Connect.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(175, Byte), Integer))
        Me.Button_Connect.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(175, Byte), Integer))
        Me.Button_Connect.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button_Connect.Font = New System.Drawing.Font("Segoe UI Semibold", 9.5!, System.Drawing.FontStyle.Bold)
        Me.Button_Connect.ForeColor = System.Drawing.Color.White
        Me.Button_Connect.Location = New System.Drawing.Point(12, 170)
        Me.Button_Connect.Name = "Button_Connect"
        Me.Button_Connect.Padding = New System.Windows.Forms.Padding(3)
        Me.Button_Connect.Size = New System.Drawing.Size(187, 36)
        Me.Button_Connect.TabIndex = 1
        Me.Button_Connect.Text = "Connect Modbus"
        Me.Button_Connect.UseVisualStyleBackColor = False
        '
        'Label_DeviceAddress_Title
        '
        Me.Label_DeviceAddress_Title.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(238, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.Label_DeviceAddress_Title.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label_DeviceAddress_Title.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Label_DeviceAddress_Title.ForeColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(116, Byte), Integer), CType(CType(140, Byte), Integer))
        Me.Label_DeviceAddress_Title.Location = New System.Drawing.Point(3, 0)
        Me.Label_DeviceAddress_Title.Name = "Label_DeviceAddress_Title"
        Me.Label_DeviceAddress_Title.Size = New System.Drawing.Size(74, 24)
        Me.Label_DeviceAddress_Title.TabIndex = 0
        Me.Label_DeviceAddress_Title.Text = " Device"
        Me.Label_DeviceAddress_Title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel_Communication
        '
        Me.Panel_Communication.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.Panel_Communication.Controls.Add(Me.Panel_Header)
        Me.Panel_Communication.Controls.Add(Me.Panel_Body)
        Me.Panel_Communication.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel_Communication.Location = New System.Drawing.Point(0, 0)
        Me.Panel_Communication.Name = "Panel_Communication"
        Me.Panel_Communication.Size = New System.Drawing.Size(411, 260)
        Me.Panel_Communication.TabIndex = 0
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
        Me.Panel_Header.Size = New System.Drawing.Size(411, 42)
        Me.Panel_Header.TabIndex = 10
        '
        'Label_FormTitle
        '
        Me.Label_FormTitle.AutoSize = True
        Me.Label_FormTitle.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Bold)
        Me.Label_FormTitle.ForeColor = System.Drawing.Color.White
        Me.Label_FormTitle.Location = New System.Drawing.Point(14, 11)
        Me.Label_FormTitle.Name = "Label_FormTitle"
        Me.Label_FormTitle.Size = New System.Drawing.Size(230, 20)
        Me.Label_FormTitle.TabIndex = 0
        Me.Label_FormTitle.Text = "MODBUS SERIAL CONNECTION"
        '
        'Label_ConnectionStatus_Title
        '
        Me.Label_ConnectionStatus_Title.AutoSize = True
        Me.Label_ConnectionStatus_Title.Font = New System.Drawing.Font("Segoe UI", 8.0!)
        Me.Label_ConnectionStatus_Title.ForeColor = System.Drawing.Color.FromArgb(CType(CType(180, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(215, Byte), Integer))
        Me.Label_ConnectionStatus_Title.Location = New System.Drawing.Point(335, 16)
        Me.Label_ConnectionStatus_Title.Name = "Label_ConnectionStatus_Title"
        Me.Label_ConnectionStatus_Title.Size = New System.Drawing.Size(42, 13)
        Me.Label_ConnectionStatus_Title.TabIndex = 1
        Me.Label_ConnectionStatus_Title.Text = "STATUS"
        '
        'Label_ConnectionStatus
        '
        Me.Label_ConnectionStatus.BackColor = System.Drawing.Color.FromArgb(CType(CType(80, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.Label_ConnectionStatus.Location = New System.Drawing.Point(385, 16)
        Me.Label_ConnectionStatus.Name = "Label_ConnectionStatus"
        Me.Label_ConnectionStatus.Size = New System.Drawing.Size(12, 12)
        Me.Label_ConnectionStatus.TabIndex = 2
        '
        'Panel_Body
        '
        Me.Panel_Body.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.Panel_Body.Controls.Add(Me.GroupBox_ComParams)
        Me.Panel_Body.Controls.Add(Me.Button_Connect)
        Me.Panel_Body.Controls.Add(Me.Panel_Divider)
        Me.Panel_Body.Controls.Add(Me.GroupBox_Config)
        Me.Panel_Body.Controls.Add(Me.Button_Start)
        Me.Panel_Body.Controls.Add(Me.Label2)
        Me.Panel_Body.Controls.Add(Me.Splitter1)
        Me.Panel_Body.Controls.Add(Me.Splitter2)
        Me.Panel_Body.Controls.Add(Me.SplitContainer1)
        Me.Panel_Body.Controls.Add(Me.Panel1)
        Me.Panel_Body.Controls.Add(Me.Panel_Com_Parameters)
        Me.Panel_Body.Controls.Add(Me.Panel_Baudrate)
        Me.Panel_Body.Controls.Add(Me.Panel_DeviceAddress)
        Me.Panel_Body.Controls.Add(Me.Panel2)
        Me.Panel_Body.Controls.Add(Me.Panel3)
        Me.Panel_Body.Controls.Add(Me.Panel4)
        Me.Panel_Body.Location = New System.Drawing.Point(0, 42)
        Me.Panel_Body.Name = "Panel_Body"
        Me.Panel_Body.Size = New System.Drawing.Size(411, 218)
        Me.Panel_Body.TabIndex = 11
        '
        'GroupBox_ComParams
        '
        Me.GroupBox_ComParams.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(250, Byte), Integer), CType(CType(253, Byte), Integer))
        Me.GroupBox_ComParams.Controls.Add(Me.TableLayout_ComParams)
        Me.GroupBox_ComParams.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.GroupBox_ComParams.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold)
        Me.GroupBox_ComParams.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(122, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.GroupBox_ComParams.Location = New System.Drawing.Point(12, 10)
        Me.GroupBox_ComParams.Name = "GroupBox_ComParams"
        Me.GroupBox_ComParams.Size = New System.Drawing.Size(187, 154)
        Me.GroupBox_ComParams.TabIndex = 0
        Me.GroupBox_ComParams.TabStop = False
        Me.GroupBox_ComParams.Text = "  COM PARAMETERS"
        '
        'TableLayout_ComParams
        '
        Me.TableLayout_ComParams.BackColor = System.Drawing.Color.Transparent
        Me.TableLayout_ComParams.ColumnCount = 2
        Me.TableLayout_ComParams.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.45454!))
        Me.TableLayout_ComParams.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.54546!))
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
        Me.TableLayout_ComParams.Location = New System.Drawing.Point(5, 19)
        Me.TableLayout_ComParams.Name = "TableLayout_ComParams"
        Me.TableLayout_ComParams.RowCount = 5
        Me.TableLayout_ComParams.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
        Me.TableLayout_ComParams.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
        Me.TableLayout_ComParams.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
        Me.TableLayout_ComParams.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
        Me.TableLayout_ComParams.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
        Me.TableLayout_ComParams.Size = New System.Drawing.Size(177, 129)
        Me.TableLayout_ComParams.TabIndex = 0
        '
        'TextBox_DeviceAddress
        '
        Me.TextBox_DeviceAddress.BackColor = System.Drawing.Color.FromArgb(CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.TextBox_DeviceAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox_DeviceAddress.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox_DeviceAddress.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.TextBox_DeviceAddress.ForeColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(36, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.TextBox_DeviceAddress.Location = New System.Drawing.Point(82, 3)
        Me.TextBox_DeviceAddress.Margin = New System.Windows.Forms.Padding(2, 3, 3, 3)
        Me.TextBox_DeviceAddress.MaxLength = 3
        Me.TextBox_DeviceAddress.Name = "TextBox_DeviceAddress"
        Me.TextBox_DeviceAddress.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox_DeviceAddress.Size = New System.Drawing.Size(92, 23)
        Me.TextBox_DeviceAddress.TabIndex = 1
        Me.TextBox_DeviceAddress.Text = "1"
        Me.TextBox_DeviceAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label_Baudrate_Title
        '
        Me.Label_Baudrate_Title.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(238, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.Label_Baudrate_Title.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label_Baudrate_Title.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Label_Baudrate_Title.ForeColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(116, Byte), Integer), CType(CType(140, Byte), Integer))
        Me.Label_Baudrate_Title.Location = New System.Drawing.Point(3, 24)
        Me.Label_Baudrate_Title.Name = "Label_Baudrate_Title"
        Me.Label_Baudrate_Title.Size = New System.Drawing.Size(74, 24)
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
        Me.ComboBox_Baudrate.ForeColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(36, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.ComboBox_Baudrate.FormattingEnabled = True
        Me.ComboBox_Baudrate.Location = New System.Drawing.Point(82, 27)
        Me.ComboBox_Baudrate.Margin = New System.Windows.Forms.Padding(2, 3, 3, 3)
        Me.ComboBox_Baudrate.Name = "ComboBox_Baudrate"
        Me.ComboBox_Baudrate.Size = New System.Drawing.Size(92, 23)
        Me.ComboBox_Baudrate.TabIndex = 2
        '
        'Label_Parity_Title
        '
        Me.Label_Parity_Title.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(238, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.Label_Parity_Title.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label_Parity_Title.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Label_Parity_Title.ForeColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(116, Byte), Integer), CType(CType(140, Byte), Integer))
        Me.Label_Parity_Title.Location = New System.Drawing.Point(3, 48)
        Me.Label_Parity_Title.Name = "Label_Parity_Title"
        Me.Label_Parity_Title.Size = New System.Drawing.Size(74, 24)
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
        Me.ComboBox_Parity.ForeColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(36, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.ComboBox_Parity.FormattingEnabled = True
        Me.ComboBox_Parity.Location = New System.Drawing.Point(82, 51)
        Me.ComboBox_Parity.Margin = New System.Windows.Forms.Padding(2, 3, 3, 3)
        Me.ComboBox_Parity.Name = "ComboBox_Parity"
        Me.ComboBox_Parity.Size = New System.Drawing.Size(92, 23)
        Me.ComboBox_Parity.TabIndex = 3
        '
        'Label_StopBit_Title
        '
        Me.Label_StopBit_Title.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(238, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.Label_StopBit_Title.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label_StopBit_Title.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Label_StopBit_Title.ForeColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(116, Byte), Integer), CType(CType(140, Byte), Integer))
        Me.Label_StopBit_Title.Location = New System.Drawing.Point(3, 72)
        Me.Label_StopBit_Title.Name = "Label_StopBit_Title"
        Me.Label_StopBit_Title.Size = New System.Drawing.Size(74, 24)
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
        Me.ComboBox_StopBit.ForeColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(36, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.ComboBox_StopBit.FormattingEnabled = True
        Me.ComboBox_StopBit.Location = New System.Drawing.Point(82, 75)
        Me.ComboBox_StopBit.Margin = New System.Windows.Forms.Padding(2, 3, 3, 3)
        Me.ComboBox_StopBit.Name = "ComboBox_StopBit"
        Me.ComboBox_StopBit.Size = New System.Drawing.Size(92, 23)
        Me.ComboBox_StopBit.TabIndex = 4
        '
        'Label_COMPort_Title
        '
        Me.Label_COMPort_Title.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(238, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.Label_COMPort_Title.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label_COMPort_Title.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Label_COMPort_Title.ForeColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(116, Byte), Integer), CType(CType(140, Byte), Integer))
        Me.Label_COMPort_Title.Location = New System.Drawing.Point(3, 96)
        Me.Label_COMPort_Title.Name = "Label_COMPort_Title"
        Me.Label_COMPort_Title.Size = New System.Drawing.Size(74, 33)
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
        Me.ComboBox_COMPort.ForeColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(36, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.ComboBox_COMPort.FormattingEnabled = True
        Me.ComboBox_COMPort.Location = New System.Drawing.Point(82, 99)
        Me.ComboBox_COMPort.Margin = New System.Windows.Forms.Padding(2, 3, 3, 3)
        Me.ComboBox_COMPort.Name = "ComboBox_COMPort"
        Me.ComboBox_COMPort.Size = New System.Drawing.Size(92, 23)
        Me.ComboBox_COMPort.TabIndex = 5
        '
        'Panel_Divider
        '
        Me.Panel_Divider.BackColor = System.Drawing.Color.FromArgb(CType(CType(210, Byte), Integer), CType(CType(218, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.Panel_Divider.Location = New System.Drawing.Point(207, 10)
        Me.Panel_Divider.Name = "Panel_Divider"
        Me.Panel_Divider.Size = New System.Drawing.Size(1, 189)
        Me.Panel_Divider.TabIndex = 12
        '
        'GroupBox_Config
        '
        Me.GroupBox_Config.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(250, Byte), Integer), CType(CType(253, Byte), Integer))
        Me.GroupBox_Config.Controls.Add(Me.Label_Epack_)
        Me.GroupBox_Config.Controls.Add(Me.Button_Connect_TCP)
        Me.GroupBox_Config.Controls.Add(Me.Button_ReadTCP)
        Me.GroupBox_Config.Controls.Add(Me.CheckBox_KeepConfig)
        Me.GroupBox_Config.Controls.Add(Me.ComboBox_Configuration)
        Me.GroupBox_Config.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.GroupBox_Config.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold)
        Me.GroupBox_Config.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(122, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.GroupBox_Config.Location = New System.Drawing.Point(216, 10)
        Me.GroupBox_Config.Name = "GroupBox_Config"
        Me.GroupBox_Config.Size = New System.Drawing.Size(183, 154)
        Me.GroupBox_Config.TabIndex = 13
        Me.GroupBox_Config.TabStop = False
        Me.GroupBox_Config.Text = "  CONFIGURATION"
        '
        'Button_Connect_TCP
        '
        Me.Button_Connect_TCP.BackColor = System.Drawing.Color.FromArgb(28, 44, 70)
        Me.Button_Connect_TCP.FlatAppearance.BorderSize = 0
        Me.Button_Connect_TCP.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button_Connect_TCP.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold)
        Me.Button_Connect_TCP.ForeColor = System.Drawing.Color.FromArgb(180, 200, 230)
        Me.Button_Connect_TCP.Location = New System.Drawing.Point(7, 120)
        Me.Button_Connect_TCP.Name = "Button_Connect_TCP"
        Me.Button_Connect_TCP.Size = New System.Drawing.Size(87, 26)
        Me.Button_Connect_TCP.TabIndex = 100
        Me.Button_Connect_TCP.Text = "Conn. TCP"
        Me.Button_Connect_TCP.UseVisualStyleBackColor = False
        '
        'Button_ReadTCP
        '
        Me.Button_ReadTCP.BackColor = System.Drawing.Color.FromArgb(28, 44, 70)
        Me.Button_ReadTCP.FlatAppearance.BorderSize = 0
        Me.Button_ReadTCP.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button_ReadTCP.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold)
        Me.Button_ReadTCP.ForeColor = System.Drawing.Color.FromArgb(140, 165, 200)
        Me.Button_ReadTCP.Location = New System.Drawing.Point(102, 120)
        Me.Button_ReadTCP.Name = "Button_ReadTCP"
        Me.Button_ReadTCP.Size = New System.Drawing.Size(80, 26)
        Me.Button_ReadTCP.TabIndex = 101
        Me.Button_ReadTCP.Text = "Read TCP"
        Me.Button_ReadTCP.UseVisualStyleBackColor = False
        '
        'CheckBox_KeepConfig
        '
        Me.CheckBox_KeepConfig.AutoSize = True
        Me.CheckBox_KeepConfig.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.CheckBox_KeepConfig.ForeColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(36, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.CheckBox_KeepConfig.Location = New System.Drawing.Point(4, 22)
        Me.CheckBox_KeepConfig.Name = "CheckBox_KeepConfig"
        Me.CheckBox_KeepConfig.Size = New System.Drawing.Size(177, 19)
        Me.CheckBox_KeepConfig.TabIndex = 0
        Me.CheckBox_KeepConfig.Text = "Keep Previous Configuration"
        Me.CheckBox_KeepConfig.UseVisualStyleBackColor = True
        '
        'ComboBox_Configuration
        '
        Me.ComboBox_Configuration.BackColor = System.Drawing.Color.FromArgb(CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ComboBox_Configuration.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Configuration.Enabled = False
        Me.ComboBox_Configuration.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ComboBox_Configuration.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ComboBox_Configuration.ForeColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(36, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.ComboBox_Configuration.FormattingEnabled = True
        Me.ComboBox_Configuration.Location = New System.Drawing.Point(7, 49)
        Me.ComboBox_Configuration.Name = "ComboBox_Configuration"
        Me.ComboBox_Configuration.Size = New System.Drawing.Size(170, 23)
        Me.ComboBox_Configuration.TabIndex = 1
        Me.ComboBox_Configuration.Tag = "Configuration"
        '
        'Button_Start
        '
        Me.Button_Start.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(180, Byte), Integer), CType(CType(100, Byte), Integer))
        Me.Button_Start.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button_Start.Enabled = False
        Me.Button_Start.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(180, Byte), Integer), CType(CType(100, Byte), Integer))
        Me.Button_Start.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(22, Byte), Integer), CType(CType(155, Byte), Integer), CType(CType(82, Byte), Integer))
        Me.Button_Start.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button_Start.Font = New System.Drawing.Font("Segoe UI Semibold", 9.5!, System.Drawing.FontStyle.Bold)
        Me.Button_Start.ForeColor = System.Drawing.Color.White
        Me.Button_Start.Location = New System.Drawing.Point(216, 170)
        Me.Button_Start.Name = "Button_Start"
        Me.Button_Start.Padding = New System.Windows.Forms.Padding(3)
        Me.Button_Start.Size = New System.Drawing.Size(183, 36)
        Me.Button_Start.TabIndex = 2
        Me.Button_Start.Text = "Start"
        Me.Button_Start.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(0, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(0, 13)
        Me.Label2.TabIndex = 99
        Me.Label2.Visible = False
        '
        'Splitter1
        '
        Me.Splitter1.Location = New System.Drawing.Point(3, 0)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(3, 218)
        Me.Splitter1.TabIndex = 80
        Me.Splitter1.TabStop = False
        Me.Splitter1.Visible = False
        '
        'Splitter2
        '
        Me.Splitter2.Location = New System.Drawing.Point(0, 0)
        Me.Splitter2.Name = "Splitter2"
        Me.Splitter2.Size = New System.Drawing.Size(3, 218)
        Me.Splitter2.TabIndex = 81
        Me.Splitter2.TabStop = False
        Me.Splitter2.Visible = False
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Size = New System.Drawing.Size(54, 9)
        Me.SplitContainer1.SplitterDistance = 25
        Me.SplitContainer1.SplitterWidth = 3
        Me.SplitContainer1.TabIndex = 82
        Me.SplitContainer1.Visible = False
        '
        'Panel1
        '
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(9, 9)
        Me.Panel1.TabIndex = 83
        Me.Panel1.Visible = False
        '
        'Panel_Com_Parameters
        '
        Me.Panel_Com_Parameters.Location = New System.Drawing.Point(0, 0)
        Me.Panel_Com_Parameters.Name = "Panel_Com_Parameters"
        Me.Panel_Com_Parameters.Size = New System.Drawing.Size(9, 9)
        Me.Panel_Com_Parameters.TabIndex = 84
        Me.Panel_Com_Parameters.Visible = False
        '
        'Panel_Baudrate
        '
        Me.Panel_Baudrate.Location = New System.Drawing.Point(0, 0)
        Me.Panel_Baudrate.Name = "Panel_Baudrate"
        Me.Panel_Baudrate.Size = New System.Drawing.Size(9, 9)
        Me.Panel_Baudrate.TabIndex = 85
        Me.Panel_Baudrate.Visible = False
        '
        'Panel_DeviceAddress
        '
        Me.Panel_DeviceAddress.Location = New System.Drawing.Point(0, 0)
        Me.Panel_DeviceAddress.Name = "Panel_DeviceAddress"
        Me.Panel_DeviceAddress.Size = New System.Drawing.Size(9, 9)
        Me.Panel_DeviceAddress.TabIndex = 86
        Me.Panel_DeviceAddress.Visible = False
        '
        'Panel2
        '
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(9, 9)
        Me.Panel2.TabIndex = 87
        Me.Panel2.Visible = False
        '
        'Panel3
        '
        Me.Panel3.Location = New System.Drawing.Point(0, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(9, 9)
        Me.Panel3.TabIndex = 88
        Me.Panel3.Visible = False
        '
        'Panel4
        '
        Me.Panel4.Location = New System.Drawing.Point(0, 0)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(9, 9)
        Me.Panel4.TabIndex = 89
        Me.Panel4.Visible = False
        '
        'Timer_Connection
        '
        '
        'Label_Epack_
        '
        Me.Label_Epack_.AutoSize = True
        Me.Label_Epack_.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Label_Epack_.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label_Epack_.Location = New System.Drawing.Point(34, 100)
        Me.Label_Epack_.Name = "Label_Epack_"
        Me.Label_Epack_.Size = New System.Drawing.Size(127, 15)
        Me.Label_Epack_.TabIndex = 102
        Me.Label_Epack_.Text = "Epack TCP Connection"
        '
        'ConnectionWindow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(411, 260)
        Me.Controls.Add(Me.Panel_Communication)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "ConnectionWindow"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "53U - Modbus Connection"
        Me.Panel_Communication.ResumeLayout(False)
        Me.Panel_Header.ResumeLayout(False)
        Me.Panel_Header.PerformLayout()
        Me.Panel_Body.ResumeLayout(False)
        Me.Panel_Body.PerformLayout()
        Me.GroupBox_ComParams.ResumeLayout(False)
        Me.TableLayout_ComParams.ResumeLayout(False)
        Me.TableLayout_ComParams.PerformLayout()
        Me.GroupBox_Config.ResumeLayout(False)
        Me.GroupBox_Config.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Button_Connect As System.Windows.Forms.Button
    Friend WithEvents Label_DeviceAddress_Title As System.Windows.Forms.Label
    Friend WithEvents Panel_Communication As System.Windows.Forms.Panel
    Friend WithEvents Label_ConnectionStatus_Title As System.Windows.Forms.Label
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents CheckBox_KeepConfig As System.Windows.Forms.CheckBox
    Friend WithEvents ComboBox_Configuration As System.Windows.Forms.ComboBox
    Friend WithEvents Button_Start As System.Windows.Forms.Button
    Friend WithEvents Splitter2 As System.Windows.Forms.Splitter
    Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
    Friend WithEvents Label_ConnectionStatus As System.Windows.Forms.Panel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel_Com_Parameters As System.Windows.Forms.Panel
    Friend WithEvents Label_COMPort_Title As System.Windows.Forms.Label
    Friend WithEvents ComboBox_COMPort As System.Windows.Forms.ComboBox
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents ComboBox_StopBit As System.Windows.Forms.ComboBox
    Friend WithEvents Label_StopBit_Title As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents ComboBox_Parity As System.Windows.Forms.ComboBox
    Friend WithEvents Label_Parity_Title As System.Windows.Forms.Label
    Friend WithEvents Panel_Baudrate As System.Windows.Forms.Panel
    Friend WithEvents ComboBox_Baudrate As System.Windows.Forms.ComboBox
    Friend WithEvents Label_Baudrate_Title As System.Windows.Forms.Label
    Friend WithEvents Panel_DeviceAddress As System.Windows.Forms.Panel
    Friend WithEvents TextBox_DeviceAddress As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Timer_Connection As System.Windows.Forms.Timer
    Friend WithEvents ComPort As System.IO.Ports.SerialPort
    Friend WithEvents Panel_Header As System.Windows.Forms.Panel
    Friend WithEvents Label_FormTitle As System.Windows.Forms.Label
    Friend WithEvents Panel_Body As System.Windows.Forms.Panel
    Friend WithEvents GroupBox_ComParams As System.Windows.Forms.GroupBox
    Friend WithEvents TableLayout_ComParams As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Panel_Divider As System.Windows.Forms.Panel
    Friend WithEvents GroupBox_Config As System.Windows.Forms.GroupBox
    Friend WithEvents Button_Connect_TCP As Button
    Friend WithEvents SerialPort1 As IO.Ports.SerialPort
    Friend WithEvents Button_ReadTCP As Button
    Friend WithEvents Label_Epack_ As Label
End Class