Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports System.Globalization

' =============================================================================
' HistoricalWindow.HdPanel.vb  -- Partial Class (5/6)
'
' Bande HD en bas, pleine largeur.
'
' Les groupes CSV (HDI1, HDI2, HDI3, HDIN, HDU12...) sont combines en
' 3 super-groupes :
'   HDi     = HDI1 + HDI2 + HDI3 + HDIN  (courant)
'   HDU L-L = HDU12 + HDU23 + HDU31      (tension phase-phase)
'   HDU L-N = HDU1N + HDU2N + HDU3N      (tension phase-neutre)
'
' DGV : une LIGNE par sous-groupe, une COLONNE par ordre H2..Hn
' Valeurs mises a jour a chaque deplacement du curseur.
' =============================================================================
Partial Class HistoricalWindow

#Region "Structures super-groupes"

    Private Structure HdSuperGroup
        Public Label As String
        ' Indices dans CsvData.HdGroups appartenant a ce super-groupe
        Public SubGroupIndices As List(Of Integer)
    End Structure

#End Region

#Region "Champs HD"

    Private _hdDgv As DataGridView = Nothing
    Private _hdSuperGroups As New List(Of HdSuperGroup)()
    Private _hdActiveSuper As Integer = 0   ' index dans _hdSuperGroups

#End Region

