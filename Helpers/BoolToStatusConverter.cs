using System;
using System.Globalization;
using System.Windows.Data;

namespace HousingManagement
{
    public class BoolToStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isPaid = (bool)value;
            return isPaid ? "Оплачено" : "Не оплачено";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}