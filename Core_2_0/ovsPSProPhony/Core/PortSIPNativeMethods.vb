
'''///////////////////////////////////////////////////////////////////////
'
' IMPORTANT: DON'T EDIT THIS FILE
'
'''///////////////////////////////////////////////////////////////////////



Imports System.Collections.Generic
Imports System.Text
Imports System.Runtime.InteropServices


Namespace PortSIP
    Class PortSIP_NativeMethods

        ' The callbacks of portsip_sdk.dll

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function registerSuccess(callbackIndex As Int32, callbackObject As Int32, statusText As [String], statusCode As Int32, sipMessage As StringBuilder) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function registerFailure(callbackIndex As Int32, callbackObject As Int32, statusText As [String], statusCode As Int32, sipMessage As StringBuilder) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function inviteIncoming(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, callerDisplayName As [String], caller As [String], calleeDisplayName As [String],
            callee As [String], audioCodecNames As [String], videoCodecNames As [String], <MarshalAs(UnmanagedType.I1)> existsAudio As [Boolean], <MarshalAs(UnmanagedType.I1)> existsVideo As [Boolean], sipMessage As StringBuilder) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function inviteTrying(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function inviteSessionProgress(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, audioCodecNames As [String], videoCodecNames As [String], <MarshalAs(UnmanagedType.I1)> existsEarlyMedia As [Boolean],
            <MarshalAs(UnmanagedType.I1)> existsAudio As [Boolean], <MarshalAs(UnmanagedType.I1)> existsVideo As [Boolean], sipMessage As StringBuilder) As Int32


        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function inviteRinging(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, statusText As [String], statusCode As Int32, sipMessage As StringBuilder) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function inviteAnswered(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, callerDisplayName As [String], caller As [String], calleeDisplayName As [String],
            callee As [String], audioCodecNames As [String], videoCodecNames As [String], <MarshalAs(UnmanagedType.I1)> existsAudio As [Boolean], <MarshalAs(UnmanagedType.I1)> existsVideo As [Boolean], sipMessage As StringBuilder) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function inviteFailure(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, reason As [String], code As Int32, sipMessage As StringBuilder) As Int32


        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function inviteUpdated(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, audioCodecNames As [String], videoCodecNames As [String], <MarshalAs(UnmanagedType.I1)> existsAudio As [Boolean],
            <MarshalAs(UnmanagedType.I1)> existsVideo As [Boolean], sipMessage As StringBuilder) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function inviteConnected(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function inviteBeginingForward(callbackIndex As Int32, callbackObject As Int32, forwardTo As [String]) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function inviteClosed(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function remoteHold(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function remoteUnHold(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, audioCodecNames As [String], videoCodecNames As [String], <MarshalAs(UnmanagedType.I1)> existsAudio As [Boolean],
            <MarshalAs(UnmanagedType.I1)> existsVideo As [Boolean]) As Int32


        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function dialogStateUpdated(callbackIndex As Int32, callbackObject As Int32, BLFMonitoredUri As [String], BLFDialogState As [String], BLFDialogId As [String], BLFDialogDirection As [String]) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function receivedRefer(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, referId As Int32, [to] As [String], from As [String],
            referSipMessage As StringBuilder) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function referAccepted(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function referRejected(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, reason As [String], code As Int32) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function transferTrying(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function transferRinging(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function ACTVTransferSuccess(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function ACTVTransferFailure(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, reason As [String], code As Int32) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function receivedSignaling(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, signaling As StringBuilder) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function sendingSignaling(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, signaling As StringBuilder) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function waitingVoiceMessage(callbackIndex As Int32, callbackObject As Int32, messageAccount As [String], urgentNewMessageCount As Int32, urgentOldMessageCount As Int32, newMessageCount As Int32,
            oldMessageCount As Int32) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function waitingFaxMessage(callbackIndex As Int32, callbackObject As Int32, messageAccount As [String], urgentNewMessageCount As Int32, urgentOldMessageCount As Int32, newMessageCount As Int32,
            oldMessageCount As Int32) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function recvDtmfTone(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, tone As Int32) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function presenceRecvSubscribe(callbackIndex As Int32, callbackObject As Int32, subscribeId As Int32, fromDisplayName As [String], from As [String], subject As [String]) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function presenceOnline(callbackIndex As Int32, callbackObject As Int32, fromDisplayName As [String], from As [String], stateText As [String]) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function presenceOffline(callbackIndex As Int32, callbackObject As Int32, fromDisplayName As [String], from As [String]) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function recvOptions(callbackIndex As Int32, callbackObject As Int32, optionsMessage As StringBuilder) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function recvInfo(callbackIndex As Int32, callbackObject As Int32, infoMessage As StringBuilder) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function recvNotifyOfSubscription(callbackIndex As Int32, callbackObject As Int32, subscribeId As Int32, notifyMessage As StringBuilder, <MarshalAs(UnmanagedType.LPArray, SizeParamIndex:=5)> contentData As Byte(), dataLength As Int32) As Int32
        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function subscriptionFailure(callbackIndex As Int32, callbackObject As Int32, subscribeId As Int32, statusCode As Int32) As Int32
        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function subscriptionTerminated(callbackIndex As Int32, callbackObject As Int32, subscribeId As Int32) As Int32


        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function recvMessage(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, mimeType As [String], subMimeType As [String], <MarshalAs(UnmanagedType.LPArray, SizeParamIndex:=6)> messageData As Byte(),
            messageDataLength As Int32) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function recvOutOfDialogMessage(callbackIndex As Int32, callbackObject As Int32, fromDisplayName As [String], from As [String], toDisplayName As [String], [to] As [String],
            mimeType As [String], subMimeType As [String], <MarshalAs(UnmanagedType.LPArray, SizeParamIndex:=9)> messageData As Byte(), messageDataLength As Int32) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function sendMessageSuccess(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, messageId As Int32) As Int32


        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function sendMessageFailure(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, messageId As Int32, reason As [String], code As Int32) As Int32


        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function sendOutOfDialogMessageSuccess(callbackIndex As Int32, callbackObject As Int32, messageId As Int32, fromDisplayName As [String], from As [String], toDisplayName As [String],
            [to] As [String]) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function sendOutOfDialogMessageFailure(callbackIndex As Int32, callbackObject As Int32, messageId As Int32, fromDisplayName As [String], from As [String], toDisplayName As [String],
            [to] As [String], reason As [String], code As Int32) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function playAudioFileFinished(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, fileName As [String]) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function playVideoFileFinished(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32) As Int32


        '''////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        '''////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ' The delegate methods for callback functions of portsip_sdk.dll
        ' These callback functions allows you to access the raw audio and video data.

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function audioRawCallback(callbackObject As IntPtr, sessionId As Int32, callbackType As Int32, <MarshalAs(UnmanagedType.LPArray, SizeParamIndex:=4)> data As Byte(), dataLength As Int32, samplingFreqHz As Int32) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function videoRawCallback(callbackObject As IntPtr, sessionId As Int32, callbackType As Int32, width As Int32, height As Int32, <[In], Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex:=6)> data As Byte(),
            dataLength As Int32) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function receivedRTPCallback(callbackObject As IntPtr, sessionId As Int32, <MarshalAs(UnmanagedType.I1)> isAudio As [Boolean], <MarshalAs(UnmanagedType.LPArray, SizeParamIndex:=4)> RTPPacket As Byte(), packetSize As Int32) As Int32

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function sendingRTPCallback(callbackObject As IntPtr, sessionId As Int32, <MarshalAs(UnmanagedType.I1)> isAudio As [Boolean], <MarshalAs(UnmanagedType.LPArray, SizeParamIndex:=4)> RTPPacket As Byte(), packetSize As Int32) As Int32


        '''////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        '''////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PSCallback_createCallbackDispatcher() As IntPtr
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_releaseCallbackDispatcher(callbackDispatcher As IntPtr)
        End Sub


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setRegisterSuccessHandler(callbackDispatcher As IntPtr, eventHandler As registerSuccess, callbackIndex As Int32, callbackObject As Int32)
        End Sub

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setRegisterFailureHandler(callbackDispatcher As IntPtr, eventHandler As registerFailure, callbackIndex As Int32, callbackObject As Int32)
        End Sub

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setInviteIncomingHandler(callbackDispatcher As IntPtr, eventHandler As inviteIncoming, callbackIndex As Int32, callbackObject As Int32)
        End Sub

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setInviteTryingHandler(callbackDispatcher As IntPtr, eventHandler As inviteTrying, callbackIndex As Int32, callbackObject As Int32)
        End Sub

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setInviteSessionProgressHandler(callbackDispatcher As IntPtr, eventHandler As inviteSessionProgress, callbackIndex As Int32, callbackObject As Int32)
        End Sub

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setInviteRingingHandler(callbackDispatcher As IntPtr, eventHandler As inviteRinging, callbackIndex As Int32, callbackObject As Int32)
        End Sub


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setInviteAnsweredHandler(callbackDispatcher As IntPtr, eventHandler As inviteAnswered, callbackIndex As Int32, callbackObject As Int32)
        End Sub

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setInviteFailureHandler(callbackDispatcher As IntPtr, eventHandler As inviteFailure, callbackIndex As Int32, callbackObject As Int32)
        End Sub


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setInviteUpdatedHandler(callbackDispatcher As IntPtr, eventHandler As inviteUpdated, callbackIndex As Int32, callbackObject As Int32)
        End Sub

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setInviteConnectedHandler(callbackDispatcher As IntPtr, eventHandler As inviteConnected, callbackIndex As Int32, callbackObject As Int32)
        End Sub


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setInviteBeginingForwardHandler(callbackDispatcher As IntPtr, eventHandler As inviteBeginingForward, callbackIndex As Int32, callbackObject As Int32)
        End Sub

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setInviteClosedHandler(callbackDispatcher As IntPtr, eventHandler As inviteClosed, callbackIndex As Int32, callbackObject As Int32)
        End Sub


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setDialogStateUpdatedHandler(callbackDispatcher As IntPtr, eventHandler As dialogStateUpdated, callbackIndex As Int32, callbackObject As Int32)
        End Sub

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setRemoteHoldHandler(callbackDispatcher As IntPtr, eventHandler As remoteHold, callbackIndex As Int32, callbackObject As Int32)
        End Sub


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setRemoteUnHoldHandler(callbackDispatcher As IntPtr, eventHandler As remoteUnHold, callbackIndex As Int32, callbackObject As Int32)
        End Sub


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setReceivedReferHandler(callbackDispatcher As IntPtr, eventHandler As receivedRefer, callbackIndex As Int32, callbackObject As Int32)
        End Sub

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setReferAcceptedHandler(callbackDispatcher As IntPtr, eventHandler As referAccepted, callbackIndex As Int32, callbackObject As Int32)
        End Sub



        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setReferRejectedHandler(callbackDispatcher As IntPtr, eventHandler As referRejected, callbackIndex As Int32, callbackObject As Int32)
        End Sub


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setTransferTryingHandler(callbackDispatcher As IntPtr, eventHandler As transferTrying, callbackIndex As Int32, callbackObject As Int32)
        End Sub


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setTransferRingingHandler(callbackDispatcher As IntPtr, eventHandler As transferRinging, callbackIndex As Int32, callbackObject As Int32)
        End Sub


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setACTVTransferSuccessHandler(callbackDispatcher As IntPtr, eventHandler As ACTVTransferSuccess, callbackIndex As Int32, callbackObject As Int32)
        End Sub


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setACTVTransferFailureHandler(callbackDispatcher As IntPtr, eventHandler As ACTVTransferFailure, callbackIndex As Int32, callbackObject As Int32)
        End Sub


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setReceivedSignalingHandler(callbackDispatcher As IntPtr, eventHandler As receivedSignaling, callbackIndex As Int32, callbackObject As Int32)
        End Sub


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setSendingSignalingHandler(callbackDispatcher As IntPtr, eventHandler As sendingSignaling, callbackIndex As Int32, callbackObject As Int32)
        End Sub


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setWaitingVoiceMessageHandler(callbackDispatcher As IntPtr, eventHandler As waitingVoiceMessage, callbackIndex As Int32, callbackObject As Int32)
        End Sub

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setWaitingFaxMessageHandler(callbackDispatcher As IntPtr, eventHandler As waitingFaxMessage, callbackIndex As Int32, callbackObject As Int32)
        End Sub



        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setRecvDtmfToneHandler(callbackDispatcher As IntPtr, eventHandler As recvDtmfTone, callbackIndex As Int32, callbackObject As Int32)
        End Sub




        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setPresenceRecvSubscribeHandler(callbackDispatcher As IntPtr, eventHandler As presenceRecvSubscribe, callbackIndex As Int32, callbackObject As Int32)
        End Sub


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setPresenceOnlineHandler(callbackDispatcher As IntPtr, eventHandler As presenceOnline, callbackIndex As Int32, callbackObject As Int32)
        End Sub


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setPresenceOfflineHandler(callbackDispatcher As IntPtr, eventHandler As presenceOffline, callbackIndex As Int32, callbackObject As Int32)
        End Sub


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setRecvOptionsHandler(callbackDispatcher As IntPtr, eventHandler As recvOptions, callbackIndex As Int32, callbackObject As Int32)
        End Sub


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setRecvInfoHandler(callbackDispatcher As IntPtr, eventHandler As recvInfo, callbackIndex As Int32, callbackObject As Int32)
        End Sub



        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setPlayAudioFileFinishedHandler(callbackDispatcher As IntPtr, eventHandler As playAudioFileFinished, callbackIndex As Int32, callbackObject As Int32)
        End Sub

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setPlayVideoFileFinishedHandler(callbackDispatcher As IntPtr, eventHandler As playVideoFileFinished, callbackIndex As Int32, callbackObject As Int32)
        End Sub


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setRecvMessageHandler(callbackDispatcher As IntPtr, eventHandler As recvMessage, callbackIndex As Int32, callbackObject As Int32)
        End Sub



        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setRecvOutOfDialogMessageHandler(callbackDispatcher As IntPtr, eventHandler As recvOutOfDialogMessage, callbackIndex As Int32, callbackObject As Int32)
        End Sub



        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setSendMessageSuccessHandler(callbackDispatcher As IntPtr, eventHandler As sendMessageSuccess, callbackIndex As Int32, callbackObject As Int32)
        End Sub


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setSendMessageFailureHandler(callbackDispatcher As IntPtr, eventHandler As sendMessageFailure, callbackIndex As Int32, callbackObject As Int32)
        End Sub


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setSendOutOfDialogMessageSuccessHandler(callbackDispatcher As IntPtr, eventHandler As sendOutOfDialogMessageSuccess, callbackIndex As Int32, callbackObject As Int32)
        End Sub


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PSCallback_setSendOutOfDialogMessageFailureHandler(callbackDispatcher As IntPtr, eventHandler As sendOutOfDialogMessageFailure, callbackIndex As Int32, callbackObject As Int32)
        End Sub


        '''//////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
        '''//////////////////////////////////////////////////////////////////////////////////////////////////////////////// 

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PortSIP_delCallbackParameters(parameters As IntPtr)
        End Sub


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_initialize(callbackDispatcher As IntPtr, transportType As TRANSPORT_TYPE, localIp As [String], localSIPPort As Int32, logLevel As Int32, logFilePath As [String],
            maxCallSessions As Int32, sipAgentString As [String], audioDeviceLayer As Int32, videoDeviceLayer As Int32, TLSCertificatesRootPath As [String], TLSCipherList As [String],
            verifyTLSCertificate As [Boolean], ByRef errorCode As Int32) As IntPtr
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PortSIP_unInitialize(libSDK As IntPtr)
        End Sub

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_setLicenseKey(libSDK As IntPtr, key As [String]) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_getNICNums() As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_getLocalIpAddress(index As Int32, ip As StringBuilder, ipSize As Int32) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_setUser(libSDK As IntPtr, userName As [String], displayName As [String], authName As [String], password As [String], sipDomain As [String],
            sipServerAddr As [String], sipServerPort As Int32, stunServerAddr As [String], stunServerPort As Int32, outboundServerAddr As [String], outboundServerPort As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PortSIP_removeUser(libSDK As IntPtr)
        End Sub

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_setDisplayName(libSDK As IntPtr, displayName As [String]) As Integer
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_setInstanceId(libSDK As IntPtr, uuid As [String]) As Integer
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_refreshRegistration(libSDK As IntPtr, regExpires As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_registerServer(libSDK As IntPtr, regExpires As Int32, retryTimes As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_unRegisterServer(libSDK As IntPtr) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_getVersion(libSDK As IntPtr, ByRef majorVersion As Int32, ByRef minorVersion As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_enableRport(libSDK As IntPtr, <MarshalAs(UnmanagedType.I1)> enable As [Boolean]) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_enableEarlyMedia(libSDK As IntPtr, <MarshalAs(UnmanagedType.I1)> enable As [Boolean]) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_enableReliableProvisional(libSDK As IntPtr, <MarshalAs(UnmanagedType.I1)> enable As [Boolean]) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_enable3GppTags(libSDK As IntPtr, <MarshalAs(UnmanagedType.I1)> enable As [Boolean]) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PortSIP_enableCallbackSignaling(libSDK As IntPtr, <MarshalAs(UnmanagedType.I1)> enableSending As [Boolean], <MarshalAs(UnmanagedType.I1)> enableReceived As [Boolean])
        End Sub

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_setRtpCallback(libSDK As IntPtr, callbackObj As IntPtr, receivedRTPCallbackHandler As receivedRTPCallback, sendingRTPCallbackHandler As sendingRTPCallback) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_addAudioCodec(libSDK As IntPtr, codecType As Int32) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_addVideoCodec(libSDK As IntPtr, codecType As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_isAudioCodecEmpty(libSDK As IntPtr) As <MarshalAs(UnmanagedType.I1)> [Boolean]
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_isVideoCodecEmpty(libSDK As IntPtr) As <MarshalAs(UnmanagedType.I1)> [Boolean]
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_setAudioCodecPayloadType(libSDK As IntPtr, codecType As Int32, payloadType As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_setVideoCodecPayloadType(libSDK As IntPtr, codecType As Int32, payloadType As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PortSIP_clearAudioCodec(libSDK As IntPtr)
        End Sub

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PortSIP_clearVideoCodec(libSDK As IntPtr)
        End Sub

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_setAudioCodecParameter(libSDK As IntPtr, codecType As Int32, parameter As [String]) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_setVideoCodecParameter(libSDK As IntPtr, codecType As Int32, parameter As [String]) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_setSrtpPolicy(libSDK As IntPtr, srtpPolicy As Int32, <MarshalAs(UnmanagedType.I1)> allowSrtpOverUnsecureTransport As [Boolean]) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_setRtpPortRange(libSDK As IntPtr, minimumRtpAudioPort As Int32, maximumRtpAudioPort As Int32, minimumRtpVideoPort As Int32, maximumRtpVideoPort As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_setRtcpPortRange(libSDK As IntPtr, minimumRtcpAudioPort As Int32, maximumRtcpAudioPort As Int32, minimumRtcpVideoPort As Int32, maximumRtcpVideoPort As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_enableCallForward(libSDK As IntPtr, <MarshalAs(UnmanagedType.I1)> forBusyOnly As [Boolean], forwardTo As [String]) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_disableCallForward(libSDK As IntPtr) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_enableSessionTimer(libSDK As IntPtr, timerSeconds As Int32, refreshMode As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_disableSessionTimer(libSDK As IntPtr) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PortSIP_setDoNotDisturb(libSDK As IntPtr, <MarshalAs(UnmanagedType.I1)> state As [Boolean])
        End Sub


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_enableAutoCheckMwi(libSDK As IntPtr, <MarshalAs(UnmanagedType.I1)> state As [Boolean]) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_setRtpKeepAlive(libSDK As IntPtr, <MarshalAs(UnmanagedType.I1)> state As [Boolean], keepAlivePayloadType As Int32, deltaTransmitTimeMS As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_setKeepAliveTime(libSDK As IntPtr, keepAliveTime As Int32) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_getSipMessageHeaderValue(libSDK As IntPtr, sipMessage As [String], headerName As [String], headerValue As StringBuilder, headerValueLength As Int32) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_addSipMessageHeader(libSDK As IntPtr, sessionId As Int32, methodName As [String], msgType As Int32, headerName As [String], headerValue As [String]) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_removeAddedSipMessageHeader(libSDK As IntPtr, sipMessageHeaderId As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_modifySipMessageHeader(libSDK As IntPtr, sessionId As Int32, methodName As [String], msgType As Int32, headerName As [String], headerValue As [String]) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_removeModifiedSipMessageHeader(libSDK As IntPtr, sipMessageHeaderId As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_clearAddedSipMessageHeaders(libSDK As IntPtr) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_clearModifiedSipMessageHeaders(libSDK As IntPtr) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_addSupportedMimeType(libSDK As IntPtr, methodName As [String], mimeType As [String], subMimeType As [String]) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_setAudioSamples(libSDK As IntPtr, ptime As Int32, maxPtime As Int32) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_setAudioDeviceId(libSDK As IntPtr, recordingDeviceId As Int32, playoutDeviceId As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_setVideoDeviceId(libSDK As IntPtr, deviceId As Int32) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_setVideoResolution(libSDK As IntPtr, width As Int32, height As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_setAudioBitrate(libSDK As IntPtr, sessionId As Int32, audioCodecType As Int32, bitrateKbps As Int32) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_setVideoBitrate(libSDK As IntPtr, sessionId As Int32, bitrateKbps As Int32) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_setVideoFrameRate(libSDK As IntPtr, sessionId As Int32, frameRate As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_sendVideo(libSDK As IntPtr, sessionId As Int32, <MarshalAs(UnmanagedType.I1)> sendState As [Boolean]) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PortSIP_muteMicrophone(libSDK As IntPtr, <MarshalAs(UnmanagedType.I1)> mute As [Boolean])
        End Sub

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PortSIP_muteSpeaker(libSDK As IntPtr, <MarshalAs(UnmanagedType.I1)> mute As [Boolean])
        End Sub

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PortSIP_setChannelOutputVolumeScaling(libSDK As IntPtr, sessionId As Int32, scaling As Int32)
        End Sub

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PortSIP_setChannelInputVolumeScaling(libSDK As IntPtr, sessionId As Int32, scaling As Int32)
        End Sub


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PortSIP_setLocalVideoWindow(libSDK As IntPtr, localVideoWindow As IntPtr)
        End Sub


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_setRemoteVideoWindow(libSDK As IntPtr, sessionId As Int32, remoteVideoWindow As IntPtr) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_displayLocalVideo(libSDK As IntPtr, <MarshalAs(UnmanagedType.I1)> state As [Boolean]) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_setVideoNackStatus(libSDK As IntPtr, <MarshalAs(UnmanagedType.I1)> state As [Boolean]) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_call(libSDK As IntPtr, callTo As [String], <MarshalAs(UnmanagedType.I1)> sendSdp As [Boolean], <MarshalAs(UnmanagedType.I1)> videoCall As [Boolean]) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_rejectCall(libSDK As IntPtr, sessionId As Int32, code As Integer) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_hangUp(libSDK As IntPtr, sessionId As Int32) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_answerCall(libSDK As IntPtr, sessionId As Int32, <MarshalAs(UnmanagedType.I1)> videoCall As [Boolean]) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_updateCall(libSDK As IntPtr, sessionId As Int32, <MarshalAs(UnmanagedType.I1)> enableAudio As [Boolean], <MarshalAs(UnmanagedType.I1)> enableVideo As [Boolean]) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_hold(libSDK As IntPtr, sessionId As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_unHold(libSDK As IntPtr, sessionId As Int32) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_refer(libSDK As IntPtr, sessionId As Int32, referTo As [String]) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_attendedRefer(libSDK As IntPtr, sessionId As Int32, replaceSessionId As Int32, referTo As [String]) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_attendedRefer2(libSDK As IntPtr, sessionId As Int32, replaceSessionId As Int32, replaceMethod As [String], target As [String], referTo As [String]) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_outOfDialogRefer(libSDK As IntPtr, replaceSessionId As Int32, replaceMethod As [String], target As [String], referTo As [String]) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_acceptRefer(libSDK As IntPtr, referId As Int32, referSignaling As [String]) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_rejectRefer(libSDK As IntPtr, referId As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_muteSession(libSDK As IntPtr, sessionId As Int32, <MarshalAs(UnmanagedType.I1)> muteIncomingAudio As [Boolean], <MarshalAs(UnmanagedType.I1)> muteOutgoingAudio As [Boolean], <MarshalAs(UnmanagedType.I1)> muteIncomingVideo As [Boolean], <MarshalAs(UnmanagedType.I1)> muteOutgoingVideo As [Boolean]) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_forwardCall(libSDK As IntPtr, sessionId As Int32, forwardTo As [String]) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_pickupBLFCall(libSDK As IntPtr, replaceDialogId As [String], <MarshalAs(UnmanagedType.I1)> videoCall As [Boolean]) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_sendDtmf(libSDK As IntPtr, sessionId As Int32, dtmfMethod As Int32, code As Int32, dtmfDuration As Int32, <MarshalAs(UnmanagedType.I1)> playDtmfTone As [Boolean]) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_enableSendPcmStreamToRemote(libSDK As IntPtr, sessionId As Int32, <MarshalAs(UnmanagedType.I1)> state As [Boolean], streamSamplesPerSec As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_sendPcmStreamToRemote(libSDK As IntPtr, sessionId As Int32, <MarshalAs(UnmanagedType.LPArray)> data As Byte(), dataLength As Int32) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_enableSendVideoStreamToRemote(libSDK As IntPtr, sessionId As Int32, <MarshalAs(UnmanagedType.I1)> state As [Boolean]) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_sendVideoStreamToRemote(libSDK As IntPtr, sessionId As Int32, <MarshalAs(UnmanagedType.LPArray)> data As Byte(), dataLength As Int32, width As Int32, height As Int32) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_enableAudioStreamCallback(libSDK As IntPtr, sessionId As Int32, <MarshalAs(UnmanagedType.I1)> enable As [Boolean], callbackMode As Int32, callbackObject As IntPtr, callbackHandler As audioRawCallback) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_enableVideoStreamCallback(libSDK As IntPtr, sessionId As Int32, callbackMode As Int32, callbackObject As IntPtr, callbackHandler As videoRawCallback) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_startRecord(libSDK As IntPtr, sessionId As Int32, recordFilePath As [String], recordFileName As [String], <MarshalAs(UnmanagedType.I1)> appendTimeStamp As [Boolean], audioFileFormat As Int32,
            audioRecordMode As Int32, videoFileCodecType As Int32, videoRecordMode As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_stopRecord(libSDK As IntPtr, sessionId As Int32) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_playVideoFileToRemote(libSDK As IntPtr, sessionId As Int32, aviFile As [String], <MarshalAs(UnmanagedType.I1)> [loop] As [Boolean], <MarshalAs(UnmanagedType.I1)> playAudio As [Boolean]) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_stopPlayVideoFileToRemote(libSDK As IntPtr, sessionId As Int32) As Int32
        End Function



        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_playAudioFileToRemote(libSDK As IntPtr, sessionId As Int32, fileName As [String], fileSamplesPerSec As Int32, <MarshalAs(UnmanagedType.I1)> [loop] As [Boolean]) As Int32
        End Function



        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_stopPlayAudioFileToRemote(libSDK As IntPtr, sessionId As Int32) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_playAudioFileToRemoteAsBackground(libSDK As IntPtr, sessionId As Int32, fileName As [String], fileSamplesPerSec As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_stopPlayAudioFileToRemoteAsBackground(libSDK As IntPtr, sessionId As Int32) As Int32
        End Function



        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_createAudioConference(libSDK As IntPtr) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_createVideoConference(libSDK As IntPtr, conferenceVideoWindow As IntPtr, width As Int32, height As Int32, <MarshalAs(UnmanagedType.I1)> displayLocalVideoInConference As [Boolean]) As Int32
        End Function



        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PortSIP_destroyConference(libSDK As IntPtr)
        End Sub


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_joinToConference(libSDK As IntPtr, sessionId As Int32) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_setConferenceVideoWindow(libSDK As IntPtr, videoWindow As IntPtr) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_removeFromConference(libSDK As IntPtr, sessionId As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_setAudioRtcpBandwidth(libSDK As IntPtr, sessionId As Int32, BitsRR As Int32, BitsRS As Int32, KBitsAS As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_setVideoRtcpBandwidth(libSDK As IntPtr, sessionId As Int32, BitsRR As Int32, BitsRS As Int32, KBitsAS As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_getAudioStatistics(libSDK As IntPtr, sessionId As Int32,
            ByRef sendBytes As Int32, ByRef sendPackets As Int32, ByRef sendPacketsLost As Int32, ByRef sendFractionLost As Int32, ByRef sendRttMS As Int32, ByRef sendCodecType As Int32, ByRef sendJitterMS As Int32, ByRef sendAudioLevel As Int32,
            ByRef recvBytes As Int32, ByRef recvPackets As Int32, ByRef recvPacketsLost As Int32, ByRef recvFractionLost As Int32, ByRef recvCodecType As Int32, ByRef recvJitterMS As Int32, ByRef recvAudioLevel As Int32) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_getVideoStatistics(libSDK As IntPtr, sessionId As Int32, ByRef sendBytes As Int32, ByRef sendPackets As Int32, ByRef sendPacketsLost As Int32, ByRef sendFractionLost As Int32,
                                                             ByRef sendRttMS As Int32, ByRef sendCodecType As Int32, ByRef sendFrameWidth As Int32, ByRef sendFrameHeight As Int32, ByRef sendBitrateBPS As Int32, ByRef sendFramerate As Int32,
                                                             ByRef recvBytes As Int32, ByRef recvPackets As Int32, ByRef recvPacketsLost As Int32, ByRef recvFractionLost As Int32, ByRef recvCodecType As Int32,
                                                             ByRef recvFrameWidth As Int32, ByRef recvFrameHeight As Int32, ByRef recvBitrateBPS As Int32, ByRef recvFramerate As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PortSIP_enableAEC(libSDK As IntPtr, ecMode As Int32)
        End Sub

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PortSIP_enableVAD(libSDK As IntPtr, <MarshalAs(UnmanagedType.I1)> state As [Boolean])
        End Sub

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PortSIP_enableCNG(libSDK As IntPtr, <MarshalAs(UnmanagedType.I1)> state As [Boolean])
        End Sub

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PortSIP_enableAGC(libSDK As IntPtr, agcMode As Int32)
        End Sub


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PortSIP_enableANS(libSDK As IntPtr, nsMode As Int32)
        End Sub

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_enableAudioQos(libSDK As IntPtr, <MarshalAs(UnmanagedType.I1)> state As [Boolean]) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_enableVideoQos(libSDK As IntPtr, <MarshalAs(UnmanagedType.I1)> state As [Boolean]) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_setVideoMTU(libSDK As IntPtr, mtu As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_sendOptions(libSDK As IntPtr, [to] As [String], sdp As [String]) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_sendInfo(libSDK As IntPtr, sessionId As Int32, mimeType As [String], subMimeType As [String], infoContents As [String]) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_sendSubscription(libSDK As IntPtr, [to] As [String], eventName As [String]) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_terminateSubscription(libSDK As IntPtr, subscritionId As Int32) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_sendMessage(libSDK As IntPtr, sessionId As Int32, mimeType As [String], subMimeType As [String], <MarshalAs(UnmanagedType.LPArray)> message As Byte(), messageLength As Int32) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_sendOutOfDialogMessage(libSDK As IntPtr, [to] As [String], mimeType As [String], subMimeType As [String], <MarshalAs(UnmanagedType.I1)> isSMS As [Boolean], <MarshalAs(UnmanagedType.LPArray)> message As Byte(), messageLength As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_setDefaultSubscriptionTime(libSDK As IntPtr, secs As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_setDefaultPublicationTime(libSDK As IntPtr, secs As Int32) As Int32
        End Function



        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_setPresenceMode(libSDK As IntPtr, mode As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_presenceSubscribe(libSDK As IntPtr, [to] As [String], subject As [String]) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_presenceTerminateSubscribe(libSDK As IntPtr, subscribeId As Int32) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_presenceRejectSubscribe(libSDK As IntPtr, subscribeId As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_presenceAcceptSubscribe(libSDK As IntPtr, subscribeId As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_setPresenceStatus(libSDK As IntPtr, subscribeId As Int32, stateText As [String]) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_getNumOfRecordingDevices(libSDK As IntPtr) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_getNumOfPlayoutDevices(libSDK As IntPtr) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_getRecordingDeviceName(libSDK As IntPtr, deviceIndex As Int32, nameUTF8 As StringBuilder, nameUTF8Length As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_getPlayoutDeviceName(libSDK As IntPtr, deviceIndex As Int32, nameUTF8 As StringBuilder, nameUTF8Length As Int32) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_setSpeakerVolume(libSDK As IntPtr, volume As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_getSpeakerVolume(libSDK As IntPtr) As Int32
        End Function



        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_setMicVolume(libSDK As IntPtr, volume As Int32) As Int32
        End Function

        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_getMicVolume(libSDK As IntPtr) As Int32
        End Function


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Sub PortSIP_audioPlayLoopbackTest(libSDK As IntPtr, <MarshalAs(UnmanagedType.I1)> enable As [Boolean])
        End Sub


        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_getNumOfVideoCaptureDevices(libSDK As IntPtr) As Int32
        End Function



        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_getVideoCaptureDeviceName(libSDK As IntPtr, deviceIndex As Int32, uniqueIdUTF8 As StringBuilder, uniqueIdUTF8Length As Int32, deviceNameUTF8 As StringBuilder, deviceNameUTF8Length As Int32) As Int32
        End Function
        <DllImport("nPhony-X.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Shared Function PortSIP_showVideoCaptureSettingsDialogBox(libSDK As IntPtr, uniqueIdUTF8 As [String], uniqueIdUTF8Length As Int32, dialogTitle As [String], parentWindow As IntPtr, x As Int32,
            y As Int32) As Int32
        End Function

    End Class
End Namespace

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik
'Facebook: facebook.com/telerik
'=======================================================
