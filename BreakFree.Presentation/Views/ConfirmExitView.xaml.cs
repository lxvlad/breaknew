using System.Windows;

namespace BreakFree.Presentation.Views
{
    public partial class ConfirmExitView : Window
    {
        public ConfirmExitView()
        {
            InitializeComponent();
        }

        // Кнопка "Так"
        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        // Кнопка "Ні"
        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}