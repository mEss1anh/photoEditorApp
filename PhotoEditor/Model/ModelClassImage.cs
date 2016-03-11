using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Object;
//using System.MarshalByRefObject;
using System.Drawing;

namespace PhotoEditor.Model
{
    class ModelClassImage
    {
        public string ImageSource { get; set; }

        
        public Image IMG { get; set; }

       // public double IMGWidth { get; set; }


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
