using System;
using System.IO;
#if __IOS__
using Foundation;
using UIKit;
#endif

namespace Acr.IO
{
    public class FileSystemImpl : IFileSystem
    {
        public FileSystemImpl()
        {
#if WINDOWS_UWP
            var path = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            this.AppData = new DirectoryInfo(path);
            this.Cache = new DirectoryInfo(Path.Combine(path, "Cache"));
            this.Public = new DirectoryInfo(Path.Combine(path, "Public"));
            this.Temp = new DirectoryInfo(Path.Combine(path, "Temp"));
#elif __IOS__
            var documents = UIDevice.CurrentDevice.CheckSystemVersion(8, 0)
                ? NSFileManager.DefaultManager.GetUrls (NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User)[0].Path
                : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var library = Path.Combine(documents, "..", "Library");
            this.AppData = new DirectoryInfo(library);
            this.Cache = new DirectoryInfo(Path.Combine(library, "Caches"));
            this.Temp = new DirectoryInfo(Path.Combine(documents, "..", "tmp"));
            this.Public = new DirectoryInfo(documents);
#elif __ANDROID__
            try
            {
                var ctx = Android.App.Application.Context;
                this.AppData = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));

                var ext = ctx.GetExternalFilesDir(null);
                if (ext != null)
                    this.Public = new DirectoryInfo(ext.AbsolutePath);

                var cacheDir = ctx.GetExternalFilesDir(null);
                if (cacheDir != null) {
                    this.Cache = new DirectoryInfo(cacheDir.AbsolutePath);
                    this.Temp = new DirectoryInfo(cacheDir.AbsolutePath);
                }
            }
            catch { }
#endif
        }


        public DirectoryInfo AppData { get; }
        public DirectoryInfo Cache { get; }
        public DirectoryInfo Public { get; }
        public DirectoryInfo Temp { get; }
    }
}
