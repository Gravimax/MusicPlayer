using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicPlayer.FileSupport
{
    public class FolderWatcher : IDisposable
    {
        private List<System.IO.FileSystemWatcher> watcherList = new List<System.IO.FileSystemWatcher>();

        public FolderWatcher()
        {

        }

        public void AddFolderWatch(string path)
        {
            System.IO.FileSystemWatcher fileWatcher = new System.IO.FileSystemWatcher();
            fileWatcher.Path = path;
            fileWatcher.Filter = "*.*";
            fileWatcher.NotifyFilter = System.IO.NotifyFilters.DirectoryName |
                System.IO.NotifyFilters.FileName |
                System.IO.NotifyFilters.CreationTime |
                System.IO.NotifyFilters.LastAccess;
            fileWatcher.IncludeSubdirectories = true;

            //fileWatcher.Changed += new System.IO.FileSystemEventHandler(OnChange);
            fileWatcher.Created += new System.IO.FileSystemEventHandler(OnFileChanged);
            fileWatcher.Deleted += new System.IO.FileSystemEventHandler(OnFileChanged);
            fileWatcher.Renamed += new System.IO.RenamedEventHandler(OnFileRenamed);

            fileWatcher.EnableRaisingEvents = true; // Enable the watcher;
            watcherList.Add(fileWatcher);
        }

        public void RemoveFileWatcher(string folder)
        {
            System.IO.FileSystemWatcher fileWatcher = watcherList.FirstOrDefault(x => x.Path == folder);
            if (fileWatcher != null)
            {
                //fileWatcher.Changed -= new System.IO.FileSystemEventHandler(OnChange);
                fileWatcher.Created -= new System.IO.FileSystemEventHandler(OnFileChanged);
                fileWatcher.Deleted -= new System.IO.FileSystemEventHandler(OnFileChanged);
                fileWatcher.Renamed -= new System.IO.RenamedEventHandler(OnFileRenamed);
                fileWatcher.EnableRaisingEvents = false;

                watcherList.Remove(fileWatcher);
                fileWatcher = null;
            }
        }

        public void RemoveAllWatchers()
        {
            if (watcherList != null)
            {
                foreach (var fileWatcher in watcherList.ToArray())
                {
                    fileWatcher.Created -= new System.IO.FileSystemEventHandler(OnFileChanged);
                    fileWatcher.Deleted -= new System.IO.FileSystemEventHandler(OnFileChanged);
                    fileWatcher.Renamed -= new System.IO.RenamedEventHandler(OnFileRenamed);
                    fileWatcher.EnableRaisingEvents = false;

                    watcherList.Remove(fileWatcher);
                }

                watcherList = null;
            }
        }

        public event FileChangedEventHandler FileChanged;

        protected void OnFileChanged(object sender, System.IO.FileSystemEventArgs e)
        {
            FileChanged?.Invoke(sender, e);
        }

        public event FileRenamedEventHandler FileRenamed;

        protected void OnFileRenamed(object sender, System.IO.RenamedEventArgs e)
        {
            FileRenamed?.Invoke(sender, e);
        }


        #region Dispose

        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        System.Runtime.InteropServices.SafeHandle handle = new Microsoft.Win32.SafeHandles.SafeFileHandle(IntPtr.Zero, true);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            RemoveAllWatchers();
            disposed = true;
        }

        #endregion
    }
}
