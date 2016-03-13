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
    class ModelClassImage 
    {
        
        public ModelClassImage(string imageSource)
        {
            ImageSource = imageSource;
        }

        public string ImageSource { get; set; }
        
        //private string _imageSource;

        //public string ImageSource
        //{
        //    get { return _imageSource; }
        //    set
        //    {
        //        if (_imageSource != value)
        //        {
        //            _imageSource = value;
        //            OnPropertyChanged("ImageSource");
        //        }
        //    }
    


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
