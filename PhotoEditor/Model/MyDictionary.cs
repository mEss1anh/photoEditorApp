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
        public int MyProperty { get; set; }
        public Dictionary<string, string> ListOfActions
        {
            get
            {
                return new Dictionary<string, string>()
                {
                     ["Open"] = "User opened file:",
                     ["Close"] = "User closed file" 
                };
            }
        }
    }
}
