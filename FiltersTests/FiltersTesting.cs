using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhotoEditor.ViewModel;
using System.Drawing;

namespace FiltersTests
{
    [TestClass]
    public class FiltersTesting
    {
        [TestMethod]
        //[ExpectedException(typeof(ArgumentNullException))]
        public void Cannot_pass_null_to_ApplyColorMatrix()
        {
            // MainViewModel mvm = new MainViewModel();
            try
            {
                Bitmap testImage = MainViewModel.ApplyColorMatrix(null, null);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        //[ExpectedException(typeof(ArgumentNullException))]
        public void Cannot_pass_null_to_GetArgbCopy()
        {
            try
            {
                Bitmap testImage = MainViewModel.GetArgbCopy(null);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        //[ExpectedException(typeof(ArgumentNullException))]
        public void Cannot_pass_null_to_DrawWithTransparency()
        {
            try
            {
                Bitmap testImage = MainViewModel.DrawWithTransparency(null);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        //[ExpectedException(typeof(ArgumentNullException))]
        public void Cannot_pass_null_to_DrawAsGrayscale()
        {
            try
            {
                Bitmap testImage = MainViewModel.DrawAsGrayscale(null);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        //[ExpectedException(typeof(ArgumentNullException))]
        public void Cannot_pass_null_to_DrawAsSepia()
        {
            try
            {
                Bitmap testImage = MainViewModel.DrawAsSepia(null);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        //[ExpectedException(typeof(ArgumentNullException))]
        public void Cannot_pass_null_to_DrawWithSharpness()
        {
            try
            {
                Bitmap testImage = MainViewModel.DrawWithSharpness(null);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        //[ExpectedException(typeof(ArgumentNullException))]
        public void Cannot_pass_null_to_DrawWithMedian()
        {
            try
            {
                Bitmap testImage = MainViewModel.DrawWithMedian(null);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        //[ExpectedException(typeof(ArgumentNullException))]
        public void Cannot_pass_null_to_ConvolutionFilter()
        {
            try
            {
                Bitmap testImage = MainViewModel.ConvolutionFilter(null, null);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        //[ExpectedException(typeof(ArgumentNullException))]
        public void Cannot_pass_null_to_ResizingOfImage()
        {
            MainViewModel mvm = new MainViewModel();
            try
            {
                Bitmap testImage = mvm.ResizingOfImage(null, 1, 2);
            }
            catch
            {
                Assert.Fail();
            }
        }


    }
}
