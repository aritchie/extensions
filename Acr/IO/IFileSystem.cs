using System;
using System.IO;


namespace Acr.IO
{
    public interface IFileSystem
    {
        /// <summary>
        /// This is a private directory where you can store data for your app only
        /// iOS: /Library
        /// </summary>
        DirectoryInfo AppData { get; }

        /// <summary>
        /// This is a private directory for your cache data for your app only - data is cleared from here as the OS determines it needs space
        /// iOS: /Cache
        /// </summary>
        DirectoryInfo Cache { get; }

        /// <summary>
        /// Files stored here can be made available to external apps
        /// iOS: /Documents
        /// </summary>
        DirectoryInfo Public { get; }

        string ToFileUri(string path);
    }
}