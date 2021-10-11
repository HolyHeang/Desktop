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

namespace CamemisOffLine.Component
{
    /// <summary>
    /// Interaction logic for PrintPopup.xaml
    /// </summary>
    public partial class PrintPopup : Window
    {
        public string position { get; set; }
        public Visibility CheckCenter { get; set; }
        public Visibility CheckRight { get; set; }
        public bool isPrint { get; set; } = false;

        public PrintPopup()
        {
            InitializeComponent();
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            if(txtPosition.Text =="")
            {
                position = "នាយក";
                this.Close();
            }
            else
            {
                position = txtPosition.Text;
                this.Close();
            }
            isPrint = true;
        }

        private void checkCenter_Checked(object sender, RoutedEventArgs e)
        {

            try
            {
                if (checkCenter.IsChecked == true)
                {
                    CheckCenter = Visibility.Visible;
                    CheckRight = Visibility.Collapsed;

                    barcenter.Visibility = Visibility.Visible;
                    barRight.Visibility = Visibility.Collapsed;
                }
                else if (checkRight.IsChecked == true)
                {
                    CheckCenter = Visibility.Collapsed;
                    CheckRight = Visibility.Visible;
                    barcenter.Visibility = Visibility.Collapsed;
                    barRight.Visibility = Visibility.Visible;
                }
            }
            catch { }
        }

        private void Exit_MouseEnter(object sender, MouseEventArgs e)
        {
            Exit.Background = Brushes.Red;
        }

        private void Exit_MouseLeave(object sender, MouseEventArgs e)
        {
            Exit.Background = Brushes.Transparent;
        }

        private void Exit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

    }
}
