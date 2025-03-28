' first code chagne from new repo

Imports System.ComponentModel
Imports System.Text.RegularExpressions
Imports System.Data
Imports ovsPSProPhony

'Imports WebSocketSharp
'Imports WebSocketSharp.Server
Imports System.Reflection
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports System.Media
Imports System.Xml
Imports System
Imports System.Collections.Generic
Imports Alchemy
Imports Alchemy.Classes
Imports System.Collections.Concurrent
Imports System.Threading

'Imports System.IO

'' Code Changes note
'' April-06-2023 in sendTowsClinet msgbox is comminted now
'' April-06-2023 in updateStatusText msgbox is comminted now, now writing to a file
'' April-08-2023 now fresh call will reset ivr veriable and stop existing files which left playing
'' Nov-15-2023   Single instance issue fixed
'' Nov-15-2023   Extension details in log file

Public Class Form_Splash
    Protected Shared OnlineConnections As ConcurrentDictionary(Of String, Connection) = New ConcurrentDictionary(Of String, Connection)()

    Private ContinuesCloseCounter As Long = 0

    '' taking this veriable to send receveied error msg back to PLUSE UI.
    Private _TerminalServerSession As Boolean = False
    Private sTempResponseString As String = ""

    Private meDefaultText As String = "OVS Agent Desktop"
    Private sgAPIClientEnabled As Boolean = False
    Private IVRisWorking As Boolean = False
    Private sInputs As String = ""
    Private sLanguage As String = ""
    Private tsString As String = ""
    Private wsConnectionID As String = ""
    Private logoutRequestfromWS As Boolean = False
    Private bRequestFromWSClent As Boolean = False

    Private wsServiceName As String = "/"

    Private WithEvents wSSNativeHost As Class_MSWS

    Private wSSHost As WebSocketServer
    Public Shared wsMessages As New Collection
    Private resetConnection As Boolean = False

    Private WithEvents ovsControl As New ovsPSProPhony.TControl

    Private WithEvents uCallback As New Class_UCallBack()

    'Private _sessionID As Long = -2
    Private controlBoxTop = 35
    Private controlBoxLeft = 0

    Private RingPlayer As SoundPlayer = New SoundPlayer()
    Private ConnectedPlayer As SoundPlayer = New SoundPlayer()

    Private _tmpsessionID As String


    Private Sub updateStatusText(Text As String)
        Try
            Label_Status.Text = Text
            Label_Status.Refresh()

        Catch ex As Exception
            Write_Log("ERR", "updateStatusText(Error)" & ex.Message)
        End Try
    End Sub
    Friend WithEvents AudioConverter1 As CSAudioConverter.AudioConverter
    Private Function doMP3Conversion(sWaveFileName As String) As String

        'Check if already running:
        If AudioConverter1.OperationState = CSAudioConverter.OperationState.Running Then
            Return ""
        End If

        'Check if there is source multimedia file to process:
        If sWaveFileName = "" Then
            Return ""
        End If

#Region "DecodeModeExplain"
        'Set the mode of the decoder, this can be the LocalCodecs which 
        'will use the codecs that installed On the system, Or FFMpeg 
        'which will use the FFMpeg libraries. If you decide to use the 
        'FFMpeg libraries, please make sure the FFMpeg folder with dll 
        'files is existed under the output directory of the project:
#End Region

        AudioConverter1.DecodeMode = CType([Enum].Parse(GetType(CSAudioConverter.DecodeMode), "LocalCodecs"), CSAudioConverter.DecodeMode)

        'Set the destination path and properties:

        'Set the destination file:
        AudioConverter1.DestinatioFile = sWaveFileName & ".MP3"

        '[Optional Default is MP3]
        'Set the destination audio format, this can be AAC, APE, MP2, MP3, OGG, PCM WAV, ACM WAV And WMA
        AudioConverter1.Format = CType([Enum].Parse(GetType(CSAudioConverter.Format), "MP3"), CSAudioConverter.Format)

#Region "MoreOptionalProperties"


        '[Optional: Default is 192]
        'Set the bitrate of the destination
        AudioConverter1.BitrateVal = 8

        '[Optional: Default is 44100]
        'Set the sample rate of the destination (from 8000 to 48000)
        AudioConverter1.SamplerateVal = 8000

        '[Optional: Default is 16]
        'Set the bit-depth of the destination file (8, 16, 24, 32)
        AudioConverter1.Bits = CType([Enum].Parse(GetType(CSAudioConverter.Bits), 8), CSAudioConverter.Bits)

        '[Optional: Default is 2]
        'Set the number of the channels of the destination file (1 Or 2)
        AudioConverter1.Channels = CType([Enum].Parse(GetType(CSAudioConverter.Channels), "1"), CSAudioConverter.Channels)

#End Region

        'Add one Or more source multimedia files to the SourceFiles list. 
        'If you will add more then one source file to the list, 
        'the control will join all the list into one multimedia file

        'First clear the SourceFiles list:
        AudioConverter1.SourceFiles.Clear()

        'Set the time length to converter:
        Dim from_time As TimeSpan = New TimeSpan("00", "00", "00")
        Dim to_time As TimeSpan = New TimeSpan("00", "00", "00")

        'Add the file to the SourceFiles list:
        Dim sourceFile As Options.Core.SourceFile = New Options.Core.SourceFile(sWaveFileName, from_time, to_time)

#Region "ExamplesOfAddingFiles"
        'If you want to convert all the file you can add source file without the from_time And the to_time
        'audioConverter1.SourceFiles.Add(New Options.Core.SourceFile(txtSourceMediaFile1.Text));

        'This will also convert all the file
        'audioConverter1.SourceFiles.Add(New Options.Core.SourceFile(txtSourceMediaFile1.Text, 0, 0));

        'This will convert from the from_time to the end of the file
        'audioConverter1.SourceFiles.Add(New Options.Core.SourceFile(txtSourceMediaFile1.Text, from_time, 0));
