using BreakFree.BLL.Services;
using BreakFree.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;



namespace BreakFree.Presentation.Views
{
    public partial class CalendarWindow : Window
    {
        private readonly int _userId;
        private readonly DailyStatusService _statusService;

        public CalendarWindow(int userId)
        {
            InitializeComponent();
            _userId = userId;
            _statusService = new DailyStatusService();
            LoadCalendar();
        }

        private void LoadCalendar()
        {
            var statuses = _statusService.GetStatusesByUser(_userId);
            int dayCounter = 1;

            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {

                    if (dayCounter > DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month))
                        break;

                    var dayBorder = new Border
                    {
                        BorderBrush = Brushes.Gray,
                        BorderThickness = new Thickness(1),
                        Margin = new Thickness(1),
                        Background = Brushes.LightGreen
                    };

                    if (statuses.Exists(s => s.DateTime.Day == dayCounter && s.DateTime.Month == DateTime.Now.Month && s.DateTime.Year == DateTime.Now.Year))
                    {
                        dayBorder.Background = Brushes.LightCoral; 
                    }

                    var text = new TextBlock
                    {
                        Text = dayCounter.ToString(),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };


                    dayBorder.Child = text;
                    CalendarGrid.Children.Add(dayBorder);
                    Grid.SetRow(dayBorder, row);
                    Grid.SetColumn(dayBorder, col);

                    dayCounter++;
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            JournalWindow journalWindow = new JournalWindow(_userId);
            journalWindow.Show();
        }

        // private void AddRelapse_Click(object sender, RoutedEventArgs e)
        // {
        //     AddRelapseWindow addWindow = new AddRelapseWindow(_userId);
        //     addWindow.Show();
        // }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            var main = new Dashboard(_userId);
            main.Show();
            this.Close();
        }
    }
}
