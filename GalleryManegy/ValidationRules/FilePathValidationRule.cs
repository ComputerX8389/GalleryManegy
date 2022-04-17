using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GalleryManegy.ValidationRules
{
    internal class FilePathValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(Directory.Exists(value.ToString()))
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                return new ValidationResult(false, $"{value} is not a valid directory");
            }
        }
    }
}
