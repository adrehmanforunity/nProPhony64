Imports System.Net.WebSockets
Imports System.Text
Imports System.Threading
Imports System.Threading.Tasks
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

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
            ConnectAsync(wsdbServerConnectionTimeout).Wait(wsdbServerConnectionTimeout)
        Catch ex As Exception
            _dbException = ex.Message
            Throw New Exception("Exception New(): " & _dbException, ex.InnerException)
        End Try
    End Sub

    Private Async Function ConnectAsync(timeout As Integer) As Task
        If _webSocket IsNot Nothing Then
            Await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, String.Empty, CancellationToken.None)
        End If

        _webSocket = New ClientWebSocket()
        Try
            Using cts As New CancellationTokenSource(timeout)
                Await _webSocket.ConnectAsync(New Uri(_wsdbServer), cts.Token)
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
    Public Async Function RunQueryAsync(query As String, queryTimeout As Integer) As Task(Of DataTable)
        If _webSocket Is Nothing OrElse _webSocket.State <> WebSocketState.Open Then
            Throw New Exception("DB socket not connected")
        End If



        Dim message As String = CreateQueryMessage(query, queryTimeout)
        Dim buffer As Byte() = Encoding.UTF8.GetBytes(message)
        Try
            Using cts As New CancellationTokenSource(queryTimeout)
                _dbException = ""
                _dbelapsedMilliseconds = 0
                _dbrowsAffected = 0
                Await _webSocket.SendAsync(New ArraySegment(Of Byte)(buffer), WebSocketMessageType.Text, True, cts.Token)
                _dbResponseJSON = Await ReceiveResponseAsync(queryTimeout, cts.Token)
                ' Parse the JSON response and handle exceptions
                Dim jsonResponse As JObject = JObject.Parse(_dbResponseJSON)
                _dbException = jsonResponse("exception")?.ToString()
                _dbelapsedMilliseconds = CLng(jsonResponse("ElapsedMilliseconds"))
                _dbrowsAffected = CLng(jsonResponse("rowsAffected"))
                If Not String.IsNullOrEmpty(_dbException) Then
                    Throw New Exception(_dbException)
                End If
                ' Convert the "data" section to a DataTable
                If jsonResponse("data").Count > 0 Then
                    Return ConvertJsonToDataTable(jsonResponse("data"))
                Else
                    Return New DataTable
                End If
            End Using
        Catch ex As OperationCanceledException
            _dbException = "Query timed out"
            Throw New TimeoutException(_dbException)
        Catch ex As Exception
            _dbException = ex.Message
            Throw New Exception("Exception RunQueryAsync(): " & ex.Message)
        End Try
    End Function

    Public Function RunQuery(query As String, queryTimeout As Integer) As DataTable
        If _webSocket Is Nothing OrElse _webSocket.State <> WebSocketState.Open Then
            Throw New Exception("DB socket not connected")
        End If

        Dim message As String = CreateQueryMessage(query, queryTimeout)
        Dim buffer As Byte() = Encoding.UTF8.GetBytes(message)
        Try
            Using cts As New CancellationTokenSource(queryTimeout)
                _dbException = ""
                _dbelapsedMilliseconds = 0
                _dbrowsAffected = 0

                ' Send the message synchronously
                _webSocket.SendAsync(New ArraySegment(Of Byte)(buffer), WebSocketMessageType.Text, True, cts.Token).GetAwaiter().GetResult()

                ' Receive the response synchronously
                _dbResponseJSON = ReceiveResponse(queryTimeout, cts.Token)

                ' Parse the JSON response and handle exceptions
                Dim jsonResponse As JObject = JObject.Parse(_dbResponseJSON)
                _dbException = jsonResponse("exception")?.ToString()
                _dbelapsedMilliseconds = CLng(jsonResponse("ElapsedMilliseconds"))
                _dbrowsAffected = CLng(jsonResponse("rowsAffected"))
                If Not String.IsNullOrEmpty(_dbException) Then
                    Throw New Exception(_dbException)
                End If

                ' Convert the "data" section to a DataTable
                If jsonResponse("data").Count > 0 Then
                    Return ConvertJsonToDataTable(jsonResponse("data"))
                Else
                    Return New DataTable()
                End If
            End Using
        Catch ex As OperationCanceledException
            _dbException = ex.Message
            Throw New TimeoutException(_dbException)
        Catch ex As Exception
            _dbException = ex.Message
            Throw New Exception("Exception RunQuery(): " & _dbException)
        End Try
    End Function

    Private Function ReceiveResponse(queryTimeout As Integer, cancellationToken As CancellationToken) As String
        Dim buffer As Byte() = New Byte(4095) {}
        Dim responseMessage As New StringBuilder()
        Dim startTime As DateTime = DateTime.Now
        Do
            Dim result As WebSocketReceiveResult = _webSocket.ReceiveAsync(New ArraySegment(Of Byte)(buffer), cancellationToken).GetAwaiter().GetResult()
            responseMessage.Append(Encoding.UTF8.GetString(buffer, 0, result.Count))
            If (DateTime.Now - startTime).TotalMilliseconds >= queryTimeout Then
                Throw New TimeoutException("Query timed out")
            End If
            If result.EndOfMessage Then Exit Do
            Thread.Sleep(100)
        Loop
        Return responseMessage.ToString()
    End Function


    Private Function ConvertJsonToDataTable(jsonArray As JToken) As DataTable
        Dim dataTable As New DataTable()
        Try
            ' Extract columns from the first row
            Dim firstRow As JObject = jsonArray.First
            For Each prop As JProperty In firstRow.Properties()
                dataTable.Columns.Add(prop.Name, GetType(String))
            Next
            ' Populate rows
            For Each row As JObject In jsonArray.Children(Of JObject)()
                Dim dataRow As DataRow = dataTable.NewRow()
                For Each prop As JProperty In row.Properties()
                    dataRow(prop.Name) = prop.Value.ToString()
                Next
                dataTable.Rows.Add(dataRow)
            Next
            Return dataTable
        Catch ex As Exception
            Throw New Exception("Exception ConvertJsonToDataTable(): " & ex.Message)
        End Try
    End Function
    Private Function CreateQueryMessage(query As String, queryTimeout As Integer) As String
        Dim timestamp As String = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
        Dim id As String = Guid.NewGuid().ToString()
        Return $"{{""GUID"":""{id}"",""timestamp"":""{timestamp}"",""user"":""user1"",""auth"":""AuthKey:your-auth-key"",""type"":""query"",""query"":""{query}"",""connectiontimeout"":{5000},""querytimeout"":{queryTimeout}}}"
    End Function

    Private Async Function ReceiveResponseAsync(queryTimeout As Integer, cancellationToken As CancellationToken) As Task(Of String)
        Try
            Dim buffer As Byte() = New Byte(4095) {}
            Dim responseMessage As New StringBuilder()
            Dim startTime As DateTime = DateTime.Now
            Do
                Dim result As WebSocketReceiveResult = Await _webSocket.ReceiveAsync(New ArraySegment(Of Byte)(buffer), cancellationToken)
                responseMessage.Append(Encoding.UTF8.GetString(buffer, 0, result.Count))
                If (DateTime.Now - startTime).TotalMilliseconds >= queryTimeout Then
                    Throw New TimeoutException("Query timed out")
                End If
                If result.EndOfMessage Then Exit Do
                Await Task.Delay(100, cancellationToken)
            Loop
            Return responseMessage.ToString()
        Catch ex As Exception
            Throw New Exception("Exception ReceiveResponseAsync(): " & ex.Message)
        End Try
    End Function
    Public ReadOnly Property State() As Integer
        Get
            Return _state
        End Get
    End Property
    Public ReadOnly Property DBException() As String
        Get
            Return _dbException
        End Get
    End Property

    Public ReadOnly Property dbResponseJSON() As String
        Get
            Return _dbResponseJSON
        End Get
    End Property
    Public ReadOnly Property DBElapsedMilliseconds() As Long
        Get
            Return _dbelapsedMilliseconds
        End Get
    End Property
    Public ReadOnly Property DBrowsAffected() As Long
        Get
            Return _dbrowsAffected
        End Get
    End Property
    Public Property CommandTimeout() As Integer
        Get
            Return _commandTimeout
        End Get
        Set(ByVal value As Integer)
            _commandTimeout = value
        End Set
    End Property

    Public Function SendData(_Type As String, _DBCallID As String, _SequenceNo As String, _CallerID As String, _Direction As String, _InteractionID As String, _SessionID As String, _LoginName As String, _FileName As String, bData As Byte()) As Boolean
        Dim combinedData As Byte()
        Try
            ' Validate WebSocket connection
            If _webSocket Is Nothing OrElse _webSocket.State <> WebSocketState.Open Then
                Throw New Exception("WebSocket socket not connected")
            End If

            Dim timestamp As String = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            Dim id As String = Guid.NewGuid().ToString()

            'sHeader = $"{{""GUID"":""{id}"",""timestamp"":""{timestamp}"",""DBCallID"":""{_DBCallID}"",""CallerID"":""{CallerID}"",""Directions"":""{_Directions}"",""InteractionID"":""{_InteractionID}"",""SessionID"":""{_SessionID}"",""SequenceNo"":""{_SequenceNo}"",""LoginName"":""{_LoginName}"",""FileName"":""{_FileName}""}}"
            ' Create JSON header object

            Dim headerObj As New Dictionary(Of String, Object)()
            headerObj.Add("GUID", $"{id}")
            headerObj.Add("Type", $"{_Type}")
            headerObj.Add("DBCallID", $"{_DBCallID}")
            headerObj.Add("CallerID", $"{_CallerID}")
            headerObj.Add("Direction", $"{_Direction}")
            headerObj.Add("InteractionID", $"{_InteractionID}")
            headerObj.Add("SessionID", $"{_SessionID}")
            headerObj.Add("SequenceNo", $"{_SequenceNo}")
            headerObj.Add("LoginName", $"{_LoginName}")
            headerObj.Add("TimeStamp", $"{timestamp}")
            headerObj.Add("FileName", $"{_FileName}")
            ' Convert JSON object to string
            Dim headerJson As String = JsonConvert.SerializeObject(headerObj)
            ' Combine header and data
            combinedData = CombineHeaderAndData(headerJson, bData)
        Catch ex As Exception
            Throw New Exception("Exception sending data to server: " & ex.Message)
        End Try
        Try
            ' Send the combined data synchronously
            _webSocket.SendAsync(New ArraySegment(Of Byte)(combinedData), WebSocketMessageType.Binary, True, CancellationToken.None).GetAwaiter().GetResult()
            Return True
        Catch ex As Exception
            Throw New Exception("Error sending data to server: " & ex.Message)
        End Try
    End Function

    ' Function to combine header and data for efficient sending
    Private Function CombineHeaderAndData(sHeader As String, bData As Byte()) As Byte()
        Try
            Dim headerBytes As Byte() = Encoding.UTF8.GetBytes(sHeader)
            Dim combinedLength As Integer = headerBytes.Length + bData.Length
            Dim combinedData As Byte()
            ReDim Preserve combinedData(combinedLength - 1)
            ' Copy header bytes
            Array.Copy(headerBytes, 0, combinedData, 0, headerBytes.Length)
            ' Copy data bytes
            Array.Copy(bData, 0, combinedData, headerBytes.Length, bData.Length)
            Return combinedData
        Catch ex As Exception
            Throw New Exception("Exception CombineHeaderAndData(): " & ex.Message)
        End Try
    End Function
End Class
