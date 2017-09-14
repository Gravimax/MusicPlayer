using System.Collections.Generic;

namespace MusicPlayer
{
    public class PlayListUpdatedMessage
    {
        public PlayListUpdatedMessage(List<MediaItem> newPlayList)
        {
            NewPlayList = newPlayList;
        }

        public readonly List<MediaItem> NewPlayList;
    }
}
