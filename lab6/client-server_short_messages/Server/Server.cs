using System.Net;
using System.Net.Sockets;
using System.Text;

public class Server {
    public static void Main() {
        IPHostEntry host = Dns.GetHostEntry("localhost");

        IPAddress ipAddress = host.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

        Socket serverSocket = new(
            localEndPoint.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp);

        serverSocket.Bind(localEndPoint);
        serverSocket.Listen(100);

        Socket clientSocket = serverSocket.Accept();

        byte[] buffer = new byte[1_024];

        int received = clientSocket.Receive(buffer, SocketFlags.None);

        String clientMessage = Encoding.UTF8.GetString(buffer, 0, received);

        Console.WriteLine("Received from client: " + clientMessage);
        string answer = Console.ReadLine();

        var echoBytes = Encoding.UTF8.GetBytes(answer);
        clientSocket.Send(echoBytes, 0);
        try {
            serverSocket.Shutdown(SocketShutdown.Both);
            serverSocket.Close();
        } catch {}
    }
}