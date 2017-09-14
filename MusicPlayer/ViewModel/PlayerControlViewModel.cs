using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MusicPlayer.Models;
using MusicPlayer.PlayerCore;
using System;

namespace MusicPlayer.ViewModel
{
    public class PlayerControlViewModel : ViewModelBase, IDisposable
    {
        #region Properties


        private PlayerControlConfig playerConfig;
        private CSMediaPlayer mediaPlayer;

        #region Playtype

        private PlayType _playType;

        public PlayType OrderType
        {
            get { return _playType; }
            set
            {
                _playType = value;
                playerConfig.PlayType = value;
                RaisePropertyChanged("NormalPlayType");
                RaisePropertyChanged("RandomPlayType");
                RaisePropertyChanged("SufflePlayType");
            }
        }

        public bool NormalPlayType
        {
            get { return _playType == PlayType.Normal; }
            set
            {
                OrderType = PlayType.Normal;
                RaisePropertyChanged("NormalPlayType");
            }
        }

        public bool RandomPlayType
        {
            get { return _playType == PlayType.Random; }
            set
            {
                OrderType = PlayType.Random;
                RaisePropertyChanged("RandomPlayType");
            }
        }

        public bool SufflePlayType
        {
            get { return _playType == PlayType.Shuffle; }
            set
            {
                OrderType = PlayType.Shuffle;
                RaisePropertyChanged("SufflePlayType");
            }
        }

        private bool _repeatSelection;
        public bool RepeatSelection
        {
            get
            {
                return _repeatSelection;
            }

            set
            {
                _repeatSelection = value;
            }
        }

        #endregion

        #region Current Values

        private MediaItem _currentFile;
        public MediaItem CurrentFile
        {
            get
            {
                return _currentFile;
            }

            set
            {
                _currentFile = value;
                RaisePropertyChanged("CurrentFile");
                RaisePropertyChanged("CurrentTitle");
            }
        }

        public string CurrentTitle
        {
            get
            {
                if (_currentFile != null)
                {
                    return _currentFile.Artist + " - " + _currentFile.Title;
                }

                return "";
            }
        }

        private double _volume;

        public double Volume
        {
            get
            {
                return mediaPlayer != null ? mediaPlayer.Volume : 0;
            }
            set
            {
                if (mediaPlayer != null)
                {
                    mediaPlayer.Volume = value;
                    RaisePropertyChanged("Volume");
                }
            }
        }

        private bool _isMuted;
        public bool IsMuted
        {
            get
            {
                return _isMuted;
            }
            set
            {
                if (mediaPlayer != null)
                {
                    _isMuted = value;
                    mediaPlayer.IsMuted = value;
                    RaisePropertyChanged("IsMuted");
                    RaisePropertyChanged("Volume");
                }
            }
        }

        double _mediaTimeLength;

        public double MediaTimeLength
        {
            get
            {
                return _mediaTimeLength;
            }

            set
            {
                _mediaTimeLength = value;
                RaisePropertyChanged("MediaTimeLength");
            }
        }

        string _currentTime;
        public string CurrentTime
        {
            get
            {
                return _currentTime;
            }

            set
            {
                _currentTime = value;
                RaisePropertyChanged("CurrentTime");
            }
        }

        double _currentPosition;
        public double CurrentPosition
        {
            get
            {
                return _currentPosition;
            }

            set
            {
                _currentPosition = value;
                RaisePropertyChanged("CurrentPosition");
            }
        }

        private bool _userMovingSlider;

        string _timeLength;
        public string TimeLength
        {
            get { return _timeLength; }
            set
            {
                _timeLength = value;
                RaisePropertyChanged("TimeLength");
            }
        }

