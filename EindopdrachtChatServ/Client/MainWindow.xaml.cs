using System;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Client
{
    public partial class MainWindow : Window
    {
        private TcpClient client;
        private NetworkStream stream;
        private string clientUsername;
        private Thread receiveThread;
        private String toBeGuessWord;
        private String guessWord;

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
            if (!whiteLine && !message.StartsWith("/game") && !message.StartsWith("/guess"))
            {
                Dispatcher.Invoke(() => { chatListBox.Items.Add("You: " + message); });
                SendToServer(clientUsername + ": " + message);
            }

            if (message.StartsWith("/game"))
            {
                //Hieronder wordt het woord doorgegeven wat geraden moet worden.
                StartWordGame(message.Substring(6));
            }

            if (message.StartsWith("/guess"))
            {
                GuessWord(message.Substring(7));
                
            }
            

            messageTextBox.Clear();
        }

        private void GuessWord(String guessWord)
        {
            this.guessWord = guessWord;
            Dispatcher.Invoke(() => { chatListBox.Items.Add("You guessed: " + this.guessWord); });
            SendToServer(clientUsername + " guessed: " + this.guessWord);
            
            //Hieronder komt de logica zodat de woorden vergeleken kunnen worden.
            ArrayList letters = new ArrayList();
            string correctLetters = "";
            for (int i = 0; i < this.guessWord.Length; i++)
            {
                char letter = this.guessWord[i];
                letters.Add(letter);
            }

            foreach (char listLetter in letters)
            {
                if (toBeGuessWord.Contains(listLetter))
                {
                    correctLetters += + listLetter + " ";
                }
            }
            
            Dispatcher.Invoke(() => { chatListBox.Items.Add("You got letter(s): " + letters); });
            SendToServer(clientUsername + " got letter(s): " + letters);
            
            if (guessWord == toBeGuessWord)
            {
                Dispatcher.Invoke(() => { chatListBox.Items.Add("You won! The word was: " + toBeGuessWord); });
                SendToServer(clientUsername + " guessed: " + toBeGuessWord);
            }
            // else if (guessWord != toBeGuessWord)
            // {
            //     
            // }
        }

        private void StartWordGame(String word)
        {
            toBeGuessWord = word;
            Dispatcher.Invoke(() => { chatListBox.Items.Add("(Only you can see this)\nWord to be guessed: " + toBeGuessWord); });
            
            //Hieronder gaat het woord in letters gesplits worden zodat ze vergeleken kunnen worden.
            
            
            
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