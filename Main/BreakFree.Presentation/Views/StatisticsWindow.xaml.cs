using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using BreakFree.BLL.Services;
using BreakFree.DAL.Entities;

namespace BreakFree.Presentation.Views
{
    public partial class StatisticsView : Window
    {
        private readonly DailyStatusService _dailyStatusService;
        private readonly int _userId;

        private const int MaxCravingLevel = 10; // шкала тяги 0–10
        private const int MaxDaysOnChart = 15;  // максимум днів на графіку

        public int TotalCleanDays { get; private set; }
        public int LongestStreakDays { get; private set; }
        public int TotalRelapses { get; private set; }

        public List<DailySummaryPoint> ChartData { get; private set; } = new();
        public List<TriggerStat> TopTriggers { get; private set; } = new();
        public List<AchievementBadge> Achievements { get; private set; } = new();

        // для дизайнера
        public StatisticsView() : this(0) { }

        public StatisticsView(int userId)
        {
            InitializeComponent();

            _userId = userId;
            _dailyStatusService = new DailyStatusService();

            Loaded += StatisticsView_Loaded;
        }

        private void StatisticsView_Loaded(object sender, RoutedEventArgs e)
        {
            LoadStatistics();
            DataContext = this;
            DrawChart();
        }

        // ===================== ЗАВАНТАЖЕННЯ СТАТИСТИКИ =====================

