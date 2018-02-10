#if !NETSTANDARD10
using System;
using System.IO;


namespace Acr.IO
{
    public interface IFileSystem
    {
        DirectoryInfo AppData { get; }
        DirectoryInfo Cache { get; }
        DirectoryInfo Public { get; }
        DirectoryInfo Temp { get; }
    }
}
#endif