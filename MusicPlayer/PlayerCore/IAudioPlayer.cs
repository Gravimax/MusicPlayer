using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.PlayerCore
{
    public interface IAudioPlayer
    {
        void Play(string filePath);
        void Pause();
        void Resume();
        void Stop();
        void SetCurrentPosition(double position);

        double Volume { get; set; }
        double Balance { get; set; }
        bool IsMuted { get; set; }
        bool IsPaused { get; }
        bool CanPause { get; }

        PlayStatus Status { get; }

        event ClockUpdatedEventHandler ClockUpdated;
        void OnClockUpdated(object sender, EventArgs e);

        event ClockCompletedEventHandler ClockCompleted;
        void OnClockCompleted(object sender, EventArgs e);

        event MediaOpenedEventHandler MediaOpened;
        void OnMediaOpened(object sender, EventArgs e);

        event MediaEndedEventHandler MediaEnded;
        void OnMediaEnded(object sender, EventArgs e);

        event MediaErrorEventHandler MediaError;
        void OnMediaError(object sender, EventArgs e);
    }
}
