using MusicPlayer.Models;

namespace MusicPlayer
{
    public class FolderChangedMessage
    {
        public FolderChangedMessage(DirectoryItem folder)
        {
            this.Folder = folder;
        }

        public readonly DirectoryItem Folder;
    }
}
