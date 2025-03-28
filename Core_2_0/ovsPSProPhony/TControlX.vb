Imports System.Threading
Imports System.Collections.Generic
Imports System.Text
Imports System.ComponentModel
Imports System.Data
Imports System.IO
Imports ovsPSProPhony.PortSIP
Imports System.Threading.Thread
Imports System.Runtime.InteropServices
Imports WinSound



Public Class TControl
    Implements PortSIP.SIPCallbackEvents


    Public Property StreamWriterStatus(Optional ChannelNo As Integer = 0) As Boolean
        Get
            '_thStreamWriterStatus = False
            Return _Channel(ChannelNo).Thread_Media_Recorder_status
        End Get
        Set(ByVal value As Boolean)
            _Channel(ChannelNo).Thread_Media_Recorder_status = value
        End Set
    End Property
    Private Sub writeRecordingData(ChannelNo As Integer)
        Try
            If IsNothing(_Channel(ChannelNo)._recfileData) = False Then
                If _Channel(ChannelNo)._recfileData.Length > 1 Then
                    Dim tmpFileData() As Byte

                    SyncLock (_Channel(ChannelNo)._recfileData)
                        tmpFileData = _Channel(ChannelNo)._recfileData
                        _Channel(ChannelNo)._recfileData = Nothing
                    End SyncLock
                    SyncLock tmpFileData
                        _Channel(ChannelNo)._recfileDataWritenAt = DateTime.Now
                        'If _Channel(ChannelNo)._recDoCerateFile Then
                        If My.Computer.FileSystem.FileExists(_Channel(ChannelNo)._recFileName) Then
                            WinSound.WaveFile.AppendData(_Channel(ChannelNo)._recFileName, tmpFileData, True)
                        Else
                            WinSound.WaveFile.Create(_Channel(ChannelNo)._recFileName, 8000, 16, 1, tmpFileData)
                        End If
                    End SyncLock

                    'If _Channel(ChannelNo)._recFileName Then
                    'WinSound.WaveFile.Create(_Channel(ChannelNo)._recFileName, 8000, 16, 1, tmpFileData)
                    '_Channel(ChannelNo)._recDoCerateFile = False
                    'Else
                    'WinSound.WaveFile.AppendData(_Channel(ChannelNo)._recFileName, tmpFileData, True)
                    'End If
                End If
            End If
        Catch ex As Exception
            RaiseError(ChannelNo, -1, -1, New Exception("writeRecordingData() " & ex.Message))
        End Try
    End Sub
    Private Sub thMedia_Recorder(ChannelNo As Integer)
        Try
            Debug.WriteLine("thMedia_Recorder Starting for Channel=" & ChannelNo)
            _Channel(ChannelNo).Thread_Media_Recorder_status = False
            Dim theDiff As Long = 0
            While True
                If _Channel(ChannelNo).Thread_Media_Recorder_status = False Then
                    sendGenericEvent(ChannelNo, -1, "thMedia_Recorder", -1, "IAMALIVE", "")
                    _Channel(ChannelNo).Thread_Media_Recorder_status = True
                End If
                If IsNothing(_Channel(ChannelNo)._recfileData) = False Then
                    If _Channel(ChannelNo)._recfileData.Length > 1 Then
                        If DateDiff(DateInterval.Second, _Channel(ChannelNo)._recfileDataWritenAt, Now) >= _Channel(ChannelNo)._recInterval Then
                            writeRecordingData(ChannelNo)
                        End If
                    End If
                End If
                Threading.Thread.Sleep(100)
            End While
            _Channel(ChannelNo).Thread_Media_Recorder_status = False
        Catch ex As Exception
            _Channel(ChannelNo).Thread_Media_Recorder_status = False
            RaiseError(ChannelNo, -1, -1, New Exception("thMedia_Recorder() " & ex.Message))
        End Try
    End Sub

    Private _Channels As Integer
    Private _coreMode As Integer
    Private _Recording_CALLBACK_MODE As AUDIOSTREAM_CALLBACK_MODE
    Dim _logLevel As PORTSIP_LOG_LEVEL
    Public Property LogLevel() As Integer
        Get
            Return _logLevel
        End Get
        Set(ByVal value As Integer)
            Select Case value
                Case PORTSIP_LOG_LEVEL.PORTSIP_LOG_DEBUG
                    _logLevel = PORTSIP_LOG_LEVEL.PORTSIP_LOG_DEBUG
                Case PORTSIP_LOG_LEVEL.PORTSIP_LOG_ERROR
                    _logLevel = PORTSIP_LOG_LEVEL.PORTSIP_LOG_ERROR
                Case PORTSIP_LOG_LEVEL.PORTSIP_LOG_INFO
                    _logLevel = PORTSIP_LOG_LEVEL.PORTSIP_LOG_INFO
                Case PORTSIP_LOG_LEVEL.PORTSIP_LOG_NONE
                    _logLevel = PORTSIP_LOG_LEVEL.PORTSIP_LOG_NONE
                Case PORTSIP_LOG_LEVEL.PORTSIP_LOG_WARNING
                    _logLevel = PORTSIP_LOG_LEVEL.PORTSIP_LOG_WARNING
                Case Else
                    _logLevel = PORTSIP_LOG_LEVEL.PORTSIP_LOG_NONE
            End Select
        End Set
    End Property
    Private Function enableAudioStreamCallback(ChannelNo As Integer, sessionID As Long, enable As Boolean, Optional __AUDIOSTREAM_CALLBACK_MODE As AUDIOSTREAM_CALLBACK_MODE = AUDIOSTREAM_CALLBACK_MODE.AUDIOSTREAM_NONE) As Boolean
        enableAudioStreamCallback = False
        Try
            _Recording_CALLBACK_MODE = __AUDIOSTREAM_CALLBACK_MODE
            Dim rt As Integer = 0
            sendGenericEvent(ChannelNo, -1, "onInviteRecord", sessionID, "enableAudioStreamCallback(" & rt & ")", "End")
            rt = _Channel(ChannelNo)._sdkLib.enableAudioStreamCallback(ChannelNo, sessionID, enable, _Recording_CALLBACK_MODE)
            sendGenericEvent(ChannelNo, -1, "onInviteRecord", sessionID, "enableAudioStreamCallback(" & rt & ")", "End")
            Return True
        Catch ex As Exception
            RaiseError(ChannelNo, -1, sessionID, New Exception("enableAudioStreamCallback() " & ex.Message))
        End Try
    End Function
    Public Property Channels() As Integer
        Get
            Return _Channels
        End Get
        Set(ByVal value As Integer)
            _Channels = value
            ReDim _Channel(_Channels - 1)
        End Set
    End Property
    Public Property CoreMode() As Integer
        Get
            Return _coreMode
        End Get
        Set(ByVal value As Integer)
            _coreMode = value
        End Set
    End Property

    Public Enum SystemSettings
        EC
        VAD
        CNG
        AGC
        ANS
        NACK
        SDP
    End Enum
    Public Function SystemSetting(ChannelNo As Integer, SystemParam As SystemSettings, Enable As Boolean) As Boolean
        SystemSetting = False
        Select Case SystemParam
            Case SystemSettings.EC = Enable
            Case SystemSettings.VAD : _Channel(ChannelNo)._VAD = Enable
            Case SystemSettings.CNG : _Channel(ChannelNo)._CNG = Enable
            Case SystemSettings.AGC : _Channel(ChannelNo)._AGC = Enable
            Case SystemSettings.ANS : _Channel(ChannelNo)._ANS = Enable
            Case SystemSettings.NACK : _Channel(ChannelNo)._NACK = Enable
            Case SystemSettings.SDP : _Channel(ChannelNo)._SDP = Enable
        End Select
        SystemSetting = True
    End Function

#Region "callrelatedEvents"
    Public Delegate Sub Event_CallGeneric2_Handler(ByVal Sender As Object, ByVal e As Event_CallGeneric2_Args)
    Public Event Event_CallGeneric2 As Event_CallGeneric2_Handler
    Public Class Event_CallGeneric2_Args
        Inherits EventArgs
        Public callbackIndex As Integer
        Public callbackObject As Integer
        Public eventName As String = ""
        Public sessionId As Int32
        Public callerDisplayName As [String]
        Public caller As [String]
        Public calleeDisplayName As [String]
        Public callee As [String]
        Public audioCodecNames As [String]
        Public videoCodecNames As [String]
        Public existsAudio As [Boolean]
        Public existsVideo As [Boolean]
        Public sipMessage As StringBuilder
        Public Sub New(ByVal __callbackIndex As Integer,
                    ByVal __callbackObject As Integer,
                    ByVal __eventName As String,
                    __sessionId As Integer,
                    __callerDisplayName As [String],
                    __caller As [String],
                    __calleeDisplayName As [String],
                    __callee As [String],
                    __audioCodecNames As [String],
                    __videoCodecNames As [String],
                    __existsAudio As [Boolean],
                    __existsVideo As [Boolean],
                    __sipMessage As StringBuilder)

            callbackIndex = __callbackIndex
            callbackObject = __callbackObject
            eventName = __eventName
            sessionId = __sessionId
            callerDisplayName = __callerDisplayName
            caller = __caller
            calleeDisplayName = __calleeDisplayName
            callee = __callee
            audioCodecNames = __audioCodecNames
            videoCodecNames = __videoCodecNames
            existsAudio = __existsAudio
            existsVideo = __existsVideo
            sipMessage = __sipMessage
        End Sub
    End Class

    Private Sub sendGeneric2Event(
                    ByVal __callbackIndex As Integer,
                    ByVal __callbackObject As Integer,
                    ByVal __eventName As String,
                    ByVal __sessionId As Integer,
                    ByVal __callerDisplayName As [String],
                    ByVal __caller As [String],
                    ByVal __calleeDisplayName As [String],
                    ByVal __callee As [String],
                    ByVal __audioCodecNames As [String],
                    ByVal __videoCodecNames As [String],
                    ByVal __existsAudio As [Boolean],
                    ByVal __existsVideo As [Boolean],
                    ByVal __sipMessage As StringBuilder)

        RaiseEvent Event_CallGeneric2(Me, New Event_CallGeneric2_Args(
                    __callbackIndex,
                    __callbackObject,
                    __eventName,
                    __sessionId,
                    __callerDisplayName,
                    __caller,
                    __calleeDisplayName,
                    __callee,
                    __audioCodecNames,
                    __videoCodecNames,
                    __existsAudio,
                    __existsVideo,
                    __sipMessage))
    End Sub
    Public Delegate Sub Event_CallGeneric_Handler(ByVal Sender As Object, ByVal e As Event_CallGeneric_Args)
    Public Event Event_CallGeneric As Event_CallGeneric_Handler
    Public Class Event_CallGeneric_Args
        Inherits EventArgs
        Public callbackIndex As Integer
        Public callbackObject As Integer
        Public eventName As String = ""
        Public sessionId As Int32
        Public data1 As String = ""
        Public data2 As String = ""
        Public Sub New(ByVal __callbackIndex As Integer,
                    ByVal __callbackObject As Integer,
                    ByVal __eventName As String, __sessionId As Integer, ___data1 As String, ___data2 As String)
            callbackIndex = __callbackIndex
            callbackObject = __callbackObject
            eventName = __eventName
            sessionId = __sessionId
            data1 = ___data1
            data2 = ___data2
        End Sub
    End Class
    Private Sub sendGenericEvent(ByVal __callbackIndex As Integer,
                    ByVal __callbackObject As Integer,
                    __eventName As String, __sessionId As Integer, ___data1 As String, ___data2 As String)
        RaiseEvent Event_CallGeneric(Me, New Event_CallGeneric_Args(__callbackIndex, __callbackObject, __eventName, __sessionId, ___data1, ___data2))
    End Sub

#End Region

#Region "Event_Initialize_Handler"
    Public Delegate Sub Event_Initialize_Handler(ByVal Sender As Object, ByVal e As Event_Initialize_Args)
    Public Event Event_Initialize As Event_Initialize_Handler
    Public Class Event_Initialize_Args
        Inherits EventArgs
        Public callbackIndex As Integer
        Public callbackObject As Integer
        Public psudoLineNo As Integer
        Public ip As String = ""
        Public port As Integer = 0
        Public lines As Integer = 0
        Public transport As TRANSPORT_TYPE
        Public Sub New(ByVal __callbackIndex As Integer,
                    ByVal __callbackObject As Integer,
                    __psudoLineNo As Integer, ByVal __IP As String, __Port As Integer, ___lines As Integer, ___transPort As TRANSPORT_TYPE)
            callbackIndex = __callbackIndex
            callbackObject = __callbackObject
            psudoLineNo = __psudoLineNo
            ip = __IP
            port = __Port
            lines = ___lines
            transport = ___transPort
        End Sub

    End Class
#End Region

#Region "Error Sender"
    Public Delegate Sub Event_Error_Handler(ByVal Sender As Object, ByVal e As Event_Error_Args)
    Public Event Event_Error As Event_Error_Handler
    Public Class Event_Error_Args
        Inherits EventArgs
        Public callbackIndex As Integer
        Public callbackObject As Integer
        Public ex As Exception
        Public Sub New(ByVal __callbackIndex As Integer,
                    ByVal __callbackObject As Integer,
                    ByVal sessionId As Int32,
                    ByVal __ex As Exception)

            callbackIndex = __callbackIndex
            callbackObject = __callbackObject
            sessionId = sessionId
            ex = __ex
        End Sub
    End Class
#End Region

#Region "Event_ExtRegister_Handler"
    'ByVal statusText As [String], ByVal statusCode As Int32, sipMessage As StringBuilder


    Public Delegate Sub Event_ExtRegister_Handler(ByVal Sender As Object, ByVal e As Event_ExtRegister_Args)
    Public Event Event_ExtRegister As Event_ExtRegister_Handler
    Public Class Event_ExtRegister_Args
        Inherits EventArgs
        Public callbackIndex As Integer
        Public callbackObject As Integer
        Public registered As Boolean
        Public statusText As String = ""
        Public statusCode As Integer = 0
        Public sipMessage As String
        Public Sub New(ByVal __callbackIndex As Integer,
                    ByVal __callbackObject As Integer,
                    ByVal ___registered As Boolean, ___statusText As String, ___statusCode As Integer, ___sipMessage As String)
            callbackIndex = __callbackIndex
            callbackObject = __callbackObject
            registered = ___registered
            statusText = ___statusText
            statusCode = ___statusCode
            sipMessage = ___sipMessage
        End Sub

    End Class
