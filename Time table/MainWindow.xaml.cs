using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            Init();
        }

        int id;
        List<GeneralProgramme> generlProgrammeList;
        List<ClassProgramme> classProgrammeList;
        List<TeacherProgramme> teacherProgrammeList;
        List<string> classList;

        static List<SubjectModel> subjectList; 
        List<SubjectPeriodPerWeekModel> subjectPeriodPerWeekList;
        List<TeacherClassModel> teacherList;

        static int subjectId = 0;
        static int teacherClassId=0;

        public void Init() {
            //datagrid init
            TeacherDataGrid.Visibility = System.Windows.Visibility.Hidden;
            tbTimeTableFormatParam.Visibility = System.Windows.Visibility.Hidden;
            ClassDataGrid.Visibility = System.Windows.Visibility.Hidden;
            DaysListview.Visibility = System.Windows.Visibility.Visible;
            GeneralDataGrid.Visibility = System.Windows.Visibility.Visible;

            //clear fields
            tbPClass.Clear();
            tbPTeacher.Clear();
            //tbPSubject.Clear();
            //tbElective.Clear();
            //tbValidation.Text = "";
            //cmbDay.SelectedIndex = 0;

            tbTimeTableFormatParam.SelectAll();
            tbPClass.Focus();

            btnPAdd.IsEnabled = false;
            btnPClear.IsEnabled = true;
            btnPDelete.IsEnabled = false;
            btnPEdit.IsEnabled = false;
            btnPPrint.IsEnabled = true;

            GetSubject();
            GetSubjectPeriodPerWeek();
            GetClass();
            GetTeacher();
        }

        private void GetClass() {
            TimeTableEntities.TimetableEntityManager timetableManager = new TimeTableEntities.TimetableEntityManager();
            var classes = timetableManager.GetClass();
            classList = new List<string>();
            classList.AddRange(classes);
        }


        public void BtnPAdd_Click(object sender, RoutedEventArgs e) {
            var validationSuccess = ValidateUserInput();
            var scheduler = new TimeTableEntities.Scheduler();
            TimeTableEntities.ValidationModel validationMessage;
            if (validationSuccess) {
                validationMessage = scheduler.AddSchedule(CloneSchedule());
                if (validationMessage.Id != "Success") {
                    MessageBox.Show(validationMessage.Message, validationMessage.Id, MessageBoxButton.OK, MessageBoxImage.Error);
                    tbPValidation.Text = String.Format("{0}\n{1}", validationMessage.Id, validationMessage.Message);
                }
                GetTimeTable();
            }
        }


        private void BtnPEdit_Click(object sender, RoutedEventArgs e) {
            var validationSuccess = ValidateUserInput();
            var scheduler = new TimeTableEntities.Scheduler();
            TimeTableEntities.ValidationModel validationMessage;
            if (validationSuccess) {
                validationMessage = scheduler.EditSchedule(CloneSchedule());
                if (validationMessage.Id != "Success") {
                    MessageBox.Show(validationMessage.Message, validationMessage.Id, MessageBoxButton.OK, MessageBoxImage.Error);
                    tbPValidation.Text = String.Format("{0}\n{1}", validationMessage.Id, validationMessage.Message);
                }
                GetTimeTable();
            }

        }


        private void BtnPDelete_Click(object sender, RoutedEventArgs e) {
            var scheduler = new TimeTableEntities.Scheduler();
            //TimeTableEntities.ValidationModel validationMessage;
            scheduler.DeleteSchedule(id);

        }


        private bool ValidateUserInput() {

            var message = "";

            //if (tbPSubject.Text.Length > 5) {
            //    message += "Enter subject alias. Max length is 5.";
            //    MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return false;
            //}
            return true;
            //if (String.IsNullOrWhiteSpace(tbElective.Text)
            //    message = "If this is an elective subject, enter the elective code";           


        }


        private TimeTableEntities.ScheduleModel CloneSchedule() {
            var schedule = new TimeTableEntities.ScheduleModel();
            schedule.Class = tbPClass.Text;
            schedule.PeriodPerWeek = int.Parse(cmbPeriodPerWeek.Text);
            //schedule.Subject = tbPSubject.Text;
            schedule.Teacher = tbPTeacher.Text;

            return schedule;

        }


        public void GetTimeTable() {
            var scheduler = new TimeTableEntities.Scheduler();
            var format = cmbTimeTableFormat.SelectedItem;
            IEnumerable<TimeTableEntities.Schedule> schedules = scheduler.GetSchedule();
            switch (format.ToString().ToLower()) {

                case "class":
                    if (!String.IsNullOrWhiteSpace(tbTimeTableFormatParam.Text))
                        //schedules = scheduler.GetClassTimeTable(tbTimeTableFormatParam);
                        GetClassTimeTable(tbTimeTableFormatParam.Text, scheduler);
                    break;
                case "teacher":
                    if (!String.IsNullOrWhiteSpace(tbTimeTableFormatParam.Text))
                        //schedules = scheduler.GetTeacherSchedule(tbTimeTableFormatParam.Text);
                        GetTeacherTimeTable(tbTimeTableFormatParam.Text, scheduler);
                    else
                        MessageBox.Show("Enter teacher's name");
                    break;
                default:
                    GetGeneralTimeTable(scheduler);
                    break;
            }
        }

        private void GetClassTimeTable(string classParam, TimeTableEntities.Scheduler scheduler) {
            classProgrammeList = new List<ClassProgramme>();
            string[] recess = { "B", "R", "E", "A", "K" };
            var i = -1;
            var schedules = scheduler.GetClassSchedule(classParam);
            if (schedules != null && schedules.Count() > 0) {
                var scheduleGroup = from s in schedules
                                    //orderby s.Day, s.Period
                                    group s by s.Day into grouping
                                    select new {
                                        grouping.Key,
                                        grouping
                                    };

                foreach (var grp in scheduleGroup) {
                    ClassProgramme classSchedule = new ClassProgramme();
                    foreach (var prg in grp.grouping) {
                        CloneClassProgramme(grp.Key, prg, ref classSchedule);
                    }
                    classSchedule.Recess = recess[++i];
                    classProgrammeList.Add(classSchedule);
                }

            }
            ListCollectionView collectionView = new ListCollectionView(classProgrammeList);
            collectionView.GroupDescriptions.Add(new PropertyGroupDescription("Day"));
            ClassDataGrid.ItemsSource = collectionView;
        }


        private void CloneClassProgramme(string day, TimeTableEntities.Schedule prg, ref ClassProgramme classSchedule) {


            var subject = prg.Subject.ToUpper();
            classSchedule.Day = day;


            if (prg.Period == 1) {
                classSchedule.P1 = subject;
                classSchedule.P1Id = prg.Id;
            }
            else if (prg.Period == 2) {
                classSchedule.P2 = subject;
                classSchedule.P2Id = prg.Id;
            }
            else if (prg.Period == 3) {
                classSchedule.P3 = subject;
                classSchedule.P3Id = prg.Id;
            }
            else if (prg.Period == 4) {
                classSchedule.P4 = subject;
                classSchedule.P4Id = prg.Id;
            }
            else if (prg.Period == 5) {
                classSchedule.P5 = subject;
                classSchedule.P5Id = prg.Id;
            }
            else if (prg.Period == 6) {
                classSchedule.P6 = subject;
                classSchedule.P6Id = prg.Id;
            }
            else if (prg.Period == 7) {
                classSchedule.P7 = subject;
                classSchedule.P7Id = prg.Id;
            }
            else if (prg.Period == 8) {
                classSchedule.P8 = subject;
                classSchedule.P8Id = prg.Id;
            }
            else if (prg.Period == 9) {
                classSchedule.P9 = subject;
                classSchedule.P9Id = prg.Id;
            }
        }


        private void GetTeacherTimeTable(string teacherParam, TimeTableEntities.Scheduler scheduler) {
            teacherProgrammeList = new List<TeacherProgramme>();
            //string[] recess = { "B", "R", "E", "A", "K" };
            var schedules = scheduler.GetTeacherSchedule(teacherParam).ToList().OrderBy(s => s.Class, new ClassComparer());

            if (schedules != null && schedules.Count() > 0) {
                var scheduleGroup = from s in schedules
                                    group s by s.Day into grouping
                                    select new {
                                        grouping.Key,
                                        grouping
                                    };

                foreach (var grp in scheduleGroup) {
                    TeacherProgramme teacherSchedule = new TeacherProgramme();
                    foreach (var prg in grp.grouping) {
                        CloneTeacherProgramme(grp.Key, prg, ref teacherSchedule);
                    }
                    teacherProgrammeList.Add(teacherSchedule);
                }

            }
            else {
                MessageBox.Show("Check that the name has been entered correctly", "Teacher Does Not Exist");
            }
            ListCollectionView collectionView = new ListCollectionView(teacherProgrammeList);
            collectionView.GroupDescriptions.Add(new PropertyGroupDescription("Class"));
            TeacherDataGrid.ItemsSource = collectionView;
        }


        private void CloneTeacherProgramme(string day, TimeTableEntities.Schedule prg, ref TeacherProgramme teacherSchedule) {

            var subject = prg.Subject.ToUpper();
            teacherSchedule.Day = day;


            if (prg.Period == 1) {
                teacherSchedule.P1 = prg.Class.ToUpper() + " - " + subject;
                teacherSchedule.P1Id = prg.Id;
            }
            else if (prg.Period == 2) {
                teacherSchedule.P2 = subject;
                teacherSchedule.P2Id = prg.Id;
            }
            else if (prg.Period == 3) {
                teacherSchedule.P3 = prg.Class.ToUpper() + " - " + subject;
                teacherSchedule.P3Id = prg.Id;
            }
            else if (prg.Period == 4) {
                teacherSchedule.P4 = prg.Class.ToUpper() + " - " + subject;
                teacherSchedule.P4Id = prg.Id;
            }
            else if (prg.Period == 5) {
                teacherSchedule.P5 = prg.Class.ToUpper() + " - " + subject;
                teacherSchedule.P5Id = prg.Id;
            }
            else if (prg.Period == 6) {
                teacherSchedule.P6 = prg.Class.ToUpper() + " - " + subject;
                teacherSchedule.P6Id = prg.Id;
            }
            else if (prg.Period == 7) {
                teacherSchedule.P7 = prg.Class.ToUpper() + " - " + subject;
                teacherSchedule.P7Id = prg.Id;
            }
            else if (prg.Period == 8) {
                teacherSchedule.P8 = prg.Class.ToUpper() + " - " + subject;
                teacherSchedule.P8Id = prg.Id;
            }
            else if (prg.Period == 9) {
                teacherSchedule.P9 = prg.Class.ToUpper() + " - " + subject;
                teacherSchedule.P9Id = prg.Id;
            }


        }


        public void GetGeneralTimeTable(TimeTableEntities.Scheduler scheduler) {
            generlProgrammeList = new List<GeneralProgramme>();
            var schedules = scheduler.GetSchedule().ToList().OrderBy(s => s.Class, new ClassComparer());
            var scheduleGroup = from s in schedules
                                //orderby s.Day, s.Period
                                group s by s.Class into grouping
                                select new {
                                    grouping.Key,
                                    grouping
                                };

            foreach (var grp in scheduleGroup) {
                GeneralProgramme generalSchedule = new GeneralProgramme();
                foreach (var prg in grp.grouping) {
                    CloneGeneralProgramme(grp.Key, prg, ref generalSchedule);
                }
                generlProgrammeList.Add(generalSchedule);
            }

            ListCollectionView collectionView = new ListCollectionView(generlProgrammeList);
            collectionView.GroupDescriptions.Add(new PropertyGroupDescription("Class"));
            GeneralDataGrid.ItemsSource = collectionView;

            ClearField();
        }


        private void CloneGeneralProgramme(string grpKey, TimeTableEntities.Schedule prg, ref GeneralProgramme generalSchedule) {

            generalSchedule.Class = grpKey.ToUpper();

            prg.Subject = prg.Subject.ToUpper();


            //mondayday
            if (prg.Day.ToLower() == "monday" && prg.Period == 1) {
                generalSchedule.MP1 = prg.Subject;
                generalSchedule.MP1Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "monday" && prg.Period == 2) {
                generalSchedule.MP2 = prg.Subject;
                generalSchedule.MP2Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "monday" && prg.Period == 3) {
                generalSchedule.MP3 = prg.Subject;
                generalSchedule.MP3Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "monday" && prg.Period == 4) {
                generalSchedule.MP4 = prg.Subject;
                generalSchedule.MP4Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "monday" && prg.Period == 5) {
                generalSchedule.MP5 = prg.Subject;
                generalSchedule.MP5Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "monday" && prg.Period == 6) {
                generalSchedule.MP6 = prg.Subject;
                generalSchedule.MP6Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "monday" && prg.Period == 7) {
                generalSchedule.MP7 = prg.Subject;
                generalSchedule.MP7Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "monday" && prg.Period == 8) {
                generalSchedule.MP8 = prg.Subject;
                generalSchedule.MP8Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "monday" && prg.Period == 9) {
                generalSchedule.MP9 = prg.Subject;
                generalSchedule.MP9Id = prg.Id;
            }

          //tuesdayday
            else if (prg.Day.ToLower() == "tuesday" && prg.Period == 1) {
                generalSchedule.TP1 = prg.Subject;
                generalSchedule.TP1Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "tuesday" && prg.Period == 2) {
                generalSchedule.TP2Id = prg.Id;
                generalSchedule.TP2 = prg.Subject;
            }
            else if (prg.Day.ToLower() == "tuesday" && prg.Period == 3) {
                generalSchedule.TP3 = prg.Subject;
                generalSchedule.TP3Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "tuesday" && prg.Period == 4) {
                generalSchedule.TP4 = prg.Subject;
                generalSchedule.TP4Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "tuesday" && prg.Period == 5) {
                generalSchedule.TP5 = prg.Subject;
                generalSchedule.TP5Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "tuesday" && prg.Period == 6) {
                generalSchedule.TP6 = prg.Subject;
                generalSchedule.TP6Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "tuesday" && prg.Period == 7) {
                generalSchedule.TP7 = prg.Subject;
                generalSchedule.TP7Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "tuesday" && prg.Period == 8) {
                generalSchedule.TP8 = prg.Subject;
                generalSchedule.TP8Id = prg.Id;
            }

            else if (prg.Day.ToLower() == "tuesday" && prg.Period == 9) {
                generalSchedule.TP9 = prg.Subject;
                generalSchedule.TP9Id = prg.Id;
            }
            //wednesdaynesday
            else if (prg.Day.ToLower() == "wednesday" && prg.Period == 1) {
                generalSchedule.WP1 = prg.Subject;
                generalSchedule.WP1Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "wednesday" && prg.Period == 2) {
                generalSchedule.WP2 = prg.Subject;
                generalSchedule.WP2Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "wednesday" && prg.Period == 3) {
                generalSchedule.WP3 = prg.Subject;
                generalSchedule.WP3Id = prg.Id;
            }

            else if (prg.Day.ToLower() == "wednesday" && prg.Period == 4) {
                generalSchedule.WP4 = prg.Subject;
                generalSchedule.WP4Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "wednesday" && prg.Period == 5) {
                generalSchedule.WP5 = prg.Subject;
                generalSchedule.WP5Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "wednesday" && prg.Period == 6) {
                generalSchedule.WP6 = prg.Subject;
                generalSchedule.WP6Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "wednesday" && prg.Period == 7) {
                generalSchedule.WP7 = prg.Subject;
                generalSchedule.WP7Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "wednesday" && prg.Period == 8) {
                generalSchedule.WP8 = prg.Subject;
                generalSchedule.WP8Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "wednesday" && prg.Period == 9) {
                generalSchedule.WP9 = prg.Subject;
                generalSchedule.WP9Id = prg.Id;
            }
            //thursdaysday
            else if (prg.Day.ToLower() == "thursday" && prg.Period == 1) {
                generalSchedule.THP1 = prg.Subject;
                generalSchedule.THP1Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "thursday" && prg.Period == 2) {
                generalSchedule.THP2 = prg.Subject;
                generalSchedule.THP2Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "thursday" && prg.Period == 3) {
                generalSchedule.THP3 = prg.Subject;
                generalSchedule.THP3Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "thursday" && prg.Period == 4) {
                generalSchedule.THP4 = prg.Subject;
                generalSchedule.THP4Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "thursday" && prg.Period == 5) {
                generalSchedule.THP5 = prg.Subject;
                generalSchedule.THP5Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "thursday" && prg.Period == 6) {
                generalSchedule.THP6 = prg.Subject;
                generalSchedule.THP6Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "thursday" && prg.Period == 7) {
                generalSchedule.THP7 = prg.Subject;
                generalSchedule.THP7Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "thursday" && prg.Period == 8) {
                generalSchedule.THP8 = prg.Subject;
                generalSchedule.THP8Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "thursday" && prg.Period == 9) {
                generalSchedule.THP9 = prg.Subject;
                generalSchedule.THP9Id = prg.Id;
            }
            //fridayday
            else if (prg.Day.ToLower() == "friday" && prg.Period == 1) {
                generalSchedule.FP1 = prg.Subject;
                generalSchedule.FP1Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "friday" && prg.Period == 2) {
                generalSchedule.FP2 = prg.Subject;
                generalSchedule.FP2Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "friday" && prg.Period == 3) {
                generalSchedule.FP3 = prg.Subject;
                generalSchedule.FP3Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "friday" && prg.Period == 4) {
                generalSchedule.FP4 = prg.Subject;
                generalSchedule.FP4Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "friday" && prg.Period == 5) {
                generalSchedule.FP5 = prg.Subject;
                generalSchedule.FP5Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "friday" && prg.Period == 6) {
                generalSchedule.FP6 = prg.Subject;
                generalSchedule.FP6Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "friday" && prg.Period == 7) {
                generalSchedule.FP7 = prg.Subject;
                generalSchedule.FP7Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "friday" && prg.Period == 8) {
                generalSchedule.FP8 = prg.Subject;
                generalSchedule.FP8Id = prg.Id;
            }
            else if (prg.Day.ToLower() == "friday" && prg.Period == 9) {
                generalSchedule.FP9 = prg.Subject;
                generalSchedule.FP9Id = prg.Id;
            }


        }




        public void BtnPClear_Click(object sender, RoutedEventArgs e) {
            ClearField();
        }

        private void ClearField() {
            tbPClass.Clear();
            tbPTeacher.Clear();
            //tbPSubject.Clear();
            //tbElective.Clear();
            tbPValidation.Text = "";
            cmbPeriodPerWeek.SelectedIndex = 0;

            btnPAdd.IsEnabled = false;
            btnPClear.IsEnabled = true;
            btnPDelete.IsEnabled = false;
            btnPEdit.IsEnabled = false;
            btnPPrint.IsEnabled = false;

        }

        public void tb_GotFocus(object sender, RoutedEventArgs e) {
            //    if (!String.IsNullOrWhiteSpace(tbPClass.Text) && !String.IsNullOrWhiteSpace(tbPTeacher.Text)
            //        && !String.IsNullOrWhiteSpace(tbPSubject.Text) && !String.IsNullOrWhiteSpace(cmbPeriodPerWeek.Text))
            //        EnableAddEditBtn();
        }


        public void EnableAddEditBtn() {
            btnPAdd.IsEnabled = true;
            btnPEdit.IsEnabled = true;
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


        private void CmbPSubject_Loaded(object sender, RoutedEventArgs e) {

            List<string> periods = new List<string>();
            periods.AddRange(subjectList.Select(s => s.Name));

            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            comboBox.ItemsSource = periods;

            comboBox.SelectedIndex = 0;
        }


        private void CmbTimeTableFormat_Loaded(object sender, RoutedEventArgs e) {

            List<string> format = new List<string>();
            format.Add("General");
            format.Add("Class");
            //format.Add("Subject");
            format.Add("Teacher");

            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            comboBox.ItemsSource = format;

            comboBox.SelectedIndex = 0;
        }

        private void GeneralDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e) {
            //Get the newly selected cells
            IList<DataGridCellInfo> selectedCells = e.AddedCells;

            var selectedCell = selectedCells[0];
            if (selectedCells.Count > 0)
                DisplaySchedule(selectedCell);
            DataGrid dataGrid = sender as DataGrid;



            var value = dataGrid.SelectedValue;
            var path = dataGrid.SelectedValuePath;
        }

        private void DisplaySchedule(DataGridCellInfo selectedCell) {

            if (!String.IsNullOrEmpty(selectedCell.Column.SortMemberPath) && selectedCell.Item != null) {

                var scheduler = new TimeTableEntities.Scheduler();
                TimeTableEntities.ScheduleModel schedule;

                int period;
                string periodHeader;
                string prgClass;

                var format = cmbTimeTableFormat.Text;
                switch (format.ToLower()) {
                    case "class":
                        periodHeader = selectedCell.Column.SortMemberPath;
                        period = int.Parse(periodHeader.Substring(1));
                        prgClass = tbTimeTableFormatParam.Text;
                        var classProgrammeItems = selectedCell.Item as ClassProgramme;
                        id = (int)classProgrammeItems[periodHeader + "Id"];


                        schedule = scheduler.GetSchedule(id);
                        if (schedule != null) {
                            tbPClass.Text = prgClass;
                            tbPTeacher.Text = schedule.Teacher;
                            //tbElective.Text = schedule.Elective;
                            //tbPSubject.Text = schedule.Subject;
                            cmbPeriodPerWeek.Text = schedule.PeriodPerWeek.ToString();
                        }
                        break;

                    case "teacher":
                        if ((string)selectedCell.Column.Header != "Day") {
                            periodHeader = selectedCell.Column.SortMemberPath;
                            period = int.Parse(periodHeader.Substring(1));
                            var teacherProgrammeItems = selectedCell.Item as TeacherProgramme;
                            id = (int)teacherProgrammeItems[periodHeader + "Id"];

                            schedule = scheduler.GetSchedule(id);
                            if (schedule != null) {
                                tbPClass.Text = schedule.Class;
                                tbPTeacher.Text = schedule.Teacher;
                                //tbElective.Text = schedule.Elective;
                                //tbPSubject.Text = schedule.Subject;
                                cmbPeriodPerWeek.Text = schedule.PeriodPerWeek.ToString();
                            }
                        }
                        break;

                    case "general":
                        var dayPeriod = selectedCell.Column.SortMemberPath;
                        string dayAlias;
                        period = int.Parse(dayPeriod.Substring(dayPeriod.Length - 1, 1));
                        int pIndex = dayPeriod.IndexOf('P');
                        dayAlias = dayPeriod.Substring(0, pIndex);
                        var generalProgrammeItems = selectedCell.Item as GeneralProgramme;
                        id = GetId(generalProgrammeItems, dayPeriod);
                        prgClass = (string)generalProgrammeItems["Class"];

                        schedule = scheduler.GetSchedule(id);
                        if (schedule != null) {
                            tbPClass.Text = prgClass;
                            tbPTeacher.Text = schedule.Teacher;
                            //tbElective.Text = schedule.Elective;
                            //tbPSubject.Text = schedule.Subject;
                            cmbPeriodPerWeek.Text = schedule.PeriodPerWeek.ToString();
                        }
                        break;
                }
            }


        }


        private int GetId(GeneralProgramme generalProgrammeItems, string dayPeriod) {
            return (int)generalProgrammeItems[dayPeriod + "Id"];
        }


        private void CmbPTimeTableFormat_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var selectedFormat = e.AddedItems[0];
            if (selectedFormat.ToString().Trim() == "Class") {
                tbTimeTableFormatParam.Text = "Enter Class";
                TeacherDataGrid.Visibility = System.Windows.Visibility.Hidden;
                DaysListview.Visibility = System.Windows.Visibility.Hidden;
                GeneralDataGrid.Visibility = System.Windows.Visibility.Hidden;
                tbTimeTableFormatParam.Visibility = System.Windows.Visibility.Visible;
                ClassDataGrid.Visibility = System.Windows.Visibility.Visible;
                tbTimeTableFormatParam.SelectAll();
            }
            //else if (selectedFormat.ToString().Trim() == "Subject") {
            //    tbTimeTableFormatParam.Text = "Enter Subject";
            //    tbTimeTableFormatParam.Visibility = System.Windows.Visibility.Visible;
            //    DaysListview.Visibility = System.Windows.Visibility.Hidden;
            //    GeneralDataGrid.Visibility = System.Windows.Visibility.Hidden;
            //}
            else if (selectedFormat.ToString().Trim() == "Teacher") {
                tbTimeTableFormatParam.Text = "Enter Name";
                DaysListview.Visibility = System.Windows.Visibility.Hidden;
                GeneralDataGrid.Visibility = System.Windows.Visibility.Hidden;
                ClassDataGrid.Visibility = System.Windows.Visibility.Hidden;
                TeacherDataGrid.Visibility = System.Windows.Visibility.Visible;
                tbTimeTableFormatParam.Visibility = System.Windows.Visibility.Visible;
            }
            else {
                tbTimeTableFormatParam.Visibility = System.Windows.Visibility.Hidden;
                ClassDataGrid.Visibility = System.Windows.Visibility.Hidden;
                TeacherDataGrid.Visibility = System.Windows.Visibility.Hidden;
                DaysListview.Visibility = System.Windows.Visibility.Visible;
                GeneralDataGrid.Visibility = System.Windows.Visibility.Visible;
                GetTimeTable();
            }

        }


        private void tbTimeTableFormatParam_KeyDown(object sender, KeyEventArgs e) {

            if (e.Key == Key.Enter || e.Key == Key.Return) {
                GetTimeTable();
            }

        }


        private void ClassDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e) {

            //Get the newly selected cells
            IList<DataGridCellInfo> selectedCells = e.AddedCells;

            var selectedCell = selectedCells[0];
            if (selectedCells.Count > 0)
                DisplaySchedule(selectedCell);
            DataGrid dataGrid = sender as DataGrid;



            var value = dataGrid.SelectedValue;
            var path = dataGrid.SelectedValuePath;

        }


        private void TeacherDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e) {
            //Get the newly selected cells
            IList<DataGridCellInfo> selectedCells = e.AddedCells;

            var selectedCell = selectedCells[0];
            if (selectedCells.Count > 0)
                DisplaySchedule(selectedCell);
            DataGrid dataGrid = sender as DataGrid;



            var value = dataGrid.SelectedValue;
            var path = dataGrid.SelectedValuePath;
        }

        //subject tab
        private void GetSubject() {
            var timetablEntityMgr = new TimeTableEntities.TimetableEntityManager();
            subjectList = new List<SubjectModel>();

            var subjects = timetablEntityMgr.GetSubject().OrderBy(s => s.Name);
            if (subjects.Count() > 0) {
                CopyItemToModel(subjects);
                SubjectDatagrid.ItemsSource = subjectList;
                tbTotalSubject.Text = "Total Subject : " + subjectList.Count();
            }

        }


        private void CopyItemToModel(IEnumerable<TimeTableEntities.Subject> subjects) {
            foreach (var s in subjects) {
                var subject = new SubjectModel();
                subject.Id = s.Id;
                subject.Name = s.Name;
                subject.Alias = s.Alias;
                subject.Science = s.IsScience ;
                subject.Elective = s.IsElective ;
                subjectList.Add(subject);
            }
        }


        private void CmbSCourse_Loaded(object sender, RoutedEventArgs e) {

            List<string> courses = new List<string>();
            courses.Add("");
            courses.Add("Art");
            courses.Add("Science");
            courses.Add("Business");
            courses.Add("General");

            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            comboBox.ItemsSource = courses;
        }


        private void CmbSLevel_Loaded(object sender, RoutedEventArgs e) {

            List<string> levels = new List<string>();
            levels.Add("");
            levels.Add("JS");
            levels.Add("SS1");
            levels.Add("SS2&3");

            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            comboBox.ItemsSource = levels;

            comboBox.SelectedIndex = 0;
        }


        private void CmbSPeriodPerWeek_Loaded(object sender, RoutedEventArgs e) {

            List<string> periodPerWeek = new List<string>();
            periodPerWeek.Add("");
            periodPerWeek.Add("1");
            periodPerWeek.Add("2");
            periodPerWeek.Add("3");
            periodPerWeek.Add("4");
            periodPerWeek.Add("5");
            periodPerWeek.Add("6");

            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            comboBox.ItemsSource = periodPerWeek;

            comboBox.SelectedIndex = 0;
        }


        private void SubjectDatagrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e) {
            //var datagrid = sender as DataGrid;
            //var existing = datagrid.SelectedItems[0] as SubjectModel;
            //var subjectModel = e.Row.Item as SubjectModel;
            //var timetableEntityMgr = new TimeTableEntities.TimetableEntityManager();

            //var subject = new TimeTableEntities.Subject();

            //subject.Name = subjectModel.Name;
            //subject.Alias = subjectModel.Alias;
            //subject.IsElective = subjectModel.Elective;
            //subject.IsScience = subjectModel.Science;

            //if (!e.Row.IsNewItem) {
            //    subject.Id = existing.Id;
            //}
            //if (e.EditAction == DataGridEditAction.Commit) {

            //}
            #region
            //if (e.EditingElement.GetType() == typeof(System.Windows.Controls.CheckBox)) {
            //    var element = e.EditingElement as CheckBox;
            //    existing.Science = element.IsChecked ?? false;
            //    existing.Elective = element.IsChecked ?? false;

            //}
            //else {
            //    var element = e.EditingElement as TextBox;
            //    string text = element.Text;

            //    if (text.All(Char.IsLetter)) {
            //        if ((string)e.Column.Header == "Alias" && text.Length > 4) {
            //            errorMessage = "maximum string length required for Alias = 4";
            //            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //            e.Cancel = true;
            //        }
            //        else {
            //            existing.Name = text;
            //            existing.Alias = text;
            //        }
            //    }
            //    else {
            //        errorMessage = "Wrong character enetered\n Only letters are allowed. \n\n Example; Subject: Integrated Science. Alias: ISC";
            //        MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //        e.Cancel = true;
            //    }
            //}
            #endregion


        }

        private void SubjectDatagrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e) {
            //Get the newly selected cells
            IList<DataGridCellInfo> selectedCells = e.AddedCells;

            if (selectedCells.Count > 0) {
                DisplaySubject(selectedCells[0]);

                DataGrid dataGrid = sender as DataGrid;

                var value = dataGrid.SelectedValue;
                var path = dataGrid.SelectedValuePath;
            }

        }


        private void DisplaySubject(DataGridCellInfo selectedCell) {
            //cmbSPeriodPerWeek.SelectedIndex = 0;
            if (!String.IsNullOrEmpty(selectedCell.Column.SortMemberPath) && selectedCell.Item != null) {

                if (selectedCell.Item.GetType() == typeof(TimeTable.SubjectModel)) {
                    subjectId = ((TimeTable.SubjectModel)(selectedCell.Item)).Id;
                    var subjectPeriodPerWeek = (from s in subjectPeriodPerWeekList
                                                where s.Subject == ((TimeTable.SubjectModel)(selectedCell.Item)).Alias
                                                select s).FirstOrDefault();
                    tbSSubject.Text = ((TimeTable.SubjectModel)(selectedCell.Item)).Name;
                    tbSAlias.Text = ((TimeTable.SubjectModel)(selectedCell.Item)).Alias;
                    cmbSCourse.SelectedItem = subjectPeriodPerWeek.Course;
                    cmbSLevel.SelectedItem = subjectPeriodPerWeek.Level;
                    cmbSPeriodPerWeek.SelectedItem = (subjectPeriodPerWeek.PeriodPerWeek).ToString();
                    chSScience.IsChecked = ((TimeTable.SubjectModel)(selectedCell.Item)).Science;
                    chSElective.IsChecked = ((TimeTable.SubjectModel)(selectedCell.Item)).Elective;
                }
                else if (selectedCell.Item.GetType() == typeof(TimeTable.SubjectPeriodPerWeekModel)) {
                    var subject = (from s in subjectList
                                   where s.Alias == ((TimeTable.SubjectPeriodPerWeekModel)(selectedCell.Item)).Subject
                                   select s).FirstOrDefault();
                    subjectId = subject.Id;
                    tbSSubject.Text = subject.Name;
                    tbSAlias.Text = ((TimeTable.SubjectPeriodPerWeekModel)(selectedCell.Item)).Subject;
                    cmbSCourse.SelectedItem = ((TimeTable.SubjectPeriodPerWeekModel)(selectedCell.Item)).Course;
                    cmbSLevel.SelectedItem = ((TimeTable.SubjectPeriodPerWeekModel)(selectedCell.Item)).Level;
                    cmbSPeriodPerWeek.SelectedItem = (((TimeTable.SubjectPeriodPerWeekModel)(selectedCell.Item)).PeriodPerWeek).ToString();
                    chSScience.IsChecked = subject.Science;
                    chSElective.IsChecked = subject.Elective;
                }
            }
        }


        public void btnAddSubject_Click(object sender, RoutedEventArgs e) {
            var timetableEntityManager = new TimeTableEntities.TimetableEntityManager();
            var dbSubject = new TimeTableEntities.Subject();
            var dbSubjectPeriodPerWeek = new TimeTableEntities.SubjectPeriodPerWeek();
            if (!subjectList.Exists(s => s.Name == tbSSubject.Text.Trim())) {
                dbSubject.Alias = tbSAlias.Text.Trim().ToUpper();
                dbSubject.Name = tbSSubject.Text.Trim();
                dbSubject.IsScience = chSScience.IsChecked??false;
                dbSubject.IsElective = chSElective.IsChecked??false;
                timetableEntityManager.AddSubject(dbSubject);
                GetSubject();
            }
            {
                dbSubjectPeriodPerWeek.Subject = tbSAlias.Text.Trim().ToUpper();
                dbSubjectPeriodPerWeek.PeriodPerWeek = int.Parse(cmbSPeriodPerWeek.SelectedValue.ToString() == "" ? "0" :
                    cmbSPeriodPerWeek.SelectedValue.ToString());
                dbSubjectPeriodPerWeek.Level = (string)cmbSLevel.SelectedValue;
                dbSubjectPeriodPerWeek.Course = (string)cmbSCourse.SelectedValue;
                timetableEntityManager.AddSubjectPeriodPerWeek(dbSubjectPeriodPerWeek);
                GetSubjectPeriodPerWeek();
            }
        }


        public void btnEditSubject_Click(object sender, RoutedEventArgs e) {
            var timetableEntityManager = new TimeTableEntities.TimetableEntityManager();
            var dbSubject = new TimeTableEntities.Subject();
            var dbSubjectPeriodPerWeek = new TimeTableEntities.SubjectPeriodPerWeek();
            dbSubject.Alias = tbSAlias.Text.Trim().ToUpper();
            dbSubject.Name = tbSSubject.Text.Trim();
            dbSubject.IsScience = chSScience.IsChecked ?? false;
            dbSubject.IsElective = chSElective.IsChecked ?? false;
            timetableEntityManager.EditSubject(dbSubject);

            dbSubjectPeriodPerWeek.Subject = tbSAlias.Text.Trim().ToUpper();
            dbSubjectPeriodPerWeek.PeriodPerWeek = int.Parse(cmbSPeriodPerWeek.SelectedValue.ToString() == "" ? "0" :
                    cmbSPeriodPerWeek.SelectedValue.ToString());
            dbSubjectPeriodPerWeek.Level = (string)cmbSLevel.SelectedValue;
            dbSubjectPeriodPerWeek.Course = (string)cmbSCourse.SelectedValue;
            timetableEntityManager.EditSubjectPeriodPerWeek(dbSubjectPeriodPerWeek);

            GetSubject();
            GetSubjectPeriodPerWeek();
        }


        public void btnClearSubject_Click(object sender, RoutedEventArgs e) {
            tbSSubject.Clear();
            tbSAlias.Clear();
            cmbSCourse.SelectedIndex = 0;
            cmbSLevel.SelectedIndex = 0;
            cmbSPeriodPerWeek.SelectedIndex = 0;
            chSScience.IsChecked = false;
            chSElective.IsChecked = false;
        }


        public void btnDeleteSubject_Click(object sender, RoutedEventArgs e) {
            var timetableEntityManager = new TimeTableEntities.TimetableEntityManager();
            var dbSubjectPeriodPerWeek = new TimeTableEntities.SubjectPeriodPerWeek();

            {
                //SubjectPeriodPerWeekDatagrid.SelectedIndex = -1;
                dbSubjectPeriodPerWeek.Subject = tbSAlias.Text.Trim();
                dbSubjectPeriodPerWeek.PeriodPerWeek = int.Parse(cmbSPeriodPerWeek.SelectedValue.ToString() == "" ? "0" :
                    cmbSPeriodPerWeek.SelectedValue.ToString());
                dbSubjectPeriodPerWeek.Level = (string)cmbSLevel.SelectedValue;
                dbSubjectPeriodPerWeek.Course = (string)cmbSCourse.SelectedValue;
                timetableEntityManager.DeleteSubjectPeriodPerWeek(dbSubjectPeriodPerWeek);
                GetSubjectPeriodPerWeek();
            }
            var sppw = (from s in subjectPeriodPerWeekList
                        where s.Subject == tbSAlias.Text.Trim()
                        select s);
            if (sppw.Count() == 0) {
              
                var id = (from s in subjectList
                          where s.Name == tbSSubject.Text &&
                          s.Alias == tbSAlias.Text
                          select s.Id).FirstOrDefault();
                timetableEntityManager.DeleteSubject(id);
                GetSubject();
                btnClearSubject_Click(sender, e);
            }

        }


        //subject period per week
        private void GetSubjectPeriodPerWeek() {
            var timetablEntityMgr = new TimeTableEntities.TimetableEntityManager();
            subjectPeriodPerWeekList = new List<SubjectPeriodPerWeekModel>();

            var subjectPeriodPerWeek = timetablEntityMgr.GetSubjectPeriodPerWeek().OrderBy(s => s.Subject);
            if (subjectPeriodPerWeek.Count() > 0) {
                CopyItemToModel(subjectPeriodPerWeek);
                SubjectPeriodPerWeekDatagrid.ItemsSource = subjectPeriodPerWeekList;
            }

        }


        private void CopyItemToModel(IEnumerable<TimeTableEntities.SubjectPeriodPerWeek> subjectPeriodPerWeek) {
            foreach (var s in subjectPeriodPerWeek) {
                var sppw = new SubjectPeriodPerWeekModel();
                sppw.Subject = s.Subject;
                sppw.Level = s.Level;
                sppw.PeriodPerWeek = s.PeriodPerWeek;
                sppw.Course = s.Course;
                subjectPeriodPerWeekList.Add(sppw);
            }
        }


        //teacher tab
        private void GetTeacher() {
            var timetablEntityMgr = new TimeTableEntities.TimetableEntityManager();
            teacherList = new List<TeacherClassModel>();

            var teachers = timetablEntityMgr.GetTeacherClass().OrderBy(s => s.Name);
            if (teachers.Count() > 0) {
                CopyItemToModel(teachers);
                TeacherClassDatagrid.ItemsSource = teacherList;
            }

        }


        private void TeacherClassDatagrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e) {
            //Get the newly selected cells
            IList<DataGridCellInfo> selectedCells = e.AddedCells;
            
            if (selectedCells.Count > 0) {             
                DisplayTeacherClass(selectedCells[0]);

                DataGrid dataGrid = sender as DataGrid;

                var value = dataGrid.SelectedValue;
                var path = dataGrid.SelectedValuePath;
            }

        }
               

        private void DisplayTeacherClass(DataGridCellInfo selectedCell) {
            teacherClassId = ((TimeTable.TeacherClassModel)(selectedCell.Item)).Id;
            tbTName.Text = ((TimeTable.TeacherClassModel)(selectedCell.Item)).Name;
            cmbTClass.SelectedItem = ((TimeTable.TeacherClassModel)(selectedCell.Item)).Class;
            cmbTSubject.SelectedItem = ((TimeTable.TeacherClassModel)(selectedCell.Item)).Subject;
        }


        private void CopyItemToModel(IEnumerable<TimeTableEntities.TeacherClass> teachers) {
            foreach (var t in teachers) {
                var teacher = new TeacherClassModel();
                teacher.Id = t.Id;
                teacher.Name = t.Name;
                teacher.Class = t.Class;
                teacher.Subject = t.Subject;
                teacherList.Add(teacher);
            }
        }

        private void CmbTSubject_Loaded(object sender, RoutedEventArgs e) {

            List<string> teacherClass = new List<string>();
            teacherClass.Add("");
            teacherClass.AddRange(subjectList.Select(s => s.Name));

            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            comboBox.ItemsSource = teacherClass;

            comboBox.SelectedIndex = 0;
        }


        private void CmbTClass_Loaded(object sender, RoutedEventArgs e) {

            List<string> tClass = new List<string>();
            tClass.Add("");
            tClass.AddRange(classList);

            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            comboBox.ItemsSource = tClass;

            comboBox.SelectedIndex = 0;
        }


        public void btnAddTeacher_Click(object sender, RoutedEventArgs e) {           
            var timetableEntityManager = new TimeTableEntities.TimetableEntityManager();
            var dbTeacher = new TimeTableEntities.TeacherClass();
           
                if (ValidateTeacherClassSuccess()) {
                    dbTeacher.Name = tbTName.Text.Trim().ToLower();
                    dbTeacher.Class = cmbTClass.SelectedValue.ToString();
                    dbTeacher.Subject = cmbTSubject.SelectedValue.ToString();
                    timetableEntityManager.AddTeacherClass(dbTeacher);
                    GetTeacher();
                }       
        }
           

        public void btnEditTeacher_Click(object sender, RoutedEventArgs e) {              
            var timetableEntityManager = new TimeTableEntities.TimetableEntityManager();
            var dbTeacher = new TimeTableEntities.TeacherClass();


            if (ValidateTeacherClassSuccess()) {
                dbTeacher.Id = teacherClassId;
                dbTeacher.Name = tbTName.Text.Trim().ToLower();
                dbTeacher.Class = cmbTClass.SelectedValue.ToString();
                dbTeacher.Subject = cmbTSubject.SelectedValue.ToString();
                timetableEntityManager.EditTeacherClass(dbTeacher);
                GetTeacher();
            }
        }

        private bool ValidateTeacherClassSuccess() {
            var result = true;
            TbTErroMessage.Text = "";
            if (String.IsNullOrWhiteSpace(tbTName.Text)) {
                TbTErroMessage.Text = "Enter teacher's name \n";
                result = false;
            }
             if (String.IsNullOrWhiteSpace(cmbTSubject.SelectedValue.ToString())) {
                TbTErroMessage.Text += "Enter subject \n";
                result = false; 
            }
           if (String.IsNullOrWhiteSpace(cmbTClass.SelectedValue.ToString())) {
                TbTErroMessage.Text += "Enter class";
                result = false;
            }
            else if (teacherList.Exists(t => t.Name == tbTName.Text.Trim().ToLower() &&
                          t.Subject == cmbTSubject.SelectedValue.ToString() &&
                          t.Class == cmbTClass.SelectedValue.ToString())) {
                TbTErroMessage.Text = "Teacher  for this class and subject already exist";
                result = false;
            }
            else if (teacherList.Exists(t=> t.Class ==cmbTClass.SelectedValue.ToString() &&
                          t.Subject == cmbTSubject.SelectedValue.ToString())) {
                TbTErroMessage.Text = "Another Teacher has been assigned for this subject and for this class \n ";
                result = false;
            }
            return result;
        }


        public void btnClearTeacher_Click(object sender, RoutedEventArgs e) {
            tbTName.Clear();
            cmbTClass.SelectedIndex = 0;
            TbTErroMessage.Text = "";
            cmbTSubject.SelectedIndex = 0;
            chTElective.IsChecked = false;
        }


        public void btnDeleteTeacher_Click(object sender, RoutedEventArgs e) {
            var timetableEntityManager = new TimeTableEntities.TimetableEntityManager();
            var dbTeacher = new TimeTableEntities.TeacherClass();
            timetableEntityManager.DeleteTeacherClass(teacherClassId);

            GetTeacher();
            btnClearTeacher_Click(sender, e);

        }


    }
}
