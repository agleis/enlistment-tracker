using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Enlistment_Tracker
{
    public static class DirectoryStripper
    {
        public static string StripDirectory(this string directory)
        {
            var lastSlash = directory.LastIndexOf("\\");
            if (lastSlash > -1)
                return directory.Substring(lastSlash + 1);

            return directory;
        }
    }
}