#End Region

#Region "Devices"
    Public Function Microphone(Optional Mute As Boolean = False) As Boolean
        Microphone = False
        Try

            _Channel(0)._sdkLib.muteMicrophone(Mute)
            Return True
        Catch ex As Exception
            RaiseError(-1, -1, -1, New Exception("Microphone() " & ex.Message))

        End Try
    End Function
    Public Property MicrophoneVolme() As Integer

        Get
            Try
                Return _Channel(0)._sdkLib.getMicVolume
            Catch ex As Exception
                RaiseError(-1, -1, -1, New Exception("MicrophoneVolme(getMicVolume) " & ex.Message))
            End Try

        End Get
        Set(ByVal value As Integer)
            If value > 255 Or value < 0 Then
                RaiseError(-1, -1, -1, New Exception("MicrophoneVolume() " & " value should in in range of 0 to 255"))
            End If
            Try
                _Channel(0)._sdkLib.setMicVolume(value)
            Catch ex As Exception
                RaiseError(-1, -1, -1, New Exception("MicrophoneVolme(setMicVolume) " & ex.Message))
            End Try

        End Set
    End Property
    Public Property SpeakerVolme() As Integer
        Get
            Try
                Return _Channel(0)._sdkLib.getSpeakerVolume
            Catch ex As Exception
                RaiseError(-1, -1, -1, New Exception("SpeakerVolme(getSpeakerVolume) " & ex.Message))
            End Try
        End Get
        Set(ByVal value As Integer)
            If value > 255 Or value < 0 Then
                RaiseError(-1, -1, -1, New Exception("SpeakerVolme() " & " value should in in range of 0 to 255"))
            End If
            Try
                _Channel(0)._sdkLib.setSpeakerVolume(value)
            Catch ex As Exception
                RaiseError(-1, -1, -1, New Exception("SpeakerVolme(setSpeakerVolume) " & ex.Message))
            End Try
        End Set
    End Property
    Public ReadOnly Property getNumMicrophones() As Integer
        Get
            Try
                Return _Channel(0)._sdkLib.getNumOfRecordingDevices()
            Catch ex As Exception
                RaiseError(-1, -1, -1, New Exception("getNumMicrophones() " & ex.Message))
            End Try
        End Get
    End Property
    Public ReadOnly Property getNumSpeakers() As Integer
        Get
            Try
                Return _Channel(0)._sdkLib.getNumOfPlayoutDevices()
            Catch ex As Exception
                RaiseError(-1, -1, -1, New Exception("getNumSpeakers() " & ex.Message))
            End Try
        End Get
    End Property
    Public ReadOnly Property getNumCamras() As Integer
        Get
            Try
                Return _Channel(0)._sdkLib.getNumOfVideoCaptureDevices()
            Catch ex As Exception
                RaiseError(-1, -1, -1, New Exception("getNumCamras() " & ex.Message))
            End Try
        End Get
    End Property
    Public ReadOnly Property getMicrophones(index) As String
        Get
            Dim deviceName As New StringBuilder()
            deviceName.Length = 256
            Try
                If _Channel(0)._sdkLib.getRecordingDeviceName(index, deviceName, 256) = 0 Then
                    Return deviceName.ToString()
                Else
                    RaiseError(-1, -1, -1, New Exception("getMicrophones() " & " Invalid index (" & index & ")"))
                End If
            Catch ex As Exception
                RaiseError(-1, -1, -1, New Exception("getMicrophones() " & ex.Message))
            End Try
        End Get
    End Property
    Public ReadOnly Property getSpeaker(index) As String
        Get
            Dim deviceName As New StringBuilder()
            deviceName.Length = 256
            Try
                If _Channel(0)._sdkLib.getPlayoutDeviceName(index, deviceName, 256) = 0 Then
                    Return deviceName.ToString()
                Else
                    RaiseError(-1, -1, -1, New Exception("getSpeaker() " & " Invalid index (" & index & ")"))
                End If
            Catch ex As Exception
                RaiseError(-1, -1, -1, New Exception("getSpeaker() " & ex.Message))
            End Try
        End Get
    End Property
    Public Function setAudioDeviceId(Optional MicrophoneID As Integer = 1, Optional SpeakerID As Integer = 1) As Boolean
        Try
            _Channel(0)._sdkLib.setAudioDeviceId(MicrophoneID, SpeakerID)
            Return True
        Catch ex As Exception
            RaiseError(-1, -1, -1, New Exception("setAudioDeviceId() " & ex.Message))
        End Try
    End Function
    Public ReadOnly Property getCamra(index) As String
        Get
            Dim uniqueId As New StringBuilder()
            uniqueId.Length = 256
            Dim deviceName As New StringBuilder()
            deviceName.Length = 256
            Try
                If _Channel(0)._sdkLib.getVideoCaptureDeviceName(index, uniqueId, 256, deviceName, 256) = 0 Then
                    Return deviceName.ToString()
                Else
                    RaiseError(-1, -1, -1, New Exception("getCamra() " & " Invalid index (" & index & ")"))
                End If
            Catch ex As Exception
                RaiseError(-1, -1, -1, New Exception("getCamra() " & ex.Message))
            End Try
        End Get
    End Property
    Public WriteOnly Property setCamra() As Boolean
        Set(value As Boolean)
            Try
                _Channel(0)._sdkLib.setVideoDeviceId(value)

            Catch ex As Exception
                RaiseError(-1, -1, -1, New Exception("setCamra() " & ex.Message))
            End Try
        End Set
    End Property

    Private Sub UpdateAudioCodecs(ChannelNo As Integer)
        Try
            _Channel(ChannelNo)._sdkLib.clearAudioCodec()
            _Channel(ChannelNo)._sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_PCMU)
            _Channel(ChannelNo)._sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_PCMA)
            _Channel(ChannelNo)._sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_G729)
            _Channel(ChannelNo)._sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_ILBC)
            _Channel(ChannelNo)._sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_GSM)
            _Channel(ChannelNo)._sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_AMR)
            _Channel(ChannelNo)._sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_G722)
            _Channel(ChannelNo)._sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_SPEEX)
            _Channel(ChannelNo)._sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_AMRWB)
            _Channel(ChannelNo)._sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_SPEEXWB)
            _Channel(ChannelNo)._sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_G7221)
            _Channel(ChannelNo)._sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_OPUS)
            _Channel(ChannelNo)._sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_DTMF)
        Catch ex As Exception
            Throw New ArgumentException("UpdateAudioCodecs(" & ChannelNo & ") " & ex.Message, ex.InnerException)
            RaiseError(ChannelNo, -1, -1, New Exception("UpdateAudioCodecs(" & ChannelNo & ") " & ex.Message))

        End Try

    End Sub
    Private Sub UpdateVideoCodecs(ChannelNo As Integer)
        Try
            _Channel(ChannelNo)._sdkLib.clearVideoCodec()
            _Channel(ChannelNo)._sdkLib.addVideoCodec(VIDEOCODEC_TYPE.VIDEO_CODEC_VP8)
            '_Channel(ChannelNo)._sdkLib.addVideoCodec(VIDEOCODEC_TYPE.VIDEO_CODEC_H263)
            '_Channel(ChannelNo)._sdkLib.addVideoCodec(VIDEOCODEC_TYPE.VIDEO_CODEC_H263_1998)
            '_Channel(ChannelNo)._sdkLib.addVideoCodec(VIDEOCODEC_TYPE.VIDEO_CODEC_H264)
            '_Channel(ChannelNo)._sdkLib.addVideoCodec(VIDEOCODEC_TYPE.VIDEO_CODEC_VP9)
        Catch ex As Exception
            RaiseError(ChannelNo, -1, -1, New Exception("UpdateVideoCodecs(" & ChannelNo & ") " & ex.Message))

        End Try
    End Sub


    Private _VideoRemoteHandle As IntPtr = IntPtr.Zero
    Private _VideoLocalHandle As IntPtr = IntPtr.Zero

    Private Function setRemoteVideoWindow(ChannelNo As Integer, sessionID As Long, remoteVideoWindow As IntPtr) As Boolean
        setRemoteVideoWindow = False
        _VideoRemoteHandle = IntPtr.Zero
        Dim rt As Integer
        Try
            rt = _Channel(ChannelNo)._sdkLib.setRemoteVideoWindow(sessionID, _VideoRemoteHandle)
            If rt <> 0 Then
                Throw New Exception()
                RaiseError(ChannelNo, -1, -1, New Exception("setRemoteVideoWindow(" & ChannelNo & ") failed setRemoteVideoWindow() with error:" & rt))
                Return False
            End If
            _VideoRemoteHandle = remoteVideoWindow
            Return True
        Catch ex As Exception
            RaiseError(ChannelNo, -1, -1, New Exception("setRemoteVideoWindow(" & ChannelNo & ") " & ex.Message))
        End Try
    End Function

    Private Function setLocalVideoWindow(ChannelNo As Integer, Enable As Boolean, localVideoWindow As IntPtr) As Boolean
        setLocalVideoWindow = False
        _VideoLocalHandle = IntPtr.Zero
        Dim rt As Integer
        Try
            'rt = _Channel(0)._sdkLib.displayLocalVideo(Enable, Enable, localVideoWindow)
            rt = _Channel(ChannelNo)._sdkLib.displayLocalVideo(Enable)
            If rt <> 0 Then

                RaiseError(ChannelNo, -1, -1, New Exception("setLocalVideoWindow(" & ChannelNo & ") failed setLocalVideoWindow(" & Enable & ") with error:" & rt))

                Return False
            End If
            _VideoLocalHandle = localVideoWindow
            Return True
        Catch ex As Exception
            RaiseError(ChannelNo, -1, -1, New Exception("setLocalVideoWindow(" & ChannelNo & ") " & ex.Message))
        End Try
    End Function


    Public VideoLocalHandle As IntPtr = IntPtr.Zero



    Public WriteOnly Property CamraDisplay() As Boolean
        Set(value As Boolean)
            Dim rt As Integer
            If value Then
                'rt = _Channel(0)._sdkLib.displayLocalVideo(value, value, VideoLocalHandle)
                rt = _Channel(0)._sdkLib.displayLocalVideo(value)
                If rt <> 0 Then
                    RaiseError(0, -1, -1, New Exception("CamraDisplay() cant enable Camra() error:" & rt))
                End If
            Else
                'rt = _Channel(0)._sdkLib.displayLocalVideo(value, value, IntPtr.Zero)
                rt = _Channel(0)._sdkLib.displayLocalVideo(value)
                If rt <> 0 Then
                    RaiseError(0, -1, -1, New Exception("CamraDisplay() cant disable Camra() error:" & rt))
                End If
            End If
        End Set
    End Property
    Public WriteOnly Property sendVideo(ChannelNo As Integer) As Boolean

        Set(ByVal value As Boolean)
            Dim _CallSessions As Long
            Dim rt As Integer = _Channel(ChannelNo)._sdkLib.sendVideo(_CallSessions, value)
            If rt <> 0 Then
                If rt <> 0 Then
                    RaiseError(0, -1, -1, New Exception("sendVideo(" & ChannelNo & ") failed (" & value & ") error:" & rt))
                End If
            End If

        End Set
    End Property

#End Region

