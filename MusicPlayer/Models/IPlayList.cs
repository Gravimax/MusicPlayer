using System.Collections.Generic;

namespace MusicPlayer.Models
{
    public interface IPlayList
    {
        string Name { get; set; }

        string RootFolder { get; set; }

        List<string> Folders { get; set; }

        List<MusicFile> MusicFiles { get; set; }

        List<string> FolderList { get; }
    }
}
