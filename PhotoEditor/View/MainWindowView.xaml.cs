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

        private void image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == e.LeftButton)
            {
                int ix = (int)(e.XButton1);
                int iy = (int)(e.Y / _zoom);
                //reset _pt2
                _pt2 = new Point(0, 0);
                _pt = new Point(ix, iy);
                // pictureBox1.Invalidate();
                picBoxImageProcessing.Invalidate();
            }
        }

        private void image_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
