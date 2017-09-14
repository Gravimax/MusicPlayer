using System.Collections.Generic;

namespace MusicPlayer.Models
{
    public interface IMusicLibrary
    {
        string Name { get; set; }

        string Path { get; set; }

        bool WatchFolder { get; set; }

        List<DirectoryItem> Folders { get; set; }

        List<MusicFile> MusicFiles { get; set; }
    }
}