#End Region
        AudioConverter1.SourceFiles.Add(sourceFile)
        'Add more multimedia files to join as adding the second multimedia file...
        'Run the process:
        AudioConverter1.Convert()
        Return AudioConverter1.DestinatioFile
    End Function

    ' Function to check if a process is running
    Private Shared Function IsProcessRunning(processName As String) As Boolean
    End Function

    Private Sub Form_Splash_Load(sender As Object, e As EventArgs) Handles Me.Load
        System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = False
        _TerminalServerSession = System.Windows.Forms.SystemInformation.TerminalServerSession
        If _TerminalServerSession Then
            MessageBox.Show(Me, "Note: We have detected its a remote session, call recording & voice playback function might malfuction!", "nProPhony", MessageBoxIcon.Exclamation AndAlso MessageBoxButtons.OK)
        End If

        Dim args As String()

        Configurations.bDisableTrayICON = False
        AgentInformation.AppVersion = "Instance-31.6"
        Label_Version.Text = AgentInformation.AppVersion
        Try
            LogDir = System.IO.Path.GetTempPath & "SoftPhone"
            If Dir(LogDir, FileAttribute.Directory) = "" Then
                Try
                    MkDir(LogDir)
                Catch ex As Exception
                End Try
            End If
            Try
                FileWriter = New Class_FileWriter(LogDir)

                args = Environment.GetCommandLineArgs()
                For Each arg In args
                    If arg.ToString.ToUpper.Trim() = "DTI=1" Then
                        Configurations.bDisableTrayICON = True
                        Exit For
                    End If
                Next
                For Each arg In args
                    If LCase(arg.ToString.ToUpper.Trim()) = "password" Then
                        Dim password As String = InputBox("Enter the password:")
                        Dim wrapper As New Simple3Des
                        Dim AES_EncryptedPassword As String = wrapper.AES_Encrypt(password)
                        Dim newpasswordstring = ""
                        Try
                            Console.WriteLine(AES_EncryptedPassword)
                            Clipboard.SetText(AES_EncryptedPassword)
                        Catch ex As Exception
                        End Try
                        Application.Exit()
                        End
                    End If
                Next
            Catch ex As Exception
            End Try

            appPath = appPath.Replace("\nProPhony.exe", "")


            Dim processName As String = "nProPhony"
            ' Check if the process is already running
            ' Get all processes with the specified name
            Dim processes() As Process = Process.GetProcessesByName(processName)
            ' Check if any processes were found
            If processes.Length > 1 Then ' If more than one instance is found, the process is already running
                Write_Log("", " ********** Applicaiton is        ********** " & " already running...")

                MessageBox.Show("Another instance of the application is already running.", "Application Running", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Application.ExitThread()
                End
            End If


            AudioConverter1 = New CSAudioConverter.AudioConverter()
            AudioConverter1.UserName = "Your email"
            AudioConverter1.UserKey = "Your registration key"
            Configurations.RecordinChunkTimeOut = 0
            'xObject = New Utilities.IOWrapper(LogDir)
            'yObject = New Utilities.WaveIO(LogDir)

            Dim x As String = ""
            Try
                x = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName
            Catch ex As Exception
                x = "not found"
            End Try
            Write_Log("", " ********** Applicaiton is        ********** " & AgentInformation.AppVersion & ", App=" & x & ", TerminalServerSession=" & _TerminalServerSession)
            Write_Log("", " ********** Applicaiton Starting  ********** " & AgentInformation.AppVersion & ", App Path=" & appPath)
            Write_Log("", " ********** LOG FOLDER            ********** " & LogDir)
            Write_Log("", " ********** Tray ICON Disabled    ********** " & Configurations.bDisableTrayICON)

            RunUploader()

            Try
                'Debug.WriteLine("app=" & System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)
                x = FileSystem.FileDateTime(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName).ToString()
            Catch ex As Exception
                x = "AppDateTime - " & ex.Message
            End Try
            Write_Log("", " ********** App DataTime          ********** " & x)
            Try
                x = FileSystem.FileDateTime(appPath & "\" & "APIConnector.dll").ToString()
            Catch ex As Exception
                x = "APIConnector - " & ex.Message
            End Try
            Write_Log("", " ********** Connector DataTime    ********** " & x)

            SetupScreen("Startup", "")
            LoadMachineDetails()
            updateStatusText("Initializing(Screen Setup)")

            If System.IO.File.Exists(appPath & "\ring.wav") Then
                RingPlayer.SoundLocation = appPath & "\ring.wav"
                RingPlayer.LoadAsync()
            Else
                Write_Log("", " ********** No ring File found    ********** ")
            End If

            If System.IO.File.Exists(appPath & "\connected.wav") Then
                ConnectedPlayer.SoundLocation = appPath & "\connected.wav"
                ConnectedPlayer.LoadAsync()
            Else
                Write_Log("", " ********** No connected File found    ********** ")
            End If

            uCallback.SetupEvent(0, "ConnectDB", 1000, -1, -1, -1, Nothing)
            Write_Log("", " ********** Waiting DB Connection  ********** ")
            'Me.TopMost = False
            Configurations.bLogSIPDetails = False
            If GetINIStringOldWay("Configuration", "SIPDetails", "0") = "1" Then
                Write_Log("", " ********** SIP Details Logging Enabled  ********** ")
                Configurations.bLogSIPDetails = True
            End If

            If wsClientAllowed = True Then
                Try
                    If Configurations.bDisableTrayICON <> True Then
                        Me.WindowState = FormWindowState.Minimized
                    Else
                        Me.TopMost = True
                        Application.DoEvents()
                        Me.TopMost = False
                        Me.WindowState = FormWindowState.Normal
                    End If
                Catch ex As Exception
                End Try
            End If


        Catch ex As Exception
            Me.Cursor = Cursors.Default
            Write_Log("ERR", "Splash_Load(Error)" & ex.Message)
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    'Public Class ovsCCwss
    '    Inherits WebSocketBehavior
    '    Protected Overrides Sub OnMessage(id As String, e As MessageEventArgs)
    '        'MyBase.OnMessage(id, e)
    '        Try
    '            Debug.WriteLine("OnMessage() Sessions.Count:" & Sessions.Count & ", id=" & id)
    '        Catch ex As Exception
    '            Debug.WriteLine("OnMessage() Exception 1:" & ex.Message)
    '        End Try
    '        'Sessions.SendTo("ID:" & id & ",Data:" & e.Data, id)
    '        Try
    '            Sessions.MSGQueueAdd("OnMessage~~~" & id & "~~~" & e.Data)
    '            Debug.WriteLine("OnMessage(Added) " & e.Data)
    '            'wsMessages.Add("OnMessage~~~" & id & "~~~" & e.Data)
    '        Catch ex As Exception
    '            Debug.WriteLine("OnMessage() Exception 2:" & ex.Message)
    '        End Try
    '    End Sub
    '    Protected Overrides Sub OnClose(id As String, e As CloseEventArgs)
    '        'MyBase.OnClose(id, e)
    '        Try
    '            Debug.WriteLine("OnClose() Sessions.Count:" & Sessions.Count & ", id=" & id)
    '        Catch ex As Exception
    '        End Try
    '        Try
    '            Sessions.MSGQueueAdd("OnClose~~~" & id & "~~~" & Sessions.Count)
    '            'wsMessages.Add("OnClose~~~id=" & id & "~~~Sessions.Count=" & Sessions.Count)
    '        Catch ex As Exception
    '        End Try

    '    End Sub
    '    Protected Overrides Sub OnError(id As String, e As ErrorEventArgs)
    '        'MyBase.OnError(id, e)
    '        Try
    '            Debug.WriteLine("OnError() Sessions.Count:" & Sessions.Count & ", id=" & id & ",Message=" & e.Message)
    '        Catch ex As Exception
    '        End Try
    '        Try
    '            Sessions.MSGQueueAdd("OnError~~~" & id & "~~~" & e.Message)
    '            'wsMessages.Add("OnError~~~id=" & id & "~~~Message=" & e.Message)
    '        Catch ex As Exception

    '        End Try

    '    End Sub
    '    Protected Overrides Sub OnOpen(id As String)
    '        'MyBase.OnOpen(id)
    '        Try
    '            Debug.WriteLine("OnOpen() Sessions.Count:" & Sessions.Count & ", id=" & id)
    '        Catch ex As Exception

    '        End Try
    '        Try
    '            Sessions.MSGQueueAdd("OnOpen~~~" & id & "~~~" & Sessions.Count)
    '            'wsMessages.Add("OnOpen~~~" & id & "~~~" & Sessions.Count)
    '        Catch ex As Exception

    '        End Try
    '    End Sub

    'End Class

    Public Function SetupwsClient() As Boolean
        SetupwsClient = False
        Try
            Write_Log("", "SetupwsClient(Start) nLine:" & CallInformation.nLine)

            Application.DoEvents()
            Write_Log("", "SetupwsClient() nLine:" & "wsInitialize")
            uCallback.SetupEvent(0, "wsInitialize", 3, -1, -1, -1, Nothing)
            Write_Log("", "SetupwsClient() nLine:0 " & "Initialize")
            SetupwsClient = True
            sgAPIClientEnabled = False
            If GetINIString("Main", "sgAPIClientEnabled", "0") = "1" Then
                sgAPIClientEnabled = True
            End If
            Write_Log("", "SetupwsClient(End) nLine:" & "sgAPIClientEnabled:" & sgAPIClientEnabled)
        Catch ex As Exception
            Write_Log("ERR", "SetupwsClient() " & ex.Message)
        End Try
    End Function

    Private Sub uCallback_Event_Occured(Sender As Object, e As Class_UCallBack.Event_Occured_Args) Handles uCallback.Event_Occured
        Dim ReminderText = ""
        Dim timeNow As String = DateTime.Now.ToString("ss:ffff").ToString()

        Try
            ReminderText = e.ReminderText


            Select Case e.ReminderText
                Case "CheckNewEvents"
                    Try
                        uCallback.SetupEvent(-1, "CheckNewEvents", 1000, -1, -1, -1, Nothing)
                        Console.WriteLine(timeNow & " - processCheckNewEvents:Timer Enabled:" & DateTime.Now)
                        Try
                            Me.Refresh()
                        Catch ex As Exception
                        End Try
                    Catch ex As Exception
                        Console.WriteLine(timeNow & " - uCallback_Event_Occured:Exception:Timer Enabled:" & DateTime.Now)
                    End Try
                    Call processCheckNewEvents(timeNow)


                Case "StartRecordingWithDeley"
                    Try
                        If CallInformation.CallStatus = "Talking" Then
                            StartStopRecording(False, _tmpsessionID)
                            uCallback.SetupEvent(-1, "PlayConnectedBeep", Configurations.ConnectedBeepDelay, -1, -1, -1, Nothing)
                        Else
                            Write_Log("APP", "uCallback_Event_Occured(StartRecordingWithDeley) CallStatus not Talking")
                        End If
                    Catch ex As Exception
                        Write_Log("ERR", "uCallback_Event_Occured(StartRecordingWithDeley:Error)" & ex.Message)
                    End Try
                Case "PlayConnectedBeep"
                    Try
                        If CallInformation.CallStatus = "Talking" Then
                            ConnectedPlayer.Play()
                            Write_Log("APP", "uCallback_Event_Occured(Talking) Beep Played")
                        Else
                            Write_Log("APP", "uCallback_Event_Occured(PlayConnectedBeep) CallStatus not Talking")
                        End If
                    Catch ex As Exception
                        Write_Log("ERR", "uCallback_Event_Occured(PlayConnectedBeep:Error)" & ex.Message)
                    End Try

                Case "RecordingUpload"
                    If CallInformation.CallStatus = "Talking" Then
                        Console.WriteLine("RecordingStartedAT:" & DateDiff("s", CallInformation.RecordingStartedAT, Now))
                        If DateDiff("s", CallInformation.RecordingStartedAT, Now) >= 5 Then
                            CallInformation.thUploader = New Threading.Thread(Sub() AsyncUploadRecording())
                            CallInformation.thUploader.Name = "The recording uploader"
                            CallInformation.thUploader.IsBackground = True
                            CallInformation.thUploader.Start()
                        End If
                    End If


                Case "wsInitialize"
                    Dim wsConnectionTime As Long = 0
                    Dim TmpValue As String = "24"
                    TmpValue = GetINIStringOldWay("Main", "wsConnectionTimeOut", "24")
                    If IsNumeric(TmpValue) = False Then
                        TmpValue = "24"
                    End If

                    Write_Log("", "Event_Occured() Initialize")
                    Try
                        'wSS.Start()
                        If IsNothing(wSSHost) = False Then
                            wSSHost.Stop()
                        End If
                        Application.DoEvents()
                        If Configurations.UseNativeWS = False Then
                            If IsNothing(wSSHost) = False Then
                                wSSHost.Stop()
                            End If
                            Application.DoEvents()
                            wSSHost = New WebSocketServer(8181, Net.IPAddress.Loopback) With {
                                .OnReceive = AddressOf OnReceive,
                                .OnSend = AddressOf OnSend,
                                .OnConnected = AddressOf OnConnect,
                                .OnDisconnect = AddressOf OnDisconnect,
                                .TimeOut = New TimeSpan(Val(TmpValue), 0, 0)}

                            Write_Log("", "Event_Occured() wsRestaring")
                            wSSHost.Restart()
                            Write_Log("", "Event_Occured() wsRestarted")
                        Else
                            If IsNothing(wSSNativeHost) = False Then
                                If wSSNativeHost.Initialized Then
                                    wSSNativeHost.UnInitialize()
                                    Application.DoEvents()
                                End If
                            End If
                            wSSNativeHost = New Class_MSWS
                            Application.DoEvents()
                            Write_Log("", "Event_Occured() wsRestaring")
                            wSSNativeHost.Initialize()
                            Write_Log("", "Event_Occured() wsRestarted")
                        End If
                    Catch ex As Exception
                        Write_Log("", "Event_Occured() wsRestarted Exception:" & ex.Message & " - it seems like it was 2nd instnace")
                        NotifyIcon1.BalloonTipText = ""
                        NotifyIcon1.Visible = False
                        Me.TopMost = True
                        MessageBox.Show("Please ensure there is no other session is running of ProPhony Application.", "Application Running", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Application.ExitThread()
                        End
                    End Try
                    'wSSHost = wSS.WebSocketServices.Item(wsServiceName)
                    Write_Log("", "Event_Occured()" & "WebSocketServices(" & wsServiceName & ")")
                    Try
                        If resetConnection = False Then
                            'uCallback.SetupEvent(-1, "CheckNewEvents", 1, -1, -1, -1, Nothing)
                            uCallback.SetupEvent(-1, "CheckNewEvents", 1000, -1, -1, -1, Nothing)
                        End If
                        resetConnection = False
                        ContinuesCloseCounter = 0
                        Exit Sub
                    Catch ex As Exception
                        Write_Log("", "Event_Occured() Exception:" & ex.Message)
                        MessageBox.Show(Me, "Exception:" & ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try



                Case "ConnectDB"
                    Write_Log("", " ********** Connecting Database  ********** ")
                    updateStatusText("Connecting Database..")
                    dbClass = New Class_DBConnector
                    Dim x As String = ""
                    updateStatusText("wait opening database()")
                    Application.DoEvents()
                    Dim dbConnected As Boolean = False
                    Try
                        dbConnected = dbClass.OpenDatabase(x)
                    Catch exx As Exception
                        SetupScreen("Login", "")
                        Write_Log("ERR", " Connection Failed " & exx.Message)
                        updateStatusText("Database connecting failed(" & exx.Message & ")")
                        MessageBox.Show("Error connecting database. " & vbNewLine & "Exception:" & exx.Message)
                        Exit Sub
                    End Try
                    If dbConnected = False Then
                        SetupScreen("Login", "")
                        Write_Log("ERR", " Connection Failed " & "False")
                        updateStatusText("Database connecting failed(" & "False" & ")")
                        MessageBox.Show("Error connecting database. " & vbNewLine & "Exception:" & "False")
                        Exit Sub
                    End If

                    updateStatusText("Database Connected (Successfully) ")
                    Application.DoEvents()
                    Try
                        updateStatusText("loading Configuration")
                        dbClass.LoadConfiguration()
                        DoDefaultConfiguration()
                        updateStatusText("Configuration Loaded")
                    Catch ex As Exception
                        Write_Log("", "Configuration:" & ex.Message)
                        MessageBox.Show(Me, "Configuration:" & ex.Message)
                        End
                    End Try
                    updateStatusText("Database Connected (Successfully) ")

                    Write_Log("", "Intiallize Telephony Now")
                    Application.DoEvents()
                    uCallback.SetupEvent(0, "IntiallizeTelephony", 1000, -1, -1, -1, Nothing)

                Case "IntiallizeTelephony"

                    updateStatusText("Initializing Telephony.. Wait")

                    Application.DoEvents()

                    ovsControl = New ovsPSProPhony.TControl(LogDir)
                    '' Channels = Numner of SIP Accounts
                    'ovsControl.Channels = 1

                    ovsControl.SystemSetting(0, TControl.SystemSettings.AGC, False)

                    ovsControl.FoldersLog = LogDir
                    ovsControl.LogLevel = -1
                    Dim _logLevel As Integer = -1

                    _logLevel = Val(GetINIStringOldWay("Main", "DeepLogLevel", "-1"))

                    Select Case _logLevel
                        Case -1, 1, 2, 3, 4
                            ovsControl.LogLevel = _logLevel
                    End Select

                    ovsControl.CoreMode = 0

                    '' required to be reverted back to 2
                    '' for testing purpose only we have made this to 1
                    'ovsControl.MAX_LINES(0) = ServerInformation.Lines
                    '' Required nubmer of lines within each channel
                    ovsControl.MAX_LINES(0) = 1


                    '' no need to set by defauly its same 0.0.0.0
                    ovsControl.localIPAddress = "0.0.0.0"

                    Dim preferredIP As String = GetINIStringOldWay("Main", "preferredIP", "")
                    Write_Log("", "Preferred IP:" & preferredIP)

                    Dim tmpNICName As String = ""
                    Dim tmpNICCount As Integer = ovsControl.NICCount
                    Write_Log("", "NICCount:" & tmpNICCount)
                    For index = 1 To tmpNICCount
                        'If index > 9 Then Exit For
                        tmpNICName = ovsControl.localIPAddress(index)
                        If Trim(preferredIP) <> "" Then
                            If Trim(preferredIP) = Trim(tmpNICName) Then
                                ovsControl.localIPAddress = tmpNICName
                                Write_Log("", "Preferred IP is set to use only")
                            End If
                        End If
                        Write_Log("", "NIC:" & tmpNICCount & "/" & index & "=" & tmpNICName)
                        AgentInformation.FromIPAddress = AgentInformation.FromIPAddress & ovsControl.localIPAddress(index) & ","
                    Next
                    Dim tmpstring As String = ""
                    Try
                        Write_Log("", "Here to Initialize with Line= " & ServerInformation.Lines & ", SessionTimeout=" & ServerInformation.SessionTimeout & ", TCPEnabled=" & ServerInformation.TCPEnabled)
                        'If ServerInformation.TCPEnabled = True Then
                        'ovsControl.Initialize(ServerInformation.Lines, 0, LogDir, TControl.TRANSPORT_TYPE.TRANSPORT_TCP)
                        ovsControl.Initialize(1, 0, LogDir, IIf(ServerInformation.TCPEnabled, TControl.TRANSPORT_TYPE.TRANSPORT_TCP, TControl.TRANSPORT_TYPE.TRANSPORT_UDP))
                        'ovsControl.InitializeChannel(1, ServerInformation.Lines, ovsControl.CoreMode, LogDir, TControl.TRANSPORT_TYPE.TRANSPORT_TCP)
                        'Else
                        'ovsControl.Initialize(ServerInformation.Lines, 0, LogDir, TControl.TRANSPORT_TYPE.TRANSPORT_UDP)
                        'ovsControl.Initialize(1, 0, LogDir, TControl.TRANSPORT_TYPE.TRANSPORT_UDP)
                        'ovsControl.InitializeChannel(1, ServerInformation.Lines, ovsControl.CoreMode, LogDir, TControl.TRANSPORT_TYPE.TRANSPORT_TCP)

                        'End If
                        Write_Log("", "Initialize request sent")
                        Application.DoEvents()
                        For i = 0 To ServerInformation.Lines - 1
                            Write_Log("", "Setting SessionTimeout for line(" & i & ") = " & ServerInformation.SessionTimeout)
                            Try
                                ovsControl.SessionTimeout(i) = ServerInformation.SessionTimeout
                                Application.DoEvents()
                                Write_Log("", "SessionTimeout is set for line(" & i & ") = " & ServerInformation.SessionTimeout)
                            Catch ex As Exception
                                Application.DoEvents()
                                Write_Log("", "SessionTimeout is set failed for line(" & i & ") = " & ServerInformation.SessionTimeout & ", Exception=" & ex.Message)
                            End Try
                        Next
                        'ovsControl.SessionTimeout(1) = 90
                        'ovsControl.SessionTimeout(2) = 90
                        'ovsControl.InitializeChannel(0)
                    Catch ex As Exception
                        Write_Log("", "failed to initialize Telephony: " & ex.Message)
                        MessageBox.Show(Me, "failed to initialize Telephony: " & ex.Message)
                        End
                    End Try
                    Try

                        Select Case GetINIString("Main", "SIPMode", "1").ToLower().Trim()
                            Case "0", "asterisk"
                                Write_Log("", "SIP Mode: " & "Standard")
                                ovsControl.SystemSetting(0, TControl.SystemSettings.SDP, False)
                            Case Else
                                Write_Log("", "SIP Mode: " & "AVAYA")
                                ovsControl.SystemSetting(0, TControl.SystemSettings.SDP, True)
                        End Select
                    Catch ex As Exception
                        Write_Log("", "failed to set SIP Mode: " & ex.Message)
                        MessageBox.Show(Me, "failed to set SIP Mode: " & ex.Message)
                    End Try
                    Try

                        Dim tmpTotalSpeakers As Long = ovsControl.getNumSpeakers
                        Dim SpeakerIndexToRemmember As Integer = 0
                        'Dim TmpString As String = ""
                        Write_Log("", "Speakers() Total=" & tmpTotalSpeakers)
                        For index = 0 To tmpTotalSpeakers - 1
                            tmpstring = ovsControl.getSpeaker(index)
                            Write_Log("", "Speakers() " & index & "=" & tmpstring)
                            tmpstring = tmpstring.ToLower()
                            If tmpstring.Contains("plan") Then
                                SpeakerIndexToRemmember = index
                            End If
                        Next
                        tmpstring = ""
                        Dim tmpTotalMICs As Long = ovsControl.getNumMicrophones

                        Dim MICIndexToRemmember As Integer = 0
                        Write_Log("", "MICs() Total=" & tmpTotalMICs)
                        For index = 0 To tmpTotalMICs - 1
                            tmpstring = ovsControl.getMicrophones(index)
                            Write_Log("", "MICs() " & index & "=" & tmpstring)
                            tmpstring = tmpstring.ToLower()
                            If tmpstring.Contains("plan") Then
                                MICIndexToRemmember = index
                            End If
                        Next
                        tmpstring = ""
                        ovsControl.setAudioDeviceId(MICIndexToRemmember, SpeakerIndexToRemmember)
                        Write_Log("", "setAudioDeviceId() MICIndex=" & MICIndexToRemmember & ",Speaker=" & SpeakerIndexToRemmember)

                    Catch ex As Exception
                        Write_Log("", "failed to read media devices information: " & ex.Message)
                        MessageBox.Show(Me, "failed to read media devices information: " & ex.Message)
                    End Try

                Case "NoAnsCheck"
                    If ProgressBar_Answer.Value = ProgressBar_Answer.Maximum Then
                        '' marked call un answered
                        Write_Log("", "uCallback_Event_Occured(NoAns)")
                        CallInformation.CallStatus = "NoAns"
                        ovsControl.Call_Reject(0, CallInformation.nLine)
                    Else
                        If CallInformation.CallStatus = "Ringing" Then
                            If AgentInformation.DND = False Then
                                ProgressBar_Answer.Value = ProgressBar_Answer.Value + 1
                                uCallback.SetupEvent(1, "NoAnsCheck", 1000, -1, -1, -1, Nothing)
                            End If
                        End If
                    End If
            End Select
        Catch ex As Exception
            Write_Log("ERR", "uCallback_Event_Occured(Error)" & ", ReminderText=" & ReminderText & ", Exception" & ex.Message)
            MessageBox.Show("uCallback_Event_Occured(Error)" & ", ReminderText=" & ReminderText & ", Exception" & ex.Message, "uCallback_Event_Occured(Error)", MessageBoxButtons.OK)
        End Try
    End Sub
    Public Shared Sub OnConnect(ByVal aContext As UserContext)
        Try
            Dim countBefore As Long = -1
            Dim countAfter As Long = -1
            countBefore = OnlineConnections.Count
            ' Create a new Connection Object to save client context information
            Dim conn = New Connection With {.Context = aContext}
            ' Add a connection Object to thread-safe collection
            Call OnlineConnections.TryAdd(aContext.ClientAddress.ToString(), conn)

            wsMessages.Add("OnOpen~~~" & aContext.ClientAddress.ToString() & "~~~" & 0)
            countAfter = OnlineConnections.Count
            Write_Log("", "OnConnect(" & aContext.ClientAddress.ToString().ToString() & ") CountBefore=" & countBefore & ", CountAfter=" & countAfter)

        Catch ex As Exception
            Console.WriteLine("OnConnect(Error) " & ex.Message.ToString())
            Write_Log("", "OnConnect(Error) " & ex.Message)
        End Try

    End Sub



    Public Shared Sub OnReceive(ByVal aContext As UserContext)
        Dim msg As String = ""
        Dim msgfrom As String = ""
        Try
            msg = aContext.DataFrame.ToString()
            msgfrom = aContext.ClientAddress.ToString().ToString().ToString()
            Console.WriteLine("Data Received From [" & msgfrom & "] - " + msg)
            'Sessions.MSGQueueAdd("OnMessage~~~" & id & "~~~" & e.Data)
            If Trim(msg) <> "" Then
                wsMessages.Add("OnMessage~~~" & msgfrom & "~~~" & msg)
            End If
        Catch ex As Exception
            Write_Log("", "OnReceive(Error) " & ex.Message)
            Console.WriteLine("OnReceive(Error) " & ex.Message.ToString())
        End Try

    End Sub
    Public Shared Sub OnSend(ByVal aContext As UserContext)
        Dim msg As String = ""
        Dim msgto As String = ""
        Try
            msg = aContext.DataFrame.ToString()
            msgto = aContext.ClientAddress.ToString().ToString().ToString()
            Console.WriteLine("Data Sent To : " & aContext.ClientAddress.ToString().ToString())
        Catch ex As Exception
            Write_Log("", "OnSend(Error) " & ex.Message)
            Console.WriteLine("OnSend(Error) " & ex.Message.ToString())
        End Try
    End Sub
    Public Shared Sub OnDisconnect(ByVal aContext As UserContext)
        Try

            Dim CountBefore As Long = -1
            Dim CountAfter As Long = -1

            'Console.WriteLine("Client Disconnected : " & aContext.ClientAddress.ToString().ToString())
            ' Remove the connection Object from the thread-safe collection
            Dim conn As Connection = New Connection
            CountBefore = OnlineConnections.Count
            Call OnlineConnections.TryRemove(aContext.ClientAddress.ToString(), conn)
            CountAfter = OnlineConnections.Count
            ' Dispose timer to stop sending messages to the client.
            Console.WriteLine("OnDisconnect() count=" & OnlineConnections.Count)
            wsMessages.Add("OnClose~~~" & aContext.ClientAddress.ToString() & "~~~" & CountAfter)
            Write_Log("", "OnDisconnect(" & aContext.ClientAddress.ToString().ToString() & ") CountBefore=" & CountBefore & ", CountAfter=" & CountAfter)

        Catch ex As Exception
            Write_Log("", "OnDisconnect(Error) " & ex.Message)
            Console.WriteLine("OnDisconnect(Error) " & ex.Message.ToString())
        End Try

    End Sub


    Private Sub processCheckNewEvents(timenow As String)
        Try
            Try
                Console.WriteLine(timenow & " - processCheckNewEvents:" & DateTime.Now)
            Catch ex As Exception
            End Try

            Dim MSG As String = ""
            Dim LastMSG As String = ""

            'Dim MSGQueueCount As Long = wSSHost.Sessions.MSGQueueCount

            Dim MSGCount As Long = 0
            Dim AppendMsgCount As Boolean = False

            MSG = ""
            Try
                If wsMessages.Count > 0 Then
                    MSG = wsMessages.Item(1)
                    wsMessages.Remove(1)
                End If
            Catch ex As Exception
            End Try
            If Trim(MSG) <> "" Then
                While MSG <> ""
                    'If MSG.Contains("OnClose~~~") Or MSG.Contains("OnClose~~~") Then
                    'AppendMsgCount = True
                    'Try
                    'MSGCount = 0
                    'Catch ex As Exception
                    '          MSGCount = -1
                    '               End Try
                    'End If
                    If MSG.Contains("OnClose~~~") And LastMSG.Contains("OnClose~~~") Then
                        ContinuesCloseCounter = ContinuesCloseCounter + 1
                        Write_Log("", " processCheckNewEvents(" & ContinuesCloseCounter & ") Required to reset, MSGQueueGet=" & MSGCount & ", Sessions=" & 0)
                    Else
                        ContinuesCloseCounter = 0
                        resetConnection = False
                    End If

                    If ContinuesCloseCounter = 10 Then
                        resetConnection = True
                        If MSG.Contains("OnClose~~~") Then
                            CloseWSConnection(MSG)
                        End If
                        Exit While
                    End If

                    Timer_wsReader_Timer(MSG)
                    LastMSG = MSG
                    AppendMsgCount = False
                    Application.DoEvents()
                    Threading.Thread.Sleep(200)
                    Try
                        If wsMessages.Count > 0 Then
                            MSG = wsMessages.Item(1)
                            wsMessages.Remove(1)
                        Else
                            MSG = ""
                        End If
                    Catch ex As Exception
                    End Try
                End While
            End If
            If resetConnection = True Then
                '' reset web socket
                Write_Log("", " processCheckNewEvents(" & ContinuesCloseCounter & ") Closing All Connections, MSGQueueGet=" & 0 & ", Sessions=" & 0 & ",wsMessages.count" & wsMessages.Count & ", Clearing")
                resetConnection = False
                'resetConnection = True
                wsMessages.Clear()
                CloseWSConnection(MSG)
                SetupwsClient()
                Write_Log("", " processCheckNewEvents(" & ContinuesCloseCounter & ") Closed All Connections")
            End If

        Catch ex As Exception
            Write_Log("ERR", "processCheckNewEvents Exception:" & ex.Message)
        End Try
    End Sub
    Private Sub CloseWSConnection(MSG As String)
        Try
            Dim connectionID As String
            Dim tmpString() As String
            tmpString = Split(MSG, "~~~")
            connectionID = tmpString(1)
            'wSSHost.Sessions.CloseSession(connectionID)
        Catch ex As Exception
            If ex.Message.Contains("The session could not be found.") Then
                '' ignore
            Else
                Write_Log("ERR", "CloseWSConnection(" & MSG & ") Exception:" & ex.Message)
            End If
        End Try
    End Sub


    Private Sub restDBConnection(temp As String)
        Try
            updateStatusText("restDBConnection() Start ")
            Write_Log("APP", "restDBConnection() Start ")
            dbClass = Nothing
            Application.DoEvents()
            dbClass = New Class_DBConnector
            Dim x As String = ""
            Application.DoEvents()
            Dim dbConnected As Boolean = False
            Try
                dbConnected = dbClass.OpenDatabase(x)
                updateStatusText("restDBConnection() DBConnected :" & x)
                Write_Log("APP", "restDBConnection() DBConnected :" & x)

            Catch exx As Exception
                updateStatusText("restDBConnection() DBException=" & exx.Message)
                Write_Log("APP", "restDBConnection() DBException=" & exx.Message)
            End Try
        Catch ex As Exception
            Write_Log("ERR", "restDBConnection(Error)" & ex.Message & ", temp=" & temp)
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Private Sub DoDefaultConfiguration()

        Configurations.UseNativeWS = True
        If GetINIString("Main", "UseNativeWS", "1") = "0" Then
            Configurations.UseNativeWS = False
        End If


        Configurations.GetDTMFViaManager = False

        Configurations.Company = GetINIString("Main", "Company", "Default")
        Configurations.NoAns = 10

        Dim tmpVal As String
        tmpVal = GetINIString("Main", "RecordingChunks", "0")
        If IsNumeric(tmpVal) = True Then
            If Val(tmpVal) >= 5000 Then
                Configurations.RecordinChunkTimeOut = Val(tmpVal)
            End If
        End If
        tmpVal = GetINIString("Main", "NoAnsTime", "10")
        If IsNumeric(tmpVal) = True Then
            Configurations.NoAns = Val(tmpVal)
        End If

        ServerInformation.Port = 5060
        tmpVal = GetINIStringOldWay("Main", "ServerPort", "5060")
        If IsNumeric(tmpVal) = True Then
            ServerInformation.Port = Val(tmpVal)
        End If


        ServerInformation.Lines = 0
        tmpVal = Trim(LCase(GetINIStringOldWay("Main", "MaxLines", "1")))
        If IsNumeric(tmpVal) = True Then
            ServerInformation.Lines = Val(tmpVal)
        End If

        ServerInformation.SessionTimeout = -1
        tmpVal = Trim(LCase(GetINIStringOldWay("Main", "SessionTimeout ", "-1")))
        If IsNumeric(tmpVal) = True Then
            ServerInformation.SessionTimeout = Val(tmpVal)
        End If


        ServerInformation.TCPEnabled = False
        Select Case Trim(LCase(GetINIStringOldWay("Main", "CommunicationMode", "UDP")))
            Case "1", "tcp"
                ServerInformation.TCPEnabled = True
        End Select


        Configurations.CallLogsEnabled = True
        If GetINIString("Main", "CallLog", "1") = "0" Then
            Configurations.CallLogsEnabled = False
        End If

        Configurations.RecordCalls = True
        Configurations.RecordingFormat = "wav"
        If GetINIString("Main", "RecordCalls", "1") = "0" Then
            Configurations.RecordCalls = False
        Else
            Configurations.RecordCalls = True
            Configurations.RecordingFormat = "wav"
            If LCase(GetINIString("Main", "RecordingFormat", "wav")) = "mp3" Then
                Configurations.RecordingFormat = "mp3"
            End If
        End If
        Configurations.RecordingFormat = "wav"

        Configurations.URLCalling = False
        Configurations.URLSToCall.URL_Ringing_Secure = False
        Configurations.URLSToCall.URL_Connected_Secure = False
        Configurations.URLSToCall.URL_Hangup_Secure = False

        Configurations.URLSToCall.URL_Ringing_TO = 0
        Configurations.URLSToCall.URL_Connected_TO = 0
        Configurations.URLSToCall.URL_Hangup_TO = 0

        Configurations.URLSToCall.URL_Debug = True

        If GetINIString("URLCalling", "URLCalling", "0") = "1" Then

            If GetINIString("URLCalling", "URLDebug", "0") = "1" Then
                Configurations.URLSToCall.URL_Debug = True
            End If

            Dim TmpString As String
            Configurations.URLCalling = True

            Configurations.URLSToCall.URL_Ringing = GetINIString("URLCalling", "URL_Ringing", "")
            Configurations.URLSToCall.URL_Connected = GetINIString("URLCalling", "URL_Connected", "")
            Configurations.URLSToCall.URL_Hangup = GetINIString("URLCalling", "URL_Hangup", "")

            If Configurations.URLSToCall.URL_Ringing <> "" Then
                If GetINIString("URLCalling", "URL_Ringing_Secure", "0") = "1" Then
                    Configurations.URLSToCall.URL_Ringing_Secure = True
                    Configurations.URLSToCall.URL_Ringing_TO = 3
                    TmpString = GetINIString("URLCalling", "URL_Ringing_TO", "5")
                    If IsNumeric(TmpString) Then
                        Configurations.URLSToCall.URL_Ringing_TO = Val(TmpString)
                    End If
                End If
            End If

            If Configurations.URLSToCall.URL_Connected <> "" Then
                If GetINIString("URLCalling", "URL_Connected_Secure", "0") = "1" Then
                    Configurations.URLSToCall.URL_Connected_Secure = True
                    Configurations.URLSToCall.URL_Connected_TO = 3
                    TmpString = GetINIString("URLCalling", "URL_Connected_TO", "5")
                    If IsNumeric(TmpString) Then
                        Configurations.URLSToCall.URL_Connected_TO = Val(TmpString)
                    End If
                End If
            End If

            If Configurations.URLSToCall.URL_Hangup <> "" Then
                If GetINIString("URLCalling", "URL_Hangup_Secure", "0") = "1" Then
                    Configurations.URLSToCall.URL_Hangup_Secure = True
                    Configurations.URLSToCall.URL_Hangup_TO = 3
                    TmpString = GetINIString("URLCalling", "URL_Hangup_TO", "5")
                    If IsNumeric(TmpString) Then
                        Configurations.URLSToCall.URL_Hangup_TO = Val(TmpString)
                    End If
                End If
            End If

        End If


        ListView_Workcodes.Columns.Add("Selected", 50, HorizontalAlignment.Right) 'Add column 3
        ListView_Workcodes.Columns.Add("Key", 50, HorizontalAlignment.Right) 'Add column 3
        ListView_Workcodes.Columns.Add("Workcode", 200, HorizontalAlignment.Left) 'Add column 2
        Dim wsDT As DataTable
        wsDT = dbClass.Get_Workcodes

        Dim tmpString1(2) As String
        For Each row In wsDT.Rows

            '"KEY" & DatabaseInformation.DBRecordSet("RowID"), "" & DatabaseInformation.DBRecordSet("Description")
            tmpString1(0) = "KEY" & row("RowID")
            tmpString1(1) = "KEY" & row("RowID")
            tmpString1(2) = "" & row("Description")
            Dim tempNode As New ListViewItem
            tempNode = New ListViewItem(tmpString1)
            ListView_Workcodes.Items.Add(tempNode)
        Next


        'RunDBSelectionQuery("SELECT * FROM TBL_WorkCodes where status = 1 order by Description")
        'Do While Not DatabaseInformation.DBRecordSet.EOF
        'Form_Main.TreeView_WorkCodes.Nodes.Add , , "KEY" & DatabaseInformation.DBRecordSet("RowID"), "" & DatabaseInformation.DBRecordSet("Description")
        'DatabaseInformation.DBRecordSet.MoveNext
        'DoEvents
        'Loop


        wsClientCloseAllConnections = True
        If GetINIString("Main", "wsClientAllowed", "0") = "1" Then
            wsClientAllowed = True
            If GetINIStringOldWay("Main", "wsClientCloseAllConnections", "1") = "0" Then
                wsClientCloseAllConnections = False
            End If
            Call SetupwsClient()
        End If

        Configurations.ExtPooling = False    ' By Default, Need to make this false to make things backword compatable
        If GetINIString("Main", "ExtPooling", "0") = "1" Then
            'WriteAppLog "Main:DB:ExtPooling=True"
            Configurations.ExtPooling = True
        End If
        AgentInformation.ActivitySessionID = 0

        Configurations.ConnectedBeepDelay = 1000
        tmpVal = GetINIString("Main", "ConnectedBeepDelay", Configurations.ConnectedBeepDelay.ToString())
        If IsNumeric(tmpVal) = True Then
            'If Val(tmpVal) > 0 Then
            Configurations.ConnectedBeepDelay = Val(tmpVal)
            'End If
        End If
        tmpVal = ""

        Configurations.RecordingStartDelay = 1500
        tmpVal = GetINIString("Main", "RecordingStartDelay", Configurations.RecordingStartDelay.ToString())
        If IsNumeric(tmpVal) = True Then
            Select Case tmpVal
                Case > Configurations.RecordingStartDelay
                    Configurations.RecordingStartDelay = Val(tmpVal)
                Case <= 0
                    Configurations.RecordingStartDelay = Val(tmpVal)
                Case Else
                    Configurations.RecordingStartDelay = 2
            End Select
        End If


    End Sub
    Private Sub SetupScreen(xScreen As String, OtherText As String)
        Try
            Write_Log("", "SetupScreen(Starting) nLine:" & xScreen & " " & OtherText)

            Select Case xScreen
                Case "Ready"
                    updateStatusText("Getting Ready in " & OtherText & " Mode")
                    ProgressBar_Answer.Value = 0
                    GroupBox_Workcode.Enabled = False
                    GroupBox_Workcode.Visible = False
                    Try
                        GroupBox_Mode.Enabled = False
                        GroupBox_Mode.Visible = False
                        updateStatusText("Done")
                    Catch ex As Exception
                        updateStatusText(ex.Message)
                    End Try

                    Label_AgentMode.Text = "Mode:" & OtherText
                    Label_AgentStatus.Text = "State:" & "Idle"
                    If Val(AgentInformation.ActivitySessionID) > -1 Then
                        Try
                            dbClass.Activity_Set_SessionID(OtherText)
                        Catch ex As Exception
                            Dim tmpText As String = "SetupScreen(Activity_Set_SessionID) nLine:" & xScreen & " " & "Exception:" & " " & ex.Message
                            Write_Log("", tmpText)
                        End Try

                    End If
                    Try
                        Call dbClass.Activity_Get_SessionID("Idle", True)
                    Catch ex As Exception
                        Dim tmpText As String = "SetupScreen(Activity_Get_SessionID) nLine:" & xScreen & " " & "Exception:" & " " & ex.Message
                        Write_Log("", tmpText)
                    End Try

                    If CallInformation.RefID_DB > -1 Then
                        Try
                            Call dbClass.SetCallInformation("ACW", "")
                        Catch ex As Exception
                            Dim tmpText As String = "SetupScreen(Activity_Get_SessionID) nLine:" & xScreen & " " & "Exception:" & " " & ex.Message
                            Write_Log("", tmpText)
                        End Try
                    End If

                    'Label_Recording.BackColor = Frame_Login.BackColor
                    'Label_Recording.ForeColor = vbYellow
                    'Label_Recording.Caption = ""

                    'Label_Queue.Caption = ""
                    'TextBox_Number.Text = ""

                    CallInformation.SendBacktoIVR = False
                    CallInformation.SendbacktoIVRLanguage = ""
                    CallInformation.SendbacktoIVRVDN = ""

                    CallInformation.RefID_Server = -1
                    CallInformation.RefID_DB = -1
                    CallInformation.CallerID = ""
                    CallInformation.CalledID = ""
                    CallInformation.RecordingFileName = ""

                    CallInformation.CRMSessionID = ""
                    CallInformation.CRMInteractionID = ""
                    CallInformation.CustomerID = ""

                    CallInformation.CallerVerifyed = -1
                    CallInformation.FeedBack = -2

                    CallInformation.HangupByAgent = False
                    CallInformation.Transfering = False

                    CallInformation.nLine = -1
                    CallInformation.nLineSessionID1 = -1
                    CallInformation.nLineSessionID2 = -1

                    CallInformation.Transfering = False
                    CallInformation.nLineSessionStatus = "None"

                    CallInformation.DestinationExt = ""


                    CallInformation.TalkingStartedAt = DateTime.Now
                    CallInformation.TotalTalkTime = 0

                    ovsControl.Microphone(False)
                    TextBox_Number.Text = ""

                    Button_Call_Conf.Enabled = False
                    Button_Call_Trans.Enabled = False
                    Button_Call_Hold.Enabled = False

                    'Button_Call_Hold.Image = My.Resources.icon_hold
                    Button_Call_Hold.Tag = ""
                    Button_Call_Hold.Text = "&Hold"


                    'Button_Call_Mute.Image = My.Resources.icon_unmute
                    Button_Call_Mute.Enabled = False
                    Button_Call_Mute.Tag = ""
                    Button_Call_Mute.Text = "&Mute"

                    Button_Call_Hangup.Enabled = False
                    Button_Call_Connect.Enabled = False
                    If OtherText = "Inbound" Then
                        ovsControl.DND(0) = False
                        Application.DoEvents()
                        Button_Call_Connect.Text = "&Accept"
                        Button_Call_Hangup.Text = "&Reject"
                        CallInformation.CallDirection = "Inbound"
                        TextBox_Number.ReadOnly = True
                        updateStatusText("Waiting for inbound call for you")
                    Else
                        ovsControl.DND(0) = True
                        Button_Call_Connect.Text = "&Call"
                        Button_Call_Hangup.Text = "&Hangup"
                        CallInformation.CallStatus = ""
                        CallInformation.CallDirection = "Outbound"
                        Button_Call_Connect.Enabled = True
                        TextBox_Number.ReadOnly = False
                        updateStatusText("Now you can make call")
                    End If
                    AgentInformation.DND = False
                    AgentInformation.DNDCaption = ""
                    Try
                        dbClass.UpdateRealTimeStatus(CallInformation.CallDirection, "")
                    Catch ex As Exception
                        Dim tmpText As String = "SetupScreen(UpdateRealTimeStatus) nLine:" & xScreen & " " & "Exception:" & " " & ex.Message
                        Write_Log("", tmpText)
                    End Try
                    CallInformation.CallingOut = False
                    GroupBox_Call.Enabled = True
                    GroupBox_Call.Visible = True
                    GroupBox_Call.BringToFront()
                    Label_Select_DND.Enabled = True


                Case "DND"
                    RunUploader()
                    ovsControl.DND(0) = True

                    AgentInformation.DND = True
                    AgentInformation.DNDCaption = OtherText

                    If Val(AgentInformation.ActivitySessionID) > 0 Then
                        Try
                            dbClass.Activity_Set_SessionID(AgentInformation.DNDCaption)
                        Catch ex As Exception
                            Dim tmpText As String = "SetupScreen(Activity_Set_SessionID) nLine:" & xScreen & " " & "Exception:" & " " & ex.Message
                            Write_Log("", tmpText)
                        End Try
                    End If

                    Try
                        dbClass.Activity_Get_SessionID(AgentInformation.DNDCaption, True)
                    Catch ex As Exception
                        Dim tmpText As String = "SetupScreen(Activity_Get_SessionID) nLine:" & xScreen & " " & "Exception:" & " " & ex.Message
                        Write_Log("", tmpText)
                    End Try
                    Try
                        dbClass.UpdateRealTimeStatus(OtherText, "")
                    Catch ex As Exception
                        Dim tmpText As String = "SetupScreen(UpdateRealTimeStatus) nLine:" & xScreen & " " & "Exception:" & " " & ex.Message
                        Write_Log("", tmpText)
                    End Try

                    If OtherText <> "NR-ACW" Then
                        CallInformation.RefID_DB = -1
                        CallInformation.RefID_Server = -1
                    End If
                    If OtherText = "NR-ACW" Or OtherText = "Reject" Or OtherText = "Failed" Or OtherText = "Cancel" Then
                        Try
                            Call dbClass.SetCallInformation("CallEnd", "")
                        Catch ex As Exception
                            Dim tmpText As String = "SetupScreen(SetCallInformation) nLine:" & xScreen & " " & "Exception:" & " " & ex.Message
                            Write_Log("", tmpText)
                        End Try
                        CallInformation.CallStatus = "NR-ACW"
                    End If
                    'If OtherText = "NR-Login" Then
                    'sendTowsClinet("{" & """cmd"":{""ts"":""" & tsString & """,""login"":""loggin-in"",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}")
                    'End If


                    GroupBox_Login.Enabled = False
                    GroupBox_Login.Visible = False
                    GroupBox_Mode.Visible = True
                    GroupBox_Mode.Enabled = True
                    GroupBox_Mode.BringToFront()
                    Application.DoEvents()
                    Label_AgentStatus.Text = "State:" & OtherText
                    updateStatusText("Take the next required action")
                    sendTowsClinet("{" & """cmd"":{""ts"":""" & tsString & """,""nr"":" & """" & OtherText & """" & ",""ActivitySessionID"":""" & AgentInformation.ActivitySessionID & """ }" & "}")

                    If OtherText = "Failed" Or OtherText = "Reject" Or OtherText = "Cancel" Then
                        OtherText = "NR-ACW"
                    End If

                    If OtherText = "NR-ACW" Then
                        Call GoDoCheckNoACW()
                    End If

                Case "Startup"
                    RunUploader()
                    Me.Cursor = Cursors.WaitCursor
                    Me.Width = 376
                    Me.Height = 270
                    Me.StartPosition = FormStartPosition.CenterScreen
                    Application.DoEvents()
                    GroupBox_Login.Visible = True
                    GroupBox_Login.Enabled = True

                    TextBox_LoginName.Enabled = False
                    TextBox_Password.Enabled = False
                    Button_Login.Enabled = False
                    GroupBox_Login.BringToFront()
                    Application.DoEvents()

                    Application.DoEvents()
                    AgentInformation.LoggedIn = False
                    AgentInformation.LoginName = ""
                    AgentInformation.AgentID = ""
                    AgentInformation.FirstName = ""
                    AgentInformation.LastName = ""
                    AgentInformation.Supervisor = False

                    GroupBox_Title.Left = controlBoxLeft
                    GroupBox_Title.Top = -10
                    GroupBox_Mode.Left = controlBoxLeft
                    GroupBox_Mode.Top = controlBoxTop

                    GroupBox_Login.Left = controlBoxLeft
                    GroupBox_Login.Top = controlBoxTop

                    GroupBox_Workcode.Left = controlBoxLeft
                    GroupBox_Workcode.Top = controlBoxTop

                    GroupBox_Call.Enabled = False
                    GroupBox_Call.Visible = False
                    GroupBox_Call.Location = New System.Drawing.Point(0, 35)

                    GroupBox_Call.Left = controlBoxLeft
                    GroupBox_Call.Top = controlBoxTop


                    GroupBox_Workcode.Visible = False
                    GroupBox_Workcode.Enabled = False

                    GroupBox_Mode.Visible = False
                    GroupBox_Mode.Enabled = False

                    Application.DoEvents()
                    GroupBox_Login.Enabled = True
                    Button_Login.Enabled = True

                    Me.Cursor = Cursors.Default

                Case "Login"
                    RunUploader()
                    Me.Cursor = Cursors.Default
                    Application.DoEvents()
                    Try
                        Application.DoEvents()
                        GroupBox_Login.Visible = True
                        GroupBox_Login.Enabled = True
                        Application.DoEvents()
                    Catch ex As Exception
                    End Try
                    updateStatusText("Now login please..")
                    If wsClientAllowed = True Then
                        Try
                            If Configurations.bDisableTrayICON <> True Then
                                Me.WindowState = FormWindowState.Minimized
                            Else
                                Me.TopMost = True
                                Application.DoEvents()
                                Me.TopMost = False
                                Me.WindowState = FormWindowState.Normal
                            End If
                        Catch ex As Exception
                        End Try
                    End If

                    Application.DoEvents()
                    GroupBox_Mode.Visible = False
                    GroupBox_Mode.Enabled = False
                    GroupBox_Login.Visible = True
                    GroupBox_Login.Enabled = True
                    GroupBox_Login.BringToFront()
                    TextBox_LoginName.Enabled = True
                    TextBox_Password.Enabled = True
                    TextBox_LoginName.Text = ""
                    TextBox_Password.Text = ""
                    Button_Login.Enabled = True
                    Application.DoEvents()
                    Try
                        TextBox_LoginName.Focus()
                    Catch ex As Exception
                        Dim tmpText As String = "SetupScreen(Error) nLine:" & xScreen & " " & OtherText & " " & ex.Message
                        Write_Log("", tmpText)
                    End Try

            End Select
            Write_Log("", "SetupScreen(End) nLine:" & xScreen & " " & OtherText)

        Catch ex As Exception
            Dim tmpText As String = "SetupScreen(Error) nLine:" & xScreen & " " & OtherText & " " & ex.Message
            Write_Log("", tmpText)
            MessageBox.Show(Me, tmpText)
        End Try
    End Sub
    Private Sub GoDoCheckNoACW()
        Try
            If GetINIString("Main", "NoACW", "0") = "1" Then
                updateStatusText("Auto Ready is Configured")
                If CallInformation.CallDirection = "Outbound" Then
                    SetupScreen("Ready", "Outbound")
                Else
                    SetupScreen("Ready", "Inbound")
                End If
            End If
        Catch ex As Exception

            Write_Log("", "Process_GoDoCheckNoACW:Error:" & ex.Message)
            MsgBox("Process_GoDoCheckNoACW:" & ex.Message)

        End Try

    End Sub

    Private Sub ovsControl_Event_Initialize(Sender As Object, e As TControl.Event_Initialize_Args) Handles ovsControl.Event_Initialize
        Write_Log("", "Event_Initialize() lines=" & e.lines & ",ip=" & e.ip)

        ovsControl.FoldersVoice = Application.StartupPath
        Write_Log("", "FoldersVoice=" & ovsControl.FoldersVoice)

        Dim tmpstring As String = ""
        Dim num As Integer = 0
        num = ovsControl.getNumSpeakers()
        For i As Integer = 0 To num - 1
            tmpstring = "Speakers(" & i & ") " & ovsControl.getSpeaker(i)
            Write_Log("", tmpstring)
            Application.DoEvents()
        Next
        num = ovsControl.getNumMicrophones
        For i As Integer = 0 To num - 1
            tmpstring = "Microphones(" & i & ") " & ovsControl.getMicrophones(i)
            Write_Log("", tmpstring)
            Application.DoEvents()
        Next
        SetupScreen("Login", "")
    End Sub

    Private Sub TextBox_LoginName_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox_LoginName.KeyDown
        Try
            If Trim((TextBox_LoginName.Text)) <> "" Then
                If e.KeyCode = Keys.Enter Then
                    TextBox_Password.Focus()
                End If
            End If

        Catch ex As Exception
        End Try

    End Sub


    Private Sub TextBox_Password_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox_Password.KeyDown
        Try

            If Trim((TextBox_Password.Text)) <> "" Then
                If e.KeyCode = Keys.Enter Then
                    Button_Login_Click(sender, e)
                End If
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Button_Login_Click(sender As Object, e As EventArgs) Handles Button_Login.Click
        Dim bIngnorePassword As Boolean = False

        Try
            If Button_Login.Enabled = False Then
                Exit Sub
            End If

            If Trim((TextBox_Password.Text)) = "" Then
                updateStatusText("Password is required")
                Try
                    TextBox_Password.Focus()
                Catch ex As Exception
                    Button_Login.Enabled = True
                    Dim tmpText As String = "Button_Login_Click(Error) Password:" & " " & ex.Message
                    Write_Log("", tmpText)
                End Try
                SetupScreen("Login", "")
                updateStatusText("Password is required")
                Exit Sub
            End If
            If Trim((TextBox_LoginName.Text)) = "" Then
                updateStatusText("User Name is required")
                TextBox_Password.Text = ""
                Try
                    TextBox_LoginName.Focus()
                Catch ex As Exception
                    SetupScreen("Login", "")
                    Dim tmpText As String = "Button_Login_Click(Error) LoginName:" & " " & ex.Message
                    Write_Log("", tmpText)
                End Try
                Exit Sub
            End If
            Button_Login.Enabled = False
            GroupBox_Login.Enabled = False
            If wsClientAllowed = True Then
                If bRequestFromWSClent = True Then
                    bIngnorePassword = True
                End If
            End If
            Dim rDataTable As DataTable = dbClass.VerifyCredentials(TextBox_LoginName.Text, TextBox_Password.Text, bIngnorePassword)
            bRequestFromWSClent = False
            Debug.WriteLine("VerifyCredentials:" & rDataTable.Rows.Count)
            If rDataTable.Rows.Count <= 0 Then
                updateStatusText("User name or password is invalid..")
                TextBox_Password.Text = ""
                Try
                    TextBox_Password.Focus()
                Catch ex As Exception
                    If bIngnorePassword Then
                        Dim msgFromWSClinet As String = "{" & """cmd"":{""ts"":""" & tsString & """,""login"":""Sorry, User name or password invalid"",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                        sendTowsClinet(msgFromWSClinet)
                    End If
                    Button_Login.Enabled = True
                    Dim tmpText As String = "Button_Login_Click(Error) Password1:" & " " & ex.Message
                    Write_Log("", tmpText)
                End Try
                Button_Login.Enabled = True
                GroupBox_Login.Enabled = True
                Exit Sub
            End If
            updateStatusText("Credentials verfyied..")

            AgentInformation.LoginName = rDataTable(0)("LoginName")
            AgentInformation.AgentID = "" & rDataTable(0)("RowID")
            AgentInformation.FirstName = "" & rDataTable(0)("FirstName")
            AgentInformation.LastName = "" & rDataTable(0)("LastName")
            AgentInformation.Supervisor = False
            If ("" & rDataTable(0)("Supervisor") = True) Then
                AgentInformation.Supervisor = True
            End If


            AgentInformation.PABXExt = "" & rDataTable(0)("Ext")
            Write_Log("", "Button_Login(Default)" & "AgentInformation.PABXExt=" & AgentInformation.PABXExt)

            AgentInformation.PABXSecret = "" & rDataTable(0)("ExtSecret")
            AgentInformation.LoginDateTime = DateTime.Now
            AgentInformation.QueueID = rDataTable(0)("QueueID")
            AgentInformation.QueueName = ""

            If AgentInformation.Supervisor = True Then
                Select Case LCase(Command$)
                    Case "backup", "autobackup"
                    Case "restore"
                End Select
            End If

            AgentInformation.PoolID = ""
            If Configurations.ExtPooling = False Then
                ServerInformation.Primary = LCase(GetINIStringOldWay("Main", "Server", ""))
                If Trim(ServerInformation.Primary) = "" Then
                    If bIngnorePassword Then
                        Dim msgFromWSClinet As String = "{" & """cmd"":{""ts"":""" & tsString & """,""login"":""Sorry, No telephony server found in configuration"",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                        sendTowsClinet(msgFromWSClinet)
                    End If
                    SetupScreen("Login", "")
                    updateStatusText("No telephony server found in configuration")
                    MessageBox.Show(Me, "No telephony server found in configuration")
                    Exit Sub
                End If

                Try
                    ServerInformation.Port = Val(GetINIStringOldWay("Main", "ServerPort", "5060"))
                Catch ex As Exception
                    ServerInformation.Port = "5060"
                End Try
                Application.DoEvents()
            Else

                AgentInformation.PABXExt = ""
                AgentInformation.PABXSecret = ""
                ServerInformation.Primary = ""
                ServerInformation.Port = 5060

                Dim xDT As New DataTable
                xDT = dbClass.Extenstion_GET_FromPool(AgentInformation.AgentID)
                If xDT.Rows.Count <= 0 Then
                    If bIngnorePassword Then
                        Dim msgFromWSClinet As String = "{" & """cmd"":{""ts"":""" & tsString & """,""login"":""Sorry, no idle resouce found, releae some resources and try again"",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                        sendTowsClinet(msgFromWSClinet)
                    End If
                    TextBox_Password.Text = ""
                    SetupScreen("Login", "")
                    Write_Log("", "Login(), No ext found AgentID:" & AgentInformation.AgentID)
                    updateStatusText("Sorry, no idle resouce found, releae some resources and try again")
                    Write_Log("", "Button_Login(POOL)" & "AgentInformation.PABXExt=" & "" & "Sorry, no idle resouce found, releae some resources and try again")
                    MessageBox.Show(Me, "Sorry, no idle resouce found, releae some resources and try again")
                    Exit Sub
                End If

                AgentInformation.PoolID = "" & xDT.Rows(0)("RowID")
                AgentInformation.PABXExt = "" & xDT.Rows(0)("Ext")
                Write_Log("", "Button_Login(POOL)" & "AgentInformation.PABXExt=" & AgentInformation.PABXExt & ", AgentInformation.PoolID=" & AgentInformation.PoolID)
                Me.Text = Me.Text & "(" & AgentInformation.PoolID & "," & AgentInformation.PABXExt & ")"

                AgentInformation.PABXSecret = "" & xDT.Rows(0)("Password")
                ServerInformation.Primary = "" & xDT.Rows(0)("ServerIP")
                ServerInformation.Port = Val("0" & xDT.Rows(0)("ServerPort"))
                If ("" & xDT.Rows(0)("TCP") = "1") Then
                    ServerInformation.TCPEnabled = True
                End If
            End If
            updateStatusText("Telephony Registering..")
            Application.DoEvents()
            sendTowsClinet("{" & """cmd"":{""ts"":""" & tsString & """,""Login"":" & """Requesting""" & ",""State"":""" & 0 & """,""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}")
            DoTelephonyRegister()
        Catch ex As Exception
            If bIngnorePassword Then
                Dim msgFromWSClinet As String = "{" & """cmd"":{""ts"":""" & tsString & """,""login"":""Sorry, Exception=""" & ex.Message & """,""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                sendTowsClinet(msgFromWSClinet)
            End If

            SetupScreen("Login", "")
            Write_Log("", "Button_Login()" & ex.Message)
            MessageBox.Show(Me, "Button_Login()" & ex.Message)
        End Try
    End Sub
    Private Sub DoTelephonyUnRegister()

        Try
            ovsControl.ExtRegister(0, False)
            AgentInformation.LoggedIn = False
        Catch ex As Exception
            Write_Log("", "TelephonyUnRegister()" & ex.Message)
            MessageBox.Show(Me, "TelephonyUnRegister()" & ex.Message)
        End Try

    End Sub

    Private Sub DoTelephonyRegister()
        Try
            updateStatusText("SIP@" & ServerInformation.Primary & ":" & ServerInformation.Port)
            Write_Log("", "DoTelephonyRegister() SIP@" & ServerInformation.Primary & ": " & ServerInformation.Port)
            Application.DoEvents()

            ovsControl.User_UserName(0) = AgentInformation.PABXExt
            ovsControl.User_AuthName(0) = AgentInformation.PABXExt
            ovsControl.User_Display(0) = AgentInformation.FirstName & " " & AgentInformation.LastName
            ovsControl.User_Secret(0) = AgentInformation.PABXSecret
            ovsControl.User_Domain(0) = ServerInformation.Primary

            ovsControl.SIPServerIP(0) = ServerInformation.Primary
            ovsControl.SIPServerPort(0) = ServerInformation.Port

            ovsControl.SIPProxtIP(0) = ""
            ovsControl.SIPProxtPort(0) = 0

            ovsControl.SIPStunIP(0) = ""
            ovsControl.SIPStunPort(0) = 0
            Try
                updateStatusText("Wait, " & AgentInformation.PABXExt & "@" & ServerInformation.Primary & ":" & ServerInformation.Port)
                Write_Log("", "DoTelephonyRegister()" & "Register requested, AgentInformation.PABXExt=" & AgentInformation.PABXExt & "@" & ServerInformation.Primary & ":" & ServerInformation.Port)
                ovsControl.ExtRegister(0, True)
            Catch ex As Exception
                Write_Log("", "TelephonyRegister()" & ex.Message)
                MessageBox.Show(Me, "TelephonyRegister()" & ex.Message)
            End Try
        Catch ex As Exception
            Write_Log("", "TelephonyRegister()" & ex.Message)
            MessageBox.Show(Me, "TelephonyRegister()" & ex.Message)

        End Try
    End Sub

    Private Sub ovsControl_Event_ExtRegister(Sender As Object, e As TControl.Event_ExtRegister_Args) Handles ovsControl.Event_ExtRegister
        Write_Log("", "ExtRegister() registered=" & e.registered & ", statusCode=" & e.statusCode)

        If e.statusCode = "0" Then
            updateStatusText("Logout!")
            sendTowsClinet("{" & """cmd"":{""ts"":""" & tsString & """,""Login"":" & """Sorry""" & ",""State"":""" & e.statusCode & """,""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}")
        ElseIf e.statusCode = "408" Then
            updateStatusText("Sorry,Request timeout!")
            'sendTowsClinet("{" & """cmd"":{""ts"":""" & tsString & """,""login"":""Sorry,408=Timeout"",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}")
            sendTowsClinet("{" & """cmd"":{""ts"":""" & tsString & """,""Login"":" & """Sorry""" & ",""State"":""" & e.statusCode & """,""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}")
        ElseIf e.statusCode = "200" Then
            Call Process_LoggedInSuccess("")
            Exit Sub
        Else
            updateStatusText("Sorry failed with code:" & IIf(e.statusCode = "401", "401-Unauthorized", e.statusCode))
            sendTowsClinet("{" & """cmd"":{""ts"":""" & tsString & """,""Login"":" & """Sorry""" & ",""State"":""" & e.statusCode & """,""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}")
        End If
        'sendTowsClinet("{" & """cmd"":{""ts"":""" & tsString & """,""login"":""failed-"""" & e.statusCode &  """",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}")
    End Sub
    Private Sub CallURL(ByVal Activity As String, ByVal URL As String)

        If wsClientAllowed = True Then
            Dim TmpString As String
            TmpString = "{" & """StatusChange""" & ":{" &
            """Activity""" & ":" & """" & Activity & """" & "," &
            """CallerID""" & ":" & """" & CallInformation.CallerID & """" & "," &
            """CalledID""" & ":" & """" & CallInformation.CalledID & """" & "," &
            """CustomerID""" & ":" & """" & CallInformation.CustomerID & """" & "," &
            """CallerVerifyed""" & ":" & """" & CallInformation.CallerVerifyed & """" & "," &
            """FeedBack""" & ":" & """" & CallInformation.FeedBack & """" & "," &
            """RefID_DB""" & ":" & """" & CallInformation.RefID_DB & """" & "," &
            """RefID_Server""" & ":" & """" & CallInformation.RefID_Server & """" & "," &
            """CallDirection""" & ":" & """" & CallInformation.CallDirection & """" & "," &
            """QueueID""" & ":" & """" & AgentInformation.QueueID & """" & "," &
            """QueueName""" & ":" & """" & AgentInformation.QueueName & """" & "," &
            """talktime""" & ":" & """" & CallInformation.TotalTalkTime & """" & "," &
            """mode""" & ":" & """" & AgentInformation.ReadyMode & """" & "," &
            """ivr""" & ":" & """" & IVRisWorking & """" & "," &
            """CRMInteractionID""" & ":" & """" & CallInformation.CRMInteractionID & """" & "," &
            """CRMSessionID""" & ":" & """" & CallInformation.CRMSessionID & """" &
            "}}"
            sendTowsClinet(TmpString)
            Exit Sub
        End If
    End Sub
    'Private Sub sg_ASYNC_Send_Session_Close()
    'Dim StartTime As DateTime = Now
    'Try
    '       Write_Log("", "sg_ASYNC_Send_Session_Close(Start) in (0)")

    'Dim thread As New Thread(Sub()
    '                                    sg_Send_Session_Close()
    'End Sub)
    '       thread.Start()
    'Dim xDiff = DateDiff(DateInterval.Second, StartTime, Now)
    '       Write_Log("", "sg_ASYNC_Send_Session_Close(End) in (" & xDiff & ")")
    'Catch ex As Exception
    '        Write_Log("", "sg_ASYNC_Send_Session_Close(Error) nLine:" & CallInformation.nLine & " " & ex.Message)
    'End Try
    'End Sub
    Private Sub sg_Send_Session_Close()
        Try
            sTempResponseString = ""
            If sgAPIClientEnabled Then
                Write_Log("", "sg_Send_Session_Close:True, CallInformation.CallerID:" & CallInformation.CallerID & ", CallInformation.CRMSessionID:" & CallInformation.CRMInteractionID & ", CallInformation.CRMSessionID:" & CallInformation.CRMSessionID)
                Dim xTimeOut As Long
                xTimeOut = 5
                Dim sTmpString As String
                sTmpString = Trim(GetINIString("sgAPI", "APICloseSessionTimeOut", "5"))
                If IsNumeric(sTmpString) = True Then
                    xTimeOut = Val(sTmpString)
                End If
                Write_Log("", "sg_Send_Session_Close:Not Nothing,xTimeOut=" & xTimeOut)
                Dim sgAPIURL As String
                sgAPIURL = GetINIString("sgAPI", "APICloseSession", "http://18.188.178.46:8100" & "/api/Ivr/CloseSession")

                Dim sgAPITOKEN As String
                sgAPITOKEN = GetINIString("sgAPI", "APITOKEN", "TOKEN")
                Write_Log("", "sg_Send_Session_Close:Not Nothing,xTimeOut=" & xTimeOut & ",sgAPIURL=" & sgAPIURL)
                Dim tmpText As String = ""
                Dim APIClient As New APIConnector.Connector(LogDir, 1, "", "", "", 0, "sgBOK")
                Try
                    tmpText = APIClient.sgBOK_EnqueueCall(sgAPIURL, sgAPITOKEN, CallInformation.CRMInteractionID, CallInformation.CRMSessionID, CallInformation.CustomerID, CallInformation.CalledID, "Agent-" & AgentInformation.AgentID, CallInformation.CustomerID, AgentInformation.LoginName, 0, xTimeOut, False)
                    If tmpText <> "" Then
                        Write_Log("", "sg_Send_Session_Close:tmpText:" & tmpText & ",tmpString=" & tmpText & ", TimeOut=" & xTimeOut & ", Interaction=" & CallInformation.CRMInteractionID & ", CRMSessionID=" & CallInformation.CRMSessionID)
                    End If
                Catch ex As Exception
                    Write_Log("", "sg_Send_Session_Close:Error:" & ex.Message & ",tmpString=" & tmpText & ", TimeOut=" & xTimeOut & ", Interaction=" & CallInformation.CRMInteractionID & ", CRMSessionID=" & CallInformation.CRMSessionID)
                End Try
            End If
        Catch ex As Exception
            Write_Log("", "sg_Send_Session_Close(Error) nLine:" & CallInformation.nLine & " " & ex.Message)
            MessageBox.Show(Me, "sg_Send_Session_Close()" & ex.Message)

        End Try
    End Sub

    Private Sub Process_CallDisconnected(Caller As String, Callee As String, sessionID As String, SIPString As String)
        Try
            Write_Log("", "Process_CallDisconnected(Starting) nLine:" & CallInformation.nLine & ", DND:" & ovsControl.DND & ", AgentInformation.DND:" & AgentInformation.DND & ", Transfering:" & CallInformation.Transfering & ", CallingOut:" & CallInformation.CallingOut & ", SIPString:" & SIPString & ", IVR = " & IVRisWorking)
            Dim msgFromWSClinet As String = ""

            If SIPString = "Rejected incoming call due to DND" Then
                Write_Log("", "Process_CallDisconnected(End) nLine:" & CallInformation.nLine & " is ignored as its a disconnect msg for rejected call")
                Exit Sub
            End If


            If CallInformation.Transfering Then
                If CallInformation.nLineSessionID2 = sessionID Or CallInformation.nLineSessionID2 = 0 Then
                    msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""callagent"":""Disconnected"",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ ,""talktime"":""" & CallInformation.TotalTalkTime & """ }" & "}"
                    sendTowsClinet(msgFromWSClinet)
                    updateStatusText("Agent Disconnected")
                    CallInformation.Transfering = False
                    Exit Sub
                ElseIf CallInformation.nLineSessionID1 = sessionID Then
                    msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""callagent"":" & """Canceling""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                    sendTowsClinet(msgFromWSClinet)
                    ovsControl.Call_Hangup(0, CallInformation.nLineSessionID2)
                    msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""callagent"":""Disconnected"",""loginsessionid"":""" & AgentInformation.LoginSessionID & """,""talktime"":""" & CallInformation.TotalTalkTime & """ }" & "}"
                    sendTowsClinet(msgFromWSClinet)
                    updateStatusText("Agent Disconnected")
                    CallInformation.Transfering = False
                End If
            End If

            'Timer_1.Enabled = False
            If AgentInformation.DND = True Then
                Write_Log("", "Process_CallDisconnected:Ignore(CallDirection:" & CallInformation.CallDirection & ") nLine:" & CallInformation.nLine & " - " & "" & ", DND:" & AgentInformation.DND)
                Exit Sub
            End If



            Select Case CallInformation.CallStatus

                Case "Ringing", "Cancel"
                    Write_Log("", "CallDisconnected:While Ringing -- CallDirection=" & CallInformation.CallDirection & ",CallStatus" & CallInformation.CallStatus)

                    If CallInformation.CallDirection = "Inbound" Then
                        SetupScreen("DND", "Abandon")
                        CallInformation.CallStatus = "Abandon"
                        updateStatusText("Abandoned " & CallInformation.CallerID)
                        '' Added on 1st November 2022 By Atif 
                        Call GoDoCheckNoACW()
                        If GetINIStringOldWay("Main", "SendSessionCloseOnAbandon", "0") = "1" Then
                            Dim TimeStarted As DateTime = Now
                            Application.DoEvents()
                            updateStatusText("Sending Session Close" & CallInformation.CallerID)
                            sg_Send_Session_Close()
                            Application.DoEvents()
                            Dim xDiff As Integer = DateDiff(DateInterval.Second, TimeStarted, Now())
                            updateStatusText("Sent Session Close(" & xDiff & ") " & CallInformation.CallerID)
                            Write_Log("", "Session Close Sent in = " & xDiff & ", for=" & CallInformation.CallStatus)
                        End If
                    Else
                        If CallInformation.CallStatus = "Cancel" Then
                            SetupScreen("DND", "Cancel")
                        Else
                            SetupScreen("DND", "Reject")
                        End If
                        updateStatusText(CallInformation.CallerID & "failed to connect")
                        Call GoDoCheckNoACW()
                    End If

                Case "NoAns"
                    Write_Log("", "CallDisconnected:with NoAns")
                    SetupScreen("DND", "NoAns")
                    updateStatusText("you didn't answered " & CallInformation.CallerID)


                Case "Abandon" '' needs to be ignored 

                Case Else

                    updateStatusText("Call Closed Received" & CallInformation.CallerID & "," & SIPString)

                    '' New Change by Adeel on Nov 28 2023
                    '' to handle hangup messages on blind call transfer
                    If SIPString = "e.data2=,Call_refer" Then
                        Write_Log("", "Process_CallDisconnected(End) nLine:" & CallInformation.nLine & " its Call transfered")
                        ovsControl.Call_Hangup(0, CallInformation.nLine)
                        Exit Sub
                    End If

                    Try
                        ovsControl.media_stopGetDigits(0, 0, CallInformation.nLineSessionID1)
                    Catch ex As Exception
                    End Try
                    Application.DoEvents()
                    Try
                        ovsControl.media_stopPlay(0, 0, CallInformation.nLineSessionID1)
                    Catch ex As Exception
                    End Try
                    Application.DoEvents()

                    IVRisWorking = False

                    ' While a while here
                    'Dim _waitStartTime As DateTime = DateTime.Now
                    'Dim xDateDiff As Long = 0
                    'While True
                    'xDateDiff = DateDiff(DateInterval.Second, _waitStartTime, Now)
                    'If xDateDiff >= 2 Then
                    'Exit While
                    'End If
                    'Application.DoEvents()
                    'Threading.Thread.Sleep(100)
                    'End While

                    updateStatusText("Call Closed Processing" & CallInformation.CallerID)
                    Application.DoEvents()
                    'Insert Recording Data here
                    If CallInformation.Recording = True Then
                        Write_Log("", "CallDisconnected:StartStopRecording(I AM RECORDING) " & ", Session: " & CallInformation.CRMSessionID & ", Interaction: " & CallInformation.CRMInteractionID)
                        Call StartStopRecording(True)
                    Else
                        Write_Log("", "CallDisconnected:StartStopRecording(I AM NOT RECORDING)" & ", Session: " & CallInformation.CRMSessionID & ", Interaction: " & CallInformation.CRMInteractionID)
                    End If
                    If CallInformation.CallStatus <> "OnHold" Then
                        Try
                            CallInformation.TotalTalkTime = CallInformation.TotalTalkTime + DateDiff(DateInterval.Second, CallInformation.TalkingStartedAt, Now)
                        Catch ex As Exception
                        End Try
                    End If
                    CallURL("Hangup", Configurations.URLSToCall.URL_Hangup)
                    CallInformation.TotalTalkTime = 0
                    CallInformation.TalkingStartedAt = DateTime.Now

                    'sg_Send_Session_Close()

                    Dim TimeStarted As DateTime = Now
                    Application.DoEvents()
                    updateStatusText("Sending Session Close" & CallInformation.CallerID)
                    sg_Send_Session_Close()
                    Application.DoEvents()
                    Dim xDiff As Integer = DateDiff(DateInterval.Second, TimeStarted, Now())
                    updateStatusText("Sent Session Close(" & xDiff & ") " & CallInformation.CallerID)
                    Write_Log("", "Session Close Sent in = " & xDiff & ", for=" & CallInformation.CallStatus)


                    If CallInformation.CallDirection = "Inbound" Then
                        SetupScreen("DND", "NR-ACW")
                    Else
                        Debug.WriteLine("CallStatus :" & CallInformation.CallStatus)
                        If CallInformation.CallStatus = "Calling" Then
                            SetupScreen("DND", "Failed")
                        Else
                            SetupScreen("DND", "NR-ACW")
                        End If
                    End If


                    Write_Log("", "CallDisconnected:StartStopRecording(Stop) IVR = " & IVRisWorking)
            End Select
            Write_Log("", "Process_CallDisconnected(End) nLine:" & CallInformation.nLine)
        Catch ex As Exception
            Write_Log("", "Process_CallDisconnected(Error) nLine:" & CallInformation.nLine & " " & ex.Message)
            MessageBox.Show(Me, "Process_CallDisconnected()" & ex.Message)

        End Try



    End Sub
    Private Sub RunUploader()
        Dim StartTime As DateTime = Now
        Try
            Write_Log("", "RunUploader(Start) in (0)")

            Dim FileNameToShell As String = AgentInformation.AppPath & "\RecordingUploader.exe"
            If IO.File.Exists(FileNameToShell) Then
                'Shell(FileNameToShell, AppWinStyle.Hide)
                Dim exePath As String = FileNameToShell
                Dim thread As New Thread(Sub()
                                             Shell(exePath, AppWinStyle.Hide, True)
                                         End Sub)
                thread.Start()
            End If
            Dim xDiff = DateDiff(DateInterval.Second, StartTime, Now)
            Write_Log("", "RunUploader(End) in (" & xDiff & ")")
        Catch ex As Exception
            Write_Log("", "RunUploader(Error) nLine:" & CallInformation.nLine & " " & ex.Message)
        End Try
    End Sub

    Private Sub Process_LoggedInSuccess(ByVal sLocalURI As String)
        Try
            TextBox_LoginName.Text = ""
            TextBox_Password.Text = ""

            Write_Log("", "LoggedInSuccess() sLocalURI=" & sLocalURI)
            updateStatusText("Telephony Login success")
            CallInformation.SendBacktoIVR = False
            CallInformation.SendbacktoIVRLanguage = ""
            CallInformation.SendbacktoIVRVDN = ""

            CallInformation.RefID_DB = -1
            CallInformation.RefID_Server = -1
            AgentInformation.LoginSessionID = "-1"
            Call dbClass.Login_Get_SessionID()
            AgentInformation.LoggedIn = True
            If AgentInformation.LoginSessionID <> "-1" Then
                AgentInformation.LoginDateTime = DateTime.Now
                If Configurations.ExtPooling = True Then
                    Call dbClass.Extenstion_Reserve_FromPool(Val(AgentInformation.AgentID), Val(AgentInformation.PoolID))
                End If
            Else
                updateStatusText("Somethings went wrong! No sessionID")
                Write_Log("", "Somethings went wrong! No sessionID")
                Exit Sub
            End If

            Configurations.AutoPickup = False
            If GetINIString("QueueID" & AgentInformation.QueueID, "AutoPickup", "0") = "1" Then
                Configurations.AutoPickup = True
            End If
            SetupScreen("DND", "NR-Login")
        Catch ex As Exception
            Write_Log("", "Process_LoggedInSuccess()" & ex.Message)
            MessageBox.Show(Me, "Process_LoggedInSuccess()" & ex.Message)
        End Try
    End Sub

    Private Sub Button_Logout_Click(sender As Object, e As EventArgs) Handles Button_Logout.Click
        Call Process_Logout()
        SetupScreen("Login", "")
    End Sub
    Private Sub Process_Logout()
        Try
            Write_Log("", "Process_Logout(Start) nLine:" & CallInformation.nLine)

            If Configurations.ExtPooling = True Then
                Try
                    Call dbClass.Extenstion_Release_FromPool(Val(AgentInformation.AgentID), Val(AgentInformation.PoolID))
                Catch ex As Exception
                    Dim tmpText As String = "Process_Logout(Extenstion_Release_FromPool) nLine:" & AgentInformation.AgentID & " " & "Exception:" & " " & ex.Message
                    Write_Log("", tmpText)
                End Try

            End If
            If Val(AgentInformation.ActivitySessionID) > 0 Then
                Try
                    dbClass.Activity_Set_SessionID("Logout")
                Catch ex As Exception
                    Dim tmpText As String = "Process_Logout(Activity_Set_SessionID) nLine:" & AgentInformation.AgentID & " " & "Exception:" & " " & ex.Message
                    Write_Log("", tmpText)
                End Try
            End If
            sendTowsClinet("{" & """cmd"":{""ts"":""" & tsString & """,""logout"":""logout"",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}")
            If Val(AgentInformation.LoginSessionID) > 0 Then
                Try
                    Call dbClass.Login_Set_SessionID()
                Catch ex As Exception
                    Dim tmpText As String = "Process_Logout(Login_Set_SessionID) nLine:" & AgentInformation.AgentID & " " & "Exception:" & " " & ex.Message
                    Write_Log("", tmpText)
                End Try
            End If
            DoTelephonyUnRegister()
            SetupScreen("Startup", "")
            updateStatusText("Logout Done")

            If GetINIString("Main", "wsClientAllowed", "0") = "1" Then
                wsClientAllowed = True
                'Call SetupwsClient()
            End If

            Write_Log("", "Process_Logout(End) nLine:" & CallInformation.nLine)
            'SetupScreen("Login", "")
            Me.Text = meDefaultText
        Catch ex As Exception
            Write_Log("", "Process_Logout(Error) nLine:" & CallInformation.nLine & "" & ex.Message)
            MessageBox.Show(Me, "Process_Logout(Error) nLine:" & CallInformation.nLine & "" & ex.Message)
        End Try
    End Sub
    Private Sub Button_Outbound_Click(sender As Object, e As EventArgs) Handles Button_Outbound.Click
        SetupScreen("Ready", "Outbound")
    End Sub
    Private Sub Button_NR_Break_Click(sender As Object, e As EventArgs) Handles Button_NR_Break.Click
        SetupScreen("DND", "Break")
    End Sub
    Private Sub Button_NR_Lunch_Click(sender As Object, e As EventArgs) Handles Button_NR_Lunch.Click
        SetupScreen("DND", "Lunch")
    End Sub
    Private Sub Button_NR_Prayer_Click(sender As Object, e As EventArgs) Handles Button_NR_Prayer.Click
        SetupScreen("DND", "Prayer")
    End Sub
    Private Sub Button_NR_Meeting_Click(sender As Object, e As EventArgs) Handles Button_NR_Meeting.Click
        SetupScreen("DND", "Meeting")
    End Sub

    Private Sub Button_Inbound_Click(sender As Object, e As EventArgs) Handles Button_Inbound.Click

        SetupScreen("Ready", "Inbound")

    End Sub


    Private Sub Label_Select_DND_Click(sender As Object, e As EventArgs) Handles Label_Select_DND.Click
        'If Label_DND.Enabled = True Then

        GroupBox_Mode.Visible = True
        GroupBox_Mode.Enabled = True
        GroupBox_Mode.BringToFront()

        'Frame_NotReady.Visible = True
        'Frame_NotReady.Enabled = True
        'Frame_NotReady.ZOrder 0
        'Label_NRCaption.Caption = "Ready"
        'End If

    End Sub

    Private Sub Form_Splash_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Try
            Write_Log("", "Closing()")
            If AgentInformation.LoggedIn = True Then
                Write_Log("", "Closing() User Selected Yes")
                If MessageBox.Show(Me, "Are you sure wait to logout?", Me.Text, MessageBoxButtons.YesNo) = vbYes Then
                    Process_Logout()
                Else
                    Write_Log("", "Closing() User Selected No")
                    e.Cancel = True
                    Exit Sub
                End If
            End If
            Try
                Application.DoEvents()
                NotifyIcon1.BalloonTipText = ""
                NotifyIcon1.Visible = False
                Application.DoEvents()
            Catch ex As Exception
            End Try
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Form_Splash_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        Try
            RunUploader()
            If IsNothing(wSSHost) = False Then
                wSSHost.Stop()
            End If
        Catch ex As Exception
        Finally
            Write_Log("", "Closed()")
        End Try
    End Sub

    Private Sub Button_Call_Connect_Click(sender As Object, e As EventArgs) Handles Button_Call_Connect.Click
        Button_Call_Connect.Enabled = False
        Button_Call_Hangup.Enabled = False
        Try
            If CallInformation.CallDirection = "Inbound" Then
                Write_Log("", "Connect_Click() Here to accpet a call")
                Dim msgFromWSClinet As String = ""
                'ovsControl.Call_Answer(0, CallInformation.nLine)
                If ovsControl.Call_Answer(0, CallInformation.nLine) = True Then
                    msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""CallAccept"":" & """OKAY""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                    sendTowsClinet(msgFromWSClinet)
                Else
                    'msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""CallAccept"":" & """Sorry""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                    'Call GoDoCheckNoACW()
                    Process_CallDisconnected(CallInformation.CallerID, CallInformation.CalledID, CallInformation.nLineSessionID1, "")
                End If

            Else
                If Trim(TextBox_Number.Text) <> "" Then
                    Write_Log("", "Connect_Click() Here to Make a call(" & TextBox_Number.Text & ")")
                    CallInformation.CalledID = TextBox_Number.Text
                    CallInformation.CallerID = AgentInformation.PABXExt
                    CallInformation.CallDirection = "Outbound"
                    CallInformation.CallStatus = "Calling"
                    CallInformation.CallerVerifyed = False
                    ovsControl.Call_Make(0, TextBox_Number.Text)
                End If
            End If
        Catch ex As Exception
            Write_Log("", "Button_Call:Error:" & ex.Message)
            MessageBox.Show(Me, "Button_Call:Error:" & ex.Message)
        End Try
    End Sub

    Private Sub ovsControl_Event_CallGeneric2(Sender As Object, e As TControl.Event_CallGeneric2_Args) Handles ovsControl.Event_CallGeneric2
        Write_Log("", "CallGeneric2(Start) nLine:" & e.eventName & ", sessionId=" & e.sessionId & ", callee=" & e.callee & ", caller=" & e.caller & ", sipMessage=" & e.sipMessage.ToString())

        Try
            Select Case e.eventName
                Case "onInviteIncoming"
                    Process_CallRinging(e.callbackIndex, e.callbackObject, e.caller, e.callee, "" & e.sessionId, e.sipMessage.ToString())
                Case ""
            End Select
            Write_Log("", "CallGeneric2(End) nLine:" & e.eventName & ", sessionId=" & e.sessionId)

        Catch ex As Exception
            Write_Log("", "CallGeneric2(Error) nLine:" & e.eventName & ", sessionId=" & e.sessionId & " " & ex.Message)
        End Try
    End Sub

    Private Sub Process_CallConnected(Caller As String, Callee As String, sessionID As String, SIPString As String)
        Try
            Write_Log("", "Process_CallConnected(Starting) nLine:" & CallInformation.nLine & " Transfering:" & CallInformation.Transfering)
            Dim msgFromWSClinet As String = ""

            If CallInformation.Transfering Then
                CallInformation.nLineSessionID2 = sessionID
                updateStatusText("Agent Connected")
                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""callagent"":""Connected"",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                sendTowsClinet(msgFromWSClinet)
                Exit Sub
            End If

            ' from sample "1997" <sip:1997@192.168.198.222>;tag=as6dec8652
            ProgressBar_Answer.Value = 0
            ProgressBar_Answer.Visible = False

            GroupBox_Mode.Visible = False
            GroupBox_Mode.Enabled = False
            Label_Select_DND.Enabled = False

            Button_Call_Connect.Enabled = False
            Button_Call_Hangup.Text = "&Hangup"
            Button_Call_Hangup.Enabled = True
            TextBox_Number.ReadOnly = True

            'If CallInformation.CallDirection <> "Inbound" Then
            'If CallInformation.RefID_DB = -1 Then
            'Process_CallRinging from, localURI, line
            'End If
            'End If

            CallInformation.CallingOut = False
            CallInformation.CallStatus = "Talking"
            CallInformation.RecordingFileChunkNo = 0
            Label_AgentStatus.Text = "State:" & CallInformation.CallStatus
            updateStatusText("Talking to " & CallInformation.CallerID)
            Try
                dbClass.UpdateRealTimeStatus(CallInformation.CallStatus, CallInformation.CalledID)
            Catch ex As Exception
                Dim tmpText As String = "CallConnected(UpdateRealTimeStatus) nLine:" & sessionID & " " & "Exception:" & " " & ex.Message
                Write_Log("", tmpText)
            End Try
            'If Command_Hold.Tag = "OnHoldNow" Then ' Call Received Back From Hold
            'Call SetCallInformaiton("UnHold", "")
            'Else ' Call is Connected Now

            Try
                Call dbClass.SetCallInformation("Answer", "")
            Catch ex As Exception
                Dim tmpText As String = "CallConnected(SetCallInformation) nLine:" & sessionID & " " & "Exception:" & " " & ex.Message
                Write_Log("", tmpText)
            End Try
            'CallURL "Connected", Configurations.URLSToCall.URL_Connected
            'End If
            Button_Call_Hold.Enabled = True
            Button_Call_Mute.Enabled = True
            Button_Call_Trans.Enabled = True
            ovsControl.Microphone(False)
            If CallInformation.nLine = -1 Then
                CallInformation.nLine = sessionID
            End If

            'Start Call Recording here
            CallInformation.TalkingStartedAt = DateTime.Now
            CallInformation.TotalTalkTime = 0
            _tmpsessionID = sessionID
            'Call StartStopRecording(False, sessionID)
            CallURL("Connected", Configurations.URLSToCall.URL_Connected)
            Write_Log("", "Process_CallConnected(End) nLine:" & CallInformation.nLine)
        Catch ex As Exception
            Write_Log("", "Process_CallConnected(Error) nLine:" & CallInformation.nLine & " " & ex.Message)
            MessageBox.Show(Me, "Process_CallConnected(Error) nLine:" & CallInformation.nLine & " " & ex.Message)

        End Try

    End Sub
    Private Function AsyncUploadRecording()
        Try
            Write_Log("", "Process_AsyncUploadRecording(Start) nLine:" & CallInformation.nLine)
            StartStopRecording(True)
            Application.DoEvents()
            If CallInformation.CallStatus = "Talking" Then
                StartStopRecording(False)
            End If
            Write_Log("", "Process_AsyncUploadRecording(End) nLine:" & CallInformation.nLine)
        Catch ex As Exception
            Write_Log("", "Process_AsyncUploadRecording(Error) nLine:" & CallInformation.nLine)
        End Try
        Return ""
    End Function
    Private Function StartStopRecording(Optional bStop As Boolean = False, Optional sessionID As String = "-1") As Boolean
        StartStopRecording = False
        Try
            If Configurations.RecordCalls = False Then Exit Function
            Write_Log("", "StartStopRecording(Start) nLine:" & CallInformation.nLine & " " & bStop)
            If bStop Then
                ovsControl.Call_StopRecord(0, CallInformation.nLine)
                CallInformation.Recording = False
                Application.DoEvents()
                updateStatusText("Uploading.. Wait")
                Dim x As String = dbClass.CallRecording_Put()
                updateStatusText("Uploaded(" & x & ")")
                StartStopRecording = True
            Else
                CallInformation.RecordingFileName = CallInformation.RefID_DB & "-" & Now.ToString("yyyyMMddHHmmssffff") & "-" & CallInformation.RecordingFileChunkNo
                Console.WriteLine("RecordingFileName :" & CallInformation.RecordingFileName)
                CallInformation.RecordingFileChunkNo = CallInformation.RecordingFileChunkNo + 1
                ovsControl.Call_StartRecord(0, CallInformation.nLineSessionID1, CallInformation.RecordingFileName, LogDir & "\", PortSIP.AUDIOSTREAM_CALLBACK_MODE.AUDIOSTREAM_NONE)
                CallInformation.RecordingFileName = LogDir & "\" & CallInformation.RecordingFileName & "." & Configurations.RecordingFormat
                CallInformation.RecordingStartedAT = DateTime.Now
                If Configurations.RecordinChunkTimeOut > 0 Then
                    uCallback.SetupEvent(-1, "RecordingUpload", Configurations.RecordinChunkTimeOut, -1, -1, -1, Nothing)
                End If
                StartStopRecording = True
                CallInformation.Recording = True
            End If
            Write_Log("", "StartStopRecording(End) nLine:" & CallInformation.nLine & " " & bStop & ", FN:" & CallInformation.RecordingFileName & ",Recording:" & CallInformation.Recording)

        Catch ex As Exception
            Write_Log("", "StartStopRecording(Error) nLine:" & CallInformation.nLine & " " & bStop & " " & ex.Message)
            updateStatusText("StartStopRecording Failed (" & ex.Message & ")")
            'MessageBox.Show(Me, "StartStopRecording(Error) nLine:" & CallInformation.nLine & " " & bStop & " " & ex.Message)
        End Try
    End Function


    Private Sub Process_CallRinging(ChannelNo As Long, LineNo As Long, Caller As String, Callee As String, sessionID As String, SIPString As String)
        Dim Process_CallRinging_Time_Start As DateTime = Now
        Try

            Write_Log("", "******************************************************************************************************************************************")
            Write_Log("", "Process_CallRinging(Start) nLine:" & CallInformation.nLine & ", Transfering = " & CallInformation.Transfering & ", IVR = " & IVRisWorking)
            If IVRisWorking = True Then
                Write_Log("", "Process_CallRinging(Start) nLine:" & CallInformation.nLine & "Here is the case, Please check the logs for last call on this station")
            End If


            If CallInformation.Transfering Then
                If CallInformation.nLineSessionStatus = "Try" Then
                    CallInformation.nLineSessionID2 = sessionID
                    updateStatusText("Agent Try")
                    CallInformation.nLineSessionStatus = "Ringing"
                Else
                    updateStatusText("Agent Ringing")
                    CallInformation.nLineSessionStatus = "Ringing"
                End If
                Exit Sub
            End If

            updateStatusText("CallRinging Started")

            GroupBox_Mode.Enabled = False
            GroupBox_Mode.Visible = False
            GroupBox_Call.Enabled = True
            GroupBox_Call.Visible = True
            GroupBox_Call.Location = New System.Drawing.Point(0, 35)

            Dim tmpfrom As String
            If CallInformation.CallDirection = "Inbound" Then
                tmpfrom = Mid(Caller, 1, InStr(Caller, "@") - 1)
                tmpfrom = Mid(tmpfrom, InStr(tmpfrom, ":") + 1)
                CallInformation.CallerID = ""
                If Trim(tmpfrom) <> "" Then
                    CallInformation.CallerID = tmpfrom
                End If

                tmpfrom = ""
                tmpfrom = Mid(Callee, 1, InStr(Callee, "@") - 1)
                tmpfrom = Mid(tmpfrom, InStr(tmpfrom, ":") + 1)
                CallInformation.CalledID = ""
                If Trim(tmpfrom) <> "" Then
                    CallInformation.CalledID = tmpfrom
                End If
            End If

            CallInformation.CallStatus = "Ringing"
            CallInformation.nLine = sessionID
            CallInformation.nLineSessionID1 = sessionID
            CallInformation.nLineSessionID2 = -1
            Dim time_Start As DateTime = Now
            Try
                updateStatusText("Update RT Start (0)")
                dbClass.UpdateRealTimeStatus(CallInformation.CallStatus, CallInformation.CallerID)
                Dim DateDiffx As Long = DateDiff(DateInterval.Second, time_Start, Now)
                updateStatusText("Update RT End (" & DateDiffx & "sec(s))")
                Write_Log("", "Process_CallRinging:From dbClass UpdateRealTimeStatus took:" & DateDiffx & "(s)")
            Catch ex As Exception
                Dim tmpText As String = "CallRinging(UpdateRealTimeStatus) nLine:" & sessionID & " " & "Exception:" & " " & ex.Message
                Write_Log("", tmpText)
            End Try


            Label_AgentStatus.Text = "State:" & CallInformation.CallStatus
            Label_Select_DND.Enabled = False

            Write_Log("", "Process_CallRinging:CallInformation.RefID_Server:" & CallInformation.RefID_Server)

            Dim tmpCallerID As String = ""
            If CallInformation.RefID_Server = -1 Then
                Try
                    time_Start = Now
                    updateStatusText("Server_Get_SessionID Start (0)")
                    Call dbClass.Server_Get_SessionID(tmpCallerID)
                    Dim DateDiffx As Long = DateDiff(DateInterval.Second, time_Start, Now)
                    updateStatusText("Server_Get_SessionID End (" & DateDiffx & "(secs))")
                    Write_Log("", "Process_CallRinging:From dbClass Server_Get_SessionID took:" & DateDiffx & "(secs)")
                Catch ex As Exception
                    Dim tmpText As String = "CallRinging(Server_Get_SessionID) nLine:" & sessionID & " " & "Exception:" & " " & ex.Message
                    Write_Log("", tmpText)
                End Try
                If Trim(tmpCallerID) <> "" Then
                    CallInformation.CallerID = tmpCallerID
                End If
            End If

            Write_Log("", "Process_CallRinging(0):GetNewCallSessionID:" & CallInformation.RefID_DB)
            If CallInformation.RefID_DB = -1 Then
                Write_Log("", "Process_CallRinging(1):GetNewCallSessionID:" & CallInformation.RefID_DB)
                Try
                    time_Start = Now
                    updateStatusText("SessionID_Get Start (0)")
                    Call dbClass.SessionID_Get()
                    Dim DateDiffx As Long = DateDiff(DateInterval.Second, time_Start, Now)
                    updateStatusText("Server_Get_SessionID End (" & DateDiffx & "(secs))")
                    Write_Log("", "Process_CallRinging:From dbClass SessionID_Get took:" & DateDiffx & "(secs)")
                Catch ex As Exception
                    Dim tmpText As String = "CallRinging(SessionID_Get) nLine:" & sessionID & " " & "Exception:" & " " & ex.Message
                    Write_Log("", tmpText)
                End Try

            End If
            Write_Log("", "Process_CallRinging(2):GetNewCallSessionID:" & CallInformation.RefID_DB)

            If CallInformation.RefID_DB > -1 Then
                Try
                    time_Start = Now
                    updateStatusText("SessionID_Get Start (0)")
                    dbClass.Activity_Set_SessionID("Ringing")
                    Dim DateDiffx As Long = DateDiff(DateInterval.Second, time_Start, Now)
                    updateStatusText("SessionID_Get End (" & DateDiffx & "(secs))")
                    Write_Log("", "Process_CallRinging:From dbClass Activity_Set_SessionID took:" & DateDiffx & "(secs)")

                Catch ex As Exception
                    Dim tmpText As String = "CallRinging(Activity_Set_SessionID) nLine:" & sessionID & " " & "Exception:" & " " & ex.Message
                    Write_Log("", tmpText)
                End Try

            End If
            Try
                time_Start = Now
                updateStatusText("Activity_Get_SessionID Start (0)")

                Call dbClass.Activity_Get_SessionID("Ringing", True)
                Dim DateDiffx As Long = DateDiff(DateInterval.Second, time_Start, Now)
                updateStatusText("Activity_Get_SessionID End (" & DateDiffx & "(secs))")
                Write_Log("", "Process_CallRinging:From dbClass Activity_Get_SessionID took:" & DateDiffx & "(secs)")

            Catch ex As Exception
                Dim tmpText As String = "CallRinging(Activity_Get_SessionID) nLine:" & sessionID & " " & "Exception:" & " " & ex.Message
                Write_Log("", tmpText)
            End Try

            CallInformation.Transfering = False

            If CallInformation.CallDirection = "Inbound" Then
                updateStatusText("Incomming call from " & CallInformation.CallerID)
                TextBox_Number.Text = CallInformation.CallerID
                If CallInformation.CallerID <> "" Then
                    Try
                        Clipboard.SetText(CallInformation.CallerID, TextDataFormat.Text)
                    Catch ex As Exception
                    End Try
                End If
                Button_Call_Connect.Text = "&Accept"
                Button_Call_Connect.Enabled = True
                Button_Call_Hangup.Enabled = True
                Try
                    time_Start = Now
                    Write_Log("APP", "Process_CallRinging(Ringing) Going to Play BEEP")
                    RingPlayer.Play()
                    Dim DateDiffx As Long = DateDiff(DateInterval.Second, time_Start, Now)
                    Write_Log("APP", "Process_CallRinging(Ringing) Playing took(" & DateDiffx & ")")
                Catch ex As Exception
                    Dim tmpText As String = "CallRinging(Error) Call_Ringing:" & " " & ex.Message
                    Write_Log("", tmpText)
                End Try
                Try
                    Button_Call_Connect.Focus()
                Catch ex As Exception
                    Dim tmpText As String = "CallRinging(Error) Call_Connect:" & " " & ex.Message
                    Write_Log("", tmpText)
                End Try

                If NotifyIcon1.Visible = False Then
                    Try
                        Me.WindowState = FormWindowState.Normal
                    Catch ex As Exception
                        Dim tmpText As String = "CallRinging(Error) WindowState:" & " " & ex.Message
                        Write_Log("", tmpText)
                    End Try
                    Application.DoEvents()
                    Try
                        Me.TopMost = True
                    Catch ex As Exception
                        Dim tmpText As String = "CallRinging(Error) TopMost:" & " " & ex.Message
                        Write_Log("", tmpText)
                    End Try
                    Application.DoEvents()
                    Try
                        Button_Call_Connect.Focus()
                    Catch ex As Exception
                        Dim tmpText As String = "CallRinging(Error) Focus:" & " " & ex.Message
                        Write_Log("", tmpText)
                    End Try

                End If

                Application.DoEvents()
                Me.TopMost = False
                'Call DoCallerIDCopy
                'Call PlayRingtone
                ProgressBar_Answer.Value = 0
                ProgressBar_Answer.Visible = True
                ProgressBar_Answer.Maximum = Configurations.NoAns
                uCallback.SetupEvent(1, "NoAnsCheck", 1000, -1, -1, -1, Nothing)
                Write_Log("", "Process_CallRinging(TimeCalculation) nLine:" & CallInformation.nLine & ", Configurations.NoAns:" & Configurations.NoAns & " Diff: " & DateAdd("s", Configurations.NoAns, Now()))

            Else
                updateStatusText("Calling to " & CallInformation.CallerID)
                If CallInformation.CallerID <> "" Then
                    Try
                        Clipboard.SetText(CallInformation.CallerID, TextDataFormat.Text)
                    Catch ex As Exception

                    End Try
                End If
                Button_Call_Connect.Text = "&Accept"
                Button_Call_Connect.Enabled = False
                Button_Call_Hangup.Enabled = True

                Try
                    Button_Call_Hangup.Focus()
                Catch ex As Exception
                    Dim tmpText As String = "CallRinging(Error) Call_Hangup:" & " " & ex.Message
                    Write_Log("", tmpText)
                End Try

                ProgressBar_Answer.Value = 0
                ProgressBar_Answer.Visible = False
                ProgressBar_Answer.Maximum = Configurations.NoAns
            End If
            CallURL("Ringing", Configurations.URLSToCall.URL_Ringing)
            Write_Log("", "Process_CallRinging(Start) nLine:" & CallInformation.nLine & ", Transfering = " & CallInformation.Transfering & ", IVR = " & IVRisWorking)
        Catch ex As Exception
            Write_Log("", "Process_CallRinging(Error) nLine:" & CallInformation.nLine & " " & ex.Message)
        End Try
        Try
            Dim DateDiffx As Long = DateDiff(DateInterval.Second, Process_CallRinging_Time_Start, Now)
            Write_Log("", "Processing time was(" & DateDiffx & ") ***************************************************************************************************")
        Catch ex As Exception
        End Try

    End Sub

    Private Sub ovsControl_Event_CallGeneric(Sender As Object, e As TControl.Event_CallGeneric_Args) Handles ovsControl.Event_CallGeneric



        If Configurations.bLogSIPDetails Or e.eventName = "onInviteRecord" Or e.eventName = "media_stopPlay" Or e.eventName = "media_play" Or e.eventName = "onReceivedRefer" Or e.eventName = "Error:onReceivedRefer" Then
            Write_Log("", "CallGeneric(Start) nLine:" & e.eventName & ",  sessionId=" & e.sessionId & ", data1=" & e.data1 & ", data2=" & e.data2)
        Else
            Write_Log("", "CallGeneric(Start) nLine:" & e.eventName & ",  sessionId=" & e.sessionId & ", data1=" & "" & ", data2=" & "")
        End If
        Try
            Select Case e.eventName
                Case "onInviteTrying", "onInviteRinging"
                    If CallInformation.CallStatus <> "Ringing" Then
                        If CallInformation.nLineSessionID1 = -1 Then
                            CallInformation.CallStatus = "Ringing"
                        End If
                        Process_CallRinging(e.callbackIndex, e.callbackObject, TextBox_Number.Text, AgentInformation.PABXExt, "" & e.sessionId, "")
                    End If
                Case "onInviteConnected"
                    Process_CallConnected("", "", "" & e.sessionId, "")
                    'Call Button2_Click(Sender, e)
                    uCallback.SetupEvent(-1, "StartRecordingWithDeley", Configurations.RecordingStartDelay, -1, -1, -1, Nothing)

                Case "onInviteClosed", "onInviteFailure"
                    Process_CallDisconnected("", "", "" & e.sessionId, "e.data2=" & e.data2 & "," & e.data1)


                Case "onRecvDtmfTone"
                    sInputs = sInputs & e.data1

                Case "onGetDigitsFinished"
                    If (Trim(e.data2)) <> "" Then
                        sInputs = sInputs & e.data2
                    End If

                Case "onPlayAudioFileFinished"
                    'If (Trim(e.data2)) <> "" Then
                    'sInputs = sInputs & e.data2
                    'End If
                Case Else

            End Select
            Write_Log("", "CallGeneric(End) nLine:" & e.eventName & ", sessionId=" & e.sessionId)

        Catch ex As Exception
            Write_Log("", "CallGeneric(Error) nLine:" & e.eventName & ", sessionId=" & e.sessionId & "" & ex.Message)

        End Try
    End Sub
    Public Sub ApplyAudioChanges(MicID As Integer, SpeakerID As Integer)
        Try
            Write_Log("", "ApplyAudioChanges(Start) MicID:" & MicID & ", SpeakerID=" & SpeakerID)
            ovsControl.setAudioDeviceId(MicID, SpeakerID)
            Form_AudioSetup.Label_msg.Text = "Changes applied"
            Write_Log("", "ApplyAudioChanges(End) nLine:" & CallInformation.nLine)
        Catch ex As Exception
            Write_Log("", "ApplyAudioChanges(Error) nLine:" & CallInformation.nLine & " " & ex.Message)
            'MessageBox.Show(Me, "ApplyAudioChanges(Error) nLine:" & CallInformation.nLine & " " & ex.Message)
        End Try
    End Sub

    Private Sub Button_Call_Hangup_Click(sender As Object, e As EventArgs) Handles Button_Call_Hangup.Click
        Try
            Write_Log("", "Call_Hangup_Click(Start) nLine:" & CallInformation.nLine & ", SendBacktoIVR=" & CallInformation.SendBacktoIVR)

            Button_Call_Connect.Enabled = False
            Button_Call_Hangup.Enabled = False
            Button_Call_Hold.Enabled = False
            Button_Call_Mute.Enabled = False

            If CallInformation.CallDirection = "Inbound" Then
                If CallInformation.CallStatus = "Ringing" Then
                    Write_Log("", "Call_Hangup_Click(In) Here to Reject a call")
                    ovsControl.Call_Reject(0, CallInformation.nLine)
                Else
                    Write_Log("", "Call_Hangup_Click(In) Here to Hangup a call")
                    If CallInformation.SendBacktoIVR Then
                        Write_Log("", "Call_Hangup_Click(In) Date Writing")
                        Call dbClass.InsertTransferToFedback(CallInformation.SendbacktoIVRLanguage)
                        Write_Log("", "Call_Hangup_Click(In) Date Writen, sending refer")
                        ovsControl.Call_refer(0, CallInformation.nLine, CallInformation.SendbacktoIVRVDN)
                        Write_Log("", "Call_Hangup_Click(In) Date Writen, refer sent to 0," & CallInformation.nLine & "," & CallInformation.SendbacktoIVRVDN)
                    Else
                        Write_Log("", "Call_Hangup_Click(In) sending Hangup")
                        ovsControl.Call_Hangup(0, CallInformation.nLine)
                        Write_Log("", "Call_Hangup_Click(In) Hangup Sent")
                    End If
                End If
            Else
                Write_Log("", "Call_Hangup_Click(Out) Here to Hangup a call")
                If CallInformation.CallStatus = "Ringing" Then
                    CallInformation.CallStatus = "Cancel"
                End If
                Write_Log("", "Call_Hangup_Click(Out) Sending Hangup")
                ovsControl.Call_Hangup(0, CallInformation.nLine)
                Write_Log("", "Call_Hangup_Click(Out) Hangup Hangup")
            End If
            Write_Log("", "Call_Hangup_Click(End) nLine:" & CallInformation.nLine)

        Catch ex As Exception
            Write_Log("", "Call_Hangup_Click(Error) nLine:" & CallInformation.nLine & " " & ex.Message)
            'MessageBox.Show(Me, "Call_Hangup_Click(Error) nLine:" & CallInformation.nLine & " " & ex.Message)
        End Try
    End Sub

    Private Sub Button_Call_Mute_Click(sender As Object, e As EventArgs) Handles Button_Call_Mute.Click
        Button_Call_Mute.Enabled = False
        Try
            Write_Log("", "Call_Mute_Click(Start) nLine:" & CallInformation.nLine & " " & Button_Call_Mute.Tag & "" & CallInformation.CallStatus)
            If CallInformation.CallStatus = "OnHold" Then
                updateStatusText("Can't mute, caller on hold")
                Write_Log("", "Mute Log:" & Button_Call_Mute.Tag & " Request recjected, Call is on hold already")
                Button_Call_Mute.Enabled = True
                Exit Sub
            Else
                If Button_Call_Mute.Tag = "" Then
                    ovsControl.Microphone(True)
                    'Button_Call_Mute.Image = My.Resources.icon_mute
                    Button_Call_Mute.Tag = "Mute"
                    Button_Call_Mute.Text = "Un&mute"
                    updateStatusText("Muted")
                    Label_AgentStatus.Text = "State:" & "Mute"
                Else
                    ovsControl.Microphone(False)
                    'Button_Call_Mute.Image = My.Resources.icon_unmute
                    Button_Call_Mute.Text = "&Mute"
                    Button_Call_Mute.Tag = ""
                    updateStatusText("Talking now")
                    CallInformation.CallStatus = "Talking"
                    Label_AgentStatus.Text = "State:" & "Talking"
                End If
            End If
            Write_Log("", "Call_Mute_Click(End) nLine:" & CallInformation.nLine & " " & Button_Call_Mute.Tag & "" & CallInformation.CallStatus)
        Catch ex As Exception
            Write_Log("", "Call_Mute_Click(Error) nLine:" & CallInformation.nLine & " " & ex.Message)
            'MessageBox.Show(Me, "Call_Mute_Click(Error) nLine:" & CallInformation.nLine & " " & ex.Message)
        End Try
        Button_Call_Mute.Enabled = True
    End Sub

    Private Sub Button_Call_Hold_Click(sender As Object, e As EventArgs) Handles Button_Call_Hold.Click
        Try
            Write_Log("", "Call_Hold_Click(Start) nLine:" & CallInformation.nLine & " " & Button_Call_Mute.Tag & "" & CallInformation.CallStatus)
            Button_Call_Hold.Enabled = False
            If Button_Call_Mute.Tag = "Mute" Then
                Write_Log("", "Hold Log:" & Button_Call_Hold.Tag & " Caller is Muted, UnMuting first")
                Call Button_Call_Mute_Click(sender, e)
            End If

            If Button_Call_Hold.Tag = "" Then
                'Insert Recording Data here
                Write_Log("", "Command_Hold:StartStopRecording(Stop)")
                'Call StartStopRecording(True)
                Application.DoEvents()
                Write_Log("", "Hold Log:" & Button_Call_Hold.Tag & " Goind on Hold")
                ovsControl.Call_Hold(0, CallInformation.nLine)

                CallInformation.TotalTalkTime = CallInformation.TotalTalkTime + DateDiff(DateInterval.Second, CallInformation.TalkingStartedAt, DateTime.Now)
                CallInformation.TalkingStartedAt = DateTime.Now

                Button_Call_Hangup.Enabled = False
                Button_Call_Mute.Enabled = False
                'Button_Call_Hold.Image = My.Resources.icon_unhold
                Button_Call_Hold.Tag = "OnHold"
                updateStatusText("Going on hold")
                Call StartStopRecording(True)
                CallInformation.CallStatus = "OnHold"

                Try
                    dbClass.UpdateRealTimeStatus(CallInformation.CallStatus, CallInformation.CalledID)
                Catch ex As Exception
                    Dim tmpText As String = "Call_Hold(UpdateRealTimeStatus) nLine:" & CallInformation.CalledID & " " & "Exception:" & " " & ex.Message
                    Write_Log("", tmpText)
                End Try

                Label_AgentStatus.Text = "State:" & CallInformation.CallStatus
                If Val(AgentInformation.ActivitySessionID) > 0 Then
                    Try
                        dbClass.Activity_Set_SessionID("Hold")
                    Catch ex As Exception
                        Dim tmpText As String = "Command_Hold(Activity_Set_SessionID) nLine:" & CallInformation.CalledID & " " & "Exception:" & " " & ex.Message
                        Write_Log("", tmpText)
                    End Try

                End If
                Try
                    Call dbClass.Activity_Get_SessionID("Hold", True)
                Catch ex As Exception
                    Dim tmpText As String = "Command_Hold(Activity_Get_SessionID) nLine:" & CallInformation.CalledID & " " & "Exception:" & " " & ex.Message
                    Write_Log("", tmpText)
                End Try
                Label_AgentStatus.Text = "State:" & "OnHold"
            Else
                Write_Log("", "Hold Log:" & Button_Call_Hold.Tag & " Goind on offhold")
                'Button_Call_Hold.Image = My.Resources.icon_hold
                ovsControl.Call_Unhold(0, CallInformation.nLine)
                CallInformation.TalkingStartedAt = DateTime.Now
                Call StartStopRecording(False)
                If Configurations.RecordinChunkTimeOut > 1000 Then
                    uCallback.SetupEvent(-1, "RecordingUpload", Configurations.RecordinChunkTimeOut, -1, -1, -1, Nothing)
                End If

                Button_Call_Hold.Tag = ""
                Button_Call_Hangup.Enabled = True
                Button_Call_Mute.Enabled = True
                Try
                    Call dbClass.Activity_Get_SessionID("UnHold", True)
                Catch ex As Exception
                    Dim tmpText As String = "Call_Hold(Activity_Get_SessionID) nLine:" & CallInformation.CalledID & " " & "Exception:" & " " & ex.Message
                    Write_Log("", tmpText)
                End Try

                CallInformation.CallStatus = "Talking"
                updateStatusText("Talking now")
                Label_AgentStatus.Text = "State:" & "Talking"
            End If
            Write_Log("", "Call_Hold_Click(End) nLine:" & CallInformation.nLine & " " & Button_Call_Mute.Tag & "" & CallInformation.CallStatus)
            Button_Call_Hold.Enabled = True
        Catch ex As Exception
            Button_Call_Hold.Enabled = True
            Write_Log("", "Call_Hold_Click(Error) nLine:" & CallInformation.nLine & " " & Button_Call_Mute.Tag & "" & CallInformation.CallStatus & " " & ex.Message)
            'MessageBox.Show(Me, "Call_Hold_Click(Error) nLine:" & CallInformation.nLine & " " & Button_Call_Mute.Tag & "" & CallInformation.CallStatus & " " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox_Number_TextChanged(sender As Object, e As EventArgs) Handles TextBox_Number.TextChanged
    End Sub


    Private Sub TextBox_Number_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox_Number.KeyPress
        Try
            Debug.WriteLine("KeyChar:" & e.KeyChar)
            Select Case e.KeyChar
                Case Microsoft.VisualBasic.ChrW(Keys.Return)
                    If Trim(TextBox_Number.Text) <> "" Then
                        Button_Call_Connect_Click(sender, e)
                    End If
                Case Microsoft.VisualBasic.ChrW(Keys.Back)
                Case "*", "#", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0"
                Case Else
                    e.Handled = True
            End Select
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Label_AppName_Click(sender As Object, e As EventArgs) Handles Label_AppName.Click
        Try
            If AgentInformation.Supervisor = True Then
                Label_AppName.Enabled = False
                updateStatusText("Wait, Opening Player")
                Dim x As New Form_MediaPlayer
                x.Show()
                updateStatusText("Please proceed...")
                Label_AppName.Enabled = True
            End If
        Catch ex As Exception
            Label_AppName.Enabled = True
            Write_Log("", "Button_PlayCalls:Error:" & ex.Message)
            MessageBox.Show(Me, "Button_PlayCalls:Error:" & ex.Message)
        End Try
    End Sub
#Region "webSocket"
    Private Sub Timer_wsReader_Timer(msgFromWSClinet As String)
        Dim TmpString As String

        Try
            Write_Log("", "wsReader(): " & msgFromWSClinet)
            Dim x() As String = msgFromWSClinet.Split("~~~")
            If x(0) = "OnOpen" Then
                Dim ConnectionCount As Long = -1
                Try
                    ConnectionCount = OnlineConnections.Count
                Catch ex As Exception
                End Try
                wsConnectionID = x(3)
                TmpString = "{" & """OnClose""" & ":{" &
                    """ConnectionID""" & ":" & """" & wsConnectionID & """" & "," &
                    """ConnectionCount""" & ":" & """" & ConnectionCount & """" &
                    "}}"
                sendTowsClinet(TmpString, wsConnectionID)
                Exit Sub
            ElseIf x(0) = "OnClose" Then
                If wsConnectionID = x(3) Then
                    wsConnectionID = ""
                End If
                Exit Sub
            ElseIf x(0) = "OnMessage" Then
                msgFromWSClinet = x(6)
            Else
                Exit Sub
            End If
            Dim MsgAfterFirstSpace As String = ""
            Dim MsgAfterLocaltion As Integer = 0
            MsgAfterLocaltion = InStr(1, msgFromWSClinet, " ")
            Dim TheCommandText As String
            TheCommandText = Trim(Mid(msgFromWSClinet, MsgAfterLocaltion))
            tsString = Mid(msgFromWSClinet, 4, 13)
            If TheCommandText = "" Then Exit Sub

            If AgentInformation.LoggedIn = False Then
                If InStr(1, TheCommandText, "cmd{login,") > 0 Then
                    GoTo processGetAgentLoginMsg
                ElseIf InStr(1, TheCommandText, "cmd{get-agents}") > 0 Then
                    GoTo processGetAgentsMsg
                ElseIf InStr(1, TheCommandText, "cmd{AgentStatus}") > 0 Then
                    GoTo processGetAgentsStats
                ElseIf InStr(1, TheCommandText, "cmd{ready-inbound}") > 0 Then
                    GoTo processGetAgentsStats
                ElseIf InStr(1, TheCommandText, "cmd{ready-outbound") > 0 Then
                    GoTo processGetAgentsStats
                Else
                    msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""logout"":""Not logged-in"",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                    sendTowsClinet(msgFromWSClinet)
                End If
                Exit Sub
            End If

            Select Case TheCommandText

                Case "cmd{get-agents}"
processGetAgentsMsg:
                    msgFromWSClinet = "" 'getwsAgentsList(tsString)
                    sendTowsClinet(msgFromWSClinet)

        'Case "cmd{login}"

                Case "cmd{logout}"
                    If AgentInformation.LoggedIn = False Then
                        msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""logout"":""Not logged-in"",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                        sendTowsClinet(msgFromWSClinet)
                    Else
                        'Do Process Login
                        logoutRequestfromWS = True
                        Process_Logout()
                    End If


                Case "cmd{notready-meeting}"
                    Select Case CallInformation.CallStatus
                        Case "Talking", "OnHold", "Ringing"
                            msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""nr"":" & """Sorry""" & ",""ActivitySessionID"":""" & AgentInformation.ActivitySessionID & """ }" & "}"
                            sendTowsClinet(msgFromWSClinet)
                        Case Else
                            SetupScreen("DND", "Meeting")
                    End Select
                Case "cmd{notready-prayer}"
                    Select Case CallInformation.CallStatus
                        Case "Talking", "OnHold", "Ringing"
                            msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""nr"":" & """Sorry""" & ",""ActivitySessionID"":""" & AgentInformation.ActivitySessionID & """ }" & "}"
                            sendTowsClinet(msgFromWSClinet)
                        Case Else
                            SetupScreen("DND", "Prayer")
                    End Select
                Case "cmd{notready-lunch}"
                    Select Case CallInformation.CallStatus
                        Case "Talking", "OnHold", "Ringing"
                            msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""nr"":" & """Sorry""" & ",""ActivitySessionID"":""" & AgentInformation.ActivitySessionID & """ }" & "}"
                            sendTowsClinet(msgFromWSClinet)
                        Case Else
                            SetupScreen("DND", "Lunch")
                    End Select

                Case "cmd{notready-other}"
                    Select Case CallInformation.CallStatus
                        Case "Talking", "OnHold", "Ringing"
                            msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""nr"":" & """Sorry""" & ",""ActivitySessionID"":""" & AgentInformation.ActivitySessionID & """ }" & "}"
                            sendTowsClinet(msgFromWSClinet)
                        Case Else
                            SetupScreen("DND", "Other")
                    End Select

                Case "cmd{CallReject}"
                    If CallInformation.CallStatus = "Ringing" Then
                        Button_Call_Hangup_Click(Me, e:=New EventArgs)
                        msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""CallReject"":" & """OKAY""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                    Else
                        msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""CallReject"":" & """Sorry""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                    End If
                    sendTowsClinet(msgFromWSClinet)

                Case "cmd{CallAccept}"
                    If Button_Call_Connect.Enabled = False Then
                        Write_Log("", "wsReader(): " & "Call State:" & CallInformation.CallStatus)
                        'msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""CallAccept"":" & """Sorry""" & """,""State"":" & """ & CallInformation.CallStatus & """ & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                        msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""CallAccept"":" & """Sorry""" & ",""State"":""" & CallInformation.CallStatus & """,""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                        sendTowsClinet(msgFromWSClinet)
                        Exit Sub
                    End If
                    'If CallInformation.CallStatus <> "Ringing" Then
                    'Write_Log("", "wsReader(): " & "Calling not Ringing")
                    'msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""CallAccept"":" & """Sorry""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                    'sendTowsClinet(msgFromWSClinet)
                    'Exit Sub
                    'End If
                    Write_Log("", "wsReader(): " & "Call not accepted yet :" & CallInformation.CallStatus)
                    Button_Call_Connect_Click(Me, e:=New EventArgs)

                    'If CallInformation.CallStatus = "Ringing" Then
                    'Button_Call_Connect_Click(Me, e:=New EventArgs)
                    'msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""CallAccept"":" & """OKAY""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                    'Else
                    'msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""CallAccept"":" & """Sorry""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                    'End If
                    'sendTowsClinet(msgFromWSClinet)




                Case "cmd{Callhold}", "cmd{CallUnhold}"
                    If CallInformation.CallStatus = "Talking" Or CallInformation.CallStatus = "OnHold" Then
                        Button_Call_Hold_Click(Me, e:=New EventArgs)
                        msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""hold"":" & """OKAY""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                    Else
                        msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""hold"":" & """Sorry""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                    End If
                    sendTowsClinet(msgFromWSClinet)

                Case "cmd{CallMute}", "cmd{CallUnmute}"
                    If CallInformation.CallStatus = "Talking" Then
                        ' if Command_Mute.Tag = "Mute" Then Do UnMute
                        ' if Command_Mute.Tag = "Mute" Then Do Mute
                        Button_Call_Mute_Click(Me, e:=New EventArgs)
                        msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""mute"":" & """OKAY""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                    Else
                        msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""mute"":" & """Sorry""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                    End If
                    sendTowsClinet(msgFromWSClinet)

                Case "cmd{CallStatus}"
                    TmpString = "{" & """CallStatus""" & ":{" &
                    """ts""" & ":" & """" & tsString & """" & "," &
                    """CallStatus""" & ":" & """" & CallInformation.CallStatus & """" & "," &
                    """CallerID""" & ":" & """" & CallInformation.CallerID & """" & "," &
                    """CalledID""" & ":" & """" & CallInformation.CalledID & """" & "," &
                    """CustomerID""" & ":" & """" & CallInformation.CustomerID & """" & "," &
                    """CallerVerifyed""" & ":" & """" & CallInformation.CallerVerifyed & """" & "," &
                    """FeedBack""" & ":" & """" & CallInformation.FeedBack & """" & "," &
                    """RefID_DB""" & ":" & """" & CallInformation.RefID_DB & """" & "," &
                    """RefID_Server""" & ":" & """" & CallInformation.RefID_Server & """" & "," &
                    """CallDirection""" & ":" & """" & CallInformation.CallDirection & """" & "," &
                    """QueueID""" & ":" & """" & AgentInformation.QueueID & """" & "," &
                    """QueueName""" & ":" & """" & AgentInformation.QueueName & """" & "," &
                    """CRMInteractionID""" & ":" & """" & CallInformation.CRMInteractionID & """" & "," &
                    """CRMSessionID""" & ":" & """" & CallInformation.CRMSessionID & """" &
                    "}}"
                    sendTowsClinet(TmpString)

                Case "cmd{AgentStatus}"
processGetAgentsStats:
                    'Dim TmpString As String
                    TmpString = "{" & """AgentStatus""" & ":{" &
                    """ts""" & ":" & """" & tsString & """" & "," &
                    """LoggedIn""" & ":" & """" & AgentInformation.LoggedIn & """" & "," &
                    """AgentID""" & ":" & """" & AgentInformation.AgentID & """" & "," &
                    """AppPath""" & ":" & """" & "" & """" & "," &
                    """AppVersion""" & ":" & """" & AgentInformation.AppVersion & """" & "," &
                    """DND""" & ":" & """" & AgentInformation.DND & """" & "," &
                    """DNDCaption""" & ":" & """" & AgentInformation.DNDCaption & """" & "," &
                    """FirstName""" & ":" & """" & AgentInformation.FirstName & """" & "," &
                    """LastName""" & ":" & """" & AgentInformation.LastName & """" & "," &
                    """LoginName""" & ":" & """" & AgentInformation.LoginName & """" & "," &
                    """LoginSessionID""" & ":" & """" & AgentInformation.LoginSessionID & """" & "," &
                    """PABXExt""" & ":" & """" & AgentInformation.PABXExt & """" & "," &
                    """QueueID""" & ":" & """" & AgentInformation.QueueID & """" & "," &
                    """QueueName""" & ":" & """" & AgentInformation.QueueName & """" & "," &
                    """IVR""" & ":" & """" & IVRisWorking & """" & "," &
                    """Supervisor""" & ":" & """" & AgentInformation.Supervisor & """" &
                    "}}"
                    sendTowsClinet(TmpString)

                Case Else
                    If InStr(1, TheCommandText, "cmd{login,") > 0 Then
processGetAgentLoginMsg:
                        If AgentInformation.LoggedIn = True Then
                            msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""login"":""Sorry, Already logged-in"",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                            sendTowsClinet(msgFromWSClinet)
                        Else
                            Dim tmpAgentID As String
                            tmpAgentID = Mid(TheCommandText, 11)
                            tmpAgentID = Replace(tmpAgentID, "}", "")
                            Try
                                TextBox_LoginName.Text = tmpAgentID
                            Catch ex As Exception
                            End Try
                            Try
                                TextBox_Password.Text = "TheMasterValue"
                            Catch ex As Exception
                            End Try
                            Try
                                bRequestFromWSClent = True
                                Button_Login_Click(Me, e:=New EventArgs)
                            Catch ex As Exception
                            End Try
                        End If
                    ElseIf InStr(1, TheCommandText, "cmd{CallDisconnect") > 0 Then
                        '' change on 27-Nov-2023
                        '' to send call on feedback collection if required
                        ''         true,false (false is default)
                        ''         vdn
                        ''         language

                        Dim sNewCMDText As String
                        Dim sNewCMDBroken() As String

                        sNewCMDText = Replace(TheCommandText, "cmd", "")
                        sNewCMDText = Replace(sNewCMDText, "{", "")
                        sNewCMDText = Replace(sNewCMDText, "}", "")
                        sNewCMDBroken = Split(sNewCMDText, ",")
                        Debug.Print("sNewCMDBroken.count = " & UBound(sNewCMDBroken))

                        Dim feedback As Boolean = False
                        Dim feedbacklanguage As String = "english"
                        Dim feedbackVDN As String = ""

                        If CallInformation.CallStatus = "Talking" Then
                            If CallInformation.Transfering = True Then
                                GoTo DoCalltransfer
                            Else
                                feedback = False
                                feedbacklanguage = "english"
                                feedbackVDN = ""
                                msgFromWSClinet = ""
                                If sNewCMDBroken.Length > 1 Then
                                    If LCase(sNewCMDBroken(1)) = "true" Then
                                        feedback = True
                                        Try
                                            feedbackVDN = sNewCMDBroken(2)
                                        Catch ex_feedbackVDN As Exception
                                        End Try
                                        If feedbackVDN = "" Then
                                            msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""CallDisconnect"":""Sorry, VDN missing"",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                            GoTo DoDisconnectSorry
                                        End If
                                        Try
                                            feedbacklanguage = sNewCMDBroken(3)
                                        Catch ex_feedbacklanguage As Exception
                                        End Try
                                        If feedbacklanguage = "" Then
                                            msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""CallDisconnect"":""Sorry, Language missing"",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                            GoTo DoDisconnectSorry
                                        End If
                                    End If
                                End If
                                CallInformation.SendbacktoIVRLanguage = ""
                                CallInformation.SendbacktoIVRVDN = ""
                                CallInformation.SendBacktoIVR = feedback
                                If CallInformation.SendBacktoIVR = True Then
                                    CallInformation.SendbacktoIVRLanguage = feedbacklanguage
                                    CallInformation.SendbacktoIVRVDN = feedbackVDN
                                End If

                                'If feedback = True Then
                                'Line Added By Atif 27 November 2023
                                '    ovsControl.Call_refer(0, CallInformation.nLineSessionID1, feedbackVDN)
                                'Else
                                Button_Call_Hangup_Click(Me, e:=New EventArgs)
                                'End If
                                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""CallDisconnect"":" & """OKAY""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                            End If
                        Else
                            'Atif -20 September 2022, Identified by Azam on 19 September
                            msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""CallDisconnect"":" & """Sorry""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                            'msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""CallDisconnect:" & """Sorry""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                        End If
DoDisconnectSorry:
                        sendTowsClinet(msgFromWSClinet)
                    ElseIf InStr(1, TheCommandText, "cmd{pinverify,") > 0 Then
                        '' requirs InteractionID
                        ''         SessionID
                        ''         Language
                        ''         CNIC
                        If CallInformation.CallStatus <> "Talking" Then
                            msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pinverify"":" & """Sorry""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                            sendTowsClinet(msgFromWSClinet)
                        Else
                            If IVRisWorking Then
                                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pinverify"":" & """Sorry""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                sendTowsClinet(msgFromWSClinet)
                            Else
                                IVR_TPINVerify(TheCommandText)
                                IVRisWorking = False
                            End If
                        End If

                    ElseIf InStr(1, TheCommandText, "cmd{pingentpin,") > 0 Then
                        '' requirs InteractionID
                        ''         SessionID
                        ''         Language
                        ''         CNIC
                        'If CallInformation.CallStatus <> "Talking" Then
                        '    msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingentpin"":" & """Sorry""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                        '    sendTowsClinet(msgFromWSClinet)
                        'Else
                        '    If IVRisWorking Then
                        '        msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingentpin"":" & """Sorry""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                        '        sendTowsClinet(msgFromWSClinet)
                        '    Else
                        '        IVR_TPINGen(TheCommandText)
                        '        IVRisWorking = False
                        '    End If
                        'End If



                        Write_Log("", "pingentpin Start CallStatus= " & CallInformation.CallStatus & ", IVRisWorking:" & IVRisWorking & ", Interaction:" & CallInformation.CRMInteractionID & ", SessionID=" & CallInformation.CRMSessionID)
                        If CallInformation.CallStatus <> "Talking" Then
                            Write_Log("", "pingentpin Start CallStatus= " & CallInformation.CallStatus & ", IVRisWorking:" & IVRisWorking & " Not Talking")
                            msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingentpin"":" & """Sorry""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                            sendTowsClinet(msgFromWSClinet)
                        Else

                            If IVRisWorking = True Then
                                Write_Log("", "pingentpin Start CallStatus= " & CallInformation.CallStatus & ", IVRisWorking:" & IVRisWorking & " IVR Already working")
                                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingentpin"":" & """Sorry""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                sendTowsClinet(msgFromWSClinet)
                            Else
                                Write_Log("", "pingentpin Start CallStatus= " & CallInformation.CallStatus & ", IVRisWorking:" & IVRisWorking & " IVR Actually starting")
                                IVR_TPINGen(TheCommandText)
                                Write_Log("", "pingentpin Start CallStatus= " & CallInformation.CallStatus & ", IVRisWorking:" & IVRisWorking & " IVR End")
                                IVRisWorking = False
                            End If
                        End If



                    ElseIf InStr(1, TheCommandText, "cmd{pingenatm,") > 0 Then
                        '' requirs InteractionID
                        ''         SessionID
                        ''         Language
                        ''         CNIC
                        ''         CardNo
                        Write_Log("", "pingenatm Start CallStatus= " & CallInformation.CallStatus & ", IVRisWorking:" & IVRisWorking & ", Interaction:" & CallInformation.CRMInteractionID & ", SessionID=" & CallInformation.CRMSessionID)
                        If CallInformation.CallStatus <> "Talking" Then
                            Write_Log("", "pingenatm Start CallStatus= " & CallInformation.CallStatus & ", IVRisWorking:" & IVRisWorking & " Not Talking")
                            msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingenatm"":" & """Sorry""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                            sendTowsClinet(msgFromWSClinet)
                        Else
                            If IVRisWorking Then
                                Write_Log("", "pingenatm Start CallStatus= " & CallInformation.CallStatus & ", IVRisWorking:" & IVRisWorking & " IVR Already working")
                                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingenatm"":" & """Sorry""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                sendTowsClinet(msgFromWSClinet)
                            Else
                                Write_Log("", "pingenatm Start CallStatus= " & CallInformation.CallStatus & ", IVRisWorking:" & IVRisWorking & " IVR Actually starting")
                                IVR_ATMPINGen(TheCommandText)
                                Write_Log("", "pingenatm Start CallStatus= " & CallInformation.CallStatus & ", IVRisWorking:" & IVRisWorking & " IVR End")
                                IVRisWorking = False
                            End If
                        End If
                    ElseIf InStr(1, TheCommandText, "cmd{callout,") > 0 Then
                        'Do Process Callout
                        If AgentInformation.LoggedIn = False Then
                            msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""callout"":" & """Not logged-in""" & ","",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                            sendTowsClinet(msgFromWSClinet)
                            Exit Sub
                        Else
                            If CallInformation.CallDirection <> "Outbound" Then
                                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""callout"":" & """Not in outbound mode""" & ","",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                sendTowsClinet(msgFromWSClinet)
                                Exit Sub
                            End If
                            If Label_AgentStatus.Text <> "State:Idle" Then
                                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""callout"":" & """State is not idle""" & ","",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                sendTowsClinet(msgFromWSClinet)
                                Exit Sub
                            End If
                            Dim tmpNumberToCall As String
                            tmpNumberToCall = Mid(TheCommandText, 13)
                            tmpNumberToCall = Replace(tmpNumberToCall, "}", "")
                            'On Error Resume Next
                            Try
                                TextBox_Number.Text = tmpNumberToCall
                            Catch ex As Exception
                            End Try
                            Button_Call_Connect_Click(sender:=Me, e:=New EventArgs)
                            msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""callout"":" & """Starting""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                        End If
                        sendTowsClinet(msgFromWSClinet)

                        '""""""""

                    ElseIf InStr(1, TheCommandText, "cmd{ready-inbound") > 0 Then
                        Select Case CallInformation.CallStatus
                            Case "Talking", "OnHold", "Ringing"
                                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""ready-inbound"":" & """Sorry""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                sendTowsClinet(msgFromWSClinet)
                            Case Else
                                If AgentInformation.DND = True Then
                                    'SetupReady ("Inbound")
                                    Dim tmpOBMode As String = ""
                                    tmpOBMode = Mid(TheCommandText, 20)
                                    tmpOBMode = Replace(tmpOBMode, "}", "")
                                    AgentInformation.ReadyMode = tmpOBMode
                                    Button_Inbound_Click(Me, e:=New EventArgs)
                                    msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""ready-inbound"":" & """OKAY""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                Else
                                    msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""ready-inbound"":" & """Sorry""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                End If
                                sendTowsClinet(msgFromWSClinet)
                        End Select




                    ElseIf InStr(1, TheCommandText, "cmd{ready-outbound") > 0 Then
                        Select Case CallInformation.CallStatus
                            Case "Talking", "OnHold", "Ringing"
                                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""ready-outbound"":" & """Sorry""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                sendTowsClinet(msgFromWSClinet)
                            Case Else
                                If AgentInformation.DND = True Then
                                    'SetupReady ("Outbound")
                                    Dim tmpOBMode As String = ""
                                    tmpOBMode = Mid(TheCommandText, 20)
                                    tmpOBMode = Replace(tmpOBMode, "}", "")
                                    AgentInformation.ReadyMode = tmpOBMode
                                    Button_Outbound_Click(Me, e:=New EventArgs)
                                    msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""ready-outbound"":" & """OKAY""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                Else
                                    msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""ready-outbound"":" & """Sorry""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                End If
                                sendTowsClinet(msgFromWSClinet)
                        End Select


                    ElseIf InStr(1, TheCommandText, "cmd{callagent,") > 0 Then
                        '==========================================================
                        If AgentInformation.LoggedIn <> True Then

                            ' """CRMInteractionID""" & ":" & """" & CallInformation.CRMInteractionID & """" & "," &

                            msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""callagent"":""Sorry, Not logged-in"",""loginsessionid"":""" & AgentInformation.LoginSessionID & """,""CRMInteractionID"":""" & CallInformation.CRMInteractionID & """ }" & "}"
                            sendTowsClinet(msgFromWSClinet)
                            Exit Sub
                        ElseIf CallInformation.CallStatus <> "Talking" Then
                            msgFromWSClinet = "{" & """cmd""{""ts"":""" & tsString & """,""callagent"":" & """Sorry""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """,""CRMInteractionID"":""" & CallInformation.CRMInteractionID & """ }" & "}"
                            sendTowsClinet(msgFromWSClinet)
                            Exit Sub
                        End If

                        Dim subCMD() = {""}
                        'Dim subCMD() As String
                        'subCMD = Mid(TheCommandText, 15)
                        subCMD = Split(TheCommandText, ",")
                        Dim subCMD2 As String = Replace(subCMD(1), "}", "")


                        Select Case LCase(Trim(subCMD2))
                            Case "cancel"
                                Try
                                    msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""callagent"":" & """Canceling""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                    sendTowsClinet(msgFromWSClinet)
                                    ovsControl.Call_Hangup(0, CallInformation.nLineSessionID2)
                                Catch ex As Exception
                                    msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""callagent"":" & """Error " & ex.Message & """" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                    sendTowsClinet(msgFromWSClinet)
                                End Try
                                Exit Sub
    'ATIF  START
                            Case "blindtransfer"
                                Try
                                    CallInformation.CRMInteractionID = subCMD(3)
                                    CallInformation.CRMSessionID = Replace(subCMD(4), "}", "")

                                    Console.WriteLine("subCMD " & subCMD(1) & " Received")

                                    Dim agDT As DataTable
                                    agDT = dbClass.Get_Agent(subCMD(2))
                                    If agDT.Rows.Count <= 0 Then
                                        msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""callagent"":" & """Sorry, Agent not available""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                        sendTowsClinet(msgFromWSClinet)
                                        Exit Sub
                                    End If
                                    If agDT(0)("Loggedin") = "0" Then
                                        msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""callagent"":" & """Sorry, Agent not loggedin""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                        sendTowsClinet(msgFromWSClinet)
                                        Exit Sub
                                    End If
                                    If agDT(0)("Direction") <> "Inbound" Then
                                        msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""callagent"":" & """Sorry, Agent not in inbound state""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                        sendTowsClinet(msgFromWSClinet)
                                        Exit Sub
                                    End If
                                    If agDT(0)("State") <> "Idle" Then
                                        msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""callagent"":" & """Sorry, Agent not idle""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                        sendTowsClinet(msgFromWSClinet)
                                        Exit Sub
                                    End If

                                    '' Here now i have the Extenstion which needs to dialed
                                    CallInformation.nLineSessionEXTENSION = "" & agDT(0)("PABXExt")
                                    CallInformation.nLineSessionStatus = "Try"

                                    If CallInformation.CallDirection = "Inbound" Then
                                        dbClass.Global_QMCALL_ADDCAllDetails(0, CLng(CallInformation.RefID_DB), CallInformation.CallerID, CallInformation.CalledID, AgentInformation.QueueID, 1, CallInformation.CallerVerifyed, CallInformation.CRMSessionID, CallInformation.CRMInteractionID)
                                    Else
                                        dbClass.Global_QMCALL_ADDCAllDetails(0, CLng(CallInformation.RefID_DB), CallInformation.CallerID, CallInformation.CalledID, AgentInformation.QueueID, 2, CallInformation.CallerVerifyed, CallInformation.CRMSessionID, CallInformation.CRMInteractionID)
                                    End If
                                    Write_Log("", "wsReader_Clinet:MSG:(" & "CallRefering :" & CallInformation.nLineSessionEXTENSION & "@" & subCMD(2) & ")")

                                    msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""callagent"":" & """Transfering""" & "," & """CRMInteractionID""" & ":""" & CallInformation.CRMInteractionID & """," & """CRMSessionID""" & ":""" & CallInformation.CRMSessionID & """,""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                    sendTowsClinet(msgFromWSClinet)

                                    ovsControl.Call_refer(0, CallInformation.nLineSessionID1, CallInformation.nLineSessionEXTENSION)

                                Catch ex As Exception
                                    msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""callagent"":" & """Error " & ex.Message & """" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                    sendTowsClinet(msgFromWSClinet)
                                End Try
                                Exit Sub

    'ATIF  END

                            Case "transfer"
DoCalltransfer:
                                Try
                                    CallInformation.CRMInteractionID = subCMD(2)
                                    CallInformation.CRMSessionID = subCMD(3)

                                    msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""callagent"":" & """Transfering""" & "," & """CRMInteractionID""" & ":""" & CallInformation.CRMInteractionID & """," & """CRMSessionID""" & ":""" & CallInformation.CRMSessionID & """,""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                    sendTowsClinet(msgFromWSClinet)
                                    ovsControl.Call_Transfer(0, CallInformation.nLineSessionID1, CallInformation.nLineSessionID2, CallInformation.nLineSessionEXTENSION)
                                    'ovsControl.Call_refer(0, CallInformation.nLineSessionID1, CallInformation.nLineSessionEXTENSION)

                                Catch ex As Exception
                                    msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""callagent"":" & """Error " & ex.Message & """" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                    sendTowsClinet(msgFromWSClinet)
                                End Try
                                Exit Sub

    'ATIF  START
                            Case "conference"
                                CallInformation.CRMInteractionID = subCMD(3)
                                CallInformation.CRMSessionID = Replace(subCMD(4), "}", "")


                                Console.WriteLine("subCMD " & subCMD(1) & " Received")

                                'Dim agDT As DataTable
                                'agDT = dbClass.Get_Agent(subCMD(2))
                                'If agDT.Rows.Count <= 0 Then
                                '    msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""callagent"":" & """Sorry, Agent not available""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                '    sendTowsClinet(msgFromWSClinet)
                                '    Exit Sub
                                'End If
                                'If agDT(0)("Loggedin") = "0" Then
                                '    msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""callagent"":" & """Sorry, Agent not loggedin""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                '    sendTowsClinet(msgFromWSClinet)
                                '    Exit Sub
                                'End If
                                'If agDT(0)("Direction") <> "Inbound" Then
                                '    msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""callagent"":" & """Sorry, Agent not in inbound state""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                '    sendTowsClinet(msgFromWSClinet)
                                '    Exit Sub
                                'End If
                                'If agDT(0)("State") <> "Idle" Then
                                '    msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""callagent"":" & """Sorry, Agent not idle""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                '    sendTowsClinet(msgFromWSClinet)
                                '    Exit Sub
                                'End If
                                '' Here now i have the Extenstion which needs to dialed
                                'CallInformation.nLineSessionEXTENSION = "" & agDT(0)("PABXExt")
                                'CallInformation.nLineSessionStatus = "Try"
                                'msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""callagent"":" & """Calling""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                'sendTowsClinet(msgFromWSClinet)
                                'updateStatusText("Calling :" & CallInformation.nLineSessionEXTENSION & "@" & subCMD(2))
                                'Write_Log("", "wsReader_Clinet:MSG:(" & "Calling :" & CallInformation.nLineSessionEXTENSION & "@" & subCMD(2) & ")")

                                If CallInformation.CallDirection = "Inbound" Then
                                    dbClass.Global_QMCALL_ADDCAllDetails(0, CLng(CallInformation.RefID_DB), CallInformation.CallerID, CallInformation.CalledID, AgentInformation.QueueID, 1, CallInformation.CallerVerifyed, CallInformation.CRMSessionID, CallInformation.CRMInteractionID)
                                Else
                                    dbClass.Global_QMCALL_ADDCAllDetails(0, CLng(CallInformation.RefID_DB), CallInformation.CallerID, CallInformation.CalledID, AgentInformation.QueueID, 2, CallInformation.CallerVerifyed, CallInformation.CRMSessionID, CallInformation.CRMInteractionID)
                                End If
                                Write_Log("", "wsReader_Clinet:MSG:(" & "conference :" & CallInformation.nLineSessionEXTENSION & "@" & subCMD(2) & ")")


                                Try
                                    CallInformation.nLineSessionID2 = 0
                                    Dim confstatus As Boolean = False
                                    msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""callagent"":" & """conference""" & "," & """CRMInteractionID""" & ":""" & CallInformation.CRMInteractionID & """," & """CRMSessionID""" & ":""" & CallInformation.CRMSessionID & """,""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                    sendTowsClinet(msgFromWSClinet)
                                    ovsControl.Call_CreateAudioConference(0, CallInformation.nLineSessionID1, CallInformation.nLineSessionID2)
                                    'confstatus = ovsControl.Call_Conference(0, CallInformation.nLineSessionID1, CallInformation.nLineSessionID2, CallInformation.nLineSessionEXTENSION)

                                Catch ex As Exception
                                    msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""callagent"":" & """Error " & ex.Message & """" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                    sendTowsClinet(msgFromWSClinet)
                                End Try
                                Exit Sub


                                'ATIF  END

                            Case Else

MakeCall:
                                Console.WriteLine("subCMD " & subCMD2 & " Received")

                                Dim agDT As DataTable
                                agDT = dbClass.Get_Agent(subCMD2)
                                If agDT.Rows.Count <= 0 Then
                                    msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""callagent"":" & """Sorry, Agent not available""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                    sendTowsClinet(msgFromWSClinet)
                                    Exit Sub
                                End If
                                If agDT(0)("Loggedin") = "0" Then
                                    msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""callagent"":" & """Sorry, Agent not loggedin""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                    sendTowsClinet(msgFromWSClinet)
                                    Exit Sub
                                End If
                                If agDT(0)("Direction") <> "Inbound" Then
                                    msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""callagent"":" & """Sorry, Agent not in inbound state""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                    sendTowsClinet(msgFromWSClinet)
                                    Exit Sub
                                End If
                                If agDT(0)("State") <> "Idle" Then
                                    msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""callagent"":" & """Sorry, Agent not idle""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                    sendTowsClinet(msgFromWSClinet)
                                    Exit Sub
                                End If
                                '' Here now i have the Extenstion which needs to dialed
                                CallInformation.nLineSessionEXTENSION = "" & agDT(0)("PABXExt")
                                CallInformation.nLineSessionStatus = "Try"
                                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""callagent"":" & """Calling""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                sendTowsClinet(msgFromWSClinet)
                                updateStatusText("Calling :" & CallInformation.nLineSessionEXTENSION & "@" & subCMD2)
                                Write_Log("", "wsReader_Clinet:MSG:(" & "Calling :" & CallInformation.nLineSessionEXTENSION & "@" & subCMD2 & ")")
                                Try
                                    CallInformation.nLineSessionID2 = 0
                                    ovsControl.Call_Make(0, CallInformation.nLineSessionEXTENSION, False, True)
                                    CallInformation.Transfering = True
                                    CallInformation.nLineSessionStatus = "Try"
                                Catch ex As Exception
                                    CallInformation.nLineSessionStatus = "Error"
                                    CallInformation.Transfering = False
                                    msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""callagent"":" & """Error""" & ",""loginsessionid"":""" & AgentInformation.LoginSessionID & """ }" & "}"
                                    sendTowsClinet(msgFromWSClinet)
                                    updateStatusText("Calling :Error")
                                    Write_Log("", "wsReader_Clinet:MSG:(" & "Calling :" & ex.Message)
                                    Exit Sub
                                End Try
                        End Select


                        '' 00001 - Check if provided ID is valid
                        '' 00002 - Check if provided ID is logged-in
                        '' 00003 - Check if provided ID is Ready-Inbound

                        '' R0011 - Calling

                        '' R0021 - Connected
                        '' R0022 - Busy
                        '' R0023 - NoAns
                        '' R0024 - Rejected

                        '' 00031 - other-call-cancel 
                        '' 00032 - other-call-tranfer
                        '' 00033 - other-call-conference

                        '' R0061 - other-call-canceled
                        '' R0062 - other-call-tranfered
                        '' R0063 - other-call-conferenced

                    End If
                    Write_Log("", "wsReader_Clinet:MSG:(" & TheCommandText & ")")
            End Select
            'Exit Sub


        Catch ex As Exception
            Write_Log("", "wsReader_Timer:Error:" & "" & " (" & ex.Message & ") msgFromWSClinet:" & msgFromWSClinet)
            'MessageBox.Show(Me, "wsReader_Timer:" & "" & " (" & ex.Message & ") msgFromWSClinet:" & msgFromWSClinet)
        End Try

    End Sub
    Private Function Check_Other_Agent(ByVal _agentID As String, ByRef _ext As String)
        Check_Other_Agent = False
        Try

            _ext = "2001"
            Check_Other_Agent = True
        Catch ex As Exception
            Write_Log("", "Check_Other_Agent:Error:" & "" & " (" & ex.Message() & ") _agentID:" & _agentID)
            'MessageBox.Show(Me, "wsReader_Timer:" & "" & " (" & ex.Message() & ") _agentID:" & _agentID)
        End Try
    End Function

#Region "IVRFunctions"
    Private Sub IVR_PlayFileAndGetDigits(szFileNames As String, Label As String, TimeOut As Integer, Digits As Integer, TermDigits As String)
        sInputs = ""
        If Trim(TermDigits) = "" Then
            TermDigits = "*#0123456789"
        End If
        If CallInformation.CallStatus <> "Talking" Then
            Exit Sub
        End If

        Dim xDateDiff As Integer = 0
        Dim _waitStartTime As DateTime = DateTime.Now
        Try
            Try
                'ovsControl.media_stopPlay(0, 0, CallInformation.nLineSessionID1)
                Application.DoEvents()
                ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, True, szFileNames, True, TermDigits))
                Application.DoEvents()
            Catch ex1 As Exception
                Write_Log("", "IVR_PlayFileAndGetDigits(ex1) Exception:" & ex1.Message)
                If ex1.Message.ToString.Contains("Already Playing file") Then
                    ovsControl.media_stopPlay(0, 0, CallInformation.nLineSessionID1)
                    Application.DoEvents()
                    ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, True, szFileNames, True, TermDigits))
                End If
            End Try
            Do While True
                Application.DoEvents()
                If IVRisWorking = False Then
                    Exit Do
                End If
                If CallInformation.CallStatus <> "Talking" Then
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
                If DateDiff(DateInterval.Second, _waitStartTime, Now) >= TimeOut Then
                    Exit Do
                End If
                Application.DoEvents()
                If UCase(Trim(TermDigits)) = "A" Then
                    updateStatusText(Label & ":Digits(0/0), " & xDateDiff & ", tm=" & DateTime.Now.ToString("ss:ffff").ToString())
                Else
                    updateStatusText(Label & ":Digits(" & (Digits - Len(sInputs) & "/" & Digits) & "), " & xDateDiff & ", tm=" & DateTime.Now.ToString("ss:ffff").ToString())
                End If
                Thread.Sleep(100)
            Loop
        Catch ex As Exception
            Write_Log("", "IVR_PlayFileAndGetDigits(ex) Exception:" & ex.Message)
        End Try

    End Sub

    Private Sub IVR_TPINVerify(cmd As String)
        Try
            IVRisWorking = True
            Dim msgFromWSClinet As String

            Write_Log("", "StartTPINVerify:Start:" & cmd)

            Dim sNewCMDText As String
            Dim sNewCMDBroken() As String

            sNewCMDText = Replace(cmd, "cmd", "")
            sNewCMDText = Replace(sNewCMDText, "{", "")
            sNewCMDText = Replace(sNewCMDText, "}", "")
            sNewCMDBroken = Split(sNewCMDText, ",")
            Debug.Print("sNewCMDBroken.count = " & UBound(sNewCMDBroken))

            Dim tsString As String = ""
            msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pinverify"":""Start"",""infoValidation"":""" & "Start" & """ }" & "}"
            'Form_Main.sendTowsClinet msgFromWSClinet
            sendTowsClinet(msgFromWSClinet)

            If UBound(sNewCMDBroken) < 4 Then
                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pinverify"":""Start"",""infoValidation"":""" & "Packet Missing" & """ }" & "}"
                sendTowsClinet(msgFromWSClinet)
                Exit Sub
            End If

            Dim i As Integer
            For i = 0 To UBound(sNewCMDBroken) - 1
                If Len(Trim(sNewCMDBroken(i))) = 0 Then
                    msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pinverify"":""Start"",""infoValidation"":""" & "Empty or invalida packet" & """ }" & "}"
                    sendTowsClinet(msgFromWSClinet)
                    Exit Sub
                End If
            Next

            If IsNumeric(sNewCMDBroken(4)) = False Then
                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pinverify"":""Start"",""infoValidation"":""" & "CNIC Invalid " & sNewCMDBroken(4) & """ }" & "}"
                sendTowsClinet(msgFromWSClinet)
                Exit Sub
            End If

            If Len(sNewCMDBroken(4)) <> 13 Then
                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pinverify"":""Start"",""infoValidation"":""" & "CNIC Invalid " & sNewCMDBroken(4) & """ }" & "}"
                sendTowsClinet(msgFromWSClinet)
                Exit Sub
            End If

            CallInformation.CRMInteractionID = sNewCMDBroken(1)
            CallInformation.CRMSessionID = sNewCMDBroken(2)

            sLanguage = sNewCMDBroken(3)
            CallInformation.CustomerID = sNewCMDBroken(4)
            'Call Command_VerifyTPIN_Click

            ovsControl.media_stopGetDigits(0, 0, CallInformation.nLineSessionID1)
            ovsControl.media_stopPlay(0, 0, CallInformation.nLineSessionID1)
            Application.DoEvents()
            updateStatusText("Playing Existing TPIN1")
            Application.DoEvents()
            sInputs = ""
            Application.DoEvents()

            If Not _TerminalServerSession Then
                ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\TPINExisting.wav", True, "A"))
            End If
            sInputs = ""

            Application.DoEvents()
            Dim tmpstring As String = ovsControl.media_getDigits(New TControl.media_getDigits_Args(0, 0, CallInformation.nLineSessionID1, True, 4, 15, True))
            Dim _waitStartTime As DateTime = DateTime.Now
            Dim xDateDiff As Integer = 0
            While True
                If CallInformation.CallStatus <> "Talking" Then
                    Exit While
                End If
                If IVRisWorking = False Then
                    Exit While
                End If

                xDateDiff = DateDiff(DateInterval.Second, _waitStartTime, Now)
                If DateDiff(DateInterval.Second, _waitStartTime, Now) >= 15 Then
                    'retVal = 0
                    Exit While
                End If
                If Len(sInputs) = 4 Then
                    Exit While
                End If
                Application.DoEvents()
                Threading.Thread.Sleep(200)
                updateStatusText("Existing TPIN waiting for digits :" & (4 - Len(sInputs)) & ", " & xDateDiff)
            End While
            'ovsControl.media_stopGetDigits(0, 0, CallInformation.nLineSessionID1)

            Debug.WriteLine(DateTime.Now().ToString & " return after 4 Digits X " & sInputs)

            If CallInformation.CallStatus <> "Talking" Then
                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pinverify"":""Start"",""infoValidation"":""" & "Not talking" & """ }" & "}"
                sendTowsClinet(msgFromWSClinet)
                Exit Sub
            End If

            If Len(sInputs) = 0 Then
                Application.DoEvents()
                If Not _TerminalServerSession Then
                    ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\TPINNoInput.wav", True, "A"))
                End If
                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pinverify"":""Start"",""infoValidation"":""" & "TPIN Blank" & """ }" & "}"
                updateStatusText("Next command is awaited, no input")
                sendTowsClinet(msgFromWSClinet)
                Exit Sub
            End If
            If (Len(sInputs) <> 4) Or (IsNumeric(sInputs) = False) Then
                Application.DoEvents()
                If Not _TerminalServerSession Then
                    ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\TPINInvalidLength.wav", True, "A"))
                End If
                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pinverify"":""Start"",""infoValidation"":""" & "InvalidLength" & """ }" & "}"
                updateStatusText("Next command is awaited, invalid lenght")
                sendTowsClinet(msgFromWSClinet)
                Exit Sub
            End If


            msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pinverify"":""Start"",""infoValidation"":""" & "Calling API" & """ }" & "}"
            sendTowsClinet(msgFromWSClinet)

            '' call the API
            Dim MakeTPINResponse As Boolean = False
            updateStatusText("Calling API")
            MakeTPINResponse = MakeTPIN(sInputs, True)

            If MakeTPINResponse = True Then
                updateStatusText("Calling API Response: " & MakeTPINResponse)
                Application.DoEvents()
                Write_Log("", "IVR_TPINVerify:Stop calling")
                ovsControl.media_stopPlay(0, 0, CallInformation.nLineSessionID1)
                Application.DoEvents()
                Write_Log("", "IVR_TPINVerify:Stop called, now playing")
                Application.DoEvents()
                CallInformation.CallerVerifyed = 2
                If Not _TerminalServerSession Then
                    ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\TPINVerifyed.wav", True, ""))
                End If

                Write_Log("", "IVR_TPINVerify:File played, sending response to Pulse")
                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pinverify"":""Start"",""infoValidation"":""" & "Done" & """ }" & "}"
                sendTowsClinet(msgFromWSClinet)
                Write_Log("", "IVR_TPINVerify:ResponseSent:True:" & "" & " (" & msgFromWSClinet & ")")
            Else
                updateStatusText("Calling API Response: " & MakeTPINResponse)
                Application.DoEvents()
                Write_Log("", "IVR_TPINVerify:Stop calling")
                ovsControl.media_stopPlay(0, 0, CallInformation.nLineSessionID1)
                Application.DoEvents()
                Write_Log("", "IVR_TPINVerify:Stop called, now playing")
                Application.DoEvents()
                If Not _TerminalServerSession Then
                    ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\TPINInvalid.wav", True, "A"))
                End If

                Write_Log("", "IVR_TPINVerify:File played, sending response to Pulse")
                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pinverify"":""Start"",""infoValidation"":""" & "Failed" & """ ,""InfoDescription"":""" & sTempResponseString & """ }" & "}"
                '& """ ,""ResponseString"":""" & sTempResponseString & """
                sendTowsClinet(msgFromWSClinet)
                Write_Log("", "IVR_TPINVerify:ResponseSent:False:" & "" & " (" & msgFromWSClinet & ")")
            End If

        Catch ex As Exception
            Write_Log("", "IVR_TPINVerify:Error:" & "" & " (" & ex.Message & ")")
            updateStatusText("IVR_TPINVerify:Error:" & "" & " (" & ex.Message & ")")
            'MessageBox.Show(Me, "IVR_TPINVerify:" & "" & " (" & ex.Message & ")")
        End Try

    End Sub
    Private Sub IVR_ATMPINGen(cmd As String)
        Dim msgFromWSClinet As String

        Try

            'Dim x As Integer
            'Dim y As Integer
            'Console.WriteLine("x / y: " & x / y)
            'Console.WriteLine("x \ y: " & x \ y)
            'Console.WriteLine("x Mod y: " & x Mod y)

            Dim iCurrentTryCountPIN2 As Integer = 0

            IVRisWorking = True

            Write_Log("", "IVR_ATMPINGen:Start:" & cmd)

            Dim sNewCMDText As String
            Dim sNewCMDBroken() As String

            sNewCMDText = Replace(cmd, "cmd", "")
            sNewCMDText = Replace(sNewCMDText, "{", "")
            sNewCMDText = Replace(sNewCMDText, "}", "")
            sNewCMDBroken = Split(sNewCMDText, ",")
            Dim sCardNo As String = ""

            Debug.Print("sNewCMDBroken.count = " & UBound(sNewCMDBroken))

            Dim tsString As String = ""
            msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingenatm"":""Start"",""infoValidation"":""" & "Start" & """ }" & "}"
            sendTowsClinet(msgFromWSClinet)

            If UBound(sNewCMDBroken) < 4 Then
                updateStatusText("Error: Packet Missing")
                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingenatm"":""Start"",""infoValidation"":""" & "Packet Missing" & """ }" & "}"
                sendTowsClinet(msgFromWSClinet)
                Exit Sub
            End If

            Dim i As Integer
            For i = 0 To UBound(sNewCMDBroken) - 1
                If Len(Trim(sNewCMDBroken(i))) = 0 Then
                    updateStatusText("Error: Empty or Invalid packet")
                    msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingenatm"":""Start"",""infoValidation"":""" & "Empty or invalid packet" & """ }" & "}"
                    sendTowsClinet(msgFromWSClinet)
                    Exit Sub
                End If
            Next

            If IsNumeric(sNewCMDBroken(4)) = False Then
                updateStatusText("Error: CNIC Invalid X1")
                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingenatm"":""Start"",""infoValidation"":""" & "CNIC Invalid " & sNewCMDBroken(4) & """ }" & "}"
                sendTowsClinet(msgFromWSClinet)
                Exit Sub
            End If

            If Len(sNewCMDBroken(4)) <> 13 Then
                updateStatusText("Error: CNIC Invalid X2")
                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingenatm"":""Start"",""infoValidation"":""" & "CNIC Invalid " & sNewCMDBroken(4) & """ }" & "}"
                sendTowsClinet(msgFromWSClinet)
                Exit Sub
            End If

            CallInformation.CRMInteractionID = sNewCMDBroken(1)
            CallInformation.CRMSessionID = sNewCMDBroken(2)

            sLanguage = sNewCMDBroken(3)
            CallInformation.CustomerID = sNewCMDBroken(4)
            sCardNo = sNewCMDBroken(5)
            If Len(sCardNo) <> 16 Then
                updateStatusText("Error: Card Invalid")
                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingenatm"":""Start"",""infoValidation"":""" & "Card Invalid " & sNewCMDBroken(5) & """ }" & "}"
                sendTowsClinet(msgFromWSClinet)
                Exit Sub
            End If

            'Call Command_VerifyTPIN_Click
            Dim iCurrentTryCount As Integer = 0
            Dim iMaxInputTries As Integer = 3
            Dim TPIN1 As String = ""
            Dim xDateDiff As Integer = 0

