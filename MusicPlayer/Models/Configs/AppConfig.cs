using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace MusicPlayer.Models
{
    [Serializable]
    public class AppConfig
    {
        [XmlElement(DataType = "string", ElementName = "libraryfolder")]
        public string LibraryFolder { get; set; }

        public ListType SelectedListType { get; set; }

        [XmlElement(DataType = "string", ElementName = "playlist")]
        public string Playlist { get; set; }
    }
}
