using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BreakFree.Presentation.Views
{
    public partial class SosTipsView : Window
    {
        // чи вже якась кнопка зараз "грає" анімацію
        private bool _isAnyButtonAnimating = false;

        public SosTipsView()
        {
            InitializeComponent();
        }

        // Закриваємо тільки це вікно і повертаємо головну (Owner)
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // якщо це вікно було відкрите з Dashboard як Owner — показуємо його назад
            if (this.Owner != null)
            {
                this.Owner.Show();
            }

            this.Close();
        }

        // =======================  ✓ СПРАЦЮВАЛО  =======================

        private async void TipWorked_Click(object sender, RoutedEventArgs e)
        {
            if (_isAnyButtonAnimating)
                return;

            if (sender is not Button btn)
                return;

            _isAnyButtonAnimating = true;

            var original = btn.Background;
            btn.Background = Brushes.LightGreen;

            await Task.Delay(5000); // 5 секунд

            btn.Background = original;
            _isAnyButtonAnimating = false;
        }

        // ========================  ✘ НЕ СПРАЦЮВАЛО  ======================

        private async void TipNotWorked_Click(object sender, RoutedEventArgs e)
        {
            if (_isAnyButtonAnimating)
                return;

            if (sender is not Button btn)
                return;

            _isAnyButtonAnimating = true;

            var original = btn.Background;
            btn.Background = Brushes.IndianRed;

            await Task.Delay(5000); // 5 секунд

            btn.Background = original;
            _isAnyButtonAnimating = false;
        }
    }
}