GetATMPIN1Again:
            Write_Log("", "ATMPIN entering in taking ATMPIN iCurrentTryCount=[" & iCurrentTryCount & "]")

            iCurrentTryCount = iCurrentTryCount + 1
            Application.DoEvents()
            updateStatusText("Playing ATMPIN1 (ct=" & iCurrentTryCount & ")")

            ovsControl.media_stopGetDigits(0, 0, CallInformation.nLineSessionID1)
            Application.DoEvents()
            ovsControl.media_stopPlay(0, 0, CallInformation.nLineSessionID1)
            Application.DoEvents()
            sInputs = ""
            If Not _TerminalServerSession Then
                IVR_PlayFileAndGetDigits("\Sounds\" & sLanguage & "\ATMPINNNewEnter.wav", "ATM PIN 1", 21, 4, "*#0123456789")
                'ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, True, "\Sounds\" & sLanguage & "\ATMPINNNewEnter.wav", True, "*#0123456789"))
                'Debug.WriteLine("return from media_play")
            End If
            Application.DoEvents()
            'Dim tmpstring As String = ovsControl.media_getDigits(New TControl.media_getDigits_Args(0, 0, CallInformation.nLineSessionID1, True, (4 - Len(sInputs)), 15, True))
            'Dim _waitStartTime As DateTime = DateTime.Now
            'While True
            '    xDateDiff = DateDiff(DateInterval.Second, _waitStartTime, Now)

            '    If CallInformation.CallStatus <> "Talking" Then
            '        If ovsControl.Playing = True Then
            '            ovsControl.media_stopPlay(0, 0, CallInformation.nLineSessionID1)
            '        End If
            '        Exit While
            '    End If
            '    If Len(sInputs) = 1 Then
            '        If ovsControl.Playing = True Then
            '            ovsControl.media_stopPlay(0, 0, CallInformation.nLineSessionID1)
            '        End If
            '    End If
            '    If xDateDiff >= 21 Then
            '        If ovsControl.Playing = True Then
            '            ovsControl.media_stopPlay(0, 0, CallInformation.nLineSessionID1)
            '        End If
            '        Exit While
            '    End If
            '    If Len(sInputs) = 4 Then
            '        If ovsControl.Playing = True Then
            '            ovsControl.media_stopPlay(0, 0, CallInformation.nLineSessionID1)
            '        End If
            '        Exit While
            '    End If
            '    Application.DoEvents()
            '    Threading.Thread.Sleep(200)
            '    updateStatusText("ATMPIN1 waiting :" & (4 - Len(sInputs)) & ", " & xDateDiff & ", ct=" & iCurrentTryCount & ", tm=" & DateTime.Now.ToString("ss:ffff").ToString())
            'End While

            'Debug.WriteLine(DateTime.Now().ToString & " return after 4 Digits X " & sInputs)

            If CallInformation.CallStatus <> "Talking" Then
                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingenatm"":""Start"",""infoValidation"":""" & "Not talking" & """ }" & "}"
                sendTowsClinet(msgFromWSClinet)
                Exit Sub
            End If
            If Len(sInputs) = 0 Then
                Application.DoEvents()
                If Not _TerminalServerSession Then
                    updateStatusText("Playing NoInput (ct=" & iCurrentTryCount & ")")
                    Application.DoEvents()
                    IVR_PlayFileAndGetDigits("\Sounds\" & sLanguage & "\ATMPINNoInput.wav", "No Input", 10, 4, "A")
                    'ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\ATMPINNoInput.wav", True, "A"))
                End If
                Application.DoEvents()

                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingenatm"":""Start"",""currenttry"":""" & iCurrentTryCount & """,""infoValidation"":""" & "NoInput" & """ }" & "}"
                sendTowsClinet(msgFromWSClinet)
                If iCurrentTryCount < iMaxInputTries Then
                    Application.DoEvents()
                    If Not _TerminalServerSession Then
                        updateStatusText("Playing TryAgain (ct=" & iCurrentTryCount & ")")
                        IVR_PlayFileAndGetDigits("\Sounds\" & sLanguage & "\tryagain.wav", "Try Again", 10, 4, "A")
                        'ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\tryagain.wav", True, "A"))
                    End If
                    GoTo GetATMPIN1Again
                Else
                    GoTo MaxTries
                End If
            End If
            If (Len(sInputs) <> 4) Or (IsNumeric(sInputs) = False) Then
                Application.DoEvents()
                If Not _TerminalServerSession Then
                    updateStatusText("Playing Invalid Lenght (ct=" & iCurrentTryCount & ")")
                    IVR_PlayFileAndGetDigits("\Sounds\" & sLanguage & "\ATMPINInvalidLength.wav", "Invalid Lenght", 10, 4, "A")
                    'ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\ATMPINInvalidLength.wav", True, "A"))
                End If

                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingenatm"":""Start"",""currenttry"":""" & iCurrentTryCount & """,""infoValidation"":""" & "Invalidlength" & """ }" & "}"
                sendTowsClinet(msgFromWSClinet)
                If iCurrentTryCount < iMaxInputTries Then
                    Application.DoEvents()
                    If Not _TerminalServerSession Then
                        updateStatusText("Playing try Again (ct=" & iCurrentTryCount & ")")
                        IVR_PlayFileAndGetDigits("\Sounds\" & sLanguage & "\tryagain.wav", "Try Again", 10, 4, "A")
                        'ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\tryagain.wav", True, "A"))
                    End If
                    GoTo GetATMPIN1Again
                Else
                    GoTo MaxTries
                End If
            ElseIf iCurrentTryCount > iMaxInputTries Then
MaxTries:
                Application.DoEvents()
                If Not _TerminalServerSession Then
                    updateStatusText("Maximum tries done")
                    IVR_PlayFileAndGetDigits("\Sounds\" & sLanguage & "\ATMPINMaxTries.wav", "Max tries done", 10, 4, "A")
                    'ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\ATMPINMaxTries.wav", True, "A"))
                End If
                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingenatm"":""Start"",""currenttry"":""" & iCurrentTryCount & """,""infoValidation"":""" & "Max Attempts Done" & """ }" & "}"
                sendTowsClinet(msgFromWSClinet)
                Exit Sub
            End If


            TPIN1 = sInputs
            iCurrentTryCountPIN2 = 0
