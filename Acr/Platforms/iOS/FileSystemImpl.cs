using System;
using System.IO;
using Foundation;
using UIKit;


namespace Acr.IO
{
    public class FileSystemImpl : IFileSystem
    {
        public FileSystemImpl()
        {
            var documents = UIDevice.CurrentDevice.CheckSystemVersion(8, 0)
                ? NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User)[0].Path
                : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var library = Path.Combine(documents, "..", "Library");
            this.AppData = new DirectoryInfo(library);
            this.Cache = new DirectoryInfo(Path.Combine(library, "Caches"));
            this.Temp = new DirectoryInfo(Path.Combine(documents, "..", "tmp"));
            this.Public = new DirectoryInfo(documents);
        }

        public DirectoryInfo AppData { get; }
        public DirectoryInfo Cache { get; }
        public DirectoryInfo Public { get; }
        public DirectoryInfo Temp { get; }
    }
}
