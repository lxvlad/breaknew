using System.Windows;

namespace BreakFree.Presentation.Views
{
    public partial class MotivationView : Window
    {
        public MotivationView()
        {
            InitializeComponent();
        }

        // Кнопка відкриття SOS-порад
        private void SosButton_Click(object sender, RoutedEventArgs e)
        {
            SosTipsView sosView = new SosTipsView();
            sosView.ShowDialog();
        }

        // Кнопка "Скасувати"
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}