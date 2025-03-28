Imports System.IO
Imports System.Threading
Imports System.Windows.Forms
Public Class Class_FileWriter

    Private _logDir As String = System.IO.Path.GetTempPath
    Public Property LogDir() As String
        Get
            Return _logDir
        End Get
        Set(ByVal value As String)
            _logDir = value
        End Set
    End Property
    Private _XFileName As String = _logDir & "\" & String.Format("{0}_{1}_APP.txt", Now.ToString("yyyyMMddHH"), "XXX", ".log")
    Private logQueue As New Queue(Of Class_FileData)
    Private ThreadFileIO As Thread
    Private ThreadFileIOCreate As Boolean = False
    Public Sub New(Optional sLogDir As String = "")
        Try
            _logDir = System.IO.Path.GetTempPath
            ThreadFileIOCreate = False
            If Trim(sLogDir) = "" Then
                _logDir = System.IO.Path.GetTempPath
            ElseIf Directory.Exists(sLogDir) Then
                _logDir = sLogDir
            End If
            _XFileName = _logDir & "\" & String.Format("{0}_{1}_APP.txt", Now.ToString("yyyyMMddHH"), "XXX", ".log")

            Write_TO_FileX(_XFileName, "-1".ToString.PadLeft(3, "0") & " " & DateTime.Now.ToString("HH:mm:ss:ffff").ToString.PadLeft(13, " ") & ", Class_FileWriter New() IOThread=" & True)
            logQueue = New Queue(Of Class_FileData)
            ThreadFileIO = New Thread(Sub() thFileWriter())
            ThreadFileIO.IsBackground = True
            ThreadFileIO.Name = "nProPhone File Writing Process"
            ThreadFileIO.Start()
            ThreadFileIOCreate = True
            Write_TO_FileX(_XFileName, "-1".ToString.PadLeft(3, "0") & " " & DateTime.Now.ToString("HH:mm:ss:ffff").ToString.PadLeft(13, " ") & ", Write_TO_File() Started ThreadFileIO id:" & ThreadFileIO.ManagedThreadId & ", name:" & ThreadFileIO.Name)
        Catch ex As Exception
            Write_TO_FileX(_XFileName, "-1".ToString.PadLeft(3, "0") & " " & DateTime.Now.ToString("HH:mm:ss:ffff").ToString.PadLeft(13, " ") & ", Class_FileWriter New() Exception=" & ex.Message.ToString())
        End Try
    End Sub

    Public Sub Write_TO_File(FileName As String, Text As String)

        Try

            If IsNothing(ThreadFileIO) Then
                Write_TO_FileX(FileName, Text)
            Else
                If Trim(FileName) <> "" Then
                    If Trim(Text) <> "" Then
                        Dim x As New Class_FileData
                        x.FileName = FileName
                        x.FileData = Text
                        logQueue.Enqueue(x)
                    End If
                End If
            End If
            Exit Sub
        Catch ex As Exception
            Console.WriteLine("Write_TO_File:Error:" & ex.Message)
            _XFileName = _logDir & "\" & String.Format("{0}_{1}_APP.txt", Now.ToString("yyyyMMddHH"), "XXX", ".log")
            Write_TO_FileX(_XFileName, "-1".ToString.PadLeft(3, "0") & " " & DateTime.Now.ToString("HH:mm:ss:ffff").ToString.PadLeft(13, " ") & ", Write_TO_File() Error:" & ex.Message)
        End Try
    End Sub

    Private Sub thFileWriter()

        Dim xCount As Integer = -1
        Dim xErrorLevel As String = "-1"

        Dim FileName As String = ""
        Dim QueueCountNow As String = ""
        Dim FileData As String = ""

        Dim xQueueData As New Class_FileData
        Try

            Write_TO_FileX(_XFileName, DateTime.Now.ToString("HH:mm:ss:ffff").ToString.PadLeft(13, " ") & " -1".ToString.PadLeft(3, "0") & ", thFileWriter() Starting")

            xErrorLevel = "1"
            While True
                xCount = 0
                xErrorLevel = "2"
                xCount = logQueue.Count
                If (xCount) > 0 Then
                    FileName = ""
                    QueueCountNow = ""
                    FileData = ""
                    xErrorLevel = "3"
                    SyncLock (logQueue)
                        Try
                            xErrorLevel = "4.1"
                            xQueueData = logQueue.Dequeue
                            xErrorLevel = "4.2"
                            FileName = xQueueData.FileName
                            xErrorLevel = "4.3"
                            QueueCountNow = xCount.ToString.PadLeft(5, "0")
                            xErrorLevel = "4.4"
                            FileData = xQueueData.FileData
                            xErrorLevel = "4.5"
                            Write_TO_FileX(FileName, QueueCountNow & " - " & FileData)
                            xErrorLevel = "4.6"
                        Catch ex As Exception
                            'Write_TO_FileX(_XFileName, DateTime.Now.ToString("HH:mm:ss:ffff").ToString.PadLeft(13, " ") & " -1".ToString.PadLeft(3, "0") & ", thFileWriter(" & xCount & ",LL:" & xErrorLevel & ") inside Exception:" & ex.Message & " - [FileName" & FileName & ", QueueCountNow = " & QueueCountNow & ", FileData=" & FileData & "]")
                            Application.DoEvents()
                            Thread.Sleep(20)
                        End Try
                    End SyncLock
                    xErrorLevel = "7"
                End If
                Application.DoEvents()
                Thread.Sleep(100)
            End While
            _XFileName = _logDir & "\" & String.Format("{0}_{1}_ERR.txt", Now.ToString("yyyyMMddHH"), "XXX", ".log")
            Write_TO_FileX(_XFileName, DateTime.Now.ToString("HH:mm:ss:ffff").ToString.PadLeft(13, " ") & " -1".ToString.PadLeft(3, "0") & ", thFileWriter() Exiting")
        Catch ex As Exception
            _XFileName = _logDir & "\" & String.Format("{0}_{1}_ERR.txt", Now.ToString("yyyyMMddHH"), "XXX", ".log")
            Write_TO_FileX(_XFileName, DateTime.Now.ToString("HH:mm:ss:ffff").ToString.PadLeft(13, " ") & " -1".ToString.PadLeft(3, "0") & ", thFileWriter(" & xCount & ",LL:" & xErrorLevel & ") inside Exception:" & ex.Message)
            Console.WriteLine("thFileWriter:Error:" + ex.Message)
        End Try

    End Sub
    'Private Sub Write_TO_FileX(FileName As String, Text As String)

    '    Try
    '        Dim _FileStreamWriter = New StreamWriter(FileName, True, System.Text.Encoding.ASCII)
    '        _FileStreamWriter.WriteLine(Text)
    '        _FileStreamWriter.Close()
    '    Catch ex As Exception
    '        Try
    '            Dim _FileStreamWriter = New StreamWriter(FileName & ".01." & "Exception", True, System.Text.Encoding.ASCII)
    '            _FileStreamWriter.WriteLine("Ex:" & ex.Message & " - " & Text)
    '            _FileStreamWriter.Close()
    '        Catch exx As Exception
    '            Try
    '                Dim _FileStreamWriter = New StreamWriter(FileName & ".02." & "Exception", True, System.Text.Encoding.ASCII)
    '                _FileStreamWriter.WriteLine("Ex:" & exx.Message & " - " & Text)
    '                _FileStreamWriter.Close()
    '            Catch exy As Exception
    '                Try
    '                    Dim _FileStreamWriter = New StreamWriter(FileName & ".03." & "Exception", True, System.Text.Encoding.ASCII)
    '                    _FileStreamWriter.WriteLine("Ex:" & exy.Message & " - " & Text)
    '                    _FileStreamWriter.Close()
    '                Catch exz As Exception
    '                    Try
    '                        Dim _FileStreamWriter = New StreamWriter(FileName & ".04." & "Exception", True, System.Text.Encoding.ASCII)
    '                        _FileStreamWriter.WriteLine("Ex:" & exz.Message & " - " & Text)
    '                        _FileStreamWriter.Close()

    '                    Catch exZA As Exception
    '                        Dim _FileStreamWriter = New StreamWriter(FileName & ".05." & "Exception", True, System.Text.Encoding.ASCII)
    '                        _FileStreamWriter.WriteLine("Ex:" & exZA.Message & " - " & Text)
    '                        _FileStreamWriter.Close()
    '                    End Try
    '                End Try
    '            End Try
    '        End Try
    '    End Try
    'End Sub
    Private Sub Write_TO_FileX(FileName As String, Text As String)

        Dim maxTry As Integer = 9

        Dim exceptionText As String = ""
        Dim nFileName As String = FileName
        Dim nFileData As String = Text

        For tryNo = 1 To maxTry
            Try
                If tryNo > 1 Then
                    nFileName = FileName & "." & (tryNo - 1).ToString.PadLeft(2, "0") & ".Exception"
                    nFileData = "Ex:" & exceptionText & " - " & Text
                End If
                Dim _FileStreamWriter = New StreamWriter(nFileName, True, System.Text.Encoding.ASCII)
                _FileStreamWriter.WriteLine(nFileData)
                _FileStreamWriter.Close()
                Exit For
            Catch ex As Exception
                exceptionText = ex.Message
                If tryNo > maxTry Then
                    Console.WriteLine("Write_TO_File(tryNo=" & tryNo & ") Exception:" & exceptionText)
                End If
            End Try
            If tryNo > 1 Then
                Application.DoEvents()
                Thread.Sleep(20)
            End If
        Next
    End Sub
    Protected Overrides Sub Finalize()
        Try
            _XFileName = _logDir & "\" & String.Format("{0}_{1}_APP.txt", Now.ToString("yyyyMMddHH"), "XXX", ".log")
            Write_TO_FileX(_XFileName, "-1".ToString.PadLeft(3, "0") & ", Class_FileWriter Finalize() IOThread=" & True)
        Catch ex As Exception
        End Try
        MyBase.Finalize()
    End Sub
End Class
