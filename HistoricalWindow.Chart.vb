Option Strict Off
Option Explicit On

Imports com.quinncurtis.chart2dnet
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Collections.Generic

' =============================================================================
' HistoricalWindow.Chart.vb  -- Partial Class (3/6)
'
' Structure identique a Historicals_ChartGases :
'   - HistChartPanel herite ChartView
'   - chartVu As ChartView = Me  (pas As Object)
'   - Me.GetChartObjectsArrayList.Clear() + Me.ResetMouseListeners() sur Me
'   - zoomObj et cursorObj crees mais PAS ajoutes via AddChartObject
'   - SetCurrentMouseListener(zoomObj/cursorObj) pour changer de mode
'   - CustomChartDataCursor identique a l exemple avec timer anti-bounce
' =============================================================================

Public Class HistChartPanel
    Inherits ChartView

    ' Exactement comme dans l exemple : As ChartView = Me
    Private chartVu As ChartView = Me

    Public Event CursorMoved(xPos As Double)

    Private Enum T_MOUSE_MODE
        MODE_ZOOM = 0
        MODE_CURSOR = 1
    End Enum
    Private MouseMode As T_MOUSE_MODE = T_MOUSE_MODE.MODE_ZOOM

    Private zoomObj As HistChartPanel.ZoomWithStack
    Private dataCursorObj As HistChartPanel.CustomChartDataCursor
    Private pTransform As CartesianCoordinates
    Private _plotList As New List(Of SimpleLinePlot)()

    Public Sub New()
        MyBase.New()
        Me.BackColor = Drawing.Color.FromArgb(12, 18, 32)
    End Sub

    ' ── InitializeChart — structure identique a l exemple ────────────────────
    Public Sub InitializeChart(tMin As Double, tMax As Double,
                               yMin As Double, yMax As Double,
                               unitLabel As String,
                               timeData As Double(),
                               signals As List(Of HistoricalWindow.CsvSignal),
                               sigIndices As List(Of Integer),
                               Optional tabColor As Drawing.Color = Nothing)
        ' Note: Drawing.Color est un struct, Nothing == Color.Empty
        Try
            ' Comme dans l exemple : Clear + ResetMouseListeners sur Me
            Me.GetChartObjectsArrayList.Clear()
            Me.ResetMouseListeners()
            _plotList.Clear()

            Dim axFont As New Drawing.Font("Segoe UI", 7.5, Drawing.FontStyle.Regular)
            Dim ttlFont As New Drawing.Font("Segoe UI", 8.5, Drawing.FontStyle.Bold)

            pTransform = New CartesianCoordinates(ChartObj.LINEAR_SCALE, ChartObj.LINEAR_SCALE)
            pTransform.SetCoordinateBounds(tMin, yMin, tMax, yMax)
            pTransform.SetGraphBorderDiagonal(0.13, 0.06, 0.93, 0.9)

            ' Fond
            chartVu.AddChartObject(New Background(pTransform, ChartObj.GRAPH_BACKGROUND,
                                                  Drawing.Color.FromArgb(12, 18, 32)))
            ' Axe X
            Dim axX As New LinearAxis(pTransform, ChartObj.X_AXIS)
            axX.LineColor = Drawing.Color.FromArgb(80, 100, 140)
            chartVu.AddChartObject(axX)
            Dim axXLbl As New NumericAxisLabels(axX)
            axXLbl.SetTextFont(axFont)
            axXLbl.SetColor(Drawing.Color.FromArgb(140, 165, 200))
            chartVu.AddChartObject(axXLbl)
            Dim axXTitle As New AxisTitle(axX, ttlFont, "Time (s)")
            axXTitle.SetColor(Drawing.Color.FromArgb(100, 130, 170))
            chartVu.AddChartObject(axXTitle)

            ' Axe Y avec label unite
            Dim axY As New LinearAxis(pTransform, ChartObj.Y_AXIS, yMin, yMax)
            axY.LineColor = Drawing.Color.FromArgb(120, 145, 180)
            axY.AxisTickDir = ChartObj.AXIS_MIN
            chartVu.AddChartObject(axY)
            Dim axYLbl As New NumericAxisLabels(axY)
            axYLbl.SetTextFont(axFont)
            axYLbl.SetColor(Drawing.Color.FromArgb(120, 145, 180))
            chartVu.AddChartObject(axYLbl)
            If unitLabel <> "" Then
                Dim fullYLabel As String = GetFullYLabel(unitLabel)
                Dim axYTitle As New AxisTitle(axY, ttlFont, fullYLabel)
                Dim titleColor As Drawing.Color = If(Not tabColor.IsEmpty,
                    tabColor, Drawing.Color.FromArgb(140, 165, 200))
                axYTitle.SetColor(titleColor)
                chartVu.AddChartObject(axYTitle)
            End If

            ' Grilles
            Dim xGrid As New Grid(axX, axY, ChartObj.X_AXIS, ChartObj.GRID_MAJOR)
            xGrid.SetColor(Drawing.Color.FromArgb(36, 50, 76))
            chartVu.AddChartObject(xGrid)
            Dim yGrid As New Grid(axX, axY, ChartObj.Y_AXIS, ChartObj.GRID_MAJOR)
            yGrid.SetColor(Drawing.Color.FromArgb(36, 50, 76))
            chartVu.AddChartObject(yGrid)

            ' Courbes
            For Each i As Integer In sigIndices
                Dim sig As HistoricalWindow.CsvSignal = signals(i)
                Dim ds As New SimpleDataset(sig.Name, timeData, sig.Values)
                Dim attr As New ChartAttribute(sig.PlotColor, 1.5, DashStyle.Solid)
                Dim plt As New SimpleLinePlot(pTransform, ds, attr)
                plt.SetFastClipMode(ChartObj.FASTCLIP_X)
                chartVu.AddChartObject(plt)
                _plotList.Add(plt)
            Next

            ' Zoom — cree mais pas AddChartObject (identique a l exemple)
            Dim transformArray As CartesianCoordinates() = {pTransform}
            zoomObj = New ZoomWithStack(chartVu, transformArray, 1, True)
            zoomObj.SetButtonMask(System.Windows.Forms.MouseButtons.Left)
            zoomObj.SetZoomYEnable(True)
            zoomObj.SetZoomXEnable(True)
            zoomObj.SetZoomXRoundMode(ChartObj.AUTOAXES_FAR)
            zoomObj.SetZoomYRoundMode(ChartObj.AUTOAXES_FAR)
            zoomObj.SetEnable(True)
            zoomObj.SetZoomStackEnable(True)
            zoomObj.SetColor(Drawing.Color.FromArgb(180, 200, 230))

            ' Curseur — cree mais pas AddChartObject (identique a l exemple)
            dataCursorObj = New CustomChartDataCursor(chartVu, pTransform, ChartObj.MARKER_VLINE, 8, Me)
            dataCursorObj.SetEnable(True)
            dataCursorObj.MouseMarker.SetColor(Drawing.Color.FromArgb(255, 30, 60))

            ' Mode par defaut : zoom (identique a l exemple)
            If MouseMode = T_MOUSE_MODE.MODE_ZOOM Then
                chartVu.SetCurrentMouseListener(zoomObj)
            Else
                chartVu.SetCurrentMouseListener(dataCursorObj)
            End If

            chartVu.ResizeMode = 0

        Catch ex As Exception
            ' Ne pas crasher
        Finally
            Me.Refresh()
        End Try
    End Sub

    ' ── SetZoom / SetCursor — identique a l exemple ───────────────────────────
    ' Masque ou affiche un plot par son index local dans ce panneau
    Public Sub SetPlotVisible(localIdx As Integer, visible As Boolean)
        If localIdx < 0 OrElse localIdx >= _plotList.Count Then Return
        _plotList(localIdx).SetChartObjEnable(If(visible, ChartObj.OBJECT_ENABLE, ChartObj.OBJECT_DISABLE))
        chartVu.UpdateDraw()
    End Sub

    ' Titre complet de l axe Y selon l unite
    Private Shared Function GetFullYLabel(unit As String) As String
        Select Case unit.ToUpper()
            Case "A" : Return "Current (A)"
            Case "V" : Return "Voltage (V)"
            Case "W" : Return "Active Power (W)"
            Case "VAR" : Return "Reactive Power (VAR)"
            Case "VA" : Return "Apparent Power (VA)"
            Case "PF" : Return "Power Factor"
            Case "HZ" : Return "Frequency (Hz)"
            Case "%" : Return "THD (%)"
            Case Else : Return unit
        End Select
    End Function

    Public Sub SetModeZoom()
        MouseMode = T_MOUSE_MODE.MODE_ZOOM
        chartVu.ResetMouseListeners()
        chartVu.SetCurrentMouseListener(zoomObj)
    End Sub

    Public Sub SetModeCursor()
        MouseMode = T_MOUSE_MODE.MODE_CURSOR
        chartVu.ResetMouseListeners()
        chartVu.SetCurrentMouseListener(dataCursorObj)
    End Sub

    ' Re-leve l evenement CursorMoved depuis la classe declarante
    Friend Sub RaiseCursorMoved(xPos As Double)
        RaiseEvent CursorMoved(xPos)
    End Sub

    ' ── ZoomWithStack — identique a l exemple ─────────────────────────────────
    Private Class ZoomWithStack
        Inherits ChartZoom

        Public Sub New(ByVal component As ChartView, ByVal transforms() As CartesianCoordinates,
                       ByVal n As Integer, ByVal brescale As Boolean)
            MyBase.New(component, transforms, brescale)
        End Sub

        Public Overrides Sub OnMouseDown(ByVal mouseevent As System.Windows.Forms.MouseEventArgs)
            If (mouseevent.Button And System.Windows.Forms.MouseButtons.Right) <> 0 Then
                Me.PopZoomStack()
            Else
                MyBase.OnMouseDown(mouseevent)
            End If
        End Sub
    End Class

    ' ── CustomChartDataCursor — identique a l exemple ─────────────────────────
    ' Avec timer anti-bounce exactement comme dans Historicals_ChartGases
    Friend Class CustomChartDataCursor
        Inherits DataCursor

        Private EventRun As Boolean = False
        Private TimerCursor As System.Timers.Timer
        Private _owner As HistChartPanel   ' pour remonter l evenement

        Public Sub New(ByVal achartview As ChartView,
                       ByVal thetransform As CartesianCoordinates,
                       ByVal nmarkertype As Integer,
                       ByVal rsize As Double,
                       ByVal owner As HistChartPanel)
            MyBase.New(achartview, thetransform, nmarkertype, rsize)
            _owner = owner
            TimerCursor = New System.Timers.Timer()
            TimerCursor.Interval = 100
            AddHandler TimerCursor.Elapsed, AddressOf TimerElapsed
        End Sub

        Public Overrides Sub OnMouseMove(ByVal mouseevent As System.Windows.Forms.MouseEventArgs)
            If EventRun Then Return
            If (mouseevent.Button And GetButtonMask()) <> 0 Then
                EventRun = True
                MyBase.OnMouseMove(mouseevent)
                Dim loc As Point2D = GetLocation()
                _owner.RaiseCursorMoved(loc.X)
                TimerCursor.Start()
            End If
        End Sub

        Public Overrides Sub OnMouseDown(ByVal mouseevent As System.Windows.Forms.MouseEventArgs)
            If (mouseevent.Button And GetButtonMask()) <> 0 Then
                MyBase.OnMouseDown(mouseevent)
                Dim loc As Point2D = GetLocation()
                _owner.RaiseCursorMoved(loc.X)
            End If
        End Sub

        Private Sub TimerElapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs)
            EventRun = False
        End Sub
    End Class

