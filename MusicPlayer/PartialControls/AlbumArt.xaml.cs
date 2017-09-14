using MusicPlayer.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MusicPlayer.PartialControls
{
    /// <summary>
    /// Interaction logic for AlbumArt.xaml
    /// </summary>
    public partial class AlbumArt : UserControl
    {
        public AlbumArt()
        {
            InitializeComponent();
        }

        private void albumImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                ViewAlbumArt view = new ViewAlbumArt();
                view.albumArt.Source = albumImage.Source;
                view.Owner = Window.GetWindow(this);
                view.Show();
            }
        }
    }
}
