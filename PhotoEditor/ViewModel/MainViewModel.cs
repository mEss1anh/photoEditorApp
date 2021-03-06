﻿using Microsoft.Win32;
using PhotoEditor.Model;
using PhotoEditor.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace PhotoEditor.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {

        #region INotifyImplement

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Fields
        private LocalBitmap _openedImage;
        ObservableCollection<string> _listOfActions;
        #endregion

        #region Properties

        public LocalBitmap OpenedImage
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

        public ObservableCollection<string> ListOfActions
        {
            get { return _listOfActions; }
            set
            {
                if (_listOfActions != value)
                {
                    _listOfActions = value;
                    OnPropertyChanged("ListOfActions");
                }
            }
        }

        #endregion

        #region Commands

        public ICommand ClickOpenCommand { get; set; }
        public ICommand ClickSaveCommand { get; set; }
        public ICommand ClickRotateRightCommand { get; set; }
        public ICommand ClickRotateLeftCommand { get; set; }
        public ICommand ClickTransparencyCommand { get; set; }
        public ICommand ClickGrayscaleCommand { get; set; }
        public ICommand ClickSepiaCommand { get; set; }
        public ICommand ClickAcuteCommand { get; set; }
        public ICommand ClickMedianCommand { get; set; }
        public ICommand ClickBlurCommand { get; set; }
        public ICommand ClickResizeMinusCommand { get; set; }
        public ICommand ClickResizePlusCommand { get; set; }

        #endregion

        #region ViewModelCtor

        public MainViewModel()
        {
            ClickOpenCommand = new Command(arg => OpenFile());
            ClickSaveCommand = new Command(arg => SaveFile());
            ClickRotateRightCommand = new Command(arg => RotateRight());
            ClickRotateLeftCommand = new Command(arg => RotateLeft());
            ClickTransparencyCommand = new Command(arg => TransparencyFilter());
            ClickGrayscaleCommand = new Command(arg => GrayscaleFilter());
            ClickSepiaCommand = new Command(arg => SepiaFilter());
            ClickMedianCommand = new Command(arg => MedianFilter());
            ClickAcuteCommand = new Command(arg => AcuteFilter());
            ClickBlurCommand = new Command(arg => BlurFilter());
            ClickResizePlusCommand = new Command(arg => ResizeImagePlus());
            ClickResizeMinusCommand = new Command(arg => ResizeImageMinus());
            ListOfActions = new ObservableCollection<string>();
        }

        #endregion

        #region Open/Save Methods
        public void OpenFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.InitialDirectory = Environment.CurrentDirectory;

            dialog.Filter = "Image files (*.jpg;*.png;*.bmp)| *.jpg;*.png;*.bmp";
            if (dialog.ShowDialog().Value)
            {
                try
                {
                    ClearFile();
                    var converter = new ImageSourceConverter();
                    ImageSource imgSrc = (ImageSource)converter.ConvertFromString(dialog.FileName);
                    OpenedImage = new LocalBitmap(new Bitmap(dialog.FileName), imgSrc);
                    OpenedImage.ImgFormat = OpenedImage.Img.RawFormat;
                    saveInfo(MyDictionary.ListOfActions["Open"]);
                }
                catch
                {
                    new ProblemsSolver().Show();
                }
            }
        }


        public void SaveFile()
        {
            try
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.FileName = "My image";
                dlg.Filter = "JPEG (*.jpg)|*.jpg|PNG(*.png)|*.png|BitMap(*.bmp)|*.bmp";
                bool? result = dlg.ShowDialog();

                if (result == true)
                {
                    ImageFormat format = OpenedImage.ImgFormat;
                    if (format.Equals(ImageFormat.Jpeg))
                        OpenedImage.Img.Save(dlg.FileName, ImageFormat.Jpeg);
                    else if (format.Equals(ImageFormat.Bmp))
                        OpenedImage.Img.Save(dlg.FileName, ImageFormat.Bmp);
                    else if (format.Equals(ImageFormat.Png))
                        OpenedImage.Img.Save(dlg.FileName, ImageFormat.Png);
                    saveInfo(MyDictionary.ListOfActions["Save"]);
                }
            }
            catch
            {
                new ProblemsSolver().Show();
            }
        }

        #endregion

        #region Rotation methods
        public void RotateRight()
        {
            try
            {
                OpenedImage.Img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                OpenedImage.Source = ConvertBitmapToImageSource(OpenedImage.Img);
                saveInfo(MyDictionary.ListOfActions["RotationRight"]);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("It's high time You chose an image!");
            }
        }

        public void RotateLeft()
        {
            try
            {
                OpenedImage.Img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                OpenedImage.Source = ConvertBitmapToImageSource(OpenedImage.Img);
                saveInfo(MyDictionary.ListOfActions["RotationLeft"]);
            }

            catch (NullReferenceException)
            {
                MessageBox.Show("It's high time You chose an image!");

            }
        }
        #endregion

        #region SupportFilters methods
        public static Bitmap ApplyColorMatrix(Bitmap img, ColorMatrix colorMatrix)
        {
            try
            {
                Bitmap bmpSource = GetArgbCopy(img);
                Bitmap imageToApplyFilter = new Bitmap(bmpSource.Width, bmpSource.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (Graphics graphics = Graphics.FromImage(imageToApplyFilter))
                {
                    ImageAttributes bmpAttributes = new ImageAttributes();
                    bmpAttributes.SetColorMatrix(colorMatrix);
                    graphics.DrawImage(bmpSource, new Rectangle(0, 0, bmpSource.Width, bmpSource.Height),
                                        0, 0, bmpSource.Width, bmpSource.Height, GraphicsUnit.Pixel, bmpAttributes);
                    return imageToApplyFilter;
                }
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public static Bitmap GetArgbCopy(Bitmap img)
        {
            try
            {

                Bitmap bmpNew = new Bitmap(img.Width, img.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (Graphics graphics = Graphics.FromImage(bmpNew))
                {
                    graphics.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);
                    graphics.Flush();
                }

                return bmpNew;
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("It's high time You chose an image!");
                return null;
            }

        }

        public static Bitmap DrawWithTransparency(Bitmap img)
        {
            try
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


            catch (NullReferenceException)
            {
                MessageBox.Show("It's high time You chose an image!");
                return null;
            }
        }

        public static Bitmap DrawAsGrayscale(Bitmap sourceImage)
        {
            try
            {
                ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                                    {
                            new float[]{.3f, .3f, .3f, 0, 0},
                            new float[]{.59f, .59f, .59f, 0, 0},
                            new float[]{.11f, .11f, .11f, 0, 0},
                            new float[]{0, 0, 0, 1, 0},
                            new float[]{0, 0, 0, 0, 1}
                                    });


                return ApplyColorMatrix(sourceImage, colorMatrix);
            }

            catch (NullReferenceException)
            {
                MessageBox.Show("It's high time You chose an image!");
                return null;
            }
        }

        public static Bitmap DrawAsSepia(Bitmap sourceImage)
        {
            try
            {
                ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                           {
                        new float[]{.393f, .349f, .272f, 0, 0},
                        new float[]{.769f, .686f, .534f, 0, 0},
                        new float[]{.189f, .168f, .131f, 0, 0},
                        new float[]{0, 0, 0, 1, 0},
                        new float[]{0, 0, 0, 0, 1}
                           });


                return ApplyColorMatrix(sourceImage, colorMatrix);
            }

            catch (NullReferenceException)
            {
                MessageBox.Show("It's high time You chose an image!");
                return null;
            }
        }


        public static Bitmap DrawWithSharpness(Bitmap image)
        {
            try
            {
                Bitmap sharpenImage = (Bitmap)image.Clone();

                int filterWidth = 3;
                int filterHeight = 3;
                int width = image.Width;
                int height = image.Height;

                double[,] filter = new double[filterWidth, filterHeight];
                filter[0, 0] = filter[0, 1] = filter[0, 2] = filter[1, 0] = filter[1, 2] = filter[2, 0] = filter[2, 1] = filter[2, 2] = -1;
                filter[1, 1] = 9;

                double factor = 1.0;
                double bias = 0.0;

                System.Drawing.Color[,] result = new System.Drawing.Color[image.Width, image.Height];


                BitmapData pbits = sharpenImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);


                int bytes = pbits.Stride * height;
                byte[] rgbValues = new byte[bytes];


                System.Runtime.InteropServices.Marshal.Copy(pbits.Scan0, rgbValues, 0, bytes);

                int rgb;

                for (int x = 0; x < width; ++x)
                {
                    for (int y = 0; y < height; ++y)
                    {
                        double red = 0.0, green = 0.0, blue = 0.0;

                        for (int filterX = 0; filterX < filterWidth; filterX++)
                        {
                            for (int filterY = 0; filterY < filterHeight; filterY++)
                            {
                                int imageX = (x - filterWidth / 2 + filterX + width) % width;
                                int imageY = (y - filterHeight / 2 + filterY + height) % height;

                                rgb = imageY * pbits.Stride + 3 * imageX;

                                red += rgbValues[rgb + 2] * filter[filterX, filterY];
                                green += rgbValues[rgb + 1] * filter[filterX, filterY];
                                blue += rgbValues[rgb + 0] * filter[filterX, filterY];
                            }
                            int r = Math.Min(Math.Max((int)(factor * red + bias), 0), 255);
                            int g = Math.Min(Math.Max((int)(factor * green + bias), 0), 255);
                            int b = Math.Min(Math.Max((int)(factor * blue + bias), 0), 255);

                            result[x, y] = System.Drawing.Color.FromArgb(r, g, b);
                        }
                    }
                }

                for (int x = 0; x < width; ++x)
                {
                    for (int y = 0; y < height; ++y)
                    {
                        rgb = y * pbits.Stride + 3 * x;

                        rgbValues[rgb + 2] = result[x, y].R;
                        rgbValues[rgb + 1] = result[x, y].G;
                        rgbValues[rgb + 0] = result[x, y].B;
                    }
                }


                System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, pbits.Scan0, bytes);

                sharpenImage.UnlockBits(pbits);

                return sharpenImage;
            }

            catch (NullReferenceException)
            {
                MessageBox.Show("It's high time You chose an image!");
                return null;
            }
        }

        public static Bitmap DrawWithMedian(Bitmap sourceBitmap,
                                  int matrixSize = 3)
        {
            try
            {
                BitmapData sourceData =
                           sourceBitmap.LockBits(new Rectangle(0, 0,
                           sourceBitmap.Width, sourceBitmap.Height),
                           ImageLockMode.ReadOnly,
                           System.Drawing.Imaging.PixelFormat.Format32bppArgb);


                byte[] pixelBuffer = new byte[sourceData.Stride *
                                              sourceData.Height];


                byte[] resultBuffer = new byte[sourceData.Stride *
                                               sourceData.Height];


                System.Runtime.InteropServices.Marshal.Copy(sourceData.Scan0, pixelBuffer, 0,
                                           pixelBuffer.Length);


                sourceBitmap.UnlockBits(sourceData);


                int filterOffset = (matrixSize - 1) / 2;
                int calcOffset = 0;


                int byteOffset = 0;


                List<int> neighbourPixels = new List<int>();
                byte[] middlePixel;


                for (int offsetY = filterOffset; offsetY <
                    sourceBitmap.Height - filterOffset; offsetY++)
                {
                    for (int offsetX = filterOffset; offsetX <
                        sourceBitmap.Width - filterOffset; offsetX++)
                    {
                        byteOffset = offsetY *
                                     sourceData.Stride +
                                     offsetX * 4;


                        neighbourPixels.Clear();


                        for (int filterY = -filterOffset;
                            filterY <= filterOffset; filterY++)
                        {
                            for (int filterX = -filterOffset;
                                filterX <= filterOffset; filterX++)
                            {


                                calcOffset = byteOffset +
                                             (filterX * 4) +
                                             (filterY * sourceData.Stride);


                                neighbourPixels.Add(BitConverter.ToInt32(
                                                 pixelBuffer, calcOffset));
                            }
                        }


                        neighbourPixels.Sort();

                        middlePixel = BitConverter.GetBytes(
                                           neighbourPixels[filterOffset]);


                        resultBuffer[byteOffset] = middlePixel[0];
                        resultBuffer[byteOffset + 1] = middlePixel[1];
                        resultBuffer[byteOffset + 2] = middlePixel[2];
                        resultBuffer[byteOffset + 3] = middlePixel[3];
                    }
                }


                Bitmap resultBitmap = new Bitmap(sourceBitmap.Width,
                                                 sourceBitmap.Height);


                BitmapData resultData =
                           resultBitmap.LockBits(new Rectangle(0, 0,
                           resultBitmap.Width, resultBitmap.Height),
                           ImageLockMode.WriteOnly,
                           System.Drawing.Imaging.PixelFormat.Format32bppArgb);


                System.Runtime.InteropServices.Marshal.Copy(resultBuffer, 0, resultData.Scan0,
                                           resultBuffer.Length);


                resultBitmap.UnlockBits(resultData);


                return resultBitmap;
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("It's high time You chose an image!");
                return null;
            }
        }

        public static Bitmap ConvolutionFilter(Bitmap sourceBitmap, double[,] filterMatrix, double factor = 1, int bias = 0)
        {
            try
            {

                BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height),
                                                           ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
                byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];

                System.Runtime.InteropServices.Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
                sourceBitmap.UnlockBits(sourceData);

                double blue = 0.0;
                double green = 0.0;
                double red = 0.0;

                int filterWidth = filterMatrix.GetLength(1);
                int filterHeight = filterMatrix.GetLength(0);

                int filterOffset = (filterWidth - 1) / 2;
                int calcOffset = 0;

                int byteOffset = 0;

                for (int offsetY = filterOffset; offsetY <
                    sourceBitmap.Height - filterOffset; offsetY++)
                {
                    for (int offsetX = filterOffset; offsetX <
                        sourceBitmap.Width - filterOffset; offsetX++)
                    {
                        blue = 0;
                        green = 0;
                        red = 0;

                        byteOffset = offsetY *
                                     sourceData.Stride +
                                     offsetX * 4;

                        for (int filterY = -filterOffset;
                            filterY <= filterOffset; filterY++)
                        {
                            for (int filterX = -filterOffset;
                                filterX <= filterOffset; filterX++)
                            {

                                calcOffset = byteOffset +
                                             (filterX * 4) +
                                             (filterY * sourceData.Stride);

                                blue += pixelBuffer[calcOffset] *
                                        filterMatrix[filterY + filterOffset,
                                                            filterX + filterOffset];

                                green += (double)(pixelBuffer[calcOffset + 1]) *
                                         filterMatrix[filterY + filterOffset,
                                                            filterX + filterOffset];

                                red += (double)(pixelBuffer[calcOffset + 2]) *
                                       filterMatrix[filterY + filterOffset,
                                                          filterX + filterOffset];
                            }
                        }

                        blue = factor * blue + bias;
                        green = factor * green + bias;
                        red = factor * red + bias;

                        blue = (blue > 255 ? 255 :
                               (blue < 0 ? 0 :
                                blue));

                        green = (green > 255 ? 255 :
                                (green < 0 ? 0 :
                                 green));

                        red = (red > 255 ? 255 :
                              (red < 0 ? 0 :
                               red));

                        resultBuffer[byteOffset] = (byte)(blue);
                        resultBuffer[byteOffset + 1] = (byte)(green);
                        resultBuffer[byteOffset + 2] = (byte)(red);
                        resultBuffer[byteOffset + 3] = 255;
                    }
                }


                Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);


                BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                         resultBitmap.Width, resultBitmap.Height),
                                                          ImageLockMode.WriteOnly,
                                                     System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                System.Runtime.InteropServices.Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
                resultBitmap.UnlockBits(resultData);

                return resultBitmap;
            }

            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("It's high time You chose an image!");
                return null;
            }
        }
        #endregion

        #region ImplementFilters methods
        void TransparencyFilter()
        {
            OpenedImage.Img = DrawWithTransparency(OpenedImage.Img);
            OpenedImage.Source = ConvertBitmapToImageSource(OpenedImage.Img);
            saveInfo(MyDictionary.ListOfActions["Transparency"]);
        }

        void GrayscaleFilter()
        {
            OpenedImage.Img = DrawAsGrayscale(OpenedImage.Img);
            OpenedImage.Source = ConvertBitmapToImageSource(OpenedImage.Img);
            saveInfo(MyDictionary.ListOfActions["Grayscale"]);
        }

        void SepiaFilter()
        {
            OpenedImage.Img = DrawAsSepia(OpenedImage.Img);
            OpenedImage.Source = ConvertBitmapToImageSource(OpenedImage.Img);
            saveInfo(MyDictionary.ListOfActions["Sepia"]);
        }

        void MedianFilter()
        {
            OpenedImage.Img = DrawWithMedian(OpenedImage.Img);
            OpenedImage.Source = ConvertBitmapToImageSource(OpenedImage.Img);
            saveInfo(MyDictionary.ListOfActions["Median"]);
        }

        void AcuteFilter()
        {
            OpenedImage.Img = DrawWithSharpness(OpenedImage.Img);
            OpenedImage.Source = ConvertBitmapToImageSource(OpenedImage.Img);
            saveInfo(MyDictionary.ListOfActions["Acute"]);
        }

        void BlurFilter()
        {
            OpenedImage.Img = ConvolutionFilter(OpenedImage.Img, GaussianMatrixForBlur.GaussianBlur3x3);
            OpenedImage.Source = ConvertBitmapToImageSource(OpenedImage.Img);
            saveInfo(MyDictionary.ListOfActions["Blur"]);
        }
        #endregion

        #region Resize methods
        public Bitmap ResizingOfImage(Bitmap image, int width, int height)
        {
            Bitmap imageCopy = image;
            try
            {
                var rectangleToImplement = new Rectangle(0, 0, width, height);
                var imageToImplement = new Bitmap(width, height);

                imageToImplement.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                using (var graphics = Graphics.FromImage(imageToImplement))
                {
                    graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                    graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                    using (var wrapMode = new ImageAttributes())
                    {
                        wrapMode.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);
                        graphics.DrawImage(image, rectangleToImplement, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                    }
                }
                return imageToImplement;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("It's high time you chose an image!");
                return null;
            }
        }

        void ResizeImagePlus()
        {
            try
            {
                OpenedImage.Img = ResizingOfImage(OpenedImage.Img, (int)(OpenedImage.Img.Width * 1.3), (int)(OpenedImage.Img.Height * 1.3));
                OpenedImage.Source = ConvertBitmapToImageSource(OpenedImage.Img);
                saveInfo(MyDictionary.ListOfActions["Big"]);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);

            }
            catch (NullReferenceException)
            {
                MessageBox.Show("It's high time You chose an image!");

            }
        }

        void ResizeImageMinus()
        {
            try
            {
                OpenedImage.Img = ResizingOfImage(OpenedImage.Img, (int)(OpenedImage.Img.Width / 1.3), (int)(OpenedImage.Img.Height / 1.3));
                OpenedImage.Source = ConvertBitmapToImageSource(OpenedImage.Img);
                saveInfo(MyDictionary.ListOfActions["Small"]);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);

            }
            catch (NullReferenceException)
            {
                MessageBox.Show("It's high time You chose an image!");

            }
        }

        #endregion

        #region Auxiliary methods

        public ImageSource ConvertBitmapToImageSource(Bitmap imToConvert)
        {
            try
            {
                Bitmap bmp = new Bitmap(imToConvert);
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, ImageFormat.Bmp);

                BitmapImage image = new BitmapImage();
                image.BeginInit();
                ms.Seek(0, SeekOrigin.Begin);
                image.StreamSource = ms;
                image.EndInit();
                ImageSource sc = image;
                return sc;
            }
            catch (OutOfMemoryException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            catch (ExternalException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }
        #endregion

        #region Async Methods

        public async void saveInfo(string str)
        {
            Application.Current.Dispatcher.Invoke(() => displayInfo(str, ListOfActions.Count), DispatcherPriority.Normal);
            await Task.Factory.StartNew(() => saveInfoToFile(str));
        }

        private void saveInfoToFile(string str)
        {
            using (StreamWriter w = File.AppendText("FileOfActions"))
            {
                w.WriteLine(str);
            }
        }

        void displayInfo(string str, int len)
        {
            ListOfActions.Add(str);
        }

        void ClearFile()
        {
            File.Delete("FileOfActions");
        }

        #endregion
    }
}