End Class

' =============================================================================
' Partial Class HistoricalWindow
' =============================================================================
Partial Class HistoricalWindow

#Region "Champs"
    Private _tabPanels As New Dictionary(Of String, HistChartPanel)()
    Private _cursorX As Double = 0.0
    ' Mapping : index global signal -> (tabName, localIdx dans ce tab)
    Friend _sigTabMap As New Dictionary(Of Integer, List(Of (TabName As String, LocalIdx As Integer)))()
#End Region

#Region "Build chart"

    Friend Sub BuildChart(data As CsvData)
        If data Is Nothing OrElse data.TimeData.Length = 0 Then Return
        TabControl_Charts.TabPages.Clear()
        _tabPanels.Clear()
        _sigTabMap.Clear()
        TabControl_Charts.ItemSize = New Size(72, 22)

        Dim tMin As Double = data.TimeData(0)
        Dim tMax As Double = data.TimeData(data.TimeData.Length - 1)
        If tMax <= tMin Then tMax = tMin + 1.0

        ' Grouper par unite
        Dim groups As New Dictionary(Of String, List(Of Integer))()
        For i As Integer = 0 To data.Signals.Count - 1
            Dim key As String = GroupKeyForSignal(data.Signals(i))
            If Not groups.ContainsKey(key) Then groups(key) = New List(Of Integer)()
            groups(key).Add(i)
        Next

        ' Tab special "All Powers" : regroupe W, VAR, VA
        Dim powerKeys As New HashSet(Of String)(
            {"Active Power (W)", "Reactive Power (VAR)", "Apparent Power (VA)"})
        Dim allPowerIndices As New List(Of Integer)()
        For Each key As String In powerKeys
            If groups.ContainsKey(key) Then
                allPowerIndices.AddRange(groups(key))
            End If
        Next
        If allPowerIndices.Count > 0 Then
            AddChartTab("All Powers", allPowerIndices, tMin, tMax, data)
        End If

        ' Tabs individuels dans l ordre : Current, Voltage L-L, Voltage L-N, powers, PF, Hz, THD, reste
        Dim orderedKeys As New List(Of String)() From {
            "Current (A)", "Voltage L-L (V)", "Voltage L-N (V)",
            "Active Power (W)", "Reactive Power (VAR)", "Apparent Power (VA)",
            "Power Factor (PF)", "Frequency (Hz)", "THD Current (%)", "THD Voltage (%)",
            "Phase Dir"}
        For Each key As String In orderedKeys
            If groups.ContainsKey(key) Then
                AddChartTab(key, groups(key), tMin, tMax, data)
            End If
        Next
        ' Groupes non reconnus
        For Each kvp As KeyValuePair(Of String, List(Of Integer)) In groups
            If Not orderedKeys.Contains(kvp.Key) Then
                AddChartTab(kvp.Key, kvp.Value, tMin, tMax, data)
            End If
        Next

        AddHandler TabControl_Charts.DrawItem, AddressOf DrawTab
        TabControl_Charts.Refresh()
    End Sub

    ' Nom court de l onglet base sur l unite / groupe
    Friend Shared Function GroupKeyForSignal(sig As CsvSignal) As String
        Select Case sig.Unit.ToUpper()
            Case "A" : Return "Current (A)"
            Case "W" : Return "Active Power (W)"
            Case "VAR" : Return "Reactive Power (VAR)"
            Case "VA" : Return "Apparent Power (VA)"
            Case "PF" : Return "Power Factor (PF)"
            Case "HZ" : Return "Frequency (Hz)"
            Case "%"
                ' Distinguer THD courant vs tension par le nom du signal
                If sig.Name.ToUpper().Contains("THDI") OrElse sig.Name.ToUpper().Contains("THD I") OrElse
                   sig.Name.ToUpper().Contains("CURRENT") Then
                    Return "THD Current (%)"
                Else
                    Return "THD Voltage (%)"
                End If
        End Select
        ' Distinguer tension L-L vs L-N si pas d unite standard
        If sig.Unit.ToUpper() = "V" Then
            If sig.Name.ToUpper().Contains("L-N") OrElse sig.Name.ToUpper().Contains("LN") OrElse
               sig.Name.ToUpper().EndsWith("N") Then
                Return "Voltage L-N (V)"
            Else
                Return "Voltage L-L (V)"
            End If
        End If
        ' Fallback : chercher dans AxisGroups
        For Each ag As UserPreferences.AxisGroupDef In UserPreferences.AxisGroups
            If ag.Unit.ToUpper() = sig.Unit.ToUpper() Then
                Return ag.Name & If(ag.Unit <> "", " (" & ag.Unit & ")", "")
            End If
        Next
        Return If(sig.Unit <> "", sig.Unit, "Other")
    End Function

    Friend Function GetYBounds(sigIndices As List(Of Integer),
                                       signals As List(Of CsvSignal)) As (yMin As Double, yMax As Double)
        If sigIndices.Count = 0 Then Return (0.0, 1.0)
        Dim tabKey As String = GroupKeyForSignal(signals(sigIndices(0)))

        ' Priorite 1 : bornes de session (reglees dans le drawer Axes)
        If _sessionYBounds.ContainsKey(tabKey) Then
            Return _sessionYBounds(tabKey)
        End If

        ' Priorite 2 : UserPreferences
        Dim unit As String = signals(sigIndices(0)).Unit
        For Each ag As UserPreferences.AxisGroupDef In UserPreferences.AxisGroups
            If ag.Unit.ToUpper() = unit.ToUpper() AndAlso ag.Members.Length > 0 Then
                Dim repId As SignalID = ag.Members(0)
                If RegisterMap.HasDef(repId) Then
                    Return (UserPreferences.Instance.GetAxisMin(repId),
                            UserPreferences.Instance.GetAxisMax(repId))
                End If
            End If
        Next
        ' Fallback auto
        Dim yMin As Double = Double.MaxValue
        Dim yMax As Double = Double.MinValue
        For Each i As Integer In sigIndices
            For Each v As Double In signals(i).Values
                If v < yMin Then yMin = v
                If v > yMax Then yMax = v
            Next
        Next
        If yMin = Double.MaxValue Then yMin = 0.0
        If yMax = Double.MinValue OrElse yMax = yMin Then yMax = yMin + 1.0
        Dim m As Double = (yMax - yMin) * 0.05
        Return (yMin - m, yMax + m)
    End Function

    Private Sub AddChartTab(tabName As String, sigIndices As List(Of Integer),
                             tMin As Double, tMax As Double, data As CsvData)
        ' Pour les tabs multi-unites (ex: All Powers), calculer les bornes auto
        Dim unit As String = data.Signals(sigIndices(0)).Unit
        Dim allSameUnit As Boolean = True
        For Each idx As Integer In sigIndices
            If data.Signals(idx).Unit <> unit Then allSameUnit = False : Exit For
        Next
        If Not allSameUnit Then unit = ""  ' unite mixte : pas de label Y

        Dim bounds As (yMin As Double, yMax As Double)
        If Not allSameUnit Then
            ' Bornes auto sur toutes les valeurs
            Dim yMin As Double = Double.MaxValue
            Dim yMax As Double = Double.MinValue
            For Each idx As Integer In sigIndices
                For Each v As Double In data.Signals(idx).Values
                    If v < yMin Then yMin = v
                    If v > yMax Then yMax = v
                Next
            Next
            If yMin = Double.MaxValue Then yMin = 0.0
            If yMax = Double.MinValue OrElse yMax = yMin Then yMax = yMin + 1.0
            Dim margin As Double = (yMax - yMin) * 0.05
            bounds = (yMin - margin, yMax + margin)
        Else
            bounds = GetYBounds(sigIndices, data.Signals)
        End If

        Dim tab As New TabPage(tabName)
        tab.BackColor = Drawing.Color.FromArgb(12, 18, 32)
        TabControl_Charts.TabPages.Add(tab)

        Dim tabClr As Drawing.Color = GetTabColor(tabName)

        Dim panel As New HistChartPanel()
        panel.Dock = DockStyle.Fill
        panel.InitializeChart(tMin, tMax, bounds.yMin, bounds.yMax,
                              unit, data.TimeData, data.Signals, sigIndices, tabClr)
        AddHandler panel.CursorMoved, AddressOf OnCursorMoved
        tab.Controls.Add(panel)
        _tabPanels(tabName) = panel

        ' Enregistrer le mapping global->local (liste pour signaux dans plusieurs tabs)
        For localIdx As Integer = 0 To sigIndices.Count - 1
            Dim gIdx As Integer = sigIndices(localIdx)
            If Not _sigTabMap.ContainsKey(gIdx) Then
                _sigTabMap(gIdx) = New List(Of (TabName As String, LocalIdx As Integer))()
            End If
            _sigTabMap(gIdx).Add((tabName, localIdx))
        Next
    End Sub

    ' Nom court pour affichage dans l onglet (preserves la cle interne)
    Private Shared Function ShortTabName(fullName As String) As String
        Select Case fullName
            Case "Current (A)" : Return "I (A)"
            Case "Voltage L-L (V)" : Return "U L-L"
            Case "Voltage L-N (V)" : Return "U L-N"
            Case "Active Power (W)" : Return "P (W)"
            Case "Reactive Power (VAR)" : Return "Q (VAR)"
            Case "Apparent Power (VA)" : Return "S (VA)"
            Case "All Powers" : Return "Powers"
            Case "Power Factor (PF)" : Return "PF"
            Case "Frequency (Hz)" : Return "Freq"
            Case "THD Current (%)" : Return "THDi"
            Case "THD Voltage (%)" : Return "THDu"
            Case "Phase Dir" : Return "Phase"
            Case Else : Return fullName
        End Select
    End Function

    Private Sub DrawTab(sender As Object, e As DrawItemEventArgs)
        Dim tc As TabControl = CType(sender, TabControl)
        Dim sel As Boolean = (tc.SelectedIndex = e.Index)
        Using br As New SolidBrush(If(sel, Drawing.Color.FromArgb(0, 90, 160),
                                       Drawing.Color.FromArgb(22, 34, 58)))
            e.Graphics.FillRectangle(br, e.Bounds)
        End Using
        Dim sf As New StringFormat()
        sf.Alignment = StringAlignment.Center
        sf.LineAlignment = StringAlignment.Center
        Dim fnt As Drawing.Font = If(sel,
            New Drawing.Font("Segoe UI", 8.5, Drawing.FontStyle.Bold),
            New Drawing.Font("Segoe UI", 8.5, Drawing.FontStyle.Regular))
        Using br As New SolidBrush(If(sel, Drawing.Color.White,
                                       Drawing.Color.FromArgb(160, 185, 220)))
            Dim displayName As String = ShortTabName(tc.TabPages(e.Index).Text)
            e.Graphics.DrawString(displayName, fnt, br, e.Bounds, sf)
        End Using
    End Sub

