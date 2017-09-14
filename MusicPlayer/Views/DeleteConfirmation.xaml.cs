using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MusicPlayer.Views
{
    /// <summary>
    /// Interaction logic for DeleteConfirmation.xaml
    /// </summary>
    public partial class DeleteConfirmation : Window
    {
        public DeleteConfirmation()
        {
            InitializeComponent();
        }

        public FileDeleteType DeletionType = FileDeleteType.ListOnly;

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DeletionType = delList.IsChecked == true ? FileDeleteType.ListOnly : FileDeleteType.ListAndComputer;
            this.DialogResult = true;
            this.Close();
        }
    }
}
