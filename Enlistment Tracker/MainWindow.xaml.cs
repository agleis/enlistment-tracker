using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Enlistment_Tracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Enlistment> enlistments = new List<Enlistment>()
        {
            new Enlistment("Adams", "", State.Done),
            new Enlistment("Baker", "", State.Done),
            new Enlistment("Glacier", "", State.Done),
            new Enlistment("Hood", "", State.Done),
            new Enlistment("Jefferson", "", State.Done),
            new Enlistment("Lassen", "", State.Done),
            new Enlistment("Rainier", "", State.Done),
            new Enlistment("Shasta", "", State.Done),
            new Enlistment("St. Helens", "", State.Done),
        };

        public MainWindow()
        {
            InitializeComponent();
            DataContext = enlistments;
        }

        private void ListButtonClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;

            var context = button.DataContext as Enlistment;
            if (context == null)
                return;

            if (context.State == State.Done)
            {
                context.State = State.WIP;
                button.Background = Brushes.Firebrick;
                (button.Content as TextBlock).Text = "WIP";
            }
            else if (context.State == State.WIP)
            {
                context.State = State.InPR;
                button.Background = Brushes.ForestGreen;
                (button.Content as TextBlock).Text = "In PR";
            }
            else if (context.State == State.InPR)
            {
                context.State = State.Done;
                button.Background = Brushes.DodgerBlue;
                (button.Content as TextBlock).Text = "Done";
            }
        }
    }

    public enum State
    {
        WIP,
        InPR,
        Done
    }

    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var state = value as State?;
            if (!state.HasValue)
                return Binding.DoNothing;

            switch(state.Value)
            {
                case State.Done:
                    return Brushes.AliceBlue;
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

            if (brush == Brushes.AliceBlue)
                return State.Done;
            if (brush == Brushes.ForestGreen)
                return State.InPR;
            if (brush == Brushes.Firebrick)
                return State.WIP;

            return Binding.DoNothing;
        }
    }

    public class Enlistment
    {
        public string Name { get; set; }
        public string Branch { get; set; }
        public State State { get; set; }

        public Enlistment(string name, string branch, State state)
        {
            Name = name;
            Branch = branch;
            State = state;
        }
    }
}
