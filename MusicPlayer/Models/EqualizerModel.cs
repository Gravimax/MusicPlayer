using System;
using System.Xml.Serialization;

namespace MusicPlayer.Models
{
    [Serializable]
    public  class EqualizerModel : Notifier
    {
        private string _name;
        [XmlElement(DataType = "string", ElementName = "Name")]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private double _value0;
        [XmlElement(DataType = "double", ElementName = "Value0")]
        public double Value0
        {
            get { return _value0; }

            set
            {
                _value0 = value;
                OnPropertyChanged("Value0");
            }
        }

        private double _value1;
        [XmlElement(DataType = "double", ElementName = "Value1")]
        public double Value1
        {
            get { return _value1; }

            set
            {
                _value1 = value;
                OnPropertyChanged("Value1");
            }
        }

        private double _value2;
        [XmlElement(DataType = "double", ElementName = "Value2")]
        public double Value2
        {
            get { return _value2; }

            set
            {
                _value2 = value;
                OnPropertyChanged("Value2");
            }
        }

        private double _value3;
        [XmlElement(DataType = "double", ElementName = "Value3")]
        public double Value3
        {
            get { return _value3; }

            set
            {
                _value3 = value;
                OnPropertyChanged("Value3");
            }
        }

        private double _value4;
        [XmlElement(DataType = "double", ElementName = "Value4")]
        public double Value4
        {
            get { return _value4; }

            set
            {
                _value4 = value;
                OnPropertyChanged("Value4");
            }
        }

        private double _value5;
        [XmlElement(DataType = "double", ElementName = "Value5")]
        public double Value5
        {
            get { return _value5; }

            set
            {
                _value5 = value;
                OnPropertyChanged("Value5");
            }
        }

        private double _value6;
        [XmlElement(DataType = "double", ElementName = "Value6")]
        public double Value6
        {
            get { return _value6; }

            set
            {
                _value6 = value;
                OnPropertyChanged("Value6");
            }
        }

        private double _value7;
        [XmlElement(DataType = "double", ElementName = "Value7")]
        public double Value7
        {
            get { return _value7; }

            set
            {
                _value7 = value;
                OnPropertyChanged("Value7");
            }
        }

        private double _value8;
        [XmlElement(DataType = "double", ElementName = "Value8")]
        public double Value8
        {
            get { return _value8; }

            set
            {
                _value8 = value;
                OnPropertyChanged("Value8");
            }
        }

        private double _value9;
        [XmlElement(DataType = "double", ElementName = "Value9")]
        public double Value9
        {
            get { return _value9; }

            set
            {
                _value9 = value;
                OnPropertyChanged("Value9");
            }
        }
    }
}
