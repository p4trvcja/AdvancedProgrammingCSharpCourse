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

         while (true) {
            Console.WriteLine("List of commands:");
            Console.WriteLine("    \"!end\" - terminate both server and client programs");
            Console.WriteLine("    \"list\" - list all directories and files located in the directory specified by the variable \"my_dir\"");
            Console.WriteLine("    \"in [name]\" - list all directories and files located in the directory specified by \"[name]\"");
            Console.Write("> ");

            string? message = Console.ReadLine();
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            byte[] sizeBytes = BitConverter.GetBytes(messageBytes.Length);

            socket.Send(sizeBytes, SocketFlags.None);
            socket.Send(messageBytes, SocketFlags.None);

            byte[] responseSize = new byte[4];
            int received = socket.Receive(responseSize, 4, SocketFlags.None);
            received = BitConverter.ToInt32(responseSize, 0);

            var buffer = new byte[received];
            int bytesNumber = socket.Receive(buffer, SocketFlags.None);

            String serverResponse = Encoding.UTF8.GetString(buffer, 0, bytesNumber);
            Console.WriteLine("Received from server: " + serverResponse);

            if (message == "!end")
                break;
        }

        try {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        } catch{}
    }
}