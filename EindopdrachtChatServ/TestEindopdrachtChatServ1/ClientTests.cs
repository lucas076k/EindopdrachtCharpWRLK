using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TestEindopdrachtChatServ1;

public class ClientTests
{
    private TcpListener server;
    
    [SetUp]
    public void SetUp()
    {
        server = new TcpListener(IPAddress.Any, 8888);
        server.Start();
    }
    
    [Test]
    public void SendMessageToServer()
    {
        try
        {
            using (TcpClient client = new TcpClient("localhost", 8888))
            using (NetworkStream stream = client.GetStream())
            {
                string message = "Test message";
                byte[] messageBytes = Encoding.ASCII.GetBytes(message);
                
                stream.Write(messageBytes, 0, messageBytes.Length);

                //Delay to process the message
                Thread.Sleep(1000);
            }
        }
        catch (Exception ex)
        {
            Assert.True(false, "Fout bij het verzenden van bericht naar server: " + ex.Message);
        }
    }
    
    [Test]
    public void ReceiveMessagesFromServer()
    {
        int serverPort = 8888;
        string serverMessage = "Hello from the server";
        
        TcpListener serverListener = new TcpListener(IPAddress.Loopback, serverPort);
        serverListener.Start();
        
        TcpClient client = new TcpClient();
        client.Connect(IPAddress.Loopback, serverPort);
        
        byte[] serverMessageBytes = Encoding.ASCII.GetBytes(serverMessage);
        client.GetStream().Write(serverMessageBytes, 0, serverMessageBytes.Length);
        
        TcpClient serverClient = serverListener.AcceptTcpClient();
        byte[] receivedMessageBytes = new byte[1024];
        int bytesRead = serverClient.GetStream().Read(receivedMessageBytes, 0, receivedMessageBytes.Length);
        string receivedMessage = Encoding.ASCII.GetString(receivedMessageBytes, 0, bytesRead);
        
        Assert.That(serverMessage, Is.EqualTo(receivedMessage));
        
        serverClient.Close();
        client.Close();
        serverListener.Stop();
    }
    
    [TearDown]
    public void TearDown()
    {
        server.Stop();
    }
}