Public Class PageLock
    Inherits Lock

    Public Sub New(pagename As String, sessionid As String, username As String)
        MyBase.New(pagename, username, DateTime.Now)
        Me.PageName = pagename
        Me.refreshExpirationTimer()
    End Sub

    Public Property PageName As String
    Public Property SessionID As String


End Class
