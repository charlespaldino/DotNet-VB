# .Net-VB
Code repo for .Net VB related code samples

Skills Demonstrated:
.Net VB
FTP
FILE IO
Salt, Hash, and Encryption.


Classes:
=FTPManager
Handles sending an order and its products to an FTP server, for the end client to process.

=LockManager
Handles locking down pages on systems with concurrent users. 
This is a tool that can tell users the page is currently in use and can not be edited or viewed.
This assumes there is a Lock object that stores the page, sessionid, and username.

=Security Utils
I pulled a guide from online and tooled up salting, hashing, and encryption for use on sensitive data.
For ease of use, there is one main entry method, with exposed helped methods.
This also includes a basic tool to help clean any SQL Injection.
