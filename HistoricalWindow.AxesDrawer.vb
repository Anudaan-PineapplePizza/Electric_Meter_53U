Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports System.Globalization

' =============================================================================
' HistoricalWindow.AxesDrawer.vb  -- Partial Class (7/7)
' =============================================================================
Partial Class HistoricalWindow

#Region "Champs drawer"

    Private _drawerOpen As Boolean = False
    Private _drawerTimer As Timer = Nothing
    Private Const DrawerW As Integer = 320

    Friend _sessionYBounds As New Dictionary(Of String, (yMin As Double, yMax As Double))()
    Friend _sessionTMin As Double = Double.NaN
    Friend _sessionTMax As Double = Double.NaN

    Private _axesDgv As DataGridView = Nothing

    Private Const KEY_TIME As String = "TIME"

#End Region

#Region "Couleurs par onglet"

    Friend Shared Function GetTabColor(tabKey As String) As Drawing.Color
        Select Case tabKey
            Case "Current (A)" : Return Drawing.Color.FromArgb(0, 220, 150)
            Case "Voltage L-L (V)" : Return Drawing.Color.DodgerBlue
            Case "Voltage L-N (V)" : Return Drawing.Color.FromArgb(100, 180, 255)
            Case "Active Power (W)" : Return Drawing.Color.FromArgb(255, 185, 50)
            Case "Reactive Power (VAR)" : Return Drawing.Color.FromArgb(255, 100, 50)
            Case "Apparent Power (VA)" : Return Drawing.Color.FromArgb(200, 130, 255)
            Case "All Powers" : Return Drawing.Color.FromArgb(255, 160, 80)
            Case "Power Factor (PF)" : Return Drawing.Color.FromArgb(255, 110, 110)
            Case "Frequency (Hz)" : Return Drawing.Color.FromArgb(140, 200, 255)
            Case "THD Current (%)" : Return Drawing.Color.FromArgb(0, 220, 120)
            Case "THD Voltage (%)" : Return Drawing.Color.FromArgb(80, 160, 255)
            Case Else : Return Drawing.Color.FromArgb(140, 165, 200)
        End Select
    End Function

#End Region

