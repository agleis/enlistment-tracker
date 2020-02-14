using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
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
using LibGit2Sharp;
using Enlistment_Tracker.StateManagement;

namespace Enlistment_Tracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            StateManager.Initialize();
            InitializeComponent();
            Page page;
            if (StateManager.GetState<bool>("isWelcomed"))
                page = new EnlistmentsPage(StateManager.GetState<string>("rootDirectory"));
            else
                page = new WelcomePage();
            mainFrame.Navigate(page);
        }
    }

    public enum State
    {
        WIP,
        InPR,
        Done
    }

    public class Enlistment : INotifyPropertyChanged
    {
        private string _name;
        private string _branch;
        private State _state;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public string Branch
        {
            get
            {
                return _branch;
            }
            set
            {
                _branch = value;
                OnPropertyChanged();
            }
        }
        public State State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                OnPropertyChanged();
            }
        }

        public Enlistment(string name, string branch, State state)
        {
            Name = name;
            Branch = branch;
            State = state;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
