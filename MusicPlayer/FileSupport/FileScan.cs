using MusicPlayer.Database;
using MusicPlayer.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagLib;

namespace MusicPlayer.FileSupport
{
    public static class FileScan
    {
        public static string[] exts = { ".mp3", ".flac", ".m4a", ".wma", ".wav", ".ogg" };
        public static string fileFilters = "All files (*.*)|*.*;MP3 (*.mp3)|*.mp3;FLAC (*.flac)|*.flac;M4A (*.m4a)|*.m4a;OGG (*.ogg)|*.ogg;WMA (*.wma)|*.wma;WAVE (*.wav)|*.wav";

        /// <summary>
        /// Adds the new library.
        /// </summary>
        /// <returns></returns>
        public static Library AddNewLibrary()
        {
            string folder = FileUtilities.SelectFolder();
            
            if (!string.IsNullOrEmpty(folder))
            {
                Library library = new Library();
                library.WatchFolder = false;

                DirectoryInfo dirInfo = new DirectoryInfo(folder);
                library.Name = dirInfo.Name;
                library.Path = folder;

                DatabaseAccess dba = new DatabaseAccess();

                dba.AddNewLibrary(library);

                List<LibraryFolder> folders = GetFolders(dirInfo.FullName);
                foreach (var item in folders)
                {
                    item.LibraryId = library.Id;
                }

                dba.AddLibraryFolders(folders);

                List<MediaItem> music = GetMusicAllDirectories(library.Path);
                dba.AddMMediaItems(music);

                return dba.GetLibrary(); // Gets the lib and the folders
            }

            return null;
        }

        /// <summary>
        /// Refreshes the library.
        /// </summary>
        /// <param name="musicLibrary">The music library.</param>
        /// <param name="mediaItems">The media items.</param>
        /// <returns></returns>
        public static RefreshCounts RefreshLibrary(Library musicLibrary)
        {
            int removedFolders = 0;
            int newFolders = 0;
            int removedCount = 0;
            int newCount = 0;

            using (MediaPlayerDBEntities context = new MediaPlayerDBEntities())
            {
                // Handle the folders first
                System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(musicLibrary.Path);
                var directories = info.GetDirectories().ToList();

                // Check for removed folders
                foreach (var item in musicLibrary.LibraryFolders.ToArray())
                {
                    if (directories.FirstOrDefault(x => x.FullName == item.Path) == null)
                    {
                        context.LibraryFolders.Remove(item);
                        context.SaveChanges();
                        musicLibrary.LibraryFolders.Remove(item);
                        removedFolders++;
                    }
                }

                // Check for new folders
                foreach (var item in directories)
                {
                    if (musicLibrary.LibraryFolders.FirstOrDefault(x => x.Path == item.FullName) == null)
                    {
                        LibraryFolder libFolder = DirInfoToLibraryFolder(item);
                        context.LibraryFolders.Add(libFolder);
                        context.SaveChanges();
                        musicLibrary.LibraryFolders.Add(libFolder);
                        newFolders++;
                    }
                }

                // Then the music
                List<MediaItem> mediaItems = context.MediaItems.Where(x => x.FilePath.Contains(musicLibrary.Path)).ToList();
                List<MediaItem> items = GetMusicAllDirectories(musicLibrary.Path);

                // Check for removed items in current list
                foreach (var item in mediaItems.ToArray())
                {
                    if (items.FirstOrDefault(x => x.FilePath == item.FilePath) == null)
                    {
                        context.MediaItems.Remove(item);
                        context.SaveChanges();
                        mediaItems.Remove(item);
                        removedCount++;
                    }
                }

                // Check for new items in library folders
                foreach (var item in items)
                {
                    if (mediaItems.FirstOrDefault(x => x.FilePath == item.FilePath) == null)
                    {
                        context.MediaItems.Add(item);
                        context.SaveChanges();
                        mediaItems.Add(item);
                        newCount++;
                    }
                }
            }

            return new RefreshCounts(removedCount, newCount, removedFolders, newFolders);
        }

