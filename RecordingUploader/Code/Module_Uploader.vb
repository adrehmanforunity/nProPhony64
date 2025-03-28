Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.WebRequestMethods
Imports System.Reflection
Imports System.Text
Imports System.Timers


Module Module_Uploader

    Dim arrFilename As String()

    Private MyODBCConnectionToMSSQL As SqlConnection

    Private _myDatabaseType As String = "mssql"
    Private _myDatabaseIP As String
    Private _myDBCheckTimeout As Integer

    Private _myDatabasePort As Integer
    Private _myDatabaseName As String
    Private _myDatabaseUser As String
    Private _myDatabasePassword As String
    Private _myConnectionString As String

    Private _myDatabaseConnectionTimeOut As Integer
    Private _myDatabaseQueryTimeout As Integer

    Public sFolderToScan As String
    Private Declare Auto Function GetPrivateProfileString Lib "kernel32" (ByVal lpAppName As String,
           ByVal lpKeyName As String,
           ByVal lpDefault As String,
           ByVal lpReturnedString As StringBuilder,
           ByVal nSize As Integer,
           ByVal lpFileName As String) As Integer
    Public Function OpenDatabase(ByRef err) As Boolean
        OpenDatabase = False
        err = ""
        Try
            Select Case _myDatabaseType
                Case "mssql" : OpenDatabase = OpenDBMSSql(err)
            End Select
        Catch ex As Exception
            Write_App_Log("Uploader - OpenDatabase:Here to Open Database Connection Exception:" & ex.Message)
        End Try
    End Function
    Private Function OpenDBMSSql(ByRef err As String) As Boolean
        OpenDBMSSql = False
        Try

            Dim strConnection As String = "Data Source=" & _myDatabaseIP & ";Connection Timeout=" & _myDatabaseConnectionTimeOut & ";Initial Catalog=" & _myDatabaseName & ";User ID=" & _myDatabaseUser & ";Password=" & _myDatabasePassword & ";"

            If _myConnectionString <> "" Then
                strConnection = _myConnectionString
            End If

            MyODBCConnectionToMSSQL = New SqlConnection(strConnection)

            Try
                Write_App_Log("Uploader - OpenDBMSSql:Here to Open Database Connection Again State:" & MyODBCConnectionToMSSQL.State)
                MyODBCConnectionToMSSQL.Close()
                Application.DoEvents()
                Write_App_Log("Uploader - OpenDBMSSql:Here to Open Database Connection Again New State:" & MyODBCConnectionToMSSQL.State)
                'MyODBCConnectionToMSSQL = Nothing
            Catch ex As Exception
                Write_App_Log("Uploader - OpenDBMSSql:Here to Open Database Connection Exception:" & ex.Message)
            End Try
            Application.DoEvents()

            If MyODBCConnectionToMSSQL.State <> ConnectionState.Open Then
                MyODBCConnectionToMSSQL.Open()
            End If
            OpenDBMSSql = True
        Catch ex As Exception
            err = ex.Message
            Write_App_Log("Uploader - OpenDBMSSql:Here to Open Database Connection Exception:" & ex.Message)
        End Try
    End Function

    Public Function GetINIStringOldWay(sHeader As String, sItem As String, sDefault As String) As String
        Try
            Dim sb As StringBuilder
            sb = New StringBuilder(500)
            Dim location = Assembly.GetExecutingAssembly().Location
            Dim appPath = Path.GetDirectoryName(location)       ' C:\Some\Directory
            Dim appName = Path.GetFileName(location)            ' MyLibrary.DLL

            Dim iniFileName As String = appPath & "\ProPhony.ini"
            Dim res As Integer = GetPrivateProfileString(sHeader, sItem, sDefault, sb, sb.Capacity, iniFileName)
            GetINIStringOldWay = sb.ToString()
        Catch ex As Exception
            GetINIStringOldWay = ""
        End Try
    End Function
    Public Sub Write_App_Log(ActivityText As String)
        Dim _FileName As String = Path.GetTempPath & "SoftPhone\" & DateTime.Now.ToString("yyyyMMdd_UPL") & String.Format(".Log", DateTime.Today.ToString("yyyyMMdd"))
        Write_TO_File(_FileName, DateTime.Now.ToString("HH:mm:ss") & " " & ActivityText)
    End Sub
    Public Sub Write_TO_File(FileName As String, Text As String)
        Try
            Using writer As New StreamWriter(FileName, True)
                writer.WriteLine(Text)
            End Using
        Catch ex As Exception

        End Try
    End Sub
    Private Function GetRND() As String
        Static Random_Number As New Random()
        Return Random_Number.Next(10000, 99999).ToString
    End Function

    Private Sub ScanWAVFiles()
        Try
            arrFilename = Directory.GetFiles(Path.GetTempPath & "SoftPhone", "*.wav")
            Write_App_Log("Uploader - ScanWAVFiles() File(s) Found :" & arrFilename.Length)
            If arrFilename.Length > 0 Then
                ProcessFiles()
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub ProcessFiles()
        Try



            'Dim strInFile As String = ""
            'Dim strOutFile As String = ""
            Dim LoopCount As Integer = 0

            For LoopCount = 0 To arrFilename.Length - 1
                Try
                    Write_App_Log("Uploader - ProcessFiles() File Found : Starting : " & arrFilename(LoopCount))
                    If System.IO.File.Exists(arrFilename(LoopCount)) = False Then
                        Write_App_Log("Uploader - ProcessFiles() File not exist : exit : " & arrFilename(LoopCount))
                        GoTo skipThisFile
                    End If
                    Dim fileModifiedDateTime As DateTime = System.IO.File.GetLastWriteTime(arrFilename(LoopCount))
                    Dim fileModifiedDifference As Long = 0
                    fileModifiedDifference = DateDiff(DateInterval.Second, fileModifiedDateTime, Now)
                    If DateDiff(DateInterval.Second, fileModifiedDateTime, Now) < 10 Then
                        Write_App_Log("Uploader - ProcessFiles() File is to latest : exit : " & arrFilename(LoopCount) & ", " & fileModifiedDateTime & ", " & fileModifiedDifference)
                        GoTo skipThisFile
                    End If

                    Dim fs As FileStream
                    Dim br As BinaryReader

                    Dim info As New FileInfo(arrFilename(LoopCount))
                    Dim WavFile(CInt(info.Length)) As Byte

                    Try
                        fs = New FileStream(arrFilename(LoopCount), FileMode.Open, FileAccess.Read)
                        br = New BinaryReader(fs)
                        WavFile = br.ReadBytes(CInt(info.Length))
                        Dim intWavLength As Integer = Int(CType(WavFile.Length, Integer))
                    Finally
                        If Not (br Is Nothing) Then br.Close()
                        If Not (fs Is Nothing) Then fs.Close()
                    End Try

                    Dim RefID_DB() As String = Split(System.IO.Path.GetFileName(arrFilename(LoopCount)), "-")
                    If Len(RefID_DB(0)) > 5 Then
                        UpdateNow(WavFile, RefID_DB(0))
                        Write_App_Log("Uploader - ProcessFiles() File Uploaded : " & arrFilename(LoopCount))
                        System.IO.File.Delete(arrFilename(LoopCount))
                    Else
                        Write_App_Log("Uploader - ProcessFiles() File Skip: " & RefID_DB(0))
                    End If
                Catch exx As Exception
                    Write_App_Log("Uploader - ProcessFiles(exx) Exception: " & exx.Message & ", FileName:" & arrFilename(LoopCount))
                End Try
