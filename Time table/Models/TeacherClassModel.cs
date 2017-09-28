using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTable {
    public class TeacherClassModel : INotifyPropertyChanged {
        public int Id { get; set; }

        private string _name;
        public string Name {
            get { return _name; }
            set {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private string _class;
        public string Class {
            get { return _class; }
            set {
                if (_class == value) return;
                _class = value;
                OnPropertyChanged("Class");
            }
        }

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


        private bool _booked;
        public bool Booked {
            get {
                return _booked;
            }
            set {
                if (_booked == value) return;
                _booked = value;
                OnPropertyChanged("Booked");
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