#End Region

#Region "Mode switch"

    Friend Sub SetChartModeZoom()
        For Each p As HistChartPanel In _tabPanels.Values
            p.SetModeZoom()
        Next
        Button_Zoom.BackColor = Drawing.Color.FromArgb(0, 90, 140)
        Button_Zoom.ForeColor = Drawing.Color.White
        Button_Cursor.BackColor = Drawing.Color.FromArgb(28, 44, 70)
        Button_Cursor.ForeColor = Drawing.Color.FromArgb(200, 215, 240)
    End Sub

    Friend Sub SetChartModeCursor()
        For Each p As HistChartPanel In _tabPanels.Values
            p.SetModeCursor()
        Next
        Button_Cursor.BackColor = Drawing.Color.FromArgb(0, 90, 140)
        Button_Cursor.ForeColor = Drawing.Color.White
        Button_Zoom.BackColor = Drawing.Color.FromArgb(28, 44, 70)
        Button_Zoom.ForeColor = Drawing.Color.FromArgb(200, 215, 240)
    End Sub

#End Region

#Region "Cursor callback"

    Private Sub OnCursorMoved(xPos As Double)
        _cursorX = xPos
        UpdateValuePanelAtCursor(xPos)
        UpdateHdPanelAtCursor(xPos)
    End Sub

    Friend Function FindNearestTickIndex(xPos As Double) As Integer
        If _csvData Is Nothing OrElse _csvData.TimeData.Length = 0 Then Return 0
        Dim best As Integer = 0
        Dim bestDist As Double = Double.MaxValue
        For i As Integer = 0 To _csvData.TimeData.Length - 1
            Dim d As Double = Math.Abs(_csvData.TimeData(i) - xPos)
            If d < bestDist Then bestDist = d : best = i
        Next
        Return best
    End Function

#End Region

End Class