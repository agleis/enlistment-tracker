using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Enlistment_Tracker.Converters
{
    public class StateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var state = value as State?;
            if (!state.HasValue)
                return Binding.DoNothing;

            switch (state.Value)
            {
                case State.Done:
                    return Brushes.DodgerBlue;
                case State.InPR:
                    return Brushes.ForestGreen;
                case State.WIP:
                    return Brushes.Firebrick;
                default:
                    return Binding.DoNothing;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var brush = value as Brush;
            if (brush == null)
                return Binding.DoNothing;

            if (brush == Brushes.DodgerBlue)
                return State.Done;
            if (brush == Brushes.ForestGreen)
                return State.InPR;
            if (brush == Brushes.Firebrick)
                return State.WIP;

            return Binding.DoNothing;
        }
    }
}
