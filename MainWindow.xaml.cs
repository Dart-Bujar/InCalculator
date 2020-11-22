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
using ApplCEEHACk.Pages;

namespace ApplCEEHACk
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DbModel.dataBaseSetEntities db;
        public List<DbModel.PatientsList> pl;
        
        public MainWindow()
        {
            InitializeComponent();
            Main m = new Main();
            this.Content = m;
            var s = m.Content as Main;
            
               
           
        }
        public void ChangePage(Page p)
        {
            this.Content = p;
        } 

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
          


        }
    }
}