#Region "media functions"

    Public Property Digits(Optional ChannelNo As Integer = 0) As String
        Get
            Return _Channel(ChannelNo)._Digits
        End Get
        Set(ByVal value As String)
            _Channel(ChannelNo)._Digits = value
        End Set
    End Property



    Public Class media_getDigits_Args
        Friend _callbackIndex As Integer
        Friend _callbackObject As Integer
        Friend _sessionID As Long
        Friend _callAsync As Boolean
        Friend _numberOfDigits As Integer
        Friend _numberOfSeconds As Integer
        Friend _bClearDigitBuffer As Boolean = False
        Friend _szStopTone As String

        Public Sub New(ByVal callbackIndex As Integer, ByVal callbackObject As Integer, sessionID As Long, ByVal callAsync As Boolean, numberOfDigits As Integer, numberOfSeconds As Integer, Optional bClearDigitBuffer As Boolean = False, Optional szStopTone As String = "*#0123456789")
            _callbackIndex = callbackIndex
            _callbackObject = callbackObject
            _sessionID = sessionID
            _callAsync = callAsync
            _numberOfDigits = numberOfDigits
            _numberOfSeconds = numberOfSeconds
            _bClearDigitBuffer = bClearDigitBuffer
            _szStopTone = szStopTone
        End Sub
    End Class
    Public Class media_play_Args
        Friend _callbackIndex As Integer
        Friend _callbackObject As Integer
        Friend _sessionID As Long
        Friend _callAsync As Boolean
        Friend _szFileName As String
        Friend _bClearDigitBuffer As Boolean = True
        Friend _szStopTone As String

        Public Sub New(ByVal callbackIndex As Integer, ByVal callbackObject As Integer, sessionID As Long, ByVal CallAsync As Boolean, szFileName As String, Optional bClearDigitBuffer As Boolean = True, Optional szStopTone As String = "*#0123456789")
            _callbackIndex = callbackIndex
            _callbackObject = callbackObject
            _sessionID = sessionID
            _callAsync = CallAsync
            _szFileName = szFileName
            _bClearDigitBuffer = bClearDigitBuffer
            _szStopTone = szStopTone
        End Sub
    End Class
    Public Function media_stopPlay(ByVal callbackIndex As Integer, ByVal callbackObject As Integer, sessionID As Long) As String
        Try
            _Channel(callbackIndex)._stopPlaying = True
            _Channel(callbackIndex)._Playing = False
            media_stopPlay = "Stop"
        Catch ex As Exception
            media_stopPlay = "Failed-" & ex.Message
            RaiseError(callbackIndex, callbackObject, sessionID, New Exception("media_stopPlay(" & callbackIndex & ") " & "failed with error (" & ex.Message & ")"))
        End Try
    End Function
    Public Function media_stopGetDigits(ByVal callbackIndex As Integer, ByVal callbackObject As Integer, sessionID As Long) As String
        Try
            _Channel(callbackIndex)._stopGettingDigits = True
            media_stopGetDigits = "Stop"
        Catch ex As Exception
            media_stopGetDigits = "Failed-" & ex.Message
            RaiseError(callbackIndex, callbackObject, sessionID, New Exception("media_stopGetDigits(" & callbackIndex & ") " & "failed with error (" & ex.Message & ")"))

        End Try
    End Function

    Public Function media_getDigits(e As media_getDigits_Args) As String
        media_getDigits = ""
        Try
            If _Channel(e._callbackIndex)._GetDigits = True Then
                Throw New Exception("media_getDigits(" & e._callbackIndex & ") " & "Already Getting Digits(s) (" & "" & ")")
                Exit Function
            End If
            Try
                If Not IsNothing(_Channel(e._callbackIndex).Thread_Media_GetDigits) Then
                    If _Channel(e._callbackIndex).Thread_Media_GetDigits.ThreadState = Threading.ThreadState.Running Then
                        _Channel(e._callbackIndex)._stopPlaying = True
                        _Channel(e._callbackIndex).Thread_Media_GetDigits.Abort()
                        _Channel(e._callbackIndex).Thread_Media_GetDigits = Nothing
                    End If
                End If
            Catch ex As Exception
            End Try
            _Channel(e._callbackIndex).Thread_Media_GetDigits = New Thread(AddressOf thMedia_getDigits)
            _Channel(e._callbackIndex).Thread_Media_GetDigits.Name = "Media_GetDigits_Line_" & e._callbackIndex
            _Channel(e._callbackIndex).Thread_Media_GetDigits.IsBackground = True
            _Channel(e._callbackIndex).Thread_Media_GetDigits.Start(e)
            Return "Started"
        Catch ex As Exception
            media_getDigits = "Failed-" & ex.Message
            Throw New Exception("media_getDigits(" & e._callbackIndex & ") " & "failed with error (" & ex.Message & ")")
        End Try
    End Function

    Public Function media_play(e As media_play_Args) As String
        media_play = ""
        Try
            If _Channel(e._callbackIndex)._Playing = True Then
                Throw New Exception("media_play(" & e._callbackIndex & ") " & "Already Playing file(s) (" & "" & ")")
                Exit Function
            End If
            Try
                If Not IsNothing(_Channel(e._callbackIndex).Thread_Media_Player) Then
                    If _Channel(e._callbackIndex).Thread_Media_Player.ThreadState = Threading.ThreadState.Running Then
                        _Channel(e._callbackIndex)._stopPlaying = True
                        _Channel(e._callbackIndex).Thread_Media_Player.Abort()
                        _Channel(e._callbackIndex).Thread_Media_Player = Nothing
                        _Channel(e._callbackIndex)._sdkLib.stopPlayAudioFileToRemote(e._sessionID)
                    End If
                End If
            Catch ex As Exception
            End Try
            _Channel(e._callbackIndex).Thread_Media_Player = New Thread(AddressOf thMedia_Player)
            _Channel(e._callbackIndex).Thread_Media_Player.Name = "Media_Player_Line_" & e._callbackIndex
            _Channel(e._callbackIndex).Thread_Media_Player.IsBackground = True
            _Channel(e._callbackIndex).Thread_Media_Player.Start(e)
            Return "Started"
        Catch ex As Exception
            media_play = "Failed-" & ex.Message
            Throw New Exception("media_play(" & e._callbackIndex & ") " & "failed with error (" & ex.Message & ")")
        End Try
    End Function
    Friend Function thMedia_getDigits(e As media_getDigits_Args) As Boolean
        thMedia_getDigits = False
        Dim retVal As Integer = 0
        Dim CurrentDigit As String
        _Channel(e._callbackIndex)._stopGettingDigits = False
        If Trim(e._szStopTone) = "" Then e._szStopTone = "0123456789*#"
        If e._bClearDigitBuffer = True Then
            _Channel(e._callbackIndex)._Digits = ""
        End If
        Try
            _Channel(e._callbackIndex)._GetDigits = True
            Dim _waitStartTime As DateTime = Now
            sendGenericEvent(e._callbackIndex, e._callbackObject, "onGetDigitsStart", e._sessionID, "" & retVal, "" & _Channel(e._callbackIndex)._Digits)
            While True
                'If _Channel(e._callbackIndex)._inCall Then
                'retVal = _Channel(e._callbackIndex)._sdkLib.stopPlayAudioFileToRemote(e._sessionID)
                'retVal = 4
                'Exit While
                ' Check if call is finished exit the loop
                'End If
                If _Channel(e._callbackIndex)._stopGettingDigits = True Then
                    retVal = 3
                    Exit While
                End If

                If UCase(e._szStopTone) <> "A" Then
                    CurrentDigit = Right(_Channel(e._callbackIndex)._Digits, 1)
                    If CurrentDigit <> "" Then
                        If e._szStopTone.IndexOf(CurrentDigit) >= 0 Then
                            retVal = 2
                            Exit While
                        End If
                    End If
                End If
                If _Channel(e._callbackIndex)._Digits.Length >= e._numberOfDigits Then
                    retVal = 1
                    Exit While
                End If
                If DateDiff(DateInterval.Second, _waitStartTime, Now) >= e._numberOfSeconds Then
                    retVal = 0
                    Exit While
                End If
                Threading.Thread.Sleep(20)
            End While

            Select Case retVal
                Case 0 ' Input Timeout
                Case 1 ' Max Digits
                Case 2 ' szStopTone Received
                Case 3 ' User Stop
                Case 4 ' call Ended
                Case Else ' error playing this file
            End Select
            sendGenericEvent(e._callbackIndex, e._callbackObject, "onGetDigitsFinished", e._sessionID, "" & retVal, "" & _Channel(e._callbackIndex)._Digits)
            thMedia_getDigits = True
        Catch ex As Exception
            _Channel(e._callbackIndex)._GetDigits = False
            RaiseError(e._callbackIndex, e._callbackObject, e._sessionID, New Exception("thMedia_getDigits(" & e._callbackIndex & ") " & "failed with error (" & ex.Message & ")"))
        End Try
        _Channel(e._callbackIndex)._GetDigits = False
    End Function

    Friend Function thMedia_Player(e As media_play_Args) As Boolean
        thMedia_Player = False
        Dim retVal As Integer = 0
        Dim szWAVFileName As String = ""
        Dim CurrentDigit As String

        _Channel(e._callbackIndex)._stopPlaying = False
        If Trim(e._szStopTone) = "" Then e._szStopTone = "0123456789*#"
        If e._bClearDigitBuffer = True Then
            _Channel(e._callbackIndex)._Digits = ""
        End If
        Try
            _Channel(e._callbackIndex)._Playing = True
            Dim _fileNames() As String = e._szFileName.Split(",".ToCharArray)
            Dim _FileName As String = ""
            For index = 0 To _fileNames.Length - 1
                _FileName = _fileNames(index)
                'Next
                'For Each _FileName As String In _fileNames
                'If _Channel(e._callbackIndex)._inCall Then
                'retVal = _Channel(e._callbackIndex)._sdkLib.stopPlayAudioFileToRemote(e._sessionID)
                'retVal = 4
                ' Check if call is finished exit the loop
                'End If
                If Trim(_FileName) = "" Then
                    retVal = -1
                    GoTo NextFile
                End If
                szWAVFileName = _FoldersVoice & "\" & Trim(_FileName)
                If File.Exists(szWAVFileName) = False Then
                    retVal = 1
                    GoTo NextFile
                End If
                Dim exMessage As String = ""
                Try
                    retVal = _Channel(e._callbackIndex)._sdkLib.playAudioFileToRemote(e._sessionID, szWAVFileName, 16000, False)
                Catch ex As Exception
                    exMessage = ex.Message
                    retVal = 1000
                    GoTo NextFile
                End Try
                _Channel(e._callbackIndex)._currentFilePlayed = False
                retVal = 0
                While _Channel(e._callbackIndex)._currentFilePlayed = False
                    '' stay in loop while this file is being played
                    If UCase(e._szStopTone) <> "A" Then
                        CurrentDigit = Right(_Channel(e._callbackIndex)._Digits, 1)
                        If CurrentDigit <> "" Then
                            If e._szStopTone.IndexOf(CurrentDigit) >= 0 Then
                                retVal = _Channel(e._callbackIndex)._sdkLib.stopPlayAudioFileToRemote(e._sessionID)
                                retVal = 2
                                Exit While
                            End If
                            CurrentDigit = ""
                        End If
                    End If
                    If _Channel(e._callbackIndex)._stopPlaying = True Then
                        retVal = _Channel(e._callbackIndex)._sdkLib.stopPlayAudioFileToRemote(e._sessionID)
                        retVal = 3
                        Exit While
                    End If
                    Thread.Sleep(20)
                End While
NextFile:
                sendGenericEvent(e._callbackIndex, e._callbackObject, "onPlayAudioFileProgress", e._sessionID, _FileName, "[" & index & "/" & _fileNames.Length - 1 & "]")
                Select Case retVal
                    Case -1 ' blank File name ignore
                    Case 0 ' keep playing
                    Case 1 ' file not found
                    Case 2 ' szStopTone Received
                        Exit For
                    Case 3 ' stop playing by Applicaiton
                        Exit For
                    Case 4 ' call Ended
                        Exit For
                    Case Else ' error playing this file
                End Select
            Next
            sendGenericEvent(e._callbackIndex, e._callbackObject, "onPlayAudioFileFinished", e._sessionID, "" & retVal, "" & _Channel(e._callbackIndex)._Digits)
            thMedia_Player = True
        Catch ex As Exception
            _Channel(e._callbackIndex)._Playing = False
            RaiseError(e._callbackIndex, e._callbackObject, e._sessionID, New Exception("thMedia_Player(" & e._callbackIndex & ") " & "failed with error (" & ex.Message & ")"))
        End Try
        _Channel(e._callbackIndex)._Playing = False

    End Function
#End Region


#Region "Servers Configuration"
    Private _Channel(0) As _ChannelsStructDef
    Private Structure _ChannelsStructDef

        Public Thread_Media_Recorder As Thread
        Public Thread_Media_Recorder_status As Boolean


        Public _recfileData() As Byte
        Public _recfileDataWritenAt As DateTime
        Public _recInterval As Integer
        Public _recFileName As String
        Public _recDoCerateFile As Boolean


        Public Thread_Media_Player As Thread
        Public _Playing As Boolean

        Public Thread_Media_GetDigits As Thread
        Public _GetDigits As Boolean

        Public _currentFilePlayed As Boolean
        Public _stopPlaying As Boolean
        Public _stopGettingDigits As Boolean

        Public _Digits As String

        Public _sdkLib As PortSIPLib
        Public _SessionTimeout As Integer
        Public _RegisterTimeout As Integer
        Public _UseRandomLocalPort As Boolean
        Public _RTPPortRange As Integer
        Public _UserAgentName As String
        Public _VideoResolution As Enum_VideoResolution
        Public _VideoWidth As Long
        Public _VideoHeight As Long
        Public _VideoQuality As Integer
        Public _Transport As TRANSPORT_TYPE
        Public _SRTP As SRTP_POLICY
        Public _psudoLineNo As Integer
        Public _LocalPort As Long
        Public _DND As Boolean
        Public _Initialized As Boolean
        Public _LINE_BASE As Integer
        Public _MAX_LINES As Integer
        Public _Registered As Integer
        '-1 = not registered
        ' 0 = registering
        ' 1 = registered
        ' 2 = registering


        Public _EC As Boolean
        Public _VAD As Boolean
        Public _CNG As Boolean
        Public _AGC As Boolean
        Public _ANS As Boolean
        Public _NACK As Boolean
        Public _SDP As Boolean


        Public _User_AuthName As String
        Public _User_UserName As String
        Public _User_Display As String
        Public _User_Secret As String
        Public _User_Domain As String
        Public _User_RealM As String

        Public _SIPServerIP As String
        Public _SIPServerPort As Long
        Public _SIPStunIP As String
        Public _SIPStunPort As Long
        Public _SIPProxtIP As String
        Public _SIPProxtPort As Long
    End Structure

    Public Property User_AuthName(Optional ChannelNo As Integer = 0) As String
        Get
            Return _Channel(ChannelNo)._User_AuthName
        End Get
        Set(ByVal value As String)
            _Channel(ChannelNo)._User_AuthName = value
        End Set
    End Property

    Public Property User_UserName(Optional ChannelNo As Integer = 0) As String
        Get
            Return _Channel(ChannelNo)._User_UserName
        End Get
        Set(ByVal value As String)
            _Channel(ChannelNo)._User_UserName = value
        End Set
    End Property
    Public Property User_Display(Optional ChannelNo As Integer = 0) As String
        Get
            Return _Channel(ChannelNo)._User_Display
        End Get
        Set(ByVal value As String)
            _Channel(ChannelNo)._User_Display = value
        End Set
    End Property
    Public Property User_Secret(Optional ChannelNo As Integer = 0) As String
        Get
            Return _Channel(ChannelNo)._User_Secret
        End Get
        Set(ByVal value As String)
            _Channel(ChannelNo)._User_Secret = value
        End Set
    End Property
    Public Property User_Domain(Optional ChannelNo As Integer = 0) As String
        Get
            Return _Channel(ChannelNo)._User_Domain
        End Get
        Set(ByVal value As String)
            _Channel(ChannelNo)._User_Domain = value
        End Set
    End Property
    Public Property User_RealM(Optional ChannelNo As Integer = 0) As String
        Get
            Return _Channel(ChannelNo)._User_RealM
        End Get
        Set(ByVal value As String)
            _Channel(ChannelNo)._User_RealM = value
        End Set
    End Property


    Public Property SessionTimeout(Optional ChannelNo As Integer = 0) As Integer
        Get
            Return _Channel(ChannelNo)._SessionTimeout
        End Get
        Set(value As Integer)
            Try
                Dim rt As Integer = 0
                rt = _Channel(ChannelNo)._sdkLib.enableSessionTimer(_Channel(ChannelNo)._SessionTimeout, SESSION_REFRESH_MODE.SESSION_REFERESH_UAC)
                rt = _Channel(ChannelNo)._sdkLib.enableSessionTimer(_Channel(ChannelNo)._SessionTimeout, SESSION_REFRESH_MODE.SESSION_REFERESH_UAS)
                If rt <> 0 Then
                    Throw New Exception("SessionTimeout(" & ChannelNo & ") " & " enableSessionTimer failed with error (" & rt & ")")
                    Return
                End If
                _Channel(ChannelNo)._SessionTimeout = value
            Catch ex As Exception
                Throw New Exception("SessionTimeout(" & ChannelNo & ") " & " enableSessionTimer failed with error (" & ex.Message & ")")
            End Try
        End Set

    End Property
    Public Property RegisterTimeout(Optional ChannelNo As Integer = 0) As Integer
        Get
            Return _Channel(ChannelNo)._RegisterTimeout
        End Get
        Set(value As Integer)
            _Channel(ChannelNo)._RegisterTimeout = value
        End Set
    End Property
    Public Property UseRandomLocalPort(Optional ChannelNo As Integer = 0) As Boolean
        Get
            Return _Channel(ChannelNo)._UseRandomLocalPort
        End Get
        Set(value As Boolean)
            _Channel(ChannelNo)._UseRandomLocalPort = value
        End Set
    End Property
    Public ReadOnly Property RTPPortRange(Optional ChannelNo As Integer = 0) As Integer
        Get
            Return _Channel(ChannelNo)._RTPPortRange
        End Get
    End Property
    Public Property UserAgentName(Optional ChannelNo As Integer = 0) As String
        Get
            Return _Channel(ChannelNo)._UserAgentName
        End Get
        Set(ByVal value As String)
            _Channel(ChannelNo)._UserAgentName = value
        End Set
    End Property
