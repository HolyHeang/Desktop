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
using CamemisOffLine.Component;

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

            WindowState = System.Windows.WindowState.Maximized;

            name = Properties.Settings.Default.username;
            pass = Properties.Settings.Default.password;
        }

       
        //-----------------------------Login
       
        //-----------------------------end login
        //start window
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
            btnLogin.Visibility = Visibility.Collapsed;
            btnPass.Visibility = Visibility.Collapsed;
            btnUser.Visibility = Visibility.Collapsed;
            gridBackLogin.Visibility = Visibility.Collapsed;
            txtBoxPass.Text = txtPass.Password;
            

            if (!InternetChecker())
            {
                this.Opacity = 0.5;
                MessageBoxControl message = new MessageBoxControl();
                message.title = Properties.Langs.Lang.Information;
                message.discription = Properties.Langs.Lang.No_internet_connection;
                message.buttonType = 2;
                message.ShowDialog();
                this.Opacity = 1;
            }
            lblError.Visibility = Visibility.Collapsed;
            Init_data();
            if(Properties.Settings.Default.checkLoginOrLogut== "login")
            {
                if (Properties.Settings.Default.role == "1")
                {
                    Teacher teacher = new Teacher();
                    teacher.Show();
                }
                else if (Properties.Settings.Default.role=="2"|| Properties.Settings.Default.role == "3")
                {
                    PartTeacher partTeacher = new PartTeacher();
                    partTeacher.Show();
                }
                
                this.Close();
            }
        }
        //end start window
        private void txtUser_GotFocus(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            lblError.Visibility = Visibility.Collapsed;
            btnPassCode.BorderBrush = (Brush)bc.ConvertFrom("#1183CA");
            btnPassCode.CornerRadius =  new CornerRadius(2);
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


        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
           

            try
            {
                if (InternetChecker())
                {
                    if (txtUser.Text=="" || txtPass.Password=="")
                    {
                        this.Opacity = 0.5;
                        MessageBoxControl message = new MessageBoxControl();
                        message.title = Properties.Langs.Lang.Information;
                        message.discription = Properties.Langs.Lang.incorrectPassandUser;
                        message.buttonType = 2;
                        message.ShowDialog();
                       
                       
                        Properties.Settings.Default.checkLoginOrLogut = "logout";
                        Properties.Settings.Default.Save();
                        this.Opacity = 1;
                    }
                    else
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            client.DefaultRequestHeaders.Accept.Add(
                           new MediaTypeWithQualityHeaderValue("application/json"));
                            using (HttpResponseMessage res = client.PostAsJsonAsync("https://api-camemis-mobile.camis-info.com", new PersonalLogin { action_key = "EnOLNTB1Q", schoolUrl = txtCode.Text,username=txtUser.Text,password=txtPass.Password }).Result)
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
                                        this.Opacity = 0.5;
                                        MessageBoxControl message = new MessageBoxControl();
                                        message.title = Properties.Langs.Lang.Information;
                                        message.discription = Properties.Langs.Lang.No_profile_image;
                                        message.buttonType = 2;
                                        message.ShowDialog();
                                        Properties.Settings.Default.localProfileLink = "";
                                        Properties.Settings.Default.Save();
                                        this.Opacity = 1;
                                    }
                                    if (d.success)
                                    {
                                        Properties.Settings.Default.checkLoginOrLogut = "login";
                                        Properties.Settings.Default.teacherId = d.data.ID;
                                        Properties.Settings.Default.Token = d.laravel_token_data.access_token;
                                        Properties.Settings.Default.acessUrl = d.laravel_token_data.access_url;
                                        Properties.Settings.Default.profileName = d.data.NAME;
                                        Properties.Settings.Default.gender = d.data.GENDER;
                                        Properties.Settings.Default.role = d.data.ROLE;
                                        Properties.Settings.Default.userData = datas;
                                        Properties.Settings.Default.username = txtUser.Text;
                                        Properties.Settings.Default.password = txtPass.Password;
                                        Properties.Settings.Default.ExpireDate = d.laravel_token_data.expires_in_date;
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
                                        this.Opacity = 0.5;
                                        MessageBoxControl message = new MessageBoxControl();
                                        message.title = Properties.Langs.Lang.Information;
                                        message.discription = Properties.Langs.Lang.incorrectPassandUser;
                                        message.buttonType = 2;
                                        message.ShowDialog();
                                        this.Opacity = 1;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (txtUser.Text.Equals("") || txtPass.Password.Equals("") || txtBoxPass.Text.Equals(""))
                    {
                        this.Opacity = 0.5;
                        MessageBoxControl message = new MessageBoxControl();
                        message.title = Properties.Langs.Lang.Information;
                        message.discription = Properties.Langs.Lang.incorrectPassandUser;
                        message.buttonType = 2;
                        message.ShowDialog();
                        this.Opacity = 1;
                    }
                    else
                    {
                        if (name == txtUser.Text && pass == txtPass.Password && pass == txtBoxPass.Text)
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
                            this.Opacity = 0.5;
                            MessageBoxControl message = new MessageBoxControl();
                            message.title = Properties.Langs.Lang.Information;
                            message.discription = Properties.Langs.Lang.incorrectPassandUser;
                            message.buttonType = 2;
                            message.ShowDialog();
                            this.Opacity = 1;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            txtBoxPass.Visibility = Visibility.Collapsed;
            txtPass.Visibility = Visibility.Visible;
            txtPass.Password = txtBoxPass.Text;
            showPass.Kind = MaterialDesignThemes.Wpf.PackIconKind.Eye;
        }
        //mouse over exit button........

        //Button Click Next
        private async void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (txtPass.Password == "")
            {
                showPass.Visibility = Visibility.Collapsed;
            }
            else
            {
                showPass.Visibility = Visibility.Visible;
            }
            if (InternetChecker())
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                   new MediaTypeWithQualityHeaderValue("application/json"));
                    using (HttpResponseMessage res = client.PostAsJsonAsync("https://api-camemis-mobile.camis-info.com", new Schools {action_key= "Pu0QUvj82x",url=txtCode.Text }).Result)
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
                                Properties.Settings.Default.logoNameLeft = d.data.schoolSettings.LOGO_LEFT_NAME;
                                Properties.Settings.Default.Save();
                                btnNext.Visibility = Visibility.Collapsed;
                                btnPassCode.Visibility = Visibility.Collapsed;
                                btnUser.Visibility = Visibility.Visible;
                                btnPass.Visibility = Visibility.Visible;
                                btnLogin.Visibility = Visibility.Visible;
                                gridBackLogin.Visibility = Visibility.Visible;
                                
     
                            }
                            else
                            {
                                this.Opacity = 0.5;
                                MessageBoxControl message = new MessageBoxControl();
                                message.title = Properties.Langs.Lang.Information;
                                message.discription = Properties.Langs.Lang.School_code_not_found;
                                message.buttonType = 2;
                                message.ShowDialog();
                                this.Opacity = 1;
                            }
                        }
                    }
                }
            }
            else
            {
                if (txtCode.Text.Equals(""))
                {
                    this.Opacity = 0.5;
                    MessageBoxControl message = new MessageBoxControl();
                    message.title = Properties.Langs.Lang.Information;
                    message.discription = Properties.Langs.Lang.incorrectPassandUser;
                    message.buttonType = 2;
                    message.ShowDialog();
                    this.Opacity = 1;
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
                       
                    }
                    else
                    {
                        this.Opacity = 0.5;
                        MessageBoxControl message = new MessageBoxControl();
                        message.title = Properties.Langs.Lang.Information;
                        message.discription = Properties.Langs.Lang.School_code_not_found;
                        message.buttonType = 2;
                        message.ShowDialog();
                        this.Opacity = 1;
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

        //...................................Buttom Bar Top....................................
        private void gridExit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void gridExit_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid g = (Grid)sender;
            g.Background = Brushes.Red;
        }

        private void gridExit_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid g = (Grid)sender;
            g.Background = Brushes.Transparent;
        }

        private void gridmax_MouseDown(object sender, MouseButtonEventArgs e)
        {


            if (maximized.Kind == MaterialDesignThemes.Wpf.PackIconKind.WindowMaximize)
            {

                WindowState = System.Windows.WindowState.Maximized;
                GridForm.Margin=new Thickness(0);
                maximized.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowRestore;
            }
            else
            {
                
                ControlMaximize.DoMaximize(this);
                GridForm.Margin = new Thickness(0);
                maximized.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowMaximize;               
            }
        }

        private void gridmax_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid g = (Grid)sender;
            g.Background = Brushes.Gray;
        }

        private void gridmax_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid g = (Grid)sender;
            g.Background = Brushes.Transparent;
        }

        private void gridmini_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = System.Windows.WindowState.Minimized;
        }

        private void gridmini_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid g = (Grid)sender;
            g.Background = Brushes.Gray;
        }

        private void gridmini_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid g = (Grid)sender;
            g.Background = Brushes.Transparent;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            btnLogin.Visibility = Visibility.Collapsed;
            btnPass.Visibility = Visibility.Collapsed;
            btnUser.Visibility = Visibility.Collapsed;
            btnPassCode.Visibility = Visibility.Visible;
            btnNext.Visibility = Visibility.Visible;
            gridBackLogin.Visibility = Visibility.Collapsed;

            txtBoxPass.Visibility = Visibility.Collapsed;
            txtPass.Visibility = Visibility.Visible;
            txtPass.Password = txtBoxPass.Text;
            showPass.Kind = MaterialDesignThemes.Wpf.PackIconKind.Eye;

        }

        private void txtCode_MouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            btnPassCode.BorderBrush = (Brush)bc.ConvertFrom("#1183CA");
            btnPassCode.CornerRadius = new CornerRadius(0);
            MarPasscode.Foreground= (Brush)bc.ConvertFrom("#1183CA");
        }

        private void txtCode_MouseLeave(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            btnPassCode.BorderBrush = Brushes.Gray;
            btnPassCode.CornerRadius = new CornerRadius(0);
            MarPasscode.Foreground = Brushes.Gray;
        }

        private void txtPass_MouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            btnPass.BorderBrush = (Brush)bc.ConvertFrom("#1183CA");
            btnPass.CornerRadius = new CornerRadius(0);
            MarPass.Foreground = (Brush)bc.ConvertFrom("#1183CA");
        }

        private void txtPass_MouseLeave(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            btnPass.BorderBrush = Brushes.Gray;
            btnPass.CornerRadius = new CornerRadius(0);
            MarPass.Foreground = Brushes.Gray;
        }

        private void txtUser_MouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            btnUser.BorderBrush = (Brush)bc.ConvertFrom("#1183CA");
            btnUser.CornerRadius = new CornerRadius(0);
            MarUser.Foreground = (Brush)bc.ConvertFrom("#1183CA");
        }

        private void txtUser_MouseLeave(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            btnUser.BorderBrush = Brushes.Gray;
            btnUser.CornerRadius = new CornerRadius(0);
            MarUser.Foreground = Brushes.Gray;
        }

        private void showPass_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (showPass.Kind == MaterialDesignThemes.Wpf.PackIconKind.Eye)
            {
                txtBoxPass.Text = txtPass.Password;
                txtPass.Visibility = Visibility.Collapsed;
                txtBoxPass.Visibility = Visibility.Visible;
               
                showPass.Kind = MaterialDesignThemes.Wpf.PackIconKind.EyeOff;
            }
            else
            {
                txtPass.Password = txtBoxPass.Text;
                txtBoxPass.Visibility = Visibility.Collapsed;
                txtPass.Visibility = Visibility.Visible;
                
                showPass.Kind = MaterialDesignThemes.Wpf.PackIconKind.Eye;
            }
                
        }

        private void GridForm_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void txtPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (txtPass.Password == "")
            {
                showPass.Visibility = Visibility.Collapsed;
            }
            else
            {
                showPass.Visibility = Visibility.Visible;
            }
        }

        private void txtBoxPass_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        //..................................End................................
        //Internte Checker
        public bool InternetChecker()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.youtube.com"))
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
