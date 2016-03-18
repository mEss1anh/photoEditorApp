using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Object;
//using System.MarshalByRefObject;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace PhotoEditor.Model
{
    class ModelClassImage : INotifyPropertyChanged
    {
        #region Implement INotyfyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
        private LocalBitmap lb;
        public LocalBitmap Lb
        {
            get { return lb; }
            set
            {
                if (lb != value)
                {
                    lb = value;
                    OnPropertyChanged("Lb");
                }
            }
        }
        public ModelClassImage(string _imageSource, string _extension, LocalBitmap _bmp)
        {
            ImageSource = _imageSource;
            Extension = _extension;
            lb = _bmp;
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

        public string Extension { get; set; }

        public class LocalBitmap : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            ImageSource _source;
            public ImageSource Source
            {
                get { return _source; }
                set
                {
                    if (_source != value)
                    {
                        _source = value;
                        OnPropertyChanged("Source");
                    }
                }
            }

            Bitmap _img;
            public Bitmap Img
            {
                get { return _img; }
                set
                {
                    if (_img != value)
                    {
                        _img = value;
                        OnPropertyChanged("Img");
                    }
                }
            }

            public LocalBitmap(Bitmap _img, ImageSource _source)
            {
                Img = _img;
                Source = _source;
            }
        }

    }

}
