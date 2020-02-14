using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Enlistment_Tracker.StateManagement
{
    /// <summary>
    /// Interaction logic for EnlistmentsPage.xaml
    /// </summary>
    public partial class EnlistmentsPage : Page
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

        public EnlistmentsPage(string rootDirectory)
        {
            var enlistments = new List<Enlistment>();
            var directories = Directory.GetDirectories(rootDirectory);
            foreach (var directory in directories)
            {
                var directoryStripped = DirectoryStripped(directory);
                if (directorySkipList.Contains(directoryStripped))
                    continue;

                using (var repo = new Repository(directory))
                {
                    var checkedOutBranch = repo.Head;
                    var branchStripped = BranchStripped(checkedOutBranch.FriendlyName);
                    var state = RepoStateManager.GetRepoState(directoryStripped) ?? State.Done;
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
            var lastSlash = branch.LastIndexOf("/");
            if (lastSlash > -1)
                return branch.Substring(lastSlash + 1);

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
                context.State = State.WIP;
            else if (context.State == State.WIP)
                context.State = State.InPR;
            else if (context.State == State.InPR)
                context.State = State.Done;

            RepoStateManager.SetRepoState(context.Name, context.State);
        }
    }
}
