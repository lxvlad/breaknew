using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BreakFree.Presentation.Views
{
    public partial class AboutView : Window
    {
        public AboutView()
        {
            InitializeComponent();
        }


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}