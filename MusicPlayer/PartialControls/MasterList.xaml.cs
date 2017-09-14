using MusicPlayer.Models;
using System.Windows;
using System.Windows.Controls;

namespace MusicPlayer.PartialControls
{
    /// <summary>
    /// Interaction logic for MasterList.xaml
    /// </summary>
    public partial class MasterList : UserControl
    {
        public MasterList()
        {
            InitializeComponent();
        }

        private void tvFolderView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<FolderChangedMessage>(new FolderChangedMessage((DirectoryItem)e.NewValue));
        }

        public void SetListType(int listType)
        {
            if (listType >= 0)
            {
                tabPlaylists.SelectedIndex = listType;
            }
        }
    }
}
