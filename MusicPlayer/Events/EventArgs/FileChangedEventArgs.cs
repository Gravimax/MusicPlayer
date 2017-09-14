using System;

namespace MusicPlayer
{
    public class FileChangedEventArgs : EventArgs
    {
        public FileChangedEventArgs(string filePath, System.IO.WatcherChangeTypes changeType)
        {
            FilePath = filePath;
            ChangeType = changeType;
        }

        public readonly string FilePath;
        public readonly System.IO.WatcherChangeTypes ChangeType;
    }
}
