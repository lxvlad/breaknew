using System;
using System.Windows;
using BreakFree.BLL.Services;


namespace BreakFree.Presentation.Views
{
    public partial class ChangePasswordWindow : Window
    {

        private int userId;

        private UserService userService;


        public ChangePasswordWindow(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            userService = new UserService();
        }


        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            string currentPassword = txtCurrentPassword.Password.Trim();
            string newPassword = txtNewPassword.Password.Trim();

            if (string.IsNullOrWhiteSpace(currentPassword) || string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Заповніть усі поля.");
                return;
            }

            bool success = userService.ChangePassword(userId, currentPassword, newPassword);

            if (success)
            {
                ProfileWindow profileWindow = new ProfileWindow(userId);
                profileWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Невірний поточний пароль");
            }
        }


        private void CancelButton_Click(Object sender, RoutedEventArgs e)
        {
            ProfileWindow profileWindow = new ProfileWindow(userId);
            profileWindow.Show();
            this.Close();
        }
    }
}
