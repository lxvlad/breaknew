using BreakFree.BLL.Services;
using BreakFree.DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace BreakFree.Presentation.Views
{
    public partial class JournalWindow : Window
    {
        private readonly int _userId;

        private readonly DailyStatusService _statusService;


        public JournalWindow(int userId)
        {
            InitializeComponent();
            _userId = userId;
            _statusService = new DailyStatusService();
            LoadJournal();
        }


        private void LoadJournal()
        {
            var statuses = _statusService.GetStatusesByUser(_userId);


            var journalItems = statuses.Select(s => new JournalItem
            {
                Date = s.DateTime.ToString("dd.MM"),
                Reason = s.Trigger,
                Note = s.Note
            }).ToList();

            JournalDataGrid.ItemsSource = journalItems;
        }


        private void AddRelapse_Click(object sender, RoutedEventArgs e)
        {
            AddRelapseWindow addWindow = new AddRelapseWindow(_userId);
            addWindow.ShowDialog();
            LoadJournal(); 
        }


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }


    public class JournalItem
    {
        public string Date { get; set; }
        public string Reason { get; set; }
        public string Note { get; set; }
    }
}
