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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CamemisOffLine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            

        }

        private void Dt_Tick(object sender, EventArgs e)
        {
            loading.Width += 5;
            if (loading.Width >= 800)
            {
                dt.Stop();
                this.Close();
            }
        }

        DispatcherTimer dt = new DispatcherTimer();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dt.Interval = TimeSpan.FromMilliseconds(10);
            dt.Tick += Dt_Tick;
            loading.Width = 0;
            dt.Start();
        }
    }
}
