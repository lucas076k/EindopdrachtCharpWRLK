using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace ClientChat
{
    public partial class MainWindow : Window
    {
        private TcpClient client;
        private NetworkStream stream;
        private string clientAddress;
        private Thread receiveThread;

        public MainWindow()
        {
            InitializeComponent();
            client = new TcpClient();
            client.Connect("localhost", 8888);
            stream = client.GetStream();
            clientAddress = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();

            receiveThread = new Thread(ReceiveMessages);
            receiveThread.Start();
        }

        private void ReceiveMessages()
        {
            while (true)
            {
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                Dispatcher.Invoke(() => { chatListBox.Items.Add(clientAddress + ": " + message); });
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e) 
        {
            string message = messageTextBox.Text;
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            stream.Write(buffer, 0, buffer.Length);

            // Adds message to ListBox
            Dispatcher.Invoke(() => { chatListBox.Items.Add("You: " + message); });

            // Sends message to other clients
            SendToServer(message);

            messageTextBox.Clear();
        }

        private void SendToServer(string message)
        {
            try
            {
                byte[] buffer = Encoding.ASCII.GetBytes(message);
                stream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                // Handel eventuele uitzonderingen af
                MessageBox.Show("Fout bij het verzenden van bericht naar server: " + ex.Message);
            }
        }
    }
}