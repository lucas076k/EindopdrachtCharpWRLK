using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Client;

public partial class MainWindow : Window
{
    private TcpClient client;
    private NetworkStream stream;
    private string clientAddress;

    public MainWindow()
    {
        InitializeComponent();
        client = new TcpClient();
        client.Connect("localhost", 8888);
        stream = client.GetStream();
        clientAddress = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();

        Thread receiveThread = new Thread(ReceiveMessages);
        receiveThread.Start();
    }

    private void ReceiveMessages()
    {
        while (true)
        {
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);

            Dispatcher.Invoke(() =>
            {
                chatListBox.Items.Add(clientAddress + ": " + message);
            });
        }
    }

    private void SendButton_Click(object sender, RoutedEventArgs e)
    {
        string message = messageTextBox.Text;
        byte[] buffer = Encoding.ASCII.GetBytes(message);
        stream.Write(buffer, 0, buffer.Length);

        chatListBox.Items.Add("You: " + message);
        messageTextBox.Clear();
    }
}