using System;
using System.Windows;
using BreakFree.BLL.Services;

namespace BreakFree.Presentation.Views
{
    public partial class DeleteProfileWindow : Window
    {
        private int userId;
        
        private UserService userService;


        public DeleteProfileWindow(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            userService = new UserService();
        }


        private void DeleteProfileButton_Click(object sender, RoutedEventArgs e)
        {
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введіть пароль");
                return;
            }

            bool success = userService.DeleteUser(userId, password);

            if (success)
            {
                Application.Current.Shutdown();
            }
            else
            {
                MessageBox.Show("Невірний пароль або сталася помилка");
            }
        }


        private void CancelButton_Click(Object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
