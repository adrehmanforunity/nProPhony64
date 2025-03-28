Imports System.Net
Imports System.Net.WebSockets
Imports System.Text
Imports System.Threading
Imports System.Collections.Concurrent
Imports System.Timers
Public Class Class_MSWS

#Region "Events"
#Region "WS_Exception_Events"
    Public Delegate Sub Event_Exception_Handler(ByVal Sender As Object, ByVal e As Event_Exception_Args)
    Public Event Event_Exception_Received As Event_Exception_Handler
    Public Class Event_Exception_Args
        Inherits EventArgs
        Public sKey As [String]
        Public sFunction As [String]
        Public sException As [String]
        Public Sub New(__skey As [String], __sFunction As [String], __sException As [String])
            sKey = __skey
            sFunction = __sFunction
            sException = __sException
        End Sub
    End Class
#End Region
#Region "WS_Data_Received_Events"
    Public Delegate Sub Event_Data_Received_Handler(ByVal Sender As Object, ByVal e As Event_Data_Received_Args)
    Public Event Event_Data_Received As Event_Data_Received_Handler
    Public Class Event_Data_Received_Args
        Inherits EventArgs
        Public sKey As [String]
        Public sData As [String]
        Public Sub New(__skey As [String], __sDate As [String])
            sKey = __skey
            sData = __sDate
        End Sub
    End Class
#End Region
#Region "WS_Disconnection_Received_Events"
    Public Delegate Sub Event_Disconnection_Received_Handler(ByVal Sender As Object, ByVal e As Event_Disconnection_Received_Args)
    Public Event Event_Disconnection_Received As Event_Disconnection_Received_Handler
    Public Class Event_Disconnection_Received_Args
        Inherits EventArgs
        Public sKey As [String]
        Public iCount As [Int32]
        Public Sub New(__sKey As [String], __iCount As [Int32])
            sKey = __sKey
            iCount = __iCount
        End Sub
    End Class
#End Region
#Region "WS_Connection_Received_Events"
    Public Delegate Sub Event_Connection_Received_Handler(ByVal Sender As Object, ByVal e As Event_Connection_Received_Args)
    Public Event Event_Connection_Received As Event_Connection_Received_Handler
    Public Class Event_Connection_Received_Args
        Inherits EventArgs
        Public sKey As [String]
        Public iCount As [Int32]
        Public Sub New(__sKey As [String], __iCount As [Int32])
            sKey = __sKey
            iCount = __iCount
        End Sub
    End Class
#End Region
#End Region
#Region "Properties"
    Private _KeepAlive As Boolean
    Public ReadOnly Property KeepAlive() As Boolean
        Get
            Return _KeepAlive
        End Get
    End Property
    Private _Initialized As Boolean
    Public ReadOnly Property Initialized() As Boolean
        Get
            Return _Initialized
        End Get
    End Property
    Private _Connections As Integer
    Public ReadOnly Property Connections() As Integer
        Get
            Try
                _Connections = clients.Count
            Catch ex As Exception
                _Connections = 0
            End Try
            Return _Connections
        End Get
    End Property
