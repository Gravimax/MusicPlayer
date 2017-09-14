using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Models
{
    public class MusicTag
    {
        public string Album { get; set; }

        private string[] _albumArtists;
        public string[] AlbumArtists
        {
            get
            {
                return _albumArtists;
            }
            set
            {
                _albumArtists = value;
            }
        }

        public string AlbumArtistList
        {
            get
            {
                if (AlbumArtists != null)
                {
                    return string.Join(",", AlbumArtists);
                }

                return null;
            }
            set
            {
                if (value != null)
                {
                    AlbumArtists = value.Split(',');
                }
                else
                {
                    AlbumArtists = null;
                }
            }
        }
        public string Comment { get; set; }

        private string[] _composers;
        public string[] Composers
        {
            get
            {
                return _composers;
            }
            set
            {
                _composers = value;
            }
        }

        public string ComposerList
        {
            get
            {
                if (Composers != null)
                {
                    return string.Join(",", Composers);
                }

                return null;
            }
            set
            {
                if (value != null)
                {
                    Composers = value.Split(',');
                }
                else
                {
                    Composers = null;
                }
            }
        }

        public string Conductor { get; set; }

        public string Copyright { get; set; }

        public uint Disc { get; set; }

        public uint DiscCount { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }

        private string[] _genres;
        public string[] Genres
        {
            get
            {
                return _genres;
            }
            set
            {
                _genres = value;
            }
        }

        public string GenreList
        {
            get
            {
                if (Genres != null)
                {
                    return string.Join(",", Genres);
                }

                return null;
            }
            set
            {
                if (value != null)
                {
                    Genres = value.Split(',');
                }
                else
                {
                    Genres = null;
                }
            }
        }

        public string Lyrics { get; set; }

        private string[] _performers { get; set; }
        public string[] Performers
        {
            get
            {
                return _performers;
            }
            set
            {
                _performers = value;
            }
        }

        public string PerformerList
        {
            get
            {
                if (Performers != null)
                {
                    return string.Join(",", Performers);
                }

                return null;
            }
            set
            {
                if (value != null)
                {
                    Performers = value.Split(',');
                }
                else
                {
                    Performers = null;
                }
            }
        }

        public string Title { get; set; }

        public uint Track { get; set; }

        public uint TrackCount { get; set; }

        public uint Year { get; set; }
    }
}
