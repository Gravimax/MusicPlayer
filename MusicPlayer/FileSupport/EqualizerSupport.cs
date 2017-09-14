using MusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.FileSupport
{
    public class EqualizerSupport
    {
        public static void SaveEqualizerSettings(EqualizerModel eqModel)
        {
            if (!Directory.Exists(FileUtilities.GetApplicationPath() + "\\EqSettings\\")) { Directory.CreateDirectory(FileUtilities.GetApplicationPath() + "\\EqSettings\\"); }

            string filePath = FileUtilities.GetApplicationPath() + "\\EqSettings\\" + eqModel.Name + ".eq";
            FileUtilities.ObjectToXMlFile<EqualizerModel>(filePath, eqModel);
        }

        public static List<EqualizerModel> LoadEqSettingsList()
        {
            if (!Directory.Exists(FileUtilities.GetApplicationPath() + "\\EqSettings\\")) { Directory.CreateDirectory(FileUtilities.GetApplicationPath() + "\\EqSettings\\"); }

            string filePath = FileUtilities.GetApplicationPath() + "\\EqSettings\\";
            List<string> dirFiles = Directory.GetFiles(filePath, "*.eq").ToList();

            List<EqualizerModel> playlists = new List<EqualizerModel>();

            foreach (var file in dirFiles)
            {
                playlists.Add(FileUtilities.XMLFileToObject<EqualizerModel>(file));
            }

            return playlists;
        }

        public static void DeleteEqSettings(string name)
        {
            string filePath = FileUtilities.GetApplicationPath() + "\\EqSettings\\" + name + ".eq";
            FileUtilities.DeleteFile(filePath);
        }
    }
}