#Region "Init drawer"

    Friend Sub InitAxesDrawer()
        Panel_AxesDrawer.SuspendLayout()
        Panel_AxesDrawer.Controls.Clear()

        Dim clrBg As Drawing.Color = Drawing.Color.FromArgb(22, 32, 55)
        Dim clrHdr As Drawing.Color = Drawing.Color.FromArgb(18, 28, 50)
        Dim clrText As Drawing.Color = Drawing.Color.FromArgb(200, 215, 240)
        Dim clrSub As Drawing.Color = Drawing.Color.FromArgb(90, 115, 165)
        Dim clrBorder As Drawing.Color = Drawing.Color.FromArgb(55, 85, 145)

        Panel_AxesDrawer.BackColor = clrBg
        AddHandler Panel_AxesDrawer.Paint, Sub(s As Object, ev As PaintEventArgs)
                                               Using pen As New Drawing.Pen(clrBorder, 1)
                                                   ev.Graphics.DrawRectangle(pen, 0, 0,
                                                       Panel_AxesDrawer.Width - 1,
                                                       Panel_AxesDrawer.Height - 1)
                                               End Using
                                           End Sub

        Dim pTitle As New Panel()
        pTitle.BackColor = clrHdr
        pTitle.Dock = DockStyle.Top
        pTitle.Height = 34
        Panel_AxesDrawer.Controls.Add(pTitle)

        Dim lbl As New Label()
        lbl.Text = "AXES SETTINGS"
        lbl.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        lbl.ForeColor = clrText
        lbl.Location = New Point(12, 8)
        lbl.AutoSize = True
        pTitle.Controls.Add(lbl)

        Dim btnClose As New Button()
        btnClose.Text = "X"
        btnClose.Size = New Size(26, 26)
        btnClose.Location = New Point(pTitle.Width - 30, 4)
        btnClose.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnClose.FlatStyle = FlatStyle.Flat
        btnClose.FlatAppearance.BorderSize = 0
        btnClose.BackColor = Drawing.Color.FromArgb(70, 30, 30)
        btnClose.ForeColor = Drawing.Color.FromArgb(230, 140, 140)
        btnClose.Font = New Font("Segoe UI", 8, FontStyle.Bold)
        btnClose.TextAlign = Drawing.ContentAlignment.MiddleCenter
        AddHandler btnClose.Click, Sub(s As Object, e As EventArgs) CloseAxesDrawer()
        pTitle.Controls.Add(btnClose)
        btnClose.BringToFront()

        _axesDgv = New DataGridView()
        _axesDgv.BackgroundColor = Drawing.Color.FromArgb(14, 20, 36)
        _axesDgv.BorderStyle = BorderStyle.None
        _axesDgv.GridColor = Drawing.Color.FromArgb(35, 50, 80)
        _axesDgv.RowHeadersVisible = False
        _axesDgv.AllowUserToAddRows = False
        _axesDgv.AllowUserToDeleteRows = False
        _axesDgv.AllowUserToResizeRows = False
        _axesDgv.SelectionMode = DataGridViewSelectionMode.CellSelect
        _axesDgv.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or
                          AnchorStyles.Left Or AnchorStyles.Right
        _axesDgv.Location = New Point(6, 40)
        _axesDgv.Size = New Size(Panel_AxesDrawer.Width - 12, Panel_AxesDrawer.Height - 86)
        _axesDgv.ColumnHeadersHeight = 24
        _axesDgv.RowTemplate.Height = 28
        _axesDgv.ColumnHeadersHeightSizeMode =
            DataGridViewColumnHeadersHeightSizeMode.DisableResizing

        Dim hStyle As New DataGridViewCellStyle()
        hStyle.BackColor = clrHdr
        hStyle.ForeColor = clrSub
        hStyle.Font = New Font("Segoe UI", 7.5, FontStyle.Bold)
        hStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        _axesDgv.ColumnHeadersDefaultCellStyle = hStyle

        Dim cStyle As New DataGridViewCellStyle()
        cStyle.BackColor = Drawing.Color.FromArgb(18, 28, 50)
        cStyle.ForeColor = clrText
        cStyle.Font = New Font("Segoe UI", 8.5)
        cStyle.SelectionBackColor = Drawing.Color.FromArgb(35, 55, 100)
        cStyle.SelectionForeColor = Drawing.Color.White
        _axesDgv.DefaultCellStyle = cStyle

        ' Colonne Group (lecture seule)
        Dim cGrp As New DataGridViewTextBoxColumn()
        cGrp.HeaderText = "Group"
        cGrp.Name = "ACol_Group"
        cGrp.Width = 120
        cGrp.ReadOnly = True
        cGrp.SortMode = DataGridViewColumnSortMode.NotSortable
        _axesDgv.Columns.Add(cGrp)

        ' Colonne Min (Start pour le temps)
        Dim cMin As New DataGridViewTextBoxColumn()
        cMin.HeaderText = "Min / Start"
        cMin.Name = "ACol_Min"
        cMin.Width = 70
        cMin.SortMode = DataGridViewColumnSortMode.NotSortable
        Dim csMin As New DataGridViewCellStyle()
        csMin.Alignment = DataGridViewContentAlignment.MiddleRight
        csMin.ForeColor = Drawing.Color.FromArgb(255, 160, 80)
        cMin.DefaultCellStyle = csMin
        _axesDgv.Columns.Add(cMin)

        ' Colonne Max (End pour le temps)
        Dim cMax As New DataGridViewTextBoxColumn()
        cMax.HeaderText = "Max / End"
        cMax.Name = "ACol_Max"
        cMax.Width = 70
        cMax.SortMode = DataGridViewColumnSortMode.NotSortable
        Dim csMax As New DataGridViewCellStyle()
        csMax.Alignment = DataGridViewContentAlignment.MiddleRight
        csMax.ForeColor = Drawing.Color.FromArgb(80, 200, 120)
        cMax.DefaultCellStyle = csMax
        _axesDgv.Columns.Add(cMax)

        Panel_AxesDrawer.Controls.Add(_axesDgv)

        Dim btnApply As New Button()
        btnApply.Text = "Apply"
        btnApply.Size = New Size(80, 28)
        btnApply.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnApply.Location = New Point(Panel_AxesDrawer.Width - 90, Panel_AxesDrawer.Height - 36)
        btnApply.FlatStyle = FlatStyle.Flat
        btnApply.FlatAppearance.BorderSize = 0
        btnApply.BackColor = Drawing.Color.FromArgb(0, 100, 60)
        btnApply.ForeColor = Drawing.Color.White
        btnApply.Font = New Font("Segoe UI", 8.5, FontStyle.Bold)
        AddHandler btnApply.Click, Sub(s As Object, e As EventArgs) ApplyAxesChanges()
        Panel_AxesDrawer.Controls.Add(btnApply)

        Dim btnReset As New Button()
        btnReset.Text = "Reset"
        btnReset.Size = New Size(70, 28)
        btnReset.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btnReset.Location = New Point(8, Panel_AxesDrawer.Height - 36)
        btnReset.FlatStyle = FlatStyle.Flat
        btnReset.FlatAppearance.BorderSize = 0
        btnReset.BackColor = Drawing.Color.FromArgb(30, 44, 75)
        btnReset.ForeColor = clrText
        btnReset.Font = New Font("Segoe UI", 8.5, FontStyle.Bold)
        AddHandler btnReset.Click, Sub(s As Object, e As EventArgs) ResetAxesToPrefs()
        Panel_AxesDrawer.Controls.Add(btnReset)

        Panel_AxesDrawer.ResumeLayout(True)
    End Sub

    Friend Sub PopulateAxesDgv()
        If _axesDgv Is Nothing OrElse _csvData Is Nothing Then Return
        Dim ci As CultureInfo = CultureInfo.InvariantCulture
        _axesDgv.Rows.Clear()

        ' ── Ligne Time Range en premier ──────────────────────────────────
        Dim tDataMin As Double = If(_csvData.TimeData.Length > 0, _csvData.TimeData(0), 0.0)
        Dim tDataMax As Double = If(_csvData.TimeData.Length > 0,
            _csvData.TimeData(_csvData.TimeData.Length - 1), 1.0)
        Dim tStart As Double = If(Not Double.IsNaN(_sessionTMin), _sessionTMin, tDataMin)
        Dim tEnd As Double = If(Not Double.IsNaN(_sessionTMax), _sessionTMax, tDataMax)

        Dim riT As Integer = _axesDgv.Rows.Add()
        _axesDgv.Rows(riT).Cells("ACol_Group").Value = KEY_TIME
        _axesDgv.Rows(riT).Cells("ACol_Min").Value = tStart.ToString("G6", ci)
        _axesDgv.Rows(riT).Cells("ACol_Max").Value = tEnd.ToString("G6", ci)
        _axesDgv.Rows(riT).Cells("ACol_Group").Style.ForeColor =
            Drawing.Color.FromArgb(220, 200, 80)
        _axesDgv.Rows(riT).Cells("ACol_Group").Style.Font =
            New Font("Segoe UI", 8, FontStyle.Bold)
        ' Afficher "Time Range" dans la cellule groupe (KEY_TIME est la cle interne)
        _axesDgv.Rows(riT).Cells("ACol_Group").Value = "Time Range (s)"
        ' Stocker la cle interne dans le Tag de la ligne
        _axesDgv.Rows(riT).Tag = KEY_TIME

        ' ── Lignes Y par groupe ────────────────────────────────────────────
        Dim seen As New HashSet(Of String)()
        For sigIdx As Integer = 0 To _csvData.Signals.Count - 1
            Dim key As String = GroupKeyForSignal(_csvData.Signals(sigIdx))
            If seen.Contains(key) Then Continue For
            seen.Add(key)

            Dim idxList As New List(Of Integer)()
            For k As Integer = 0 To _csvData.Signals.Count - 1
                If GroupKeyForSignal(_csvData.Signals(k)) = key Then idxList.Add(k)
            Next

            Dim bounds = If(_sessionYBounds.ContainsKey(key),
                _sessionYBounds(key),
                GetYBounds(idxList, _csvData.Signals))

            Dim grpColor As Drawing.Color = GetTabColor(key)
            Dim ri As Integer = _axesDgv.Rows.Add()
            _axesDgv.Rows(ri).Cells("ACol_Group").Value = key
            _axesDgv.Rows(ri).Cells("ACol_Min").Value = bounds.yMin.ToString("G6", ci)
            _axesDgv.Rows(ri).Cells("ACol_Max").Value = bounds.yMax.ToString("G6", ci)
            _axesDgv.Rows(ri).Cells("ACol_Group").Style.ForeColor = grpColor
            _axesDgv.Rows(ri).Cells("ACol_Group").Style.Font =
                New Font("Segoe UI", 8, FontStyle.Bold)
            _axesDgv.Rows(ri).Tag = key
            If ri Mod 2 = 1 Then
                _axesDgv.Rows(ri).DefaultCellStyle.BackColor =
                    Drawing.Color.FromArgb(26, 38, 65)
            End If
        Next

        ' All Powers (auto)
        Dim hasPower As Boolean = seen.Contains("Active Power (W)") OrElse
                                  seen.Contains("Reactive Power (VAR)") OrElse
                                  seen.Contains("Apparent Power (VA)")
        If hasPower AndAlso Not seen.Contains("All Powers") Then
            Dim ri As Integer = _axesDgv.Rows.Add()
            _axesDgv.Rows(ri).Cells("ACol_Group").Value = "All Powers"
            _axesDgv.Rows(ri).Cells("ACol_Min").Value = ""
            _axesDgv.Rows(ri).Cells("ACol_Max").Value = ""
            _axesDgv.Rows(ri).Cells("ACol_Group").Style.ForeColor = GetTabColor("All Powers")
            _axesDgv.Rows(ri).Cells("ACol_Group").Style.Font =
                New Font("Segoe UI", 8, FontStyle.Italic)
            _axesDgv.Rows(ri).Tag = "All Powers"
        End If
    End Sub

