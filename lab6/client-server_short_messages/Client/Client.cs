using System.Net;
using System.Net.Sockets;
using System.Text;

public class Client {
    public static void Main() {
        IPHostEntry host = Dns.GetHostEntry("localhost");

        IPAddress ipAddress = host.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

        Socket socket = new(
            localEndPoint.AddressFamily, 
            SocketType.Stream, 
            ProtocolType.Tcp);

        socket.Connect(localEndPoint);

        string message = Console.ReadLine();
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);

        socket.Send(messageBytes, SocketFlags.None);

        var buffer = new byte[1_024];
        int bytesNumber = socket.Receive(buffer, SocketFlags.None);

        String serverAnswer = Encoding.UTF8.GetString(buffer, 0, bytesNumber);
        Console.WriteLine("Received from server: " + serverAnswer);
        try {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        } catch{}
    }
}