#Region "Construction"

    Friend Sub BuildHdPanel(data As CsvData)
        Panel_HdGroupBar.Controls.Clear()
        Panel_HdGrid.Controls.Clear()
        _hdDgv = Nothing
        _hdSuperGroups.Clear()
        _hdActiveSuper = 0

        If data Is Nothing OrElse Not data.HasHd OrElse data.HdGroups.Count = 0 Then
            Panel_HdBand.Visible = False
            Return
        End If

        Panel_HdBand.Visible = True

        ' ── Construire les super-groupes ──────────────────────────────────────
        Dim sgHdi As New HdSuperGroup()
        sgHdi.Label = "HDi  (current)"
        sgHdi.SubGroupIndices = New List(Of Integer)()

        Dim sgLL As New HdSuperGroup()
        sgLL.Label = "HDU  L - L"
        sgLL.SubGroupIndices = New List(Of Integer)()

        Dim sgLN As New HdSuperGroup()
        sgLN.Label = "HDU  L - N"
        sgLN.SubGroupIndices = New List(Of Integer)()

        For gi As Integer = 0 To data.HdGroups.Count - 1
            Dim name As String = data.HdGroups(gi).GroupName.ToUpper()
            If name.StartsWith("HDI") Then
                sgHdi.SubGroupIndices.Add(gi)
            ElseIf name.StartsWith("HDU") AndAlso name.EndsWith("N") Then
                sgLN.SubGroupIndices.Add(gi)
            ElseIf name.StartsWith("HDU") Then
                sgLL.SubGroupIndices.Add(gi)
            Else
                ' Groupe inconnu -> ajoute dans HDi par defaut
                sgHdi.SubGroupIndices.Add(gi)
            End If
        Next

        ' N ajouter que les super-groupes non vides
        For Each sg As HdSuperGroup In New HdSuperGroup() {sgHdi, sgLL, sgLN}
            If sg.SubGroupIndices.Count > 0 Then _hdSuperGroups.Add(sg)
        Next

        If _hdSuperGroups.Count = 0 Then
            Panel_HdBand.Visible = False
            Return
        End If

        ' ── Boutons de super-groupes ──────────────────────────────────────────
        Dim x As Integer = 8
        For si As Integer = 0 To _hdSuperGroups.Count - 1
            Dim btn As New Button()
            btn.Text = _hdSuperGroups(si).Label
            btn.Size = New Size(130, 22)
            btn.Location = New Point(x, 5)
            btn.FlatStyle = FlatStyle.Flat
            btn.FlatAppearance.BorderSize = 0
            btn.BackColor = If(si = 0, Drawing.Color.FromArgb(0, 90, 140),
                                       Drawing.Color.FromArgb(28, 44, 70))
            btn.ForeColor = Drawing.Color.FromArgb(200, 215, 240)
            btn.Font = New Font("Segoe UI", 7.5, FontStyle.Bold)
            btn.Cursor = Cursors.Hand
            btn.Tag = si
            AddHandler btn.Click, AddressOf HdSuperGroupBtn_Click
            Panel_HdGroupBar.Controls.Add(btn)
            x += 136
        Next

        ' ── DataGridView ──────────────────────────────────────────────────────
        _hdDgv = New DataGridView()
        _hdDgv.Dock = DockStyle.Fill
        _hdDgv.BackgroundColor = Drawing.Color.FromArgb(12, 18, 32)
        _hdDgv.BorderStyle = BorderStyle.None
        _hdDgv.GridColor = Drawing.Color.FromArgb(30, 45, 70)
        _hdDgv.RowHeadersVisible = False
        _hdDgv.AllowUserToAddRows = False
        _hdDgv.AllowUserToDeleteRows = False
        _hdDgv.AllowUserToResizeRows = False
        _hdDgv.MultiSelect = False
        _hdDgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        _hdDgv.ScrollBars = ScrollBars.Horizontal
        _hdDgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        _hdDgv.ColumnHeadersHeight = 24
        _hdDgv.RowTemplate.Height = 26

        Dim hStyle As New DataGridViewCellStyle()
        hStyle.BackColor = Drawing.Color.FromArgb(22, 34, 58)
        hStyle.ForeColor = Drawing.Color.FromArgb(90, 115, 165)
        hStyle.Font = New Font("Segoe UI", 7.5, FontStyle.Bold)
        hStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        _hdDgv.ColumnHeadersDefaultCellStyle = hStyle

        Dim cStyle As New DataGridViewCellStyle()
        cStyle.BackColor = Drawing.Color.FromArgb(16, 26, 46)
        cStyle.ForeColor = Drawing.Color.FromArgb(200, 220, 245)
        cStyle.Font = New Font("Consolas", 8.5, FontStyle.Bold)
        cStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        cStyle.SelectionBackColor = Drawing.Color.FromArgb(30, 50, 90)
        cStyle.SelectionForeColor = Drawing.Color.White
        _hdDgv.DefaultCellStyle = cStyle

        Panel_HdGrid.Controls.Add(_hdDgv)

        ' Construire les colonnes pour le premier super-groupe
        RebuildDgvColumns(data, 0)
    End Sub

    Private Sub RebuildDgvColumns(data As CsvData, superIdx As Integer)
        If _hdDgv Is Nothing OrElse superIdx >= _hdSuperGroups.Count Then Return
        _hdDgv.Columns.Clear()
        _hdDgv.Rows.Clear()

        ' Deconnecter l ancien handler avant d en ajouter un nouveau
        RemoveHandler _hdDgv.CellPainting, AddressOf HdDgv_CellPainting
        AddHandler _hdDgv.CellPainting, AddressOf HdDgv_CellPainting

        Dim sg As HdSuperGroup = _hdSuperGroups(superIdx)

        ' Determine si c est un groupe courant ou tension pour choisir les colonnes fondamentales
        Dim isCurrentGroup As Boolean = sg.Label.ToUpper().Contains("HDI") OrElse
                                         sg.Label.ToUpper().Contains("CURRENT")

        ' Colonne Group (nom sous-groupe)
        Dim cName As New DataGridViewTextBoxColumn()
        cName.HeaderText = "Group"
        cName.Name = "Col_Group"
        cName.Width = 90
        cName.ReadOnly = True
        cName.SortMode = DataGridViewColumnSortMode.NotSortable
        Dim csName As New DataGridViewCellStyle()
        csName.BackColor = Drawing.Color.FromArgb(18, 28, 48)
        csName.ForeColor = Drawing.Color.FromArgb(140, 170, 210)
        csName.Font = New Font("Segoe UI", 8, FontStyle.Bold)
        csName.Alignment = DataGridViewContentAlignment.MiddleLeft
        cName.DefaultCellStyle = csName
        _hdDgv.Columns.Add(cName)

        ' Colonne fondamentale (I rms ou U selon le groupe)
        Dim cFund As New DataGridViewTextBoxColumn()
        cFund.HeaderText = If(isCurrentGroup, "I (A)", "U (V)")
        cFund.Name = "Col_Fund"
        cFund.Width = 58
        cFund.ReadOnly = True
        cFund.SortMode = DataGridViewColumnSortMode.NotSortable
        Dim csFund As New DataGridViewCellStyle()
        csFund.Alignment = DataGridViewContentAlignment.MiddleRight
        csFund.ForeColor = Drawing.Color.FromArgb(200, 220, 245)
        cFund.DefaultCellStyle = csFund
        _hdDgv.Columns.Add(cFund)

        ' Colonne THD global
        Dim cThd As New DataGridViewTextBoxColumn()
        cThd.HeaderText = "THD"
        cThd.Name = "Col_THD"
        cThd.Width = 48
        cThd.ReadOnly = True
        cThd.SortMode = DataGridViewColumnSortMode.NotSortable
        Dim csThd As New DataGridViewCellStyle()
        csThd.Alignment = DataGridViewContentAlignment.MiddleRight
        csThd.ForeColor = Drawing.Color.FromArgb(200, 220, 245)
        cThd.DefaultCellStyle = csThd
        _hdDgv.Columns.Add(cThd)

        ' Colonnes harmoniques H2..Hn
        If sg.SubGroupIndices.Count > 0 Then
            Dim firstGrp As CsvHdGroup = data.HdGroups(sg.SubGroupIndices(0))
            For oi As Integer = 0 To firstGrp.Orders.Length - 1
                Dim c As New DataGridViewTextBoxColumn()
                c.HeaderText = "H" & firstGrp.Orders(oi).ToString()
                c.Name = "Col_H" & firstGrp.Orders(oi).ToString()
                c.Width = 50
                c.ReadOnly = True
                c.SortMode = DataGridViewColumnSortMode.NotSortable
                _hdDgv.Columns.Add(c)
            Next
        End If

        ' Lignes
        Dim clrEven As Drawing.Color = Drawing.Color.FromArgb(16, 26, 46)
        Dim clrOdd As Drawing.Color = Drawing.Color.FromArgb(20, 32, 56)
        For li As Integer = 0 To sg.SubGroupIndices.Count - 1
            Dim gi As Integer = sg.SubGroupIndices(li)
            Dim ri As Integer = _hdDgv.Rows.Add()
            _hdDgv.Rows(ri).Cells(0).Value = data.HdGroups(gi).GroupName
            For c As Integer = 1 To _hdDgv.Columns.Count - 1
                _hdDgv.Rows(ri).Cells(c).Value = "--"
            Next
            _hdDgv.Rows(ri).DefaultCellStyle.BackColor = If(li Mod 2 = 0, clrEven, clrOdd)
        Next
    End Sub

    ' Gradient vert->rouge sur les colonnes harmoniques selon la valeur en %
    Private Sub HdDgv_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs)
        If e.RowIndex < 0 OrElse e.ColumnIndex < 2 Then Return  ' skip header + Group + Fund
        Dim colName As String = If(_hdDgv.Columns.Count > e.ColumnIndex,
                                   _hdDgv.Columns(e.ColumnIndex).Name, "")
        ' Seulement les colonnes harmoniques (Col_H...) et THD
        If Not colName.StartsWith("Col_H") AndAlso colName <> "Col_THD" Then Return

        Dim valObj As Object = e.Value
        If valObj Is Nothing Then Return
        Dim valStr As String = valObj.ToString().Trim()
        Dim valD As Double = 0.0
        If Not Double.TryParse(valStr, NumberStyles.Any,
                               CultureInfo.InvariantCulture, valD) Then Return

        ' Gradient : 0% = vert (0,180,80), 15%+ = rouge (220,50,30)
        Dim t As Double = Math.Min(1.0, valD / 15.0)
        Dim clrText As Drawing.Color = Drawing.Color.FromArgb(
            CInt(0 + t * 220),  ' R
            CInt(180 - t * 130),  ' G
            CInt(80 - t * 50))   ' B

        e.PaintBackground(e.CellBounds, True)
        Dim sf As New Drawing.StringFormat()
        sf.Alignment = Drawing.StringAlignment.Far
        sf.LineAlignment = Drawing.StringAlignment.Center
        Using br As New Drawing.SolidBrush(clrText)
            e.Graphics.DrawString(valStr, e.CellStyle.Font, br,
                New Drawing.RectangleF(e.CellBounds.X, e.CellBounds.Y,
                                       e.CellBounds.Width - 4, e.CellBounds.Height),
                sf)
        End Using
        e.Handled = True
    End Sub

    Private Sub HdSuperGroupBtn_Click(sender As Object, e As EventArgs)
        If _csvData Is Nothing Then Return
        Dim si As Integer = CInt(CType(sender, Button).Tag)
        _hdActiveSuper = si

        For Each ctrl As Control In Panel_HdGroupBar.Controls
            Dim b As Button = TryCast(ctrl, Button)
            If b IsNot Nothing Then
                b.BackColor = If(CInt(b.Tag) = si,
                    Drawing.Color.FromArgb(0, 90, 140),
                    Drawing.Color.FromArgb(28, 44, 70))
            End If
        Next

        RebuildDgvColumns(_csvData, si)
        PopulateHdRows(FindNearestTickIndex(_cursorX))
    End Sub

