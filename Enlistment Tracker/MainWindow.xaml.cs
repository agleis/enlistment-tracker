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

namespace Enlistment_Tracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly HashSet<string> directorySkipList = new HashSet<string>()
        {
            "$RECYCLE.BIN",
            "Actual Shub VM",
            "blueboard",
            "Enlistment Tracker",
            "NP",
            "services-tools",
            "Shub shub shub",
            "Shub VM",
            "St. Helens Utilities",
            "System Volume Information",
            "Terminal",
            "vscode",
            "Watson-Tool",
            "watson-tool-clean",
            "wiki"
        };

        Dictionary<string, object> states = new Dictionary<string, object>();

        public MainWindow()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            var store = IsolatedStorageFile.GetUserStoreForAssembly();

            using (var stream = store.OpenFile("settings.cfg", FileMode.OpenOrCreate, FileAccess.Read))
            {
                if (stream.Length > 0)
                    states = (Dictionary<string, object>)formatter.Deserialize(stream);
            }
            var enlistments = new List<Enlistment>();
            var directories = Directory.GetDirectories("C:\\Users\\amg295\\source\\repos");
            foreach (var directory in directories)
            {
                var directoryStripped = DirectoryStripped(directory);
                if (directorySkipList.Contains(directoryStripped))
                    continue;

                using (var repo = new Repository(directory))
                {
                    var checkedOutBranch = repo.Head;
                    var branchStripped = BranchStripped(checkedOutBranch.FriendlyName);
                    var state = states.ContainsKey(directoryStripped) ? (State)states[directoryStripped] : State.Done;
                    enlistments.Add(new Enlistment(directoryStripped, branchStripped, state));
                }
            }
            InitializeComponent();
            DataContext = enlistments;
        }

        private string DirectoryStripped(string directory)
        {
            var lastSlash = directory.LastIndexOf("\\");
            if (lastSlash > -1)
                return directory.Substring(lastSlash + 1);

            return directory;
        }

        private string BranchStripped(string branch)
        {
            if (branch.StartsWith("adgleisn/"))
                return branch.Remove(0, 9);

            return branch;
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
                states[context.Name] = State.WIP;
                context.State = State.WIP;
            }
            else if (context.State == State.WIP)
            {
                states[context.Name] = State.InPR;
                context.State = State.InPR;
            }
            else if (context.State == State.InPR)
            {
                states[context.Name] = State.Done;
                context.State = State.Done;
            }

            BinaryFormatter formatter = new BinaryFormatter();
            var store = IsolatedStorageFile.GetUserStoreForAssembly();

            // Save
            using (var stream = store.OpenFile("settings.cfg", FileMode.OpenOrCreate, FileAccess.Write))
            {
                formatter.Serialize(stream, states);
            }
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
