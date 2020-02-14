using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Enlistment_Tracker.Converters
{
    public class IsIncludedToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isIncluded = value as bool?;
            if (!isIncluded.HasValue)
                return Binding.DoNothing;

            if (isIncluded.Value)
                return "#444444";

            return "#222222";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = value as string;
            if (color == null)
                return Binding.DoNothing;

            if (color == "#444444")
                return true;

            return false;
        }
    }
}