GetTPIN2Again:
            iCurrentTryCountPIN2 = iCurrentTryCountPIN2 + 1
            sInputs = ""
            updateStatusText("ATMPIN 1, Collecting, ATMPIN 2, TRY=" & iCurrentTryCountPIN2)
            sInputs = ""
            msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingenatm"":""Start"",""infoValidation"":""" & "New ATMPIN Confirmation" & """ }" & "}"
            sendTowsClinet(msgFromWSClinet)
            '' ATMPIN1 Collected till here.

            'ovsControl.media_stopGetDigits(0, 0, CallInformation.nLineSessionID1)
            'ovsControl.media_stopPlay(0, 0, CallInformation.nLineSessionID1)

            Application.DoEvents()
            If Not _TerminalServerSession Then
                updateStatusText("Playing ATMPIN2")
                IVR_PlayFileAndGetDigits("\Sounds\" & sLanguage & "\ATMPINreEnter.wav", "ATM PIN 2", 21, 4, "*#0123456789")
                'ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\ATMPINreEnter.wav", True, "1234567890"))
            End If
            Application.DoEvents()
            'tmpstring = ovsControl.media_getDigits(New TControl.media_getDigits_Args(0, 0, CallInformation.nLineSessionID1, False, (4 - Len(sInputs)), 15, True))
            '_waitStartTime = DateTime.Now
            'While True
            'xDateDiff = DateDiff(DateInterval.Second, _waitStartTime, Now)
            'If CallInformation.CallStatus <> "Talking" Then
            'If ovsControl.Playing = True Then
            'ovsControl.media_stopPlay(0, 0, CallInformation.nLineSessionID1)
            'End If
            'Exit While
            'End If
            'If Len(sInputs) = 1 Then
            'If ovsControl.Playing = True Then
            'ovsControl.media_stopPlay(0, 0, CallInformation.nLineSessionID1)
            'End If
            'End If
            '
            'If xDateDiff >= 21 Then
            ''retVal = 0
            'Exit While
            'End If
            'If Len(sInputs) = 4 Then
            'Exit While
            'End If
            'Application.DoEvents()
            'Threading.Thread.Sleep(200)
            'updateStatusText("ATMPIN2 waiting :" & (4 - Len(sInputs)) & ", " & xDateDiff & ", ct=" & iCurrentTryCount & ", tm=" & DateTime.Now.ToString("ss:ffff").ToString())
            'End While
            Debug.WriteLine(DateTime.Now().ToString & " return after 4 Digits Y " & sInputs)

            If CallInformation.CallStatus <> "Talking" Then
                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingenatm"":""Start"",""infoValidation"":""" & "Not talking" & """ }" & "}"
                sendTowsClinet(msgFromWSClinet)
                Exit Sub
            End If

            If Len(sInputs) = 0 Then
                Application.DoEvents()
                If Not _TerminalServerSession Then
                    updateStatusText("Playing NoInput")
                    IVR_PlayFileAndGetDigits("\Sounds\" & sLanguage & "\ATMPINNoInput.wav", "No Input", 10, 4, "A")
                    'ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\ATMPINNoInput.wav", True, "A"))
                End If
                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingenatm"":""Start"",""currenttry"":""" & iCurrentTryCountPIN2 & """,""infoValidation"":""" & "NoInput" & """ }" & "}"
                sendTowsClinet(msgFromWSClinet)
                If iCurrentTryCountPIN2 < iMaxInputTries Then
                    Application.DoEvents()
                    If Not _TerminalServerSession Then
                        updateStatusText("Playing Try Again")
                        IVR_PlayFileAndGetDigits("\Sounds\" & sLanguage & "\tryagain.wav", "Try again", 10, 4, "A")
                        'ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\tryagain.wav", True, "A"))
                    End If
                    GoTo GetTPIN2Again
                Else
                    GoTo MaxTries
                End If
            End If
            If (Len(sInputs) <> 4) Or (IsNumeric(sInputs) = False) Then
                Application.DoEvents()
                If Not _TerminalServerSession Then
                    updateStatusText("Playing Invalid Length")
                    IVR_PlayFileAndGetDigits("\Sounds\" & sLanguage & "\ATMPINInvalidLength.wav", "Invalid lenght", 10, 4, "A")
                    'ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\ATMPINInvalidLength.wav", True, "A"))
                End If

                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingenatm"":""Start"",""currenttry"":""" & iCurrentTryCountPIN2 & """,""infoValidation"":""" & "Invalidlength" & """ }" & "}"
                sendTowsClinet(msgFromWSClinet)
                If iCurrentTryCountPIN2 < iMaxInputTries Then
                    Application.DoEvents()
                    If Not _TerminalServerSession Then
                        updateStatusText("Playing Try Again")
                        IVR_PlayFileAndGetDigits("\Sounds\" & sLanguage & "\tryagain.wav", "Try again", 10, 4, "A")
                        'ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\tryagain.wav", True, "A"))
                    End If
                    GoTo GetTPIN2Again
                Else
                    GoTo MaxTries
                End If
            End If
            If (sInputs <> TPIN1) Then
                updateStatusText("ATMPIN ATMPIN2 iMaxInputTries [" & iMaxInputTries & "] iCurrentTryCount [" & iCurrentTryCount & "]")
                If iCurrentTryCount < iMaxInputTries Then
                    Application.DoEvents()
                    If Not _TerminalServerSession Then
                        updateStatusText("Playing not same [" & iMaxInputTries & "] iCurrentTryCount [" & iCurrentTryCount & "]")
                        IVR_PlayFileAndGetDigits("\Sounds\" & sLanguage & "\ATMPINNotSame.wav", "PIN not same", 10, 4, "A")
                        'ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\ATMPINNotSame.wav", True, "A"))
                    End If
                    Application.DoEvents()
                    If Not _TerminalServerSession Then
                        updateStatusText("Playing Try Again [" & iMaxInputTries & "] iCurrentTryCount [" & iCurrentTryCount & "]")
                        IVR_PlayFileAndGetDigits("\Sounds\" & sLanguage & "\tryAgain.wav", "Try Again", 10, 4, "A")
                        'ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\tryAgain.wav", True, "A"))
                    End If
                    msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingenatm"":""Start"",""currenttry"":""" & iCurrentTryCount & """,""infoValidation"":""" & "PIN Not Same" & """ }" & "}"
                    sendTowsClinet(msgFromWSClinet)
                    GoTo GetATMPIN1Again
                Else
                    Application.DoEvents()
                    If Not _TerminalServerSession Then
                        updateStatusText("Playing PIN not same")
                        IVR_PlayFileAndGetDigits("\Sounds\" & sLanguage & "\ATMPINNotSame.wav", "PIN not same", 10, 4, "A")
                        'ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\ATMPINNotSame.wav", True, "A"))
                    End If

                    GoTo MaxTries
                End If
            End If

            Write_Log("", "ATMPIN ATMPIN2 also is good now, Calling ATMPIN Maker API")
            updateStatusText("Both PINs are good")

            msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingenatm"":""Start"",""infoValidation"":""" & "Calling API" & """ }" & "}"
            sendTowsClinet(msgFromWSClinet)


            '' call the API
            Dim MakeATMPINResponse As Boolean = False
            updateStatusText("Calling API")
            MakeATMPINResponse = MakeATMPIN(sInputs, sCardNo)

            If MakeATMPINResponse = True Then
                updateStatusText("API Response=" & MakeATMPINResponse)
                CallInformation.CallerVerifyed = 2
                Application.DoEvents()
                If Not _TerminalServerSession Then
                    IVR_PlayFileAndGetDigits("\Sounds\" & sLanguage & "\ATMPINDone.wav", "ATM PIN Done", 10, 4, "A")
                    'ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\ATMPINDone.wav", True, "A"))
                End If
                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingenatm"":""Start"",""infoValidation"":""" & "Done" & """ }" & "}"
                sendTowsClinet(msgFromWSClinet)
                updateStatusText("API Response=" & MakeATMPINResponse)
                Write_Log("", "IVR_ATMPINGen:ResponseSent:True:" & "" & " (" & msgFromWSClinet & ")")
            Else
                updateStatusText("API Response=" & MakeATMPINResponse & " - " & sTempResponseString)
                Application.DoEvents()
                If Not _TerminalServerSession Then
                    IVR_PlayFileAndGetDigits("\Sounds\" & sLanguage & "\ATMPINFailed.wav", "ATM PIN Failed", 10, 4, "A")
                    'ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\ATMPINFailed.wav", True, ""))
                End If
                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingenatm"":""Start"",""infoValidation"":""" & "Failed" & """ ,""InfoDescription"":""" & sTempResponseString & """ }" & "}"
                '& """ ,""ResponseString"":""" & sTempResponseString & """
                sendTowsClinet(msgFromWSClinet)
                Write_Log("", "IVR_ATMPINGen:ResponseSent:False:" & "" & " (" & msgFromWSClinet & ")")
                updateStatusText("API Response=" & MakeATMPINResponse & " - " & sTempResponseString)
            End If
        Catch ex As Exception

            msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingenatm"":""Start"",""infoValidation"":""" & "Failed" & """ ,""InfoDescription"":""" & "Exception:" & ex.Message & """ }" & "}"
            '& """ ,""ResponseString"":""" & sTempResponseString & """
            sendTowsClinet(msgFromWSClinet)

            'Try
            'ovsControl.media_stopPlay(0, 0, CallInformation.nLineSessionID1)
            'Catch exx As Exception
            'End Try
            ' Try
            'ovsControl.media_stopGetDigits(0, 0, CallInformation.nLineSessionID1)
            'Catch exx As Exception
            'End Try
            Write_Log("", "IVR_ATMPINGen:Error:" & "" & " (" & ex.Message & ")")
            updateStatusText("IVR_ATMPINGen:Error:" & "" & " (" & ex.Message & ")")
            'MessageBox.Show(Me, "IVR_ATMPINGen:" & "" & " (" & ex.Message & ")")
        End Try

    End Sub
    Private Sub IVR_TPINGen(cmd As String)
        Try
            Dim iCurrentTryCountPIN2 As Integer = 0

            IVRisWorking = True
            Dim msgFromWSClinet As String

            Write_Log("", "IVR_TPINGen:Start:" & cmd)

            Dim sNewCMDText As String
            Dim sNewCMDBroken() As String

            sNewCMDText = Replace(cmd, "cmd", "")
            sNewCMDText = Replace(sNewCMDText, "{", "")
            sNewCMDText = Replace(sNewCMDText, "}", "")
            sNewCMDBroken = Split(sNewCMDText, ",")
            Debug.Print("sNewCMDBroken.count = " & UBound(sNewCMDBroken))

            Dim tsString As String = ""
            msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingentpin"":""Start"",""infoValidation"":""" & "Start" & """ }" & "}"
            'Form_Main.sendTowsClinet msgFromWSClinet
            sendTowsClinet(msgFromWSClinet)

            If UBound(sNewCMDBroken) < 4 Then
                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingentpin"":""Start"",""infoValidation"":""" & "Packet Missing" & """ }" & "}"
                sendTowsClinet(msgFromWSClinet)
                Exit Sub
            End If

            Dim i As Integer
            For i = 0 To UBound(sNewCMDBroken) - 1
                If Len(Trim(sNewCMDBroken(i))) = 0 Then
                    msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingentpin"":""Start"",""infoValidation"":""" & "Empty or invalida packet" & """ }" & "}"
                    sendTowsClinet(msgFromWSClinet)
                    Exit Sub
                End If
            Next

            If IsNumeric(sNewCMDBroken(4)) = False Then
                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingentpin"":""Start"",""infoValidation"":""" & "CNIC Invalid " & sNewCMDBroken(4) & """ }" & "}"
                sendTowsClinet(msgFromWSClinet)
                Exit Sub
            End If

            If Len(sNewCMDBroken(4)) <> 13 Then
                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingentpin"":""Start"",""infoValidation"":""" & "CNIC Invalid " & sNewCMDBroken(4) & """ }" & "}"
                sendTowsClinet(msgFromWSClinet)
                Exit Sub
            End If

            CallInformation.CRMInteractionID = sNewCMDBroken(1)
            CallInformation.CRMSessionID = sNewCMDBroken(2)

            sLanguage = sNewCMDBroken(3)
            CallInformation.CustomerID = sNewCMDBroken(4)
            'Call Command_VerifyTPIN_Click
            Dim iCurrentTryCount As Integer = 0
            Dim iMaxInputTries As Integer = 3
            Dim TPIN1 As String = ""
            Dim xDateDiff As Integer = 0