skipThisFile:
                Application.DoEvents()
                Threading.Thread.Sleep(1000)
            Next
            'Write_App_Log("Uploader - ProcessFiles(ex) Sleeping: " & DateTime.Now.ToString)
            'Threading.Thread.Sleep(10000)
            'Write_App_Log("Uploader - ProcessFiles(ex) Awake: " & DateTime.Now.ToString)
            End
        Catch ex As Exception
            Write_App_Log("Uploader - ProcessFiles(ex) Exception: " & ex.Message)
            End
        End Try

    End Sub
    Public Sub UpdateNow(ByVal ByteArr() As Byte, RefID_DB As String)

        Dim RowID As String = Now.ToString("yyyyMMddHHffff") & GetRND()
        Dim strSQL As String

        Dim oCmd As SqlCommand
        Dim oBLOBParam As SqlParameter

        Try
            strSQL = "INSERT INTO TBL_CallsData (RowID,RefID_DB,InsertMoment,CallData) VALUES (@RowID,@RefID_DB, @InsertMoment, @CallData)"
            oCmd = MyODBCConnectionToMSSQL.CreateCommand()
            oCmd.CommandText = strSQL
            oBLOBParam = New SqlParameter("@CallData", SqlDbType.Binary, ByteArr.Length, ParameterDirection.Input.ToString)
            oBLOBParam.Value = ByteArr
            oCmd.Parameters.AddWithValue("@RowID", RowID)
            oCmd.Parameters.AddWithValue("@RefID_DB", RefID_DB)
            oCmd.Parameters.AddWithValue("@InsertMoment", Now.ToString("yyyy-MM-dd HH:mm:ss"))
            oCmd.Parameters.Add(oBLOBParam)
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Write_App_Log("Uploader - UpdateNow() Exception:" & ex.Message)
        End Try
    End Sub
    Sub TimerElapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
        sFolderToScan = System.IO.Path.GetTempPath & "SoftPhone"
        Try

            arrFilename = Directory.GetFiles(sFolderToScan, "*.wav")
            If arrFilename.Length > 0 Then

                _myDatabaseType = "mssql"
                _myDatabasePort = "1443"

                _myDatabaseIP = LCase(GetINIStringOldWay("Database", "DBComServer", "192.168.18.90"))
                _myDatabasePort = Int(GetINIStringOldWay("Database", "DatabasePort", _myDatabasePort))
                _myDatabaseName = GetINIStringOldWay("Database", "Database", "ProPhonyV3")
                _myDatabaseUser = GetINIStringOldWay("Database", "UserID", "client")
                _myDatabasePassword = GetINIStringOldWay("Database", "Password", "client1")
                _myConnectionString = Trim(GetINIStringOldWay("Database", "ConnectionString", ""))

                Dim err As String = ""
                If OpenDatabase(err) = False Then
                    Write_App_Log("Uploader - Main:" & err)
                    End
                End If
                Write_App_Log("Uploader - Main:" & "Going in")
                ScanWAVFiles()
                Threading.Thread.Sleep(10000)
                Write_App_Log("Uploader - Main:" & "Going in")

                Try
                    MyODBCConnectionToMSSQL.Close()
                Catch ex As Exception
                End Try
            Else
                Write_App_Log("Uploader - Main:" & "No file(s) found")
                End
            End If
        Catch ex As Exception
            Write_App_Log("Uploader - Main() Exception:" & ex.Message)
        End Try
    End Sub
    Sub Main()
        TimerElapsed(Nothing, Nothing)
        'Dim timer As Timer = New Timer(200)
        'AddHandler timer.Elapsed, New ElapsedEventHandler(AddressOf TimerElapsed)
        'Timer.Start()



    End Sub
End Module
