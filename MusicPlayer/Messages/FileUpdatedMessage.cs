using System;

namespace MusicPlayer
{
    public class FileUpdatedMessage
    {
        public FileUpdatedMessage(string oldName, string newName, System.IO.WatcherChangeTypes changeType, MediaItem newMediaItem)
        {
            OldName = oldName;
            NewName = newName;
            ChangeType = changeType;
            NewMediaItem = newMediaItem;
        }

        public readonly string OldName;
        public readonly string NewName;
        public readonly System.IO.WatcherChangeTypes ChangeType;
        public readonly MediaItem NewMediaItem;
    }
}
