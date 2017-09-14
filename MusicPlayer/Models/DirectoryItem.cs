using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MusicPlayer.Models
{
    [Serializable]
    public class DirectoryItem : IDirectoryItem
    {
        public List<DirectoryItem> Items { get; set; }

        public DirectoryItem()
        {
            Items = new List<DirectoryItem>();
        }

        [XmlElement(DataType = "boolean", ElementName = "isExpanded")]
        public bool IsExpanded { get; set; }

        string _name;
        [XmlElement(DataType = "string", ElementName = "name")]
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        string _path;
        [XmlElement(DataType = "string", ElementName = "path")]
        public string Path
        {
            get
            {
                return _path;
            }

            set
            {
                _path = value;
            }
        }
    }
}
