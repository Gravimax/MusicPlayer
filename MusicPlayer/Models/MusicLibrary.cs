using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MusicPlayer.Models
{
    [Serializable]
    [XmlType(TypeName = "musiclibrary")]
    public class MusicLibrary : Notifier, IMusicLibrary
    {
        [XmlElement(DataType = "string", ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(DataType = "string", ElementName = "path")]
        public string Path { get; set; }

        [XmlElement(DataType = "boolean", ElementName = "watchFolder")]
        public bool WatchFolder { get; set; }

        List<DirectoryItem> _folders;
        public List<DirectoryItem> Folders
        {
            get
            {
                return _folders;
            }

            set
            {
                _folders = value;
                OnPropertyChanged("Folders");
            }
        }

        public List<MusicFile> MusicFiles { get; set; }
    }
}