        /// <summary>
        /// Rescans a folder.
        /// </summary>
        /// <param name="musicLibrary">The music library.</param>
        /// <param name="folder">The folder.</param>
        /// <returns></returns>
        public static RefreshCounts RescanFolder(Library musicLibrary, string folder)
        {
            int removedFolders = 0;
            int newFolders = 0;
            int removedCount = 0;
            int newCount = 0;

            using (MediaPlayerDBEntities context = new MediaPlayerDBEntities())
            {
                // Handle the folders first
                System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(folder);
                var directories = info.GetDirectories().ToList();

                // Check for removed folders
                foreach (var item in musicLibrary.LibraryFolders.Where(x => x.Path.Contains(folder)).ToArray())
                {
                    if (directories.FirstOrDefault(x => x.FullName == item.Path) == null)
                    {
                        context.LibraryFolders.Remove(item);
                        context.SaveChanges();
                        musicLibrary.LibraryFolders.Remove(item);
                        removedFolders++;
                    }
                }

                // Check for new folders
                foreach (var item in directories)
                {
                    if (musicLibrary.LibraryFolders.FirstOrDefault(x => x.Path == item.FullName) == null)
                    {
                        LibraryFolder libFolder = DirInfoToLibraryFolder(item);
                        context.LibraryFolders.Add(libFolder);
                        context.SaveChanges();
                        musicLibrary.LibraryFolders.Add(libFolder);
                        newFolders++;
                    }
                }

                // Then the music
                List<MediaItem> mediaItems = context.MediaItems.Where(x => x.FilePath.Contains(folder)).ToList();
                List<MediaItem> items = GetMusicAllDirectories(folder);

                // Check for removed items in current list
                foreach (var item in mediaItems.ToArray())
                {
                    if (items.FirstOrDefault(x => x.FilePath == item.FilePath) == null)
                    {
                        context.MediaItems.Remove(item);
                        context.SaveChanges();
                        mediaItems.Remove(item);
                        removedCount++;
                    }
                }

                // Check for new items in library folders
                foreach (var item in items)
                {
                    if (mediaItems.FirstOrDefault(x => x.FilePath == item.FilePath) == null)
                    {
                        context.MediaItems.Add(item);
                        context.SaveChanges();
                        mediaItems.Add(item);
                        newCount++;
                    }
                }
            }

            return new RefreshCounts(removedCount, newCount, removedFolders, newFolders);
        }

        /// <summary>
        /// Gets the music for directory.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static List<MediaItem> GetMusicForDirectory(string path)
        {
            List<MediaItem> temp = new List<MediaItem>();

            List<string> files = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly).Where(file => exts.Any(x => file.EndsWith(x, System.StringComparison.OrdinalIgnoreCase))).ToList();

            foreach (var file in files)
            {
                temp.Add(FileToMediaItem(file));
            }

            return temp;
        }

        /// <summary>
        /// Gets the music all directories.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static List<MediaItem> GetMusicAllDirectories(string path)
        {
            List<MediaItem> playList = new List<MediaItem>();

            List<string> files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Where(file => exts.Any(x => file.EndsWith(x, System.StringComparison.OrdinalIgnoreCase))).ToList();

            foreach (var file in files)
            {
                playList.Add(FileToMediaItem(file));
            }

            return playList;
        }

        /// <summary>
        /// Gets the folders.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static List<LibraryFolder> GetFolders(string path)
        {
            var dirInfo = new DirectoryInfo(path);
            List<LibraryFolder> temp = new List<LibraryFolder>();
            temp.Add(new LibraryFolder { Name = dirInfo.Name, Path = dirInfo.FullName });
            temp.AddRange((from dir in dirInfo.GetDirectories("*", SearchOption.AllDirectories)
                    select new LibraryFolder { Name = dir.Name, Path = dir.FullName, ParentFolder = dir.Parent.FullName }).ToList());

            return temp;
        }

        //public static PlayList ScanFolder(string name)
        //{
        //    PlayList playList = new PlayList();
        //    playList.Folders = new List<string>();
        //    playList.MusicFiles = new List<MusicFile>();
        //    playList.Name = name;

        //    string folder = FileUtilities.SelectFolder();

        //    if (!string.IsNullOrEmpty(folder) && Directory.Exists(folder))
        //    {
        //        playList.RootFolder = folder;

