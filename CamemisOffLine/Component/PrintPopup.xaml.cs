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
        }
    }
}
