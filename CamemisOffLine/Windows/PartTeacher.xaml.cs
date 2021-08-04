﻿using CamemisOffLine.Component;
using CamemisOffLine.Report;
using Library;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
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
    /// Interaction logic for PartTeacher.xaml
    /// </summary>
    public partial class PartTeacher : Window
    {
        public string YearSelection { get; private set; }
        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Templates);
        string classId = "",term="";
        InputScore obj = new InputScore();
        private string studentYear;

        public PartTeacher()
        {
            InitializeComponent();
            DispatcherTimer Internet = new DispatcherTimer();
            Internet.Tick += Internet_Tick;
            Internet.Interval = TimeSpan.FromSeconds(30);
            Internet.Start();
        }
            int ping = 0;

        private void Internet_Tick(object sender, EventArgs e)
        {

            Ping myPing = new Ping();
            try
            {
                PingReply reply = myPing.Send(@"Google.com", 1000);
                if (Teacher.InternetChecker() && internet)
                {
                    if (reply != null)
                    {
                        ping = int.Parse((reply.RoundtripTime).ToString());
                        if (ping >= 0 && ping <= 99)
                            wifiIcon.Foreground = Brushes.Green;
                        else if (ping >= 100 && ping <= 200)
                            wifiIcon.Foreground = Brushes.Yellow;
                        else
                        {
                            wifiIcon.Foreground = Brushes.Red;
                            this.Opacity = 0.5;
                           /* MessageBoxControl message = new MessageBoxControl();
                            message.title = "ដំណឹង";
                            message.discription = "សេវាអ៊ីនធឺណែតខ្សោយ!! សូមត្រួតពីនិត្យអ៊ីនធឺណែតរបស់អ្នកម្តងទៀត";
                            message.buttonType = 2;
                            message.ShowDialog();*/
                            this.Opacity = 1;
                        }
                        txtPing.Text = "Ping :" + (ping) + "ms";
                    }
                }
                else
                {
                    ping = 999;
                    wifiIcon.Foreground = Brushes.Red;
                    txtPing.Text = "Ping :" + (ping) + "ms";
                    wifiIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WifiOff;
                }
            }
            catch
            {
                ping = 999;
                wifiIcon.Foreground = Brushes.Red;
                txtPing.Text = "Ping :" + (ping) + "ms";
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
            //.........................Control Header Result..................

            Selectresult.Visibility = Visibility.Collapsed;
            cbSelectClass.Visibility = Visibility.Collapsed;
            cbSelectMonth.Visibility = Visibility.Collapsed;
            cbSelectSubject.Visibility = Visibility.Collapsed;
            //.....................................
            DockTree.Visibility = Visibility.Collapsed;
            tabcontrolResult.SelectedIndex = 1;
            tabcontrolScore.SelectedIndex = 1;
            TilteSelection.Content = Properties.Langs.Lang.Message_Box_Stu_Result_Title_select_year;
            TitleSchool.Text = Properties.Settings.Default.schoolName;
            txtTitleClassName.Visibility = Visibility.Collapsed;
            btnLearningResult.IsEnabled = false;

            if (Teacher.InternetChecker())
            {
                btnCheck.IsChecked = true;
                txtxCheckinternet.Content = "Online";
            }
            else
            {
                btnCheck.IsChecked = false;
                txtxCheckinternet.Content = "Offline";
            }
            //-------------User Profile----------------
            if (Properties.Settings.Default.localProfileLink.ToString() != "")
                imgUserProfile.Source = new BitmapImage(new Uri(Properties.Settings.Default.localProfileLink.ToString()));
            //------------------------------------------

            //...............select Menu Stype.................
            var bc = new BrushConverter();
            btnScore.Background = Brushes.White;
            MateriaScore.Foreground = (Brush)bc.ConvertFrom("#1183CA");
            lblScore.Foreground = (Brush)bc.ConvertFrom("#1183CA");

            //..................Show defual index................
            tabcontrolMenu.SelectedIndex = 0;
            tabcontrolResult.SelectedIndex = 1;
            tabcontrolResulandInput.SelectedIndex = 0;
            tabcontrolScore.SelectedIndex = 1;
            DockMonth.Visibility = Visibility.Collapsed;
            btnInputScore.Background = Brushes.White;
            lblTitle.Content = Properties.Langs.Lang.score;
            stacButtonTop.Visibility = Visibility.Collapsed;

            //.............Slide Left..............
            slidLeft.Width = 45;
            gridAcc.Visibility = Visibility.Collapsed;
            lblnameCompany.Visibility = Visibility.Collapsed;
           


            //btnHome.IsEnabled = false;
            ///.................Part Setting...................
            btnAbout.Visibility = Visibility.Collapsed;
            btnColor.Visibility = Visibility.Collapsed;
            MateriaSettingUp.Visibility = Visibility.Collapsed;
            btnLang.Visibility = Visibility.Collapsed;
            gridLanguage.Visibility = Visibility.Collapsed;
            MateriaLangDrop.Visibility = Visibility.Visible;
            MateriaSettingDrop.Visibility = Visibility.Visible;
            //....................End.........................
            Loading loading = new Loading();
            //-------------------Acadymic Year----------------
            if (Teacher.InternetChecker()==true)
            {
                loading.Owner = this;
                loading.ShowInTaskbar = false;
                loading.Show();
                string accessUrl = Properties.Settings.Default.acessUrl;
                string token = Properties.Settings.Default.Token;
                var respone = await RESTApiHelper.GetAll(accessUrl, "/get-teaching-class", token);
                var obj = JObject.Parse(respone).ToObject<Teachers>().data;
                Properties.Settings.Default.schoolAcademyYear = respone;
                Properties.Settings.Default.Save();
                cbAcademyYear.ItemsSource = obj.Select(s=>s.name);
            }
            else
            {
                string respone = Properties.Settings.Default.schoolAcademyYear;
                var obj = JObject.Parse(respone).ToObject<Teachers>().data;              
                cbAcademyYear.ItemsSource = obj.Select(s => s.name);
            }
            //------------------------------------------------
            loading.Close();
        }

        //...................Event Top Bar.....................................


        //...................Min................................
        private void gridMin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = System.Windows.WindowState.Minimized;
        }

        private void gridMin_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid g = (Grid)sender;
            g.Background = Brushes.LightGray;
        }

        private void gridMin_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid g = (Grid)sender;
            g.Background = Brushes.Transparent;
        }
        //..............Min end..................


        //..............Max..............
        private void gridMax_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Normal)
            {
                WindowState = System.Windows.WindowState.Maximized;
                materiaMax.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowRestore;
            }
            else
            {
                WindowState = System.Windows.WindowState.Normal;
                materiaMax.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowMaximize;
            }
        }

        private void gridMax_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid g = (Grid)sender;
            g.Background = Brushes.LightGray;
        }

        private void gridMax_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid g = (Grid)sender;
            g.Background = Brushes.Transparent;
        }
        //.................End Max................


        //.............Close..................
        private void gridClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void gridClose_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid g = (Grid)sender;
            g.Background = Brushes.Red;
        }

        private void gridClose_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid g = (Grid)sender;
            g.Background = Brushes.Transparent;
        }
        //....................End close..................

        //.....................End.................................................
        

        //..........................Tab Button..................
        private void btnLearningResult_Click(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            btnLearningResult.Background = Brushes.White;  
            btnInputScore.Background = (Brush)bc.ConvertFrom("#66D3D3D3");
            tabcontrolResulandInput.SelectedIndex = 1;
        }

        private void btnInputScore_Click(object sender, RoutedEventArgs e)
        {
            
            var bc = new BrushConverter();
            btnLearningResult.Background = (Brush)bc.ConvertFrom("#66D3D3D3");
            btnInputScore.Background = Brushes.White;
            tabcontrolResulandInput.SelectedIndex = 0;
            
        }

        //......................Hover Slide Left......................
        private void slidLeft_MouseEnter(object sender, MouseEventArgs e)
        {
            slidLeft.Width = 320;
            gridAcc.Visibility = Visibility.Visible;
            lblnameCompany.Visibility = Visibility.Visible;
            
            MateriaSettingDrop.Visibility = Visibility.Visible;
            //btnHome.IsEnabled = true;
            MateriaLangDrop.Visibility = Visibility.Visible;
            MateriaSettingDrop.Visibility = Visibility.Visible;
        }

        private void slidLeft_MouseLeave(object sender, MouseEventArgs e)
        {
            slidLeft.Width = 45;
           
        
            gridAcc.Visibility = Visibility.Collapsed;
            lblnameCompany.Visibility = Visibility.Collapsed;
           


            //btnHome.IsEnabled = false;
            ///.................Part Setting...................
            btnAbout.Visibility = Visibility.Collapsed;
            btnColor.Visibility = Visibility.Collapsed;
            MateriaSettingUp.Visibility = Visibility.Collapsed;
            btnLang.Visibility = Visibility.Collapsed;
            gridLanguage.Visibility = Visibility.Collapsed;
            MateriaLangDrop.Visibility = Visibility.Visible;
            MateriaSettingDrop.Visibility = Visibility.Visible;
            //....................End.........................

        }
        bool checkSelection = true, startProgram = false, changeAcademyYear = true;
        string year = "";
        private void cbAcademyYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            clearAllSelection();
            TilteSelection.Content = Properties.Langs.Lang.Message_Box_Stu_Result_Title;
            cbSelectClass.Visibility = Visibility.Visible;
            List<KeyValuePair<string, string>> data = new List<KeyValuePair<string, string>>();
            try
            {
                if (checkSelection)
                {
                    if (startProgram)
                    {

                        checkSelection = !checkSelection;
                    }
                }
                else
                {

                    if (!startProgram)
                    {
                        checkSelection = !checkSelection;
                    }
                }

                var cb = cbAcademyYear.SelectedValue;

                var obj = JObject.Parse(Properties.Settings.Default.schoolAcademyYear).ToObject<Teachers>().data.Where(y => y.name.Equals(cb.ToString()));
                foreach(var item in obj)
                {
                    schoolYearId = item.schoolyear;
                    titleYear = item.name;
                    foreach(var i in item.teaching_classes)
                    {
                        data.Add(new KeyValuePair<string, string>(i.name,i.id));
                    }
                }
                //tvAcademy.ItemsSource = obj;
                cbSelectClass.ItemsSource = data;
                cbSelectClass.DisplayMemberPath = "Key";
                cbSelectClass.SelectedValuePath = "Value";
                changeAcademyYear = true;
                year = cb.ToString();
                LabelTitle.Content = Properties.Langs.Lang.Message_Box_Stu_Result_Title;
                //tvAcademy.Visibility = Visibility.Visible;
                DockTree.Visibility = Visibility.Collapsed;
                tabcontrolResult.SelectedIndex = 1;
                tabcontrolScore.SelectedIndex = 1;
                this.IsEnabled = true;
            }
            catch
            {

            }
        }
        //............Button in slide left.......................
        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            if (MateriaSettingDrop.Visibility == Visibility.Visible)
            {
                //....setting.....
                MateriaSettingDrop.Visibility = Visibility.Collapsed;
                MateriaSettingUp.Visibility = Visibility.Visible;

                //..........Btn lang........
                btnLang.Visibility = Visibility.Visible;
                MateriaLangDrop.Visibility = Visibility.Visible;
                MateriaLangUp.Visibility = Visibility.Collapsed;

                //..........btn Color............
                btnColor.Visibility = Visibility.Visible;
                MateriaColorDrop.Visibility = Visibility.Visible;
                MateriaColorUp.Visibility = Visibility.Collapsed;

                //...........btn..................
                btnAbout.Visibility = Visibility.Visible;
            }
            else
            {
                 //....setting.....
                MateriaSettingDrop.Visibility = Visibility.Visible;
                MateriaSettingUp.Visibility = Visibility.Collapsed;

                //..........Btn lang........
                btnLang.Visibility = Visibility.Collapsed;
                MateriaLangDrop.Visibility = Visibility.Visible;
                MateriaLangUp.Visibility = Visibility.Collapsed;
                gridLanguage.Visibility = Visibility.Collapsed;

                //..........btn Color............
                btnColor.Visibility = Visibility.Collapsed;
                MateriaColorDrop.Visibility = Visibility.Visible;
                MateriaColorUp.Visibility = Visibility.Collapsed;

                //...........btn..................
                btnAbout.Visibility = Visibility.Collapsed;
            }
        }

        private void btnLang_Click(object sender, RoutedEventArgs e)
        {
            if (MateriaLangDrop.Visibility == Visibility.Visible)
            {
                gridLanguage.Visibility = Visibility.Visible;
                MateriaLangDrop.Visibility = Visibility.Collapsed;
                MateriaLangUp.Visibility = Visibility.Visible;

            }
            else
            {
                gridLanguage.Visibility = Visibility.Collapsed;
                MateriaLangDrop.Visibility = Visibility.Visible;
                MateriaLangUp.Visibility = Visibility.Collapsed;
            }
        }

        private void btnScore_Click(object sender, RoutedEventArgs e)
        {
            lblTitle.Content = Properties.Langs.Lang.score;
            var bc = new BrushConverter();
            btnScore.Background = Brushes.White;
            MateriaScore.Foreground = (Brush)bc.ConvertFrom("#1183CA");
            lblScore.Foreground = (Brush)bc.ConvertFrom("#1183CA");
        }

        private void btnScore_MouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            btnScore.Background = Brushes.White;
            MateriaScore.Foreground = (Brush)bc.ConvertFrom("#1183CA");
            lblScore.Foreground = (Brush)bc.ConvertFrom("#1183CA");
        }

        private void btnScore_MouseLeave(object sender, MouseEventArgs e)
        {
            if(tabcontrolResult.SelectedIndex == 1)
            {
                var bc = new BrushConverter();
                btnScore.Background = Brushes.White;
                MateriaScore.Foreground = (Brush)bc.ConvertFrom("#1183CA");
                lblScore.Foreground = (Brush)bc.ConvertFrom("#1183CA");
            }

            else
            {
                var bc = new BrushConverter();
                btnScore.Background = Brushes.Transparent;
                MateriaScore.Foreground = Brushes.White;
                lblScore.Foreground = Brushes.White;
            }
           
        }


        //.....................Button Language............
        private void Khmer_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Language = "km-KH";
            Properties.Settings.Default.Save();
            System.Threading.Thread.CurrentThread.CurrentUICulture =
            new System.Globalization.CultureInfo("km-KH");
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void English_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Language = "en-US";
            Properties.Settings.Default.Save();
            System.Threading.Thread.CurrentThread.CurrentUICulture =
            new System.Globalization.CultureInfo("en-US");
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void Chinese_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Language = "zh-Hans";
            Properties.Settings.Default.Save();
            System.Threading.Thread.CurrentThread.CurrentUICulture =
            new System.Globalization.CultureInfo("zh-Hans");
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
        string schoolYearId = "";
       /* private async void tvAcademy_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
           
            try
            {
                List<KeyValuePair<string, string>> data = new List<KeyValuePair<string, string>>();
                //checkMonthButton = true;
                var item = tvAcademy.SelectedItem as TeachingClass;
                //treeViewItemChange(item.id);
                classId = item.id;
                txtTitleClassName.Text = "ថ្នាក់ទី"+item.name;
                LabelTitle.Content = Properties.Langs.Lang.Message_Box_Stu_Result_Title_select_month;
                tabcontrolScore.SelectedIndex = 1;

                if (Teacher.InternetChecker()==true&&internet&&ping<=100)
                {
                    
                    string accessUrl = Properties.Settings.Default.acessUrl;
                    string token = Properties.Settings.Default.Token;
                    var respone = await RESTApiHelper.GetAll(accessUrl, "/get-teaching-subject-class/"+item.id, token);
                    Properties.Settings.Default.teachingSubject = respone;
                    Properties.Settings.Default.Save();
                    var obj = JObject.Parse(respone).ToObject<GetTeachingSubjectClass>().data;
                    treeViewItemChange(item.id);

                    var respone1 = await RESTApiHelper.GetAll(accessUrl, "/academic/" + classId + "/grade-time-shift", token);
                    var obj1 = JObject.Parse(respone1).ToObject<TimesButton>();
                    foreach (var item1 in obj1.data)
                    {
                        foreach (var month in item1.months)
                        {
                            data.Add(new KeyValuePair<string, string>(month.displayMonth, item1.semester));
                        }
                        data.Add(new KeyValuePair<string, string>(item1.name, item1.semester));
                    }
                    string[] str = Properties.Settings.Default.monthofTheAcademyYear.Split('|');
                    for(int i=0;i<str.Length-1;i++)
                    {
                        var d = JObject.Parse(str[i]).ToObject<TimesButton>();

                        if(d.classId!=classId)
                        {
                            obj1.classId = classId;
                            Properties.Settings.Default.monthofTheAcademyYear+=str[i]+"|";
                            Properties.Settings.Default.Save();
                        }
                    }

                    cbSelectMonth.ItemsSource = data;
                    cbSelectMonth.DisplayMemberPath = "Key";
                    cbSelectMonth.SelectedValuePath = "Value";
                    cbSelectResultMonth.ItemsSource = data;
                    cbSelectResultMonth.DisplayMemberPath = "Key";
                    cbSelectResultMonth.SelectedValuePath = "Value";

                    btnResultofTheYear1.Visibility = Visibility.Visible;
                    btnResultofTheYear.Visibility = Visibility.Visible;

                    cbSelectSubject.ItemsSource = obj;
                    cbSelectSubject.DisplayMemberPath = "name";
                    cbSelectSubject.SelectedValuePath = "id";
                    //Task<string> task = GetMonthlyResultFormApiAsync();
                    DockTree.Visibility = Visibility.Collapsed;
                    tabcontrolResult.SelectedIndex = 1;
                    tabcontrolScore.SelectedIndex = 1;

                    Task<string> task = GetMonthlyResultFormApiAsync();



                }

                else
                {
                    string respone = Properties.Settings.Default.teachingSubject;
                    var obj = JObject.Parse(respone).ToObject<GetTeachingSubjectClass>().data;
                    treeViewItemChange(item.id);

                    string[] respone1 = Properties.Settings.Default.monthofTheAcademyYear.Split('|');
                    TimesButton obj1 = new TimesButton();
                    for(int i = 0;i<respone1.Length-1;i++)
                    {
                        var d = JObject.Parse(respone1[i]).ToObject<TimesButton>();
                        if(d.classId==classId)
                        {
                            obj1 = d;
                        }
                    }

                    foreach (var item1 in obj1.data)
                    {
                        foreach (var month in item1.months)
                        {
                            data.Add(new KeyValuePair<string, string>(month.displayMonth, item1.semester));
                        }
                        data.Add(new KeyValuePair<string, string>(item1.name, item1.semester));
                    }
                    cbSelectMonth.ItemsSource = data;
                    cbSelectMonth.DisplayMemberPath = "Key";
                    cbSelectMonth.SelectedValuePath = "Value";
                    cbSelectResultMonth.ItemsSource = data;
                    cbSelectResultMonth.DisplayMemberPath = "Key";
                    cbSelectResultMonth.SelectedValuePath = "Value";


                    btnResultofTheYear1.Visibility = Visibility.Visible;
                    btnResultofTheYear.Visibility = Visibility.Visible;

                    cbSelectSubject.ItemsSource = obj;
                    cbSelectSubject.DisplayMemberPath = "name";
                    cbSelectSubject.SelectedValuePath = "id";
                    DockTree.Visibility = Visibility.Collapsed;
                    tabcontrolResult.SelectedIndex = 1;
                    tabcontrolScore.SelectedIndex = 1;
                    
                }
            }
            catch
            {
                //checkMonthButton = false;
                LabelTitle.Content = Properties.Langs.Lang.Message_Box_Stu_Result_Title;
               
            }
        }*/

        private void Vietnam_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Language = "vi-VN";


            Properties.Settings.Default.Save();
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxControl message = new MessageBoxControl();
            message.title = "ព័ត៌មាន";
            message.discription = "តើអ្នកពីតជាចង់ចាក់ចេញពី​ CAMEMIS DESKTOP​ មែនទេ?";
            this.IsEnabled = false;
            this.Opacity = 0.5;
            message.ShowDialog();
            this.Opacity = 1;
            this.IsEnabled = true;

            if (message.result == 1)
            {

                Properties.Settings.Default.checkLoginOrLogut = "logout";
                Properties.Settings.Default.Save();
                Login login = new Login();
                this.Close();
                login.Show();
            }
            else
            {
                Properties.Settings.Default.checkLoginOrLogut = "login";
                Properties.Settings.Default.Save();
            }
        }

        string SubjectId="",maxScore="";
        private async void cbSelectSubject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                List<KeyValuePair<string, string>> data = new List<KeyValuePair<string, string>>();
                var subject = cbSelectSubject.SelectedItem as TeachingSubject;
                lButton.Visibility = Visibility.Collapsed;
                SubjectId = subject.id;
                maxScore = subject.max_score;
                DockMonth.Visibility = Visibility.Collapsed;
                tabcontrolScore.SelectedIndex = 1;
                TilteSelection.Content = Properties.Langs.Lang.Message_Box_Stu_Result_Title_select_month;
                cbSelectMonth.Visibility = Visibility.Visible;
                if (Teacher.InternetChecker()&&internet)
                {
                    
                    string accessUrl = Properties.Settings.Default.acessUrl;
                    string token = Properties.Settings.Default.Token;
                    var respone = await RESTApiHelper.GetAll(accessUrl, "/academic/" + classId + "/grade-time-shift", token);
                    Properties.Settings.Default.monthofTheAcademyYear = respone;
                    Properties.Settings.Default.Save();
                    var obj = JObject.Parse(respone).ToObject<TimesButton>().data;
                    lButton.ItemsSource = obj;
                    foreach(var item in obj)
                    {
                        foreach(var month in item.months)
                        {
                            data.Add(new KeyValuePair<string, string>(month.displayMonth, item.semester));
                        }
                        data.Add(new KeyValuePair<string, string>(item.name, item.semester));
                    }

                    cbSelectMonth.ItemsSource = data;
                    cbSelectMonth.DisplayMemberPath = "Key";
                    cbSelectMonth.SelectedValuePath = "Value";
                    cbSelectResultMonth.ItemsSource = data;
                    cbSelectResultMonth.DisplayMemberPath = "Key";
                    cbSelectResultMonth.SelectedValuePath = "Value";
                    btnResultofTheYear.Visibility = Visibility.Visible;
                    btnResultofTheYear1.Visibility = Visibility.Visible;
                    DockMonth.Visibility = Visibility.Collapsed;
                    tabcontrolScore.SelectedIndex = 1;
                    TilteSelection.Content = Properties.Langs.Lang.Message_Box_Stu_Result_Title_select_month;

                }
                else
                {
                    string respone = Properties.Settings.Default.monthofTheAcademyYear;
                    var obj = JObject.Parse(respone).ToObject<TimesButton>().data;
                    foreach (var item in obj)
                    {
                        foreach (var month in item.months)
                        {
                            data.Add(new KeyValuePair<string, string>(month.displayMonth, item.semester));
                        }
                        data.Add(new KeyValuePair<string, string>(item.name, item.semester));
                    }
                    cbSelectMonth.ItemsSource = data;
                    cbSelectMonth.DisplayMemberPath = "Key";
                    cbSelectMonth.SelectedValuePath = "Value";
                    cbSelectResultMonth.ItemsSource = data;
                    cbSelectResultMonth.DisplayMemberPath = "Key";
                    cbSelectResultMonth.SelectedValuePath = "Value";
                    btnResultofTheYear.Visibility = Visibility.Visible;
                    btnResultofTheYear1.Visibility = Visibility.Visible;
                    DockMonth.Visibility = Visibility.Collapsed;
                    tabcontrolScore.SelectedIndex = 1;
                    TilteSelection.Content = Properties.Langs.Lang.Message_Box_Stu_Result_Title_select_month;
                }
            }
            catch { }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        //........................................................


        //-----------------Local function-------------------------
        private void treeViewItemChange(string name)
        {
            var reponse = Properties.Settings.Default.schoolAcademyYear;
            var obj = JObject.Parse(reponse).ToObject<Teachers>().data;
            foreach (var item in obj)
            {
                foreach (var Grade in item.teaching_classes)
                {
                    if(Grade.id==name)
                    {
                        Grade.color = "#1183CA";
                        break;
                    }
                }
            }
           /* tvAcademy.ItemsSource = null;
            tvAcademy.ItemsSource = obj.Where(y => y.name.Equals(year));*/
        }
        bool internet = true;
        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            //---------------Turn on turn off internet------------------
            if (Teacher.InternetChecker() == true)
            {
                if (btnCheck.IsChecked == true)
                {
                    txtxCheckinternet.Content = "Online";
                    internet = true;
                    wifiIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Wifi;
                }
                else
                {
                    txtxCheckinternet.Content = "Offline";
                    internet = false;
                    wifiIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WifiOff;
                }

            }
            else
            {
                btnCheck.IsChecked = false;
                this.Opacity = 0.5;
                MessageBoxControl message = new MessageBoxControl();
                message.title = "អ៊ិនធឺណែត";
                message.discription = "មិនមានការភ្ជាប់អ៊ិនធឺណែត";
                message.buttonType = 2;
                message.Owner = this;
                message.ShowDialog();
                this.Opacity = 1;
            }
            //-----------------------------------------------------------
        }
        string months = "";
        private async void btnMonths_Click(object sender, RoutedEventArgs e)
        {
 
            MessageBoxControl message = new MessageBoxControl();
            Loading loading = new Loading();
            tabcontrolScore.SelectedIndex = 0;
            stacButtonTop.Visibility = Visibility.Visible;
            try
            {
                
                string respone = "";
                var button = sender as Button;
                var month = DateChange.checkMonthString(button.Content.ToString());
                term = button.Tag.ToString();
                months = month.ToString();
                type = "1";
                message.title = "ដំណឹង";
                message.discription = "ទាញទិន្នន័យបានជោគជ័យ";
                message.buttonType = 2;
                loading.Show();
                stacButtonTop.Visibility = Visibility.Visible;
                string[] data=null;
               
                if (CheckFileExist(months) == false)
                {
                    if (Teacher.InternetChecker() && internet)
                    {
                        
                        respone = await SaveString(months);
                        data = respone.Split('|');
                        tabcontrolScore.SelectedIndex = 0;
                        stacButtonTop.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    respone = File.ReadAllText(filePath + "\\" + classId + " " + month + " " + SubjectId + ".txt");
                    data = respone.Split('|');
                }
                obj = JObject.Parse(data[0]).ToObject<InputScore>();
                foreach(var item in obj.data)
                {
                    if(item.score==null)
                    {
                        item.score = "0";
                        item.subject_score_max = maxScore;
                        item.subject_score_min = "0";
                    }
                }
                try
                {
                    if (obj.approve_learning_result.is_approved == "1")
                    {
                        txtApproveDate.Text = "កាលបរិច្ឆេទដែលបានអនុម័ត :" + obj.approve_learning_result.approved_date;
                        txtApproveDate.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        txtApproveDate.Visibility = Visibility.Collapsed;
                    }
                }
                catch
                {
                    txtApproveDate.Visibility = Visibility.Collapsed;
                }
                NumberList(obj.data);

                try
                {
                    var expireDate = JObject.Parse(data[1]).ToObject<MonthlyScoreExpire>().data;
                    txtExpireDate.Text = "ការកំណត់ថ្ងៃសុពលភាពនៃការបញ្ចូលពិន្ទុ : " + expireDate.expired_at;
                }catch
                {
                    txtExpireDate.Text = "ការកំណត់ថ្ងៃសុពលភាពនៃការបញ្ចូលពិន្ទុ : --";
                }
                if (obj.approve_learning_result.is_approved == "1")
                {
                    DGScoreMonth.IsEnabled = false;
                    btnPost.Visibility = Visibility.Collapsed;
                    btnDeleteAll.Visibility = Visibility.Collapsed;
                    btnSave.Visibility = Visibility.Collapsed;
                }
                else
                {
                    try
                    {
                        var expireDate = JObject.Parse(data[1]).ToObject<MonthlyScoreExpire>().data;
                        if (expireDate.is_expired == "True")
                        {
                            DGScoreMonth.IsEnabled = false;
                            btnPost.Visibility = Visibility.Collapsed;
                            btnDeleteAll.Visibility = Visibility.Collapsed;
                            btnSave.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            DGScoreMonth.IsEnabled = true;
                            btnPost.Visibility = Visibility.Visible;
                            btnDeleteAll.Visibility = Visibility.Visible;
                            btnSave.Visibility = Visibility.Visible;
                        }
                    }
                    catch
                    {
                        txtExpireDate.Text = "ការកំណត់ថ្ងៃសុពលភាពនៃការបញ្ចូលពិន្ទុ : --";
                        DGScoreMonth.IsEnabled = false;
                        btnPost.Visibility = Visibility.Collapsed;
                        btnDeleteAll.Visibility = Visibility.Collapsed;
                        btnSave.Visibility = Visibility.Collapsed;
                        File.Delete(filePath + "\\" + classId + " " + month + " " + SubjectId + ".txt");
                    }
                }
                
                DGScoreMonth.ItemsSource = null;
                DGScoreMonth.ItemsSource = obj.data;
                loading.Close();
                this.Opacity = 0.5;
                message.ShowDialog();
                this.Opacity = 1;
                buttonSelecte(button.Content.ToString());
            }
            catch {
                DGScoreMonth.ItemsSource = null;
                loading.Close();

                stacButtonTop.Visibility = Visibility.Collapsed;
                tabcontrolScore.SelectedIndex = 1;
                TilteSelection.Content = Properties.Langs.Lang.noresultdata;
            }
        }
        string type = "";
        private async Task<string> SaveString(string month)
        {
            string accessUrl = Properties.Settings.Default.acessUrl;
            string token = Properties.Settings.Default.Token;
            string respone = "",id="";
            string date = "";
            if (type == "1")
            {
                date = await GetExpireDateAsync(schoolYearId, type, term, months);
                respone = await RESTApiHelper.GetAll(accessUrl, "/academic/" + classId + "/monthly-subject-result?month=" + month + "&subject_id=" + SubjectId + "&type=1&term=" + term, token);
            }
            else if (type == "2")
            {
                date = await GetExpireDateAsync(schoolYearId, type, term);
                respone = await RESTApiHelper.GetAll(accessUrl, "/academic/" + classId + "/monthly-subject-result?subject_id=" + SubjectId + "&type=2&term=" + term, token);
            }
            
            var change = JObject.Parse(respone).ToObject<InputScore>();
            change.Datadate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            foreach(var item in change.data)
            {
                if(item.score==null)
                {
                    item.score = "0";

                }
                if (item.profileMedia.id == null)
                    id = item.student_id;
                else
                    id = item.profileMedia.id;
                if(!File.Exists(filePath + "\\" + id + ".jpg")){
                    SaveImage(id, ImageFormat.Jpeg, item.profileMedia.file_show);
                }
                item.profileMedia.file_show = filePath + "\\" + id + ".jpg";
            }

            string JsonString = JsonConvert.SerializeObject(change);

            using (StreamWriter writer = new StreamWriter(filePath + "\\" + classId + " " + month + " " + SubjectId + ".txt"))
            {
                writer.WriteLine(JsonString+"|"+date);
            }
            return JsonString+"|"+date;
        }

        private void isCheck_Click(object sender, RoutedEventArgs e)
        {
            if (btnCheckAutoSave.IsChecked == true)
            {
                string JsonString = JsonConvert.SerializeObject(obj);
                saveLocalString(months, JsonString, true);
            }
            var item = DGScoreMonth.SelectedItem as StudentInformation;
            item.visible = "Collapsed";
            DGScoreMonth.ItemsSource = null;
            DGScoreMonth.ItemsSource = obj.data;
        }

        private async void btnPost_Click(object sender, RoutedEventArgs e)
        {
            Loading load = new Loading();
            this.IsEnabled = false;
            load.Show();
            string accessUrl = Properties.Settings.Default.acessUrl;
            string token = Properties.Settings.Default.Token;
            MessageBoxControl message = new MessageBoxControl();
            MessageBoxControl message1 = new MessageBoxControl();
            if(Teacher.InternetChecker()==true&&internet)
            {
                message.title = "ត្រួតពិនិត្យអ៊ីនធឺណែត";
                message.discription = "ល្បឿនអ៊ីនរបស់អ្នក "+txtPing.Text+"\n"+"តើអ្នកចង់បញ្ចូនទិន្នន័យពេលនេះទេ?";
                this.Opacity = 0.5;
                message.ShowDialog();
                this.Opacity = 1;
                if (message.result==1)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Add(
                       new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        using (HttpResponseMessage res = client.PostAsJsonAsync(accessUrl + "/academic/" + classId + "/monthly-subject-result", new InputScore { month = months, subject_id = SubjectId, term = term, type = "1", data = obj.data }).Result)
                        {
                            using (HttpContent content = res.Content)
                            {
                                string datas = await content.ReadAsStringAsync();
                                var obj = JObject.Parse(datas).ToObject<InputScore>();
                                message1.title = "ការបញ្ចូនទិន្នន័យ";
                                if (obj.message.Equals("data error"))
                                    message1.discription = "ទិន្នន័យរបស់អ្នកមាន​បញ្ហា សូមត្រួតពិនិត្យឡើងវិញ";
                                else if (obj.message.Equals("true"))
                                {
                                    File.Delete(filePath + "\\" + classId + " " + months + " " + SubjectId + ".txt");
                                    message1.discription = "ការបញ្ជូនទិន្នន័យបានជោគជ័យ";
                                    DGScoreMonth.ItemsSource = null;
                                }
                                else if (obj.message.Equals("false"))
                                    message1.discription = "ការបញ្ជូនទិន្នន័យមិនបានជោគជ័យ";
                                else if (obj.message.Equals("fail"))
                                    message1.discription = "ការបញ្ជូនទិន្នន័យមិនបានជោគជ័យ";
                                message1.buttonType = 2;
                            }
                        }
                    }
                }
            }
            else
            {
                message1.title = "អ៊ីនធឺណែត";
                message1.discription = "មិនមានការតភ្ជាប់អ៊ីនធឺណែត";
                message1.buttonType = 2;
            }
            this.IsEnabled = true;
            load.Close();
            message1.Owner = this;
            this.Opacity = 0.5;
            message1.ShowDialog();
            this.Opacity = 1;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Loading loading = new Loading();
            loading.Show();
            this.IsEnabled = false;
            obj.Datadate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            string JsonString = JsonConvert.SerializeObject(obj);
            saveLocalString(months, JsonString,true);
            //Console.WriteLine(JsonString);
            this.IsEnabled = true;
            loading.Close();
        }

        private void btnDelect_Click(object sender, RoutedEventArgs e)
        {
            this.Opacity = 0.5;
            MessageBoxControl message = new MessageBoxControl();
            message.title = "ការលុបទិន្នន៏យ";
            message.discription = "តើអ្នកចង់លុបទិន្នន័យនេះមែនទេ?";
            message.ShowDialog();
            if(message.result==1)
            {
                var button = DGScoreMonth.SelectedItem as StudentInformation;

                button.absent_exam = false;
                button.score = null;
                button.teacher_comment = null;

                DGScoreMonth.ItemsSource = null;
                DGScoreMonth.ItemsSource = obj.data;
            }
            this.Opacity = 1;
            GC.Collect();
        }

        private void btnDeleteAll_Click(object sender, RoutedEventArgs e)
        {
            this.Opacity = 0.5;
            MessageBoxControl message = new MessageBoxControl();
            message.title = "ការលុបទិន្នន៏យ";
            message.discription = "តើអ្នកចង់លុបទិន្នន័យទាំងអស់មែនទេ?";
            message.ShowDialog();
            if(message.result == 1)
            {
                foreach (var item in obj.data)
                {
                    item.absent_exam = false;
                    item.score = null;
                    item.teacher_comment = null;
                }
                if(btnCheckAutoSave.IsChecked==true)
                {
                    string JsonString = JsonConvert.SerializeObject(obj);
                    saveLocalString(months, JsonString, true);
                }
                DGScoreMonth.ItemsSource = null;
                DGScoreMonth.ItemsSource = obj.data;
            }
            this.Opacity = 1;
            GC.Collect();
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var item = DGScoreMonth.SelectedItem as StudentInformation;
            try
            {
                if (item.score == "")
                {
                    item.visible = "Visible";
                }
                else
                {
                    if (int.Parse(item.score) > int.Parse(item.subject_score_max) || int.Parse(item.score) < int.Parse(item.subject_score_min))
                    {
                        item.visible = "Visible";
                        item.color = "Red";
                    }
                    else
                    {
                        item.visible = "Collapsed";
                        item.color = "Blue";
                    }
                        
                }
                if (btnCheckAutoSave.IsChecked == true)
                {
                    string JsonString = JsonConvert.SerializeObject(obj);
                    saveLocalString(months, JsonString, false);
                }
                DGScoreMonth.ItemsSource = null;
                DGScoreMonth.ItemsSource = obj.data;
            }
            catch
            {

            }
            GC.Collect();
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            clearAllSelection();
            cbAcademyYear.Text = "សូមជ្រើសរើសឆ្នាំសិក្សា";
        }

        void clearAllSelection()
        {
            btnLearningResult.IsEnabled = false;
            cbSelectClass.ItemsSource = null;
            cbSelectMonth.ItemsSource = null;
            cbSelectSubject.ItemsSource = null;
            cbSelectClass.Text = "សូមជ្រើសរើសថ្នាក់";
            cbSelectMonth.Text = "សូមជ្រើសរើសឆ្នាំខែ/ឆមាស";
            cbSelectSubject.Text = "សូមជ្រើសរើសមុខវិជ្ជា";

            Selectresult.Visibility = Visibility.Collapsed;
            cbSelectClass.Visibility = Visibility.Collapsed;
            cbSelectMonth.Visibility = Visibility.Collapsed;
            cbSelectSubject.Visibility = Visibility.Collapsed;

            DockTree.Visibility = Visibility.Collapsed;
            tabcontrolResult.SelectedIndex = 1;
            tabcontrolScore.SelectedIndex = 1;
            TilteSelection.Content = Properties.Langs.Lang.Message_Box_Stu_Result_Title_select_year;

            var bc = new BrushConverter();
            btnLearningResult.Background = (Brush)bc.ConvertFrom("#66D3D3D3");
            btnInputScore.Background = Brushes.White;
            this.IsEnabled = true;
            tabcontrolMenu.SelectedIndex = 0;
            tabcontrolResult.SelectedIndex = 1;
            tabcontrolResulandInput.SelectedIndex = 0;
            tabcontrolScore.SelectedIndex = 1;
            DockMonth.Visibility = Visibility.Collapsed;
            btnInputScore.Background = Brushes.White;
            lblTitle.Content = Properties.Langs.Lang.score;
            btnSave.Visibility = Visibility.Collapsed;
            btnPost.Visibility = Visibility.Collapsed;
            btnDeleteAll.Visibility = Visibility.Collapsed;
            btnPrint.Visibility = Visibility.Collapsed;
            btnLearningResult.IsEnabled = false;
        }

        private bool CheckFileExist(string month)
        {
            MessageBoxControl message = new MessageBoxControl();
            message.title = "ការទាញទិន្នន័យ";
            message.discription = "មិនមានទិន្នន័យរក្សាទុក សូមប្រើប្រាស់អ៊ីនធឺណេតដើម្បីទាញទិន្នន័យថ្មី";
            message.buttonType = 2;
            if (File.Exists(filePath + "\\" + classId + " " + month + " " + SubjectId + ".txt"))
            {
                
                return true;
            }
                
            else
            {
                this.Opacity = 0.5;
                message.ShowDialog();
                this.Opacity = 1;
                return false;
            }
               
        }

        //..........................Control Text Input Score........................
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {          
            var item = DGScoreMonth.SelectedItem as StudentInformation;
            int r;
            e.Handled = !int.TryParse(e.Text,out r);
        }
        //................................End.......................................


        //......................Button Sum in Textbox Score.....................
         private void btnSum_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var item = DGScoreMonth.SelectedItem as StudentInformation;

            if(item.score==null)
            {
                item.score = "0";
            }
            else
            {
                if (int.Parse(item.score) > int.Parse(item.subject_score_max) - 1)
                {
                    item.score = item.subject_score_max;
                }
                else
                {
                    if (string.IsNullOrEmpty(item.score))
                    {
                        item.score = 0.ToString();
                    }
                    bool success = Int32.TryParse(item.score, out var number);
                    if (!success)
                    {
                        // show error
                        return;
                    }

                    item.score = btn.Name == "UpBtn" ? (++number).ToString() : (++number).ToString();
                }
            }
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           

            var btn = sender as Button;
            var item = DGScoreMonth.SelectedItem as StudentInformation;


            try
            {
                if (int.Parse(item.score) < int.Parse(item.subject_score_min) + 1)
                {
                    if (item.score == null)
                    {
                        item.score = "0";
                    }
                    else
                    {
                        item.score = item.subject_score_min;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(item.score.Trim()))
                    {
                        item.score = 0.ToString();
                    }
                    bool success = Int32.TryParse(item.score, out var number);
                    if (!success)
                    {
                        // show error
                        return;
                    }

                    item.score = btn.Name == "DownBtn" ? (--number).ToString() : (--number).ToString();
                }
            }
            catch
            {
                item.score = " ";
            }
           
        }

        private async void btnSemester_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxControl message = new MessageBoxControl();
            Loading loading = new Loading();
            tabcontrolScore.SelectedIndex = 0;
            stacButtonTop.Visibility = Visibility.Visible;
            try
            {

                string respone = "";
                var button = sender as Button;
                term = button.Tag.ToString();
                type = "2";
                message.title = "ដំណឹង";
                message.discription = "ទាញទិន្នន័យបានជោគជ័យ";
                message.buttonType = 2;
                loading.Show();
                stacButtonTop.Visibility = Visibility.Visible;
                string[] data = null;
                if (CheckFileExist(term) == false)
                {
                    if (Teacher.InternetChecker() && internet)
                    {
                        respone = await SaveString(months);
                        data = respone.Split('|');
                        tabcontrolScore.SelectedIndex = 0;
                        stacButtonTop.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    respone = File.ReadAllText(filePath + "\\" + classId + " " + term + " " + SubjectId + ".txt");
                    data = respone.Split('|');
                }
                obj = JObject.Parse(data[0]).ToObject<InputScore>();
                txtDataDate.Text = "កាលបរិច្ឆេទរបស់ទិន្នន័យ : "+obj.Datadate;
                foreach (var item in obj.data)
                {
                    if (item.score == null)
                    {
                        item.score = "0";
                        item.subject_score_max = maxScore;
                        item.subject_score_min = "0";
                    }
                }
                NumberList(obj.data);
                try
                {
                    var expireDate = JObject.Parse(data[1]).ToObject<MonthlyScoreExpire>().data;
                    txtExpireDate.Text = "ការកំណត់ថ្ងៃសុពលភាពនៃការបញ្ចូលពិន្ទុ : " + expireDate.expired_at;
                    if (expireDate.is_expired == "True")
                    {
                        DGScoreMonth.IsEnabled = false;
                        btnPost.Visibility = Visibility.Collapsed;
                        btnDeleteAll.Visibility = Visibility.Collapsed;
                        btnSave.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        DGScoreMonth.IsEnabled = true;
                        btnPost.Visibility = Visibility.Visible;
                        btnDeleteAll.Visibility = Visibility.Visible;
                        btnSave.Visibility = Visibility.Visible;
                    }
                }
                catch
                {
                    txtExpireDate.Text = "ការកំណត់ថ្ងៃសុពលភាពនៃការបញ្ចូលពិន្ទុ : មិនមានការកំណត់";
                }
                DGScoreMonth.ItemsSource = null;
                DGScoreMonth.ItemsSource = obj.data;
                loading.Close();
                this.Opacity = 0.5;
                message.ShowDialog();
                this.Opacity = 1;
                buttonSelecte(button.Content.ToString());
            }
            catch
            {
                DGScoreMonth.ItemsSource = null;
                loading.Close();

                stacButtonTop.Visibility = Visibility.Collapsed;
                tabcontrolScore.SelectedIndex = 1;
                TilteSelection.Content = Properties.Langs.Lang.noresultdata;
            }
        }

        private void btnMonthsResult_Click(object sender, RoutedEventArgs e)
        {
            btnSaveSemester.Visibility = Visibility.Collapsed;
            btnPostSemester.Visibility = Visibility.Collapsed;
            MessageBoxControl message = new MessageBoxControl();
            Loading loading = new Loading();
            tabcontrolLearn1.SelectedIndex = 0;
            Selectresult.Visibility = Visibility.Collapsed;
            var obj = new List<StudentMonthlyResult>();
            try
            {

                string respone = "";
                var button = sender as Button;
                var month = DateChange.checkMonthString(button.Content.ToString());
                term = button.Tag.ToString();
                months = month.ToString();
                message.title = "ដំណឹង";
                message.discription = "ទាញទិន្នន័យបានជោគជ័យ";
                message.buttonType = 2;
                loading.Show();
                Selectresult.Visibility = Visibility.Collapsed;
                tabcontrolLearn1.SelectedIndex = 1;

                obj = GetData(month.ToString());

                foreach (var item in obj)
                {
                    item.localProfileLink = filePath + "\\" + item.profileMedia.id + ".jpg";
                }

                NumberList(obj,"1");
                DGMonthlyResult.ItemsSource = null;
                DGMonthlyResult.ItemsSource = obj;
                loading.Close();
                this.Opacity = 0.5;
                message.ShowDialog();
                this.Opacity = 1;
                Selectresult.Visibility = Visibility.Collapsed;
                buttonSelecte1(button.Content.ToString());
            }
            catch
            {
                DGMonthlyResult.ItemsSource = null;
                loading.Close();
                tabcontrolLearn1.SelectedIndex = 0;
                Selectresult.Visibility = Visibility.Collapsed;
                lblErrandSelect.Content = Properties.Langs.Lang.noresultdata;
            }
        }



        List<Morality> moralities = new List<Morality>();
        string nameSemester = "";
        private void btnSemesterResult_Click(object sender, RoutedEventArgs e)
        {
            btnSaveSemester.Visibility = Visibility.Collapsed;
            btnPostSemester.Visibility = Visibility.Collapsed;
            MessageBoxControl message = new MessageBoxControl();
            Loading loading = new Loading();
            tabcontrolLearn1.SelectedIndex = 2;
            Selectresult.Visibility = Visibility.Collapsed;
            var obj = new List<StudentMonthlyResult>();
            try
            {
                List<Morality> moralities1 = new List<Morality>();
                string respone = "",date="";
                var button = sender as Button;
                nameSemester = button.Content.ToString();
                message.title = "ដំណឹង";
                message.discription = "ទាញទិន្នន័យបានជោគជ័យ";
                message.buttonType = 2;
                loading.Show();
                Selectresult.Visibility = Visibility.Collapsed;
                tabcontrolLearn1.SelectedIndex = 1;

                obj = GetData("", "semester", button.Content.ToString());
                NumberList(obj,"2");

                if(!File.Exists(filePath + "\\" + classId + " " + nameSemester + ".txt"))
                {
                    foreach (var item in obj)
                    {
                        item.localProfileLink = filePath + "\\" + item.profileMedia.id + ".jpg";
                        moralities1.Add(new Morality
                        {
                            number = item.numbers,
                            avg_score = item.result_semester.avg_score,
                            gender = item.gender,
                            id = item.result_semester.id,
                            name = item.name,
                            rank = item.result_semester.rank,
                            bangkeun_phal = item.result_semester.bangkeun_phal,
                            morality = item.result_semester.morality,
                            health = item.result_semester.health,
                            grading = item.result_semester.grading,
                            profile= item.localProfileLink
                        });
                    }
                    date = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    moralities = moralities1;
                }
                else
                {
                    var responeString = File.ReadAllText(filePath + "\\" + classId + " " + nameSemester + ".txt");
                    moralities = JObject.Parse(responeString).ToObject<ListMorality>().data;
                    date = JObject.Parse(responeString).ToObject<ListMorality>().date;
                }
                txtDate.Text = "កាលបរិច្ឆេទ​ : " +date;
                DGSemester.ItemsSource = null;
                DGSemesterExam.ItemsSource = null;
                DGSemesterClass.ItemsSource = null;
                DGSemester.ItemsSource = obj;
                DGSemesterExam.ItemsSource = obj;
                DGSemesterClass.ItemsSource = moralities.OrderBy(s=>s.rank);
                loading.Close();
                this.Opacity = 0.5;
                message.ShowDialog();
                this.Opacity = 1;
                tabcontrolLearn1.SelectedIndex = 2;
                Selectresult.Visibility = Visibility.Visible;
                buttonSelecte1(button.Content.ToString());
            }
            catch
            {
                DGMonthlyResult.ItemsSource = null;
                loading.Close();
                tabcontrolLearn1.SelectedIndex = 0;
                Selectresult.Visibility = Visibility.Collapsed;
                lblErrandSelect.Content = Properties.Langs.Lang.noresultdata;
            }
            Selectresult.Visibility = Visibility.Visible;
        }

        private void btnResultofTheYear1_Click(object sender, RoutedEventArgs e)
        {
            /*MessageBoxControl message = new MessageBoxControl();
            Loading loading = new Loading();
            tabcontrolLearn1.SelectedIndex = 2;
            Selectresult.Visibility = Visibility.Collapsed;
            var obj = new List<StudentMonthlyResult>();
            try
            {

                string respone = "";
                var button = sender as Button;

                message.title = "ដំណឹង";
                message.discription = "ទាញទិន្នន័យបានជោគជ័យ";
                message.buttonType = 2;
                loading.Show();
                Selectresult.Visibility = Visibility.Collapsed;
                tabcontrolLearn1.SelectedIndex = 1;

                obj = GetData("", "Year", button.Content.ToString());

                NumberList(obj, "2");
                DGYear.ItemsSource = obj;
                loading.Close();
                this.Opacity = 0.5;
                message.ShowDialog();
                this.Opacity = 1;
                tabcontrolLearn1.SelectedIndex = 2;
                Selectresult.Visibility = Visibility.Visible;

            }
            catch
            {
                DGYear.ItemsSource = null;
                loading.Close();
                tabcontrolLearn1.SelectedIndex = 0;
                Selectresult.Visibility = Visibility.Collapsed;
                lblErrandSelect.Content = Properties.Langs.Lang.noresultdata;
            }
            Selectresult.Visibility = Visibility.Collapsed;*/
        }
        private void DGScoreMonth_PreviewKeyDown(object sender, KeyEventArgs e)
        {
           
        }

       

        //............................End..................................................
        private void saveLocalString(string month, string respone,bool checkAutosave)
        {
            MessageBoxControl message = new MessageBoxControl();
            try
            {
                
                using (StreamWriter writer = new StreamWriter(filePath + "\\" + classId + " " + month + " " + SubjectId + ".txt"))
                {
                    writer.WriteLine(respone);
                }
               if(checkAutosave)
                {
                    this.Opacity = 0.5;
                    message.title = "ដំណឹង";
                    message.discription = "ការរក្សាទុកបានជោគជ័យ";
                    message.buttonType = 2;
                    message.ShowDialog();
                    this.Opacity = 1;
                }
            }
            catch
            {
                this.Opacity = 0.5;
                message.title = "ដំណឹង";
                message.discription = "ការរក្សាទុកមិនបានជោគជ័យ";
                message.buttonType = 1;
                message.ShowDialog();
                this.Opacity = 1;
            }
        }
        private void NumberList(List<StudentInformation> obj)
        {
            int i = 1;
            foreach (var item in obj)
            {
                if (item.gender == "1")
                    item.gender = "ប្រុស";
                else
                    item.gender = "ស្រី";
                item.number = DateChange.Num(i);
                item.tabIndex = 1000 + i;
                if (i == 2)
                    item.focus = true;
                else
                    item.focus = false;
                i++;
            }
        }
        private void NumberList(List<StudentMonthlyResult> obj,string title)
        {
            int i = 1;
            if(title=="1")
            {
                foreach (var item in obj)
                {
                    if (item.gender == "1")
                        item.gender = "ប្រុស";
                    else
                        item.gender = "ស្រី";
                    if (item.result_monthly.avg_score == "0")
                    {
                        item.result_monthly.avg_score = "មិនចាត់ថ្នាក់";
                        item.result_monthly.color = "Red";
                    }
                    else
                    {
                        item.result_monthly.color = "Blue";
                    }

                    item.numbers = DateChange.Num(i);
                    i++;
                }
            }
            else if(title=="3")
            {
                foreach (var item in obj)
                {
                    if (item.gender == "1")
                        item.gender = "ប្រុស";
                    else
                        item.gender = "ស្រី";
                    if (item.result_monthly.avg_score == "0")
                    {
                        item.result_monthly.avg_score = "មិនចាត់ថ្នាក់";
                        item.result_monthly.color = "Red";
                    }
                    else
                    {
                        item.result_monthly.color = "Blue";
                    }

                    item.numbers = DateChange.Num(i);
                    i++;
                }
            }
            else
            {
                foreach (var item in obj)
                {
                    if (item.gender == "1")
                        item.gender = "ប្រុស";
                    else
                        item.gender = "ស្រី";
                    if (item.result_semester_exam.avg_score == "0")
                    {
                        item.result_semester_exam.avg_score = "មិនចាត់ថ្នាក់";
                        item.result_semester_exam.color = "Red";
                    }
                    else
                    {
                        item.result_semester_exam.color = "Blue";
                    }
                    if (item.result_semester.is_fail == "0")
                        item.result_semester.is_fail = "ជាប់";
                    else
                        item.result_semester.is_fail = "ធ្លាក់";
                    if (item.result_semester_exam.is_fail == "0")
                        item.result_semester_exam.is_fail = "ជាប់";
                    else
                        item.result_semester_exam.is_fail = "ធ្លាក់";
                    if (item.result_semester.morality == "ល្អ")
                        item.result_semester.morality = "0";
                    else if (item.result_semester.morality == "ល្អបង្គួរ")
                        item.result_semester.morality = "1";
                    else if (item.result_semester.morality == "មធ្យម")
                        item.result_semester.morality = "2";
                    else if (item.result_semester.morality == "ខ្សោយ")
                        item.result_semester.morality = "3";
                    else if (item.result_semester.morality == "មិនចាត់ថ្នាក់")
                        item.result_semester.morality = "4";

                    if (item.result_semester.health == "ល្អ")
                        item.result_semester. health = "0";
                    else if (item.result_semester.health == "ល្អបង្គួរ")
                        item.result_semester.health = "1";
                    else if (item.result_semester.health == "មធ្យម")
                        item.result_semester.health = "2";
                    else if (item.result_semester.health == "ខ្សោយ")
                        item.result_semester.health = "3";
                    else if (item.result_semester.health == "មិនចាត់ថ្នាក់")
                        item.result_semester.health = "4";

                    if (item.result_semester.bangkeun_phal == "ល្អ")
                        item.result_semester.bangkeun_phal = "0";
                    else if (item.result_semester.bangkeun_phal == "ល្អបង្គួរ")
                        item.result_semester.bangkeun_phal = "1";
                    else if (item.result_semester.bangkeun_phal == "មធ្យម")
                        item.result_semester.bangkeun_phal = "2";
                    else if (item.result_semester.bangkeun_phal == "ខ្សោយ")
                        item.result_semester.bangkeun_phal = "3";
                    else if (item.result_semester.bangkeun_phal == "មិនចាត់ថ្នាក់")
                        item.result_semester.bangkeun_phal = "4";
                    item.numbers = DateChange.Num(i);
                    i++;
                }
            }
        }
        // Fuck all you guys
        //--------------------------------------------------------

        //---------------button focus--------------
        private void buttonSelecte(string name)
        {
            var reponse = Properties.Settings.Default.monthofTheAcademyYear;
            var obj = JObject.Parse(reponse).ToObject<TimesButton>().data;

            foreach (var item in obj)
            {
                if (item.name == name)
                {
                    item.colors = "White";
                }
                else
                {
                    item.colors = "WhiteSmoke";
                }
                foreach (var i in item.months)
                {
                    if (i.displayMonth == name)
                    {
                        i.color = "White";
                    }
                    else
                    {
                        i.color = "WhiteSmoke";
                    }
                }
            }
            lButton.ItemsSource = null;
            lButton.ItemsSource = obj;
        }
        private void buttonSelecte1(string name)
        {
            var reponse = Properties.Settings.Default.monthofTheAcademyYear;
            var obj = JObject.Parse(reponse).ToObject<TimesButton>().data;

            foreach (var item in obj)
            {
                if (item.name == name)
                {
                    item.colors = "White";
                }
                else
                {
                    item.colors = "WhiteSmoke";
                }
                foreach (var i in item.months)
                {
                    if (i.displayMonth == name)
                    {
                        i.color = "White";
                    }
                    else
                    {
                        i.color = "WhiteSmoke";
                    }
                }
            }
            lButton1.ItemsSource = null;
            lButton1.ItemsSource = obj;
        }

        private async Task<string> GetExpireDateAsync(string schoolYearId,string type,string semester,string month="")
        {
            string accessUrl = Properties.Settings.Default.acessUrl;
            string token = Properties.Settings.Default.Token;
            string respone = "";

            if(type=="1")
            {
                respone = await RESTApiHelper.GetAll(accessUrl, "/monthly-score-enter-expire/" + schoolYearId + "?type=" + type + "&semester=" + semester + "&month=" + month, token);
            }
            else if(type=="2")
            {
                respone = await RESTApiHelper.GetAll(accessUrl, "/monthly-score-enter-expire/" + schoolYearId + "?type=" + type + "&semester=" + semester, token);
            }

            return respone;
        }

        //......................................Button Result.....................
        private void btnExamSemester_Click(object sender, RoutedEventArgs e)
        {
            txtTitleMonth.Text = "លទ្ធផលប្រលង"+monthName;
            tabcontrolLearn1.SelectedIndex = 5;
            btnSaveSemester.Visibility = Visibility.Collapsed;
            btnPostSemester.Visibility = Visibility.Collapsed;
        }

        private void btnClassification_Click(object sender, RoutedEventArgs e)
        {
            txtTitleMonth.Text = "ចំណាត់ថ្នាក់ចំណាត់ប្រភេទ"+monthName;
            tabcontrolLearn1.SelectedIndex = 4;
            btnSaveSemester.Visibility = Visibility.Visible;
            btnPostSemester.Visibility = Visibility.Visible;
        }

        private void btnResultSemester_Click(object sender, RoutedEventArgs e)
        {
            txtTitleMonth.Text = "លទ្ធផលប្រចាំ"+monthName;
            tabcontrolLearn1.SelectedIndex = 2;
            btnSaveSemester.Visibility = Visibility.Collapsed;
            btnPostSemester.Visibility = Visibility.Collapsed;
        }
        //.........................End..............
        static public string DecodeFrom64(string encodedData)
        {
            byte[] encodedDataAsBytes
                = System.Convert.FromBase64String(encodedData);
            string returnValue =
               System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
            return returnValue;
        }
        static public string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes
                  = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue
                  = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }
        private bool IsImageHave(string studentId)
        {
            string path = filePath + "\\" + studentId + ".jpg";
            if (File.Exists(path))
            {
                return true;
            }
            return false;
        }

        private void btnSaveSemester_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxControl message = new MessageBoxControl();
            try
            {
                var data = new ListMorality();
                data.data = moralities;
                data.date = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                string JsonString = JsonConvert.SerializeObject(data);
                using (StreamWriter writer = new StreamWriter(filePath + "\\" + classId +" "+ nameSemester + ".txt"))
                {
                    writer.WriteLine(JsonString);
                }
                message.discription = "ការរក្សាទុកបានជោគជ័យ";
            }
            catch
            {
                message.discription = "ការរក្សាទុកមិនបានជោគជ័យ";
            }
            this.Opacity = 0.5;
            message.title = "ដំណឹង";
            message.buttonType = 1;
            message.ShowDialog();
            this.Opacity = 1;
        }

        private async void btnPostSemester_Click(object sender, RoutedEventArgs e)
        {
            var data = moralities;
            foreach(var item in data)
            {
                if (item.morality == "0")
                {
                    item.morality = "ល្អ";
                }
                else if (item.morality == "1")
                {
                    item.morality = "ល្អបង្គួរ";
                }
                else if (item.morality == "2")
                {
                    item.morality = "មធ្យម";
                }
                else if (item.morality == "3")
                {
                    item.morality = "ខ្សោយ";
                }
                else
                {
                    item.morality = "មិនចាត់ថ្នាក់";
                }

                if (item.health == "0")
                {
                    item.health = "ល្អ";
                }
                else if (item.health == "1")
                {
                    item.health = "ល្អបង្គួរ";
                }
                else if (item.health == "2")
                {
                    item.health = "មធ្យម";
                }
                else if (item.health == "3")
                {
                    item.health = "ខ្សោយ";
                }
                else
                {
                    item.health = "មិនចាត់ថ្នាក់";
                }

                if (item.bangkeun_phal == "0")
                {
                    item.bangkeun_phal = "ល្អ";
                }
                else if (item.bangkeun_phal == "1")
                {
                    item.bangkeun_phal = "ល្អបង្គួរ";
                }
                else if (item.bangkeun_phal == "2")
                {
                    item.bangkeun_phal = "មធ្យម";
                }
                else if (item.bangkeun_phal == "3")
                {
                    item.bangkeun_phal = "ខ្សោយ";
                }
                else
                {
                    item.bangkeun_phal = "មិនចាត់ថ្នាក់";
                }
            }
            string accessUrl = Properties.Settings.Default.acessUrl;
            string token = Properties.Settings.Default.Token;
            MessageBoxControl message = new MessageBoxControl();
            MessageBoxControl message1 = new MessageBoxControl();
            if (Teacher.InternetChecker() == true && internet)
            {
                message.title = "ត្រួតពិនិត្យអ៊ីនធឺណែត";
                message.discription = "ល្បឿនអ៊ីនរបស់អ្នក " + txtPing.Text + "\n" + "តើអ្នកចង់បញ្ចូនទិន្នន័យពេលនេះទេ?";
                this.Opacity = 0.5;
                message.ShowDialog();
                this.Opacity = 1;
                if (message.result == 1)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Add(
                       new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        using (HttpResponseMessage res = client.PostAsJsonAsync(accessUrl + "/save-semester-morality/"+classId, new ListMorality { data=data }).Result)
                        {
                            using (HttpContent content = res.Content)
                            {
                                string datas = await content.ReadAsStringAsync();
                                var obj = JObject.Parse(datas).ToObject<ListMorality>();
                                message1.title = "ការបញ្ចូនទិន្នន័យ";
                                if (obj.data.Count == 0)
                                {
                                    message1.discription = "ការបញ្ចូនទិន្នន័យបានជោគជ័យ";
                                    SwitchData(data);
                                    DGSemesterClass.ItemsSource = null;
                                    DGSemesterClass.ItemsSource = data;
                                    File.Delete(filePath + "\\" + classId + " " + nameSemester + ".txt");
                                }
                                else
                                {
                                    message1.discription = "ការបញ្ចូនទិន្នន័យមិនបានជោគជ័យ";
                                }
                                message1.buttonType = 2;
                            }
                        }
                    }
                }
            }
            else
            {
                message1.title = "អ៊ីនធឺណែត";
                message1.discription = "មិនមានការតភ្ជាប់អ៊ីនធឺណែត";
                message1.buttonType = 2;
            }

            message1.Owner = this;
            this.Opacity = 0.5;
            message1.ShowDialog();
            this.Opacity = 1;
        }
        void SwitchData(List<Morality> data)
        {
            foreach (var item in data)
            {
                if (item.morality == "ល្អ")
                {
                    item.morality = "0";
                }
                else if (item.morality == "ល្អបង្គួរ")
                {
                    item.morality = "1";
                }
                else if (item.morality == "មធ្យម")
                {
                    item.morality = "2";
                }
                else if (item.morality == "ខ្សោយ")
                {
                    item.morality = "3";
                }
                else
                {
                    item.morality = "4";
                }

                if (item.health == "ល្អ")
                {
                    item.health = "0";
                }
                else if (item.health == "ល្អបង្គួរ")
                {
                    item.health = "1";
                }
                else if (item.health == "មធ្យម")
                {
                    item.health = "2";
                }
                else if (item.health == "ខ្សោយ")
                {
                    item.health = "3";
                }
                else
                {
                    item.health = "4";
                }

                if (item.bangkeun_phal == "ល្អ")
                {
                    item.bangkeun_phal = "0";
                }
                else if (item.bangkeun_phal == "ល្អបង្គួរ")
                {
                    item.bangkeun_phal = "1";
                }
                else if (item.bangkeun_phal == "មធ្យម")
                {
                    item.bangkeun_phal = "2";
                }
                else if (item.bangkeun_phal == "ខ្សោយ")
                {
                    item.bangkeun_phal = "3";
                }
                else
                {
                    item.bangkeun_phal = "4";
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(btnCheckAutoSave.IsChecked==true)
            {
                try
                {
                    var data = new ListMorality();
                    data.data = moralities;
                    data.date = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    string JsonString = JsonConvert.SerializeObject(data);
                    using (StreamWriter writer = new StreamWriter(filePath + "\\" + classId + " " + nameSemester + ".txt"))
                    {
                        writer.WriteLine(JsonString);
                    }
                }
                catch
                {

                }
            }
        }
        string title = "",label="",titleYear="";
        private async void cbSelectMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var item = sender as ComboBox;
                var selection = (KeyValuePair<string, string>)item.SelectedItem;
                label = selection.Key;
                DGScoreMonth.ItemsSource = null;
               
                if (selection.Key.Equals("ឆមាសទី១") || selection.Key.Equals("ឆមាសទី២"))
                {
                    title = "semester";
                    MessageBoxControl message = new MessageBoxControl();
                    Loading loading = new Loading();
                    tabcontrolScore.SelectedIndex = 0;
                    stacButtonTop.Visibility = Visibility.Visible;
                    try
                    {

                        string respone = "";

                        term = selection.Value;
                        type = "2";
                        message.title = "ដំណឹង";
                        message.discription = "ទាញទិន្នន័យបានជោគជ័យ";
                        message.buttonType = 2;
                        loading.Show();
                        stacButtonTop.Visibility = Visibility.Visible;
                        string[] data = null;
                        if (CheckFileExist(term) == false)
                        {
                            if (Teacher.InternetChecker() && internet)
                            {
                                respone = await SaveString(months);
                                data = respone.Split('|');
                                tabcontrolScore.SelectedIndex = 0;
                                stacButtonTop.Visibility = Visibility.Visible;
                            }
                        }
                        else
                        {
                            respone = File.ReadAllText(filePath + "\\" + classId + " " + term + " " + SubjectId + ".txt");
                            data = respone.Split('|');
                        }
                        obj = JObject.Parse(data[0]).ToObject<InputScore>();
                        txtDataDate.Text = "កាលបរិច្ឆេទរបស់ទិន្នន័យ : " + obj.Datadate;
                        foreach (var item1 in obj.data)
                        {
                            if (item1.score == null|| item1.score == "0")
                            {
                                item1.score = "";
                            }
                            item1.subject_score_max = maxScore;
                            item1.subject_score_min = "0";
                            item1.error = "ពិន្ទុអតិបរិមា: " + maxScore;
                        }
                        NumberList(obj.data);
                        try
                        {
                            var expireDate = JObject.Parse(data[1]).ToObject<MonthlyScoreExpire>().data;
                            txtExpireDate.Text = "ការកំណត់ថ្ងៃសុពលភាពនៃការបញ្ចូលពិន្ទុ : " + expireDate.expired_at;
                            if (expireDate.is_expired == "True")
                            {
                                DGScoreMonth.IsEnabled = false;
                                btnPost.Visibility = Visibility.Collapsed;
                                btnDeleteAll.Visibility = Visibility.Collapsed;
                                btnSave.Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                DGScoreMonth.IsEnabled = true;
                                btnPost.Visibility = Visibility.Visible;
                                btnDeleteAll.Visibility = Visibility.Visible;
                                btnSave.Visibility = Visibility.Visible;
                            }
                        }
                        catch
                        {
                            txtExpireDate.Text = "ការកំណត់ថ្ងៃសុពលភាពនៃការបញ្ចូលពិន្ទុ : --";
                        }
                        DGScoreMonth.ItemsSource = null;
                        DGScoreMonth.ItemsSource = obj.data;
                        loading.Close();
                        this.Opacity = 0.5;
                        message.ShowDialog();
                        this.Opacity = 1;
                    }
                    catch
                    {
                        DGScoreMonth.ItemsSource = null;
                        loading.Close();

                        stacButtonTop.Visibility = Visibility.Collapsed;
                        tabcontrolScore.SelectedIndex = 1;
                        TilteSelection.Content = Properties.Langs.Lang.noresultdata;
                    }
                }
                else
                {
                    title = "month";
                    MessageBoxControl message = new MessageBoxControl();
                    Loading loading = new Loading();
                    tabcontrolScore.SelectedIndex = 0;
                    stacButtonTop.Visibility = Visibility.Visible;
                    try
                    {

                        string respone = "";

                        var month = DateChange.checkMonthString(selection.Key);
                        term = selection.Value;
                        months = month.ToString();
                        type = "1";
                        message.title = "ដំណឹង";
                        message.discription = "ទាញទិន្នន័យបានជោគជ័យ";
                        message.buttonType = 2;
                        loading.Show();
                        stacButtonTop.Visibility = Visibility.Visible;
                        string[] data = null;

                        if (CheckFileExist(months) == false)
                        {
                            if (Teacher.InternetChecker() && internet)
                            {

                                respone = await SaveString(months);
                                data = respone.Split('|');
                                tabcontrolScore.SelectedIndex = 0;
                                stacButtonTop.Visibility = Visibility.Visible;
                            }
                        }
                        else
                        {
                            respone = File.ReadAllText(filePath + "\\" + classId + " " + month + " " + SubjectId + ".txt");
                            data = respone.Split('|');
                        }
                        obj = JObject.Parse(data[0]).ToObject<InputScore>();
                        txtDataDate.Text = "កាលបរិច្ឆេទរបស់ទិន្នន័យ : " + obj.Datadate;
                        foreach (var item1 in obj.data)
                        {
                            if (item1.score == null||item1.score == "0")
                            {
                                item1.score = "";
                            }
                            item1.subject_score_max = maxScore;
                            item1.subject_score_min = "0";
                            item1.error = "ពិន្ទុអតិបរិមា: "+maxScore;
                        }
                        try
                        {
                            if (obj.approve_learning_result.is_approved == "1")
                            {
                                txtApproveDate.Text = "កាលបរិច្ឆេទដែលបានអនុម័ត :" + obj.approve_learning_result.approved_date;
                                txtApproveDate.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                txtApproveDate.Visibility = Visibility.Collapsed;
                            }
                        }
                        catch
                        {
                            txtApproveDate.Visibility = Visibility.Collapsed;
                        }
                        NumberList(obj.data);

                        try
                        {
                            var expireDate = JObject.Parse(data[1]).ToObject<MonthlyScoreExpire>().data;
                            txtExpireDate.Text = "ការកំណត់ថ្ងៃសុពលភាពនៃការបញ្ចូលពិន្ទុ : " + expireDate.expired_at;
                        }
                        catch
                        {
                            txtExpireDate.Text = "ការកំណត់ថ្ងៃសុពលភាពនៃការបញ្ចូលពិន្ទុ : --";
                        }
                        try
                        {
                            if (obj.approve_learning_result.is_approved == "1")
                            {
                                DGScoreMonth.IsEnabled = false;
                                btnPost.Visibility = Visibility.Collapsed;
                                btnDeleteAll.Visibility = Visibility.Collapsed;
                                btnSave.Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                try
                                {
                                    var expireDate = JObject.Parse(data[1]).ToObject<MonthlyScoreExpire>().data;
                                    if (expireDate.is_expired == "True")
                                    {
                                        DGScoreMonth.IsEnabled = false;
                                        btnPost.Visibility = Visibility.Collapsed;
                                        btnDeleteAll.Visibility = Visibility.Collapsed;
                                        btnSave.Visibility = Visibility.Collapsed;
                                    }
                                    else
                                    {
                                        DGScoreMonth.IsEnabled = true;
                                        btnPost.Visibility = Visibility.Visible;
                                        btnDeleteAll.Visibility = Visibility.Visible;
                                        btnSave.Visibility = Visibility.Visible;
                                    }
                                }
                                catch
                                {
                                    txtExpireDate.Text = "ការកំណត់ថ្ងៃសុពលភាពនៃការបញ្ចូលពិន្ទុ : --";
                                    DGScoreMonth.IsEnabled = true;
                                    btnPost.Visibility = Visibility.Visible;
                                    btnDeleteAll.Visibility = Visibility.Visible;
                                    btnSave.Visibility = Visibility.Visible;
                                    File.Delete(filePath + "\\" + classId + " " + month + " " + SubjectId + ".txt");
                                }
                            }
                        }
                        catch
                        {
                            try
                            {
                                var expireDate = JObject.Parse(data[1]).ToObject<MonthlyScoreExpire>().data;
                                if (expireDate.is_expired == "True")
                                {
                                    DGScoreMonth.IsEnabled = false;
                                    btnPost.Visibility = Visibility.Collapsed;
                                    btnDeleteAll.Visibility = Visibility.Collapsed;
                                    btnSave.Visibility = Visibility.Collapsed;
                                }
                                else
                                {
                                    DGScoreMonth.IsEnabled = true;
                                    btnPost.Visibility = Visibility.Visible;
                                    btnDeleteAll.Visibility = Visibility.Visible;
                                    btnSave.Visibility = Visibility.Visible;
                                }
                            }
                            catch
                            {
                                txtExpireDate.Text = "ការកំណត់ថ្ងៃសុពលភាពនៃការបញ្ចូលពិន្ទុ : --";
                                DGScoreMonth.IsEnabled = true;
                                btnPost.Visibility = Visibility.Visible;
                                btnDeleteAll.Visibility = Visibility.Visible;
                                btnSave.Visibility = Visibility.Visible;
                                File.Delete(filePath + "\\" + classId + " " + month + " " + SubjectId + ".txt");
                            }
                        }
                        DGScoreMonth.ItemsSource = null;
                        DGScoreMonth.ItemsSource = obj.data;
                        File.Delete(filePath + "\\" + classId + " " + month + " " + SubjectId + ".txt");
                        btnPrint.Visibility = Visibility.Visible;
                        btnSave.Visibility = Visibility.Visible;
                        loading.Close();
                        this.Opacity = 0.5;
                        message.ShowDialog();
                        this.Opacity = 1;
                    }
                    catch
                    {
                        DGScoreMonth.ItemsSource = null;
                        loading.Close();

                        stacButtonTop.Visibility = Visibility.Collapsed;
                        tabcontrolScore.SelectedIndex = 1;
                        TilteSelection.Content = Properties.Langs.Lang.noresultdata;
                    }
                }
            }

            catch
            {

            }
        }
        int time = 1;
        private async void cbSelectClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                this.IsEnabled = false;
                btnLearningResult.IsEnabled = true;
                cbSelectSubject.Text = "សូមជ្រើរើសមុខវិជ្ជា";
                cbSelectSubject.SelectedIndex = -1;
                cbSelectMonth.Text = "សូមជ្រើសរើសខែ/ឆមាស";
                cbSelectMonth.SelectedIndex = -1;
                cbSelectMonth.Visibility = Visibility.Collapsed;
                DGMonthlyResult.ItemsSource = null;
                DGSemesterClass.ItemsSource = null;
                DGSemesterExam.ItemsSource = null;
                List<KeyValuePair<string, string>> data = new List<KeyValuePair<string, string>>();
                txtTitleClassName.Visibility = Visibility.Visible;
                //checkMonthButton = true;
                TilteSelection.Content = Properties.Langs.Lang.Message_Box_Stu_Result_Title_select_Subject;
                cbSelectSubject.Visibility = Visibility.Visible;
                var item = sender as ComboBox;
                var selection = (KeyValuePair<string, string>)item.SelectedItem;
                //treeViewItemChange(item.id);
                classId = selection.Value;
                txtTitleClassName.Text = "ថ្នាក់ទី" + selection.Key;
                LabelTitle.Content = Properties.Langs.Lang.Message_Box_Stu_Result_Title_select_month;
                tabcontrolScore.SelectedIndex = 1;

                if (Teacher.InternetChecker() == true && internet && ping <= 100)
                {

                    string accessUrl = Properties.Settings.Default.acessUrl;
                    string token = Properties.Settings.Default.Token;
                    var respone = await RESTApiHelper.GetAll(accessUrl, "/get-teaching-subject-class/" + selection.Value, token);
                    var obj = JObject.Parse(respone).ToObject<GetTeachingSubjectClass>();
                    obj.classId = selection.Value;
                    obj.teacherId = Properties.Settings.Default.teacherId;
                    

                    var respone1 = await RESTApiHelper.GetAll(accessUrl, "/academic/" + selection.Value + "/grade-time-shift", token);
                    var obj1 = JObject.Parse(respone1).ToObject<TimesButton>();
                    foreach (var item1 in obj1.data)
                    {
                        foreach (var month in item1.months)
                        {
                            data.Add(new KeyValuePair<string, string>(month.displayMonth, item1.semester));
                        }
                        data.Add(new KeyValuePair<string, string>(item1.name, item1.semester));
                    }
                   
                    cbSelectMonth.ItemsSource = data;
                    cbSelectMonth.DisplayMemberPath = "Key";
                    cbSelectMonth.SelectedValuePath = "Value";
                    cbSelectResultMonth.ItemsSource = data;
                    cbSelectResultMonth.DisplayMemberPath = "Key";
                    cbSelectResultMonth.SelectedValuePath = "Value";

                    btnResultofTheYear1.Visibility = Visibility.Visible;
                    btnResultofTheYear.Visibility = Visibility.Visible;

                    foreach (var sub in obj.data)
                    {
                        sub.name = sub.name + " (" + sub.max_score + ")";
                        
                    }

                    cbSelectSubject.ItemsSource = obj.data;
                    cbSelectSubject.DisplayMemberPath = "name";
                    cbSelectSubject.SelectedValuePath = "id";
                    //Task<string> task = GetMonthlyResultFormApiAsync();
                    SaveLocalSubject(JsonConvert.SerializeObject(obj), selection.Value);
                    SaveAcademyMonth(JsonConvert.SerializeObject(obj1),schoolYearId);
                    DockTree.Visibility = Visibility.Collapsed;
                    tabcontrolResult.SelectedIndex = 1;
                    tabcontrolScore.SelectedIndex = 1;
                    string use = "";
                    if (time == 1)
                        use = "use";
                    else
                        use = "not use";

                    GetMonthlyResultFormApiAsync();
                    time++;
                    this.IsEnabled = true;
                }

                else
                {
                    try
                    {
                        var obj1 = GetAcademyFromLocal(schoolYearId);

                        foreach (var item1 in obj1.data)
                        {
                            foreach (var month in item1.months)
                            {
                                data.Add(new KeyValuePair<string, string>(month.displayMonth, item1.semester));
                            }
                            data.Add(new KeyValuePair<string, string>(item1.name, item1.semester));
                        }

                        cbSelectMonth.ItemsSource = data;
                        cbSelectMonth.DisplayMemberPath = "Key";
                        cbSelectMonth.SelectedValuePath = "Value";
                        cbSelectResultMonth.ItemsSource = data;
                        cbSelectResultMonth.DisplayMemberPath = "Key";
                        cbSelectResultMonth.SelectedValuePath = "Value";
                    }
                    catch
                    {
                        MessageBoxControl message = new MessageBoxControl();
                        message.title = "ដំណឹង";
                        message.discription = "មិនមានទីន្នន័យរក្សាទុក";
                        message.buttonType = 2;
                        this.Opacity = 0.5;
                        message.ShowDialog();
                        this.Opacity = 1;
                        cbSelectSubject.Visibility = Visibility.Collapsed;
                    }

                    btnResultofTheYear1.Visibility = Visibility.Visible;
                    btnResultofTheYear.Visibility = Visibility.Visible;

                    try
                    {
                        var obj = GetSubjectFromLocal(selection.Value).data;
                        if (obj == null)
                        {
                            MessageBoxControl message = new MessageBoxControl();
                            message.title = "ដំណឹង";
                            message.discription = "មិនមានទីន្នន័យមុខវិជ្ចាបង្រៀនរក្សាទុក";
                            message.buttonType = 1;
                            this.Opacity = 0.5;
                            message.ShowDialog();
                            this.Opacity = 1;
                            cbSelectSubject.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            
                            cbSelectSubject.Visibility = Visibility.Visible;
                            cbSelectSubject.ItemsSource = obj;
                            cbSelectSubject.DisplayMemberPath = "name";
                            cbSelectSubject.SelectedValuePath = "id";
                        }
                    }
                    catch
                    {
                        MessageBoxControl message = new MessageBoxControl();
                        message.title = "ដំណឹង";
                        message.discription = "មិនមានទីន្នន័យមុខវិជ្ចាបង្រៀនរក្សាទុក";
                        message.buttonType = 2;
                        this.Opacity = 0.5;
                        message.ShowDialog();
                        this.Opacity = 1;
                        cbSelectSubject.Visibility = Visibility.Collapsed;
                    }

                    
                    DockTree.Visibility = Visibility.Collapsed;
                    tabcontrolResult.SelectedIndex = 1;
                    tabcontrolScore.SelectedIndex = 1;

                }
                this.IsEnabled = true;
            }
            catch
            {
                //checkMonthButton = false;
                LabelTitle.Content = Properties.Langs.Lang.Message_Box_Stu_Result_Title;
                this.IsEnabled = true;
            }
        }
        string monthName = "";
        List<StudentMonthlyResult> resultData = new List<StudentMonthlyResult>();
        private void cbSelectResultMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var item = sender as ComboBox;
                string id="";
                var selection = (KeyValuePair<string, string>)item.SelectedItem;
                monthName = selection.Key;
                if (selection.Key.Equals("ឆមាសទី១") || selection.Key.Equals("ឆមាសទី២"))
                {
                    title = "semester";
                    txtTitleMonth.Text = "លទ្ធផលប្រចាំ​ "+selection.Key;
                    btnSaveSemester.Visibility = Visibility.Collapsed;
                    btnPostSemester.Visibility = Visibility.Collapsed;
                    MessageBoxControl message = new MessageBoxControl();
                    Loading loading = new Loading();
                    tabcontrolLearn1.SelectedIndex = 2;
                    Selectresult.Visibility = Visibility.Collapsed;
                    resultData = new List<StudentMonthlyResult>();
                    try
                    {
                        List<Morality> moralities1 = new List<Morality>();
                        string respone = "", date = "";

                        nameSemester = selection.Key;
                        message.title = "ដំណឹង";
                        message.discription = "ទាញទិន្នន័យបានជោគជ័យ";
                        message.buttonType = 2;
                        loading.Show();
                        Selectresult.Visibility = Visibility.Collapsed;
                        tabcontrolLearn1.SelectedIndex = 1;

                        resultData = GetData("", "semester", selection.Key);
                        NumberList(resultData, "2");
                        foreach(var item1 in resultData)
                        {
                            if (item1.profileMedia.id == null)
                                id = item1.student_id;
                            else
                                id = item1.profileMedia.id;
                            item1.localProfileLink = filePath + "\\" + id + ".jpg";
                        }
                        if (!File.Exists(filePath + "\\" + classId + " " + nameSemester + ".txt"))
                        {
                            foreach (var item1 in resultData)
                            {
                                item1.localProfileLink = filePath + "\\" + item1.profileMedia.id + ".jpg";
                                moralities1.Add(new Morality
                                {
                                    number = item1.numbers,
                                    avg_score = item1.result_semester.avg_score,
                                    gender = item1.gender,
                                    id = item1.result_semester.id,
                                    name = item1.name,
                                    rank = item1.result_semester.rank,
                                    bangkeun_phal = item1.result_semester.bangkeun_phal,
                                    morality = item1.result_semester.morality,
                                    health = item1.result_semester.health,
                                    grading = item1.result_semester.grading,
                                    profile = item1.localProfileLink
                                });
                            }
                            date = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                            moralities = moralities1;
                        }
                        else
                        {
                            var responeString = File.ReadAllText(filePath + "\\" + classId + " " + nameSemester + ".txt");
                            moralities = JObject.Parse(responeString).ToObject<ListMorality>().data;
                            date = JObject.Parse(responeString).ToObject<ListMorality>().date;
                        }
                        txtDate.Text = "កាលបរិច្ឆេទ​ : " + date;
                        DGSemester.ItemsSource = null;
                        DGSemesterExam.ItemsSource = null;
                        DGSemesterClass.ItemsSource = null;
                        DGSemester.ItemsSource = resultData;
                        DGSemesterExam.ItemsSource = resultData;
                        DGSemesterClass.ItemsSource = moralities.OrderBy(s => s.rank);
                        loading.Close();
                        this.Opacity = 0.5;
                        message.ShowDialog();
                        this.Opacity = 1;
                        tabcontrolLearn1.SelectedIndex = 2;
                        Selectresult.Visibility = Visibility.Visible;
                    }
                    catch
                    {
                        DGMonthlyResult.ItemsSource = null;
                        loading.Close();
                        tabcontrolLearn1.SelectedIndex = 0;
                        Selectresult.Visibility = Visibility.Collapsed;
                        lblErrandSelect.Content = Properties.Langs.Lang.noresultdata;
                    }
                    Selectresult.Visibility = Visibility.Visible;
                }
                else
                {
                    title = "month";
                    txtTitleMonth.Text = "លទ្ធផលប្រចាំ​ ខែ" + selection.Key;
                    btnSaveSemester.Visibility = Visibility.Collapsed;
                    btnPostSemester.Visibility = Visibility.Collapsed;
                    MessageBoxControl message = new MessageBoxControl();
                    Loading loading = new Loading();
                    tabcontrolLearn1.SelectedIndex = 0;
                    Selectresult.Visibility = Visibility.Collapsed;
                    resultData = new List<StudentMonthlyResult>();
                    try
                    {

                        string respone = "";
                       
                        var month = DateChange.checkMonthString(selection.Key);
                        term = selection.Value;
                        months = month.ToString();
                        message.title = "ដំណឹង";
                        message.discription = "ទាញទិន្នន័យបានជោគជ័យ";
                        message.buttonType = 2;
                        loading.Show();
                        Selectresult.Visibility = Visibility.Collapsed;
                        tabcontrolLearn1.SelectedIndex = 1;

                        resultData = GetData(month.ToString());

                        foreach (var item1 in resultData)
                        {
                            item1.localProfileLink = filePath + "\\" + item1.profileMedia.id + ".jpg";
                        }

                        NumberList(resultData, "1");
                        foreach (var item1 in resultData)
                        {
                            if (item1.profileMedia.id == null)
                                id = item1.student_id;
                            else
                                id = item1.profileMedia.id;
                            item1.localProfileLink = filePath + "\\" + id + ".jpg";
                        }
                        DGMonthlyResult.ItemsSource = null;
                        DGMonthlyResult.ItemsSource = resultData;
                        loading.Close();
                        this.Opacity = 0.5;
                        message.ShowDialog();
                        this.Opacity = 1;
                        Selectresult.Visibility = Visibility.Collapsed;
                    }
                    catch
                    {
                        DGMonthlyResult.ItemsSource = null;
                        loading.Close();
                        tabcontrolLearn1.SelectedIndex = 0;
                        Selectresult.Visibility = Visibility.Collapsed;
                        lblErrandSelect.Content = Properties.Langs.Lang.noresultdata;
                    }
                }
            }
            catch { }
        }

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
        //-------------------Save Data in local---------------------------
        private async Task<string> GetMonthlyResultFormApiAsync(string use = "use")
        {
            Loading loading = new Loading();
            //----------------AccessUrl and Token---------------------------
            string accessUrl = Properties.Settings.Default.acessUrl;
            string token = Properties.Settings.Default.Token, id = "";

            //---------------------------------------------------------------
            var months = Properties.Settings.Default.monthofTheAcademyYear;
            var obj = JObject.Parse(months).ToObject<TimesButton>().data;
            int time = 1;
            string responeMonth = "", reponseSemester = "", reponseYear = "", encryptionString = "", encryptionStringSemester = "", encryptionStringYear = "", photos = "";
            loading.Show();
            if (internet && Teacher.InternetChecker())
            {
                foreach (var item in obj)
                {
                    foreach (var month in item.months)
                    {
                        responeMonth += month.month + "|";
                        responeMonth += await RESTApiHelper.GetAll(accessUrl, "/academic/" + classId + "/monthly-result?month=" + month.month + "&term=" + item.semester, token);
                        responeMonth += "*";
                        if (time == 1)
                        {
                            reponseSemester = await RESTApiHelper.GetAll(accessUrl, "/academic/" + classId + "/semester-result?term=" + "FIRST_SEMESTER", token);
                            reponseYear = await RESTApiHelper.GetAll(accessUrl, "/academic/" + classId + "/yearly-result", token) + "|" + DateTime.Now.ToString();
                            photos = await RESTApiHelper.GetAll(accessUrl, "/academic/" + classId + "/monthly-result?month=" + month.month + "&term=" + item.semester, token);
                            time++;
                        }
                        else if (time == 2)
                        {
                            reponseSemester += "|";
                            reponseSemester += await RESTApiHelper.GetAll(accessUrl, "/academic/" + classId + "/semester-result?term=" + "SECOND_SEMESTER", token);
                            time++;
                        }
                    }
                }
            }
            else
            {
                loading.Close();
                return null;
            }
            time = 1;
            encryptionString = EncodeTo64(responeMonth);
            encryptionStringSemester = EncodeTo64(reponseSemester);
            encryptionStringYear = EncodeTo64(reponseYear);
            GC.Collect();
            using (StreamWriter writer = new StreamWriter(filePath + "\\" + classId + ".txt"))
            {
                writer.WriteLine(encryptionString);
            }
            GC.Collect();
            using (StreamWriter writer = new StreamWriter(filePath + "\\" + "semester" + classId + ".txt"))
            {
                writer.WriteLine(encryptionStringSemester);
            }
            GC.Collect();
            using (StreamWriter writer = new StreamWriter(filePath + "\\" + "Year" + classId + ".txt"))
            {
                writer.WriteLine(encryptionStringYear);
            }
            GC.Collect();
            var photo = JObject.Parse(photos).ToObject<StudentMonthlyResultData>().data;

            foreach (var i in photo.Take(1))
            {
                if (i.profileMedia.id == null)
                    id = i.student_id;
                else
                    id = i.profileMedia.id;
            }

            try
            {
                if (use == "use")
                {
                    if (IsImageHave(id))
                    {
                        MessageBoxControl message = new MessageBoxControl();
                        message.title = "ដំណឹង";
                        message.discription = "តើចង់កែប្រែរូបដែរមានហើយឬទេ?";
                        this.Opacity = 0.5;
                        message.ShowDialog();
                        this.Opacity = 1;
                        if (message.result == 1)
                        {
                            foreach (var item in photo)
                            {
                                if (time == 1)
                                {
                                    SaveImage(item.instructor.profileMedia.file_name, ImageFormat.Jpeg, item.instructor.profileMedia.file_show);
                                    time++;
                                }
                                if (item.profileMedia.id == null)
                                    id = item.student_id;
                                else
                                    id = item.profileMedia.id;
                                SaveImage(id, ImageFormat.Jpeg, item.profileMedia.file_show);
                                GC.Collect();
                                item.localProfileLink = filePath + "\\" + id + ".jpg";
                            }
                        }
                        else
                        {
                            foreach (var item in photo)
                            {
                                if (item.profileMedia.id == null)
                                    id = item.student_id;
                                else
                                    id = item.profileMedia.id;
                                item.localProfileLink = filePath + "\\" + id + ".jpg";
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in photo)
                        {
                            if (time == 1)
                            {
                                SaveImage(item.instructor.profileMedia.file_name, ImageFormat.Jpeg, item.instructor.profileMedia.file_show);
                                time++;
                            }
                            if (item.profileMedia.id == null)
                                id = item.student_id;
                            else
                                id = item.profileMedia.id;
                            SaveImage(id, ImageFormat.Jpeg, item.profileMedia.file_show);
                            GC.Collect();
                            item.localProfileLink = filePath + "\\" + id + ".jpg";
                        }
                    }
                }
            }
            catch
            {
                loading.Close();
                return encryptionString;
            }
            loading.Close();
            return encryptionString;
        }
        //----------------------------------------------------------------
        //----------------------Get Data from local-----------------------
        private List<StudentMonthlyResult> GetData(string month = "", string title = "", string semester = "", string id = "")
        {
            string respone = "";
            if (id == "")
            {
                respone = GetStringFromFile(title);
            }
            else
            {
                respone = GetStringFromFile(title, id: id);
            }
            if (title == "semester")
            {
                string[] allRespone = respone.Split('|');
                if (semester == "ឆមាសទី១")
                {
                    var obj = JObject.Parse(allRespone[0].ToString()).ToObject<StudentMonthlyResultData>().data;
                    return obj.OrderBy(s => s.result_semester.rank).ToList();
                }
                else if (semester == "ឆមាសទី២")
                {
                    var obj = JObject.Parse(allRespone[1].ToString()).ToObject<StudentMonthlyResultData>().data;
                    return obj.OrderBy(s => s.result_semester.rank).ToList();
                }
            }
            else if (title == "Year")
            {
                string[] allRespone = respone.Split('|');
                var obj = JObject.Parse(allRespone[0]).ToObject<StudentMonthlyResultData>().data;
                return obj;
            }
            else
            {
                string[] allRespone = respone.Split('*');
                foreach (var item in allRespone)
                {
                    var allMonth = item.Split('|');
                    if (allMonth[0] == month)
                    {
                        var obj = JObject.Parse(allMonth[1].ToString()).ToObject<StudentMonthlyResultData>().data;
                        Properties.Settings.Default.studentMonthlyResult = allMonth[1];
                        Properties.Settings.Default.Save();
                        return obj.OrderBy(s => s.result_monthly.rank).ToList();
                    }
                }
            }
            return null;
        }

        private void btnPrintResult_Click(object sender, RoutedEventArgs e)
        {
            MonthlyResult monthly = new MonthlyResult(resultData,title,titleYear);
            monthly.Show();
        }

        //----------------------------------------------------------------

        //-------------------Save Subject in Local------------------------
        private void SaveLocalSubject(string respone,string classId)
        {
            string saveString = respone;
            if (!File.Exists(filePath + "\\" + "subject " + Properties.Settings.Default.teacherId + ".txt"))
            {
                using (StreamWriter writer = new StreamWriter(filePath + "\\" + "subject " + Properties.Settings.Default.teacherId + ".txt"))
                {
                    writer.WriteLine(saveString + "|");
                }
            }
            else
            {
                string returnString = File.ReadAllText(filePath + "\\" + "subject " + Properties.Settings.Default.teacherId + ".txt");
                string[] data = returnString.Split('|');
                bool save = false;
                for (int i = 0; i < data.Length-1; i++)
                {
                    var obj = JObject.Parse(data[i]).ToObject<GetTeachingSubjectClass>();
                    if(obj.classId==classId)
                    {
                        save = false;
                        break;
                    }
                    else
                    {
                        save = true;
                    }
                }
                if(save==true)
                {
                    using (StreamWriter writer = new StreamWriter(filePath + "\\" + "subject " + Properties.Settings.Default.teacherId + ".txt"))
                    {
                        writer.WriteLine(saveString + "|" + returnString);
                    }
                    return;
                }

            } 
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            List<StudentInformation> obejct = new List<StudentInformation>();
            try
            {
                obejct = obj.data.OrderBy(s => int.Parse(s.rank)).ToList();
            }catch
            {
                obejct = obj.data.ToList();
            }
            NumberList(obejct);
            MonthlySubjectResult monthlySubject = new MonthlySubjectResult(obejct, title,label,titleYear);
            monthlySubject.Show();
        }

        //----------------------------------------------------------------
        //----------------------Get Academy Month from Local--------------
        private void SaveAcademyMonth(string respone, string year)
        {
            string saveString = respone;
            using (StreamWriter writer = new StreamWriter(filePath + "\\" + "academyYear " + year + ".txt"))
            {
                writer.WriteLine(saveString);
            }
        }
        //----------------------------------------------------------------
        //----------------------Get Subject from local--------------------
        GetTeachingSubjectClass GetSubjectFromLocal (string classId)
        {
            try
            {
                string returnString = File.ReadAllText(filePath + "\\" + "subject " + Properties.Settings.Default.teacherId + ".txt");
                string[] data = returnString.Split('|');

                for (int i = 0; i < data.Length-1; i++)
                {
                    var obj = JObject.Parse(data[i]).ToObject<GetTeachingSubjectClass>();
                    if (obj.classId == classId)
                    {
                        return obj;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
            
        }
        //----------------------------------------------------------------
        //----------------------Get Academy month from local--------------
        TimesButton GetAcademyFromLocal(string year)
        {
            try
            {
                string returnString = File.ReadAllText(filePath + "\\" + "academyYear " + year + ".txt");
                var obj = JObject.Parse(returnString).ToObject<TimesButton>();
                return obj;
            }
            catch
            {
                return null;
            }
        }
        //----------------------------------------------------------------
        //-------------------Get String from File-------------------------
        private string GetStringFromFile(string title = "", string id = "")
        {
            try
            {
                if (id == "")
                {
                    string readText = "";
                    if (title == "semester")
                    {
                        readText = File.ReadAllText(filePath + "\\" + "semester" + classId + ".txt");
                    }
                    else if (title == "Year")
                    {
                        readText = File.ReadAllText(filePath + "\\" + "Year" + classId + ".txt");
                    }
                    else
                    {
                        readText = File.ReadAllText(filePath + "\\" + classId + ".txt");
                    }
                    return DecodeFrom64(readText);
                }
                else
                {
                    string readText = "";
                    if (title == "semester")
                    {
                        readText = File.ReadAllText(filePath + "\\" + "semester" + id + ".txt");
                    }
                    else if (title == "Year")
                    {
                        readText = File.ReadAllText(filePath + "\\" + "Year" + id + ".txt");
                    }
                    else
                    {
                        readText = File.ReadAllText(filePath + "\\" + id + ".txt");
                    }
                    return DecodeFrom64(readText);
                }
            }
            catch
            {
                return null;
            }
        }
        //----------------------------------------------------------------
    }
}