#Region "VideoQuality"
    Public Property VideoQuality(Optional ChannelNo As Integer = 0) As Integer
        Get
            Return _Channel(ChannelNo)._VideoQuality
        End Get
        Set(ByVal Quality As Integer)
            _Channel(ChannelNo)._VideoQuality = Quality
        End Set
    End Property
#End Region

#Region "VideoResolution"
    Public Enum Enum_VideoResolution As Integer
        Resolution_QCIF = 0
        Resolution_CIF = 1
        Resolution_VGA = 2
        Resolution_SVGA = 3
        Resolution_XVGA = 4
        Resolution_720P = 5
        Resolution_QVGA = 6
    End Enum
    Public Property VideoResolution(Optional ChannelNo As Integer = 0) As Enum_VideoResolution
        Get
            Return _Channel(ChannelNo)._VideoResolution
        End Get
        Set(ByVal value As Enum_VideoResolution)
            Select Case value
                Case Enum_VideoResolution.Resolution_CIF
                    _Channel(ChannelNo)._VideoWidth = 352
                    _Channel(ChannelNo)._VideoHeight = 288
                    Exit Select
                Case Enum_VideoResolution.Resolution_VGA
                    _Channel(ChannelNo)._VideoWidth = 640
                    _Channel(ChannelNo)._VideoHeight = 480
                    Exit Select
                Case Enum_VideoResolution.Resolution_SVGA
                    _Channel(ChannelNo)._VideoWidth = 800
                    _Channel(ChannelNo)._VideoHeight = 600
                    Exit Select
                Case Enum_VideoResolution.Resolution_XVGA
                    _Channel(ChannelNo)._VideoWidth = 1024
                    _Channel(ChannelNo)._VideoHeight = 768
                    Exit Select
                Case Enum_VideoResolution.Resolution_720P
                    _Channel(ChannelNo)._VideoWidth = 1280
                    _Channel(ChannelNo)._VideoHeight = 720
                    Exit Select
                Case Enum_VideoResolution.Resolution_QVGA
                    _Channel(ChannelNo)._VideoWidth = 320
                    _Channel(ChannelNo)._VideoHeight = 240
                    Exit Select
                Case Else ' 0
                    _Channel(ChannelNo)._VideoWidth = 176
                    _Channel(ChannelNo)._VideoHeight = 144
                    Exit Select
            End Select
            Try
                _Channel(ChannelNo)._sdkLib.setVideoResolution(_Channel(ChannelNo)._VideoWidth, _Channel(ChannelNo)._VideoHeight)
            Catch ex As Exception
                Throw New ArgumentException("setVideoResolution(" & ChannelNo & ") " & ex.Message, ex.InnerException)
            End Try

        End Set
    End Property

#End Region

#Region "AudioCodecs"

