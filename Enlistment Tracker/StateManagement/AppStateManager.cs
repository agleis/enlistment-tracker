using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enlistment_Tracker.StateManagement
{
    class AppStateManager : StateManager
    {
        public static bool IsWelcomed
        {
            get => GetState<bool>("isWelcomed");
            set => SetState("isWelcomed", value);
        }

        public static string RootDirectory
        {
            get => GetState<string>("rootDirectory");
            set => SetState("rootDirectory", value);
        }
    }
}
