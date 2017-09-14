using MusicPlayer.Models;
using System;

namespace MusicPlayer
{
    public class PlayFileMessage
    {
        public PlayFileMessage(MediaItem musicFile)
        {
            SelectedMusicFile = musicFile;
        }

        public readonly MediaItem SelectedMusicFile;
    }
}
