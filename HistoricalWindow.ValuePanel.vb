Option Strict On
Option Explicit On

Imports System.Collections.Generic

' =============================================================================
' HistoricalWindow.ValuePanel.vb  -- Partial Class (5/7)
'
' Panel "Values at Cursor" -- genere exactement comme BuildElecValuesPanel
' de MeterWindow.LiveView.vb :
'   - Groupes ordonnes (Voltage -> Current -> Power -> Phase -> Harmonic -> ePack)
'   - Separateur pleine largeur : nom groupe + boutons All/None a droite
'   - Deux colonnes de cases (CellH=38, SepH=28, Gap=2)
'   - Checkbox en haut droite -> masque/affiche la courbe sur HistChartPanel
'   - Label valeur mis a jour au deplacement du curseur
' =============================================================================
Partial Class HistoricalWindow

#Region "Champs"

    Private _histValLabels As New Dictionary(Of Integer, Label)()
    Private _histChkBoxes As New Dictionary(Of Integer, CheckBox)()

    Private ReadOnly ColorHistBackground As Drawing.Color = Drawing.Color.FromArgb(14, 22, 40)
    Private ReadOnly ColorHistCell As Drawing.Color = Drawing.Color.FromArgb(16, 26, 46)

#End Region

#Region "Construction"

    Friend Sub BuildValuePanel(data As CsvData)
        If Panel_Values Is Nothing Then Return
        Panel_Values.SuspendLayout()
        Panel_Values.Controls.Clear()
        _histValLabels.Clear()
        _histChkBoxes.Clear()

        If data Is Nothing OrElse data.Signals.Count = 0 Then
            Panel_Values.ResumeLayout(True)
            Return
        End If

        Dim groupOrder As SignalGroup() = {
            SignalGroup.Voltage, SignalGroup.Current, SignalGroup.Power,
            SignalGroup.Phase, SignalGroup.Harmonic, SignalGroup.ePack}

        Dim byGroup As New Dictionary(Of SignalGroup, List(Of Integer))()
        For Each sg As SignalGroup In groupOrder
            byGroup(sg) = New List(Of Integer)()
        Next
        For i As Integer = 0 To data.Signals.Count - 1
            Dim g As SignalGroup = data.Signals(i).Group
            If byGroup.ContainsKey(g) Then
                byGroup(g).Add(i)
            Else
                byGroup(SignalGroup.Voltage).Add(i)
            End If
        Next

        Dim anyVisible As Boolean = False
        For Each sg As SignalGroup In groupOrder
            If byGroup(sg).Count > 0 Then anyVisible = True : Exit For
        Next
        If Not anyVisible Then
            Panel_Values.ResumeLayout(False)
            Return
        End If

        Const CellH As Integer = 38
        Const SepH As Integer = 28
        Const Gap As Integer = 2
        Const Cols As Integer = 2
        Dim scrollBarW As Integer = If(Not Panel_Values.VerticalScroll.Visible,
            SystemInformation.VerticalScrollBarWidth, 0)
        Dim panelW As Integer = Math.Max(180, Panel_Values.ClientSize.Width - scrollBarW - 2)
        Dim cellW As Integer = panelW \ Cols

        Dim totalH As Integer = 4
        For Each sg As SignalGroup In groupOrder
            If byGroup(sg).Count = 0 Then Continue For
            totalH += SepH + Gap
            Dim rowsH As Integer = CInt(Math.Ceiling(byGroup(sg).Count / CDbl(Cols)))
            totalH += rowsH * (CellH + Gap)
        Next

        Dim container As New Panel()
        container.Name = "_valContainer"
        container.Width = panelW
        container.Height = Math.Max(totalH, Panel_Values.ClientSize.Height)
        container.BackColor = ColorHistBackground
        container.Location = New Point(0, 0)

        Dim yPos As Integer = 4

        For Each sg As SignalGroup In groupOrder
            Dim grpIndices As List(Of Integer) = byGroup(sg)
            If grpIndices.Count = 0 Then Continue For

            Dim grpColor As Drawing.Color = Drawing.Color.FromArgb(90, 115, 165)
            Dim grpName As String = sg.ToString()
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

            Dim sep As New Panel()
            sep.Width = panelW - 2
            sep.Height = SepH
            sep.Location = New Point(1, yPos)
            sep.BackColor = Drawing.Color.FromArgb(20, 32, 54)

            Dim capturedColor As Drawing.Color = grpColor
            AddHandler sep.Paint, Sub(s2 As Object, ev2 As PaintEventArgs)
                                      Using pen As New Drawing.Pen(capturedColor, 1)
                                          ev2.Graphics.DrawLine(pen, 0, sep.Height - 1,
                                                                sep.Width, sep.Height - 1)
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
                                          SetGroupHistVisible(capturedSg, False)
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
                                         SetGroupHistVisible(capturedSg, True)
                                     End Sub

            sep.Controls.AddRange(New Control() {lblGrp, btnAll, btnNone})
            container.Controls.Add(sep)
            yPos += SepH + Gap

            For idx As Integer = 0 To grpIndices.Count - 1
                Dim globalSigIdx As Integer = grpIndices(idx)
                Dim sig As CsvSignal = data.Signals(globalSigIdx)
                Dim col As Integer = idx Mod Cols
                Dim xCell As Integer = col * cellW + 2
                Dim yCell As Integer = yPos + (idx \ Cols) * (CellH + Gap)

                Dim cell As New Panel()
                cell.Width = cellW - 4
                cell.Height = CellH
                cell.Location = New Point(xCell, yCell)
                cell.BackColor = ColorHistCell
                cell.Tag = globalSigIdx

                Dim lblName As New Label()
                lblName.Text = sig.Name
                lblName.Font = New Font("Segoe UI", 7, Drawing.FontStyle.Bold)
                lblName.ForeColor = sig.PlotColor
                lblName.Location = New Point(4, 2)
                lblName.Size = New Size(cell.Width - 24, 14)

                Dim lblVal As New Label()
                lblVal.Text = "-- " & sig.Unit
                lblVal.Font = New Font("Consolas", 9, Drawing.FontStyle.Bold)
                lblVal.ForeColor = Drawing.Color.FromArgb(210, 230, 255)
                lblVal.Location = New Point(4, 18)
                lblVal.Size = New Size(cell.Width - 24, 17)
                lblVal.Tag = globalSigIdx

                Dim chk As New CheckBox()
                chk.Text = ""
                chk.Size = New Size(15, 15)
                chk.Location = New Point(cell.Width - 18, 2)
                chk.Checked = True
                chk.ForeColor = Drawing.Color.FromArgb(80, 110, 150)
                chk.BackColor = Drawing.Color.Transparent
                chk.Tag = globalSigIdx

                Dim capturedIdx As Integer = globalSigIdx
                AddHandler chk.CheckedChanged, Sub(s2 As Object, ev2 As EventArgs)
                                                   SetHistSignalVisible(capturedIdx, chk.Checked)
                                               End Sub

                cell.Controls.AddRange(New Control() {lblName, lblVal, chk})
                _histValLabels(globalSigIdx) = lblVal
                _histChkBoxes(globalSigIdx) = chk
                container.Controls.Add(cell)
            Next

            Dim grpRows As Integer = CInt(Math.Ceiling(grpIndices.Count / CDbl(Cols)))
            yPos += grpRows * (CellH + Gap)
        Next

        Panel_Values.Controls.Add(container)
        Panel_Values.AutoScroll = True
        Panel_Values.ResumeLayout(True)
        Panel_Values.Invalidate(True)
        Panel_Values.Refresh()
    End Sub