        //        playList.Folders = Directory.GetDirectories(folder, "*.*", SearchOption.AllDirectories).ToList();

        //        List<string> files = Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories).Where(file => exts.Any(x => file.EndsWith(x, System.StringComparison.OrdinalIgnoreCase))).ToList();

        //        foreach (var file in files)
        //        {
        //            playList.MusicFiles.Add(FileToMusicFile(file));
        //        }

        //        playList.MusicFiles = playList.MusicFiles.OrderBy(x => x.Artist).ThenBy(y => y.Album).ThenBy(z => z.Track).ToList();

        //        return playList;
        //    }
        //    else
        //    {
        //        return playList;
        //    }
        //}

        /// <summary>
        /// Adds a folder from the library to a playlist. Including sub folders.
        /// </summary>
        /// <param name="folderPath">The folder path.</param>
        /// <param name="playListId">The play list identifier.</param>
        /// <returns></returns>
        public static RefreshCounts AddLibraryFolderToPlaylist(string folderPath, int playListId)
        {
            int addedCounts = 0;

            using (MediaPlayerDBEntities context = new MediaPlayerDBEntities())
            {
                List<MediaItem> items = context.MediaItems.Where(x => x.FilePath.Contains(folderPath)).ToList();
                Playlist playlist = context.Playlists.SingleOrDefault(x => x.Id == playListId);

                if (playlist != null && items.Count > 0)
                {
                    foreach (var item in items)
                    {
                        if (playlist.MediaItems.FirstOrDefault(x => x.FilePath == item.FilePath) == null)
                        {
                            addedCounts++;
                            playlist.MediaItems.Add(item);
                        }
                    }

                    context.SaveChanges();
                }
            }

            return new RefreshCounts(0, addedCounts, 0, 0);
        }

        /// <summary>
        /// Adds selected music files to MediaItems and Playlist. Not including sub folders.
        /// </summary>
        /// <param name="folderPath">The folder path.</param>
        /// <param name="playListId">The play list identifier.</param>
        /// <returns></returns>
        public static RefreshCounts AddFilesToPlaylist(string folderPath, int playListId)
        {
            List<MediaItem> temp = new List<MediaItem>();

            List<string> files = FileUtilities.SelectFiles("", fileFilters);

            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    temp.Add(FileToMediaItem(file));
                }
            }
            else
            {
                return new RefreshCounts(0,0, 0, 0);
            }

            using (MediaPlayerDBEntities context = new MediaPlayerDBEntities())
            {
                context.MediaItems.AddRange(temp);
                context.SaveChanges();

                Playlist playlist = context.Playlists.SingleOrDefault(x => x.Id == playListId);

                if (playlist != null)
                {
                    foreach (var item in temp)
                    {
                        playlist.MediaItems.Add(item);
                    }

                    context.SaveChanges();
                }
            }

            return new RefreshCounts(0, temp.Count, 0, 0);
        }

        public static LibraryFolder DirInfoToLibraryFolder(DirectoryInfo info)
        {
            return new LibraryFolder { Name = info.Name, ParentFolder = info.Parent.FullName, Path = info.FullName };
        }

        /// <summary>
        /// Converts a music file to a MediaItem
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public static MediaItem FileToMediaItem(string file)
        {
            var tagFile = TagLib.File.Create(file, ReadStyle.Average);

            return new MediaItem
            {
                BitRate = tagFile.Properties.AudioBitrate,
                Duration = tagFile.Properties.Duration.Ticks,
                FilePath = file,
                Folder = FileUtilities.GetFilePath(file),
                Title = tagFile.Tag.Title ?? string.Empty,
                Artist = tagFile.Tag.FirstArtist ?? tagFile.Tag.FirstAlbumArtist ?? string.Empty,
                Track = (int)tagFile.Tag.Track,
                Album = tagFile.Tag.Album ?? string.Empty,
                Comment = tagFile.Tag.Comment,
                Genre = tagFile.Tag.FirstGenre,
                Year = (int)tagFile.Tag.Year,
                Type = FileUtilities.FileToType(file)
            };
        }
    }
}
