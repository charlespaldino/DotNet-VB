Imports System.IO
Imports System.Net
Imports System.Text
Imports Microsoft.VisualBasic

''' <summary>
''' This is sample code, assumes PurchaseOrder and Product classes exist.
''' </summary>
Public Class FTPManager

    Public Shared Property LOCAL_TEMP_PATH As String = "C:\temp"
    Public Shared Property FTP_URL As String = "ftp://xxx.name.com:port/"
    Private Shared Property USERNAME As String = "USERNAME"
    Private Shared Property PASSWORD As String = "PASSWORD"
    Public Shared Property UPLOAD_PATH As String = ""
    Public Shared Property FILE_NAME As String = "name_MM-DD-YYYY"

    ''' <summary>
    ''' Sends an object to the FTP server in csv format.
    ''' Uses the ID/Name and file_name template.
    ''' </summary>
    Public Shared Sub castFTP(ftp_object As Object)
        Dim id As String = ReflectionUtils.getPropertyValueByName("ID", ftp_object)

        If String.IsNullOrEmpty(id) Then
            id = ReflectionUtils.getPropertyValueByName("Name", ftp_object)
        End If

        'Format file name for today's date.
        Dim filename As String = id & "_" & FILE_NAME
        filename = filename.Replace("MM", If(DateTime.Now.Month < 10, "0" & DateTime.Now.Month, "" & DateTime.Now.Month))
        filename = filename.Replace("DD", If(DateTime.Now.Day < 10, "0" & DateTime.Now.Day, "" & DateTime.Now.Day))
        filename = filename.Replace("YYYY", DateTime.Now.Year)

        'Append the object details.
        Dim details As Dictionary(Of String, String) = ReflectionUtils.getPropertyValues(ftp_object)
        Dim data As New StringBuilder()

        details.Keys.ToList().ForEach(Sub(key)
                                          data.Append(key & ",")
                                      End Sub)
        data.Remove(data.Length - 1, 1) 'Trims last ,

        details.Values.ToList().ForEach(Sub(value)
                                            data.Append(value & ",")
                                        End Sub)
        data.Remove(data.Length - 1, 1)

        'Write to file, send off, then clean up.
        Dim stream_writer As StreamWriter = New StreamWriter(File.Create(LOCAL_TEMP_PATH & filename))
        stream_writer.WriteLine(data.ToString())
        stream_writer.Close()

        Using client As New WebClient()
            client.Credentials = New NetworkCredential(USERNAME, PASSWORD)
            client.UploadFile(FTP_URL + filename, "STOR", LOCAL_TEMP_PATH & filename)
        End Using

        File.Delete(LOCAL_TEMP_PATH & filename)

    End Sub


End Class