#End Region

#Region "Visibilite signaux"

    Private Sub SetHistSignalVisible(globalIdx As Integer, visible As Boolean)
        If Not _sigTabMap.ContainsKey(globalIdx) Then Return
        For Each mapping In _sigTabMap(globalIdx)
            If _tabPanels.ContainsKey(mapping.TabName) Then
                _tabPanels(mapping.TabName).SetPlotVisible(mapping.LocalIdx, visible)
            End If
        Next
    End Sub

    Private Sub SetGroupHistVisible(sg As SignalGroup, visible As Boolean)
        If _csvData Is Nothing Then Return
        For i As Integer = 0 To _csvData.Signals.Count - 1
            If _csvData.Signals(i).Group <> sg Then Continue For
            SetHistSignalVisible(i, visible)
            If _histChkBoxes.ContainsKey(i) Then _histChkBoxes(i).Checked = visible
        Next
    End Sub

#End Region

#Region "Mise a jour au curseur"

    Friend Sub UpdateValuePanelAtCursor(xPos As Double)
        If _csvData Is Nothing OrElse _histValLabels.Count = 0 Then Return
        Dim tickIdx As Integer = FindNearestTickIndex(xPos)

        Panel_Values.SuspendLayout()
        For i As Integer = 0 To _csvData.Signals.Count - 1
            If Not _histValLabels.ContainsKey(i) Then Continue For
            Dim sig As CsvSignal = _csvData.Signals(i)
            If tickIdx >= 0 AndAlso tickIdx < sig.Values.Length Then
                _histValLabels(i).Text = FormatVal(sig.Values(tickIdx), sig.Unit) & " " & sig.Unit
            Else
                _histValLabels(i).Text = "--"
            End If
        Next
        Panel_Values.ResumeLayout(False)

        If Label_CursorTime IsNot Nothing Then
            Label_CursorTime.Text = "t = " & xPos.ToString("0.00") & " s"
        End If
    End Sub

    Private Shared Function FormatVal(v As Double, unit As String) As String
        Select Case unit.ToUpper()
            Case "A" : Return v.ToString("0.000")
            Case "V" : Return v.ToString("0.00")
            Case "W", "VA", "VAR" : Return v.ToString("0")
            Case "PF" : Return v.ToString("0.000")
            Case "HZ" : Return v.ToString("0.00")
            Case "%" : Return v.ToString("0.0")
            Case Else : Return v.ToString("0.00")
        End Select
    End Function

#End Region

End Class