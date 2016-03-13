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

    class MainViewModel : INotifyPropertyChanged
    {
        // static ModelClassImage m;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private ModelClassImage _openedImage;

        public ModelClassImage OpenedImage
        {
            get { return _openedImage; }
            set
            {
                if (_openedImage != value)
                {
                    _openedImage = value;
                    OnPropertyChanged("OpenedImage");
                }
            }
        }
            FileDialogClass fileDial;

        public ICommand ClickOpenCommand { get; set; }
        //static ModelClassImage OpenedImage;

        public MainViewModel()
        {
            //m = new ModelClassImage();
            fileDial = new FileDialogClass();
            ClickOpenCommand = new Command(arg => OpenFile());
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
                    OpenedImage = new ModelClassImage(dialog.FileName);
                }
                catch
                {
                    MessageBox.Show("Ошибка загрузки данных");
                }
            }
        }

        public class FileDialogClass
        {
            //public event PropertyChangedEventHandler PropertyChanged;

            public FileDialogClass()
            {
                //dialog = new OpenFileDialog();
            }


            
        }
    }
}
