Option Strict On
Option Explicit On

Imports System.IO
Imports System.Collections.Generic



' =============================================================================
' MeterWindow.vb  -- Partial Class (1/4)
' =============================================================================
Public Class MeterWindow

    Friend ReadOnly _prefs As UserPreferences = UserPreferences.Instance
    Friend _isChartFrozen As Boolean = False

    ' Mode demo : lance MeterWindow sans connexion Modbus, timer pause automatiquement
    Public Shared IsDemoMode As Boolean = False
    Friend _isChartReady As Boolean = False
    Private _lossAlreadyHandled As Boolean = False
    Friend _latestReadings As New Dictionary(Of SignalID, Double)

    ' =============================================================================
    ' Mise en forme et couleurs
    ' =============================================================================
#Region "Region Colors"
    ' -- Forme & header --
    Dim clrFormBg As System.Drawing.Color = System.Drawing.Color.FromArgb(236, 241, 247)
    Dim clrHeaderBg As System.Drawing.Color = System.Drawing.Color.FromArgb(36, 52, 80)
    Dim clrHeaderTitle As System.Drawing.Color = System.Drawing.Color.FromArgb(220, 230, 245)
    Dim clrTabActive As System.Drawing.Color = System.Drawing.Color.FromArgb(0, 122, 204)
    Dim clrTabInactive As System.Drawing.Color = System.Drawing.Color.FromArgb(28, 42, 66)
    Dim clrTabActiveFg As System.Drawing.Color = System.Drawing.Color.White
    Dim clrTabInactiveFg As System.Drawing.Color = System.Drawing.Color.FromArgb(140, 165, 200)
    ' -- Panel principal --
    Dim clrMainBg As System.Drawing.Color = clrFormBg
    Dim clrGroupBoxBg As System.Drawing.Color = System.Drawing.Color.FromArgb(248, 250, 254)
    Dim clrGroupBoxFg As System.Drawing.Color = clrHeaderBg
    ' -- Panneau valeurs electriques --
    Dim clrElecNameBg As System.Drawing.Color = System.Drawing.Color.FromArgb(228, 235, 246)  ' fond label nom
    Dim clrElecNameFg As System.Drawing.Color = System.Drawing.Color.FromArgb(90, 108, 136)  ' texte label nom
    Dim clrElecUnitFg As System.Drawing.Color = clrElecNameFg                                 ' texte unite
    Dim clrValBg As System.Drawing.Color = System.Drawing.Color.FromArgb(12, 20, 36)   ' fond TextBox valeur
    ' -- Couleurs signaux (ForeColor TextBox) --
    Dim clrSigI As System.Drawing.Color = System.Drawing.Color.FromArgb(0, 220, 150)  ' Courant
    Dim clrSigU As System.Drawing.Color = System.Drawing.Color.DodgerBlue               ' Tension
    Dim clrSigP As System.Drawing.Color = System.Drawing.Color.FromArgb(255, 185, 50)  ' P actif
    Dim clrSigQ As System.Drawing.Color = System.Drawing.Color.FromArgb(255, 100, 50)  ' Q reactif
    Dim clrSigS As System.Drawing.Color = System.Drawing.Color.FromArgb(200, 130, 255)  ' S apparent
    Dim clrSigPF As System.Drawing.Color = System.Drawing.Color.FromArgb(255, 110, 110)  ' Power Factor
    Dim clrSigF As System.Drawing.Color = System.Drawing.Color.FromArgb(140, 200, 255)  ' Frequence
    Dim clrSigPhaseDir As System.Drawing.Color = System.Drawing.Color.FromArgb(160, 180, 210)  ' Phase Dir texte
    Dim clrSigPhaseDirBg As System.Drawing.Color = clrValBg                                      ' Phase Dir fond
    ' -- THD --
    Dim clrTHDiNameBg As System.Drawing.Color = System.Drawing.Color.FromArgb(22, 50, 40)   ' fond nom THDi
    Dim clrTHDuNameBg As System.Drawing.Color = System.Drawing.Color.FromArgb(20, 36, 62)   ' fond nom THDu
    Dim clrTHDiFg As System.Drawing.Color = System.Drawing.Color.FromArgb(80, 220, 160)  ' texte THDi
    Dim clrTHDuFg As System.Drawing.Color = System.Drawing.Color.FromArgb(80, 160, 255)  ' texte THDu
    Dim clrTHDValBg As System.Drawing.Color = clrValBg                                      ' fond val THD
    Dim clrTHDUnitFg As System.Drawing.Color = clrElecNameFg
    ' -- Checkbox THD nuances --
    Dim clrChkTHDi1 As System.Drawing.Color = System.Drawing.Color.FromArgb(0, 220, 120)
    Dim clrChkTHDi2 As System.Drawing.Color = System.Drawing.Color.FromArgb(0, 190, 95)
    Dim clrChkTHDi3 As System.Drawing.Color = System.Drawing.Color.FromArgb(0, 160, 70)
    Dim clrChkTHDiN As System.Drawing.Color = System.Drawing.Color.FromArgb(0, 250, 170)
    Dim clrChkTHDu12 As System.Drawing.Color = System.Drawing.Color.FromArgb(80, 160, 255)
    Dim clrChkTHDu23 As System.Drawing.Color = System.Drawing.Color.FromArgb(50, 130, 235)
    Dim clrChkTHDu31 As System.Drawing.Color = System.Drawing.Color.FromArgb(30, 100, 215)
    Dim clrChkTHDu1N As System.Drawing.Color = System.Drawing.Color.FromArgb(110, 185, 255)
    Dim clrChkTHDu2N As System.Drawing.Color = System.Drawing.Color.FromArgb(140, 210, 255)
    Dim clrChkTHDu3N As System.Drawing.Color = System.Drawing.Color.FromArgb(170, 230, 255)
    Dim clrChkPhaseDir As System.Drawing.Color = System.Drawing.Color.FromArgb(255, 220, 0)
    ' -- Footer --
    Dim clrFooterBg As System.Drawing.Color = System.Drawing.Color.FromArgb(22, 34, 56)
    Dim clrFooterGroup As System.Drawing.Color = System.Drawing.Color.FromArgb(28, 44, 70)
    Dim clrFooterTitle As System.Drawing.Color = System.Drawing.Color.FromArgb(90, 130, 190)
    Dim clrBtnSamplingBg As System.Drawing.Color = System.Drawing.Color.FromArgb(0, 105, 145)
    Dim clrBtnFreezeBg As System.Drawing.Color = System.Drawing.Color.FromArgb(14, 72, 148)
    Dim clrBtnFreezeFg As System.Drawing.Color = System.Drawing.Color.FromArgb(170, 215, 255)
    Dim clrBtnResetBg As System.Drawing.Color = System.Drawing.Color.FromArgb(50, 64, 88)
    Dim clrBtnResetFg As System.Drawing.Color = System.Drawing.Color.FromArgb(195, 210, 230)
    Dim clrBtnRecBg As System.Drawing.Color = System.Drawing.Color.FromArgb(30, 80, 30)
    Dim clrBtnRecFg As System.Drawing.Color = System.Drawing.Color.FromArgb(160, 240, 160)
    Dim clrExitGroupBg As System.Drawing.Color = System.Drawing.Color.FromArgb(58, 20, 20)
    Dim clrBtnStopBg As System.Drawing.Color = System.Drawing.Color.FromArgb(140, 28, 28)
    Dim clrBtnStopFg As System.Drawing.Color = System.Drawing.Color.FromArgb(255, 210, 210)
    ' -- Hover boutons --
    Dim clrHoverSampling As System.Drawing.Color = System.Drawing.Color.FromArgb(0, 130, 175)
    Dim clrHoverRec As System.Drawing.Color = System.Drawing.Color.FromArgb(42, 105, 42)
    Dim clrHoverFreeze As System.Drawing.Color = System.Drawing.Color.FromArgb(22, 92, 175)
    Dim clrHoverReset As System.Drawing.Color = System.Drawing.Color.FromArgb(66, 82, 110)
    Dim clrHoverStop As System.Drawing.Color = System.Drawing.Color.FromArgb(175, 42, 42)
    Dim clrHoverRecPause As System.Drawing.Color = System.Drawing.Color.FromArgb(42, 60, 95)
    ' -- Harmonics / DataGridView --
    Dim clrHarmBg As System.Drawing.Color = System.Drawing.Color.FromArgb(18, 24, 38)
    Dim clrDGVHeaderBg As System.Drawing.Color = System.Drawing.Color.FromArgb(26, 36, 58)
    Dim clrDGVHeaderFg As System.Drawing.Color = System.Drawing.Color.FromArgb(160, 180, 210)
    Dim clrDGVCellBg As System.Drawing.Color = System.Drawing.Color.FromArgb(22, 30, 48)
    Dim clrDGVCellFg As System.Drawing.Color = System.Drawing.Color.FromArgb(210, 225, 245)
    Dim clrDGVSelBg As System.Drawing.Color = System.Drawing.Color.FromArgb(40, 60, 100)
    Dim clrDGVGrid As System.Drawing.Color = clrHeaderBg
    ' -- Divers --
    Dim clrLedOff As System.Drawing.Color = System.Drawing.Color.FromArgb(80, 80, 80)
    Dim clrStatusFg As System.Drawing.Color = System.Drawing.Color.FromArgb(120, 140, 170)
    Dim clrFlash As System.Drawing.Color = System.Drawing.Color.FromArgb(0, 200, 180)
    Dim clrTHDFgUnused As System.Drawing.Color = System.Drawing.Color.FromArgb(100, 140, 120)
    ' -- Chart groupbox --
    Dim clrChartGroupBg As System.Drawing.Color = clrGroupBoxBg