#End Region

#Region "Logique Apply / Reset"

    Private Sub ApplyAxesChanges()
        If _axesDgv Is Nothing OrElse _csvData Is Nothing Then Return
        Dim ci As CultureInfo = CultureInfo.InvariantCulture

        ' ── Sauvegarder l etat courant ────────────────────────────────────
        Dim savedTabName As String = If(TabControl_Charts.SelectedTab IsNot Nothing,
                                          TabControl_Charts.SelectedTab.Text, "")
        Dim savedIsCursor As Boolean = (Button_Cursor.BackColor =
                                        Drawing.Color.FromArgb(0, 90, 140))

        ' Snapshot des checkboxes : globalIdx -> checked
        Dim chkSnapshot As New Dictionary(Of Integer, Boolean)()
        For Each kvp As KeyValuePair(Of Integer, CheckBox) In _histChkBoxes
            chkSnapshot(kvp.Key) = kvp.Value.Checked
        Next

        ' ── Lire les nouvelles valeurs du DGV ────────────────────────────
        _sessionYBounds.Clear()
        _sessionTMin = Double.NaN
        _sessionTMax = Double.NaN

        For Each row As DataGridViewRow In _axesDgv.Rows
            Dim tag As String = TryCast(row.Tag, String)
            If String.IsNullOrEmpty(tag) Then Continue For

            Dim sMin As String = TryCast(row.Cells("ACol_Min").Value, String)
            Dim sMax As String = TryCast(row.Cells("ACol_Max").Value, String)

            If tag = KEY_TIME Then
                ' Plage temporelle
                Dim tS, tE As Double
                If Double.TryParse(sMin, NumberStyles.Any, ci, tS) Then _sessionTMin = tS
                If Double.TryParse(sMax, NumberStyles.Any, ci, tE) Then _sessionTMax = tE
            ElseIf tag <> "All Powers" Then
                ' Bornes Y
                Dim yMin, yMax As Double
                If Double.TryParse(sMin, NumberStyles.Any, ci, yMin) AndAlso
                   Double.TryParse(sMax, NumberStyles.Any, ci, yMax) AndAlso
                   yMax > yMin Then
                    _sessionYBounds(tag) = (yMin, yMax)
                End If
            End If
        Next

        ' ── Reconstruire les graphiques ───────────────────────────────────
        BuildChart(_csvData)
        Label_CursorTime.Text = "t = --"

        ' ── Restaurer l onglet selectionne ────────────────────────────────
        For Each tab As TabPage In TabControl_Charts.TabPages
            If tab.Text = savedTabName Then
                TabControl_Charts.SelectedTab = tab
                Exit For
            End If
        Next

        ' ── Restaurer le mode zoom/curseur ────────────────────────────────
        If savedIsCursor Then
            SetChartModeCursor()
        Else
            SetChartModeZoom()
        End If

        ' ── Restaurer les courbes masquees ────────────────────────────────
        For Each kvp As KeyValuePair(Of Integer, Boolean) In chkSnapshot
            If Not kvp.Value Then
                SetHistSignalVisible(kvp.Key, False)
                ' Synchroniser la checkbox dans le panel
                If _histChkBoxes.ContainsKey(kvp.Key) Then
                    _histChkBoxes(kvp.Key).Checked = False
                End If
            End If
        Next

        ' ── Fermer le drawer ──────────────────────────────────────────────
        CloseAxesDrawer()
    End Sub

    Private Sub ResetAxesToPrefs()
        _sessionYBounds.Clear()
        _sessionTMin = Double.NaN
        _sessionTMax = Double.NaN
        PopulateAxesDgv()
    End Sub

