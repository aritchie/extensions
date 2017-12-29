using System;

namespace Acr.IO
{
    public class FileProgress
    {
        public TimeSpan TimeRemaining { get; }
        public TimeSpan TimeSpent { get; }
        public int PercentComplete { get; }
        public long FileSize { get; }
        public long BytesCompleted { get; }
        public double BytesPerSecond { get; }
        // TODO: throughput per second


        public FileProgress(long bytesCompleted, long fileSize, DateTime startUtc)
        {
            this.FileSize = fileSize;
            this.BytesCompleted = bytesCompleted;
            this.PercentComplete = Convert.ToInt32(bytesCompleted * 100.0 / fileSize);
            this.TimeSpent = DateTime.UtcNow.Subtract(startUtc);
            this.BytesPerSecond = bytesCompleted / this.TimeSpent.TotalSeconds; // TODO: should be a sampling, not overall
            this.TimeRemaining = TimeSpan.FromSeconds(this.BytesCompleted / this.BytesPerSecond);
        }
    }
}
/*
public class DirectoryProgress
{
    public FileProgress CurrentFile { get; }
    public long TotalFiles { get; set; }
    public long FilesCompleted { get; set; }
    public int TotalDirectories { get; set; }
    public int DirectoriesCompleted { get; set; }
}



        public async Task<IFile> CopyToAsync(string path, bool overwrite, Action<FileCopyProgress> onProgress, CancellationToken token)
{
  

            return new File(path);
        }

#endregion


        string GetMimeType()
        {
            var ext = Path.GetExtension(this.Name);

#if __ANDROID__
			if (ext == null)
				return "*.*";

			ext = ext.ToLower().TrimStart('.');
			var mimeType = Android.Webkit.MimeTypeMap.Singleton.GetMimeTypeFromExtension(ext);
			return mimeType ?? "*.*";
#else
            //			if (ext == null)
            //				return String.Empty;
            //
            //			switch (ext.ToLower()) {
            //			case ".jpg" : return "image/jpg";
            //			case ".png" : return "image/png";
            //			case ".gif" : return "image/gif";
            //			case ".pdf" : return "application/pdf";
            //			case ".docs": return "application/";
            //			}
            return String.Empty;
#endif
        }
    }
}
#endif

#if __PLATFORM__
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace Acr.IO
{

    public class Directory : FileSystemObject, IDirectory
    {
        readonly DirectoryInfo info;


        public Directory(string path) : this(new DirectoryInfo(path)) {}
        internal Directory(DirectoryInfo info) : base(info)
        {
            this.info = info;
        }


        public Task<IEnumerable<IFileSystemObject>> GetFileSystemObjectsAsync(string pattern = null, FsSearchOption option = FsSearchOption.TopDirectoryOnly)
        {
            var opt = option == FsSearchOption.AllDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return Task.Run(() => this.info
                .GetFileSystemInfos(pattern ?? "*.*", opt)
                .Select(x => new FileSystemObject(x))
                .Cast<IFileSystemObject>()
            );
        }


        public Task<IEnumerable<IFile>> GetFilesAsync(string pattern = null, FsSearchOption option = FsSearchOption.TopDirectoryOnly)
        {
            var opt = option == FsSearchOption.AllDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return Task.Run(() => this.info
                .GetFiles(pattern ?? "*.*", opt)
                .Select(x => new File(x))
                .Cast<IFile>()
            );
        }


        public Task<IEnumerable<IDirectory>> GetDirectoriesAsync(string pattern = null, FsSearchOption option = FsSearchOption.TopDirectoryOnly)
        {
            var opt = option == FsSearchOption.AllDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return Task.Run(() => this.info
                .GetDirectories(pattern ?? "*.*", opt)
                .Select(x => new Directory(x))
                .Cast<IDirectory>()
            );
        }


        IDirectory root;
        public IDirectory Root
        {
            get
            {
                this.root = this.root ?? new Directory(this.info.Root);
                return this.root;
            }
        }


        private IDirectory parent;
        public IDirectory Parent
        {
            get
            {
                this.parent = this.parent ?? new Directory(this.info.Parent);
                return this.parent;
            }
        }


        public void Create() {
            this.info.Create();
        }


        public void MoveTo(string path) {
            this.info.MoveTo(path);
        }


        public bool FileExists(string fileName) {
            var path = Path.Combine(this.FullName, fileName);
            return System.IO.File.Exists(path);
        }


        public IFile CreateFile(string fileName) {
            var path = Path.Combine(this.FullName, fileName);
            return new File(new FileInfo(path));
        }


        public IDirectory CreateSubdirectory(string path) {
            var dir = this.info.CreateSubdirectory(path);
            return new Directory(dir);
        }


        public void Delete(bool recursive = false) {
            this.info.Delete(recursive);
        }


        private IEnumerable<IDirectory> directories;
        public IEnumerable<IDirectory> Directories {
            get {
                this.directories = this.directories ?? this.info.GetDirectories().Select(x => new Directory(x)).ToList();
                return this.directories;
            }
        }


        private IEnumerable<IFile> files;
        public IEnumerable<IFile> Files {
            get {
                this.files = this.files ?? this.info.GetFiles().Select(x => new File(x)).ToList();
                return this.files;
            }
        }
    }
}
 */
