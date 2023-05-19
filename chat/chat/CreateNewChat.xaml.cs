using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace chat
{
    /// <summary>
    /// Interaction logic for CreateNewChat.xaml
    /// </summary>
    public partial class CreateNewChat : Window
    {
        public CreateNewChat()
        {
            InitializeComponent();
        }

        private void Close_click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Create_click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
