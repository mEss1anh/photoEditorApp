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

        public ModelClassImage(string _imageSource, string _extension, Image _img)
        {
            ImageSource = _imageSource;
            Extension = _extension;
            IMG = _img;
        }

        public string ImageSource { get; set; }
        
        public Image IMG { get; set; }

        public string Extension { get; set; }

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

        private double _angle;

        public double Angle
        {
            get { return _angle; }
            set
            {
                if (_angle != value)
                {
                    _angle = value;
                    OnPropertyChanged("Angle");
                }
            }
        }

    }
}
