using MusicPlayer.FileSupport;
using MusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicPlayer.PartialControls
{
    /// <summary>
    /// Interaction logic for Equalizer.xaml
    /// </summary>
    public partial class Equalizer : UserControl, INotifyPropertyChanged
    {
        #region Properties

        #region Sliders

        #endregion

        #region TextBoxes

        public string Text0
        {
            get { return SelectedEq.Value0.ToString(); }

            set
            {
                if (double.TryParse(value, out tempProp))
                {
                    SelectedEq.Value0 = tempProp;
                }

                OnPropertyChanged("Text0");
            }
        }

        public string Text1
        {
            get { return SelectedEq.Value1.ToString(); }

            set
            {
                if (double.TryParse(value, out tempProp))
                {
                    SelectedEq.Value1 = tempProp;
                }

                OnPropertyChanged("Text0");
            }
        }

        public string Text2
        {
            get { return SelectedEq.Value2.ToString(); }

            set
            {
                if (double.TryParse(value, out tempProp))
                {
                    SelectedEq.Value2 = tempProp;
                }

                OnPropertyChanged("Text0");
            }
        }

        public string Text3
        {
            get { return SelectedEq.Value3.ToString(); }

            set
            {
                if (double.TryParse(value, out tempProp))
                {
                    SelectedEq.Value3 = tempProp;
                }

                OnPropertyChanged("Text0");
            }
        }

        public string Text4
        {
            get { return SelectedEq.Value4.ToString(); }

            set
            {
                if (double.TryParse(value, out tempProp))
                {
                    SelectedEq.Value4 = tempProp;
                }

                OnPropertyChanged("Text0");
            }
        }

        public string Text5
        {
            get { return SelectedEq.Value5.ToString(); }

            set
            {
                if (double.TryParse(value, out tempProp))
                {
                    SelectedEq.Value5 = tempProp;
                }

                OnPropertyChanged("Text0");
            }
        }

        public string Text6
        {
            get { return SelectedEq.Value6.ToString(); }

            set
            {
                if (double.TryParse(value, out tempProp))
                {
                    SelectedEq.Value6 = tempProp;
                }

                OnPropertyChanged("Text0");
            }
        }

        public string Text7
        {
            get { return SelectedEq.Value7.ToString(); }

            set
            {
                if (double.TryParse(value, out tempProp))
                {
                    SelectedEq.Value7 = tempProp;
                }

                OnPropertyChanged("Text0");
            }
        }

        public string Text8
        {
            get { return SelectedEq.Value8.ToString(); }

            set
            {
                if (double.TryParse(value, out tempProp))
                {
                    SelectedEq.Value8 = tempProp;
                }

                OnPropertyChanged("Text0");
            }
        }

        public string Text9
        {
            get { return SelectedEq.Value9.ToString(); }

            set
            {
                if (double.TryParse(value, out tempProp))
                {
                    SelectedEq.Value9 = tempProp;
                }

                OnPropertyChanged("Text0");
            }
        }

        #endregion

        private EqualizerModel _selectedEq;
        public EqualizerModel SelectedEq
        {
            get { return _selectedEq; }
            set
            {
                if (_selectedEq != null && value.Name != _selectedEq.Name)
                {
                    UpdateNew(value);
                }

                _selectedEq = value;
                OnPropertyChanged("SelectedEq");
            }
        }

        private List<EqualizerModel> _eqList;

        public List<EqualizerModel> EqList
        {
            get { return _eqList; }
            set
            {
                _eqList = value;
                OnPropertyChanged("EqList");
            }
        }

        double tempProp = 0;
        private const double MaxDB = 20;
        private const double MaxEqValue = 100;

        #endregion

        public Equalizer()
        {
            InitializeComponent();
            this.DataContext = this;
            SelectedEq = new EqualizerModel();
        }

        public void LoadEqSettings(string lastLoaded = "")
        {
            EqList = EqualizerSupport.LoadEqSettingsList();
            if (string.IsNullOrEmpty(lastLoaded)) { lastLoaded = "Default"; }
            EqualizerModel eqm = EqList.FirstOrDefault(x => x.Name == lastLoaded);
            if (eqm != null)
            {
                SelectedEq = eqm;
            }
            else
            {
                SelectedEq = new EqualizerModel();
                SelectedEq.Name = "Default";
                EqualizerSupport.SaveEqualizerSettings(SelectedEq);
                EqList.Add(SelectedEq);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectedEq = new EqualizerModel();
            for (int i = 0; i <= 9; i++)
            {
                OnEqChanged(i, 0);
                OnPropertyChanged("Text" + i.ToString());
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedEq != null && SelectedEq.Name != "Default")
            {
                EqList.Remove(SelectedEq);
                EqualizerSupport.DeleteEqSettings(SelectedEq.Name);
            }
            else
            {
                MessageBox.Show("Error", "Cannot delete empty or default settings", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbEqList.Text != SelectedEq.Name && EqList.FirstOrDefault(x => x.Name == cmbEqList.Text) == null)
            {
                if (!string.IsNullOrEmpty(cmbEqList.Text))
                {
                    SelectedEq.Name = cmbEqList.Text;
                    EqualizerSupport.SaveEqualizerSettings(SelectedEq);
                }
            }
            else
            {
                EqualizerSupport.SaveEqualizerSettings(SelectedEq);
            }
        }

        private void sldr_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            if (slider != null)
            {
                int index = int.Parse(slider.Name.Substring(4, 1));
                OnPropertyChanged("Text" + index.ToString());
                OnEqChanged(index, e.NewValue);
            }
        }

        private void UpdateNew(EqualizerModel eqModel)
        {
            OnEqChanged(0, eqModel.Value0);
            OnEqChanged(1, eqModel.Value1);
            OnEqChanged(2, eqModel.Value2);
            OnEqChanged(3, eqModel.Value3);
            OnEqChanged(4, eqModel.Value4);
            OnEqChanged(5, eqModel.Value5);
            OnEqChanged(6, eqModel.Value6);
            OnEqChanged(7, eqModel.Value7);
            OnEqChanged(8, eqModel.Value8);
            OnEqChanged(9, eqModel.Value9);

            for (int i = 0; i <= 9; i++)
            {
                OnPropertyChanged("Text" + i.ToString());
            }
        }

        private void OnEqChanged(int filterIndex, double value)
        {
            double perc = value / MaxEqValue;
            var eqValue = (float)(perc * MaxDB);

            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<EqChangedMessage>(new EqChangedMessage(filterIndex, eqValue));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
