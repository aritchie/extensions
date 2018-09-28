using System;
using System.IO;


namespace Acr.IO
{
    public class FileSystemImpl : IFileSystem
    {
        public FileSystemImpl()
        {
            var ctx = Android.App.Application.Context;

            this.AppData = new DirectoryInfo(ctx.FilesDir.AbsolutePath);
            this.Cache = new DirectoryInfo(ctx.CacheDir.AbsolutePath);
            var publicDir = ctx.GetExternalFilesDir(null);
            if (publicDir != null)
                this.Public = new DirectoryInfo(publicDir.AbsolutePath);
        }


        public DirectoryInfo AppData { get; }
        public DirectoryInfo Cache { get; }
        public DirectoryInfo Public { get; }

        public string ToFileUri(string path) => "file:/" + path;
    }
}