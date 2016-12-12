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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InstaKiller.Wpf
{
    /// <summary>
    /// Interaction logic for PageSignIn.xaml
    /// </summary>
    public partial class PageSignIn : Page
    {
        public PageSignIn()
        {
            InitializeComponent();
            DataContext = new PageSignInViewModel();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxEmail.Text == "Success!")
            {
                NavigationService.Navigate(new PageLoadPhoto());
            }
        }
    }
}
