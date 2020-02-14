using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Enlistment_Tracker.StateManagement
{
    /// <summary>
    /// Interaction logic for WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Page
    {
        private Enlistment _enlistment;

        public WelcomePage()
        {
            InitializeComponent();
        }

        private void FileButtonClick(object sender, RoutedEventArgs e)
        {
            var folderDialog = new FolderBrowserDialog();
            var result = folderDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    FileName.Text = folderDialog.SelectedPath;
                    FileName.ToolTip = folderDialog.SelectedPath;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    FileName.Text = null;
                    FileName.ToolTip = null;
                    return;
            }
        }

        private void WelcomeConfirm(object sender, RoutedEventArgs e)
        {
            StateManager.SetState("isWelcomed", true);
            StateManager.SetState("rootDirectory", FileName.Text);
            this.NavigationService.Navigate(new EnlistmentsPage(FileName.Text));
        }
    }
}
