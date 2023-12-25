Imports Microsoft.VisualBasic

''' <summary>
''' This is sample code, assumes various objects exist.
''' </summary>
Public Class LockManager

    ''' <summary>
    ''' Contains a list of IDs and the locks on them.
    ''' </summary>
    Private Shared Property EditLockList As Dictionary(Of String, Lock) = New Dictionary(Of String, Lock)

    ''' <summary>
    ''' Adds this edit lock to the list.
    ''' </summary>
    Public Shared Sub addEditLock(lock As Lock)
        If Not EditLockList.ContainsKey(lock.ID) Then
            EditLockList.Add(lock.ID, lock)
        End If
    End Sub

    ''' <summary>
    ''' Adds this a new edit lock to the list.
    ''' </summary>
    Public Shared Sub addEditLock(page As String, sessionid As String, username As String)
        If Not EditLockList.ContainsKey(page) Then
            EditLockList.Add(page, New PageLock(page, sessionid, username))
        Else
            EditLockList(page).refreshExpirationTimer()
        End If
    End Sub


    ''' <summary>
    ''' Refreshes this edit lock.
    ''' </summary>
    Public Shared Sub refreshEditLock(page As String)
        If EditLockList.ContainsKey(page) Then
            EditLockList(page).refreshExpirationTimer()
        End If
    End Sub

    ''' <summary>
    ''' Removes this edit lock from the list.
    ''' </summary>
    Public Shared Sub removeEditLock(lock As Lock)
        EditLockList.Remove(lock.ID)
    End Sub

    ''' <summary>
    ''' Removes this edit lock from the list based off of the given page.
    ''' </summary>
    Public Shared Sub removeEditLock(page As String)
        EditLockList.Remove(page)
    End Sub

    ''' <summary>
    ''' Removes all edit locks linked to this username.
    ''' </summary>
    Public Shared Sub removeEditLocksByUser(username As String)
        For Each editlock In EditLockList.Values.ToList()
            If editlock.UserName Is Nothing Then
                EditLockList.Remove(editlock.ID)
                Return
            End If

            If editlock.UserName.Equals(username) Then
                EditLockList.Remove(editlock.ID)
            End If
        Next
    End Sub

    ''' <summary>
    ''' Checks if this page has been locked.
    ''' </summary>
    ''' <returns>True, if this page is in the locked list.</returns>
    Public Shared Function isEditLocked(page As String) As Boolean
        Return EditLockList.ContainsKey(page) AndAlso Not EditLockList(page).isTimerExpired()
    End Function

    ''' <summary>
    ''' Checks if this page is locked by the given user.
    ''' </summary>
    ''' <param name="page">The page to check.</param>
    ''' <param name="username">The username to check</param>
    ''' <returns>True, if this page is locked by the given user and not expired.</returns>
    Public Shared Function isEditLockedByUser(page As String, username As String) As Boolean
        If EditLockList.ContainsKey(page) Then
            If EditLockList(page).UserName.Equals(username) Then
                Return True
            Else
                Return EditLockList(page).isTimerExpired()
            End If
        End If

        Return False
    End Function

    ''' <summary>
    ''' Gets the username of the user holding the lock for this page.
    ''' </summary>
    ''' <returns>N/A, if not found.</returns>
    Public Shared Function getEditLockUsername(page As String) As String
        Dim editlock As Lock = EditLockList(page)

        Return If(editlock Is Nothing, "N/A", editlock.UserName)
    End Function

End Class
