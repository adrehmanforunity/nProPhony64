'!
' * @author Copyright (c) 2008-2017 PortSIP Solutions,Inc. All rights reserved.
' * @version 15
' * @see https://www.portsip.com
' * @brief PortSIP SDK Callback events.
' 
' PortSIP VoIP SDK Callback events description.
' 



Imports System.Collections.Generic
Imports System.Text



Namespace PortSIP
    Class Session
        Private mSessionId As Integer = 0
        Private mHoldState As Boolean = False
        Private mSessionState As Boolean = False
        Private mRecvCallState As Boolean = False
        Private mOriginCallSessionId As Integer = 0
        Private mIsReferCall As Boolean = False
        Private mHasEarlyMedia As Boolean = False
        Private mDialogMessageId As Integer = 0
        Private mOutOfDialogMessageId As Integer = 0

        Public Function hasEarlyMedia() As Boolean
            Return mHasEarlyMedia
        End Function
        Public Sub setEarlyMeida(earlyMedia As Boolean)
            mHasEarlyMedia = earlyMedia
        End Sub

        Public Function isReferCall() As Boolean
            Return mIsReferCall
        End Function

        Public Function getOriginCallSessionId() As Integer
            Return mOriginCallSessionId
        End Function

        Public Sub setReferCall(referCall As Boolean, originCallSessionId As Integer)
            mIsReferCall = referCall
            mOriginCallSessionId = originCallSessionId
        End Sub

        Public Sub setDialogMessageId(id As Integer)
            mDialogMessageId = id
        End Sub

        Public Function getDialogMessageId() As Integer
            Return mDialogMessageId
        End Function

        Public Sub setOutOfDialogMessageId(id As Integer)
            mOutOfDialogMessageId = id
        End Sub

        Public Function getOutOfDialogMessageId() As Integer
            Return mOutOfDialogMessageId
        End Function



        Public Sub reset()
            mSessionId = 0
            mHoldState = False
            mSessionState = False
            mRecvCallState = False
            mOriginCallSessionId = 0
            mIsReferCall = False
            mDialogMessageId = 0
            mOutOfDialogMessageId = 0
        End Sub


        Public Sub setSessionId(sessionId As Integer)
            mSessionId = sessionId
        End Sub


        Public Function getSessionId() As Integer
            Return mSessionId
        End Function

        Public Sub setHoldState(state As Boolean)
            mHoldState = state
        End Sub


        Public Function getHoldState() As Boolean
            Return mHoldState
        End Function

        Public Sub setSessionState(state As Boolean)
            mSessionState = state
        End Sub


        Public Function getSessionState() As Boolean
            Return mSessionState
        End Function



        Public Sub setRecvCallState(state As Boolean)
            mRecvCallState = state
        End Sub


        Public Function getRecvCallState() As Boolean
            Return mRecvCallState
        End Function

    End Class
End Namespace

