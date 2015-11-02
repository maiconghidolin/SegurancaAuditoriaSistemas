Imports System
Imports System.Net
Imports System.Net.Sockets
Imports System.Text

Module Program

    Sub Main(args() As [String])
        Dim s As New Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
        Dim broadcast As IPAddress = IPAddress.Parse("127.0.0.1")
        Dim sendbuf() As Byte = {0, 0, 0}
        Dim ep As New IPEndPoint(broadcast, 32000)
        s.SendTo(sendbuf, ep)
        Console.WriteLine("Message sent to the broadcast address")
    End Sub
End Module