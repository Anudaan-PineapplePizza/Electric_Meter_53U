Option Strict Off
Option Explicit On

Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.Collections.Generic
Imports com.quinncurtis.chart2dnet
Imports com.quinncurtis.rtgraphnet

' =============================================================================
' LiveChart with QuinnCurtis
'
' Y1 (left)  : signals in the Y1 axis group
' Y2 (right) : signals in the Y2 axis group + overflow
'
' =============================================================================
Public Class LiveChart
    Inherits ChartView

    ' =========================================================================
    ' STOPWATCH
    ' =========================================================================
    Private _sw As New Stopwatch()

    Public Sub StartStopwatch()
        _sw.Start()
    End Sub

    Public Sub PauseStopwatch()
        _sw.Stop()
    End Sub

    Public Sub ResetStopwatch()
        _sw.Reset()
    End Sub

    Public ReadOnly Property ElapsedSeconds As Double
        Get
            Return _sw.Elapsed.TotalSeconds
        End Get
    End Property

    ' =========================================================================
    ' Y AXIS GROUP SELECTION
    ' =========================================================================
    Private _y1GroupIndex As Integer = 0
    Private _y2GroupIndex As Integer = 3

    Private Function IsInY1Group(id As SignalID) As Boolean
        If _y1GroupIndex < 0 OrElse _y1GroupIndex >= UserPreferences.AxisGroups.Length Then Return False
        For Each m As SignalID In UserPreferences.AxisGroups(_y1GroupIndex).Members
            If m = id Then Return True
        Next
        Return False
    End Function

    Private Function IsInY2Group(id As SignalID) As Boolean
        If _y2GroupIndex < 0 OrElse _y2GroupIndex >= UserPreferences.AxisGroups.Length Then Return False
        For Each m As SignalID In UserPreferences.AxisGroups(_y2GroupIndex).Members
            If m = id Then Return True
        Next
        Return False
    End Function

    Private Function GetTransformFor(id As SignalID) As CartesianCoordinates
        Return If(IsInY1Group(id), _transformY1, _transformY2)
    End Function

    ' =========================================================================
    ' Y2 TITLE — painted manually so it sits in the right margin
    ' =========================================================================
    Private _y2TitleText As String = ""
    Private _y2TitleColor As Color = Color.Gray
    Private _y2TitleFont As New Font("Segoe UI", 9, FontStyle.Bold)

    Private Sub UpdateY2TitleFields()
        _y2TitleText = GroupLabel(_y2GroupIndex)
        _y2TitleColor = GroupColor(_y2GroupIndex)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        If String.IsNullOrEmpty(_y2TitleText) Then Return

        Dim g As Graphics = e.Graphics
        Dim sz As SizeF = g.MeasureString(_y2TitleText, _y2TitleFont)

        ' Place the title vertically centred, near the right edge
        Dim xPos As Single = Me.ClientSize.Width - 35.0F
        Dim yPos As Single = (Me.ClientSize.Height + sz.Width) / 2.0F

        Dim state As Drawing2D.GraphicsState = g.Save()
        g.TranslateTransform(xPos, yPos)
        g.RotateTransform(-90)
        Using br As New SolidBrush(_y2TitleColor)
            g.DrawString(_y2TitleText, _y2TitleFont, br, 0.0F, 0.0F)
        End Using
        g.Restore(state)
    End Sub

    ' =========================================================================
    ' INTERNAL STATE
    ' =========================================================================
    Private _xWindowSeconds As Double = 180.0
    Private Const MAX_POINTS As Integer = 5000

    Private _trackedSignals As New List(Of RegisterDef)
    Private _timeDataPerSignal As New Dictionary(Of SignalID, List(Of Double))
    Private _valueDataPerSignal As New Dictionary(Of SignalID, List(Of Double))
    Private _lineStyles As New Dictionary(Of SignalID, ChartAttribute)
    Private _chartDatasets As New Dictionary(Of SignalID, SimpleDataset)
    Private _plots As New Dictionary(Of SignalID, SimpleLinePlot)

    ' =========================================================================
    ' DESIGNER
    ' =========================================================================
    Public Sub New()
        MyBase.New()
        InitializeComponent()
    End Sub

    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then components.Dispose()
            _y2TitleFont.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Name = "LiveChart"
        Me.Size = New System.Drawing.Size(820, 400)
        Me.BackColor = Color.FromArgb(16, 22, 36)
    End Sub

    ' =========================================================================
    ' QCCHART2D OBJECTS
    ' =========================================================================
    Private _chartView As ChartView = Me

    Private _transformY1 As CartesianCoordinates
    Private _transformY2 As CartesianCoordinates
    Private _backgroundFill As Background
    Private _axisX As LinearAxis
    Private _axisXLabels As NumericAxisLabels
    Private _axisXTitle As AxisTitle
    Private _axisY1 As LinearAxis
    Private _axisY1Labels As NumericAxisLabels
    Private _axisY1Title As AxisTitle
    Private _axisY2 As LinearAxis
    Private _axisY2Labels As NumericAxisLabels
    ' _axisY2Title intentionally commented — title drawn via OnPaint
    Private _gridVertical As Grid
    Private _gridHorizontal As Grid

    ' =========================================================================
    ' AXIS SCALE / LABEL / COLOR HELPERS
    ' =========================================================================
    Private Function GroupScale(groupIdx As Integer) As (min As Double, max As Double)
        If groupIdx < 0 OrElse groupIdx >= UserPreferences.AxisGroups.Length Then Return (0, 1)
        Dim g As UserPreferences.AxisGroupDef = UserPreferences.AxisGroups(groupIdx)
        Dim repId As SignalID = g.Members(0)
        Return (UserPreferences.Instance.GetAxisMin(repId),
                UserPreferences.Instance.GetAxisMax(repId))
    End Function

    Private Shared Function GroupLabel(groupIdx As Integer) As String
        If groupIdx < 0 OrElse groupIdx >= UserPreferences.AxisGroups.Length Then Return ""
        Dim g As UserPreferences.AxisGroupDef = UserPreferences.AxisGroups(groupIdx)
        Return g.Name & If(g.Unit <> "", " (" & g.Unit & ")", "")
    End Function

    Private Shared Function GroupColor(groupIdx As Integer) As Color
        If groupIdx < 0 OrElse groupIdx >= UserPreferences.AxisGroups.Length Then Return Color.Gray
        Return UserPreferences.AxisGroups(groupIdx).Clr
    End Function

    ' =========================================================================
    ' INITIALIZE PLOT VALUES
    ' =========================================================================
    Public Sub InitializePlotValues()
        _timeDataPerSignal.Clear()
        _valueDataPerSignal.Clear()
        _lineStyles.Clear()
        For Each r As RegisterDef In _trackedSignals
            _timeDataPerSignal(r.ID) = New List(Of Double)
            _valueDataPerSignal(r.ID) = New List(Of Double)
            _lineStyles(r.ID) = New ChartAttribute(r.PlotColor, 1.5, DashStyle.Solid)
        Next
    End Sub

    ' =========================================================================
    ' MERGE SIGNAL DATA
    ' =========================================================================
    Private Sub MergeSignalData(newSignals As List(Of RegisterDef))
        Dim toRemove As New List(Of SignalID)
        For Each id As SignalID In _timeDataPerSignal.Keys
            Dim stillActive As Boolean = False
            For Each r As RegisterDef In newSignals
                If r.ID = id Then stillActive = True : Exit For
            Next
            If Not stillActive Then toRemove.Add(id)
        Next
        For Each id As SignalID In toRemove
            _timeDataPerSignal.Remove(id)
            _valueDataPerSignal.Remove(id)
            _lineStyles.Remove(id)
        Next
        For Each r As RegisterDef In newSignals
            If Not _timeDataPerSignal.ContainsKey(r.ID) Then
                _timeDataPerSignal(r.ID) = New List(Of Double)
                _valueDataPerSignal(r.ID) = New List(Of Double)
            End If
            _lineStyles(r.ID) = New ChartAttribute(r.PlotColor, 1.5, DashStyle.Solid)
        Next
    End Sub

    ' =========================================================================
    ' INITIALIZE CHART
    '   keepData = True  : keep existing series data, rebuild axes only
    '   keepData = False : full reset
    ' =========================================================================
    Public Sub InitializeChart(activeSignals As List(Of RegisterDef), keepData As Boolean)
        _trackedSignals = activeSignals

        If keepData Then
            MergeSignalData(activeSignals)
        Else
            InitializePlotValues()
        End If

        _xWindowSeconds = UserPreferences.Instance.ChartTimeWindow

        Dim scY1 As (min As Double, max As Double) = GroupScale(_y1GroupIndex)
        Dim scY2 As (min As Double, max As Double) = GroupScale(_y2GroupIndex)
        Dim clY1 As Color = GroupColor(_y1GroupIndex)
        Dim clY2 As Color = GroupColor(_y2GroupIndex)

        UpdateY2TitleFields()

        Try
            _transformY1 = New CartesianCoordinates(
                0.0, scY1.min, _xWindowSeconds, scY1.max)
            _transformY1.SetGraphBorderDiagonal(0.1, 0.05, 0.9, 0.9)

            _transformY2 = New CartesianCoordinates(
                0.0, scY2.min, _xWindowSeconds, scY2.max)
            _transformY2.SetGraphBorderDiagonal(0.1, 0.05, 0.9, 0.9)

            _chartView.ResetChartObjectList()

            _backgroundFill = New Background(_transformY1, ChartObj.GRAPH_BACKGROUND,
                                             Color.FromArgb(16, 22, 36))
            _chartView.AddChartObject(_backgroundFill)

            Dim axFont As New Font("Segoe UI", 8.0, FontStyle.Regular)
            Dim ttlFont As New Font("Segoe UI", 9, FontStyle.Bold)

            ' X axis
            _axisX = New LinearAxis(_transformY1, ChartObj.X_AXIS)
            _axisX.LineColor = Color.FromArgb(80, 100, 140)
            _chartView.AddChartObject(_axisX)

            _axisXLabels = New NumericAxisLabels(_axisX)
            _axisXLabels.SetTextFont(axFont)
            _axisXLabels.SetColor(Color.FromArgb(140, 165, 200))
            _chartView.AddChartObject(_axisXLabels)

            _axisXTitle = New AxisTitle(_axisX, ttlFont, "Time (s)")
            _axisXTitle.SetColor(Color.FromArgb(120, 145, 180))
            _chartView.AddChartObject(_axisXTitle)

            ' Y1 axis — AxisTitle works fine here (left side)
            _axisY1 = New LinearAxis(_transformY1, ChartObj.Y_AXIS)
            _axisY1.LineColor = clY1
            _axisY1.LineWidth = 2
            _chartView.AddChartObject(_axisY1)

            _axisY1Labels = New NumericAxisLabels(_axisY1)
            _axisY1Labels.SetTextFont(axFont)
            _axisY1Labels.SetColor(clY1)
            _chartView.AddChartObject(_axisY1Labels)

            _axisY1Title = New AxisTitle(_axisY1, ttlFont, GroupLabel(_y1GroupIndex))
            _axisY1Title.SetColor(clY1)
            _chartView.AddChartObject(_axisY1Title)

            ' Y2 axis — no AxisTitle object; title drawn in OnPaint
            _axisY2 = New LinearAxis(_transformY2, ChartObj.Y_AXIS)
            _axisY2.LineColor = clY2
            _axisY2.LineWidth = 2
            _axisY2.SetAxisIntercept(_axisX.GetAxisMax())
            _axisY2.SetAxisTickDir(ChartObj.AXIS_MAX)
            _chartView.AddChartObject(_axisY2)

            _axisY2Labels = New NumericAxisLabels(_axisY2)
            _axisY2Labels.SetTextFont(axFont)
            _axisY2Labels.SetColor(clY2)
            _chartView.AddChartObject(_axisY2Labels)

            ' Grid
            _gridVertical = New Grid(_axisX, _axisY1, ChartObj.X_AXIS, ChartObj.GRID_ALL)
            _gridHorizontal = New Grid(_axisX, _axisY1, ChartObj.Y_AXIS, ChartObj.GRID_ALL)
            _gridVertical.SetColor(Color.FromArgb(36, 50, 76))
            _gridHorizontal.SetColor(Color.FromArgb(36, 50, 76))
            _chartView.AddChartObject(_gridVertical)
            _chartView.AddChartObject(_gridHorizontal)

            ' One plot per signal
            If Not Me.DesignMode Then
                _chartDatasets.Clear()
                _plots.Clear()
                For Each r As RegisterDef In _trackedSignals
                    Dim pT As CartesianCoordinates = GetTransformFor(r.ID)
                    Dim xInit() As Double = {0.0, 0.1}
                    Dim yInit() As Double = {0.0, 0.0}

                    If keepData AndAlso
                       _timeDataPerSignal.ContainsKey(r.ID) AndAlso
                       _timeDataPerSignal(r.ID).Count >= 2 Then
                        xInit = _timeDataPerSignal(r.ID).ToArray()
                        yInit = _valueDataPerSignal(r.ID).ToArray()
                    End If

                    Dim ds As New SimpleDataset(r.Name, xInit, yInit)
                    Dim plt As New SimpleLinePlot(pT, ds, _lineStyles(r.ID))
                    plt.SetFastClipMode(ChartObj.FASTCLIP_X)
                    _chartDatasets(r.ID) = ds
                    _plots(r.ID) = plt
                    _chartView.AddChartObject(plt)
                Next
            End If

        Finally
            Me.Refresh()
        End Try
    End Sub

    ' =========================================================================
    ' SET AXIS GROUPS
    ' =========================================================================
    Public Sub SetAxisGroups(y1GroupIdx As Integer, y2GroupIdx As Integer)
        _y1GroupIndex = Math.Max(0, Math.Min(y1GroupIdx, UserPreferences.AxisGroups.Length - 1))
        _y2GroupIndex = Math.Max(0, Math.Min(y2GroupIdx, UserPreferences.AxisGroups.Length - 1))
        UpdateY2TitleFields()
        UpdateGroupAppearance()
    End Sub

    Private Sub UpdateGroupAppearance()
        If _transformY1 Is Nothing OrElse _transformY2 Is Nothing Then Return

        Dim scY1 As (min As Double, max As Double) = GroupScale(_y1GroupIndex)
        Dim scY2 As (min As Double, max As Double) = GroupScale(_y2GroupIndex)
        Dim clY1 As Color = GroupColor(_y1GroupIndex)
        Dim clY2 As Color = GroupColor(_y2GroupIndex)

        _transformY1.SetScaleStartY(scY1.min) : _transformY1.SetScaleStopY(scY1.max)
        _transformY2.SetScaleStartY(scY2.min) : _transformY2.SetScaleStopY(scY2.max)

        If _axisY1 IsNot Nothing Then
            _axisY1.LineColor = clY1
            _axisY1.CalcAutoAxis()
        End If
        If _axisY1Labels IsNot Nothing Then
            _axisY1Labels.SetColor(clY1)
            _axisY1Labels.CalcAutoAxisLabels()
        End If
        If _axisY1Title IsNot Nothing Then
            _axisY1Title.SetTextString(GroupLabel(_y1GroupIndex))
            _axisY1Title.SetColor(clY1)
        End If

        If _axisY2 IsNot Nothing Then
            _axisY2.LineColor = clY2
            _axisY2.CalcAutoAxis()
        End If
        If _axisY2Labels IsNot Nothing Then
            _axisY2Labels.SetColor(clY2)
            _axisY2Labels.CalcAutoAxisLabels()
        End If
        If _axisY2 IsNot Nothing AndAlso _axisX IsNot Nothing Then
            _axisY2.SetAxisIntercept(_axisX.GetAxisMax())
        End If

        _chartView.UpdateDraw()
        Me.Invalidate()
    End Sub

    ' Legacy compatibility
    Public Sub SetSelectedSignal(id As SignalID)
        Dim grpIdx As Integer = UserPreferences.Instance.GetGroupIdxForSignal(id)
        If grpIdx < 0 Then Return
        If grpIdx = _y2GroupIndex Then
            SetAxisGroups(_y1GroupIndex, grpIdx)
        Else
            SetAxisGroups(grpIdx, _y2GroupIndex)
        End If
    End Sub

    ' =========================================================================
    ' UPDATE AXIS SCALES
    ' =========================================================================
    Public Sub UpdateAxisScales()
        If _transformY1 Is Nothing Then Return
        _xWindowSeconds = UserPreferences.Instance.ChartTimeWindow
        UpdateGroupAppearance()
        If _axisX IsNot Nothing Then _axisX.CalcAutoAxis()
        If _axisXLabels IsNot Nothing Then _axisXLabels.CalcAutoAxisLabels()
        _chartView.UpdateDraw()
    End Sub

    ' =========================================================================
    ' SET SIGNAL VALUE
    ' =========================================================================
    Public Sub SetSignalValue(id As SignalID, rawValue As Double)
        If Not _timeDataPerSignal.ContainsKey(id) Then Return
        Dim t As Double = _sw.Elapsed.TotalSeconds
        _timeDataPerSignal(id).Add(t)
        _valueDataPerSignal(id).Add(rawValue)
        Dim cutoff As Double = t - _xWindowSeconds - 5.0
        Do While _timeDataPerSignal(id).Count > 2 AndAlso _timeDataPerSignal(id)(0) < cutoff
            _timeDataPerSignal(id).RemoveAt(0)
            _valueDataPerSignal(id).RemoveAt(0)
        Loop
        If _timeDataPerSignal(id).Count > MAX_POINTS Then
            _timeDataPerSignal(id).RemoveAt(0)
            _valueDataPerSignal(id).RemoveAt(0)
        End If
    End Sub

    ' =========================================================================
    ' SET PLOT ENABLED
    ' =========================================================================
    Public Sub SetPlotEnabled(id As SignalID, enabled As Boolean)
        If Not _plots.ContainsKey(id) Then Return
        _plots(id).SetChartObjEnable(If(enabled, ChartObj.OBJECT_ENABLE, 0))
        _chartView.UpdateDraw()
    End Sub

    ' =========================================================================
    ' RESET CHART
    ' =========================================================================
    Public Sub ResetChart()
        InitializeChart(_trackedSignals, False)
    End Sub

    ' =========================================================================
    ' UPDATE CHART DRAW  (called every Timer_Sample tick)
    ' =========================================================================
    Public Sub UpdateChartDraw()
        If _transformY1 Is Nothing Then Return

        ' Push latest data into each plot
        For Each r As RegisterDef In _trackedSignals
            Dim id As SignalID = r.ID
            If Not _plots.ContainsKey(id) Then Continue For
            If Not _timeDataPerSignal.ContainsKey(id) Then Continue For
            If _timeDataPerSignal(id).Count < 2 Then Continue For
            Dim ds As New SimpleDataset(r.Name,
                _timeDataPerSignal(id).ToArray(),
                _valueDataPerSignal(id).ToArray())
            _plots(id).SetDataset(ds)
            _chartDatasets(id) = ds
        Next

        ' Scroll X
        Dim elapsed As Double = _sw.Elapsed.TotalSeconds
        Dim xEnd As Double = Math.Max(elapsed, _xWindowSeconds)
        Dim xStart As Double = xEnd - _xWindowSeconds
        _transformY1.SetScaleStartX(xStart) : _transformY1.SetScaleStopX(xEnd)
        _transformY2.SetScaleStartX(xStart) : _transformY2.SetScaleStopX(xEnd)

        ' Y scales
        Dim scY1 As (min As Double, max As Double) = GroupScale(_y1GroupIndex)
        Dim scY2 As (min As Double, max As Double) = GroupScale(_y2GroupIndex)
        _transformY1.SetScaleStartY(scY1.min) : _transformY1.SetScaleStopY(scY1.max)
        _transformY2.SetScaleStartY(scY2.min) : _transformY2.SetScaleStopY(scY2.max)

        ' Recalculate axes
        If _axisX IsNot Nothing Then _axisX.CalcAutoAxis()
        If _axisXLabels IsNot Nothing Then _axisXLabels.CalcAutoAxisLabels()
        If _axisY1 IsNot Nothing Then _axisY1.CalcAutoAxis()
        If _axisY1Labels IsNot Nothing Then _axisY1Labels.CalcAutoAxisLabels()
        If _axisY2 IsNot Nothing Then _axisY2.CalcAutoAxis()
        If _axisY2Labels IsNot Nothing Then _axisY2Labels.CalcAutoAxisLabels()

        If _axisY2 IsNot Nothing AndAlso _axisX IsNot Nothing Then
            _axisY2.SetAxisIntercept(_axisX.GetAxisMax())
        End If

        _chartView.UpdateDraw()
        ' OnPaint fires automatically after the control redraws
    End Sub

End Class