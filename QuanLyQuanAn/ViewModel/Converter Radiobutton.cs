using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace QuanLyQuanAn.ViewModel
{
    internal class Converter_Radiobutton : IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string Option = parameter as string;
            if(value is bool isChecked && isChecked)
            {
                return Option;
            }
            return Binding.DoNothing;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string Option = parameter as string;
            if (value is string selectedOption && selectedOption == Option)
            {
                return true;
            }
            return false;
        }
    }
}
