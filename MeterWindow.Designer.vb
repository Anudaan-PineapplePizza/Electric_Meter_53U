<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MeterWindow
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Panel_Header = New System.Windows.Forms.Panel()
        Me.Label_Title = New System.Windows.Forms.Label()
        Me.Button_ViewLive = New System.Windows.Forms.Button()
        Me.Button_ViewHarmonics = New System.Windows.Forms.Button()
        Me.Button_ViewHistorical = New System.Windows.Forms.Button()
        Me.Label_StatusText = New System.Windows.Forms.Label()
        Me.Label_ConnectionStatus2 = New System.Windows.Forms.Panel()
        Me.Panel_Main = New System.Windows.Forms.Panel()
        Me.GroupBox_ElecValues = New System.Windows.Forms.GroupBox()
        Me.Panel_ElecValScroll = New System.Windows.Forms.Panel()
        Me.TableLayoutPanelElecVal = New System.Windows.Forms.TableLayoutPanel()
        Me.GroupBox_Chart = New System.Windows.Forms.GroupBox()
        Me.Main_Chart = New Electric_Meter_Project.LiveChart()
        Me.Panel_Harmonics = New System.Windows.Forms.Panel()
        Me.DataGridView_THD = New System.Windows.Forms.DataGridView()
        Me.Col_Name = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Col_Latest = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Col_Max = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Col_Bar = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel_Footer = New System.Windows.Forms.Panel()
        Me.Panel_FooterSampleGroup = New System.Windows.Forms.Panel()
        Me.Label_SampleRate_Title = New System.Windows.Forms.Label()
        Me.TrackBar_Sample = New System.Windows.Forms.TrackBar()
        Me.Label_SampleRate_Value = New System.Windows.Forms.Label()
        Me.Button_Sampling = New System.Windows.Forms.Button()
        Me.Panel_FooterChartGroup = New System.Windows.Forms.Panel()
        Me.Label_FooterChartTitle = New System.Windows.Forms.Label()
        Me.Button_ChartPause = New System.Windows.Forms.Button()
        Me.Button_ChartReset = New System.Windows.Forms.Button()
        Me.Panel_FooterRecordGroup = New System.Windows.Forms.Panel()
        Me.Label_FooterRecordTitle = New System.Windows.Forms.Label()
        Me.Button_Record = New System.Windows.Forms.Button()
        Me.Button_RecordPause = New System.Windows.Forms.Button()
        Me.Panel_FooterSettingsGroup = New System.Windows.Forms.Panel()
        Me.Button_Settings = New System.Windows.Forms.Button()
        Me.Panel_FooterExitGroup = New System.Windows.Forms.Panel()
        Me.Button_Stop = New System.Windows.Forms.Button()
        Me.Timer_Sample = New System.Windows.Forms.Timer(Me.components)
        Me.Timer_Flash = New System.Windows.Forms.Timer(Me.components)
        Me.Panel_Header.SuspendLayout()
        Me.Panel_Main.SuspendLayout()
        Me.GroupBox_ElecValues.SuspendLayout()
        Me.Panel_ElecValScroll.SuspendLayout()
        Me.TableLayoutPanelElecVal.SuspendLayout()
        Me.GroupBox_Chart.SuspendLayout()
        Me.Panel_Harmonics.SuspendLayout()
        CType(Me.DataGridView_THD, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel_Footer.SuspendLayout()
        Me.Panel_FooterSampleGroup.SuspendLayout()
        CType(Me.TrackBar_Sample, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel_FooterChartGroup.SuspendLayout()
        Me.Panel_FooterRecordGroup.SuspendLayout()
        Me.Panel_FooterSettingsGroup.SuspendLayout()
        Me.Panel_FooterExitGroup.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel_Header
        '
        Me.Panel_Header.BackColor = System.Drawing.Color.FromArgb(CType(CType(36, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.Panel_Header.Controls.Add(Me.Label_Title)
        Me.Panel_Header.Controls.Add(Me.Button_ViewLive)
        Me.Panel_Header.Controls.Add(Me.Button_ViewHarmonics)
        Me.Panel_Header.Controls.Add(Me.Button_ViewHistorical)
        Me.Panel_Header.Controls.Add(Me.Label_StatusText)
        Me.Panel_Header.Controls.Add(Me.Label_ConnectionStatus2)
        Me.Panel_Header.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel_Header.Location = New System.Drawing.Point(0, 0)
        Me.Panel_Header.Name = "Panel_Header"
        Me.Panel_Header.Size = New System.Drawing.Size(1288, 49)
        Me.Panel_Header.TabIndex = 0
        '
        'Label_Title
        '
        Me.Label_Title.AutoSize = True
        Me.Label_Title.Font = New System.Drawing.Font("Segoe UI Semibold", 13.0!, System.Drawing.FontStyle.Bold)
        Me.Label_Title.ForeColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(245, Byte), Integer))
        Me.Label_Title.Location = New System.Drawing.Point(14, 13)
        Me.Label_Title.Name = "Label_Title"
        Me.Label_Title.Size = New System.Drawing.Size(237, 25)
        Me.Label_Title.TabIndex = 0
        Me.Label_Title.Text = "ELECTRIC METER MONITOR"
        '
        'Button_ViewLive
        '
        Me.Button_ViewLive.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(122, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.Button_ViewLive.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button_ViewLive.FlatAppearance.BorderSize = 0
        Me.Button_ViewLive.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button_ViewLive.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold)
        Me.Button_ViewLive.ForeColor = System.Drawing.Color.White
        Me.Button_ViewLive.Location = New System.Drawing.Point(390, 8)
        Me.Button_ViewLive.Name = "Button_ViewLive"
        Me.Button_ViewLive.Size = New System.Drawing.Size(110, 30)
        Me.Button_ViewLive.TabIndex = 10
        Me.Button_ViewLive.Text = "LIVE"
        Me.Button_ViewLive.UseVisualStyleBackColor = False
        '
        'Button_ViewHarmonics
        '
        Me.Button_ViewHarmonics.BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(66, Byte), Integer))
        Me.Button_ViewHarmonics.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button_ViewHarmonics.FlatAppearance.BorderSize = 0
        Me.Button_ViewHarmonics.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button_ViewHarmonics.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold)
        Me.Button_ViewHarmonics.ForeColor = System.Drawing.Color.FromArgb(CType(CType(140, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.Button_ViewHarmonics.Location = New System.Drawing.Point(508, 8)
        Me.Button_ViewHarmonics.Name = "Button_ViewHarmonics"
        Me.Button_ViewHarmonics.Size = New System.Drawing.Size(130, 30)
        Me.Button_ViewHarmonics.TabIndex = 11
        Me.Button_ViewHarmonics.Text = "HARMONICS"
        Me.Button_ViewHarmonics.UseVisualStyleBackColor = False

        'Button_ViewHistorical
        Me.Button_ViewHistorical.BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(66, Byte), Integer))
        Me.Button_ViewHistorical.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button_ViewHistorical.FlatAppearance.BorderSize = 0
        Me.Button_ViewHistorical.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button_ViewHistorical.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold)
        Me.Button_ViewHistorical.ForeColor = System.Drawing.Color.FromArgb(CType(CType(140, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.Button_ViewHistorical.Location = New System.Drawing.Point(648, 8)
        Me.Button_ViewHistorical.Name = "Button_ViewHistorical"
        Me.Button_ViewHistorical.Size = New System.Drawing.Size(130, 30)
        Me.Button_ViewHistorical.TabIndex = 12
        Me.Button_ViewHistorical.Text = "HISTORICAL"
        Me.Button_ViewHistorical.UseVisualStyleBackColor = False
        '
        'Label_StatusText
        '
        Me.Label_StatusText.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label_StatusText.AutoSize = True
        Me.Label_StatusText.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold)
        Me.Label_StatusText.ForeColor = System.Drawing.Color.FromArgb(CType(CType(120, Byte), Integer), CType(CType(140, Byte), Integer), CType(CType(170, Byte), Integer))
        Me.Label_StatusText.Location = New System.Drawing.Point(1176, 18)
        Me.Label_StatusText.Name = "Label_StatusText"
        Me.Label_StatusText.Size = New System.Drawing.Size(51, 13)
        Me.Label_StatusText.TabIndex = 14
        Me.Label_StatusText.Text = "OFFLINE"
        '
        'Label_ConnectionStatus2
        '
        Me.Label_ConnectionStatus2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label_ConnectionStatus2.BackColor = System.Drawing.Color.FromArgb(CType(CType(80, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.Label_ConnectionStatus2.Location = New System.Drawing.Point(1234, 16)
        Me.Label_ConnectionStatus2.Name = "Label_ConnectionStatus2"
        Me.Label_ConnectionStatus2.Size = New System.Drawing.Size(16, 17)
        Me.Label_ConnectionStatus2.TabIndex = 15
        '
        'Panel_Main
        '
        Me.Panel_Main.BackColor = System.Drawing.Color.FromArgb(CType(CType(236, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.Panel_Main.Controls.Add(Me.GroupBox_ElecValues)
        Me.Panel_Main.Controls.Add(Me.GroupBox_Chart)
        Me.Panel_Main.Controls.Add(Me.Panel_Harmonics)
        Me.Panel_Main.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel_Main.Location = New System.Drawing.Point(0, 49)
        Me.Panel_Main.Name = "Panel_Main"
        Me.Panel_Main.Padding = New System.Windows.Forms.Padding(14)
        Me.Panel_Main.Size = New System.Drawing.Size(1272, 698)
        Me.Panel_Main.TabIndex = 1
        '
        'GroupBox_ElecValues
        '
        Me.GroupBox_ElecValues.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(250, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.GroupBox_ElecValues.Controls.Add(Me.Panel_ElecValScroll)
        Me.GroupBox_ElecValues.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.GroupBox_ElecValues.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold)
        Me.GroupBox_ElecValues.ForeColor = System.Drawing.Color.FromArgb(CType(CType(36, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.GroupBox_ElecValues.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.GroupBox_ElecValues.Location = New System.Drawing.Point(14, 14)
        Me.GroupBox_ElecValues.Name = "GroupBox_ElecValues"
        Me.GroupBox_ElecValues.Size = New System.Drawing.Size(228, 670)
        Me.GroupBox_ElecValues.TabIndex = 0
        Me.GroupBox_ElecValues.TabStop = False
        Me.GroupBox_ElecValues.Text = "  ELECTRICAL VALUES"
        '
        'Panel_ElecValScroll
        '
        Me.Panel_ElecValScroll.AutoScroll = True
        Me.Panel_ElecValScroll.BackColor = System.Drawing.Color.Transparent
        Me.Panel_ElecValScroll.Controls.Add(Me.TableLayoutPanelElecVal)
        Me.Panel_ElecValScroll.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel_ElecValScroll.Location = New System.Drawing.Point(3, 19)
        Me.Panel_ElecValScroll.Name = "Panel_ElecValScroll"
        Me.Panel_ElecValScroll.Padding = New System.Windows.Forms.Padding(2)
        Me.Panel_ElecValScroll.Size = New System.Drawing.Size(222, 516)
        Me.Panel_ElecValScroll.TabIndex = 0
        '
        'GroupBox_Chart
        '
        Me.GroupBox_Chart.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(250, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.GroupBox_Chart.Controls.Add(Me.Main_Chart)
        Me.GroupBox_Chart.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.GroupBox_Chart.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold)
        Me.GroupBox_Chart.ForeColor = System.Drawing.Color.FromArgb(CType(CType(36, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.GroupBox_Chart.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox_Chart.Location = New System.Drawing.Point(256, 14)
        Me.GroupBox_Chart.Name = "GroupBox_Chart"
        Me.GroupBox_Chart.Size = New System.Drawing.Size(1002, 670)
        Me.GroupBox_Chart.TabIndex = 1
        Me.GroupBox_Chart.TabStop = False
        Me.GroupBox_Chart.Text = "  LIVE CHART"
        '
        'Main_Chart
        '
        Me.Main_Chart.BackColor = System.Drawing.Color.FromArgb(CType(CType(16, Byte), Integer), CType(CType(22, Byte), Integer), CType(CType(36, Byte), Integer))
        Me.Main_Chart.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Main_Chart.Location = New System.Drawing.Point(3, 19)
        Me.Main_Chart.Name = "Main_Chart"
        Me.Main_Chart.Size = New System.Drawing.Size(1002, 670)
        Me.Main_Chart.TabIndex = 0
        '
        'Panel_Harmonics
        '
        Me.Panel_Harmonics.BackColor = System.Drawing.Color.FromArgb(CType(CType(18, Byte), Integer), CType(CType(24, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.Panel_Harmonics.Controls.Add(Me.DataGridView_THD)
        Me.Panel_Harmonics.Location = New System.Drawing.Point(14, 14)
        Me.Panel_Harmonics.Name = "Panel_Harmonics"
        Me.Panel_Harmonics.Size = New System.Drawing.Size(1244, 670)
        Me.Panel_Harmonics.TabIndex = 2
        Me.Panel_Harmonics.Visible = False
        '
        'DataGridView_THD
        '
        Me.DataGridView_THD.AllowUserToAddRows = False
        Me.DataGridView_THD.AllowUserToDeleteRows = False
        Me.DataGridView_THD.AllowUserToResizeRows = False
        Me.DataGridView_THD.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(18, Byte), Integer), CType(CType(24, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.DataGridView_THD.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.DataGridView_THD.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal
        Me.DataGridView_THD.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(26, Byte), Integer), CType(CType(36, Byte), Integer), CType(CType(58, Byte), Integer))
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold)
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(160, Byte), Integer), CType(CType(180, Byte), Integer), CType(CType(210, Byte), Integer))
        Me.DataGridView_THD.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridView_THD.ColumnHeadersHeight = 30
        Me.DataGridView_THD.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.DataGridView_THD.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Col_Name, Me.Col_Latest, Me.Col_Max, Me.Col_Bar})
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(CType(CType(22, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(48, Byte), Integer))
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Consolas", 10.0!, System.Drawing.FontStyle.Bold)
        DataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(210, Byte), Integer), CType(CType(225, Byte), Integer), CType(CType(245, Byte), Integer))
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(100, Byte), Integer))
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridView_THD.DefaultCellStyle = DataGridViewCellStyle6
        Me.DataGridView_THD.GridColor = System.Drawing.Color.FromArgb(CType(CType(36, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.DataGridView_THD.Location = New System.Drawing.Point(10, 10)
        Me.DataGridView_THD.MultiSelect = False
        Me.DataGridView_THD.Name = "DataGridView_THD"
        Me.DataGridView_THD.ReadOnly = True
        Me.DataGridView_THD.RowHeadersVisible = False
        Me.DataGridView_THD.RowTemplate.Height = 42
        Me.DataGridView_THD.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.DataGridView_THD.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView_THD.Size = New System.Drawing.Size(1008, 516)
        Me.DataGridView_THD.TabIndex = 0
        '
        'Col_Name
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold)
        Me.Col_Name.DefaultCellStyle = DataGridViewCellStyle2
        Me.Col_Name.HeaderText = "Measure"
        Me.Col_Name.Name = "Col_Name"
        Me.Col_Name.ReadOnly = True
        Me.Col_Name.Width = 130
        '
        'Col_Latest
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Col_Latest.DefaultCellStyle = DataGridViewCellStyle3
        Me.Col_Latest.HeaderText = "Latest"
        Me.Col_Latest.Name = "Col_Latest"
        Me.Col_Latest.ReadOnly = True
        Me.Col_Latest.Width = 90
        '
        'Col_Max
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Col_Max.DefaultCellStyle = DataGridViewCellStyle4
        Me.Col_Max.HeaderText = "Max"
        Me.Col_Max.Name = "Col_Max"
        Me.Col_Max.ReadOnly = True
        Me.Col_Max.Width = 90
        '
        'Col_Bar
        '
        Me.Col_Bar.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        DataGridViewCellStyle5.Padding = New System.Windows.Forms.Padding(4, 8, 4, 8)
        Me.Col_Bar.DefaultCellStyle = DataGridViewCellStyle5
        Me.Col_Bar.HeaderText = ""
        Me.Col_Bar.Name = "Col_Bar"
        Me.Col_Bar.ReadOnly = True
        '
        'Panel_Footer
        '
        Me.Panel_Footer.BackColor = System.Drawing.Color.FromArgb(CType(CType(22, Byte), Integer), CType(CType(34, Byte), Integer), CType(CType(56, Byte), Integer))
        Me.Panel_Footer.Controls.Add(Me.Panel_FooterSampleGroup)
        Me.Panel_Footer.Controls.Add(Me.Panel_FooterChartGroup)
        Me.Panel_Footer.Controls.Add(Me.Panel_FooterRecordGroup)
        Me.Panel_Footer.Controls.Add(Me.Panel_FooterSettingsGroup)
        Me.Panel_Footer.Controls.Add(Me.Panel_FooterExitGroup)
        Me.Panel_Footer.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel_Footer.Location = New System.Drawing.Point(0, 615)
        Me.Panel_Footer.Name = "Panel_Footer"
        Me.Panel_Footer.Size = New System.Drawing.Size(1288, 45)
        Me.Panel_Footer.TabIndex = 2
        '
        'Panel_FooterSampleGroup
        '
        Me.Panel_FooterSampleGroup.BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(44, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.Panel_FooterSampleGroup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel_FooterSampleGroup.Controls.Add(Me.Label_SampleRate_Title)
        Me.Panel_FooterSampleGroup.Controls.Add(Me.TrackBar_Sample)
        Me.Panel_FooterSampleGroup.Controls.Add(Me.Label_SampleRate_Value)
        Me.Panel_FooterSampleGroup.Controls.Add(Me.Button_Sampling)
        Me.Panel_FooterSampleGroup.Location = New System.Drawing.Point(8, 4)
        Me.Panel_FooterSampleGroup.Name = "Panel_FooterSampleGroup"
        Me.Panel_FooterSampleGroup.Size = New System.Drawing.Size(302, 36)
        Me.Panel_FooterSampleGroup.TabIndex = 9
        '
        'Label_SampleRate_Title
        '
        Me.Label_SampleRate_Title.AutoSize = True
        Me.Label_SampleRate_Title.Font = New System.Drawing.Font("Segoe UI", 6.5!, System.Drawing.FontStyle.Bold)
        Me.Label_SampleRate_Title.ForeColor = System.Drawing.Color.FromArgb(CType(CType(90, Byte), Integer), CType(CType(130, Byte), Integer), CType(CType(190, Byte), Integer))
        Me.Label_SampleRate_Title.Location = New System.Drawing.Point(4, 1)
        Me.Label_SampleRate_Title.Name = "Label_SampleRate_Title"
        Me.Label_SampleRate_Title.Size = New System.Drawing.Size(41, 12)
        Me.Label_SampleRate_Title.TabIndex = 0
        Me.Label_SampleRate_Title.Text = "SAMPLE"
        '
        'TrackBar_Sample
        '
        Me.TrackBar_Sample.BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(44, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TrackBar_Sample.Location = New System.Drawing.Point(4, 12)
        Me.TrackBar_Sample.Maximum = 50
        Me.TrackBar_Sample.Minimum = 1
        Me.TrackBar_Sample.Name = "TrackBar_Sample"
        Me.TrackBar_Sample.Size = New System.Drawing.Size(114, 45)
        Me.TrackBar_Sample.TabIndex = 1
        Me.TrackBar_Sample.TickFrequency = 5
        Me.TrackBar_Sample.Value = 5
        '
        'Label_SampleRate_Value
        '
        Me.Label_SampleRate_Value.AutoSize = True
        Me.Label_SampleRate_Value.Font = New System.Drawing.Font("Consolas", 8.0!, System.Drawing.FontStyle.Bold)
        Me.Label_SampleRate_Value.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(180, Byte), Integer))
        Me.Label_SampleRate_Value.Location = New System.Drawing.Point(122, 15)
        Me.Label_SampleRate_Value.Name = "Label_SampleRate_Value"
        Me.Label_SampleRate_Value.Size = New System.Drawing.Size(37, 13)
        Me.Label_SampleRate_Value.TabIndex = 2
        Me.Label_SampleRate_Value.Text = "-- ms"
        '
        'Button_Sampling
        '
        Me.Button_Sampling.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(105, Byte), Integer), CType(CType(145, Byte), Integer))
        Me.Button_Sampling.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button_Sampling.FlatAppearance.BorderSize = 0
        Me.Button_Sampling.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(130, Byte), Integer), CType(CType(175, Byte), Integer))
        Me.Button_Sampling.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button_Sampling.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold)
        Me.Button_Sampling.ForeColor = System.Drawing.Color.White
        Me.Button_Sampling.Location = New System.Drawing.Point(178, 11)
        Me.Button_Sampling.Name = "Button_Sampling"
        Me.Button_Sampling.Size = New System.Drawing.Size(110, 22)
        Me.Button_Sampling.TabIndex = 3
        Me.Button_Sampling.Text = "STOP"
        Me.Button_Sampling.UseVisualStyleBackColor = False
        '
        'Panel_FooterChartGroup
        '
        Me.Panel_FooterChartGroup.BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(44, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.Panel_FooterChartGroup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel_FooterChartGroup.Controls.Add(Me.Label_FooterChartTitle)
        Me.Panel_FooterChartGroup.Controls.Add(Me.Button_ChartPause)
        Me.Panel_FooterChartGroup.Controls.Add(Me.Button_ChartReset)
        Me.Panel_FooterChartGroup.Location = New System.Drawing.Point(318, 4)
        Me.Panel_FooterChartGroup.Name = "Panel_FooterChartGroup"
        Me.Panel_FooterChartGroup.Size = New System.Drawing.Size(228, 36)
        Me.Panel_FooterChartGroup.TabIndex = 10
        '
        'Label_FooterChartTitle
        '
        Me.Label_FooterChartTitle.AutoSize = True
        Me.Label_FooterChartTitle.Font = New System.Drawing.Font("Segoe UI", 6.5!, System.Drawing.FontStyle.Bold)
        Me.Label_FooterChartTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(90, Byte), Integer), CType(CType(130, Byte), Integer), CType(CType(190, Byte), Integer))
        Me.Label_FooterChartTitle.Location = New System.Drawing.Point(4, 1)
        Me.Label_FooterChartTitle.Name = "Label_FooterChartTitle"
        Me.Label_FooterChartTitle.Size = New System.Drawing.Size(56, 12)
        Me.Label_FooterChartTitle.TabIndex = 0
        Me.Label_FooterChartTitle.Text = "LIVE CHART"
        '
        'Button_ChartPause
        '
        Me.Button_ChartPause.BackColor = System.Drawing.Color.FromArgb(CType(CType(14, Byte), Integer), CType(CType(72, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.Button_ChartPause.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button_ChartPause.FlatAppearance.BorderSize = 0
        Me.Button_ChartPause.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(22, Byte), Integer), CType(CType(92, Byte), Integer), CType(CType(175, Byte), Integer))
        Me.Button_ChartPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button_ChartPause.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold)
        Me.Button_ChartPause.ForeColor = System.Drawing.Color.FromArgb(CType(CType(170, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Button_ChartPause.Location = New System.Drawing.Point(16, 12)
        Me.Button_ChartPause.Name = "Button_ChartPause"
        Me.Button_ChartPause.Size = New System.Drawing.Size(104, 20)
        Me.Button_ChartPause.TabIndex = 1
        Me.Button_ChartPause.Text = "FREEZE"
        Me.Button_ChartPause.UseVisualStyleBackColor = False
        '
        'Button_ChartReset
        '
        Me.Button_ChartReset.BackColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(88, Byte), Integer))
        Me.Button_ChartReset.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button_ChartReset.FlatAppearance.BorderSize = 0
        Me.Button_ChartReset.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(66, Byte), Integer), CType(CType(82, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.Button_ChartReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button_ChartReset.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold)
        Me.Button_ChartReset.ForeColor = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(210, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.Button_ChartReset.Location = New System.Drawing.Point(126, 12)
        Me.Button_ChartReset.Name = "Button_ChartReset"
        Me.Button_ChartReset.Size = New System.Drawing.Size(86, 20)
        Me.Button_ChartReset.TabIndex = 2
        Me.Button_ChartReset.Text = "RESET"
        Me.Button_ChartReset.UseVisualStyleBackColor = False
        '
        'Panel_FooterRecordGroup
        '
        Me.Panel_FooterRecordGroup.BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(44, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.Panel_FooterRecordGroup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel_FooterRecordGroup.Controls.Add(Me.Label_FooterRecordTitle)
        Me.Panel_FooterRecordGroup.Controls.Add(Me.Button_Record)
        Me.Panel_FooterRecordGroup.Controls.Add(Me.Button_RecordPause)
        Me.Panel_FooterRecordGroup.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel_FooterRecordGroup.Location = New System.Drawing.Point(766, 4)
        Me.Panel_FooterRecordGroup.Name = "Panel_FooterRecordGroup"
        Me.Panel_FooterRecordGroup.Size = New System.Drawing.Size(222, 36)
        Me.Panel_FooterRecordGroup.TabIndex = 12
        '
        'Label_FooterRecordTitle
        '
        Me.Label_FooterRecordTitle.AutoSize = True
        Me.Label_FooterRecordTitle.Font = New System.Drawing.Font("Segoe UI", 6.5!, System.Drawing.FontStyle.Bold)
        Me.Label_FooterRecordTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(90, Byte), Integer), CType(CType(130, Byte), Integer), CType(CType(190, Byte), Integer))
        Me.Label_FooterRecordTitle.Location = New System.Drawing.Point(2, 1)
        Me.Label_FooterRecordTitle.Name = "Label_FooterRecordTitle"
        Me.Label_FooterRecordTitle.Size = New System.Drawing.Size(61, 12)
        Me.Label_FooterRecordTitle.TabIndex = 0
        Me.Label_FooterRecordTitle.Text = "CSV RECORD"
        '
        'Button_Record
        '
        Me.Button_Record.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Button_Record.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button_Record.FlatAppearance.BorderSize = 0
        Me.Button_Record.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(105, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.Button_Record.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button_Record.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold)
        Me.Button_Record.ForeColor = System.Drawing.Color.FromArgb(CType(CType(160, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.Button_Record.Location = New System.Drawing.Point(4, 11)
        Me.Button_Record.Name = "Button_Record"
        Me.Button_Record.Size = New System.Drawing.Size(104, 20)
        Me.Button_Record.TabIndex = 1
        Me.Button_Record.Text = "REC"
        Me.Button_Record.UseVisualStyleBackColor = False
        '
        'Button_RecordPause
        '
        Me.Button_RecordPause.BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(44, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.Button_RecordPause.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button_RecordPause.Enabled = False
        Me.Button_RecordPause.FlatAppearance.BorderSize = 0
        Me.Button_RecordPause.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(95, Byte), Integer))
        Me.Button_RecordPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button_RecordPause.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold)
        Me.Button_RecordPause.ForeColor = System.Drawing.Color.FromArgb(CType(CType(140, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.Button_RecordPause.Location = New System.Drawing.Point(112, 11)
        Me.Button_RecordPause.Name = "Button_RecordPause"
        Me.Button_RecordPause.Size = New System.Drawing.Size(104, 20)
        Me.Button_RecordPause.TabIndex = 2
        Me.Button_RecordPause.Text = "PAUSE REC"
        Me.Button_RecordPause.UseVisualStyleBackColor = False
        '
        'Panel_FooterSettingsGroup
        '
        Me.Panel_FooterSettingsGroup.BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(44, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.Panel_FooterSettingsGroup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel_FooterSettingsGroup.Controls.Add(Me.Button_Settings)
        Me.Panel_FooterSettingsGroup.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel_FooterSettingsGroup.Location = New System.Drawing.Point(996, 4)
        Me.Panel_FooterSettingsGroup.Name = "Panel_FooterSettingsGroup"
        Me.Panel_FooterSettingsGroup.Size = New System.Drawing.Size(132, 36)
        Me.Panel_FooterSettingsGroup.TabIndex = 13
        '
        'Button_Settings
        '
        Me.Button_Settings.BackColor = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(58, Byte), Integer), CType(CType(88, Byte), Integer))
        Me.Button_Settings.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button_Settings.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Button_Settings.FlatAppearance.BorderSize = 0
        Me.Button_Settings.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(58, Byte), Integer), CType(CType(78, Byte), Integer), CType(CType(118, Byte), Integer))
        Me.Button_Settings.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button_Settings.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold)
        Me.Button_Settings.ForeColor = System.Drawing.Color.FromArgb(CType(CType(190, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Button_Settings.Location = New System.Drawing.Point(0, 0)
        Me.Button_Settings.Name = "Button_Settings"
        Me.Button_Settings.Size = New System.Drawing.Size(130, 34)
        Me.Button_Settings.TabIndex = 0
        Me.Button_Settings.Text = "PREFERENCES"
        Me.Button_Settings.UseVisualStyleBackColor = False
        '
        'Panel_FooterExitGroup
        '
        Me.Panel_FooterExitGroup.BackColor = System.Drawing.Color.FromArgb(CType(CType(58, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.Panel_FooterExitGroup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel_FooterExitGroup.Controls.Add(Me.Button_Stop)
        Me.Panel_FooterExitGroup.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel_FooterExitGroup.Location = New System.Drawing.Point(1136, 4)
        Me.Panel_FooterExitGroup.Name = "Panel_FooterExitGroup"
        Me.Panel_FooterExitGroup.Size = New System.Drawing.Size(128, 36)
        Me.Panel_FooterExitGroup.TabIndex = 11
        '
        'Button_Stop
        '
        Me.Button_Stop.BackColor = System.Drawing.Color.FromArgb(CType(CType(140, Byte), Integer), CType(CType(28, Byte), Integer), CType(CType(28, Byte), Integer))
        Me.Button_Stop.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button_Stop.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Button_Stop.FlatAppearance.BorderSize = 0
        Me.Button_Stop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(175, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.Button_Stop.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button_Stop.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold)
        Me.Button_Stop.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(210, Byte), Integer), CType(CType(210, Byte), Integer))
        Me.Button_Stop.Location = New System.Drawing.Point(0, 0)
        Me.Button_Stop.Name = "Button_Stop"
        Me.Button_Stop.Size = New System.Drawing.Size(126, 34)
        Me.Button_Stop.TabIndex = 0
        Me.Button_Stop.Text = "QUIT"
        Me.Button_Stop.UseVisualStyleBackColor = False
        '
        'Timer_Sample
        '
        Me.Timer_Sample.Interval = 500
        '
        'Timer_Flash
        '
        Me.Timer_Flash.Interval = 150
        '
        'MeterWindow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(236, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1272, 792)
        Me.Controls.Add(Me.Panel_Header)
        Me.Controls.Add(Me.Panel_Main)
        Me.Controls.Add(Me.Panel_Footer)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "MeterWindow"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Electric Meter Monitor"
        Me.Panel_Header.ResumeLayout(False)
        Me.Panel_Header.PerformLayout()
        Me.Panel_Main.ResumeLayout(False)
        Me.GroupBox_ElecValues.ResumeLayout(False)
        Me.Panel_ElecValScroll.ResumeLayout(False)
        Me.TableLayoutPanelElecVal.ResumeLayout(False)
        Me.TableLayoutPanelElecVal.PerformLayout()
        Me.GroupBox_Chart.ResumeLayout(False)
        Me.Panel_Harmonics.ResumeLayout(False)
        CType(Me.DataGridView_THD, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel_Footer.ResumeLayout(False)
        Me.Panel_FooterSampleGroup.ResumeLayout(False)
        Me.Panel_FooterSampleGroup.PerformLayout()
        CType(Me.TrackBar_Sample, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel_FooterChartGroup.ResumeLayout(False)
        Me.Panel_FooterChartGroup.PerformLayout()
        Me.Panel_FooterRecordGroup.ResumeLayout(False)
        Me.Panel_FooterRecordGroup.PerformLayout()
        Me.Panel_FooterSettingsGroup.ResumeLayout(False)
        Me.Panel_FooterExitGroup.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    ' === DECLARATIONS ======================================================
    Friend WithEvents Panel_Header As System.Windows.Forms.Panel
    Friend WithEvents Label_Title As System.Windows.Forms.Label
    Friend WithEvents Button_ViewLive As System.Windows.Forms.Button
    Friend WithEvents Button_ViewHarmonics As System.Windows.Forms.Button
    Friend WithEvents Button_ViewHistorical As System.Windows.Forms.Button
    Friend WithEvents Label_ConnectionStatus2 As System.Windows.Forms.Panel
    Friend WithEvents Label_StatusText As System.Windows.Forms.Label
    Friend WithEvents Panel_Main As System.Windows.Forms.Panel
    Friend WithEvents GroupBox_ElecValues As System.Windows.Forms.GroupBox
    Friend WithEvents Panel_ElecValScroll As System.Windows.Forms.Panel
    Friend WithEvents TableLayoutPanelElecVal As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Panel_FooterRecordGroup As System.Windows.Forms.Panel
    Friend WithEvents Label_FooterRecordTitle As System.Windows.Forms.Label
    Friend WithEvents Button_Record As System.Windows.Forms.Button
    Friend WithEvents Button_RecordPause As System.Windows.Forms.Button
    Friend WithEvents GroupBox_Chart As System.Windows.Forms.GroupBox
    Friend WithEvents Main_Chart As LiveChart
    Friend WithEvents Panel_Harmonics As System.Windows.Forms.Panel
    Friend WithEvents DataGridView_THD As System.Windows.Forms.DataGridView
    Friend WithEvents Col_Name As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Col_Latest As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Col_Max As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Col_Bar As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Panel_Footer As System.Windows.Forms.Panel
    Friend WithEvents Panel_FooterSampleGroup As System.Windows.Forms.Panel
    Friend WithEvents Label_SampleRate_Title As System.Windows.Forms.Label
    Friend WithEvents TrackBar_Sample As System.Windows.Forms.TrackBar
    Friend WithEvents Label_SampleRate_Value As System.Windows.Forms.Label
    Friend WithEvents Button_Sampling As System.Windows.Forms.Button
    Friend WithEvents Panel_FooterChartGroup As System.Windows.Forms.Panel
    Friend WithEvents Label_FooterChartTitle As System.Windows.Forms.Label
    Friend WithEvents Button_ChartPause As System.Windows.Forms.Button
    Friend WithEvents Button_ChartReset As System.Windows.Forms.Button
    Friend WithEvents Panel_FooterExitGroup As System.Windows.Forms.Panel
    Friend WithEvents Button_Stop As System.Windows.Forms.Button
    Friend WithEvents Panel_FooterSettingsGroup As System.Windows.Forms.Panel
    Friend WithEvents Button_Settings As System.Windows.Forms.Button
    Friend WithEvents Timer_Sample As System.Windows.Forms.Timer
    Friend WithEvents Timer_Flash As System.Windows.Forms.Timer

End Class