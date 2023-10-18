using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Net.Sockets;
using System.ComponentModel;

namespace CSharpLucasWesselEindOpdracht
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        
        private char playerChar;
        private char oppenentChar;
        private Socket socket;
        private BackgroundWorker worker = new BackgroundWorker();
        private TcpListener server;
        private TcpClient client;

        public Game(bool isHost, string ip = null)
        {
            InitializeComponent();
            worker.DoWork += Worker_DoWork;

            if (isHost)
            {
                playerChar = 'X'; 
                oppenentChar = 'O';
                server = new TcpListener(System.Net.IPAddress.Any, 123);
                server.Start();
                socket = server.AcceptSocket();
            }
            else
            {
                playerChar = 'O';
                oppenentChar = 'X';

                try
                {
                    client = new TcpClient(ip, 123);
                    socket = client.Client;
                    worker.RunWorkerAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Close();
                }
            }
        }

        private void Worker_DoWork(object? sender, DoWorkEventArgs e)
        {
            if (Winner()) return;
            DisableButtons();
            label1.Content = "Opponent's turn";
            GetOppenentMove();
            if (!Winner()) EnableEmptyButtons(); label1.Content = "Your turn";
        }

        private bool Winner()
        {
            if(button1.Content == button2.Content && button1.Content == button3.Content && !button1.Content.Equals(""))
            {
                if (button1.Content.Equals(playerChar)) WinGame();

                else LoseGame();
                
                return true;
            }

            else if(button4.Content == button5.Content && button4.Content == button6.Content && !button4.Content.Equals(""))
            {
                if (button1.Content.Equals(playerChar)) WinGame();

                else LoseGame();

                return true;
            }

            else if(button7.Content == button8.Content && button7.Content == button9.Content && !button7.Content.Equals(""))
            {
                if (button1.Content.Equals(playerChar)) WinGame();

                else LoseGame();

                return true;
            }

            else if (button1.Content == button4.Content && button1.Content == button7.Content && !button1.Content.Equals(""))
            {
                if (button1.Content.Equals(playerChar)) WinGame();

                else LoseGame();

                return true;
            }

            else if (button2.Content == button5.Content && button2.Content == button8.Content && !button2.Content.Equals(""))
            {
                if (button1.Content.Equals(playerChar)) WinGame();

                else LoseGame();

                return true;
            }

            else if (button3.Content == button6.Content && button3.Content == button9.Content && !button3.Content.Equals(""))
            {
                if (button1.Content.Equals(playerChar)) WinGame();

                else LoseGame();

                return true;
            }

            else if (button1.Content == button5.Content && button1.Content == button9.Content && !button1.Content.Equals(""))
            {
                if (button1.Content.Equals(playerChar)) WinGame();

                else LoseGame();

                return true;
            }

            else if (button3.Content == button5.Content && button3.Content == button7.Content && !button3.Content.Equals(""))
            {
                if (button1.Content.Equals(playerChar)) WinGame();

                else LoseGame();

                return true;
            }

            // draw
            else if (!button1.Content.Equals("") && !button2.Content.Equals("") && !button3.Content.Equals("") && !button4.Content.Equals("") && 
                !button5.Content.Equals("") && !button6.Content.Equals("") && !button7.Content.Equals("") && !button8.Content.Equals("") && !button9.Content.Equals(""))
            {
                DrawGame();

                return true;
            }

            return false;
        }

        private void WinGame()
        {
            label1.Content = "You won!";
        }

        private void LoseGame()
        {
            label1.Content = "You lost :(";
        }

        private void DrawGame()
        {
            label1.Content = "Draw!";
        }

        private void DisableButtons()
        {
            button1.IsEnabled = false;
            button2.IsEnabled = false;
            button3.IsEnabled = false;
            button4.IsEnabled = false;
            button5.IsEnabled = false;
            button6.IsEnabled = false;
            button7.IsEnabled = false;
            button8.IsEnabled = false;
            button9.IsEnabled = false;
        }

        private void EnableEmptyButtons()
        {
            if (button1.Equals("")) button1.IsEnabled = true;
            if (button2.Equals("")) button2.IsEnabled = true;
            if (button3.Equals("")) button3.IsEnabled = true;
            if (button4.Equals("")) button4.IsEnabled = true;
            if (button5.Equals("")) button5.IsEnabled = true;
            if (button6.Equals("")) button6.IsEnabled = true;
            if (button7.Equals("")) button7.IsEnabled = true;
            if (button8.Equals("")) button8.IsEnabled = true;
            if (button9.Equals("")) button9.IsEnabled = true;
        }

        private void GetOppenentMove()
        {
            byte[] buttonByte = new byte[1];
            socket.Receive(buttonByte);

            if (buttonByte[0] == 1) button1.Content = oppenentChar;
            if (buttonByte[0] == 2) button2.Content = oppenentChar;
            if (buttonByte[0] == 3) button3.Content = oppenentChar;
            if (buttonByte[0] == 4) button4.Content = oppenentChar;
            if (buttonByte[0] == 5) button5.Content = oppenentChar;
            if (buttonByte[0] == 6) button6.Content = oppenentChar;
            if (buttonByte[0] == 7) button7.Content = oppenentChar;
            if (buttonByte[0] == 8) button8.Content = oppenentChar;
            if (buttonByte[0] == 9) button9.Content = oppenentChar;
        }

        private void button1_Click(object sender, RoutedEventArgs e) {buttonClick(new Byte[] { 1 }, button1);}

        private void button2_Click(object sender, RoutedEventArgs e) {buttonClick(new Byte[] { 2 }, button2);}

        private void button3_Click(object sender, RoutedEventArgs e) {buttonClick(new Byte[] { 3 }, button3);}

        private void button4_Click(object sender, RoutedEventArgs e) {buttonClick(new Byte[] { 4 }, button4);}

        private void button5_Click(object sender, RoutedEventArgs e) {buttonClick(new Byte[] { 5 }, button5);}

        private void button6_Click(object sender, RoutedEventArgs e) {buttonClick(new Byte[] { 6 }, button6);}

        private void button7_Click(object sender, RoutedEventArgs e) {buttonClick(new Byte[] { 7 }, button7);}

        private void button8_Click(object sender, RoutedEventArgs e) {buttonClick(new Byte[] { 8 }, button8);}

        private void button9_Click(object sender, RoutedEventArgs e) {buttonClick(new Byte[] { 9 }, button9);}

        private void buttonClick(byte[] buttonNumber, Button button)
        {
            socket.Send(buttonNumber);
            button.Content = playerChar;
            worker.RunWorkerAsync();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            worker.WorkerSupportsCancellation = true;
            worker.CancelAsync();
            if (server != null) server.Stop();
        }
    }
}
