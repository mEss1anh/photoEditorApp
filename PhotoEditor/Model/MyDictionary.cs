using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoEditor.Model
{
    class MyDictionary
    {
        public List<string> ActionDetails { get; set; }
        public static Dictionary<string, string> ListOfActions
        {
            get
            {
                return new Dictionary<string, string>()
                {
                     ["Open"] = "Image has been opened",
                     ["Close"] = "Image has been Closed",
                     ["Save"] = "Image has been Saved",
                     ["RotationRight"] = "Right rotation has been applied",
                     ["RotationLeft"] = "Left rotation has been applied",
                     ["Transparency"] = "Transparency has been applied",
                     ["Grayscale"] = "Grayscale filter has been applied",
                     ["Sepia"] = "Sepia filter has been applied",
                     ["Median"] = "Median filter has been applied",
                     ["Acute"] = "Acute filter has been applied",
                     ["Blur"] = "Blur filter has been applied",
                     ["Small"] = "The image has been lessened",
                     ["Big"] = "The image was enlarged"
                };
            }
        }
    }
}
