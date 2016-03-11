using Microsoft.Win32;
using PhotoEditor.Model;
using PhotoEditor.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Drawing;

namespace PhotoEditor.ViewModel
{

    class MainViewModel
    {
        static ModelClassImage m;

        FileDialogClass fileDial;

        static Image OpenedImage;
        public ICommand ClickOpenCommand { get; set;}
        

        public MainViewModel()
        {
            m = new ModelClassImage();
            fileDial = new FileDialogClass();
            ClickOpenCommand = new Command(arg => fileDial.OpenFile());
        }

     

        public class FileDialogClass
        {
            //public event PropertyChangedEventHandler PropertyChanged;
           
            public FileDialogClass()
            {
                //dialog = new OpenFileDialog();
            }


            public void OpenFile()
            {
                OpenFileDialog dialog = new OpenFileDialog();
               
                dialog.InitialDirectory = Environment.CurrentDirectory;


                dialog.Filter = "Image files |*.jpg;*.png;*.bmp";
                if (dialog.ShowDialog().Value)
                {
                    try
                    {
                        m.ImageSource = dialog.FileName;
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка загрузки данных");
                    }
                }
                //?
                OpenedImage = new Bitmap(m.ImageSource, true);

            }
        }
    }



}
