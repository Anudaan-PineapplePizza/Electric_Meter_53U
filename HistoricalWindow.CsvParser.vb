Option Strict On
Option Explicit On

Imports System.IO
Imports System.Globalization

' =============================================================================
' HistoricalWindow.CsvParser.vb  -- Partial Class (4/6)
' =============================================================================
Partial Class HistoricalWindow

#Region "Structures"

    Public Structure CsvSignal
        Public Name As String
        Public Unit As String
        Public PlotColor As Drawing.Color
        Public Group As SignalGroup   ' depuis # Signal,i,nom,unit,color,group
        Public ColIndex As Integer
        Public Values As Double()
    End Structure

    Public Structure CsvHdGroup
        Public GroupName As String
        Public Orders As Integer()
        ' ValuesPerOrder(orderIdx)(tickIdx)
        Public ValuesPerOrder As Double()()
    End Structure

    Public Class CsvData
        Public FilePath As String = ""
        Public RecordDate As DateTime = DateTime.MinValue
        Public SampleIntervalMs As Integer = 500
        Public TimeData As Double() = New Double() {}
        Public Signals As New List(Of CsvSignal)
        ' HD groupes tels que stockes dans le CSV (HDI1, HDI2, ...)
        Public HdGroups As New List(Of CsvHdGroup)
        Public HdOrders As Integer() = New Integer() {}
        Public HasHd As Boolean = False
    End Class

#End Region

