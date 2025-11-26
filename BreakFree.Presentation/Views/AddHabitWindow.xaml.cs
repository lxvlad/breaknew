using System;
using System.Windows;
using BreakFree.BLL.Services;

namespace BreakFree.Presentation.Views
{
    public partial class AddHabitWindow : Window
    {
        private int userId;
        private HabitService habitService;

        public AddHabitWindow(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            habitService = new HabitService();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var habitName = txtHabitName.Text.Trim();
            var startDateString = txtStartDate.Text.Trim();
            var goalString = txtGoal.Text.Trim();
            var motivation = txtMotivation.Text.Trim();

            if (string.IsNullOrWhiteSpace(habitName) || 
                string.IsNullOrWhiteSpace(startDateString) ||
                string.IsNullOrWhiteSpace(goalString))
            {
                MessageBox.Show("Будь ласка, заповніть усі поля.");
                return;
            }

            if (!DateTime.TryParse(startDateString, out var startDate))
            {
                MessageBox.Show("Неправильний формат дати.");
                return;
            }

            if (!int.TryParse(goalString, out var goal))
            {
                MessageBox.Show("Ціль повинна бути числом.");
                return;
            }

            try
            {
                habitService.AddHabit(userId, habitName, startDate, goal, motivation);
                var main = new Dashboard(userId);
                main.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при збереженні: " + ex.Message);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            var main = new Dashboard(userId);
            main.Show();
            this.Close();
        }
    }
}
