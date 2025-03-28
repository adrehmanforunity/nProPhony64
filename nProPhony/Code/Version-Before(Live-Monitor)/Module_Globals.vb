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

End Module
