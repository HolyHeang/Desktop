using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CamemisOffLine.Windows
{
    /// <summary>
    /// Interaction logic for Loading.xaml
    /// </summary>
    public partial class Loading : Window
    {
        bool check = true;
        public Loading(bool img = false)
        {
            InitializeComponent();

            if(img)
            {
                check = img;
                image.Visibility = Visibility.Collapsed;
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            }
            else
            {
                image.Visibility = Visibility.Visible;
            }
        }
    }
}
