using System.Collections.Generic;

namespace MusicPlayer.Models
{
    public interface IDirectoryItem
    {
        bool IsExpanded { get; set; }

        List<DirectoryItem> Items { get; set; }

        string Name { get; set; }

        string Path { get; set; }
    }
}
