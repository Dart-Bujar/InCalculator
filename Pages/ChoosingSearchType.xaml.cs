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

namespace ApplCEEHACk.Pages
{
    /// <summary>
    /// Логика взаимодействия для ChoosingSearchType.xaml
    /// </summary>
    public partial class ChoosingSearchType : Page
    {
        public ChoosingSearchType()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FindByCode_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SearchPatient sp = new SearchPatient();
            var win = Window.GetWindow(this);
            win.Content = sp;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Main m = new Main();
            var win = Window.GetWindow(this);
            win.Content = m;
        }
    }
}
