Imports System.Data.Odbc
Imports System.Data.SqlClient
Imports System.IO
Imports MySql.Data.MySqlClient
Imports NAudio.Wave
Imports NAudio.Lame
Imports Utilities
Public Class Class_DBConnector
    Private WithEvents UCallBack As Class_UCallBack
    Private INICollection As New Dictionary(Of String, String)
    Private _lastSuccessfulQuery As DateTime
    Private _myDatabaseSQL As DateTime

    Private _myDatabaseType As String

    Private _myDatabaseIP As String
    Private _myDBCheckTimeout As Integer

    Private _myDatabasePort As Integer
    Private _myDatabaseName As String
    Private _myDatabaseUser As String
    Private _myDatabasePassword As String
    Private _myConnectionString As String

    Private _myDatabaseConnectionTimeOut As Integer
    Private _myDatabaseQueryTimeout As Integer

    Private _myDatabaseRecordingQueryTimeout As Integer

    Private MyODBCConnectionToMSSQL As SqlConnection
    Private MyODBCConnectionToMYSQL As MySqlConnection
    Private MyDataReaderToMSSQL As SqlCommand
    Private MyDataReaderToMYSQL As MySqlDataReader
    Public Function SetCallInformation(Data1 As String, Data2 As String) As Boolean
        SetCallInformation = False
        Try
            Dim querytext As String = ""
            Select Case Data1
                Case "CallEnd"
                    querytext = querytext & ""
                    If _myDatabaseType = "mssql" Then
                        querytext = querytext & "update TBL_AgentCalls  set "
                        querytext = querytext & "TBL_AgentCalls.DateTimeHangup = GETDATE(), "
                        querytext = querytext & "TBL_AgentCalls.CallHangupBy='" & IIf(CallInformation.HangupByAgent, 1, 0) & "', "
                        querytext = querytext & "TBL_AgentCalls.CallStatus ='" & Data1 & "', "
                        querytext = querytext & "TBL_AgentCalls.isFeedback ='" & CallInformation.FeedBack & "', "
                        querytext = querytext & "TBL_AgentCalls.CustomerID ='" & CallInformation.CustomerID & "', "
                        querytext = querytext & "TBL_AgentCalls.isVerifyed ='" & CallInformation.CallerVerifyed & "' "
                        querytext = querytext & "where TBL_AgentCalls.RowID=" & CallInformation.RefID_DB
                    Else
                        querytext = querytext & "update TBL_AgentCalls x set "
                        querytext = querytext & "x.DateTimeHangup = now(), "
                        querytext = querytext & "x.CallHangupBy='" & IIf(CallInformation.HangupByAgent, 1, 0) & "', "
                        querytext = querytext & "x.CallStatus ='" & Data1 & "', "
                        querytext = querytext & "x.isFeedback =" & CallInformation.FeedBack & ", "
                        querytext = querytext & "x.CustomerID ='" & CallInformation.CustomerID & "', "
                        querytext = querytext & "x.isVerifyed =" & CallInformation.CallerVerifyed & " "
                        querytext = querytext & "where x.RowID=" & CallInformation.RefID_DB
                    End If

                Case "Answer"
                    querytext = "update TBL_AgentCalls x set x.isAnswered= 1,DateTimeAccpeted=now() where x.RowID=" & CallInformation.RefID_DB
                    If _myDatabaseType = "mssql" Then
                        querytext = "update TBL_AgentCalls set TBL_AgentCalls.isAnswered= 1,DateTimeAccpeted=GETDATE() where TBL_AgentCalls.RowID=" & CallInformation.RefID_DB
                    End If
                    If Val(AgentInformation.ActivitySessionID) > 0 Then
                        dbClass.Activity_Set_SessionID("Talking")
                    End If
                    Call dbClass.Activity_Get_SessionID("Talking", True)
                Case "ACW"
                    Select Case _myDatabaseType
                        Case "access" : querytext = "update TBL_AgentCalls set TBL_AgentCalls.CallDurationACW=DATEDIFF('n',      TBL_AgentCalls.DateTimeHangup , NOW()) where TBL_AgentCalls.RowID=" & CallInformation.RefID_DB
                        Case "mssql" : querytext = "update TBL_AgentCalls set TBL_AgentCalls.CallDurationACW=DATEDIFF(second,      TBL_AgentCalls.DateTimeHangup , GETDATE()) where TBL_AgentCalls.RowID=" & CallInformation.RefID_DB
                        Case Else : querytext = "update TBL_AgentCalls x set x.CallDurationACW=TIMESTAMPDIFF(SECOND,x.DateTimeHangup , now()) where x.RowID=" & CallInformation.RefID_DB
                    End Select
            End Select
            dbClass.RunDBQuery(querytext)
            SetCallInformation = True
        Catch ex As Exception
            Write_Log("", "SetCallInformation:Error:" & ex.Message)
            Throw New System.Exception("SetCallInformation() " & ex.Message)

        End Try

    End Function
    Public Sub UpdateRealTimeStatus(sStatusText As String, NumberString As String)
        'On Error GoTo errinUpdateReadTimeStatus
        Dim dtResult As New DataTable
        Try
            Dim sQueryText As String
            Select Case sStatusText
                Case "NR-Login"
                    sQueryText = "select RowID from TBL_RealTime where PABXExt = '" & AgentInformation.PABXExt & "'"
                    dtResult = RunDBQuery(sQueryText)
                    If dtResult.Rows.Count <= 0 Then
                        Select Case _myDatabaseType
                            Case "access" : sQueryText = "INSERT INTO TBL_RealTime (PABXExt,QueueID,AgentID,Direction,State,Data1,Loggedin,DateTime_Updated) VALUES('" & AgentInformation.PABXExt & "'," & AgentInformation.QueueID & "," & AgentInformation.AgentID & ",'','','',1,NOW())"
                            Case "mssql" : sQueryText = "INSERT INTO TBL_RealTime  VALUES('" & AgentInformation.PABXExt & "',-1," & AgentInformation.AgentID & ",'','','',1,GETDATE())"
                            Case Else : sQueryText = "INSERT INTO TBL_RealTime  VALUES(NULL,'" & AgentInformation.PABXExt & "',-1," & AgentInformation.AgentID & ",'','','',1,NOW())"
                        End Select
                        RunDBQuery(sQueryText)
                    End If
                    Select Case _myDatabaseType
                        Case "access" : sQueryText = "Update TBL_RealTime set AgentID=" & AgentInformation.AgentID & ", QueueID=" & AgentInformation.QueueID & ", Direction='login',State='Login',Data1='',Loggedin=1,DateTime_Updated=NOW() where PABXExt='" & AgentInformation.PABXExt & "'"
                        Case "mssql" : sQueryText = "Update TBL_RealTime set AgentID=" & AgentInformation.AgentID & ", Direction='login',State='Login',Data1='',Loggedin=1,DateTime_Updated=GETDATE() where PABXExt='" & AgentInformation.PABXExt & "'"
                        Case Else : sQueryText = "Update TBL_RealTime set AgentID=" & AgentInformation.AgentID & ", Direction='login',State='Login',Data1='',Loggedin=1,DateTime_Updated=NOW() where PABXExt='" & AgentInformation.PABXExt & "'"
                    End Select

                Case "Logout"
                    sQueryText = "Update TBL_RealTime set AgentID='-1', Direction='Logout',State='Logout',Data1='logout',Loggedin=0,DateTime_Updated=NOW() where PABXExt='" & AgentInformation.PABXExt & "'"
                    If _myDatabaseType = "mssql" Then
                        sQueryText = "Update TBL_RealTime set AgentID='-1', Direction='Logout',State='Logout',Data1='logout',Loggedin=0,DateTime_Updated=GETDATE() where PABXExt='" & AgentInformation.PABXExt & "'"
                    End If

                Case "Inbound", "Outbound"
                    sQueryText = "Update TBL_RealTime set Direction='" & sStatusText & "',State='Idle',Data1='',DateTime_Updated=NOW() where PABXExt='" & AgentInformation.PABXExt & "'"
                    If _myDatabaseType = "mssql" Then
                        sQueryText = "Update TBL_RealTime set Direction='" & sStatusText & "',State='Idle',Data1='',DateTime_Updated=GETDATE() where PABXExt='" & AgentInformation.PABXExt & "'"
                    End If

                Case "Ringing"
                    sQueryText = "Update TBL_RealTime set State='" & sStatusText & "',Data1='" & NumberString & "',DateTime_Updated=NOW() where PABXExt='" & AgentInformation.PABXExt & "'"
                    If _myDatabaseType = "mssql" Then
                        sQueryText = "Update TBL_RealTime set State='" & sStatusText & "',Data1='" & NumberString & "',DateTime_Updated=GETDATE() where PABXExt='" & AgentInformation.PABXExt & "'"
                    End If

                Case "Talking", "OnHold", "Abandon", "Reject"
                    sQueryText = "Update TBL_RealTime set State='" & sStatusText & "',Data1='" & NumberString & "',DateTime_Updated=NOW() where PABXExt='" & AgentInformation.PABXExt & "'"
                    If _myDatabaseType = "mssql" Then
                        sQueryText = "Update TBL_RealTime set State='" & sStatusText & "',Data1='" & NumberString & "',DateTime_Updated=GETDATE() where PABXExt='" & AgentInformation.PABXExt & "'"
                    End If

                Case Else
                    sQueryText = "Update TBL_RealTime set State='" & sStatusText & "',Data1='',DateTime_Updated=NOW() where PABXExt='" & AgentInformation.PABXExt & "'"
                    If _myDatabaseType = "mssql" Then
                        sQueryText = "Update TBL_RealTime set State='" & sStatusText & "',Data1='',DateTime_Updated=GETDATE() where PABXExt='" & AgentInformation.PABXExt & "'"
                    End If

            End Select

            RunDBQuery(sQueryText)

        Catch ex As Exception
            Write_Log("", "UpdateRealyTimeStatus:Error:" & ex.Message)
            Throw New System.Exception("UpdateRealyTimeStatus() " & ex.Message)
        End Try



    End Sub
    Public Function RunDBQuery(Query As String, Optional from As String = "") As DataTable
        Dim _DST = New DataTable
        Dim tryCount As Integer = 0
