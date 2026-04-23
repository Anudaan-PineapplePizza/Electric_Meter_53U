Option Strict On
Option Explicit On

' =============================================================================
' HistoricalWindow.Designer.vb  -- Partial Class (2/6)
' =============================================================================
Partial Class HistoricalWindow

    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then components.Dispose()
        MyBase.Dispose(disposing)
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Panel_Header = New System.Windows.Forms.Panel()
        Me.Label_Title = New System.Windows.Forms.Label()
        Me.Button_Open = New System.Windows.Forms.Button()
        Me.Button_Zoom = New System.Windows.Forms.Button()
        Me.Button_Cursor = New System.Windows.Forms.Button()
        Me.Button_Axes = New System.Windows.Forms.Button()
        Me.Label_CursorTime = New System.Windows.Forms.Label()
        Me.Panel_AxesDrawer = New System.Windows.Forms.Panel()
        Me.Panel_HdBand = New System.Windows.Forms.Panel()
        Me.Panel_HdGroupBar = New System.Windows.Forms.Panel()
        Me.Panel_HdGrid = New System.Windows.Forms.Panel()
        Me.SplitMain = New System.Windows.Forms.SplitContainer()
        Me.GroupBox_Values = New System.Windows.Forms.GroupBox()
        Me.Panel_Values = New System.Windows.Forms.Panel()
        Me.TabControl_Charts = New System.Windows.Forms.TabControl()

        Me.Panel_Header.SuspendLayout()
        Me.Panel_HdBand.SuspendLayout()
        CType(Me.SplitMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitMain.Panel1.SuspendLayout()
        Me.SplitMain.Panel2.SuspendLayout()
        Me.SplitMain.SuspendLayout()
        Me.GroupBox_Values.SuspendLayout()
        Me.SuspendLayout()

        ' Panel_Header
        Me.Panel_Header.BackColor = System.Drawing.Color.FromArgb(18, 28, 50)
        Me.Panel_Header.Controls.Add(Me.Label_Title)
        Me.Panel_Header.Controls.Add(Me.Button_Open)
        Me.Panel_Header.Controls.Add(Me.Button_Zoom)
        Me.Panel_Header.Controls.Add(Me.Button_Cursor)
        Me.Panel_Header.Controls.Add(Me.Button_Axes)
        Me.Panel_Header.Controls.Add(Me.Label_CursorTime)
        Me.Panel_Header.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel_Header.Location = New System.Drawing.Point(0, 0)
        Me.Panel_Header.Name = "Panel_Header"
        Me.Panel_Header.Size = New System.Drawing.Size(1272, 46)

        Me.Label_Title.AutoSize = True
        Me.Label_Title.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.Label_Title.ForeColor = System.Drawing.Color.FromArgb(200, 215, 240)
        Me.Label_Title.Location = New System.Drawing.Point(14, 13)
        Me.Label_Title.Name = "Label_Title"
        Me.Label_Title.Text = "HISTORICAL VIEWER"

        Me.Button_Open.BackColor = System.Drawing.Color.FromArgb(0, 100, 60)
        Me.Button_Open.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button_Open.FlatAppearance.BorderSize = 0
        Me.Button_Open.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button_Open.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold)
        Me.Button_Open.ForeColor = System.Drawing.Color.White
        Me.Button_Open.Location = New System.Drawing.Point(200, 9)
        Me.Button_Open.Name = "Button_Open"
        Me.Button_Open.Size = New System.Drawing.Size(110, 28)
        Me.Button_Open.Text = "Open CSV..."
        Me.Button_Open.UseVisualStyleBackColor = False

        Me.Button_Zoom.BackColor = System.Drawing.Color.FromArgb(0, 90, 140)
        Me.Button_Zoom.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button_Zoom.FlatAppearance.BorderSize = 0
        Me.Button_Zoom.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button_Zoom.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold)
        Me.Button_Zoom.ForeColor = System.Drawing.Color.White
        Me.Button_Zoom.Location = New System.Drawing.Point(322, 9)
        Me.Button_Zoom.Name = "Button_Zoom"
        Me.Button_Zoom.Size = New System.Drawing.Size(80, 28)
        Me.Button_Zoom.Text = "Zoom"
        Me.Button_Zoom.UseVisualStyleBackColor = False

        Me.Button_Cursor.BackColor = System.Drawing.Color.FromArgb(28, 44, 70)
        Me.Button_Cursor.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button_Cursor.FlatAppearance.BorderSize = 0
        Me.Button_Cursor.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button_Cursor.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold)
        Me.Button_Cursor.ForeColor = System.Drawing.Color.FromArgb(200, 215, 240)
        Me.Button_Cursor.Location = New System.Drawing.Point(408, 9)
        Me.Button_Cursor.Name = "Button_Cursor"
        Me.Button_Cursor.Size = New System.Drawing.Size(80, 28)
        Me.Button_Cursor.Text = "Cursor"
        Me.Button_Cursor.UseVisualStyleBackColor = False

        ' Button_Axes
        Me.Button_Axes.BackColor = System.Drawing.Color.FromArgb(28, 44, 70)
        Me.Button_Axes.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button_Axes.FlatAppearance.BorderSize = 0
        Me.Button_Axes.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button_Axes.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold)
        Me.Button_Axes.ForeColor = System.Drawing.Color.FromArgb(200, 215, 240)
        Me.Button_Axes.Location = New System.Drawing.Point(494, 9)
        Me.Button_Axes.Name = "Button_Axes"
        Me.Button_Axes.Size = New System.Drawing.Size(70, 28)
        Me.Button_Axes.Text = "Axes"
        Me.Button_Axes.UseVisualStyleBackColor = False

        ' Label_CursorTime
        Me.Label_CursorTime.AutoSize = True
        Me.Label_CursorTime.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label_CursorTime.ForeColor = System.Drawing.Color.FromArgb(0, 210, 180)
        Me.Label_CursorTime.Location = New System.Drawing.Point(578, 15)
        Me.Label_CursorTime.Name = "Label_CursorTime"
        Me.Label_CursorTime.Text = "t = --"

        ' Panel_AxesDrawer (drawer lateral, masque par defaut, positionne a droite du TabControl)
        Me.Panel_AxesDrawer.BackColor = System.Drawing.Color.FromArgb(16, 26, 50)
        Me.Panel_AxesDrawer.Name = "Panel_AxesDrawer"
        Me.Panel_AxesDrawer.Size = New System.Drawing.Size(300, 600)
        Me.Panel_AxesDrawer.Visible = False

        ' Panel_HdBand (Bottom)
        Me.Panel_HdBand.BackColor = System.Drawing.Color.FromArgb(10, 16, 28)
        Me.Panel_HdBand.Controls.Add(Me.Panel_HdGrid)
        Me.Panel_HdBand.Controls.Add(Me.Panel_HdGroupBar)
        Me.Panel_HdBand.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel_HdBand.Name = "Panel_HdBand"
        Me.Panel_HdBand.Size = New System.Drawing.Size(1272, 180)
        Me.Panel_HdBand.Visible = False   ' masque jusqu au chargement d un CSV avec HD

        Me.Panel_HdGroupBar.BackColor = System.Drawing.Color.FromArgb(18, 28, 50)
        Me.Panel_HdGroupBar.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel_HdGroupBar.Height = 32
        Me.Panel_HdGroupBar.Name = "Panel_HdGroupBar"

        Me.Panel_HdGrid.BackColor = System.Drawing.Color.FromArgb(12, 18, 32)
        Me.Panel_HdGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel_HdGrid.Name = "Panel_HdGrid"

        ' SplitMain (Fill, entre header et HD band)
        Me.SplitMain.BackColor = System.Drawing.Color.FromArgb(25, 38, 62)
        Me.SplitMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitMain.Location = New System.Drawing.Point(0, 46)
        Me.SplitMain.Name = "SplitMain"
        Me.SplitMain.Orientation = System.Windows.Forms.Orientation.Vertical
        Me.SplitMain.Panel1MinSize = 180
        Me.SplitMain.Panel2MinSize = 400
        Me.SplitMain.Size = New System.Drawing.Size(1272, 554)
        Me.SplitMain.SplitterDistance = 230
        Me.SplitMain.SplitterWidth = 4

        ' Panel1 : Values
        Me.SplitMain.Panel1.BackColor = System.Drawing.Color.FromArgb(10, 16, 30)
        Me.SplitMain.Panel1.Controls.Add(Me.GroupBox_Values)

        Me.GroupBox_Values.BackColor = System.Drawing.Color.FromArgb(16, 24, 42)
        Me.GroupBox_Values.Controls.Add(Me.Panel_Values)
        Me.GroupBox_Values.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox_Values.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.GroupBox_Values.Font = New System.Drawing.Font("Segoe UI", 7.5!, System.Drawing.FontStyle.Bold)
        Me.GroupBox_Values.ForeColor = System.Drawing.Color.FromArgb(90, 115, 165)
        Me.GroupBox_Values.Name = "GroupBox_Values"
        Me.GroupBox_Values.TabStop = False
        Me.GroupBox_Values.Text = "  VALUES AT CURSOR"

        Me.Panel_Values.AutoScroll = True
        Me.Panel_Values.BackColor = System.Drawing.Color.FromArgb(12, 20, 36)
        Me.Panel_Values.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel_Values.Name = "Panel_Values"

        ' Panel2 : TabControl des graphiques
        Me.SplitMain.Panel2.BackColor = System.Drawing.Color.FromArgb(10, 16, 30)
        Me.SplitMain.Panel2.Controls.Add(Me.TabControl_Charts)

        Me.TabControl_Charts.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl_Charts.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed
        Me.TabControl_Charts.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular)
        Me.TabControl_Charts.ItemSize = New System.Drawing.Size(130, 26)
        Me.TabControl_Charts.Name = "TabControl_Charts"

        ' Formulaire
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(10, 16, 30)
        Me.ClientSize = New System.Drawing.Size(1272, 780)
        Me.Controls.Add(Me.SplitMain)
        Me.Controls.Add(Me.Panel_AxesDrawer)
        Me.Controls.Add(Me.Panel_HdBand)
        Me.Controls.Add(Me.Panel_Header)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.5!)
        Me.ForeColor = System.Drawing.Color.FromArgb(200, 215, 240)
        Me.MinimumSize = New System.Drawing.Size(900, 600)
        Me.Name = "HistoricalWindow"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Historical Viewer"

        Me.Panel_Header.ResumeLayout(False)
        Me.Panel_Header.PerformLayout()
        Me.Panel_HdBand.ResumeLayout(False)
        Me.SplitMain.Panel1.ResumeLayout(False)
        Me.SplitMain.Panel2.ResumeLayout(False)
        CType(Me.SplitMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitMain.ResumeLayout(False)
        Me.GroupBox_Values.ResumeLayout(False)
        Me.ResumeLayout(False)
    End Sub

    Friend WithEvents Panel_Header As System.Windows.Forms.Panel
    Friend WithEvents Label_Title As System.Windows.Forms.Label
    Friend WithEvents Button_Open As System.Windows.Forms.Button
    Friend WithEvents Button_Zoom As System.Windows.Forms.Button
    Friend WithEvents Button_Cursor As System.Windows.Forms.Button
    Friend WithEvents Label_CursorTime As System.Windows.Forms.Label
    Friend WithEvents Button_Axes As System.Windows.Forms.Button
    Friend WithEvents Panel_AxesDrawer As System.Windows.Forms.Panel
    Friend WithEvents Panel_HdBand As System.Windows.Forms.Panel
    Friend WithEvents Panel_HdGroupBar As System.Windows.Forms.Panel
    Friend WithEvents Panel_HdGrid As System.Windows.Forms.Panel
    Friend WithEvents SplitMain As System.Windows.Forms.SplitContainer
    Friend WithEvents GroupBox_Values As System.Windows.Forms.GroupBox
    Friend WithEvents Panel_Values As System.Windows.Forms.Panel
    Friend WithEvents TabControl_Charts As System.Windows.Forms.TabControl

End Class