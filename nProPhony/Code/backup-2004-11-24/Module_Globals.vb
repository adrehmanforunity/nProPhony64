Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports System.Threading


Module Module_Globals

#Region "local Logging Related"
    Public FileWriter As Class_FileWriter
    Public Sub Write_Log(FileName As String, Text As String)
        FileName = UCase(FileName)
        If Trim(FileName) = "" Then
            FileName = "APP"
        End If
        Dim _FileName As String
        _FileName = Now.ToString("yyyyMMddHH") & "_" & FileName & ".Log"
        Dim str = DateTime.Now.ToString("HH:mm:ss:ffff").ToString.PadLeft(13, " ") & " " & Text
        Try
            If IsNothing(FileWriter) Then

            Else

                FileWriter.Write_TO_File(LogDir & "\" & _FileName, str)
            End If
        Catch ex As Exception
        End Try
    End Sub
#End Region

    Public CallInformation As TypeCallInformation
    Public Structure TypeCallInformation
        Public CallDirection As String
        Public CallerID As String
        Public CalledID As String
        Public CallStatus As String
        Public RefID_DB As String
        Public RefID_Server As String
        Public CRMSessionID As String
        Public CRMInteractionID As String
        Public CustomerID As String
        Public HangupByAgent As Boolean
        Public CallingOut As Boolean
        Public CallerVerifyed As Integer
        Public FeedBack As Integer
        Public Recording As Boolean
        Public RecordingFileName As String
        Public thUploader As Thread
        Public RecordingStartedAT As DateTime
        Public RecordingFileChunkNo As Integer
        Public nLine As Long
        Public nLineSessionID1 As Long
        Public nLineSessionID2 As Long
        Public nLineSessionEXTENSION As String
        Public nLineSessionStatus As String
        Public Transfering As Boolean
        Public Conference As Boolean
        Public DestinationExt As String
        Public TalkingStartedAt As DateTime
        Public TotalTalkTime As Long
        Public SendBacktoIVR As Boolean
        Public SendbacktoIVRLanguage As String
        Public SendbacktoIVRVDN As String

    End Structure

    Public wsClientAllowed As Boolean
    Public wsClientCloseAllConnections As Boolean

    Public ServerInformation As TypeServerInformation
    Public Structure TypeServerInformation
        Public Primary As String
        Public Secondary As String
        Public Database As String
        Public Port As Long
        Public TCPEnabled As Boolean
        Public SessionTimeout As Long
        Public Lines As Long
    End Structure

    Public Configurations As New TypeConfigurations
    Public Structure TypeConfigurations
        Public ExtensionRenaming As Boolean
        Public IgnoreRenameError As Boolean

        Public UseNativeWS As Boolean
        Public bDisableTrayICON As Boolean
        Public bLogSIPDetails As Boolean

        Public RecordinChunkTimeOut As Long

        Public ExtPooling As Boolean
        Public AutoPickup As Boolean

        Public NoAns As Integer
        Public CallLogsEnabled As Boolean
        Public GetDTMFViaManager As Boolean
        ' For IMS
        Public Company As String
        Public TPINConnection As Boolean

        Public VersionControl As Boolean
        Public VersionAllowed As String
        Public RecordCalls As Boolean
        Public RecordingFormat As String
        Public URLCalling As Boolean
        Public URLSToCall As URLS
        Public RecordingStartDelay As Integer
        Public ConnectedBeepDelay As Integer

    End Structure
    Public Structure URLS

        Public URL_Ringing As String
        Public URL_Connected As String
        Public URL_Hangup As String

        Public URL_Ringing_Secure As Boolean
        Public URL_Connected_Secure As Boolean
        Public URL_Hangup_Secure As Boolean

        Public URL_Ringing_TO As Integer
        Public URL_Connected_TO As Integer
        Public URL_Debug As Boolean
        Public URL_Hangup_TO As Integer
    End Structure

    Public AgentInformation As New TypeAgentStateInformation
    Public Structure TypeAgentStateInformation
        Public PoolID As String
        Public LoginSessionID As String
        Public LoginDateTime As Date

        Public QueueID As Long
        Public QueueName As String

        Public AgentID As String
        Public LoginName As String
        Public FirstName As String
        Public LastName As String
        Public PABXExt As String
        Public PABXSecret As String
        Public Supervisor As Boolean
        Public ActivitySessionID As String
        Public DND As Boolean
        Public DNDCaption As String
        Public LoggedIn As Boolean
        Public ReadyMode As String
        '----------- Version 3
        Public FromComputerName As String
        Public FromIPAddress As String
        Public FromOSUser As String
        Public AppTitle As String
        Public AppVersion As String
        Public AppDate As String
        Public AppPath As String
    End Structure


    Public yObject As Utilities.WaveIO
    Public xObject As Utilities.IOWrapper

    Public LogDir As String

    Public appPath As String = Application.ExecutablePath()

    Public dbClass As Class_DBConnector

    Private Declare Auto Function GetPrivateProfileString Lib "kernel32" (ByVal lpAppName As String,
           ByVal lpKeyName As String,
           ByVal lpDefault As String,
           ByVal lpReturnedString As StringBuilder,
           ByVal nSize As Integer,
           ByVal lpFileName As String) As Integer

    Public Function GetINIString(sHeader As String, sItem As String, sDefault As String) As String
        GetINIString = dbClass.GetINIString(sHeader, sItem, sDefault)
    End Function

    Public Function GetINIStringOldWay(sHeader As String, sItem As String, sDefault As String) As String
        Try
            Dim sb As StringBuilder
            sb = New StringBuilder(500)
            'Debug.Print("DirectoryPath:" & My.Application.Info.DirectoryPath)
            'MessageBox.Show("DirectoryPath:" & My.Application.Info.DirectoryPath)
            Dim res As Integer = GetPrivateProfileString(sHeader, sItem, sDefault, sb, sb.Capacity, My.Application.Info.DirectoryPath & "\ProPhony.ini")
            GetINIStringOldWay = sb.ToString()
        Catch ex As Exception
            GetINIStringOldWay = ""
        End Try
    End Function

    Public Sub LoadMachineDetails()
        Try
            AgentInformation.AppTitle = "nProPhony Desktop"
            'AgentInformation.AppVersion = "nV1.0"
            AgentInformation.AppPath = My.Application.Info.DirectoryPath
            AgentInformation.FromOSUser = "" & SystemInformation.UserName
            AgentInformation.FromComputerName = "" & SystemInformation.ComputerName
            AgentInformation.FromIPAddress = ""
            AgentInformation.AppDate = ""
            'Dim IpAddrs() As String GetIpAddrTable
            'Dim i As Integer
            'For i = LBound(IpAddrs) To UBound(IpAddrs)
            'AgentInformation.FromIPAddress = AgentInformation.FromIPAddress & IpAddrs(i) & ","
            'Next
            Try
                AgentInformation.AppDate = System.IO.File.GetLastWriteTime(AgentInformation.AppPath & "\NProPhony.exe")
            Catch ex As Exception
            End Try
        Catch ex As Exception
            Write_Log("", "LoadMachineDetails:Error:" & ex.Message)
            MessageBox.Show("LoadMachineDetails:" & ex.Message)
            End
        End Try
    End Sub
#Region "conversion"
    '    Public Function CompressRecording(sFileName As String, Optional format As String = "gsm") As String
    '        Dim outputFile As String = String.Empty
    '        Try
    '            Write_Log("", "CompressRecording(Start) sFileName: " & sFileName & ", Format: " & format)

    '            Define the input And output file paths
    '            Dim inputFile As String = sFileName
    '            outputFile = System.IO.Path.ChangeExtension(inputFile, format)

    '            FFmpeg executable path in your application's directory
    '            Dim appPath As String = AppDomain.CurrentDomain.BaseDirectory
    '            Dim ffmpegPath As String = System.IO.Path.Combine(appPath, "ffmpeg", "ffmpeg.exe")

    '            Check If the FFmpeg executable exists
    '            If Not System.IO.File.Exists(ffmpegPath) Then
    '                Write_Log("", "CompressRecording(Error) FFmpeg executable not found at: " & ffmpegPath)
    '                Return sFileName ' Return the original file if FFmpeg is not found
    '            End If

    '            Determine the FFmpeg command based on the desired format
    '            Dim arguments As String
    '            If format.ToLower() = "gsm" Then
    '                arguments = $"-i ""{inputFile}"" -c:a libgsm -ar 8000 ""{outputFile}"""
    '            ElseIf format.ToLower() = "mp3" Then
    '                arguments = $"-i ""{inputFile}"" -vn -ar 44100 -ac 2 -b:a 192k ""{outputFile}"""
    '            Else
    '                Write_Log("", "CompressRecording(Error) Unsupported format: " & format)
    '                Return sFileName ' Return the original file if the format is not supported
    '            End If

    '            Create the process start info
    '            Dim processStartInfo As New ProcessStartInfo With {
    '            .FileName = ffmpegPath,
    '            .Arguments = arguments,
    '            .RedirectStandardOutput = True,
    '            .RedirectStandardError = True,
    '            .UseShellExecute = False,
    '            .CreateNoWindow = True
    '        }

    '            Start the process
    '            Using process As New Process()
    '                process.StartInfo = processStartInfo
    '                process.Start()

    '                Read the output And error (for debugging)
    '                Dim output As String = process.StandardOutput.ReadToEnd()
    '                Dim errors As String = process.StandardError.ReadToEnd()
    '                process.WaitForExit()
    '                Write_Log("", "CompressRecording(ExitCode) ExitCode: " & process.ExitCode)
    '                Check the process exit code
    '                If process.ExitCode = 0 Then
    '                    Try
    '                        If System.IO.File.Exists(inputFile) Then
    '                            System.IO.File.Delete(inputFile)
    '                        End If
    '                        Write_Log("", "CompressRecording(Success) Conversion to " & format & " completed successfully!, and source file deleted")
    '                    Catch ex As Exception
    '                        Write_Log("", "CompressRecording(Success) Conversion to " & format & " completed successfully!, and failed to delete source file")
    '                    End Try
    '                Else
    '                    Write_Log("", "CompressRecording(Error) Error during conversion: " & errors)
    'Cleanup:            If the Then conversion failed, delete any Partial output file
    '                    If System.IO.File.Exists(outputFile) Then
    '                            System.IO.File.Delete(outputFile)
    '                        End If
    '                        outputFile = sFileName ' Ensure we return a consistent failure result
    '                    End If
    '            End Using
    '        Catch ex As Exception
    '            Write_Log("", "CompressRecording(Exception) Exception: " & ex.Message)
    '            outputFile = sFileName ' Return the original file to indicate failure
    '        End Try

    '        Write_Log("", "CompressRecording(Complete) Result: " & If(String.IsNullOrEmpty(outputFile), "Conversion failed", outputFile))
    '        Return outputFile
    '    End Function


    'Public Function CompressRecording(sFileName As String, Optional format As String = "gsm") As String
    '    Dim outputFile As String = String.Empty
    '    Try
    '        Write_Log("", "CompressRecording(Start) sFileName: " & sFileName & ", Format: " & format)

    '        ' FFmpeg executable path in your application's directory
    '        Dim appPath As String = AppDomain.CurrentDomain.BaseDirectory
    '        Dim ffmpegPath As String = System.IO.Path.Combine(appPath, "ffmpeg", "ffmpeg.exe")

    '        ' Check if the FFmpeg executable exists
    '        If Not System.IO.File.Exists(ffmpegPath) Then
    '            Write_Log("", "CompressRecording(Error) FFmpeg executable not found at: " & ffmpegPath)
    '            Return sFileName ' Return the original file if FFmpeg is not found
    '        End If

    '        ' Define the input and output file paths
    '        Dim inputFile As String = sFileName
    '        Select Case format.ToLower()
    '            Case "mp3", "mp31"
    '                outputFile = System.IO.Path.ChangeExtension(inputFile, "mp3")
    '                If format.ToLower() = "mp31" Then
    '                    outputFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(outputFile),
    '                                                    System.IO.Path.GetFileNameWithoutExtension(outputFile) & "_lowbitrate.mp3")
    '                End If
    '            Case "opus", "opus1"
    '                outputFile = System.IO.Path.ChangeExtension(inputFile, "opus")
    '                If format.ToLower() = "opus1" Then
    '                    outputFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(outputFile),
    '                                                    System.IO.Path.GetFileNameWithoutExtension(outputFile) & "_lowbitrate.opus")
    '                End If
    '            Case Else
    '                outputFile = System.IO.Path.ChangeExtension(inputFile, "gsm")
    '        End Select

    '        ' Determine the FFmpeg command based on the desired format
    '        Dim arguments As String

    '        ' Check if the format is MP3 with standard settings (stereo, 44.1 kHz, 192 kbps)
    '        If format.ToLower() = "mp3" Then
    '            ' MP3 compression at 192 kbps, stereo, 44.1 kHz (good for higher-quality audio)
    '            arguments = $"-i ""{inputFile}"" -vn -ar 44100 -ac 2 -b:a 192k ""{outputFile}"""

    '            ' Check if the format is MP3 with optimized settings for voice (mono, 16 kHz, 64 kbps)
    '        ElseIf format.ToLower() = "mp31" Then
    '            ' MP3 compression at 64 kbps, mono, 16 kHz (smaller file size, optimized for voice)
    '            arguments = $"-i ""{inputFile}"" -vn -ar 16000 -ac 1 -b:a 64k ""{outputFile}"""

    '            ' Check if the format is Opus with a low bitrate (16 kbps)
    '        ElseIf format.ToLower() = "opus" Then
    '            ' Opus compression at 16 kbps (suitable for low-bandwidth voice recordings)
    '            arguments = $"-i ""{inputFile}"" -c:a libopus -b:a 16k ""{outputFile}"""

    '            ' Check if the format is Opus with an even lower bitrate (8 kbps)
    '        ElseIf format.ToLower() = "opus1" Then
    '            ' Opus compression at 8 kbps (very low bandwidth, further reduced file size)
    '            arguments = $"-i ""{inputFile}"" -c:a libopus -b:a 8k ""{outputFile}"""

    '            ' Default to GSM format if no other format is matched
    '        Else
    '            ' GSM compression (narrowband 8 kHz, 13 kbps, optimized for telephony voice quality)
    '            arguments = $"-i ""{inputFile}"" -c:a libgsm -ar 8000 ""{outputFile}"""
    '        End If

    '        ' Create the process start info
    '        Dim processStartInfo As New ProcessStartInfo With {
    '        .FileName = ffmpegPath,
    '        .Arguments = arguments,
    '        .RedirectStandardOutput = True,
    '        .RedirectStandardError = True,
    '        .UseShellExecute = False,
    '        .CreateNoWindow = True
    '    }

    '        ' Start the process
    '        Using process As New Process()
    '            process.StartInfo = processStartInfo
    '            process.Start()

    '            ' Read the output and error (for debugging)
    '            Dim output As String = process.StandardOutput.ReadToEnd()
    '            Dim errors As String = process.StandardError.ReadToEnd()
    '            process.WaitForExit()
    '            Write_Log("", "CompressRecording(ExitCode) ExitCode: " & process.ExitCode)

    '            ' Check the process exit code
    '            If process.ExitCode = 0 Then
    '                Try
    '                    ' Cleanup: if successful, delete the source file
    '                    If System.IO.File.Exists(inputFile) Then
    '                        System.IO.File.Delete(inputFile)
    '                    End If
    '                    Write_Log("", "CompressRecording(Success) Conversion to " & format & " completed successfully!, and source file deleted")
    '                Catch ex As Exception
    '                    Write_Log("", "CompressRecording(Success) Conversion to " & format & " completed successfully!, and failed to delete source file")
    '                End Try
    '            Else
    '                Write_Log("", $"CompressRecording(Error) Error during conversion of {inputFile} to {outputFile}: {errors}")
    '                ' Cleanup: If the conversion failed, delete any partial output file
    '                If System.IO.File.Exists(outputFile) Then
    '                    System.IO.File.Delete(outputFile)
    '                End If
    '                outputFile = sFileName ' Ensure we return a consistent failure result
    '            End If
    '        End Using
    '    Catch ex As Exception
    '        Write_Log("", "CompressRecording(Exception) Exception: " & ex.Message)
    '        outputFile = sFileName ' Return the original file to indicate failure
    '    End Try
    '    Write_Log("", "CompressRecording(Complete) Result: " & If(String.IsNullOrEmpty(outputFile), "Conversion failed", outputFile))
    '    Return outputFile
    'End Function
    Public Function CompressRecording(sFileName As String, Optional format As String = "gsm") As String
        Dim outputFile As String = String.Empty
        Dim inputFileSize As Long = 0
        Dim outputFileSize As Long = 0
        Dim duration As TimeSpan = TimeSpan.Zero
        Dim success As Boolean = False
        Dim errorMessage As String = String.Empty

        Try
            Write_Log("", "CompressRecording(Start) sFileName: " & sFileName & ", Format: " & format)

            ' FFmpeg executable path in your application's directory
            Dim appPath As String = AppDomain.CurrentDomain.BaseDirectory
            Dim ffmpegPath As String = System.IO.Path.Combine(appPath, "ffmpeg", "ffmpeg.exe")

            ' Check if the FFmpeg executable exists
            If Not System.IO.File.Exists(ffmpegPath) Then
                Write_Log("", "CompressRecording(Error) FFmpeg executable not found at: " & ffmpegPath)
                Return sFileName ' Return the original file if FFmpeg is not found
            End If

            ' Define the input and output file paths
            Dim inputFile As String = sFileName
            inputFileSize = New System.IO.FileInfo(inputFile).Length ' Get the input file size
            Select Case format.ToLower()
                Case "mp3", "mp31"
                    outputFile = System.IO.Path.ChangeExtension(inputFile, "mp3")
                    If format.ToLower() = "mp31" Then
                        outputFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(outputFile),
                                                        System.IO.Path.GetFileNameWithoutExtension(outputFile) & "_lowbitrate.mp3")
                    End If
                Case "opus", "opus1"
                    outputFile = System.IO.Path.ChangeExtension(inputFile, "opus")
                    If format.ToLower() = "opus1" Then
                        outputFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(outputFile),
                                                        System.IO.Path.GetFileNameWithoutExtension(outputFile) & "_lowbitrate.opus")
                    End If
                Case Else
                    outputFile = System.IO.Path.ChangeExtension(inputFile, "gsm")
            End Select

            ' Determine the FFmpeg command based on the desired format
            Dim arguments As String

            ' Check if the format is MP3 with standard settings (stereo, 44.1 kHz, 192 kbps)
            If format.ToLower() = "mp3" Then
                arguments = $"-i ""{inputFile}"" -vn -ar 44100 -ac 2 -b:a 192k ""{outputFile}"""
            ElseIf format.ToLower() = "mp31" Then
                arguments = $"-i ""{inputFile}"" -vn -ar 16000 -ac 1 -b:a 64k ""{outputFile}"""
            ElseIf format.ToLower() = "opus" Then
                arguments = $"-i ""{inputFile}"" -c:a libopus -b:a 16k ""{outputFile}"""
            ElseIf format.ToLower() = "opus1" Then
                arguments = $"-i ""{inputFile}"" -c:a libopus -b:a 8k ""{outputFile}"""
            Else
                arguments = $"-i ""{inputFile}"" -c:a libgsm -ar 8000 ""{outputFile}"""
            End If

            ' Create the process start info
            Dim processStartInfo As New ProcessStartInfo With {
            .FileName = ffmpegPath,
            .Arguments = arguments,
            .RedirectStandardOutput = True,
            .RedirectStandardError = True,
            .UseShellExecute = False,
            .CreateNoWindow = True
        }

            ' Start the process and log the duration
            Dim startTime As DateTime = DateTime.Now
            Using process As New Process()
                process.StartInfo = processStartInfo
                process.Start()

                ' Read the output and error (for debugging)
                Dim output As String = process.StandardOutput.ReadToEnd()
                Dim errors As String = process.StandardError.ReadToEnd()
                process.WaitForExit()

                duration = DateTime.Now - startTime
                Write_Log("", $"CompressRecording(ExitCode) ExitCode: {process.ExitCode}")

                ' Check the process exit code
                If process.ExitCode = 0 Then
                    success = True ' Mark success
                    If System.IO.File.Exists(outputFile) Then
                        outputFileSize = New System.IO.FileInfo(outputFile).Length
                        ' Cleanup: if successful, delete the source file
                        If System.IO.File.Exists(inputFile) Then
                            System.IO.File.Delete(inputFile)
                        End If
                        Write_Log("", "CompressRecording(Success) Conversion to " & format & " completed successfully!, and source file deleted")
                    Else
                        '' concider this as failure dont kill the source file
                        success = False  ' Mark success
                        outputFileSize = inputFileSize
                        errorMessage = "CompressRecording(Success) Conversion to " & format & " completed successfully!, but output file not found"
                    End If
                Else
                    outputFileSize = inputFileSize
                    errorMessage = $"Error during conversion of {inputFile} to {outputFile}: {errors}"
                    ' Cleanup: If the conversion failed, delete any partial output file
                    If System.IO.File.Exists(outputFile) Then
                        System.IO.File.Delete(outputFile)
                    End If
                    outputFile = sFileName ' Ensure we return a consistent failure result
                End If
            End Using
        Catch ex As Exception
            errorMessage = "Exception: " & ex.Message
            Write_Log("", $"CompressRecording(Exception) {errorMessage}")
            outputFile = sFileName ' Return the original file to indicate failure
        End Try
        ' Summary log in a single line
        Write_Log("", $"CompressRecording(Summary) Input File Size: {inputFileSize} bytes, Output File Size: {outputFileSize} bytes, Conversion Successful: {success}, Error: {errorMessage}, Duration: {duration.TotalSeconds} seconds")
        Write_Log("", "CompressRecording(Complete) Result: " & If(String.IsNullOrEmpty(outputFile), "Conversion failed", outputFile))
        Return outputFile
    End Function

#End Region


End Module
