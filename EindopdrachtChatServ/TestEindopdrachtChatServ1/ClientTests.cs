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

                // Stuur het bericht naar de server
                stream.Write(messageBytes, 0, messageBytes.Length);

                //Delay to process the message
                Thread.Sleep(1000);

                // Hier zou je kunnen verifiëren dat het bericht correct is ontvangen en verwerkt door de server
                // Bijvoorbeeld, controleer of het bericht verschijnt in de lijst met chatberichten op de server.

                // Als het bericht correct wordt verwerkt, slaagt de test.
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
        // Arrange
        int serverPort = 8888;
        string serverMessage = "Hello from the server";

        // Start een simulatie van een server met TcpListener
        TcpListener serverListener = new TcpListener(IPAddress.Loopback, serverPort);
        serverListener.Start();

        // Simuleer een client die verbinding maakt met de server
        TcpClient client = new TcpClient();
        client.Connect(IPAddress.Loopback, serverPort);
        
        // Simuleer het verzenden van een bericht van de server naar de client
        byte[] serverMessageBytes = Encoding.ASCII.GetBytes(serverMessage);
        client.GetStream().Write(serverMessageBytes, 0, serverMessageBytes.Length);

        // Act - Lees het bericht van de client
        TcpClient serverClient = serverListener.AcceptTcpClient();
        byte[] receivedMessageBytes = new byte[1024];
        int bytesRead = serverClient.GetStream().Read(receivedMessageBytes, 0, receivedMessageBytes.Length);
        string receivedMessage = Encoding.ASCII.GetString(receivedMessageBytes, 0, bytesRead);

        // Assert
        Assert.That(serverMessage, Is.EqualTo(receivedMessage));

        // Opruimen
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