GetTPIN1Again:
            Write_Log("", "TPIN entering in taking TPIN iCurrentTryCount=[" & iCurrentTryCount & "]")

            iCurrentTryCount = iCurrentTryCount + 1
            updateStatusText("Asking for PIN 1 try:" & iCurrentTryCount)
            ovsControl.media_stopGetDigits(0, 0, CallInformation.nLineSessionID1)
            ovsControl.media_stopPlay(0, 0, CallInformation.nLineSessionID1)
            Application.DoEvents()
            updateStatusText("Playing TPIN1")
            Application.DoEvents()
            sInputs = ""
            Application.DoEvents()
            If Not _TerminalServerSession Then
                ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\TPINNNewEnter.wav", True, "A"))
            End If
            sInputs = ""
            Application.DoEvents()
            Dim tmpstring As String = ovsControl.media_getDigits(New TControl.media_getDigits_Args(0, 0, CallInformation.nLineSessionID1, True, 4, 15, True))
            Dim _waitStartTime As DateTime = DateTime.Now
            While True
                If CallInformation.CallStatus <> "Talking" Then
                    Exit While
                End If
                If IVRisWorking = False Then
                    Exit While
                End If
                xDateDiff = DateDiff(DateInterval.Second, _waitStartTime, Now)

                If xDateDiff >= 15 Then
                    'retVal = 0
                    Exit While
                End If
                If Len(sInputs) = 4 Then
                    Exit While
                End If
                Application.DoEvents()
                Threading.Thread.Sleep(200)
                updateStatusText("TPIN1 waiting for digits :" & (4 - Len(sInputs)) & ", " & xDateDiff)
            End While

            Debug.WriteLine(DateTime.Now().ToString & " return after 4 Digits X " & sInputs)

            If CallInformation.CallStatus <> "Talking" Then
                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingentpin"":""Start"",""infoValidation"":""" & "Not talking" & """ }" & "}"
                sendTowsClinet(msgFromWSClinet)
                Exit Sub
            End If
            If Len(sInputs) = 0 Then
                Application.DoEvents()
                If Not _TerminalServerSession Then
                    ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\TPINNoInput.wav", True, "A"))
                End If
                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingentpin"":""Start"",""currenttry"":""" & iCurrentTryCount & """,""infoValidation"":""" & "NoInput" & """ }" & "}"
                sendTowsClinet(msgFromWSClinet)
                If iCurrentTryCount < iMaxInputTries Then
                    Application.DoEvents()
                    If Not _TerminalServerSession Then
                        ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\tryagain.wav", True, "A"))
                    End If
                    GoTo GetTPIN1Again
                Else
                    GoTo MaxTries
                End If
            End If
            If (Len(sInputs) <> 4) Or (IsNumeric(sInputs) = False) Then
                Application.DoEvents()
                If Not _TerminalServerSession Then
                    ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\TPINInvalidLength.wav", True, "A"))
                End If
                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingentpin"":""Start"",""currenttry"":""" & iCurrentTryCount & """,""infoValidation"":""" & "Invalidlength" & """ }" & "}"
                sendTowsClinet(msgFromWSClinet)
                If iCurrentTryCount < iMaxInputTries Then
                    Application.DoEvents()
                    If Not _TerminalServerSession Then
                        ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\tryagain.wav", True, "A"))
                    End If
                    GoTo GetTPIN1Again
                Else
                    GoTo MaxTries
                End If
            ElseIf iCurrentTryCount > iMaxInputTries Then
