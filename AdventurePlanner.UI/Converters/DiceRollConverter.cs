using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Polyhedral;

namespace AdventurePlanner.UI.Converters
{
    [ValueConversion(typeof(DiceRoll), typeof(string))]
    public class DiceRollConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToString(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = (string)value;

            return DiceRoll.Parse(s);
        }

        #endregion
    }
}
