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
    /// Interaction logic for MessageBoxControl.xaml
    /// </summary>
    public partial class MessageBoxControl : Window
    {
        public int result { get; set; }
        public string title { get; set; }
        public string discription { get; set; }
        public int buttonType { get; set; }
        public string yes { get; set; } = Properties.Langs.Lang.yes;
        public string no { get; set; } = Properties.Langs.Lang.cancel;
        public MessageBoxControl()
        {
            InitializeComponent();
        }
        private void showButton(int btnType)
        {
            if (btnType == 1)
                btnYes.Visibility = Visibility.Collapsed;
            else if(btnType == 2)
                btnNo.Visibility = Visibility.Collapsed;
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            result = 0;
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            result = 1;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            btnYes.Content = yes;
            btnNo.Content = no;
            txtDis.Text = this.discription;
            txtTitle.Text = this.title;
            showButton(buttonType);
        }
    }
}