MaxTries:
                Application.DoEvents()
                If Not _TerminalServerSession Then
                    updateStatusText("Playing Maximum tries")
                    ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\TPINMaxTries.wav", True, "A"))
                End If
                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingentpin"":""Start"",""currenttry"":""" & iCurrentTryCount & """,""infoValidation"":""" & "Max Attempts Done" & """ }" & "}"
                sendTowsClinet(msgFromWSClinet)
                Exit Sub
            End If


            TPIN1 = sInputs

            iCurrentTryCountPIN2 = 0
GetTPIN2Again:
            iCurrentTryCountPIN2 = iCurrentTryCountPIN2 + 1
            sInputs = ""
            updateStatusText("TPIN 1, Collecting, TPIN 2, TRY=" & iCurrentTryCountPIN2)
            sInputs = ""
            msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingentpin"":""Start"",""infoValidation"":""" & "New TPIN Confirmation" & """ }" & "}"
            sendTowsClinet(msgFromWSClinet)
            '' TPIN1 Collected till here.

            ovsControl.media_stopGetDigits(0, 0, CallInformation.nLineSessionID1)
            ovsControl.media_stopPlay(0, 0, CallInformation.nLineSessionID1)
            updateStatusText("Playing TPIN2")

            Application.DoEvents()

            If Not _TerminalServerSession Then
                ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\TPINreEnter.wav", True, "A"))
            End If
            Application.DoEvents()
            sInputs = ""
            tmpstring = ovsControl.media_getDigits(New TControl.media_getDigits_Args(0, 0, CallInformation.nLineSessionID1, False, 4, 15, True))
            _waitStartTime = DateTime.Now
            While True
                If CallInformation.CallStatus <> "Talking" Then
                    Exit While
                End If
                If IVRisWorking = False Then
                    Exit While
                End If
                xDateDiff = DateDiff(DateInterval.Second, _waitStartTime, Now)
                If xDateDiff >= 15 Then
                    'retVal = 0
                    Exit While
                End If
                If Len(sInputs) = 4 Then
                    Exit While
                End If
                Application.DoEvents()
                Threading.Thread.Sleep(200)
                updateStatusText("TPIN2 waiting for digits :" & (4 - Len(sInputs)) & ", " & xDateDiff)
            End While
            Debug.WriteLine(DateTime.Now().ToString & " return after 4 Digits Y " & sInputs)

            If CallInformation.CallStatus <> "Talking" Then
                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingentpin"":""Start"",""infoValidation"":""" & "Not talking" & """ }" & "}"
                sendTowsClinet(msgFromWSClinet)
                Exit Sub
            End If

            If Len(sInputs) = 0 Then
                Application.DoEvents()
                If Not _TerminalServerSession Then
                    ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\TPINNoInput.wav", True, "A"))
                End If
                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingentpin"":""Start"",""currenttry"":""" & iCurrentTryCountPIN2 & """,""infoValidation"":""" & "NoInput" & """ }" & "}"
                sendTowsClinet(msgFromWSClinet)
                If iCurrentTryCountPIN2 < iMaxInputTries Then
                    Application.DoEvents()
                    If Not _TerminalServerSession Then
                        ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\tryagain.wav", True, "A"))
                    End If
                    GoTo GetTPIN2Again
                Else
                    GoTo MaxTries
                End If
            End If
            If (Len(sInputs) <> 4) Or (IsNumeric(sInputs) = False) Then
                Application.DoEvents()
                If Not _TerminalServerSession Then
                    ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\TPINInvalidLength.wav", True, "A"))
                End If
                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingentpin"":""Start"",""currenttry"":""" & iCurrentTryCountPIN2 & """,""infoValidation"":""" & "Invalidlength" & """ }" & "}"
                sendTowsClinet(msgFromWSClinet)
                If iCurrentTryCountPIN2 < iMaxInputTries Then
                    Application.DoEvents()
                    If Not _TerminalServerSession Then
                        ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\tryagain.wav", True, "A"))
                    End If
                    GoTo GetTPIN2Again
                Else
                    GoTo MaxTries
                End If
            End If
            If (sInputs <> TPIN1) Then
                updateStatusText("TPIN TPIN2 iMaxInputTries [" & iMaxInputTries & "] iCurrentTryCount [" & iCurrentTryCount & "]")
                If iCurrentTryCount < iMaxInputTries Then
                    Application.DoEvents()
                    If Not _TerminalServerSession Then
                        ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\TPINNotSame.wav", True, "A"))
                    End If
                    Application.DoEvents()
                    If Not _TerminalServerSession Then
                        ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\tryAgain.wav", True, "A"))
                    End If
                    msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingentpin"":""Start"",""currenttry"":""" & iCurrentTryCount & """,""infoValidation"":""" & "PIN Not Same" & """ }" & "}"
                    sendTowsClinet(msgFromWSClinet)
                    GoTo GetTPIN1Again
                Else
                    Application.DoEvents()
                    If Not _TerminalServerSession Then
                        ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\TPINNotSame.wav", True, "A"))
                    End If
                    GoTo MaxTries
                End If
            End If

            Write_Log("", "TPIN TPIN2 also is good now, Calling TPIN Maker API")
            updateStatusText("Both PINs are good")

            msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingentpin"":""Start"",""infoValidation"":""" & "Calling API" & """ }" & "}"
            sendTowsClinet(msgFromWSClinet)


            '' call the API
            Dim MakeTPINResponse As Boolean = False
            updateStatusText("Calling API")
            MakeTPINResponse = MakeTPIN(sInputs, False)

            If MakeTPINResponse = True Then
                updateStatusText("API Response=" & MakeTPINResponse)
                CallInformation.CallerVerifyed = 2
                Application.DoEvents()
                If Not _TerminalServerSession Then
                    ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\TPINDone.wav", True, "A"))
                End If
                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingentpin"":""Start"",""infoValidation"":""" & "Done" & """ }" & "}"
                sendTowsClinet(msgFromWSClinet)
                Write_Log("", "IVR_TPINGen:ResponseSent:True:" & "" & " (" & msgFromWSClinet & ")")
            Else
                updateStatusText("API Response=" & MakeTPINResponse)
                Application.DoEvents()
                If Not _TerminalServerSession Then
                    ovsControl.media_play(New ovsPSProPhony.TControl.media_play_Args(0, 0, CallInformation.nLineSessionID1, False, "\Sounds\" & sLanguage & "\TPINFailed.wav", True, "A"))
                End If
                msgFromWSClinet = "{" & """cmd"":{""ts"":""" & tsString & """,""pingentpin"":""Start"",""infoValidation"":""" & "Failed" & """ ,""InfoDescription"":""" & sTempResponseString & """ }" & "}"
                sendTowsClinet(msgFromWSClinet)
                Write_Log("", "IVR_TPINGen:ResponseSent:False:" & "" & " (" & msgFromWSClinet & ")")
            End If

        Catch ex As Exception
            Write_Log("", "IVR_TPINGen:Error:" & "" & " (" & ex.Message & ")")
            updateStatusText("IVR_TPINGen:Error:" & "" & " (" & ex.Message & ")")
        End Try

    End Sub


    Private Function MakeTPIN(TPIN As String, Optional Verify As Boolean = False) As Boolean
        MakeTPIN = False
        sTempResponseString = "" & "" & ""

        Try
            MakeTPIN = False
            Dim xTimeOut As Long
            xTimeOut = 5
            Dim sTmpString As String
            If Verify = True Then
                sTmpString = Trim(GetINIString("sgAPI", "APIVerifyTPINTimout", "5"))
            Else
                sTmpString = Trim(GetINIString("sgAPI", "APICreateTPINTimout", "5"))
            End If
            If IsNumeric(sTmpString) = True Then
                xTimeOut = Val(sTmpString)
            End If
            Write_Log("", "MakeTPIN:Not Nothing,xTimeOut=" & xTimeOut)

            Dim sgAPIURL As String
            If Verify = True Then
                sgAPIURL = GetINIString("sgAPI", "APIVerifyTPIN", "http://18.188.178.46:8100" & "/api/CustomerPINValidation/pin/verfyTelPin")
            Else
                sgAPIURL = GetINIString("sgAPI", "APICreateTPIN", "http://18.188.178.46:8100" & "/api/CustomerPINValidation/pin/createTelPin")
            End If
            Dim sgAPITOKEN As String
            sgAPITOKEN = GetINIString("sgAPI", "APITOKEN", "TOKEN")
            Write_Log("", "MakeTPIN,xTimeOut=" & xTimeOut & ",sgAPIURL=" & sgAPIURL)
            Dim tmpText As String = ""

            If Verify = True Then
                Write_Log("", "VeryfyTPIN:URL=" & sgAPIURL & ", TimeOut=" & xTimeOut & ", sgAPITOKEN=")
            Else
                Write_Log("", "MakeTPIN:URL=" & sgAPIURL & ", TimeOut=" & xTimeOut & ", sgAPITOKEN=")
            End If
            'On Error Resume Next
            If Verify = True Then
                Dim APIClient As New APIConnector.Connector(LogDir, 1, "", "", "", 0, "sgBOK")
                Try
                    sTempResponseString = "" & "" & ""
                    tmpText = APIClient.sgBOK_TPINValidate(sgAPIURL, sgAPITOKEN, CallInformation.CRMInteractionID, CallInformation.CRMSessionID, CallInformation.RefID_DB, CallInformation.CalledID, AgentInformation.LoginName, CallInformation.CustomerID, TPIN, xTimeOut)
                    'tmpText = APIClient.sgBOK_TPINValidate(sgAPIURL, sgAPITOKEN, CallInformation.CRMInteractionID, CallInformation.CRMSessionID, CallInformation.RefID_DB, CallInformation.CalledID, "Agent-" & AgentInformation.AgentID, CallInformation.CustomerID, TPIN, xTimeOut)
                    sTempResponseString = "" & tmpText & ""

                Catch ex As Exception
                    sTempResponseString = "" & ex.Message & ""
                    tmpText = "ex=" & ex.Message
                End Try
            Else
                Dim APIClient As New APIConnector.Connector(LogDir, 1, "", "", "", 0, "sgBOK")
                Try
                    sTempResponseString = "" & "" & ""

                    tmpText = APIClient.sgBOK_TPINGenerate(sgAPIURL, sgAPITOKEN, CallInformation.CRMInteractionID, CallInformation.CRMSessionID, CallInformation.RefID_DB, CallInformation.CalledID, AgentInformation.LoginName, CallInformation.CustomerID, TPIN, xTimeOut)
                    'tmpText = APIClient.sgBOK_TPINGenerate(sgAPIURL, sgAPITOKEN, CallInformation.CRMInteractionID, CallInformation.CRMSessionID, CallInformation.RefID_DB, CallInformation.CalledID, "Agent-" & AgentInformation.AgentID, CallInformation.CustomerID, TPIN, xTimeOut)
                    sTempResponseString = "" & tmpText & ""
                Catch ex As Exception
                    sTempResponseString = "" & ex.Message & ""
                    tmpText = "ex=" & ex.Message
                End Try
            End If
            If tmpText <> "200" Then
                Write_Log("", "MakeTPIN:Error:" & tmpText & ",tmpString=" & tmpText & ", TimeOut=" & xTimeOut & ", Interaction=" & CallInformation.CRMInteractionID & ", CRMSessionID=" & CallInformation.CRMSessionID & ", sTempResponseString=" & sTempResponseString)
                MakeTPIN = False
            Else
                If Verify = True Then
                    Write_Log("", "MakeTPIN:Verifyed:" & tmpText & ",tmpString=" & tmpText & ", TimeOut=" & xTimeOut & ", Interaction=" & CallInformation.CRMInteractionID & ", CRMSessionID=" & CallInformation.CRMSessionID & ", sTempResponseString=" & sTempResponseString)
                    updateStatusText("TPIN Verifyed")
                Else
                    Write_Log("", "MakeTPIN:Created:" & tmpText & ",tmpString=" & tmpText & ", TimeOut=" & xTimeOut & ", Interaction=" & CallInformation.CRMInteractionID & ", CRMSessionID=" & CallInformation.CRMSessionID & ", sTempResponseString=" & sTempResponseString)
                    updateStatusText("TPIN Created")
                End If
                CallInformation.CallerVerifyed = 3
                MakeTPIN = True
            End If

        Catch ex As Exception
            Write_Log("", "MakeTPIN:Exception:" & "" & " (" & ex.Message & ")")
            'MessageBox.Show(Me, "MakeTPIN:" & "" & " (" & ex.Message & ")")
        End Try
    End Function
    Private Function MakeATMPIN(ATMPIN As String, CardNo As String) As Boolean
        MakeATMPIN = False
        sTempResponseString = "" & "" & ""
        Try
            MakeATMPIN = False
            Dim xTimeOut As Long
            xTimeOut = 5
            Dim sTmpString As String
            sTmpString = Trim(GetINIString("sgAPI", "APICreateATMPINTimout", "5"))
            If IsNumeric(sTmpString) = True Then
                xTimeOut = Val(sTmpString)
            End If
            Write_Log("", "MakeATMPIN,xTimeOut=" & xTimeOut)

            Dim sgAPIURL As String
            sgAPIURL = GetINIString("sgAPI", "APICreateATMPIN", "http://18.188.178.46:8100" & "/api/CustomerPINValidation/pin/createTelPin")
            Dim sgAPITOKEN As String
            sgAPITOKEN = GetINIString("sgAPI", "APITOKEN", "TOKEN")
            Write_Log("", "MakeATMPIN:Not Nothing,xTimeOut=" & xTimeOut & ",sgAPIURL=" & sgAPIURL)
            Dim tmpText As String = ""

            Write_Log("", "MakeATMPIN:URL=" & sgAPIURL & ", TimeOut=" & xTimeOut & ", sgAPITOKEN=")
            'On Error Resume Next

            Dim APIClient As New APIConnector.Connector(LogDir, 1, "", "", "", 0, "sgBOK")
            Try
                sTempResponseString = "" & "" & ""
                updateStatusText("Calling API Timeout(" & xTimeOut & ")")
                tmpText = APIClient.sgBOK_ATMPINGenerate(sgAPIURL, sgAPITOKEN, CallInformation.CRMInteractionID, CallInformation.CRMSessionID, CallInformation.RefID_DB, CallInformation.CalledID, AgentInformation.LoginName, CallInformation.CustomerID, CardNo, ATMPIN, xTimeOut)
                'tmpText = APIClient.sgBOK_ATMPINGenerate(sgAPIURL, sgAPITOKEN, CallInformation.CRMInteractionID, CallInformation.CRMSessionID, CallInformation.RefID_DB, CallInformation.CalledID, "Agent-" & AgentInformation.AgentID, CallInformation.CustomerID, CardNo, ATMPIN, xTimeOut)
                sTempResponseString = "" & tmpText & ""
            Catch ex As Exception
                tmpText = "ex=" & ex.Message
                sTempResponseString = "" & ex.Message & ""
            End Try
            If tmpText <> "200" Then
                Write_Log("", "MakeATMPIN:Error:" & tmpText & ",tmpString=" & tmpText & ", TimeOut=" & xTimeOut & ", Interaction=" & CallInformation.CRMInteractionID & ", CRMSessionID=" & CallInformation.CRMSessionID & ", sTempResponseString=" & sTempResponseString)
                updateStatusText("ATMPIN not created")
            Else
                Write_Log("", "MakeATMPIN:Created:" & tmpText & ",tmpString=" & tmpText & ", TimeOut=" & xTimeOut & ", Interaction=" & CallInformation.CRMInteractionID & ", CRMSessionID=" & CallInformation.CRMSessionID & ", sTempResponseString=" & sTempResponseString)
                updateStatusText("ATMPIN Created")
                'CallInformation.CallerVerifyed = 3
                MakeATMPIN = True
            End If

        Catch ex As Exception
            Write_Log("", "MakeATMPIN:Error:" & "" & " (" & ex.Message & ")")
            'MessageBox.Show(Me, "MakeATMPIN:" & "" & " (" & ex.Message & ")")
        End Try
    End Function

#End Region

    Private Sub Button_Call_Trans_Click(sender As Object, e As EventArgs) Handles Button_Call_Trans.Click
        Try
            'updateStatusText("StreamWriterStatus:" & ovsControl.StreamWriterStatus(0))
            'ovsControl.StreamWriterStatus(0) = False
        Catch ex As Exception
            Write_Log("", "Call_Trans:Error:" & ex.Message)
            'MessageBox.Show(Me, "Call_Trans:Error:" & ex.Message)
        End Try
    End Sub

    Private Sub ovsControl_Event_Error(Sender As Object, e As TControl.Event_Error_Args) Handles ovsControl.Event_Error
        updateStatusText("Event_Error:" & e.ex.Message)
        Write_Log("", "Event_Error:" & e.ex.Message)
    End Sub

    Private Sub Button_WCClose_Click(sender As Object, e As EventArgs) Handles Button_WCClose.Click

    End Sub

    Private Sub Button_Call_Conf_Click(sender As Object, e As EventArgs) Handles Button_Call_Conf.Click

    End Sub

    Private Sub Form_Splash_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        Try
            'If wsClientAllowed = True Then
            If Me.WindowState = FormWindowState.Minimized Then
                If Configurations.bDisableTrayICON = False Then
                    NotifyIcon1.Visible = True
                    ShowInTaskbar = False
                End If
            End If
            'End If

        Catch ex As Exception
            Write_Log("", "Splash_Resize(Error) nLine:" & CallInformation.nLine & "" & ex.Message)
            'MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub NotifyIcon1_DoubleClick(sender As Object, e As EventArgs) Handles NotifyIcon1.DoubleClick
        Try
            Write_Log("", "NotifyIcon1_DoubleClick() nLine:" & CallInformation.nLine & ", " & Configurations.bDisableTrayICON & "=True")

            If Configurations.bDisableTrayICON = True Then
                NotifyIcon1.BalloonTipText = ""
                NotifyIcon1.Visible = False
                Me.TopMost = True
                Application.DoEvents()
                Me.TopMost = False
                Me.WindowState = FormWindowState.Normal
            End If
        Catch ex As Exception
            Write_Log("", "NotifyIcon1_DoubleClick(Error) nLine:" & CallInformation.nLine & "" & ex.Message)
        End Try
    End Sub

    Private Sub Button_AudioSetup_Click(sender As Object, e As EventArgs) Handles Button_AudioSetup.Click
        Try
            Write_Log("", "Process_AudioSetup(Start) nLine:" & CallInformation.nLine)
            Dim tmpstring As String
            Form_AudioSetup.Show()
            Form_AudioSetup.ComboBox_Mic.Items.Clear()
            Form_AudioSetup.ComboBox_Speaker.Items.Clear()

            Dim tmpTotalSpeakers As Long = ovsControl.getNumSpeakers
            Dim SpeakerIndexToRemmember As Integer = 0
            'Dim TmpString As String = ""
            For index = 0 To tmpTotalSpeakers - 1
                tmpstring = ovsControl.getSpeaker(index)
                Form_AudioSetup.ComboBox_Speaker.Items.Insert(index, tmpstring)
            Next
            If Form_AudioSetup.ComboBox_Speaker.Items.Count > 0 Then
                Form_AudioSetup.ComboBox_Speaker.Text = Form_AudioSetup.ComboBox_Speaker.Items(0)
            End If
            tmpstring = ""
            Dim tmpTotalMICs As Long = ovsControl.getNumMicrophones

            Dim MICIndexToRemmember As Integer = 0
            For index = 0 To tmpTotalMICs - 1
                tmpstring = ovsControl.getMicrophones(index)
                Form_AudioSetup.ComboBox_Mic.Items.Insert(index, tmpstring)
            Next
            If Form_AudioSetup.ComboBox_Mic.Items.Count > 0 Then
                Form_AudioSetup.ComboBox_Mic.Text = Form_AudioSetup.ComboBox_Mic.Items(0)
            End If

            tmpstring = ""
            Form_AudioSetup.Label_msg.Text = ""
            Write_Log("", "Process_AudioSetup(End) nLine:" & CallInformation.nLine)
        Catch ex As Exception
            Write_Log("", "Process_AudioSetup(Error) nLine:" & CallInformation.nLine & "" & ex.Message)
            'MessageBox.Show(Me, "Process_AudioSetup(Error) nLine:" & CallInformation.nLine & "" & ex.Message)
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            StartStopRecording(False, _tmpsessionID)
        Catch ex As Exception
            Write_Log("", "Button2_Click(Error) nLine:" & CallInformation.nLine & "" & ex.Message)
            'MsgBox(ex)
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            StartStopRecording(True)
        Catch ex As Exception
            Write_Log("", "Button3_Click(Error) nLine:" & CallInformation.nLine & "" & ex.Message)
            'MsgBox(ex)
        End Try

    End Sub

    Private Sub TextBox_LoginName_TextChanged(sender As Object, e As EventArgs) Handles TextBox_LoginName.TextChanged

    End Sub

    Private Sub TextBox_LoginName_GotFocus(sender As Object, e As EventArgs) Handles TextBox_LoginName.GotFocus
        Try
            Debug.WriteLine("TextBox_LoginName_GotFocus()")
        Catch ex As Exception
        End Try
    End Sub

    Private Sub GroupBox_Login_Enter(sender As Object, e As EventArgs) Handles GroupBox_Login.Enter
        Try
            Debug.WriteLine("GroupBox_Login_Enter()")
        Catch ex As Exception
        End Try
    End Sub

    Private Sub GroupBox_Login_GotFocus(sender As Object, e As EventArgs) Handles GroupBox_Login.GotFocus
        Try
            Debug.WriteLine("GroupBox_Login_GotFocus()")
        Catch ex As Exception
        End Try
    End Sub

    Private Sub TextBox_Number_DoubleClick(sender As Object, e As EventArgs) Handles TextBox_Number.DoubleClick
        Try
            uCallback.SetupEvent(-1, "CheckNewEvents", 1000, -1, -1, -1, Nothing)
            Debug.WriteLine("DoubleClick enabled")
        Catch ex As Exception
            Debug.WriteLine("DoubleClick Exception:" & ex.Message)
        End Try
    End Sub

    Private Sub Label_Status_DoubleClick(sender As Object, e As EventArgs) Handles Label_Status.DoubleClick
        Try
            Shell("explorer " & LogDir, AppWinStyle.NormalFocus)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Label_Version_Click(sender As Object, e As EventArgs) Handles Label_Version.Click

    End Sub
#End Region
#Region "Events_NWSS"
    Private Sub wSSNativeHost_Event_Connection_Received(Sender As Object, e As Class_MSWS.Event_Connection_Received_Args) Handles wSSNativeHost.Event_Connection_Received
        Try
            Dim iCount As Integer = e.iCount
            Try
                Dim conn = New Connection With {.Context = Nothing}
                ' Add a connection Object to thread-safe collection
                Call OnlineConnections.TryAdd(e.sKey.ToString, conn)
            Catch ex As Exception
            End Try

            wsMessages.Add("OnOpen~~~" & e.sKey.ToString() & "~~~" & iCount.ToString())
            'Timer_wsReader_Timer("OnOpen~~~" & e.sKey.ToString() & "~~~" & iCount.ToString())

            'Write_Log("", "OnConnect(" & iCount & ") CountBefore=" & iCount & ", CountAfter=" & iCount & ", Key=" & e.sKey)
        Catch ex As Exception
            Write_Log("", "wSSNativeHost_Event_Connection(Error) " & ex.Message)
        End Try
    End Sub
    Private Sub wSSNativeHost_Event_Data_Received(Sender As Object, e As Class_MSWS.Event_Data_Received_Args) Handles wSSNativeHost.Event_Data_Received
        Try
            Dim msg As String = ""
            Dim msgfrom As String = ""

            msg = e.sKey.ToString()
            msgfrom = e.sData
            If Trim(msg) <> "" Then
                wsMessages.Add("OnMessage~~~" & msg & "~~~" & msgfrom)
                'Timer_wsReader_Timer("OnMessage~~~" & msg & "~~~" & msgfrom)
            End If
        Catch ex As Exception
            Write_Log("", "wSSNativeHost_Event_Data(Error) " & ex.Message)
        End Try

    End Sub
    Private Sub wSSNativeHost_Event_Disconnection_Received(Sender As Object, e As Class_MSWS.Event_Disconnection_Received_Args) Handles wSSNativeHost.Event_Disconnection_Received
        Try
            Dim iCount As Integer = e.iCount

            Try
                Dim conn = New Connection With {.Context = Nothing}
                Call OnlineConnections.TryRemove(e.sKey.ToString, conn)
            Catch ex As Exception
            End Try

            wsMessages.Add("OnClose~~~" & e.sKey & "~~~" & iCount)
            Timer_wsReader_Timer("OnClose~~~" & e.sKey & "~~~" & iCount)

        Catch ex As Exception
            Write_Log("", "wSSNativeHost_Event_Disconnection(Error) " & ex.Message)
        End Try
    End Sub
    Private Sub wSSNativeHost_Event_Exception_Received(Sender As Object, e As Class_MSWS.Event_Exception_Args) Handles wSSNativeHost.Event_Exception_Received
        Try
            Dim iCount As Integer = wSSNativeHost.Connections
            wsMessages.Add("Exception~~~" & e.sKey & "~~~" & iCount & "~~~" & e.sException)
            'Timer_wsReader_Timer("OnException~~~" & e.sKey & "~~~" & iCount & "~~~" & e.sException)
        Catch ex As Exception
            Write_Log("", "wSSNativeHost_Event_Exception()" & ", sKey=" & e.sKey & ", sFunction=" & e.sFunction & ", sException=" & e.sException & ", Key=" & e.sKey & ", sException=" & e.sException)
        End Try

    End Sub
    Private Sub sendTowsClinet(msgtoSend As String, Optional connectionID As String = "")
        Try
            If Configurations.UseNativeWS = False Then
                sendTowsClinetViaAlchemy(msgtoSend, connectionID)
            Else
                Write_Log("", "sending: to Connections=" & wSSNativeHost.Connections & " (" & msgtoSend & ")")
                wSSNativeHost.SendToAll(msgtoSend)
                Write_Log("", "   sent: to Connections=" & wSSNativeHost.Connections & " (" & msgtoSend & ")")
            End If
        Catch ex As Exception
            Write_Log("", "sendTowsClinet:Error:" & ex.Message & " (" & msgtoSend & ")")
        End Try
    End Sub
    Private Sub sendTowsClinetViaAlchemy(msgtoSend As String, Optional connectionID As String = "")
        If wsClientAllowed = False Then Exit Sub
sendAgain:

        Try
            Dim conn As Connection
            Dim connectionCount As Long = 0
            If Trim(connectionID) <> "" Then
                Try
                    conn = OnlineConnections.Item(connectionID)
                    conn.wssend(msgtoSend)
                Catch ex As Exception
                    Write_Log("", "sendTowsClinetViaAlchemy:Sending YException(" & connectionCount & "):" & msgtoSend & ")")
                End Try
            Else
                For Each item In OnlineConnections
                    Try
                        conn = item.Value
                        conn.wssend(msgtoSend)
                        connectionCount = connectionCount + 1
                    Catch ex As Exception
                        Write_Log("", "sendTowsClinetViaAlchemy:Sending LException(" & connectionCount & "):" & msgtoSend & ")")
                    End Try
                    Threading.Thread.Sleep(20)
                    Application.DoEvents()
                Next
            End If
            Write_Log("", "sendTowsClinetViaAlchemy:sent(" & connectionCount & "):" & msgtoSend & ")")
        Catch ex As Exception
            Write_Log("", "sendTowsClinetViaAlchemy:Error:" & ex.Message & " (" & msgtoSend & ")")
        End Try
    End Sub

    Private Sub Form_Splash_AutoSizeChanged(sender As Object, e As EventArgs) Handles Me.AutoSizeChanged

    End Sub

    Private Sub Form_Splash_ResizeBegin(sender As Object, e As EventArgs) Handles Me.ResizeBegin

    End Sub

    Private Sub Label_Status_Click(sender As Object, e As EventArgs) Handles Label_Status.Click

    End Sub

#End Region

End Class