#Region "Parser"

    Public Shared Function LoadCsv(path As String) As CsvData
        Try
            Dim lines As String() = File.ReadAllLines(path, System.Text.Encoding.UTF8)
            Return ParseLines(lines, path)
        Catch ex As Exception
            MessageBox.Show("Error loading CSV:" & Environment.NewLine & ex.Message,
                "Load error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        End Try
    End Function

    Private Shared Function ParseLines(lines As String(), path As String) As CsvData
        Dim ci As CultureInfo = CultureInfo.InvariantCulture
        Dim result As New CsvData()
        result.FilePath = path

        ' ── Passe 1 : metadata (lignes #) ────────────────────────────────────
        Dim metaSignals As New List(Of (Name As String, Unit As String, ColorHex As String, Group As String))()
        Dim hdGroupMeta As New List(Of (Idx As Integer, Name As String))()
        Dim hdOrdersList As New List(Of Integer)()

        For Each line As String In lines
            If Not line.StartsWith("#") Then Continue For
            Dim parts As String() = line.Substring(1).Trim().Split(","c)
            If parts.Length < 2 Then Continue For
            Select Case parts(0).Trim()
                Case "Date"
                    Dim dt As DateTime
                    If DateTime.TryParseExact(parts(1).Trim(), "yyyy-MM-dd HH:mm:ss",
                            ci, DateTimeStyles.None, dt) Then result.RecordDate = dt
                Case "SampleInterval"
                    Dim v As Integer
                    If Integer.TryParse(parts(1).Trim(), v) Then result.SampleIntervalMs = v
                Case "Signal"
                    ' # Signal,<i>,<nom>,<unite>,<ARGB_hex>,<group_int>
                    If parts.Length >= 5 Then
                        Dim grpStr As String = If(parts.Length >= 6, parts(5).Trim(), "0")
                        metaSignals.Add((parts(2).Trim(), parts(3).Trim(), parts(4).Trim(), grpStr))
                    End If
                Case "HD_Group"
                    If parts.Length >= 3 Then
                        Dim gi As Integer
                        If Integer.TryParse(parts(1).Trim(), gi) Then
                            hdGroupMeta.Add((gi, parts(2).Trim()))
                        End If
                    End If
                Case "HD_Orders"
                    For i As Integer = 1 To parts.Length - 1
                        Dim o As Integer
                        If Integer.TryParse(parts(i).Trim(), o) Then hdOrdersList.Add(o)
                    Next
            End Select
        Next

        ' ── Passe 2 : donnees ────────────────────────────────────────────────
        Dim inHd As Boolean = False
        Dim sigHeaderDone As Boolean = False
        Dim hdHeaderDone As Boolean = False
        Dim sigColCount As Integer = 0
        Dim hdColCount As Integer = 0
        Dim timeList As New List(Of Double)()
        Dim sigColData As New Dictionary(Of Integer, List(Of Double))()
        Dim hdColData As New Dictionary(Of Integer, List(Of Double))()
        Dim hdTimeList As New List(Of Double)()

        For Each line As String In lines
            If String.IsNullOrWhiteSpace(line) Then Continue For
            If line.StartsWith("#") Then
                If line.Contains("HD_SECTION") Then inHd = True
                Continue For
            End If

            Dim cols As String() = line.Split(","c)

            If Not inHd Then
                If Not sigHeaderDone Then
                    sigColCount = cols.Length
                    For c As Integer = 1 To sigColCount - 1
                        sigColData(c) = New List(Of Double)()
                    Next
                    sigHeaderDone = True
                    Continue For
                End If
                Dim t As Double
                If Double.TryParse(cols(0), NumberStyles.Any, ci, t) Then
                    timeList.Add(t)
                    For c As Integer = 1 To Math.Min(cols.Length, sigColCount) - 1
                        Dim v As Double
                        sigColData(c).Add(If(Double.TryParse(cols(c), NumberStyles.Any, ci, v), v, 0.0))
                    Next
                End If
            Else
                If Not hdHeaderDone Then
                    hdColCount = cols.Length
                    For c As Integer = 1 To hdColCount - 1
                        hdColData(c) = New List(Of Double)()
                    Next
                    hdHeaderDone = True
                    Continue For
                End If
                Dim t As Double
                If Double.TryParse(cols(0), NumberStyles.Any, ci, t) Then
                    hdTimeList.Add(t)
                    For c As Integer = 1 To Math.Min(cols.Length, hdColCount) - 1
                        Dim v As Double
                        hdColData(c).Add(If(Double.TryParse(cols(c), NumberStyles.Any, ci, v), v, 0.0))
                    Next
                End If
            End If
        Next

        ' ── Signaux ───────────────────────────────────────────────────────────
        result.TimeData = timeList.ToArray()
        For c As Integer = 1 To sigColCount - 1
            Dim sig As New CsvSignal()
            sig.ColIndex = c
            If c - 1 < metaSignals.Count Then
                sig.Name = metaSignals(c - 1).Name
                sig.Unit = metaSignals(c - 1).Unit
                sig.PlotColor = ColorFromHex(metaSignals(c - 1).ColorHex, sig.Name)
                Dim grpInt As Integer
                If Integer.TryParse(metaSignals(c - 1).Group, grpInt) Then
                    sig.Group = CType(grpInt, SignalGroup)
                Else
                    sig.Group = SignalGroup.Voltage
                End If
            Else
                sig.Name = "Signal " & c.ToString()
                sig.Unit = ""
                sig.PlotColor = Drawing.Color.White
                sig.Group = SignalGroup.Voltage
            End If
            sig.Values = If(sigColData.ContainsKey(c), sigColData(c).ToArray(), New Double() {})
            result.Signals.Add(sig)
        Next

        ' ── Harmoniques ───────────────────────────────────────────────────────
        If hdHeaderDone AndAlso hdGroupMeta.Count > 0 AndAlso hdOrdersList.Count > 0 Then
            result.HasHd = True
            result.HdOrders = hdOrdersList.ToArray()
            Dim ordersCount As Integer = hdOrdersList.Count
            For gi As Integer = 0 To hdGroupMeta.Count - 1
                Dim grp As New CsvHdGroup()
                grp.GroupName = hdGroupMeta(gi).Name
                grp.Orders = hdOrdersList.ToArray()
                grp.ValuesPerOrder = New Double(ordersCount - 1)() {}
                For oi As Integer = 0 To ordersCount - 1
                    Dim colIdx As Integer = gi * ordersCount + oi + 1
                    grp.ValuesPerOrder(oi) = If(hdColData.ContainsKey(colIdx),
                        hdColData(colIdx).ToArray(), New Double(hdTimeList.Count - 1) {})
                Next
                result.HdGroups.Add(grp)
            Next
        End If

        Return result
    End Function

    ' ── Conversion couleur ARGB hex ───────────────────────────────────────────
    ' "FF1E90FF" -> Color.FromArgb(255, 30, 144, 255)
    ' CInt(Convert.ToInt64) overflow pour valeurs >= 0x80000000 -> on parse ARGB
    ' composante par composante.
    Private Shared Function ColorFromHex(hex As String, signalName As String) As Drawing.Color
        Try
            If hex.Length = 8 Then
                Dim a As Integer = Convert.ToInt32(hex.Substring(0, 2), 16)
                Dim r As Integer = Convert.ToInt32(hex.Substring(2, 2), 16)
                Dim g As Integer = Convert.ToInt32(hex.Substring(4, 2), 16)
                Dim b As Integer = Convert.ToInt32(hex.Substring(6, 2), 16)
                Return Drawing.Color.FromArgb(a, r, g, b)
            End If
        Catch
        End Try
        ' Fallback : chercher dans RegisterMap par nom
        Return ColorFromRegisterMap(signalName)
    End Function

    Private Shared Function ColorFromRegisterMap(signalName As String) As Drawing.Color
        Try
            For Each r As RegisterDef In RegisterMap.Registers
                If r.Name = signalName Then Return r.PlotColor
            Next
        Catch
        End Try
        Return Drawing.Color.FromArgb(160, 180, 220)
    End Function

#End Region

End Class