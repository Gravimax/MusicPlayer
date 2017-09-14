using System.Xml.Serialization;
using System;

namespace MusicPlayer.Models
{
    [Serializable]
    public class MainConfig
    {
        [XmlElement(DataType = "double", ElementName = "Height")]
        public double Height { get; set; }

        [XmlElement(DataType = "double", ElementName = "Left")]
        public double Left { get; set; }

        [XmlElement(DataType = "double", ElementName = "Top")]
        public double Top { get; set; }

        [XmlElement(DataType = "double", ElementName = "Width")]
        public double Width { get; set; }
    }
}
