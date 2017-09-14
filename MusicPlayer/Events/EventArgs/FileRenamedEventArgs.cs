using System;

namespace MusicPlayer
{
    public class FileRenamedEventArgs : EventArgs
    {
        public FileRenamedEventArgs(string oldName, string newName)
        {
            OldName = oldName;
            NewName = newName;
        }

        public readonly string OldName;
        public readonly string NewName;
    }
}
