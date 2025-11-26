using System.Windows;
using BreakFree.BLL.Services;

namespace BreakFree.Presentation.Views
{
    public partial class RegisterWindow : Window
    {

        private readonly UserService _userService = new();

        public RegisterWindow()
        {
            InitializeComponent();
        }


        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var name = NameTextBox.Text.Trim();
            var email = EmailTextBox.Text.Trim();
            var password = PasswordBox.Password;
            var confirmPassword = ConfirmPasswordBox.Password;

            if (password != confirmPassword)
            {
                MessageBox.Show("Паролі не збігаються!");
                return;
            }

            bool success = _userService.Register(name, email, password); 
            
            if (success)
            {
                MessageBox.Show("Реєстрація пройшла успішно!");
                var login = new LoginWindow();
                login.Show();
                this.Close();
            }

            else
            {
                MessageBox.Show("Цей Email вже зареєстрований!");
            }
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            var login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}
