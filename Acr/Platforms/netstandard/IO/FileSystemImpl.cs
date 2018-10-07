using System;
using System.IO;
using Acr.IO;


namespace Acr.Platforms.netstandard.IO
{
    public class FileSystemImpl : IFileSystem
    {
        public FileSystemImpl()
        {
            this.AppData = this.Cache = this.Public = new DirectoryInfo(".");
        }


        public DirectoryInfo AppData { get; set; }
        public DirectoryInfo Cache { get; set; }
        public DirectoryInfo Public { get; set; }
        public string ToFileUri(string path) => "file://" + path;
    }
}
