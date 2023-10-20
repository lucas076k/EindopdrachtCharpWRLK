using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using System.Runtime.CompilerServices;


namespace Server;

class Server
{
    private static List<TcpClient> clients = new List<TcpClient>();
    private static TcpListener server;
    private static string filePath = "MessagesFile.txt";

    public static void Main()
    {
        File.WriteAllText(filePath, "Welkom in de chat! \n");
        server = new TcpListener(IPAddress.Any, 8888);
        server.Start();
        Console.WriteLine("Server started succesfully!");

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            clients.Add(client);
            Thread clientThread = new Thread(HandleClient);
            clientThread.Start(client);
        }
    }

    static void HandleClient(object obj)
    {
        TcpClient client = (TcpClient)obj;
        NetworkStream stream = client.GetStream();
        
        string fileContent = File.ReadAllText("MessagesFile.txt");
        byte[] buf = Encoding.ASCII.GetBytes(fileContent);
        stream.Write(buf, 0, buf.Length);

        while (true)
        {
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);

            Console.WriteLine(message);
            File.AppendAllText(filePath, message + "\n");

            foreach (TcpClient c in clients)
            {
                if (c != client)
                {
                    NetworkStream cStream = c.GetStream();
                    cStream.Write(buffer, 0, bytesRead);
                }
            }
        }
    }
}