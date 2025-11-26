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
using BreakFree.BLL.Services;


namespace BreakFree.Presentation.Views
{

    public partial class ProfileWindow : Window
    {
        private int userId;
        private readonly UserService userService = new UserService();

        public ProfileWindow(int userId)
        {
            this.userId = userId;
            InitializeComponent();
            LoadUserData();
        }

        private void LoadUserData()
        {
            var user = userService.GetUserById(userId);
            if (user == null)
                return;

            txtUsername.Text = user.UserName;
            txtEmail.Text = user.Email;
        }


        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordWindow changePasswordWindow = new ChangePasswordWindow(userId);
            changePasswordWindow.Show();

            this.Close();
            //this.Close();
        }

        private void DeleteProfileButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteProfileWindow deleteProfileWindow = new DeleteProfileWindow(userId);
            deleteProfileWindow.Show();
            //this.Close();
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            string newUsername = txtUsername.Text.Trim();
            string newEmail = txtEmail.Text.Trim();

            if (string.IsNullOrWhiteSpace(newUsername) || string.IsNullOrWhiteSpace(newEmail))
            {
                MessageBox.Show("Заповніть всі поля");
                return;
            }

            bool success = userService.UpdateUser(userId, newUsername, newEmail);

            if (success)
            {
                var main = new Dashboard(userId);
                main.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Email або Username вже використовується іншим користувачем");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ConfirmCancelWindow confirm = new ConfirmCancelWindow();
            
            if (confirm.ShowDialog() == true)
            {
                var main = new Dashboard(userId);
                main.Show();
                this.Close();
            }
        }
    }
}
