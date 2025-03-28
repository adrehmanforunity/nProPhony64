
'!
' * @author Copyright (c) 2008-2017 PortSIP Solutions,Inc. All rights reserved.
' * @version 15
' * @see https://www.portsip.com
' * @brief PortSIP SDK Callback events.
' 
' PortSIP VoIP SDK Callback events description.
' 


''///////////////////////////////////////////////////////////////////////
'
'  !!!IMPORTANT!!! DON'T EDIT BELOW SOURCE CODE  
'
''///////////////////////////////////////////////////////////////////////  

Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace PortSIP
    '!
    '    *  SIPCallbackEvents PortSIP VoIP SDK Callback events
    '    

    Interface SIPCallbackEvents
        '* @defgroup groupDelegate SDK Callback events
        '         * SDK Callback events
        '         * @{
        '         

        '* @defgroup group21 Register events
        '         * Register events
        '         * @{
        '         


        '!
        '         *  When successfully registered to server, this event will be triggered.
        '         *
        '         *  @param callbackIndex This is a callback index passed in when creating the SDK library.
        '         *  @param callbackObject This is a callback object passed in when creating the SDK library.
        '         *  @param statusText The status text.
        '         *  @param statusCode The status code.
        '         

        Function onRegisterSuccess(callbackIndex As Int32, callbackObject As Int32, statusText As [String], statusCode As Int32, sipMessage As StringBuilder) As Int32

        '!
        '         *  If registration to SIP server fails, this event will be triggered.
        '         *
        '         *  @param statusText The status text.
        '         *  @param statusCode The status code.
        '         

        Function onRegisterFailure(callbackIndex As Int32, callbackObject As Int32, statusText As [String], statusCode As Int32, sipMessage As StringBuilder) As Int32

        '* @} 

        ' end of group21

        '* @defgroup group22 Call events
        '         * @{
        '         


        '!
        '         *  When the call is coming, this event will be triggered.
        '         *
        '         *  @param sessionId         The session ID of the call.
        '         *  @param callerDisplayName The display name of caller
        '         *  @param caller            The caller.
        '         *  @param calleeDisplayName The display name of callee.
        '         *  @param callee            The callee.
        '         *  @param audioCodecNames   The matched audio codecs. It's separated by "#" if there are more than one codecs.
        '         *  @param videoCodecNames   The matched video codecs. It's separated by "#" if there are more than one codecs.
        '         *  @param existsAudio       If it's true, it indicates that this call includes the audio.
        '         *  @param existsVideo       If it's true, it indicates that this call includes the video.
        '         *  @param sipMessage        The SIP message received.         
        '         

        Function onInviteIncoming(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, callerDisplayName As [String], caller As [String], calleeDisplayName As [String],
            callee As [String], audioCodecNames As [String], videoCodecNames As [String], existsAudio As [Boolean], existsVideo As [Boolean], sipMessage As StringBuilder) As Int32

        '!
        '         *  If the outgoing call is being processed, this event will be triggered.
        '         *
        '         *  @param sessionId The session ID of the call.
        '         

        Function onInviteTrying(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32) As Int32

        '!
        '         *  Once the caller received the "183 session progress" message, this event will be triggered.
        '         *
        '         *  @param sessionId        The session ID of the call.
        '         *  @param audioCodecNames  The matched audio codecs. It's separated by "#" if there are more than one codecs.
        '         *  @param videoCodecNames  The matched video codecs. It's separated by "#" if there are more than one codecs.
        '         *  @param existsEarlyMedia If it's true, it indicates that the call has early media.
        '         *  @param existsAudio      If it's true, it indicates that this call includes the audio.
        '         *  @param existsVideo      If it's true, it indicates that this call includes the video.
        '         *  @param sipMessage       The SIP message received.   
        '         

        Function onInviteSessionProgress(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, audioCodecNames As [String], videoCodecNames As [String], existsEarlyMedia As [Boolean],
            existsAudio As [Boolean], existsVideo As [Boolean], sipMessage As StringBuilder) As Int32

        '!
        '         *  If the outgoing call was ringing, this event would be triggered.
        '         *
        '         *  @param sessionId  The session ID of the call.
        '         *  @param statusText The status text.
        '         *  @param statusCode The status code.
        '         *  @param sipMessage The SIP message received. 
        '         

        Function onInviteRinging(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, statusText As [String], statusCode As Int32, sipMessage As StringBuilder) As Int32

        '!
        '         *  If the remote party answered the call, this event would be triggered.
        '         *
        '         *  @param sessionId         The session ID of the call.
        '         *  @param callerDisplayName The display name of caller
        '         *  @param caller            The caller.
        '         *  @param calleeDisplayName The display name of callee.
        '         *  @param callee            The callee.
        '         *  @param audioCodecNames   The matched audio codecs. It's separated by "#" if there are more than one codecs.
        '         *  @param videoCodecNames   The matched video codecs. It's separated by "#" if there are more than one codecs.
        '         *  @param existsAudio       If it's true, it indicates that this call includes the audio.
        '         *  @param existsVideo       If it's true, it indicates that this call includes the video.
        '         *  @param sipMessage           The SIP message received. 
        '         

        Function onInviteAnswered(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, callerDisplayName As [String], caller As [String], calleeDisplayName As [String],
            callee As [String], audioCodecNames As [String], videoCodecNames As [String], existsAudio As [Boolean], existsVideo As [Boolean], sipMessage As StringBuilder) As Int32

        '!
        '         *  If the outgoing call fails, this event will be triggered.
        '         *
        '         *  @param sessionId The session ID of the call.
        '         *  @param reason    The failure reason.
        '         *  @param code      The failure code.
        '         

        Function onInviteFailure(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, reason As [String], code As Int32, sipMessage As StringBuilder) As Int32

        '!
        '         *  This event will be triggered when remote party updates this call.
        '         *
        '         *  @param sessionId   The session ID of the call.
        '         *  @param audioCodecNames The matched audio codecs. It's separated by "#" if there are more than one codecs.
        '         *  @param videoCodecNames The matched video codecs. It's separated by "#" if there are more than one codecs.
        '         *  @param existsAudio If it's true, it indicates that this call includes the audio.
        '         *  @param existsVideo If it's true, it indicates that this call includes the video.
        '         *  @param sipMessage  The SIP message received. 
        '         

        Function onInviteUpdated(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, audioCodecNames As [String], videoCodecNames As [String], existsAudio As [Boolean],
            existsVideo As [Boolean], sipMessage As StringBuilder) As Int32

        '!
        '         *  This event would be triggered when UAC sent/UAS received ACK(the call is connected). Some functions (hold, updateCall etc...) can be called only after the call connected, otherwise these functions will return error.
        '         *
        '         *  @param sessionId The session ID of the call.
        '         

        Function onInviteConnected(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32) As Int32

        '!
        '         *  If the enableCallForward method is called and a call is incoming, the call will be forwarded automatically and this event will be triggered.
        '         *
        '         *  @param forwardTo The forwarding target SIP URI.
        '         

        Function onInviteBeginingForward(callbackIndex As Int32, callbackObject As Int32, forwardTo As [String]) As Int32

        '!
        '         *  This event is triggered once remote side closes the call.
        '         *
        '         *  @param sessionId The session ID of the call.
        '         

        Function onInviteClosed(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32) As Int32

        '!
        '         *  If a user subscribed and his dialog status monitored, when the monitored user is holding a call
        '         *  or being rang, this event will be triggered
        '         *
        '         *  @param BLFMonitoredUri the monitored user's URI
        '         *  @param BLFDialogState - the status of the call
        '         *  @param BLFDialogId - the id of the call
        '         *  @param BLFDialogDirection - the direction of the call
        '         


        Function onDialogStateUpdated(callbackIndex As Int32, callbackObject As Int32, BLFMonitoredUri As [String], BLFDialogState As [String], BLFDialogId As [String], BLFDialogDirection As [String]) As Int32

        '!
        '         *  If the remote side placed the call on hold, this event would be triggered.
        '         *
        '         *  @param sessionId The session ID of the call.
        '         

        Function onRemoteHold(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32) As Int32

        '!
        '         *  If the remote side un-hold the call, this event would be triggered.
        '         *
        '         *  @param sessionId   The session ID of the call.
        '         *  @param audioCodecNames The matched audio codecs. It's separated by "#" if there are more than one codecs.
        '         *  @param videoCodecNames The matched video codecs. It's separated by "#" if there are more than one codecs.
        '         *  @param existsAudio If it's true, it indicates that this call includes the audio.
        '         *  @param existsVideo If it's true, it indicates that this call includes the video.
        '         

        Function onRemoteUnHold(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, audioCodecNames As [String], videoCodecNames As [String], existsAudio As [Boolean],
            existsVideo As [Boolean]) As Int32


        '* @} 

        ' end of group22

        '* @defgroup group23 Refer events
        '         * @{
        '         


        '!
        '         *  This event will be triggered once receiving a REFER message.
        '         *
        '         *  @param sessionId       The session ID of the call.
        '         *  @param referId         The ID of the REFER message. Pass it to acceptRefer or rejectRefer
        '         *  @param to              The refer target.
        '         *  @param from            The sender of REFER message.
        '         *  @param referSipMessage The SIP message of "REFER". Pass it to "acceptRefer" function.
        '         

        Function onReceivedRefer(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, referId As Int32, [to] As [String], from As [String],
            referSipMessage As StringBuilder) As Int32

        '!
        '         *  This callback will be triggered once remote side called "acceptRefer" to accept the REFER.
        '         *
        '         *  @param sessionId The session ID of the call.
        '         

        Function onReferAccepted(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32) As Int32

        '!
        '         *  This callback will be triggered once remote side called "rejectRefer" to reject the REFER.
        '         *
        '         *  @param sessionId The session ID of the call.
        '         *  @param reason    Reject reason.
        '         *  @param code      Reject code.
        '         

        Function onReferRejected(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, reason As [String], code As Int32) As Int32

        '!
        '         *  When the refer call is being processed, this event will be triggered.
        '         *
        '         *  @param sessionId The session ID of the call.
        '         

        Function onTransferTrying(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32) As Int32

        '!
        '         *  When the refer call is ringing, this event will be triggered.
        '         *
        '         *  @param sessionId The session ID of the call.
        '         

        Function onTransferRinging(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32) As Int32

        '!
        '         *  When the refer call succeeds, this event will be triggered. The ACTV means Active.
        '            For example: A established the call with B, and A transfered B to C. When C accepts the refer call, A will receive this event.
        '         *
        '         *  @param sessionId The session ID of the call.
        '         

        Function onACTVTransferSuccess(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32) As Int32

        '!
        '         *  When the refer call fails, this event will be triggered. The ACTV means Active.
        '         For example: A established the call with B, and A transfered B to C. When C rejects the refer call, A will receive this event.
        '         *
        '         *  @param sessionId The session ID of the call.
        '         *  @param reason    The error reason.
        '         *  @param code      The error code.
        '         

        Function onACTVTransferFailure(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, reason As [String], code As Int32) As Int32

        '* @} 

        ' end of group23

        '* @defgroup group24 Signaling events
        '         * @{
        '         

        '!
        '         *  This event will be triggered when receiving an SIP message.
        '         *
        '         *  @param sessionId The session ID of the call.
        '         *  @param signaling The SIP message received.
        '         

        Function onReceivedSignaling(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, signaling As StringBuilder) As Int32

        '!
        '         *  This event will be triggered when a SIP message sent.
        '         *
        '         *  @param sessionId The session ID of the call.
        '         *  @param signaling The SIP message sent.
        '         

        Function onSendingSignaling(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, signaling As StringBuilder) As Int32

        '* @} 

        ' end of group24

        '* @defgroup group25 MWI events
        '         * @{
        '         


        '!
        '         *  If there is voice message (MWI) waiting, this event will be triggered.
        '         *
        '         *  @param messageAccount        Voice message account
        '         *  @param urgentNewMessageCount Urgent new message count.
        '         *  @param urgentOldMessageCount Urgent old message count.
        '         *  @param newMessageCount       New message count.
        '         *  @param oldMessageCount       Old message count.
        '         

        Function onWaitingVoiceMessage(callbackIndex As Int32, callbackObject As Int32, messageAccount As [String], urgentNewMessageCount As Int32, urgentOldMessageCount As Int32, newMessageCount As Int32,
            oldMessageCount As Int32) As Int32

        '!
        '         *  If there is fax message (MWI) waiting, this event will be triggered.
        '         *
        '         *  @param messageAccount        Fax message account
        '         *  @param urgentNewMessageCount Urgent new message count.
        '         *  @param urgentOldMessageCount Urgent old message count.
        '         *  @param newMessageCount       New message count.
        '         *  @param oldMessageCount       Old message count.
        '         

        Function onWaitingFaxMessage(callbackIndex As Int32, callbackObject As Int32, messageAccount As [String], urgentNewMessageCount As Int32, urgentOldMessageCount As Int32, newMessageCount As Int32,
            oldMessageCount As Int32) As Int32

        '* @} 

        ' end of group25

        '* @defgroup group26 DTMF events
        '         * @{
        '         


        '!
        '         *  This event will be triggered when receiving a DTMF tone from remote side.
        '         *
        '         *  @param sessionId The session ID of the call.
        '         *  @param tone      DTMF tone.
        '         * <p><table>
        '         * <tr><th>code</th><th>Description</th></tr>
        '         * <tr><td>0</td><td>The DTMF tone 0.</td></tr><tr><td>1</td><td>The DTMF tone 1.</td></tr><tr><td>2</td><td>The DTMF tone 2.</td></tr>
        '         * <tr><td>3</td><td>The DTMF tone 3.</td></tr><tr><td>4</td><td>The DTMF tone 4.</td></tr><tr><td>5</td><td>The DTMF tone 5.</td></tr>
        '         * <tr><td>6</td><td>The DTMF tone 6.</td></tr><tr><td>7</td><td>The DTMF tone 7.</td></tr><tr><td>8</td><td>The DTMF tone 8.</td></tr>
        '         * <tr><td>9</td><td>The DTMF tone 9.</td></tr><tr><td>10</td><td>The DTMF tone *.</td></tr><tr><td>11</td><td>The DTMF tone #.</td></tr>
        '         * <tr><td>12</td><td>The DTMF tone A.</td></tr><tr><td>13</td><td>The DTMF tone B.</td></tr><tr><td>14</td><td>The DTMF tone C.</td></tr>
        '         * <tr><td>15</td><td>The DTMF tone D.</td></tr><tr><td>16</td><td>The DTMF tone FLASH.</td></tr>
        '         * </table></p>
        '         

        Function onRecvDtmfTone(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, tone As Int32) As Int32

        '* @} 

        ' end of group26

        '* @defgroup group27 INFO/OPTIONS message events
        '         * @{
        '         


        '!
        '         *  This event will be triggered when receiving the OPTIONS message.
        '         *
        '         *  @param optionsMessage The received whole OPTIONS message in text format.
        '         

        Function onRecvOptions(callbackIndex As Int32, callbackObject As Int32, optionsMessage As StringBuilder) As Int32

        '!
        '         *  This event will be triggered when receiving the INFO message.
        '         *
        '         *  @param infoMessage The received whole INFO message in text format.
        '         

        Function onRecvInfo(callbackIndex As Int32, callbackObject As Int32, infoMessage As StringBuilder) As Int32


        Function onRecvNotifyOfSubscription(callbackIndex As Int32, callbackObject As Int32, subscribeId As Int32, notifyMsg As StringBuilder, contentData As Byte(), contentLenght As Int32) As Int32

        Function onSubscriptionFailure(callbackIndex As Int32, callbackObject As Int32, subscribeId As Int32, statusCode As Int32) As Int32

        Function onSubscriptionTerminated(callbackIndex As Int32, callbackObject As Int32, subscribeId As Int32) As Int32


        '* @} 

        ' end of group27

        '* @defgroup group28 Presence events
        '         * @{
        '         

        '!
        '         *  This event will be triggered when receiving the SUBSCRIBE request from a contact.
        '         *
        '         *  @param subscribeId     The ID of SUBSCRIBE request.
        '         *  @param fromDisplayName The display name of contact.
        '         *  @param from            The contact who sends the SUBSCRIBE request.
        '         *  @param subject         The subject of the SUBSCRIBE request.
        '         

        Function onPresenceRecvSubscribe(callbackIndex As Int32, callbackObject As Int32, subscribeId As Int32, fromDisplayName As [String], from As [String], subject As [String]) As Int32

        '!
        '         *  When the contact is online or changes presence status, this event will be triggered.
        '         *
        '         *  @param fromDisplayName The display name of contact.
        '         *  @param from            The contact who sends the SUBSCRIBE request.
        '         *  @param stateText       The presence status text.
        '         

        Function onPresenceOnline(callbackIndex As Int32, callbackObject As Int32, fromDisplayName As [String], from As [String], stateText As [String]) As Int32

        '!
        '         *  When the contact goes offline, this event will be triggered.
        '         *
        '         *  @param fromDisplayName The display name of contact.
        '         *  @param from            The contact who sends the SUBSCRIBE request
        '         

        Function onPresenceOffline(callbackIndex As Int32, callbackObject As Int32, fromDisplayName As [String], from As [String]) As Int32

        '!
        '         *  This event will be triggered when receiving a MESSAGE message in dialog.
        '         *
        '         *  @param sessionId         The session ID of the call.
        '         *  @param mimeType          The message mime type.
        '         *  @param subMimeType       The message sub mime type.
        '         *  @param messageData       The received message body. It can be text or binary data.
        '         *  @param messageDataLength The length of "messageData".
        '         

        Function onRecvMessage(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, mimeType As [String], subMimeType As [String], messageData As Byte(),
            messageDataLength As Int32) As Int32

        '!
        '         *  This event will be triggered when receiving a MESSAGE message out of dialog. For example: pager message.
        '         *
        '         *  @param fromDisplayName   The display name of sender.
        '         *  @param from              The message sender.
        '         *  @param toDisplayName     The display name of recipient.
        '         *  @param to                The recipient.
        '         *  @param mimeType          The message mime type.
        '         *  @param subMimeType       The message sub mime type.
        '         *  @param messageData       The received message body. It can be text or binary data.
        '         *  @param messageDataLength The length of "messageData".
        '         

        Function onRecvOutOfDialogMessage(callbackIndex As Int32, callbackObject As Int32, fromDisplayName As [String], from As [String], toDisplayName As [String], [to] As [String],
            mimeType As [String], subMimeType As [String], messageData As Byte(), messageDataLength As Int32) As Int32

        '!
        '         *  If the message is sent successfully in dialog, this event will be triggered.
        '         *
        '         *  @param sessionId The session ID of the call.
        '         *  @param messageId The message ID. It's equal to the return value of sendMessage function.
        '         

        Function onSendMessageSuccess(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, messageId As Int32) As Int32

        '!
        '         *  If the message fails to be sent out of dialog, this event will be triggered.
        '         *
        '         *  @param sessionId The session ID of the call.
        '         *  @param messageId The message ID. It's equal to the return value of sendMessage function.
        '         *  @param reason    The failure reason.
        '         *  @param code      Failure code.
        '         

        Function onSendMessageFailure(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, messageId As Int32, reason As [String], code As Int32) As Int32



        '!
        '         *  If the message was sent succeeded out of dialog, this event will be triggered.
        '         *
        '         *  @param messageId       The message ID. It's equal to the return value of SendOutOfDialogMessage function.
        '         *  @param fromDisplayName The display name of message sender.
        '         *  @param from            The message sender.
        '         *  @param toDisplayName   The display name of message recipient.
        '         *  @param to              The message receiver.
        '         

        Function onSendOutOfDialogMessageSuccess(callbackIndex As Int32, callbackObject As Int32, messageId As Int32, fromDisplayName As [String], from As [String], toDisplayName As [String],
            [to] As [String]) As Int32

        '!
        '         *  If the message was sent failure out of dialog, this event will be triggered.
        '         *
        '         *  @param messageId       The message ID. It's equal to the return value of SendOutOfDialogMessage function.
        '         *  @param fromDisplayName The display name of message sender
        '         *  @param from            The message sender.
        '         *  @param toDisplayName   The display name of message recipient.
        '         *  @param to              The message recipient.
        '         *  @param reason          The failure reason.
        '         *  @param code            The failure code.
        '         

        Function onSendOutOfDialogMessageFailure(callbackIndex As Int32, callbackObject As Int32, messageId As Int32, fromDisplayName As [String], from As [String], toDisplayName As [String],
            [to] As [String], reason As [String], code As Int32) As Int32

        '* @} 

        ' end of group29

        '* @defgroup group30 Play audio and video file finished events
        '         * @{
        '         


        '!
        '         *  If playAudioFileToRemote function is called with no loop mode, this event will be triggered once the file play finished.
        '         *
        '         *  @param sessionId The session ID of the call.
        '         *  @param fileName  The name of the file played.
        '         

        Function onPlayAudioFileFinished(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32, fileName As [String]) As Int32

        '!
        '         *  If playVideoFileToRemote function is called with no loop mode, this event will be triggered once the file play finished.
        '         *
        '         *  @param sessionId The session ID of the call.
        '         

        Function onPlayVideoFileFinished(callbackIndex As Int32, callbackObject As Int32, sessionId As Int32) As Int32

        '* @} 

        ' end of group30

        '* @defgroup group31 RTP callback events
        '         * @{
        '         

        '!
        '         *  If setRTPCallback function is called to enable the RTP callback, this event will be triggered once receiving a RTP packet.
        '         *
        '         *  @param sessionId  The session ID of the call.
        '         *  @param isAudio    If the received RTP packet is of audio, this parameter returns true, otherwise false.
        '         *  @param RTPPacket  The memory of whole RTP packet.
        '         *  @param packetSize The size of received RTP Packet.
        '          @note Don't call any SDK API functions in this event directly. If you want to call the API functions or other code which is time-consuming, you should post a message to another thread and execute SDK API functions or other code in another thread.
        '         

        Function onReceivedRtpPacket(callbackObject As IntPtr, sessionId As Int32, isAudio As [Boolean], RTPPacket As Byte(), packetSize As Int32) As Int32

        '!
        '         *  If setRTPCallback function is called to enable the RTP callback, this event will be triggered once a RTP packet sent.
        '         *
        '         *  @param sessionId  The session ID of the call.
        '         *  @param isAudio    If the received RTP packet is of audio, this parameter returns true, otherwise false.
        '         *  @param RTPPacket  The memory of whole RTP packet.
        '         *  @param packetSize The size of received RTP Packet.
        '          @note Don't call any SDK API functions in this event directly. If you want to call the API functions or other code which is time-consuming, you should post a message to another thread and execute SDK API functions or other code in another thread.
        '         

        Function onSendingRtpPacket(callbackObject As IntPtr, sessionId As Int32, isAudio As [Boolean], RTPPacket As Byte(), packetSize As Int32) As Int32

        '* @} 

        ' end of group31

        '* @defgroup group32 Audio and video stream callback events
        '        * @{
        '        


        '!
        '         *  This event will be triggered once receiving the audio packets if called enableAudioStreamCallback function.
        '         *
        '         *  @param sessionId         The session ID of the call.
        '         *  @param audioCallbackMode TThe type which is passed in enableAudioStreamCallback function.
        '         *  @param data              The memory of audio stream. It's in PCM format.
        '         *  @param dataLength        The data size.
        '         *  @param samplingFreqHz    The audio stream sample in HZ. For example, it's 8000 or 16000.
        '          @note Don't call any SDK API functions in this event directly. If you want to call the API functions or other code which is time-consuming, you should post a message to another thread and execute SDK API functions or other code in another thread.
        '         

        Function onAudioRawCallback(callbackObject As IntPtr, sessionId As Int32, callbackType As Int32, data As Byte(), dataLength As Int32, samplingFreqHz As Int32) As Int32

        '!
        '         *  This event will be triggered once receiving the video packets if called enableVideoStreamCallback function.
        '         *
        '         *  @param sessionId         The session ID of the call.
        '         *  @param videoCallbackMode The type which is passed in enableVideoStreamCallback function.
        '         *  @param width             The width of video image.
        '         *  @param height            The height of video image.
        '         *  @param data              The memory of video stream. It's in YUV420 format, YV12.
        '         *  @param dataLength        The data size.
        '          @note Don't call any SDK API functions in this event directly. If you want to call the API functions or other code which is time-consuming, you should post a message to another thread and execute SDK API functions or other code in another thread.
        '         

        Function onVideoRawCallback(callbackObject As IntPtr, sessionId As Int32, callbackType As Int32, width As Int32, height As Int32, data As Byte(),
            dataLength As Int32) As Int32
        '* @} 

        ' end of group32
        '* @} 

        ' end of groupDelegate

    End Interface
End Namespace

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik
'Facebook: facebook.com/telerik
'=======================================================
