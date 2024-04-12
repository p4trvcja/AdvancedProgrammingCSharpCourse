using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class Server {
    private static string my_dir = Directory.GetCurrentDirectory();

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
        Console.WriteLine("Client connected.");

        while(true) {
            byte[] sizeBuf = new byte[4];
            int received = clientSocket.Receive(sizeBuf, 4, SocketFlags.None);
            received = BitConverter.ToInt32(sizeBuf, 0);

            var buffer = new byte[received];
            int bytesNumber = clientSocket.Receive(buffer, SocketFlags.None);

            string clientMessage = Encoding.UTF8.GetString(buffer, 0, bytesNumber);
            Console.WriteLine("Received from client: " + clientMessage);

            string response = processClientMessage(clientMessage);
            byte[] responseBytes = Encoding.UTF8.GetBytes(response);
            byte[] sizeBytes = BitConverter.GetBytes(responseBytes.Length);
            
            clientSocket.Send(sizeBytes, SocketFlags.None);
            clientSocket.Send(responseBytes, SocketFlags.None);

            if(clientMessage == "!end")
                break;
        }

        try {
            serverSocket.Shutdown(SocketShutdown.Both);
            serverSocket.Close();
        } catch {}
    }

    private static string processClientMessage(string message) {
        string response = "";

        switch(message) {
            case "list":
                response = list();
                break;
            case "!end":
                response = "Server shutting down";
                break;
            default:
                if (message.StartsWith("in")) {
                    try {
                        string sub_dir = message.Substring(3);
                        if (Directory.Exists(sub_dir)) {
                        string curr_dir = my_dir;
                        my_dir = sub_dir;
                        response = list();
                        my_dir = curr_dir;
                    } else {
                        response = "Directory does not exist.";
                    }
                    } catch {
                        response = "Invalid path. Try again";
                        break;
                    }
                } else {
                    response = "Invalid command. Try again";
                }
                break;
        }
        return response;
    }

    private static string list() {
        string[] directories = Directory.GetDirectories(my_dir);
        string[] files = Directory.GetFiles(my_dir);

        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Contents in directory: " + my_dir);
        stringBuilder.AppendLine("Directories: ");
        foreach(string dir in directories)
            stringBuilder.AppendLine("    " + Path.GetFileName(dir));

        stringBuilder.AppendLine("Files: ");
        foreach(string file in files)
            stringBuilder.AppendLine("    " + Path.GetFileName(file));

        return stringBuilder.ToString();
    }

}