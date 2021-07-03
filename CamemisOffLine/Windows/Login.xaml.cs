using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
using CamemisOffLine.Class;
using Newtonsoft.Json;
using System.Runtime;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using Library;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using CamemisOffLine.Windows;

namespace CamemisOffLine
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    /// 
    
    public partial class Login : Window
    {
        HttpClient client = new HttpClient();

        string name = "",pass= "";
        public Login()
        {
            InitializeComponent();
            name = Properties.Settings.Default.username;
            pass = Properties.Settings.Default.password;
           
        }

        private void PackIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
        //-----------------------------Login
       
        //-----------------------------end login
        //start window
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            btnLogin.Visibility = Visibility.Collapsed;
            btnPass.Visibility = Visibility.Collapsed;
            btnUser.Visibility = Visibility.Collapsed;
            btnNext.Margin = btnLogin.Margin;
            gridLogin.Width = 380;
            gridLogin.Height = 300;

            if (!InternetChecker())
            {
                MessageBox.Show("No internet connection", "Warnning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            lblError.Visibility = Visibility.Collapsed;
            Init_data();
        }
        //end start window
        private void txtUser_GotFocus(object sender, RoutedEventArgs e)
        {
            lblError.Visibility = Visibility.Collapsed;
        }
        //save data
        private void SaveData()
        {
            if (cbRememberMe.IsChecked == true)
            {
                Properties.Settings.Default.username = txtUser.Text;
                Properties.Settings.Default.password = txtPass.Password;
                Properties.Settings.Default.schoolCode = txtCode.Text;
                Properties.Settings.Default.remember = "yes";
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.username = txtUser.Text;
                Properties.Settings.Default.schoolCode = txtCode.Text;
                Properties.Settings.Default.password = "";
                Properties.Settings.Default.remember = "no";
                Properties.Settings.Default.Save();
            }
        }
        //end save data
        //Init data
        private void Init_data()
        {
            if (Properties.Settings.Default.username != string.Empty)
            {
                if(Properties.Settings.Default.remember=="yes")
                {
                    txtUser.Text = Properties.Settings.Default.username;
                    txtPass.Password = Properties.Settings.Default.password;
                    txtCode.Text = Properties.Settings.Default.schoolCode;
                    cbRememberMe.IsChecked = true;
                }
                else
                {
                    txtUser.Text = Properties.Settings.Default.username;
                    txtCode.Text = Properties.Settings.Default.schoolCode;
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            backgroundImg.Source = new BitmapImage(new Uri(@"/CamemisOffLine;component/Image/Nature1.jpg", UriKind.Relative));
            
           
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            backgroundImg.Source = new BitmapImage(new Uri(@"/CamemisOffLine;component/Image/Nature2.jpg", UriKind.Relative));


        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            backgroundImg.Source = new BitmapImage(new Uri(@"/CamemisOffLine;component/Image/Nature3.jpg", UriKind.Relative));
            
          
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            backgroundImg.Source = new BitmapImage(new Uri(@"/CamemisOffLine;component/Image/Nature4.jpg", UriKind.Relative));
           
           
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            backgroundImg.Source = new BitmapImage(new Uri(@"/CamemisOffLine;component/Image/Nature5.jpg", UriKind.Relative));
            
           
        }


        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (InternetChecker())
                {
                    if (txtUser.Text.Equals("") || txtPass.Password.Equals(""))
                    {
                        MessageBox.Show(Properties.Langs.Lang.Incorect_User_Pass);
                        Properties.Settings.Default.checkLoginOrLogut = "logout";
                        Properties.Settings.Default.Save();
                    }
                    else
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            client.DefaultRequestHeaders.Accept.Add(
                           new MediaTypeWithQualityHeaderValue("application/json"));
                            using (HttpResponseMessage res = client.PostAsJsonAsync("https://mobile-campus.camemis-learn.com", new PersonalLogin { action_key = "EnOLNTB1Q", schoolUrl = txtCode.Text,username=txtUser.Text,password=txtPass.Password }).Result)
                            {
                                using (HttpContent content = res.Content)
                                {
                                    string datas = await content.ReadAsStringAsync();
                                    var d = JObject.Parse(datas).ToObject<PersonalDataLogin>();
                                    try
                                    {
                                        SaveImage(d.data.ID, ImageFormat.Jpeg, d.data.PROFILE_PHOTO);
                                        Properties.Settings.Default.localProfileLink = filePath + "\\" + d.data.ID + ".jpg";
                                    }
                                    catch
                                    {
                                        MessageBox.Show("No profile image.!!!","Warnning",MessageBoxButton.OK,MessageBoxImage.Warning);
                                        Properties.Settings.Default.localProfileLink = "";
                                        Properties.Settings.Default.Save();
                                    }
                                    if (d.success)
                                    {
                                        Properties.Settings.Default.checkLoginOrLogut = "login";
                                        Properties.Settings.Default.Token = d.laravel_token_data.access_token;
                                        Properties.Settings.Default.acessUrl = d.laravel_token_data.access_url;
                                        Properties.Settings.Default.profileName = d.data.NAME;
                                        Properties.Settings.Default.gender = d.data.GENDER;
                                        Properties.Settings.Default.role = d.data.ROLE;
                                        Properties.Settings.Default.userData = datas;
                                        Properties.Settings.Default.username = txtUser.Text;
                                        Properties.Settings.Default.password = txtPass.Password;
                                        Properties.Settings.Default.Save();
                                        SaveData();
                                        Teacher teacher = new Teacher();
                                        PartTeacher partTeacher = new PartTeacher();
                                        this.Close();
                                        if(d.data.ROLE=="1")
                                        {
                                            teacher.Show();
                                        }
                                        else if(d.data.ROLE == "2"||d.data.ROLE=="3")
                                        {
                                            partTeacher.Show();
                                        }
                                    }
                                    else
                                    {
                                        Properties.Settings.Default.checkLoginOrLogut = "logout";
                                        Properties.Settings.Default.Save();
                                        MessageBox.Show(Properties.Langs.Lang.fail, Properties.Langs.Lang.Information, MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (txtUser.Text.Equals("") || txtPass.Password.Equals(""))
                    {
                        MessageBox.Show(Properties.Langs.Lang.Incorect_User_Pass,Properties.Langs.Lang.Information, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        if (name == txtUser.Text && pass == txtPass.Password)
                        {

                            Properties.Settings.Default.checkLoginOrLogut = "login";
                            Properties.Settings.Default.Save();
                            Teacher teacher = new Teacher();
                            this.Close();
                            teacher.Show();
                        }
                        else
                        {

                            Properties.Settings.Default.checkLoginOrLogut = "logout";
                            Properties.Settings.Default.Save();
                            MessageBox.Show(Properties.Langs.Lang.Incorect_User_Pass, Properties.Langs.Lang.Information, MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //mouse over exit button........

        private void grideExit_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid g = (Grid)sender;
            g.Background = Brushes.LightGray;
        }

        private void grideExit_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid g = (Grid)sender;
            g.Background = Brushes.Transparent;
        }

        private void btnLogin_MouseEnter(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
           btn.Background = Brushes.Gray;
            
        }

        private void btnLogin_MouseLeave(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            btn.Background = Brushes.Transparent;
        }

        //Button Click Next
        private async void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if(InternetChecker())
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                   new MediaTypeWithQualityHeaderValue("application/json"));
                    using (HttpResponseMessage res = client.PostAsJsonAsync("https://mobile-campus.camemis-learn.com",new Schools {action_key= "Pu0QUvj82x",url=txtCode.Text }).Result)
                    {
                        using (HttpContent content = res.Content)
                        {
                            string datas = await content.ReadAsStringAsync();
                            var d = JObject.Parse(datas).ToObject<SchoolUrl>();

                            if (d.success)
                            {
                                Properties.Settings.Default.schoolName = d.data.schoolSettings.LOGO_LEFT_SLOGAN;
                                Properties.Settings.Default.schoolName_en = d.data.schoolSettings.LOGO_LEFT_SLOGAN_EN;
                                Properties.Settings.Default.schoolData = datas;
                                Properties.Settings.Default.schoolCode = txtCode.Text;
                                Properties.Settings.Default.Save();
                                btnNext.Visibility = Visibility.Collapsed;
                                btnPassCode.Visibility = Visibility.Collapsed;
                                btnUser.Visibility = Visibility.Visible;
                                btnPass.Visibility = Visibility.Visible;
                                btnLogin.Visibility = Visibility.Visible;
                                gridLogin.Width = 380;
                                gridLogin.Height = 400;
                            }
                            else
                            {
                                MessageBox.Show("School not Found.!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
            else
            {
                if (txtCode.Text.Equals(""))
                {
                    MessageBox.Show(Properties.Langs.Lang.Incorect_User_Pass, Properties.Langs.Lang.Information, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (Properties.Settings.Default.schoolCode == txtCode.Text)
                    {

                        btnNext.Visibility = Visibility.Collapsed;
                        btnPassCode.Visibility = Visibility.Collapsed;
                        btnUser.Visibility = Visibility.Visible;
                        btnPass.Visibility = Visibility.Visible;
                        btnLogin.Visibility = Visibility.Visible;
                        gridLogin.Width = 380;
                        gridLogin.Height = 400;
                    }
                    else
                    {
                        MessageBox.Show("School not Found.!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }



        //end Init data

        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Templates);
        public void SaveImage(string filename, ImageFormat format, string imageUrl)
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(imageUrl);
            System.Drawing.Bitmap bitmap;
            bitmap = new System.Drawing.Bitmap(stream);

            if (bitmap != null)
            {
                bitmap.Save(filePath + "\\" + filename + ".jpg", format);
            }

            stream.Flush();
            stream.Close();
            client.Dispose();
        }

        //Internte Checker
        public bool InternetChecker()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        //end Internet checker
      
    }
}
