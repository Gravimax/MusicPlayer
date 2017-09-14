using MusicPlayer.Models;
using System.Collections.Generic;
using System.IO;

namespace MusicPlayer.FileSupport
{
    public static class AppConfiguration
    {
        private static string appPath = FileUtilities.GetApplicationPath() + "\\Config\\AppConfig.xml";
        private static string mainPath = FileUtilities.GetApplicationPath() + "\\Config\\MainConfig.xml";
        private static string controlPath = FileUtilities.GetApplicationPath() + "\\Config\\PlayerControlConfig.xml";
        private static string listPath = FileUtilities.GetApplicationPath() + "\\Config\\PlayListConfig.xml";

        public static AppConfig LoadAppConfig()
        {
            AppConfig appConfig = null;

            if (new FileInfo(appPath).Exists)
            {
                return FileUtilities.XMLFileToObject<AppConfig>(appPath);
            }
            else
            {
                appConfig = new AppConfig
                {
                    LibraryFolder = "",
                    Playlist = "",
                    SelectedListType = ListType.Library
                };

                SaveAppConfig(appConfig);
                return appConfig;
            }
        }

        public static void SaveAppConfig(AppConfig config)
        {
            FileUtilities.ObjectToXMlFile<AppConfig>(appPath, config, true);
        }

        public static MainConfig LoadMainConfig()
        {
            MainConfig mainConfig = null;

            if (new FileInfo(mainPath).Exists)
            {
                return FileUtilities.XMLFileToObject<MainConfig>(mainPath);
            }
            else
            {
                mainConfig = new MainConfig
                {
                    Height = 768,
                    Left = 0,
                    Top = 0,
                    Width = 1200
                };

                SaveMainConfig(mainConfig);
                return mainConfig;
            }
        }

        public static void SaveMainConfig(MainConfig config)
        {
            FileUtilities.ObjectToXMlFile<MainConfig>(mainPath, config, true);
        }

        public static PlayerControlConfig LoadPlayerControlConfig()
        {
            PlayerControlConfig config = null;

            if (new FileInfo(controlPath).Exists)
            {
                return FileUtilities.XMLFileToObject<PlayerControlConfig>(controlPath);
            }
            else
            {
                config = new PlayerControlConfig
                {
                    LastEqSettings = "Default",
                    LastPlayed = "",
                    PlayType = PlayType.Normal,
                    Volume = 0.75
                };

                SavePlayerControlConfig(config);
                return config;
            }
        }

        public static void SavePlayerControlConfig(PlayerControlConfig config)
        {
            FileUtilities.ObjectToXMlFile<PlayerControlConfig>(controlPath, config, true);
        }

        public static PlayListConfig LoadPlayListConfig()
        {
            PlayListConfig config = null;

            if (new FileInfo(listPath).Exists)
            {
                return FileUtilities.XMLFileToObject<PlayListConfig>(listPath);
            }
            else
            {
                config = new PlayListConfig
                {
                    SortColumn = "Artist",
                    SortDirection = System.ComponentModel.ListSortDirection.Ascending
                };

                SavePlayListConfig(config);
                return config;
            }
        }

        public static void SavePlayListConfig(PlayListConfig config)
        {
            FileUtilities.ObjectToXMlFile<PlayListConfig>(listPath, config, true);
        }

        public static List<string> GetGenreList()
        {
            List<string> genres = null;
            string filePath = FileUtilities.GetApplicationPath() + "\\Data\\Genres.xml";
            FileInfo fi = new FileInfo(filePath);

            if (fi.Exists)
            {
                genres = FileUtilities.XMLFileToObject<List<string>>(filePath);
            }
            
            return genres != null ? genres : new List<string>();
        }
    }
}
