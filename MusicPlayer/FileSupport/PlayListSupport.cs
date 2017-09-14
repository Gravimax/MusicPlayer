using MusicPlayer.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MusicPlayer.FileSupport
{
    public static class PlayListSupport
    {
        public static void SavePlayList(PlayList playlist)
        {
            if (!Directory.Exists(FileUtilities.GetApplicationPath() + "\\Playlists\\")) { Directory.CreateDirectory(FileUtilities.GetApplicationPath() + "\\Playlists\\"); }

            string filePath = FileUtilities.GetApplicationPath() + "\\Playlists\\" + playlist.Name + ".ply";
            FileUtilities.ObjectToXMlFile<PlayList>(filePath, playlist);
        }

        public static List<PlayList> LoadPlayLists()
        {
            if (!Directory.Exists(FileUtilities.GetApplicationPath() + "\\Playlists\\")) { Directory.CreateDirectory(FileUtilities.GetApplicationPath() + "\\Playlists\\"); }

            string filePath = FileUtilities.GetApplicationPath() + "\\Playlists\\";
            List<string> dirFiles = Directory.GetFiles(filePath, "*.ply").ToList();

            List<PlayList> playlists = new List<PlayList>();

            foreach (var file in dirFiles)
            {
                playlists.Add(FileUtilities.XMLFileToObject<PlayList>(file));
            }

            return playlists;
        }

        public static List<string> GetPlaylistsList()
        {
            if (!Directory.Exists(FileUtilities.GetApplicationPath() + "\\Playlists\\"))
            {
                Directory.CreateDirectory(FileUtilities.GetApplicationPath() + "\\Playlists\\");
                return new List<string>();
            }

            string filePath = FileUtilities.GetApplicationPath() + "\\Playlists\\";
            List<string> dirFiles = Directory.GetFiles(filePath, "*.ply").ToList();

            List<string> playlists = new List<string>();

            foreach (var file in dirFiles)
            {
                playlists.Add(FileUtilities.GetFileNameNoExtention(file));
            }

            return playlists;
        }

        public static PlayList GetPlaylist(string name)
        {
            if (!name.EndsWith(".ply")) { name += ".ply"; }
            string filePath = FileUtilities.GetApplicationPath() + "\\Playlists\\" + name;
            if (FileUtilities.FileExists(filePath))
            {
                return FileUtilities.XMLFileToObject<PlayList>(filePath);
            }
            else
            {
                throw new FileNotFoundException("Not found:" + filePath);
            }
        }

        public static void DeletePlaylist(string name)
        {
            string filePath = FileUtilities.GetApplicationPath() + "\\Playlists\\" + name;
            FileUtilities.DeleteFile(filePath);
        }

        public static void SaveMusicLibarary(MusicLibrary library)
        {
            if (!Directory.Exists(FileUtilities.GetApplicationPath() + "\\Playlists\\")) { Directory.CreateDirectory(FileUtilities.GetApplicationPath() + "\\Playlists\\"); }

            string filePath = FileUtilities.GetApplicationPath() + "\\Playlists\\MusicLibrary.lib";
            FileUtilities.ObjectToXMlFile<MusicLibrary>(filePath, library);
        }

        public static MusicLibrary LoadMusicLibrary()
        {
            if (!Directory.Exists(FileUtilities.GetApplicationPath() + "\\Playlists\\")) { Directory.CreateDirectory(FileUtilities.GetApplicationPath() + "\\Playlists\\"); }

            string filePath = FileUtilities.GetApplicationPath() + "\\Playlists\\MusicLibrary.lib";

            FileInfo fi = new FileInfo(filePath);
            if (fi.Exists)
            {
                MusicLibrary library = FileUtilities.XMLFileToObject<MusicLibrary>(filePath);

                return library;
            }

            return null;
        }

        public static void DeleteMusicLibrary()
        {
            string filePath = FileUtilities.GetApplicationPath() + "\\Playlists\\MusicLibrary.lib";
            FileUtilities.DeleteFile(filePath);
        }


    }
}
