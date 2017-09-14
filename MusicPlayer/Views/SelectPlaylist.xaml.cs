using MusicPlayer.Database;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace MusicPlayer.Views
{
    /// <summary>
    /// Interaction logic for SelectPlaylist.xaml
    /// </summary>
    public partial class SelectPlaylist : Window, INotifyPropertyChanged
    {
        private List<Playlist> _playList = new List<Playlist>();

        public List<Playlist> Playlist
        {
            get { return _playList; }
            set
            {
                _playList = value;
                OnPropertyChanged("Playlist");
            }
        }

        private Playlist _selectedList;

        public Playlist SelectedList
        {
            get { return _selectedList; }
            set
            {
                _selectedList = value;
                OnPropertyChanged("SelectedList");
            }
        }

        public DelegateCommand DoubleClickCommand { get; private set; }

        public SelectPlaylist()
        {
            InitializeComponent();
            DoubleClickCommand = new DelegateCommand(DoubleClick);
            this.DataContext = this;
            DatabaseAccess dba = new DatabaseAccess();
            Playlist = dba.GetPlaylists();
        }

        private void DoubleClick(object args)
        {
            this.DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        }
    }
}
