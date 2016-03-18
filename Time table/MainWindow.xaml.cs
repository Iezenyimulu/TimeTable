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

        List<GeneralProgramme> programmeList ;

        public void DefaultApp() {
            GetTimeTable();
            InitializeForm();
            tbClass.Focus();
        }

        public void AddSchedule(object sender, RoutedEventArgs e) {
            var validationSuccess = ValidateUserInput();
            var scheduler = new TimeTableEntities.Scheduler();
            TimeTableEntities.ValidationModel validationMessage;
            if (validationSuccess) {
                validationMessage = scheduler.AddSchedule(CloneSchedule());
                if (validationMessage.Id != "Success") {
                    MessageBox.Show(validationMessage.Message, validationMessage.Id, MessageBoxButton.OK, MessageBoxImage.Error);
                    tbValidation.Text = String.Format("{0}\n{1}", validationMessage.Id, validationMessage.Message);
                }
                GetTimeTable();
            }
        }


        private bool ValidateUserInput() {

            var message = "";

            if (tbSubject.Text.Length > 5) {
                message += "Enter subject alias. Max length is 5.";
                MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
            //if (String.IsNullOrWhiteSpace(tbElective.Text)
            //    message = "If this is an elective subject, enter the elective code";           


        }

        private TimeTableEntities.ScheduleModel CloneSchedule() {
            var schedule = new TimeTableEntities.ScheduleModel();
            schedule.Class = tbClass.Text;
            schedule.PeriodPerWeek = int.Parse(cmbPeriodPerWeek.Text);
            schedule.Subject = tbSubject.Text;
            schedule.Teacher = tbTeacher.Text;
            schedule.Elective = tbElective.Text;
            return schedule;

        }



        public void GetTimeTable() {
            var scheduler = new TimeTableEntities.Scheduler();
            programmeList = new List<GeneralProgramme>();
            var format = cmbTimeTableFormat.Text;
            IEnumerable<TimeTableEntities.Schedule> schedules = scheduler.GetSchedule();
            switch (format.ToLower()) {

                case "subject":
                    if (!String.IsNullOrWhiteSpace(tbSubject.Text))
                        schedules = scheduler.GetSubjectSchedule(tbSubject.Text);
                    else
                        MessageBox.Show("Enter Subject");
                    break;
                case "teacher":
                    if (!String.IsNullOrWhiteSpace(tbTeacher.Text))
                        schedules = scheduler.GetTeacherSchedule(tbTeacher.Text);
                    else
                        MessageBox.Show("Enter Teacher");
                    break;
                default:
                    schedules = scheduler.GetSchedule().ToList().OrderBy(s=>s.Class, new ClassComparer());
                    var scheduleGroup = from s in schedules
                                        //orderby s.Day, s.Period
                                        group s by s.Class into grouping
                                        select new {
                                            grouping.Key,
                                            grouping
                                        };




                    foreach (var grp in scheduleGroup) {
                        GeneralProgramme classSchedule = new GeneralProgramme();
                        foreach (var prg in grp.grouping) {
                            CloneClassProgramme(grp.Key, prg, ref classSchedule);
                        }
                        programmeList.Add(classSchedule);
                    }

                    ListCollectionView collectionView = new ListCollectionView(programmeList);
                    collectionView.GroupDescriptions.Add(new PropertyGroupDescription("Class"));
                    GeneralDataGrid.ItemsSource = collectionView;
                    break;
            }
            ClearField();
        }

        private void CloneClassProgramme(string grpKey, TimeTableEntities.Schedule prg, ref GeneralProgramme classSchedule) {

            classSchedule.Class = grpKey.ToUpper();
            
            prg.Subject = prg.Subject.ToUpper();
            

            //monday
            if (prg.Day.ToLower() == "mon" && prg.Period == 1) {
                classSchedule.MP1 = prg.Subject;
                classSchedule.MP1Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "mon" && prg.Period == 2){
                classSchedule.MP2 = prg.Subject;
                classSchedule.MP2Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "mon" && prg.Period == 3){
                classSchedule.MP3 = prg.Subject;
                classSchedule.MP3Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "mon" && prg.Period == 4){
                classSchedule.MP4 = prg.Subject;
                classSchedule.MP4Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "mon" && prg.Period == 5){
                classSchedule.MP5 = prg.Subject;
                classSchedule.MP5Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "mon" && prg.Period == 6){
                classSchedule.MP6 = prg.Subject;
                classSchedule.MP6Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "mon" && prg.Period == 7){
                classSchedule.MP7 = prg.Subject;
                classSchedule.MP7Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "mon" && prg.Period == 8){
                classSchedule.MP8 = prg.Subject;
                classSchedule.MP8Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "mon" && prg.Period == 9){
                classSchedule.MP9 = prg.Subject;
                classSchedule.MP9Id = prg.Id;
            }

          //tuesday
            else if (prg.Day.ToLower() == "tues" && prg.Period == 1) {
                classSchedule.TP1 = prg.Subject;
                classSchedule.TP1Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "tues" && prg.Period == 2) {
                classSchedule.TP2Id = prg.Id;
                classSchedule.TP2 = prg.Subject;
            }
            else if (prg.Day.ToLower() == "tues" && prg.Period == 3) {
                classSchedule.TP3 = prg.Subject;
                classSchedule.TP3Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "tues" && prg.Period == 4) {
                classSchedule.TP4 = prg.Subject;
                classSchedule.TP4Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "tues" && prg.Period == 5) {
                classSchedule.TP5 = prg.Subject;
                classSchedule.TP5Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "tues" && prg.Period == 6) {
                classSchedule.TP6 = prg.Subject;
                classSchedule.TP6Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "tues" && prg.Period == 7) {
                classSchedule.TP7 = prg.Subject;
                classSchedule.TP7Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "tues" && prg.Period == 8) {
                classSchedule.TP8 = prg.Subject;
                classSchedule.TP8Id = prg.Id;
            }

            else if (prg.Day.ToLower() == "tues" && prg.Period == 9) {
                classSchedule.TP9 = prg.Subject;
                classSchedule.TP9Id = prg.Id;
            }
            //wednesday
            else if (prg.Day.ToLower() == "wed" && prg.Period == 1) {
                classSchedule.WP1 = prg.Subject;
                classSchedule.WP1Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "wed" && prg.Period == 2) {
                classSchedule.WP2 = prg.Subject;
                classSchedule.WP2Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "wed" && prg.Period == 3) {
                classSchedule.WP3 = prg.Subject;
                classSchedule.WP3Id = prg.Id;
            }

            else if (prg.Day.ToLower() == "wed" && prg.Period == 4) {
                classSchedule.WP4 = prg.Subject;
                classSchedule.WP4Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "wed" && prg.Period == 5) {
                classSchedule.WP5 = prg.Subject;
                classSchedule.WP5Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "wed" && prg.Period == 6) {
                classSchedule.WP6 = prg.Subject;
                classSchedule.WP6Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "wed" && prg.Period == 7) {
                classSchedule.WP7 = prg.Subject;
                classSchedule.WP7Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "wed" && prg.Period == 8) {
                classSchedule.WP8 = prg.Subject;
                classSchedule.WP8Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "wed" && prg.Period == 9) {
                classSchedule.WP9 = prg.Subject;
                classSchedule.WP9Id = prg.Id;
            }
            //thursday
            else if (prg.Day.ToLower() == "thur" && prg.Period == 1) {
                classSchedule.THP1 = prg.Subject;
                classSchedule.THP1Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "thur" && prg.Period == 2) {
                classSchedule.THP2 = prg.Subject;
                classSchedule.THP2Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "thur" && prg.Period == 3) {
                classSchedule.THP3 = prg.Subject;
                classSchedule.THP3Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "thur" && prg.Period == 4) {
                classSchedule.THP4 = prg.Subject;
                classSchedule.THP4Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "thur" && prg.Period == 5) {
                classSchedule.THP5 = prg.Subject;
                classSchedule.THP5Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "thur" && prg.Period == 6) {
                classSchedule.THP6 = prg.Subject;
                classSchedule.THP6Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "thur" && prg.Period == 7) {
                classSchedule.THP7 = prg.Subject;
                classSchedule.THP7Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "thur" && prg.Period == 8) {
                classSchedule.THP8 = prg.Subject;
                classSchedule.THP8Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "thur" && prg.Period == 9) {
                classSchedule.THP9 = prg.Subject;
                classSchedule.THP9Id = prg.Id;
            }
            //friday
            else if (prg.Day.ToLower() == "fri" && prg.Period == 1) {
                classSchedule.FP1 = prg.Subject;
                classSchedule.FP1Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "fri" && prg.Period == 2) {
                classSchedule.FP2 = prg.Subject;
                classSchedule.FP2Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "fri" && prg.Period == 3) {
                classSchedule.FP3 = prg.Subject;
                classSchedule.FP3Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "fri" && prg.Period == 4) {
                classSchedule.FP4 = prg.Subject;
                classSchedule.FP4Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "fri" && prg.Period == 5) {
                classSchedule.FP5 = prg.Subject;
                classSchedule.FP5Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "fri" && prg.Period == 6) {
                classSchedule.FP6 = prg.Subject;
                classSchedule.FP6Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "fri" && prg.Period == 7) {
                classSchedule.FP7 = prg.Subject;
                classSchedule.FP7Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "fri" && prg.Period == 8) {
                classSchedule.FP8 = prg.Subject;
                classSchedule.FP8Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "fri" && prg.Period == 9) {
                classSchedule.FP9 = prg.Subject;
                classSchedule.FP9Id = prg.Id;
            }


        }


        


        public void InitializeForm() {
            //clear fields
            tbClass.Clear();
            tbTeacher.Clear();
            tbSubject.Clear();
            tbElective.Clear();
            //tbValidation.Text = "";
            //cmbDay.SelectedIndex = 0;


            btnAdd.IsEnabled = false;
            btnClear.IsEnabled = true;
            btnDelete.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnPrint.IsEnabled = false;
        }


        public void btnClear_Click(object sender, RoutedEventArgs e) {            
            ClearField();
        }

        private void ClearField() {
            tbClass.Clear();
            tbTeacher.Clear();
            tbSubject.Clear();
            tbElective.Clear();
            tbValidation.Text = "";
            cmbPeriodPerWeek.SelectedIndex = 0;

            btnAdd.IsEnabled = false;
            btnClear.IsEnabled = true;
            btnDelete.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnPrint.IsEnabled = false;

        }

        public void tb_GotFocus(object sender, RoutedEventArgs e) {
            if (!String.IsNullOrWhiteSpace(tbClass.Text) && !String.IsNullOrWhiteSpace(tbTeacher.Text)
                && !String.IsNullOrWhiteSpace(tbSubject.Text) && !String.IsNullOrWhiteSpace(cmbPeriodPerWeek.Text))
                EnableAddEditBtn();
        }


        public void EnableAddEditBtn() {
            btnAdd.IsEnabled = true;
            btnEdit.IsEnabled = true;
        }

        private void CmbDay_Loaded(object sender, RoutedEventArgs e) {

            List<string> days = new List<string>();
            days.Add("");
            days.Add("Mon");
            days.Add("thu");
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
            periods.Add("");
            periods.Add("1");
            periods.Add("2");
            periods.Add("3");
            periods.Add("4");
            periods.Add("5");
            periods.Add("6");

            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            comboBox.ItemsSource = periods;

            comboBox.SelectedIndex = 0;
        }

        private void CmbTimeTableFormat_Loaded(object sender, RoutedEventArgs e) {

            List<string> format = new List<string>();
            format.Add("General");
            format.Add("Subject");
            format.Add("Teacher");

            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            comboBox.ItemsSource = format;

            comboBox.SelectedIndex = 0;
        }

        private void GeneralDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        }

        private void GeneralDataGrid_SelectionChanged(object sender, RoutedEventArgs e) {
            DataGrid dataGrid = sender as DataGrid;
            IList<DataGridCellInfo>  selectedcells = dataGrid.SelectedCells;
            if (selectedcells.Count > 0) {
               var cell = dataGrid.SelectedItem;
               var value = dataGrid.SelectedValue;
            }

        }

        private void GeneralDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e) {
            //Get the newly selected cells
            IList<DataGridCellInfo> selectedCells = e.AddedCells;

            var selectedCell = selectedCells[0];

            DisplaySchedule(selectedCell);
            DataGrid dataGrid = sender as DataGrid;
          


            var value = dataGrid.SelectedValue;
            var path = dataGrid.SelectedValuePath;
        }

        private void DisplaySchedule(DataGridCellInfo selectedCell) {
            var dayPeriod = selectedCell.Column.SortMemberPath;
            
            int id;
            int period;
            string dayAlias;
            string prgClass;
            

            if (!String.IsNullOrEmpty(dayPeriod) && selectedCell.Item != null) {
                period = int.Parse(dayPeriod.Substring(dayPeriod.Length -1, 1));
                int pIndex = dayPeriod.IndexOf('P');
                dayAlias = dayPeriod.Substring(0, pIndex);
                var classProgrammeItems = selectedCell.Item as GeneralProgramme;
                id = (int)classProgrammeItems[dayPeriod + "Id"]; 
                prgClass = (string)classProgrammeItems["Class"];

                var scheduler = new TimeTableEntities.Scheduler();
                var schedule = scheduler.GetSchedule(id);
                if (schedule != null) {
                    tbClass.Text = prgClass;
                    tbTeacher.Text = schedule.Teacher;
                    tbElective.Text = schedule.Elective;
                    tbSubject.Text = schedule.Subject;
                    cmbPeriodPerWeek.Text = schedule.PeriodPerWeek.ToString();
                }
            }
           

        }



    }
}
