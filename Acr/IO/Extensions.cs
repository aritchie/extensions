using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Acr.IO
{
    public static class Extensions
    {
        public static IObservable<FileProgress> CopyProgress(this FileInfo from, FileInfo target, bool overwriteIfExists) => Observable.Create<FileProgress>(async ob =>
        {
            var completed = false;
            var cts = new CancellationTokenSource();
            if (overwriteIfExists)
                target.DeleteIfExists();

            var buffer = new byte[65535];
            var totalCopy = 0;
            var start = DateTime.UtcNow;

            using (var readStream = from.OpenRead())
            {
                using (var writeStream = target.Create())
                {
                    var read = await readStream.ReadAsync(buffer, 0, buffer.Length, cts.Token);
                    while (read > 0 && !cts.IsCancellationRequested)
                    {
                        await writeStream.WriteAsync(buffer, 0, read, cts.Token).ConfigureAwait(false);
                        read = await readStream.ReadAsync(buffer, 0, buffer.Length, cts.Token).ConfigureAwait(false);
                        totalCopy += read;
                        ob.OnNext(new FileProgress(totalCopy, from.Length, start));
                    }
                }
            }
            completed = true;
            ob.OnCompleted();

            return () =>
            {
                cts.Cancel();
                if (!completed)
                    target.DeleteIfExists();
            };
        });


        public static FileInfo GetExistingFile(this DirectoryInfo directory, string fileName)
            => directory
                .GetFiles()
                .FirstOrDefault(x => x.Name.Equals(fileName, StringComparison.Ordinal));


        public static void DeleteIfExists(this FileInfo file)
        {
            if (file.Exists)
                file.Delete();
        }


        public static Task<Stream> OpenWriteAsync(this FileInfo file) => Task<Stream>.Factory.StartNew(file.OpenWrite);
        public static Task<Stream> OpenReadAsync(this FileInfo file) => Task<Stream>.Factory.StartNew(file.OpenRead);

    }
}
