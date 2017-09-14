using System.Windows.Controls;

namespace MusicPlayer.PartialControls
{
    /// <summary>
    /// Interaction logic for PlayList.xaml
    /// </summary>
    public partial class PlayList : UserControl
    {
        public PlayList()
        {
            InitializeComponent();

            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<SelectionChangedMessage>(this, (message) =>
            {
                this.musicList.SelectedItem = musicList.Items[message.Index];
                this.UpdateLayout();
                this.musicList.ScrollIntoView(musicList.Items[message.Index]);
            });
        }
    }
}
