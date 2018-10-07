using System;


namespace Acr.IO
{
    public static partial class FileSystem
    {
        static FileSystem()
        {
            Current = new FileSystemImpl();
        }
    }
}
