using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MusicPlayer.Database;
using MusicPlayer.FileSupport;
using MusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace MusicPlayer.ViewModel
{
    public class ApplicationViewModel : ViewModelBase, IDisposable
    {
        #region Properties


        public AppConfig appConfig;
        FolderWatcher folderWatcher;

        private ListType currentListType;
        //public List<string> Genres { get; set; }

        #region Playlists

        ObservableCollection<Playlist> _playLists;
        public ObservableCollection<Playlist> PlayLists
        {
            get
            {
                return _playLists;
            }

            set
            {
                _playLists = value;
                RaisePropertyChanged("PlayLists");
            }
        }

        Playlist _selectedPlaylist;
        public Playlist SelectedPlayList
        {
            get { return _selectedPlaylist; }
            set
            {
                _selectedPlaylist = value;
                RaisePropertyChanged("SelectedPlayList");
            }
        }

        #endregion


        #region Music Libraries

        private DirectoryItem _directoryItems;

        public DirectoryItem DirectoryItems
        {
            get { return _directoryItems; }
            set
            {
                _directoryItems = value;
                RaisePropertyChanged("DirectoryItems");
            }
        }

        private Library _library;

        public Library SelectedLibrary
        {
            get { return _library; }
            set
            {
                _library = value;
                RaisePropertyChanged("SelectedLibrary");
            }
        }

        private DirectoryItem _selectedFolder;

        public DirectoryItem SelectedFolder
        {
            get { return _selectedFolder; }
            set
            {
                _selectedFolder = value;
                RaisePropertyChanged("SelectedFolder");
            }
        }

        //MusicLibrary _myMusicLibraries;

        //public MusicLibrary MyMusicLibrary
        //{
        //    get { return _myMusicLibraries; }
        //    set
        //    {
        //        _myMusicLibraries = value;
        //        RaisePropertyChanged("MyMusicLibrary");
        //    }
        //}

        //ObservableCollection<DirectoryItem> _directories;
        //public ObservableCollection<DirectoryItem> Directories
        //{
        //    get { return _directories; }
        //    set
        //    {
        //        _directories = value;
        //        RaisePropertyChanged("Directories");
        //    }
        //}

        //private DirectoryItem selectedFolder;

        #endregion


        #region Delegates

        public DelegateCommand AddNewLibraryCommand { get; private set; }
        public DelegateCommand RemoveLibraryCommand { get; private set; }
        public DelegateCommand RescanLibraryCommand { get; private set; }


        public DelegateCommand AddNewPlaylistCommand { get; private set; }
        public DelegateCommand AddFolderToPlaylistCommand { get; private set; }
        public DelegateCommand AddFilesToPlaylistCommand { get; private set; }
        public DelegateCommand RenamePlaylistCommand { get; private set; }
        public DelegateCommand DeletePlaylistCommand { get; private set; }

        public DelegateCommand SelectPlayListCommand { get; private set; }
        public DelegateCommand SetPlayListCommand { get; private set; }

        public DelegateCommand CloseApplicationCommand { get; private set; }

        #endregion


        #endregion


        #region Ctors


        public ApplicationViewModel()
        {
            AddNewLibraryCommand = new DelegateCommand(AddNewLibrary);
            RemoveLibraryCommand = new DelegateCommand(RemoveLibrary);
            RescanLibraryCommand = new DelegateCommand(RescanFolder);

            AddNewPlaylistCommand = new DelegateCommand(AddNewPlaylist);
            AddFolderToPlaylistCommand = new DelegateCommand(AddFolderToPlaylist);
            AddFilesToPlaylistCommand = new DelegateCommand(AddFilesToPlaylist);
            RenamePlaylistCommand = new DelegateCommand(RenamePlaylist);
            DeletePlaylistCommand = new DelegateCommand(DeletePlaylist);

            SelectPlayListCommand = new DelegateCommand(SelectPlayList);
            SetPlayListCommand = new DelegateCommand(SetPlaylist);

            CloseApplicationCommand = new DelegateCommand(CloseApplication);

            folderWatcher = new FolderWatcher();
            folderWatcher.FileChanged += OnFileChanged;
            folderWatcher.FileRenamed += OnFileRenamed;
            LoadAppConfig();

            SetMessaging();

            LoadData();
        }


        #endregion


        #region Support


        private void SetMessaging()
        {
            Messenger.Default.Register<FolderChangedMessage>(this, (message) =>
            {
                FolderSelectionChanged(message);
            });

            Messenger.Default.Register<AppClosingMessage>(this, (message) =>
            {
                FileSupport.AppConfiguration.SaveAppConfig(appConfig);
                this.Dispose();
            });
        }

        public AppConfig LoadAppConfig()
        {
            appConfig = FileSupport.AppConfiguration.LoadAppConfig();
            return appConfig;
        }

        private void LoadData()
        {
            //Genres = AppConfiguration.GetGenreList();

            LoadPlayLists();
            LoadMusicLibraries();

            // Set last lib or ply and sort
            SetLastList(appConfig);
        }

        private void SetLastList(AppConfig appConfig)
        {
            DatabaseAccess dba = new DatabaseAccess();

            if (!string.IsNullOrEmpty(appConfig.LibraryFolder))
            {
                if (SelectedLibrary != null)
                {
                    var tempPlayList = dba.GetMediaFilesByPath(SelectedLibrary.Path);
                    OnPlayListUpdated(tempPlayList);

                    currentListType = ListType.Library;
                    OnApplicationUpdated(currentListType, _selectedPlaylist != null ? _selectedPlaylist.Id : 0);
                }
            }
            else if (!string.IsNullOrEmpty(appConfig.Playlist))
            {
                Playlist list = dba.GetPlaylistByName(appConfig.Playlist);
                if (list != null)
                {
                    var tempPlayList = dba.GetMediaFilesByPlaylist(list.Id);
                    OnPlayListUpdated(tempPlayList);

                    currentListType = ListType.Playlist;
                    OnApplicationUpdated(currentListType, _selectedPlaylist != null ? _selectedPlaylist.Id : 0);
                }
            }
        }


        #endregion


        #region Playlists


        private void AddNewPlaylist(object args)
        {
            Views.EditPlayList inputBox = new Views.EditPlayList("", "Playlist Name", PlayLists.Select(x => x.Name).ToList());
            inputBox.Closed += (sender, e) =>
            {
                if (inputBox.DialogResult == true)
                {
                    Playlist playlist = new Playlist();
                    playlist.Name = inputBox.Value;

                    DatabaseAccess dba = new DatabaseAccess();
                    dba.AddNewPlaylist(playlist);
                    PlayLists.Add(playlist);
                }
            };
            inputBox.ShowDialog();
        }

        private void AddFolderToPlaylist(object args)
        {
            //if (args != null)
            //{
            //    PlayList playList = args as PlayList;

            //    if (playList != null)
            //    {
            //        PlayList temp = FileScan.AddFolder();
            //        if (playList.MusicFiles != null)
            //        {
            //            playList.Folders.AddRange(temp.Folders);
            //            playList.MusicFiles.AddRange(temp.MusicFiles);
            //            PlayListSupport.SavePlayList(playList);
            //        }
            //    }
            //}
        }

        private void AddFilesToPlaylist(object args)
        {
            //if (args != null)
            //{
            //    Playlist playList = args as Playlist;

            //    if (playList != null)
            //    {
            //        List<MediaItem> temp = FileScan.AddFiles();
            //        if (temp != null && temp.Count > 0)
            //        {
            //            playList.MusicFiles.AddRange(temp);
            //            PlayListSupport.SavePlayList(playList);
            //        }
            //    }
            //}
        }

        private void RenamePlaylist(object args)
        {
            if (args != null)
            {
                Playlist playList = args as Playlist;
                string temp = playList.Name;
                if (playList != null)
                {
                    Views.EditPlayList inputBox = new Views.EditPlayList(playList.Name, "Change Playlist Name", PlayLists.Select(x => x.Name).ToList());
                    inputBox.Value = playList.Name;

                    inputBox.Closed += (sender, e) =>
                    {
                        if (inputBox.DialogResult == true)
                        {
                            if (!string.IsNullOrEmpty(inputBox.Value))
                            {
                                DatabaseAccess dba = new DatabaseAccess();
                                playList.Name = inputBox.Value;
                                dba.UpdatePlaylist(playList);
                            }
                            else
                            {
                                playList.Name = temp;
                            }
                        }
                    };
                    inputBox.ShowDialog();
                }
            }
        }

        private void DeletePlaylist(object args)
        {
            if (args != null)
            {
                Playlist playList = args as Playlist;
                if (playList != null)
                {
                    DatabaseAccess dba = new DatabaseAccess();
                    dba.DeletePlaylist(playList.Id);
                    PlayLists.Remove(playList);
                    SelectedPlayList = null;
                }
            }  
        }

        private void LoadPlayLists()
        {
            DatabaseAccess dba = new DatabaseAccess();
            PlayLists = dba.GetPlaylists().ToObservableCollection();
        }

        private void SetPlaylist(object args)
        {
            SelectPlayList(null);
        }

        private void SelectPlayList(object args)
        {
            SelectedFolder = null;
            appConfig.Playlist = _selectedPlaylist.Name;
            appConfig.LibraryFolder = string.Empty;
            OnApplicationUpdated(currentListType, _selectedPlaylist != null ? _selectedPlaylist.Id : 0);

            DatabaseAccess dba = new DatabaseAccess();
            var tempPlayList = dba.GetMediaFilesByPlaylist(_selectedPlaylist.Id);
            OnPlayListUpdated(tempPlayList);
            currentListType = ListType.Playlist;
            appConfig.SelectedListType = currentListType;
        }


        #endregion


        #region Libraries


        private void AddNewLibrary(object args)
        {
            // Eventually add watch option
            if (SelectedLibrary != null)
            {
                OnAlertMessage("Add library error", "There can only be one music library at a time");
            }
            else
            {
                Library library = FileScan.AddNewLibrary();
                if (library != null)
                {
                    SelectedLibrary = library;
                }
            }
        }

        private void RemoveLibrary(object args)
        {
            DatabaseAccess dba = new DatabaseAccess();
            dba.DeleteLibrary(SelectedLibrary.Id);
            if (SelectedLibrary.WatchFolder)
            {
                RemoveFileWatcher(SelectedLibrary.Path);
            }

            SelectedLibrary = null;

            //PlayListSupport.DeleteMusicLibrary();
            appConfig.LibraryFolder = string.Empty;
        }

        private void RescanFolder(object args)
        {
            DirectoryItem item = args as DirectoryItem;
            if (item != null)
            {
                FileScan.RescanFolder(SelectedLibrary, item.Path);
                ProcessLibrary();
            }
        }

        private void LoadMusicLibraries()
        {
            DatabaseAccess dba = new DatabaseAccess();
            SelectedLibrary = dba.GetLibrary();

            ProcessLibrary();

            if (SelectedLibrary != null && SelectedLibrary.WatchFolder)
            {
                AddFolderWatch(SelectedLibrary.Path);
            }
        }

        private void ProcessLibrary()
        {
            if (SelectedLibrary != null)
            {
                DirectoryItem dirItems = new DirectoryItem { Name = SelectedLibrary.Name, Path = SelectedLibrary.Path, Items = new List<DirectoryItem>() };
                DirectoryItem item = new DirectoryItem { Name = SelectedLibrary.Name, Path = SelectedLibrary.Path, Items = new List<DirectoryItem>() };
                dirItems.Items.Add(item);
                GetDirectories(item);
                DirectoryItems = dirItems;
            }
        }

        //private void GetDirectories(DirectoryItem dItem)
        //{
        //    System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(dItem.Path);

        //    var directories = info.GetDirectories();

        //    foreach (var item in directories)
        //    {
        //        DirectoryItem dirItem = new DirectoryItem { Name = item.Name, Path = item.FullName, Items = new List<DirectoryItem>() };
        //        dItem.Items.Add(dirItem);
        //        GetDirectories(dirItem);
        //    }
        //}

        private void GetDirectories(DirectoryItem dItem)
        {
            foreach (var item in SelectedLibrary.LibraryFolders.Where(x => x.ParentFolder == dItem.Path))
            {
                DirectoryItem dirItem = new DirectoryItem { Name = item.Name, Path = item.Path, Items = new List<DirectoryItem>() };
                dItem.Items.Add(dirItem);
                GetDirectories(dirItem);
            }
        }

        public void FolderSelectionChanged(FolderChangedMessage e)
        {
            SelectedFolder = e.Folder;
            SelectedPlayList = null;
            currentListType = ListType.Library;
            appConfig.SelectedListType = currentListType;

            DatabaseAccess dba = new DatabaseAccess();
            var tempPlayList = dba.GetMediaFilesByPath(_selectedFolder.Path);
            OnPlayListUpdated(tempPlayList);
            OnApplicationUpdated(currentListType, 0);
            appConfig.Playlist = string.Empty;
            appConfig.LibraryFolder = _selectedFolder.Path;
        }


        #endregion


        #region Watch Events

        private void AddFolderWatch(string path)
        {
            try
            {
                folderWatcher.AddFolderWatch(path);
            }
            catch (Exception ex)
            {
                OnAlertMessage("Error", ex.Message);
            }
        }

        private void RemoveFileWatcher(string path)
        {
            try
            {
                folderWatcher.RemoveFileWatcher(path);
            }
            catch (Exception ex)
            {
                OnAlertMessage("Error", ex.Message);
            }
        }

        private void OnFileChanged(object sender, System.IO.FileSystemEventArgs e)
        {
            try
            {
                using (MediaPlayerDBEntities context = new MediaPlayerDBEntities())
                {
                    switch (e.ChangeType)
                    {
                        case System.IO.WatcherChangeTypes.Created:
                            MediaItem newItem = FileScan.FileToMediaItem(e.FullPath);
                            context.MediaItems.Add(newItem);
                            context.SaveChanges();
                            OnFileUpdated(null, null, e.ChangeType, newItem);
                            break;
                        case System.IO.WatcherChangeTypes.Deleted:
                            MediaItem deletedItem = context.MediaItems.FirstOrDefault(x => x.FilePath == e.FullPath);
                            if (deletedItem != null)
                            {
                                context.MediaItems.Remove(deletedItem);
                                context.SaveChanges();
                                OnFileUpdated(e.FullPath, null, e.ChangeType, null);
                            }
                            break;
                        default:
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                OnAlertMessage("Error", ex.Message);
            }
        }

        private void OnFileRenamed(object sender, System.IO.RenamedEventArgs e)
        {
            try
            {
                using (MediaPlayerDBEntities context = new MediaPlayerDBEntities())
                {
                    MediaItem item = context.MediaItems.FirstOrDefault(x => x.FilePath == e.OldFullPath);
                    if (item != null)
                    {
                        item.FilePath = e.FullPath;
                        context.SaveChanges();
                        OnFileUpdated(e.OldName, e.FullPath, System.IO.WatcherChangeTypes.Renamed, null);
                    }
                }
            }
            catch (Exception ex)
            {
                OnAlertMessage("Error", ex.Message);
            }
        }


        #endregion


        #region Messaging


        /// <summary>
        /// Called when a file is updated to update the current list if necessary.
        /// </summary>
        /// <param name="oldName">The old name.</param>
        /// <param name="newName">The new name.</param>
        /// <param name="changeType">Type of the change.</param>
        /// <param name="newMediaItem">The new media item.</param>
        protected void OnFileUpdated(string oldName, string newName, System.IO.WatcherChangeTypes changeType, MediaItem newMediaItem)
        {
            Messenger.Default.Send<FileUpdatedMessage>(new FileUpdatedMessage(oldName, newName, changeType, newMediaItem));
        }

        protected void OnApplicationUpdated(ListType listType, int currentPlaylistId)
        {
            Messenger.Default.Send<ListTypeUpdatedMessage>(new ListTypeUpdatedMessage(listType, currentPlaylistId));
        }

        protected void OnPlayListUpdated(List<MediaItem> list)
        {
            Messenger.Default.Send<PlayListUpdatedMessage>(new PlayListUpdatedMessage(list));
        }

        protected void OnAlertMessage(string title, string message)
        {
            Messenger.Default.Send<AlertMessageMessage>(new AlertMessageMessage(title, message));
        }

        protected void OnNofify(string title, string message)
        {
            Messenger.Default.Send<NotifyMessage>(new NotifyMessage(title, message));
        }

        //public event ApplicationLoadedEventHandler ApplicationLoaded;

        //protected void OnApplicationLoaded(AppConfig appConfig)
        //{
        //    if (ApplicationLoaded != null)
        //    {
        //        int type = -1;
        //        if (!string.IsNullOrEmpty(appConfig.LibraryFolder))
        //        {
        //            type = 0;
        //        }
        //        else if (!string.IsNullOrEmpty(appConfig.Playlist))
        //        {
        //            type = 1;
        //        }
        //    }
        //}

        protected void OnPlayListConfigSet(string column, ListSortDirection direction)
        {
            Messenger.Default.Send<PlayListConfigSetMessage>(new PlayListConfigSetMessage(column, direction));
        }

        private void CloseApplication(object args)
        {
            if (SelectedLibrary != null)
            {
                DatabaseAccess dba = new DatabaseAccess();
                dba.UpdateLibraryFolders(SelectedLibrary.LibraryFolders.ToList());
            }

            Messenger.Default.Send<CloseAppMessage>(new CloseAppMessage());
        }


        #endregion


        #region Dispose

        private bool disposed = false;
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                // Managed cleanup code here, while managed refs still valid
                if (disposing)
                {
                    // Dispose managed resources.
                    //if (play != null)
                    //{
                    //    play.Dispose();
                    //}
                }

                // Unmanaged cleanup code here
                disposed = true;
            }
        }

        ~ApplicationViewModel()
        {
            Dispose(false);
        }

        #endregion
    }
}
