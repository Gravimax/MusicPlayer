using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MusicPlayer.Models
{
    [Serializable]
    public class PlayerControlConfig
    {
        [XmlElement(DataType = "string", ElementName = "LastEqSettings")]
        public string LastEqSettings { get; set; }

        [XmlElement(DataType = "string", ElementName = "LastPlayed")]
        public string LastPlayed { get; set; }

        public PlayType PlayType { get; set; }

        [XmlElement(DataType = "double", ElementName = "volume")]
        public double Volume { get; set; }
    }
}
