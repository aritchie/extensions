#if !NETSTANDARD1_0
using System;
using System.IO;


namespace Acr.IO
{
    public interface IFileSystem
    {
        DirectoryInfo AppData { get; }
        DirectoryInfo Cache { get; }
        DirectoryInfo Public { get; }

        string ToFileUri(string path);
    }
}
#endif
