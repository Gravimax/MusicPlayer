using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MusicPlayer.Database;
using MusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace MusicPlayer.ViewModel
{
    public class PlayListControlViewModel : ViewModelBase, IDisposable
    {
        #region Properties

        private PlayListConfig playConfig;
        private List<MediaItem> corePlayList;

        private List<MediaItem> _history = new List<MediaItem>();

        private int historyIndex = 0;
        private int currentIndex = 0;

        private ListType currentListType;
        private int currentPlaylistId;

        ObservableCollection<MediaItem> _currentPlayList;
        public ObservableCollection<MediaItem> CurrentPlayList
        {
            get
            {
                return _currentPlayList;
            }

            set
            {
                _currentPlayList = value;
                RaisePropertyChanged("CurrentPlayList");
            }
        }

        // Playback follows cursor or double click
        private MediaItem __userSelected;
        public MediaItem UserSelected
        {
            get
            {
                return __userSelected;
            }

            set
            {
                __userSelected = value;
                RaisePropertyChanged("UserSelected");
            }
        }

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
            }
        }

        #region Music Filters

        string _selectedAlbumFilter;

        public string SelectedAlbumFilter
        {
            get { return _selectedAlbumFilter; }
            set
            {
                _selectedAlbumFilter = value;
                RaisePropertyChanged("SelectedAlbumFilter");
            }
        }

        List<string> _albumFilters;

        public List<string> AlbumFilters
        {
            get { return _albumFilters; }
            set
            {
                _albumFilters = value;
                RaisePropertyChanged("AlbumFilters");
            }
        }

        string _selectedArtistFilter;

        public string SelectedArtistFilter
        {
            get { return _selectedArtistFilter; }
            set
            {
                _selectedArtistFilter = value;
                RaisePropertyChanged("SelectedArtistFilter");
            }
        }

        List<string> _artistFilters;

        public List<string> ArtistFilters
        {
            get { return _artistFilters; }
            set
            {
                _artistFilters = value;
                RaisePropertyChanged("ArtistFilters");
            }
        }

        string _selectedGenreFilter;

        public string SelectedGenreFilter
        {
            get { return _selectedGenreFilter; }
            set
            {
                _selectedGenreFilter = value;
                RaisePropertyChanged("SelectedGenreFilter");
            }
        }

        List<string> _genreFilters;

        public List<string> GenreFilters
        {
            get { return _genreFilters; }
            set
            {
                _genreFilters = value;
                RaisePropertyChanged("GenreFilters");
            }
        }

        #endregion

        #region Delegates

        public DelegateCommand PlaySelectionCommand { get; private set; }
        public DelegateCommand AlbumFilterCommand { get; private set; }
        public DelegateCommand ArtistFilterCommand { get; private set; }
        public DelegateCommand GenreFilterCommand { get; private set; }
        public DelegateCommand SortCommand { get; private set; }
        public DelegateCommand EditSelectedFileCommand { get; private set; }
        public DelegateCommand RemoveSelectedFileCommand { get; private set; }
        public DelegateCommand AddToPlaylistCommand { get; private set; }

        #endregion

        #endregion


        #region Ctors


        public PlayListControlViewModel()
        {
            PlaySelectionCommand = new DelegateCommand(PlaySelection);
            AlbumFilterCommand = new DelegateCommand(AlbumFilterPlaylist);
            ArtistFilterCommand = new DelegateCommand(ArtistFilterPlaylist);
            GenreFilterCommand = new DelegateCommand(GenreFilterPlaylist);
            SortCommand = new DelegateCommand(Sort);
            EditSelectedFileCommand = new DelegateCommand(EditCurrentFile);
            RemoveSelectedFileCommand = new DelegateCommand(RemoveCurrentFile);
            AddToPlaylistCommand = new DelegateCommand(AddToPlaylist);

            playConfig = FileSupport.AppConfiguration.LoadPlayListConfig();
            _sortColumn = playConfig.SortColumn;
            _sortDirection = playConfig.SortDirection;

            RegisterMessaging();
        }


        #endregion


        #region Support


        private void RegisterMessaging()
        {
            Messenger.Default.Register<PlayTypeUpdatedMessage>(this, (message) =>
            {
                if (message.PlayType != PlayType.None)
                {
                    historyIndex = 0;
                    currentIndex = 0;
                }
            });

            Messenger.Default.Register<ListTypeUpdatedMessage>(this, (message) =>
            {
                currentListType = message.ListType;
                currentPlaylistId = message.CurrentPlaylistId;
            });

            Messenger.Default.Register<PlayListUpdatedMessage>(this, (message) =>
            {
                corePlayList = message.NewPlayList;
                CurrentPlayList = message.NewPlayList.ToObservableCollection();
                if (corePlayList != null)
                {
                    AlbumFilters = GetAlbumFilters(corePlayList);
                    ArtistFilters = GetArtistFilters(corePlayList);
                    GenreFilters = GetGenreFilters(corePlayList);
                }
            });

            Messenger.Default.Register<AppClosingMessage>(this, (message) =>
            {
                FileSupport.AppConfiguration.SavePlayListConfig(playConfig);
                this.Dispose();
            });

            Messenger.Default.Register<GetNextFileMessage>(this, (message) =>
            {
                OnGetNextFile(message);
            });

            Messenger.Default.Register<GetPreviousFileMessage>(this, (message) =>
            {
                OnGetPreviousFile(message);
            });

            Messenger.Default.Register<FileUpdatedMessage>(this, (message) =>
            {
                OnFileUpdated(message);
            });
        }


        #endregion


        #region Next/Previous

        private void PlaySelection(object args)
        {
            PlayUserSelected();
        }

        public void OnGetNextFile(GetNextFileMessage e)
        {
            switch (e.SelectedPlayType)
            {
                case PlayType.Normal:
                    PlayNextNormal();
                    break;
                case PlayType.Random:
                    PlayNextRandom();
                    break;
                case PlayType.Shuffle:
                    PlayNextShuffle();
                    break;
                default:
                    PlayNextNormal();
                    break;
            }
        }

        public void OnGetPreviousFile(GetPreviousFileMessage e)
        {
            switch (e.SelectedPlayType)
            {
                case PlayType.Normal:
                    PlayPreviousNormal();
                    break;
                case PlayType.Random:
                    PlayPreviousRandom();
                    break;
                case PlayType.Shuffle:
                    PlayPreviousShuffle();
                    break;
                default:
                    PlayPreviousNormal();
                    break;
            }
        }

        private void PlayPreviousNormal()
        {
            currentIndex--;
            if (currentIndex < 0) { currentIndex = 0; }
            CurrentFile = CurrentPlayList.ElementAt(currentIndex);
            UserSelected = CurrentFile;
            OnPlayFile(_currentFile, currentIndex);
        }

        private void PlayPreviousRandom()
        {
            historyIndex--;
            if (historyIndex < 0) { historyIndex = 0; }
            if (_history.Count == 0) { return; }
            CurrentFile = corePlayList.ElementAt(historyIndex);
            UserSelected = CurrentFile;
            OnPlayFile(_currentFile, historyIndex);
        }

        private void PlayPreviousShuffle()
        {
            historyIndex--;
            if (historyIndex < 0) { historyIndex = 0; }
            CurrentFile = corePlayList.ElementAt(historyIndex);
            UserSelected = CurrentFile;
            OnPlayFile(_currentFile, historyIndex);
        }

        private void PlayNextNormal()
        {
            if (UserSelected != null && UserSelected != CurrentFile)
            {
                PlayUserSelected();
            }
            else
            {
                currentIndex++;
                if (currentIndex < (corePlayList.Count - 1))
                {
                    CurrentFile = CurrentPlayList.ElementAt(currentIndex);
                    UserSelected = CurrentFile;
                    OnPlayFile(_currentFile, currentIndex);
                }
            }
        }

        private void PlayNextRandom()
        {
            if (UserSelected != null)
            {
                PlayUserSelected();
            }
            else
            {
                _history.Add(corePlayList.ElementAt(currentIndex));
                historyIndex = _history.Count;

                Random next = new Random();
                currentIndex = next.Next(0, corePlayList.Count);
                CurrentFile = CurrentPlayList.ElementAt(currentIndex);
                UserSelected = CurrentFile;

                OnPlayFile(_currentFile, currentIndex);
            }
        }

        private void PlayNextShuffle()
        {
            if (UserSelected != null)
            {
                PlayUserSelected();
            }
            else
            {
                _history.Add(corePlayList.ElementAt(currentIndex));
                historyIndex = _history.Count;

                List<MediaItem> temp = GetAvailableFiles();
                if (temp.Count > 0)
                {
                    Random next = new Random();
                    int index = next.Next(0, temp.Count);
                    CurrentFile = temp.ElementAt(index);
                    currentIndex = CurrentPlayList.IndexOf(_currentFile);
                    UserSelected = CurrentFile;

                    OnPlayFile(_currentFile, currentIndex);
                }
            }
        }

        private void PlayUserSelected()
        {
            if (currentIndex >= 0)
            {
                currentIndex = CurrentPlayList.IndexOf(UserSelected);
                CurrentFile = CurrentPlayList.ElementAt(currentIndex);
                OnPlayFile(_currentFile, currentIndex);
                UserSelected = null;
            }
        }

        private List<MediaItem> GetAvailableFiles()
        {
            List<MediaItem> temp = new List<MediaItem>();

            foreach (var file in CurrentPlayList)
            {
                if (_history.SingleOrDefault(x => x.FilePath == file.FilePath) == null)
                {
                    temp.Add(file);
                }
            }

            return temp;
        }

        #endregion


        #region Sort

        ListSortDirection _sortDirection;
        string _sortColumn;

        private void Sort(object args)
        {
            string column = args as string;
            if (column != null)
            {
                if (_sortColumn == column)
                {
                    // Toggle sorting direction 
                    _sortDirection = _sortDirection == ListSortDirection.Ascending ?
                                                       ListSortDirection.Descending :
                                                       ListSortDirection.Ascending;
                }
                else
                {
                    _sortColumn = column;
                    _sortDirection = ListSortDirection.Ascending;
                }

                if (_sortDirection == ListSortDirection.Ascending)
                {
                    OrderByAscending(column);
                }
                else
                {
                    OrderByDecending(column);
                }

                playConfig.SortColumn = _sortColumn;
                playConfig.SortDirection = _sortDirection;
                OnPlayListConfigUpdated(_sortColumn, _sortDirection);
            }
        }

        private void OrderByAscending(string column)
        {
            switch (column)
            {
                case "Artist":
                    CurrentPlayList = CurrentPlayList.OrderBy(x => x.Artist).ThenBy(y => y.Album).ThenBy(z => z.Track).ToObservableCollection();
                    break;
                case "Album":
                    CurrentPlayList = CurrentPlayList.OrderBy(x => x.Album).ToObservableCollection();
                    break;
                case "Title":
                    CurrentPlayList = CurrentPlayList.OrderBy(x => x.Title).ToObservableCollection();
                    break;
                case "Year":
                    CurrentPlayList = CurrentPlayList.OrderBy(x => x.Year).ToObservableCollection();
                    break;
                case "BitRate":
                    CurrentPlayList = CurrentPlayList.OrderBy(x => x.BitRate).ToObservableCollection();
                    break;
                case "Genre":
                    CurrentPlayList = CurrentPlayList.OrderBy(x => x.Genre).ToObservableCollection();
                    break;
                case "Duration":
                    CurrentPlayList = CurrentPlayList.OrderBy(x => x.Duration).ToObservableCollection();
                    break;
                case "Track":
                    CurrentPlayList = CurrentPlayList.OrderBy(x => x.Track).ToObservableCollection();
                    break;
                case "Time":
                    CurrentPlayList = CurrentPlayList.OrderBy(x => x.Duration).ToObservableCollection();
                    break;
                default:
                    break;
            }
        }

        private void OrderByDecending(string column)
        {
            switch (column)
            {
                case "Artist":
                    CurrentPlayList = CurrentPlayList.OrderByDescending(x => x.Artist).ThenBy(y => y.Album).ThenBy(z => z.Track).ToObservableCollection();
                    break;
                case "Album":
                    CurrentPlayList = CurrentPlayList.OrderByDescending(x => x.Album).ToObservableCollection();
                    break;
                case "Title":
                    CurrentPlayList = CurrentPlayList.OrderByDescending(x => x.Title).ToObservableCollection();
                    break;
                case "Year":
                    CurrentPlayList = CurrentPlayList.OrderByDescending(x => x.Year).ToObservableCollection();
                    break;
                case "BitRate":
                    CurrentPlayList = CurrentPlayList.OrderByDescending(x => x.BitRate).ToObservableCollection();
                    break;
                case "Genre":
                    CurrentPlayList = CurrentPlayList.OrderByDescending(x => x.Genre).ToObservableCollection();
                    break;
                case "Duration":
                    CurrentPlayList = CurrentPlayList.OrderByDescending(x => x.Duration).ToObservableCollection();
                    break;
                case "Track":
                    CurrentPlayList = CurrentPlayList.OrderByDescending(x => x.Track).ToObservableCollection();
                    break;
                case "Time":
                    CurrentPlayList = CurrentPlayList.OrderByDescending(x => x.Duration).ToObservableCollection();
                    break;
                default:
                    break;
            }
        }

        #endregion


        #region Filter Playlist

        private void AlbumFilterPlaylist(object args)
        {
            if (_selectedAlbumFilter != null)
            {
                if (_selectedAlbumFilter == "All")
                {
                    CurrentPlayList = corePlayList.ToObservableCollection();
                }
                else
                {
                    CurrentPlayList = corePlayList.Where(x => x.Album == _selectedAlbumFilter).ToObservableCollection();
                }
            }
        }

        private List<string> GetAlbumFilters(List<MediaItem> files)
        {
            List<string> filters = new List<string>();
            filters.Add("All");
            filters.AddRange(files.OrderBy(y => y.Album).Select(x => x.Album ?? string.Empty).Distinct().ToList());
            return filters;
        }

        private void ArtistFilterPlaylist(object args)
        {
            if (_selectedArtistFilter != null)
            {
                if (_selectedArtistFilter == "All")
                {
                    CurrentPlayList = corePlayList.ToObservableCollection();
                }
                else
                {
                    CurrentPlayList = corePlayList.Where(x => x.Artist == _selectedArtistFilter).ToObservableCollection();
                }
            }
        }

        private List<string> GetArtistFilters(List<MediaItem> files)
        {
            List<string> filters = new List<string>();
            filters.Add("All");
            filters.AddRange(files.OrderBy(y => y.Artist).Select(x => x.Artist ?? string.Empty).Distinct().ToList());
            return filters;
        }

        private void GenreFilterPlaylist(object args)
        {
            if (_selectedGenreFilter != null)
            {
                if (_selectedGenreFilter == "All")
                {
                    CurrentPlayList = corePlayList.ToObservableCollection();
                }
                else
                {
                    CurrentPlayList = corePlayList.Where(x => x.Genre == _selectedGenreFilter).ToObservableCollection();
                }
            }
        }

        private List<string> GetGenreFilters(List<MediaItem> files)
        {
            List<string> filters = new List<string>();
            filters.Add("All");
            filters.AddRange(files.OrderBy(y => y.Genre).Select(x => x.Genre ?? string.Empty).Distinct().ToList());
            return filters;
        }

        #endregion


        #region File Support


        public void OnFileUpdated(FileUpdatedMessage e)
        {
            switch (e.ChangeType)
            {
                case System.IO.WatcherChangeTypes.Created:
                    if (currentListType == ListType.Library)
                    {
                        corePlayList.Add(e.NewMediaItem);
                        CurrentPlayList.Add(e.NewMediaItem);
                    }
                    break;
                case System.IO.WatcherChangeTypes.Deleted:
                    MediaItem deletedItem = CurrentPlayList.FirstOrDefault(x => x.FilePath == e.OldName);
                    if (deletedItem != null)
                    {
                        CurrentPlayList.Remove(deletedItem);
                        corePlayList.Remove(deletedItem);
                    }
                    break;
                case System.IO.WatcherChangeTypes.Renamed:
                    MediaItem updatedItem = CurrentPlayList.FirstOrDefault(x => x.FilePath == e.OldName);
                    if (updatedItem != null)
                    {
                        updatedItem.FilePath = e.NewName;
                    }
                    break;
                default:
                    break;
            }
        }
        

        private void RemoveCurrentFile(object args)
        {
            if (args != null)
            {
                MediaItem file = args as MediaItem;

                if (file != null)
                {
                    Views.DeleteConfirmation delConfirm = new Views.DeleteConfirmation();
                    delConfirm.Closed += (sender, e) =>
                    {
                        if (delConfirm.DialogResult == true)
                        {
                            DatabaseAccess dba = new DatabaseAccess();
                            switch (currentListType)
                            {
                                case ListType.Playlist:
                                    dba.RemoveMediaItemFromPlaylist(currentPlaylistId, file);
                                    break;
                                default:
                                    break;
                            }

                            CurrentPlayList.Remove(file);
                            corePlayList.Remove(file);
                            if (_history.Contains(file))
                            {
                                _history.Remove(file);
                            }

                            if (delConfirm.DeletionType == FileDeleteType.ListAndComputer)
                            {
                                dba.DeleteMediaItem(file.Id);
                            };
                        }
                    };
                    delConfirm.ShowDialog();
                }
            }
        }

        private void AddToPlaylist(object args)
        {
            System.Collections.IList items = (System.Collections.IList)args;

            var files = items.Cast<MediaItem>().ToList();

            if (files != null && files.Count > 0)
            {
                MusicPlayer.Views.SelectPlaylist select = new MusicPlayer.Views.SelectPlaylist();
                select.Closed += (sender, e) =>
                {
                    if (select.SelectedList != null)
                    {
                        if (currentPlaylistId > 0 && select.SelectedList.Id != currentPlaylistId)
                        {
                            DatabaseAccess dba = new DatabaseAccess();
                            dba.AddMediaItemsToPlaylist(select.SelectedList.Id, files);
                        }
                    }
                };
                select.ShowDialog();
            }
        }


        #endregion


        #region Edit File Properties


        private void EditCurrentFile(object args)
        {
            if (args != null)
            {
                MediaItem file = args as MediaItem;

                if (file != null)
                {
                    Views.FileProperties props = new Views.FileProperties(file.FilePath);
                    props.Closed += (sender, e) =>
                    {
                        if (props.DialogResult == true && props.SelectedFile != null)
                        {
                            // Update mediaitem properties
                        }
                    };
                    props.ShowDialog();
                }
            }
        }


        #endregion


        #region Messaging


        protected void OnPlayFile(MediaItem musicFile, int index)
        {
            Messenger.Default.Send<SelectionChangedMessage>(new SelectionChangedMessage(index));
            Messenger.Default.Send<PlayFileMessage>(new PlayFileMessage(musicFile));
        }

        protected void OnPlayListConfigUpdated(string columnName, ListSortDirection sortDirection)
        {
            Messenger.Default.Send<PlayListConfigUpdateMessage>(new PlayListConfigUpdateMessage(columnName, sortDirection));
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
            //
            disposed = true;
        }

        #endregion
    }
}
