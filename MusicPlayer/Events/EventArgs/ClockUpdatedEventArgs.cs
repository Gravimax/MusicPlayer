using System;

namespace MusicPlayer
{
    public class ClockUpdatedEventArgs : EventArgs
    {
        public ClockUpdatedEventArgs(TimeSpan? currentTime)
        {
            CurrentTime = currentTime;
        }

        public readonly TimeSpan? CurrentTime;
    }
}
