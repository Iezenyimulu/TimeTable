using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTable {
    public class SubjectModel : INotifyPropertyChanged{

        public int Id { get; set; }

        private string _name;
        public string Name {
            get {
                return _name;
            }
            set {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private string _alias;
        public string Alias {
            get {
                return _alias;
            }
            set {
                if (_alias == value) return;
                _alias = value;
                OnPropertyChanged("Alias");
            }
        }

        private bool _science;
        public bool Science {
            get {
                return _science;
            }
            set {
                if (_science == value) return;
                _science = value;
                OnPropertyChanged("Science");
            }
        }

        private bool _elective;
        public bool Elective {
            get {
                return _elective;
            }
            set {
                if (_elective == value) return;
                _elective = value;
                OnPropertyChanged("Elective");
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        //public string Error {
        //    get { throw new NotImplementedException(); }
        //}

        //public string this[string columnName] {
        //    get {
        //           //this get will be invoked everytime user change the Name or Age field in Datagird
        //       //columnName contains the property name which is modified by user.
        //        var timetableEntityMgr = new TimeTableEntities.TimetableEntityManager();
        //        string error = String.Empty;
        //        switch (columnName) {
        //            case "Name":
        //                if(string.IsNullOrEmpty(Name))
        //                       error = "Subject name cannot be empty";
                        
        //                break;
        //            case"Alias":
        //                if (Alias.Length > 5)
        //                    error = "Maximum string length required for Alias = 5";
        //                break;
        //        }
        //        return error;
        //    }
        //}
    }

}
