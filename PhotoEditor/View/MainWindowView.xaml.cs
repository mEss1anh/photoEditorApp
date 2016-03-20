using PhotoEditor.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PhotoEditor.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary> 
    public partial class MainWindowView
    {
        //public static bool Availability = false;

        MainViewModel _viewModel = new MainViewModel();
        public MainWindowView()
        {
            InitializeComponent();
            DataContext = _viewModel;
                 
        }



        private void image_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //if (Availability)

            //{
           // Welcome.Visibility = Welcome.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;

            Welcome.Visibility = Visibility.Hidden;
            History.Visibility = Visibility.Visible;
            ListOfChanges.Visibility = Visibility.Visible;
            ResizePlus.Visibility = Visibility.Visible;
            ResizeMinus.Visibility = Visibility.Visible;
            RotateLeft.Visibility = Visibility.Visible;
            RotateRight.Visibility = Visibility.Visible;
            Crop.Visibility = Visibility.Visible;
            Contrast.Visibility = Visibility.Visible;
            BW.Visibility = Visibility.Visible;
            Sepia.Visibility = Visibility.Visible;
            Blur.Visibility = Visibility.Visible;
            Acutance.Visibility = Visibility.Visible;
            Aqua.Visibility = Visibility.Visible;
            Save.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    Welcome.Visibility = Visibility.Visible;
            //    History.Visibility = Visibility.Hidden;
            //    ListOfChanges.Visibility = Visibility.Hidden;
            //    ResizePlus.Visibility = Visibility.Hidden;
            //    ResizeMinus.Visibility = Visibility.Hidden;
            //    RotateLeft.Visibility = Visibility.Hidden;
            //    RotateRight.Visibility = Visibility.Hidden;
            //    Crop.Visibility = Visibility.Hidden;
            //    Contrast.Visibility = Visibility.Hidden;
            //    BW.Visibility = Visibility.Hidden;
            //    Sepia.Visibility = Visibility.Hidden;
            //    Blur.Visibility = Visibility.Hidden;
            //    Acutance.Visibility = Visibility.Hidden;
            //    Aqua.Visibility = Visibility.Hidden;
            //    Save.Visibility = Visibility.Hidden;
            //}
        }

       
    }
}
