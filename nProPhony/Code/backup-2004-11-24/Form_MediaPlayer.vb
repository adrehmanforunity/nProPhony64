'Test Comment by atif Player
'Test Comment by atif Player

Imports System.IO
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Window
Imports AxWMPLib
Imports NAudio.Lame
Imports NAudio.Wave
Imports NAudio.Wave.Compression
Imports NAudio.Wave.SampleProviders
Imports TagLib
Imports TagLib.Riff
'Imports Microsoft.DirectX.AudioVideoPlayback




Public Class Form_MediaPlayer
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            dbClass.Activity_Get_SessionID("RecordingSearch")
            LoadData(2)
            dbClass.Activity_Set_SessionID("RecordingSearch")
        Catch ex As Exception
            MessageBox.Show("Exception:" & ex.Message)
        End Try
    End Sub
    Private Sub LoadAgents()

        Try

            Combo_Users.Items.Clear()

            Dim dt As DataTable

            dt = dbClass.RunDBQuery("SELECT RowID, LoginName, FirstName, LastName FROM TBL_Agents where status = 1  order by FirstName,LastName")
            'dt = dbClass.RunDBQuery("SELECT distinct(FirstName) as FirstName FROM TBL_Agents where status = 1  group by FirstName,LastName")

            Combo_Users.Items.Add("*")

            If dt.Rows.Count > 0 Then
                Label_Status.Text = dt.Rows.Count & ", Record(s) found"
                Application.DoEvents()
                For index = 0 To dt.Rows.Count - 1
                    Try
                        Combo_Users.Items.Add(dt.Rows(index).Item("FirstName"))
                    Catch ex As Exception
                    End Try
                Next
            Else
                Label_Status.Text = "No, Record(s) found"
                Application.DoEvents()
            End If

            Combo_Users.SelectedIndex = 0
            Exit Sub

        Catch ex As Exception
            MessageBox.Show(ex.Message)

        End Try

    End Sub
    Private Sub LoadData(ByVal RecType As Integer)
        Dim xCaption As String = Me.Text
        Try

            Dim where As String = ""
            Me.Enabled = False

            Dim CLI As String = ""
            Dim Direction As Integer

            DataGridView1.Rows.Clear()
            Label7.Text = "Found: 0"
            If TextBox_CLI.Text = "" Then
                TextBox_CLI.Text = "*"
            End If



            If Combo_Users.Text <> "*" Then
                where = where & "AND TBL_Agents.FirstName = '" & Combo_Users.Text & "'"
            End If

            If Combo_Type.Text <> "*" Then
                If Combo_Type.Text = "In-Bound" Then
                    Direction = 0
                ElseIf Combo_Type.Text = "Out-Bound" Then
                    Direction = 1
                End If

                where = where & " AND TBL_AgentCalls.Direction = " & Direction
            End If

            If TextBox_CLI.Text <> "*" Then
                If Direction = 0 Then
                    where = where & "AND TBL_AgentCalls.CallerID = '" & TextBox_CLI.Text & "'"
                Else
                    where = where & "AND TBL_AgentCalls.CalledID = '" & TextBox_CLI.Text & "'"
                End If
            End If

            If TextBox_Limit.Text = "" Then TextBox_Limit.Text = "100"

            '      where = where & " LIMIT " & TextBox_Limit.Text

            Me.Text = xCaption & " Wait..."
            Application.DoEvents()
            Me.Enabled = False

            Label_Status.Text = "Please wait, let me find"
            Application.DoEvents()
            Dim TmpQueryString As String

            If RecType = 1 Then
                TmpQueryString = "SELECT TOP " & TextBox_Limit.Text & " TBL_AgentCalls.RowID as CallID,TBL_AgentLoginSessions.AgentID,TBL_Agents.RowID AS LoginID,TBL_Agents.Ext AS Ext,TBL_Agents.LoginName,TBL_Agents.FirstName,TBL_Agents.LastName,TBL_AgentCalls.DateTimeAccpeted,TBL_AgentCalls.DateTimeHangup, DateDiff(second,TBL_AgentCalls.DateTimeAccpeted,TBL_AgentCalls.DateTimeHangup) As CallDuration,TBL_Agents.Ext,TBL_AgentLoginSessions.RowID,TBL_AgentCalls.Direction,TBL_AgentCalls.CallerID,TBL_AgentCalls.CalledID,TBL_AgentCalls.DateTimeCall,TBL_AgentCalls.DateTimeHangup,TBL_AgentCalls.isVerifyed,TBL_AgentCalls.CallStatus from TBL_AgentCalls LEFT JOIN TBL_AgentLoginSessions ON TBL_AgentLoginSessions.RowID = TBL_AgentCalls.LoginSessionID LEFT JOIN TBL_Agents ON TBL_Agents.RowID = TBL_AgentLoginSessions.AgentID Where TBL_AgentCalls.DateTimeCall BETWEEN '" & DateTimePicker_Start.Text & "' AND '" & DateTimePicker_End.Text & "' AND TBL_AgentCalls.isAnswered= 0 AND DateTimeHangup IS NOT NULL " & where & " ORDER BY TBL_AgentCalls.DateTimeCall decs"
            ElseIf RecType = 2 Then
                TmpQueryString = "SELECT TOP " & TextBox_Limit.Text & " TBL_AgentCalls.RowID as CallID,TBL_AgentLoginSessions.AgentID,TBL_Agents.RowID AS LoginID,TBL_Agents.Ext AS Ext,TBL_Agents.LoginName,TBL_Agents.FirstName,TBL_Agents.LastName,TBL_AgentCalls.DateTimeAccpeted,TBL_AgentCalls.DateTimeHangup, DateDiff(second,TBL_AgentCalls.DateTimeAccpeted,TBL_AgentCalls.DateTimeHangup) As CallDuration,TBL_Agents.Ext,TBL_AgentLoginSessions.RowID,TBL_AgentCalls.Direction,TBL_AgentCalls.CallerID,TBL_AgentCalls.CalledID,TBL_AgentCalls.DateTimeCall,TBL_AgentCalls.DateTimeHangup,TBL_AgentCalls.isVerifyed,TBL_AgentCalls.CallStatus from TBL_AgentCalls LEFT JOIN TBL_AgentLoginSessions ON TBL_AgentLoginSessions.RowID = TBL_AgentCalls.LoginSessionID LEFT JOIN TBL_Agents ON TBL_Agents.RowID = TBL_AgentLoginSessions.AgentID Where TBL_AgentCalls.DateTimeCall BETWEEN '" & DateTimePicker_Start.Text & "' AND '" & DateTimePicker_End.Text & "' AND TBL_AgentCalls.isAnswered= 1 AND DateTimeHangup IS NOT NULL " & where & " ORDER BY TBL_AgentCalls.DateTimeCall desc "
            Else
                TmpQueryString = "SELECT TOP " & TextBox_Limit.Text & " TBL_AgentCalls.RowID as CallID,TBL_AgentLoginSessions.AgentID,TBL_Agents.RowID AS LoginID,TBL_Agents.Ext AS Ext,TBL_Agents.LoginName,TBL_Agents.FirstName,TBL_Agents.LastName,TBL_AgentCalls.DateTimeAccpeted,TBL_AgentCalls.DateTimeHangup, DateDiff(second,TBL_AgentCalls.DateTimeAccpeted,TBL_AgentCalls.DateTimeHangup) As CallDuration,TBL_Agents.Ext,TBL_AgentLoginSessions.RowID,TBL_AgentCalls.Direction,TBL_AgentCalls.CallerID,TBL_AgentCalls.CalledID,TBL_AgentCalls.DateTimeCall,TBL_AgentCalls.DateTimeHangup,TBL_AgentCalls.isVerifyed,TBL_AgentCalls.CallStatus from TBL_AgentCalls LEFT JOIN TBL_AgentLoginSessions ON TBL_AgentLoginSessions.RowID = TBL_AgentCalls.LoginSessionID LEFT JOIN TBL_Agents ON TBL_Agents.RowID = TBL_AgentLoginSessions.AgentID Where TBL_AgentCalls.DateTimeCall BETWEEN '" & DateTimePicker_Start.Text & "' AND '" & DateTimePicker_End.Text & "' AND DateTimeHangup IS NOT NULL " & where & " ORDER BY TBL_AgentCalls.DateTimeCall desc"
            End If

            Dim dt As DataTable
            Debug.WriteLine("TmpQueryString:" & TmpQueryString)

            dt = dbClass.RunDBQuery(TmpQueryString)
            If dt.Rows.Count > 0 Then
                Label_Status.Text = dt.Rows.Count & ", Record(s) found"
                Label7.Text = "Found: " & dt.Rows.Count

                Application.DoEvents()
                For index = 0 To DataGridView1.Rows.Count - 1
                    Try
                        DataGridView1.Rows.RemoveAt(index)
                    Catch ex As Exception
                    End Try
                Next
            Else
                Label_Status.Text = "No, Record(s) found"
                Application.DoEvents()
            End If

            Label_Status.Text = "Loading Record(s) found"
            Application.DoEvents()
            For Each TableRow In dt.Rows
                Select Case Label_Status.Text
                    Case "Loading Record(s) found /"
                        Label_Status.Text = "Loading Record(s) found -"
                    Case "Loading Record(s) found -"
                        Label_Status.Text = "Loading Record(s) found |"
                    Case "Loading Record(s) found |"
                        Label_Status.Text = "Loading Record(s) found \"
                    Case Else
                        Label_Status.Text = "Loading Record(s) found /"
                End Select

                Dim x As DateTime
                x = TableRow("DateTimeCall")
                DataGridView1.Rows.Add(TableRow("CallID"), TableRow("RowID"), TableRow("DateTimeCall"), TableRow("DateTimeAccpeted"), TableRow("DateTimeHangup"), TableRow("CallerID"), TableRow("CalledID"), IIf(TableRow("Direction") = "1", "Out", "In"), TableRow("FirstName") & " " & TableRow("LastName"), TableRow("Ext"), TableRow("CallDuration"), "Play", "Download")
                'Application.DoEvents()
                'Threading.Thread.Sleep(1)
            Next
            Label_Status.Text = "Done, Select any Record to play"
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Me.Text = xCaption
        Application.DoEvents()
        Me.Enabled = True

    End Sub


    Private Sub Form_MediaPlayer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Me.Text = "Media Player (" & AgentInformation.AppVersion & ")"

            DateTimePicker_Start.Value = DateTime.Now.ToString("yyyy-MM-dd") & " 00:00:00"
            DateTimePicker_End.Value = DateTime.Now.ToString("yyyy-MM-dd") & " 23:59:59"
            DataGridView1.Columns(0).Visible = False
            DataGridView1.Columns(1).Visible = False


            LoadAgents()
            Combo_Type.SelectedIndex = 0

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub AxWindowsMediaPlayer1_PlayStateChange(sender As Object, e As _WMPOCXEvents_PlayStateChangeEvent)
        ' Test the current state of the player, display a message for each state.
        Select Case e.newState
            Case 0 ' Undefined
                Label_Status.Text = "PlayStateChange:Undefined"
            Case 1 ' Stopped
                Label_Status.Text = "PlayStateChange:Stopped"
            Case 2 ' Paused
                Label_Status.Text = "PlayStateChange:Paused"
            Case 3 ' Playing
                Label_Status.Text = "PlayStateChange:Playing"
            Case 4 ' ScanForward
                Label_Status.Text = "PlayStateChange:ScanForward"
            Case 5 ' ScanReverse
                Label_Status.Text = "PlayStateChange:ScanReverse"
            Case 6 ' Buffering
                Label_Status.Text = "PlayStateChange:Buffering"
            Case 7 ' Waiting
                Label_Status.Text = "PlayStateChange:Waiting"
            Case 8 ' MediaEnded
                Label_Status.Text = "PlayStateChange:MediaEnded"
            Case 9 ' Transitioning
                Label_Status.Text = "PlayStateChange:Transitioning"
            Case 10 ' Ready
                Label_Status.Text = "PlayStateChange:Ready"
            Case 11 ' Reconnecting
                Label_Status.Text = "PlayStateChange:Reconnecting"
            Case 12 ' Last
                Label_Status.Text = "PlayStateChange:Last"
            Case Else
                Label_Status.Text = ("PlayStateChange:Unknown: " + e.newState.ToString())
        End Select
        Debug.WriteLine(e.newState & ":" & Label_Status.Text)

    End Sub
    Private Sub AxWindowsMediaPlayer1_Warning(sender As Object, e As _WMPOCXEvents_WarningEvent)
        Label_Status.Text = "warning:" & e.warningType.ToString
    End Sub
    Private Sub AxWindowsMediaPlayer1_OpenStateChange(sender As Object, e As _WMPOCXEvents_OpenStateChangeEvent)
        Label_Status.Text = "OpenStateChange:" & e.newState
    End Sub
    Public Shared Sub Concatenate(ByVal outputFile As String, ByVal sourceFiles As IEnumerable(Of String))

        Dim xAudioFileReader(sourceFiles.Count - 1) As AudioFileReader
        Dim xxCOunt As Integer = 0

        Try
            For Each sourceFile As String In sourceFiles
                xAudioFileReader(xxCOunt) = New AudioFileReader(sourceFile)
                xxCOunt = xxCOunt + 1
            Next
            Dim playlist = New ConcatenatingSampleProvider(xAudioFileReader)
            WaveFileWriter.CreateWaveFile16(outputFile, playlist)
        Catch ex As Exception
            Throw New InvalidOperationException(ex.Message)
        End Try

    End Sub

    Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, ByVal value As T) As T
        target = value
        Return value
    End Function

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        'MessageBox.Show("ColumnIndex:" & e.ColumnIndex & ", RowIndex:" & e.RowIndex)
        Try

            Select Case e.ColumnIndex
                Case 12 '' download
                    Dim xCaption As String = Me.Text
                    Try

                        Dim selectedPath As String = ""
                        Using ofd As New FolderBrowserDialog
                            If ofd.ShowDialog = DialogResult.OK Then
                                selectedPath = ofd.SelectedPath
                            End If
                        End Using

                        Debug.WriteLine("selectedPath:" & selectedPath)
                        If Trim(selectedPath) = "" Then
                            Label_Status.Text = "Downloading.. (Cancelled)"
                            Exit Sub
                        End If


                        Me.Text = xCaption & " Wait..."
                        Application.DoEvents()
                        'Me.Enabled = False
                        Me.Cursor = Cursors.WaitCursor
                        Dim RowID As String = DataGridView1.Rows(e.RowIndex).Cells(0).Value
                        Dim RecID As String = DataGridView1.Rows(e.RowIndex).Cells(1).Value
                        Dim Rec8 As String = DataGridView1.Rows(e.RowIndex).Cells(8).Value
                        Dim Rec7 As String = DataGridView1.Rows(e.RowIndex).Cells(7).Value
                        Dim Rec6 As String = DataGridView1.Rows(e.RowIndex).Cells(6).Value
                        Dim Rec5 As String = DataGridView1.Rows(e.RowIndex).Cells(5).Value

                        Debug.WriteLine("RowID:" & RowID & ", RecID:" & RecID)

                        dbClass.Activity_Get_SessionID(RowID & "_RecordingDownload")

                        Label_Status.Text = "wait, let me retrive the data"
                        Dim FolderName As String = selectedPath
                        Try
                            FolderName = dbClass.CallRecording_Get(RowID, RecID, FolderName)
                            Label_Status.Text = "done, downloading, let me merge"
                        Catch ex As Exception
                            Label_Status.Text = "Sorry, Error:" & ex.Message
                            'Me.Enabled = True
                            Me.Cursor = Cursors.Default
                            Me.Text = xCaption
                            Exit Sub
                        End Try
                        Application.DoEvents()
                        Dim files() As String = IO.Directory.GetFiles(FolderName & "\")


                        Dim FileName As String = ""
                        Dim strMerge(0) As String
                        Dim WaveFiles As New List(Of String)
                        For Each file As String In files
                            WaveFiles.Add(file)
                            Application.DoEvents()
                        Next
                        Dim NewFileName As String = ""
                        NewFileName = selectedPath & "\" & RowID & "-" & RecID & "-" & Rec8 & "-" & Rec7 & "-Download.wav"
                        Try
                            Kill(NewFileName)
                        Catch ex As Exception
                            Debug.WriteLine("Existing file kill exception:" & ex.Message)
                        End Try
                        Concatenate(NewFileName, WaveFiles)
                        For Each file As String In files
                            Try
                                Kill(file)
                                Application.DoEvents()
                            Catch ex As Exception
                                Debug.WriteLine(ex.Message)
                            End Try
                        Next
                        Try
                            '' remove temp folder
                            RmDir(FolderName)
                        Catch ex As Exception
                            Debug.WriteLine(ex.Message)
                        End Try
                        Application.DoEvents()
                        'dbClass.Activity_Set_SessionID(RowID & "_Recordingdownloaded")
                        Label_Status.Text = "Merged.."

                        Dim tag As ID3TagData = New ID3TagData With {
                            .Title = "CallCenter(" & Rec5 & ")",
                            .Artist = Rec8,
                            .Album = RecID & "-" & Rec6,
                            .Year = "1947",
                            .Comment = RowID,
                            .Genre = LameMP3FileWriter.Genres(1),
                            .Subtitle = Rec7
                        }
                        Label_Status.Text = "Wait, Converting.."

                        Using reader = New AudioFileReader(NewFileName)
                            Using writer = New LameMP3FileWriter(NewFileName & ".mp3", reader.WaveFormat, 128, tag)
                                reader.CopyTo(writer)
                            End Using
                        End Using

                        Label_Status.Text = "Conversion done.."

                        Application.DoEvents()
                        Try
                            Kill(NewFileName)
                        Catch ex As Exception
                            Debug.WriteLine("Kill Exception:" & ex.Message)
                        End Try
                        Try
                            Console.WriteLine("selectedPath:" & selectedPath)
                            Shell("Explorer " & selectedPath, AppWinStyle.NormalFocus, False, 0)
                        Catch ex As Exception
                            Debug.WriteLine("Shell Exception:" & ex.Message)
                        End Try
                    Catch ex As Exception
                        Label_Status.Text = "Sorry, Error:" & ex.Message
                    End Try
                    Me.Enabled = True
                    Me.Cursor = Cursors.Default
                    Me.Text = xCaption
                    Label_Status.Text = "All Set (Y)"

                Case 11 '' Play

                    Dim xCaption As String = Me.Text
                    Try
                        Me.Text = xCaption & " Wait..."
                        Application.DoEvents()
                        Me.Enabled = False
                        Me.Cursor = Cursors.WaitCursor
                        Dim RowID As String = DataGridView1.Rows(e.RowIndex).Cells(0).Value
                        Dim RecID As String = DataGridView1.Rows(e.RowIndex).Cells(1).Value

                        Debug.WriteLine("RowID:" & RowID & ", RecID:" & RecID)

                        dbClass.Activity_Get_SessionID(RowID & "_RecordingPlay")

                        Label_Status.Text = "wait, let me retrive the data"
                        Dim FolderName As String = ""
                        Try
                            FolderName = dbClass.CallRecording_Get(RowID, RecID)
                            Label_Status.Text = "done, let me play"
                        Catch ex As Exception
                            Label_Status.Text = "Sorry, Error:" & ex.Message
                            Me.Enabled = True
                            Me.Cursor = Cursors.Default
                            Me.Text = xCaption
                            Exit Sub
                        End Try
                        If FolderName = "" Then
                            Label_Status.Text = "Sorry, Recording not found"
                            Me.Enabled = True
                            Me.Cursor = Cursors.Default
                            Me.Text = xCaption
                            Exit Sub
                        End If
                        Label_Status.Text = "Decrypting.."
                        Application.DoEvents()

                        Dim files() As String = IO.Directory.GetFiles(FolderName & "\")


                        AxWindowsMediaPlayer1.uiMode = "full"
                        AxWindowsMediaPlayer1.settings.setMode("loop", False)
                        AxWindowsMediaPlayer1.settings.playCount = 1
                        Dim Playlist = AxWindowsMediaPlayer1.newPlaylist("MyPlayList", "")
                        AxWindowsMediaPlayer1.currentPlaylist = Playlist
                        Playlist.clear()

                        Dim FileName As String = ""
                        Dim strMerge(0) As String


                        For Each file As String In files


                            ' Do work, example
                            If IsNothing(strMerge) Then
                                ReDim Preserve strMerge(0)
                            Else
                                ReDim Preserve strMerge(strMerge.Length + 1)
                            End If
                            Application.DoEvents()
                            Label_Status.Text = "Downloading.. (" & strMerge.Length & "/" & files.Length & ")"
                            Application.DoEvents()
                            FileName = file
                            strMerge(strMerge.Length - 1) = file
                            Console.WriteLine("FileName:" & FileName)
                            Try
                                Dim x = yObject.Decrypt(FileName)
                                Label_Status.Text = "Decrypted:Result=" & x
                                Application.DoEvents()
                            Catch ex As Exception
                                MessageBox.Show(Me, "Exception:" & ex.Message, MessageBoxButtons.OK)
                            End Try
                            Playlist.appendItem(AxWindowsMediaPlayer1.newMedia(FileName))
                        Next

                        'Dim NewFileName As String = FolderName & "\" & RowID & ".wav"
                        'Dim Y As Boolean = yObject.Merge(strMurge, NewFileName)
                        Application.DoEvents()
                        'AxWindowsMediaPlayer1.URL = NewFileName

                        AxWindowsMediaPlayer1.Ctlcontrols.play()
                        Label_Status.Text = "Playing.."
                        'Kill(FileName)
                        dbClass.Activity_Set_SessionID(RowID & "_RecordingPlay")

                    Catch ex As Exception
                        Label_Status.Text = "Sorry, Error:" & ex.Message
                    End Try
                    Me.Enabled = True
                    Me.Cursor = Cursors.Default
                    Me.Text = xCaption
                Case Else

                    Debug.WriteLine("ColumnIndex:" & e.ColumnIndex)
            End Select
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub TextBox_Limit_TextChanged(sender As Object, e As EventArgs) Handles TextBox_Limit.TextChanged
        Try

            If IsNumeric(TextBox_Limit.Text) Then
                If TextBox_Limit.Text.Length < 2 Or TextBox_Limit.Text.Length > 4 Then
                    TextBox_Limit.Text = "100"
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub TabControl_TabIndexChanged(sender As Object, e As EventArgs) Handles TabControl.TabIndexChanged, TabControl.SelectedIndexChanged
        Try

            If TabControl.SelectedTab.TabIndex = 0 Then
                LoadData(2)        'Load Calls with Recordings Only
            ElseIf TabControl.SelectedTab.TabIndex = 1 Then
                LoadData(1)        'Load Calls without Recordings Only
            ElseIf TabControl.SelectedTab.TabIndex = 2 Then
                LoadData(0)        'Load All Calls with or without Recordings
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub DateTimePicker_Start_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker_Start.ValueChanged
        'MessageBox.Show(DateTimePicker_Start.Value)
    End Sub

    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter

    End Sub
    Private Sub Form_MediaPlayer_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        dbClass.Activity_Set_SessionID("Accessing_Recorder")
    End Sub

    Private Sub AxWindowsMediaPlayer1_Enter(sender As Object, e As EventArgs) Handles AxWindowsMediaPlayer1.Enter

    End Sub

    Private Sub TextBox_CLI_TextChanged(sender As Object, e As EventArgs) Handles TextBox_CLI.TextChanged

    End Sub

    Private Sub TabControl_Click(sender As Object, e As EventArgs) Handles TabControl.Click

    End Sub
End Class