<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form_Splash
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_Splash))
        Me.Label_Status = New System.Windows.Forms.Label()
        Me.GroupBox_Login = New System.Windows.Forms.GroupBox()
        Me.Button_Login = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TextBox_Password = New System.Windows.Forms.TextBox()
        Me.TextBox_LoginName = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.GroupBox_Title = New System.Windows.Forms.GroupBox()
        Me.Label_AgentStatus = New System.Windows.Forms.Label()
        Me.Label_AgentMode = New System.Windows.Forms.Label()
        Me.Label_Version = New System.Windows.Forms.Label()
        Me.Label_AppName = New System.Windows.Forms.Label()
        Me.GroupBox_Mode = New System.Windows.Forms.GroupBox()
        Me.Button_AudioSetup = New System.Windows.Forms.Button()
        Me.Button_Logout = New System.Windows.Forms.Button()
        Me.Button_NR_Meeting = New System.Windows.Forms.Button()
        Me.Button_NR_Prayer = New System.Windows.Forms.Button()
        Me.Button_NR_Break = New System.Windows.Forms.Button()
        Me.Button_NR_Lunch = New System.Windows.Forms.Button()
        Me.Button_Outbound = New System.Windows.Forms.Button()
        Me.Button_Inbound = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.GroupBox_Workcode = New System.Windows.Forms.GroupBox()
        Me.ListView_Workcodes = New System.Windows.Forms.ListView()
        Me.Button_WCClose = New System.Windows.Forms.Button()
        Me.Label_WCCaption = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.GroupBox_Call = New System.Windows.Forms.GroupBox()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button_Call_Hangup = New System.Windows.Forms.Button()
        Me.Button_Call_Conf = New System.Windows.Forms.Button()
        Me.Button_Call_Trans = New System.Windows.Forms.Button()
        Me.Button_Call_Hold = New System.Windows.Forms.Button()
        Me.Button_Call_Mute = New System.Windows.Forms.Button()
        Me.Button_Call_Connect = New System.Windows.Forms.Button()
        Me.Label_Select_WC = New System.Windows.Forms.PictureBox()
        Me.Label_Select_DND = New System.Windows.Forms.PictureBox()
        Me.ProgressBar_Answer = New System.Windows.Forms.ProgressBar()
        Me.TextBox_Number = New System.Windows.Forms.TextBox()
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.GroupBox_Login.SuspendLayout()
        Me.GroupBox_Title.SuspendLayout()
        Me.GroupBox_Mode.SuspendLayout()
        Me.GroupBox_Workcode.SuspendLayout()
        Me.GroupBox_Call.SuspendLayout()
        CType(Me.Label_Select_WC, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label_Select_DND, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label_Status
        '
        Me.Label_Status.BackColor = System.Drawing.Color.Transparent
        Me.Label_Status.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Label_Status.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_Status.ForeColor = System.Drawing.Color.White
        Me.Label_Status.Location = New System.Drawing.Point(0, 198)
        Me.Label_Status.Name = "Label_Status"
        Me.Label_Status.Size = New System.Drawing.Size(358, 25)
        Me.Label_Status.TabIndex = 2
        Me.Label_Status.Text = "Status here"
        '
        'GroupBox_Login
        '
        Me.GroupBox_Login.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox_Login.Controls.Add(Me.Button_Login)
        Me.GroupBox_Login.Controls.Add(Me.Label5)
        Me.GroupBox_Login.Controls.Add(Me.TextBox_Password)
        Me.GroupBox_Login.Controls.Add(Me.TextBox_LoginName)
        Me.GroupBox_Login.Controls.Add(Me.Label4)
        Me.GroupBox_Login.Controls.Add(Me.Label3)
        Me.GroupBox_Login.Enabled = False
        Me.GroupBox_Login.Location = New System.Drawing.Point(482, 242)
        Me.GroupBox_Login.Name = "GroupBox_Login"
        Me.GroupBox_Login.Size = New System.Drawing.Size(480, 220)
        Me.GroupBox_Login.TabIndex = 3
        Me.GroupBox_Login.TabStop = False
        '
        'Button_Login
        '
        Me.Button_Login.Enabled = False
        Me.Button_Login.Location = New System.Drawing.Point(308, 149)
        Me.Button_Login.Name = "Button_Login"
        Me.Button_Login.Size = New System.Drawing.Size(72, 28)
        Me.Button_Login.TabIndex = 2
        Me.Button_Login.Text = "&Login"
        Me.Button_Login.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label5.Location = New System.Drawing.Point(6, 18)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(481, 21)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Please login now."
        '
        'TextBox_Password
        '
        Me.TextBox_Password.Enabled = False
        Me.TextBox_Password.Location = New System.Drawing.Point(222, 110)
        Me.TextBox_Password.Name = "TextBox_Password"
        Me.TextBox_Password.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TextBox_Password.Size = New System.Drawing.Size(158, 22)
        Me.TextBox_Password.TabIndex = 1
        '
        'TextBox_LoginName
        '
        Me.TextBox_LoginName.Enabled = False
        Me.TextBox_LoginName.Location = New System.Drawing.Point(222, 79)
        Me.TextBox_LoginName.Name = "TextBox_LoginName"
        Me.TextBox_LoginName.Size = New System.Drawing.Size(158, 22)
        Me.TextBox_LoginName.TabIndex = 0
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(70, 109)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(158, 22)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Password :"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(70, 78)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(158, 22)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Login Name :"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'GroupBox_Title
        '
        Me.GroupBox_Title.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox_Title.Controls.Add(Me.Label_AgentStatus)
        Me.GroupBox_Title.Controls.Add(Me.Label_AgentMode)
        Me.GroupBox_Title.Controls.Add(Me.Label_Version)
        Me.GroupBox_Title.Controls.Add(Me.Label_AppName)
        Me.GroupBox_Title.Location = New System.Drawing.Point(-1, -8)
        Me.GroupBox_Title.Name = "GroupBox_Title"
        Me.GroupBox_Title.Size = New System.Drawing.Size(481, 58)
        Me.GroupBox_Title.TabIndex = 5
        Me.GroupBox_Title.TabStop = False
        '
        'Label_AgentStatus
        '
        Me.Label_AgentStatus.BackColor = System.Drawing.Color.Transparent
        Me.Label_AgentStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_AgentStatus.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label_AgentStatus.Location = New System.Drawing.Point(343, 33)
        Me.Label_AgentStatus.Name = "Label_AgentStatus"
        Me.Label_AgentStatus.Size = New System.Drawing.Size(129, 14)
        Me.Label_AgentStatus.TabIndex = 9
        Me.Label_AgentStatus.Text = "DND:Not Login"
        Me.Label_AgentStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label_AgentMode
        '
        Me.Label_AgentMode.BackColor = System.Drawing.Color.Transparent
        Me.Label_AgentMode.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_AgentMode.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label_AgentMode.Location = New System.Drawing.Point(343, 17)
        Me.Label_AgentMode.Name = "Label_AgentMode"
        Me.Label_AgentMode.Size = New System.Drawing.Size(129, 14)
        Me.Label_AgentMode.TabIndex = 8
        Me.Label_AgentMode.Text = "Mode:None"
        Me.Label_AgentMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label_Version
        '
        Me.Label_Version.BackColor = System.Drawing.Color.Transparent
        Me.Label_Version.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_Version.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label_Version.Location = New System.Drawing.Point(59, 14)
        Me.Label_Version.Name = "Label_Version"
        Me.Label_Version.Size = New System.Drawing.Size(129, 14)
        Me.Label_Version.TabIndex = 7
        Me.Label_Version.Text = "Version: 1.0"
        '
        'Label_AppName
        '
        Me.Label_AppName.BackColor = System.Drawing.Color.Transparent
        Me.Label_AppName.Font = New System.Drawing.Font("Microsoft Sans Serif", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_AppName.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label_AppName.Location = New System.Drawing.Point(5, 21)
        Me.Label_AppName.Name = "Label_AppName"
        Me.Label_AppName.Size = New System.Drawing.Size(149, 30)
        Me.Label_AppName.TabIndex = 6
        Me.Label_AppName.Text = "nProPhony"
        '
        'GroupBox_Mode
        '
        Me.GroupBox_Mode.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox_Mode.Controls.Add(Me.Button_AudioSetup)
        Me.GroupBox_Mode.Controls.Add(Me.Button_Logout)
        Me.GroupBox_Mode.Controls.Add(Me.Button_NR_Meeting)
        Me.GroupBox_Mode.Controls.Add(Me.Button_NR_Prayer)
        Me.GroupBox_Mode.Controls.Add(Me.Button_NR_Break)
        Me.GroupBox_Mode.Controls.Add(Me.Button_NR_Lunch)
        Me.GroupBox_Mode.Controls.Add(Me.Button_Outbound)
        Me.GroupBox_Mode.Controls.Add(Me.Button_Inbound)
        Me.GroupBox_Mode.Controls.Add(Me.Label2)
        Me.GroupBox_Mode.Location = New System.Drawing.Point(528, 25)
        Me.GroupBox_Mode.Name = "GroupBox_Mode"
        Me.GroupBox_Mode.Size = New System.Drawing.Size(480, 220)
        Me.GroupBox_Mode.TabIndex = 16
        Me.GroupBox_Mode.TabStop = False
        '
        'Button_AudioSetup
        '
        Me.Button_AudioSetup.Location = New System.Drawing.Point(379, 45)
        Me.Button_AudioSetup.Name = "Button_AudioSetup"
        Me.Button_AudioSetup.Size = New System.Drawing.Size(81, 43)
        Me.Button_AudioSetup.TabIndex = 21
        Me.Button_AudioSetup.Text = "Audio Setup"
        Me.Button_AudioSetup.UseVisualStyleBackColor = True
        '
        'Button_Logout
        '
        Me.Button_Logout.Location = New System.Drawing.Point(379, 178)
        Me.Button_Logout.Name = "Button_Logout"
        Me.Button_Logout.Size = New System.Drawing.Size(81, 27)
        Me.Button_Logout.TabIndex = 20
        Me.Button_Logout.Text = "Lo&gout"
        Me.Button_Logout.UseVisualStyleBackColor = True
        '
        'Button_NR_Meeting
        '
        Me.Button_NR_Meeting.Location = New System.Drawing.Point(236, 162)
        Me.Button_NR_Meeting.Name = "Button_NR_Meeting"
        Me.Button_NR_Meeting.Size = New System.Drawing.Size(97, 43)
        Me.Button_NR_Meeting.TabIndex = 19
        Me.Button_NR_Meeting.Text = "&Meeting"
        Me.Button_NR_Meeting.UseVisualStyleBackColor = True
        '
        'Button_NR_Prayer
        '
        Me.Button_NR_Prayer.Location = New System.Drawing.Point(133, 162)
        Me.Button_NR_Prayer.Name = "Button_NR_Prayer"
        Me.Button_NR_Prayer.Size = New System.Drawing.Size(97, 43)
        Me.Button_NR_Prayer.TabIndex = 18
        Me.Button_NR_Prayer.Text = "&Payer"
        Me.Button_NR_Prayer.UseVisualStyleBackColor = True
        '
        'Button_NR_Break
        '
        Me.Button_NR_Break.Location = New System.Drawing.Point(133, 113)
        Me.Button_NR_Break.Name = "Button_NR_Break"
        Me.Button_NR_Break.Size = New System.Drawing.Size(97, 43)
        Me.Button_NR_Break.TabIndex = 17
        Me.Button_NR_Break.Text = "&Break"
        Me.Button_NR_Break.UseVisualStyleBackColor = True
        '
        'Button_NR_Lunch
        '
        Me.Button_NR_Lunch.Location = New System.Drawing.Point(236, 113)
        Me.Button_NR_Lunch.Name = "Button_NR_Lunch"
        Me.Button_NR_Lunch.Size = New System.Drawing.Size(97, 43)
        Me.Button_NR_Lunch.TabIndex = 16
        Me.Button_NR_Lunch.Text = "&Lunch"
        Me.Button_NR_Lunch.UseVisualStyleBackColor = True
        '
        'Button_Outbound
        '
        Me.Button_Outbound.Location = New System.Drawing.Point(236, 45)
        Me.Button_Outbound.Name = "Button_Outbound"
        Me.Button_Outbound.Size = New System.Drawing.Size(97, 43)
        Me.Button_Outbound.TabIndex = 15
        Me.Button_Outbound.Text = "&Outbound"
        Me.Button_Outbound.UseVisualStyleBackColor = True
        '
        'Button_Inbound
        '
        Me.Button_Inbound.Location = New System.Drawing.Point(133, 45)
        Me.Button_Inbound.Name = "Button_Inbound"
        Me.Button_Inbound.Size = New System.Drawing.Size(97, 43)
        Me.Button_Inbound.TabIndex = 14
        Me.Button_Inbound.Text = "&Inbound"
        Me.Button_Inbound.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(6, 18)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(481, 21)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "Select next action "
        '
        'GroupBox_Workcode
        '
        Me.GroupBox_Workcode.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox_Workcode.Controls.Add(Me.ListView_Workcodes)
        Me.GroupBox_Workcode.Controls.Add(Me.Button_WCClose)
        Me.GroupBox_Workcode.Controls.Add(Me.Label_WCCaption)
        Me.GroupBox_Workcode.Enabled = False
        Me.GroupBox_Workcode.Location = New System.Drawing.Point(53, 284)
        Me.GroupBox_Workcode.Name = "GroupBox_Workcode"
        Me.GroupBox_Workcode.Size = New System.Drawing.Size(480, 220)
        Me.GroupBox_Workcode.TabIndex = 17
        Me.GroupBox_Workcode.TabStop = False
        '
        'ListView_Workcodes
        '
        Me.ListView_Workcodes.HideSelection = False
        Me.ListView_Workcodes.Location = New System.Drawing.Point(8, 43)
        Me.ListView_Workcodes.Name = "ListView_Workcodes"
        Me.ListView_Workcodes.Size = New System.Drawing.Size(465, 138)
        Me.ListView_Workcodes.TabIndex = 7
        Me.ListView_Workcodes.UseCompatibleStateImageBehavior = False
        '
        'Button_WCClose
        '
        Me.Button_WCClose.Location = New System.Drawing.Point(404, 186)
        Me.Button_WCClose.Name = "Button_WCClose"
        Me.Button_WCClose.Size = New System.Drawing.Size(72, 28)
        Me.Button_WCClose.TabIndex = 6
        Me.Button_WCClose.Text = "&Close"
        Me.Button_WCClose.UseVisualStyleBackColor = True
        '
        'Label_WCCaption
        '
        Me.Label_WCCaption.BackColor = System.Drawing.Color.Transparent
        Me.Label_WCCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_WCCaption.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label_WCCaption.Location = New System.Drawing.Point(6, 18)
        Me.Label_WCCaption.Name = "Label_WCCaption"
        Me.Label_WCCaption.Size = New System.Drawing.Size(481, 21)
        Me.Label_WCCaption.TabIndex = 1
        Me.Label_WCCaption.Text = "Select workcodes please."
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(133, 45)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(97, 43)
        Me.Button1.TabIndex = 14
        Me.Button1.Text = "Inbound"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'GroupBox_Call
        '
        Me.GroupBox_Call.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox_Call.Controls.Add(Me.Button3)
        Me.GroupBox_Call.Controls.Add(Me.Button2)
        Me.GroupBox_Call.Controls.Add(Me.Button_Call_Hangup)
        Me.GroupBox_Call.Controls.Add(Me.Button_Call_Conf)
        Me.GroupBox_Call.Controls.Add(Me.Button_Call_Trans)
        Me.GroupBox_Call.Controls.Add(Me.Button_Call_Hold)
        Me.GroupBox_Call.Controls.Add(Me.Button_Call_Mute)
        Me.GroupBox_Call.Controls.Add(Me.Button_Call_Connect)
        Me.GroupBox_Call.Controls.Add(Me.Label_Select_WC)
        Me.GroupBox_Call.Controls.Add(Me.Label_Select_DND)
        Me.GroupBox_Call.Controls.Add(Me.ProgressBar_Answer)
        Me.GroupBox_Call.Controls.Add(Me.TextBox_Number)
        Me.GroupBox_Call.Enabled = False
        Me.GroupBox_Call.Location = New System.Drawing.Point(-1, 43)
        Me.GroupBox_Call.Name = "GroupBox_Call"
        Me.GroupBox_Call.Size = New System.Drawing.Size(480, 220)
        Me.GroupBox_Call.TabIndex = 7
        Me.GroupBox_Call.TabStop = False
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(160, 110)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(72, 28)
        Me.Button3.TabIndex = 33
        Me.Button3.Text = "St&op"
        Me.Button3.UseVisualStyleBackColor = True
        Me.Button3.Visible = False
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(82, 109)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(72, 28)
        Me.Button2.TabIndex = 32
        Me.Button2.Text = "&Start"
        Me.Button2.UseVisualStyleBackColor = True
        Me.Button2.Visible = False
        '
        'Button_Call_Hangup
        '
        Me.Button_Call_Hangup.Location = New System.Drawing.Point(399, 53)
        Me.Button_Call_Hangup.Name = "Button_Call_Hangup"
        Me.Button_Call_Hangup.Size = New System.Drawing.Size(72, 50)
        Me.Button_Call_Hangup.TabIndex = 31
        Me.Button_Call_Hangup.Text = "Han&gup"
        Me.Button_Call_Hangup.UseVisualStyleBackColor = True
        '
        'Button_Call_Conf
        '
        Me.Button_Call_Conf.Location = New System.Drawing.Point(315, 53)
        Me.Button_Call_Conf.Name = "Button_Call_Conf"
        Me.Button_Call_Conf.Size = New System.Drawing.Size(72, 50)
        Me.Button_Call_Conf.TabIndex = 30
        Me.Button_Call_Conf.Text = "&Conf"
        Me.Button_Call_Conf.UseVisualStyleBackColor = True
        '
        'Button_Call_Trans
        '
        Me.Button_Call_Trans.Location = New System.Drawing.Point(242, 53)
        Me.Button_Call_Trans.Name = "Button_Call_Trans"
        Me.Button_Call_Trans.Size = New System.Drawing.Size(72, 50)
        Me.Button_Call_Trans.TabIndex = 29
        Me.Button_Call_Trans.Text = "&Trans"
        Me.Button_Call_Trans.UseVisualStyleBackColor = True
        '
        'Button_Call_Hold
        '
        Me.Button_Call_Hold.Location = New System.Drawing.Point(153, 53)
        Me.Button_Call_Hold.Name = "Button_Call_Hold"
        Me.Button_Call_Hold.Size = New System.Drawing.Size(72, 50)
        Me.Button_Call_Hold.TabIndex = 28
        Me.Button_Call_Hold.Text = "&Hold"
        Me.Button_Call_Hold.UseVisualStyleBackColor = True
        '
        'Button_Call_Mute
        '
        Me.Button_Call_Mute.Location = New System.Drawing.Point(79, 53)
        Me.Button_Call_Mute.Name = "Button_Call_Mute"
        Me.Button_Call_Mute.Size = New System.Drawing.Size(72, 50)
        Me.Button_Call_Mute.TabIndex = 27
        Me.Button_Call_Mute.Text = "&Mute"
        Me.Button_Call_Mute.UseVisualStyleBackColor = True
        '
        'Button_Call_Connect
        '
        Me.Button_Call_Connect.Location = New System.Drawing.Point(6, 53)
        Me.Button_Call_Connect.Name = "Button_Call_Connect"
        Me.Button_Call_Connect.Size = New System.Drawing.Size(72, 50)
        Me.Button_Call_Connect.TabIndex = 26
        Me.Button_Call_Connect.Text = "&Accept"
        Me.Button_Call_Connect.UseVisualStyleBackColor = True
        '
        'Label_Select_WC
        '
        Me.Label_Select_WC.Image = Global.nProPhony.My.Resources.Resources.icon_WC1
        Me.Label_Select_WC.Location = New System.Drawing.Point(5, 15)
        Me.Label_Select_WC.Name = "Label_Select_WC"
        Me.Label_Select_WC.Size = New System.Drawing.Size(43, 31)
        Me.Label_Select_WC.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.Label_Select_WC.TabIndex = 24
        Me.Label_Select_WC.TabStop = False
        '
        'Label_Select_DND
        '
        Me.Label_Select_DND.Image = Global.nProPhony.My.Resources.Resources.icon_DND
        Me.Label_Select_DND.Location = New System.Drawing.Point(363, 14)
        Me.Label_Select_DND.Name = "Label_Select_DND"
        Me.Label_Select_DND.Size = New System.Drawing.Size(110, 32)
        Me.Label_Select_DND.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.Label_Select_DND.TabIndex = 23
        Me.Label_Select_DND.TabStop = False
        '
        'ProgressBar_Answer
        '
        Me.ProgressBar_Answer.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ProgressBar_Answer.Location = New System.Drawing.Point(3, 199)
        Me.ProgressBar_Answer.Name = "ProgressBar_Answer"
        Me.ProgressBar_Answer.Size = New System.Drawing.Size(474, 18)
        Me.ProgressBar_Answer.TabIndex = 15
        '
        'TextBox_Number
        '
        Me.TextBox_Number.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TextBox_Number.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox_Number.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox_Number.ImeMode = System.Windows.Forms.ImeMode.[On]
        Me.TextBox_Number.Location = New System.Drawing.Point(54, 13)
        Me.TextBox_Number.Name = "TextBox_Number"
        Me.TextBox_Number.Size = New System.Drawing.Size(306, 34)
        Me.TextBox_Number.TabIndex = 8
        Me.TextBox_Number.Text = "098230948239048"
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info
        Me.NotifyIcon1.BalloonTipText = "OVS Desktop Agent"
        Me.NotifyIcon1.BalloonTipTitle = "OVS Desktop Agent"
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "nProPhony OVS Agent"
        '
        'Form_Splash
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.BackColor = System.Drawing.Color.MidnightBlue
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(358, 223)
        Me.Controls.Add(Me.GroupBox_Workcode)
        Me.Controls.Add(Me.GroupBox_Call)
        Me.Controls.Add(Me.GroupBox_Mode)
        Me.Controls.Add(Me.GroupBox_Login)
        Me.Controls.Add(Me.GroupBox_Title)
        Me.Controls.Add(Me.Label_Status)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "Form_Splash"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "OVS Agent Desktop"
        Me.TopMost = True
        Me.GroupBox_Login.ResumeLayout(False)
        Me.GroupBox_Login.PerformLayout()
        Me.GroupBox_Title.ResumeLayout(False)
        Me.GroupBox_Mode.ResumeLayout(False)
        Me.GroupBox_Workcode.ResumeLayout(False)
        Me.GroupBox_Call.ResumeLayout(False)
        Me.GroupBox_Call.PerformLayout()
        CType(Me.Label_Select_WC, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label_Select_DND, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label_Status As Label
    Friend WithEvents GroupBox_Login As GroupBox
    Friend WithEvents TextBox_Password As TextBox
    Friend WithEvents TextBox_LoginName As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents GroupBox_Title As GroupBox
    Friend WithEvents Label_Version As Label
    Friend WithEvents Label_AppName As Label
    Friend WithEvents Button_Login As Button
    Friend WithEvents Label5 As Label
    Friend WithEvents Label_AgentStatus As Label
    Friend WithEvents Label_AgentMode As Label
    Friend WithEvents GroupBox_Mode As GroupBox
    Friend WithEvents Button_Logout As Button
    Friend WithEvents Button_NR_Meeting As Button
    Friend WithEvents Button_NR_Prayer As Button
    Friend WithEvents Button_NR_Break As Button
    Friend WithEvents Button_NR_Lunch As Button
    Friend WithEvents Button_Outbound As Button
    Friend WithEvents Button_Inbound As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents GroupBox_Workcode As GroupBox
    Friend WithEvents Button_WCClose As Button
    Friend WithEvents Label_WCCaption As Label
    Friend WithEvents ListView_Workcodes As ListView
    Friend WithEvents GroupBox_Call As GroupBox
    Friend WithEvents TextBox_Number As TextBox
    Friend WithEvents ProgressBar_Answer As ProgressBar
    Friend WithEvents Label_Select_DND As PictureBox
    Friend WithEvents Label_Select_WC As PictureBox
    Friend WithEvents Button_Call_Hangup As Button
    Friend WithEvents Button_Call_Conf As Button
    Friend WithEvents Button_Call_Trans As Button
    Friend WithEvents Button_Call_Hold As Button
    Friend WithEvents Button_Call_Mute As Button
    Friend WithEvents Button_Call_Connect As Button
    Friend WithEvents NotifyIcon1 As NotifyIcon
    Friend WithEvents Button_AudioSetup As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents Button2 As Button
End Class
