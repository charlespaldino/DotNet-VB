Public MustInherit Class Lock
    Protected Const DEFAULT_TIMER_MINUTES As Integer = 5

    Public Property ID As String
    Public Property UserName As String
    Public Property CreatedAt As DateTime
    Public Property ExpiresAt As DateTime

    Protected Sub New(ID As String, username As String, expires_at As DateTime)
        Me.ID = ID
        Me.UserName = username
        Me.CreatedAt = DateTime.Now
        Me.ExpiresAt = expires_at
    End Sub

    ''' <summary>
    ''' Sets the expiration time to the current time, plus the default minutes.
    ''' </summary>
    Public Sub refreshExpirationTimer()
        ExpiresAt = DateTime.Now
        ExpiresAt.AddMinutes(DEFAULT_TIMER_MINUTES)
    End Sub

    ''' <summary>
    ''' Checks the expiration time against the current time.
    ''' </summary>
    ''' <returns>True, if expired (ExpiresAt is before or equal to DateTime.Now)</returns>
    Public Function isTimerExpired() As Boolean
        Return ExpiresAt <= DateTime.Now
    End Function
End Class
