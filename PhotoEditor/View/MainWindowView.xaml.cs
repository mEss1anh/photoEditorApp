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

        MainViewModel _viewModel = new MainViewModel();
        public MainWindowView()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        //private void Resize_Click(object sender, RoutedEventArgs e)
        //{
        //    new Parameters().ShowDialog();
        //}

        private void Crop_Click(object sender, RoutedEventArgs e)
        {
            //new Parameters().ShowDialog();
        }

        

        private void image_SizeChanged(object sender, SizeChangedEventArgs e)
        {
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
        }

        //пока запасной вариант

        //private void Open_Click(object sender, RoutedEventArgs e)
        //{
        //    Welcome.Visibility = Visibility.Hidden;
        //    History.Visibility = Visibility.Visible;
        //    ListOfChanges.Visibility = Visibility.Visible;
        //    Resize.Visibility = Visibility.Visible;
        //    RotateLeft.Visibility = Visibility.Visible;
        //    RotateRight.Visibility = Visibility.Visible;
        //    Crop.Visibility = Visibility.Visible;
        //    Contrast.Visibility = Visibility.Visible;
        //    BW.Visibility = Visibility.Visible;
        //    Sepia.Visibility = Visibility.Visible;
        //    Blur.Visibility = Visibility.Visible;
        //    Acutance.Visibility = Visibility.Visible;
        //    Aqua.Visibility = Visibility.Visible;
        //    Save.Visibility = Visibility.Visible;
        //<!-- Click="Open_Click"-->
        //}
    }
}
