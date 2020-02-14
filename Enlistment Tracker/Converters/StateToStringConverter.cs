using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Enlistment_Tracker.Converters
{
    public class StateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var state = value as State?;
            if (!state.HasValue)
                return Binding.DoNothing;

            switch (state.Value)
            {
                case State.Done:
                    return "Done";
                case State.InPR:
                    return "In PR";
                case State.WIP:
                    return "WIP";
                default:
                    return Binding.DoNothing;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as string;
            if (str == null)
                return Binding.DoNothing;

            if (str == "Done")
                return State.Done;
            if (str == "In PR")
                return State.InPR;
            if (str == "WIP")
                return State.WIP;

            return Binding.DoNothing;
        }
    }
}
