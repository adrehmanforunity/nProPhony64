Imports Alchemy.Classes
Imports System
Imports System.Timers
Public Class Connection
    Public Property Context As UserContext
    Public Sub New()
    End Sub
    Public Sub wssend(data As String)
        Try
            Context.Send(data)
        Catch ex As Exception
            Throw New System.Exception("wssend(Exception) " & ex.Message)
        End Try
    End Sub

    Protected Overrides Sub Finalize()
        Try
            Context = Nothing
        Catch ex As Exception
        End Try
        MyBase.Finalize()
    End Sub
End Class