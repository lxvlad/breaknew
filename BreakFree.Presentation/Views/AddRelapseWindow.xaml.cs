using BreakFree.BLL.Services;
using BreakFree.DAL.Entities;
using System;
using System.Windows;
using System.Windows.Controls;

namespace BreakFree.Presentation.Views
{
    public partial class AddRelapseWindow : Window
    {
        private readonly int _habitId;

        private readonly DailyStatusService _dailyStatusService;


        public AddRelapseWindow(int habitId)
        {
            InitializeComponent();
            _habitId = habitId;
            _dailyStatusService = new DailyStatusService();
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var status = new DailyStatus
            {
                HabitId = _habitId,
                DateTime = DateTimePicker.SelectedDate ?? DateTime.Now,
                Trigger = (TriggerComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
                Note = NoteTextBox.Text,
                CravingLevel = (int)IntensitySlider.Value
            };

            _dailyStatusService.AddDailyStatus(status);
            MessageBox.Show("Зрив додано!");
            this.Close();
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // var main = new Dashboard(userId);
            // main.Show();
            this.Close();
        }
    }
}
