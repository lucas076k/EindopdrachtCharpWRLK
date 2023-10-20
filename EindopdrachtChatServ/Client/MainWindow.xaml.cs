using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Client
{
    public partial class MainWindow : Window
    {
        private TcpClient client;
        private NetworkStream stream;
        private string clientUsername;
        private Thread receiveThread;
        
        public MainWindow(string clientUsername)
        {
            InitializeComponent();
            client = new TcpClient();
            client.Connect("localhost", 8888);
            stream = client.GetStream();
            this.clientUsername = clientUsername;;

            receiveThread = new Thread(ReceiveMessages);
            receiveThread.Start();

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string fileContent = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Dispatcher.Invoke(() => { chatListBox.Items.Add(fileContent); });
            
            Dispatcher.Invoke(() => { chatListBox.Items.Add("You joined the chat"); });
            SendToServer(clientUsername + " has joined the chat");
            
        }

        private async void ReceiveMessages()
        {
            while (true)
            {
                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                
                Dispatcher.Invoke(() => { chatListBox.Items.Add(message); });
            }
        }

        private void SendMessage(object sender, RoutedEventArgs e) 
        {
            string message = messageTextBox.Text;
            
            bool whiteLine = string.IsNullOrWhiteSpace(message);
            if (!whiteLine)
            {
                Dispatcher.Invoke(() => { chatListBox.Items.Add("You: " + message); });
                SendToServer(clientUsername + ": " + message);
            }
            

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
                MessageBox.Show("Fout bij het verzenden van bericht naar server: " + ex.Message);
            }
        }

        private void EnterPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage(sender, e);
            }
        }
    }
}