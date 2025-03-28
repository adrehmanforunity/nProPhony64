Public Class Form_AudioSetup
    Private Sub Button_Apply_Click(sender As Object, e As EventArgs) Handles Button_Apply.Click
        Try
            Write_Log("", "Button_Apply(Start) nLine:" & CallInformation.nLine)
            Dim MicIndex As Integer = ComboBox_Mic.SelectedIndex
            Dim SpeakerIndex As Integer = ComboBox_Speaker.SelectedIndex
            If (MicIndex <> -1 Or SpeakerIndex <> -1) Then
                Form_Splash.ApplyAudioChanges(MicIndex, SpeakerIndex)

            Else
                Label_msg.Text = "please make a selection in each of above boxes"
            End If
            Write_Log("", "Button_Apply(End) MicIndex:" & MicIndex & ",SpeakerIndex=" & SpeakerIndex)
        Catch ex As Exception
            Write_Log("", "Button_Apply(Error) nLine:" & CallInformation.nLine & "" & ex.Message)
            MessageBox.Show(Me, "Button_Apply(Error) nLine:" & CallInformation.nLine & "" & ex.Message)
        End Try

    End Sub

    Private Sub Form_AudioSetup_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Write_Log("", "AudioSetup_Load(Start) nLine:" & CallInformation.nLine)

            Write_Log("", "AudioSetup_Load(End) nLine:" & CallInformation.nLine)
        Catch ex As Exception
            Write_Log("", "AudioSetup_Load(Error) nLine:" & CallInformation.nLine & "" & ex.Message)
            MessageBox.Show(Me, "AudioSetup_Load(Error) nLine:" & CallInformation.nLine & "" & ex.Message)
        End Try

    End Sub

End Class