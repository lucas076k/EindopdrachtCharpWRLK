using System.Windows;

namespace Client;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
    }
    
    
    
    private void LoginButton_OnClick(object sender, RoutedEventArgs e)
    {
        //Closes LoginWindow.xaml and opens MainWindow.xaml
        if (!string.IsNullOrWhiteSpace(UserTextBox.Text))
        {
            MainWindow mainWindow = new MainWindow(UserTextBox.Text);
            mainWindow.Show();
            this.Close();
        }
        else
        {
            MessageBox.Show("Fill in a username before you login to the server!", "Invalid username.", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
    
}