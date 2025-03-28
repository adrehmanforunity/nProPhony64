
'!
' * @author Copyright (c) 2008-2017 PortSIP Solutions,Inc. All rights reserved.
' * @version 15
' * @see https://www.portsip.com
' * @brief The PortSIP VoIP SDK class.
' 
' PortSIP VoIP SDK functions class description.
' 

Imports System.Collections.Generic
Imports System.Text
Imports System.Runtime.InteropServices


'''///////////////////////////////////////////////////////////////////////
'
'  !!!IMPORTANT!!! DON'T EDIT BELOW SOURCE CODE  
'
'''///////////////////////////////////////////////////////////////////////

'!
'*  PortSIP The PortSIP VoIP SDK namespace
'

Namespace PortSIP
    Class PortSIPLib

        Private _SIPCallbackEvents As SIPCallbackEvents


        Public Sub New(callbackIndex As Int32, callbackObject As Int32, calbackevents As SIPCallbackEvents)
            initializeCallbackFunctions()

            _callbackDispatcher = IntPtr.Zero
            _LibSDK = IntPtr.Zero
            _callbackObject = callbackObject
            _callbackIndex = callbackIndex

            _SIPCallbackEvents = calbackevents
        End Sub


        Public Function createCallbackHandlers() As [Boolean]
            ' This must be called before initialization
            If _callbackDispatcher <> IntPtr.Zero Then
                Return False
            End If

            If createSIPCallbackHandle() = False Then
                Return False
            End If

            setCallbackHandlers()

            Return True

        End Function


        Private Function createSIPCallbackHandle() As [Boolean]
            _callbackDispatcher = PortSIP_NativeMethods.PSCallback_createCallbackDispatcher()
            If _callbackDispatcher = IntPtr.Zero Then
                Return False
            End If

            Return True
        End Function


        Public Sub releaseCallbackHandlers()
            ' This must called after unInitialization
            If _callbackDispatcher = IntPtr.Zero Then
                Return
            End If

            PortSIP_NativeMethods.PSCallback_releaseCallbackDispatcher(_callbackDispatcher)
            _callbackDispatcher = IntPtr.Zero
        End Sub



        Private Sub setCallbackHandlers()
            PortSIP_NativeMethods.PSCallback_setRegisterSuccessHandler(_callbackDispatcher, _rgs, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setRegisterFailureHandler(_callbackDispatcher, _rgf, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setInviteIncomingHandler(_callbackDispatcher, _ivi, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setInviteTryingHandler(_callbackDispatcher, _ivt, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setInviteSessionProgressHandler(_callbackDispatcher, _ivsp, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setInviteRingingHandler(_callbackDispatcher, _ivr, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setInviteAnsweredHandler(_callbackDispatcher, _iva, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setInviteFailureHandler(_callbackDispatcher, _ivf, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setInviteUpdatedHandler(_callbackDispatcher, _ivu, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setInviteConnectedHandler(_callbackDispatcher, _ivc, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setInviteBeginingForwardHandler(_callbackDispatcher, _ivbf, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setInviteClosedHandler(_callbackDispatcher, _ivcl, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setDialogStateUpdatedHandler(_callbackDispatcher, _dsu, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setRemoteHoldHandler(_callbackDispatcher, _rmh, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setRemoteUnHoldHandler(_callbackDispatcher, _rmuh, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setReceivedReferHandler(_callbackDispatcher, _rvr, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setReferAcceptedHandler(_callbackDispatcher, _rfa, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setReferRejectedHandler(_callbackDispatcher, _rfr, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setTransferTryingHandler(_callbackDispatcher, _tft, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setTransferRingingHandler(_callbackDispatcher, _tfr, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setACTVTransferSuccessHandler(_callbackDispatcher, _ats, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setACTVTransferFailureHandler(_callbackDispatcher, _atf, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setReceivedSignalingHandler(_callbackDispatcher, _rvs, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setSendingSignalingHandler(_callbackDispatcher, _sds, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setWaitingVoiceMessageHandler(_callbackDispatcher, _wvm, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setWaitingFaxMessageHandler(_callbackDispatcher, _wfm, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setRecvDtmfToneHandler(_callbackDispatcher, _rdt, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setPresenceRecvSubscribeHandler(_callbackDispatcher, _prs, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setPresenceOnlineHandler(_callbackDispatcher, _pon, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setPresenceOfflineHandler(_callbackDispatcher, _pof, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setRecvOptionsHandler(_callbackDispatcher, _rvo, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setRecvInfoHandler(_callbackDispatcher, _rvi, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setPlayAudioFileFinishedHandler(_callbackDispatcher, _paf, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setPlayVideoFileFinishedHandler(_callbackDispatcher, _pvf, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setRecvMessageHandler(_callbackDispatcher, _rvm, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setRecvOutOfDialogMessageHandler(_callbackDispatcher, _rodm, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setSendMessageSuccessHandler(_callbackDispatcher, _sms, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setSendMessageFailureHandler(_callbackDispatcher, _smf, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setSendOutOfDialogMessageSuccessHandler(_callbackDispatcher, _sdms, _callbackIndex, _callbackObject)
            PortSIP_NativeMethods.PSCallback_setSendOutOfDialogMessageFailureHandler(_callbackDispatcher, _sdmf, _callbackIndex, _callbackObject)
        End Sub

        '* @defgroup groupSDK SDK functions
        '* SDK functions
        '* @{
        '

        '* @defgroup group1 Initialize and register functions
        '    * Initialize and register functions
        '    * @{
        '    


        '!
        ' @brief Initialize the SDK.
        '
        '@param transport      Transport for SIP signaling. TRANSPORT_PERS is the PortSIP private transport for anti SIP blocking. It must be used with PERS.
        '@param localIP        The local computer IP address to be bound (for example: 192.168.1.108). It will be used for sending and receiving SIP messages and RTP packets. If this IP is passed in IPv6 format, the SDK will be using IPv6.<br>
        '                      If you want the SDK to choose correct network interface (IP) automatically, please pass the "0.0.0.0"(for IPv4) or "::" (for IPv6).
        ' @param localSIPPort  The SIP message transport listener port, for example: 5060.
        ' @param logLevel      Set the application log level. The SDK will generate "PortSIP_Log_datatime.log" file if the log enabled.
        ' @param logFilePath   The log file path. The path (folder) MUST be existent.
        ' @param maxCallLines  Theoretically unlimited lines could be supported depending on the device capability. For SIP client recommended value ranges 1 - 100;
        ' @param sipAgent      The User-Agent header to be inserted into SIP messages.
        ' @param audioDeviceLayer Specify the audio device layer to be used: <br>
        '            0 = Use the OS default device.<br>
        '            1 = Virtual device, usually use this for the device which has no sound device installed.<br>
        ' @param videoDeviceLayer Specifies the video device layer that should be used: <br>
        '            0 = Use the OS default device.<br>
        '            1 = Use Virtual device. Usually use this for the device which has no camera installed.
        ' @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code
        '         

        Public Function initialize(transportType As TRANSPORT_TYPE, localIp As [String], localSIPPort As Int32, logLevel As PORTSIP_LOG_LEVEL, logFilePath As [String], maxCallSessions As Int32,
            sipAgentString As [String], audioDeviceLayer As Int32, videoDeviceLayer As Int32, TLSCertificatesRootPath As [String], TLSCipherList As [String], verifyTLSCertificate As [Boolean]) As Int32
            If _callbackDispatcher = IntPtr.Zero OrElse _LibSDK <> IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Dim errorCode As Int32 = 0
            _LibSDK = PortSIP_NativeMethods.PortSIP_initialize(_callbackDispatcher, transportType, localIp, localSIPPort, DirectCast(logLevel, Int32), logFilePath,
                maxCallSessions, sipAgentString, audioDeviceLayer, videoDeviceLayer, TLSCertificatesRootPath, TLSCipherList,
                verifyTLSCertificate, errorCode)

            Return errorCode
        End Function


        '!
        '  @brief Un-initialize the SDK and release resources.
        '         

        Public Sub unInitialize()
            If _LibSDK = IntPtr.Zero Then
                Return
            End If

            PortSIP_NativeMethods.PortSIP_unInitialize(_LibSDK)
            _LibSDK = IntPtr.Zero
        End Sub

        '!
        '    @brief Get the current version number of the SDK.
        '  
        '    @param majorVersion Return the major version number.
        '    @param minorVersion Return the minor version number.
        '  
        '    @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '           

        Public Function getVersion(ByRef majorVersion As Int32, ByRef minorVersion As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                majorVersion = 0
                minorVersion = 0

                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_getVersion(_LibSDK, majorVersion, minorVersion)
        End Function


        '!
        '  @brief Set the license key. It must be called before setUser function.
        '
        '  @param key The SDK license key, please purchase from PortSIP.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function setLicenseKey(key As [String]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_setLicenseKey(_LibSDK, key)
        End Function

        '* @} 

        ' end of group1

        '* @defgroup group2 NIC and local IP functions
        ' @{
        '         


        '!
        '  @brief Get the Network Interface Card numbers.
        '
        '  @return If the function succeeds, it will return the NIC numbers which is greater than or equal to 0. If the function fails, it will return a specific error code.
        '         

        Public Function getNICNums() As Int32
            Return PortSIP_NativeMethods.PortSIP_getNICNums()
        End Function

        '!
        '  @brief Get the local IP address by Network Interface Card index.
        '
        '  @param index The IP address index. For example, if the PC has two NICs, and we wish to obtain the second NIC IP. Set this parameter to 1 and the first NIC IP index is 0.
        '  @param ip The buffer that is used to receive the IP. 
        '  @param ipSize The IP buffer size, which cannot be less than 32 bytes. 
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code. 
        '         

        Public Function getLocalIpAddress(index As Int32, ip As StringBuilder, ipSize As Int32) As Int32
            Return PortSIP_NativeMethods.PortSIP_getLocalIpAddress(index, ip, ipSize)
        End Function


        '!
        '  @brief Set user account info.
        '
        '  @param userName           Account (User name) of the SIP. Usually provided by an IP-Telephony service provider.
        '  @param displayName        The display name of user. You can set it as your like, such as "James Kend". It's optional.
        '  @param authName           Authorization user name (usually equal to the username).
        '  @param password           The password of user. It's optional.
        '  @param localIp            The local computer IP address to be bound. For example: 192.168.1.108. It will be used for sending and receiving SIP message and RTP packet. If pass this IP as the IPv6 format, the SDK will use IPv6.
        '  @param localSipPort       The SIP message transport listener port. For example: 5060.
        '  @param userDomain         User domain; this parameter is optional that allows to pass an empty string if you are not using the domain.
        '  @param sipServer          SIP proxy server IP or domain. For example: xx.xxx.xx.x or sip.xxx.com.
        '  @param sipServerPort      Port of the SIP proxy server. For example: 5060.
        '  @param stunServer         Stun server used for NAT traversal. It's optional and can pass empty string to disable STUN.
        '  @param stunServerPort     STUN server port. It will be ignored if the outboundServer is empty.
        '  @param outboundServer     Outbound proxy server. For example: sip.domain.com. It's optional and allows to pass a empty string if not using outbound server.
        '  @param outboundServerPort Outbound proxy server port, it will be ignored if the outboundServer is empty.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function setUser(userName As [String], displayName As [String], authName As [String], password As [String], sipDomain As [String], sipServerAddr As [String],
            sipServerPort As Int32, stunServerAddr As [String], stunServerPort As Int32, outboundServerAddr As [String], outboundServerPort As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_setUser(_LibSDK, userName, displayName, authName, password, sipDomain,
                sipServerAddr, sipServerPort, stunServerAddr, stunServerPort, outboundServerAddr, outboundServerPort)

        End Function

        '!
        '  @brief Remove the user. It will un-register from SIP server given that the user is already registered.
        '         

        Public Sub removeUser()
            If _LibSDK = IntPtr.Zero Then
                Return
            End If

            PortSIP_NativeMethods.PortSIP_removeUser(_LibSDK)
        End Sub

        '!
        '  @brief Set the display name of user.
        '
        '  @param displayName that will appear in the From/To Header. 
        '  
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function setDisplayName(displayName As [String]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_setDisplayName(_LibSDK, displayName)
        End Function

        '!
        '  @brief Set outbound (RFC5626) instanceId to be used in contact headers
        '
        '  @param uuid The ID that will appear in the contact header. Please make sure it's a unique ID.
        '  
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function setInstanceId(uuid As [String]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_setInstanceId(_LibSDK, uuid)
        End Function



        '!
        '  @brief Register to SIP proxy server (login to server).
        '
        '  @param expires Registration refresh Interval in seconds with maximum 3600. It will be inserted into SIP REGISTER message headers.
        '   @param retryTimes The retry times if failed to refresh the registration. If it's set to be less than or equal to 0, the retry will be disabled and onRegisterFailure callback triggered when retry failed.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '  If registration to server succeeded, onRegisterSuccess will be triggered, otherwise onRegisterFailure triggered.
        '         

        Public Function registerServer(expires As Int32, retryTimes As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_registerServer(_LibSDK, expires, retryTimes)
        End Function


        '!
        '  @brief Refresh the registration manually after successfully registered.
        '
        '  @param expires Registration refresh Interval with maximum 3600, in seconds. It will be inserted into SIP REGISTER message headers.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '  If registration to server succeeded, onRegisterSuccess will be triggered, otherwise onRegisterFailure triggered.
        '         

        Private Function refreshRegistration(regExpires As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_refreshRegistration(_LibSDK, regExpires)
        End Function

        '!
        '  @brief Un-register from the SIP proxy server.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function unRegisterServer() As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_unRegisterServer(_LibSDK)
        End Function

        '!
        '  @brief Enable/disable rport(RFC3581).
        '
        '  @param enable Set to true to enable the SDK to support rport. By default it is enabled.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function enableRport(enable As [Boolean]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_enableRport(_LibSDK, enable)
        End Function

        '!
        '  @brief Enable/disable Early Media.
        '
        '  @param enable Set to true to enable the SDK to support Early Media. By default the Early Media is disabled.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function enableEarlyMedia(enable As [Boolean]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_enableEarlyMedia(_LibSDK, enable)
        End Function

        '!
        '  @brief Enable/disable PRACK.
        '
        '  @param enable Set to true to enable the SDK to support PRACK. The PRACK is disabled by default.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function enableReliableProvisional(enable As [Boolean]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_enableReliableProvisional(_LibSDK, enable)
        End Function

        '!
        '  @brief Enable/disable the 3Gpp tags, including "ims.icsi.mmtel" and "g.3gpp.smsip".
        '
        '  @param enable Set to true to enable SDK to support 3Gpp tags.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function enable3GppTags(enable As [Boolean]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_enable3GppTags(_LibSDK, enable)
        End Function

        '!
        '   @brief Enable/disable callback the sent SIP messages.
        '   
        '   @param enableSending Set as true to enable to callback the sent SIP messages, or false to disable. Once enabled, the "onSendingSignaling" event will be triggered when the SDK sends a SIP message.
        '   @param enableReceived Set as true to enable to callback the received SIP messages, or false to disable. Once enabled, the "onReceivedSignaling" event will be triggered when the SDK receives a SIP message.
        '         

        Public Sub enableCallbackSignaling(enableSending As [Boolean], enableReceived As [Boolean])
            If _LibSDK = IntPtr.Zero Then
                Return
            End If

            PortSIP_NativeMethods.PortSIP_enableCallbackSignaling(_LibSDK, enableSending, enableReceived)
        End Sub

        '!
        '  @brief Set the RTP callbacks to allow access to the sent and received RTP packets.
        '
        '  @param callbackObject The callback object that you passed in can be accessed once the callback function triggered.
        '  @param enable Set to true to enable the RTP callback for received and sent RTP packets. The onSendingRtpPacket and onReceivedRtpPacket events will be triggered.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function setRtpCallback(callbackObject As Int32, enable As [Boolean]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            If enable = True Then
                Return PortSIP_NativeMethods.PortSIP_setRtpCallback(_LibSDK, CType(callbackObject, IntPtr), _rrc, _src)
            End If

            Return PortSIP_NativeMethods.PortSIP_setRtpCallback(_LibSDK, CType(callbackObject, IntPtr), Nothing, Nothing)
        End Function



        '* @} 

        ' end of group2

        '* @defgroup group3 Audio and video codecs functions
        ' @{
        '         

        '!
        '  @brief Enable an audio codec. It will be appears in SDP.
        '
        '  @param codecType Audio codec type.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function addAudioCodec(codecType As AUDIOCODEC_TYPE) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_addAudioCodec(_LibSDK, DirectCast(codecType, Int32))
        End Function

        '!
        '  @brief Enable a video codec. It will appear in SDP.
        '
        '  @param codecType Video codec type.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function addVideoCodec(codecType As VIDEOCODEC_TYPE) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_addVideoCodec(_LibSDK, DirectCast(codecType, Int32))
        End Function


        '!
        '  @brief Detect if enabled audio codecs is empty or not.
        '
        '  @return If no audio codec is enabled, it will return value true, otherwise false.
        '         

        Public Function isAudioCodecEmpty() As [Boolean]
            If _LibSDK = IntPtr.Zero Then
                Return True
            End If

            Return PortSIP_NativeMethods.PortSIP_isAudioCodecEmpty(_LibSDK)
        End Function


        '!
        '  @brief Detect if enabled video codecs is empty or not.
        '
        '  @return If no video codec is enabled, it will return value true, otherwise false.
        '         

        Public Function isVideoCodecEmpty() As [Boolean]
            If _LibSDK = IntPtr.Zero Then
                Return True
            End If

            Return PortSIP_NativeMethods.PortSIP_isVideoCodecEmpty(_LibSDK)
        End Function

        '!
        '  @brief Set the RTP payload type for dynamic audio codec.
        '
        '  @param codecType   Audio codec type, which is defined in the PortSIPTypes file.
        '  @param payloadType The new RTP payload type that you want to set.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return value a specific error code.
        '         

        Public Function setAudioCodecPayloadType(codecType As AUDIOCODEC_TYPE, payloadType As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_setAudioCodecPayloadType(_LibSDK, DirectCast(codecType, Int32), payloadType)
        End Function

        '!
        '  @brief Set the RTP payload type for dynamic Video codec.
        '
        '  @param codecType   Video codec type, which is defined in the PortSIPTypes file.
        '  @param payloadType The new RTP payload type that you want to set.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function setVideoCodecPayloadType(codecType As VIDEOCODEC_TYPE, payloadType As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_setVideoCodecPayloadType(_LibSDK, DirectCast(codecType, Int32), payloadType)
        End Function


        '!
        '  @brief Remove all enabled audio codecs.
        '         

        Public Sub clearAudioCodec()
            If _LibSDK = IntPtr.Zero Then
                Return
            End If

            PortSIP_NativeMethods.PortSIP_clearAudioCodec(_LibSDK)
        End Sub

        '!
        '  @brief Remove all enabled video codecs.
        '         

        Public Sub clearVideoCodec()
            If _LibSDK = IntPtr.Zero Then
                Return
            End If

            PortSIP_NativeMethods.PortSIP_clearVideoCodec(_LibSDK)
        End Sub

        '!
        '  @brief Set the codec parameter for audio codec.
        '
        '  @param codecType Audio codec type, defined in the PortSIPTypes file.
        '  @param parameter The parameter in string format.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        ' @remark Example:
        '@code setAudioCodecParameter(AUDIOCODEC_AMR, "mode-set=0; octet-align=1; robust-sorting=0"); @endcode
        '         

        Public Function setAudioCodecParameter(codecType As AUDIOCODEC_TYPE, parameter As [String]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_setAudioCodecParameter(_LibSDK, DirectCast(codecType, Int32), parameter)
        End Function

        '!
        '  @brief Set the codec parameter for video codec.
        '
        '  @param codecType Video codec type, defined in the PortSIPTypes file.
        '  @param parameter The parameter in string format.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return value a specific error code.
        ' @remark Example:
        '@code setVideoCodecParameter(VIDEO_CODEC_H264, "profile-level-id=420033; packetization-mode=0"); @endcode
        '         

        Public Function setVideoCodecParameter(codecType As VIDEOCODEC_TYPE, parameter As [String]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_setVideoCodecParameter(_LibSDK, DirectCast(codecType, Int32), parameter)
        End Function
        '* @} 

        ' end of group3

        '* @defgroup group4 Additional setting functions
        ' @{
        '         




        '!
        '  @brief Set the SRTP policy.
        '
        '  @param srtpPolicy The SRTP policy.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return value a specific error code.
        '         

        Public Function setSrtpPolicy(srtpPolicy As SRTP_POLICY, allowSrtpOverUnsecureTransport As [Boolean]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_setSrtpPolicy(_LibSDK, DirectCast(srtpPolicy, Int32), allowSrtpOverUnsecureTransport)
        End Function

        '!
        '  @brief Set the RTP ports range for audio and video streaming.
        '
        '  @param minimumRtpAudioPort The minimum RTP port for audio stream.
        '  @param maximumRtpAudioPort The maximum RTP port for audio stream.
        '  @param minimumRtpVideoPort The minimum RTP port for video stream.
        '  @param maximumRtpVideoPort The maximum RTP port for video stream.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '  @remark
        '  The port range ((max - min) % maxCallLines) should be greater than 4.
        '         

        Public Function setRtpPortRange(minimumRtpAudioPort As Int32, maximumRtpAudioPort As Int32, minimumRtpVideoPort As Int32, maximumRtpVideoPort As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_setRtpPortRange(_LibSDK, minimumRtpAudioPort, maximumRtpAudioPort, minimumRtpVideoPort, maximumRtpVideoPort)
        End Function

        '!
        '  @brief Set the RTCP ports range for audio and video streaming.
        '
        '  @param minimumRtcpAudioPort The minimum RTCP port for audio stream.
        '  @param maximumRtcpAudioPort The maximum RTCP port for audio stream.
        '  @param minimumRtcpVideoPort The minimum RTCP port for video stream.
        '  @param maximumRtcpVideoPort The maximum RTCP port for video stream.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '  @remark
        '  The port range ((max - min) % maxCallLines) should be greater than 4.
        '         

        Public Function setRtcpPortRange(minimumRtcpAudioPort As Int32, maximumRtcpAudioPort As Int32, minimumRtcpVideoPort As Int32, maximumRtcpVideoPort As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_setRtcpPortRange(_LibSDK, minimumRtcpAudioPort, maximumRtcpAudioPort, minimumRtcpVideoPort, maximumRtcpVideoPort)
        End Function

        '!
        '  @brief Enable call forward.
        '
        '  @param forBusyOnly If this parameter is set as true, the SDK will forward all incoming calls when currently it's busy. If it's set as false, the SDK forward all inconing calls anyway.
        '  @param forwardTo   The call forward target. It must be like sip:xxxx@sip.portsip.com.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function enableCallForward(forBusyOnly As [Boolean], forwardTo As [String]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_enableCallForward(_LibSDK, forBusyOnly, forwardTo)
        End Function

        '!
        '  @brief Disable the call forwarding. The SDK is not forwarding any incoming call after this function is called.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, the return value is a specific error code.
        '         

        Public Function disableCallForward() As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_disableCallForward(_LibSDK)
        End Function

        '!
        '  @brief Allows to periodically refresh Session Initiation Protocol (SIP) sessions by sending INVITE requests repeatedly.
        '
        '  @param timerSeconds The value of the refreshment interval in seconds. Minimum value of 90 seconds required.
        '  @param refreshMode  Allow to set the session refresh by UAC or UAS: SESSION_REFERESH_UAC or SESSION_REFERESH_UAS;
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '  @remark The repeated INVITE requests, or re-INVITEs, are sent during an active call log to allow user agents (UA) or proxies to determine the status of a SIP session. 
        '  Without this keepalive mechanism, proxies that remember incoming and outgoing requests (stateful proxies) may continue to retain call state in vain. 
        '  If a UA fails to send a BYE message at the end of a session or if the BYE message is lost because of network problems, a stateful proxy will not know that the session has ended. 
        '  The re-INVITES ensure that active sessions stay active and completed sessions are terminated.
        '         

        Public Function enableSessionTimer(timerSeconds As Int32, refreshMode As SESSION_REFRESH_MODE) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_enableSessionTimer(_LibSDK, timerSeconds, DirectCast(refreshMode, Int32))
        End Function

        '!
        '  @brief Disable the session timer.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function disableSessionTimer() As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_disableSessionTimer(_LibSDK)
        End Function

        '!
        '  @brief Enable the "Do not disturb" to enable/disable.
        '
        '  @param state If it is set to true, the SDK will reject all incoming calls anyway.
        '         

        Public Sub setDoNotDisturb(state As [Boolean])
            If _LibSDK = IntPtr.Zero Then
                Return
            End If

            PortSIP_NativeMethods.PortSIP_setDoNotDisturb(_LibSDK, state)
        End Sub


        '!
        '  @brief Allows to enable/disable the check MWI (Message Waiting Indication) automatically.
        '
        '  @param state If it is set as true, MWI will be checked automatically once successfully registered to a SIP proxy server.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function enableAutoCheckMwi(state As [Boolean]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_enableAutoCheckMwi(_LibSDK, state)
        End Function

        '!
        '  @brief Enable or disable to send RTP keep-alive packet when the call is established.
        '
        '  @param state                Set to true to allow to send the keep-alive packet during the conversation.
        '  @param keepAlivePayloadType The payload type of the keep-alive RTP packet. It's usually set to 126.
        '  @param deltaTransmitTimeMS  The keep-alive RTP packet sending interval, in millisecond. Recommend value ranges 15000 - 300000.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function setRtpKeepAlive(state As [Boolean], keepAlivePayloadType As Int32, deltaTransmitTimeMS As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_setRtpKeepAlive(_LibSDK, state, keepAlivePayloadType, deltaTransmitTimeMS)
        End Function

        '!
        '  @brief Enable or disable to send SIP keep-alive packet.
        '
        '  @param keepAliveTime This is the SIP keep alive time interval in seconds. Set it to 0 to disable the SIP keep alive. Recommend to set as 30 or 50.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function setKeepAliveTime(keepAliveTime As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_setKeepAliveTime(_LibSDK, keepAliveTime)
        End Function



        '!
        '  @brief Access the SIP header of SIP message.
        '
        '  @param sipMessage        The SIP message.
        '  @param headerName        The header which wishes to access the SIP message.
        '  @param headerValue       The buffer to receive header value.
        '  @param headerValueLength The headerValue buffer size. Usually we recommend to set it more than 512 bytes.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        ' @remark
        ' When receiving a SIP message in the onReceivedSignaling callback event, and wishes to get SIP message header value, please use getSipMessageHeaderValue:
        ' @code
        '            StringBuilder value = new StringBuilder();
        '            value.Length = 512;
        '            getSipMessageHeaderValue(message, name, value);
        ' @endcode
        '         

        Public Function getSipMessageHeaderValue(sipMessage As [String], headerName As [String], headerValue As StringBuilder, headerValueLength As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_getSipMessageHeaderValue(_LibSDK, sipMessage, headerName, headerValue, headerValueLength)
        End Function

        '!
        '  @brief Add the extension header (custom header) into every outgoing SIP message.
        '
        '  @param headerName  The custom header name that will appears in every outgoing SIP message.
        '  @param headerValue The custom header value.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function addSipMessageHeader(sessionId As Int32, methodName As [String], msgType As Int32, headerName As [String], headerValue As [String]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_addSipMessageHeader(_LibSDK, sessionId, methodName, msgType, headerName, headerValue)
        End Function

        '!
        '  @brief Clear the added extension headers (custom headers)
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '  @remark For example, we have added two custom headers into every outgoing SIP message, and wish to remove them.
        ' @code
        '            addExtensionHeader("Blling", "usd100.00");	
        '            addExtensionHeader("ServiceId", "8873456");
        '            clearAddextensionHeaders();
        ' @endcode
        '         

        Public Function removeAddedSipMessageHeader(sipMessageHeaderId As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_removeAddedSipMessageHeader(_LibSDK, sipMessageHeaderId)
        End Function

        '!
        '  @brief Modify the special SIP header value for every outgoing SIP message.
        '
        '  @param headerName  The SIP header name for which the value will be modified.
        '  @param headerValue The heaver value to be modified.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return value a specific error code.
        '         

        Public Function modifySipMessageHeader(sessionId As Int32, methodName As [String], msgType As Int32, headerName As [String], headerValue As [String]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_modifySipMessageHeader(_LibSDK, sessionId, methodName, msgType, headerName, headerValue)
        End Function

        '!
        '  @brief Clear the modify headers value, no longer modify every outgoing SIP message header values.
        '
        '  @return If the function succeeds, the return value is 0. If the function fails, the return value is a specific error code.
        '  @remark  Example, modified two headers value for every outging SIP message and then clear it:
        '         @code
        '            modifyHeaderValue("Expires", "1000");	
        '            modifyHeaderValue("User-Agent", "MyTest Softphone 1.0");
        '            cleaModifyHeaders();
        '         @endcode
        '         

        Public Function removeModifiedSipMessageHeader(sipMessageHeaderId As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_removeModifiedSipMessageHeader(_LibSDK, sipMessageHeaderId)
        End Function


        Public Function clearAddedSipMessageHeaders() As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_clearAddedSipMessageHeaders(_LibSDK)
        End Function

        Public Function clearModifiedSipMessageHeaders() As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_clearModifiedSipMessageHeaders(_LibSDK)
        End Function


        '* @} 

        ' end of group5

        '* @defgroup group6 Audio and video functions
        ' @{
        '         



        '!
        '  @brief Set the SDK to receive the SIP messages that include special mime type.
        '
        '  @param methodName  Method name of the SIP message, such as INVITE, OPTION, INFO, MESSAGE, UPDATE, ACK etc. For more details please read the RFC3261.
        '  @param mimeType    The mime type of SIP message.
        '  @param subMimeType The sub mime type of SIP message.
        '  
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '@remark         
        ' By default, PortSIP VoIP SDK supports these media types (mime types) below for incoming SIP messages:
        ' @code
        '                        "message/sipfrag" in NOTIFY message.
        '                        "application/simple-message-summary" in NOTIFY message.
        '                        "text/plain" in MESSAGE message.
        '                        "application/dtmf-relay" in INFO message.
        '                        "application/media_control+xml" in INFO message.
        ' @endcode
        ' The SDK allows to receive SIP messages that include above mime types. Now if remote side send an INFO
        ' SIP message with its "Content-Type" header value "text/plain". SDK will reject this INFO message,
        ' as "text/plain" of INFO message is not included in the default support list.
        ' How should we enable the SDK to receive the SIP INFO message that includes "text/plain" mime type? The answer is
        ' addSupportedMimyType: 
        ' @code
        '                        addSupportedMimeType("INFO", "text", "plain");
        ' @endcode
        ' If we want to receive the NOTIFY message with "application/media_control+xml", please: 
        '@code
        '                        addSupportedMimeType("NOTIFY", "application", "media_control+xml");
        ' @endcode
        ' For more details about the mime type, please visit this website: http://www.iana.org/assignments/media-types/ 
        '         

        Public Function addSupportedMimeType(methodName As [String], mimeType As [String], subMimeType As [String]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_addSupportedMimeType(_LibSDK, methodName, mimeType, subMimeType)
        End Function



        '!
        '  @brief Set the audio capture sample.
        '
        '  @param ptime    It should be a multiple of 10 between 10 - 60 (with 10 and 60 inclusive).
        '  @param maxPtime For the "maxptime" attribute, it should be a multiple of 10 between 10 - 60 (with 10 and 60 inclusive). It cannot be less than "ptime".
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '  @remark It will appear in the SDP of INVITE and 200 OK message as "ptime and "maxptime" attribute.
        '         

        Public Function setAudioSamples(ptime As Int32, maxPtime As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_setAudioSamples(_LibSDK, ptime, maxPtime)
        End Function



        '* @} 

        ' end of group4

        '* @defgroup group5 Access SIP message header functions
        ' @{
        '         




        '!
        '  @brief Set the audio device that will be used for audio call. 
        '
        '  @param recordingDeviceId    Device ID (index) for audio recording. (Microphone). 
        '  @param playoutDeviceId      Device ID (index) for audio playback (Speaker). 
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function setAudioDeviceId(recordingDeviceId As Int32, playoutDeviceId As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_setAudioDeviceId(_LibSDK, recordingDeviceId, playoutDeviceId)
        End Function

        '!
        '  @brief Set the video device that will be used for video call.
        '
        '  @param deviceId Device ID (index) for video device (camera).
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function setVideoDeviceId(deviceId As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_setVideoDeviceId(_LibSDK, deviceId)
        End Function

        '!
        '  @brief Set the video capturing resolution.
        '
        '  @param width Video width.
        '  @param height Video height.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function setVideoResolution(width As Int32, height As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_setVideoResolution(_LibSDK, width, height)
        End Function

        '!
        ' *  @brief Set the audio bitrate.
        ' *
        ' *  @param bitrateKbps The audio bitrate in KBPS.
        ' *
        ' *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        ' 

        Public Function setAudioBitrate(sessionId As Int32, audioCodecType As AUDIOCODEC_TYPE, bitrateKbps As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_setAudioBitrate(_LibSDK, sessionId, DirectCast(audioCodecType, Int32), bitrateKbps)
        End Function

        '!
        '  @brief Set the video bitrate.
        '
        '  @param bitrateKbps The video bitrate in KBPS.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function setVideoBitrate(sessionId As Int32, bitrateKbps As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_setVideoBitrate(_LibSDK, sessionId, bitrateKbps)
        End Function

        '!
        '  @brief Set the video frame rate. 
        '
        '  @param frameRate The frame rate value with minimum value 5, and maximum value 30. A greater value will enable you better video quality but requires more bandwidth.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '  @remark Usually you do not need to call this function to set the frame rate. The SDK uses default frame rate.
        '         

        Public Function setVideoFrameRate(sessionId As Int32, frameRate As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_setVideoFrameRate(_LibSDK, sessionId, frameRate)
        End Function

        '!
        '  @brief Send the video to remote side.
        '
        '  @param sessionId The session ID of the call.
        '  @param sendState Set to true to send the video, or false to stop sending.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function sendVideo(sessionId As Int32, sendState As [Boolean]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_sendVideo(_LibSDK, sessionId, sendState)
        End Function


        '!
        '  @brief Mute the device microphone. It's unavailable for Android and iOS.
        '
        '  @param mute If the value is set to true, the microphone will be muted. You may also set it to false to un-mute it.
        '         

        Public Sub muteMicrophone(mute As [Boolean])
            If _LibSDK = IntPtr.Zero Then
                Return
            End If

            PortSIP_NativeMethods.PortSIP_muteMicrophone(_LibSDK, mute)
        End Sub

        '!
        '  @brief Mute the device speaker. It's unavailable for Android and iOS.
        '
        '  @param mute If the value is set to true, the speaker is muted. You may also set it to false to un-mute it.
        '         

        Public Sub muteSpeaker(mute As [Boolean])
            If _LibSDK = IntPtr.Zero Then
                Return
            End If

            PortSIP_NativeMethods.PortSIP_muteSpeaker(_LibSDK, mute)
        End Sub


        '!
        ' Set a volume |scaling| to be applied to the outgoing signal of a specific
        ' audio channel. 
        ' 
        ' @param sessionId
        '            The session ID of the call.
        ' @param scaling
        '            Valid scale ranges [0, 1000]. Default is 100.
        ' @return If the function succeeds, it will return value 0. If the function
        '         fails, it will return a specific error code.
        '         

        Public Sub setChannelOutputVolumeScaling(sessionId As Int32, scaling As Int32)
            If _LibSDK = IntPtr.Zero Then
                Return
            End If

            PortSIP_NativeMethods.PortSIP_setChannelOutputVolumeScaling(_LibSDK, sessionId, scaling)
        End Sub

        'Public Sub setChannelInputVolumeScaling(sessionId As Int32, scaling As Int32)
        '    If _LibSDK = IntPtr.Zero Then
        '       Return
        '  End If
        '
        '   PortSIP_NativeMethods.PortSIP_setChannelInputVolumeScaling(_LibSDK, sessionId, scaling)
        'End Sub

        '!
        ' Set a volume |scaling| to be applied to the microphone signal of a specific
        ' audio channel. 
        ' 
        ' @param sessionId
        '            The session ID of the call.
        ' @param scaling
        '            Valid scale ranges [0, 1000]. Default is 100.
        ' @return If the function succeeds, it will return value 0. If the function
        '         fails, it will return a specific error code.
        '         
        Public Sub setChannelInputVolumeScaling(sessionId As Int32, scaling As Int32)
            If _LibSDK = IntPtr.Zero Then
                Return
            End If

            PortSIP_NativeMethods.PortSIP_setChannelInputVolumeScaling(_LibSDK, sessionId, scaling)
        End Sub


        '!
        '  @brief Set the window that is used to display the local video image.
        '
        '  @param localVideoWindow The window on which the local video image from camera will be displayed. 
        '         

        Public Sub setLocalVideoWindow(localVideoWindow As IntPtr)
            If _LibSDK = IntPtr.Zero Then
                Return
            End If

            PortSIP_NativeMethods.PortSIP_setLocalVideoWindow(_LibSDK, localVideoWindow)
        End Sub

        '!
        '  @brief Set the window for a session that is used to display the received remote video image.
        '
        '  @param sessionId         The session ID of the call.
        '  @param remoteVideoWindow The window to display received remote video image. 
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function setRemoteVideoWindow(sessionId As Int32, remoteVideoWindow As IntPtr) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_setRemoteVideoWindow(_LibSDK, sessionId, remoteVideoWindow)
        End Function

        '!
        '  @brief Start/stop displaying the local video image.
        '
        '  @param state Set to true to display local video image.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function displayLocalVideo(state As [Boolean]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_displayLocalVideo(_LibSDK, state)
        End Function

        '!
        '  @brief Enable/disable the NACK feature (rfc6642) that helps to improve the video quality.
        '
        '  @param state Set to true to enable.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function setVideoNackStatus(state As [Boolean]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_setVideoNackStatus(_LibSDK, state)
        End Function



        '* @} 

        ' end of group6

        '* @defgroup group7 Call functions
        ' @{
        '         


        '!
        '  @brief Make a call
        '
        '  @param callee    The callee. It can be either name or full SIP URI. For example: user001, sip:user001@sip.iptel.org or sip:user002@sip.yourdomain.com:5068
        '  @param sendSdp   If it's set to false, the outgoing call doesn't include the SDP in INVITE message.
        '  @param videoCall If it's set to true with at least one video codecs added, the outgoing call will include the video codec into SDP.
        '
        '  @return If the function succeeds, it will return the session ID of the call that is greater than 0. If the function fails, it will return a specific error code. Note: the function success just means the outgoing call is being processed. You need to detect the call final state in onInviteTrying, onInviteRinging, onInviteFailure callback events.
        '         

        Public Function [call](callee As [String], sendSdp As [Boolean], videoCall As [Boolean]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.INVALID_SESSION_ID
            End If

            Return PortSIP_NativeMethods.PortSIP_call(_LibSDK, callee, sendSdp, videoCall)

        End Function

        '!
        '  @brief rejectCall Reject the incoming call.
        '
        '  @param sessionId The sessionId of the call.
        '  @param code      Reject code. For example, 486, 480 etc.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function rejectCall(sessionId As Int32, code As Integer) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_rejectCall(_LibSDK, sessionId, code)
        End Function

        '!
        '  @brief hangUp Hang up the call.
        '
        '  @param sessionId Session ID of the call.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function hangUp(sessionId As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_hangUp(_LibSDK, sessionId)
        End Function

        '!
        '  @brief answerCall Answer the incoming call.
        '
        '  @param sessionId The session ID of call.
        '  @param videoCall If the incoming call is a video call and the video codec is matched, set it to true to answer the video call.<br>If it's set to false, the answered call will not include video codec answer anyway.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function answerCall(sessionId As Int32, videoCall As [Boolean]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_answerCall(_LibSDK, sessionId, videoCall)
        End Function

        '!
        '  @brief Use the re-INVITE to update the established call.
        '  @param sessionId   The session ID of call.
        '  @param enableAudio Set to true to allow the audio in updated call, or false to disable audio in updated call.
        '  @param enableVideo Set to true to allow the video in updated call, or false to disable video in updated call.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return value a specific error code.
        '  @remark
        '            Example usage:<br>
        '  Example 1: A called B with the audio only, B answered A, there has an audio conversation between A, B. Now A wants to see B visually, 
        '            A could use these functions to do it.
        '            @code
        '                        clearVideoCodec();	
        '                        addVideoCodec(VIDEOCODEC_H264);
        '                        updateCall(sessionId, true, true);
        '            @endcode
        '            Example 2: Remove video stream from current conversation. 
        '            @code
        '                        updateCall(sessionId, true, false);
        '            @endcode
        '         

        Public Function updateCall(sessionId As Int32, enableAudio As Boolean, enableVideo As Boolean) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_updateCall(_LibSDK, sessionId, enableAudio, enableVideo)
        End Function

        '!
        '  @brief To place a call on hold.
        '
        '  @param sessionId The session ID of call.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function hold(sessionId As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_hold(_LibSDK, sessionId)
        End Function

        '!
        '  @brief Take off hold.
        '
        '  @param sessionId The session ID of call.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function unHold(sessionId As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_unHold(_LibSDK, sessionId)
        End Function

        '!
        '  @brief Mute the specified session audio or video.
        '
        '  @param sessionId         The session ID of the call.
        '  @param muteIncomingAudio Set it to true to mute incoming audio stream, and remote side audio cannot be heard.
        '  @param muteOutgoingAudio Set it to true to mute outgoing audio stream, and the remote side can't hear the audio.
        '  @param muteIncomingVideo Set it to true to mute incoming video stream, and the remote side video will be invisible.
        '  @param muteOutgoingVideo Set it to true to mute outgoing video stream, and the remote side can't see the video.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function muteSession(sessionId As Int32, muteIncomingAudio As [Boolean], muteOutgoingAudio As [Boolean], muteIncomingVideo As [Boolean], muteOutgoingVideo As [Boolean]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_muteSession(_LibSDK, sessionId, muteIncomingAudio, muteOutgoingAudio, muteIncomingVideo, muteOutgoingVideo)
        End Function

        '!
        '  @brief Forward call to another one when receiving the incoming call.
        '
        '  @param sessionId The session ID of the call.
        '  @param forwardTo Target of the forwarding. It can be "sip:number@sipserver.com" or "number" only.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return value a specific error code.
        '         

        Public Function forwardCall(sessionId As Int32, forwardTo As [String]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_forwardCall(_LibSDK, sessionId, forwardTo)
        End Function



        '!
        '        *  @brief This function will be used for picking up a call based on the BLF (Busy Lamp Field) status.
        '        *
        '        *  @param replaceDialogId The session ID of the call.
        '        *  @param videoCall Target of the forwarding. It can be "sip:number@sipserver.com" or "number" only.
        '        *
        '        *  @return If the function succeeds, it will return a session ID that is greater than 0 to the new call,
        '	    * otherwise returns a specific error code that is less than 0.
        '        *  @remark
        '            The scenario is:<br>
        '        *   1. User 101 subscribed the user 100's call status: sendSubscription(mSipLib, "100", "dialog");
        '        *   2. When 100 hold a call or 100 is ringing, onDialogStateUpdated callback will be triggered,
        '        *   and 101 will receive this callback. Now 101 can use pickupBLFCall function to pick the call rather than 
        '        *   100 to talk with caller.
        '        

        Public Function pickupBLFCall(replaceDialogId As [String], videoCall As [Boolean]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_pickupBLFCall(_LibSDK, replaceDialogId, videoCall)
        End Function



        '!
        '  @brief Send DTMF tone.
        '
        '  @param sessionId    The session ID of the call.
        '  @param dtmfMethod   DTMF tone could be sent with two methods: DTMF_RFC2833 and DTMF_INFO, of which DTMF_RFC2833 is recommend.
        '  @param code         The DTMF tone (0-16).
        ' <p><table>
        ' <tr><th>code</th><th>Description</th></tr>
        ' <tr><td>0</td><td>The DTMF tone 0.</td></tr><tr><td>1</td><td>The DTMF tone 1.</td></tr><tr><td>2</td><td>The DTMF tone 2.</td></tr>
        ' <tr><td>3</td><td>The DTMF tone 3.</td></tr><tr><td>4</td><td>The DTMF tone 4.</td></tr><tr><td>5</td><td>The DTMF tone 5.</td></tr>
        ' <tr><td>6</td><td>The DTMF tone 6.</td></tr><tr><td>7</td><td>The DTMF tone 7.</td></tr><tr><td>8</td><td>The DTMF tone 8.</td></tr>
        ' <tr><td>9</td><td>The DTMF tone 9.</td></tr><tr><td>10</td><td>The DTMF tone *.</td></tr><tr><td>11</td><td>The DTMF tone #.</td></tr>
        ' <tr><td>12</td><td>The DTMF tone A.</td></tr><tr><td>13</td><td>The DTMF tone B.</td></tr><tr><td>14</td><td>The DTMF tone C.</td></tr>
        ' <tr><td>15</td><td>The DTMF tone D.</td></tr><tr><td>16</td><td>The DTMF tone FLASH.</td></tr>
        ' </table></p>
        '  @param dtmfDuration The DTMF tone samples. Recommended value 160.
        '  @param playDtmfTone If it is set to true, the SDK plays local DTMF tone sound when sending DTMF.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function sendDtmf(sessionId As Int32, dtmfMethod As DTMF_METHOD, code As Integer, dtmfDuration As Integer, playDtmfTone As Boolean) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_sendDtmf(_LibSDK, sessionId, DirectCast(dtmfMethod, Int32), code, dtmfDuration, playDtmfTone)
        End Function
        '* @} 

        ' end of group7

        '* @defgroup group8 Refer functions
        ' @{
        '         


        '!
        '  @brief Refer the current call to another one.<br>
        '  @param sessionId The session ID of the call.
        '  @param referTo   Target of the refer, which can be either "sip:number@sipserver.com" or "number".
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '  @remark
        '         @code
        '            refer(sessionId, "sip:testuser12@sip.portsip.com");
        '         @endcode
        '         You can download the demo AVI at: <br>
        '         "http://www.portsip.com/downloads/video/blindtransfer.rar". <br>Use the Windows Media
        '         Player to play the AVI file after extracted, and it will demonstrate the transfer.
        '         

        Public Function refer(sessionId As Int32, referTo As [String]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_refer(_LibSDK, sessionId, referTo)
        End Function

        '!
        '  @brief  Make an attended refer.
        '
        '  @param sessionId        The session ID of the call.
        '  @param replaceSessionId Session ID of the repferred call.
        '  @param referTo          Target of the refer, which can be either "sip:number@sipserver.com" or "number".
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '  @remark
        '            Please read the sample project source code for more details, or download the demo AVI at: http://www.portsip.com/downloads/video/blindtransfer.rar<br>
        '           Please use the Windows Media Player to play the AVI file after extracted, and it will demonstrate the transfer.
        '         

        Public Function attendedRefer(sessionId As Int32, replaceSessionId As Int32, referTo As [String]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_attendedRefer(_LibSDK, sessionId, replaceSessionId, referTo)
        End Function

        '!
        '  @brief  Make an attended refer with specified request line and specified method embeded into the "Refer-To" header.
        '
        '  @param sessionId        Session ID of the call.
        '  @param replaceSessionId Session ID of the replaced call.
        '  @param replaceMethod    The SIP method name which will be embeded in the "Refer-To" header, usually INVITE or BYE.
        '  @param target           The target to which the REFER message will be sent. It appears in the "Request Line" of REFER message.
        '  @param referTo          Target of the refer that appears in the "Refer-To" header. It can be either "sip:number@sipserver.com" or "number".
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '  @remark
        '         Please refer to the sample project source code for more details. Or you can watch the video on YouTube at https://www.youtube.com/watch?v=_2w9EGgr3FY.
        '         It will demonstrate the transmission.
        '         

        Public Function attendedRefer2(libSDK As IntPtr, sessionId As Int32, replaceSessionId As Int32, replaceMethod As [String], target As [String], referTo As [String]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_attendedRefer2(_LibSDK, sessionId, replaceSessionId, replaceMethod, target, referTo)
        End Function

        '!
        '  @brief  Send an out of dialog REFER to replace the specified call.
        '
        '  @param replaceSessionId The session ID of the session which will be replaced.
        '  @param replaceMethod    The SIP method name which will be added in the "Refer-To" header, usually INVITE or BYE.
        '  @param target           The target to which the REFER message will be sent.
        '  @param referTo          The URI to be added into the "Refer-To" header.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function outOfDialogRefer(replaceSessionId As Int32, replaceMethod As [String], target As [String], referTo As [String]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_outOfDialogRefer(_LibSDK, replaceSessionId, replaceMethod, target, referTo)
        End Function

        '!
        '  @brief Accept the REFER request, and a new call will be made if called this function. The function is usually called after onReceivedRefer callback event.
        '
        '  @param referId        The ID of REFER request that comes from onReceivedRefer callback event.
        '  @param referSignalingMessage The SIP message of REFER request that comes from onReceivedRefer callback event.
        '
        '  @return If the function succeeds, it will return a session ID greater than 0 to the new call for REFER; otherwise a specific error code less than 0.
        '         

        Public Function acceptRefer(referId As Int32, referSignalingMessage As [String]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_acceptRefer(_LibSDK, referId, referSignalingMessage)
        End Function

        '!
        '  @brief Reject the REFER request.
        '
        '  @param referId The ID of REFER request that comes from onReceivedRefer callback event.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function rejectRefer(referId As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_rejectRefer(_LibSDK, referId)
        End Function
        '* @} 

        ' end of group8

        '* @defgroup group9 Send audio and video stream functions
        ' @{
        '         


        '!
        '  @brief Enable the SDK to send PCM stream data to remote side from another source instead of microphone.
        '
        '  @param sessionId           The session ID of call.
        '  @param state               Set to true to enable the send stream, or false to disable.
        '  @param streamSamplesPerSec The PCM stream data sample in seconds. For example: 8000 or 16000.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '  @remark This function MUST be called first to send the PCM stream data to another side.
        '         

        Public Function enableSendPcmStreamToRemote(sessionId As Int32, state As [Boolean], streamSamplesPerSec As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_enableSendPcmStreamToRemote(_LibSDK, sessionId, state, streamSamplesPerSec)
        End Function

        '!
        '  @brief Send the audio stream in PCM format from another source instead of audio device capturing (microphone).
        '
        '  @param sessionId Session ID of the call conversation.
        '  @param data        The PCM audio stream data. It must be 16bit, mono.
        '  @param dataLength  The size of data. 
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '  @remark Usually it should be used as below:
        '  @code
        '                        enableSendPcmStreamToRemote(sessionId, true, 16000);	
        '                        sendPcmStreamToRemote(sessionId, data, dataSize);
        '  @endcode
        '         

        Public Function sendPcmStreamToRemote(sessionId As Int32, data As Byte(), dataLength As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_sendPcmStreamToRemote(_LibSDK, sessionId, data, dataLength)
        End Function

        '!
        '  @brief Enable the SDK send video stream data to remote side from another source instead of camera.
        '
        '  @param sessionId The session ID of call.
        '  @param state     Set to true to enable the sending stream, or false to disable.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function enableSendVideoStreamToRemote(sessionId As Int32, state As [Boolean]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_enableSendVideoStreamToRemote(_LibSDK, sessionId, state)
        End Function

        '!
        '  @brief Send the video stream to remote side.
        '
        '  @param sessionId Session ID of the call conversation.
        '  @param data      The video stream data. It must be in i420 format.
        '  @param dataLength The size of data. 
        '  @param width     The video image width.
        '  @param height    The video image height.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '  @remark  Send the video stream in i420 from another source instead of video device capturing (camera).<br>
        '         Before calling this funtion, you MUST call the enableSendVideoStreamToRemote function.<br>
        ' Usually it should be used as below:
        '         @code
        '                    enableSendVideoStreamToRemote(sessionId, true);	
        '                    sendVideoStreamToRemote(sessionId, data, dataSize, 352, 288);
        '         @endcode
        '         

        Public Function sendVideoStreamToRemote(sessionId As Int32, data As Byte(), dataLength As Int32, width As Int32, height As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_sendVideoStreamToRemote(_LibSDK, sessionId, data, dataLength, width, height)
        End Function

        '* @} 

        ' end of group9

        '* @defgroup group10 RTP packets, Audio stream and video stream callback functions
        ' @{
        '         



        '!
        '  @brief Enable/disable the audio stream callback
        '
        '  @param callbackObject The callback object that you passed in can be accessed once callback function triggered.
        '  @param sessionId    The session ID of call.
        '  @param enable       Set to true to enable audio stream callback, or false to stop the callback.
        '  @param callbackMode The audio stream callback mode
        ' <p><table>
        ' <tr><th>Mode</th><th>Description</th></tr>
        ' <tr><td>AUDIOSTREAM_LOCAL_MIX</td><td>Callback the audio stream from microphone for all channels.  </td></tr>
        ' <tr><td>AUDIOSTREAM_LOCAL_PER_CHANNEL</td><td>Callback the audio stream from microphone for one channel based on the given sessionId. </td></tr>
        ' <tr><td>AUDIOSTREAM_REMOTE_MIX</td><td>Callback the received audio stream that mixed all included channels. </td></tr>
        ' <tr><td>AUDIOSTREAM_REMOTE_PER_CHANNEL</td><td>Callback the received audio stream for one channel based on the given sessionId.</td></tr>
        ' </table></p>
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '  @remark The onAudioRawCallback event will be triggered if the callback is enabled.
        '         

        Public Function enableAudioStreamCallback(callbackObject As Int32, sessionId As Int32, enable As [Boolean], callbackMode As AUDIOSTREAM_CALLBACK_MODE) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_enableAudioStreamCallback(_LibSDK, sessionId, enable, DirectCast(callbackMode, Int32), CType(callbackObject, IntPtr), _arc)
        End Function

        '!
        '  @brief Enable/disable the video stream callback.
        '
        '  @param callbackObject The callback object that you passed in can be accessed once callback function triggered.
        '  @param sessionId    The session ID of call.
        '  @param callbackMode The video stream callback mode.
        ' <p><table>
        ' <tr><th>Mode</th><th>Description</th></tr>
        ' <tr><td>VIDEOSTREAM_NONE</td><td>Disable video stream callback. </td></tr>
        ' <tr><td>VIDEOSTREAM_LOCAL</td><td>Local video stream callback. </td></tr>
        ' <tr><td>VIDEOSTREAM_REMOTE</td><td>Remote video stream callback. </td></tr>
        ' <tr><td>VIDEOSTREAM_BOTH</td><td>Both local and remote video stream callback. </td></tr>
        ' </table></p>
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '  @remark The onVideoRawCallback event will be triggered if the callback is enabled.
        '         

        Public Function enableVideoStreamCallback(callbackObject As Int32, sessionId As Int32, callbackMode As VIDEOSTREAM_CALLBACK_MODE) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If


            Return PortSIP_NativeMethods.PortSIP_enableVideoStreamCallback(_LibSDK, sessionId, DirectCast(callbackMode, Int32), CType(callbackObject, IntPtr), _vrc)
        End Function


        '* @} 

        ' end of group10

        '* @defgroup group11 Record functions
        ' @{
        '         


        '!
        '  @brief Start recording the call.
        '
        '  @param sessionId        The session ID of call conversation.
        '  @param recordFilePath   The file path to which the record file will be saved. It must be existent.
        '  @param recordFileName   The file name of record file. For example: audiorecord.wav or videorecord.avi.
        '  @param appendTimestamp  Set to true to append the timestamp to the recording file name.
        '  @param audioFileFormat  The audio record file format.
        '  @param audioRecordMode  The audio record mode.
        '  @param videoFileCodecType The codec which is used for compressing the video data to save into video record file.
        '  @param videoRecordMode  Allow to set video record mode, with record received and/or sent supported.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function startRecord(sessionId As Int32, recordFilePath As [String], recordFileName As [String], appendTimestamp As [Boolean], audioFileFormat As AUDIO_RECORDING_FILEFORMAT, audioRecordMode As RECORD_MODE,
            videoFileCodecType As VIDEOCODEC_TYPE, videoRecordMode As RECORD_MODE) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_startRecord(_LibSDK, sessionId, recordFilePath, recordFileName, appendTimestamp, DirectCast(audioFileFormat, Int32),
                DirectCast(audioRecordMode, Int32), DirectCast(videoFileCodecType, Int32), DirectCast(videoRecordMode, Int32))
        End Function

        '!
        '  @brief Stop record.
        '
        '  @param sessionId The session ID of call conversation.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function stopRecord(sessionId As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_stopRecord(_LibSDK, sessionId)
        End Function
        '* @} 

        ' end of group11

        '* @defgroup group12 Play audio and video file to remote functions
        ' @{
        '         



        '!
        '  @brief Play an AVI file to remote party.
        '
        '  @param sessionId Session ID of the call.
        '  @param fileName   The full file path, such as "c:\\test.avi".
        '  @param loop      Set to false to stop playing video file when it is ended, or true to play it repeatedly.
        '  @param playAudio If it's set to true, audio and video will be played together; if false, only video will be played.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function playVideoFileToRemote(sessionId As Int32, fileName As [String], [loop] As [Boolean], playAudio As [Boolean]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull


            End If

            Return PortSIP_NativeMethods.PortSIP_playVideoFileToRemote(_LibSDK, sessionId, fileName, [loop], playAudio)
        End Function

        '!
        '  @brief Stop playing video file to remote side.
        '
        '  @param sessionId Session ID of the call.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function stopPlayVideoFileToRemote(sessionId As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull


            End If

            Return PortSIP_NativeMethods.PortSIP_stopPlayVideoFileToRemote(_LibSDK, sessionId)
        End Function

        '!
        '  @brief Play a wave file to remote party.
        '
        '  @param sessionId         Session ID of the call.
        '  @param fileName          The full filepath, such as "c:\\test.wav".
        '  @param fileSamplesPerSec The wave file sample in seconds. It should be 8000, 16000 or 32000.
        '  @param loop              Set to false to stop playing audio file when it is ended, or true to play it repeatedly.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function playAudioFileToRemote(sessionId As Int32, fileName As [String], fileSamplesPerSec As Int32, [loop] As [Boolean]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull


            End If

            Return PortSIP_NativeMethods.PortSIP_playAudioFileToRemote(_LibSDK, sessionId, fileName, fileSamplesPerSec, [loop])
        End Function

        '!
        '  @brief Stop playing wave file to remote side.
        '
        '  @param sessionId Session ID of the call.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function stopPlayAudioFileToRemote(sessionId As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull


            End If

            Return PortSIP_NativeMethods.PortSIP_stopPlayAudioFileToRemote(_LibSDK, sessionId)
        End Function

        '!
        '  @brief Play a wave file to remote party as conversation background sound.
        '
        '  @param sessionId         Session ID of the call.
        '  @param fileName          The full filepath, such as "c:\\test.wav".
        '  @param fileSamplesPerSec The wave file sample in seconds. It should be 8000, 16000 or 32000.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function playAudioFileToRemoteAsBackground(sessionId As Int32, fileName As [String], fileSamplesPerSec As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull


            End If

            Return PortSIP_NativeMethods.PortSIP_playAudioFileToRemoteAsBackground(_LibSDK, sessionId, fileName, fileSamplesPerSec)
        End Function

        '!
        '  @brief Stop playing wave file to remote party as conversation background sound.
        '
        '  @param sessionId Session ID of the call.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function stopPlayAudioFileToRemoteAsBackground(sessionId As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull


            End If

            Return PortSIP_NativeMethods.PortSIP_stopPlayAudioFileToRemoteAsBackground(_LibSDK, sessionId)
        End Function

        '* @} 

        ' end of group12

        '* @defgroup group13 Conference functions
        ' @{
        '         


        '!
        '  @brief Create an audio conference. It will be failed if the existent conference is not ended yet.
        '  
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function createAudioConference() As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_createAudioConference(_LibSDK)
        End Function

        '!
        '  @brief Create a video conference. It will be failed if the existent conference is not ended yet.
        '
        '  @param conferenceVideoWindow         The UIView which is used to display the conference video.
        '  @param videoResolution               The conference video resolution.
        '  @param displayLocalVideoInConference Display the local video on video window or not. 
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function createVideoConference(conferenceVideoWindow As IntPtr, width As Int32, height As Int32, displayLocalVideoInConference As [Boolean]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_createVideoConference(_LibSDK, conferenceVideoWindow, width, height, displayLocalVideoInConference)
        End Function

        '!
        '  @brief End the existent conference.
        ' 
        '         

        Public Sub destroyConference()
            If _LibSDK = IntPtr.Zero Then
                Return
            End If

            PortSIP_NativeMethods.PortSIP_destroyConference(_LibSDK)
        End Sub

        '!
        '  @brief Set the window for a conference that is used to display the received remote video image.
        '
        '  @param videoWindow The UIView which is used to display the conference video.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function setConferenceVideoWindow(videoWindow As IntPtr) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_setConferenceVideoWindow(_LibSDK, videoWindow)
        End Function

        '!
        '  @brief Join a session into existent conference. If the call is in hold, it will be un-hold automatically.
        '
        '  @param sessionId Session ID of the call.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function joinToConference(sessionId As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_joinToConference(_LibSDK, sessionId)
        End Function

        '!
        '  @brief Remove a session from an existent conference.
        '
        '  @param sessionId Session ID of the call.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function removeFromConference(sessionId As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_removeFromConference(_LibSDK, sessionId)
        End Function

        '* @} 

        ' end of group13

        '* @defgroup group14 RTP and RTCP QOS functions
        ' @{
        '         


        '!
        '  @brief Set the audio RTCP bandwidth parameters to the RFC3556.
        '
        '  @param sessionId The session ID of call conversation.
        '  @param BitsRR    The bits for the RR parameter.
        '  @param BitsRS    The bits for the RS parameter.
        '  @param KBitsAS   The Kbits for the AS parameter.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function setAudioRtcpBandwidth(sessionId As Int32, BitsRR As Int32, BitsRS As Int32, KBitsAS As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_setAudioRtcpBandwidth(_LibSDK, sessionId, BitsRR, BitsRS, KBitsAS)
        End Function

        '!
        '  @brief Set the video RTCP bandwidth parameters as the RFC3556.
        '
        '  @param sessionId The session ID of call conversation.
        '  @param BitsRR    The bits for the RR parameter.
        '  @param BitsRS    The bits for the RS parameter.
        '  @param KBitsAS   The Kbits for the AS parameter.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function setVideoRtcpBandwidth(sessionId As Int32, BitsRR As Int32, BitsRS As Int32, KBitsAS As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_setVideoRtcpBandwidth(_LibSDK, sessionId, BitsRR, BitsRS, KBitsAS)
        End Function


        '* @} 

        ' end of group14

        '* @defgroup group15 RTP statistics functions
        ' @{
        '         


        '!
        ' *  @brief Obtain the statistics of audio channel.
        ' *
        ' *  @param sessionId          The session ID of call conversation.
        ' *  @param sendBytes          The number of sent bytes.
        ' *  @param sendPackets        The number of sent packets.
        ' *  @param sendPacketsLost    The number of sent but lost packet.
        ' *  @param sendFractionLost   Fraction of sent but lost packet in percentage.
        ' *  @param sendRttMS          The round-trip time of the session, in milliseconds.
        ' *  @param sendCodecType      Send Audio codec Type.
        ' *  @param sendJitterMS       The sent jitter, in milliseconds.
        ' *  @param sendAudioLevel     The sent audio level.It ranges 0 - 9.
        ' *  @param recvBytes          The number of received bytes.
        ' *  @param recvPackets        The number of received packets.
        ' *  @param recvPacketsLost    The number of received but lost packet.
        ' *  @param recvFractionLost   Fraction of received but lost packet in percentage.
        ' *  @param recvCodecType      Received Audio codec Type.
        ' *  @param recvJitterMS       The received jitter, in milliseconds.
        ' *  @param recvAudioLevel     The received audio level.It ranges 0 - 9.
        ' *
        ' *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        ' *
        Public Function getAudioStatistics(ByVal sessionId As Int32, <Out> ByRef sendBytes As Int32, <Out> ByRef sendPackets As Int32, <Out> ByRef sendPacketsLost As Int32, <Out> ByRef sendFractionLost As Int32, <Out> ByRef sendRttMS As Int32, <Out> ByRef sendCodecType As Int32, <Out> ByRef sendJitterMS As Int32, <Out> ByRef sendAudioLevel As Int32, <Out> ByRef recvBytes As Int32, <Out> ByRef recvPackets As Int32, <Out> ByRef recvPacketsLost As Int32, <Out> ByRef recvFractionLost As Int32, <Out> ByRef recvCodecType As Int32, <Out> ByRef recvJitterMS As Int32, <Out> ByRef recvAudioLevel As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                sendBytes = 0
                sendPackets = 0
                sendPacketsLost = 0
                sendFractionLost = 0
                sendRttMS = 0
                sendCodecType = 0
                sendJitterMS = 0
                sendAudioLevel = 0
                recvBytes = 0
                recvPackets = 0
                recvPacketsLost = 0
                recvFractionLost = 0
                recvCodecType = 0
                recvJitterMS = 0
                recvAudioLevel = 0
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_getAudioStatistics(_LibSDK, sessionId, sendBytes, sendPackets, sendPacketsLost, sendFractionLost, sendRttMS, sendCodecType, sendJitterMS, sendAudioLevel, recvBytes, recvPackets, recvPacketsLost, recvFractionLost, recvCodecType, recvJitterMS, recvAudioLevel)
        End Function



        '!
        ' *  @brief Obtain the RTP statisics of video.
        ' *
        ' *  @param sessionId       The session ID of call conversation.
        ' *  @param sendBytes          The number of sent bytes.
        ' *  @param sendPackets        The number of sent packets.
        ' *  @param sendPacketsLost    The number of sent but lost packet.
        ' *  @param sendFractionLost   Fraction of sent lost in percentage.
        ' *  @param sendRttMS          The round-trip time of the session, in milliseconds.
        ' *  @param sendCodecType      Send Video codec Type.
        ' *  @param sendFrameWidth     Frame width for the sent video.
        ' *  @param sendFrameHeight    Frame height for the sent video.
        ' *  @param sendBitrateBPS     Bitrate in BPS for the sent video.
        ' *  @param sendFramerate      Frame rate for the sent video.
        ' *  @param recvBytes          The number of received bytes.
        ' *  @param recvPackets        The number of received packets.
        ' *  @param recvPacketsLost    The number of received but lost packet.
        ' *  @param recvFractionLost   Fraction of received but lost packet in percentage.
        ' *  @param recvCodecType      Received Video codec Type.
        ' *  @param recvFrameWidth     Frame width for the received video.
        ' *  @param recvFrameHeight    Frame height for the received video.
        ' *  @param recvBitrateBPS     (This parameter is not implemented yet) Bitrate in BPS for the received video.
        ' *  @param recvFramerate      Framerate for the received video.
        ' *
        ' *  @return  If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        ' *    

        Public Function getVideoStatistics(ByVal sessionId As Int32, <Out> ByRef sendBytes As Int32, <Out> ByRef sendPackets As Int32, <Out> ByRef sendPacketsLost As Int32,
                                           <Out> ByRef sendFractionLost As Int32, <Out> ByRef sendRttMS As Int32, <Out> ByRef sendCodecType As Int32, <Out> ByRef sendFrameWidth As Int32,
                                           <Out> ByRef sendFrameHeight As Int32, <Out> ByRef sendBitrateBPS As Int32, <Out> ByRef sendFramerate As Int32, <Out> ByRef recvBytes As Int32, <Out> ByRef recvPackets As Int32,
                                           <Out> ByRef recvPacketsLost As Int32, <Out> ByRef recvFractionLost As Int32, <Out> ByRef recvCodecType As Int32, <Out> ByRef recvFrameWidth As Int32,
                                           <Out> ByRef recvFrameHeight As Int32, <Out> ByRef recvBitrateBPS As Int32, <Out> ByRef recvFramerate As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                sendBytes = 0
                sendPackets = 0
                sendPacketsLost = 0
                sendFractionLost = 0
                sendRttMS = 0
                sendCodecType = 0
                sendFrameWidth = 0
                sendFrameHeight = 0
                sendBitrateBPS = 0
                sendFramerate = 0
                recvBytes = 0
                recvPackets = 0
                recvPacketsLost = 0
                recvFractionLost = 0
                recvCodecType = 0
                recvFrameWidth = 0
                recvFrameHeight = 0
                recvBitrateBPS = 0
                recvFramerate = 0
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_getVideoStatistics(_LibSDK, sessionId, sendBytes, sendPackets, sendPacketsLost, sendFractionLost, sendRttMS, sendCodecType, sendFrameWidth, sendFrameHeight, sendBitrateBPS, sendFramerate, recvBytes, recvPackets, recvPacketsLost, recvFractionLost, recvCodecType, recvFrameWidth, recvFrameHeight, recvBitrateBPS, recvFramerate)
        End Function


        '* @} 

        ' end of group15

        '* @defgroup group16 Audio effect functions
        ' @{
        '         


        '!
        '  @brief Enable/disable Voice Activity Detection (VAD).
        '
        '  @param state Set to true to enable VAD, or false to disable.
        '         

        Public Sub enableVAD(state As [Boolean])
            If _LibSDK = IntPtr.Zero Then
                Return
            End If

            PortSIP_NativeMethods.PortSIP_enableVAD(_LibSDK, state)
        End Sub

        '!
        '  @brief Enable/disable AEC (Acoustic Echo Cancellation).
        '
        '  @param ecMode Allow to set the AEC mode to influence different scenarios.
        '  
        ' <p><table>
        ' <tr><th>Mode</th><th>Description</th></tr>
        ' <tr><td>EC_NONE</td><td>Disable AEC. </td></tr>
        ' <tr><td>EC_DEFAULT</td><td>Platform default AEC. </td></tr>
        ' <tr><td>EC_CONFERENCE</td><td>Desktop platform (Windows, MAC) Conferencing default (aggressive AEC). </td></tr>
        ' </table></p>
        ' 
        '         

        Public Sub enableAEC(ecMode As EC_MODES)
            If _LibSDK = IntPtr.Zero Then
                Return
            End If

            PortSIP_NativeMethods.PortSIP_enableAEC(_LibSDK, DirectCast(ecMode, Int32))
        End Sub

        '!
        '  @brief Enable/disable Comfort Noise Generator (CNG).
        '
        '  @param state Set it to true to enable CNG, or false to disable.
        '         

        Public Sub enableCNG(state As [Boolean])
            If _LibSDK = IntPtr.Zero Then
                Return
            End If

            PortSIP_NativeMethods.PortSIP_enableCNG(_LibSDK, state)
        End Sub

        '!
        '  @brief Enable/disable Automatic Gain Control (AGC).
        '
        '  @param agcMode  Allow to set the AGC mode to influence different scenarios.
        '  
        ' <p><table>
        ' <tr><th>Mode</th><th>Description</th></tr>
        ' <tr><td>AGC_DEFAULT</td><td>Disable AGC. </td></tr>
        ' <tr><td>AGC_DEFAULT</td><td>Platform default. </td></tr>
        ' <tr><td>AGC_ADAPTIVE_ANALOG</td><td>Desktop platform (Windows, MAC) adaptive mode for use when analog volume control exists. </td></tr>
        ' <tr><td>AGC_ADAPTIVE_DIGITAL</td><td>Scaling takes place in the digital domain (e.g. for conference servers and embedded devices). </td></tr>
        ' <tr><td>AGC_FIXED_DIGITAL</td><td>It can be used on embedded devices where the capture signal level is predictable. </td></tr>
        ' </table></p>
        '         

        Public Sub enableAGC(agcMode As AGC_MODES)
            If _LibSDK = IntPtr.Zero Then
                Return
            End If

            PortSIP_NativeMethods.PortSIP_enableAGC(_LibSDK, DirectCast(agcMode, Int32))
        End Sub

        '!
        '  @brief Enable/disable Audio Noise Suppression (ANS).
        '
        '  @param nsMode Allow to set the NS mode to influence different scenarios.
        '  
        ' <p><table>
        ' <tr><th>Mode                     </th><th>Description</th></tr>
        ' <tr><td>NS_NONE                  </td><td>Disable NS. </td></tr>
        ' <tr><td>NS_DEFAULT               </td><td>Platform default. </td></tr>
        ' <tr><td>NS_Conference            </td><td>Conferencing default. </td></tr>
        ' <tr><td>NS_LOW_SUPPRESSION       </td><td>Lowest suppression. </td></tr>
        ' <tr><td>NS_MODERATE_SUPPRESSION  </td><td>Moderate suppression. </td></tr>
        ' <tr><td>NS_HIGH_SUPPRESSION      </td><td>High suppression </td></tr>
        ' <tr><td>NS_VERY_HIGH_SUPPRESSION </td><td>Highest suppression. </td></tr>
        ' </table></p>
        '         

        Public Sub enableANS(nsMode As NS_MODES)
            If _LibSDK = IntPtr.Zero Then
                Return
            End If

            PortSIP_NativeMethods.PortSIP_enableANS(_LibSDK, DirectCast(nsMode, Int32))
        End Sub



        '!
        '    @brief Set the DSCP (differentiated services code point) value of QoS (Quality of Service) for audio channel.
        '    
        '    @param state        Set to true to enable audio QoS.
        '    @param DSCPValue    The six-bit DSCP value. Valid value ranges 0-63. As defined in RFC 2472, the DSCP value is the high-order 6 bits of the IP version 4 (IPv4) TOS field and the IP version 6 (IPv6) Traffic Class field.
        '    @param priority     The 802.1p priority (PCP) field in a 802.1Q/VLAN tag. Values 0-7 indicate the priority, while value -1 leaves the priority setting unchanged.
        '    
        '    @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function enableAudioQos(state As [Boolean]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_enableAudioQos(_LibSDK, state)
        End Function

        '!
        '  @brief Set the DSCP (differentiated services code point) value of QoS (Quality of Service) for video channel.
        '
        '  @param state   Set To True To enable video QoS And DSCP value will be 34; Or False To disable video QoS, And DSCP value will be 0.
        ' 
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function enableVideoQos(state As [Boolean]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_enableVideoQos(_LibSDK, state)
        End Function

        '!
        '  @brief Set the MTU size for video RTP packet.
        '
        '  @param mtu    Set MTU value. Allow value ranges (512-65507). Other value will be modified to the default 1450.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function setVideoMTU(mtu As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_setVideoMTU(_LibSDK, mtu)
        End Function



        '* @} 

        ' end of group16

        '* @defgroup group17 Send OPTIONS/INFO/MESSAGE functions
        ' @{
        '         


        '!
        '  @brief Send OPTIONS message.
        '
        '  @param to  The recipient of OPTIONS message.
        '  @param sdp The SDP of OPTIONS message. It's optional if user does not wish to send the SDP with OPTIONS message.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function sendOptions([to] As [String], sdp As [String]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_sendOptions(_LibSDK, [to], sdp)
        End Function

        '!
        '  @brief Send a INFO message to remote side in a call.
        '
        '  @param sessionId    The session ID of call.
        '  @param mimeType     The mime type of INFO message.
        '  @param subMimeType  The sub mime type of INFO message.
        '  @param infoContents The contents that is sent with INFO message.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function sendInfo(sessionId As Int32, mimeType As [String], subMimeType As [String], infoContents As [String]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_sendInfo(_LibSDK, sessionId, mimeType, subMimeType, infoContents)
        End Function


        '!
        '   @brief Send a SUBSCRIBE message to subscribe an event.
        ' 
        '   @param to    The user/extension to be subscribed.
        '   @param eventName    The event name to be subscribed.
        ' 
        '   @return If the function succeeds, it will return the ID of SUBSCRIBE which is greater than 0. If the function fails, it will return a specific error code which is less than 0.
        '   @remark
        '    Example 1, below code indicates that user/extension 101 is subscribed to MWI (Message Waiting notifications) for checking his voicemail: 
        '    int32 mwiSubId = sendSubscription("sip:101@test.com", "message-summary");
        '    
        '    Example 2, to monitor a user/extension call status, 
        '    You can use code: sendSubscription(mSipLib, "100", "dialog");
        '    Extension 100 refers to the user/extension to be monitored. Once being monitored, when extension 100 hold a call or is ringing, the
        '     onDialogStateUpdated callback will be triggered.
        '          

        Public Function sendSubscription([to] As [String], eventName As [String]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_sendSubscription(_LibSDK, [to], eventName)
        End Function


        '!
        '  @brief Terminate the given subscription.
        '
        '  @param subscribeId    The ID of the subscription.
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '  @remark
        '  For example, if you want stop check the MWI, use below code:
        '           @code 
        '           terminateSubscription(mwiSubId);
        '           @endcode 
        '         

        Public Function terminateSubscription(subscribeId As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_terminateSubscription(_LibSDK, subscribeId)
        End Function

        '!
        '  @brief Send a MESSAGE message to remote side in dialog.
        '
        '  @param sessionId     The session ID of the call.
        '  @param mimeType      The mime type of MESSAGE message.
        '  @param subMimeType   The sub mime type of MESSAGE message.
        '  @param message       The contents which is sent with MESSAGE message. Binary data allowed.
        '  @param messageLength The message size.
        '
        '  @return If the function succeeds, it will return a message ID that allows to track the message sending state in onSendMessageSuccess and onSendMessageFailure. If the function fails, it will return a specific error code less than 0.
        '  @remark  Example 1: send a plain text message. Note: to send text in other languages, please use the UTF-8 to encode the message before sending.
        '         @code
        '         sendMessage(sessionId, "text", "plain", "hello",6);
        '         @endcode
        '         Example 2: send a binary message.
        '         @code
        '         sendMessage(sessionId, "application", "vnd.3gpp.sms", binData, binDataSize);
        '         @endcode
        '         

        Public Function sendMessage(sessionId As Int32, mimeType As [String], subMimeType As [String], message As Byte(), messageLength As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_sendMessage(_LibSDK, sessionId, mimeType, subMimeType, message, messageLength)
        End Function

        '!
        '  @brief Send an out of dialog MESSAGE message to remote side.
        '
        '  @param to            The message recipient, such as sip:receiver@portsip.com
        '  @param mimeType      The mime type of MESSAGE message.
        '  @param subMimeType   The sub mime type of MESSAGE message.
        '  @isSMS isSMS         Set to YES to specify "messagetype=SMS" in the To line, or NO to disable.
        '  @param message       The contents which is sent with MESSAGE message. Binary data allowed.
        '  @param messageLength The message size.
        '
        '  @return If the function succeeds, it will return a message ID that allows to track the message sending state in onSendOutOfMessageSuccess and onSendOutOfMessageFailure. If the function fails, it will return a specific error code less than 0.
        '  @remark
        '  Example 1: send a plain text message. Note: to send text in other languages, please use the UTF-8 to encode the message before sending.
        '  @code
        '            sendOutOfDialogMessage("sip:user1@sip.portsip.com", "text", "plain", False, "hello", 6);
        '  @endcode
        '         Example 2: send a binary message.
        '  @code
        '           sendOutOfDialogMessage("sip:user1@sip.portsip.com","application",  "vnd.3gpp.sms", False, binData, binDataSize);
        '         @endcode
        '         

        Public Function sendOutOfDialogMessage([to] As [String], mimeType As [String], subMimeType As [String], isSMS As [Boolean], message As Byte(), messageLength As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_sendOutOfDialogMessage(_LibSDK, [to], mimeType, subMimeType, isSMS, message, messageLength)
        End Function


        '!
        '  @brief Set the default expiration time to be used when creating a subscription.
        '
        '  @param secs    The default expiration time of subscription.
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function setDefaultSubscriptionTime(secs As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_setDefaultSubscriptionTime(_LibSDK, secs)
        End Function

        '!
        '        *  @brief Set the default expiration time to be used when creating a publication.
        '        *  
        '        *  @param secs    The default expiration time of publication.
        '        *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '        

        Public Function setDefaultPublicationTime(secs As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_setDefaultPublicationTime(_LibSDK, secs)
        End Function

        '!
        '        *  @brief Indicate the SDK uses the P2P mode for presence or presence agent mode.
        '        *  
        '        *  @param mode    0 - P2P mode; 1 - Presence Agent mode, default is P2P mode.
        '        *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '        *  @remark
        '        *  Since presence agent mode requires the PBX/Server support the PUBLISH,
        '        *  please ensure you have your and PortSIP PBX support this 
        '        *  feature. For more details please visit: https://www.portsip.com/portsip-pbx
        '        

        Public Function setPresenceMode(mode As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_setPresenceMode(_LibSDK, mode)
        End Function


        '* @} 

        ' end of group17

        '* @defgroup group18 Presence functions
        ' @{
        '         



        '!
        '  @brief Send a SUBSCRIBE message for subscribing the contact's presence status.
        '
        '  @param to The target contact. It must be like sip:contact001@sip.portsip.com.
        '  @param subject This subject text will be inserted into the SUBSCRIBE message. For example: "Hello, I'm Jason".<br>
        '         The subject maybe in UTF-8 format. You should use UTF-8 to decode it.
        '
        '  @return If the function succeeds, it will return value subscribeId. If the function fails, it will return a specific error code.
        '         

        Public Function presenceSubscribe([to] As [String], subject As [String]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_presenceSubscribe(_LibSDK, [to], subject)
        End Function

        '!
        '  @brief Terminate the given presence subscription.
        '
        '  @param subscribeId    The ID of the subscription.
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function presenceTerminateSubscribe(subscribeId As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_presenceTerminateSubscribe(_LibSDK, subscribeId)
        End Function


        '!
        '  @brief Reject a presence SUBSCRIBE request which is received from contact.
        '
        '  @param subscribeId Subscription ID. When receiving a SUBSCRIBE request from contact, the event onPresenceRecvSubscribe will be triggered. The event includes the subscription ID.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '  @remark
        '  If the P2P presence mode is enabled, when someone subscribe your presence status,
        '  you will receive the subscribe request in the callback, and you can use use this function to accept it.
        '         

        Public Function presenceRejectSubscribe(subscribeId As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_presenceRejectSubscribe(_LibSDK, subscribeId)
        End Function

        '!
        '        *  @brief Accept the presence SUBSCRIBE request which is received from contact.
        '        *
        '        *  @param subscribeId Subscription ID. When receiving a SUBSCRIBE request from contact, the event onPresenceRecvSubscribe will be triggered. The event will include the subscription ID.
        '        *
        '        *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '        *  @remark
        '        *  If the P2P presence mode is enabled, when someone subscribes your presence status,
        '        *  you will receive the subscription request in the callback, and you can use use this function to reject it.
        '        

        Public Function presenceAcceptSubscribe(subscribeId As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_presenceAcceptSubscribe(_LibSDK, subscribeId)
        End Function

        '!
        '  @brief Set the presence status.
        '
        '  @param subscribeId Subscription ID. 
        '  @param stateText   The state text of presence online. For example: "I'm here".
        '      If you want to appear as offline to others, please pass the "Offline" to "statusText" parameter.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '  @remark
        '   With P2P presence mode, when receiving a SUBSCRIBE request from contact, the event onPresenceRecvSubscribe will be triggered. 
        '   The event includes the subscription ID. This function will cause the SDK sending a NOTIFY message to
        '   update your presence status, and you must pass the correct subscribeId.
        '   
        '   With presence agent mode, this function will cause the SDK to send a PUBLISH message to 
        '    update your presence status, and you must pass 0 to the "subscribeId" parameter.
        '         

        Public Function setPresenceStatus(subscribeId As Int32, stateText As [String]) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_setPresenceStatus(_LibSDK, subscribeId, stateText)
        End Function

        '* @} 

        ' end of group18

        '* @defgroup group19 Device Manage functions.
        ' @{
        '         


        '!
        '  @brief Gets the count of audio devices available for audio recording.
        '
        '  @return It will return the count of recording devices. If the function fails, it will return a specific error code less than 0.
        '         

        Public Function getNumOfRecordingDevices() As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_getNumOfRecordingDevices(_LibSDK)
        End Function

        '!
        '  @brief Gets the number of audio devices available for audio playout
        '
        '  @return It will return the count of playout devices. If the function fails, it will return a specific error code less than 0.
        '         

        Public Function getNumOfPlayoutDevices() As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_getNumOfPlayoutDevices(_LibSDK)
        End Function

        '!
        '  @brief Gets the name of a specific recording device given by an index.
        '
        '  @param deviceIndex Device index (0, 1, 2, ..., N-1), where N is given by getNumOfRecordingDevices (). Also -1 is a valid value and will return the name of the default recording device.
        '  @param nameUTF8 A character buffer to which the device name will be copied as a null-terminated string in UTF-8 format. 
        '  @param nameUTF8Length The size of nameUTF8 buffer. It cannot be less than 128.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code. 
        '         

        Public Function getRecordingDeviceName(deviceIndex As Int32, nameUTF8 As StringBuilder, nameUTF8Length As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_getRecordingDeviceName(_LibSDK, deviceIndex, nameUTF8, nameUTF8Length)
        End Function

        '!
        '  @brief Get the name of a specific playout device given by an index.
        '
        '  @param deviceIndex 
        '  @param deviceIndex Device index (0, 1, 2, ..., N-1), where N is given by getNumOfRecordingDevices (). Also -1 is a valid value and will return the name of the default recording device.
        '  @param nameUTF8 A character buffer to which the device name will be copied as a null-terminated string in UTF-8 format. 
        '  @param nameUTF8Length The size of nameUTF8 buffer. It cannot be less than 128.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code. 
        '         

        Public Function getPlayoutDeviceName(deviceIndex As Int32, nameUTF8 As StringBuilder, nameUTF8Length As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_getPlayoutDeviceName(_LibSDK, deviceIndex, nameUTF8, nameUTF8Length)
        End Function

        '!
        '  @brief Set the speaker volume level,
        '
        '  @param volume Volume level of speaker. Valid value ranges 0 - 255.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function setSpeakerVolume(volume As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_setSpeakerVolume(_LibSDK, volume)
        End Function

        '!
        '  @brief Gets the speaker volume level.
        '
        '  @return If the function succeeds, it will return the speaker volume with valid range 0 - 255. If the function fails, it will return a specific error code.
        '         

        Public Function getSpeakerVolume() As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_getSpeakerVolume(_LibSDK)
        End Function


        '!
        '  @brief Sets the microphone volume level.
        '
        '  @param volume The microphone volume level, the valid value is 0 - 255.
        '
        '  @return If the function succeeds, the return value is 0. If the function fails, the return value is a specific error code.
        '         

        Public Function setMicVolume(volume As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_setMicVolume(_LibSDK, volume)
        End Function

        '!
        '  @brief Retrieves the current microphone volume.
        '
        '  @return If the function succeeds, it will return the microphone volume. If the function fails, it will return a specific error code.
        '         

        Public Function getMicVolume() As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_getMicVolume(_LibSDK)
        End Function

        '!
        '  @brief Use it for the audio device loop back test
        '
        '  @param enable Set to true to start audio look back test; or fase to stop.
        '         

        Public Sub audioPlayLoopbackTest(enable As [Boolean])
            If _LibSDK = IntPtr.Zero Then
                Return
            End If

            PortSIP_NativeMethods.PortSIP_audioPlayLoopbackTest(_LibSDK, enable)
        End Sub

        '!
        '  @brief Get the number of available capturing devices.
        '
        '  @return It will return the count of video capturing devices. If it fails, it will return a specific error code less than 0.
        '         

        Public Function getNumOfVideoCaptureDevices() As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_getNumOfVideoCaptureDevices(_LibSDK)
        End Function

        '!
        '  @brief Get the name of a specific video capture device given by an index.
        '
        '  @param deviceIndex          Device index (0, 1, 2, ..., N-1), where N is given by getNumOfVideoCaptureDevices (). Also -1 is a valid value and will return the name of the default capturing device.
        '  @param uniqueIdUTF8   Unique identifier of the capturing device.
        '  @param uniqueIdUTF8Length Size in bytes of uniqueIdUTF8. 
        '  @param deviceNameUTF8 A character buffer to which the device name will be copied as a null-terminated string in UTF-8 format.
        '  @param deviceNameUTF8Length The size of nameUTF8 buffer. It cannot be less than 128.
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function getVideoCaptureDeviceName(deviceIndex As Int32, uniqueIdUTF8 As StringBuilder, uniqueIdUTF8Length As Int32, deviceNameUTF8 As StringBuilder, deviceNameUTF8Length As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_getVideoCaptureDeviceName(_LibSDK, deviceIndex, uniqueIdUTF8, uniqueIdUTF8Length, deviceNameUTF8, deviceNameUTF8Length)
        End Function

        '!
        '  @brief Display the capture device property dialog box for the specified capture device.
        '
        '  @param uniqueIdUTF8     Unique identifier of the capture device. 
        '  @param uniqueIdUTF8Length Size in bytes of uniqueIdUTF8. 
        '  @param dialogTitle      The title of the video settings dialog. 
        '  @param parentWindow     Parent window used for the dialog box. It should originally be a HWND. 
        '  @param x                Horizontal position for the dialog relative to the parent window, in pixels. 
        '  @param y                Vertical position for the dialog relative to the parent window, in pixels. 
        '
        '  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        '         

        Public Function showVideoCaptureSettingsDialogBox(uniqueIdUTF8 As [String], uniqueIdUTF8Length As Int32, dialogTitle As [String], parentWindow As IntPtr, x As Int32, y As Int32) As Int32
            If _LibSDK = IntPtr.Zero Then
                Return PortSIP_Errors.ECoreSDKObjectNull
            End If

            Return PortSIP_NativeMethods.PortSIP_showVideoCaptureSettingsDialogBox(_LibSDK, uniqueIdUTF8, uniqueIdUTF8Length, dialogTitle, parentWindow, x,
                y)
        End Function
        '* @} 
        ' end of group19
        '* @} 
        ' end of groupSDK
        '''////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        '''////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ''' <summary>
        ''' Private members and methods
        ''' </summary>


        Private _rgs As PortSIP_NativeMethods.registerSuccess
        Private _rgf As PortSIP_NativeMethods.registerFailure
        Private _ivi As PortSIP_NativeMethods.inviteIncoming
        Private _ivt As PortSIP_NativeMethods.inviteTrying
        Private _ivsp As PortSIP_NativeMethods.inviteSessionProgress
        Private _ivr As PortSIP_NativeMethods.inviteRinging
        Private _iva As PortSIP_NativeMethods.inviteAnswered
        Private _ivf As PortSIP_NativeMethods.inviteFailure
        Private _ivu As PortSIP_NativeMethods.inviteUpdated
        Private _ivc As PortSIP_NativeMethods.inviteConnected
        Private _ivbf As PortSIP_NativeMethods.inviteBeginingForward
        Private _ivcl As PortSIP_NativeMethods.inviteClosed
        Private _dsu As PortSIP_NativeMethods.dialogStateUpdated
        Private _rmh As PortSIP_NativeMethods.remoteHold
        Private _rmuh As PortSIP_NativeMethods.remoteUnHold
        Private _rvr As PortSIP_NativeMethods.receivedRefer
        Private _rfa As PortSIP_NativeMethods.referAccepted
        Private _rfr As PortSIP_NativeMethods.referRejected
        Private _tft As PortSIP_NativeMethods.transferTrying
        Private _tfr As PortSIP_NativeMethods.transferRinging
        Private _ats As PortSIP_NativeMethods.ACTVTransferSuccess
        Private _atf As PortSIP_NativeMethods.ACTVTransferFailure
        Private _rvs As PortSIP_NativeMethods.receivedSignaling
        Private _sds As PortSIP_NativeMethods.sendingSignaling
        Private _wvm As PortSIP_NativeMethods.waitingVoiceMessage
        Private _wfm As PortSIP_NativeMethods.waitingFaxMessage
        Private _rdt As PortSIP_NativeMethods.recvDtmfTone
        Private _prs As PortSIP_NativeMethods.presenceRecvSubscribe
        Private _pon As PortSIP_NativeMethods.presenceOnline
        Private _pof As PortSIP_NativeMethods.presenceOffline
        Private _rvo As PortSIP_NativeMethods.recvOptions
        Private _rvi As PortSIP_NativeMethods.recvInfo
        Private _rns As PortSIP_NativeMethods.recvNotifyOfSubscription
        Private _sbf As PortSIP_NativeMethods.subscriptionFailure
        Private _sbt As PortSIP_NativeMethods.subscriptionTerminated
        Private _rvm As PortSIP_NativeMethods.recvMessage
        Private _rodm As PortSIP_NativeMethods.recvOutOfDialogMessage
        Private _sms As PortSIP_NativeMethods.sendMessageSuccess
        Private _smf As PortSIP_NativeMethods.sendMessageFailure
        Private _sdms As PortSIP_NativeMethods.sendOutOfDialogMessageSuccess
        Private _sdmf As PortSIP_NativeMethods.sendOutOfDialogMessageFailure
        Private _paf As PortSIP_NativeMethods.playAudioFileFinished
        Private _pvf As PortSIP_NativeMethods.playVideoFileFinished
        Private _arc As PortSIP_NativeMethods.audioRawCallback
        Private _vrc As PortSIP_NativeMethods.videoRawCallback
        Private _rrc As PortSIP_NativeMethods.receivedRTPCallback
        Private _src As PortSIP_NativeMethods.sendingRTPCallback


        Private Sub initializeCallbackFunctions()

            _arc = New PortSIP_NativeMethods.audioRawCallback(AddressOf onAudioRawCallback)
            _vrc = New PortSIP_NativeMethods.videoRawCallback(AddressOf onVideoRawCallback)
            _rrc = New PortSIP_NativeMethods.receivedRTPCallback(AddressOf onReceivedRtpPacket)
            _src = New PortSIP_NativeMethods.sendingRTPCallback(AddressOf onSendingRtpPacket)


            _paf = New PortSIP_NativeMethods.playAudioFileFinished(AddressOf onPlayAudioFileFinished)
            _pvf = New PortSIP_NativeMethods.playVideoFileFinished(AddressOf onPlayVideoFileFinished)

            _rgs = New PortSIP_NativeMethods.registerSuccess(AddressOf onRegisterSuccess)
            _rgf = New PortSIP_NativeMethods.registerFailure(AddressOf onRegisterFailure)
            _ivi = New PortSIP_NativeMethods.inviteIncoming(AddressOf onInviteIncoming)
            _ivt = New PortSIP_NativeMethods.inviteTrying(AddressOf onInviteTrying)
            _ivsp = New PortSIP_NativeMethods.inviteSessionProgress(AddressOf onInviteSessionProgress)
            _ivr = New PortSIP_NativeMethods.inviteRinging(AddressOf onInviteRinging)
            _iva = New PortSIP_NativeMethods.inviteAnswered(AddressOf onInviteAnswered)
            _ivf = New PortSIP_NativeMethods.inviteFailure(AddressOf onInviteFailure)
            _ivu = New PortSIP_NativeMethods.inviteUpdated(AddressOf onInviteUpdated)
            _ivc = New PortSIP_NativeMethods.inviteConnected(AddressOf onInviteConnected)
            _ivbf = New PortSIP_NativeMethods.inviteBeginingForward(AddressOf onInviteBeginingForward)
            _ivcl = New PortSIP_NativeMethods.inviteClosed(AddressOf onInviteClosed)
            _dsu = New PortSIP_NativeMethods.dialogStateUpdated(AddressOf onDialogStateUpdated)
            _rmh = New PortSIP_NativeMethods.remoteHold(AddressOf onRemoteHold)
            _rmuh = New PortSIP_NativeMethods.remoteUnHold(AddressOf onRemoteUnHold)
            _rvr = New PortSIP_NativeMethods.receivedRefer(AddressOf onReceivedRefer)
            _rfa = New PortSIP_NativeMethods.referAccepted(AddressOf onReferAccepted)
            _rfr = New PortSIP_NativeMethods.referRejected(AddressOf onReferRejected)
            _tft = New PortSIP_NativeMethods.transferTrying(AddressOf onTransferTrying)
            _tfr = New PortSIP_NativeMethods.transferRinging(AddressOf onTransferRinging)
            _ats = New PortSIP_NativeMethods.ACTVTransferSuccess(AddressOf onACTVTransferSuccess)
            _atf = New PortSIP_NativeMethods.ACTVTransferFailure(AddressOf onACTVTransferFailure)
            _rvs = New PortSIP_NativeMethods.receivedSignaling(AddressOf onReceivedSignaling)
            _sds = New PortSIP_NativeMethods.sendingSignaling(AddressOf onSendingSignaling)
            _wvm = New PortSIP_NativeMethods.waitingVoiceMessage(AddressOf onWaitingVoiceMessage)
            _wfm = New PortSIP_NativeMethods.waitingFaxMessage(AddressOf onWaitingFaxMessage)
            _rdt = New PortSIP_NativeMethods.recvDtmfTone(AddressOf onRecvDtmfTone)
            _prs = New PortSIP_NativeMethods.presenceRecvSubscribe(AddressOf onPresenceRecvSubscribe)
            _pon = New PortSIP_NativeMethods.presenceOnline(AddressOf onPresenceOnline)
            _pof = New PortSIP_NativeMethods.presenceOffline(AddressOf onPresenceOffline)
            _rvo = New PortSIP_NativeMethods.recvOptions(AddressOf onRecvOptions)
            _rvi = New PortSIP_NativeMethods.recvInfo(AddressOf onRecvInfo)

            _rns = New PortSIP_NativeMethods.recvNotifyOfSubscription(AddressOf onRecvNotifyOfSubscription)
            _sbf = New PortSIP_NativeMethods.subscriptionFailure(AddressOf onSubscriptionFailure)
            _sbt = New PortSIP_NativeMethods.subscriptionTerminated(AddressOf onSubscriptionTerminated)

            _rvm = New PortSIP_NativeMethods.recvMessage(AddressOf onRecvMessage)
            _rodm = New PortSIP_NativeMethods.recvOutOfDialogMessage(AddressOf onRecvOutOfDialogMessage)
            _sms = New PortSIP_NativeMethods.sendMessageSuccess(AddressOf onSendMessageSuccess)
            _smf = New PortSIP_NativeMethods.sendMessageFailure(AddressOf onSendMessageFailure)
            _sdms = New PortSIP_NativeMethods.sendOutOfDialogMessageSuccess(AddressOf onSendOutOfDialogMessageSuccess)
            _sdmf = New PortSIP_NativeMethods.sendOutOfDialogMessageFailure(AddressOf onSendOutOfDialogMessageFailure)

        End Sub

        Private Function onRegisterSuccess(callbackIndex As Int32, callbackObject As Int32, statusText As [String], statusCode As Int32, sipMessage As StringBuilder) As Int32
            _SIPCallbackEvents.onRegisterSuccess(callbackIndex, callbackObject, statusText, statusCode, sipMessage)
            Return 0
        End Function

        Private Function onRegisterFailure(callbackIndex As Int32, callbackObject As Int32, statusText As [String], statusCode As Int32, sipMessage As StringBuilder) As Int32
            _SIPCallbackEvents.onRegisterFailure(callbackIndex, callbackObject, statusText, statusCode, sipMessage)
            Return 0
        End Function


        Private Function onInviteIncoming(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, callerDisplayName As [String], caller As [String], calleeDisplayName As [String],
            callee As [String], audioCodecNames As [String], videoCodecNames As [String], existsAudio As [Boolean], existsVideo As [Boolean], sipMessage As StringBuilder) As Int32
            _SIPCallbackEvents.onInviteIncoming(callbackIndex, callbackObject, sessionId, callerDisplayName, caller, calleeDisplayName,
                callee, audioCodecNames, videoCodecNames, existsAudio, existsVideo, sipMessage)
            Return 0
        End Function

        Private Function onInviteTrying(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32) As Int32
            _SIPCallbackEvents.onInviteTrying(_callbackIndex, _callbackObject, sessionId)
            Return 0
        End Function

        Private Function onInviteSessionProgress(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, audioCodecNames As [String], videoCodecNames As [String], existsEarlyMedia As [Boolean],
            existsAudio As [Boolean], existsVideo As [Boolean], sipMessage As StringBuilder) As Int32
            _SIPCallbackEvents.onInviteSessionProgress(_callbackIndex, _callbackObject, sessionId, audioCodecNames, videoCodecNames, existsEarlyMedia,
                existsAudio, existsVideo, sipMessage)

            Return 0
        End Function

        Private Function onInviteRinging(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, statusText As [String], statusCode As Int32, sipMessage As StringBuilder) As Int32
            _SIPCallbackEvents.onInviteRinging(_callbackIndex, _callbackObject, sessionId, statusText, statusCode, sipMessage)

            Return 0
        End Function

        Private Function onInviteAnswered(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, callerDisplayName As [String], caller As [String], calleeDisplayName As [String],
            callee As [String], audioCodecNames As [String], videoCodecNames As [String], existsAudio As [Boolean], existsVideo As [Boolean], sipMessage As StringBuilder) As Int32
            _SIPCallbackEvents.onInviteAnswered(_callbackIndex, _callbackObject, sessionId, callerDisplayName, caller, calleeDisplayName,
                callee, audioCodecNames, videoCodecNames, existsAudio, existsVideo, sipMessage)

            Return 0
        End Function

        Private Function onInviteFailure(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, reason As [String], code As Int32, sipMessage As StringBuilder) As Int32
            _SIPCallbackEvents.onInviteFailure(_callbackIndex, _callbackObject, sessionId, reason, code, sipMessage)
            Return 0
        End Function

        Private Function onInviteUpdated(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, audioCodecNames As [String], videoCodecNames As [String], existsAudio As [Boolean],
            existsVideo As [Boolean], sipMessage As StringBuilder) As Int32
            _SIPCallbackEvents.onInviteUpdated(_callbackIndex, _callbackObject, sessionId, audioCodecNames, videoCodecNames, existsAudio,
                existsVideo, sipMessage)
            Return 0
        End Function

        Private Function onInviteConnected(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32) As Int32
            _SIPCallbackEvents.onInviteConnected(_callbackIndex, _callbackObject, sessionId)

            Return 0

        End Function

        Private Function onInviteBeginingForward(callbackIndex As Int32, callbackObject As Int32, forwardTo As [String]) As Int32
            _SIPCallbackEvents.onInviteBeginingForward(_callbackIndex, _callbackObject, forwardTo)
            Return 0
        End Function

        Private Function onInviteClosed(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32) As Int32
            _SIPCallbackEvents.onInviteClosed(_callbackIndex, _callbackObject, sessionId)
            Return 0
        End Function

        ' 
        ' If a user subscribed and his dialog status monitored, when the monitored user is holding a call
        ' or is being ring, this callback will be triggered.
        '
        ' BLFMonitoredUri - the monitored user's URI
        ' BLFDialogState - the status of the call
        ' BLFDialogId - the ID of the call
        ' BLFDialogDirection - the direction of the call
        '
        Private Function onDialogStateUpdated(callbackIndex As Int32, callbackObject As Int32, BLFMonitoredUri As [String], BLFDialogState As [String], BLFDialogId As [String], BLFDialogDirection As [String]) As Int32
            _SIPCallbackEvents.onDialogStateUpdated(callbackIndex, callbackObject, BLFMonitoredUri, BLFDialogState, BLFDialogId, BLFDialogDirection)
            Return 0
        End Function

        Private Function onRemoteHold(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32) As Int32
            _SIPCallbackEvents.onRemoteHold(_callbackIndex, _callbackObject, sessionId)

            Return 0

        End Function

        Private Function onRemoteUnHold(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, audioCodecNames As [String], videoCodecNames As [String], existsAudio As [Boolean],
            existsVideo As [Boolean]) As Int32
            _SIPCallbackEvents.onRemoteUnHold(_callbackIndex, _callbackObject, sessionId, audioCodecNames, videoCodecNames, existsAudio,
                existsVideo)
            Return 0
        End Function

        Private Function onReceivedRefer(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, referId As Int32, [to] As [String], from As [String],
            referSipMessage As StringBuilder) As Int32
            _SIPCallbackEvents.onReceivedRefer(_callbackIndex, _callbackObject, sessionId, referId, [to], from,
                referSipMessage)
            Return 0
        End Function

        Private Function onReferAccepted(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32) As Int32
            _SIPCallbackEvents.onReferAccepted(_callbackIndex, _callbackObject, sessionId)
            Return 0
        End Function

        Private Function onReferRejected(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, reason As [String], code As Int32) As Int32
            _SIPCallbackEvents.onReferRejected(_callbackIndex, _callbackObject, sessionId, reason, code)
            Return 0
        End Function

        Private Function onTransferTrying(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32) As Int32
            _SIPCallbackEvents.onTransferTrying(_callbackIndex, _callbackObject, sessionId)
            Return 0
        End Function

        Private Function onTransferRinging(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32) As Int32
            _SIPCallbackEvents.onTransferRinging(_callbackIndex, _callbackObject, sessionId)
            Return 0
        End Function

        Private Function onACTVTransferSuccess(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32) As Int32
            _SIPCallbackEvents.onACTVTransferSuccess(_callbackIndex, _callbackObject, sessionId)
            Return 0
        End Function

        Private Function onACTVTransferFailure(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, reason As [String], code As Int32) As Int32
            _SIPCallbackEvents.onACTVTransferFailure(_callbackIndex, _callbackObject, sessionId, reason, code)
            Return 0
        End Function

        Private Function onReceivedSignaling(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, signaling As StringBuilder) As Int32
            _SIPCallbackEvents.onReceivedSignaling(_callbackIndex, _callbackObject, sessionId, signaling)
            Return 0
        End Function

        Private Function onSendingSignaling(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, signaling As StringBuilder) As Int32
            _SIPCallbackEvents.onSendingSignaling(_callbackIndex, _callbackObject, sessionId, signaling)
            Return 0
        End Function

        Private Function onWaitingVoiceMessage(callbackIndex As Int32, callbackObject As Int32, messageAccount As [String], urgentNewMessageCount As Int32, urgentOldMessageCount As Int32, newMessageCount As Int32,
            oldMessageCount As Int32) As Int32
            _SIPCallbackEvents.onWaitingVoiceMessage(_callbackIndex, _callbackObject, messageAccount, urgentNewMessageCount, urgentOldMessageCount, newMessageCount,
                oldMessageCount)
            Return 0
        End Function

        Private Function onWaitingFaxMessage(callbackIndex As Int32, callbackObject As Int32, messageAccount As [String], urgentNewMessageCount As Int32, urgentOldMessageCount As Int32, newMessageCount As Int32,
            oldMessageCount As Int32) As Int32
            _SIPCallbackEvents.onWaitingFaxMessage(_callbackIndex, _callbackObject, messageAccount, urgentNewMessageCount, urgentOldMessageCount, newMessageCount,
                oldMessageCount)
            Return 0
        End Function

        Private Function onRecvDtmfTone(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, tone As Int32) As Int32
            _SIPCallbackEvents.onRecvDtmfTone(_callbackIndex, _callbackObject, sessionId, tone)
            Return 0
        End Function

        Private Function onRecvOptions(callbackIndex As Int32, callbackObject As Int32, optionsMessage As StringBuilder) As Int32
            _SIPCallbackEvents.onRecvOptions(_callbackIndex, _callbackObject, optionsMessage)
            Return 0
        End Function

        Private Function onRecvInfo(callbackIndex As Int32, callbackObject As Int32, infoMessage As StringBuilder) As Int32
            _SIPCallbackEvents.onRecvInfo(_callbackIndex, _callbackObject, infoMessage)
            Return 0
        End Function


        Private Function onRecvNotifyOfSubscription(callbackIndex As Int32, callbackObject As Int32, subscribeId As Int32, notifyMsg As StringBuilder, contentData As Byte(), contentLenght As Int32) As Int32
            _SIPCallbackEvents.onRecvNotifyOfSubscription(_callbackIndex, _callbackObject, subscribeId, notifyMsg, contentData, contentLenght)
            Return 0
        End Function

        Private Function onSubscriptionFailure(callbackIndex As Int32, callbackObject As Int32, subscribeId As Int32, statusCode As Int32) As Int32
            _SIPCallbackEvents.onSubscriptionFailure(_callbackIndex, _callbackObject, subscribeId, statusCode)
            Return 0
        End Function

        Private Function onSubscriptionTerminated(callbackIndex As Int32, callbackObject As Int32, subscribeId As Int32) As Int32
            _SIPCallbackEvents.onSubscriptionTerminated(_callbackIndex, _callbackObject, subscribeId)
            Return 0
        End Function


        '!
        '  This event will be triggered when receiving the SUBSCRIBE request from a contact.
        '
        '  @param subscribeId     The ID of SUBSCRIBE request.
        '  @param fromDisplayName The display name of contact.
        '  @param from            The contact who send the SUBSCRIBE request.
        '  @param subject         The subject of the SUBSCRIBE request.
        '         

        Private Function onPresenceRecvSubscribe(callbackIndex As Int32, callbackObject As Int32, subscribeId As Int32, fromDisplayName As [String], from As [String], subject As [String]) As Int32
            _SIPCallbackEvents.onPresenceRecvSubscribe(_callbackIndex, _callbackObject, subscribeId, fromDisplayName, from, subject)
            Return 0
        End Function

        Private Function onPresenceOnline(callbackIndex As Int32, callbackObject As Int32, fromDisplayName As [String], from As [String], stateText As [String]) As Int32
            _SIPCallbackEvents.onPresenceOnline(_callbackIndex, _callbackObject, fromDisplayName, from, stateText)
            Return 0
        End Function

        Private Function onPresenceOffline(callbackIndex As Int32, callbackObject As Int32, fromDisplayName As [String], from As [String]) As Int32
            _SIPCallbackEvents.onPresenceOffline(_callbackIndex, _callbackObject, fromDisplayName, from)
            Return 0
        End Function

        Private Function onRecvMessage(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, mimeType As [String], subMimeType As [String], messageData As Byte(),
            messageDataLength As Int32) As Int32
            _SIPCallbackEvents.onRecvMessage(_callbackIndex, _callbackObject, sessionId, mimeType, subMimeType, messageData,
                messageDataLength)
            Return 0
        End Function

        Private Function onRecvOutOfDialogMessage(callbackIndex As Int32, callbackObject As Int32, fromDisplayName As [String], from As [String], toDisplayName As [String], [to] As [String],
            mimeType As [String], subMimeType As [String], messageData As Byte(), messageDataLength As Int32) As Int32
            _SIPCallbackEvents.onRecvOutOfDialogMessage(_callbackIndex, _callbackObject, fromDisplayName, from, toDisplayName, [to],
                mimeType, subMimeType, messageData, messageDataLength)
            Return 0
        End Function

        Private Function onSendMessageSuccess(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, messageId As Int32) As Int32
            _SIPCallbackEvents.onSendMessageSuccess(_callbackIndex, _callbackObject, sessionId, messageId)
            Return 0
        End Function

        Private Function onSendMessageFailure(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, messageId As Int32, reason As [String], code As Int32) As Int32
            _SIPCallbackEvents.onSendMessageFailure(_callbackIndex, _callbackObject, sessionId, messageId, reason, code)
            Return 0
        End Function

        Private Function onSendOutOfDialogMessageSuccess(callbackIndex As Int32, callbackObject As Int32, messageId As Int32, fromDisplayName As [String], from As [String], toDisplayName As [String],
            [to] As [String]) As Int32
            _SIPCallbackEvents.onSendOutOfDialogMessageSuccess(_callbackIndex, _callbackObject, messageId, fromDisplayName, from, toDisplayName,
                [to])
            Return 0
        End Function

        Private Function onSendOutOfDialogMessageFailure(callbackIndex As Int32, callbackObject As Int32, messageId As Int32, fromDisplayName As [String], from As [String], toDisplayName As [String],
            [to] As [String], reason As [String], code As Int32) As Int32
            _SIPCallbackEvents.onSendOutOfDialogMessageFailure(_callbackIndex, _callbackObject, messageId, fromDisplayName, from, toDisplayName,
                [to], reason, code)
            Return 0
        End Function

        Private Function onPlayAudioFileFinished(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, fileName As [String]) As Int32
            _SIPCallbackEvents.onPlayAudioFileFinished(_callbackIndex, _callbackObject, sessionId, fileName)
            Return 0
        End Function

        Private Function onPlayVideoFileFinished(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32) As Int32
            _SIPCallbackEvents.onPlayVideoFileFinished(_callbackIndex, _callbackObject, sessionId)

            Return 0
        End Function

        Private Function onReceivedRtpPacket(callbackObject As IntPtr, sessionId As Int32, <MarshalAs(UnmanagedType.I1)> isAudio As [Boolean], <MarshalAs(UnmanagedType.LPArray, SizeParamIndex:=4)> RTPPacket As Byte(), packetSize As Int32) As Int32
            _SIPCallbackEvents.onReceivedRtpPacket(callbackObject, sessionId, isAudio, RTPPacket, packetSize)


            Return 0
        End Function

        Private Function onSendingRtpPacket(callbackObject As IntPtr, sessionId As Int32, <MarshalAs(UnmanagedType.I1)> isAudio As [Boolean], <MarshalAs(UnmanagedType.LPArray, SizeParamIndex:=4)> RTPPacket As Byte(), packetSize As Int32) As Int32
            _SIPCallbackEvents.onSendingRtpPacket(callbackObject, sessionId, isAudio, RTPPacket, packetSize)


            Return 0
        End Function

        Private Function onAudioRawCallback(callbackObject As IntPtr, sessionId As Int32, callbackType As Int32, <MarshalAs(UnmanagedType.LPArray, SizeParamIndex:=4)> data As Byte(), dataLength As Int32, samplingFreqHz As Int32) As Int32
            _SIPCallbackEvents.onAudioRawCallback(callbackObject, sessionId, callbackType, data, dataLength, samplingFreqHz)
            Return 0
        End Function

        Private Function onVideoRawCallback(callbackObject As IntPtr, sessionId As Int32, callbackType As Int32, width As Int32, height As Int32, <MarshalAs(UnmanagedType.LPArray, SizeParamIndex:=6)> data As Byte(),
            dataLength As Int32) As Int32
            Return _SIPCallbackEvents.onVideoRawCallback(callbackObject, sessionId, callbackType, width, height, data,
                dataLength)

        End Function


        '''/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        '''/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        Private _callbackDispatcher As IntPtr
        Private _callbackObject As Int32
        Private _LibSDK As IntPtr
        Private _callbackIndex As Int32
    End Class
End Namespace

