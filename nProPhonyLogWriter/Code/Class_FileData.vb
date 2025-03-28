Friend Class Class_FileData
    Private _FileName As String
    Public Property FileName() As String
        Get
            Return _FileName
        End Get
        Set(ByVal value As String)
            _FileName = value
        End Set
    End Property
    Private _FileData As String
    Public Property FileData() As String
        Get
            Return _FileData
        End Get
        Set(ByVal value As String)
            _FileData = value
        End Set
    End Property

End Class
