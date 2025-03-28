Imports System.ComponentModel

Public Class Form_Video
    Public Function RemoteVideoRefresh() As Boolean
        remoteVideoPanel.Refresh()
        RemoteVideoRefresh = True
    End Function


    Public Function getRemoteVideoHandle() As IntPtr
        getRemoteVideoHandle = remoteVideoPanel.Handle
    End Function


    Public Function getlocalVideoHandle() As IntPtr
        getlocalVideoHandle = localVideoPanel.Handle
    End Function


    Private Sub Form_Video_Resize(sender As Object, e As EventArgs) Handles Me.Resize

        remoteVideoPanel.Width = Me.Width

        If Me.Width < 825 Then
            Me.Width = 825
        End If
        If Me.Height < 650 Then
            Me.Height = 650
        End If

        remoteVideoPanel.Left = 5
        remoteVideoPanel.Top = 5

        If Me.WindowState = WindowState.Maximized Then
            remoteVideoPanel.Width = 1280
            remoteVideoPanel.Height = 720
        Else
            remoteVideoPanel.Width = 800
            remoteVideoPanel.Height = 600
        End If


        'remoteVideoPanel.Left = ((Me.Width - remoteVideoPanel.Width) / 2)
        'remoteVideoPanel.Top = (((Me.Height - remoteVideoPanel.Height) / 2) - 45)
        'Console.WriteLine("Height:" & Me.Height.ToString)
        'Console.WriteLine("Top:" & remoteVideoPanel.Top.ToString)

        localVideoPanel.Left = (remoteVideoPanel.Width - 40) - localVideoPanel.Width
        localVideoPanel.Top = 25 '(Me.Height - 50) - localVideoPanel.Height

    End Sub

    Private Sub Form_Video_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Left = 50
        Me.Top = 50
        Me.WindowState = WindowState.Minimized
    End Sub

    Private Sub Form_Video_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        e.Cancel = True
        Me.Visible = False
    End Sub

    Private Sub remoteVideoPanel_Paint(sender As Object, e As PaintEventArgs) Handles remoteVideoPanel.Paint

    End Sub
End Class