#End Region

    Public Enum VIDEOCODEC_TYPE As Integer
        VIDEO_CODE_NONE = -1
        ''''< Do not use Video codec
        VIDEO_CODEC_I420 = 113
        ''''< I420/YUV420 Raw Video format. Used with startRecord only 
        VIDEO_CODEC_H263 = 34
        ''''< H263 video codec
        VIDEO_CODEC_H263_1998 = 115
        ''''< H263+/H263 1998 video codec
        VIDEO_CODEC_H264 = 125
        ''''< H264 video codec
        VIDEO_CODEC_VP8 = 120
        ''''< VP8 video code
        VIDEO_CODEC_VP9 = 122
        ''''< VP9 video code
    End Enum
    Public Enum AUDIO_RECORDING_FILEFORMAT As Integer
        FILEFORMAT_WAVE = 1
        ''''<	The record audio file is in WAVE format. 
        FILEFORMAT_AMR
        ''''<	The record audio file is in AMR format - all voice data are compressed by AMR codec. 
    End Enum
    '''The audio/Video record mode
    Public Enum RECORD_MODE As Integer
        RECORD_NONE = 0
        ''''<	Not Record. 
        RECORD_RECV = 1
        ''''<	Only record the received data. 
        RECORD_SEND
        ''''<	Only record the sent data. 
        RECORD_BOTH
        ''''<	The record audio file is in WAVE format. 
    End Enum
    Public Enum SRTP_POLICY As Integer
        SRTP_POLICY_NONE = 0
        ''''< Do not use SRTP. The SDK can receive the encrypted call(SRTP) and unencrypted call both, but can't place outgoing encrypted call. 
        SRTP_POLICY_FORCE
        ''''< All calls must use SRTP. The SDK allows to receive encrypted call and place outgoing encrypted call only.
        SRTP_POLICY_PREFER
        ''''< Top priority for using SRTP. The SDK allows to receive encrypted and decrypted call, and to place outgoing encrypted call and unencrypted call.
    End Enum
    Public Enum TRANSPORT_TYPE As Integer
        TRANSPORT_UDP = 0
        ''''< UDP Transport
        TRANSPORT_TLS
        ''''< Tls Transport
        TRANSPORT_TCP
        ''''< TCP Transport
        TRANSPORT_PERS
        ''''< PERS is the PortSIP private transport for anti SIP blocking. It must be used with the PERS Server.
    End Enum
    Public Property Transport(Optional ChannelNo As Integer = 0) As TRANSPORT_TYPE
        Get
            Return _Channel(ChannelNo)._Transport
        End Get
        Set(ByVal value As TRANSPORT_TYPE)
            _Channel(ChannelNo)._Transport = value
        End Set
    End Property
    Public Property SRTP(Optional ChannelNo As Integer = 0) As SRTP_POLICY
        Get
            Return _Channel(ChannelNo)._SRTP
        End Get
        Set(ByVal value As SRTP_POLICY)
            _Channel(ChannelNo)._SRTP = value
        End Set
    End Property

#Region "Servers"


    Public Property SIPServerIP(Optional ChannelNo As Integer = 0) As String
        Get
            Return _Channel(ChannelNo)._SIPServerIP
        End Get
        Set(ByVal value As String)
            _Channel(ChannelNo)._SIPServerIP = value
        End Set
    End Property
    Public Property SIPServerPort(Optional ChannelNo As Integer = 0) As String
        Get
            Return _Channel(ChannelNo)._SIPServerPort
        End Get
        Set(ByVal value As String)
            _Channel(ChannelNo)._SIPServerPort = value
        End Set
    End Property

    Public Property SIPStunIP(Optional ChannelNo As Integer = 0) As String
        Get
            Return _Channel(ChannelNo)._SIPStunIP
        End Get
        Set(ByVal value As String)
            _Channel(ChannelNo)._SIPStunIP = value
        End Set
    End Property
    Public Property SIPStunPort(Optional ChannelNo As Integer = 0) As String
        Get
            Return _Channel(ChannelNo)._SIPStunPort
        End Get
        Set(ByVal value As String)
            _Channel(ChannelNo)._SIPStunPort = value
        End Set
    End Property

    Public Property SIPProxtIP(Optional ChannelNo As Integer = 0) As String
        Get
            Return _Channel(ChannelNo)._SIPProxtIP
        End Get
        Set(ByVal value As String)
            _Channel(ChannelNo)._SIPProxtIP = value
        End Set
    End Property


    Public Property SIPProxtPort(Optional ChannelNo As Integer = 0) As String
        Get
            Return _Channel(ChannelNo)._SIPProxtPort
        End Get
        Set(ByVal value As String)
            _Channel(ChannelNo)._SIPProxtPort = value
        End Set
    End Property

#End Region

#End Region

#Region "User Configuration"
    Public ReadOnly Property Registered(Optional ChannelNo As Integer = 0) As Integer
        Get
            Return _Channel(ChannelNo)._Registered
        End Get
    End Property


#End Region

    Public Property psudoLineNo(Optional ChannelNo As Integer = 0) As Integer
        Get
            Return _Channel(ChannelNo)._psudoLineNo
        End Get
        Set(value As Integer)
            _Channel(ChannelNo)._psudoLineNo = value
        End Set
    End Property
    Public Property LocalPort(Optional ChannelNo As Integer = 0) As Long
        Get
            Return _Channel(ChannelNo)._LocalPort
        End Get
        Set(value As Long)
            If _Channel(ChannelNo)._UseRandomLocalPort = False Then
                _Channel(ChannelNo)._LocalPort = value
            End If
        End Set
    End Property

    Public Property DND(Optional ChannelNo As Integer = 0) As Boolean
        Get
            Return _Channel(ChannelNo)._DND
        End Get
        Set(ByVal DND As Boolean)
            Try
                _Channel(ChannelNo)._sdkLib.setDoNotDisturb(DND)
                _Channel(ChannelNo)._DND = DND
            Catch ex As Exception
                Throw New ArgumentException("DND(" & ChannelNo & ") " & ex.Message, ex.InnerException)
            End Try
        End Set
    End Property
    Private _localIPAddress As String
    Public WriteOnly Property localIPAddress() As String

        Set(ByVal value As String)
            Try
                Dim _NICNums As Integer = _Channel(0)._sdkLib.getNICNums
                If _Channel(0)._Initialized Then
                    RaiseError(-1, -1, -1, New Exception("localIPAddress() " & " Control is initialized"))
                    Exit Property
                End If
                If Trim(value) = "0.0.0.0" Then
                    _localIPAddress = value
                    Exit Property
                End If
                Dim i As Integer = 0
                For i = 0 To _NICNums
                    Dim _NICIP As StringBuilder
                    _NICIP = New StringBuilder
                    _NICIP.Length = 64
                    Dim rt As Integer
                    rt = _Channel(0)._sdkLib.getLocalIpAddress(i, _NICIP, 64)
                    If _NICIP.ToString.Trim = value.Trim Then
                        _localIPAddress = _NICIP.ToString.Trim
                        Exit Property
                    End If
                Next
                RaiseError(-1, -1, -1, New Exception("localIPAddress() " & " Invalid IP Address provided(" & value & ")"))
            Catch ex As Exception
                RaiseError(-1, -1, -1, ex)
            End Try
        End Set
    End Property
    Public ReadOnly Property localIPAddress(index) As String
        Get
            Dim _NICIP As StringBuilder
            _NICIP = New StringBuilder
            _NICIP.Length = 64
            Try
                Dim rt As Integer
                rt = _Channel(0)._sdkLib.getLocalIpAddress(index, _NICIP, 64)
                Return _NICIP.ToString()
            Catch ex As Exception
                RaiseError(-1, -1, -1, New Exception("localIPAddress() " & ex.Message))
                Return ""
            End Try
        End Get
    End Property
    Public ReadOnly Property NICCount() As Integer
        Get
            Try
                Return _Channel(0)._sdkLib.getNICNums()
            Catch ex As Exception
                NICCount = 0
                RaiseError(-1, -1, -1, New Exception(("NICCount() " & ex.Message)))
            End Try
        End Get
    End Property

    Private _FoldersLog As String
    Public Property FoldersLog() As String
        Get
            Return _FoldersLog
        End Get
        Set(ByVal value As String)
            If Directory.Exists(value) Then
                _FoldersLog = value
            Else
                RaiseError(-1, -1, -1, New Exception("FoldersLog() " & " Invalid path (" & value & ")"))
            End If
        End Set
    End Property

    Private _FoldersVoice As String = ""
    Public Property FoldersVoice() As String
        Get
            Return _FoldersVoice
        End Get
        Set(ByVal value As String)
            If Directory.Exists(value) Then
                _FoldersVoice = value
            Else
                RaiseError(-1, -1, -1, New Exception("FoldersVoice() " & " Invalid path (" & value & ")"))
            End If
        End Set
    End Property

    Private _FoldersRecord As String = ""
    Public Property FoldersRecord() As String
        Get
            Return _FoldersRecord
        End Get
        Set(ByVal value As String)
            If Directory.Exists(value) Then
                _FoldersVoice = value
            Else
                RaiseError(-1, -1, -1, New Exception("FoldersRecord() " & " Invalid path (" & value & ")"))
            End If
        End Set
    End Property

    Public ReadOnly Property Initialized(Optional ChannelNo As Integer = 0) As Boolean
        Get
            Return _Channel(ChannelNo)._Initialized
        End Get
    End Property


    Public Property MAX_LINES(Optional ChannelNo As Integer = 0) As Integer
        Get
            Return _Channel(ChannelNo)._MAX_LINES
        End Get
        Set(ByVal value As Integer)
            _Channel(ChannelNo)._MAX_LINES = value
        End Set
    End Property

    Private Sub SetupLogFolder(Optional LogFolder As String = "")
        Try
            If LogFolder = "" Then
                _FoldersLog = Path.GetTempPath() & "ovsPSProPhony"
            Else
                _FoldersLog = LogFolder
            End If
            If Directory.Exists(_FoldersLog) = False Then
                Directory.CreateDirectory(_FoldersLog)
            End If
            _FoldersLog = _FoldersLog & "\" & Now.ToString("yyyyMMddHHmmssffff")
            Directory.CreateDirectory(_FoldersLog)
        Catch ex As Exception
            RaiseError(-1, -1, -1, New Exception("SetupLogFolder(" & _FoldersLog & ") " & " Error (" & ex.Message & ")"))
        End Try
    End Sub
    Private Sub DoStartupStuff(Optional ChannelNo As Integer = 0)
        Try

            ReDim _Channel(ChannelNo)._recfileData(0)
            _Channel(ChannelNo)._recInterval = 2
            _Channel(ChannelNo)._recFileName = ""
            _Channel(ChannelNo)._recDoCerateFile = False

            _Channel(ChannelNo).Thread_Media_Recorder_status = False
            _Channel(ChannelNo)._psudoLineNo = -1
            _coreMode = -1

            _Channel(ChannelNo)._Digits = ""
            _Channel(ChannelNo)._UseRandomLocalPort = True
            _Channel(ChannelNo)._RegisterTimeout = 30
            _Channel(ChannelNo)._UserAgentName = "CoreProPhonyV4"
            _Channel(ChannelNo)._Registered = -1
            _Channel(ChannelNo)._LocalPort = -1
            _localIPAddress = "0.0.0.0"
            _Channel(ChannelNo)._Transport = TRANSPORT_TYPE.TRANSPORT_UDP
            _Channel(ChannelNo)._SRTP = SRTP_POLICY.SRTP_POLICY_NONE

            _Channel(ChannelNo)._VideoResolution = Enum_VideoResolution.Resolution_CIF
            _Channel(ChannelNo)._VideoWidth = 352
            _Channel(ChannelNo)._DND = True
            _Channel(ChannelNo)._EC = True
            _Channel(ChannelNo)._VAD = False
            _Channel(ChannelNo)._CNG = False
            _Channel(ChannelNo)._AGC = False
            _Channel(ChannelNo)._ANS = True
            _Channel(ChannelNo)._NACK = True
            _Channel(ChannelNo)._SDP = False
            _Channel(ChannelNo)._MAX_LINES = 2

            _Channel(ChannelNo).Thread_Media_Recorder_status = False
            _Channel(ChannelNo).Thread_Media_Recorder = New Thread(Sub() thMedia_Recorder(ChannelNo))
            _Channel(ChannelNo).Thread_Media_Recorder.IsBackground = True
            _Channel(ChannelNo).Thread_Media_Recorder.Name = "thMedia_Recorder-CH-" & ChannelNo
            _Channel(ChannelNo).Thread_Media_Recorder.Priority = ThreadPriority.Highest
            sendGenericEvent(ChannelNo, -1, "thMedia_Recorder", -1, "Starting", "")
            _Channel(ChannelNo).Thread_Media_Recorder.Start()
            sendGenericEvent(ChannelNo, -1, "thMedia_Recorder", -1, "Started", "")
        Catch ex As Exception
            RaiseError(ChannelNo, -1, -1, New Exception("DoStartupStuff(" & ChannelNo & ") " & ex.Message))
        End Try
    End Sub
    Private Sub EnableAllAudioCodecs(Optional ChannelNo As Integer = 0)
        Try
            _Channel(ChannelNo)._sdkLib.clearAudioCodec()
            _Channel(ChannelNo)._sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_PCMU)
            _Channel(ChannelNo)._sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_PCMA)
            _Channel(ChannelNo)._sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_G729)
            _Channel(ChannelNo)._sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_ILBC)
            _Channel(ChannelNo)._sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_GSM)
            _Channel(ChannelNo)._sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_AMR)
            _Channel(ChannelNo)._sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_G722)
            _Channel(ChannelNo)._sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_SPEEX)
            _Channel(ChannelNo)._sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_AMRWB)
            _Channel(ChannelNo)._sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_SPEEXWB)
            _Channel(ChannelNo)._sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_G7221)
            _Channel(ChannelNo)._sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_OPUS)
            _Channel(ChannelNo)._sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_DTMF)
        Catch ex As Exception
            RaiseError(ChannelNo, -1, -1, New Exception("EnableAllAudioCodecs(" & ChannelNo & ") " & "Control is not Initialized"))
        End Try
    End Sub
    Private Sub EnableAllVideoCodecs(Optional ChannelNo As Integer = 0, Optional Codecs As VIDEOCODEC_TYPE = VIDEOCODEC_TYPE.VIDEO_CODEC_VP8)
        Try
            _Channel(ChannelNo)._sdkLib.clearVideoCodec()
            '_Channel(ChannelNo)._sdkLib.addVideoCodec(VIDEOCODEC_TYPE.VIDEO_CODEC_H263)
            '_Channel(ChannelNo)._sdkLib.addVideoCodec(VIDEOCODEC_TYPE.VIDEO_CODEC_H263_1998)
            '_Channel(ChannelNo)._sdkLib.addVideoCodec(VIDEOCODEC_TYPE.VIDEO_CODEC_H264)
            _Channel(ChannelNo)._sdkLib.addVideoCodec(VIDEOCODEC_TYPE.VIDEO_CODEC_VP8)
            '_Channel(ChannelNo)._sdkLib.addVideoCodec(VIDEOCODEC_TYPE.VIDEO_CODEC_VP9)
        Catch ex As Exception
            RaiseError(ChannelNo, -1, -1, New Exception("EnableAllVideoCodecs(" & ChannelNo & ") " & "Control is not Initialized"))
        End Try

    End Sub


    Public Sub New(Optional LogFolder As String = "")
        Try
            _Channels = 1
            _coreMode = 1
            ' Create and set the SIP callback handers, this MUST called before
            _Channel(0)._sdkLib = New PortSIPLib(0, 0, Me)
            ' Create and set the SIP callback handers, this MUST called before
            _Channel(0)._sdkLib.createCallbackHandlers()

            If Directory.Exists(LogFolder) = False Then
                LogFolder = ""
            End If
            SetupLogFolder(LogFolder)
            DoStartupStuff(0)
        Catch ex As Exception
            RaiseError(-1, -1, -1, New Exception("New() " & ex.Message))
        End Try
    End Sub
    Private Sub RaiseError(callbackIndex As Integer, callbackObject As Integer, ByVal sessionId As Int32, ex As Exception)
        Try
            RaiseEvent Event_Error(Me, New Event_Error_Args(callbackIndex, callbackObject, sessionId, ex))
        Catch exx As Exception
            Debug.WriteLine("RaiseError:callbackIndex=" & callbackIndex & ", callbackObject:" & callbackObject & ", sessionId=" & sessionId & ", exx=" & exx.Message)
        End Try
    End Sub
    Private Sub _writeLog(msg As String)
        Try

        Catch ex As Exception
            Throw New Exception("_writeLog() " & "Control is not Initialized")
        End Try
    End Sub
    Public Function ExtRegister(Optional ChannelNo As Integer = 0, Optional bRegister As Boolean = True) As Boolean
        ExtRegister = False
        Dim rt As Long = 0

        Try
            If Not _Channel(ChannelNo)._Initialized Then
                RaiseError(ChannelNo, -1, -1, New Exception("ExtRegister(" & ChannelNo & ") " & "Control is not Initialized"))
                Exit Function
            End If

            If bRegister Then
                If _Channel(ChannelNo)._Registered <> -1 Then
                    RaiseError(ChannelNo, -1, -1, New Exception("ExtRegister(" & ChannelNo & ") " & "Already Registered"))
                    ExtRegister = False
                    Exit Function
                End If

                rt = _Channel(ChannelNo)._sdkLib.setUser(_Channel(ChannelNo)._User_UserName,
                                    _Channel(ChannelNo)._User_Display,
                                    _Channel(ChannelNo)._User_AuthName,
                                    _Channel(ChannelNo)._User_Secret,
                                    _Channel(ChannelNo)._User_Domain,
                                _Channel(ChannelNo)._SIPServerIP,
                                _Channel(ChannelNo)._SIPServerPort,
                                _Channel(ChannelNo)._SIPStunIP,
                                _Channel(ChannelNo)._SIPStunPort,
                                _Channel(ChannelNo)._SIPProxtIP,
                                _Channel(ChannelNo)._SIPProxtPort)
                If rt <> 0 Then
                    RaiseError(ChannelNo, -1, -1, New Exception("ExtRegister(setUser(" & ChannelNo & ")) " & "Failed to registered with error code:" & rt))
                    Return False
                End If
                rt = _Channel(ChannelNo)._sdkLib.registerServer(_Channel(ChannelNo)._RegisterTimeout, 0)
                If rt <> 0 Then
                    _Channel(ChannelNo)._sdkLib.removeUser()
                    RaiseError(ChannelNo, -1, -1, New Exception("ExtRegister(registerServer(" & ChannelNo & ")) " & "Failed to registered with error code:" & rt))
                    Return False
                End If
                _Channel(ChannelNo)._Registered = 0
                Return True
            End If
            _Channel(ChannelNo)._DND = True
            _Channel(ChannelNo)._sdkLib.setDoNotDisturb(True)
            _Channel(ChannelNo)._sdkLib.unRegisterServer()
            _Channel(ChannelNo)._sdkLib.removeUser()
            _Channel(ChannelNo)._Registered = -1
            RaiseEvent Event_ExtRegister(Me, New Event_ExtRegister_Args(ChannelNo, -1, False, "unRegister", 0, ""))
            Return True
        Catch ex As Exception
            RaiseError(ChannelNo, -1, -1, New Exception("ExtRegister(" & ChannelNo & ") " & ex.Message))
        End Try

    End Function
    Public Function InitializeChannel(Optional ChannelNo As Integer = 1, Optional MAX_LINES As Integer = 2, Optional CoreMode As Integer = 0, Optional FoldersVoice As String = "", Optional Transport As TRANSPORT_TYPE = TRANSPORT_TYPE.TRANSPORT_UDP) As Boolean
        InitializeChannel = False
        Try
            If ((ChannelNo < 0) Or (ChannelNo > (_Channels - 1))) Then
                RaiseError(ChannelNo, -1, -1, New Exception("InitializeChannel(" & ChannelNo & ") " & "Channel not exist "))
                Exit Function
            End If
            _Channel(ChannelNo)._sdkLib = New PortSIPLib(ChannelNo, 0, Me)
            _Channel(ChannelNo)._sdkLib.createCallbackHandlers()
            Try
                DoStartupStuff(ChannelNo)
                _coreMode = CoreMode
                _FoldersVoice = FoldersVoice
                _Channel(ChannelNo)._MAX_LINES = MAX_LINES
                _Channel(ChannelNo)._Transport = Transport
                _Initialize(ChannelNo)
                Thread.Sleep(20)
            Catch ex As Exception
                RaiseError(ChannelNo, -1, -1, New Exception("InitializeChannel(" & ChannelNo & ") " & ex.Message))
            End Try
        Catch ex As Exception
            RaiseError(ChannelNo, -1, -1, New Exception("InitializeChannel(" & ChannelNo & ") " & ex.Message))
        End Try
    End Function

    Public Function Initialize(Optional MAX_LINES As Integer = 2, Optional CoreMode As Integer = 0, Optional FoldersVoice As String = "", Optional Transport As TRANSPORT_TYPE = TRANSPORT_TYPE.TRANSPORT_UDP) As Boolean
        Initialize = False
        Try
            For index = 0 To _Channels - 1
                InitializeChannel(index, MAX_LINES, CoreMode, FoldersVoice, Transport)
                Thread.Sleep(20)
            Next
        Catch ex As Exception
            RaiseError(-1, -1, -1, New Exception("Initialize() " & ex.Message))
            Exit Function
        End Try
    End Function

    Private Function _Initialize(ChannelNo As Integer) As Boolean
        _Initialize = False

        Try
            If _Channel(ChannelNo)._psudoLineNo <> 1 Then
                _Channel(ChannelNo)._psudoLineNo = 901 + ChannelNo
            End If

            If _Channel(ChannelNo)._UseRandomLocalPort Or (_Channel(ChannelNo)._LocalPort = 0) Then
                ' Generate the random port for SIP
                '_Channel(ChannelNo)._LocalPort = rd.[Next](1000, 5000) + (4000 + _Channel(ChannelNo)._psudoLineNo)
                _Channel(ChannelNo)._LocalPort = 21001 + ChannelNo
                Debug.WriteLine("LineNo:" & ChannelNo & ", _LocalPort:" & _Channel(ChannelNo)._LocalPort)
            End If

            Dim rt As Int32 = 0
            Try
                '' SP  = 0 
                '' IVR = 1
                rt = _Channel(ChannelNo)._sdkLib.initialize(_Channel(ChannelNo)._Transport, _localIPAddress, _Channel(ChannelNo)._LocalPort, _logLevel, _FoldersLog, _Channel(ChannelNo)._MAX_LINES, _Channel(ChannelNo)._UserAgentName, _coreMode, _coreMode, "\", "", False)
            Catch ex As Exception
                RaiseError(-1, -1, -1, New Exception("Initialize(" & ChannelNo & ") Error:" & rt))
                Exit Function
            End Try
            If rt <> 0 Then
                _Channel(ChannelNo)._sdkLib.releaseCallbackHandlers()
                RaiseError(-1, -1, -1, New Exception("Initialize(" & ChannelNo & ") Error:" & rt))
                Return False
            End If
            _Channel(ChannelNo)._sdkLib.disableSessionTimer()
            _Channel(ChannelNo)._sdkLib.enableAutoCheckMwi(False)
            _Channel(ChannelNo)._sdkLib.setSrtpPolicy(_Channel(ChannelNo)._SRTP, True)
            Dim LicenceKey As String = "1WINPh4xODlGMENFRERCOTFFOUQ1QkZFRjJBNkJDNEMzRjZFMUA4REMxRTUxQUY1ODg5QTk4MTNFREY4MEQzMEFGQzY3Q0BBQTVGRjg3QkE0MTA1Qjc1NUMwNUM2MjRDMTYyNDkyRUBFNDU4MDFEMzEzMEU2OTVDNDY3M0NEQUZDRTRCNTE3RA"
            '"PORTSIP_TEST_LICENSE"
            rt = _Channel(ChannelNo)._sdkLib.setLicenseKey(LicenceKey)
            If rt = PortSIP_Errors.ECoreTrialVersionLicenseKey Then
            End If
            If rt = PortSIP_Errors.ECoreWrongLicenseKey Then
                _Channel(ChannelNo)._sdkLib.releaseCallbackHandlers()
                RaiseError(-1, -1, -1, New Exception("Initialize(" & ChannelNo & ") Error:" & "The wrong license key was detected, please check contact OVS Sales (" & rt & ")"))
                Return False
            End If
            ' Set The RTP Ports
            Dim TryCount As Integer = 0
TryAgain:
            Try
                TryCount = TryCount + 1
                rt = SetRTPPortRange(ChannelNo)
                If rt <> 0 Then
                    If TryCount < 3 Then
                        Debug.WriteLine("SetRTPPortRange(" & ChannelNo & "):" & rt)
                        GoTo TryAgain
                    End If
                End If
            Catch ex As Exception
                If TryCount < 3 Then
                    GoTo TryAgain
                End If
                _Channel(ChannelNo)._sdkLib.releaseCallbackHandlers()
                RaiseError(-1, -1, -1, New Exception("Initialize(" & ChannelNo & ") SetRTPPortRange SETUP FAILED Error:" & ex.Message))
                Return False
            End Try

            Dim _M, _J As Integer
            rt = _Channel(ChannelNo)._sdkLib.getVersion(_M, _J)
            EnableAllVideoCodecs(ChannelNo)
            EnableAllAudioCodecs(ChannelNo)
            _Channel(ChannelNo)._sdkLib.enableAEC(True)
            _Channel(ChannelNo)._sdkLib.enableAEC(False)
            _Channel(ChannelNo)._sdkLib.enableCNG(False)
            _Channel(ChannelNo)._sdkLib.enableAGC(False)
            _Channel(ChannelNo)._sdkLib.enableANS(False)
            _Channel(ChannelNo)._sdkLib.setVideoNackStatus(True)
            _Channel(ChannelNo)._sdkLib.enableReliableProvisional(False)
            '_Channel(ChannelNo)._sdkLib.displayLocalVideo(False, False, IntPtr.Zero)
            _Channel(ChannelNo)._sdkLib.displayLocalVideo(False)
            _Channel(ChannelNo)._Initialized = True
            _Initialize = True
            RaiseEvent Event_Initialize(Me, New Event_Initialize_Args(ChannelNo, -1, _Channel(ChannelNo)._psudoLineNo, "Version(" & _M & "." & _J & ") ,LineNo:" & _Channel(ChannelNo)._psudoLineNo & ",IP:" & _localIPAddress, _Channel(ChannelNo)._LocalPort, _Channel(ChannelNo)._MAX_LINES, _Channel(ChannelNo)._Transport))
        Catch ex As Exception
            RaiseError(-1, -1, -1, New Exception("Initialize(" & ChannelNo & ") " & ex.Message))
        End Try
    End Function
    Private Function SetRTPPortRange(ChannelNo As Integer) As Integer
        SetRTPPortRange = -1
        Try
            Dim rd As New Random()
            Dim minimumRtpAudioPort As Integer = 20001 + ((ChannelNo + 1) * 100)
            minimumRtpAudioPort = rd.[Next](minimumRtpAudioPort / 2, (minimumRtpAudioPort + 499) / 2) * 2
            Dim maximumRtpAudioPort As Int32 = (minimumRtpAudioPort + 1) + 499

            Dim minimumRtpVideoPort As Int32 = 0

            minimumRtpVideoPort = rd.[Next]((maximumRtpAudioPort + 1) / 2, (maximumRtpAudioPort + 499) / 2) * 2
            Dim maximumRtpVideoPort As Int32 = minimumRtpVideoPort + 499

            Dim PortText As String = "Ports:_LocalPort:" & _Channel(ChannelNo)._LocalPort & ", minimumRtpAudioPort:" & minimumRtpAudioPort & " , maximumRtpAudioPort:" & maximumRtpAudioPort & " , minimumRtpVideoPort:" & minimumRtpVideoPort & " , maximumRtpVideoPort:" & maximumRtpVideoPort
            Debug.WriteLine("setRtpPortRange:" & PortText)
            SetRTPPortRange = _Channel(ChannelNo)._sdkLib.setRtpPortRange(minimumRtpAudioPort, maximumRtpAudioPort, minimumRtpVideoPort, maximumRtpVideoPort)
            If SetRTPPortRange <> 0 Then
                RaiseError(-1, -1, -1, New Exception("SetRTPPortRange(" & ChannelNo & ") Error:" & "Failed to set setRtpPortRange with error (" & SetRTPPortRange & ") " & PortText))
            End If
        Catch ex As Exception
            RaiseError(-1, -1, -1, New Exception("SetRTPPortRange(" & ChannelNo & ") " & ex.Message))
        End Try
    End Function
#Region "SPEvents"

    'Friend Function onScreenRawCallback(ByVal callbackObject As IntPtr, ByVal sessionId As Int32, ByVal callbackType As Int32, ByVal width As Int32, ByVal height As Int32, <MarshalAs(UnmanagedType.LPArray, SizeParamIndex:=6)> ByVal data As Byte(),ByVal dataLength As Int32) As Int32 Implements SIPCallbackEvents.onScreenRawCallback
    '
    '                !!! IMPORTANT !!!
    '
    '                Don't call any PortSIP SDK API functions in here directly. If you want to call the PortSIP API functions or 
    '                other code which will spend long time, you should post a message to main thread(main window) or other thread,
    '                let the thread to call SDK API functions or other code.
    '
    '                The video data format is YUV420, YV12.
    '            


    '
    ' IMPORTANT: the data length is stored in dataLength parameter!!!
    '

    'Dim type As DIRECTION_MODE = CType(callbackType, DIRECTION_MODE)


    'If type = DIRECTION_MODE.DIRECTION_SEND Then

    'ElseIf type = DIRECTION_MODE.DIRECTION_RECV Then
    '    End If


    'Return 0

    'End Function

    Friend Function onRemoteHold(callbackIndex As Integer, callbackObject As Integer, ByVal sessionId As Int32) As Int32 Implements SIPCallbackEvents.onRemoteHold
        sendGenericEvent(callbackIndex, callbackObject, "onRemoteHold", sessionId, "", "")
        Return 0
    End Function
    Friend Function onRemoteUnHold(callbackIndex As Integer, callbackObject As Integer, ByVal sessionId As Int32,
                                ByVal audioCodecNames As [String],
                                ByVal videoCodecNames As [String],
                                ByVal existsAudio As [Boolean],
                                ByVal existsVideo As [Boolean]) As Int32 Implements SIPCallbackEvents.onRemoteUnHold

        Return 0
    End Function
    Friend Function onReceivedRefer(callbackIndex As Integer, callbackObject As Integer,
                                    ByVal sessionId As Int32,
                                    ByVal referId As Int32,
                                    ByVal [to] As [String],
                                    ByVal from As [String],
    ByVal referSipMessage As StringBuilder) As Int32 Implements SIPCallbackEvents.onReceivedRefer
        sendGenericEvent(callbackIndex, callbackObject, "onReceivedRefer", sessionId, "referId:" & referId, "[to]:" & [to] & ", from:" & from)
        Return 0

    End Function

    Friend Function onReferAccepted(callbackIndex As Integer,
                                    callbackObject As Integer,
                                    ByVal sessionId As Int32) As Int32 Implements SIPCallbackEvents.onReferAccepted
        sendGenericEvent(callbackIndex, callbackObject, "onReferAccepted", sessionId, "", "")
        Return 0
    End Function


    Friend Function onReferRejected(callbackIndex As Integer, callbackObject As Integer, ByVal sessionId As Int32, ByVal reason As [String], ByVal code As Int32) As Int32 Implements SIPCallbackEvents.onReferRejected
        sendGenericEvent(callbackIndex, callbackObject, "onReferRejected", sessionId, reason.ToString, code.ToString)
        Return 0
    End Function

    Friend Function onTransferTrying(callbackIndex As Integer, callbackObject As Integer, ByVal sessionId As Int32) As Int32 Implements SIPCallbackEvents.onTransferTrying
        sendGenericEvent(callbackIndex, callbackObject, "onTransferTrying", sessionId, "", "")
        Return 0
    End Function

    Friend Function onTransferRinging(callbackIndex As Integer, callbackObject As Integer, ByVal sessionId As Int32) As Int32 Implements SIPCallbackEvents.onTransferRinging
        sendGenericEvent(callbackIndex, callbackObject, "onTransferRinging", sessionId, "", "")
        Return 0
    End Function

    Friend Function onACTVTransferSuccess(callbackIndex As Integer, callbackObject As Integer, ByVal sessionId As Int32) As Int32 Implements SIPCallbackEvents.onACTVTransferSuccess
        sendGenericEvent(callbackIndex, callbackObject, "onACTVTransferSuccess", sessionId, "", "")
        Return 0
    End Function


    Friend Function onACTVTransferFailure(callbackIndex As Integer, callbackObject As Integer, ByVal sessionId As Int32,
                                        ByVal reason As [String],
                                        ByVal code As Int32) As Int32 Implements SIPCallbackEvents.onACTVTransferFailure
        sendGenericEvent(callbackIndex, callbackObject, "onACTVTransferFailure", sessionId, reason.ToString, code.ToString)

        '  reason is error reason
        '  code is error code

        Return 0
    End Function

    Friend Function onReceivedSignaling(callbackIndex As Integer, callbackObject As Integer, ByVal sessionId As Int32, ByVal signaling As StringBuilder) As Int32 Implements SIPCallbackEvents.onReceivedSignaling
        ' This event will be fired when the SDK received a SIP message
        ' you can use signaling to access the SIP message.
        sendGenericEvent(callbackIndex, callbackObject, "onReceivedSignaling", sessionId, signaling.ToString, "")

        Return 0
    End Function


    Friend Function onSendingSignaling(callbackIndex As Integer, callbackObject As Integer, ByVal sessionId As Int32, ByVal signaling As StringBuilder) As Int32 Implements SIPCallbackEvents.onSendingSignaling
        ' This event will be fired when the SDK sent a SIP message
        ' you can use signaling to access the SIP message.
        sendGenericEvent(callbackIndex, callbackObject, "onSendingSignaling", sessionId, signaling.ToString, "")

        Return 0
    End Function




    Friend Function onWaitingVoiceMessage(callbackIndex As Integer, callbackObject As Integer, ByVal messageAccount As [String],
                                        ByVal urgentNewMessageCount As Int32,
                                        ByVal urgentOldMessageCount As Int32,
                                        ByVal newMessageCount As Int32,
                                        ByVal oldMessageCount As Int32) As Int32 Implements SIPCallbackEvents.onWaitingVoiceMessage

        ' You can use these parameters to check the voice message count

        '  urgentNewMessageCount;
        '  urgentOldMessageCount;
        '  newMessageCount;
        '  oldMessageCount;

        Return 0
    End Function

    Friend Function onInviteFailure(callbackIndex As Integer, callbackObject As Integer, ByVal sessionId As Int32, ByVal reason As [String], ByVal code As Int32, ByVal sipMessage As StringBuilder) As Int32 Implements SIPCallbackEvents.onInviteFailure
        sendGenericEvent(callbackIndex, callbackObject, "onInviteFailure", sessionId, reason.ToString, code.ToString)
        If Not _Channel(0)._DND Then
            DND = True
        End If
        Return 0
    End Function

    Friend Function onWaitingFaxMessage(callbackIndex As Integer, callbackObject As Integer, ByVal messageAccount As [String],
                                    ByVal urgentNewMessageCount As Int32,
                                    ByVal urgentOldMessageCount As Int32,
                                    ByVal newMessageCount As Int32,
    ByVal oldMessageCount As Int32) As Int32 Implements SIPCallbackEvents.onWaitingFaxMessage


        ' You can use these parameters to check the FAX message count

        '  urgentNewMessageCount;
        '  urgentOldMessageCount;
        '  newMessageCount;
        '  oldMessageCount;

        Return 0
    End Function


    Friend Function onRecvDtmfTone(callbackIndex As Integer, callbackObject As Integer, ByVal sessionId As Int32, ByVal tone As Int32) As Int32 Implements SIPCallbackEvents.onRecvDtmfTone
        Select Case tone
            Case 0 To 9 : _Channel(callbackIndex)._Digits = _Channel(callbackIndex)._Digits & tone.ToString
            Case 10 : _Channel(callbackIndex)._Digits = _Channel(callbackIndex)._Digits & "*"
            Case 11 : _Channel(callbackIndex)._Digits = _Channel(callbackIndex)._Digits & "#"
        End Select
        If _Channel(callbackIndex)._GetDigits = False Then
            sendGenericEvent(callbackIndex, callbackObject, "onRecvDtmfTone", sessionId, Right(_Channel(callbackIndex)._Digits, 1), "")
        End If
        Return 0
    End Function


    Friend Function onPresenceRecvSubscribe(callbackIndex As Integer, callbackObject As Integer, ByVal subscribeId As Int32, ByVal fromDisplayName As [String], ByVal from As [String], ByVal subject As [String]) As Int32 Implements SIPCallbackEvents.onPresenceRecvSubscribe
        sendGenericEvent(callbackIndex, callbackObject, "onPresenceOnline", -2, fromDisplayName.ToString, from & "|" & subject)
        Return 0
    End Function


    Friend Function onPresenceOnline(callbackIndex As Integer, callbackObject As Integer, ByVal fromDisplayName As [String], ByVal from As [String], ByVal stateText As [String]) As Int32 Implements SIPCallbackEvents.onPresenceOnline
        sendGenericEvent(callbackIndex, callbackObject, "onPresenceOnline", -2, fromDisplayName.ToString, from)
        Return 0
    End Function

    Friend Function onPresenceOffline(callbackIndex As Integer, callbackObject As Integer, ByVal fromDisplayName As [String], ByVal from As [String]) As Int32 Implements SIPCallbackEvents.onPresenceOffline
        sendGenericEvent(callbackIndex, callbackObject, "onPresenceOffline", -2, fromDisplayName.ToString, from)
        Return 0
    End Function


    Friend Function onRecvOptions(callbackIndex As Integer, callbackObject As Integer, ByVal optionsMessage As StringBuilder) As Int32 Implements SIPCallbackEvents.onRecvOptions
        '         string text = "Received an OPTIONS message: ";
        '       text += optionsMessage.ToString();
        '     MessageBox.Show(text, "Received an OPTIONS message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        sendGenericEvent(callbackIndex, callbackObject, "onRecvOptions", -2, optionsMessage.ToString, "")

        Return 0
    End Function

    Friend Function onRecvInfo(callbackIndex As Integer, callbackObject As Integer, ByVal infoMessage As StringBuilder) As Int32 Implements SIPCallbackEvents.onRecvInfo
        sendGenericEvent(callbackIndex, callbackObject, "onRecvInfo", -2, infoMessage.ToString, "")
        Return 0
    End Function

    Friend Function onRecvNotifyOfSubscription(callbackIndex As Integer,
                                    callbackObject As Integer,
                                    subscribeId As Int32, notifyMsg As StringBuilder, contentData As Byte(), contentLenght As Int32) As Int32 Implements SIPCallbackEvents.onRecvNotifyOfSubscription

        Return 0
    End Function

    Friend Function onSubscriptionFailure(callbackIndex As Integer, callbackObject As Integer, subscribeId As Int32, statusCode As Int32) As Int32 Implements SIPCallbackEvents.onSubscriptionFailure
        Return 0
    End Function

    Friend Function onSubscriptionTerminated(callbackIndex As Integer, callbackObject As Integer, subscribeId As Int32) As Int32 Implements SIPCallbackEvents.onSubscriptionTerminated

        Return 0
    End Function


    Friend Function onRecvMessage(callbackIndex As Integer, callbackObject As Integer, ByVal sessionId As Int32, ByVal mimeType As [String], ByVal subMimeType As [String], ByVal messageData As Byte(), ByVal messageDataLength As Int32) As Int32 Implements SIPCallbackEvents.onRecvMessage
        sendGenericEvent(callbackIndex, callbackObject, "OnReceMessage", sessionId, mimeType, subMimeType)
        Return 0
    End Function


    Friend Function onRecvOutOfDialogMessage(callbackIndex As Integer, callbackObject As Integer, ByVal fromDisplayName As [String], ByVal from As [String], ByVal toDisplayName As [String], ByVal [to] As [String],
    ByVal mimeType As [String], ByVal subMimeType As [String], ByVal messageData As Byte(), ByVal messageDataLength As Int32) As Int32 Implements SIPCallbackEvents.onRecvOutOfDialogMessage

        Return 0
    End Function

    Friend Function onSendMessageSuccess(callbackIndex As Integer, callbackObject As Integer, ByVal sessionId As Int32, ByVal messageId As Int32) As Int32 Implements SIPCallbackEvents.onSendMessageSuccess
        sendGenericEvent(callbackIndex, callbackObject, "onSendMessageSuccess", sessionId, messageId.ToString, "")
        Return 0
    End Function


    Friend Function onSendMessageFailure(callbackIndex As Integer, callbackObject As Integer, ByVal sessionId As Int32, ByVal messageId As Int32, ByVal reason As [String], ByVal code As Int32) As Int32 Implements SIPCallbackEvents.onSendMessageFailure
        Return 0
    End Function

    Friend Function onSendOutOfDialogMessageSuccess(callbackIndex As Integer, callbackObject As Integer, ByVal messageId As Int32, ByVal fromDisplayName As [String], ByVal from As [String], ByVal toDisplayName As [String],
    ByVal [to] As [String]) As Int32 Implements SIPCallbackEvents.onSendOutOfDialogMessageSuccess


        Return 0
    End Function

    Friend Function onSendOutOfDialogMessageFailure(callbackIndex As Integer, callbackObject As Integer, ByVal messageId As Int32, ByVal fromDisplayName As [String], ByVal from As [String], ByVal toDisplayName As [String], ByVal [to] As [String], ByVal reason As [String], ByVal code As Int32) As Int32 Implements SIPCallbackEvents.onSendOutOfDialogMessageFailure
        Return 0
    End Function


    Friend Function onPlayAudioFileFinished(callbackIndex As Integer, callbackObject As Integer, ByVal sessionId As Int32, ByVal fileName As [String]) As Int32 Implements SIPCallbackEvents.onPlayAudioFileFinished
        _Channel(callbackIndex)._currentFilePlayed = True
        Return 0
    End Function

    Friend Function onPlayVideoFileFinished(callbackIndex As Integer, callbackObject As Integer, ByVal sessionId As Int32) As Int32 Implements SIPCallbackEvents.onPlayVideoFileFinished
        sendGenericEvent(callbackIndex, callbackObject, "onPlayVideoFileFinished", sessionId, "", "")
        Return 0
    End Function


    'Friend Function onRTPPacketCallback(callbackObject As IntPtr, sessionId As Int32, mediaType As Int32, direction As Int32, RTPPacket As Byte(), packetSize As Int32) As Int32 Implements SIPCallbackEvents.onRTPPacketCallback
    '
    '                !!! IMPORTANT !!!
    '
    '                Don't call any PortSIP SDK API functions in here directly. If you want to call the PortSIP API functions or 
    '                other code which will spend long time, you should post a message to main thread(main window) or other thread,
    '                let the thread to call SDK API functions or other code.
    '
    '            


    'Return 0
    'End Function

    Friend Function onAudioRawCallback(callbackObject As IntPtr, sessionId As Int32, callbackType As Int32, <MarshalAs(UnmanagedType.LPArray, SizeParamIndex:=4)> data As Byte(), dataLength As Int32, samplingFreqHz As Int32) As Int32 Implements SIPCallbackEvents.onAudioRawCallback

        'Dim dataDetials As New _class_recordingData
        'dataDetials.data = data
        'dataDetials.dataLength = dataLength
        'dataDetials.samplingFreqHz = samplingFreqHz
        '_recordingData.Enqueue(dataDetials)

        Dim numInt As Integer = callbackObject.ToInt32()

        If IsNothing(_Channel(callbackObject)._recfileData) Then
            ReDim _Channel(callbackObject)._recfileData(data.Length - 1)
        End If
        _Channel(callbackObject)._recfileData = _Channel(callbackObject)._recfileData.Concat(data).ToArray()

        Console.WriteLine("onAudioRawCallback()" & " sessionId:" & sessionId & " data:" & data.Length & " dataLength:" & dataLength & " samplingFreqHz:" & samplingFreqHz & ", _fileData:" & _Channel(callbackObject)._recfileData.Length)
        '
        '                !!! IMPORTANT !!!
        '
        '                Don't call any PortSIP SDK API functions in here directly. If you want to call the PortSIP API functions or 
        '                other code which will spend long time, you should post a message to main thread(main window) or other thread,
        '                let the thread to call SDK API functions or other code.
        '
        '            


        ' The data parameter is audio stream as PCM format, 16bit, Mono.
        ' the dataLength parameter is audio steam data length.



        '
        ' IMPORTANT: the data length is stored in dataLength parameter!!!
        '

        'Dim type As DIRECTION_MODE = CType(callbackType, DIRECTION_MODE)

        'If type = DIRECTION_MODE.DIRECTION_SEND Then
        ' The callback data is from local record device of each session, use the sessionId to identifying the session.
        'ElseIf type = DIRECTION_MODE.DIRECTION_RECV Then
        ' The callback data is received from remote side of each session, use the sessionId to identifying the session.
        'End If




        Return 0
    End Function


    Friend Function onVideoRawCallback(callbackObject As IntPtr, sessionId As Int32, callbackType As Int32, width As Int32, height As Int32, <MarshalAs(UnmanagedType.LPArray, SizeParamIndex:=6)> data As Byte(),
        dataLength As Int32) As Int32 Implements SIPCallbackEvents.onVideoRawCallback
        '
        '                !!! IMPORTANT !!!
        '
        '                Don't call any PortSIP SDK API functions in here directly. If you want to call the PortSIP API functions or 
        '                other code which will spend long time, you should post a message to main thread(main window) or other thread,
        '                let the thread to call SDK API functions or other code.
        '
        '                The video data format is YUV420, YV12.
        '            


        '
        ' IMPORTANT: the data length is stored in dataLength parameter!!!
        '

        'Dim type As DIRECTION_MODE = CType(callbackType, DIRECTION_MODE)
        'If type = DIRECTION_MODE.DIRECTION_SEND Then
        'ElseIf type = DIRECTION_MODE.DIRECTION_RECV Then
        'End If


        Return 0

    End Function

    Friend Function onRegisterSuccess(callbackIndex As Integer, callbackObject As Integer, statusText As [String], statusCode As Int32, sipMessage As StringBuilder) As Int32 Implements PortSIP.SIPCallbackEvents.onRegisterSuccess
        _Channel(callbackIndex)._Registered = 1
        _Channel(callbackIndex)._DND = True
        _Channel(callbackIndex)._sdkLib.setDoNotDisturb(True)
        RaiseEvent Event_ExtRegister(Me, New Event_ExtRegister_Args(callbackIndex, callbackObject, True, statusText, statusCode, sipMessage.ToString))
        Return 0
    End Function

    Friend Function onRegisterFailure(callbackIndex As Integer, callbackObject As Integer, ByVal statusText As [String], ByVal statusCode As Int32, sipMessage As StringBuilder) As Int32 Implements SIPCallbackEvents.onRegisterFailure
        _Channel(callbackIndex)._Registered = -1
        RaiseEvent Event_ExtRegister(Me, New Event_ExtRegister_Args(callbackIndex, callbackObject, False, statusText, statusCode, sipMessage.ToString))

        Return 0
    End Function
    Friend Function onDialogStateUpdated(callbackIndex As Integer, callbackObject As Integer, BLFMonitoredUri As [String],
                                        BLFDialogState As [String],
                                        BLFDialogId As [String],
                                        BLFDialogDirection As [String]) As Int32 Implements SIPCallbackEvents.onDialogStateUpdated
        sendGenericEvent(callbackIndex, callbackObject, "onDialogStateUpdated", -2, BLFDialogState, BLFDialogId & "|" & BLFDialogDirection)

        Return 0
    End Function
    Friend Function onInviteClosed(callbackIndex As Integer, callbackObject As Integer, ByVal sessionId As Int32) As Int32 Implements SIPCallbackEvents.onInviteClosed
        Microphone(False)
        Call_StopRecord(callbackIndex, sessionId)
        sendGenericEvent(callbackIndex, callbackObject, "onInviteClosed", sessionId, "", "")
        If Not _Channel(callbackIndex)._DND Then
            DND = True
        End If
        Return 0
    End Function
    Function onSendingRtpPacket(callbackObject As IntPtr, sessionId As Integer, isAudio As Boolean, RTPPacket As Byte(), packetSize As Integer) As Integer Implements SIPCallbackEvents.onSendingRtpPacket
        Console.WriteLine("onSendingRtpPacket()" & " sessionId:" & sessionId & " isAudio:" & isAudio & " RTPPacket:" & RTPPacket.Length & " packetSize:" & packetSize)
        Return 0
    End Function
    Function onReceivedRtpPacket(callbackObject As IntPtr, sessionId As Integer, isAudio As Boolean, RTPPacket As Byte(), packetSize As Integer) As Integer Implements SIPCallbackEvents.onReceivedRtpPacket
        Console.WriteLine("onReceivedRtpPacket()" & " sessionId:" & sessionId & " isAudio:" & isAudio & " RTPPacket:" & RTPPacket.Length & " packetSize:" & packetSize)
        Return 0
    End Function ' for interface 'SIPCallbackEvents'

    Friend Function onInviteBeginingForward(callbackIndex As Integer, callbackObject As Integer, ByVal forwardTo As [String]) As Int32 Implements SIPCallbackEvents.onInviteBeginingForward
        Dim Text As String = "An incoming call was forwarded to: "
        Text = Text & forwardTo
        sendGenericEvent(callbackIndex, callbackObject, "onInviteBeginingForward", -2, forwardTo, "")

        Return 0
    End Function
    Friend Function onInviteConnected(callbackIndex As Integer, callbackObject As Integer, ByVal sessionId As Int32) As Int32 Implements SIPCallbackEvents.onInviteConnected
        sendGenericEvent(callbackIndex, callbackObject, "onInviteConnected", sessionId, "", "")
        If Not _Channel(callbackIndex)._DND Then
            DND = True
        End If
        Return 0
    End Function

    Friend Function onInviteTrying(callbackIndex As Integer, callbackObject As Integer, ByVal sessionId As Int32) As Int32 Implements SIPCallbackEvents.onInviteTrying
        sendGenericEvent(callbackIndex, callbackObject, "onInviteTrying", sessionId, "", "")
        DND = True
        Return 0
    End Function

    Friend Function onInviteRinging(callbackIndex As Integer,
                                    callbackObject As Integer,
                                    ByVal sessionId As Int32,
                                ByVal statusText As [String],
                                ByVal statusCode As Int32,
                                ByVal sipMessage As StringBuilder) As Int32 Implements SIPCallbackEvents.onInviteRinging
        sendGenericEvent(callbackIndex, callbackObject, "onInviteRinging", sessionId, statusText, statusCode.ToString & "|" & sipMessage.ToString)
        If Not _Channel(callbackIndex)._DND Then
            DND = True
        End If

        Return 0
    End Function

    Friend Function onInviteUpdated(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, audioCodecNames As [String], videoCodecNames As [String], existsAudio As [Boolean],
        existsVideo As [Boolean], sipMessage As StringBuilder) As Int32 Implements SIPCallbackEvents.onInviteUpdated

        sendGeneric2Event(callbackIndex, callbackObject, "onInviteSessionProgress", sessionId,
                                    "",
                                    "",
                                    "",
                                    "",
                                    audioCodecNames,
                                    videoCodecNames,
                                    existsAudio,
                                    existsVideo,
                                    sipMessage)

        Return 0
    End Function

    Friend Function onInviteSessionProgress(callbackIndex As Integer,
                                        callbackObject As Integer,
                                        ByVal sessionId As Int32,
                                        ByVal audioCodecNames As [String],
                                        ByVal videoCodecNames As [String],
                                        ByVal existsEarlyMedia As [Boolean],
                                        ByVal existsAudio As [Boolean],
                                        ByVal existsVideo As [Boolean],
                                        ByVal sipMessage As StringBuilder) As Int32 Implements SIPCallbackEvents.onInviteSessionProgress

        sendGeneric2Event(callbackIndex, callbackObject, "onInviteSessionProgress", sessionId,
                                    "",
                                    "",
                                    "",
                                    "",
                                    audioCodecNames,
                                    videoCodecNames,
                                    existsAudio,
                                    existsVideo,
                                    sipMessage)
        Return 0
    End Function

    Friend Function onInviteAnswered(callbackIndex As Integer,
                                    callbackObject As Integer,
                                    ByVal sessionId As Int32,
                                    ByVal callerDisplayName As [String],
                                    ByVal caller As [String],
                                    ByVal calleeDisplayName As [String],
                                    ByVal callee As [String],
                                    ByVal audioCodecNames As [String],
                                    ByVal videoCodecNames As [String],
                                    ByVal existsAudio As [Boolean],
                                    ByVal existsVideo As [Boolean],
                                    ByVal sipMessage As StringBuilder) As Int32 Implements SIPCallbackEvents.onInviteAnswered

        sendGeneric2Event(callbackIndex, callbackObject, "onInviteAnswered", sessionId,
                                    callerDisplayName,
                                    caller,
                                    calleeDisplayName,
                                    callee,
                                    audioCodecNames,
                                    videoCodecNames,
                                    existsAudio,
                                    existsVideo,
                                    sipMessage)

        If Not _Channel(callbackIndex)._DND Then
            DND = True
        End If
        Return 0
    End Function

    Friend Function onInviteIncoming(callbackIndex As Integer,
                                    callbackObject As Integer,
                                    ByVal sessionId As Int32,
                                    ByVal callerDisplayName As [String],
                                    ByVal caller As [String],
                                    ByVal calleeDisplayName As [String],
                                    ByVal callee As [String],
                                    ByVal audioCodecNames As [String],
                                    ByVal videoCodecNames As [String],
                                    ByVal existsAudio As [Boolean],
                                    ByVal existsVideo As [Boolean],
                                    ByVal sipMessage As StringBuilder) As Int32 Implements SIPCallbackEvents.onInviteIncoming

        sendGeneric2Event(callbackIndex, callbackObject, "onInviteIncoming", sessionId,
                                    callerDisplayName,
                                    caller,
                                    calleeDisplayName,
                                    callee,
                                    audioCodecNames,
                                    videoCodecNames,
                                    existsAudio,
                                    existsVideo,
                                    sipMessage)

        If Not _Channel(callbackIndex)._DND Then
            DND = True
        End If

        Return 0

    End Function

#End Region
    Protected Overrides Sub Finalize()
        Try
            For index = 0 To _Channel.Count - 1
                Try

                    If Not IsNothing(_Channel) Then
                        If Not IsNothing(_Channel(index)._sdkLib) Then
                            _Channel(index)._sdkLib.unRegisterServer()
                        End If
                    End If
                Catch ex As Exception
                End Try
                Try
                    If Not IsNothing(_Channel) Then
                        If Not IsNothing(_Channel(index)._sdkLib) Then
                            _Channel(index)._sdkLib.unInitialize()
                        End If
                    End If
                Catch ex As Exception
                End Try
                Try
                    If Not IsNothing(_Channel) Then
                        If Not IsNothing(_Channel(index)._sdkLib) Then
                            _Channel(index)._sdkLib.releaseCallbackHandlers()
                        End If
                    End If
                Catch ex As Exception
                End Try
                Try
                    _Channel(index)._sdkLib = Nothing
                Catch ex As Exception
                End Try
            Next
        Catch ex As Exception
        End Try
        MyBase.Finalize()
    End Sub

#Region "Call related Functions"
    Public Function Call_Make(ChannelNo As Integer, NumberToCall As String, Optional videoCall As Boolean = False) As Boolean
        Call_Make = False
        If Trim(NumberToCall) = "" Then
            RaiseError(ChannelNo, -1, -1, New Exception("Call_Make(" & ChannelNo & ") Error:" & " failed with error (" & "Number is required" & ") sessionID:" & -1))
        End If

        ' Ensure the we have been added one audio codec at least
        If _Channel(0)._sdkLib.isAudioCodecEmpty() = True Then
            'InitDefaultAudioCodecs()
        End If

        Dim sessionId As Integer = _Channel(ChannelNo)._sdkLib.call(NumberToCall, _Channel(ChannelNo)._SDP, videoCall)
        If sessionId <= 0 Then
            RaiseError(ChannelNo, -1, sessionId, New Exception("Call_Make(" & ChannelNo & ") Error:" & " failed with error (" & sessionId & ") sessionID:" & sessionId))
            Return False
        End If
        Try
            _Channel(ChannelNo)._sdkLib.setRemoteVideoWindow(sessionId, _VideoRemoteHandle)
        Catch ex As Exception
        End Try
        Return True

    End Function

    Public Function Call_StartRecord(ChannelNo As Integer, sessionID As Long, sfileName As String, sfilePath As String, Optional recordingMode As PortSIP.AUDIOSTREAM_CALLBACK_MODE = AUDIOSTREAM_CALLBACK_MODE.AUDIOSTREAM_NONE) As Boolean
        Call_StartRecord = False
        Try
            If Not Directory.Exists(sfilePath) Then
                RaiseError(ChannelNo, -1, sessionID, New Exception("Call_StartRecord(" & ChannelNo & ") Error:" & " invalid path (" & sfilePath & ") sessionID:" & sessionID))
                Return False
            End If

            If File.Exists(sfilePath & sfileName) Then
                RaiseError(ChannelNo, -1, sessionID, New Exception("Call_StartRecord(" & ChannelNo & ") Error:" & " file already exits (" & sfilePath & sfileName & ") sessionID:" & sessionID))
                Return False
            End If

            Dim audioRecordFileFormat As AUDIO_RECORDING_FILEFORMAT = AUDIO_RECORDING_FILEFORMAT.FILEFORMAT_WAVE
            Dim videoRecordFileFormat As VIDEOCODEC_TYPE = VIDEOCODEC_TYPE.VIDEO_CODEC_H264

            _Channel(ChannelNo)._recFileName = sfilePath & sfileName & ".wav"

            If recordingMode = AUDIOSTREAM_CALLBACK_MODE.AUDIOSTREAM_NONE Then
                Dim rt As Integer = 0
                rt = _Channel(ChannelNo)._sdkLib.startRecord(sessionID,
                                sfilePath,
                                sfileName,
                                False,
                                audioRecordFileFormat,
                                RECORD_MODE.RECORD_BOTH,
                                videoRecordFileFormat,
                                RECORD_MODE.RECORD_BOTH)
                If rt <> 0 Then
                    RaiseError(ChannelNo, -1, sessionID, New Exception("Call_StartRecord(" & ChannelNo & ") Error:" & " failed with error (" & rt & ") sessionID:" & sessionID))
                    Exit Function
                End If
                sendGenericEvent(ChannelNo, -1, "onInviteRecord", sessionID, "Started-file", sfilePath & sfileName)
            Else
                _Channel(ChannelNo)._recDoCerateFile = True
                enableAudioStreamCallback(ChannelNo, sessionID, True, AUDIOSTREAM_CALLBACK_MODE.AUDIOSTREAM_LOCAL_MIX)
                sendGenericEvent(ChannelNo, -1, "onInviteRecord", sessionID, "Started-Buffer", sfilePath & sfileName)
            End If
            Return True
        Catch ex As Exception
            RaiseError(ChannelNo, -1, sessionID, New Exception("Call_StartRecord(" & ChannelNo & ") " & ex.Message))
        End Try
    End Function
    Public Function Call_StopRecord(ChannelNo As Integer, sessionID As Long) As Boolean
        Call_StopRecord = False
        Try
            Try
                Try
                    _Channel(ChannelNo)._sdkLib.stopRecord(sessionID)
                Catch ex As Exception
                End Try
                enableAudioStreamCallback(ChannelNo, sessionID, False, AUDIOSTREAM_CALLBACK_MODE.AUDIOSTREAM_REMOTE_MIX)
                '_Channel(ChannelNo)._recfileDataWritenAt = DateTime.Now
                writeRecordingData(ChannelNo)
                sendGenericEvent(ChannelNo, -1, "onInviteRecord", sessionID, "Stop", _Channel(ChannelNo)._recFileName)
                Return True
            Catch ex As Exception
            End Try
        Catch ex As Exception
            RaiseError(ChannelNo, -1, sessionID, New Exception("Call_StopRecord(" & ChannelNo & ") " & ex.Message))
        End Try
    End Function

    Public Function Call_sendVideo(ChannelNo As Integer, sessionID As Long, Optional Enabled As Boolean = False) As Boolean
        Call_sendVideo = False
        Try
            Dim rt As Integer = _Channel(ChannelNo)._sdkLib.sendVideo(sessionID, Enabled)
        Catch ex As Exception
            RaiseError(ChannelNo, -1, sessionID, New Exception("Call_sendVideo(" & ChannelNo & ") " & ex.Message))
        End Try
    End Function
    Public Function Call_Answer(ChannelNo As Integer, sessionID As Long, Optional localVideoWindow As IntPtr = Nothing, Optional remoteVideoWindow As IntPtr = Nothing) As Boolean
        If remoteVideoWindow <> Nothing Then
            Try
                setRemoteVideoWindow(ChannelNo, sessionID, remoteVideoWindow)
            Catch ex As Exception
                RaiseError(ChannelNo, -1, sessionID, New Exception("setRemoteVideoWindow(" & ChannelNo & ") " & ex.Message))
                Return False
            End Try
        End If

        If localVideoWindow <> Nothing Then
            Try
                setLocalVideoWindow(ChannelNo, True, localVideoWindow)
            Catch ex As Exception
                RaiseError(ChannelNo, -1, sessionID, New Exception("setRemoteVideoWindow() " & ex.Message))
                Return False
            End Try
        Else
            setLocalVideoWindow(ChannelNo, False, localVideoWindow)
        End If
        Call_Answer = False
        Dim rt As Integer = 0
        Try
            _Channel(ChannelNo)._sdkLib.setRemoteVideoWindow(sessionID, _VideoRemoteHandle)
            rt = _Channel(ChannelNo)._sdkLib.answerCall(sessionID, True)
            If rt <> 0 Then
                RaiseError(ChannelNo, -1, sessionID, New Exception("Call_Answer(" & ChannelNo & ") Error:" & " failed with error (" & rt & ") sessionID:" & sessionID))
                Exit Function
            End If
            Call_Answer = True
        Catch ex As Exception
            RaiseError(ChannelNo, -1, sessionID, New Exception("Call_Answer(" & ChannelNo & ") " & ex.Message))
        End Try
    End Function
    Public Function Call_Hangup(ChannelNo As Integer, sessionID As Long) As Boolean
        Call_Hangup = False
        Dim rt As Integer = 0
        Try
            Call_StopRecord(ChannelNo, sessionID)
            rt = _Channel(ChannelNo)._sdkLib.hangUp(sessionID)
            If rt <> 0 Then
                RaiseError(ChannelNo, -1, sessionID, New Exception("Call_Hangup(" & ChannelNo & ") Error:" & " failed with error (" & rt & ") sessionID:" & sessionID))
                Exit Function
            End If
            sendGenericEvent(ChannelNo, -1, "onInviteClosed", sessionID, "Byuser", "")
            If Not _Channel(ChannelNo)._DND Then
                DND = True
            End If
            Call_Hangup = True
        Catch ex As Exception
            RaiseError(ChannelNo, -1, sessionID, New Exception("Call_Hangup(" & ChannelNo & ") " & ex.Message))
        End Try
    End Function
    Public Function Call_Reject(ChannelNo As Integer, sessionID As Long, Optional code As Integer = 607) As Boolean
        Call_Reject = False
        Dim rt As Integer = 0

        Try
            Call_StopRecord(ChannelNo, sessionID)
            rt = _Channel(ChannelNo)._sdkLib.rejectCall(sessionID, code)
            If rt <> 0 Then
                RaiseError(ChannelNo, -1, sessionID, New Exception("Call_Reject(" & ChannelNo & ") Error:" & " failed with error (" & rt & ") sessionID:" & sessionID))
                Exit Function
            End If
            sendGenericEvent(ChannelNo, -1, "onInviteClosed", sessionID, "Byuser=Rejected", "")
            If Not _Channel(ChannelNo)._DND Then
                DND = True
            End If
            Call_Reject = True
        Catch ex As Exception
            RaiseError(ChannelNo, -1, sessionID, New Exception("Call_Reject(" & ChannelNo & ") " & ex.Message))
        End Try
    End Function

    Public Function Call_Hold(ChannelNo As Integer, sessionID As Long) As Boolean
        Call_Hold = False
        Dim rt As Integer = 0

        Try
            rt = _Channel(ChannelNo)._sdkLib.hold(sessionID)
            If rt <> 0 Then
                RaiseError(ChannelNo, -1, sessionID, New Exception("Call_Hold(" & ChannelNo & ") Error:" & " failed with error (" & rt & ") sessionID:" & sessionID))
                Exit Function
            End If
            Call_Hold = True
        Catch ex As Exception
            RaiseError(ChannelNo, -1, sessionID, New Exception("Call_Hold(" & ChannelNo & ") " & ex.Message))
        End Try
    End Function
    Public Function Call_Unhold(ChannelNo As Integer, sessionID As Long) As Boolean
        Call_Unhold = False
        Dim rt As Integer = 0
        Try
            rt = _Channel(ChannelNo)._sdkLib.unHold(sessionID)
            If rt <> 0 Then
                RaiseError(ChannelNo, -1, sessionID, New Exception("Call_Unhold(" & ChannelNo & ") Error:" & " failed with error (" & rt & ") sessionID:" & sessionID))
                Exit Function
            End If
            Call_Unhold = True
        Catch ex As Exception
            RaiseError(ChannelNo, -1, sessionID, New Exception("Call_Unhold(" & ChannelNo & ") " & ex.Message))
        End Try
    End Function


#End Region

End Class
