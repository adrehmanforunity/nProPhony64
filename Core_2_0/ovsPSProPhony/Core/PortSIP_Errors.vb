'!
'     * @author Copyright (c) 2008-2017 PortSIP Solutions,Inc. All rights reserved.
'     * @version 15
'     * @see https://www.portsip.com/
'     * @class PortSIPLib
'     * @brief The PortSIP VoIP SDK class.
' 
'     PortSIP VoIP SDK functions class description.
'     

''///////////////////////////////////////////////////////////////////////
'
'  !!!IMPORTANT!!! DON'T EDIT BELOW SOURCE CODE  
'
''///////////////////////////////////////////////////////////////////////
'' 

Imports System.Collections.Generic
Imports System.Text

Namespace PortSIP
    Public Class PortSIP_Errors
        Public Shared ReadOnly INVALID_SESSION_ID As Integer = -1
        Public Shared ReadOnly CONFERENCE_SESSION_ID As Integer = &H7FFF


        Public Shared ReadOnly ECoreAlreadyInitialized As Integer = -60000
        Public Shared ReadOnly ECoreNotInitialized As Integer = -60001
        Public Shared ReadOnly ECoreSDKObjectNull As Integer = -60002
        Public Shared ReadOnly ECoreArgumentNull As Integer = -60003
        Public Shared ReadOnly ECoreInitializeWinsockFailure As Integer = -60004
        Public Shared ReadOnly ECoreUserNameAuthNameEmpty As Integer = -60005
        Public Shared ReadOnly ECoreInitializeStackFailure As Integer = -60006
        Public Shared ReadOnly ECorePortOutOfRange As Integer = -60007
        Public Shared ReadOnly ECoreAddTcpTransportFailure As Integer = -60008
        Public Shared ReadOnly ECoreAddTlsTransportFailure As Integer = -60009
        Public Shared ReadOnly ECoreAddUdpTransportFailure As Integer = -60010
        Public Shared ReadOnly ECoreMiniAudioPortOutOfRange As Integer = -60011
        Public Shared ReadOnly ECoreMaxAudioPortOutOfRange As Integer = -60012
        Public Shared ReadOnly ECoreMiniVideoPortOutOfRange As Integer = -60013
        Public Shared ReadOnly ECoreMaxVideoPortOutOfRange As Integer = -60014
        Public Shared ReadOnly ECoreMiniAudioPortNotEvenNumber As Integer = -60015
        Public Shared ReadOnly ECoreMaxAudioPortNotEvenNumber As Integer = -60016
        Public Shared ReadOnly ECoreMiniVideoPortNotEvenNumber As Integer = -60017
        Public Shared ReadOnly ECoreMaxVideoPortNotEvenNumber As Integer = -60018
        Public Shared ReadOnly ECoreAudioVideoPortOverlapped As Integer = -60019
        Public Shared ReadOnly ECoreAudioVideoPortRangeTooSmall As Integer = -60020
        Public Shared ReadOnly ECoreAlreadyRegistered As Integer = -60021
        Public Shared ReadOnly ECoreSIPServerEmpty As Integer = -60022
        Public Shared ReadOnly ECoreExpiresValueTooSmall As Integer = -60023
        Public Shared ReadOnly ECoreCallIdNotFound As Integer = -60024
        Public Shared ReadOnly ECoreNotRegistered As Integer = -60025
        Public Shared ReadOnly ECoreCalleeEmpty As Integer = -60026
        Public Shared ReadOnly ECoreInvalidUri As Integer = -60027
        Public Shared ReadOnly ECoreAudioVideoCodecEmpty As Integer = -60028
        Public Shared ReadOnly ECoreNoFreeDialogSession As Integer = -60029
        Public Shared ReadOnly ECoreCreateAudioChannelFailed As Integer = -60030
        Public Shared ReadOnly ECoreSessionTimerValueTooSmall As Integer = -60040
        Public Shared ReadOnly ECoreAudioHandleNull As Integer = -60041
        Public Shared ReadOnly ECoreVideoHandleNull As Integer = -60042
        Public Shared ReadOnly ECoreCallIsClosed As Integer = -60043
        Public Shared ReadOnly ECoreCallAlreadyHold As Integer = -60044
        Public Shared ReadOnly ECoreCallNotEstablished As Integer = -60045
        Public Shared ReadOnly ECoreCallNotHold As Integer = -60050
        Public Shared ReadOnly ECoreSipMessaegEmpty As Integer = -60051
        Public Shared ReadOnly ECoreSipHeaderNotExist As Integer = -60052
        Public Shared ReadOnly ECoreSipHeaderValueEmpty As Integer = -60053
        Public Shared ReadOnly ECoreSipHeaderBadFormed As Integer = -60054
        Public Shared ReadOnly ECoreBufferTooSmall As Integer = -60055
        Public Shared ReadOnly ECoreSipHeaderValueListEmpty As Integer = -60056
        Public Shared ReadOnly ECoreSipHeaderParserEmpty As Integer = -60057
        Public Shared ReadOnly ECoreSipHeaderValueListNull As Integer = -60058
        Public Shared ReadOnly ECoreSipHeaderNameEmpty As Integer = -60059
        Public Shared ReadOnly ECoreAudioSampleNotmultiple As Integer = -60060
        '	The audio sample should be a multiple of 10
        Public Shared ReadOnly ECoreAudioSampleOutOfRange As Integer = -60061
        '	The audio sample ranges 10 - 60
        Public Shared ReadOnly ECoreInviteSessionNotFound As Integer = -60062
        Public Shared ReadOnly ECoreStackException As Integer = -60063
        Public Shared ReadOnly ECoreMimeTypeUnknown As Integer = -60064
        Public Shared ReadOnly ECoreDataSizeTooLarge As Integer = -60065
        Public Shared ReadOnly ECoreSessionNumsOutOfRange As Integer = -60066
        Public Shared ReadOnly ECoreNotSupportCallbackMode As Integer = -60067
        Public Shared ReadOnly ECoreNotFoundSubscribeId As Integer = -60068
        Public Shared ReadOnly ECoreCodecNotSupport As Integer = -60069
        Public Shared ReadOnly ECoreCodecParameterNotSupport As Integer = -60070
        Public Shared ReadOnly ECorePayloadOutofRange As Integer = -60071
        '  Dynamic Payload ranges 96 - 127
        Public Shared ReadOnly ECorePayloadHasExist As Integer = -60072
        '  Duplicate Payload values are not allowed.
        Public Shared ReadOnly ECoreFixPayloadCantChange As Integer = -60073
        '  It's fixed payload type that cannot be modified.
        Public Shared ReadOnly ECoreCodecTypeInvalid As Integer = -60074
        Public Shared ReadOnly ECoreCodecWasExist As Integer = -60075
        Public Shared ReadOnly ECorePayloadTypeInvalid As Integer = -60076
        Public Shared ReadOnly ECoreArgumentTooLong As Integer = -60077
        Public Shared ReadOnly ECoreMiniRtpPortMustIsEvenNum As Integer = -60078
        Public Shared ReadOnly ECoreCallInHold As Integer = -60079
        Public Shared ReadOnly ECoreNotIncomingCall As Integer = -60080
        Public Shared ReadOnly ECoreCreateMediaEngineFailure As Integer = -60081
        Public Shared ReadOnly ECoreAudioCodecEmptyButAudioEnabled As Integer = -60082
        Public Shared ReadOnly ECoreVideoCodecEmptyButVideoEnabled As Integer = -60083
        Public Shared ReadOnly ECoreNetworkInterfaceUnavailable As Integer = -60084
        Public Shared ReadOnly ECoreWrongDTMFTone As Integer = -60085
        Public Shared ReadOnly ECoreWrongLicenseKey As Integer = -60086
        Public Shared ReadOnly ECoreTrialVersionLicenseKey As Integer = -60087
        Public Shared ReadOnly ECoreOutgoingAudioMuted As Integer = -60088
        Public Shared ReadOnly ECoreOutgoingVideoMuted As Integer = -60089
        Public Shared ReadOnly ECoreFailedCreateSdp As Integer = -60090
        Public Shared ReadOnly ECoreTrialVersionExpired As Integer = -60091
        Public Shared ReadOnly ECoreStackFailure As Integer = -60092
        Public Shared ReadOnly ECoreTransportExists As Integer = -60093
        Public Shared ReadOnly ECoreUnsupportTransport As Integer = -60094
        Public Shared ReadOnly ECoreAllowOnlyOneUser As Integer = -60095
        Public Shared ReadOnly ECoreUserNotFound As Integer = -60096
        Public Shared ReadOnly ECoreTransportsIncorrect As Integer = -60097
        Public Shared ReadOnly ECoreCreateTransportFailure As Integer = -60098
        Public Shared ReadOnly ECoreTransportNotSet As Integer = -60099
        Public Shared ReadOnly ECoreECreateSignalingFailure As Integer = -60100
        Public Shared ReadOnly ECoreArgumentIncorrect As Integer = -60101

        ' audio
        Public Shared ReadOnly EAudioFileNameEmpty As Integer = -70000
        Public Shared ReadOnly EAudioChannelNotFound As Integer = -70001
        Public Shared ReadOnly EAudioStartRecordFailure As Integer = -70002
        Public Shared ReadOnly EAudioRegisterRecodingFailure As Integer = -70003
        Public Shared ReadOnly EAudioRegisterPlaybackFailure As Integer = -70004
        Public Shared ReadOnly EAudioGetStatisticsFailure As Integer = -70005
        Public Shared ReadOnly EAudioIsPlaying As Integer = -70006
        Public Shared ReadOnly EAudioPlayObjectNotExist As Integer = -70007
        Public Shared ReadOnly EAudioPlaySteamNotEnabled As Integer = -70008
        Public Shared ReadOnly EAudioRegisterCallbackFailure As Integer = -70009
        Public Shared ReadOnly EAudioCreateAudioConferenceFailure As Integer = -70010
        Public Shared ReadOnly EAudioOpenPlayFileFailure As Integer = -70011
        Public Shared ReadOnly EAudioPlayFileModeNotSupport As Integer = -70012
        Public Shared ReadOnly EAudioPlayFileFormatNotSupport As Integer = -70013
        Public Shared ReadOnly EAudioPlaySteamAlreadyEnabled As Integer = -70014
        Public Shared ReadOnly EAudioCreateRecordFileFailure As Integer = -70015
        Public Shared ReadOnly EAudioCodecNotSupport As Integer = -70016
        Public Shared ReadOnly EAudioPlayFileNotEnabled As Integer = -70017
        Public Shared ReadOnly EAudioPlayFileUnknowSeekOrigin As Integer = -70018
        Public Shared ReadOnly EAudioCantSetDeviceIdDuringCall As Integer = -70019
        Public Shared ReadOnly EAudioVolumeOutOfRange As Integer = -70020

        ' video
        Public Shared ReadOnly EVideoFileNameEmpty As Integer = -80000
        Public Shared ReadOnly EVideoGetDeviceNameFailure As Integer = -80001
        Public Shared ReadOnly EVideoGetDeviceIdFailure As Integer = -80002
        Public Shared ReadOnly EVideoStartCaptureFailure As Integer = -80003
        Public Shared ReadOnly EVideoChannelNotFound As Integer = -80004
        Public Shared ReadOnly EVideoStartSendFailure As Integer = -80005
        Public Shared ReadOnly EVideoGetStatisticsFailure As Integer = -80006
        Public Shared ReadOnly EVideoStartPlayAviFailure As Integer = -80007
        Public Shared ReadOnly EVideoSendAviFileFailure As Integer = -80008
        Public Shared ReadOnly EVideoRecordUnknowCodec As Integer = -80009
        Public Shared ReadOnly EVideoCantSetDeviceIdDuringCall As Integer = -80010
        Public Shared ReadOnly EVideoUnsupportCaptureRotate As Integer = -80011
        Public Shared ReadOnly EVideoUnsupportCaptureResolution As Integer = -80012
        Public Shared ReadOnly ECameraSwitchTooOften As Integer = -80013
        Public Shared ReadOnly EMTUOutOfRange As Integer = -80014

        ' Device
        Public Shared ReadOnly EDeviceGetDeviceNameFailure As Integer = -90001
    End Class
End Namespace