#End Region

#Region "Ouverture / fermeture animee"

    Private Sub Button_Axes_Click(sender As Object, e As EventArgs) Handles Button_Axes.Click
        If _drawerOpen Then
            CloseAxesDrawer()
        Else
            OpenAxesDrawer()
        End If
    End Sub

    Private Sub OpenAxesDrawer()
        If _axesDgv Is Nothing Then InitAxesDrawer()
        PopulateAxesDgv()

        Panel_AxesDrawer.Height = SplitMain.Height
        Panel_AxesDrawer.Top = SplitMain.Top
        Panel_AxesDrawer.Left = Me.ClientSize.Width
        Panel_AxesDrawer.Visible = True
        Panel_AxesDrawer.BringToFront()

        _drawerOpen = True
        Button_Axes.BackColor = Drawing.Color.FromArgb(0, 90, 140)
        StartDrawerAnimation(opening:=True)
    End Sub

    Friend Sub CloseAxesDrawer()
        _drawerOpen = False
        Button_Axes.BackColor = Drawing.Color.FromArgb(28, 44, 70)
        StartDrawerAnimation(opening:=False)
    End Sub

    Private Sub StartDrawerAnimation(opening As Boolean)
        If _drawerTimer IsNot Nothing Then
            _drawerTimer.Stop()
            _drawerTimer.Dispose()
        End If
        _drawerTimer = New Timer()
        _drawerTimer.Interval = 8
        AddHandler _drawerTimer.Tick, Sub(s As Object, ev As EventArgs)
                                          AnimateDrawerStep(opening)
                                      End Sub
        _drawerTimer.Start()
    End Sub

    Private Sub AnimateDrawerStep(opening As Boolean)
        Dim targetLeft As Integer = If(opening,
            Me.ClientSize.Width - DrawerW, Me.ClientSize.Width)
        Dim remaining As Integer = Math.Abs(Panel_AxesDrawer.Left - targetLeft)
        Dim slideStep As Integer = Math.Max(8, remaining \ 3)

        If opening Then
            If Panel_AxesDrawer.Left > targetLeft Then
                Panel_AxesDrawer.Left = Math.Max(targetLeft, Panel_AxesDrawer.Left - slideStep)
            Else
                Panel_AxesDrawer.Left = targetLeft
                _drawerTimer.Stop()
            End If
        Else
            If Panel_AxesDrawer.Left < Me.ClientSize.Width Then
                Panel_AxesDrawer.Left = Math.Min(Me.ClientSize.Width,
                    Panel_AxesDrawer.Left + slideStep)
            Else
                Panel_AxesDrawer.Visible = False
                _drawerTimer.Stop()
            End If
        End If
    End Sub

    Private Sub HistoricalWindow_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        If _drawerOpen AndAlso Panel_AxesDrawer.Visible Then
            Panel_AxesDrawer.Left = Me.ClientSize.Width - DrawerW
            Panel_AxesDrawer.Height = SplitMain.Height
            Panel_AxesDrawer.Top = SplitMain.Top
        End If
    End Sub

#End Region

End Class
' NOTE: Also add to HistoricalWindow.Chart.vb:
' 1. In BuildChart, replace the first 4 lines with:
'      Dim workData As CsvData = FilterDataByTime(data, _sessionTMin, _sessionTMax)
'      TabControl_Charts.TabPages.Clear()
'      ...
'      Dim tMin = workData.TimeData(0), tMax = workData.TimeData(last)
'    Then use 'workData' instead of 'data' in groups loop and AddChartTab calls
'
' 2. Add the FilterDataByTime function below (in the Partial Class HistoricalWindow region)