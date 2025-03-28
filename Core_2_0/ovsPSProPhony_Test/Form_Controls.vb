Imports System.ComponentModel
Imports System.Threading
Imports ovsPSProPhony.TControl
Imports System.Media
Imports System.Windows


Public Class Form_Controls
    Dim Form_VideoViewer = New Form_Video() ' Must be created on this thread!
    Dim WithEvents soundplayer As New SoundPlayer
    Dim WithEvents ovsControl As New ovsPSProPhony.TControl("c:\temp")
    Dim _sessionID As Long = -2
    Dim _VideoRemoteHandle As IntPtr = IntPtr.Zero
    Dim _VideoLocalHandle As IntPtr = IntPtr.Zero
    Dim sInputs As String = ""
    Dim inCall As Boolean = False

    Private Sub loadMediaDevicesSpeakers()

        Dim num As Integer = ovsControl.getNumSpeakers()
        For i As Integer = 0 To num - 1
            ComboBoxSpeakers.Items.Add(ovsControl.getSpeaker(i))
        Next

    End Sub
    Private Sub loadMediaDevicesMicrophones()

        Dim num As Integer = ovsControl.getNumMicrophones
        For i As Integer = 0 To num - 1
            ComboBoxMicrophones.Items.Add(ovsControl.getMicrophones(i))
        Next
    End Sub

    Private Sub loadMediaDevicesCamras()

        Dim num As Integer = ovsControl.getNumCamras
        For i As Integer = 0 To num - 1
            ComboBoxCameras.Items.Add(ovsControl.getCamra(i))
        Next
    End Sub
    Private Sub loadMediaDevices()

        loadMediaDevicesSpeakers()
        If ComboBoxSpeakers.Items.Count > 0 Then
            ComboBoxSpeakers.SelectedIndex = 0
        End If
        loadMediaDevicesMicrophones()
        If ComboBoxMicrophones.Items.Count > 0 Then
            ComboBoxMicrophones.SelectedIndex = 0
        End If
        loadMediaDevicesCamras()
        If ComboBoxCameras.Items.Count > 0 Then
            ComboBoxCameras.SelectedIndex = 0
        End If


        Dim volume As Integer = ovsControl.SpeakerVolme
        TrackBarSpeaker.SetRange(0, 255)
        TrackBarSpeaker.Value = volume

        volume = ovsControl.MicrophoneVolme
        TrackBarMicrophone.SetRange(0, 255)
        TrackBarMicrophone.Value = volume

    End Sub

    Private Sub addToList(msg As String)
        Try
            If IsNothing(ListBox_Events) Then Exit Sub
            If ListBox_Events.Items.Count > 100 Then
                ListBox_Events.Items.Clear()
                'ListBox_Events.Items.RemoveAt(ListBox_Events.Items.Count)
            End If
            ListBox_Events.Items.Insert(0, msg)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Control.CheckForIllegalCrossThreadCalls = False

        For index = 1 To ovsControl.NICCount
            ComboBox_IPAddresses.Items.Add(ovsControl.localIPAddress(index))
        Next
        ovsControl.localIPAddress = "0.0.0.0"
        For index = 1 To 9
            ComboBox_Channels.Items.Add(ovsControl.localIPAddress(index))
        Next
        ComboBox_Channels.Text = "2"
        ComboBox_Transport.Items.Add("UDP")
        ComboBox_Transport.Items.Add("TCP")
        ComboBox_Transport.Items.Add("TLS")
        ComboBox_Transport.Items.Add("PERS")
        ComboBox_Transport.Text = "UDP"
        TextBox_logDir.Text = ovsControl.FoldersLog
        addToList("Ready to Use")


        DataGridLines.Columns(0).Width = 30
        DataGridLines.Columns(1).Width = 90
        DataGridLines.Columns(2).Width = 30
        DataGridLines.Columns(3).Width = 30
        DataGridLines.Columns(4).Width = 30
        DataGridLines.Columns(5).Width = 30
        DataGridLines.Columns(6).Width = 30
        DataGridLines.Columns(7).Width = 90
        DataGridLines.Columns(8).Width = 90
        DataGridLines.Columns(9).Width = 90

        'Dim ExtStart As Integer = 1145
        'Dim TotalExt As Integer = 1
        'Dim Port As Integer = 5060
        'Dim Server As String = "10.5.5.214"

        Dim ExtStart As Integer = 1004
        Dim TotalExt As Integer = 1
        Dim Port As Integer = 5060
        Dim Server As String = "192.168.16.137"


        For i = 1 To (TotalExt)
            DataGridLines.Rows.Add()
            DataGridLines.Rows(i - 1).Cells(0).Value = "" & (i.ToString.PadLeft(3, "0"))
            DataGridLines.Rows(i - 1).Cells(1).Value = "" & Server
            DataGridLines.Rows(i - 1).Cells(2).Value = "" & Port
            DataGridLines.Rows(i - 1).Cells(3).Value = "" & ExtStart + (i)
            DataGridLines.Rows(i - 1).Cells(4).Value = "" & ExtStart + (i)
            DataGridLines.Rows(i - 1).Cells(5).Value = "" & ExtStart + (i)
            'DataGridLines.Rows(i - 1).Cells(6).Value = "" & "44332211"
            DataGridLines.Rows(i - 1).Cells(6).Value = "" & "ab0000"
            DataGridLines.Rows(i - 1).Cells(7).Value = "" & Server
        Next

        Dim th As System.Threading.Thread = New Threading.Thread(AddressOf form_opener)
        th.Name = "videoHandler"
        th.SetApartmentState(ApartmentState.STA)
        th.Start("hello")


    End Sub


    Public Sub form_opener(x As String)
        Try
            _VideoRemoteHandle = Form_VideoViewer.getRemoteVideoHandle
            _VideoLocalHandle = Form_VideoViewer.getlocalVideoHandle
            Form_VideoViewer.visible = False
            Application.Run(Form_VideoViewer)
        Catch ex As Exception
            MessageBox.Show(ex.Message())
        End Try
    End Sub
    Private Sub Button_Initialize_Click(sender As Object, e As EventArgs) Handles Button_Initialize.Click
        addToList("Initializing")

        Button_Initialize.Enabled = False
        Try
            'ovsControl.CoreMode = Val(TextBox_Mode.Text)
            'ovsControl.localIPAddress = "1212123123123123"
            ovsControl.CoreMode = 1
            ovsControl.Channels = DataGridLines.Rows.Count - 1

            'If CheckBox_TCP.Checked Then
            'ovsControl.Initialize(2, 0, TextBox_VoiceFolder.Text, TRANSPORT_TYPE.TRANSPORT_TCP)
            'Else
            'ovsControl.Initialize(2, 0, TextBox_VoiceFolder.Text, TRANSPORT_TYPE.TRANSPORT_UDP)
            'End If
            ovsControl.FoldersVoice = TextBox_VoiceFolder.Text
            ovsControl.Initialize(0)
            ovsControl.SessionTimeout(0) = 90

        Catch ex As Exception
            Button_Initialize.Enabled = True
            MessageBox.Show(ex.Message())
        End Try
    End Sub


    Private Sub Button_Register_Click(sender As Object, e As EventArgs) Handles Button_Register.Click

        Dim lineNo As Integer = GetSelectedLineNo()
        addToList(lineNo & ", Registering")


        'Dim userName As String = "1004"
        'Dim usersecret As String = "ab0000"
        'Dim SIPServerIP As String = "192.168.16.137"
        'Dim SIPServerPort As String = "5060"
        'Dim User_Domain As String = SIPServerIP


        'Dim userName As String = "1133"
        'Dim usersecret As String = "44332211"
        'Dim SIPServerIP As String = "10.5.5.214"
        'Dim SIPServerPort As String = "5060"
        'Dim User_Domain As String = SIPServerIP


        Dim userName As String = "1004"
        Dim usersecret As String = "ab0000"
        Dim SIPServerIP As String = "192.168.16.137"
        Dim SIPServerPort As String = "5060"
        Dim User_Domain As String = SIPServerIP


        ovsControl.FoldersVoice = TextBox_VoiceFolder.Text

        ovsControl.User_UserName(lineNo) = userName
        ovsControl.User_AuthName(lineNo) = userName
        ovsControl.User_Display(lineNo) = userName
        ovsControl.User_Secret(lineNo) = usersecret
        ovsControl.User_Domain(lineNo) = SIPServerIP
        ovsControl.SIPServerIP(lineNo) = SIPServerIP
        ovsControl.SIPServerPort(lineNo) = SIPServerPort
        ovsControl.SIPProxtIP(lineNo) = ""
        ovsControl.SIPProxtPort(lineNo) = 0
        ovsControl.SIPStunIP(lineNo) = ""
        ovsControl.SIPStunPort(lineNo) = 0
        Try
            ovsControl.ExtRegister(lineNo, True)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub Button_UnRegister_Click(sender As Object, e As EventArgs) Handles Button_UnRegister.Click
        Try
            Dim lineNo As Integer = GetSelectedLineNo()

            addToList(lineNo & ", UnRegistering")
            If ovsControl.ExtRegister(lineNo, False) Then
                addToList(lineNo & ", Unregistered")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub



    Private Sub ComboBoxMicrophones_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxMicrophones.SelectedIndexChanged

        ovsControl.setAudioDeviceId(ComboBoxMicrophones.SelectedIndex, ComboBoxSpeakers.SelectedIndex)

    End Sub

    Private Sub ComboBoxSpeakers_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxSpeakers.SelectedIndexChanged
        ovsControl.setAudioDeviceId(ComboBoxMicrophones.SelectedIndex, ComboBoxSpeakers.SelectedIndex)
    End Sub


    Private Sub TrackBarSpeaker_ValueChanged(sender As Object, e As EventArgs) Handles TrackBarSpeaker.ValueChanged
        ovsControl.SpeakerVolme = TrackBarSpeaker.Value
    End Sub


    Private Sub TrackBarMicrophone_ValueChanged(sender As Object, e As EventArgs) Handles TrackBarMicrophone.ValueChanged
        ovsControl.MicrophoneVolme = TrackBarMicrophone.Value
    End Sub




    Private Sub Button_DNDOn_Click(sender As Object, e As EventArgs) Handles Button_DNDOn.Click
        Try
            Dim lineNo As Integer = GetSelectedLineNo()
            ovsControl.DND(lineNo) = True
            Button_DNDOn.Enabled = True
            Button_DNDoff.Enabled = True

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Private Function GetSelectedLineNo() As Integer
        GetSelectedLineNo = 0
        For i = 0 To DataGridLines.Rows.Count - 1
            If DataGridLines.Rows(i).Selected Then
                GetSelectedLineNo = i
                Exit For
            End If
        Next
    End Function

    Private Sub Button_DNDoff_Click(sender As Object, e As EventArgs) Handles Button_DNDoff.Click
        Try
            Dim lineNo As Integer = GetSelectedLineNo()
            ovsControl.DND(lineNo) = False
            Button_DNDOn.Enabled = True
            Button_DNDoff.Enabled = True
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub



    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Dim lineNo As Integer = GetSelectedLineNo()
            _sessionID = DataGridLines.Rows(lineNo).Cells(8).Value
            If ovsControl.Call_Hangup(lineNo, _sessionID) Then
                _sessionID = -1
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            Dim lineNo As Integer = GetSelectedLineNo()
            _sessionID = DataGridLines.Rows(lineNo).Cells(8).Value

            If ovsControl.Call_Hold(lineNo, _sessionID) Then
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            Dim lineNo As Integer = GetSelectedLineNo()
            _sessionID = DataGridLines.Rows(lineNo).Cells(8).Value

            If ovsControl.Call_Unhold(lineNo, _sessionID) Then
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Try

            If ovsControl.Microphone(True) Then
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Try
            If ovsControl.Microphone(False) Then
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Try

            ovsControl.Call_Make(0, TextBox_Number.Text)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub Form_Controls_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Try
            Form_VideoViewer = Nothing
            ovsControl = Nothing
            Application.Exit()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button_Answer_Click(sender As Object, e As EventArgs) Handles Button_Answer.Click
        Try
            Form_VideoViewer.Visible = True
            Application.DoEvents()
            Dim lineNo As Integer = GetSelectedLineNo()
            _sessionID = DataGridLines.Rows(lineNo).Cells(8).Value
            ovsControl.Call_Answer(lineNo, _sessionID, _VideoLocalHandle, _VideoRemoteHandle)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub




    Private Sub Button_RecordStop_Click(sender As Object, e As EventArgs)
        Try
            If ovsControl.Call_StopRecord(0, _sessionID) Then
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub Button_CamShare_Click(sender As Object, e As EventArgs) Handles Button_CamShare.Click
        Try
            ovsControl.VideoResolution = Enum_VideoResolution.Resolution_QCIF
            _VideoLocalHandle = Form_VideoViewer.getlocalVideoHandle()
            _VideoRemoteHandle = Form_VideoViewer.getremoteVideoHandle()
            ovsControl.Call_sendVideo(_sessionID, True)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub Button_CamUnshare_Click(sender As Object, e As EventArgs) Handles Button_CamUnshare.Click
        Try

            ovsControl.Call_sendVideo(_sessionID, False)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub Button_VideoViewer_Click(sender As Object, e As EventArgs) Handles Button_VideoViewer.Click
        Try
            Form_VideoViewer.visible = True
        Catch ex As Exception
        End Try
    End Sub
    Private Sub Button_RecordStart_Click(sender As Object, e As EventArgs) Handles Button_RecordStart.Click
        Try
            Dim dt As Date = Date.Now
            Dim str As String = dt.Year & dt.Month & dt.Day & dt.Hour & dt.Second & dt.Millisecond
            Dim sFileName As String = str & ".wav"
            Dim sPath As String = IO.Path.GetTempPath() & "ovsPSProPhony\"
            Dim lineNo As Integer = GetSelectedLineNo()
            _sessionID = DataGridLines.Rows(lineNo).Cells(8).Value
            'ovsControl.Call_StartRecord(lineNo, _sessionID, sFileName, sPath, ovsPSProPhony.PortSIP.AUDIOSTREAM_CALLBACK_MODE.AUDIOSTREAM_REMOTE_MIX)
            ovsControl.Call_StartRecord(lineNo, _sessionID, sFileName, sPath, ovsPSProPhony.PortSIP.AUDIOSTREAM_CALLBACK_MODE.AUDIOSTREAM_NONE)

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub Button_PlayFiles_Click(sender As Object, e As EventArgs) Handles Button_PlayFiles.Click
        Try
            Dim lineNo As Integer = GetSelectedLineNo()
            _sessionID = DataGridLines.Rows(lineNo).Cells(8).Value
            addToList(lineNo & ", PlayFiles")
            sInputs = ""
            ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(lineNo, 0, _sessionID, True, "TPINExisting.wav", True, "*#1234567890"))

            Application.DoEvents()
            Application.DoEvents()
            Dim tmpstring As String = ""
            'tmpstring = ovsControl.media_getDigits(New media_getDigits_Args(lineNo, 0, _sessionID, True, (4 - Len(sInputs)), 15, True))
            Dim _waitStartTime As DateTime = DateTime.Now
            Dim xDateDiff As Integer = 0
            While True
                'If CallInformation.CallStatus <> "Talking" Then
                'Exit While
                'End If
                xDateDiff = DateDiff(DateInterval.Second, _waitStartTime, Now)
                If DateDiff(DateInterval.Second, _waitStartTime, Now) >= 15 Then
                    'retVal = 0
                    Exit While
                End If
                If Len(sInputs) = 4 Then
                    Exit While
                End If
                'If ovsControl.Playing = False Then
                'Exit While
                'End If
                Application.DoEvents()
                Threading.Thread.Sleep(200)
                'updateStatusText("Existing TPIN waiting for digits :" & (4 - Len(sInputs)) & ", " & xDateDiff)
                'addToList(lineNo & ", Returned ASYNC, Going to stop")
            End While
            addToList(lineNo & ", Returned ASYNC, Going to stop")
            Application.DoEvents()
            ovsControl.media_stopPlay(0, 0, _sessionID)
            Application.DoEvents()
            addToList(lineNo & ", Returned from stop, Going to Play SYNC")
            Application.DoEvents()
            ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(lineNo, 0, _sessionID, True, "TPINInvalid.wav", True, "A"))
            Application.DoEvents()
            Do While ovsControl.Playing = True
                Application.DoEvents()
                Thread.Sleep(200)
            Loop
            addToList(lineNo & ", Returned from SYNC Play, Going to to stop")
            Application.DoEvents()
            ovsControl.media_stopPlay(0, 0, _sessionID)
            Application.DoEvents()

            addToList(lineNo & ", Returned from stop")
        Catch ex As Exception
            addToList("Button_PlayFiles:Exception:" & ex.Message)
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Try
            Dim lineNo As Integer = GetSelectedLineNo()
            _sessionID = DataGridLines.Rows(lineNo).Cells(8).Value

            'If ovsControl.enableAudioStreamCallback(lineNo, _sessionID, True, ovsPSProPhony.PortSIP.AUDIOSTREAM_CALLBACK_MODE.AUDIOSTREAM_REMOTE_MIX) Then
            ''_sessionID = -1
            'End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Try
            Dim lineNo As Integer = GetSelectedLineNo()
            _sessionID = DataGridLines.Rows(lineNo).Cells(8).Value
            'If ovsControl.enableAudioStreamCallback(lineNo, _sessionID, False, ovsPSProPhony.PortSIP.AUDIOSTREAM_CALLBACK_MODE.AUDIOSTREAM_REMOTE_MIX) Then
            '_sessionID = -1
            'End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Try
            Dim lineNo As Integer
            For i = 0 To DataGridLines.RowCount - 1
                lineNo = i
                addToList(lineNo & ", Registering")

                ovsControl.User_UserName(lineNo) = DataGridLines.Rows(lineNo).Cells(3).Value
                ovsControl.User_AuthName(lineNo) = DataGridLines.Rows(lineNo).Cells(4).Value
                ovsControl.User_Display(lineNo) = DataGridLines.Rows(lineNo).Cells(5).Value
                ovsControl.User_Secret(lineNo) = DataGridLines.Rows(lineNo).Cells(6).Value
                ovsControl.User_Domain(lineNo) = DataGridLines.Rows(lineNo).Cells(7).Value
                ovsControl.SIPServerIP(lineNo) = DataGridLines.Rows(lineNo).Cells(1).Value
                ovsControl.SIPServerPort(lineNo) = DataGridLines.Rows(lineNo).Cells(2).Value
                ovsControl.SIPProxtIP(lineNo) = ""
                ovsControl.SIPProxtPort(lineNo) = 0
                ovsControl.SIPStunIP(lineNo) = ""
                ovsControl.SIPStunPort(lineNo) = 0
                Try
                    ovsControl.ExtRegister(lineNo, True)
                Catch ex As Exception
                    addToList(lineNo & ", Error:" & ex.Message)
                End Try
            Next
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        Try
            Dim lineNo As Integer
            For i = 0 To DataGridLines.RowCount - 1
                lineNo = i
                addToList(lineNo & ", UnRegistering")
                If ovsControl.ExtRegister(lineNo, False) Then
                    addToList(lineNo & ", Unregistered")
                End If
            Next
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Dim lineNo As Integer = GetSelectedLineNo()
        _sessionID = DataGridLines.Rows(lineNo).Cells(8).Value
        addToList(lineNo & ", StopPlay")
        ovsControl.media_stopPlay(lineNo, 0, _sessionID)

    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        Try
            Dim lineNo As Integer = GetSelectedLineNo()
            _sessionID = DataGridLines.Rows(lineNo).Cells(8).Value
            addToList(lineNo & ", GetDigits")
            ovsControl.media_getDigits(New ovsPSProPhony.TControl.media_getDigits_Args(lineNo, 0, _sessionID, True, 3, 15, CheckBox_ClearBuff.Checked, "*"))
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        Dim lineNo As Integer = GetSelectedLineNo()
        _sessionID = DataGridLines.Rows(lineNo).Cells(8).Value
        addToList(lineNo & ", StopGetDigits")
        ovsControl.media_stopGetDigits(lineNo, 0, _sessionID)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            ovsControl.CoreMode = 1
            Dim lineNo As Integer = GetSelectedLineNo()
            ovsControl.InitializeChannel(lineNo, 2, 1, TextBox_VoiceFolder.Text, TRANSPORT_TYPE.TRANSPORT_UDP)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub Button_RecordStop_Click_1(sender As Object, e As EventArgs) Handles Button_RecordStop.Click
        Try
            Dim lineNo As Integer = GetSelectedLineNo()
            _sessionID = DataGridLines.Rows(lineNo).Cells(8).Value
            ovsControl.Call_StopRecord(lineNo, _sessionID)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub


    Private Sub ListBox_Events_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox_Events.SelectedIndexChanged

    End Sub

    Private Sub ListBox_Events_DoubleClick(sender As Object, e As EventArgs) Handles ListBox_Events.DoubleClick
        ListBox_Events.Items.Clear()
    End Sub

    Private Sub playFileAndGetDigits(lineNo As Integer, sessionID As Long, szFileNames As String, Label As String, TimeOut As Integer, Digits As Integer, TermDigits As String)
        sInputs = ""
        If Trim(TermDigits) = "" Then
            TermDigits = "*#0123456789"
        End If
        If inCall = False Then
            Exit Sub
        End If

        Dim xDateDiff As Integer = 0
        Dim _waitStartTime As DateTime = DateTime.Now
        Try
            Try
                ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(lineNo, 0, _sessionID, True, szFileNames, True, TermDigits))
            Catch ex1 As Exception
                If ex1.Message.ToString.Contains("Already Playing file") Then
                    ovsControl.media_stopPlay(lineNo, 0, _sessionID)
                    Application.DoEvents()
                    ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(lineNo, 0, _sessionID, True, szFileNames, True, TermDigits))
                End If
            End Try
            Do While True
                If inCall = False Then
                    Exit Do
                End If
                If UCase(Trim(TermDigits)) = "A" Then
                    sInputs = ""
                    If ovsControl.Playing = False Then
                        Exit Do
                    End If
                Else
                    If Len(sInputs) >= Digits Then
                        Exit Do
                    End If
                End If
                xDateDiff = DateDiff(DateInterval.Second, _waitStartTime, Now)
                If DateDiff(DateInterval.Second, _waitStartTime, Now) >= 15 Then
                    'retVal = 0
                    Exit Do
                End If
                Application.DoEvents()
                Thread.Sleep(100)
            Loop
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub
    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        Dim lineNo As Integer = GetSelectedLineNo()
        _sessionID = DataGridLines.Rows(lineNo).Cells(8).Value
        addToList(lineNo & ", PlayFiles")
        Try
            sInputs = ""
            playFileAndGetDigits(lineNo, _sessionID, "TPINExisting.wav", "TPIN", 21, 4, "*#0123456789")
            If inCall = False Then
                addToList(lineNo & ", Call Droped")
                Exit Sub
            End If
            If sInputs = "1111" Then
                playFileAndGetDigits(lineNo, _sessionID, "TPINDone1.wav", "TPIN", 10, 4, "A")
            Else
                playFileAndGetDigits(lineNo, _sessionID, "TPINInvalid.wav", "TPIN", 10, 4, "A")

            End If
            MessageBox.Show(sInputs)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub
    Private Sub ovsControl_Event_CallGeneric(Sender As Object, e As Event_CallGeneric_Args) Handles ovsControl.Event_CallGeneric

        Dim LineNo As Integer = 0
        addToList("evt:" & e.callbackIndex & ", evt:" & e.callbackObject & ", evt:" & e.eventName & ", sessionId:" & e.sessionId & ", data1:" & e.data1 & ", data2:" & e.data2)


        Select Case e.eventName
            Case "onInviteConnected"   '' call connected
                ovsControl.DND(0) = True
                inCall = True
                DataGridLines.Rows(e.callbackIndex).Cells(8).Value = "" & e.sessionId
                _sessionID = e.sessionId
                If CheckBox_AutoRecord.Checked Then
                    addToList("evt: Auto Recording")
                    Button_RecordStart_Click(Sender, e)
                End If
            Case "onInviteClosed"     '' call droped by other end
                'Button8_Click(Sender, e)
                inCall = False
                DataGridLines.Rows(e.callbackIndex).Cells(8).Value = "" & e.sessionId
                _sessionID = e.sessionId
                If CheckBox_AutoReady.Checked Then
                    ovsControl.DND(0) = False
                End If
            Case "onInviteFailure" ' dialing failed

                DataGridLines.Rows(e.callbackIndex).Cells(8).Value = "" & e.sessionId
                Label_callStatus.Text = "code:" & e.data2 & ", status:" & e.data1
            Case "onInviteTrying" ' dialing started

                DataGridLines.Rows(e.callbackIndex).Cells(8).Value = "" & e.sessionId
                _sessionID = e.sessionId
            Case "onInviteRinging" ' dialed ringing
                DataGridLines.Rows(e.callbackIndex).Cells(8).Value = "" & e.sessionId
                _sessionID = e.sessionId
            Case "onRecvDtmfTone"
                sInputs = sInputs & e.data1

            Case "onGetDigitsFinished"
                If (Trim(e.data2)) <> "" Then
                    sInputs = sInputs & e.data2
                End If

            Case Else
                Debug.WriteLine("CallGeneric:" & e.eventName)
        End Select
    End Sub
    Private Sub ovsControl_Event_CallGeneric2(Sender As Object, e As Event_CallGeneric2_Args) Handles ovsControl.Event_CallGeneric2
        addToList("evt2:" & e.callbackIndex & ", evt2:" & e.callbackObject & "evt2:" & e.eventName & ", sessionId:" & e.sessionId & ", caller:" & e.caller & ", callee:" & e.callee)

        Dim LineNo As Integer = 0
        Select Case e.eventName
            Case "onInviteIncoming"
                LineNo = e.callbackIndex
                DataGridLines.Rows(LineNo).Cells(8).Value = "" & e.sessionId
                _sessionID = e.sessionId
                Console.WriteLine("sipMessage:" & e.sipMessage.ToString)
                If e.existsVideo = True Then
                    addToList("evt2: its a video call")
                Else
                    addToList("evt2: its a audio call")
                End If
                If CheckBox_AutoAnswer.Checked Then
                    addToList("evt2: Auto Answering")
                    Button_Answer_Click(Sender, e)
                Else
                    Dim filename As String = "\alert-asw.wav"
                    If e.existsVideo = True Then
                        filename = "\alert-vsw.wav"
                    End If
                    soundplayer.SoundLocation = My.Computer.FileSystem.CurrentDirectory & filename
                    soundplayer.Play()
                End If
            Case "onInviteAnswered" ' dial number connected
                LineNo = e.callbackObject
                DataGridLines.Rows(LineNo).Cells(9).Value = "" & e.sessionId
                _sessionID = e.sessionId
            Case Else
                Debug.WriteLine("CallGeneric2:" & e.eventName)
        End Select
    End Sub
    Private Sub ovsControl_Event_Error(Sender As Object, e As Event_Error_Args) Handles ovsControl.Event_Error
        addToList(e.callbackObject & ", Event_Error(" & e.ex.Message & ")")
    End Sub
    Private Sub ovsControl_Event_ExtRegister(Sender As Object, e As Event_ExtRegister_Args) Handles ovsControl.Event_ExtRegister
        addToList("evt:" & e.callbackIndex & ", evt:" & e.callbackObject & ", Event_Initialize(registered=" & e.registered & ",statusCode=" & e.statusCode & ",statusText=" & e.statusText)

        Dim LineNo As Integer = 0
        LineNo = e.callbackIndex

        If e.registered = True Then
            DataGridLines.Rows(LineNo).Cells(9).Value = "YES"
            'Button_Register.Enabled = False
            'Button_UnRegister.Enabled = True
            Panel_Controls.Enabled = True
            Button_DNDoff.Enabled = True
            Button_DNDOn.Enabled = True

        Else
            DataGridLines.Rows(LineNo).Cells(9).Value = "NO"
            'Button_Register.Enabled = True
            'Button_UnRegister.Enabled = False
        End If
    End Sub
    Private Sub ovsControl_Event_Initialize(Sender As Object, e As Event_Initialize_Args) Handles ovsControl.Event_Initialize
        addToList("Event_Initialize(evt:" & e.callbackIndex & ", evt:" & e.callbackObject & ",IP:" & e.ip & "")
        Dim LineNo As Integer = 0
        LineNo = e.callbackIndex
        DataGridLines.Rows(LineNo).Cells(9).Value = "NO"
        Button_Initialize.Enabled = False
        Button_Register.Enabled = True
        groupBox3.Enabled = False
        If ovsControl.CoreMode <> 1 Then
            groupBox3.Enabled = True
            loadMediaDevices()
        End If
    End Sub

End Class
