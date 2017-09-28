using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace TimeTable {
    public class SubjectValidationRule :  ValidationRule {
            public override ValidationResult Validate(object value,
                System.Globalization.CultureInfo cultureInfo) {
                SubjectModel subject = (value as BindingGroup).Items[0] as SubjectModel;                

                //if(String.IsNullOrWhiteSpace(subject.Name))
                //    return new ValidationResult(false, "Subject name cannot be empty");
                //if (String.IsNullOrWhiteSpace(subject.Alias))
                //    return new ValidationResult(false, "Subject's alias cannot be empty");
                //if(subject.Alias.Length > 5){
                //    return new ValidationResult(false, "maximum string length required for Alias = 5");
                //}                
                //if (MainWindow.SubjectList.Count() > 0) {
                //    var nameExist = MainWindow.SubjectList.Exists(s => s.Id != subject.Id && s.Name.ToLower() == subject.Name.ToLower().Trim());
                //    if(nameExist)
                //        return new ValidationResult(false, "Subject already exist");
                //    var aliasExist = MainWindow.SubjectList.Exists(s => s.Id != subject.Id && s.Alias.ToLower() == subject.Alias.ToLower().Trim());
                //    if (nameExist)
                //        return new ValidationResult(false, "This alias already exist");
                //}

                    return ValidationResult.ValidResult;              
        }
    }
}
