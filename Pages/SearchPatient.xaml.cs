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
    /// Логика взаимодействия для SearchPatient.xaml
    /// </summary>
    public partial class SearchPatient : Page
    {

        public DbModel.dataBaseSetEntities db ;
        public List<DbModel.PatientsList> pl;
        public SearchPatient()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var id=int.Parse(patientIDbox.Text);
            db = new DbModel.dataBaseSetEntities();
            var pl = db.PatientsLists.Where(x => x.patientID == id).ToList();
            recordDisplay rd = new recordDisplay();
            var win = Window.GetWindow(this);
            InfusionSpeedCalculation case1 = new InfusionSpeedCalculation(double.Parse(pl[0].weight.ToString())); ;
            case1.getActualPumpSpeedMlPerHour();
            
            win.Content =rd;
            if(rd!=null)
            {
                rd.apptttarget.Content = "Target aTTP: "+pl[0].aptttargetMin.ToString() + "-" + pl[0].aptttargetMax.ToString();
                rd.patientID.Content = "Patient ID: "+pl[0].patientID.ToString();
                rd.lastaptt.Content = "Last aPTT: " + pl[0].RecordsLists.First().lastAptt.ToString();
                rd.lastapttTime.Content = "Time: " + pl[0].RecordsLists.First().lastApttTime.ToString();
               
                rd.patinetID = pl[0].patientID;
                rd.pl = pl;
            }
                

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e)
        {

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ChoosingSearchType ins = new ChoosingSearchType();
            var win = Window.GetWindow(this);
            win.Content = ins;
        }
        //private Model.Records GetDatafromDB(string patientID)
        //{
        //    return;
        //}
    }
}
