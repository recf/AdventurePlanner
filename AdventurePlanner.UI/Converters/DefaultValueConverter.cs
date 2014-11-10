using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AdventurePlanner.UI.Converters
{
    [ValueConversion(typeof(IEnumerable<object>), typeof(bool))]
    public class DefaultValueConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (string)value;
            var defaultValue = (string)parameter;

            return string.IsNullOrWhiteSpace(val) ? defaultValue : val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
