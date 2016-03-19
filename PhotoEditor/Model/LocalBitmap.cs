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

            private ImageFormat _imgFormat;

            public ImageFormat ImgFormat
            {
                get { return _imgFormat; }
                set
                {
                    if (_imgFormat != value)
                    {
                        _imgFormat = value;
                        OnPropertyChanged("ImgFormat");
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
