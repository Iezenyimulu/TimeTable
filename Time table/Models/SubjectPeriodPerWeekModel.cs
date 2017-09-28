using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTable {
    public class SubjectPeriodPerWeekModel : INotifyPropertyChanged {

        private string _subject;
        public string Subject {
            get {
                return _subject;
            }
            set {
                if (_subject == value) return;
                _subject = value;
                OnPropertyChanged("Subject");
            }
        }

        private int _periodperweek;
        public int PeriodPerWeek { 
            get {
                return _periodperweek;
            }
            set {
                if (_periodperweek == value) return;
                _periodperweek = value;
                OnPropertyChanged("PeriodPerWeek");
            }
        }

        public string _level;
        public string Level { 
            get { return _level; 
            } set {
                if (_level == value) return;
                _level = value;
                OnPropertyChanged("Level");
            }
        }

        public string _course;
        public string Course {
            get { return _course; }
            set {
                if (_course == value) return;
                _course = value;
                OnPropertyChanged("Course");
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
    }
}
