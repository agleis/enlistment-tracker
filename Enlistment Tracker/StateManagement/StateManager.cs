using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Enlistment_Tracker.StateManagement
{
    public class StateManager
    {
        private static Dictionary<string, object> _currentState;
        private static BinaryFormatter _formatter;
        private static IsolatedStorageFile _store;

        public static void Initialize()
        {
            _formatter = new BinaryFormatter();
            _store = IsolatedStorageFile.GetUserStoreForAssembly();

            using (var stream = _store.OpenFile("settings.cfg", FileMode.OpenOrCreate, FileAccess.Read))
            {
                if (stream.Length > 0)
                    _currentState = (Dictionary<string, object>)_formatter.Deserialize(stream);
            }
        }

        protected static object GetState(string key)
        {
            if (!_currentState.TryGetValue(key, out object value))
            {
                return null;
            }

            return value;
        }

        protected static T GetState<T>(string key)
        {
            var value = GetState(key);

            try
            {
                return (T)value;
            }
            catch (InvalidCastException)
            {
                return default(T);
            }
        }

        protected static void SetState(string key, object value)
        {
            // Save
            using (var stream = _store.OpenFile("settings.cfg", FileMode.OpenOrCreate, FileAccess.Write))
            {
                _currentState[key] = value;
                _formatter.Serialize(stream, _currentState);
            }
        }
    }
}
