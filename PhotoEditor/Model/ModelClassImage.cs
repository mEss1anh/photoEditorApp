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
        
        public ModelClassImage(string _imageSource, string _extension)
        {
            ImageSource = _imageSource;
            Extension = _extension;
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

    }
}
