using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Object;
//using System.MarshalByRefObject;
using System.Drawing;
using System.ComponentModel;

namespace PhotoEditor.Model
{
    class ModelClassImage : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ModelClassImage(string imageSource)
        {
            ImageSource = imageSource;
        }

        private string _imageSource;

        public string ImageSource
        {
            get { return _imageSource; }
            set
            {
                if (_imageSource != value)
                {
                    _imageSource = value;
                    OnPropertyChanged("ImageSource");
                }
            }
        }


        public Image IMG { get; set; }

        public double IMGWidth
        {
            get { return IMG.Width; }
            set { }
        }

        public double IMGHeight
        {
            get { return IMG.Height; }
            set { }
        }

    }
}
