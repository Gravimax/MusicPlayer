using CSCore;
using CSCore.Codecs;
using CSCore.SoundOut;
using CSCore.Streams.Effects;
using System;
using System.Windows.Forms;

namespace MusicPlayer.PlayerCore
{
    public class CSMediaPlayer : Notifier, IDisposable
    {
        #region Properties

        private ISoundOut _soundOut;
        private IWaveSource _waveSource;
        private Timer positionTimer;
        private int timerInterval = 100;
        private Equalizer _equalizer;
        private float[] eqArray = new float[10];

        public PlaybackState PlaybackState
        {
            get
            {
                if (_soundOut != null)
                {
                    return _soundOut.PlaybackState;
                }

                return PlaybackState.Stopped;
            }
        }

        private float _volume = 1;

        public double Volume
        {
            get
            {
                if (_soundOut != null)
                {
                    return System.Convert.ToDouble(_soundOut.Volume);
                }

                return 1;
            }

            set
            {
                if (_soundOut != null)
                {
                    _volume = System.Convert.ToSingle(value);
                    _soundOut.Volume = System.Convert.ToSingle(value);
                }
                else
                {
                    _volume = System.Convert.ToSingle(value);
                }
            }
        }

        public TimeSpan Position
        {
            get
            {
                if (_waveSource != null)
                {
                    return _waveSource.GetPosition();
                }

                return TimeSpan.Zero;
            }
            set
            {
                if (_waveSource != null)
                {
                    if (_waveSource.CanSeek)
                    {
                        _waveSource.SetPosition(value);
                    }
                }
            }
        }

        public TimeSpan Length
        {
            get
            {
                if (_waveSource != null)
                {
                    return _waveSource.GetLength();
                }

                return TimeSpan.Zero;
            }
        }

        public bool IsPaused
        {
            get
            {
                return Status == PlayStatus.Paused; 
            }
        }

        public bool CanPause
        {
            get
            {
                if (_waveSource != null)
                {
                    if (_soundOut != null)
                    {
                        return Status == PlayStatus.Playing;
                    }
                }

                return false;
            }
        }

        public bool IsMuted
        {
            get
            {
                return Volume == 0;
            }
            set
            {
                if (value == false)
                {
                    _soundOut.Volume = _volume;
                }
                else
                {
                    _soundOut.Volume = 0;
                }
            }
        }


        private PlayStatus _status = PlayStatus.Stopped;
        public PlayStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        #endregion

        #region Ctors

        public CSMediaPlayer()
        {
            CodecFactory.Instance.Register("ogg-vorbis", new CodecFactoryEntry(s => new NVorbisSource(s).ToWaveSource(), ".ogg"));

            // Make sure eq levels are set to 0
            for (int i = 0; i < eqArray.Length; i++)
            {
                eqArray[i] = 0;
            }

            RegisterMessaging();
        }

        private void RegisterMessaging()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<EqChangedMessage>(this, (message) =>
            {
                EqualizerFilter filter = _equalizer.SampleFilters[message.FilterIndex];
                filter.AverageGainDB = message.Value;

                eqArray[message.FilterIndex] = message.Value;
            });
        }

        #endregion

        #region Controls

        public void Pause()
        {
            if (_soundOut != null)
            {
                _soundOut.Pause();
                Status = PlayStatus.Paused;
                StopTimer();
            }
        }

        public void Play()
        {
            if (_soundOut != null)
            {
                _soundOut.Play();
                StartTimer();
                Status = PlayStatus.Playing;
            }
        }

        public void Play(string fileName)
        {
            if (_soundOut != null)
            {
                _soundOut.Play();
                StartTimer();
                Status = PlayStatus.Playing;
            }
            else
            {
                Open(fileName);
                Status = PlayStatus.Playing;
            }
        }


        public void Resume()
        {
            if (_soundOut != null)
            {
                _soundOut.Resume();
                StartTimer();
                Status = PlayStatus.Playing;
            }
        }