        private void LoadStatistics()
        {
            var statuses = _dailyStatusService.GetStatusesByUser(_userId);

            if (statuses == null || statuses.Count == 0)
            {
                TotalCleanDays = 0;
                LongestStreakDays = 0;
                TotalRelapses = 0;
                ChartData = new();
                TopTriggers = new();
                Achievements = CreateAchievements(0);   // немає серії — всі бейджі сірі
                return;
            }

            // групуємо всі статуси користувача по днях
            var groupedByDay = statuses
                .GroupBy(s => s.DateTime.Date)
                .ToDictionary(g => g.Key, g => g.ToList());

            var firstDate = groupedByDay.Keys.Min();
            var lastDate = groupedByDay.Keys.Max();

            var daily = new List<DailyDayInfo>();

            // суцільний інтервал від firstDate до lastDate:
            // дні без записів = чисті дні
            for (var date = firstDate; date <= lastDate; date = date.AddDays(1))
            {
                groupedByDay.TryGetValue(date, out var dayStatuses);

                bool hasRelapse;
                int maxCraving;

                if (dayStatuses != null && dayStatuses.Count > 0)
                {
                    maxCraving = dayStatuses.Max(s => s.CravingLevel ?? 0);
                    hasRelapse = dayStatuses.Any(IsRelapse);
                }
                else
                {
                    maxCraving = 0;
                    hasRelapse = false;
                }

                daily.Add(new DailyDayInfo
                {
                    Date = date,
                    HasRelapse = hasRelapse,
                    MaxCraving = maxCraving
                });
            }

            // Загальна статистика по всьому періоду
            TotalRelapses = daily.Count(d => d.HasRelapse);
            TotalCleanDays = daily.Count(d => !d.HasRelapse);

            // Поточна серія чистих днів (те, що ти хочеш для бейджів)
            int currentStreak = 0;
            int longest = 0;

            foreach (var day in daily)
            {
                if (day.HasRelapse)
                {
                    currentStreak = 0;      // зрив — серія обнуляється, бейджі мають зникнути
                }
                else
                {
                    currentStreak++;
                    if (currentStreak > longest)
                        longest = currentStreak;
                }
            }

            LongestStreakDays = longest;

            // Дані для графіка — тільки останні MaxDaysOnChart днів
            var dailyForChart = daily;
            if (dailyForChart.Count > MaxDaysOnChart)
                dailyForChart = dailyForChart.Skip(dailyForChart.Count - MaxDaysOnChart).ToList();

            ChartData = dailyForChart
                .Select(d => new DailySummaryPoint
                {
                    Date = d.Date,
                    HasRelapse = d.HasRelapse,
                    MaxCraving = d.MaxCraving
                })
                .ToList();

            // Топ-3 тригери (по всіх статусах зі зривом)
            var relapseStatuses = statuses
                .Where(IsRelapse)
                .Where(s => !string.IsNullOrWhiteSpace(s.Trigger));

            TopTriggers = relapseStatuses
                .GroupBy(s => s.Trigger!.Trim())
                .Select(g => new TriggerStat
                {
                    Trigger = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(t => t.Count)
                .Take(3)
                .ToList();

            // ❗ Бейджі залежать від ПОТОЧНОЇ серії, а не від longest
            Achievements = CreateAchievements(currentStreak);
        }

        // Будь-який статус з тягою > 0 вважаємо днем зі зривом
        private bool IsRelapse(DailyStatus status)
        {
            return (status.CravingLevel ?? 0) > 0;
        }

        // Поточна серія передається сюди як streak
        private List<AchievementBadge> CreateAchievements(int streak)
        {
            return new List<AchievementBadge>
            {
                new AchievementBadge { Title = "7 днів",   IsUnlocked = streak >= 7  },
                new AchievementBadge { Title = "1 місяць", IsUnlocked = streak >= 30 },
                new AchievementBadge { Title = "3 місяці", IsUnlocked = streak >= 90 }
            };
        }

        // ===================== ГРАФІК (останні 15 днів, без дат) =====================

        private void DrawChart()
        {
            if (ChartCanvas == null || ChartData == null || ChartData.Count == 0)
                return;

            ChartCanvas.Children.Clear();

            double width = ChartCanvas.ActualWidth;
            double height = ChartCanvas.ActualHeight;

            if (width <= 0) width = 300;
            if (height <= 0) height = 250;

            double leftMargin = 20;
            double rightMargin = 10;
            double topMargin = 15;
            double bottomMargin = 20;

            double plotWidth = width - leftMargin - rightMargin;
            double plotHeight = height - topMargin - bottomMargin;

            if (plotWidth <= 0 || plotHeight <= 0)
                return;

            double axisY = topMargin + plotHeight;

            // вісь X
            var axis = new Line
            {
                X1 = leftMargin,
                Y1 = axisY,
                X2 = leftMargin + plotWidth,
                Y2 = axisY,
                Stroke = Brushes.Gray,
                StrokeThickness = 1
            };
            ChartCanvas.Children.Add(axis);

            int n = ChartData.Count;
            if (n == 0) return;

            double stepX = plotWidth / n;     // чим більше днів, тим менший stepX
            double barWidth = stepX * 0.6;      // ширина бару пропорційна stepX
            if (barWidth < 4) barWidth = 4;   // мінімум
            if (barWidth > 40) barWidth = 40;  // максимум

            for (int i = 0; i < n; i++)
            {
                var d = ChartData[i];

                double centerX = leftMargin + stepX * i + stepX / 2.0;

                double heightFactor;
                if (d.HasRelapse)
                {
                    // зрив — висота ∝ максимальній тязі за день
                    heightFactor = Math.Max(0.1, (double)d.MaxCraving / MaxCravingLevel);
                }
                else
                {
                    // чистий день — максимальна висота
                    heightFactor = 1.0;
                }

                double barHeight = plotHeight * heightFactor * 0.9; // щоби не впиратись в верх

                var rect = new Rectangle
                {
                    Width = barWidth,
                    Height = barHeight,
                    Fill = d.HasRelapse ? Brushes.Red : Brushes.Green,
                    RadiusX = 4,
                    RadiusY = 4
                };

                double top = axisY - barHeight;

                Canvas.SetLeft(rect, centerX - barWidth / 2.0);
                Canvas.SetTop(rect, top);
                ChartCanvas.Children.Add(rect);

                // НІЯКИХ дат / текстів — тільки кольорові стовпці
            }
        }

        // ===================== ЕКСПОРТ =====================

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string filePath = System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "breakfree_stats.txt");

                string text =
                    $"Статистика користувача #{_userId}\n" +
                    $"Чистих днів: {TotalCleanDays}\n" +
                    $"Найдовша серія: {LongestStreakDays}\n" +
                    $"Зривів: {TotalRelapses}\n";

                System.IO.File.WriteAllText(filePath, text, Encoding.UTF8);

                MessageBox.Show("Експорт успішний!", "Експорт",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Експорт",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ===================== СКАСУВАТИ =====================

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Owner?.Show();
            Close();
        }

        // ===================== ДОПОМІЖНІ КЛАСИ =====================

        public class DailySummaryPoint
        {
            public DateTime Date { get; set; }
            public bool HasRelapse { get; set; }
            public int MaxCraving { get; set; }
        }

        internal class DailyDayInfo
        {
            public DateTime Date { get; set; }
            public bool HasRelapse { get; set; }
            public int MaxCraving { get; set; }
        }

        public class TriggerStat
        {
            public string Trigger { get; set; } = "";
            public int Count { get; set; }
        }

        public class AchievementBadge
        {
            public string Title { get; set; } = "";
            public bool IsUnlocked { get; set; }
        }
    }

    // ===================== КОНВЕРТЕРИ ДЛЯ XAML =====================

    public class ZeroToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                int count = System.Convert.ToInt32(value);
                return count == 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            catch
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    public class BoolToAchievementColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool unlocked = value is bool b && b;

            return unlocked
                ? new SolidColorBrush(Color.FromRgb(212, 248, 212))   // зелений, якщо отримано
                : new SolidColorBrush(Color.FromRgb(238, 238, 238));  // сірий, якщо ще ні
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
