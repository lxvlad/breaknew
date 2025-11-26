using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace BreakFree.Presentation.Views
{
    public partial class ConfirmCancelWindow : Window
    {
        public ConfirmCancelWindow()
        {
            InitializeComponent();
        }


        private void DiscardButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;   // Скасувати зміни
            this.Close();
        }


        private void ContinueButton_Click(Object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;  // Продовжити редагування
            this.Close();
        }
    }
}