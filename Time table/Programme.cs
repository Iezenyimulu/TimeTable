using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TimeTable {
    public abstract class Programme {

      

        //public int Id { get; set; }
        public string Class { get; set; }
        public string Day { get; set; }
        //public string Teacher { get; set; }
        public int Period { get; set; }
        public string Subject { get; set; }
    }

   

    public class GeneralProgramme : Programme {
        
       // http://stackoverflow.com/questions/10283206/c-sharp-setting-getting-the-class-properties-by-string-name
        public object this[string propertyName] {
            get {
                // probably faster without reflection:
                // like:  return Properties.Settings.Default.PropertyValues[propertyName] 
                // instead of the following
                Type myType = typeof(GeneralProgramme);
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                return myPropInfo.GetValue(this, null);
            }
            set {
                Type myType = typeof(GeneralProgramme);
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                myPropInfo.SetValue(this, value, null);

            }

        }

        public new string Class { get; set; }
        //mon        
        public string MP1 { get; set; }
        public int MP1Id { get; set; }
        public string MP2 { get; set; }
        public int MP2Id { get; set; }
        public string MP3 { get; set; }
        public int MP3Id { get; set; }
        public string MP4 { get; set; }
        public int MP4Id { get; set; }
        public string MP5 { get; set; }
        public int MP5Id { get; set; }
        public string MP6 { get; set; }
        public int MP6Id { get; set; }
        public string MP7 { get; set; }
        public int MP7Id { get; set; }
        public string MP8 { get; set; }
        public int MP8Id { get; set; }
        public string MP9 { get; set; }
        public int MP9Id { get; set; }
        //tue
        public string TP1 { get; set; }
        public int TP1Id { get; set; }
        public string TP2 { get; set; }
        public int TP2Id { get; set; }
        public string TP3 { get; set; }
        public int TP3Id { get; set; }
        public string TP4 { get; set; }
        public int TP4Id { get; set; }
        public string TP5 { get; set; }
        public int TP5Id { get; set; }
        public string TP6 { get; set; }
        public int TP6Id { get; set; }
        public string TP7 { get; set; }
        public int TP7Id { get; set; }
        public string TP8 { get; set; }
        public int TP8Id { get; set; }
        public string TP9 { get; set; }
        public int TP9Id { get; set; }
        //wed
        public string WP1 { get; set; }
        public int WP1Id { get; set; }
        public string WP2 { get; set; }
        public int WP2Id { get; set; }
        public string WP3 { get; set; }
        public int WP3Id { get; set; }
        public string WP4 { get; set; }
        public int WP4Id { get; set; }
        public string WP5 { get; set; }
        public int WP5Id { get; set; }
        public string WP6 { get; set; }
        public int WP6Id { get; set; }
        public string WP7 { get; set; }
        public int WP7Id { get; set; }
        public string WP8 { get; set; }
        public int WP8Id { get; set; }
        public string WP9 { get; set; }
        public int WP9Id { get; set; }
        //thursday
        public string THP1 { get; set; }
        public int THP1Id { get; set; }
        public string THP2 { get; set; }
        public int THP2Id { get; set; }
        public string THP3 { get; set; }
        public int THP3Id { get; set; }
        public string THP4 { get; set; }
        public int THP4Id { get; set; }
        public string THP5 { get; set; }
        public int THP5Id { get; set; }
        public string THP6 { get; set; }
        public int THP6Id { get; set; }
        public string THP7 { get; set; }
        public int THP7Id { get; set; }
        public string THP8 { get; set; }
        public int THP8Id { get; set; }
        public string THP9 { get; set; }
        public int THP9Id { get; set; }
        //friday
        public string FP1 { get; set; }
        public int FP1Id { get; set; }
        public string FP2 { get; set; }
        public int FP2Id { get; set; }
        public string FP3 { get; set; }
        public int FP3Id { get; set; }
        public string FP4 { get; set; }
        public int FP4Id { get; set; }
        public string FP5 { get; set; }
        public int FP5Id { get; set; }
        public string FP6 { get; set; }
        public int FP6Id { get; set; }
        public string FP7 { get; set; }
        public int FP7Id { get; set; }
        public string FP8 { get; set; }
        public int FP8Id { get; set; }
        public string FP9 { get; set; }
        public int FP9Id { get; set; }
    }

    public class DayProgramme {
        public string Class { get; set; }
        public int Period { get; set; }
        public string Subject { get; set; }
        public string Day { get; set; }
}

 

    public class ClassProgramme : Programme {

        // http://stackoverflow.com/questions/10283206/c-sharp-setting-getting-the-class-properties-by-string-name
        public object this[string propertyName] {
            get {
                // probably faster without reflection:
                // like:  return Properties.Settings.Default.PropertyValues[propertyName] 
                // instead of the following
                Type myType = typeof(ClassProgramme);
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                return myPropInfo.GetValue(this, null);
            }
            set {
                Type myType = typeof(ClassProgramme);
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                myPropInfo.SetValue(this, value, null);

            }

        }
        public new string Class { get; set; }
        public new int Period { get; set; }
        public new string Subject { get; set; }
        public new string Day { get; set; }
        public string Recess { get; set; }
        public string P1 { get; set; }
        public int P1Id { get; set; }
        public string P2 { get; set; }
        public int P2Id { get; set; }
        public string P3 { get; set; }
        public int P3Id { get; set; }
        public string P4 { get; set; }
        public int P4Id { get; set; }
        public string P5 { get; set; }
        public int P5Id { get; set; }
        public string P6 { get; set; }
        public int P6Id { get; set; }
        public string P7 { get; set; }
        public int P7Id { get; set; }
        public string P8 { get; set; }
        public int P8Id { get; set; }
        public string P9 { get; set; }
        public int P9Id { get; set; }

    }

    public class TeacherProgramme : Programme {

        // http://stackoverflow.com/questions/10283206/c-sharp-setting-getting-the-class-properties-by-string-name
        public object this[string propertyName] {
            get {
                // probably faster without reflection:
                // like:  return Properties.Settings.Default.PropertyValues[propertyName] 
                // instead of the following
                Type myType = typeof(TeacherProgramme);
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                return myPropInfo.GetValue(this, null);
            }
            set {
                Type myType = typeof(TeacherProgramme);
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                myPropInfo.SetValue(this, value, null);

            }

        }
        public new string Class { get; set; }
        public new int Period { get; set; }
        public new string Subject { get; set; }
        public new string Day { get; set; }
        public string Recess { get; set; }
        public string P1 { get; set; }
        public int P1Id { get; set; }
        public string P2 { get; set; }
        public int P2Id { get; set; }
        public string P3 { get; set; }
        public int P3Id { get; set; }
        public string P4 { get; set; }
        public int P4Id { get; set; }
        public string P5 { get; set; }
        public int P5Id { get; set; }
        public string P6 { get; set; }
        public int P6Id { get; set; }
        public string P7 { get; set; }
        public int P7Id { get; set; }
        public string P8 { get; set; }
        public int P8Id { get; set; }
        public string P9 { get; set; }
        public int P9Id { get; set; }

    }
}