        public void Stop()
        {
            if (_soundOut != null)
            {
                _soundOut.Stop();
                Status = PlayStatus.Stopped;
                StopTimer();
            }
        }

        public void SetCurrentPosition(double position)
        {
            if (_waveSource != null)
            {
                Position = TimeSpan.FromMilliseconds(position);
            }
        }

        public void Open(string fileName)
        {
            _waveSource = CodecFactory.Instance.GetCodec(fileName).ToSampleSource().AppendSource(Equalizer.Create10BandEqualizer, out _equalizer).ToWaveSource();
            
            // Set the eq for the track
            for (int i = 0; i < eqArray.Length; i++)
            {
                EqualizerFilter filter = _equalizer.SampleFilters[i];
                filter.AverageGainDB = eqArray[i];
            }
            
            if (WasapiOut.IsSupportedOnCurrentPlatform)
            {
                _soundOut = new WasapiOut() { Latency = 100 };
            }
            else
            {
                _soundOut = new DirectSoundOut() { Latency = 100 };
            }
            
            _soundOut.Initialize(_waveSource);
            
            OnMediaOpened(_waveSource.GetLength().TotalMilliseconds);

            _soundOut.Stopped += OnPlaybackStopped;
            
            StartTimer();
            _soundOut.Play();
        }

        private void StartTimer()
        {
            positionTimer = new Timer();
            positionTimer.Interval = timerInterval;
            positionTimer.Tick += PositionTimer_Tick;
            positionTimer.Enabled = true;
            positionTimer.Start();
        }

        private void StopTimer()
        {
            if (positionTimer != null)
            {
                positionTimer.Enabled = false;
                positionTimer.Stop();
                positionTimer.Tick -= PositionTimer_Tick;
                positionTimer = null;
            }
        }

        public void CleanUpPlayback()
        {
            StopTimer();

            if (_soundOut != null)
            {
                _soundOut.Stop();
                _soundOut.Dispose();
                _soundOut = null;
            }

            if (_waveSource != null)
            {
                _waveSource.Dispose();
                _waveSource = null;
            }
        }

        #endregion

        #region Events

        private void PositionTimer_Tick(object sender, EventArgs e)
        {
            OnClockUpdated(this, EventArgs.Empty);
        }

        public event ClockUpdatedEventHandler ClockUpdated;

        public void OnClockUpdated(object sender, EventArgs e)
        {
            double pos = _waveSource.GetPosition().TotalMilliseconds;
            double total = _waveSource.GetLength().TotalMilliseconds;

            if (pos >= total - 50)
            {
                OnMediaEnded(sender, EventArgs.Empty);
            }

            ClockUpdated?.Invoke(this, new ClockUpdatedEventArgs(_waveSource != null ? _waveSource.GetPosition() : new TimeSpan(0)));
        }

        public event ClockCompletedEventHandler ClockCompleted;

        public void OnClockCompleted(object sender, EventArgs e)
        {
            ClockCompleted?.Invoke(sender, e);
        }

        public event MediaOpenedEventHandler MediaOpened;

        public void OnMediaOpened(double length)
        {
            MediaOpened?.Invoke(this, new MediaOpenedEventArgs(length));
        }

        public void OnPlaybackStopped(object sender, PlaybackStoppedEventArgs e)
        {

        }

        public event MediaEndedEventHandler MediaEnded;

        public void OnMediaEnded(object sender, EventArgs e)
        {
            MediaEnded?.Invoke(sender, e);
        }

        public event MediaErrorEventHandler MediaError;

        public void OnMediaError(object sender, EventArgs e)
        {
            MediaError?.Invoke(sender, e);
        }

        #endregion


        #region Dispose

        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        System.Runtime.InteropServices.SafeHandle handle = new Microsoft.Win32.SafeHandles.SafeFileHandle(IntPtr.Zero, true);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
            }

            // Free any unmanaged objects here.
            //
            CleanUpPlayback();
            disposed = true;
        }

        ~CSMediaPlayer()
        {
            Dispose(false);
        }

        #endregion
    }
}
