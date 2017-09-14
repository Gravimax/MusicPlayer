using System;
using System.Xml.Serialization;

namespace MusicPlayer.Models
{
    [Serializable]
    public class MusicFile : Notifier, IMusicFile
    {
        public int Id { get; set; }

        string _album;
        [XmlElement(DataType = "string", ElementName = "album")]
        public string Album
        {
            get
            {
                return _album;
            }

            set
            {
                _album = value;
                OnPropertyChanged("Album");
            }
        }

        string _artist;
        [XmlElement(DataType = "string", ElementName = "artist")]
        public string Artist
        {
            get
            {
                return _artist;
            }

            set
            {
                _artist = value;
                OnPropertyChanged("Artist");
            }
        }

        int _bitRate;
        [XmlElement(DataType = "int", ElementName = "bitrate")]
        public int BitRate
        {
            get
            {
                return _bitRate;
            }

            set
            {
                _bitRate = value;
                OnPropertyChanged("BitRate");
            }
        }

        string _comment;
        [XmlElement(DataType = "string", ElementName = "comment")]
        public string Comment
        {
            get
            {
                return _comment;
            }

            set
            {
                _comment = value;
                OnPropertyChanged("Comment");
            }
        }

        long _duration;
        [XmlElement(DataType = "long", ElementName = "duration")]
        public long Duration
        {
            get
            {
                return _duration;
            }

            set
            {
                _duration = value;
                OnPropertyChanged("Duration");
            }
        }

        string _filePath;
        [XmlElement(DataType = "string", ElementName = "filePath")]
        public string FilePath
        {
            get
            {
                return _filePath;
            }

            set
            {
                _filePath = value;
                OnPropertyChanged("FilePath");
            }
        }

        [XmlElement(DataType = "string", ElementName = "folder")]
        public string Folder { get; set; }

        string _genre;
        [XmlElement(DataType = "string", ElementName = "genre")]
        public string Genre
        {
            get
            {
                return _genre;
            }

            set
            {
                _genre = value;
                OnPropertyChanged("Genre");
            }
        }

        string _title;
        [XmlElement(DataType = "string", ElementName = "title")]
        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        uint _track;
        [XmlElement(DataType = "unsignedInt", ElementName = "track")]
        public UInt32 Track
        {
            get
            {
                return _track;
            }

            set
            {
                _track = value;
                OnPropertyChanged("Track");
            }
        }

        string _type;
        [XmlElement(DataType = "string", ElementName = "type")]
        public string Type
        {
            get
            {
                return _type;
            }

            set
            {
                _type = value;
                OnPropertyChanged("Type");
            }
        }

        uint _year;
        [XmlElement(DataType = "unsignedInt", ElementName = "year")]
        public UInt32 Year
        {
            get
            {
                return _year;
            }

            set
            {
                _year = value;
                OnPropertyChanged("Year");
            }
        }
    }
}
