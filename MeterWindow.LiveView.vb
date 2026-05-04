Option Strict On
Option Explicit On

Imports System.Collections.Generic

' =============================================================================
' MeterWindow.LiveView.vb  -- Partial Class (2/4)
'
'Responsibility : Electrical Dynamic Values panel,
'                 Y1/Y2 axis group selection,
'                 chart signal toggles,
'                 sample timer,
'                 record CSV,
'                 footer Control buttons
' =============================================================================
Partial Class MeterWindow

    ' Dynamic RUNTIME DISPLAY CONTROLS
    ' Calue label per signal (built in BuildElecValuesPanel)
    Private _dynValueLabels As New Dictionary(Of SignalID, Label)
    ' Chart-enable checkboxes per signal
    Private _dynChartChecks As New Dictionary(Of SignalID, CheckBox)

    ' Y1/Y2 Selection axis group combo boxes (built in InitRuntimeControls)
    Private _comboY1 As ComboBox = Nothing
    Private _comboY2 As ComboBox = Nothing

    ' Suppress combo events during synchro
    Private _suppressComboEvents As Boolean = False

    ' Colors for Y1/Y2 highlighted cells (active selection Y axis on chart)
    Private ReadOnly ColorY1Background As Drawing.Color = Drawing.Color.FromArgb(0, 45, 100)
    Private ReadOnly ColorY2Background As Drawing.Color = Drawing.Color.FromArgb(0, 60, 35)
    Private ReadOnly ColorDefaultBackground As Drawing.Color = Drawing.Color.FromArgb(16, 26, 46)

    ' BUILD ELEC VALUES PANEL  (called at Load + OnPreferencesApplied)
    ' Creates compact signal cells dynamically; hides Designer static controls.
    Friend Sub BuildElecValuesPanel()
        GroupBox_ElecValues.SuspendLayout()
        Panel_ElecValScroll.SuspendLayout()

        For Each ctrl As Control In Panel_ElecValScroll.Controls
            ctrl.Visible = False
        Next
        _dynValueLabels.Clear()
        _dynChartChecks.Clear()
        Dim oldDyn As Control = Panel_ElecValScroll.Controls("_dynPanel")
        If oldDyn IsNot Nothing Then
            Panel_ElecValScroll.Controls.Remove(oldDyn)
            oldDyn.Dispose()
        End If

        ' Grouper les signaux visibles par SignalGroup dans l ordre voulu
        Dim groupOrder As SignalGroup() = {
            SignalGroup.Voltage, SignalGroup.Current, SignalGroup.Power,
            SignalGroup.Phase, SignalGroup.Harmonic, SignalGroup.ePack}

        Dim byGroup As New Dictionary(Of SignalGroup, List(Of RegisterDef))()
        For Each sg As SignalGroup In groupOrder
            byGroup(sg) = New List(Of RegisterDef)()
        Next
        For Each r As RegisterDef In RegisterMap.GetRealTimeSignals()
            If _prefs.IsPanelVisible(r.ID) AndAlso byGroup.ContainsKey(r.Group) Then
                byGroup(r.Group).Add(r)
            End If
        Next

        Dim anyVisible As Boolean = False
        For Each sg As SignalGroup In groupOrder
            If byGroup(sg).Count > 0 Then anyVisible = True : Exit For
        Next
        If Not anyVisible Then
            Panel_ElecValScroll.ResumeLayout(False)
            GroupBox_ElecValues.ResumeLayout(True)
            Panel_ElecValScroll.Invalidate(True)
            Return
        End If

        Const CellH As Integer = 38
        Const SepH As Integer = 28
        Const Gap As Integer = 2
        Const Cols As Integer = 2
        Dim scrollBarW As Integer = SystemInformation.VerticalScrollBarWidth
        Dim panelW As Integer = Math.Max(200, Panel_ElecValScroll.ClientSize.Width - scrollBarW - 2)
        Dim cellW As Integer = panelW \ Cols

        ' Pre-calculer la hauteur totale
        Dim totalH As Integer = 4
        For Each sg As SignalGroup In groupOrder
            If byGroup(sg).Count = 0 Then Continue For
            totalH += SepH + Gap
            Dim rowsH As Integer = CInt(Math.Ceiling(byGroup(sg).Count / CDbl(Cols)))
            totalH += rowsH * (CellH + Gap)
        Next

        Dim container As New Panel()
        container.Name = "_dynPanel"
        container.Width = panelW
        container.Height = totalH
        container.BackColor = Drawing.Color.FromArgb(14, 22, 40)
        container.Location = New Point(0, 0)

        Dim yPos As Integer = 4

        For Each sg As SignalGroup In groupOrder
            Dim grpSignals As List(Of RegisterDef) = byGroup(sg)
            If grpSignals.Count = 0 Then Continue For

            ' Couleur et nom du groupe depuis AxisGroups
            ' Pour ePack : aucun AxisGroup dedié, on renvoie un nom/couleur fixe
            Dim grpColor As Drawing.Color = Drawing.Color.FromArgb(90, 115, 165)
            Dim grpName As String = sg.ToString()
            If sg = SignalGroup.ePack Then
                grpName = "ePack"
                grpColor = Drawing.Color.FromArgb(0, 200, 180)
            Else
                For Each ag As UserPreferences.AxisGroupDef In UserPreferences.AxisGroups
                    Dim found As Boolean = False
                    For Each mid As SignalID In ag.Members
                        If RegisterMap.HasDef(mid) AndAlso RegisterMap.GetDef(mid).Group = sg Then
                            grpColor = ag.Clr
                            grpName = ag.Name
                            found = True
                            Exit For
                        End If
                    Next
                    If found Then Exit For
                Next
            End If

            ' Separateur pleine largeur : nom groupe a gauche, All/None a droite
            Dim sep As New Panel()
            sep.Width = panelW - 2
            sep.Height = SepH
            sep.Location = New Point(1, yPos)
            sep.BackColor = Drawing.Color.FromArgb(20, 32, 54)
            Dim capturedColor As Drawing.Color = grpColor
            AddHandler sep.Paint, Sub(s2 As Object, ev2 As PaintEventArgs)
                                      Using pen As New Drawing.Pen(capturedColor, 1)
                                          ev2.Graphics.DrawLine(pen, 0, sep.Height - 1, sep.Width, sep.Height - 1)
                                      End Using
                                  End Sub

            Dim lblGrp As New Label()
            lblGrp.Text = grpName.ToUpper()
            lblGrp.Font = New Font("Segoe UI", 7.5, Drawing.FontStyle.Bold)
            lblGrp.ForeColor = grpColor
            lblGrp.Location = New Point(6, 4)
            lblGrp.AutoSize = True

            Dim capturedSg As SignalGroup = sg

            Dim btnNone As New Button()
            btnNone.Text = "None"
            btnNone.Size = New Size(38, 20)
            btnNone.Location = New Point(sep.Width - 42, 4)
            btnNone.FlatStyle = FlatStyle.Flat
            btnNone.FlatAppearance.BorderSize = 0
            btnNone.BackColor = Drawing.Color.FromArgb(28, 44, 70)
            btnNone.ForeColor = Drawing.Color.FromArgb(140, 165, 200)
            btnNone.Font = New Font("Segoe UI", 7.0, Drawing.FontStyle.Bold)
            AddHandler btnNone.Click, Sub(s2 As Object, ev2 As EventArgs)
                                          SetGroupChartEnabled(capturedSg, False)
                                      End Sub

            Dim btnAll As New Button()
            btnAll.Text = "All"
            btnAll.Size = New Size(32, 20)
            btnAll.Location = New Point(sep.Width - 78, 4)
            btnAll.FlatStyle = FlatStyle.Flat
            btnAll.FlatAppearance.BorderSize = 0
            btnAll.BackColor = Drawing.Color.FromArgb(28, 44, 70)
            btnAll.ForeColor = Drawing.Color.FromArgb(140, 165, 200)
            btnAll.Font = New Font("Segoe UI", 7.0, Drawing.FontStyle.Bold)
            AddHandler btnAll.Click, Sub(s2 As Object, ev2 As EventArgs)
                                         SetGroupChartEnabled(capturedSg, True)
                                     End Sub

            sep.Controls.AddRange(New Control() {lblGrp, btnAll, btnNone})
            container.Controls.Add(sep)
            yPos += SepH + Gap

            ' Cases du groupe en 2 colonnes
            For idx As Integer = 0 To grpSignals.Count - 1
                Dim r As RegisterDef = grpSignals(idx)
                Dim col As Integer = idx Mod Cols
                Dim xCell As Integer = col * cellW + 2
                Dim yCell As Integer = yPos + (idx \ Cols) * (CellH + Gap)

                Dim cell As New Panel()
                cell.Width = cellW - 4
                cell.Height = CellH
                cell.Location = New Point(xCell, yCell)
                cell.BackColor = ColorDefaultBackground
                cell.Tag = r.ID

                Dim lblName As New Label()
                lblName.Text = r.Name
                lblName.Font = New Font("Segoe UI", 7, Drawing.FontStyle.Bold)
                lblName.ForeColor = r.PlotColor
                lblName.Location = New Point(4, 2)
                lblName.Size = New Size(cell.Width - 24, 14)

                Dim lblVal As New Label()
                lblVal.Text = "-- " & r.Unit
                lblVal.Font = New Font("Consolas", 9, Drawing.FontStyle.Bold)
                lblVal.ForeColor = Drawing.Color.FromArgb(210, 230, 255)
                lblVal.Location = New Point(4, 18)
                lblVal.Size = New Size(cell.Width - 24, 17)
                lblVal.Tag = r.ID

                Dim chk As New CheckBox()
                chk.Text = ""
                chk.Size = New Size(15, 15)
                chk.Location = New Point(cell.Width - 18, 2)
                chk.Checked = _prefs.IsChartEnabled(r.ID)
                chk.ForeColor = Drawing.Color.FromArgb(80, 110, 150)
                chk.BackColor = Drawing.Color.Transparent
                chk.Tag = r.ID

                Dim capturedId As SignalID = r.ID
                AddHandler chk.CheckedChanged, Sub(s2 As Object, ev2 As EventArgs)
                                                   If Not _suppressComboEvents Then
                                                       _prefs.SetEnabled(capturedId, chk.Checked)
                                                       If _isChartReady Then Main_Chart.SetPlotEnabled(capturedId, chk.Checked)
                                                       _prefs.Save()
                                                   End If
                                               End Sub

                cell.Controls.AddRange(New Control() {lblName, lblVal, chk})
                _dynValueLabels(r.ID) = lblVal
                _dynChartChecks(r.ID) = chk
                container.Controls.Add(cell)
            Next

            ' Avancer yPos apres le groupe (retour colonne gauche garanti)
            Dim grpRows As Integer = CInt(Math.Ceiling(grpSignals.Count / CDbl(Cols)))
            yPos += grpRows * (CellH + Gap)
        Next

        Panel_ElecValScroll.Controls.Add(container)
        Panel_ElecValScroll.ResumeLayout(False)
        Panel_ElecValScroll.AutoScrollPosition = New System.Drawing.Point(0, 0)
        Panel_ElecValScroll.PerformLayout()
        GroupBox_ElecValues.ResumeLayout(True)
        Panel_ElecValScroll.Invalidate(True)
        Panel_ElecValScroll.Refresh()
        GroupBox_ElecValues.Invalidate(True)
        GroupBox_ElecValues.Refresh()

        UpdateAxisGroupHighlight()
    End Sub

    ' Coche ou decoche toutes les checkboxes d un groupe (appele par All/None)
    Private Sub SetGroupChartEnabled(sg As SignalGroup, enabled As Boolean)
        _suppressComboEvents = True
        For Each kvp As KeyValuePair(Of SignalID, CheckBox) In _dynChartChecks
            If RegisterMap.HasDef(kvp.Key) AndAlso RegisterMap.GetDef(kvp.Key).Group = sg Then
                kvp.Value.Checked = enabled
                _prefs.SetEnabled(kvp.Key, enabled)
                If _isChartReady Then Main_Chart.SetPlotEnabled(kvp.Key, enabled)
            End If
        Next
        _suppressComboEvents = False
        _prefs.Save()
    End Sub


    ' UPDATE DISPLAY VALUES  (called each Timer_Sample tick)
    Friend Sub UpdateDisplayValues(values As Dictionary(Of SignalID, Double))
        For Each kvp As KeyValuePair(Of SignalID, Label) In _dynValueLabels
            Dim id As SignalID = kvp.Key
            Dim lbl As Label = kvp.Value
            If Not values.ContainsKey(id) Then lbl.Text = "ERR" : Continue For
            Dim v As Double = values(id)
            Dim unit As String = RegisterMap.GetDef(id).Unit
            Dim fmt As String
            Select Case id
                Case SignalID.I, SignalID.I1, SignalID.I2, SignalID.I3, SignalID.I_Neutral
                    fmt = v.ToString("0.000")
                Case SignalID.U, SignalID.U12, SignalID.U23, SignalID.U31,
                     SignalID.U1N, SignalID.U2N, SignalID.U3N
                    fmt = v.ToString("0.00")
                Case SignalID.P, SignalID.Q, SignalID.S,
                     SignalID.P1, SignalID.P2, SignalID.P3,
                     SignalID.Q1, SignalID.Q2, SignalID.Q3,
                     SignalID.S1, SignalID.S2, SignalID.S3
                    fmt = v.ToString("0")
                Case SignalID.PF, SignalID.PF1, SignalID.PF2, SignalID.PF3
                    fmt = v.ToString("0.000")
                Case SignalID.F
                    fmt = v.ToString("0.00")
                Case SignalID.Phase_Dir
                    fmt = If(v > 0, "LEAD", "LAG")
                    unit = ""
                Case SignalID.THDi1 To SignalID.THDu3N
                    fmt = v.ToString("0.0")
                Case Else
                    fmt = v.ToString("0.00")
            End Select
            lbl.Text = If(unit <> "", fmt & " " & unit, fmt)
        Next
    End Sub

    ' Y1 / Y2 AXIS GROUP SELECTION
    ' These combos are created inside InitRuntimeControls (HD.vb).
    ' They live in a small panel above the ElecValues GroupBox title area.
    Friend Sub InitY1Y2Combos(containerPanel As Panel)
        Dim clrBg As Drawing.Color = Drawing.Color.FromArgb(26, 36, 76)
        Dim clrText As Drawing.Color = Drawing.Color.FromArgb(200, 220, 255)
        Dim clrY1 As Drawing.Color = Drawing.Color.FromArgb(255, 255, 255)
        Dim clrY2 As Drawing.Color = Drawing.Color.FromArgb(255, 255, 255)
        Dim panelW As Integer = containerPanel.Width

        ' ── Y1 label (colored) ───────────────────────────────────────────
        Dim lblY1 As New Label()
        lblY1.Text = "Y1"
        lblY1.Font = New Font("Segoe UI", 8, Drawing.FontStyle.Bold)
        lblY1.ForeColor = clrY1
        lblY1.Location = New Point(2, 5)
        lblY1.Size = New Size(20, 18)
        lblY1.TextAlign = Drawing.ContentAlignment.MiddleCenter

        ' ── Y1 ComboBox ───────────────────────────────────────────────────
        _comboY1 = New ComboBox()
        _comboY1.DropDownStyle = ComboBoxStyle.DropDownList
        _comboY1.DrawMode = DrawMode.OwnerDrawFixed
        _comboY1.ItemHeight = 18
        _comboY1.BackColor = clrBg
        _comboY1.ForeColor = clrText
        _comboY1.Font = New Font("Segoe UI", 8, Drawing.FontStyle.Regular)
        _comboY1.Location = New Point(24, 2)
        _comboY1.Size = New Size(panelW - 26, 22)
        _comboY1.Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top
        _comboY1.FlatStyle = FlatStyle.Flat

        ' ── Y2 label (colored) ───────────────────────────────────────────
        Dim lblY2 As New Label()
        lblY2.Text = "Y2"
        lblY2.Font = New Font("Segoe UI", 8, Drawing.FontStyle.Bold)
        lblY2.ForeColor = clrY2
        lblY2.Location = New Point(2, 28)
        lblY2.Size = New Size(20, 18)
        lblY2.TextAlign = Drawing.ContentAlignment.MiddleCenter

        ' ── Y2 ComboBox ───────────────────────────────────────────────────
        _comboY2 = New ComboBox()
        _comboY2.DropDownStyle = ComboBoxStyle.DropDownList
        _comboY2.DrawMode = DrawMode.OwnerDrawFixed
        _comboY2.ItemHeight = 18
        _comboY2.BackColor = clrBg
        _comboY2.ForeColor = clrText
        _comboY2.Font = New Font("Segoe UI", 8, Drawing.FontStyle.Regular)
        _comboY2.Location = New Point(24, 25)
        _comboY2.Size = New Size(panelW - 26, 22)
        _comboY2.Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top
        _comboY2.FlatStyle = FlatStyle.Flat

        ' ── Fill ComboBox with axis groups ────────────────────────────────
        For Each group As UserPreferences.AxisGroupDef In UserPreferences.AxisGroups
            _comboY1.Items.Add(group.Name)
            _comboY2.Items.Add(group.Name)
        Next

        ' ── DrawItem handlers — Item avec son AxisGroup color ────
        AddHandler _comboY1.DrawItem, Sub(s As Object, ev As DrawItemEventArgs)
                                          If ev.Index < 0 Then Return
                                          ev.DrawBackground()
                                          Dim group As UserPreferences.AxisGroupDef = UserPreferences.AxisGroups(ev.Index)
                                          Dim clr As Drawing.Color = group.Clr
                                          Dim bg As Drawing.Color = If((ev.State And DrawItemState.Selected) = DrawItemState.Selected,
                                              Drawing.Color.FromArgb(30, 55, 95), Drawing.Color.FromArgb(16, 26, 46))
                                          Using brushBg As New Drawing.SolidBrush(bg)
                                              ev.Graphics.FillRectangle(brushBg, ev.Bounds)
                                          End Using
                                          Using brFg As New Drawing.SolidBrush(clr)
                                              ev.Graphics.DrawString(group.Name, ev.Font, brFg,
                                                  New Drawing.RectangleF(ev.Bounds.X + 3, ev.Bounds.Y + 1,
                                                                         ev.Bounds.Width - 3, ev.Bounds.Height))
                                          End Using
                                      End Sub

        AddHandler _comboY2.DrawItem, Sub(s As Object, ev As DrawItemEventArgs)
                                          If ev.Index < 0 Then Return
                                          ev.DrawBackground()
                                          Dim group As UserPreferences.AxisGroupDef = UserPreferences.AxisGroups(ev.Index)
                                          Dim clr As Drawing.Color = group.Clr
                                          Dim bg As Drawing.Color = If((ev.State And DrawItemState.Selected) = DrawItemState.Selected,
                                              Drawing.Color.FromArgb(30, 55, 95), Drawing.Color.FromArgb(16, 26, 46))
                                          Using brushBg As New Drawing.SolidBrush(bg)
                                              ev.Graphics.FillRectangle(brushBg, ev.Bounds)
                                          End Using
                                          Using brFg As New Drawing.SolidBrush(clr)
                                              ev.Graphics.DrawString(group.Name, ev.Font, brFg,
                                                  New Drawing.RectangleF(ev.Bounds.X + 3, ev.Bounds.Y + 1,
                                                                         ev.Bounds.Width - 3, ev.Bounds.Height))
                                          End Using
                                      End Sub

        AddHandler _comboY1.SelectedIndexChanged, AddressOf ComboY1_Changed
        AddHandler _comboY2.SelectedIndexChanged, AddressOf ComboY2_Changed

        containerPanel.Controls.AddRange(New Control() {lblY1, _comboY1, lblY2, _comboY2})
        SyncY1Y2CombosFromPrefs()
    End Sub

    Friend Sub SyncY1Y2CombosFromPrefs()
        If _comboY1 Is Nothing OrElse _comboY2 Is Nothing Then Return
        _suppressComboEvents = True
        _comboY1.SelectedIndex = Math.Max(0, Math.Min(_prefs.Y1GroupIndex, _comboY1.Items.Count - 1))
        _comboY2.SelectedIndex = Math.Max(0, Math.Min(_prefs.Y2GroupIndex, _comboY2.Items.Count - 1))
        _suppressComboEvents = False
    End Sub

    Private Sub ComboY1_Changed(sender As Object, e As EventArgs)
        If _suppressComboEvents OrElse _comboY1 Is Nothing Then Return
        _prefs.Y1GroupIndex = _comboY1.SelectedIndex
        _prefs.Save()
        If _isChartReady Then
            Main_Chart.SetAxisGroups(_prefs.Y1GroupIndex, _prefs.Y2GroupIndex)
        End If
        UpdateAxisGroupHighlight()
    End Sub

    Private Sub ComboY2_Changed(sender As Object, e As EventArgs)
        If _suppressComboEvents OrElse _comboY2 Is Nothing Then Return
        _prefs.Y2GroupIndex = _comboY2.SelectedIndex
        _prefs.Save()
        If _isChartReady Then
            Main_Chart.SetAxisGroups(_prefs.Y1GroupIndex, _prefs.Y2GroupIndex)
        End If
        UpdateAxisGroupHighlight()
    End Sub


    ' Highlight cells whose signal belongs to Y1 or Y2 group
    Friend Sub UpdateAxisGroupHighlight()
        Dim container As Control = Panel_ElecValScroll.Controls("_dynPanel")
        If container Is Nothing Then Return

        Dim y1g As Integer = _prefs.Y1GroupIndex
        Dim y2g As Integer = _prefs.Y2GroupIndex

        For Each cell As Control In container.Controls
            If TypeOf cell Is Panel AndAlso cell.Tag IsNot Nothing Then
                Dim id As SignalID = CType(cell.Tag, SignalID)
                Dim grp As Integer = _prefs.GetGroupIdxForSignal(id)
                If grp = y1g Then
                    cell.BackColor = ColorY1Background
                ElseIf grp = y2g Then
                    cell.BackColor = ColorY2Background
                Else
                    cell.BackColor = ColorDefaultBackground
                End If
            End If
        Next
    End Sub

    ' CHART SIGNAL STATE SYNC
    Friend Sub ApplyAllChartSignalStates()
        For Each kvp As KeyValuePair(Of SignalID, CheckBox) In _dynChartChecks
            Main_Chart.SetPlotEnabled(kvp.Key, _prefs.IsChartEnabled(kvp.Key))
        Next
    End Sub

    ' TIMER_SAMPLE — ACQUISITION
    Private Sub Timer_Sample_Tick(sender As Object, e As EventArgs) Handles Timer_Sample.Tick
        Label_SampleRate_Title.ForeColor = Drawing.Color.FromArgb(0, 210, 180)
        Timer_Flash.Start()

        If Not IsConnected() AndAlso Not ModbusTcpMaster.modbustcpisconnected AndAlso Not IsDemoMode Then
            If Not _lossAlreadyHandled Then
                _lossAlreadyHandled = True
                Main_Chart.PauseStopwatch()
                Timer_Sample.Stop()
                UpdateSamplingButton()
                ConnectionWindow.DisconnectUI()
                MessageBox.Show("Connection lost with the device.", "Disconnected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Me.Close()
            End If
            Return
        End If
        _lossAlreadyHandled = False

        Dim values As Dictionary(Of SignalID, Double) = ModbusRsMaster.ReadAllValues()

        ' Si la lecture RS renvoie Nothing, erreur de connexion -> stopper le timer
        If values Is Nothing Then
            Timer_Sample.Stop()
            UpdateSamplingButton()
            MessageBox.Show("Modbus RS read error. Sampling stopped.", "Read Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Fusionner les valeurs ePack TCP si connecte
        If ModbusTcpMaster.modbustcpisconnected Then
            ModbusTcpMaster.Read_epackTCP(False)
            ' Si Read_epackTCP a detecte une erreur, modbustcpisconnected est mis a False
            If Not ModbusTcpMaster.modbustcpisconnected Then
                RegisterMap.Refresh()
                Timer_Sample.Stop()
                UpdateSamplingButton()
                MessageBox.Show("ePack TCP connection lost. Sampling stopped.", "TCP Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            Dim epackRaw As Dictionary(Of SignalID, Double) = ModbusTcpMaster.GetEpackValues()
            For Each kvp As KeyValuePair(Of SignalID, Double) In epackRaw
                If RegisterMap.HasDef(kvp.Key) Then
                    Dim sf As Double = RegisterMap.GetDef(kvp.Key).ScaleFactor
                    values(kvp.Key) = If(sf <> 0.0, kvp.Value / sf, kvp.Value)
                End If
            Next
        End If

        _latestReadings = values

        ' Update Electircal values panel 
        UpdateDisplayValues(values)

        ' Read harmonics on every sample tick
        ReadAndRefreshHD()

        ' Update Chart (only values selected in panels)
        If Not _isChartFrozen Then
            For Each kvp As KeyValuePair(Of SignalID, Double) In values
                If _prefs.IsChartEnabled(kvp.Key) Then
                    Main_Chart.SetSignalValue(kvp.Key, kvp.Value)
                End If
            Next
            Main_Chart.UpdateChartDraw()
        End If

        ' CSV Electrical Value Export
        If _isRecording AndAlso Not _csvPaused Then WriteCsvLine()

        ' CSV HD export
        If _isRecording AndAlso Not _csvPaused AndAlso _prefs.HdExportEnabled Then
            WriteHdCsvLines()
        End If

    End Sub

    Private Sub Timer_Flash_Tick(sender As Object, e As EventArgs) Handles Timer_Flash.Tick
        Timer_Flash.Stop()
        Label_SampleRate_Title.ForeColor = Drawing.Color.FromArgb(140, 165, 200)
    End Sub

    ' FOOTER — SAMPLE RATE TRACKBAR
    Private Sub TrackBar_Sample_Scroll(sender As Object, e As EventArgs) Handles TrackBar_Sample.Scroll
        Dim interval As Integer = TrackBar_Sample.Value * 100
        Timer_Sample.Interval = interval
        _prefs.SampleInterval = interval
        _prefs.Save()
        UpdateSampleLabel()
    End Sub

    Friend Sub UpdateSampleLabel()
        Label_SampleRate_Value.Text = (TrackBar_Sample.Value * 100).ToString() & "ms"
    End Sub

    ' FOOTER — ACQUISITION BUTTON
    Private Sub Button_SamplingPause_Click(sender As Object, e As EventArgs) Handles Button_Sampling.Click
        If Timer_Sample.Enabled Then
            ' PAUSE : tout arreter
            Timer_Sample.Stop()
            Main_Chart.PauseStopwatch()
            _csvPaused = True
            _isChartFrozen = True
        Else
            ' RESUME : tout relancer
            Timer_Sample.Start()
            Main_Chart.StartStopwatch()
            _csvPaused = False
            _isChartFrozen = False
        End If
        UpdateSamplingButton()
        UpdateRecordButton()
    End Sub

    Private _pauseLabel As Label = Nothing

    Friend Sub UpdateSamplingButton()
        If Timer_Sample.Enabled Then
            Button_Sampling.BackColor = Drawing.Color.FromArgb(0, 105, 145)
            Button_Sampling.ForeColor = Drawing.Color.White
            Button_Sampling.Text = "⏹ STOP"
            If _pauseLabel IsNot Nothing Then _pauseLabel.Visible = False
        Else
            Button_Sampling.BackColor = Drawing.Color.FromArgb(28, 105, 65)
            Button_Sampling.ForeColor = Drawing.Color.FromArgb(190, 245, 215)
            Button_Sampling.Text = "▶ START"
            If _pauseLabel Is Nothing Then
                _pauseLabel = New Label()
                _pauseLabel.Text = "⏸"
                _pauseLabel.Font = New Drawing.Font("Segoe UI", 72, Drawing.FontStyle.Bold)
                _pauseLabel.ForeColor = Drawing.Color.FromArgb(160, 255, 255, 255)
                _pauseLabel.BackColor = Drawing.Color.Transparent
                _pauseLabel.AutoSize = True
                Main_Chart.Controls.Add(_pauseLabel)
            End If
            _pauseLabel.Visible = True
            _pauseLabel.Location = New Point(
            (Main_Chart.Width - _pauseLabel.Width) \ 2,
            (Main_Chart.Height - _pauseLabel.Height) \ 2)
            _pauseLabel.BringToFront()
        End If
    End Sub

    ''OLD

    '' FOOTER — CHART FREEZE / RESET / PREFERENCES
    'Private Sub Button_ChartPause_Click(sender As Object, e As EventArgs) Handles Button_ChartPause.Click
    '    If Panel_HD.Visible Then
    '        ' HD view: toggle HD freeze
    '        _isHdFrozen = Not _isHdFrozen
    '    Else
    '        ' Live view: toggle chart freeze
    '        _isChartFrozen = Not _isChartFrozen
    '        If _isChartFrozen Then
    '            Main_Chart.PauseStopwatch()
    '        ElseIf Timer_Sample.Enabled Then
    '            Main_Chart.StartStopwatch()
    '        End If
    '    End If
    '    UpdateFreezePauseButton()
    'End Sub

    'Friend Sub UpdateChartPauseButton()
    '    UpdateFreezePauseButton()
    'End Sub

    'Friend Sub UpdateFreezePauseButton()

    '    If _isHdFrozen Then
    '        Button_ChartPause.BackColor = Drawing.Color.FromArgb(110, 22, 22)
    '        Button_ChartPause.ForeColor = Drawing.Color.FromArgb(255, 180, 180)
    '        Button_ChartPause.Text = "▶ HD"
    '    Else
    '        Button_ChartPause.BackColor = Drawing.Color.FromArgb(14, 72, 148)
    '        Button_ChartPause.ForeColor = Drawing.Color.FromArgb(170, 215, 255)
    '        Button_ChartPause.Text = "❄ FREEZE"
    '    End If

    '    If _isChartFrozen Then
    '        Button_ChartPause.BackColor = Drawing.Color.FromArgb(110, 22, 22)
    '        Button_ChartPause.ForeColor = Drawing.Color.FromArgb(255, 180, 180)
    '        Button_ChartPause.Text = "▶ LIVE"
    '    Else
    '        Button_ChartPause.BackColor = Drawing.Color.FromArgb(14, 72, 148)
    '        Button_ChartPause.ForeColor = Drawing.Color.FromArgb(170, 215, 255)
    '        Button_ChartPause.Text = "❄ FREEZE"
    '    End If
    'End Sub

    Private Sub Button_ChartReset_Click(sender As Object, e As EventArgs) Handles Button_ChartReset.Click
        ChartReset()
    End Sub

    Public Sub ChartReset()
        Main_Chart.PauseStopwatch()
        Main_Chart.ResetStopwatch()
        Main_Chart.ResetChart()
        _isChartReady = True : _isChartFrozen = False
        If Timer_Sample.Enabled Then Main_Chart.StartStopwatch()
        'UpdateChartPauseButton()
        Main_Chart.SetAxisGroups(_prefs.Y1GroupIndex, _prefs.Y2GroupIndex)
        ApplyAllChartSignalStates()
        UpdateAxisGroupHighlight()
        UpdateSamplingButton()
    End Sub

    Private Sub Button_Settings_Click(sender As Object, e As EventArgs) Handles Button_Settings.Click
        Using PrefForm As New PreferencesForm(AddressOf OnPreferencesApplied)
            PrefForm.ShowDialog(Me)
        End Using
    End Sub

End Class