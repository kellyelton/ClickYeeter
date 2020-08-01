using System;
using System.Globalization;
using System.Windows.Data;

namespace ClickYeeter9000.Converters
{
    public class DoubleBoosterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var dvalue = (double)value;

            var boostString = (string)parameter;

            var boost = double.Parse(boostString);

            return Math.Min(1, dvalue + boost);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
