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

namespace TimeTable {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            DefaultApp();
        }

        public void DefaultApp() {
            UpdateSchedule();
            InitializeForm();
        }
        public void UpdateSchedule() {
            //ttDataGrid.ItemsSource = GetSchedule();
        }
        public void InitializeForm() {
            //clear fields
            tbClass.Clear();
            tbTeacher.Clear();
            tbSubject.Clear();
            tbPeriodPerWeek.Clear();

            btnAdd.IsEnabled = false;
            btnClear.IsEnabled =true;
            btnDelete.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnPrint.IsEnabled = false;
        }

        private void CmbDay_Loaded(object sender, RoutedEventArgs e) {
            
            List<string> days = new List<string>();
            days.Add("Mon");
            days.Add("Tue");
            days.Add("Wed");
            days.Add("Thur");
            days.Add("Fri");

            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            comboBox.ItemsSource = days;

            comboBox.SelectedIndex = 0;
        }

        private void CmbPeriod_Loaded(object sender, RoutedEventArgs e) {

            List<string> periods = new List<string>();
            periods.Add("1");
            periods.Add("2");
            periods.Add("3");
            periods.Add("4");
            periods.Add("5");
            periods.Add("6");
            periods.Add("7");
            periods.Add("8");
            periods.Add("9");

            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            comboBox.ItemsSource = periods;

            comboBox.SelectedIndex = 0;
        }
    }
}