#End Region

#Region "Mise a jour au curseur"

    Friend Sub UpdateHdPanelAtCursor(xPos As Double)
        If _csvData Is Nothing OrElse Not _csvData.HasHd Then Return
        If _hdDgv Is Nothing Then Return
        If Label_CursorTime IsNot Nothing Then
            Label_CursorTime.Text = "t = " & xPos.ToString("0.00") & " s"
        End If
        PopulateHdRows(FindNearestTickIndex(xPos))
    End Sub

    Private Sub PopulateHdRows(tickIdx As Integer)
        If _hdDgv Is Nothing OrElse _hdDgv.Rows.Count = 0 Then Return
        If _csvData Is Nothing OrElse _hdActiveSuper >= _hdSuperGroups.Count Then Return

        Dim sg As HdSuperGroup = _hdSuperGroups(_hdActiveSuper)
        Dim isCurrentGroup As Boolean = sg.Label.ToUpper().Contains("HDI") OrElse
                                         sg.Label.ToUpper().Contains("CURRENT")

        For li As Integer = 0 To sg.SubGroupIndices.Count - 1
            If li >= _hdDgv.Rows.Count Then Exit For
            Dim gi As Integer = sg.SubGroupIndices(li)
            Dim grp As CsvHdGroup = _csvData.HdGroups(gi)
            Dim row As DataGridViewRow = _hdDgv.Rows(li)

            ' Colonne fondamentale : chercher le signal I ou U correspondant dans Signals
            Dim fundVal As String = "--"
            Dim thdVal As String = "--"
            For Each sig As CsvSignal In _csvData.Signals
                Dim sn As String = sig.Name.ToUpper()
                Dim gn As String = grp.GroupName.ToUpper()
                ' Extraire le suffixe (ex: "1", "2", "3", "N", "12"...)
                Dim suffix As String = If(gn.Length > 3, gn.Substring(3), "")  ' HDI1 -> 1, HDU12 -> 12

                If isCurrentGroup Then
                    ' Chercher THDi correspondant
                    If (sn.StartsWith("THDI") OrElse (sn.Contains("THD") AndAlso sn.Contains("I"))) AndAlso
                       (suffix = "" OrElse sn.EndsWith(suffix)) Then
                        If tickIdx >= 0 AndAlso tickIdx < sig.Values.Length Then
                            thdVal = sig.Values(tickIdx).ToString("0.0") & "%"
                        End If
                    End If
                    ' Chercher courant fondamental
                    If (sig.Unit.ToUpper() = "A") AndAlso
                       Not sn.Contains("THD") AndAlso Not sn.Contains("EPACK") AndAlso
                       (suffix = "" OrElse sn.EndsWith(suffix) OrElse sn.EndsWith("L" & suffix)) Then
                        If tickIdx >= 0 AndAlso tickIdx < sig.Values.Length Then
                            fundVal = sig.Values(tickIdx).ToString("0.000")
                        End If
                    End If
                Else
                    ' Chercher THDu correspondant
                    If (sn.StartsWith("THDU") OrElse (sn.Contains("THD") AndAlso sn.Contains("U"))) AndAlso
                       (suffix = "" OrElse sn.EndsWith(suffix)) Then
                        If tickIdx >= 0 AndAlso tickIdx < sig.Values.Length Then
                            thdVal = sig.Values(tickIdx).ToString("0.0") & "%"
                        End If
                    End If
                    ' Chercher tension fondamentale
                    If (sig.Unit.ToUpper() = "V") AndAlso
                       Not sn.Contains("THD") AndAlso Not sn.Contains("EPACK") AndAlso
                       (suffix = "" OrElse sn.EndsWith(suffix)) Then
                        If tickIdx >= 0 AndAlso tickIdx < sig.Values.Length Then
                            fundVal = sig.Values(tickIdx).ToString("0.00")
                        End If
                    End If
                End If
            Next

            If _hdDgv.Columns.Contains("Col_Fund") Then row.Cells("Col_Fund").Value = fundVal
            If _hdDgv.Columns.Contains("Col_THD") Then row.Cells("Col_THD").Value = thdVal

            ' Colonnes harmoniques H2..Hn — valeur sans "%"
            For oi As Integer = 0 To grp.Orders.Length - 1
                Dim colName As String = "Col_H" & grp.Orders(oi).ToString()
                If Not _hdDgv.Columns.Contains(colName) Then Continue For
                If grp.ValuesPerOrder(oi) IsNot Nothing AndAlso
                   tickIdx < grp.ValuesPerOrder(oi).Length Then
                    row.Cells(colName).Value = grp.ValuesPerOrder(oi)(tickIdx).ToString("0.0")
                Else
                    row.Cells(colName).Value = "--"
                End If
            Next
        Next
    End Sub

#End Region

End Class