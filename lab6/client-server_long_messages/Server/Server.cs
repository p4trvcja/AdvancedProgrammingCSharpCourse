using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class Server {
    public static void Main() {
        IPHostEntry host = Dns.GetHostEntry("localhost");

        IPAddress ipAddress = host.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

        Socket serverSocket = new Socket(
            localEndPoint.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp);

        serverSocket.Bind(localEndPoint);
        serverSocket.Listen(100);

        Socket clientSocket = serverSocket.Accept();

        byte[] sizeBuf = new byte[4];
        int received = clientSocket.Receive(sizeBuf, 4, SocketFlags.None);
        received = BitConverter.ToInt32(sizeBuf, 0);

        var buffer = new byte[received];
        int bytesNumber = clientSocket.Receive(buffer, SocketFlags.None);

        string clientMessage = Encoding.UTF8.GetString(buffer, 0, bytesNumber);

        Console.WriteLine("Received from client: " + clientMessage);

        string response = Console.ReadLine();
        byte[] responseBytes = Encoding.UTF8.GetBytes(response);
        byte[] sizeBytes = BitConverter.GetBytes(responseBytes.Length);

        clientSocket.Send(sizeBytes, SocketFlags.None);
        clientSocket.Send(responseBytes, SocketFlags.None);

        try {
            serverSocket.Shutdown(SocketShutdown.Both);
            serverSocket.Close();
        } catch {
            // Handle exceptions properly
        }
    }
}