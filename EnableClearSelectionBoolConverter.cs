using System;
using System.Globalization;
using System.Windows.Data;

namespace Font5
{
    class EnableClearSelectionBoolConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var isAnySelected = ((int)values[0]) > 0;
            return isAnySelected && !(bool)values[1];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
