Imports Microsoft.VisualBasic

''' <summary>
''' This is sample code, assumes PurchaseOrder and Product classes exist.
''' </summary>
Public Class FTPManager

    Private Shared LOCAL_TEMP_PATH As String = ConfigurationManager.AppSettings("docspath")
    Private Shared FTP_URL As String = "ftp://xxx.store.com:port/"
    Private Shared USERNAME As String = "USERNAME"
    Private Shared PASSWORD As String = "PASSWORD"
    Private Shared UPLOAD_PATH As String = ""
    Private Shared FILE_NAME As String = "name_MM-DD-YYYY"

    ''' <summary>
    ''' Sends a purchase order to the FTP server.
    ''' </summary>
    Public Shared Sub castFTP(purchase_order As PurchaseOrder)
        'Format file name for today's date.
        Dim filename As New String
        filename = purchase_order.ID & "_" & FILE_NAME
        filename = filename.Replace("MM", If(DateTime.Now.Month < 10, "0" & DateTime.Now.Month, "" & DateTime.Now.Month))
        filename = filename.Replace("DD", If(DateTime.Now.Day < 10, "0" & DateTime.Now.Day, "" & DateTime.Now.Day))
        filename = filename.Replace("YYYY", DateTime.Now.Year)

        'Append the products inside this purchase order.
        Dim data As New StringBuilder()
        For Each product As PurchaseOrderProduct In purchase_order.PurchaseOrderProducts
            data.AppendLine(product.OrderID & ",""" & product.ProductCode & """," & product.QuantityOrdered)
        Next

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
