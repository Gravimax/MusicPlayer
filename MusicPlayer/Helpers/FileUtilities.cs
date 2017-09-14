using MusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace MusicPlayer
{
    public static class FileUtilities
    {
        public static bool FileExists(string fileName)
        {
            return File.Exists(fileName);
        }

        /// <summary>
        /// Selects a file.
        /// </summary>
        /// <param name="Title">The title.</param>
        /// <param name="Filter">The filter.</param>
        /// <param name="FilterIndex">Desired initial index of the filter.</param>
        /// <param name="InitialDir">The initial directory.</param>
        /// <returns>Path to the selected file or null.</returns>
        public static string SelectFile(string Title = "Select a file", string Filter = "All files (*.*)|*.*", int FilterIndex = 1, string InitialDir = "C:\\")
        {
            Microsoft.Win32.OpenFileDialog fldg = new Microsoft.Win32.OpenFileDialog();
            fldg.Title = Title;
            fldg.Filter = Filter;
            fldg.FilterIndex = FilterIndex;
            fldg.InitialDirectory = InitialDir;
            fldg.RestoreDirectory = true;

            if (fldg.ShowDialog() == true)
            {
                return fldg.FileName;
            }

            return null;
        }

        /// <summary>
        /// Selects a file.
        /// </summary>
        /// <param name="Title">The title.</param>
        /// <param name="Filter">The filter.</param>
        /// <param name="FilterIndex">Desired initial index of the filter.</param>
        /// <param name="InitialDir">The initial directory.</param>
        /// <returns>Path to the selected file or null.</returns>
        public static List<string> SelectFiles(string Title = "Select a file", string Filter = "All files (*.*)|*.*", int FilterIndex = 1, string InitialDir = "C:\\")
        {
            Microsoft.Win32.OpenFileDialog fldg = new Microsoft.Win32.OpenFileDialog();
            fldg.Title = Title;
            fldg.Filter = Filter;
            fldg.FilterIndex = FilterIndex;
            fldg.InitialDirectory = InitialDir;
            fldg.RestoreDirectory = true;

            if (fldg.ShowDialog() == true)
            {
                return fldg.FileNames.ToList();
            }

            return null;
        }

        /// <summary>
        /// Selects a folder.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="rootFolder">The root folder.</param>
        /// <returns>Folder path or null.</returns>
        public static string SelectFolder(string description = "Select a folder", System.Environment.SpecialFolder rootFolder = Environment.SpecialFolder.MyComputer)
        {
            System.Windows.Forms.FolderBrowserDialog fldg = new System.Windows.Forms.FolderBrowserDialog();
            fldg.Description = description;
            fldg.RootFolder = rootFolder;

            DialogResult result = fldg.ShowDialog();
            if (result == DialogResult.OK)
            {
                return fldg.SelectedPath;
            }

            return null;
        }

        public static string GetApplicationPath()
        {
            return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>String</returns>
        public static string GetFileName(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                return Path.GetFileName(filePath);
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets the file extention.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>String</returns>
        public static string GetFileExtention(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                return Path.GetExtension(fileName);
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets the file name without the extention.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>String</returns>
        public static string GetFileNameNoExtention(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                return Path.GetFileNameWithoutExtension(fileName);
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets the file path.
        /// </summary>
        /// <param name="fileName">Full path of the file.</param>
        /// <returns></returns>
        public static string GetFilePath(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                return Path.GetDirectoryName(fileName);
            }
            return string.Empty;
        }

        public static void DeleteFile(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            if (fi.Exists)
            {
                fi.Delete();
            }
        }

        public static string FileToType(string fileName)
        {
            switch (GetFileExtention(fileName))
            {
                case ".mp3":
                    return "mp3";
                case ".ogg":
                    return "Ogg-Vorbis";
                case ".flac":
                    return "FLAC";
                case ".m4a":
                    return "AAC";
                case ".wma":
                    return "Windows Media Audio";
                case ".wav":
                    return "WAVE";
                default:
                    return "Unsupported";
            }
        }

        /// <summary>
        /// Reads an xml file and converts it to its correct object type.
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>
        /// Object of type T
        /// </returns>
        public static T XMLFileToObject<T>(string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    XmlSerializer xSerializer = new XmlSerializer(typeof(T));
                    using (FileStream fs = new FileStream(fileName, FileMode.Open))
                    {
                        XmlReader xReader = new XmlTextReader(fs);
                        return (T)xSerializer.Deserialize(xReader);
                    }
                }
                else
                {
                    throw new FileNotFoundException(fileName);
                }
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        /// Takes a serializable object and saves it as an xml file.
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <param name="path">The path including the file name.</param>
        /// <param name="obj">The object to serialize.</param>
        public static void ObjectToXMlFile<T>(string path, T obj, bool checkPath = false)
        {
            if (checkPath)
            {
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
            }

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextWriter textWriter = new StreamWriter(path))
            {
                serializer.Serialize(textWriter, obj);
                textWriter.Close();
            }
        }
    }
}
