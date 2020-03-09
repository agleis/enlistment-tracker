using Enlistment_Tracker.StateManagement;
using LibGit2Sharp;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Enlistment_Tracker
{
    /// <summary>
    /// Interaction logic for WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Page
    {
        public WelcomePage()
        {
            InitializeComponent();
        }

        private void FileButtonClick(object sender, RoutedEventArgs e)
        {
            var folderPicker = new CommonOpenFileDialog();
            folderPicker.Title = "Select closest common ancestor of enlistments.";
            folderPicker.IsFolderPicker = true;
            var result = folderPicker.ShowDialog();
            switch (result)
            {
                case CommonFileDialogResult.Ok:
                    FileName.Text = folderPicker.FileName;
                    FileName.ToolTip = folderPicker.FileName;
                    break;
                case CommonFileDialogResult.Cancel:
                default:
                    FileName.Text = null;
                    FileName.ToolTip = null;
                    return;
            }
        }

        private void WelcomeConfirm(object sender, RoutedEventArgs e)
        {
            AppStateManager.RootDirectory = FileName.Text;
            var data = new DirectoryPageData()
            {
                Directory = FileName.Text
            };
            this.NavigationService.Navigate(new DirectoryPage(data));
        }
    }
}
