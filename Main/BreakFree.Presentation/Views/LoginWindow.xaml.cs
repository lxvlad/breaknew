using System.Windows;
using BreakFree.BLL.Services;

namespace BreakFree.Presentation.Views
{
    public partial class LoginWindow : Window
    {

        private readonly UserService _userService = new();


        public LoginWindow()
        {
            InitializeComponent();
        }


        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var username = txtUsername.Text.Trim();
            var password = txtPassword.Password;

            var user = _userService.Login(username, password);

            if (user != null) {
                int userId = user.UserId;
                var main = new Dashboard(userId);
                main.Show();
                Close();
            }

            else
            {
                MessageBox.Show("Invalid credentials!");
            }
        }


        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var reg = new RegisterWindow();
            reg.Show();
            Close();
        }
    }
}
