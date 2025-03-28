<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_AudioSetup
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_AudioSetup))
        Me.Label_Mic = New System.Windows.Forms.Label()
        Me.ComboBox_Mic = New System.Windows.Forms.ComboBox()
        Me.ComboBox_Speaker = New System.Windows.Forms.ComboBox()
        Me.Label_Speaker = New System.Windows.Forms.Label()
        Me.Button_Apply = New System.Windows.Forms.Button()
        Me.Label_msg = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label_Mic
        '
        Me.Label_Mic.Location = New System.Drawing.Point(11, 10)
        Me.Label_Mic.Name = "Label_Mic"
        Me.Label_Mic.Size = New System.Drawing.Size(100, 23)
        Me.Label_Mic.TabIndex = 0
        Me.Label_Mic.Text = "Mic:"
        Me.Label_Mic.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ComboBox_Mic
        '
        Me.ComboBox_Mic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Mic.FormattingEnabled = True
        Me.ComboBox_Mic.Location = New System.Drawing.Point(117, 9)
        Me.ComboBox_Mic.Name = "ComboBox_Mic"
        Me.ComboBox_Mic.Size = New System.Drawing.Size(399, 24)
        Me.ComboBox_Mic.TabIndex = 1
        '
        'ComboBox_Speaker
        '
        Me.ComboBox_Speaker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Speaker.FormattingEnabled = True
        Me.ComboBox_Speaker.Location = New System.Drawing.Point(117, 39)
        Me.ComboBox_Speaker.Name = "ComboBox_Speaker"
        Me.ComboBox_Speaker.Size = New System.Drawing.Size(399, 24)
        Me.ComboBox_Speaker.TabIndex = 3
        '
        'Label_Speaker
        '
        Me.Label_Speaker.Location = New System.Drawing.Point(11, 40)
        Me.Label_Speaker.Name = "Label_Speaker"
        Me.Label_Speaker.Size = New System.Drawing.Size(100, 23)
        Me.Label_Speaker.TabIndex = 2
        Me.Label_Speaker.Text = "Speaker:"
        Me.Label_Speaker.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Button_Apply
        '
        Me.Button_Apply.Location = New System.Drawing.Point(441, 69)
        Me.Button_Apply.Name = "Button_Apply"
        Me.Button_Apply.Size = New System.Drawing.Size(74, 25)
        Me.Button_Apply.TabIndex = 5
        Me.Button_Apply.Text = "&Apply"
        Me.Button_Apply.UseVisualStyleBackColor = True
        '
        'Label_msg
        '
        Me.Label_msg.Location = New System.Drawing.Point(14, 69)
        Me.Label_msg.Name = "Label_msg"
        Me.Label_msg.Size = New System.Drawing.Size(419, 23)
        Me.Label_msg.TabIndex = 6
        Me.Label_msg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Form_AudioSetup
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(529, 106)
        Me.Controls.Add(Me.Label_msg)
        Me.Controls.Add(Me.Button_Apply)
        Me.Controls.Add(Me.ComboBox_Speaker)
        Me.Controls.Add(Me.Label_Speaker)
        Me.Controls.Add(Me.ComboBox_Mic)
        Me.Controls.Add(Me.Label_Mic)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "Form_AudioSetup"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Setup Audio"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Label_Mic As Label
    Friend WithEvents ComboBox_Mic As ComboBox
    Friend WithEvents ComboBox_Speaker As ComboBox
    Friend WithEvents Label_Speaker As Label
    Friend WithEvents Button_Apply As Button
    Friend WithEvents Label_msg As Label
End Class
