Imports System.Net.WebSockets
Imports System.Text
Imports System.Threading
Imports System.Threading.Tasks
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Net ' For DNS resolution

Public Class WSDBConnector
    Private _webSocket As ClientWebSocket
    Private _keepAliveInterval As Integer = 30000 ' in milliseconds
    Private _commandTimeout As Integer = 5000 ' in milliseconds
    Private _state As Integer = 0
    Private _dbException As String = String.Empty
    Private _dbResponseJSON As String = String.Empty
    Private _dbelapsedMilliseconds As Long = 0
    Private _dbrowsAffected As Long = 0
    Private _wsdbServer As String = String.Empty
    Private _wsdbServerConnectionTimeout As Integer = 5000 ' in milliseconds

    Public Sub New(wsdbServer As String, Optional wsdbServerConnectionTimeout As Integer = 5000, Optional keepAliveInterval As Integer = 30000)
        Me._wsdbServer = wsdbServer
        Me._wsdbServerConnectionTimeout = wsdbServerConnectionTimeout
        Me._keepAliveInterval = keepAliveInterval

        Try
            ' Resolve the domain name to an IP address
            Dim resolvedUri As Uri = ResolveDomainToIPAsync(wsdbServer).Result
            ConnectAsync(resolvedUri, wsdbServerConnectionTimeout).Wait(wsdbServerConnectionTimeout)
        Catch ex As Exception
            _dbException = ex.Message
            Throw New Exception("Exception New(): " & _dbException, ex.InnerException)
        End Try
    End Sub

    Private Async Function ResolveDomainToIPAsync(domain As String) As Task(Of Uri)
        Try
            ' Extract the hostname from the URI
            Dim uri As New Uri(domain)
            Dim hostname As String = uri.Host

            ' Resolve the hostname to an IP address
            Dim hostEntry As IPHostEntry = Await Dns.GetHostEntryAsync(hostname)
            If hostEntry.AddressList.Length = 0 Then
                Throw New Exception("No IP addresses found for the domain.")
            End If

            ' Use the first IP address (you can choose a specific one if needed)
            Dim ipAddress As IPAddress = hostEntry.AddressList(0)

            ' Reconstruct the URI with the resolved IP address
            Dim resolvedUri As New UriBuilder(uri)
            resolvedUri.Host = ipAddress.ToString()
            Return resolvedUri.Uri
        Catch ex As Exception
            Throw New Exception("Exception ResolveDomainToIPAsync(): " & ex.Message)
        End Try
    End Function

    Private Async Function ConnectAsync(uri As Uri, timeout As Integer) As Task
        If _webSocket IsNot Nothing Then
            Await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, String.Empty, CancellationToken.None)
        End If

        _webSocket = New ClientWebSocket()
        Try
            Using cts As New CancellationTokenSource(timeout)
                Await _webSocket.ConnectAsync(uri, cts.Token)
                _state = 1 ' Connected
            End Using
        Catch ex As OperationCanceledException
            _dbException = "Connection timed out"
            Throw New TimeoutException(_dbException)
        Catch ex As Exception
            _dbException = ex.Message
            Throw New Exception("Exception ConnectAsync(): " & _dbException, ex.InnerException)
        End Try
    End Function

    ' Rest of the class remains unchanged...
End Class