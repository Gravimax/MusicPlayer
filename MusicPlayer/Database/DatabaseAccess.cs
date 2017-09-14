using MusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicPlayer.Database
{
    public class DatabaseAccess
    {
        #region Libraries


        public void AddNewLibrary(Library library)
        {
            using (MediaPlayerDBEntities context = new MediaPlayerDBEntities())
            {
                context.Libraries.Add(library);
                context.SaveChanges();
            }
        }

        public Library GetLibrary()
        {
            using (MediaPlayerDBEntities context = new MediaPlayerDBEntities())
            {
                // We are only supporting one library at a time at the moment.
                return context.Libraries.Include("LibraryFolders").FirstOrDefault();
            }
        }

        public void DeleteLibrary(int id)
        {
            using (MediaPlayerDBEntities context = new MediaPlayerDBEntities())
            {
                Library library = context.Libraries.SingleOrDefault(x => x.Id == id);
                if (library != null)
                {
                    context.Libraries.Remove(library);
                    context.SaveChanges();
                }
            }
        }

        public void AddLibraryFolders(List<LibraryFolder> folders)
        {
            using (MediaPlayerDBEntities context = new MediaPlayerDBEntities())
            {
                context.LibraryFolders.AddRange(folders);
                context.SaveChanges();
            }
        }

        public void UpdateLibraryFolders(List<LibraryFolder> folders)
        {
            using (MediaPlayerDBEntities context = new MediaPlayerDBEntities())
            {
                foreach (var item in folders)
                {
                    LibraryFolder folder = context.LibraryFolders.Single(x => x.Id == item.Id);
                    folder.IsExpanded = item.IsExpanded;
                    context.SaveChanges();
                }
            }
        }


        #endregion


        #region Media Items


        public void AddMMediaItems(List<MediaItem> mediaItems)
        {
            try
            {
                using (MediaPlayerDBEntities context = new MediaPlayerDBEntities())
                {
                    context.MediaItems.AddRange(mediaItems);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        public List<MediaItem> GetMediaFilesByPath(string path)
        {
            using (MediaPlayerDBEntities context = new MediaPlayerDBEntities())
            {
                return context.MediaItems.Where(x => x.FilePath.Contains(path)).ToList();
            }
        }

        public List<MediaItem> GetMediaFilesByPlaylist(int playlistId)
        {
            using (MediaPlayerDBEntities context = new MediaPlayerDBEntities())
            {
                Playlist playlist = context.Playlists.Include("MediaItems").SingleOrDefault(x => x.Id == playlistId);
                return playlist.MediaItems.ToList();
            }
        }

        public void DeleteMediaItem(int mediaItemId)
        {
            using (MediaPlayerDBEntities context = new MediaPlayerDBEntities())
            {
                MediaItem item = context.MediaItems.SingleOrDefault(x => x.Id == mediaItemId);
                context.MediaItems.Remove(item);
                context.SaveChanges();
            }
        }

        public void DeleteMediaItems(List<MediaItem> mediaItems)
        {
            using (MediaPlayerDBEntities context = new MediaPlayerDBEntities())
            {
                foreach (var item in mediaItems)
                {
                    MediaItem mediaItem = context.MediaItems.SingleOrDefault(x => x.Id == item.Id);
                    if (mediaItem != null)
                    {
                        context.MediaItems.Remove(mediaItem);
                    }
                }

                context.SaveChanges();
            }
        }

        public void DeleteMediaItemsByPath(string path)
        {
            using (MediaPlayerDBEntities context = new MediaPlayerDBEntities())
            {
                List<MediaItem> items = context.MediaItems.Where(x => x.FilePath.Contains(path)).ToList();
                foreach (var item in items)
                {
                    context.MediaItems.Remove(item);
                }

                context.SaveChanges();
            }
        }


        #endregion


        #region Playlists


        public void AddNewPlaylist(Playlist playList)
        {
            using (MediaPlayerDBEntities context = new MediaPlayerDBEntities())
            {
                context.Playlists.Add(new Playlist { Name = playList.Name });
                context.SaveChanges();
            }
        }

        public Playlist GetPlaylistByName(string name)
        {
            using (MediaPlayerDBEntities context = new MediaPlayerDBEntities())
            {
                return context.Playlists.FirstOrDefault(x => x.Name == name);
            }
        }

        public Playlist GetPlaylistById(int id)
        {
            using (MediaPlayerDBEntities context = new MediaPlayerDBEntities())
            {
                return context.Playlists.SingleOrDefault(x => x.Id == id);
            }
        }

        public List<Playlist> GetPlaylists()
        {
            using (MediaPlayerDBEntities context = new MediaPlayerDBEntities())
            {
                return context.Playlists.ToList();
            }
        }

        public void UpdatePlaylist(Playlist playList)
        {
            using (MediaPlayerDBEntities context = new MediaPlayerDBEntities())
            {
                Playlist item = context.Playlists.SingleOrDefault(x => x.Id == playList.Id);
                if (item != null)
                {
                    item.Name = playList.Name;
                    context.SaveChanges();
                }
            }
        }

        public void DeletePlaylist(int id)
        {
            using (MediaPlayerDBEntities context = new MediaPlayerDBEntities())
            {
                Playlist item = context.Playlists.SingleOrDefault(x => x.Id == id);
                if (item != null)
                {
                    context.Playlists.Remove(item);
                    context.SaveChanges();
                }
            }
        }

        public void AddMediaItemsToPlaylist(int playlistId, List<MediaItem> musicFiles)
        {
            using (MediaPlayerDBEntities context = new MediaPlayerDBEntities())
            {
                Playlist playlist = context.Playlists.SingleOrDefault(x => x.Id == playlistId);
                if (playlist != null)
                {
                    foreach (var item in musicFiles)
                    {
                        if (playlist.MediaItems.SingleOrDefault(x => x.Id == item.Id) == null)
                        {
                            playlist.MediaItems.Add(item);
                        }
                    }

                    context.SaveChanges();
                }
            }
        }

        public void RemoveMediaItemsFromPlaylist(int playlistId, List<MediaItem> mediaItems)
        {
            using (MediaPlayerDBEntities context = new MediaPlayerDBEntities())
            {
                Playlist playlist = context.Playlists.SingleOrDefault(x => x.Id == playlistId);
                if (playlist != null)
                {
                    foreach (var item in mediaItems)
                    {
                        if (playlist.MediaItems.SingleOrDefault(x => x.Id == item.Id) == null)
                        {
                            playlist.MediaItems.Remove(item);
                        }
                    }

                    context.SaveChanges();
                }
            }
        }

        public void RemoveMediaItemFromPlaylist(int playlistId, MediaItem mediaItem)
        {
            using (MediaPlayerDBEntities context = new MediaPlayerDBEntities())
            {
                Playlist playlist = context.Playlists.SingleOrDefault(x => x.Id == playlistId);
                if (playlist != null)
                {
                    playlist.MediaItems.Remove(mediaItem);

                    context.SaveChanges();
                }
            }
        }


        #endregion


        #region Generes


        public List<string> GetGenres()
        {
            using (MediaPlayerDBEntities context = new MediaPlayerDBEntities())
            {
                return context.Genres.Select(x => x.Name).ToList();
            }
        }

        public void UpdateGenre(Genre genre)
        {
            using (MediaPlayerDBEntities context = new MediaPlayerDBEntities())
            {
                Genre gen = context.Genres.SingleOrDefault(x => x.Id == genre.Id);
                if (gen != null)
                {
                    gen.Name = genre.Name;
                    context.SaveChanges();
                }
            }
        }

        public List<Genre> SaveNewGenres(List<string> genres)
        {
            List<Genre> temp = new List<Genre>();

            if (genres != null && genres.Count > 0)
            {
                using (MediaPlayerDBEntities context = new MediaPlayerDBEntities())
                {
                    foreach (var item in genres)
                    {
                        if (context.Genres.FirstOrDefault(x => x.Name == item) == null)
                        {
                            temp.Add(new Genre { Name = item });
                        }
                    }

                    if (temp.Count > 0)
                    {
                        context.Genres.AddRange(temp);
                        context.SaveChanges();
                    }

                    return temp;
                }
            }

            return temp;
        }


        #endregion
    }
}
