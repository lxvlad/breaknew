using BreakFree.BLL.Services;
using BreakFree.DAL.Entities;
using BreakFree.Presentation.Views;
using System;
using System.Windows;


namespace BreakFree.Presentation.Views
{
    public partial class Dashboard : Window
    {
        private readonly int userId;

        private readonly HabitService habitService;

        private List<Habit> habits;
    
        private int currentHabitIndex = 0;

        public Dashboard(int userId)
        {
            InitializeComponent();
            this.userId = userId;

            habitService = new HabitService();

            LoadHabits();
        }


        private void LoadHabits()
        {
            habits = habitService.GetUserHabits(userId);

            if (habits.Count == 0)
            {
                HabitNameText.Text = "Немає звичок";
                return;
            }

            DisplayHabit(currentHabitIndex);
    
        }


        private void DisplayHabit(int index)
        {
            if (habits == null || habits.Count == 0)
                return;

            var h = habits[index];

            HabitNameText.Text = h.HabitName;
            StartDateText.Text = h.StartDate.ToString("dd.MM.yyyy");
            MotivationText.Text = h.Motivation;

            StatusText.Text = h.IsActive ? "✔ Активна" : "✖ Не активна";
            MoneySavedText.Text = $"{h.DailyGoal * 10} ₴ (плейсхолдер)";
        }


        private void NextHabit_Click(object sender, RoutedEventArgs e)
        {
            if (habits == null || habits.Count == 0)
                return;

            currentHabitIndex = (currentHabitIndex + 1) % habits.Count;
            DisplayHabit(currentHabitIndex);
        }


        private void PreviousHabit_Click(object sender, RoutedEventArgs e)
        {
            if (habits == null || habits.Count == 0)
                return;

            currentHabitIndex = (currentHabitIndex - 1 + habits.Count) % habits.Count;
            DisplayHabit(currentHabitIndex);
        }


        private void AddRelapseButton_Click(object sender, RoutedEventArgs e)
        {
            if (habits != null && habits.Count > 0)
            {
                int habitId = habits[currentHabitIndex].HabitId;
                AddRelapseWindow relapseWindow = new AddRelapseWindow(habitId);
                relapseWindow.Show();
                // this.Close();
            }
        }


        private void CalendarButton_Click(Object sender, RoutedEventArgs e)
        {
            CalendarWindow calendarWindow = new CalendarWindow(userId);
            calendarWindow.Show();
            this.Close();
        }


        private void SosButton_Click(object sender, RoutedEventArgs e)
        {
            var sosWindow = new SosTipsView();
            sosWindow.Owner = this; // головна сторінка — власник
            this.Hide();            // ховаємо Dashboard, але не закриваємо
            sosWindow.Show();
        }

        private void AddHabitButton_Click(Object sender, RoutedEventArgs e)
        {
            AddHabitWindow habitWindow = new AddHabitWindow(userId);
            habitWindow.Show();
            this.Close();
        }


        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            AboutView aboutWindow = new AboutView();
            aboutWindow.Show(); 
        }


        private void SupportButton_Click(Object sender, RoutedEventArgs e)
        {
            Support supportWindow = new Support();
            supportWindow.Show();
        }


        private void OpenProfileButton_Click(Object sender, RoutedEventArgs e)
        {
            ProfileWindow profileWindow = new ProfileWindow(userId);
            profileWindow.Show();
            this.Close();
        }

        private void StatisticButton_Click(object sender, RoutedEventArgs e)
        {
            var stats = new StatisticsView(userId); // <-- передаємо userId !!!
            stats.Owner = this;
            this.Hide();
            stats.Show();
        }
        //private void MotivationButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var motivation = new MotivationView(userId); // <-- передаємо userId !!!
        //    motivation.Owner = this;
        //    this.Hide();
        //    motivation.Show();
        //}
    }

}