        public bool Playing
        {
            get
            {
                if (mediaPlayer != null)
                {
                    switch (mediaPlayer.Status)
                    {
                        case PlayStatus.Paused:
                        case PlayStatus.Stopped:
                        default:
                            return false;
                        case PlayStatus.Playing:
                            return true;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        #endregion

        #region Delegates

        public DelegateCommand PlayPauseSelectionCommand { get; private set; }
        public DelegateCommand StopSelectionCommand { get; private set; }
        public DelegateCommand PreviousSelectionCommand { get; private set; }
        public DelegateCommand NextSelectionCommand { get; private set; }
        public DelegateCommand PreviewMouseDownCommand { get; private set; }
        public DelegateCommand PreviewMouseUpCommand { get; private set; }
        public DelegateCommand ShowEqualizerCommand { get; private set; }

        #endregion


        #endregion


        #region Ctors


        public PlayerControlViewModel()
        {
            PlayPauseSelectionCommand = new DelegateCommand(PlayPause);
            StopSelectionCommand = new DelegateCommand(Stop);
            PreviousSelectionCommand = new DelegateCommand(Previous);
            NextSelectionCommand = new DelegateCommand(Next);
            PreviewMouseDownCommand = new DelegateCommand(PreviewMouseLeftButtonDown);
            PreviewMouseUpCommand = new DelegateCommand(PreviewMouseLeftButtonUp);
            ShowEqualizerCommand = new DelegateCommand(ShowEqualizer);

            mediaPlayer = new CSMediaPlayer();
            mediaPlayer.MediaOpened += this.mediaElement_MediaOpened;
            mediaPlayer.MediaEnded += this.mediaElement_MediaEnded;
            mediaPlayer.MediaError += this.mediaPlayer_MediaFailed;
            mediaPlayer.ClockUpdated += this.Clock_CurrentTimeInvalidated;

            MediaTimeLength = 1000;
            CurrentPosition = 0;

            playerConfig = FileSupport.AppConfiguration.LoadPlayerControlConfig();
            OrderType = playerConfig.PlayType;
            _volume = playerConfig.Volume;
            Volume = playerConfig.Volume;

            RegisterMessaging();
        }


        #endregion


        #region Support


        private void RegisterMessaging()
        {
            Messenger.Default.Register<AppClosingMessage>(this, (message) =>
            {
                FileSupport.AppConfiguration.SavePlayerControlConfig(playerConfig);
                this.Dispose();
            });

            Messenger.Default.Register<PlayFileMessage>(this, (message) =>
            {
                OnPlaySelectedFile(message);
            });
        }

        private void ShowEqualizer(object args)
        {
            Views.Equalizer eq = new Views.Equalizer(playerConfig.LastEqSettings);
            eq.Closed += (sender, e) =>
            {
                playerConfig.LastEqSettings = eq.CntrlEq.SelectedEq.Name;
            };
            eq.ShowDialog();
        }


        #endregion


        #region Basic Controls


        private void PlayPause(object args)
        {
            if (_currentFile != null)
            {
                if (mediaPlayer.IsPaused)
                {
                    mediaPlayer.Resume();
                }
                else
                {
                    if (mediaPlayer.CanPause)
                    {
                        mediaPlayer.Pause();
                    }
                }
            }

            RaisePropertyChanged("Playing");
        }

        private void Previous(object args)
        {
            OnGetPreviousFile(_playType);
        }

        private void Next(object args)
        {
            OnGetNextFile(_playType);
        }

        public void OnPlaySelectedFile(PlayFileMessage e)
        {
            if (e.SelectedMusicFile != null)
            {
                if (e.SelectedMusicFile != _currentFile)
                {
                    CurrentFile = e.SelectedMusicFile;
                }

                mediaPlayer.Stop();
                mediaPlayer.CleanUpPlayback();
                mediaPlayer.Play(CurrentFile.FilePath);

                RaisePropertyChanged("Playing");
            }
        }

        private void Stop(object args)
        {
            mediaPlayer.Stop();
            RaisePropertyChanged("Playing");
        }


        #endregion


        #region Player Events


        private void Clock_CurrentTimeInvalidated(object sender, ClockUpdatedEventArgs e)
        {
            if (_userMovingSlider) { return; }

            CurrentPosition = e.CurrentTime.Value.TotalMilliseconds;
            if (e.CurrentTime.Value.Hours > 0)
            {
                CurrentTime = e.CurrentTime.Value.ToString(@"hh\:mm\:ss");
            }
            else
            {
                CurrentTime = e.CurrentTime.Value.ToString(@"mm\:ss");
            }
        }

        private void Clock_Completed(object sender, EventArgs e)
        {
            mediaElement_MediaEnded(sender, e);
        }

        private void PreviewMouseLeftButtonDown(object args)
        {
            _userMovingSlider = true;
        }

        private void PreviewMouseLeftButtonUp(object args)
        {
            mediaPlayer.SetCurrentPosition(CurrentPosition);
            _userMovingSlider = false;
        }

        //public void SliderLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    _userMovingSlider = true;

        //    System.Windows.Controls.Slider slider = sender as System.Windows.Controls.Slider;
        //    System.Windows.Point pos = e.GetPosition(slider);
        //    double dblValue = (pos.X / (double)slider.ActualWidth) * (slider.Maximum - slider.Minimum);
        //    CurrentPosition = dblValue;
        //    mediaPlayer.SetCurrentPosition(CurrentPosition);

        //    _userMovingSlider = false;
        //}

        private void mediaElement_MediaOpened(object sender, MediaOpenedEventArgs e)
        {
            MediaTimeLength = e.Length;
        }

        private void mediaElement_MediaEnded(object sender, EventArgs e)
        {
            if (_repeatSelection)
            {
                mediaPlayer.SetCurrentPosition(0);

                return;
            }

            OnGetNextFile(_playType);
        }

        private void mediaPlayer_MediaFailed(object sender, EventArgs e)
        {
            OnAlertMessage("Error", "Unsupported or currupt file");
            Next(null);
        }


        #endregion


        #region Messaging


        protected void OnGetNextFile(PlayType playType)
        {
            Messenger.Default.Send<GetNextFileMessage>(new GetNextFileMessage(_playType));
        }

        protected void OnGetPreviousFile(PlayType playType)
        {
            Messenger.Default.Send<GetPreviousFileMessage>(new GetPreviousFileMessage(_playType));
        }

        protected void OnAlertMessage(string title, string message)
        {
            Messenger.Default.Send<AlertMessageMessage>(new AlertMessageMessage(title, message));
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
                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            mediaPlayer.Stop();
            mediaPlayer.Dispose();
            mediaPlayer = null;
            disposed = true;
        }

        #endregion
    }
}
