using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MusicPlayer.Models
{
    [Serializable]
    [XmlType(TypeName = "playList")]
    public class PlayList : IPlayList
    {
        [XmlElement(DataType = "string", ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(DataType = "string", ElementName = "folder")]
        public string RootFolder { get; set; }

        public List<string> Folders { get; set; }

        public List<MusicFile> MusicFiles { get; set; }

        [XmlIgnore]
        public List<string> FolderList
        {
            get
            {
                List<string> temp = new List<string>();
                temp.Add(RootFolder);
                temp.AddRange(Folders);
                return temp;
            }
        }
    }
}