#End Region

    Private clients As New ConcurrentDictionary(Of WebSocket, Boolean)()
    Private cancellationTokenSource As New CancellationTokenSource()
    Private acceptClientsTask As Task
    Public Sub New()
        _Initialized = False
        _KeepAlive = False
        _Connections = 0
    End Sub
    Public Function Initialize(Optional localAddress As String = "http://localhost:8181/", Optional KeepAlive As Long = 0) As Boolean
        Initialize = False
        Try

            Dim listener As HttpListener = New HttpListener()
            listener.Prefixes.Add(localAddress) ' Specify the URL to listen on
            listener.Start()

            Console.WriteLine("Listening for WebSocket connections @ " & localAddress)

            _KeepAlive = False

            If KeepAlive > 0 Then
                _KeepAlive = True
            End If

            ' Start a task to handle new client connections
            acceptClientsTask = AcceptClientsAsync(listener, cancellationTokenSource.Token)
            Initialize = True
            _Initialized = Initialize
            Console.WriteLine("WS Fully Initialized @ " & localAddress)
        Catch ex As Exception
            Throw New System.Exception(ex.Message, ex.InnerException)
        End Try
    End Function
    Public Function UnInitialize() As Boolean
        UnInitialize = False
        Try
            If _Initialized Then
                If KeepAlive > 0 Then
                End If
                cancellationTokenSource.Cancel()
                acceptClientsTask.Wait()
            End If
            UnInitialize = True
            Console.WriteLine("WS Fully Uninitialized")
        Catch ex As Exception
            RaiseEvent Event_Exception_Received(Me, New Event_Exception_Args("", "UnInitialize", ex.Message))
        End Try
    End Function
    Public Sub SendToAll(sData As [String])
        Try
            Dim dataBuffer() As Byte = Encoding.UTF8.GetBytes(sData)

            ' Create a list to store clients that need to be removed
            Dim clientsToRemove As New List(Of WebSocket)

            Dim clientList As List(Of WebSocket) = clients.Keys.ToList()
            For index As Integer = clientList.Count - 1 To 0 Step -1
                Dim client = clientList(index)
                Try
                    If client.State = WebSocketState.Open Then
                        client.SendAsync(New ArraySegment(Of Byte)(dataBuffer), WebSocketMessageType.Text, True, CancellationToken.None).Wait()
                    Else
                        RaiseEvent Event_Exception_Received(Me, New Event_Exception_Args(clients.Keys.GetHashCode().ToString, "SendToAll", "client.count:" & clients.Count & ", Message:" & "Connection is closed" & ",sData:" & sData))
                        ' Add the client to the list for removal
                        clientsToRemove.Add(client)
                    End If
                Catch ex As Exception
                    RaiseEvent Event_Exception_Received(Me, New Event_Exception_Args(clients.Keys.GetHashCode().ToString, "SendToAll", "client.count:" & clients.Count & ", Message:" & ex.Message & ",sData:" & sData))
                    ' Add the client to the list for removal
                    clientsToRemove.Add(client)
                End Try
            Next

            'For Each client In clients.Keys
            '    Try
            '        If client.State = WebSocketState.Open Then
            '            client.SendAsync(New ArraySegment(Of Byte)(dataBuffer), WebSocketMessageType.Text, True, CancellationToken.None).Wait()
            '        Else
            '            RaiseEvent Event_Exception_Received(Me, New Event_Exception_Args(clients.Keys.GetHashCode().ToString, "SendToAll", "client.count:" & clients.Count & ", Message:" & "Connection is closed" & ",sData:" & sData))
            '            ' Add the client to the list for removal
            '            clientsToRemove.Add(client)
            '        End If
            '    Catch ex As Exception
            '        RaiseEvent Event_Exception_Received(Me, New Event_Exception_Args(clients.Keys.GetHashCode().ToString, "SendToAll", "client.count:" & clients.Count & ", Message:" & ex.Message & ",sData:" & sData))
            '        ' Add the client to the list for removal
            '        clientsToRemove.Add(client)
            '    End Try
            'Next
            ' Remove invalid clients from the dictionary
            For Each clientToRemove In clientsToRemove
                Dim removed As Boolean = False
                clients.TryRemove(clientToRemove, removed)
            Next

        Catch ex As Exception
            RaiseEvent Event_Exception_Received(Me, New Event_Exception_Args("", "SendToAll", ex.Message))
        End Try
    End Sub
    Public Function SendToClient(key As [String], sData As [String]) As Boolean
        SendToClient = False
        Try
            Dim sDataBuffer() As Byte = Encoding.UTF8.GetBytes(sData)
            If Trim(key) = "" Then
                SendToAll(sData)
                SendToClient = True
                Exit Function
            End If
            Dim targetWebSocket As WebSocket = GetWebSocketByKey(key)
            If targetWebSocket IsNot Nothing Then
                If targetWebSocket.State = WebSocketState.Open Then
                    targetWebSocket.SendAsync(New ArraySegment(Of Byte)(sDataBuffer), WebSocketMessageType.Text, True, CancellationToken.None).Wait()
                    SendToClient = True
                Else
                    Dim removed As Boolean = False
                    clients.TryRemove(targetWebSocket, removed)
                    RaiseEvent Event_Exception_Received(Me, New Event_Exception_Args(clients.Keys.GetHashCode().ToString, "SendToClient", "client.count:" & clients.Count & ", Message:" & "Connection not open" & ",sData:" & sData))
                End If
            Else
                RaiseEvent Event_Exception_Received(Me, New Event_Exception_Args(clients.Keys.GetHashCode().ToString, "SendToClient", "client.count:" & clients.Count & ", Message:" & "Connection not found" & ",sData:" & sData))
            End If
        Catch ex As Exception
            RaiseEvent Event_Exception_Received(Me, New Event_Exception_Args("", "SendToClient", ex.Message))
        End Try
    End Function
    Public Sub DisconnectByKey(key As [String])
        Try
            Dim targetWebSocket As WebSocket = GetWebSocketByKey(key)
            If targetWebSocket IsNot Nothing Then
                targetWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Disconnected by server", CancellationToken.None).Wait()
                clients.TryRemove(targetWebSocket, Nothing)
            End If
        Catch ex As Exception
            RaiseEvent Event_Exception_Received(Me, New Event_Exception_Args("", "DisconnectByKey", ex.Message))
        End Try
    End Sub
    Public Sub DisconnectAll()
        Try
            For Each client In clients.Keys
                Try
                    client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Server shutting down", CancellationToken.None).Wait()
                Catch ex As Exception
                    RaiseEvent Event_Exception_Received(Me, New Event_Exception_Args(clients.Keys.GetHashCode().ToString, "DisconnectAll", ex.Message))
                End Try
            Next
            clients.Clear()
        Catch ex As Exception
            RaiseEvent Event_Exception_Received(Me, New Event_Exception_Args("", "DisconnectAll", ex.Message))
        End Try
    End Sub
    Private Function GetWebSocketByKey(key As [String]) As WebSocket
        Dim targetWebSocket As WebSocket = Nothing
        For Each client In clients.Keys
            If client.GetHashCode().ToString() = key Then
                targetWebSocket = client
                Exit For
            End If
        Next
        Return targetWebSocket
    End Function
    Private Async Function AcceptClientsAsync(listener As HttpListener, cancellationToken As CancellationToken) As Task
        While Not cancellationToken.IsCancellationRequested
            Dim context As HttpListenerContext = Await listener.GetContextAsync()

            If context.Request.IsWebSocketRequest Then
                Try
                    Dim wsContext As HttpListenerWebSocketContext = Await context.AcceptWebSocketAsync(subProtocol:=Nothing)
                    Dim webSocket As WebSocket = wsContext.WebSocket

                    clients.TryAdd(webSocket, True)
                    RaiseEvent Event_Connection_Received(Me, New Event_Connection_Received_Args(webSocket.GetHashCode(), clients.Count))
                    ' Start a task to handle communication for each client
                    Dim handleClientTask As Task = HandleClientAsync(webSocket, cancellationToken)
                Catch ex As Exception
                    RaiseEvent Event_Exception_Received(Me, New Event_Exception_Args("", "AcceptClientsAsync", ex.Message))
                End Try
            Else
                context.Response.StatusCode = 400
                context.Response.Close()
            End If
        End While
    End Function
    Private Async Function HandleClientAsync(webSocket As WebSocket, cancellationToken As CancellationToken) As Task
        Dim receiveBuffer(1024) As Byte
        While webSocket.State = WebSocketState.Open And Not cancellationToken.IsCancellationRequested
            Dim receiveResult As WebSocketReceiveResult = Await webSocket.ReceiveAsync(New ArraySegment(Of Byte)(receiveBuffer), CancellationToken.None)
            If receiveResult.MessageType = WebSocketMessageType.Text Then
                Dim receivedData As String = Encoding.UTF8.GetString(receiveBuffer, 0, receiveResult.Count)
                RaiseEvent Event_Data_Received(Me, New Event_Data_Received_Args(webSocket.GetHashCode(), receivedData))
            End If
        End While
        Try
            ' Remove the client from the dictionary when the connection is closed
            clients.TryRemove(webSocket, Nothing)
            webSocket.Dispose()
            RaiseEvent Event_Disconnection_Received(Me, New Event_Disconnection_Received_Args(webSocket.GetHashCode(), clients.Count))
        Catch ex As Exception
            RaiseEvent Event_Exception_Received(Me, New Event_Exception_Args("", "HandleClientAsync", ex.Message))
        End Try
    End Function
End Class
