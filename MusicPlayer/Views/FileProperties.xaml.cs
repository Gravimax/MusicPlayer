using MusicPlayer.Database;
using MusicPlayer.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using TagLib;

namespace MusicPlayer.Views
{
    /// <summary>
    /// Interaction logic for FileProperties.xaml
    /// </summary>
    public partial class FileProperties : Window, INotifyPropertyChanged
    {
        string _filePath;

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
        string _fileName;

        public string FileName
        {
            get
            {
                return _fileName;
            }

            set
            {
                _fileName = value;
                OnPropertyChanged("FileName");
            }
        }

        private TagLib.File file;

        private MusicTag _musicTag;
        public MusicTag SelectedFile
        {
            get
            {
                return _musicTag;
            }

            set
            {
                _musicTag = value;
                OnPropertyChanged("SelectedFile");
            }
        }

        private List<string> _generes;
        public List<string> Genres
        {
            get { return _generes; }
            set
            {
                _generes = value;
                OnPropertyChanged("Genres");
            }
        }

        public FileProperties(string filePath)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(filePath))
            {
                file = TagLib.File.Create(filePath, ReadStyle.Average);

                SelectedFile = new MusicTag();

                SelectedFile.Album = file.Tag.Album;
                SelectedFile.AlbumArtists = file.Tag.AlbumArtists != null ? file.Tag.AlbumArtists : file.Tag.Artists;
                SelectedFile.Comment = file.Tag.Comment;
                SelectedFile.Composers = file.Tag.Composers;
                SelectedFile.Conductor = file.Tag.Conductor;
                SelectedFile.Copyright = file.Tag.Copyright;
                SelectedFile.Disc = file.Tag.Disc;
                SelectedFile.DiscCount = file.Tag.DiscCount;
                SelectedFile.FileName = FileUtilities.GetFileName(filePath);
                SelectedFile.FilePath = filePath;
                SelectedFile.Genres = file.Tag.Genres;
                SelectedFile.Lyrics = file.Tag.Lyrics;
                SelectedFile.Performers = file.Tag.Performers;
                SelectedFile.Title = file.Tag.Title;
                SelectedFile.Track = file.Tag.Track;
                SelectedFile.TrackCount = file.Tag.TrackCount;
                SelectedFile.Year = file.Tag.Year;
            }

            this.DataContext = this;

            DatabaseAccess dba = new DatabaseAccess();
            Genres = dba.GetGenres();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedFile != null)
            {
                file.Tag.Album = SelectedFile.Album;
                file.Tag.AlbumArtists = SelectedFile.AlbumArtists;
                file.Tag.Comment = SelectedFile.Comment;
                file.Tag.Composers = SelectedFile.Composers;
                file.Tag.Conductor = SelectedFile.Conductor;
                file.Tag.Copyright = SelectedFile.Copyright;
                file.Tag.Disc = SelectedFile.Disc;
                file.Tag.DiscCount = SelectedFile.DiscCount;
                file.Tag.Genres = SelectedFile.Genres;
                file.Tag.Lyrics = SelectedFile.Lyrics;
                file.Tag.Performers = SelectedFile.Performers;
                file.Tag.Title = SelectedFile.Title;
                file.Tag.Track = SelectedFile.Track;
                file.Tag.TrackCount = SelectedFile.TrackCount;
                file.Tag.Year = SelectedFile.Year;

                file.Save();
            }

            this.DialogResult = true;
            this.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
