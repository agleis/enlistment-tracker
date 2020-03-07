using Enlistment_Tracker.StateManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Enlistment_Tracker
{
    /// <summary>
    /// Interaction logic for DirectoryPage.xaml
    /// </summary>
    public partial class DirectoryPage : Page
    {
        private readonly DirectoryPageData data;

        private List<DirectorySelection> context
        {
            get => DataContext as List<DirectorySelection>;
            set => DataContext = value;
        }

        public DirectoryPage(DirectoryPageData data)
        {
            AppStateManager.AppState = AppState.Welcomed;
            InitializeComponent();
            var directories = Directory.GetDirectories(data.Directory);
            var directorySelectionList = new List<DirectorySelection>();
            foreach (var directory in directories)
            {
                var directorySelection = new DirectorySelection()
                {
                    IsIncluded = false,
                    Name = directory.StripDirectory()
                };
                directorySelectionList.Add(directorySelection);
            }
            context = directorySelectionList;
            this.data = data;
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var data = new EnlistmentsPageData()
            {
                Directory = this.data.Directory,
                DirectoryIncludeList = context.Where(ds => ds.IsIncluded).Select(ds => ds.Name).ToList()
            };
            AppStateManager.DirectoryIncludeList = data.DirectoryIncludeList;
            this.NavigationService.Navigate(new EnlistmentsPage(data));
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems)
            {
                var directorySelection = item as DirectorySelection;
                directorySelection.IsIncluded = true;
                Console.WriteLine("Added" + directorySelection.Name);
            }
            foreach (var item in e.RemovedItems)
            {
                var directorySelection = item as DirectorySelection;
                directorySelection.IsIncluded = false;
                Console.WriteLine("Removed" + directorySelection.Name);
            }
        }
    }

    public class DirectorySelection : INotifyPropertyChanged
    {
        private bool _isIncluded;
        private string _name;

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

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
