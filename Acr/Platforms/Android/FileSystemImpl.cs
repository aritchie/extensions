using System;
using System.IO;


namespace Acr.IO
{
    public class FileSystemImpl : IFileSystem
    {
        public FileSystemImpl()
        {
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
        }


        public DirectoryInfo AppData { get; }
        public DirectoryInfo Cache { get; }
        public DirectoryInfo Public { get; }
        public DirectoryInfo Temp { get; }
    }
}
