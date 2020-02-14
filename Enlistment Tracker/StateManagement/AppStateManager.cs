using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enlistment_Tracker.StateManagement
{
    class AppStateManager : StateManager
    {
        public static AppState AppState
        {
            get => GetState<AppState>("appState");
            set => SetState("appState", value);
        }

        public static string RootDirectory
        {
            get => GetState<string>("rootDirectory");
            set => SetState("rootDirectory", value);
        }

        public static List<string> DirectoryIncludeList
        {
            get => GetState<List<string>>("directoryIncludeList");
            set => SetState("directoryIncludeList", value);
        }
    }
}
