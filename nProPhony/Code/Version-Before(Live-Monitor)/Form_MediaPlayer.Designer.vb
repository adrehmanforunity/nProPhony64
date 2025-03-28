<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form_MediaPlayer
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_MediaPlayer))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Combo_Type = New System.Windows.Forms.ComboBox()
        Me.Combo_Users = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Lbl_Count = New System.Windows.Forms.Label()
        Me.DateTimePicker_Start = New System.Windows.Forms.DateTimePicker()
        Me.TextBox_CLI = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.TextBox_Limit = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.DateTimePicker_End = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label_Status = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.TabControl = New System.Windows.Forms.TabControl()
        Me.Answered = New System.Windows.Forms.TabPage()
        Me.UnAnswered = New System.Windows.Forms.TabPage()
        Me.AllCalls = New System.Windows.Forms.TabPage()
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.Button_Play = New System.Windows.Forms.Button()
        Me.Button_Pause = New System.Windows.Forms.Button()
        Me.Button_Resume = New System.Windows.Forms.Button()
        Me.AxWindowsMediaPlayer1 = New AxWMPLib.AxWindowsMediaPlayer()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.colCallD = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colRecID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colDateTime = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colAcceptedOn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colHangupOn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colCallerID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colCalled = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colDirection = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colAgent = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colExtention = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colDuration = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colPlay = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.colDownload = New System.Windows.Forms.DataGridViewButtonColumn()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.TabControl.SuspendLayout()
        CType(Me.AxWindowsMediaPlayer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DataGridView1
        '
        Me.DataGridView1.BackgroundColor = System.Drawing.SystemColors.ActiveCaption
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colCallD, Me.colRecID, Me.colDateTime, Me.colAcceptedOn, Me.colHangupOn, Me.colCallerID, Me.colCalled, Me.colDirection, Me.colAgent, Me.colExtention, Me.colDuration, Me.colPlay, Me.colDownload})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Info
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Arial", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridView1.DefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridView1.Location = New System.Drawing.Point(0, 105)
        Me.DataGridView1.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.DataGridView1.MultiSelect = False
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.RowHeadersWidth = 51
        Me.DataGridView1.RowTemplate.Height = 24
        Me.DataGridView1.Size = New System.Drawing.Size(1129, 483)
        Me.DataGridView1.TabIndex = 1
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.GroupBox1.Controls.Add(Me.Combo_Type)
        Me.GroupBox1.Controls.Add(Me.Combo_Users)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.Lbl_Count)
        Me.GroupBox1.Controls.Add(Me.DateTimePicker_Start)
        Me.GroupBox1.Controls.Add(Me.TextBox_CLI)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.TextBox_Limit)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.DateTimePicker_End)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.Label_Status)
        Me.GroupBox1.Controls.Add(Me.Button1)
        Me.GroupBox1.Location = New System.Drawing.Point(1, 1)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.GroupBox1.Size = New System.Drawing.Size(1128, 69)
        Me.GroupBox1.TabIndex = 7
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Searching Options"
        '
        'Combo_Type
        '
        Me.Combo_Type.FormattingEnabled = True
        Me.Combo_Type.Items.AddRange(New Object() {"*", "In-Bound", "Out-Bound"})
        Me.Combo_Type.Location = New System.Drawing.Point(347, 45)
        Me.Combo_Type.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.Combo_Type.Name = "Combo_Type"
        Me.Combo_Type.Size = New System.Drawing.Size(180, 24)
        Me.Combo_Type.TabIndex = 18
        '
        'Combo_Users
        '
        Me.Combo_Users.FormattingEnabled = True
        Me.Combo_Users.Location = New System.Drawing.Point(347, 21)
        Me.Combo_Users.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.Combo_Users.Name = "Combo_Users"
        Me.Combo_Users.Size = New System.Drawing.Size(180, 24)
        Me.Combo_Users.TabIndex = 16
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Arial", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(946, 38)
        Me.Label7.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(149, 22)
        Me.Label7.TabIndex = 30
        Me.Label7.Text = "Found:"
        '
        'Lbl_Count
        '
        Me.Lbl_Count.Font = New System.Drawing.Font("Arial", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lbl_Count.Location = New System.Drawing.Point(1000, 38)
        Me.Lbl_Count.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Lbl_Count.Name = "Lbl_Count"
        Me.Lbl_Count.Size = New System.Drawing.Size(107, 22)
        Me.Lbl_Count.TabIndex = 29
        '
        'DateTimePicker_Start
        '
        Me.DateTimePicker_Start.CustomFormat = "yyyy-MM-dd HH:mm:ss"
        Me.DateTimePicker_Start.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimePicker_Start.Location = New System.Drawing.Point(94, 21)
        Me.DateTimePicker_Start.Name = "DateTimePicker_Start"
        Me.DateTimePicker_Start.Size = New System.Drawing.Size(181, 22)
        Me.DateTimePicker_Start.TabIndex = 24
        '
        'TextBox_CLI
        '
        Me.TextBox_CLI.Location = New System.Drawing.Point(592, 45)
        Me.TextBox_CLI.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.TextBox_CLI.Name = "TextBox_CLI"
        Me.TextBox_CLI.Size = New System.Drawing.Size(132, 22)
        Me.TextBox_CLI.TabIndex = 23
        Me.TextBox_CLI.Text = "*"
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Arial", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(556, 45)
        Me.Label6.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(43, 22)
        Me.Label6.TabIndex = 22
        Me.Label6.Text = "CLI"
        '
        'TextBox_Limit
        '
        Me.TextBox_Limit.Location = New System.Drawing.Point(592, 21)
        Me.TextBox_Limit.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.TextBox_Limit.Name = "TextBox_Limit"
        Me.TextBox_Limit.Size = New System.Drawing.Size(55, 22)
        Me.TextBox_Limit.TabIndex = 21
        Me.TextBox_Limit.Text = "100"
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Arial", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(556, 21)
        Me.Label5.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(43, 22)
        Me.Label5.TabIndex = 20
        Me.Label5.Text = "Limit"
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Arial", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(305, 45)
        Me.Label4.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(44, 22)
        Me.Label4.TabIndex = 19
        Me.Label4.Text = "Type"
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Arial", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(305, 21)
        Me.Label3.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(44, 22)
        Me.Label3.TabIndex = 17
        Me.Label3.Text = "Agent"
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Arial", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(5, 45)
        Me.Label2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(88, 22)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "End Date Time"
        '
        'DateTimePicker_End
        '
        Me.DateTimePicker_End.CustomFormat = "yyyy-MM-dd HH:mm:ss"
        Me.DateTimePicker_End.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimePicker_End.Location = New System.Drawing.Point(94, 45)
        Me.DateTimePicker_End.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.DateTimePicker_End.Name = "DateTimePicker_End"
        Me.DateTimePicker_End.Size = New System.Drawing.Size(181, 22)
        Me.DateTimePicker_End.TabIndex = 12
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Arial", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(6, 21)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(88, 22)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Start Date Time"
        '
        'Label_Status
        '
        Me.Label_Status.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_Status.Location = New System.Drawing.Point(845, 11)
        Me.Label_Status.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label_Status.Name = "Label_Status"
        Me.Label_Status.Size = New System.Drawing.Size(276, 17)
        Me.Label_Status.TabIndex = 10
        Me.Label_Status.Text = "Status: Select date and click find"
        Me.Label_Status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Location = New System.Drawing.Point(728, 21)
        Me.Button1.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(107, 42)
        Me.Button1.TabIndex = 7
        Me.Button1.Text = "&Find"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'TabControl
        '
        Me.TabControl.Controls.Add(Me.Answered)
        Me.TabControl.Controls.Add(Me.UnAnswered)
        Me.TabControl.Controls.Add(Me.AllCalls)
        Me.TabControl.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabControl.Location = New System.Drawing.Point(1, 76)
        Me.TabControl.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.TabControl.Name = "TabControl"
        Me.TabControl.SelectedIndex = 0
        Me.TabControl.Size = New System.Drawing.Size(1500, 27)
        Me.TabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.TabControl.TabIndex = 22
        Me.TabControl.Visible = False
        '
        'Answered
        '
        Me.Answered.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Answered.Location = New System.Drawing.Point(4, 27)
        Me.Answered.Margin = New System.Windows.Forms.Padding(10, 3, 10, 3)
        Me.Answered.Name = "Answered"
        Me.Answered.Padding = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.Answered.Size = New System.Drawing.Size(1492, 0)
        Me.Answered.TabIndex = 0
        Me.Answered.Text = "Answered Calls"
        Me.Answered.UseVisualStyleBackColor = True
        '
        'UnAnswered
        '
        Me.UnAnswered.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.UnAnswered.Location = New System.Drawing.Point(4, 27)
        Me.UnAnswered.Margin = New System.Windows.Forms.Padding(10, 3, 10, 3)
        Me.UnAnswered.Name = "UnAnswered"
        Me.UnAnswered.Padding = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.UnAnswered.Size = New System.Drawing.Size(1492, 0)
        Me.UnAnswered.TabIndex = 1
        Me.UnAnswered.Text = "UnAnswered"
        Me.UnAnswered.UseVisualStyleBackColor = True
        '
        'AllCalls
        '
        Me.AllCalls.Location = New System.Drawing.Point(4, 27)
        Me.AllCalls.Margin = New System.Windows.Forms.Padding(10, 3, 10, 3)
        Me.AllCalls.Name = "AllCalls"
        Me.AllCalls.Size = New System.Drawing.Size(1492, 0)
        Me.AllCalls.TabIndex = 2
        Me.AllCalls.Text = "All"
        Me.AllCalls.UseVisualStyleBackColor = True
        '
        'Button_Play
        '
        Me.Button_Play.Location = New System.Drawing.Point(5, 499)
        Me.Button_Play.Name = "Button_Play"
        Me.Button_Play.Size = New System.Drawing.Size(75, 54)
        Me.Button_Play.TabIndex = 23
        Me.Button_Play.Text = "Play"
        Me.Button_Play.UseVisualStyleBackColor = True
        Me.Button_Play.Visible = False
        '
        'Button_Pause
        '
        Me.Button_Pause.Location = New System.Drawing.Point(95, 499)
        Me.Button_Pause.Name = "Button_Pause"
        Me.Button_Pause.Size = New System.Drawing.Size(75, 54)
        Me.Button_Pause.TabIndex = 24
        Me.Button_Pause.Text = "Pause"
        Me.Button_Pause.UseVisualStyleBackColor = True
        Me.Button_Pause.Visible = False
        '
        'Button_Resume
        '
        Me.Button_Resume.Location = New System.Drawing.Point(185, 499)
        Me.Button_Resume.Name = "Button_Resume"
        Me.Button_Resume.Size = New System.Drawing.Size(75, 54)
        Me.Button_Resume.TabIndex = 25
        Me.Button_Resume.Text = "Resume"
        Me.Button_Resume.UseVisualStyleBackColor = True
        Me.Button_Resume.Visible = False
        '
        'AxWindowsMediaPlayer1
        '
        Me.AxWindowsMediaPlayer1.Enabled = True
        Me.AxWindowsMediaPlayer1.Location = New System.Drawing.Point(0, 592)
        Me.AxWindowsMediaPlayer1.Name = "AxWindowsMediaPlayer1"
        Me.AxWindowsMediaPlayer1.OcxState = CType(resources.GetObject("AxWindowsMediaPlayer1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxWindowsMediaPlayer1.Size = New System.Drawing.Size(1129, 44)
        Me.AxWindowsMediaPlayer1.TabIndex = 29
        '
        'colCallD
        '
        Me.colCallD.HeaderText = "CallID"
        Me.colCallD.MinimumWidth = 6
        Me.colCallD.Name = "colCallD"
        Me.colCallD.ReadOnly = True
        Me.colCallD.Width = 80
        '
        'colRecID
        '
        Me.colRecID.HeaderText = "RecID"
        Me.colRecID.MinimumWidth = 6
        Me.colRecID.Name = "colRecID"
        Me.colRecID.ReadOnly = True
        Me.colRecID.Width = 80
        '
        'colDateTime
        '
        Me.colDateTime.HeaderText = "Date/Time"
        Me.colDateTime.MinimumWidth = 6
        Me.colDateTime.Name = "colDateTime"
        Me.colDateTime.ReadOnly = True
        Me.colDateTime.Width = 125
        '
        'colAcceptedOn
        '
        Me.colAcceptedOn.HeaderText = "AcceptedOn"
        Me.colAcceptedOn.MinimumWidth = 6
        Me.colAcceptedOn.Name = "colAcceptedOn"
        Me.colAcceptedOn.ReadOnly = True
        Me.colAcceptedOn.Width = 125
        '
        'colHangupOn
        '
        Me.colHangupOn.HeaderText = "Hangup On"
        Me.colHangupOn.MinimumWidth = 6
        Me.colHangupOn.Name = "colHangupOn"
        Me.colHangupOn.ReadOnly = True
        Me.colHangupOn.Width = 125
        '
        'colCallerID
        '
        Me.colCallerID.HeaderText = "CallerID"
        Me.colCallerID.MinimumWidth = 6
        Me.colCallerID.Name = "colCallerID"
        Me.colCallerID.ReadOnly = True
        Me.colCallerID.Width = 125
        '
        'colCalled
        '
        Me.colCalled.HeaderText = "Called"
        Me.colCalled.MinimumWidth = 6
        Me.colCalled.Name = "colCalled"
        Me.colCalled.ReadOnly = True
        Me.colCalled.Width = 125
        '
        'colDirection
        '
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.colDirection.DefaultCellStyle = DataGridViewCellStyle1
        Me.colDirection.HeaderText = "Direction"
        Me.colDirection.MinimumWidth = 6
        Me.colDirection.Name = "colDirection"
        Me.colDirection.ReadOnly = True
        Me.colDirection.Width = 80
        '
        'colAgent
        '
        Me.colAgent.HeaderText = "Agent"
        Me.colAgent.MinimumWidth = 6
        Me.colAgent.Name = "colAgent"
        Me.colAgent.ReadOnly = True
        Me.colAgent.Width = 125
        '
        'colExtention
        '
        Me.colExtention.HeaderText = "Extention"
        Me.colExtention.MinimumWidth = 6
        Me.colExtention.Name = "colExtention"
        Me.colExtention.ReadOnly = True
        Me.colExtention.Width = 80
        '
        'colDuration
        '
        Me.colDuration.HeaderText = "Duration"
        Me.colDuration.MinimumWidth = 6
        Me.colDuration.Name = "colDuration"
        Me.colDuration.ReadOnly = True
        Me.colDuration.Width = 50
        '
        'colPlay
        '
        Me.colPlay.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.colPlay.HeaderText = "."
        Me.colPlay.MinimumWidth = 6
        Me.colPlay.Name = "colPlay"
        Me.colPlay.ReadOnly = True
        Me.colPlay.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.colPlay.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.colPlay.Width = 80
        '
        'colDownload
        '
        Me.colDownload.HeaderText = "."
        Me.colDownload.MinimumWidth = 6
        Me.colDownload.Name = "colDownload"
        Me.colDownload.ReadOnly = True
        Me.colDownload.Width = 80
        '
        'Form_MediaPlayer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1136, 640)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.AxWindowsMediaPlayer1)
        Me.Controls.Add(Me.Button_Resume)
        Me.Controls.Add(Me.Button_Pause)
        Me.Controls.Add(Me.Button_Play)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.TabControl)
        Me.Font = New System.Drawing.Font("Arial", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.MaximizeBox = False
        Me.Name = "Form_MediaPlayer"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Contact Player (beta)"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.TabControl.ResumeLayout(False)
        CType(Me.AxWindowsMediaPlayer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Combo_Type As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Combo_Users As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents DateTimePicker_End As DateTimePicker
    Friend WithEvents Label1 As Label
    Friend WithEvents Label_Status As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents TextBox_Limit As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents TabControl As TabControl
    Friend WithEvents Answered As TabPage
    Friend WithEvents UnAnswered As TabPage
    Friend WithEvents AllCalls As TabPage
    Friend WithEvents TextBox_CLI As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents DateTimePicker_Start As DateTimePicker
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents AxWindowsMediaPlayer1 As AxWMPLib.AxWindowsMediaPlayer
    Friend WithEvents Button_Play As Button
    Friend WithEvents Button_Pause As Button
    Friend WithEvents Button_Resume As Button
    Friend WithEvents Label7 As Label
    Friend WithEvents Lbl_Count As Label
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents colCallD As DataGridViewTextBoxColumn
    Friend WithEvents colRecID As DataGridViewTextBoxColumn
    Friend WithEvents colDateTime As DataGridViewTextBoxColumn
    Friend WithEvents colAcceptedOn As DataGridViewTextBoxColumn
    Friend WithEvents colHangupOn As DataGridViewTextBoxColumn
    Friend WithEvents colCallerID As DataGridViewTextBoxColumn
    Friend WithEvents colCalled As DataGridViewTextBoxColumn
    Friend WithEvents colDirection As DataGridViewTextBoxColumn
    Friend WithEvents colAgent As DataGridViewTextBoxColumn
    Friend WithEvents colExtention As DataGridViewTextBoxColumn
    Friend WithEvents colDuration As DataGridViewTextBoxColumn
    Friend WithEvents colPlay As DataGridViewButtonColumn
    Friend WithEvents colDownload As DataGridViewButtonColumn
End Class
