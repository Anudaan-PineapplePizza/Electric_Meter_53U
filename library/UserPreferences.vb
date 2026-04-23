Option Strict On
Option Explicit On

Imports System.IO
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Drawing

' =============================================================================
' UserPreferences.vb
'
' Signal visibility:
'   ChartSignals  = shown on the live CHART (formerly "Plot")
'   PanelSignals  = shown in the Electrical Values PANEL (formerly "Enabled")
'
' Y axes:
'   Y1GroupIndex / Y2GroupIndex = index into AxisGroups()
'   The chart uses the group's first chart-enabled member as the axis signal.
' =============================================================================
Public Class UserPreferences

    ' DEFINITION DES GROUPES D'AXES (Noms, Unités, Couleur, Membres)
    ' (shared with PreferencesForm + MeterWindow)
    Public Structure AxisGroupDef
        Public Name As String
        Public Unit As String
        Public Clr As Color
        Public Members As SignalID()
    End Structure

    ' ─────────────────────────────────────────────────────────────────────────
    ' AXIS GROUPS
    ' Les signaux ePack sont ajoutes A LA FIN des membres de chaque groupe afin
    ' que Members(0) reste toujours un signal 53U (toujours present dans Registers).
    ' Cela evite les exceptions dans FlushAxesGrid lorsque ePack n'est pas connecte.
    ' ─────────────────────────────────────────────────────────────────────────
    Public Shared ReadOnly AxisGroups As AxisGroupDef() = New AxisGroupDef() {
        New AxisGroupDef With {
            .Name = "Current",
            .Unit = "A",
            .Clr = Color.FromArgb(0, 220, 150),
            .Members = New SignalID() {SignalID.I, SignalID.I1, SignalID.I2, SignalID.I3, SignalID.I_Neutral,
                                       SignalID.ePack_Irms, SignalID.ePack_Isq, SignalID.ePack_Isqburst}},
        New AxisGroupDef With {
            .Name = "Voltage (L-L)",
            .Unit = "V",
            .Clr = Color.DodgerBlue,
            .Members = New SignalID() {SignalID.U, SignalID.U12, SignalID.U23, SignalID.U31,
                                       SignalID.ePack_Vline, SignalID.ePack_Vrms,
                                       SignalID.ePack_Vsq, SignalID.ePack_Vsqburst}},
        New AxisGroupDef With {
            .Name = "Voltage (L-N)",
            .Unit = "V",
            .Clr = Color.FromArgb(100, 180, 255),
            .Members = New SignalID() {SignalID.U1N, SignalID.U2N, SignalID.U3N}},
        New AxisGroupDef With {
            .Name = "Active Power",
            .Unit = "W",
            .Clr = Color.FromArgb(255, 185, 50),
            .Members = New SignalID() {SignalID.P, SignalID.P1, SignalID.P2, SignalID.P3,
                                       SignalID.ePack_P, SignalID.ePack_Pburst}},
        New AxisGroupDef With {
            .Name = "Reactive Power",
            .Unit = "VAR",
            .Clr = Color.FromArgb(255, 100, 50),
            .Members = New SignalID() {SignalID.Q, SignalID.Q1, SignalID.Q2, SignalID.Q3}},
        New AxisGroupDef With {
            .Name = "Apparent Power",
            .Unit = "VA",
            .Clr = Color.FromArgb(200, 130, 255),
            .Members = New SignalID() {SignalID.S, SignalID.S1, SignalID.S2, SignalID.S3,
                                       SignalID.ePack_S}},
        New AxisGroupDef With {
            .Name = "Power Factor", .Unit = "PF",
            .Clr = Color.FromArgb(255, 110, 110),
            .Members = New SignalID() {SignalID.PF, SignalID.PF1, SignalID.PF2, SignalID.PF3,
                                       SignalID.ePack_PF}},
        New AxisGroupDef With {
            .Name = "Frequency", .Unit = "Hz",
            .Clr = Color.FromArgb(140, 200, 255),
            .Members = New SignalID() {SignalID.F, SignalID.ePack_Frequency}},
        New AxisGroupDef With {
            .Name = "THD Current", .Unit = "%",
            .Clr = Color.FromArgb(0, 220, 120),
            .Members = New SignalID() {SignalID.THDi1, SignalID.THDi2, SignalID.THDi3, SignalID.THDiN}},
        New AxisGroupDef With {
            .Name = "THD Voltage", .Unit = "%",
            .Clr = Color.FromArgb(80, 160, 255),
            .Members = New SignalID() {SignalID.THDu12, SignalID.THDu23, SignalID.THDu31,
                                       SignalID.THDu1N, SignalID.THDu2N, SignalID.THDu3N}},
        New AxisGroupDef With {
            .Name = "Phase / Direction", .Unit = "",
            .Clr = Color.FromArgb(255, 220, 0),
            .Members = New SignalID() {SignalID.Phase_Dir, SignalID.DIR1, SignalID.DIR2, SignalID.DIR3,
                                       SignalID.UT12, SignalID.UT23, SignalID.UT31}}
    }

    ' PROPERTIES

    '─────  Chart-enabled signals (what gets plotted)  ────────────────────
    Public Property ChartSignals As New HashSet(Of SignalID)

    '─────  Panel-visible signals (what shows in Electrical Values)  ─────
    Public Property PanelSignals As New HashSet(Of SignalID)

    '─────  CSV-exported signals  ────────────────────────────────────────
    Public Property CsvSignals As New HashSet(Of SignalID)

    '─────  Index AxisGroups() for Y1 axis. Default = 0 (Current) ────────
    Public Property Y1GroupIndex As Integer = 0

    '─────  Index AxisGroups() for Y2 axis. Default = 3 (Active Power) ───
    Public Property Y2GroupIndex As Integer = 3

    Public Property SampleInterval As Integer = 500
    Public Property ChartTimeWindow As Double = 180.0
    Public Property AxisMinimums As New Dictionary(Of SignalID, Double)
    Public Property AxisMaximums As New Dictionary(Of SignalID, Double)

    ' ───── HD ──────
    Public Property HdGroups As New HashSet(Of Integer)
    Public Property HdOrders As New HashSet(Of Integer)
    Public Property HdExportEnabled As Boolean = False

    ' ───── ePack TCP ──────
    Public Property EpackEnabled As Boolean = False
    Public Property EpackIP As String = "192.168.12.20"
    Public Property EpackPort As Integer = 502

    ' SINGLETON DU FICHIER "preferences.cfg"
    Private Shared ReadOnly PrefFile As String =
        Path.Combine(Application.StartupPath, "preferences.cfg")
    Private Shared ReadOnly CI As CultureInfo = CultureInfo.InvariantCulture
    Private Shared _singleton As UserPreferences = Nothing

    Public Shared ReadOnly Property Instance As UserPreferences
        Get
            If _singleton Is Nothing Then
                _singleton = New UserPreferences()
                _singleton.Load()
            End If
            Return _singleton
        End Get
    End Property

    Public Shared Sub Reset()
        _singleton = Nothing
    End Sub

    Private Sub New()
        ' Panel defaults: all DefaultEnabled signals visible in Electrical Values
        For Each id As SignalID In RegisterMap.GetDefaultEnabledIDs()
            PanelSignals.Add(id)
            CsvSignals.Add(id)
        Next
        ' Chart defaults: only I and U (less noise on first launch)
        ChartSignals.Add(SignalID.I)
        ChartSignals.Add(SignalID.U)
        HdGroups.Add(0)
        For o As Integer = 2 To ModbusRsMaster.HD_HARMONICS_COUNT + 1
            HdOrders.Add(o)
        Next
    End Sub

    ' AXIS GROUP HELPERS
    'First chart-enabled signal in the Y1 axis group (for chart scaling)
    Public Function GetY1RepresentativeSignal() As SignalID
        Return GetGroupRepresentative(Y1GroupIndex)
    End Function

    Public Function GetY2RepresentativeSignal() As SignalID
        Return GetGroupRepresentative(Y2GroupIndex)
    End Function

    Private Function GetGroupRepresentative(groupIdx As Integer) As SignalID
        If groupIdx < 0 OrElse groupIdx >= AxisGroups.Length Then Return SignalID.I
        For Each id As SignalID In AxisGroups(groupIdx).Members
            If ChartSignals.Contains(id) Then Return id
        Next
        ' Fallback: first member
        Return AxisGroups(groupIdx).Members(0)
    End Function

    Public Function GetGroupIdxForSignal(id As SignalID) As Integer
        For i As Integer = 0 To AxisGroups.Length - 1
            For Each m As SignalID In AxisGroups(i).Members
                If m = id Then Return i
            Next
        Next
        Return -1
    End Function

    ' AXIS OVERRIDES
    Public Function GetAxisMin(id As SignalID) As Double
        If AxisMinimums.ContainsKey(id) Then Return AxisMinimums(id)
        Return RegisterMap.GetDef(id).MinVal
    End Function

    Public Function GetAxisMax(id As SignalID) As Double
        If AxisMaximums.ContainsKey(id) Then Return AxisMaximums(id)
        Return RegisterMap.GetDef(id).MaxVal
    End Function

    Public Sub SetAxisOverride(id As SignalID, minVal As Double, maxVal As Double)
        ' Si le signal n'est pas dans les registres charges (ex: ePack deconnecte), on ignore
        If Not RegisterMap.HasDef(id) Then Return
        If Math.Abs(minVal - RegisterMap.GetDef(id).MinVal) < 0.00001 Then
            AxisMinimums.Remove(id)
        Else
            AxisMinimums(id) = minVal
        End If
        If Math.Abs(maxVal - RegisterMap.GetDef(id).MaxVal) < 0.00001 Then
            AxisMaximums.Remove(id)
        Else
            AxisMaximums(id) = maxVal
        End If
    End Sub

    Public Sub ClearAxisOverrides()
        AxisMinimums.Clear()
        AxisMaximums.Clear()
        ChartTimeWindow = 180.0
    End Sub

    ' HD DEFAULTS
    Public Sub ResetHdToDefaults()
        HdGroups.Clear()
        HdGroups.Add(0)
        HdOrders.Clear()
        For o As Integer = 2 To ModbusRsMaster.HD_HARMONICS_COUNT + 1
            HdOrders.Add(o)
        Next
        HdExportEnabled = False
    End Sub

    ' LOAD PREFERENCE FILE 
    Public Sub Load()
        If Not File.Exists(PrefFile) Then Return
        Try
            For Each line As String In File.ReadAllLines(PrefFile)
                If String.IsNullOrWhiteSpace(line) OrElse line.StartsWith("#") Then Continue For
                Dim sep As Integer = line.IndexOf("="c)
                If sep < 1 Then Continue For
                Dim key As String = line.Substring(0, sep).Trim()
                Dim val As String = line.Substring(sep + 1).Trim()

                Select Case key
                    Case "Y1Group"
                        Dim v As Integer
                        If Integer.TryParse(val, v) AndAlso v >= 0 AndAlso v < AxisGroups.Length Then
                            Y1GroupIndex = v
                        End If
                    Case "Y2Group"
                        Dim v As Integer
                        If Integer.TryParse(val, v) AndAlso v >= 0 AndAlso v < AxisGroups.Length Then
                            Y2GroupIndex = v
                        End If
                    ' Legacy compat
                    Case "Y1"
                        Dim id As SignalID
                        If [Enum].TryParse(Of SignalID)(val, id) Then
                            Dim g As Integer = GetGroupIdxForSignal(id)
                            If g >= 0 Then Y1GroupIndex = g
                        End If
                    Case "Y2"
                        Dim id As SignalID
                        If [Enum].TryParse(Of SignalID)(val, id) Then
                            Dim g As Integer = GetGroupIdxForSignal(id)
                            If g >= 0 Then Y2GroupIndex = g
                        End If
                    Case "SampleMs"
                        Dim v As Integer
                        If Integer.TryParse(val, v) AndAlso v >= 100 Then SampleInterval = v
                    Case "XWindow"
                        Dim v As Double
                        If Double.TryParse(val, NumberStyles.Any, CI, v) AndAlso v >= 10 Then ChartTimeWindow = v
                    Case "HdGroups"
                        HdGroups.Clear()
                        For Each t As String In val.Split(","c)
                            Dim v As Integer
                            If Integer.TryParse(t.Trim(), v) AndAlso v >= 0 AndAlso v <= 29 Then HdGroups.Add(v)
                        Next
                    Case "HdOrders"
                        HdOrders.Clear()
                        For Each t As String In val.Split(","c)
                            Dim v As Integer
                            If Integer.TryParse(t.Trim(), v) AndAlso v >= 2 AndAlso v <= 31 Then HdOrders.Add(v)
                        Next
                    Case "HdCsv" : HdExportEnabled = (val.ToLower() = "true")
                    Case "EpackEnabled" : EpackEnabled = (val.ToLower() = "true")
                    Case "EpackIP"
                        If Not String.IsNullOrWhiteSpace(val) Then EpackIP = val
                    Case "EpackPort"
                        Dim v As Integer
                        If Integer.TryParse(val, v) AndAlso v > 0 AndAlso v <= 65535 Then EpackPort = v
                    Case "Csv"
                        CsvSignals.Clear()
                        For Each t As String In val.Split(","c)
                            Dim id As SignalID
                            If [Enum].TryParse(Of SignalID)(t.Trim(), id) Then CsvSignals.Add(id)
                        Next
                    Case Else
                        If key.StartsWith("AxisMin_") Then
                            Dim sid As SignalID : Dim v As Double
                            If [Enum].TryParse(Of SignalID)(key.Substring(8), sid) AndAlso
                               Double.TryParse(val, NumberStyles.Any, CI, v) Then AxisMinimums(sid) = v
                        ElseIf key.StartsWith("AxisMax_") Then
                            Dim sid As SignalID : Dim v As Double
                            If [Enum].TryParse(Of SignalID)(key.Substring(8), sid) AndAlso
                               Double.TryParse(val, NumberStyles.Any, CI, v) Then AxisMaximums(sid) = v
                        End If
                End Select
            Next
        Catch
        End Try
    End Sub

    ' SAVE PREFERENCES
    Public Sub Save()
        Try
            Dim lines As New List(Of String)
            lines.Add("# Electric Meter preferences")
            lines.Add("Y1Group=" & Y1GroupIndex.ToString())
            lines.Add("Y2Group=" & Y2GroupIndex.ToString())
            lines.Add("SampleMs=" & SampleInterval.ToString())
            lines.Add("XWindow=" & ChartTimeWindow.ToString(CI))
            lines.Add("Chart=" & String.Join(",", ChartSignals))
            lines.Add("Panel=" & String.Join(",", PanelSignals))
            lines.Add("Csv=" & String.Join(",", CsvSignals))
            lines.Add("HdGroups=" & String.Join(",", HdGroups))
            lines.Add("HdOrders=" & String.Join(",", HdOrders))
            lines.Add("HdCsv=" & HdExportEnabled.ToString())
            lines.Add("EpackEnabled=" & EpackEnabled.ToString())
            lines.Add("EpackIP=" & EpackIP)
            lines.Add("EpackPort=" & EpackPort.ToString())
            For Each kvp As KeyValuePair(Of SignalID, Double) In AxisMinimums
                lines.Add("AxisMin_" & kvp.Key.ToString() & "=" & kvp.Value.ToString(CI))
            Next
            For Each kvp As KeyValuePair(Of SignalID, Double) In AxisMaximums
                lines.Add("AxisMax_" & kvp.Key.ToString() & "=" & kvp.Value.ToString(CI))
            Next
            File.WriteAllLines(PrefFile, lines.ToArray())
        Catch
        End Try
    End Sub

    ' HELPERS
    Public Function IsChartEnabled(id As SignalID) As Boolean
        Return ChartSignals.Contains(id)
    End Function
    ' Keep legacy alias
    Public Function IsEnabled(id As SignalID) As Boolean
        Return ChartSignals.Contains(id)
    End Function
    Public Sub SetEnabled(id As SignalID, enabled As Boolean)
        If enabled Then ChartSignals.Add(id) Else ChartSignals.Remove(id)
    End Sub
    Public Function IsPanelVisible(id As SignalID) As Boolean
        Return PanelSignals.Contains(id)
    End Function
    Public Sub SetPanelVisible(id As SignalID, visible As Boolean)
        If visible Then PanelSignals.Add(id) Else PanelSignals.Remove(id)
    End Sub
    Public Function IsCsvEnabled(id As SignalID) As Boolean
        Return CsvSignals.Contains(id)
    End Function
    Public Sub SetCsvEnabled(id As SignalID, enabled As Boolean)
        If enabled Then CsvSignals.Add(id) Else CsvSignals.Remove(id)
    End Sub

    ' ─────────────────────────────────────────────────────────────────────────
    ' Electrical Values Panel signal funcitions
    ' GetEnableRealTimeSignals => return signals that are enable on chart with ComboBox
    ' GetPanelRealTimeSignals => return signals that are selected to be shown on the Panel (in preferences)
    ' ─────────────────────────────────────────────────────────────────────────
    Public Function GetEnabledRealTimeSignals() As List(Of RegisterDef)
        Dim result As New List(Of RegisterDef)
        For Each r As RegisterDef In RegisterMap.GetRealTimeSignals()
            If ChartSignals.Contains(r.ID) Then result.Add(r)
        Next
        Return result
    End Function

    Public Function GetPanelRealTimeSignals() As List(Of RegisterDef)
        Dim result As New List(Of RegisterDef)
        For Each r As RegisterDef In RegisterMap.GetRealTimeSignals()
            If PanelSignals.Contains(r.ID) Then result.Add(r)
        Next
        Return result
    End Function

    ' ─────────────────────────────────────────────────────────────────────────
    ' JSON LoadFromFile (called by ConnectionWindow config picker)
    ' ─────────────────────────────────────────────────────────────────────────
    Public Sub LoadFromFile(path As String)
        If Not File.Exists(path) Then Throw New FileNotFoundException("File not found", path)
        Dim json As String = File.ReadAllText(path, System.Text.Encoding.UTF8)
        Dim ci As CultureInfo = CultureInfo.InvariantCulture

        Dim m As System.Text.RegularExpressions.Match

        m = System.Text.RegularExpressions.Regex.Match(json, """sample_ms""\s*:\s*(\d+)")
        If m.Success Then
            Dim v As Integer
            If Integer.TryParse(m.Groups(1).Value, v) AndAlso v >= 100 Then SampleInterval = v
        End If

        m = System.Text.RegularExpressions.Regex.Match(json, """x_window_s""\s*:\s*([\d.]+)")
        If m.Success Then
            Dim v As Double
            If Double.TryParse(m.Groups(1).Value, NumberStyles.Any, ci, v) AndAlso v >= 10 Then ChartTimeWindow = v
        End If

        For Each arr As String() In New String()() {
                New String() {"y1_group", "Y1"},
                New String() {"y2_group", "Y2"}}
            m = System.Text.RegularExpressions.Regex.Match(
                json, """" & arr(0) & """\s*:\s*(\d+)")
            If m.Success Then
                Dim v As Integer
                If Integer.TryParse(m.Groups(1).Value, v) AndAlso v >= 0 AndAlso v < AxisGroups.Length Then
                    If arr(1) = "Y1" Then Y1GroupIndex = v Else Y2GroupIndex = v
                End If
            End If
        Next

        For Each arr As String() In New String()() {
                New String() {"chart", "chart"},
                New String() {"panel", "panel"},
                New String() {"csv", "csv"}}
            m = System.Text.RegularExpressions.Regex.Match(
                json, """" & arr(0) & """\s*:\s*\[(.*?)\]",
                System.Text.RegularExpressions.RegexOptions.Singleline)
            If m.Success Then
                Dim targetSet As HashSet(Of SignalID) =
                    If(arr(0) = "chart", ChartSignals,
                    If(arr(0) = "panel", PanelSignals, CsvSignals))
                targetSet.Clear()
                For Each t As System.Text.RegularExpressions.Match In
                        System.Text.RegularExpressions.Regex.Matches(m.Groups(1).Value, """(\w+)""")
                    Dim sid As SignalID
                    If [Enum].TryParse(Of SignalID)(t.Groups(1).Value, sid) Then targetSet.Add(sid)
                Next
            End If
        Next
    End Sub

End Class