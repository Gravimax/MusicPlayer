using System;

namespace MusicPlayer.Models
{
    public interface IMusicFile
    {
        string Album { get; set; }

        string Artist { get; set; }

        int BitRate { get; set; }

        string Comment { get; set; }

        long Duration { get; set; }

        string FilePath { get; set; }

        string Folder { get; set; }

        string Genre { get; set; }

        string Title { get; set; }

        UInt32 Year { get; set; }

        UInt32 Track { get; set; }

        string Type { get; set; }
    }
}
