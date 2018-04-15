#if !NETSTANDARD10
using System;


namespace Acr.IO
{
    public static partial class FileSystem
    {
        static IFileSystem current;
        public static IFileSystem Current
        {
            get
            {
                if (current == null)
                    throw new Exception("[Acr.Core] FileSystem platform implementation not found.  Have you added a nuget reference to your platform project?");

                return current;
            }
            set { current = value; }
        }
    }
}
#endif
