using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enlistment_Tracker.StateManagement
{
    class RepoStateManager : StateManager
    {
        public static State? GetRepoState(string repo) => GetState<State>(repo);

        public static void SetRepoState(string repo, State state) => SetState(repo, state);
    }
}
