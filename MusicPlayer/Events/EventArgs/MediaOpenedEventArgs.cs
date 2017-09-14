using System;

namespace MusicPlayer
{
    public class MediaOpenedEventArgs : EventArgs
    {
        public MediaOpenedEventArgs(double length)
        {
            Length = length;
        }

        public readonly double Length;
    }
}
