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
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;

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
        //FileDialogClass fileDial;

        public ICommand ClickOpenCommand { get; set; }
        public ICommand ClickSaveCommand { get; set; }
        public ICommand ClickRotateRightCommand { get; set; }
        //static ModelClassImage OpenedImage;

        public MainViewModel()
        {
            //m = new ModelClassImage();
            //fileDial = new FileDialogClass();
            ClickOpenCommand = new Command(arg => OpenFile());
            ClickSaveCommand = new Command(arg => SaveFile());
            ClickRotateRightCommand = new Command(arg => RotateRight());
        }

        public void OpenFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.InitialDirectory = Environment.CurrentDirectory;

            dialog.Filter = "Image files (*.jpg;*.png;*.bmp)| *.jpg;*.png;*.bmp";
            if (dialog.ShowDialog().Value)
            {
                try
                {
                    var imageFormat = new Bitmap(dialog.FileName).RawFormat;
                    OpenedImage = new ModelClassImage(dialog.FileName, Path.GetExtension(dialog.FileName),
                        new Bitmap(dialog.FileName));
                }
                catch
                {
                    MessageBox.Show("Ошибка загрузки данных");
                }
            }
        }


        public void SaveFile()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            ImageFormat format = ImageFormat.Jpeg;
            dlg.FileName = "My image";
            dlg.Filter = "JPEG (*.jpg)|*.jpg|PNG(*.png)|*.png|BitMap(*.bmp)|*.bmp";
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                string ext = OpenedImage.Extension;
                switch (ext)
                {
                    case ".jpg":
                        format = ImageFormat.Jpeg;
                        OpenedImage.IMG.Save(dlg.FileName, ImageFormat.Jpeg);
                        break;
                    case ".bmp":
                        format = ImageFormat.Bmp;
                        OpenedImage.IMG.Save(dlg.FileName, ImageFormat.Bmp);
                        break;
                    case ".png":
                        format = ImageFormat.Png;
                        OpenedImage.IMG.Save(dlg.FileName, ImageFormat.Png);
                        break;
                }
            }
        }


        public void RotateRight()
        {
            OpenedImage.Angle += 90;
        }
    }
}
