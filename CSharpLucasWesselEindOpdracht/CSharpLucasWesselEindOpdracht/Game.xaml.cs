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

namespace CSharpLucasWesselEindOpdracht
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        
        private char PlayerChar;
        private char OppenentChar;

        public Game()
        {
            InitializeComponent();
        }
        
        private bool Winner()
        {
            if(button1.Content == button2.Content && button1.Content == button3.Content && !button1.Content.Equals(""))
            {
                if (button1.Content.Equals(PlayerChar)) WinGame();

                else LoseGame();
                
                return true;
            }

            else if(button4.Content == button5.Content && button4.Content == button6.Content && !button4.Content.Equals(""))
            {
                if (button1.Content.Equals(PlayerChar)) WinGame();

                else LoseGame();

                return true;
            }

            else if(button7.Content == button8.Content && button7.Content == button9.Content && !button7.Content.Equals(""))
            {
                if (button1.Content.Equals(PlayerChar)) WinGame();

                else LoseGame();

                return true;
            }

            else if (button1.Content == button4.Content && button1.Content == button7.Content && !button1.Content.Equals(""))
            {
                if (button1.Content.Equals(PlayerChar)) WinGame();

                else LoseGame();

                return true;
            }

            else if (button2.Content == button5.Content && button2.Content == button8.Content && !button2.Content.Equals(""))
            {
                if (button1.Content.Equals(PlayerChar)) WinGame();

                else LoseGame();

                return true;
            }

            else if (button3.Content == button6.Content && button3.Content == button9.Content && !button3.Content.Equals(""))
            {
                if (button1.Content.Equals(PlayerChar)) WinGame();

                else LoseGame();

                return true;
            }

            else if (button1.Content == button5.Content && button1.Content == button9.Content && !button1.Content.Equals(""))
            {
                if (button1.Content.Equals(PlayerChar)) WinGame();

                else LoseGame();

                return true;
            }

            else if (button3.Content == button5.Content && button3.Content == button7.Content && !button3.Content.Equals(""))
            {
                if (button1.Content.Equals(PlayerChar)) WinGame();

                else LoseGame();

                return true;
            }

            // draw
            else if (!button1.Content.Equals("") && !button2.Content.Equals("") && !button3.Content.Equals("") && !button4.Content.Equals("") && 
                !button5.Content.Equals("") && !button6.Content.Equals("") && !button7.Content.Equals("") && !button8.Content.Equals("") && !button9.Content.Equals(""))
            {
                DrawGame();
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

    }
}
