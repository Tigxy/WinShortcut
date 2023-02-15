using System;
using System.Globalization;
using System.Windows.Data;

namespace win_short_cut.Converters {
    public class WidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double columnWidthPercentage = System.Convert.ToDouble(parameter);
            double width = (double)value;
            return width * (columnWidthPercentage / 100);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
