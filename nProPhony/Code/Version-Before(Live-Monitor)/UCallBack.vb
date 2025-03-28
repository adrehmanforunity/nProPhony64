Imports System.Threading


Public Class Class_UCallBack
    Dim thPool As ThreadPool

#Region "Event_Occured_Handler"
    Public Event Event_Occured As Event_Occured_Handler
    Public Delegate Sub Event_Occured_Handler(ByVal Sender As Object, ByVal e As Event_Occured_Args)
    Public Class Event_Occured_Args
        Inherits EventArgs
        Public TrunkNo As Long = -1
        Public ReminderText As String = ""
        Public msTimeOut As Long = 1
        Public ChannelID As String = ""
        Public BridgeID As String = ""
        Public PlayBackID As String = ""
        Public EventSetupAT As DateTime
        Public EventOccurAT As DateTime
        Public pObject As Object
        Public Sub New(ByVal _TrunkNo As Long,
                       ByVal Optional _ReminderText As String = "",
                       ByVal Optional _msTimeOut As Long = 1,
                       ByVal Optional _ChannelID As String = "",
                       ByVal Optional _BridgeID As String = "",
                       ByVal Optional _PlayBackID As String = "",
                       ByVal Optional _pObject As Object = Nothing)
            TrunkNo = _TrunkNo
            ReminderText = _ReminderText
            msTimeOut = _msTimeOut
            ChannelID = _ChannelID
            BridgeID = _BridgeID
            PlayBackID = _PlayBackID
            EventSetupAT = DateTime.Now
            EventOccurAT = DateTime.Now
            pObject = _pObject
        End Sub
    End Class
#End Region
    Public Function SetupEvent(ByVal _TrunkNo As Long,
                       ByVal Optional _ReminderText As String = "",
                       ByVal Optional _TimeOut As Long = 1,
                       ByVal Optional _ChannelID As String = "",
                       ByVal Optional _BridgeID As String = "",
                       ByVal Optional _PlayBackID As String = "",
                       ByVal Optional _pObject As Object = Nothing) As Long
        SetupEvent = -1
        Try
            Dim w As WaitCallback = New WaitCallback(Sub() ProcessTheWait(_TrunkNo, _ReminderText, _TimeOut, _ChannelID, _BridgeID, _PlayBackID, _pObject))
            ThreadPool.QueueUserWorkItem(w)
        Catch ex As Exception
            Throw New ArgumentException("SetupEvent(" & _TrunkNo & ") " & ex.Message, ex.InnerException)
        End Try
    End Function
    Sub ProcessTheWait(ByVal _TrunkNo As Long,
                       ByVal Optional _ReminderText As String = "",
                       ByVal Optional _msTimeOut As Long = 1,
                       ByVal Optional _ChannelID As String = "",
                       ByVal Optional _BridgeID As String = "",
                       ByVal Optional _PlayBackID As String = "",
                       ByVal Optional _pObject As Object = Nothing)
        Try
            'Dim _TimeNow As DateTime = DateTime.Now
            'If DateDiff(DateInterval.Second, _TimeNow, Now) > _TimeOut Then
            ' Thread.Sleep(200)
            ' End If

            Dim sw As New Stopwatch()
            sw.Start()
            Do While sw.ElapsedMilliseconds < (_msTimeOut)
                ' Allows UI to remain responsive
                Thread.Sleep(200)
                Application.DoEvents()
            Loop
            sw.Stop()
            RaiseEvent Event_Occured(Me, New Event_Occured_Args(_TrunkNo, _ReminderText, _msTimeOut, _ChannelID, _BridgeID, _PlayBackID, _pObject))
        Catch ex As Exception
            Throw New ArgumentException("ProcessTheWait(" & _TrunkNo & ") " & ex.Message, ex.InnerException)
        End Try
    End Sub

    Protected Overrides Sub Finalize()
        Try
            thPool = Nothing
        Catch ex As Exception
        End Try
        MyBase.Finalize()
    End Sub
End Class
