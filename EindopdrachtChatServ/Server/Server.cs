using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server;

class Server
{
    private static List<TcpClient> clients = new List<TcpClient>();
    private static TcpListener server;

    public static void Main()
    {
        server = new TcpListener(IPAddress.Any, 8888);
        server.Start();
        Console.WriteLine("Server started.");

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
        

        while (true)
        {
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);

            Console.WriteLine(message);

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