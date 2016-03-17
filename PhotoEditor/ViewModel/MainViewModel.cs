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
using System.Windows.Media;

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

        //public Bitmap IMG { get; set; }

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
        public ICommand ClickRotateLeftCommand { get; set; }
        public ICommand ClickFilterCommand { get; set; }

        //static ModelClassImage OpenedImage;

        public MainViewModel()
        {
            //m = new ModelClassImage();
            //fileDial = new FileDialogClass();
            ClickOpenCommand = new Command(arg => OpenFile());
            ClickSaveCommand = new Command(arg => SaveFile());
            ClickRotateRightCommand = new Command(arg => RotateRight());
            ClickRotateLeftCommand = new Command(arg => RotateLeft());
            ClickFilterCommand = new Command(arg => Filt());
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
                    var converter = new ImageSourceConverter();
                    ImageSource imgSrc = (ImageSource)converter.ConvertFromString(dialog.FileName);
                    OpenedImage = new ModelClassImage(dialog.FileName, Path.GetExtension(dialog.FileName),
                        new ModelClassImage.LocalBitmap(new Bitmap(dialog.FileName), imgSrc));
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
            dlg.FileName = "My image";
            dlg.Filter = "JPEG (*.jpg)|*.jpg|PNG(*.png)|*.png|BitMap(*.bmp)|*.bmp";
            bool? result = dlg.ShowDialog();
            
            //img.RotateFlip(RotateFlipType.Rotate90FlipNone);
            if (result == true)
            {
                string ext = OpenedImage.Extension;
                switch (ext)
                {
                    case ".jpg":  
                        //OpenedImage.IMG.Save(dlg.FileName, ImageFormat.Jpeg);
                        OpenedImage.Lb.Img.Save(dlg.FileName, ImageFormat.Jpeg);
                        break;
                    case ".bmp":
                        //OpenedImage.IMG.Save(dlg.FileName, ImageFormat.Bmp);
                        OpenedImage.Lb.Img.Save(dlg.FileName, ImageFormat.Bmp);
                        break;
                    case ".png":
                        //OpenedImage.IMG.Save(dlg.FileName, ImageFormat.Png);
                        OpenedImage.Lb.Img.Save(dlg.FileName, ImageFormat.Png);
                        break;
                }   
            }
        }

        #region rotation methods
        public void RotateRight()
        {
            //OpenedImage.Angle += 90;
            OpenedImage.Lb.Img.RotateFlip(RotateFlipType.Rotate90FlipNone);
            OpenedImage.Lb.Source = ConvertBitmapToImageSource(OpenedImage.Lb.Img);
        }

        public void RotateLeft()
        {
            //OpenedImage.Angle -= 90;
            OpenedImage.Lb.Img.RotateFlip(RotateFlipType.Rotate270FlipNone);
            OpenedImage.Lb.Source = ConvertBitmapToImageSource(OpenedImage.Lb.Img);
        }
        #endregion

        #region filter methods
        private static Bitmap ApplyColorMatrix(Bitmap img, ColorMatrix colorMatrix)
        {
            Bitmap bmpSource = GetArgbCopy(img);
            Bitmap imageToApplyFilter = new Bitmap(bmpSource.Width, bmpSource.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics graphics = Graphics.FromImage(imageToApplyFilter))
            {
                ImageAttributes bmpAttributes = new ImageAttributes();
                bmpAttributes.SetColorMatrix(colorMatrix);
                graphics.DrawImage(bmpSource, new Rectangle(0, 0, bmpSource.Width, bmpSource.Height),
                                    0, 0, bmpSource.Width, bmpSource.Height, GraphicsUnit.Pixel, bmpAttributes);

            }
            return imageToApplyFilter;
        }

        private static Bitmap GetArgbCopy(Bitmap img)
        {
            Bitmap bmpNew = new Bitmap((int)img.Width, (int)img.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics graphics = Graphics.FromImage(bmpNew))
            {
                graphics.DrawImage(img, new Rectangle(0, 0, bmpNew.Width, bmpNew.Height), new Rectangle(0, 0, bmpNew.Width, bmpNew.Height), GraphicsUnit.Pixel);
                graphics.Flush();
            }

            return bmpNew;
        }


        public static Bitmap DrawWithTransparency(Bitmap img)
        {
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                                {
                            new float[]{1, 0, 0, 0, 0},
                            new float[]{0, 1, 0, 0, 0},
                            new float[]{0, 0, 1, 0, 0},
                            new float[]{0, 0, 0, 0.3f, 0},
                            new float[]{0, 0, 0, 0, 1}
                                });


            return ApplyColorMatrix(img, colorMatrix);
        }

        private ImageSource ConvertBitmapToImageSource(Bitmap imToConvert)
        {
            Bitmap bmp = new Bitmap(imToConvert);
            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            ImageSource sc = (ImageSource)image;

            return sc;
        }

        void Filt()
        {
            //OpenedImage.Lb.Img = DrawWithTransparency(OpenedImage.Lb.Img);
            //OpenedImage.Lb.Source = ConvertBitmapToImageSource(OpenedImage.Lb.Img);
            OpenedImage.Lb = new ModelClassImage.LocalBitmap(DrawWithTransparency(OpenedImage.Lb.Img), ConvertBitmapToImageSource(OpenedImage.Lb.Img));
            //OpenedImage.Lb.Source = imgSrc;
            //OpenedImage.Lb.Img = new ModelClassImage.LocalBitmap(new Bitmap(OpenedImage.Lb.Source), OpenedImage.Lb.Source);
            //OpenedImage.ImageSource = OpenedImage.Lb.Source;
        }
        #endregion

        
    }
}
