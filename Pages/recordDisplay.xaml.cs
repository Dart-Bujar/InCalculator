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
using ApplCEEHACk;

namespace ApplCEEHACk.Pages
{
    
    /// <summary>
    /// Логика взаимодействия для recordDisplay.xaml
    /// </summary>
    public partial class recordDisplay : Page
    {
        public DbModel.dataBaseSetEntities _context;
        public List<DbModel.PatientsList> pl { get; set; }
        public List<DbModel.RecordsList> rl;
        public int patinetID { get; set; }
        InfusionSpeedCalculation case1;
              
        public recordDisplay()
        {
            InitializeComponent();
            _context= new DbModel.dataBaseSetEntities();
           
           
            try
            {
                //var pl = _context.PatientsLists.Where(x => x.patientID == patinetID).ToList();
               
            }
            catch
            {
                this.recValue.Content = "Database connection error. Cant find this patient";
            }
            


        }
       
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            SearchPatient sp = new SearchPatient();
            var win = Window.GetWindow(this);
            win.Content = sp;

        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            
            
                
                case1.getActualPumpSpeedMlPerHour();
                //recValue.Content =case1.GetRecomended(float.Parse(ActualAptt.Text),double.Parse(ActualPumpSpeed.Text), double.Parse(pl[0].weight.ToString()));
               recValue.Content=case1.updateAll(float.Parse(ActualAptt.Text));
                if (double.Parse(recValue.Content.ToString()) == -100)
                {
                info.Foreground = new SolidColorBrush(Color.FromRgb(204, 0, 0));
                info.Content = "Stop the infusion and wait 60 minute. " +"\n"+
                    "Then Reduce by 3 iu/kg/g";
                    recValue.Content = "";
                }else
                {
                    info.Content = "";
                }
                
                nextApptCheck.Text=case1.nextAPTTCheck.ToString();
          
               // recValue.Content = "Error. Actual appt data must contain only numbers. Try again or contacn support";
        }

        private void SumbitButton_Click(object sender, RoutedEventArgs e)
        {
            var r = new Random(patinetID);
            
                DbModel.RecordsList record = new DbModel.RecordsList();
                record.lastAptt = float.Parse(ActualAptt.Text);
                record.lastApttTime = DateTime.Now;
                record.actualAptt = float.Parse(ActualAptt.Text);
                record.patientID = pl[0].patientID;
                record.actualTime = DateTime.Parse(nextApptCheck.Text);
                record.PatientsList = null;
                record.recordID = pl[0].RecordsLists.Count() + 1;
                record.recomendedAptt = float.Parse(recValue.Content.ToString());
                record.submitedAptt = float.Parse(newPumpSpeed.Text);
                pl[0].RecordsLists.Add(record);
                _context.RecordsLists.Add(record);
                _context.SaveChanges();
                info.Foreground = new SolidColorBrush(Color.FromRgb(100,81,202));
                info.Content = "Data was saved to the DB";
         
        }

        private void newPumpSpeed_TextChanged(object sender, TextChangedEventArgs e)
           
        {
            
            case1.setActualPumpSpeedMlPerHour(float.Parse(newPumpSpeed.Text));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            case1 = new InfusionSpeedCalculation(double.Parse(pl[0].weight.ToString()));
        }
    }
}
