using System;
using System.IO;


namespace Acr.IO
{
    public class FileSystemImpl : IFileSystem
    {
        public FileSystemImpl()
        {
            var path = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            this.AppData = new DirectoryInfo(path);
            this.Cache = new DirectoryInfo(Path.Combine(path, "Cache"));
            this.Public = new DirectoryInfo(Path.Combine(path, "Public"));
            this.Temp = new DirectoryInfo(Path.Combine(path, "Temp"));
        }


        public DirectoryInfo AppData { get; }
        public DirectoryInfo Cache { get; }
        public DirectoryInfo Public { get; }
        public DirectoryInfo Temp { get; }
    }
}
