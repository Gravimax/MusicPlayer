using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace MusicPlayer.Views
{
    /// <summary>
    /// Interaction logic for EditPlayList.xaml
    /// </summary>
    public partial class EditPlayList : Window, INotifyPropertyChanged
    {
        private string oldValue;

        private string _value;
        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        private string _message;

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged("Message");
            }
        }

        private List<string> listNames;

        public EditPlayList()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public EditPlayList(string value, string title, List<string> listNames)
        {
            InitializeComponent();
            this.DataContext = this;
            this.Value = value;
            oldValue = value;
            this.Title = title;
            this.listNames = listNames;
        }

        private void txtInput_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter || e.Key == System.Windows.Input.Key.Return)
            {
                if (ValidateName())
                {
                    RemoveInvalidChars();
                    this.DialogResult = true;
                    this.Close();
                }
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateName())
            {
                RemoveInvalidChars();
                this.DialogResult = true;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private bool ValidateName()
        {
            if (string.IsNullOrEmpty(_value))
            {
                Message = "Playlist name cannot be empty";
                return false;
            }

            if (oldValue != _value)
            {
                if (listNames.Contains(_value))
                {
                    Message = "A playlist with that name already exists";
                    return false;
                }
            }

            Message = string.Empty; ;
            return true;
        }

        private void RemoveInvalidChars()
        {
            foreach (var c in System.IO.Path.GetInvalidFileNameChars())
            {
                Value = Value.Replace(c.ToString(), "");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        }
    }
}
