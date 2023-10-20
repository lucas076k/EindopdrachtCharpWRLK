using System.Net;
using System.Net.Sockets;

namespace TestEindopdrachtChatServ1;

[TestFixture]
public class ServerTests
{
    private const int Port = 8888;
    private TcpClient testClient;

    [SetUp]
    public void Setup()
    {
        testClient = new TcpClient();
    }

    [TearDown]
    public void TearDown()
    {
        testClient.Close();
    }

    [Test]
    public void ServerStartsSuccessfully()
    {
        try
        {
            TcpListener server = new TcpListener(IPAddress.Any, Port);
            server.Start();

            // Check if the server is actively listening on the specified port.
            Assert.IsTrue(server.Server.IsBound);

            server.Stop();
        }
        catch (Exception ex)
        {
            Assert.Fail("Server failed to start: " + ex.Message);
        }
    }
}