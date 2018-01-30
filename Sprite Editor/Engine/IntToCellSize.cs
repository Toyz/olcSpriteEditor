using System;
using System.Globalization;
using System.Windows.Data;

namespace SPE.Engine
{
    public class IntToCellSize : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //throw new NotImplementedException();
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((CellSize) value).Size;
            //throw new NotImplementedException();
        }
    }
}
