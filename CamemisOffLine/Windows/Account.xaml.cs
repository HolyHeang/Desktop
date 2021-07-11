using Library;
using Newtonsoft.Json.Linq;
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

namespace CamemisOffLine.Windows
{
    /// <summary>
    /// Interaction logic for Account.xaml
    /// </summary>
    public partial class Account : Window
    {
        public Account()
        {
            InitializeComponent();
            setVaule();
        }

        public Account(string name)
        {
            InitializeComponent();
            lblName.Text = name;
        }

        private void lblEx_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        private void btnPersonal_Click(object sender, RoutedEventArgs e)
        {
            GridCursor.Margin = new Thickness(10, -10, 0, 0);
            Personalinfor.SelectedIndex = 0;
        }

        private void btnChangepass_Click(object sender, RoutedEventArgs e)
        {
            GridCursor.Margin = new Thickness(180, -10, 0, 0);
            Personalinfor.SelectedIndex = 1;
        }
        private void setVaule()
        {
            if(Properties.Settings.Default.localProfileLink.ToString()!="")
                imgUserProfile.Source = new BitmapImage(new Uri(Properties.Settings.Default.localProfileLink.ToString()));
            var obj = JObject.Parse(Properties.Settings.Default.userData).ToObject<PersonalDataLogin>().data;
            lblName.Text = obj.NAME;
            txtAddress.Text = obj.ADDRESS;
            txtDate.Text = obj.DATE_BIRTH;
            txtEmail.Text = obj.EMAIL;
            txtfirst.Text = obj.FIRSTNAME;
            txtLast.Text = obj.LASTNAME;
            txtPhone.Text = obj.PHONE;
            if(obj.GENDER=="1")
            {
                txtGen.Text = "ប្រុស";
            }
            else
            {
                txtGen.Text = "ស្រី";
            }
        }
    }
}
