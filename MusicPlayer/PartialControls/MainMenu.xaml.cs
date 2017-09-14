using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MusicPlayer.PartialControls
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        // ToDo: Move this to Settings
        private void miImportGenres_Click(object sender, RoutedEventArgs e)
        {
            List<string> generas = new List<string>();

            string file = FileUtilities.SelectFile();
            if (!string.IsNullOrEmpty(file))
            {
                generas = FileUtilities.XMLFileToObject<List<string>>(file);

                Database.DatabaseAccess db = new Database.DatabaseAccess();
                db.SaveNewGenres(generas);
            }
        }
    }
}
