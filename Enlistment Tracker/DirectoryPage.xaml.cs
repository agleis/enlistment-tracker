using Enlistment_Tracker.StateManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Enlistment_Tracker
{
    /// <summary>
    /// Interaction logic for DirectoryPage.xaml
    /// </summary>
    public partial class DirectoryPage : Page
    {
        private readonly DirectoryPageData data;

        private List<string> DirectoryIncludeList;

        private List<DirectorySelection> context
        {
            get => DataContext as List<DirectorySelection>;
            set => DataContext = value;
        }

        public DirectoryPage(DirectoryPageData data)
        {
            AppStateManager.AppState = AppState.Welcomed;
            InitializeComponent();
            DirectoryIncludeList = new List<string>();
            var directories = Directory.GetDirectories(data.Directory);
            var directorySelectionList = new List<DirectorySelection>();
            foreach (var directory in directories)
            {
                var directorySelection = new DirectorySelection()
                {
                    IsIncluded = false,
                    Name = directory.StripDirectory(),
                    FullName = directory,
                    Subdirectories = GetSubdirectories(directory)
                };
                directorySelectionList.Add(directorySelection);
            }
            context = directorySelectionList;
            this.data = data;
        }

        private ObservableCollection<DirectorySelection> GetSubdirectories(string directory)
        {
            try
            {
                var subdirectories = Directory.GetDirectories(directory);
                var subdirectoriesCollection = new ObservableCollection<DirectorySelection>();
                foreach (var subdirectory in subdirectories)
                {
                    var directorySelection = new DirectorySelection()
                    {
                        IsIncluded = false,
                        Name = subdirectory.StripDirectory(),
                        FullName = subdirectory
                    };
                    subdirectoriesCollection.Add(directorySelection);
                }
                return subdirectoriesCollection;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var data = new EnlistmentsPageData()
            {
                Directory = this.data.Directory,
                DirectoryIncludeList = DirectoryIncludeList
            };
            AppStateManager.DirectoryIncludeList = data.DirectoryIncludeList;
            this.NavigationService.Navigate(new EnlistmentsPage(data));
        }

        private void treeViewItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            var tvi = (TreeViewItem)sender;
            var directorySelection = tvi.DataContext as DirectorySelection;
            if (directorySelection.IsIncluded)
            {
                directorySelection.IsIncluded = false;
                DirectoryIncludeList.Remove(directorySelection.FullName);
            }
            else
            {
                directorySelection.IsIncluded = true;
                DirectoryIncludeList.Add(directorySelection.FullName);
            }
            e.Handled = true;
        }

        private void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        {
            var tvi = (TreeViewItem)sender;
            var directorySelection = tvi.DataContext as DirectorySelection;
            foreach (var subdirectorySelection in directorySelection.Subdirectories)
                subdirectorySelection.Subdirectories = GetSubdirectories(subdirectorySelection.FullName);
        }
    }

    public class DirectorySelection : INotifyPropertyChanged
    {
        private bool _isIncluded;
        private string _name;
        private string _fullName;
        private ObservableCollection<DirectorySelection> _subdirectories;

        public bool IsIncluded
        {
            get => _isIncluded;
            set
            {
                _isIncluded = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<DirectorySelection> Subdirectories
        {
            get => _subdirectories;
            set
            {
                _subdirectories = value;
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
