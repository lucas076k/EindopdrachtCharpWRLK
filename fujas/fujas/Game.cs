using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fujas
{
    public partial class Game : Form
    {
        private char playerChar;
        private char oppenentChar;
        private Socket socket;
        private BackgroundWorker worker;
        private TcpListener server;
        private TcpClient client;

        public Game(bool isHost, string ip = null)
        {
            InitializeComponent();
            worker = new BackgroundWorker();
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
            if (Winner()) { return; }
            DisableButtons();
            label1.Text = "Opponent's turn";
            GetOppenentMove();
            label1.Text = "Your turn";
            if (Winner()) { return; }
            EnableEmptyButtons();
        }

        private bool Winner()
        {
            if (button1.Text == button2.Text && button1.Text == button3.Text && button1.Text != "")
            {
                if (button1.Text[0] == playerChar) { WinGame(); }

                else { LoseGame(); }

                return true;
            }

            else if (button4.Text == button5.Text && button4.Text == button6.Text && button4.Text != "")
            {
                if (button4.Text[0] == playerChar) { WinGame(); }

                else { LoseGame(); }

                return true;
            }

            else if (button7.Text == button8.Text && button7.Text == button9.Text && button7.Text != "")
            {
                if (button7.Text[0] == playerChar) { WinGame(); }

                else { LoseGame(); }

                return true;
            }

            else if (button1.Text == button4.Text && button1.Text == button7.Text && button1.Text != "")
            {
                if (button1.Text[0] == playerChar) { WinGame(); }

                else { LoseGame(); }

                return true;
            }

            else if (button2.Text == button5.Text && button2.Text == button8.Text && button2.Text != "")
            {
                if (button2.Text[0] == playerChar) { WinGame(); }

                else { LoseGame(); }

                return true;
            }

            else if (button3.Text == button6.Text && button3.Text == button9.Text && button3.Text != "")
            {
                if (button3.Text[0] == playerChar) { WinGame(); }

                else { LoseGame(); }

                return true;
            }

            else if (button1.Text == button5.Text && button1.Text == button9.Text && button1.Text != "")
            {
                if (button1.Text[0] == playerChar) { WinGame(); }

                else { LoseGame(); }

                return true;
            }

            else if (button3.Text == button5.Text && button3.Text == button7.Text && button3.Text != "")
            {
                if (button3.Text[0] == playerChar) { WinGame(); }

                else { LoseGame(); }

                return true;
            }

            // draw
            else if (button1.Text != "" && button2.Text != "" && button3.Text != "" && button4.Text != "" && button5.Text != "" && button6.Text != "" && button7.Text != "" && button8.Text != "" && button9.Text != "")
            {
                DrawGame();

                return true;
            }

            return false;
        }

        private void WinGame()
        {
            label1.Text = "You won!";
            DisableButtons();
        }

        private void LoseGame()
        {
            label1.Text = "You lost :(";
        }

        private void DrawGame()
        {
            label1.Text = "Draw!";
            DisableButtons();
        }

        private void DisableButtons()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;
        }

        private void EnableEmptyButtons()
        {
            if (button1.Text == "")
                button1.Enabled = true;
            if (button2.Text == "")
                button2.Enabled = true;
            if (button3.Text == "")
                button3.Enabled = true;
            if (button4.Text == "")
                button4.Enabled = true;
            if (button5.Text == "")
                button5.Enabled = true;
            if (button6.Text == "")
                button6.Enabled = true;
            if (button7.Text == "")
                button7.Enabled = true;
            if (button8.Text == "")
                button8.Enabled = true;
            if (button9.Text == "")
                button9.Enabled = true;
        }

        private void GetOppenentMove()
        {
            byte[] buttonByte = new byte[1];
            socket.Receive(buttonByte);

            if (buttonByte[0] == 1) { button1.Text = oppenentChar.ToString(); }
            if (buttonByte[0] == 2) { button2.Text = oppenentChar.ToString(); }
            if (buttonByte[0] == 3) { button3.Text = oppenentChar.ToString(); }
            if (buttonByte[0] == 4) { button4.Text = oppenentChar.ToString(); }
            if (buttonByte[0] == 5) { button5.Text = oppenentChar.ToString(); }
            if (buttonByte[0] == 6) { button6.Text = oppenentChar.ToString(); }
            if (buttonByte[0] == 7) { button7.Text = oppenentChar.ToString(); }
            if (buttonByte[0] == 8) { button8.Text = oppenentChar.ToString(); }
            if (buttonByte[0] == 9) { button9.Text = oppenentChar.ToString(); }
        }

        private void buttonClick(int buttonNumber, Button button)
        {
            Byte buttonByte = ((byte)buttonNumber);
            Byte[] bytes = { buttonByte };
            socket.Send((bytes));
            button.Text = playerChar.ToString();
            worker.RunWorkerAsync();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            worker.WorkerSupportsCancellation = true;
            worker.CancelAsync();
            if (server != null) server.Stop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            buttonClick(1, button1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            buttonClick(2, button2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            buttonClick(3, button3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            buttonClick(4, button4);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            buttonClick(5, button5);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            buttonClick(6, button6);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            buttonClick(7, button7);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            buttonClick(8, button8);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            buttonClick(9, button9);
        }
    }
}
