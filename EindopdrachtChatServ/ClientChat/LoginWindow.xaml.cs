using System.Windows;

namespace ClientChat;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
    }
    
    
    
    private void LoginButton_OnClick(object sender, RoutedEventArgs e)
    {
        //Closes LoginWindow.xaml and opens MainWindow.xaml
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();
        
    }
    
}