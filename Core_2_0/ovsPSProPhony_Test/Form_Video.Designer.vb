<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_Video
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
        Me.remoteVideoPanel = New System.Windows.Forms.Panel()
        Me.localVideoPanel = New System.Windows.Forms.Panel()
        Me.remoteVideoPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'remoteVideoPanel
        '
        Me.remoteVideoPanel.BackColor = System.Drawing.Color.Black
        Me.remoteVideoPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.remoteVideoPanel.Controls.Add(Me.localVideoPanel)
        Me.remoteVideoPanel.Location = New System.Drawing.Point(12, 12)
        Me.remoteVideoPanel.Name = "remoteVideoPanel"
        Me.remoteVideoPanel.Size = New System.Drawing.Size(800, 600)
        Me.remoteVideoPanel.TabIndex = 44
        '
        'localVideoPanel
        '
        Me.localVideoPanel.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.localVideoPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.localVideoPanel.ForeColor = System.Drawing.Color.Black
        Me.localVideoPanel.Location = New System.Drawing.Point(542, 16)
        Me.localVideoPanel.Name = "localVideoPanel"
        Me.localVideoPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.localVideoPanel.Size = New System.Drawing.Size(176, 144)
        Me.localVideoPanel.TabIndex = 46
        '
        'Form_Video
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(809, 611)
        Me.Controls.Add(Me.remoteVideoPanel)
        Me.MaximumSize = New System.Drawing.Size(1305, 770)
        Me.Name = "Form_Video"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Video Sessin Viewer"
        Me.WindowState = System.Windows.Forms.FormWindowState.Minimized
        Me.remoteVideoPanel.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Private WithEvents remoteVideoPanel As Panel
    Private WithEvents localVideoPanel As Panel
End Class