#End Region

    Friend Enum ViewMode
        Live
        Harmonic
    End Enum

    ' =============================================================================
    '   LOAD
    ' =============================================================================
    Private Sub MeterWindow_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' ── Panel_Main : Anchor tous cotes — evite le conflit avec Dock=Top du header
        Panel_Main.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        ' ── Panels internes ───────────────────────────────────────────────
        GroupBox_ElecValues.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        GroupBox_Chart.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        Panel_Harmonics.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        ' ── Elements ancrés a droite (header + footer) ───────────────────
        Label_StatusText.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Label_ConnectionStatus2.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Panel_FooterRecordGroup.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Panel_FooterSettingsGroup.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Panel_FooterExitGroup.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        ' 1. Runtime controls (Harmonic panel + Y1/Y2 combos)
        InitRuntimeControls()

        ' 2. Double-buffering on 3 panels
        EnableDoubleBuffer(Panel_Main)
        EnableDoubleBuffer(Panel_ElecValScroll)
        EnableDoubleBuffer(Panel_Harmonics)

        ' 3. Footer buttons
        Button_Sampling.Text = "⏹ STOP"
        Button_ChartPause.Text = "❄ FREEZE"
        Button_ChartReset.Text = "↺ RESET"
        Button_Record.Text = "⏺ REC"
        Button_RecordPause.Text = "⏸ PAUSE REC"
        Button_Settings.Text = "⚙ PREFERENCES"
        Button_Stop.Text = "■ QUIT"

        ' 4. Build dynamic Electrical Values panel
        BuildElecValuesPanel()

        ' Force Panel_Main to correct size immediately (Anchor doesn't apply until first resize)
        Panel_Main.Size = New Size(Me.ClientSize.Width,
                                   Me.ClientSize.Height - Panel_Header.Height - Panel_Footer.Height)
        Panel_Main.PerformLayout()

        ' 5. Sample timer
        TrackBar_Sample.Value = _prefs.SampleInterval \ 100  'ms
        Timer_Sample.Interval = _prefs.SampleInterval
        UpdateSampleLabel()
        Timer_Sample.Stop()

        ' 6. Initialize chart with saved prefs
        Main_Chart.InitializeChart(_prefs.GetPanelRealTimeSignals(), True)
        _isChartReady = True
        Main_Chart.StartStopwatch()
        Main_Chart.SetAxisGroups(_prefs.Y1GroupIndex, _prefs.Y2GroupIndex)
        ApplyAllChartSignalStates()

        InitHdPanel()
        _isHdPanelBuilt = True

        ' 7. Default view
        ShowView(ViewMode.Live)
        UpdateAxisGroupHighlight()

        ' 8. Re-sync combos AFTER chart init to ensure prefs are applied
        '    (InitRuntimeControls calls SyncY1Y2CombosFromPrefs before _isChartReady)
        SyncY1Y2CombosFromPrefs()

        ' 9. Start sampling if connected (jamais en mode demo)
        If (IsConnected() OrElse ModbusTcpMaster.modbustcpisconnected) AndAlso Not IsDemoMode Then
            Timer_Sample.Start()
        End If
        UpdateSamplingButton()
        'UpdateChartPauseButton()
        UpdateRecordButton()

        ' En mode demo : afficher un indicateur visuel
        If IsDemoMode Then
            Me.Text = "Electric Meter Monitor  [DEMO MODE]"
            Label_StatusText.Text = "DEMO"
            Label_StatusText.ForeColor = Drawing.Color.FromArgb(255, 185, 50)
        End If
    End Sub

    ' Enable double buffer pour réduire le scintillement des pannels
    Private Shared Sub EnableDoubleBuffer(ctrl As Control)
        Dim prop As System.Reflection.PropertyInfo =
            GetType(Control).GetProperty(
                "DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic Or System.Reflection.BindingFlags.Instance)
        prop?.SetValue(ctrl, True, Nothing)
    End Sub

    ' =============================================================================
    ' CLOSE
    ' =============================================================================
    Private Sub MeterWindow_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If _isRecording Then
            If MessageBox.Show(
                    "A recording is in progress. Stop and quit anyway?",
                    "Recording in progress",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) <> DialogResult.Yes Then
                e.Cancel = True : Return
            End If
        End If
        If MessageBox.Show("Disconnect and return to connection screen?", "Quit",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            e.Cancel = True : Return
        End If
        If _isRecording Then StopRecording()
        Main_Chart.PauseStopwatch()
        _prefs.Save()
        ConnectionWindow.Show()
    End Sub

    Private Sub Button_Stop_Click(sender As Object, e As EventArgs) Handles Button_Stop.Click
        Me.Close()
    End Sub

    ' =============================================================================
    ' NAVIGATION
    ' =============================================================================
    Private Sub ShowView(mode As ViewMode)
        Me.SuspendLayout() : Panel_Main.SuspendLayout() 'freeze avant changement d'affichage 

        Try
            Panel_Harmonics.Visible = False
            Panel_HD.Visible = False
            GroupBox_ElecValues.Visible = False
            GroupBox_Chart.Visible = False
            Panel_FooterChartGroup.Visible = False  ' shown per-view below

            Select Case mode
                Case ViewMode.Live
                    GroupBox_ElecValues.Visible = True
                    GroupBox_Chart.Visible = True
                    Panel_FooterChartGroup.Visible = True
                    Button_ChartPause.Visible = False
                    ' Reset button fills the whole footer chart group
                    Button_ChartReset.Location = New Point(8, 8)
                    Button_ChartReset.Width = Panel_FooterChartGroup.Width - 16
                    SetTabActive(Button_ViewLive)
                    SetTabInactive(Button_ViewHarmonics)
                    If Not _isChartReady Then
                        Main_Chart.InitializeChart(_prefs.GetEnabledRealTimeSignals(), False)
                        _isChartReady = True
                        Main_Chart.StartStopwatch()
                        Main_Chart.SetAxisGroups(_prefs.Y1GroupIndex, _prefs.Y2GroupIndex)
                        ApplyAllChartSignalStates()
                        UpdateAxisGroupHighlight()
                    End If

                Case ViewMode.Harmonic
                    Panel_HD.Visible = True
                    Panel_FooterChartGroup.Visible = False
                    SetTabInactive(Button_ViewLive)
                    SetTabActive(Button_ViewHarmonics)
                    'UpdateFreezePauseButton()
                    If Not _isHdPanelBuilt Then
                        InitHdPanel()
                        _isHdPanelBuilt = True
                    End If
            End Select
        Finally
            Panel_Main.ResumeLayout(True) : Me.ResumeLayout(True)
        End Try
    End Sub

    Friend Sub SetTabActive(btn As Button)
        btn.BackColor = Drawing.Color.FromArgb(0, 122, 204)
        btn.ForeColor = Drawing.Color.White
    End Sub
    Friend Sub SetTabInactive(btn As Button)
        btn.BackColor = Drawing.Color.FromArgb(28, 42, 66)
        btn.ForeColor = Drawing.Color.FromArgb(140, 165, 200)
    End Sub

    Private Sub Button_ViewLive_Click(s As Object, e As EventArgs) Handles Button_ViewLive.Click
        ShowView(ViewMode.Live)
    End Sub
    Private Sub Button_ViewHarmonics_Click(s As Object, e As EventArgs) Handles Button_ViewHarmonics.Click
        ShowView(ViewMode.Harmonic)
    End Sub
    Private Sub Button_ViewHistorical_Click(s As Object, e As EventArgs) Handles Button_ViewHistorical.Click
        'Dim wasRunning As Boolean = Timer_Sample.Enabled
        'If wasRunning Then
        Timer_Sample.Stop()
        Main_Chart.PauseStopwatch()
        _csvPaused = True
        'UpdateChartPauseButton()
        UpdateSamplingButton()
        'End If

        Dim hw As New HistoricalWindow()
        'AddHandler hw.FormClosed, Sub(fs As Object, fe As FormClosedEventArgs)
        '                              If wasRunning Then Timer_Sample.Start()
        '                              UpdateSamplingButton()
        '                          End Sub
        hw.Show()
    End Sub

    ' PREFERENCES UI REFRESH
    Friend Sub OnPreferencesApplied()
        ' Timers
        Timer_Sample.Interval = _prefs.SampleInterval
        TrackBar_Sample.Value = Math.Min(50, Math.Max(1, _prefs.SampleInterval \ 100))
        UpdateSampleLabel()

        ' Rebuild dynamic panel
        Panel_ElecValScroll.AutoScrollPosition = New System.Drawing.Point(0, 0)
        Panel_ElecValScroll.VerticalScroll.Value = 0
        Panel_ElecValScroll.VerticalScroll.Minimum = 0
        Panel_ElecValScroll.PerformLayout()
        Panel_ElecValScroll.Invalidate(True)
        Panel_ElecValScroll.Refresh()

        BuildElecValuesPanel()

        ' Rebuild chart
        Main_Chart.InitializeChart(_prefs.GetPanelRealTimeSignals(), True)
        'Main_Chart.InitializeChart(_prefs.GetEnabledRealTimeSignals(), True)
        _isChartReady = True
        Main_Chart.UpdateAxisScales()
        Main_Chart.SetAxisGroups(_prefs.Y1GroupIndex, _prefs.Y2GroupIndex)
        ApplyAllChartSignalStates()
        UpdateAxisGroupHighlight()
        SyncY1Y2CombosFromPrefs()
        If Timer_Sample.Enabled AndAlso Not _isChartFrozen Then Main_Chart.StartStopwatch()
        UpdateSamplingButton()

        ' HD rebuild
        InitHdPanel()
        _isHdPanelBuilt = True
        If IsConnected() OrElse IsDemoMode Then ReadAndRefreshHD()

        ChartReset()

    End Sub

End Class