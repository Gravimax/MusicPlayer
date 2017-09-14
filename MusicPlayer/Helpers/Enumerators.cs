using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer
{
    public enum PlayType
    {
        Normal = 1,
        Random = 2,
        Shuffle = 3,
        None = 4
    }

    public enum ListType
    {
        Library = 0,
        Playlist = 1
    }

    public enum FileDeleteType
    {
        ListOnly = 1,
        ListAndComputer = 2
    }

    public enum PlayStatus
    {
        Playing = 1,
        Paused = 2,
        Stopped = 3
    }
}
