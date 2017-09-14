using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MusicPlayer.Models
{
    [Serializable]
    public class FolderItem : Notifier, IFolderItem
    {
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
                OnPropertyChanged("Name");
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
                OnPropertyChanged("Path");
            }
        }
    }
}
