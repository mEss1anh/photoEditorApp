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
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace PhotoEditor.ViewModel
{

    class MainViewModel : INotifyPropertyChanged
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
        private ObservableCollection<string> _listOfActions;
        private int _height;
        private int _width;
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

        public int Width
        {
            get { return _width; }
            set
            {
                if (_width != value)
                {
                    _width = value;
                    OnPropertyChanged("Width");
                }
            }
        }


        public int Height
        {
            get { return _height; }
            set
            {
                if (_height != value)
                {
                    _height = value;
                    OnPropertyChanged("Height");
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
                    var converter = new ImageSourceConverter();
                    ImageSource imgSrc = (ImageSource)converter.ConvertFromString(dialog.FileName);
                    OpenedImage = new LocalBitmap(new Bitmap(dialog.FileName), imgSrc);
                    OpenedImage.ImgFormat = OpenedImage.Img.RawFormat;
                    Parameters p = new Parameters();
                    Width = OpenedImage.Img.Width;
                    Height = OpenedImage.Img.Height;                    
                }
                catch
                {
                    MessageBox.Show("Ошибка загрузки данных");
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

                }
            }
            catch
            {
                //окно чуть позже присоединю
                MessageBox.Show("Ошибка сохранения данных");
            }
            }

        #endregion
        
        #region Rotation methods
        public void RotateRight()
        {
            try {
                ListOfActions.Add(MyDictionary.ListOfActions["RotationRight"]);
                OpenedImage.Img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                OpenedImage.Source = ConvertBitmapToImageSource(OpenedImage.Img);
            }
           catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
               
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
                
            }
        }

        public void RotateLeft()
        {
            try
            {
                OpenedImage.Img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                OpenedImage.Source = ConvertBitmapToImageSource(OpenedImage.Img);
            }
           
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
                
            }
        }
        #endregion

        #region SupportFilters methods
        private static Bitmap ApplyColorMatrix(Bitmap img, ColorMatrix colorMatrix)
        {
            if ((img == null)||(colorMatrix == null))
                throw new ArgumentNullException();
            if ((img.GetType() != typeof(Bitmap))||(colorMatrix.GetType() != typeof(ColorMatrix)))
                throw new ArgumentException();

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

        private static Bitmap GetArgbCopy(Bitmap img)
        {
            if (img == null)
                throw new ArgumentNullException();
            if (img.GetType() != typeof(Bitmap))
                throw new ArgumentException();

            Bitmap bmpNew = new Bitmap(img.Width, img.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (Graphics graphics = Graphics.FromImage(bmpNew))
                {
                    graphics.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);
                    graphics.Flush();
                }

                return bmpNew;
            
        }


        public static Bitmap DrawWithTransparency(Bitmap img)
        {
            try {
                ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                                    {
                            new float[]{1, 0, 0, 0, 0},
                            new float[]{0, 1, 0, 0, 0},
                            new float[]{0, 0, 1, 0, 0},
                            new float[]{0, 0, 0, 0.3f, 0},
                            new float[]{0, 0, 0, 0, 1}
                                    });


                return ApplyColorMatrix(img, colorMatrix); }
           
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
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
           
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
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
            
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }


        public static Bitmap DrawWithSharpness(Bitmap image)
        {
            try {
                if (image == null)
                    throw new ArgumentNullException();
                if (image.GetType() != typeof(Bitmap))
                    throw new ArgumentException();

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
            
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public static Bitmap DrawWithMedian(Bitmap sourceBitmap,
                                  int matrixSize = 3)
        {
            try {
                if (sourceBitmap == null)
                    throw new ArgumentNullException();
                if (sourceBitmap.GetType() != typeof(Bitmap))
                    throw new ArgumentException();

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


                return resultBitmap; }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        private static Bitmap ConvolutionFilter(Bitmap sourceBitmap, double[,] filterMatrix, double factor = 1, int bias = 0)
        {
            try {

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

                return resultBitmap; }
            
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
            #endregion

        #region ImplementFilters methods
            void TransparencyFilter()
        {
            OpenedImage.Img = DrawWithTransparency(OpenedImage.Img);
            OpenedImage.Source = ConvertBitmapToImageSource(OpenedImage.Img);
        }

        void GrayscaleFilter()
        {
            OpenedImage.Img = DrawAsGrayscale(OpenedImage.Img);
            OpenedImage.Source = ConvertBitmapToImageSource(OpenedImage.Img);
        }

        void SepiaFilter()
        {
            OpenedImage.Img = DrawAsSepia(OpenedImage.Img);
            OpenedImage.Source = ConvertBitmapToImageSource(OpenedImage.Img);
        }

        void MedianFilter()
        {
            OpenedImage.Img = DrawWithMedian(OpenedImage.Img);
            OpenedImage.Source = ConvertBitmapToImageSource(OpenedImage.Img);
        }

        void AcuteFilter()
        {
            OpenedImage.Img = DrawWithSharpness(OpenedImage.Img);
            OpenedImage.Source = ConvertBitmapToImageSource(OpenedImage.Img);
        }

        void BlurFilter()
        {
            OpenedImage.Img = ConvolutionFilter(OpenedImage.Img, GaussianMatrixForBlur.GaussianBlur3x3);
            OpenedImage.Source = ConvertBitmapToImageSource(OpenedImage.Img);
        }
        #endregion

        #region Resize methods
        public Bitmap ResizingOfImage(Bitmap image, int width, int height)
        {
            Bitmap imageCopy = image;
            try
            {
                if (image == null)
                    throw new ArgumentNullException();
                //if ((image.GetType() != typeof(Bitmap)) || (width.GetType() != typeof(int)) || (height.GetType() != typeof(int)))
                //    throw new ArgumentException();
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
        }

        void ResizeImagePlus()
        { 
            OpenedImage.Img = ResizingOfImage(OpenedImage.Img, (int)(OpenedImage.Img.Width * 1.3), (int)(OpenedImage.Img.Height * 1.3));
            OpenedImage.Source = ConvertBitmapToImageSource(OpenedImage.Img);
        }

        void ResizeImageMinus()
        {
            OpenedImage.Img = ResizingOfImage(OpenedImage.Img, (int)(OpenedImage.Img.Width / 1.3), (int)(OpenedImage.Img.Height / 1.3));
            OpenedImage.Source = ConvertBitmapToImageSource(OpenedImage.Img);
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
            catch(ExternalException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            catch(ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            catch(NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public async void saveInfo()
        {

        }

        #endregion
    }
}
