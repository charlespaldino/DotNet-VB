Imports System.IO
Imports System.Security.Cryptography
Imports System.Text

Public Class SecurityUtils

    Private Const SALT_MAXLENGTH As Integer = 32
    Private Const ENCRYPTION_KEY As String = "XXXX_password_YYYY"


#Region "Usage"

    ''' <summary>
    ''' This will hash the given input And then encrypt it.
    ''' </summary>
    ''' <returns>The input that has been hashed And encrypted.</returns>
    Public Shared Function secureString(input As String) As String
        Return encrypt(generateHash(input))
    End Function

#End Region


#Region "Security - Salt/Hash/Encrypt"

    ''' <summary>
    ''' Encrypts the given string.
    ''' </summary>
    Public Shared Function encrypt(plaintext As String) As String
        Dim plaintext_bytes As Byte() = Encoding.Unicode.GetBytes(plaintext)
        Dim encryptor As Aes = Aes.Create()
        Using rfc_derivedbytes As New Rfc2898DeriveBytes(ENCRYPTION_KEY,
                                                         New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D, &H65, &H64, &H76, &H65, &H64, &H65, &H76})
            Dim memory_stream As New MemoryStream()

            encryptor.Key = rfc_derivedbytes.GetBytes(32)
            encryptor.IV = rfc_derivedbytes.GetBytes(16)

            Dim crypto_stream As New CryptoStream(memory_stream, encryptor.CreateEncryptor(), CryptoStreamMode.Write)
            crypto_stream.Write(plaintext_bytes, 0, plaintext_bytes.Length)

            crypto_stream.Close()
            memory_stream.Close()
            encryptor.Clear()

            plaintext = Convert.ToBase64String(memory_stream.ToArray())
        End Using

        Return plaintext
    End Function

    ''' <summary>
    ''' Decrypts the given string.
    ''' </summary>
    Public Shared Function decrypt(encrypted_text As String) As String
        encrypted_text = encrypted_text.Replace(" ", "+")

        Dim cipherBytes As Byte() = Convert.FromBase64String(encrypted_text)
        Dim encryptor As Aes = Aes.Create()

        Using rfc_derivedbytes As New Rfc2898DeriveBytes(ENCRYPTION_KEY,
                                                         New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D, &H65, &H64, &H76, &H65, &H64, &H65, &H76})

            Dim memorystream As New MemoryStream()

            encryptor.Key = rfc_derivedbytes.GetBytes(32)
            encryptor.IV = rfc_derivedbytes.GetBytes(16)

            Dim decrypto_stream As New CryptoStream(memorystream, encryptor.CreateDecryptor(), CryptoStreamMode.Write)
            decrypto_stream.Write(cipherBytes, 0, cipherBytes.Length)

            decrypto_stream.Close()
            memorystream.Close()
            encryptor.Clear()

            encrypted_text = Encoding.Unicode.GetString(memorystream.ToArray())
        End Using

        Return encrypted_text
    End Function

    ''' <summary>
    ''' Takes in a string And turns it into a hashed string.
    ''' </summary>
    Public Shared Function generateHash(plaintext As String) As String
        Dim algorithm As HashAlgorithm = SHA256.Create()

        Return Convert.ToBase64String(algorithm.ComputeHash(Encoding.ASCII.GetBytes(plaintext)))
    End Function

    ''' <summary>
    ''' Generates a New salt. Uses RNGCryptoServiceProvider
    ''' </summary>
    Public Shared Function generateSalt() As String
        Dim Random = New RNGCryptoServiceProvider()
        Dim salt(SALT_MAXLENGTH) As Byte

        ' Build the random bytes
        Random.GetNonZeroBytes(salt)

        ' Return the string encoded salt
        Return Convert.ToBase64String(salt)
    End Function
#End Region

#Region "Security - SQL Injection"

    ''' <summary>
    ''' This returns the same string, with various SQL keywords removed.
    ''' Removes " INSERT ", " INTO ", " FROM " , " SELECT ", " * ", " DELETE ", " DROP ", " ALTER "
    ''' </summary>
    Public Shared Function removeSQL(input As String) As String
        input = input.Replace(" INSERT ", "").Replace(" INTO ", "").Replace(" FROM ", "")
        input = input.Replace(" SELECT ", "").Replace(" * ", "").Replace(" DELETE ", "").Replace(" DROP ", "")
        input = input.Replace(" ALTER ", "")

        Return input
    End Function

#End Region

End Class
