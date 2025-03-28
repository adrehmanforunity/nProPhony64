'!
' * @author Copyright (c) 2008-2017 PortSIP Solutions,Inc. All rights reserved.
' * @version 15
' * @see https://www.portsip.com
' * @brief PortSIP SDK Callback events.
' 
' PortSIP VoIP SDK Callback events description.
' 



'''///////////////////////////////////////////////////////////////////////
'
' IMPORTANT: DON'T EDIT THIS FILE
'
'''///////////////////////////////////////////////////////////////////////



Imports System.Collections.Generic
Imports System.Text

Namespace PortSIP
    ''' Audio codec type
    Public Enum AUDIOCODEC_TYPE As Integer
        AUDIOCODEC_NONE = -1
        AUDIOCODEC_G729 = 18
        '''< G729 8KHZ 8kbit/s
        AUDIOCODEC_PCMA = 8
        '''< PCMA/G711 A-law 8KHZ 64kbit/s
        AUDIOCODEC_PCMU = 0
        '''< PCMU/G711 μ-law 8KHZ 64kbit/s
        AUDIOCODEC_GSM = 3
        '''< GSM 8KHZ 13kbit/s
        AUDIOCODEC_G722 = 9
        '''< G722 16KHZ 64kbit/s
        AUDIOCODEC_ILBC = 97
        '''< iLBC 8KHZ 30ms-13kbit/s 20 ms-15kbit/s
        AUDIOCODEC_AMR = 98
        '''< Adaptive Multi-Rate (AMR) 8KHZ (4.75,5.15,5.90,6.70,7.40,7.95,10.20,12.20)kbit/s
        AUDIOCODEC_AMRWB = 99
        '''< Adaptive Multi-Rate Wideband (AMR-WB)16KHZ (6.60,8.85,12.65,14.25,15.85,18.25,19.85,23.05,23.85)kbit/s
        AUDIOCODEC_SPEEX = 100
        '''< SPEEX 8KHZ (2-24)kbit/s
        AUDIOCODEC_SPEEXWB = 102
        '''< SPEEX 16KHZ (4-42)kbit/s
        AUDIOCODEC_ISACWB = 103
        '''< internet Speech Audio Codec(iSAC) 16KHZ (32-54)kbit/s
        AUDIOCODEC_ISACSWB = 104
        '''< internet Speech Audio Codec(iSAC) 16KHZ (32-160)kbit/s
        AUDIOCODEC_G7221 = 121
        '''< G722.1 16KHZ (16,24,32)kbit/s
        AUDIOCODEC_OPUS = 105
        '''< OPUS 48KHZ 32kbit/s
        AUDIOCODEC_DTMF = 101
        '''< DTMF RFC 2833
    End Enum

    ''' Video codec type
    Public Enum VIDEOCODEC_TYPE As Integer
        VIDEO_CODE_NONE = -1
        '''< Do not use Video codec
        VIDEO_CODEC_I420 = 113
        '''< I420/YUV420 Raw Video format. Used with startRecord only 
        VIDEO_CODEC_H263 = 34
        '''< H263 video codec
        VIDEO_CODEC_H263_1998 = 115
        '''< H263+/H263 1998 video codec
        VIDEO_CODEC_H264 = 125
        '''< H264 video codec
        VIDEO_CODEC_VP8 = 120
        '''< VP8 video code
        VIDEO_CODEC_VP9 = 122
        '''< VP9 video code
    End Enum

    ''' The audio record file format
    Public Enum AUDIO_RECORDING_FILEFORMAT As Integer
        FILEFORMAT_WAVE = 1
        '''<	The record audio file is in WAVE format. 
        FILEFORMAT_AMR
        '''<	The record audio file is in AMR format - all voice data are compressed by AMR codec. 
    End Enum

    '''The audio/Video record mode
    Public Enum RECORD_MODE As Integer
        RECORD_NONE = 0
        '''<	Not Record. 
        RECORD_RECV = 1
        '''<	Only record the received data. 
        RECORD_SEND
        '''<	Only record the sent data. 
        RECORD_BOTH
        '''<	The record audio file is in WAVE format. 
    End Enum


    Public Enum CALLBACK_SESSION_ID As Integer
        PORTSIP_LOCAL_MIX_ID = -1
        PORTSIP_REMOTE_MIX_ID = -2
    End Enum

    '''The audio stream callback mode
    Public Enum AUDIOSTREAM_CALLBACK_MODE As Integer
        AUDIOSTREAM_NONE = 0
        AUDIOSTREAM_LOCAL_MIX
        '''<	Callback the audio stream from microphone for all channels.
        AUDIOSTREAM_LOCAL_PER_CHANNEL
        '''<  Callback the audio stream from microphone for one channel based on the session ID
        AUDIOSTREAM_REMOTE_MIX
        '''<	Callback the received audio stream that mixed all included channels.
        AUDIOSTREAM_REMOTE_PER_CHANNEL
        '''<  Callback the received audio stream for one channel based on the session ID.
    End Enum

    '''The video stream callback mode
    Public Enum VIDEOSTREAM_CALLBACK_MODE As Integer
        VIDEOSTREAM_NONE = 0
        '''< Disable video stream callback
        VIDEOSTREAM_LOCAL
        '''< Local video stream callback
        VIDEOSTREAM_REMOTE
        '''< Remote video stream callback
        VIDEOSTREAM_BOTH
        '''< Both of local and remote video stream callback
    End Enum

    ''' Log level
    Public Enum PORTSIP_LOG_LEVEL As Integer
        PORTSIP_LOG_NONE = -1
        PORTSIP_LOG_ERROR = 1
        PORTSIP_LOG_WARNING = 2
        PORTSIP_LOG_INFO = 3
        PORTSIP_LOG_DEBUG = 4
    End Enum

    ''' SRTP Policy
    Public Enum SRTP_POLICY As Integer
        SRTP_POLICY_NONE = 0
        '''< Do not use SRTP. The SDK can receive the encrypted call(SRTP) and unencrypted call both, but can't place outgoing encrypted call. 
        SRTP_POLICY_FORCE
        '''< All calls must use SRTP. The SDK allows to receive encrypted call and place outgoing encrypted call only.
        SRTP_POLICY_PREFER
        '''< Top priority for using SRTP. The SDK allows to receive encrypted and decrypted call, and to place outgoing encrypted call and unencrypted call.
    End Enum

    ''' Transport for SIP signaling.
    Public Enum TRANSPORT_TYPE As Integer
        TRANSPORT_UDP = 0
        '''< UDP Transport
        TRANSPORT_TLS
        '''< Tls Transport
        TRANSPORT_TCP
        '''< TCP Transport
        TRANSPORT_PERS
        '''< PERS is the PortSIP private transport for anti SIP blocking. It must be used with the PERS Server http://www.portsip.com/pers.html.
    End Enum

    '''The session refreshment by UAC or UAS
    Public Enum SESSION_REFRESH_MODE As Integer
        SESSION_REFERESH_UAC = 0
        '''< The session refreshment by UAC
        SESSION_REFERESH_UAS
        '''< The session refreshment by UAS
    End Enum

    '''send DTMF tone with two methods
    Public Enum DTMF_METHOD
        DTMF_RFC2833 = 0
        '''<	Send DTMF tone with RFC 2833. Recommended.
        DTMF_INFO = 1
        '''<	Send DTMF tone with SIP INFO.
    End Enum


    ''' type of Echo Control
    Public Enum EC_MODES
        EC_NONE = 0
        ' Disable AEC
        EC_DEFAULT = 1
        ' Platform default AEC
        EC_CONFERENCE = 2
        ' Desktop platform(windows,MAC) Conferencing default (aggressive AEC)
        EC_AEC = 3
        ' Desktop platform(windows,MAC) Acoustic Echo Cancellation(desktop Platform default)
        EC_AECM_1 = 4
        ' Mobile platform(iOS,Android) most earpiece use
        EC_AECM_2 = 5
        ' Mobile platform(iOS,Android) Loud earpiece or quiet speakerphone use
        EC_AECM_3 = 6
        ' Mobile platform(iOS,Android) most speakerphone use (Mobile Platform default)
        EC_AECM_4 = 7
        ' Mobile platform(iOS,Android) Loud speakerphone
    End Enum

    ''' type of Automatic Gain Control
    Public Enum AGC_MODES
        AGC_NONE = 0
        ' Disable AGC
        AGC_DEFAULT
        ' Platform default
        AGC_ADAPTIVE_ANALOG
        ' Desktop platform (windows,MAC) adaptive mode for use when analog volume control exists
        AGC_ADAPTIVE_DIGITAL
        ' Scaling takes place in the digital domain (e.g. for conference servers and embedded devices)
        AGC_FIXED_DIGITAL
        ' Can be used on embedded devices where the capture signal level is predictable
    End Enum

    ''' type of Noise Suppression
    Public Enum NS_MODES
        NS_NONE = 0
        ' Disable NS
        NS_DEFAULT
        ' platform default
        NS_Conference
        ' conferencing default
        NS_LOW_SUPPRESSION
        ' lowest suppression
        NS_MODERATE_SUPPRESSION
        NS_HIGH_SUPPRESSION
        NS_VERY_HIGH_SUPPRESSION
        ' highest suppression
    End Enum

End Namespace