tryAgain:
        Try
            Debug.WriteLine("Query:" & Query)
            Select Case _myDatabaseType
                Case "mssql", "access"
                    If from = "UCallBack" Then
                        Select Case MyODBCConnectionToMSSQL.State
                            Case ConnectionState.Executing, ConnectionState.Connecting, ConnectionState.Fetching
                                Write_Log("", "RunDBQuery(" & tryCount & "):from=" & from & " - State=" & MyODBCConnectionToMYSQL.State)
                                Return _DST
                        End Select
                    End If
                    Dim selectCMD As New SqlCommand(Query, MyODBCConnectionToMSSQL)
                    selectCMD.CommandTimeout = _myDatabaseQueryTimeout
                    Dim objAdapter As New SqlDataAdapter(selectCMD)
                    objAdapter.Fill(_DST)
                Case Else
                    Dim selectCMD As New MySqlCommand(Query, MyODBCConnectionToMYSQL)
                    selectCMD.CommandTimeout = _myDatabaseQueryTimeout
                    Dim objAdapter As New MySqlDataAdapter(selectCMD)
                    objAdapter.Fill(_DST) '; //opens And closes the DB connection automatically !! (fetches from pool)
            End Select

            _myDatabaseSQL = DateTime.Now

        Catch ex As Exception
            Write_Log("", "RunDBQuery(" & tryCount & "):error=" & ex.Message & " - " & Query)
            If tryCount = 0 Then
                'If ex.Message.ToString.Contains("Internal connection fatal error.") = True Then
                tryCount = tryCount + 1
                Write_Log("", "RunDBQuery(" & tryCount & "):Closing Connection")
                CloseDatabase("")
                Dim x As String = ""
                Application.DoEvents()
                Dim dbConnected As Boolean = False
                Try
                    Write_Log("", "RunDBQuery(" & tryCount & "):Making New Connection")
                    dbConnected = dbClass.OpenDatabase(x)
                    Write_Log("", "RunDBQuery(" & tryCount & "):New connection made:" & x)
                    GoTo tryAgain
                Catch exx As Exception
                    Write_Log("", "RunDBQuery() Connection Failed " & exx.Message & ", x=" & x & ", Query=" & Query)
                End Try
                'End If
            End If
            Throw New System.Exception(ex.Message)
        End Try
        Return _DST
    End Function
    Public Function Extenstion_GET_FromPool(AgentID As Integer) As DataTable
        Dim dtResult As New DataTable
        Try
            dtResult = RunDBQuery("EXEC [SP_SYS_GET_IDLE_EXT] '1','" & AgentID & "','ProPhony','ProPhony'")
        Catch ex As Exception
            Throw New System.Exception(ex.Message)
        End Try
        Return dtResult
    End Function

    Public Function Get_Agent(ByVal _agentID As String) As DataTable
        Dim dtResult As New DataTable
        Get_Agent = dtResult
        Try
            Dim SQLString As String = ""
            SQLString = SQLString & " SELECT Top 1 [dbo].[TBL_Agents].[RowID]"
            SQLString = SQLString & " ,[dbo].[TBL_Agents].[LoginName]"
            SQLString = SQLString & " ,[dbo].[TBL_Agents].[FirstName] +' '+ [dbo].[TBL_Agents].[LastName]  as AgentName"
            SQLString = SQLString & " ,[dbo].[TBL_RealTime].[Loggedin] "
            SQLString = SQLString & " ,[dbo].[TBL_RealTime].[PABXExt] "
            SQLString = SQLString & " ,[dbo].[TBL_RealTime].[State]"
            SQLString = SQLString & " ,[dbo].[TBL_RealTime].[Direction]"
            SQLString = SQLString & " FROM [dbo].[TBL_Agents] "
            SQLString = SQLString & " INNER JOIN [dbo].[TBL_RealTime] "
            SQLString = SQLString & " ON [dbo].[TBL_Agents].RowID = [dbo].[TBL_RealTime].AgentID "
            SQLString = SQLString & " where [dbo].[TBL_Agents].LoginName='" & _agentID & "' "
            SQLString = SQLString & " order by [dbo].[TBL_RealTime].[DateTime_Updated] desc"
            dtResult = RunDBQuery(SQLString)
        Catch ex As Exception
            Write_Log("", "Get_Agent:Error:" & ex.Message)
            Throw New System.Exception("Get_Agent() " & ex.Message)
        End Try
        Get_Agent = dtResult
    End Function
    Public Function Get_Workcodes() As DataTable
        Dim dtResult As New DataTable
        Get_Workcodes = dtResult
        Try
            Dim SQLString As String = "SELECT * FROM TBL_WorkCodes where status = 1 order by Description"
            dtResult = RunDBQuery(SQLString)
        Catch ex As Exception
            Write_Log("", "Get_Workcodes:Error:" & ex.Message)
            Throw New System.Exception("Get_Workcodes() " & ex.Message)
        End Try
        Get_Workcodes = dtResult
    End Function
    Public Function Activity_Get_SessionID(DNCaption As String, Optional InsertOnly As Boolean = False) As Boolean
        Activity_Get_SessionID = False
        Dim dtResult As New DataTable

        Try
            AgentInformation.ActivitySessionID = GetRandomID()
            Dim SQLString As String
            Select Case _myDatabaseType
                Case "access" : SQLString = "INSERT INTO TBL_AgentActivity (RowID,LoginSessionID, Activity, ActivityStartDateTime, ActivityEndDateTime, ActivityDuration, NewState, RefID_DB) VALUES(" & AgentInformation.ActivitySessionID & "," & AgentInformation.LoginSessionID & ",'" & DNCaption & "', NOW(),NOW(),0,''," & CallInformation.RefID_DB & ")"
                Case "mssql" : SQLString = "INSERT INTO TBL_AgentActivity VALUES(" & AgentInformation.ActivitySessionID & "," & AgentInformation.LoginSessionID & ",'" & DNCaption & "', GETDATE(),GETDATE(),0,''," & CallInformation.RefID_DB & ")"
                Case Else : SQLString = "INSERT INTO TBL_AgentActivity VALUES(" & AgentInformation.ActivitySessionID & "," & AgentInformation.LoginSessionID & ",'" & DNCaption & "', NOW(),NOW(),0,''," & CallInformation.RefID_DB & ")"
            End Select
            dtResult = RunDBQuery(SQLString)
            If InsertOnly = True Then
                Activity_Get_SessionID = True
                Exit Function
            End If
            Select Case _myDatabaseType
                Case "access" : SQLString = "Select RowID as ActivitySessionID from TBL_AgentActivity where RowID=" & AgentInformation.ActivitySessionID & ""
                Case Else : SQLString = "Select RowID as ActivitySessionID from TBL_AgentActivity where RowID='" & AgentInformation.ActivitySessionID & "'"
            End Select
            dtResult = RunDBQuery(SQLString)
            AgentInformation.ActivitySessionID = -1
            If dtResult.Rows.Count > 0 Then
                AgentInformation.ActivitySessionID = "" & dtResult.Rows(0)("ActivitySessionID")
            End If
            Activity_Get_SessionID = True
        Catch ex As Exception
            Write_Log("", "Activity_Get_SessionID:Error:" & ex.Message)
            Throw New System.Exception("Activity_Get_SessionID() " & ex.Message)
        End Try

    End Function
    Public Function Activity_Set_SessionID(DNCaption As String) As Boolean
        Activity_Set_SessionID = False
        Dim dtResult As New DataTable

        Try
            Dim SQLString As String

            Select Case _myDatabaseType
                Case "access" : SQLString = "update TBL_AgentActivity  set TBL_AgentActivity.ActivityEndDateTime = NOW(), TBL_AgentActivity.NewState ='" & DNCaption & "', TBL_AgentActivity.RefID_DB=" & CallInformation.RefID_DB & ", TBL_AgentActivity.ActivityDuration = DATEDIFF('n',      TBL_AgentActivity.ActivityStartDateTime, NOW()) where TBL_AgentActivity.RowID=" & AgentInformation.ActivitySessionID
                Case "mssql" : SQLString = "update TBL_AgentActivity  set TBL_AgentActivity.ActivityEndDateTime = GETDATE(), TBL_AgentActivity.NewState ='" & DNCaption & "', TBL_AgentActivity.RefID_DB=" & CallInformation.RefID_DB & ", TBL_AgentActivity.ActivityDuration = DATEDIFF(second,      TBL_AgentActivity.ActivityStartDateTime, GETDATE()) where TBL_AgentActivity.RowID=" & AgentInformation.ActivitySessionID
                Case Else : SQLString = "update TBL_AgentActivity x set x.ActivityEndDateTime = now(), x.ActivityDuration=0, x.NewState ='" & DNCaption & "', x.RefID_DB=" & CallInformation.RefID_DB & ", x.ActivityDuration = TIMESTAMPDIFF(SECOND,x.ActivityStartDateTime,now()) where x.RowID=" & AgentInformation.ActivitySessionID
            End Select
            dtResult = RunDBQuery(SQLString)
            AgentInformation.ActivitySessionID = -1
            Activity_Set_SessionID = True

        Catch ex As Exception
            Write_Log("", "Activity_Set_SessionID:Error:" & ex.Message)
            Throw New System.Exception("Activity_Set_SessionID() " & ex.Message)
        End Try

    End Function
    Public Function Login_Get_SessionID() As DataTable
        Dim dtResult As New DataTable
        Dim TmpSQLString As String

        Try

            AgentInformation.LoginSessionID = GetRandomID()

            Select Case _myDatabaseType
                Case "access" : TmpSQLString = "INSERT INTO TBL_AgentLoginSessions (RowID, AgentID, LoginDateTim, LogoutDateTime, FromComputerName, FromIPAddress, FromOSUser, AppTitle, App, AppDate, AppPath ) VALUES('" & AgentInformation.LoginSessionID & "'," & AgentInformation.AgentID & ", NOW(),NOW(),'" & AgentInformation.FromComputerName & "','" & AgentInformation.FromIPAddress & "','" & AgentInformation.FromOSUser & "','" & AgentInformation.AppTitle & "','" & AgentInformation.AppVersion & "','" & AgentInformation.AppDate & "', '" & AgentInformation.AppPath & "')"
                Case "mssql" : TmpSQLString = "INSERT INTO TBL_AgentLoginSessions VALUES(" & AgentInformation.LoginSessionID & "," & AgentInformation.AgentID & ", GETDATE(),'','" & AgentInformation.FromComputerName & "','" & Mid(AgentInformation.FromIPAddress, 1, 20) & "','" & AgentInformation.FromOSUser & "','" & AgentInformation.AppTitle & "','" & AgentInformation.AppVersion & "','" & AgentInformation.AppDate & "', '" & AgentInformation.AppPath & "')"
                Case Else : TmpSQLString = "INSERT INTO TBL_AgentLoginSessions VALUES(" & AgentInformation.LoginSessionID & "," & AgentInformation.AgentID & ", NOW(),NOW(),'" & AgentInformation.FromComputerName & "','" & Mid(AgentInformation.FromIPAddress, 1, 20) & "','" & AgentInformation.FromOSUser & "','" & AgentInformation.AppTitle & "','" & AgentInformation.AppVersion & "','" & AgentInformation.AppDate & "', '" & AgentInformation.AppPath & "')"
            End Select

            dtResult = RunDBQuery(TmpSQLString)
            Select Case _myDatabaseType
                Case "access" : TmpSQLString = "Select RowID as LoginSessionID from TBL_AgentLoginSessions where RowID=" & AgentInformation.LoginSessionID & ""
                Case Else : TmpSQLString = "Select RowID as LoginSessionID from TBL_AgentLoginSessions where RowID='" & AgentInformation.LoginSessionID & "'"
            End Select

            dtResult = RunDBQuery(TmpSQLString)
            'AgentInformation.LoginSessionID = -1
            If dtResult.Rows.Count > 0 Then
                AgentInformation.LoginSessionID = "" & dtResult.Rows(0)("LoginSessionID")
            End If

        Catch ex As Exception
            Write_Log("", "Login_Get_SessionID:Error:" & ex.Message)
            Throw New System.Exception("Login_Get_SessionID() " & ex.Message)
        End Try
        Return dtResult
    End Function
    Public Function CallRecording_Get(callID As String, recID As String, Optional FolderPath As String = "") As String
        CallRecording_Get = ""
        Try

            'https://www.codeproject.com/Articles/9653/Saving-and-Retrieving-Wav-files-from-MSSQL-MSDE-an

            Dim sSQL As String = "SELECT RowID,RefID_DB FROM TBL_CallsData AS CallsData  WHERE CallsData.RefID_DB ='" & callID & "'"


            Dim xDataTable As DataTable
            xDataTable = RunDBQuery(sSQL)
            Dim RowNumber As Integer = 0

            Dim NewFolderName As String = ""
            'If FolderPath = "" Then
            NewFolderName = LogDir & "\" & GetRandomID()
            MkDir(NewFolderName)
            'Else
            'NewFolderName = FolderPath & "\" & GetRandomID()
            'End If

            For Each row In xDataTable.Rows

                Application.DoEvents()
                sSQL = "SELECT CallData FROM TBL_CallsData AS CallsData  WHERE CallsData.RowID ='" & row("RowID") & "'"
                RowNumber = RowNumber + 1
                Dim strOutFile = RowNumber & "-" & GetRandomID() & ".wav"
                Dim b() As Byte
                Dim K As Long
                Dim selectCMD As Object
                Select Case _myDatabaseType
                    Case "access", "mssql"
                        selectCMD = New SqlCommand(sSQL, MyODBCConnectionToMSSQL)
                    Case Else
                        selectCMD = New MySqlCommand(sSQL, MyODBCConnectionToMYSQL)
                End Select
                selectCMD.CommandTimeout = _myDatabaseQueryTimeout
                selectCMD.CommandText = sSQL
                If selectCMD.Connection.State <> ConnectionState.Open Then
                    selectCMD.Connection.Open()
                End If
                b = selectCMD.ExecuteScalar()
                K = UBound(b)
                Dim WriteFs As New FileStream(NewFolderName & "\" & strOutFile, FileMode.Create, FileAccess.Write)
                WriteFs.Write(b, 0, K)
                WriteFs.Close()
                CallRecording_Get = NewFolderName
                Application.DoEvents()
            Next
        Catch ex As Exception
            Write_Log("", "CallRecording_Get:Error:" & ex.Message)
            Throw New System.Exception("CallRecording_Get() " & ex.Message)
        End Try
    End Function

    Public Function CallRecording_Put() As String
        CallRecording_Put = ""
        Try
            Dim tmpFileName As String = CallInformation.RecordingFileName
            Dim tmpRefID_DB As String = CallInformation.RefID_DB

            If Trim(tmpFileName) = "" Then
                ''''' call disconnected while on hold
                Exit Function
            End If

            If File.Exists(tmpFileName) = False Then
                Write_Log("", "CallRecording_Put:Error:" & tmpFileName & " is missing, informing user:" & ") CallInformation.RefID_DB:" & CallInformation.RefID_DB & ", CalledID:" & CallInformation.CalledID & ", RecordingFileName:" & CallInformation.RecordingFileName)
                Throw New System.Exception("CallRecording_Put() " & "Please be aware, system is not recording calls, please contact system support ASAP")
            End If
            'https://www.codeproject.com/Articles/9653/Saving-and-Retrieving-Wav-files-from-MSSQL-MSDE-an

            'Write_Log("", "CallRecording_Put:Encrypting:" & tmpFileName)
            'Dim x As Boolean = yObject.Encrypt(tmpFileName)
            'Write_Log("", "CallRecording_Put:Encrypting:Result=" & x)



            Dim fs As FileStream = Nothing
            Dim br As BinaryReader = Nothing
            Dim intWavLength As Integer = 0
            Dim info As New FileInfo(tmpFileName)
            Dim WavFile(CInt(info.Length)) As Byte
            Try
                fs = New FileStream(tmpFileName, FileMode.Open, FileAccess.Read)
                br = New BinaryReader(fs)
                WavFile = br.ReadBytes(CInt(info.Length))
                intWavLength = Int(CType(WavFile.Length, Integer))
            Finally
                ' make sure objects are closed in case the thread
                ' was aborted in the middle of this method
                If Not (br Is Nothing) Then br.Close()
                If Not (fs Is Nothing) Then fs.Close()
            End Try
            Dim tmpRowID As String = GetRandomID()
            Dim tmpDateTime = Now.ToString("yyyy-MM-dd HH:mm:ss")
            Dim strBase64WavFile As String = Convert.ToBase64String(WavFile)
            Dim quertText As String : quertText = ""
            Select Case _myDatabaseType
                Case "access"
                    quertText = quertText & "INSERT INTO TBL_CallsData (RowID,RefID_DB,InsertMoment,CallData)"
                    quertText = quertText & " VALUES('" & tmpRowID & "','" & CallInformation.RefID_DB & "','" & tmpDateTime & "','" & strBase64WavFile & "')"
                Case "mssql"
                    quertText = ""
                    quertText = quertText & "INSERT INTO TBL_CallsData (RowID,RefID_DB,InsertMoment,CallData)"
                    quertText = quertText & " VALUES('" & tmpRowID & "','" & CallInformation.RefID_DB & "','" & tmpDateTime & "','" & strBase64WavFile & "')"
                Case Else
                    quertText = ""
                    quertText = quertText & "INSERT INTO TBL_CallsData (RowID,RefID_DB,InsertMoment,CallData)"
                    quertText = quertText & " VALUES('" & tmpRowID & "','" & CallInformation.RefID_DB & "','" & tmpDateTime & "','" & strBase64WavFile & "');"
            End Select
            'Dim dtResult As DataTable
            'dtResult = RunDBQuery(quertText)

            Dim sSQL As String = ""
            Dim RecordingTryCount As Integer = 0
RetryToPut:
            Select Case _myDatabaseType
                Case "access", "mssql"
                    sSQL = sSQL & "INSERT INTO TBL_CallsData(RowID,RefID_DB,InsertMoment,CallData)"
                    sSQL = sSQL & "values (@tmpRowID,@RefID_DB,@tmpDateTime, @CallData)"
                    Dim selectCMD As New SqlCommand(sSQL, MyODBCConnectionToMSSQL)
                    If RecordingTryCount > 0 Then
                        Dim x As String = ""
                        Try
                            Write_Log("", "CallRecording_Put(" & RecordingTryCount & "):DB Open")
                            If OpenDatabase(x) = False Then
                            End If
                            Write_Log("", "CallRecording_Put(" & RecordingTryCount & "):DB Open Done")
                        Catch ex As Exception
                            Write_Log("", "CallRecording_Put(" & RecordingTryCount & "):DB Open Failed")
                            Throw New System.Exception("CallRecording_Put(" & RecordingTryCount & ") " & ex.Message)
                        End Try
                    End If
                    Application.DoEvents()
                    selectCMD.CommandTimeout = _myDatabaseQueryTimeout + 10
                    selectCMD.CommandText = sSQL
                    selectCMD.Parameters.Clear()
                    selectCMD.Parameters.AddWithValue("@tmpRowID", tmpRowID)
                    selectCMD.Parameters.AddWithValue("@RefID_DB", CallInformation.RefID_DB)
                    selectCMD.Parameters.AddWithValue("@tmpDateTime", tmpDateTime)
                    selectCMD.Parameters.AddWithValue("@CallData", WavFile)
                    Try
                        If selectCMD.Connection.State <> ConnectionState.Open Then
                            Write_Log("", "CallRecording_Put(" & RecordingTryCount & "):DB Open")
                            selectCMD.Connection.Open()
                            Write_Log("", "CallRecording_Put(" & RecordingTryCount & "):DB Open Done")
                        End If
                        selectCMD.ExecuteNonQuery()
                        CallRecording_Put = tmpRowID
                    Catch oExcept As Exception
                        Write_Log("", "CallRecording_Put(" & RecordingTryCount & "):Error:" & oExcept.Message)
                        If RecordingTryCount = 0 Then
                            RecordingTryCount = RecordingTryCount + 1
                            GoTo RetryToPut
                        Else
                            Throw New System.Exception("CallRecording_Put(" & RecordingTryCount & ") " & oExcept.Message)
                        End If
                        ''Throw New System.Exception("CallRecording_Put() " & oExcept.Message)
                    End Try
                Case Else
                    sSQL = sSQL & "INSERT INTO TBL_CallsData(RowID,RefID_DB,InsertMoment,CallData)"
                    sSQL = sSQL & "values (?tmpRowID,?RefID_DB,?tmpDateTime, ?CallData)"
                    Dim selectCMD As New MySqlCommand(sSQL, MyODBCConnectionToMYSQL)
                    selectCMD.CommandTimeout = _myDatabaseQueryTimeout
                    selectCMD.CommandText = sSQL
                    selectCMD.Parameters.Clear()
                    selectCMD.Parameters.AddWithValue("?tmpRowID", tmpRowID)
                    selectCMD.Parameters.AddWithValue("?RefID_DB", CallInformation.RefID_DB)
                    selectCMD.Parameters.AddWithValue("?tmpDateTime", tmpDateTime)
                    selectCMD.Parameters.AddWithValue("?CallData", WavFile)
                    If selectCMD.Connection.State <> ConnectionState.Open Then
                        selectCMD.Connection.Open()
                    End If
                    Try
                        selectCMD.ExecuteNonQuery()
                        CallRecording_Put = tmpRowID
                    Catch oExcept As Exception
                        Write_Log("", "CallRecording_Put(" & RecordingTryCount & "):Error:" & oExcept.Message)
                        Throw New System.Exception("CallRecording_Put(" & RecordingTryCount & ") " & oExcept.Message)
                    End Try
            End Select
            Dim DiskCleansing As Integer = 0
            DiskCleansing = 0
            If GetINIString("Recording", "DiskCleansing", "1") = "1" Then
                DiskCleansing = 1
            End If
            If DiskCleansing = 1 Then
                Write_Log("", "CallRecording_Put:Deleting:" & tmpFileName & ", RowID:" & tmpRowID & ") CallInformation.RefID_DB:" & CallInformation.RefID_DB & ", CallerID:" & CallInformation.CallerID & ", CalledID:" & CallInformation.CalledID)
                Try
                    File.Delete(tmpFileName)
                    Write_Log("", "CallRecording_Put:Deleted:" & tmpFileName & ", RowID:" & tmpRowID & ") CallInformation.RefID_DB:" & CallInformation.RefID_DB & ", CallerID:" & CallInformation.CallerID & ", CalledID:" & CallInformation.CalledID)
                Catch ex As Exception
                    Write_Log("", "CallRecording_Put:Deleting:Err:" & ex.Message & ", Error:" & ex.Message & "," & tmpFileName & ", RowID:" & tmpRowID & ") CallInformation.RefID_DB:" & CallInformation.RefID_DB & ", CallerID:" & CallInformation.CallerID & ", CalledID:" & CallInformation.CalledID)
                End Try
            Else
                Write_Log("", "CallRecording_Put:Deleting:" & tmpFileName & ", RowID:" & tmpRowID & ") CallInformation.RefID_DB:" & CallInformation.RefID_DB & ", CallerID:" & CallInformation.CallerID & ", CalledID:" & CallInformation.CalledID & ", DiskCleansing is disabled")
            End If
        Catch ex As Exception
            Write_Log("", "CallRecording_Put:Error:" & ex.Message)
            Throw New System.Exception("CallRecording_Put() " & ex.Message)
        End Try
    End Function
    Public Function Login_Set_SessionID() As Boolean
        Dim dtResult As New DataTable
        Login_Set_SessionID = False
        Dim QueryString As String

        Try
            QueryString = "update TBL_AgentLoginSessions x set x.LogoutDateTime= now() where x.RowID=" & AgentInformation.LoginSessionID
            If _myDatabaseType = "mssql" Then
                QueryString = "update TBL_AgentLoginSessions set TBL_AgentLoginSessions.LogoutDateTime= GETDATE() where TBL_AgentLoginSessions.RowID=" & AgentInformation.LoginSessionID
            End If
            dtResult = RunDBQuery(QueryString)
            AgentInformation.LoginSessionID = -1
            Call UpdateRealTimeStatus("Logout", "")
            Login_Set_SessionID = True
        Catch ex As Exception
            Write_Log("", "Login_Set_SessionID:Error:" & ex.Message)
            Throw New System.Exception("Login_Set_SessionID() " & ex.Message)
        End Try
    End Function
    Private Function GetRND() As String
        'Dim xNumber As Long
        'Randomize(99999)
        'xNumber = Int((Rnd() * 99999) + 1)
        'GetRND = xNumber
        Static Random_Number As New Random()
        Return Random_Number.Next(10000, 99999).ToString
    End Function
    Private Function GetRandomID() As String
        Dim TmpString As String = ""
        TmpString = Now.ToString("yyyyMMddHHffff")
        'DateTime.Now.ToString("HH:mm:ss:ffff").ToString()
        'TmpString = Format(Now, "YYYYMMDDHHnnss")
        TmpString = TmpString & GetRND()
        GetRandomID = TmpString
    End Function
    Public Function Extenstion_Release_FromPool(AgentID As Integer, PoolID As Integer) As DataTable
        Dim dtResult As New DataTable
        Try
            dtResult = RunDBQuery("EXEC [SP_SYS_RELEASE_EXT] '" & AgentID & "','" & PoolID & "'")
        Catch ex As Exception
            Throw New System.Exception(ex.Message)
        End Try
        Return dtResult
    End Function
    Public Function Extenstion_Reserve_FromPool(AgentID As Integer, PoolID As Integer) As DataTable
        Dim dtResult As New DataTable
        Try
            dtResult = RunDBQuery("EXEC [SP_SYS_RESERVE_EXT] '" & AgentID & "','" & PoolID & "'")
        Catch ex As Exception
            Throw New System.Exception(ex.Message)
        End Try
        Return dtResult
    End Function
    Public Function GetINIString(section As String, item As String, DefaultValue As String, Optional ININame As String = "")
        Dim valueToReturn As String
        Try
            Dim keyValue As String = Trim(LCase(section) & "@" & Trim(LCase(item)))
            valueToReturn = INICollection.Item(keyValue)
            If Trim(valueToReturn) = "" Or IsNothing(valueToReturn) Then
                valueToReturn = DefaultValue
            End If
        Catch ex As Exception
            valueToReturn = DefaultValue
        End Try
        GetINIString = valueToReturn
    End Function
    Public Function LoadConfiguration() As Boolean
        LoadConfiguration = False
        Try
            Dim tmpQueury As String
            Select Case _myDatabaseType
                Case "access" : tmpQueury = "SELECT * FROM TBL_Configuration where [status] = 1 order by [Section],[Item]"
                Case Else : tmpQueury = "SELECT * FROM TBL_Configuration where status = 1 order by Section,Item"
            End Select
            Dim ConfigDataTable As New DataTable
            ConfigDataTable = RunDBQuery(tmpQueury)
            If ConfigDataTable.Rows.Count <= 0 Then
                Throw New System.Exception("Sorry, Failed to get configuration from server!")
            End If
            Dim Section As String = ""
            Dim Item As String = ""
            Dim value As String = ""

            For Each rowConfig In ConfigDataTable.Rows
                Section = ""
                Item = ""
                value = ""
                Section = Trim(LCase("" & rowConfig("Section")))
                Item = Trim(LCase("" & rowConfig("Item")))
                value = Trim(LCase("" & rowConfig("Value")))
                If Section = "" Then
                    GoTo IgnoreThisOne
                End If
                If Item = "" Then
                    GoTo IgnoreThisOne
                End If
                If value = "" Then
                    GoTo IgnoreThisOne
                End If
                INICollection.Add(Section & "@" & Item, value)
                Write_Log("", "LoadConfiguration:Section:" & Section & "@" & Item & ", value:" & value)
IgnoreThisOne:
            Next
            LoadConfiguration = True
        Catch ex As Exception
            Throw New System.Exception(ex.Message)
        End Try
    End Function
    Public Function VerifyCredentials(UserName As String, Password As String, Optional IgnorePassword As Boolean = False) As DataTable
        Dim TmpQueryString As String = ""
        Try
            Select Case _myDatabaseType
                Case "mssql", "access"
                    If IgnorePassword Then
                        TmpQueryString = "SELECT TOP 1 * FROM TBL_Agents where LOWER(LoginName) = '" & Trim(LCase(UserName)) & "' and status = 1 order by FirstName,LastName"
                    Else
                        TmpQueryString = "SELECT TOP 1 * FROM TBL_Agents where LOWER(LoginName) = '" & Trim(LCase(UserName)) & "' and Password = '" & Password & "' and status = 1 order by FirstName,LastName"
                    End If
                Case Else
                    If IgnorePassword Then
                        TmpQueryString = "SELECT * FROM TBL_Agents where LOWER(LoginName) = '" & Trim(LCase(UserName)) & "' and status = 1 order by FirstName,LastName limit 1"
                    Else
                        TmpQueryString = "SELECT * FROM TBL_Agents where LOWER(LoginName) = '" & Trim(LCase(UserName)) & "' and Password = '" & Password & "' and status = 1 order by FirstName,LastName limit 1"
                    End If
            End Select
            Return RunDBQuery(TmpQueryString)
        Catch ex As Exception
            Throw New System.Exception(ex.Message)
        End Try
    End Function
    Public Function CloseDatabase(ByRef err) As Boolean
        CloseDatabase = False
        err = ""
        Try
            MyODBCConnectionToMSSQL.Close()
        Catch ex As Exception
            Write_Log("", "CloseDatabase :Error:" & ex.Message)
        End Try
    End Function

    Public Function OpenDatabase(ByRef err) As Boolean
        OpenDatabase = False
        err = ""
        Try
            Select Case _myDatabaseType
                Case "access" : _myDatabaseType = "access"
                Case "mssql" : OpenDatabase = OpenDBMSSql(err)
                Case Else : OpenDatabase = OpenDBMySQL(err)
            End Select
            UCallBack.SetupEvent(0, "CheckDBConnection", 5000, "", "", "", Nothing)
        Catch ex As Exception
            Throw New System.Exception(ex.Message)
        End Try
    End Function
    Public Sub New()
        Try

            _myDatabaseType = "mssql"
            _myDatabasePort = "1443"
            Select Case LCase(GetINIStringOldWay("Database", "Type", "mysql"))
                Case "ovsapi"
                    _myDatabaseType = "ovsapi"
                    _myDatabasePort = "3306"
                Case "mssql"
                    _myDatabaseType = "mssql"
                    _myDatabasePort = "1443"
                Case "access"
                    _myDatabaseType = "access"
                    _myDatabasePort = "1443"
                Case Else
                    _myDatabaseType = "mssql"
                    _myDatabasePort = "1443"
            End Select

            _myDatabaseIP = LCase(GetINIStringOldWay("Database", "DBComServer", "192.168.18.90"))
            _myDatabasePort = Int(GetINIStringOldWay("Database", "DatabasePort", _myDatabasePort))
            _myDatabaseName = GetINIStringOldWay("Database", "Database", "ProPhonyV3")
            _myDatabaseUser = GetINIStringOldWay("Database", "UserID", "client")
            _myDatabasePassword = GetINIStringOldWay("Database", "Password", "client1")

            _myConnectionString = Trim(GetINIStringOldWay("Database", "ConnectionString", ""))


            If GetINIStringOldWay("Database", "Encryption", "0") = "1" Then
                Dim wrapper As New Simple3Des
                _myDatabasePassword = wrapper.AES_Decrypt(_myDatabasePassword)
            End If

            _myDatabaseConnectionTimeOut = Int(GetINIStringOldWay("Database", "ConnectionTimeOut", "10"))

            _myDatabaseQueryTimeout = Int(GetINIStringOldWay("Database", "QueryTimeout", "30"))
            _myDatabaseRecordingQueryTimeout = Int(GetINIStringOldWay("Database", "RecordingQueryTimeout", "30"))

            _myDBCheckTimeout = Int(GetINIStringOldWay("Database", "DBCheckTimeout", "300"))


            UCallBack = New Class_UCallBack
        Catch ex As Exception
            Throw New System.Exception(ex.Message)
        End Try
    End Sub
    Private Function OpenDBMSSql(ByRef err As String) As Boolean
        OpenDBMSSql = False
        Try

            Dim strConnection As String = "Data Source=" & _myDatabaseIP & ";Connection Timeout=" & _myDatabaseConnectionTimeOut & ";Initial Catalog=" & _myDatabaseName & ";User ID=" & _myDatabaseUser & ";Password=" & _myDatabasePassword & ";"

            If _myConnectionString <> "" Then
                strConnection = _myConnectionString
            End If

            MyODBCConnectionToMSSQL = New SqlConnection(strConnection)

            Try
                Write_Log("", "OpenDBMSSql:Here to Open Database Connection Again State:" & MyODBCConnectionToMSSQL.State)
                MyODBCConnectionToMSSQL.Close()
                Application.DoEvents()
                Write_Log("", "OpenDBMSSql:Here to Open Database Connection Again New State:" & MyODBCConnectionToMSSQL.State)
                'MyODBCConnectionToMSSQL = Nothing
            Catch ex As Exception
                Write_Log("", "OpenDBMSSql:Here to Open Database Connection Exception:" & ex.Message)
            End Try
            Application.DoEvents()

            If MyODBCConnectionToMSSQL.State <> ConnectionState.Open Then
                MyODBCConnectionToMSSQL.Open()
            End If
            OpenDBMSSql = True
        Catch ex As Exception
            err = ex.Message
            Throw New System.Exception(ex.Message)
        End Try
    End Function
    Private Function OpenDBAccess(ByRef err As String) As Boolean
        OpenDBAccess = False
        Try
            OpenDBAccess = True
        Catch ex As Exception
            err = ex.Message
            Throw New System.Exception(ex.Message)
        End Try
    End Function
    Private Function OpenDBMySQL(ByRef err As String) As Boolean
        OpenDBMySQL = False
        Try
            'Connector/ODBC 3.51 connection string
            Dim MyConString As String = String.Format("SERVER={0};PORT={1};UID={2}; PWD={3}; DATABASE={4};Connect Timeout={5}; pooling=false;",
                                                      _myDatabaseIP,
                                                      _myDatabasePort,
                                                      _myDatabaseUser,
                                                      _myDatabasePassword,
                                                      _myDatabaseName,
                                                      _myDatabaseConnectionTimeOut)
            Debug.WriteLine("Connection String:" & MyConString)

            ' Close the Database if alredy open
            If Not MyODBCConnectionToMYSQL Is Nothing Then MyODBCConnectionToMYSQL.Close()

            MyODBCConnectionToMYSQL = New MySql.Data.MySqlClient.MySqlConnection(MyConString)
            MyODBCConnectionToMYSQL.Open()
            OpenDBMySQL = True
        Catch MyOdbcException As OdbcException
            err = MyOdbcException.ToString
            Throw New System.Exception(MyOdbcException.ToString)
        End Try

    End Function
    'ATIF  START
    Public Sub Global_QMCALL_ADDCAllDetails(_ChannelNo As Integer, CallID As Long, CallerID As String, CalledID As String, QueueCode As String, Direction As String, CallerVerifyed As Integer, CRMSessionID As String, CRMInteractionID As String)
        Dim dtResult As New DataTable
        Try
            dtResult = RunDBQuery("EXEC [SP_SYS_ADD_CALL_DETAILS] " & "'" & CallID & "'," & "'" & CallerID & "'," & "'" & CalledID & "'," & "'" & QueueCode & "'," & "'" & Direction & "'," & "'" & CallerVerifyed & "'," & "'" & CRMSessionID & "'," & "'" & CRMInteractionID & "'")
        Catch ex As Exception
            Throw New System.Exception(ex.Message)
        End Try
    End Sub
    'ATIF  END




    Public Function Server_Get_SessionID(ByRef Optional CallerID As String = "") As Boolean
        Dim dtResult As New DataTable

        Server_Get_SessionID = False
        Try

            Write_Log("", "GetServerSessionID:CallInformation.RefID_Server:" & CallInformation.CallerID)
            Dim QueryString As String
            Select Case _myDatabaseType
                Case "mysql"
                    QueryString = "SELECT * FROM TBL_CallerIDs xCalls WHERE xCalls.CallerID = '" & CallInformation.CallerID & "' ORDER BY xCalls.DateTimeCall DESC LIMIT 1;"
                Case Else
                    QueryString = "EXEC [SP_SYS_GET_CALL_DETAILS] '" & CallInformation.CallerID & "'"
            End Select
            dtResult = RunDBQuery(QueryString)
            CallInformation.RefID_Server = -1
            If dtResult.Rows.Count > 0 Then
                CallInformation.RefID_Server = "" & dtResult.Rows(0)("RefID_Server")
                'On Error Resume Next
                If "" & dtResult.Rows(0)("CalledID") <> "" Then
                    Try
                        CallInformation.CalledID = CallInformation.CallerID
                    Catch ex As Exception
                    End Try
                    Try
                        CallInformation.CallerID = "" & dtResult.Rows(0)("CalledID")
                    Catch ex As Exception
                    End Try
                    CallerID = CallInformation.CallerID
                    Try
                        CallInformation.CustomerID = "" & dtResult.Rows(0)("CustomerID")
                    Catch ex As Exception
                    End Try
                    CallInformation.CallerVerifyed = -1
                    Try
                        If IsNumeric("" & dtResult.Rows(0)("CallerVerifyed")) Then
                            CallInformation.CallerVerifyed = Val(dtResult.Rows(0)("CallerVerifyed"))
                        End If
                    Catch ex As Exception
                    End Try

                    Try
                        CallInformation.CRMSessionID = "" & dtResult.Rows(0)("CRMSessionID")
                    Catch ex As Exception
                    End Try
                    Try
                        CallInformation.CRMInteractionID = "" & dtResult.Rows(0)("CRMInteractionID")
                    Catch ex As Exception
                    End Try
                End If
            End If
            Write_Log("", "GetServerSessionID:CallInformation.CustomerID:" & CallInformation.CustomerID & ",CRMSessionID: " & CallInformation.CRMSessionID & ", CRMInteractionID:" & CallInformation.CRMInteractionID)
            Server_Get_SessionID = True
        Catch ex As Exception
            Write_Log("", "Server_Get_SessionID :Error:" & ex.Message)
            Throw New System.Exception("Server_Get_SessionID () " & ex.Message)
        End Try
    End Function
    Public Function SessionID_Get() As Boolean
        SessionID_Get = False
        Dim dtResult As New DataTable
        Try

            CallInformation.RefID_DB = GetRandomID()

            Dim quertText As String : quertText = ""
            Select Case _myDatabaseType
                Case "access"
                    quertText = quertText & "INSERT INTO TBL_AgentCalls (RowID,RefID_Server,LoginSessionID,Direction,CallerID,CalledID,CustomerID,DateTimeCall,DateTimeAccpeted,DateTimeHangup,isAnswered,isTranfered,isFeedback,isIVR,isVerifyed,CallStatus,CallHangupBy,CallDurationHold, CallDurationACW, CallDurationIVR)"
                    quertText = quertText & " VALUES(" & CallInformation.RefID_DB & "," & CallInformation.RefID_Server & "," & AgentInformation.LoginSessionID & "," & IIf(CallInformation.CallDirection = "Inbound", 0, 1) & ",'" & CallInformation.CallerID & "','" & CallInformation.CalledID & "'," & "NULL" & ",NOW(),NULL,NULL,0,0,0,0,0,'Inbound-Ringing','',0,0,0)"
                Case "mssql"
                    quertText = ""
                    quertText = quertText & "INSERT INTO TBL_AgentCalls (RowID,RefID_Server,LoginSessionID,Direction,CallerID,CalledID,CustomerID,DateTimeCall,DateTimeAccpeted,DateTimeHangup,isAnswered,isTranfered,isFeedback,isIVR,isVerifyed,CallStatus,CallHangupBy,CallDurationHold, CallDurationACW, CallDurationIVR)"
                    quertText = quertText & " VALUES(" & CallInformation.RefID_DB & "," & CallInformation.RefID_Server & "," & AgentInformation.LoginSessionID & "," & IIf(CallInformation.CallDirection = "Inbound", 0, 1) & ",'" & CallInformation.CallerID & "','" & CallInformation.CalledID & "'," & "NULL" & ",GETDATE(),NULL,NULL,0,0,0,0,0,'Inbound-Ringing','',0,0,0)"
                Case Else
                    quertText = ""
                    quertText = quertText & "INSERT INTO TBL_AgentCalls (RowID,RefID_Server,LoginSessionID,Direction,CallerID,CalledID,CustomerID,DateTimeCall,DateTimeAccpeted,DateTimeHangup,isAnswered,isTranfered,isFeedback,isIVR,isVerifyed,CallStatus,CallHangupBy,CallDurationHold, CallDurationACW, CallDurationIVR)"
                    quertText = quertText & " VALUES(" & CallInformation.RefID_DB & "," & CallInformation.RefID_Server & "," & AgentInformation.LoginSessionID & "," & IIf(CallInformation.CallDirection = "Inbound", 0, 1) & ",'" & CallInformation.CallerID & "','" & CallInformation.CalledID & "'," & "NULL" & ",NOW(),NULL,NULL,0,0,0,0,0,'Inbound-Ringing','',0,0,0);"
            End Select

            dtResult = RunDBQuery(quertText)

            'Select Case _myDatabaseType
            'Case "access" : quertText = "select RowID as RefID_DB from TBL_AgentCalls where RowID=" & CallInformation.RefID_DB & ""
            'Case Else : quertText = "select RowID as RefID_DB from TBL_AgentCalls where RowID='" & CallInformation.RefID_DB & "'"
            'End Select
            'dtResult = RunDBQuery(quertText)
            'CallInformation.RefID_DB = -1
            'If dtResult.Rows.Count > 0 Then
            'CallInformation.RefID_DB = "" & dtResult.Rows(0)("RefID_DB")
            'End If

            SessionID_Get = True

        Catch ex As Exception
            Write_Log("", "SessionID_Get:Error:" & ex.Message)
            Throw New System.Exception("SessionID_Get() " & ex.Message)

        End Try
    End Function
    Public Function QueueName_Get(ByVal CallerID As String) As String
        QueueName_Get = ""
        Dim dtResult As New DataTable
        Try

            Write_Log("", "QueueName_Get:Call log loading started")
            Dim TmpQueryString As String
            If _myDatabaseType = "mysql" Then
                TmpQueryString = "select QueueName from TBL_Queues where QueueCode = (select QueueName from TBL_QueueLive where CallerID='" & CallerID & "' limit 1)"
            Else
                TmpQueryString = "select QueueName from TBL_Queues where QueueCode = (select Top 1 QueueName from TBL_QueueLive where CallerID='" & CallerID & "')"
            End If
            Write_Log("", (TmpQueryString))
            dtResult = RunDBQuery(TmpQueryString)
            For Each row In dtResult.Rows
                Write_Log("", "QueueName_Get:Queue Name Found " & row("QueueName"))
                QueueName_Get = "" & row("QueueName")
                Exit For
            Next
        Catch ex As Exception
            Write_Log("", "QueueName_Get:Error:" & ex.Message)
            Throw New System.Exception("QueueName_Get() " & ex.Message)
        End Try
        Write_Log("", "QueueName_Get:Queue Name log loading end")

    End Function

    Private Sub UCallBack_Event_Occured(Sender As Object, e As Class_UCallBack.Event_Occured_Args) Handles UCallBack.Event_Occured
        Try
            Select Case e.ReminderText
                Case "CheckDBConnection"
                    Dim xDateDiff As Long = DateDiff(DateInterval.Second, _myDatabaseSQL, DateTime.Now)
                    Console.WriteLine("DateDiff:" & xDateDiff)
                    If xDateDiff > _myDBCheckTimeout Then
                        _myDatabaseSQL = DateTime.Now()
                        Write_Log("", e.ReminderText & ":Started")
                        Dim TmpQueryString As String
                        If _myDatabaseType = "mysql" Then
                            TmpQueryString = "select NOW() as currentDateTime"
                        Else
                            TmpQueryString = "select GETDATE() as currentDateTime"
                        End If
                        Write_Log("", e.ReminderText & ":Query Started")
                        Dim dtResult As DataTable = RunDBQuery(TmpQueryString, "UCallBack")
                        Write_Log("", e.ReminderText & ":End")
                    End If
                    Console.WriteLine("CheckDBConnection:" & DateTime.Now & ",xDateDiff=" & xDateDiff)
            End Select
        Catch ex As Exception
            Write_Log("ERR", "UCallBack_Event_Occured:Error:" & ex.Message & " ReminderText:" & e.ReminderText)
        End Try
        Try
            UCallBack.SetupEvent(0, "CheckDBConnection", 60000, "", "", "", Nothing)
            Console.WriteLine("CheckDBConnection:_myDatabaseSQL:" & _myDatabaseSQL.ToString() & ", ")
        Catch ex As Exception
        End Try
    End Sub
End Class
