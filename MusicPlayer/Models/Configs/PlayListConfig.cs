using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace MusicPlayer.Models
{
    [Serializable]
    public class PlayListConfig
    {
        [XmlElement(DataType = "string", ElementName = "sortcolumn")]
        public string SortColumn { get; set; }

        public ListSortDirection SortDirection { get; set; }
    }
}
