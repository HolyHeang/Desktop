using CamemisOffLine.Windows;
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
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;
using System.Globalization;
using CamemisOffLine.Class;
using Newtonsoft.Json.Linq;
using Library;
using System.Net;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using MonthlyResult = CamemisOffLine.Windows.MonthlyResult;
using CamemisOffLine.Asset;
using CamemisOffLine.Component;
using CamemisOffLine.Report;
using System.Net.NetworkInformation;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;

namespace CamemisOffLine
{
    /// <summary>
    /// Interaction logic for Teacher.xaml
    /// </summary>
    /// 

    public partial class Teacher : Window
    {
        int role = 1;

        public Teacher()
        {


            InitializeComponent();

            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.LongDatePattern = "MMM/yyyy"; //This can be used for one type of DatePicker
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; //for the second type
            Thread.CurrentThread.CurrentCulture = ci;

            WindowState = System.Windows.WindowState.Maximized;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(UpdateTimer_Tick);
            timer.Interval = new TimeSpan(0, 0, 50);
            timer.Start();
            DispatcherTimer Internet = new DispatcherTimer();
            Internet.Tick += Internet_Tick;
            Internet.Interval = TimeSpan.FromSeconds(10);
            Internet.Start();

        }
        Ping myPing = new Ping();
        int ping = 0;
        private void Internet_Tick(object sender, EventArgs e)
        {
            try
            {
               PingReply reply = myPing.Send(@"Google.com", 1000);
               if(InternetChecker()&&internet)
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
                        }
                        txtPing.Text = "Ping :" + (ping) + "ms";
                    }
                    wifiIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Wifi;
                    btnCheck.IsChecked = true;
                    txtxCheckinternet.Content = "Online";
                }
                else
                {
                    ping = 999;
                    wifiIcon.Foreground = Brushes.Red;
                    txtPing.Text = "Ping :" + (ping) + "ms";
                    wifiIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WifiOff;
                    btnCheck.IsChecked = false;
                    txtxCheckinternet.Content = "Offline";
                }
            }
            catch
            {
                ping = 999;
                wifiIcon.Foreground = Brushes.Red;
                txtPing.Text = "Ping :" + (ping) + "ms";
                wifiIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WifiOff;
                btnCheck.IsChecked = false;
                txtxCheckinternet.Content = "Offline";
            }
            GC.Collect();
        }

        string todayDate = "";
        bool checkSaveImgae = true, getAllData = false;
        public Teacher(string userName, int role)
        {
            InitializeComponent();
            this.role = role;
            if (role == 1)
            {
                TeacherPart.Visibility = Visibility.Collapsed;
            }
            else if (role == 2)
            {
                adminPart.Visibility = Visibility.Collapsed;
            }
            ///treeview............
            ///...........
        }
        //combobox select in top bar and datepicker.....
        public string token = Properties.Settings.Default.Token, staffAtt = "";
        ObservableCollection<GradeTimeButton> DataButton = new ObservableCollection<GradeTimeButton>();
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DPStaffAtt.DisplayDate = DateTime.Now;
            ///.................Part Setting...................
            btnAbout.Visibility = Visibility.Collapsed;
            btnColor.Visibility = Visibility.Collapsed;
            MateriaSettingUp.Visibility = Visibility.Collapsed;
            btnLang.Visibility = Visibility.Collapsed;
            gridLanguage.Visibility = Visibility.Collapsed;
            //....................End.........................

            //-------------User Profile----------------
            if (Properties.Settings.Default.localProfileLink.ToString() != "")
                imgUserProfile.Source = new BitmapImage(new Uri(Properties.Settings.Default.localProfileLink.ToString()));
            //------------------------------------------

            //Check Start Program
            startProgram = true;
            //End
            //.................

            //***********Student Learning Result**********************
            gridformstuResult.Margin = new Thickness(0, -50, 0, 0);
            gridStudentResult.Margin = new Thickness(0, -41, 0, 0);
            //.........................
            // Collapsed in featuer Student learning result ///

            Month.Visibility = Visibility.Collapsed;
            gridgoto.Visibility = Visibility.Collapsed;
            grideStuResult.Visibility = Visibility.Collapsed;
            tabStudentResult.SelectedIndex = 0;


            //****************** END ****************///

            //.................Collab....Print Feature............

            btnStudent.Visibility = Visibility.Collapsed;
            btnTeacher.Visibility = Visibility.Collapsed;
           
            MateriaFeaprintUp.Visibility = Visibility.Collapsed;

            //****************Student Attendance Report*****************

            gridbarstuAtt.Visibility = Visibility.Collapsed;
            tabStudentAtt.SelectedIndex = 1;
            gridStudentAtt.Margin = new Thickness(0, -10, 0, 0);
           
            //defult home page//
            tabMenu.SelectedIndex = 3;
            //END.........//
            var bc = new BrushConverter();
            btnStudentLearningRsult.Background = Brushes.White;
            MateriaStuLearn.Foreground = (Brush)bc.ConvertFrom("#1183CA");
            lblStuLearn.Foreground = (Brush)bc.ConvertFrom("#1183CA");
            Tiltle.Content = Properties.Langs.Lang.Stu_Learn_Result;
            //

            if (Properties.Settings.Default.checkLoginOrLogut == "login")
            {
                if (InternetChecker())
                {
                    btnCheck.IsChecked = true;
                    txtxCheckinternet.Content = "Online";
                }
                else
                {
                    btnCheck.IsChecked = false;
                    txtxCheckinternet.Content = "Offline";
                }

                if (Properties.Settings.Default.role=="1")
                {
                    Loading loading = new Loading();
                    //------Loading-----
                    loading.Show();
                    loading.ShowInTaskbar = false;
                    loading.Owner = this;
                    this.IsEnabled = false;
                    //----end loading----
                    //Load subject data
                    //----------------------------
                    //var token = Properties.Settings.Default.Token;

                    //var respone = await RESTApiHelper.GetAll(token);

                    //var obj = JObject.Parse(respone).ToObject<Data>().data;

                    //Data.ItemsSource = obj;

                    //var d1 = Data.ItemsSource;
                    //var d2 = d1.Cast<Subject>();
                    //var d3 = d2.ToList();
                    //List<Subject> sub = d3;

                    //SQLiteHelper.SaveSubject(sub);

                    //MessageBox.Show("Save to DataBase Successfully!!");


                    Properties.Settings.Default.checkLoginOrLogut = "login";
                    Properties.Settings.Default.Save();

                    this.FontFamily = new FontFamily("/CamemisOffLine;component/FontStyle/#Khmer OS Battambang");

                    //.............Slide Left..............
                    slideLeft.Width = 45;
                    gridAcc.Visibility = Visibility.Collapsed;
                    lblnameCompany.Visibility = Visibility.Collapsed;
                    gridfeature.Margin = new Thickness(0, 0, 0, 0);


                    //btnHome.IsEnabled = false;

                    //..................Part Print....................
                    MateriaFeaprintdrop.Visibility = Visibility.Visible;
                    MateriaFeaprintUp.Visibility = Visibility.Collapsed;
                    btnTeacher.Visibility = Visibility.Collapsed;
                    btnStudent.Visibility = Visibility.Collapsed;


                    ///.................Part Setting...................
                    btnAbout.Visibility = Visibility.Collapsed;
                    btnColor.Visibility = Visibility.Collapsed;
                    MateriaSettingUp.Visibility = Visibility.Collapsed;
                    btnLang.Visibility = Visibility.Collapsed;
                    gridLanguage.Visibility = Visibility.Collapsed;
                    MateriaLangDrop.Visibility = Visibility.Visible;
                    MateriaSettingDrop.Visibility = Visibility.Visible;
                    //....................End.........................

                    //btnHome.IsEnabled = true;

                    CultureInfo culture;
                    if (Properties.Settings.Default.Language == "km-KH")
                    {
                        //Combobox.SelectedIndex = 0;
                        culture = new CultureInfo("km-KH");
                        txtDate.Text = DateTime.Now.ToString("dddd  MMMM  dd  yyyy", culture);
                    }
                    else if (Properties.Settings.Default.Language == "en-US")
                    {

                        //Combobox.SelectedIndex = 1;
                        culture = new CultureInfo("en-US");
                        txtDate.Text = DateTime.Now.ToString("dddd  MMMM  dd  yyyy", culture);
                    }
                    else if (Properties.Settings.Default.Language == "zh-Hans")
                    {
                        //Combobox.SelectedIndex = 2;
                        culture = new CultureInfo("zh-Hans");
                        txtDate.Text = DateTime.Now.ToString("dddd  MMMM  dd  yyyy", culture);
                    }
                    else if (Properties.Settings.Default.Language == "vi-VN")
                    {
                        //Combobox.SelectedIndex = 3;
                        culture = new CultureInfo("vi-VN");
                        txtDate.Text = DateTime.Now.ToString("dddd  MMMM  dd  yyyy", culture);
                    }

                    /////// DateAttendance Student..............
                    DateAtt.SelectedDate = DateTime.Now;
                    var dateValueAtt = DateAtt.SelectedDate.Value.Date;


                    lbldayAtt.Content = DateChange.checkDay(dateValueAtt.DayOfWeek.ToString());
                    lblMonthAtt.Content = DateChange.checkMonth(int.Parse(dateValueAtt.Month.ToString()));
                    lblyearAtt.Content = DateChange.Num(int.Parse(dateValueAtt.Year.ToString()));
                    lbldayNumAtt.Content = DateChange.Num(int.Parse(dateValueAtt.Day.ToString()));
                    ///
                    /// 
                    ///
                 List<StaffAttendance> at = new List<StaffAttendance>();
                    for (int i = 1; i <= 5; i++)
                    {
                        at.Add(new StaffAttendance
                        {
                            id = i.ToString(),
                            name = "Heang Holy",
                            sex = "M",
                            position = "IT",
                            mIn = "7:00",
                            mOut = "11:00",
                            eIn = "1:00",
                            eOut = "5:00",
                            late = "8:00",
                            early = "4:00",
                            signature = "dfasdf",
                            other = "fskdafakf"
                        });
                    }
                    DGStaffAtt.ItemsSource = at;
                    DGStaffAtt1.ItemsSource = at;

                    //Load Year of Academy
                    if (InternetChecker()&&internet)
                    {

                        ///.................Part Setting...................
                        btnAbout.Visibility = Visibility.Collapsed;
                        btnColor.Visibility = Visibility.Collapsed;
                        MateriaSettingUp.Visibility = Visibility.Collapsed;
                        //....................End.........................

                        gridHeadernumstu.Visibility = Visibility.Collapsed;
                        string accessUrl = Properties.Settings.Default.acessUrl;
                        string token = Properties.Settings.Default.Token;
                        var respone = await RESTApiHelper.GetAll(accessUrl, "/academic-year-structure/national", token);
                        var obj = JObject.Parse(respone).ToObject<YearofAcademy>().data;
                        Properties.Settings.Default.schoolAcademyYear = respone;
                        Properties.Settings.Default.Save();
                        List<Year> year = new List<Year>();
                        foreach (var item in obj)
                        {
                            year.Add(new Year { name = item.name });
                        }
                        cbAcademyYear.ItemsSource = year;
                        cbAcademyYear.DisplayMemberPath = "name";
                        cbAcademyYear.SelectedValuePath = "name";

                        //////////////////////////////////
                        ///
                        cbAcademyYearAtt.ItemsSource = year;
                        cbAcademyYearAtt.DisplayMemberPath = "name";
                        cbAcademyYearAtt.SelectedValuePath = "name";
                    }
                    else
                    {
                        var obj = JObject.Parse(Properties.Settings.Default.schoolAcademyYear).ToObject<YearofAcademy>().data;
                        List<Year> year = new List<Year>();
                        foreach (var item in obj)
                        {
                            year.Add(new Year { name = item.name });
                        }

                        cbAcademyYear.ItemsSource = year;
                        cbAcademyYear.DisplayMemberPath = "name";
                        cbAcademyYear.SelectedValuePath = "name";

                        ///////////////////////////////////
                        ///
                        cbAcademyYearAtt.ItemsSource = year;
                        cbAcademyYearAtt.DisplayMemberPath = "name";
                        cbAcademyYearAtt.SelectedValuePath = "name";
                    }
                    //end Load Year of Academy
                    //------Loading-----
                    loading.Close();
                    this.IsEnabled = true;
                    //----end loading----
                }
                else if (Properties.Settings.Default.role == "2" || Properties.Settings.Default.role == "3")
                {
                    PartTeacher partTeacher = new PartTeacher();
                    partTeacher.Show();
                    this.Close();
                }
            }
            else
            {
                Login login = new Login();
                this.Close();
                login.Show();
                Properties.Settings.Default.checkLoginOrLogut = "logout";
                Properties.Settings.Default.Save();
            }
        }

        bool check = true;


        //slide Menu.........

        private void slideLeft_MouseEnter(object sender, MouseEventArgs e)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 45;
            animation.To = 320;
            animation.Duration = TimeSpan.FromMilliseconds(150);
            slideLeft.BeginAnimation(WidthProperty, animation);
            check = true;
            gridAcc.Visibility = Visibility.Visible;
            lblnameCompany.Visibility = Visibility.Visible;
            gridfeature.Margin = new Thickness(0, 0, 0, 0);
            MateriaSettingDrop.Visibility = Visibility.Visible;
            //btnHome.IsEnabled = true;
            MateriaLangDrop.Visibility = Visibility.Visible;
            MateriaSettingDrop.Visibility = Visibility.Visible;
        }

        private void slideLeft_MouseLeave(object sender, MouseEventArgs e)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 320;
            animation.To = 45;
            animation.Duration = TimeSpan.FromMilliseconds(150);
            slideLeft.BeginAnimation(WidthProperty, animation);
            check = false;
            gridAcc.Visibility = Visibility.Collapsed;
            lblnameCompany.Visibility = Visibility.Collapsed;
            gridfeature.Margin = new Thickness(0, 0, 0, 0);


            //btnHome.IsEnabled = false;

            //..................Part Print....................
            MateriaFeaprintdrop.Visibility = Visibility.Visible;
            MateriaFeaprintUp.Visibility = Visibility.Collapsed;
            btnTeacher.Visibility = Visibility.Collapsed;
            btnStudent.Visibility = Visibility.Collapsed;


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
        private void gridMenu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //if (slideLeft.Width == 45)
            //{
            //    DoubleAnimation animation = new DoubleAnimation();
            //    animation.From = 45;
            //    animation.To = 320;
            //    animation.Duration = TimeSpan.FromMilliseconds(150);
            //    slideLeft.BeginAnimation(WidthProperty, animation);
            //    check = true;
            //    gridAcc.Visibility = Visibility.Visible;
            //    lblnameCompany.Visibility = Visibility.Visible;
            //    gridfeature.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaSettingDrop.Visibility = Visibility.Visible;
            //    //btnHome.IsEnabled = true;
            //    MateriaLangDrop.Visibility = Visibility.Visible;
            //    MateriaSettingDrop.Visibility = Visibility.Visible;

            //    //  ...............Margin Button.........................
            //    btnStaffAttendanceReport.Margin = new Thickness(10, 10, 10, 5);
            //    btnStudentAttendanceReport.Margin = new Thickness(10, -3, 10, 5);
            //    btnStudentLearningRsult.Margin = new Thickness(10, -3, 10, 5);
            //    btnClassSchedule.Margin = new Thickness(10, -3, 10, 5);
            //    btnTeacherSchedule.Margin = new Thickness(10, -3, 10, 5);
            //    btnStudentList.Margin = new Thickness(10, -3, 10, 5);
            //    btnSubJectList.Margin = new Thickness(10, -3, 10, 5);
            //}
            //else
            //{

            //    DoubleAnimation animation = new DoubleAnimation();
            //    animation.From = 320;
            //    animation.To = 45;
            //    animation.Duration = TimeSpan.FromMilliseconds(150);
            //    slideLeft.BeginAnimation(WidthProperty, animation);
            //    check = false;
            //    gridAcc.Visibility = Visibility.Collapsed;
            //    lblnameCompany.Visibility = Visibility.Collapsed;
            //    gridfeature.Margin = new Thickness(0, 0, 0, 0);


            //    //btnHome.IsEnabled = false;

            //    //..................Part Print....................
            //    MateriaFeaprintdrop.Visibility = Visibility.Visible;
            //    MateriaFeaprintUp.Visibility = Visibility.Collapsed;
            //    btnTeacher.Visibility = Visibility.Collapsed;
            //    btnStudent.Visibility = Visibility.Collapsed;


            //    ///.................Part Setting...................
            //    btnAbout.Visibility = Visibility.Collapsed;
            //    btnColor.Visibility = Visibility.Collapsed;
            //    MateriaSettingUp.Visibility = Visibility.Collapsed;
            //    btnLang.Visibility = Visibility.Collapsed;
            //    gridLanguage.Visibility = Visibility.Collapsed;
            //    MateriaLangDrop.Visibility = Visibility.Visible;
            //    MateriaSettingDrop.Visibility = Visibility.Visible;
            //    //....................End.........................

            //    btnStaffAttendanceReport.Margin = new Thickness(5, 10, 5, 5);
            //    btnStudentAttendanceReport.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentLearningRsult.Margin = new Thickness(5, -3, 5, 5);
            //    btnClassSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnTeacherSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentList.Margin = new Thickness(5, -3, 5, 5);
            //    btnSubJectList.Margin = new Thickness(5, -3, 5, 5);

            //}
            //checkButtonClick = true;
            //checkButtonClick = true;
        }

        //Account.........
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Account account = new Account();
            account.Owner = this;
            account.ShowDialog();
        }
        //TabControl................
        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            tabMenu.SelectedIndex = 0;
        }
        string dateForStaffAtt = "";
        private async void btnStaffAttendanceReport_Click(object sender, RoutedEventArgs e)
        {
            tabMenu.SelectedIndex = 1;
            gridAcc.Visibility = Visibility.Collapsed;
            var bc = new BrushConverter();
            //___________________Change  button State______________________
            btnStaffAttendanceReport.Background = Brushes.White;
            Tiltle.Content = Properties.Langs.Lang.Staff_Att_Re;
            btnStudentAttendanceReport.Background = Brushes.Transparent;
            btnStudentLearningRsult.Background = Brushes.Transparent;
            btnClassSchedule.Background = Brushes.Transparent;
            btnTeacherSchedule.Background = Brushes.Transparent;
            btnStudentList.Background = Brushes.Transparent;
            btnSubJectList.Background = Brushes.Transparent;
            btnSetting.Background = Brushes.Transparent;


            MateriaStaffAtt.Foreground = (Brush)bc.ConvertFrom("#1183CA");
            lblStaffAtt.Foreground = (Brush)bc.ConvertFrom("#1183CA");

            MateriaStuAtt.Foreground = Brushes.White;
            lblStuAtt.Foreground = Brushes.White;

            MateriaStuLearn.Foreground = Brushes.White;
            lblStuLearn.Foreground = Brushes.White;

            MateriaClassSche.Foreground = Brushes.White;
            lblClassSched.Foreground = Brushes.White;

            MateriaTeacherSche.Foreground = Brushes.White;
            lblTeacherSche.Foreground = Brushes.White;

            MateriaStu.Foreground = Brushes.White;
            lblStuList.Foreground = Brushes.White;

            MateriaSubj.Foreground = Brushes.White;
            lblSubject.Foreground = Brushes.White;

            MateriaSetting.Foreground = Brushes.White;
            lblSetting.Foreground = Brushes.White;
            MateriaSettingUp.Foreground = Brushes.White;
            MateriaSettingDrop.Foreground = Brushes.White;
            //________________End__________________________________________
           
            gridAcc.Visibility = Visibility.Visible;
            lblnameCompany.Visibility = Visibility.Visible;
            gridfeature.Margin = new Thickness(0, 0, 0, 0);
            MateriaSettingDrop.Visibility = Visibility.Visible;
            MateriaSettingUp.Visibility = Visibility.Collapsed;

            //..................Part Print....................
            MateriaFeaprintdrop.Visibility = Visibility.Visible;
            MateriaFeaprintUp.Visibility = Visibility.Collapsed;
            btnTeacher.Visibility = Visibility.Collapsed;
            btnStudent.Visibility = Visibility.Collapsed;


            ///.................Part Setting...................
            btnAbout.Visibility = Visibility.Collapsed;
            btnColor.Visibility = Visibility.Collapsed;
            MateriaSettingUp.Visibility = Visibility.Collapsed;
            btnLang.Visibility = Visibility.Collapsed;
            gridLanguage.Visibility = Visibility.Collapsed;
            //....................End.........................

            Loading loading = new Loading();
            loading.Owner = this;
            loading.Show();
            this.sender = sender;
            this.e = e;
            
            var accessUrl = Properties.Settings.Default.acessUrl;
            var token = Properties.Settings.Default.Token;

            
            dateForStaffAtt = DPStaffAtt.SelectedDate.Value.ToString("dd/MM/yyyy");

            if(InternetChecker())
            {
                int i = 1;
                var respone = await RESTApiHelper.GetAll(accessUrl, "/staff-attendance-permission?date=" + dateForStaffAtt, token);
                var respone1 = await RESTApiHelper.GetAll(accessUrl, "/get-daily-staff-attendance-report?date=" + dateForStaffAtt, token);
                var obj = JObject.Parse(respone).ToObject<StaffPermissionList>().data;
                var obj1 = JObject.Parse(respone1).ToObject<StaffAttendanceDailyList>().data;
                listAttStaff = JObject.Parse(respone1).ToObject<StaffAttendanceDailyList>();
                foreach (var item in obj)
                {
                    item.number = i.ToString();
                    if (item.is_approve == null)
                    {
                        item.approved_at = "";
                        item.visble = "Visible";
                    }
                    else
                    {
                        item.visble = "Collapsed";
                    }
                    i++;
                }
                i = 1;
                foreach (var item in listAttStaff.data)
                {
                    requestIdPermission = item.id;
                    staffName.Add(item.name);
                    item.number = i.ToString();
                    if (item.gender == "1")
                        item.gender = "ប្រុស";
                    else
                        item.gender = "ស្រី";
                    if (!item.daily_present.morning.in_time.Equals(""))
                        item.mIn = "Red";
                    if (!item.daily_present.morning.out_time.Equals(""))
                        item.mOut = "Red";
                    if (!item.daily_present.afternoon.out_time.Equals(""))
                        item.aOut = "Red";
                    if (!item.daily_present.afternoon.in_time.Equals(""))
                        item.aIn = "Red";

                    i++;
                }
                DGStaffAtt.ItemsSource = obj;
                DGStaffAtt1.ItemsSource = listAttStaff.data;

                staffs = obj1;           
            }

            gridAcc.Visibility = Visibility.Collapsed;
            check = false;
            loading.Close();
        }

        private void btnStudentAttendanceReport_Click_1(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();


            //___________________Change  button State______________________
            btnStaffAttendanceReport.Background = Brushes.Transparent;
            btnStudentAttendanceReport.Background = Brushes.White;
            btnStudentLearningRsult.Background = Brushes.Transparent;
            btnClassSchedule.Background = Brushes.Transparent;
            btnTeacherSchedule.Background = Brushes.Transparent;
            btnStudentList.Background = Brushes.Transparent;
            btnSubJectList.Background = Brushes.Transparent;
            btnSetting.Background = Brushes.Transparent;


            MateriaStaffAtt.Foreground = Brushes.White;
            lblStaffAtt.Foreground = Brushes.White;

            MateriaStuAtt.Foreground = (Brush)bc.ConvertFrom("#1183CA");
            lblStuAtt.Foreground = (Brush)bc.ConvertFrom("#1183CA");

            MateriaStuLearn.Foreground = Brushes.White;
            lblStuLearn.Foreground = Brushes.White;

            MateriaClassSche.Foreground = Brushes.White;
            lblClassSched.Foreground = Brushes.White;

            MateriaTeacherSche.Foreground = Brushes.White;
            lblTeacherSche.Foreground = Brushes.White;

            MateriaStu.Foreground = Brushes.White;
            lblStuList.Foreground = Brushes.White;

            MateriaSubj.Foreground = Brushes.White;
            lblSubject.Foreground = Brushes.White;

            MateriaSetting.Foreground = Brushes.White;
            lblSetting.Foreground = Brushes.White;
            MateriaSettingUp.Foreground = Brushes.White;
            MateriaSettingDrop.Foreground = Brushes.White;
            //________________End__________________________________________
            tabMenu.SelectedIndex = 2;

            //if (slideLeft.Width == 320)
            //{
            //    DoubleAnimation animation = new DoubleAnimation();
            //    animation.From = 320;
            //    animation.To = 45;
            //    animation.Duration = TimeSpan.FromMilliseconds(150);
            //    slideLeft.BeginAnimation(WidthProperty, animation);
            //    checkButtonClick = false;
            gridAcc.Visibility = Visibility.Visible;
            lblnameCompany.Visibility = Visibility.Visible;
            gridfeature.Margin = new Thickness(0, 0, 0, 0);
            MateriaSettingDrop.Visibility = Visibility.Visible;

            //..................Part Print....................
            MateriaFeaprintdrop.Visibility = Visibility.Visible;
            MateriaFeaprintUp.Visibility = Visibility.Collapsed;
            btnTeacher.Visibility = Visibility.Collapsed;
            btnStudent.Visibility = Visibility.Collapsed;


            ///.................Part Setting...................
            btnAbout.Visibility = Visibility.Collapsed;
            btnColor.Visibility = Visibility.Collapsed;
            MateriaSettingUp.Visibility = Visibility.Collapsed;
            btnLang.Visibility = Visibility.Collapsed;
            gridLanguage.Visibility = Visibility.Collapsed;
            //....................End.........................

            //btnStaffAttendanceReport.Margin = new Thickness(5, 10, 5, 5);
            //btnStudentAttendanceReport.Margin = new Thickness(5, -3, 5, 5);
            //btnStudentLearningRsult.Margin = new Thickness(5, -3, 5, 5);
            //btnClassSchedule.Margin = new Thickness(5, -3, 5, 5);
            //btnTeacherSchedule.Margin = new Thickness(5, -3, 5, 5);
            //btnStudentList.Margin = new Thickness(5, -3, 5, 5);
            //btnSubJectList.Margin = new Thickness(5, -3, 5, 5);
            //}
            //else
            //{
            //    DoubleAnimation animation = new DoubleAnimation();
            //    animation.From = 45;
            //    animation.To = 45;
            //    animation.Duration = TimeSpan.FromMilliseconds(150);
            //    slideLeft.BeginAnimation(WidthProperty, animation);
            //    MateriaSettingDrop.Visibility = Visibility.Visible;
            //}

            //if (scrolStuatt.IsVisible == true)
            //{
            //    DGStudentAtt.Margin = new Thickness(0, -36, 0, 0);
            //}
            //else
            //{
            //    DGStudentAtt.Margin = new Thickness(0, -36, 17, 0);
            //}

            check = false;
            //btnHome.IsEnabled = false;
            Tiltle.Content = Properties.Langs.Lang.Stu_Att_Re;
        }


        private void btnStudentLearningRsult_Click(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();

            //___________________Change  button State______________________
            btnStaffAttendanceReport.Background = Brushes.Transparent;
            btnStudentAttendanceReport.Background = Brushes.Transparent;
            btnStudentLearningRsult.Background = Brushes.White;
            btnClassSchedule.Background = Brushes.Transparent;
            btnTeacherSchedule.Background = Brushes.Transparent;
            btnStudentList.Background = Brushes.Transparent;
            btnSubJectList.Background = Brushes.Transparent;
            btnSetting.Background = Brushes.Transparent;


            MateriaStaffAtt.Foreground = Brushes.White;
            lblStaffAtt.Foreground = Brushes.White;

            MateriaStuAtt.Foreground = Brushes.White;
            lblStuAtt.Foreground = Brushes.White;

            MateriaStuLearn.Foreground = (Brush)bc.ConvertFrom("#1183CA");
            lblStuLearn.Foreground = (Brush)bc.ConvertFrom("#1183CA");

            MateriaClassSche.Foreground = Brushes.White;
            lblClassSched.Foreground = Brushes.White;

            MateriaTeacherSche.Foreground = Brushes.White;
            lblTeacherSche.Foreground = Brushes.White;

            MateriaStu.Foreground = Brushes.White;
            lblStuList.Foreground = Brushes.White;

            MateriaSubj.Foreground = Brushes.White;
            lblSubject.Foreground = Brushes.White;

            MateriaSetting.Foreground = Brushes.White;
            lblSetting.Foreground = Brushes.White;
            MateriaSettingUp.Foreground = Brushes.White;
            MateriaSettingDrop.Foreground = Brushes.White;
            //________________End__________________________________________
            tabMenu.SelectedIndex = 3;
            tabStudentResult.SelectedIndex = 0;

            //if (slideLeft.Width == 320)
            //{
            //    DoubleAnimation animation = new DoubleAnimation();
            //    animation.From = 320;
            //    animation.To = 45;
            //    animation.Duration = TimeSpan.FromMilliseconds(150);
            //    slideLeft.BeginAnimation(WidthProperty, animation);
            //    checkButtonClick = false;
            gridAcc.Visibility = Visibility.Visible;
            lblnameCompany.Visibility = Visibility.Visible;
            gridfeature.Margin = new Thickness(0, 0, 0, 0);
            MateriaSettingDrop.Visibility = Visibility.Visible;

            //..................Part Print....................
            MateriaFeaprintdrop.Visibility = Visibility.Visible;
            MateriaFeaprintUp.Visibility = Visibility.Collapsed;
            btnTeacher.Visibility = Visibility.Collapsed;
            btnStudent.Visibility = Visibility.Collapsed;


            ///.................Part Setting...................
            btnAbout.Visibility = Visibility.Collapsed;
            btnColor.Visibility = Visibility.Collapsed;
            MateriaSettingUp.Visibility = Visibility.Collapsed;
            btnLang.Visibility = Visibility.Collapsed;
            gridLanguage.Visibility = Visibility.Collapsed;
            //....................End.........................

            //    btnStaffAttendanceReport.Margin = new Thickness(5, 10, 5, 5);
            //    btnStudentAttendanceReport.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentLearningRsult.Margin = new Thickness(5, -3, 5, 5);
            //    btnClassSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnTeacherSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentList.Margin = new Thickness(5, -3, 5, 5);
            //    btnSubJectList.Margin = new Thickness(5, -3, 5, 5);
            //}
            //else
            //{
            //    DoubleAnimation animation = new DoubleAnimation();
            //    animation.From = 45;
            //    animation.To = 45;
            //    animation.Duration = TimeSpan.FromMilliseconds(150);
            //    slideLeft.BeginAnimation(WidthProperty, animation);
            //    MateriaSettingDrop.Visibility = Visibility.Visible;
            //}
            //check = false;
            //btnHome.IsEnabled = false;
            Tiltle.Content = Properties.Langs.Lang.Stu_Learn_Result;

        }

        private void btnClassSchedule_Click(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            //___________________Change  button State______________________
            btnStaffAttendanceReport.Background = Brushes.Transparent;
            btnStudentAttendanceReport.Background = Brushes.Transparent;
            btnStudentLearningRsult.Background = Brushes.Transparent;
            btnClassSchedule.Background = Brushes.White;
            btnTeacherSchedule.Background = Brushes.Transparent;
            btnStudentList.Background = Brushes.Transparent;
            btnSubJectList.Background = Brushes.Transparent;
            btnSetting.Background = Brushes.Transparent;


            MateriaStaffAtt.Foreground = Brushes.White;
            lblStaffAtt.Foreground = Brushes.White;

            MateriaStuAtt.Foreground = Brushes.White;
            lblStuAtt.Foreground = Brushes.White;

            MateriaStuLearn.Foreground = Brushes.White;
            lblStuLearn.Foreground = Brushes.White;

            MateriaClassSche.Foreground = (Brush)bc.ConvertFrom("#1183CA");
            lblClassSched.Foreground = (Brush)bc.ConvertFrom("#1183CA");

            MateriaTeacherSche.Foreground = Brushes.White;
            lblTeacherSche.Foreground = Brushes.White;

            MateriaStu.Foreground = Brushes.White;
            lblStuList.Foreground = Brushes.White;

            MateriaSubj.Foreground = Brushes.White;
            lblSubject.Foreground = Brushes.White;

            MateriaSetting.Foreground = Brushes.White;
            lblSetting.Foreground = Brushes.White;
            MateriaSettingUp.Foreground = Brushes.White;
            MateriaSettingDrop.Foreground = Brushes.White;
            //________________End__________________________________________
            tabMenu.SelectedIndex = 4;

            //if (slideLeft.Width == 320)
            //{
            //    DoubleAnimation animation = new DoubleAnimation();
            //    animation.From = 320;
            //    animation.To = 45;
            //    animation.Duration = TimeSpan.FromMilliseconds(150);
            //    slideLeft.BeginAnimation(WidthProperty, animation);
            //    checkButtonClick = false;
            gridAcc.Visibility = Visibility.Visible;
            lblnameCompany.Visibility = Visibility.Visible;
            gridfeature.Margin = new Thickness(0, 0, 0, 0);
            MateriaSettingDrop.Visibility = Visibility.Visible;

            //..................Part Print....................
            MateriaFeaprintdrop.Visibility = Visibility.Visible;
            MateriaFeaprintUp.Visibility = Visibility.Collapsed;
            btnTeacher.Visibility = Visibility.Collapsed;
            btnStudent.Visibility = Visibility.Collapsed;


            ///.................Part Setting...................
            btnAbout.Visibility = Visibility.Collapsed;
            btnColor.Visibility = Visibility.Collapsed;
            MateriaSettingUp.Visibility = Visibility.Collapsed;
            btnLang.Visibility = Visibility.Collapsed;
            gridLanguage.Visibility = Visibility.Collapsed;
            //....................End.........................

            //    btnStaffAttendanceReport.Margin = new Thickness(5, 10, 5, 5);
            //    btnStudentAttendanceReport.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentLearningRsult.Margin = new Thickness(5, -3, 5, 5);
            //    btnClassSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnTeacherSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentList.Margin = new Thickness(5, -3, 5, 5);
            //    btnSubJectList.Margin = new Thickness(5, -3, 5, 5);
            //}
            //else
            //{
            //    DoubleAnimation animation = new DoubleAnimation();
            //    animation.From = 45;
            //    animation.To = 45;
            //    animation.Duration = TimeSpan.FromMilliseconds(150);
            //    slideLeft.BeginAnimation(WidthProperty, animation);
            //    MateriaSettingDrop.Visibility = Visibility.Visible;
            //}
            //check = false;
            //btnHome.IsEnabled = false;
            Tiltle.Content = Properties.Langs.Lang.Cl_Sch;
        }

        private void btnTeacherSchedule_Click(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();

            //___________________Change  button State______________________
            btnStaffAttendanceReport.Background = Brushes.Transparent;
            btnStudentAttendanceReport.Background = Brushes.Transparent;
            btnStudentLearningRsult.Background = Brushes.Transparent;
            btnClassSchedule.Background = Brushes.Transparent;
            btnTeacherSchedule.Background = Brushes.White;
            btnStudentList.Background = Brushes.Transparent;
            btnSubJectList.Background = Brushes.Transparent;
            btnSetting.Background = Brushes.Transparent;


            MateriaStaffAtt.Foreground = Brushes.White;
            lblStaffAtt.Foreground = Brushes.White;

            MateriaStuAtt.Foreground = Brushes.White;
            lblStuAtt.Foreground = Brushes.White;

            MateriaStuLearn.Foreground = Brushes.White;
            lblStuLearn.Foreground = Brushes.White;

            MateriaClassSche.Foreground = Brushes.White;
            lblClassSched.Foreground = Brushes.White;

            MateriaTeacherSche.Foreground = (Brush)bc.ConvertFrom("#1183CA");
            lblTeacherSche.Foreground = (Brush)bc.ConvertFrom("#1183CA");

            MateriaStu.Foreground = Brushes.White;
            lblStuList.Foreground = Brushes.White;

            MateriaSubj.Foreground = Brushes.White;
            lblSubject.Foreground = Brushes.White;

            MateriaSetting.Foreground = Brushes.White;
            lblSetting.Foreground = Brushes.White;
            MateriaSettingUp.Foreground = Brushes.White;
            MateriaSettingDrop.Foreground = Brushes.White;
            //________________End__________________________________________
            tabMenu.SelectedIndex = 5;
            //if (slideLeft.Width == 320)
            //{
            //    DoubleAnimation animation = new DoubleAnimation();
            //    animation.From = 320;
            //    animation.To = 45;
            //    animation.Duration = TimeSpan.FromMilliseconds(150);
            //    slideLeft.BeginAnimation(WidthProperty, animation);
            //    checkButtonClick = false;
            gridAcc.Visibility = Visibility.Visible;
            lblnameCompany.Visibility = Visibility.Visible;
            gridfeature.Margin = new Thickness(0, 0, 0, 0);
            MateriaSettingDrop.Visibility = Visibility.Visible;

            //..................Part Print....................
            MateriaFeaprintdrop.Visibility = Visibility.Visible;
            MateriaFeaprintUp.Visibility = Visibility.Collapsed;
            btnTeacher.Visibility = Visibility.Collapsed;
            btnStudent.Visibility = Visibility.Collapsed;


            ///.................Part Setting...................
            btnAbout.Visibility = Visibility.Collapsed;
            btnColor.Visibility = Visibility.Collapsed;
            MateriaSettingUp.Visibility = Visibility.Collapsed;
            btnLang.Visibility = Visibility.Collapsed;
            gridLanguage.Visibility = Visibility.Collapsed;
            //....................End.........................

            //    btnStaffAttendanceReport.Margin = new Thickness(5, 10, 5, 5);
            //    btnStudentAttendanceReport.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentLearningRsult.Margin = new Thickness(5, -3, 5, 5);
            //    btnClassSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnTeacherSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentList.Margin = new Thickness(5, -3, 5, 5);
            //    btnSubJectList.Margin = new Thickness(5, -3, 5, 5);
            //}
            //else
            //{
            //    DoubleAnimation animation = new DoubleAnimation();
            //    animation.From = 45;
            //    animation.To = 45;
            //    animation.Duration = TimeSpan.FromMilliseconds(150);
            //    slideLeft.BeginAnimation(WidthProperty, animation);
            //    MateriaSettingDrop.Visibility = Visibility.Visible;
            //}
            //check = false;
            //btnHome.IsEnabled = false;
            Tiltle.Content = Properties.Langs.Lang.Tea_Sche;
        }

        private void btnStudentList_Click(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();

            //___________________Change  button State______________________
            btnStaffAttendanceReport.Background = Brushes.Transparent;
            btnStudentAttendanceReport.Background = Brushes.Transparent;
            btnStudentLearningRsult.Background = Brushes.Transparent;
            btnClassSchedule.Background = Brushes.Transparent;
            btnTeacherSchedule.Background = Brushes.Transparent;
            btnStudentList.Background = Brushes.White;
            btnSubJectList.Background = Brushes.Transparent;
            btnSetting.Background = Brushes.Transparent;


            MateriaStaffAtt.Foreground = Brushes.White;
            lblStaffAtt.Foreground = Brushes.White;

            MateriaStuAtt.Foreground = Brushes.White;
            lblStuAtt.Foreground = Brushes.White;

            MateriaStuLearn.Foreground = Brushes.White;
            lblStuLearn.Foreground = Brushes.White;

            MateriaClassSche.Foreground = Brushes.White;
            lblClassSched.Foreground = Brushes.White;

            MateriaTeacherSche.Foreground = Brushes.White;
            lblTeacherSche.Foreground = Brushes.White;

            MateriaStu.Foreground = (Brush)bc.ConvertFrom("#1183CA");
            lblStuList.Foreground = (Brush)bc.ConvertFrom("#1183CA");

            MateriaSubj.Foreground = Brushes.White;
            lblSubject.Foreground = Brushes.White;

            MateriaSetting.Foreground = Brushes.White;
            lblSetting.Foreground = Brushes.White;
            MateriaSettingUp.Foreground = Brushes.White;
            MateriaSettingDrop.Foreground = Brushes.White;
            //________________End__________________________________________
            tabMenu.SelectedIndex = 6;

            //if (slideLeft.Width == 300)
            //{
            //    DoubleAnimation animation = new DoubleAnimation();
            //    animation.From = 300;
            //    animation.To = 50;
            //    animation.Duration = TimeSpan.FromMilliseconds(150);
            //    slideLeft.BeginAnimation(WidthProperty, animation);
            //    checkButtonClick = false;
            gridAcc.Visibility = Visibility.Visible;
            lblnameCompany.Visibility = Visibility.Visible;
            gridfeature.Margin = new Thickness(0, 0, 0, 0);
            MateriaSettingDrop.Visibility = Visibility.Visible;

            //..................Part Print....................
            MateriaFeaprintdrop.Visibility = Visibility.Visible;
            MateriaFeaprintUp.Visibility = Visibility.Collapsed;
            btnTeacher.Visibility = Visibility.Collapsed;
            btnStudent.Visibility = Visibility.Collapsed;


            ///.................Part Setting...................
            btnAbout.Visibility = Visibility.Collapsed;
            btnColor.Visibility = Visibility.Collapsed;
            MateriaSettingUp.Visibility = Visibility.Collapsed;
            btnLang.Visibility = Visibility.Collapsed;
            gridLanguage.Visibility = Visibility.Collapsed;
            //....................End.........................

            //    btnStaffAttendanceReport.Margin = new Thickness(5, 10, 5, 5);
            //    btnStudentAttendanceReport.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentLearningRsult.Margin = new Thickness(5, -3, 5, 5);
            //    btnClassSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnTeacherSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentList.Margin = new Thickness(5, -3, 5, 5);
            //    btnSubJectList.Margin = new Thickness(5, -3, 5, 5);
            //}
            //else
            //{
            //    DoubleAnimation animation = new DoubleAnimation();
            //    animation.From = 45;
            //    animation.To = 45;
            //    animation.Duration = TimeSpan.FromMilliseconds(150);
            //    slideLeft.BeginAnimation(WidthProperty, animation);
            //    MateriaSettingDrop.Visibility = Visibility.Visible;
            //}
            //check = false;
            //btnHome.IsEnabled = false;
            Tiltle.Content = Properties.Langs.Lang.Stu_List;
        }
        private void btnSubJectList_Click(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();

            //___________________Change  button State______________________
            btnStaffAttendanceReport.Background = Brushes.Transparent;
            btnStudentAttendanceReport.Background = Brushes.Transparent;
            btnStudentLearningRsult.Background = Brushes.Transparent;
            btnClassSchedule.Background = Brushes.Transparent;
            btnTeacherSchedule.Background = Brushes.Transparent;
            btnStudentList.Background = Brushes.Transparent;
            btnSubJectList.Background = Brushes.White;
            btnSetting.Background = Brushes.Transparent;

            MateriaStaffAtt.Foreground = Brushes.White;
            lblStaffAtt.Foreground = Brushes.White;

            MateriaStuAtt.Foreground = Brushes.White;
            lblStuAtt.Foreground = Brushes.White;

            MateriaStuLearn.Foreground = Brushes.White;
            lblStuLearn.Foreground = Brushes.White;

            MateriaClassSche.Foreground = Brushes.White;
            lblClassSched.Foreground = Brushes.White;

            MateriaTeacherSche.Foreground = Brushes.White;
            lblTeacherSche.Foreground = Brushes.White;

            MateriaStu.Foreground = Brushes.White;
            lblStuList.Foreground = Brushes.White;

            MateriaSubj.Foreground = (Brush)bc.ConvertFrom("#1183CA");
            lblSubject.Foreground = (Brush)bc.ConvertFrom("#1183CA");

            MateriaSetting.Foreground = Brushes.White;
            lblSetting.Foreground = Brushes.White;
            MateriaSettingUp.Foreground = Brushes.White;
            MateriaSettingDrop.Foreground = Brushes.White;
            //________________End__________________________________________
            tabMenu.SelectedIndex = 7;

            //if (slideLeft.Width == 320)
            //{
            //    DoubleAnimation animation = new DoubleAnimation();
            //    animation.From = 320;
            //    animation.To = 45;
            //    animation.Duration = TimeSpan.FromMilliseconds(150);
            //    slideLeft.BeginAnimation(WidthProperty, animation);
            //    checkButtonClick = false;
            gridAcc.Visibility = Visibility.Visible;
            lblnameCompany.Visibility = Visibility.Visible;
            gridfeature.Margin = new Thickness(0, 0, 0, 0);
            MateriaSettingDrop.Visibility = Visibility.Visible;

            //..................Part Print....................
            MateriaFeaprintdrop.Visibility = Visibility.Visible;
            MateriaFeaprintUp.Visibility = Visibility.Collapsed;
            btnTeacher.Visibility = Visibility.Collapsed;
            btnStudent.Visibility = Visibility.Collapsed;


            ///.................Part Setting...................
            btnAbout.Visibility = Visibility.Collapsed;
            btnColor.Visibility = Visibility.Collapsed;
            MateriaSettingUp.Visibility = Visibility.Collapsed;
            btnLang.Visibility = Visibility.Collapsed;
            gridLanguage.Visibility = Visibility.Collapsed;
            //....................End.........................

            //    btnStaffAttendanceReport.Margin = new Thickness(5, 10, 5, 5);
            //    btnStudentAttendanceReport.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentLearningRsult.Margin = new Thickness(5, -3, 5, 5);
            //    btnClassSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnTeacherSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentList.Margin = new Thickness(5, -3, 5, 5);
            //    btnSubJectList.Margin = new Thickness(5, -3, 5, 5);
            //}
            //else
            //{
            //    DoubleAnimation animation = new DoubleAnimation();
            //    animation.From = 45;
            //    animation.To = 45;
            //    animation.Duration = TimeSpan.FromMilliseconds(150);
            //    slideLeft.BeginAnimation(WidthProperty, animation);
            //    MateriaSettingDrop.Visibility = Visibility.Visible;
            //}
            //check = false;
            //btnHome.IsEnabled = false;
            Tiltle.Content = Properties.Langs.Lang.Sub_List;
        }


        //..................................Part Feature Print..................Collab.
        private void btnFeaPrint_Click(object sender, RoutedEventArgs e)
        {
            btnStaffAttendanceReport.Background = Brushes.Transparent;
            btnStudentAttendanceReport.Background = Brushes.Transparent;
            btnStudentLearningRsult.Background = Brushes.Transparent;
            btnClassSchedule.Background = Brushes.Transparent;
            btnTeacherSchedule.Background = Brushes.Transparent;
            btnStudentList.Background = Brushes.Transparent;
            btnSubJectList.Background = Brushes.Transparent;
            btnSetting.Background = Brushes.Transparent;

            MateriaStaffAtt.Foreground = Brushes.White;
            lblStaffAtt.Foreground = Brushes.White;

            MateriaStuAtt.Foreground = Brushes.White;
            lblStuAtt.Foreground = Brushes.White;

            MateriaStuLearn.Foreground = Brushes.White;
            lblStuLearn.Foreground = Brushes.White;

            MateriaClassSche.Foreground = Brushes.White;
            lblClassSched.Foreground = Brushes.White;

            MateriaTeacherSche.Foreground = Brushes.White;
            lblTeacherSche.Foreground = Brushes.White;

            MateriaStu.Foreground = Brushes.White;
            lblStuList.Foreground = Brushes.White;

            MateriaSubj.Foreground = Brushes.White;
            lblSubject.Foreground = Brushes.White;

            MateriaSetting.Foreground = Brushes.White;
            lblSetting.Foreground = Brushes.White;
            MateriaSettingUp.Foreground = Brushes.White;
            MateriaSettingDrop.Foreground = Brushes.White;


            //if (slideLeft.Width == 320)
            //{
            //    DoubleAnimation animation = new DoubleAnimation();
            //    animation.From = 320;
            //    animation.To = 320;
            //    animation.Duration = TimeSpan.FromMilliseconds(150);
            //    slideLeft.BeginAnimation(WidthProperty, animation);
            //    checkButtonClick = false;
            gridAcc.Visibility = Visibility.Visible;
            lblnameCompany.Visibility = Visibility.Visible;



            //}
            //else
            //{
            //    DoubleAnimation animation = new DoubleAnimation();
            //    animation.From = 45;
            //    animation.To = 320;
            //    animation.Duration = TimeSpan.FromMilliseconds(150);
            //    slideLeft.BeginAnimation(WidthProperty, animation);
            //    gridAcc.Visibility = Visibility.Visible;
            //    lblnameCompany.Visibility = Visibility.Visible;


            ///.................Part Setting...................

            btnTeacher.Visibility = Visibility.Collapsed;
            MateriaFeaprintUp.Visibility = Visibility.Collapsed;
            btnStudent.Visibility = Visibility.Collapsed;
            gridLanguage.Visibility = Visibility.Collapsed;
            //....................End.........................
            //}
            check = false;

            if (MateriaFeaprintdrop.Visibility == Visibility.Visible)
            {

                btnTeacher.Visibility = Visibility.Collapsed;
                MateriaFeaprintUp.Visibility = Visibility.Visible;
                btnStudent.Visibility = Visibility.Visible;

                MateriaFeaprintdrop.Visibility = Visibility.Collapsed;


            }
            else
            {

                btnTeacher.Visibility = Visibility.Collapsed;
                MateriaFeaprintUp.Visibility = Visibility.Collapsed;
                btnStudent.Visibility = Visibility.Collapsed;

                MateriaFeaprintdrop.Visibility = Visibility.Visible;
                DoubleAnimation animation = new DoubleAnimation();
                animation.From = 320;
                animation.To = 320;
                animation.Duration = TimeSpan.FromMilliseconds(150);
                slideLeft.BeginAnimation(WidthProperty, animation);



            }
        }

        //......................Part Button in Feature Print....................
        private void btnStudent_Click(object sender, RoutedEventArgs e)
        {
            Tiltle.Content = Properties.Langs.Lang.print;
            tabMenu.SelectedIndex = 8;
            PrintStudent();

        }

        private void btnTeacher_Click(object sender, RoutedEventArgs e)
        {
            Tiltle.Content = Properties.Langs.Lang.print;
            tabMenu.SelectedIndex = 9;
            PrintStaff();
        }

        private void btnAdmin_Click(object sender, RoutedEventArgs e)
        {
            tabMenu.SelectedIndex = 10;
        }

        ///............................End.......................................



        /// <summary>
        /// ..................................Part Setting..................Collab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {

            //if (slideLeft.Width == 320)
            //{
            //    DoubleAnimation animation = new DoubleAnimation();
            //    animation.From = 320;
            //    animation.To = 320;
            //    animation.Duration = TimeSpan.FromMilliseconds(150);
            //    slideLeft.BeginAnimation(WidthProperty, animation);
            //    checkButtonClick = false;
            gridAcc.Visibility = Visibility.Visible;
            lblnameCompany.Visibility = Visibility.Visible;



            //}
            //else
            //{
            //    DoubleAnimation animation = new DoubleAnimation();
            //    animation.From = 45;
            //    animation.To = 320;
            //    animation.Duration = TimeSpan.FromMilliseconds(150);
            //    slideLeft.BeginAnimation(WidthProperty, animation);
            gridAcc.Visibility = Visibility.Visible;
            lblnameCompany.Visibility = Visibility.Visible;


            ///.................Part Setting...................
            btnAbout.Visibility = Visibility.Collapsed;
            btnColor.Visibility = Visibility.Collapsed;
            MateriaSettingUp.Visibility = Visibility.Collapsed;
            btnLang.Visibility = Visibility.Collapsed;
            gridLanguage.Visibility = Visibility.Collapsed;
            //....................End.........................
            //}
            check = false;

            if (MateriaSettingDrop.Visibility == Visibility.Visible)
            {
                btnAbout.Visibility = Visibility.Collapsed;
                btnColor.Visibility = Visibility.Collapsed;
                MateriaSettingUp.Visibility = Visibility.Visible;
                btnLang.Visibility = Visibility.Visible;

                MateriaSettingDrop.Visibility = Visibility.Collapsed;
                MateriaColorDrop.Visibility = Visibility.Visible;
                MateriaColorUp.Visibility = Visibility.Collapsed;
                MateriaLangUp.Visibility = Visibility.Collapsed;
                MateriaLangDrop.Visibility = Visibility.Visible;
            }
            else
            {
                btnAbout.Visibility = Visibility.Collapsed;
                btnColor.Visibility = Visibility.Collapsed;
                MateriaSettingUp.Visibility = Visibility.Collapsed;
                btnLang.Visibility = Visibility.Collapsed;
                gridLanguage.Visibility = Visibility.Collapsed;

                MateriaSettingDrop.Visibility = Visibility.Visible;
                MateriaColorDrop.Visibility = Visibility.Visible;

                //DoubleAnimation animation = new DoubleAnimation();
                //animation.From = 320;
                //animation.To = 320;
                //animation.Duration = TimeSpan.FromMilliseconds(150);
                //slideLeft.BeginAnimation(WidthProperty, animation);

            }
        }

        private void btnLang_Click(object sender, RoutedEventArgs e)
        {
            if (MateriaLangDrop.Visibility == Visibility.Visible)
            {
                gridLanguage.Visibility = Visibility.Visible;
                MateriaLangDrop.Visibility = Visibility.Collapsed;
                MateriaLangUp.Visibility = Visibility.Visible; ;
            }
            else
            {
                MateriaLangDrop.Visibility = Visibility.Visible;
                MateriaLangUp.Visibility = Visibility.Collapsed;
                gridLanguage.Visibility = Visibility.Collapsed;
            }
        }


        private void DockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }


        //Date Time Home Page.....
        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            txttime.Text = DateTime.Now.ToString("h:m:ss tt");
            //txtDate.Text = DateTime.Now.ToString("dddd  MMMM  dd  yyyy");

        }

        //stype popup button in menu..........

        private void btnStaffAttendanceReport_MouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();

            btnStaffAttendanceReport.Background = Brushes.White;
            MateriaStaffAtt.Foreground = (Brush)bc.ConvertFrom("#1183CA");
            lblStaffAtt.Foreground = (Brush)bc.ConvertFrom("#1183CA");

            //if (!check)
            //{

            //    popup_uc.PlacementTarget = btnStaffAttendanceReport;
            //    popup_uc.Placement = PlacementMode.Right;
            //    popup_uc.IsOpen = true;
            //    Header.PopupText.Text = Properties.Langs.Lang.Staff_Att_Re;

            //}

            //if (slideLeft.Width == 45)
            //{
            //    btnStaffAttendanceReport.Margin = new Thickness(5, 10, -2, 5);
            //    btnStudentAttendanceReport.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentLearningRsult.Margin = new Thickness(5, -3, 5, 5);
            //    btnClassSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnTeacherSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentList.Margin = new Thickness(5, -3, 5, 5);
            //    btnSubJectList.Margin = new Thickness(5, -3, 5, 5);

            //    MateriaStaffAtt.Margin = new Thickness(3, 0, 0, 0);
            //    MateriaStuAtt.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStuLearn.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaClassSche.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaTeacherSche.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStu.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaSubj.Margin = new Thickness(0, 0, 0, 0);
            //}

        }

        private void btnStaffAttendanceReport_MouseLeave(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            if (tabMenu.SelectedIndex == 1)
            {
                btnStaffAttendanceReport.Background = Brushes.White;
                MateriaStaffAtt.Foreground = (Brush)bc.ConvertFrom("#1183CA");
                lblStaffAtt.Foreground = (Brush)bc.ConvertFrom("#1183CA");
            }

            else
            {
                btnStaffAttendanceReport.Background = Brushes.Transparent;
                MateriaStaffAtt.Foreground = Brushes.White;
                lblStaffAtt.Foreground = Brushes.White;
            }


            //popup_uc.Visibility = Visibility.Collapsed;
            //popup_uc.IsOpen = false;

            //if (slideLeft.Width == 45)
            //{
            //    btnStaffAttendanceReport.Margin = new Thickness(5, 10, 5, 5);
            //    btnStudentAttendanceReport.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentLearningRsult.Margin = new Thickness(5, -3, 5, 5);
            //    btnClassSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnTeacherSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentList.Margin = new Thickness(5, -3, 5, 5);
            //    btnSubJectList.Margin = new Thickness(5, -3, 5, 5);

            //    MateriaStaffAtt.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStuAtt.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStuLearn.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaClassSche.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaTeacherSche.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStu.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaSubj.Margin = new Thickness(0, 0, 0, 0);
            //}  

        }

        private void btnStudentAttendanceReport_MouseEnter_1(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            btnStudentAttendanceReport.Background = Brushes.White;
            MateriaStuAtt.Foreground = (Brush)bc.ConvertFrom("#1183CA");
            lblStuAtt.Foreground = (Brush)bc.ConvertFrom("#1183CA");
            //if (!check)
            //{
            //    popup_uc.PlacementTarget = btnStudentAttendanceReport;
            //    popup_uc.Placement = PlacementMode.Right;
            //    popup_uc.IsOpen = true;
            //    Header.PopupText.Text = Properties.Langs.Lang.Stu_Att_Re;
            //}

            //if (slideLeft.Width == 45)
            //{
            //    btnStaffAttendanceReport.Margin = new Thickness(5, 10, 5, 5);
            //    btnStudentAttendanceReport.Margin = new Thickness(5, -3, -2, 5);
            //    btnStudentLearningRsult.Margin = new Thickness(5, -3, 5, 5);
            //    btnClassSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnTeacherSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentList.Margin = new Thickness(5, -3, 5, 5);
            //    btnSubJectList.Margin = new Thickness(5, -3, 5, 5);

            //    MateriaStaffAtt.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStuAtt.Margin = new Thickness(3, 0, 0, 0);
            //    MateriaStuLearn.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaClassSche.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaTeacherSche.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStu.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaSubj.Margin = new Thickness(0, 0, 0, 0);
            //}
        }

        private void btnStudentAttendanceReport_MouseLeave_1(object sender, MouseEventArgs e)
        {
            if (tabMenu.SelectedIndex == 2)
            {
                var bc = new BrushConverter();
                btnStudentAttendanceReport.Background = Brushes.White;
                MateriaStuAtt.Foreground = (Brush)bc.ConvertFrom("#1183CA");
                lblStuAtt.Foreground = (Brush)bc.ConvertFrom("#1183CA");
            }
            else
            {
                btnStudentAttendanceReport.Background = Brushes.Transparent;
                MateriaStuAtt.Foreground = Brushes.White;
                lblStuAtt.Foreground = Brushes.White;
            }
            //popup_uc.Visibility = Visibility.Collapsed;
            //popup_uc.IsOpen = false;


            //if (slideLeft.Width == 45)
            //{
            //    btnStaffAttendanceReport.Margin = new Thickness(5, 10, 5, 5);
            //    btnStudentAttendanceReport.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentLearningRsult.Margin = new Thickness(5, -3, 5, 5);
            //    btnClassSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnTeacherSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentList.Margin = new Thickness(5, -3, 5, 5);
            //    btnSubJectList.Margin = new Thickness(5, -3, 5, 5);

            //    MateriaStaffAtt.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStuAtt.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStuLearn.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaClassSche.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaTeacherSche.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStu.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaSubj.Margin = new Thickness(0, 0, 0, 0);
            //}

        }

        private void btnStudentLearningRsult_MouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            btnStudentLearningRsult.Background = Brushes.White;
            MateriaStuLearn.Foreground = (Brush)bc.ConvertFrom("#1183CA");
            lblStuLearn.Foreground = (Brush)bc.ConvertFrom("#1183CA");

            //if (!check)
            //{
            //    popup_uc.PlacementTarget = btnStudentLearningRsult;
            //    popup_uc.Placement = PlacementMode.Right;
            //    popup_uc.IsOpen = true;
            //    Header.PopupText.Text = Properties.Langs.Lang.Stu_Learn_Result;
            //}

            //if (slideLeft.Width == 45)
            //{
            //    btnStaffAttendanceReport.Margin = new Thickness(5, 10, 5, 5);
            //    btnStudentAttendanceReport.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentLearningRsult.Margin = new Thickness(5, -3, -2, 5);
            //    btnClassSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnTeacherSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentList.Margin = new Thickness(5, -3, 5, 5);
            //    btnSubJectList.Margin = new Thickness(5, -3, 5, 5);

            //    MateriaStaffAtt.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStuAtt.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStuLearn.Margin = new Thickness(3, 0, 0, 0);
            //    MateriaClassSche.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaTeacherSche.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStu.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaSubj.Margin = new Thickness(0, 0, 0, 0);
            //}
        }

        private void btnStudentLearningRsult_MouseLeave(object sender, MouseEventArgs e)
        {
            if (tabMenu.SelectedIndex == 3)
            {
                var bc = new BrushConverter();
                btnStudentLearningRsult.Background = Brushes.White;
                MateriaStuLearn.Foreground = (Brush)bc.ConvertFrom("#1183CA");
                lblStuLearn.Foreground = (Brush)bc.ConvertFrom("#1183CA");
            }
            else
            {
                btnStudentLearningRsult.Background = Brushes.Transparent;
                MateriaStuLearn.Foreground = Brushes.White;
                lblStuLearn.Foreground = Brushes.White;
            }

            //popup_uc.Visibility = Visibility.Collapsed;
            //popup_uc.IsOpen = false;

            //if (slideLeft.Width == 45)
            //{
            //    btnStaffAttendanceReport.Margin = new Thickness(5, 10, 5, 5);
            //    btnStudentAttendanceReport.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentLearningRsult.Margin = new Thickness(5, -3, 5, 5);
            //    btnClassSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnTeacherSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentList.Margin = new Thickness(5, -3, 5, 5);
            //    btnSubJectList.Margin = new Thickness(5, -3, 5, 5);

            //    MateriaStaffAtt.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStuAtt.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStuLearn.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaClassSche.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaTeacherSche.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStu.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaSubj.Margin = new Thickness(0, 0, 0, 0);
            //}

        }
        private void btnClassSchedule_MouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            btnClassSchedule.Background = Brushes.White;
            MateriaClassSche.Foreground = (Brush)bc.ConvertFrom("#1183CA");
            lblClassSched.Foreground = (Brush)bc.ConvertFrom("#1183CA");

            //if (!check)
            //{
            //    popup_uc.PlacementTarget = btnClassSchedule;
            //    popup_uc.Placement = PlacementMode.Right;
            //    popup_uc.IsOpen = true;
            //    Header.PopupText.Text = Properties.Langs.Lang.Cl_Sch;
            //}

            //if (slideLeft.Width == 45)
            //{
            //    btnStaffAttendanceReport.Margin = new Thickness(5, 10, 5, 5);
            //    btnStudentAttendanceReport.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentLearningRsult.Margin = new Thickness(5, -3, 5, 5);
            //    btnClassSchedule.Margin = new Thickness(5, -3, -2, 5);
            //    btnTeacherSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentList.Margin = new Thickness(5, -3, 5, 5);
            //    btnSubJectList.Margin = new Thickness(5, -3, 5, 5);

            //    MateriaStaffAtt.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStuAtt.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStuLearn.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaClassSche.Margin = new Thickness(3, 0, 0, 0);
            //    MateriaTeacherSche.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStu.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaSubj.Margin = new Thickness(0, 0, 0, 0);
            //}

        }

        private void btnClassSchedule_MouseLeave(object sender, MouseEventArgs e)
        {
            if (tabMenu.SelectedIndex == 4)
            {
                var bc = new BrushConverter();
                btnClassSchedule.Background = Brushes.White;
                MateriaClassSche.Foreground = (Brush)bc.ConvertFrom("#1183CA");
                lblClassSched.Foreground = (Brush)bc.ConvertFrom("#1183CA");
            }
            else
            {
                btnClassSchedule.Background = Brushes.Transparent;
                MateriaClassSche.Foreground = Brushes.White;
                lblClassSched.Foreground = Brushes.White;
            }

            //popup_uc.Visibility = Visibility.Collapsed;
            //popup_uc.IsOpen = false;

            //if (slideLeft.Width == 45)
            //{
            //    btnStaffAttendanceReport.Margin = new Thickness(5, 10, 5, 5);
            //    btnStudentAttendanceReport.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentLearningRsult.Margin = new Thickness(5, -3, 5, 5);
            //    btnClassSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnTeacherSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentList.Margin = new Thickness(5, -3, 5, 5);
            //    btnSubJectList.Margin = new Thickness(5, -3, 5, 5);

            //    MateriaStaffAtt.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStuAtt.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStuLearn.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaClassSche.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaTeacherSche.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStu.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaSubj.Margin = new Thickness(0, 0, 0, 0);
            //}

        }

        private void btnTeacherSchedule_MouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            btnTeacherSchedule.Background = Brushes.White;
            MateriaTeacherSche.Foreground = (Brush)bc.ConvertFrom("#1183CA");
            lblTeacherSche.Foreground = (Brush)bc.ConvertFrom("#1183CA");

            //if (!check)
            //{
            //    popup_uc.PlacementTarget = btnTeacherSchedule;
            //    popup_uc.Placement = PlacementMode.Right;
            //    popup_uc.IsOpen = true;
            //    Header.PopupText.Text = Properties.Langs.Lang.Tea_Sche;

            //}

            //if (slideLeft.Width == 45)
            //{
            //    btnStaffAttendanceReport.Margin = new Thickness(5, 10, 5, 5);
            //    btnStudentAttendanceReport.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentLearningRsult.Margin = new Thickness(5, -3, 5, 5);
            //    btnClassSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnTeacherSchedule.Margin = new Thickness(5, -3, -2, 5);
            //    btnStudentList.Margin = new Thickness(5, -3, 5, 5);
            //    btnSubJectList.Margin = new Thickness(5, -3, 5, 5);

            //    MateriaStaffAtt.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStuAtt.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStuLearn.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaClassSche.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaTeacherSche.Margin = new Thickness(3, 0, 0, 0);
            //    MateriaStu.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaSubj.Margin = new Thickness(0, 0, 0, 0);
            //}
        }


        private void btnTeacherSchedule_MouseLeave(object sender, MouseEventArgs e)
        {
            if (tabMenu.SelectedIndex == 5)
            {
                var bc = new BrushConverter();
                btnTeacherSchedule.Background = (Brush)bc.ConvertFrom("#1183CA");
                MateriaTeacherSche.Foreground = (Brush)bc.ConvertFrom("#1183CA");
                lblTeacherSche.Foreground = Brushes.Blue;
            }
            else
            {
                btnTeacherSchedule.Background = Brushes.Transparent;
                MateriaTeacherSche.Foreground = Brushes.White;
                lblTeacherSche.Foreground = Brushes.White;
            }

            //popup_uc.Visibility = Visibility.Collapsed;
            //popup_uc.IsOpen = false;

            //if (slideLeft.Width == 45)
            //{
            //    btnStaffAttendanceReport.Margin = new Thickness(5, 10, 5, 5);
            //    btnStudentAttendanceReport.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentLearningRsult.Margin = new Thickness(5, -3, 5, 5);
            //    btnClassSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnTeacherSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentList.Margin = new Thickness(5, -3, 5, 5);
            //    btnSubJectList.Margin = new Thickness(5, -3, 5, 5);

            //    MateriaStaffAtt.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStuAtt.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStuLearn.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaClassSche.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaTeacherSche.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStu.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaSubj.Margin = new Thickness(0, 0, 0, 0);
            //}

        }

        private void btnStudentList_MouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            btnStudentList.Background = Brushes.White;
            MateriaStu.Foreground = (Brush)bc.ConvertFrom("#1183CA");
            lblStuList.Foreground = (Brush)bc.ConvertFrom("#1183CA");

            //if (!check)
            //{
            //    popup_uc.PlacementTarget = btnStudentList;
            //    popup_uc.Placement = PlacementMode.Right;
            //    popup_uc.IsOpen = true;
            //    Header.PopupText.Text = Properties.Langs.Lang.Stu_List;

            //}

            //if (slideLeft.Width == 45)
            //{
            //    btnStaffAttendanceReport.Margin = new Thickness(5, 10, 5, 5);
            //    btnStudentAttendanceReport.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentLearningRsult.Margin = new Thickness(5, -3, 5, 5);
            //    btnClassSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnTeacherSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentList.Margin = new Thickness(5, -3, -2, 5);
            //    btnSubJectList.Margin = new Thickness(5, -3, 5, 5);

            //    MateriaStaffAtt.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStuAtt.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStuLearn.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaClassSche.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaTeacherSche.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStu.Margin = new Thickness(3, 0, 0, 0);
            //    MateriaSubj.Margin = new Thickness(0, 0, 0, 0);
            //}

        }

        private void btnStudentList_MouseLeave(object sender, MouseEventArgs e)
        {
            if (tabMenu.SelectedIndex == 6)
            {
                var bc = new BrushConverter();
                btnStudentList.Background = Brushes.White;
                MateriaStu.Foreground = (Brush)bc.ConvertFrom("#1183CA");
                lblStuList.Foreground = (Brush)bc.ConvertFrom("#1183CA");
            }
            else
            {
                btnStudentList.Background = Brushes.Transparent;
                MateriaStu.Foreground = Brushes.White;
                lblStuList.Foreground = Brushes.White;
            }

            //popup_uc.Visibility = Visibility.Collapsed;
            //popup_uc.IsOpen = false;

            //if (slideLeft.Width==45)
            //{
            //    btnStaffAttendanceReport.Margin = new Thickness(5, 10, 5, 5);
            //    btnStudentAttendanceReport.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentLearningRsult.Margin = new Thickness(5, -3, 5, 5);
            //    btnClassSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnTeacherSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentList.Margin = new Thickness(5, -3, 5, 5);
            //    btnSubJectList.Margin = new Thickness(5, -3, 5, 5);

            //    MateriaStaffAtt.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStuAtt.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStuLearn.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaClassSche.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaTeacherSche.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStu.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaSubj.Margin = new Thickness(0, 0, 0, 0);
            //}

        }

        private void btnSubJectList_MouseEnter_1(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            btnSubJectList.Background = Brushes.White;
            MateriaSubj.Foreground = (Brush)bc.ConvertFrom("#1183CA");
            lblSubject.Foreground = (Brush)bc.ConvertFrom("#1183CA");

            //if (!check)
            //{
            //    popup_uc.PlacementTarget = btnSubJectList;
            //    popup_uc.Placement = PlacementMode.Right;
            //    popup_uc.IsOpen = true;
            //    Header.PopupText.Text = Properties.Langs.Lang.Sub_List;

            //}

            //if (slideLeft.Width == 45)
            //{
            //    btnStaffAttendanceReport.Margin = new Thickness(5, 10, 5, 5);
            //    btnStudentAttendanceReport.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentLearningRsult.Margin = new Thickness(5, -3, 5, 5);
            //    btnClassSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnTeacherSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentList.Margin = new Thickness(5, -3, 5, 5);
            //    btnSubJectList.Margin = new Thickness(5, -3, -2, 5);

            //    MateriaStaffAtt.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStuAtt.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStuLearn.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaClassSche.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaTeacherSche.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStu.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaSubj.Margin = new Thickness(3, 0, 0, 0);
            //}

        }

        private void btnSubJectList_MouseLeave(object sender, MouseEventArgs e)
        {
            if (tabMenu.SelectedIndex == 7)
            {
                var bc = new BrushConverter();
                btnSubJectList.Background = Brushes.White;
                MateriaSubj.Foreground = (Brush)bc.ConvertFrom("#1183CA");
                lblSubject.Foreground = (Brush)bc.ConvertFrom("#1183CA");
            }
            else
            {
                btnSubJectList.Background = Brushes.Transparent;
                MateriaSubj.Foreground = Brushes.White;
                lblSubject.Foreground = Brushes.White;
            }

            //popup_uc.Visibility = Visibility.Collapsed;
            //popup_uc.IsOpen = false;

            //if (slideLeft.Width==45)
            //{
            //    btnStaffAttendanceReport.Margin = new Thickness(5, 10, 5, 5);
            //    btnStudentAttendanceReport.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentLearningRsult.Margin = new Thickness(5, -3, 5, 5);
            //    btnClassSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnTeacherSchedule.Margin = new Thickness(5, -3, 5, 5);
            //    btnStudentList.Margin = new Thickness(5, -3, 5, 5);
            //    btnSubJectList.Margin = new Thickness(5, -3, 5, 5);

            //    MateriaStaffAtt.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStuAtt.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStuLearn.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaClassSche.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaTeacherSche.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaStu.Margin = new Thickness(0, 0, 0, 0);
            //    MateriaSubj.Margin = new Thickness(0, 0, 0, 0);
            //}

        }

        //Button Change Language......
        private void Khmer_Click_1(object sender, RoutedEventArgs e)
        {

            Properties.Settings.Default.Language = "km-KH";
           
           
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Properties.Settings.Default.Save();
            Application.Current.Shutdown();
        }

        private void English_Click_1(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Language = "en-US";
          
           
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Properties.Settings.Default.Save();
            Application.Current.Shutdown();
        }

        private void Chinese_Click_1(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Language = "zh-Hans";
           
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Properties.Settings.Default.Save();
            Application.Current.Shutdown();

        }

        private void Vietnam_Click_1(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Language = "vi-VN";
            
          
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Properties.Settings.Default.Save();
            Application.Current.Shutdown();
        }

        //type Combobox top bar.........
        private void ComboBox_MouseEnter(object sender, MouseEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            cb.Background = Brushes.LightGray;

        }

        private void ComboBox_MouseLeave(object sender, MouseEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            cb.Background = Brushes.WhiteSmoke;

        }

        private void cboClassroom_MouseEnter(object sender, MouseEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            cb.Background = Brushes.LightGray;

        }

        private void cboClassroom_MouseLeave(object sender, MouseEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            cb.Background = Brushes.WhiteSmoke;

        }

        private void btnDaily_MouseEnter(object sender, MouseEventArgs e)
        {
            Button BT = (Button)sender;
            BT.Background = Brushes.LightGray;
        }

        private void btnDaily_MouseLeave(object sender, MouseEventArgs e)
        {
            Button BT = (Button)sender;
            BT.Background = null;

        }

        private void btnMonthly_MouseEnter(object sender, MouseEventArgs e)
        {
            Button BT = (Button)sender;
            BT.Background = Brushes.LightGray;
        }

        private void btnMonthly_MouseLeave(object sender, MouseEventArgs e)
        {
            Button BT = (Button)sender;
            BT.Background = null;
        }

        private void btnSemester_MouseEnter(object sender, MouseEventArgs e)
        {
            Button BT = (Button)sender;
            BT.Background = Brushes.LightGray;
        }

        private void btnSemester_MouseLeave(object sender, MouseEventArgs e)
        {
            Button BT = (Button)sender;
            BT.Background = null;

        }
        //material top bar......................

        private void gridmax_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (maximized.Kind == MaterialDesignThemes.Wpf.PackIconKind.WindowMaximize)
            {

                WindowState = System.Windows.WindowState.Maximized;
                GridForm.Margin = new Thickness(0);
                maximized.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowRestore;
            }
            else
            {
                ControlMaximize.DoMaximize(this);
                GridForm.Margin = new Thickness(0);
                maximized.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowMaximize;
            }
        }
        private void gridExit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void gridmini_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = System.Windows.WindowState.Minimized;
        }

        //Mouese Over in material top home top bar..................
        private void gridMenu_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid g = (Grid)sender;
            g.Background = Brushes.Red;
        }

        private void gridMenu_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid g = (Grid)sender;
            g.Background = Brushes.Transparent;

        }

        private void gridmini_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid g = (Grid)sender;
            g.Background = Brushes.LightGray;
        }

        private void gridmini_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid g = (Grid)sender;
            g.Background = Brushes.Transparent;
        }


        //Materail popupbox...............
        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.Show();
        }
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxControl message = new MessageBoxControl();
            message.title = Properties.Langs.Lang.Information;
            message.discription = Properties.Langs.Lang.Do_you_really_want_to_exit_CAMEMIS_DESKTOP_;

            this.IsEnabled = false;
            this.Opacity = 0.5;
            message.ShowDialog();
            this.Opacity = 1;
            this.IsEnabled = true;
           
            if (message.result == 1)
            {

                Properties.Settings.Default.checkLoginOrLogut = Properties.Langs.Lang.logout;
                Properties.Settings.Default.Save();
                Login login = new Login();
                this.Close();
                login.Show();
            }
            else
            {
                Properties.Settings.Default.checkLoginOrLogut = Properties.Langs.Lang.logout;
                Properties.Settings.Default.Save();
            }
        }

        private void txtSub_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void btnPrintAttStu_Click(object sender, RoutedEventArgs e)
        {
            /*Student_Attendance StuAtt = new Student_Attendance();
            StuAtt.SetSource(at1);
            //StuAtt.setParamet(0, lbldayNumAcard.Content);
            //StuAtt.setParamet(1, lblMonthAcard.Content);
            //StuAtt.setParamet(2, txttotalAttAcard.Text);
            //StuAtt.setParamet(3, lbldayAcard.Content);
            //StuAtt.setParamet(4, lblyearAcard.Content);
            //StuAtt.setParamet(5, txtAtthaveAcrad.Text);
            //StuAtt.setParamet(6, txtotherAcard.Text);
            //StuAtt.setParamet(7, lblSchoolAcard.Content);
            //StuAtt.setParamet(8, txttitleAcard.Text);
            //StuAtt.setParamet(9, txtsalaryAcard.Text);
            StuAtt.ShowDialog();*/
        }
        //end btn Print

        //Print int student Result......

        private void btnPrintAllSub_Click(object sender, RoutedEventArgs e)
        {
            string title = "";
            Loading loading = new Loading();
            if (tabStudentResult.SelectedIndex == 1)
            {
                title = "month";
                var data = report.data.Where(s => s.result_monthly != null);
                AllSubMonthlyResult result = new AllSubMonthlyResult(true, data.OrderBy(s=>s.result_monthly.rank).ToList(), title,yearTitle);
                loading.Show();
                this.IsEnabled = false;
                result.Show();
                loading.Close();
                this.IsEnabled = true;
            }

            else if (tabStudentResult.SelectedIndex == 2)
            {
                title = "semester";
                var data = report.data.Where(s => s.result_semester != null);
                AllSubMonthlyResult result = new AllSubMonthlyResult(true, data.OrderBy(r => r.result_semester.rank).ToList(), title,yearTitle);
                loading.Show();
                this.IsEnabled = false;
                result.Show();
                loading.Close();
                this.IsEnabled = true;
            }
        }
        private void btnPrintresultStu_Click(object sender, RoutedEventArgs e)
        {
            List<StudentMonthlyResult> results = new List<StudentMonthlyResult>();
            if (startProgram)
            {
               
                if (tabStudentResult.SelectedIndex == 1)
                {
                    foreach (var i in report.data)
                    {
                        if (i.result_monthly == null)
                        {
                            i.result_monthly = new Library.MonthlyResult { rank = report.data.Count(),avg_score="--",absence_exam=2 };
                        }
                    }
                    results = report.data.OrderBy(r => r.result_monthly!=null).ThenBy(r=>r.result_monthly.rank).ToList();
                }
                else if (tabStudentResult.SelectedIndex == 2)
                {
                    foreach (var i in report.data)
                    {
                        if (i.result_semester == null)
                        {
                            i.result_semester = new resultSemester { rank = report.data.Count(),avg_score="--" };
                            i.result_semester_exam = new resultSemesterExam { total_score = 0 };
                        }
                    }
                    results = report.data.OrderBy(r => r.result_semester!=null).ThenBy(r => r.result_semester.rank).ToList();
                }
                else if (tabStudentResult.SelectedIndex == 3)
                {
                    foreach (var i in report.data)
                    {
                        if (i.result_yearly == null)
                        {
                            i.result_yearly = new resultYearly { rank = report.data.Count(),avg_score="--" };
                        }
                    }
                    results = report.data.OrderBy(r => r.result_yearly!=null).ThenBy(r => r.result_yearly.rank).ToList();
                }
            }

            else
                results = report.data;
            try
            {
                string title = "";
                if (tabStudentResult.SelectedIndex == 1)
                    title = "month";
                else if (tabStudentResult.SelectedIndex == 2)
                    title = "semester";
                else if (tabStudentResult.SelectedIndex == 5)
                    title = "exam";
                else if (tabStudentResult.SelectedIndex == 3)
                    title = "year";
                MonthlyResult monthlyResult = new MonthlyResult(results, title,yearTitle);
                monthlyResult.Show();
            }
            catch
            {
                MessageBox.Show("ទិន្នន័យមិនគ្រប់គ្រាន់ក្នុងការបោះពុម្ភ", "ប្រុងប្រយ័ត", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        //end.......................//


        //buton in tabcontrol student resh report
        //btn month

        //buton Semester

        // Gotfocus and lostfocus in tabcontrol in student res report

        /// <summary>
        /// //////////date Packid In StaffAttendeanc Report
        /// </summary>

       
        //Internte Checker
        static public bool InternetChecker()
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
        //Selecte Grade

        //******************Student Learning Result**************************
        private void btnGrade_Click(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            btnGrade.Foreground = Brushes.White;
            btnGrade.Background = (Brush)bc.ConvertFrom("#1183CA");
            btnGrade.BorderBrush = Brushes.LightGray;

            btnMonth.Foreground = Brushes.Black;
            btnMonth.Background = Brushes.White;
            btnMonth.BorderBrush = (Brush)bc.ConvertFrom("#1183CA");

            GridCursors.Margin = new Thickness(2, 43, 0, 0);
            tabStuResult.SelectedIndex = 0;
        }
        //end selecte Grade
        //selecte Month
        private void btnMonth_Click(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            btnGrade.Foreground = Brushes.Black;
            btnGrade.Background = Brushes.White;
            btnGrade.BorderBrush = (Brush)bc.ConvertFrom("#1183CA");

            btnMonth.Foreground = Brushes.White;
            btnMonth.Background = (Brush)bc.ConvertFrom("#1183CA");
            btnMonth.BorderBrush = Brushes.LightGray;

            GridCursors.Margin = new Thickness(127, 43, 0, 0);
            tabStuResult.SelectedIndex = 1;

        }
        //end selected month

        // select treeview in student learning///
        //bool checkMonthButton = true;
        private async void tvAcademy_Selected(object sender, RoutedEventArgs e)
        {
            try
            {
                this.IsEnabled = false;
                //checkMonthButton = true;
                var items = tvAcademy.SelectedItem.ToString();
                if (items.Equals("CamemisOffLine.Children"))
                {
                    var item = tvAcademy.SelectedItem as Children;
                    treeViewItemChange(item.name);
                    this.classId = item.id.ToString();
                    /*OptionMenu.Visibility = Visibility.Collapsed;*/
                    btnMonth.Visibility = Visibility.Visible;
                    Month.Visibility = Visibility.Visible;

                    staGrade.Width = 120;
                    btnGrade.Width = 120;

                    gridStudentResult.Visibility = Visibility.Visible;
                    gridgoto.Visibility = Visibility.Visible;
                    grideStuResult.Visibility = Visibility.Visible;
                    gridformstuResult.Visibility = Visibility.Visible;
                    gridStudentResult.Margin = new Thickness(0, -42, 0, 0);
                    gridformstuResult.Margin = new Thickness(0);
                    gridHeadernumstu.Visibility = Visibility.Collapsed;
                    LabelTitle.Content = Properties.Langs.Lang.Message_Box_Stu_Result_Title_select_month;
                    DGMonthlyResult.Visibility = Visibility.Collapsed;
                    checkSaveImgae = true;
                    getAllData = false;
                    if (InternetChecker()&&internet)
                    {
                        string accessUrl = Properties.Settings.Default.acessUrl;
                        string token = Properties.Settings.Default.Token;
                        var respone = await RESTApiHelper.GetAll(accessUrl, "/academic/" + item.id + "/grade-time-shift", token);
                        DataButton = JObject.Parse(respone).ToObject<TimesButton>().data;
                        SaveAcademyMonth(respone,schoolYearId);
                        Properties.Settings.Default.monthofTheAcademyYear = respone;
                        Properties.Settings.Default.Save();
                        getAllData = true;
                        if(!File.Exists(filePath + "\\" + item.id + ".txt"))
                        {
                            Task<string> task = GetMonthlyResultFormApiAsync();
                        }
                        else
                        {
                            MessageBoxControl message = new MessageBoxControl();
                            message.title = Properties.Langs.Lang.Information;
                            message.discription = Properties.Langs.Lang.Do_you_want_to_use_existing_data_for_or_new_data_;
                            message.yes = Properties.Langs.Lang.Use_old_data;
                            message.no = Properties.Langs.Lang.Use_new_data;
                            this.Opacity = 0.5;
                            message.ShowDialog();
                            this.Opacity = 1;
                            if(message.result==1)
                                getAllData = true;
                            else
                                GetMonthlyResultFormApiAsync();
                        }
                    }
                    else
                    {
                        getAllData = true;
                        DataButton = GetAcademyFromLocal(schoolYearId).data;

                    }
                    if (changeAcademyYear)
                    {

                        lButton.ItemsSource = DataButton;
                        changeAcademyYear = false;
                        check = true;
                        lbltitleMonth.Visibility = Visibility.Collapsed;
                        btnStatistic.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        tabStudentResult.SelectedIndex = 0;
                        LabelTitle.Content = Properties.Langs.Lang.Message_Box_Stu_Result_Title_select_month;
                        gridgoto.Visibility = Visibility.Visible;
                        OptionMenu.Visibility = Visibility.Collapsed;
                        TranscripPrint.Visibility = Visibility.Collapsed;
                        lButton.ItemsSource = DataButton;
                        lbltitleMonth.Visibility = Visibility.Collapsed;
                        btnStatistic.Visibility = Visibility.Collapsed;
                    }
                    lbltitleClassResult.Content = item.name;
                    btnResultofTheYear.Visibility = Visibility.Visible;
                    classId = item.id.ToString();
                    className = item.name;
                }
                this.IsEnabled = true;
            }
            catch
            {
                //checkMonthButton = false;
                LabelTitle.Content = Properties.Langs.Lang.Message_Box_Stu_Result_Title;
                tabStudentResult.SelectedIndex = 0;
                btnMonth.Visibility = Visibility.Collapsed;
            }
            /*  if(checkMonthButton)
               {
                   GridCursors.Margin = new Thickness(115, 43, 0, 0);
                   tabStuResult.SelectedIndex = 1;
               }*/
        }

        private void tvAcademy_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem item = (TreeViewItem)(tvAcademy
            .ItemContainerGenerator
            .ContainerFromIndex(tvAcademy.Items.CurrentPosition));
        }

        /// END..................//

        // Button Open & close dock Treeviwe & goto
        private void btngoto_Click(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            btnGrade.Foreground = Brushes.White;
            btnGrade.Background = (Brush)bc.ConvertFrom("#1183CA");
            btnGrade.BorderBrush = Brushes.LightGray;

            btnMonth.Foreground = (Brush)bc.ConvertFrom("#1183CA");
            btnMonth.Background = Brushes.White;
            btnMonth.BorderBrush = (Brush)bc.ConvertFrom("#1183CA");

            staGrade.Width = 250;
            btnGrade.Width = 250;

            gridgoto.Visibility = Visibility.Collapsed;
            Month.Visibility = Visibility.Collapsed;
            GridCursors.Margin = new Thickness(10, 43, 0, 0);
            tabStuResult.SelectedIndex = 0;
            gridformstuResult.Margin = new Thickness(0, -40, 0, 0);
            gridStudentResult.Margin = new Thickness(0, -41, 0, 0);
            grideStuResult.Visibility = Visibility.Collapsed;
            docktree.Visibility = Visibility.Visible;
            lbltitleMonth.Content = "";
            gridHeadernumstu.Visibility = Visibility.Collapsed;
            lbltitleClassResult.Content = "";



            tabStudentResult.SelectedIndex = 0;
            LabelTitle.Content = Properties.Langs.Lang.Message_Box_Stu_Result_Title;

            tvAcademy.ItemsSource = null;
            var cb = cbAcademyYear.SelectedValue;
            var obj = JObject.Parse(Properties.Settings.Default.schoolAcademyYear).ToObject<YearofAcademy>().data.Where(y => y.name.Equals(cb.ToString()));
            foreach (var item in obj)
            {
                foreach (var grade in item.school_system)
                {
                    var order = grade.grade.OrderBy(s => s.sortkey).ToList();
                    grade.grade = order;
                }
            }
            tvAcademy.ItemsSource = obj;
        }

        string classId = "", monthName = "", className = "", yearTitle = "";
        List<string> studentName = new List<string>();

        //-------Get Monthly and Semester Reslut-------------------------------
        private async void btnSemester_Click(object sender, RoutedEventArgs e)
        {
            type = "2";
            txtDataDate.Content = GetDataDate();
            grid_ResultLearning.Visibility = Visibility.Visible;
            lbltitleMonth.Visibility = Visibility.Visible;
            btnStatistic.Visibility = Visibility.Visible;
            DGMonthlyResult.Visibility = Visibility.Visible;
            Selectresult.Visibility = Visibility.Visible;

            checkStart = true;
            title = "semester";
            int girlTotal = 0;
            int number = 1;
            var _tag = e.Source as Button;
            var button = sender as Button;
            string term = _tag.Tag.ToString(), monthName = button.Content.ToString(),profileId="";
            this.term = term;
            var month = DateChange.checkMonthString(button.Content.ToString());
            GetApprovedAsync(classId, month.ToString(), "2", term);
            string accessUrl = Properties.Settings.Default.acessUrl;
            string token = Properties.Settings.Default.Token;
            buttonSelecte(button.Content.ToString());
            Loading loading = new Loading();
            //------Loading-----
            loading.Show();
            loading.ShowInTaskbar = false;
            this.IsEnabled = false;
            //----end loading----
            //-------------------bind student result--------------------------
            try
            {
                studentName = new List<string>();
                this.IsEnabled = false;
                var obj = new List<StudentMonthlyResult>();
                if (getAllData)
                {
                    obj = GetData("", "semester", monthName);
                    report.data = obj;
                }
                foreach (var item in obj)
                {
                    if (checkSaveImgae && getAllData == false)
                    {
                        if (number == 2)
                        {
                            SaveImage(item.instructor.profileMedia.file_name, ImageFormat.Jpeg, item.instructor.profileMedia.file_show);
                        }
                        SaveImage(item.student_schoolyear_id, ImageFormat.Jpeg, item.profileMedia.file_show);
                    }
                    studentName.Add(item.name);
                    if (item.profileMedia.id == null)
                        profileId = item.student_id;
                    else
                        profileId = item.profileMedia.id;
                    item.localProfileLink = filePath + "\\" + profileId + ".jpg";
                    try
                    {
                        if (item.result_semester.is_fail.Equals("1"))
                        {
                            item.result_semester.is_fail = "ធ្លាក់";
                        }
                        else
                        {
                            item.result_semester.is_fail = "ជាប់";
                        }
                    }
                    catch { }
                    if (item.gender == "2")
                    {
                        girlTotal++;
                        var changeGender = obj.FirstOrDefault(g => g.gender == "2");
                        changeGender.gender = "ស្រី";

                    }
                    else
                    {
                        var changeGender = obj.FirstOrDefault(g => g.gender == "1");
                        changeGender.gender = "ប្រុស";
                    }
                    number++;
                }
                studentName.Add("");
                cmbStudentName.ItemsSource = studentName;

                foreach(var item in obj)
                {
                    try
                    {
                        if (item.result_semester.avg_score == "0")
                            item.result_semester.avg_score = "មិនចាត់ថ្នាក់";
                        if (item.result_semester_exam.avg_score == "0")
                            item.result_semester_exam.avg_score = "មិនចាត់ថ្នាក់";
                        if (item.result_semester.morality == null)
                            item.result_semester.morality = "--";
                        if (item.result_semester.bangkeun_phal == null)
                            item.result_semester.bangkeun_phal = "--";
                        if (item.result_semester.health == null)
                            item.result_semester.health = "--";
                    }
                    catch
                    {
                        
                    }
                }
                var obj1 = obj;
                try
                {
                    if (startProgram == true)
                    {
                        NumberList(obj.OrderBy(s => s.result_semester.rank).ToList());
                        NumberList(obj1.OrderBy(s => int.Parse(s.result_semester_exam.rank)).ToList());
                        DGSemester.ItemsSource = null;
                        DGSemester.ItemsSource = obj.OrderBy(s => s.result_semester.rank);
                        DGSemesterExam.ItemsSource = obj1.OrderBy(s => int.Parse(s.result_semester_exam.rank));
                        DGSemesterClass.ItemsSource = obj.OrderBy(s => s.result_semester.rank);
                    }
                    else
                    {
                        NumberList(obj.OrderBy(s => s.result_semester.rank).ToList());
                        NumberList(obj1.OrderBy(s => int.Parse(s.result_semester_exam.rank)).ToList());
                        DGSemester.ItemsSource = obj.OrderBy(s => s.result_semester.rank);
                        DGSemesterExam.ItemsSource = obj1.OrderBy(s => int.Parse(s.result_semester_exam.rank));
                        DGSemesterClass.ItemsSource = obj.OrderBy(s => s.result_semester.rank);
                    }
                    students = obj1.OrderBy(s => int.Parse(s.result_semester_exam.rank)).ToList();
                    report.data = obj.OrderBy(s => s.result_semester_exam.total_score).ToList();
                }
                catch
                {
                    NumberList(obj.OrderBy(s => s.name).ToList());
                    DGSemester.ItemsSource = null;
                    DGSemester.ItemsSource = obj.OrderBy(s => s.name);
                    DGSemesterExam.ItemsSource = obj1.OrderBy(s => s.name);
                    DGSemesterClass.ItemsSource = obj.OrderBy(s => s.name);

                }
                lblTitleTotalStudent.Content = Properties.Langs.Lang.totalStu + " : " + DGSemester.Items.Count.ToString() + " " + Properties.Langs.Lang.female + " : " + girlTotal.ToString();
                gridMonth.Visibility = Visibility.Collapsed;
                tabStudentResult.Visibility = Visibility.Visible;
                gridStudentResult.Margin = new Thickness(0);
                gridHeadernumstu.Visibility = Visibility.Visible;

                OptionMenu.Visibility = Visibility.Visible;
                TranscripPrint.Visibility = Visibility.Visible;
                gridgoto.Visibility = Visibility.Visible;
                cmbSort.SelectedIndex = 1;
                rbSmall.IsChecked = true;
            }
            catch
            {
                btnStatistic.Visibility = Visibility.Collapsed;
                OptionMenu.Visibility = Visibility.Collapsed;
                TranscripPrint.Visibility = Visibility.Collapsed;
                gridMonth.Visibility = Visibility.Visible;
                tabStudentResult.Visibility = Visibility.Collapsed;
                gridStudentResult.Margin = new Thickness(0, -42, 0, 0);
                gridHeadernumstu.Visibility = Visibility.Collapsed;
                cmbSort.SelectedIndex = 1;
                rbSmall.IsChecked = true;
            }
            this.IsEnabled = true;

            if (monthName.Equals("លទ្ធផលប្រចាំឆ្នាំ"))
            {
                tabStudentResult.SelectedIndex = 3;
                lbltitleMonth.Content = monthName;
            }
            else if (monthName.Equals("ឆមាសទី១") || monthName.Equals("ឆមាសទី២"))
            {
                tabStudentResult.SelectedIndex = 2;
                lbltitleMonth.Content = "លទ្ធផលប្រចាំ " + monthName;
            }
            else
            {
                tabStudentResult.SelectedIndex = 1;
                lbltitleMonth.Content = "លទ្ធផលប្រចាំ " + monthName;
            }
            //------Loading-----
            loading.Close();
            this.IsEnabled = true;
            checkSaveImgae = false;
            //----end loading----
        }
        string title = "";
        private async void btnMonths_Click(object sender, RoutedEventArgs e)
        {
            type = "1";
            txtDataDate.Content = GetDataDate();
            grid_ResultLearning.Visibility = Visibility.Visible;
            Selectresult.Visibility = Visibility.Collapsed;
            lbltitleMonth.Visibility = Visibility.Visible;
            btnStatistic.Visibility = Visibility.Visible;
            DGMonthlyResult.Visibility = Visibility.Visible;
            checkStart = true;
            int girlTotal = 0;
            title = "month";
            int number = 1;
            var _tag = e.Source as Button;
            var button = sender as Button;
            string term = _tag.Tag.ToString(), monthName = button.Content.ToString(),profileId="";
            var month = DateChange.checkMonthString(button.Content.ToString());
            string accessUrl = Properties.Settings.Default.acessUrl;
            string token = Properties.Settings.Default.Token;
            this.term = term;
            this.month = month.ToString();
            GetApprovedAsync(classId, month.ToString(), "1",term);
            buttonSelecte(button.Content.ToString());
            Loading loading = new Loading();
            //------Loading-----
            loading.Show();
            loading.ShowInTaskbar = false;
            this.IsEnabled = false;
            //----end loading----
            //-------------------bind student result--------------------------
            try
            {
                studentName = new List<string>();
                this.IsEnabled = false;
                var obj = new List<StudentMonthlyResult>();
                if (getAllData)
                {
                    obj = GetData(month.ToString());
                    report.data = obj;
                }
                foreach (var item in obj)
                {
                    if (checkSaveImgae && getAllData == false)
                    {
                        if (number == 2)
                        {
                            SaveImage(item.instructor.profileMedia.file_name, ImageFormat.Jpeg, item.instructor.profileMedia.file_show);
                        }
                        SaveImage(item.student_schoolyear_id, ImageFormat.Jpeg, item.profileMedia.file_show);
                    }
                    if(number==1)
                    {
                        monthName = DateChange.checkMonth(int.Parse(item.result_monthly.month));
                    }
                    if (item.profileMedia.id == null)
                        profileId = item.student_id;
                    else
                        profileId = item.profileMedia.id;
                    item.localProfileLink = filePath + "\\" + profileId + ".jpg";
                    studentName.Add(item.name);
                    try
                    {
                        if (item.result_monthly.absence_exam.Equals(1))
                        {
                            item.result_monthly.avg_score = "មិនចាត់ថ្នាក់";
                            item.result_monthly.color = "Red";
                        }
                        else
                        {
                            try
                            {
                                if (item.result_monthly.avg_score == "0" || item.result_monthly.avg_score == null)
                                {
                                    item.result_monthly.avg_score = "--";
                                    item.result_monthly.color = "Blue";
                                }
                                else
                                {
                                    var changeAvg = obj.FirstOrDefault(s => s.result_monthly.avg_score == item.result_monthly.avg_score);
                                    changeAvg.result_monthly.avg_score = double.Parse(item.result_monthly.avg_score).ToString("#.##");
                                    item.result_monthly.color = "Blue";
                                }
                            }
                            catch
                            {
                                item.result_monthly.avg_score = "--";
                                item.result_monthly.color = "Blue";
                            }

                        }
                    }
                    catch
                    {
                        
                    }
                    if (item.gender == "2")
                    {
                        girlTotal++;
                        var changeGender = obj.FirstOrDefault(g => g.gender == "2");
                        changeGender.gender = "ស្រី";

                    }
                    else
                    {
                        var changeGender = obj.FirstOrDefault(g => g.gender == "1");
                        changeGender.gender = "ប្រុស";
                    }
                    number++;
                }
                studentName.Add("");
                cmbStudentName.ItemsSource = studentName;

                try
                {
                    if (startProgram == true)
                    {
                        NumberList(obj.OrderBy(s => s.result_monthly.rank).ToList());
                        DGMonthlyResult.ItemsSource = null;
                        DGMonthlyResult.ItemsSource = obj.OrderBy(s => s.result_monthly.rank);
                    }
                    else
                    {
                        NumberList(obj.OrderBy(s => s.result_monthly.rank).ToList());
                        DGMonthlyResult.ItemsSource = obj.OrderBy(s => s.result_monthly.rank);
                    }
                    report.data = obj.OrderBy(s => s.result_monthly.rank).ToList();
                }
                catch
                {
                    NumberList(obj.OrderBy(s => s.name).ToList());
                    DGMonthlyResult.ItemsSource = null;
                    DGMonthlyResult.ItemsSource = obj.OrderBy(s => s.name);
                    report.data = obj.OrderBy(s => s.name).ToList();
                }
                lblTitleTotalStudent.Content = Properties.Langs.Lang.totalStu + " : " + DGMonthlyResult.Items.Count.ToString() + " " + Properties.Langs.Lang.female + " : " + girlTotal.ToString();
                gridMonth.Visibility = Visibility.Collapsed;
                tabStudentResult.Visibility = Visibility.Visible;
                gridStudentResult.Margin = new Thickness(0);
                gridHeadernumstu.Visibility = Visibility.Visible;

                OptionMenu.Visibility = Visibility.Visible;
                TranscripPrint.Visibility = Visibility.Visible;
                gridgoto.Visibility = Visibility.Visible;
                cmbSort.SelectedIndex = 1;
                rbSmall.IsChecked = true;

            }
            catch(Exception ex)
            {
                Console.WriteLine("Error:" + ex.ToString());
                btnStatistic.Visibility = Visibility.Collapsed;
                OptionMenu.Visibility = Visibility.Collapsed;
                TranscripPrint.Visibility = Visibility.Collapsed;
                gridMonth.Visibility = Visibility.Visible;
                tabStudentResult.Visibility = Visibility.Collapsed;
                gridStudentResult.Margin = new Thickness(0, -42, 0, 0);
                gridHeadernumstu.Visibility = Visibility.Collapsed;
                cmbSort.SelectedIndex = 1;
                rbSmall.IsChecked = true;
            }
            this.IsEnabled = true;

            if (monthName.Equals("លទ្ធផលប្រចាំឆ្នាំ"))
            {
                tabStudentResult.SelectedIndex = 3;
                lbltitleMonth.Content = monthName;
            }

            else if (monthName.Equals("ឆមាសទី១") || monthName.Equals("ឆមាសទី២"))
            {
                tabStudentResult.SelectedIndex = 2;
                lbltitleMonth.Content = "លទ្ធផលប្រចាំ " + monthName;

            }
            else
            {
                tabStudentResult.SelectedIndex = 1;
                lbltitleMonth.Content = "លទ្ធផលប្រចាំ " + monthName;
            }
            //------Loading-----
            loading.Close();
            this.IsEnabled = true;
            checkSaveImgae = false;
            //----end loading----
        }
        private void NumberList(List<StudentMonthlyResult> obj)
        {
            int i = 1;
            foreach (var item in obj)
            {
                item.numbers = DateChange.Num(i);
                i++;
            }
        }

        //--------------------------------------End Get Monthly and Semester Reslut---------------------------------------------------------
        private void Button_GotFocus(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            b.Background = Brushes.White;
        }

        private void Button_LostFocus(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            b.Background = Brushes.LightGray;
        }

        //END.................//

        //combobox on docktree...........

        bool checkSelection = true, startProgram = false, changeAcademyYear = true;
        string year = "";
        private void cbAcademyYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (checkSelection)
                {
                    tabStudentResult.SelectedIndex = 0;

                    if (startProgram)
                    {
                        checkSelection = !checkSelection;
                    }
                }
                else
                {
                    LabelTitle.Content = Properties.Langs.Lang.Message_Box_Stu_Result_Title;
                    tabStudentResult.SelectedIndex = 0;

                    if (!startProgram)
                    {
                        checkSelection = !checkSelection;
                    }
                }

                var cb = cbAcademyYear.SelectedValue;

                var obj = JObject.Parse(Properties.Settings.Default.schoolAcademyYear).ToObject<YearofAcademy>().data.Where(y => y.name.Equals(cb.ToString()));
                foreach (var item in obj)
                {
                    foreach(var grade in item.school_system)
                    {
                        var order = grade.grade.OrderBy(s => s.sortkey).ToList();
                        grade.grade = order;
                    }
                }
                tvAcademy.ItemsSource = obj;
                Month.Visibility = Visibility.Collapsed;
                lbltitleGrade.Content = "ឆ្នាំសិក្សា" + cbAcademyYear.SelectedValue.ToString();
                yearTitle = cbAcademyYear.SelectedValue.ToString(); 
                YearSelection = cbAcademyYear.SelectedValue.ToString();
                LabelTitle.Content = Properties.Langs.Lang.Message_Box_Stu_Result_Title;
                changeAcademyYear = true;
                foreach(var item in obj)
                {
                    schoolYearId = item.id;
                }
                year = cb.ToString();
            }
            catch
            {

            }

        }

        //////////***************End Student Learning Result****************************/
        /// <summary>
        /// 
        /// 
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvAcademyAtt_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem item = (TreeViewItem)(tvAcademyAtt
           .ItemContainerGenerator
           .ContainerFromIndex(tvAcademyAtt.Items.CurrentPosition));
        }

        private void tvAcademyAtt_Selected(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = tvAcademyAtt.SelectedItem as Children;

                gridStudentAtt.Visibility = Visibility.Visible;
                gridgoto.Visibility = Visibility.Visible;
                grideStuResult.Visibility = Visibility.Visible;
                gridStudentAtt.Margin = new Thickness(0);
                tabStudentAtt.SelectedIndex = 0;
                gridbarstuAtt.Visibility = Visibility.Visible;
                lbltitleClassAtt.Content = item.name;
                DockTreeview.Visibility = Visibility.Collapsed;
                DockStuAtt.Margin = new Thickness(0);
            }
            catch
            {

            }
        }

        private void MenuStuAtt_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ArroBackStuAtt.Kind == MaterialDesignThemes.Wpf.PackIconKind.ArrowRight)
            {
                ArroBackStuAtt.Kind = MaterialDesignThemes.Wpf.PackIconKind.ArrowLeft;
                DockTreeview.Visibility = Visibility.Visible;
                DockStuAtt.Margin = new Thickness(10,0,0,0);

            }
            else
            {
                ArroBackStuAtt.Kind = MaterialDesignThemes.Wpf.PackIconKind.ArrowRight;
                DockTreeview.Visibility = Visibility.Collapsed;
                DockStuAtt.Margin = new Thickness(0);

            }
        }

        bool checkSelectionAtt = true;
        private void cbAcademyYearAtt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (checkSelectionAtt)
                {
                    tabStudentResult.SelectedIndex = 1;

                    if (startProgram)
                    {

                        checkSelectionAtt = false;
                    }
                }
                else
                {
                    tabStudentResult.SelectedIndex = 1;
                    var cb = cbAcademyYearAtt.SelectedValue;

                    if (cb.ToString().Contains("សូមជ្រើសយក"))
                    {
                        LabelTitleAtt.Content = Properties.Langs.Lang.Message_Box_Stu_Result_Title_select_year;
                    }
                    else
                    {
                        LabelTitleAtt.Content = Properties.Langs.Lang.Message_Box_Stu_Result_Title;
                    }
                    ArroBackStuAtt.Kind = MaterialDesignThemes.Wpf.PackIconKind.ArrowRight;
                    var obj = JObject.Parse(Properties.Settings.Default.schoolAcademyYear).ToObject<YearofAcademy>().data.Where(y => y.name.Equals(cb.ToString()));
                    tvAcademyAtt.ItemsSource = obj;
                    lbltitleyearAtt.Content = "ឆ្នាំសិក្សា" + cbAcademyYearAtt.SelectedValue.ToString();
                }
            }
            catch
            {

            }
        }

        private void btngotoAtt_Click(object sender, RoutedEventArgs e)
        {
            gridbarstuAtt.Visibility = Visibility.Collapsed;
            tabStudentAtt.SelectedIndex = 1;
            gridStudentAtt.Margin = new Thickness(0, -10, 0, 0);
            DockTreeview.Visibility = Visibility.Visible;
            cbAcademyYearAtt.SelectedValue = "សូមជ្រើសយកឆ្នាំសិក្សា";
            gridMonth.Visibility = Visibility.Collapsed;
            tabStudentResult.Visibility = Visibility.Visible;
        }

        private void printAllTranscript_Click(object sender, RoutedEventArgs e)
        {
            Transcript transcript = new Transcript(null, null, true, true, yearTitle: yearTitle);
            transcript.ShowDialog();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var processes = Process.GetProcesses();

            var process = processes.Where(p => p.ProcessName == "Telegram").FirstOrDefault();

            Process.Start(process.MainModule.FileName);
        }

        private void btnResultofTheYear_Click(object sender, RoutedEventArgs e)
        {
            /*gridMonth.Visibility = Visibility.Visible;
            tabStudentResult.Visibility = Visibility.Collapsed;
            gridStudentResult.Margin = new Thickness(0, -42, 0, 0);
            gridHeadernumstu.Visibility = Visibility.Collapsed;*/
            term = "";
            type = "3";
            txtDataDate.Content = GetDataDate();
            grid_ResultLearning.Visibility = Visibility.Visible;
            lbltitleMonth.Visibility = Visibility.Visible;
            btnStatistic.Visibility = Visibility.Visible;
            DGMonthlyResult.Visibility = Visibility.Visible;
            checkStart = true;
            title = "year";
            int girlTotal = 0;
            int number = 1;
            var _tag = e.Source as Button;
            var button = sender as Button;
            string monthName = button.Content.ToString(), profileId = "";
            var month = DateChange.checkMonthString(button.Content.ToString());
            GetApprovedAsync(classId, month.ToString(), "3", term);
            string accessUrl = Properties.Settings.Default.acessUrl;
            string token = Properties.Settings.Default.Token;
            buttonSelecte(button.Content.ToString());
            Loading loading = new Loading();
            //------Loading-----
            loading.Show();
            loading.ShowInTaskbar = false;
            this.IsEnabled = false;
            //----end loading----
            //-------------------bind student result--------------------------
            try
            {
                studentName = new List<string>();
                this.IsEnabled = false;
                var obj = new List<StudentMonthlyResult>();
                if (getAllData)
                {
                    obj = GetData("", "year", monthName);
                    report.data = obj;
                }
                foreach (var item in obj)
                {
                    if (checkSaveImgae && getAllData == false)
                    {
                        if (number == 2)
                        {
                            SaveImage(item.instructor.profileMedia.file_name, ImageFormat.Jpeg, item.instructor.profileMedia.file_show);
                        }
                        SaveImage(item.student_schoolyear_id, ImageFormat.Jpeg, item.profileMedia.file_show);
                    }
                    studentName.Add(item.name);
                    if (item.profileMedia.id == null)
                        profileId = item.student_id;
                    else
                        profileId = item.profileMedia.id;
                    item.localProfileLink = filePath + "\\" + profileId + ".jpg";
                    try
                    {
                        if (item.result_yearly.is_fail.Equals("1"))
                        {
                            item.result_yearly.is_fail = "ធ្លាក់";
                        }
                        else
                        {
                            item.result_yearly.is_fail = "ជាប់";
                        }
                    }
                    catch { }
                    if (item.gender == "2")
                    {
                        girlTotal++;
                        var changeGender = obj.FirstOrDefault(g => g.gender == "2");
                        changeGender.gender = "ស្រី";

                    }
                    else
                    {
                        var changeGender = obj.FirstOrDefault(g => g.gender == "1");
                        changeGender.gender = "ប្រុស";
                    }
                    number++;
                }
                studentName.Add("");
                cmbStudentName.ItemsSource = studentName;

                foreach (var item in obj)
                {
                    if (item.result_yearly.avg_score == "0")
                        item.result_yearly.avg_score = "មិនចាត់ថ្នាក់";
                    else if (item.result_yearly.avg_score == null)
                        item.result_yearly.avg_score = "--";
                    if (item.result_yearly.avg_score == "0")
                        item.result_yearly.avg_score = "មិនចាត់ថ្នាក់";
                    if (item.result_yearly.avg_score == null)
                        item.result_yearly.avg_score = "--";
                    if (item.result_yearly.morality == null)
                        item.result_yearly.morality = "--";
                    if (item.result_yearly.bangkeun_phal == null)
                        item.result_yearly.bangkeun_phal = "--";
                    if (item.result_yearly.health == null)
                        item.result_yearly.health = "--";
                }

                /* if (startProgram == true)
                 {
                     NumberList(obj.OrderBy(s => s.result_yearly.rank).ToList());
                     DGYear.ItemsSource = null;
                     DGYear.ItemsSource = obj.OrderBy(s => s.result_yearly);

                 }
                 else
                 {
                     NumberList(obj.OrderBy(s => s.result_semester.rank).ToList());
                     DGYear.ItemsSource = null;
                     DGYear.ItemsSource = obj.OrderBy(s => s.result_yearly);
                 }*/


                try
                {
                    NumberList(obj.OrderBy(s => s.result_yearly.rank).ToList());
                    DGYear.ItemsSource = null;
                    DGYear.ItemsSource = obj.OrderBy(s => s.result_yearly.rank);

                    report.data = obj.OrderBy(s => s.result_yearly.rank).ToList();
                }
                catch
                {
                    NumberList(obj.OrderBy(s => s.name).ToList());
                    DGYear.ItemsSource = null;
                    DGYear.ItemsSource = obj.OrderBy(s => s.name);

                    report.data = obj.OrderBy(s => s.name).ToList();
                }
                lblTitleTotalStudent.Content = Properties.Langs.Lang.totalStu + " : " + DGYear.Items.Count.ToString() + " " + Properties.Langs.Lang.female + " : " + girlTotal.ToString();
                gridMonth.Visibility = Visibility.Collapsed;
                tabStudentResult.Visibility = Visibility.Visible;
                gridStudentResult.Margin = new Thickness(0);
                gridHeadernumstu.Visibility = Visibility.Visible;

                OptionMenu.Visibility = Visibility.Visible;
                TranscripPrint.Visibility = Visibility.Visible;
                gridgoto.Visibility = Visibility.Visible;
                cmbSort.SelectedIndex = 1;
                rbSmall.IsChecked = true;
            }
            catch
            {
                btnStatistic.Visibility = Visibility.Collapsed;
                OptionMenu.Visibility = Visibility.Collapsed;
                TranscripPrint.Visibility = Visibility.Collapsed;
                gridMonth.Visibility = Visibility.Visible;
                tabStudentResult.Visibility = Visibility.Collapsed;
                gridStudentResult.Margin = new Thickness(0, -42, 0, 0);
                gridHeadernumstu.Visibility = Visibility.Collapsed;
                cmbSort.SelectedIndex = 1;
                rbSmall.IsChecked = true;
            }
            this.IsEnabled = true;

            if (monthName.Equals("លទ្ធផលប្រចាំឆ្នាំ"))
            {
                tabStudentResult.SelectedIndex = 3;
                lbltitleMonth.Content = monthName;
            }
            else if (monthName.Equals("ឆមាសទី១") || monthName.Equals("ឆមាសទី២"))
            {
                tabStudentResult.SelectedIndex = 2;
                lbltitleMonth.Content = "លទ្ធផលប្រចាំ " + monthName;
            }
            else
            {
                tabStudentResult.SelectedIndex = 1;
                lbltitleMonth.Content = "លទ្ធផលប្រចាំ " + monthName;
            }
            //------Loading-----
            loading.Close();
            this.IsEnabled = true;
            checkSaveImgae = false;
            Selectresult.Visibility = Visibility.Collapsed;
            //----end loading----

        }

        //-------------------Print Honor Table-------------------------
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            List<StudentMonthlyResult> topStudent = new List<StudentMonthlyResult>();
            string schoolName = Properties.Settings.Default.schoolName;
            string respone = Properties.Settings.Default.studentMonthlyResult;
            string teacher = "", title = "";
            if (tabStudentResult.SelectedIndex == 1)
            {
                var data = report.data.Where(s => s.result_monthly != null);
                foreach (var item in data.OrderBy(s=>s.result_monthly.rank))
                {
                    if (item.result_monthly.rank <= 5)
                    {
                        topStudent.Add(item);
                        teacher = item.instructor.name;
                    }
                }
                title = "month";
            }
            else if (tabStudentResult.SelectedIndex == 2)
            {
                var data = report.data.Where(s => s.result_semester != null);
                foreach (var item in data.OrderBy(r => r.result_semester.rank))
                {
                    if (item.result_semester.rank <= 5)
                    {
                        topStudent.Add(item);
                        teacher = item.instructor.name;
                    }
                }
                title = "semester";
            }
            else if (tabStudentResult.SelectedIndex == 3)
            {
                var data = report.data.Where(s => s.result_yearly != null);
                foreach (var item in data.OrderBy(r => r.result_yearly.rank))
                {
                    if (item.result_yearly.rank <= 5)
                    {
                        topStudent.Add(item);
                        teacher = item.instructor.name;
                    }
                }
                title = "year";
            }
            HonoraryList honorary = new HonoraryList(topStudent, schoolName, teacher, title,yearTitle);
            honorary.Owner = this;
            honorary.ShowDialog();
        }

        private void btnStudenResultDetail_Click(object sender, RoutedEventArgs e)
        {
            StudentMonthlyResult result = DGMonthlyResult.SelectedItem as StudentMonthlyResult;
            StudentResultDetail resultDetail = new StudentResultDetail(result, studentName, report.data, "month");
            this.IsEnabled = false;
            resultDetail.ShowInTaskbar = false;
            resultDetail.ShowDialog();
            this.IsEnabled = true;
        }

        private void btnStudenPrint_Click(object sender, RoutedEventArgs e)
        {
            this.Opacity = 0.5;
            this.IsEnabled = false;
            var student = DGMonthlyResult.SelectedItem as StudentMonthlyResult;
            Transcript transcript = new Transcript(student,title:title, yearTitle: yearTitle);
            transcript.ShowDialog();
            this.Opacity = 1;
            this.IsEnabled = true;
        }
        bool checkVisibleMenu = false;
        private void menuResult_MouseDown(object sender, MouseButtonEventArgs e)
        {
           
            if (checkVisibleMenu)
            {
                ArroBackStuRes.Kind = MaterialDesignThemes.Wpf.PackIconKind.FormatIndentDecrease;
                DockControlTabResul.Margin = new Thickness(10, 0, 0, 0);
                docktree.Visibility = Visibility.Visible;
                checkVisibleMenu = !checkVisibleMenu;
            }
            else
            {
                ArroBackStuRes.Kind = MaterialDesignThemes.Wpf.PackIconKind.FormatIndentIncrease;
                DockControlTabResul.Margin = new Thickness(0, 0, 0, 0);
                docktree.Margin = new Thickness(0, -3, 0, 0);
                docktree.Visibility = Visibility.Collapsed;
                checkVisibleMenu = !checkVisibleMenu;

            }
        }
        //--------------Search and Sort student------------------------------------------
        private void txtStudentName_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                txtStudentName.Text = cmbStudentName.SelectedValue.ToString();
                txtStudentName.Focusable = true;
                txtStudentName.Select(0, txtStudentName.Text.Length);
                cmbStudentName.Text = "";
            }
            catch { }
        }

        private void cmbStudentName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var obj = report.data;
            string name = "";
            int i = 1;

            try
            {
                try
                {
                    txtStudentName.Text = cmbStudentName.SelectedValue.ToString();
                }
                catch
                {

                }
                name = txtStudentName.Text;
                if (tabStudentResult.SelectedIndex == 1)
                {
                    var result = obj.Where(s => s.name.Equals(name));
                    foreach (var item in result)
                    {
                        item.numbers = DateChange.Num(i);
                        if (item.result_monthly.absence_exam.Equals(1))
                        {
                            item.result_monthly.avg_score = "មិនចាត់ថ្នាក់";
                            item.result_monthly.color = "Red";
                        }
                        else
                        {
                            var changeAvg = obj.FirstOrDefault(s => s.result_monthly.avg_score == item.result_monthly.avg_score);
                            changeAvg.result_monthly.avg_score = double.Parse(item.result_monthly.avg_score).ToString("#.##");
                            item.result_monthly.color = "Blue";
                        }
                        i++;
                    }

                    DGMonthlyResult.ItemsSource = null;
                    DGMonthlyResult.ItemsSource = result;
                }
                else if (tabStudentResult.SelectedIndex == 2)
                {
                    var result = obj.Where(s => s.name.Equals(name));
                    foreach (var item in result)
                    {
                        item.numbers = DateChange.Num(i);
                        i++;
                    }
                    DGSemester.ItemsSource = null;
                    DGSemester.ItemsSource = result;
                }
                else if(tabStudentResult.SelectedIndex==3)
                {
                    var result = obj.Where(s => s.name.Equals(name));
                    foreach (var item in result)
                    {
                        item.numbers = DateChange.Num(i);
                        i++;
                    }
                    DGYear.ItemsSource = null;
                    DGYear.ItemsSource = result;
                }
            }
            catch
            {

            }
        }

        private void txtStudentName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtStudentName.Text == "")
            {
                if (tabStudentResult.SelectedIndex == 1)
                {
                    DGMonthlyResult.ItemsSource = null;
                    DGMonthlyResult.ItemsSource = report.data.OrderBy(r => r.result_monthly.rank);
                }
                else if (tabStudentResult.SelectedIndex == 2)
                {
                    DGSemester.ItemsSource = null;
                    DGSemester.ItemsSource = report.data.OrderBy(r => r.result_semester.rank);
                }
                else if(tabStudentResult.SelectedIndex==3)
                {
                    DGYear.ItemsSource = null;
                    DGYear.ItemsSource = report.data.OrderBy(r => r.result_yearly.rank);
                }
                cmbSort.SelectedIndex = 1;
                rbSmall.IsChecked = true;
            }
            else
            {
                cmbStudentName.ItemsSource = null;
                cmbStudentName.ItemsSource = studentName.Where(n => n.Contains(txtStudentName.Text));
                cmbStudentName.IsDropDownOpen = true;
            }
        }
        bool checkStart = false;
        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (checkStart)
            {
                sortData();
                txtStudentName.Text = "";
            }
        }

        //--------------end Search and Sort student------------------------------------------
        private void DGMonthlyResult_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            StudentMonthlyResult result = DGMonthlyResult.SelectedItem as StudentMonthlyResult;
            StudentResultDetail resultDetail = new StudentResultDetail(result, studentName, report.data, "month");
            this.IsEnabled = false;
            this.ShowInTaskbar = true;
            this.Visibility = Visibility.Hidden;
            resultDetail.ShowInTaskbar = false;
            resultDetail.ShowDialog();
            this.Visibility = Visibility.Visible;
            this.IsEnabled = true;
        }

        private void DGSemester_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            StudentMonthlyResult result = DGSemester.SelectedItem as StudentMonthlyResult;
            StudentResultDetail resultDetail = new StudentResultDetail(result, studentName, report.data, "semester");
            this.IsEnabled = false;
            this.ShowInTaskbar = true;
            this.Visibility = Visibility.Hidden;
            resultDetail.ShowInTaskbar = false;
            resultDetail.ShowDialog();
            this.Visibility = Visibility.Visible;
            this.IsEnabled = true;
        }


        private void rbSmall_Checked(object sender, RoutedEventArgs e)
        {
            if (checkStart)
            {
                sortData();
            }
        }

        private void GerateFileAllInOne_Click(object sender, RoutedEventArgs e)
        {
            string title = "";
            if (tabStudentResult.SelectedIndex == 1)
                title = "month";
            else if (tabStudentResult.SelectedIndex == 2)
                title = "semester";
            Transcript transcript = new Transcript(null, report.data.ToList(), true, true, monthName, className, title: title, yearTitle: yearTitle);
            transcript.Show();
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

        private void GenerateFileOther_Click(object sender, RoutedEventArgs e)
        {
            string title = "";
            if (tabStudentResult.SelectedIndex == 1)
                title = "month";
            else if (tabStudentResult.SelectedIndex == 2)
                title = "semester";
            Transcript transcript = new Transcript(null, report.data.ToList(), true, false, title: title, yearTitle: yearTitle);
            transcript.Show();
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

        private void printTranscriptByStudentInOne_Click(object sender, RoutedEventArgs e)
        {
            string title = "";
            if (tabStudentResult.SelectedIndex == 1)
                title = "month";
            else if (tabStudentResult.SelectedIndex == 2)
                title = "semester";
            this.Opacity = 0.5;
            ShowListStudentToPrint show = new ShowListStudentToPrint(report.data.ToList(), title, yearTitle: yearTitle);
            show.Owner = this;
            show.ShowDialog();
            this.Opacity = 1;

        }

        private void DGMonthlyResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //-----------------------------------------------------------------

        private void DateAtt_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var dateValueAtt = DateAtt.SelectedDate.Value.Date;
            lbldayAtt.Content = DateChange.checkDay(dateValueAtt.DayOfWeek.ToString());
            lblMonthAtt.Content = DateChange.checkMonth(int.Parse(dateValueAtt.Month.ToString()));
            lblyearAtt.Content = DateChange.Num(int.Parse(dateValueAtt.Year.ToString()));
            lbldayNumAtt.Content = DateChange.Num(int.Parse(dateValueAtt.Day.ToString()));
        }

        private void printTranscriptAllInOne_Click(object sender, RoutedEventArgs e)
        {
            this.Opacity = 0.5;
            string title = "";
            if (tabStudentResult.SelectedIndex == 1)
                title = "month";
            else if (tabStudentResult.SelectedIndex == 2)
                title = "semester";
            Transcript transcript = new Transcript(null, report.data.ToList(), false, false, "", "", true, title: title,yearTitle:yearTitle);
            transcript.Show();
            this.Opacity = 1;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }
        StudentMonthlyResultData report = new StudentMonthlyResultData();

        private void MenuExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            string title = "";
            if (tabStudentResult.SelectedIndex == 1)
            {
                title = "month";
                var data = report.data.Where(s => s.result_monthly != null);
                data = data.OrderBy(s => s.result_monthly.rank);
                AllSubMonthlyResult result = new AllSubMonthlyResult(false, data.ToList(), title);
                result.Show();
            }
            else if (tabStudentResult.SelectedIndex == 2)
            {
                title = "semester";
                var data = report.data.Where(s => s.result_semester_exam != null);
                data = data.OrderBy(s => s.result_semester_exam.rank);
                AllSubMonthlyResult result = new AllSubMonthlyResult(false,data.ToList(), title);
                result.Show();
            }
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (InternetChecker()&&internet)
            {
                getAllData = true;
                GetMonthlyResultFormApiAsync("not use");
            }
            else
            {
                MessageBox.Show("Save faild, no Internet Connection", "Something went worng", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void sortData()
        {
            try
            {
                string respone = Properties.Settings.Default.studentMonthlyResult;
                var obj = report.data;
                var value = cmbSort.SelectedItem as ComboBoxItem;

                if (tabStudentResult.SelectedIndex == 1)
                {
                    foreach (var item in obj)
                    {
                        if (item.result_monthly.absence_exam.Equals(1))
                        {
                            item.result_monthly.avg_score = "មិនចាត់ថ្នាក់";
                            item.result_monthly.color = "Red";
                        }
                        else
                        {
                            var changeAvg = obj.FirstOrDefault(s => s.result_monthly.avg_score == item.result_monthly.avg_score);
                            changeAvg.result_monthly.avg_score = double.Parse(item.result_monthly.avg_score).ToString("#.##");
                            item.result_monthly.color = "Blue";
                        }
                    }
                    DGMonthlyResult.ItemsSource = null;
                    if (rbSmall.IsChecked == true)
                    {
                        switch (value.Tag.ToString())
                        {
                            case "1":
                                NumberList(obj.OrderBy(r => r.name).ToList());
                                DGMonthlyResult.ItemsSource = obj.OrderBy(r => r.name);
                                report.data = obj.OrderBy(r => r.name).ToList();
                                break;
                            case "2":
                                NumberList(obj.OrderBy(r => r.result_monthly.rank).ToList());
                                DGMonthlyResult.ItemsSource = obj.OrderBy(r => r.result_monthly.rank);
                                report.data = obj.OrderBy(r => r.result_monthly.rank).ToList();
                                break;
                            case "3":
                                NumberList(obj.OrderBy(r => r.result_monthly.total_score).ToList());
                                DGMonthlyResult.ItemsSource = obj.OrderBy(r => r.result_monthly.total_score);
                                report.data = obj.OrderBy(r => r.result_monthly.total_score).ToList();
                                break;
                            case "4":
                                NumberList(obj.OrderBy(r => r.result_monthly.absence_with_permission).ToList());
                                DGMonthlyResult.ItemsSource = obj.OrderBy(r => r.result_monthly.absence_with_permission);
                                report.data = obj.OrderBy(r => r.result_monthly.absence_with_permission).ToList();
                                break;
                            case "5":
                                NumberList(obj.OrderBy(r => r.result_monthly.absence_without_permission).ToList());
                                DGMonthlyResult.ItemsSource = obj.OrderBy(r => r.result_monthly.absence_without_permission);
                                report.data = obj.OrderBy(r => r.result_monthly.absence_without_permission).ToList();
                                break;
                        }
                    }
                    else if (rbBig.IsChecked == true)
                    {
                        switch (value.Tag.ToString())
                        {
                            case "1":
                                NumberList(obj.OrderByDescending(r => r.name).ToList());
                                DGMonthlyResult.ItemsSource = obj.OrderByDescending(r => r.name);
                                report.data = obj.OrderByDescending(r => r.name).ToList();
                                break;
                            case "2":
                                NumberList(obj.OrderByDescending(r => r.result_monthly.rank).ToList());
                                DGMonthlyResult.ItemsSource = obj.OrderByDescending(r => r.result_monthly.rank);
                                report.data = obj.OrderByDescending(r => r.result_monthly.rank).ToList();
                                break;
                            case "3":
                                NumberList(obj.OrderByDescending(r => r.result_monthly.total_score).ToList());
                                DGMonthlyResult.ItemsSource = obj.OrderByDescending(r => r.result_monthly.total_score);
                                report.data = obj.OrderByDescending(r => r.result_monthly.total_score).ToList();
                                break;
                            case "4":
                                NumberList(obj.OrderByDescending(r => r.result_monthly.absence_with_permission).ToList());
                                DGMonthlyResult.ItemsSource = obj.OrderByDescending(r => r.result_monthly.absence_with_permission);
                                report.data = obj.OrderByDescending(r => r.result_monthly.absence_with_permission).ToList();
                                break;
                            case "5":
                                NumberList(obj.OrderByDescending(r => r.result_monthly.absence_without_permission).ToList());
                                DGMonthlyResult.ItemsSource = obj.OrderByDescending(r => r.result_monthly.absence_without_permission);
                                report.data = obj.OrderByDescending(r => r.result_monthly.absence_without_permission).ToList();
                                break;
                        }
                    }
                }
                else if (tabStudentResult.SelectedIndex == 2)
                {
                    DGSemester.ItemsSource = null;
                    if (rbSmall.IsChecked == true)
                    {
                        switch (value.Tag.ToString())
                        {
                            case "1":
                                NumberList(obj.OrderBy(r => r.name).ToList());
                                DGSemester.ItemsSource = obj.OrderBy(r => r.name);
                                report.data = obj.OrderBy(r => r.name).ToList();
                                break;
                            case "2":
                                NumberList(obj.OrderBy(r => r.result_semester_exam.rank).ToList());
                                DGSemester.ItemsSource = obj.OrderBy(r => r.result_semester.rank);
                                report.data = obj.OrderBy(r => r.result_semester.rank).ToList();
                                break;
                            case "3":
                                NumberList(obj.OrderBy(r => r.result_semester_exam.total_score).ToList());
                                DGSemester.ItemsSource = obj.OrderBy(r => r.result_semester_exam.total_score);
                                report.data = obj.OrderBy(r => r.result_semester_exam.total_score).ToList();
                                break;
                            case "4":
                                NumberList(obj.OrderBy(r => r.result_semester.absence_with_permission).ToList());
                                DGSemester.ItemsSource = obj.OrderBy(r => r.result_semester.absence_with_permission);
                                report.data = obj.OrderBy(r => r.result_semester.absence_with_permission).ToList();
                                break;
                            case "5":
                                NumberList(obj.OrderBy(r => r.result_semester.absence_without_permission).ToList());
                                DGSemester.ItemsSource = obj.OrderBy(r => r.result_semester.absence_without_permission);
                                report.data = obj.OrderBy(r => r.result_semester.absence_without_permission).ToList();
                                break;
                        }
                    }
                    else if (rbBig.IsChecked == true)
                    {
                        switch (value.Tag.ToString())
                        {
                            case "1":
                                NumberList(obj.OrderByDescending(r => r.name).ToList());
                                DGSemester.ItemsSource = obj.OrderByDescending(r => r.name);
                                report.data = obj.OrderByDescending(r => r.name).ToList();
                                break;
                            case "2":
                                NumberList(obj.OrderByDescending(r => r.result_semester.rank).ToList());
                                DGSemester.ItemsSource = obj.OrderByDescending(r => r.result_semester.rank);
                                report.data = obj.OrderByDescending(r => r.result_semester.rank).ToList();
                                break;
                            case "3":
                                NumberList(obj.OrderByDescending(r => r.result_semester_exam.total_score).ToList());
                                DGSemester.ItemsSource = obj.OrderByDescending(r => r.result_semester_exam.total_score);
                                report.data = obj.OrderByDescending(r => r.result_semester_exam.total_score).ToList();
                                break;
                            case "4":
                                NumberList(obj.OrderByDescending(r => r.result_semester.absence_with_permission).ToList());
                                DGSemester.ItemsSource = obj.OrderByDescending(r => r.result_semester.absence_with_permission);
                                report.data = obj.OrderByDescending(r => r.result_semester.absence_with_permission).ToList();
                                break;
                            case "5":
                                NumberList(obj.OrderByDescending(r => r.result_semester.absence_without_permission).ToList());
                                DGSemester.ItemsSource = obj.OrderByDescending(r => r.result_semester.absence_without_permission);
                                report.data = obj.OrderByDescending(r => r.result_semester.absence_without_permission).ToList();
                                break;
                        }
                    }
                }
                else if (tabStudentResult.SelectedIndex == 3)
                {
                    DGYear.ItemsSource = null;
                    if (rbSmall.IsChecked == true)
                    {
                        switch (value.Tag.ToString())
                        {
                            case "1":
                                NumberList(obj.OrderBy(r => r.name).ToList());
                                DGYear.ItemsSource = obj.OrderBy(r => r.name);
                                report.data = obj.OrderBy(r => r.name).ToList();
                                break;
                            case "2":
                                NumberList(obj.OrderBy(r => r.result_yearly.rank).ToList());
                                DGYear.ItemsSource = obj.OrderBy(r => r.result_yearly.rank);
                                report.data = obj.OrderBy(r => r.result_yearly.rank).ToList();
                                break;
                            case "3":
                                NumberList(obj.OrderBy(r => r.result_yearly.rank).ToList());
                                DGYear.ItemsSource = obj.OrderBy(r => r.result_yearly.rank);
                                report.data = obj.OrderBy(r => r.result_yearly.rank).ToList();
                                break;
                            case "4":
                                NumberList(obj.OrderBy(r => r.result_yearly.absence_with_permission).ToList());
                                DGYear.ItemsSource = obj.OrderBy(r => r.result_yearly.absence_with_permission);
                                report.data = obj.OrderBy(r => r.result_yearly.absence_with_permission).ToList();
                                break;
                            case "5":
                                NumberList(obj.OrderBy(r => r.result_yearly.absence_without_permission).ToList());
                                DGYear.ItemsSource = obj.OrderBy(r => r.result_yearly.absence_without_permission);
                                report.data = obj.OrderBy(r => r.result_yearly.absence_without_permission).ToList();
                                break;
                        }
                    }
                    else if (rbBig.IsChecked == true)
                    {
                        switch (value.Tag.ToString())
                        {
                            case "1":
                                NumberList(obj.OrderByDescending(r => r.name).ToList());
                                DGYear.ItemsSource = obj.OrderByDescending(r => r.name);
                                report.data = obj.OrderByDescending(r => r.name).ToList();
                                break;
                            case "2":
                                NumberList(obj.OrderByDescending(r => r.result_yearly.rank).ToList());
                                DGYear.ItemsSource = obj.OrderByDescending(r => r.result_yearly.rank);
                                report.data = obj.OrderByDescending(r => r.result_yearly.rank).ToList();
                                break;
                            case "3":
                                NumberList(obj.OrderByDescending(r => r.result_yearly.rank).ToList());
                                DGYear.ItemsSource = obj.OrderByDescending(r => r.result_yearly.rank);
                                report.data = obj.OrderByDescending(r => r.result_yearly.rank).ToList();
                                break;
                            case "4":
                                NumberList(obj.OrderByDescending(r => r.result_yearly.absence_with_permission).ToList());
                                DGYear.ItemsSource = obj.OrderByDescending(r => r.result_yearly.absence_with_permission);
                                report.data = obj.OrderByDescending(r => r.result_yearly.absence_with_permission).ToList();
                                break;
                            case "5":
                                NumberList(obj.OrderByDescending(r => r.result_yearly.absence_without_permission).ToList());
                                DGYear.ItemsSource = obj.OrderByDescending(r => r.result_yearly.absence_without_permission);
                                report.data = obj.OrderByDescending(r => r.result_yearly.absence_without_permission).ToList();
                                break;
                        }
                    }
                }
            }
            catch
            {

            }
        }
        //---------------Save Image-----------------
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
        //------------------------------------------
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
        private void treeViewItemChange(string name)
        {
            var reponse = Properties.Settings.Default.schoolAcademyYear;
            var obj = JObject.Parse(reponse).ToObject<YearofAcademy>().data;
            foreach (var item in obj)
            {
                foreach (var Grade in item.school_system)
                {
                    foreach (var Class in Grade.grade.OrderBy(s => s.sortkey))
                    {
                        foreach (var ClassName in Class.children)
                        {
                            if (ClassName.name == name)
                            {
                                ClassName.color = "#1183CA";
                                Class.color = "#1183CA";
                                Grade.color = "#1183CA";
                                Class.IsExpaned = true;
                                Grade.IsExpande = true;
                                break;
                            }
                        }
                    }
                }
            }
            tvAcademy.ItemsSource = null;
            foreach (var item in obj)
            {
                foreach (var grade in item.school_system)
                {
                    var order = grade.grade.OrderBy(s => s.sortkey).ToList();
                    grade.grade = order;
                }
            }
            tvAcademy.ItemsSource = obj.Where(y => y.name.Equals(year));
        }

        private void btnStatistic_Click(object sender, RoutedEventArgs e)
        {
            Loading loading = new Loading();
            this.IsEnabled = false;
            loading.Show();
            MonthlyStatisticResult monthly = new MonthlyStatisticResult(report.data.ToList());
            loading.Close();
            monthly.Show();
            this.IsEnabled = true;
        }
       

        //private void btnLang_Click(object sender, RoutedEventArgs e)
        //{
        //    if (MateriaLangDrop.Visibility == Visibility.Visible)
        //    {
        //        gridLanguage.Visibility = Visibility.Visible;
        //        MateriaLangDrop.Visibility = Visibility.Collapsed;
        //        MateriaLangUp.Visibility = Visibility.Visible; ;
        //    }
        //    else
        //    {
        //        MateriaLangDrop.Visibility = Visibility.Visible;
        //        MateriaLangUp.Visibility = Visibility.Collapsed;
        //        gridLanguage.Visibility = Visibility.Collapsed;
        //    }
        //}

        //-----------------------------------------

        //---------------------Get Data From api for Each class-------------------
        private async Task<string> GetMonthlyResultFormApiAsync(string use = "use")
        {
            Loading loading = new Loading();
            loading.Owner = this;
            this.IsEnabled = false;
            //----------------AccessUrl and Token---------------------------
            string accessUrl = Properties.Settings.Default.acessUrl;
            string token = Properties.Settings.Default.Token, id = "";

            //---------------------------------------------------------------
            var months = Properties.Settings.Default.monthofTheAcademyYear;
            List<StudentMonthlyResult> photo = new List<StudentMonthlyResult>();
            Thread t = new Thread(async () => downloadPicture(photo));
            var obj = JObject.Parse(months).ToObject<TimesButton>().data;
            int time = 1;
            string responeMonth = "", reponseSemester = "", reponseYear = "", encryptionString = "", encryptionStringSemester = "", encryptionStringYear = "", photos = "";
            loading.Show();
            if(internet&&InternetChecker())
            {
                await Task.Run(async () =>
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
                                photo = JObject.Parse(photos).ToObject<StudentMonthlyResultData>().data;
                                if (t.IsAlive)
                                {

                                    t.Abort();
                                    t.IsBackground = true;
                                    t.Start();
                                }
                                else
                                {
                                    t.IsBackground = true;
                                    t.Start();
                                }
                                time++;
                            }
                            else if (time == 2)
                            {
                                reponseSemester += "|";
                                reponseSemester += await RESTApiHelper.GetAll(accessUrl, "/academic/" + classId + "/semester-result?term=" + "SECOND_SEMESTER", token);
                                time++;
                            }
                        }
                        Console.WriteLine("Done");
                    }
                    try
                    {
                        //await Task.Run(() => downloadPicture(photo));
                    }
                    catch
                    {

                    }
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

                    foreach (var i in photo.Take(1))
                    {
                        if (i.profileMedia.id == null)
                            id = i.student_id;
                        else
                            id = i.profileMedia.id;
                    }
                });
            }
            else
            {
                loading.Close();
                this.IsEnabled = true;
                return null;
            }
            time = 1;
            this.IsEnabled = true;
            loading.Close();
            return encryptionString;
        }
        //---------------------------------------------------------------------------------------------

        //------------------------GetData from String in local Storage---------------------------------
        private string GetStringFromFile(string title = "",string id="")
        {
            try
            {
               if(id=="")
                {
                    string readText = "";
                    if (title == "semester")
                    {
                        readText = File.ReadAllText(filePath + "\\" + "semester" + classId + ".txt");
                    }
                    else if (title == "year")
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
                    else if (title == "year")
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

        private void btnPrintList_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var item = button.DataContext as CardPrint;
            MessageBoxControl message = new MessageBoxControl();
            MessageBoxControl m = new MessageBoxControl();
            m.title = Properties.Langs.Lang.Data;
            m.buttonType = 1;
            message.Owner = this;
            if (item.id >= 1 && item.id <= 4)
            {
                this.Opacity = 0.5;
                message.title = Properties.Langs.Lang.print;
                message.discription = Properties.Langs.Lang.Do_you_want_to_use_the_print_function_ + " " + item.title + "?";
                message.ShowDialog();
                this.Opacity = 1;
                if (message.result == 1 && item.id == 1)
                {
                    this.Opacity = 0.5;
                    if (schoolYearId == "")
                    {
                        m.discription = Properties.Langs.Lang.Select_school_year;
                        m.ShowDialog();
                    }
                    else
                    {
                        StudentList student = new StudentList(ping, 1, YearSelection);
                        student.schoolYearId = schoolYearId;
                        student.Show();
                    }

                }
                else if (message.result == 1 && item.id == 4)
                {
                    if (schoolYearId == "" || classId == "")
                    {
                        m.discription = Properties.Langs.Lang.Please_select_a_school_year_and_class;
                        m.ShowDialog();
                    }
                    else
                    {
                        StudentList student = new StudentList(ping, 2, studentClass);
                        student.classId = classId;
                        student.schoolYearId = schoolYearId;
                        student.Show();
                    }
                }
                else if (message.result == 1 && item.id == 3)
                {
                    if (schoolYearId == "" || gradeId == "")
                    {
                        m.discription = Properties.Langs.Lang.Please_select_a_school_year_and_group;
                        m.ShowDialog();
                    }
                    else
                    {
                        StudentList student = new StudentList(ping, 3, gradName);
                        student.gradeId = gradeId;
                        student.schoolYearId = schoolYearId;
                        student.Show();
                    }
                }
                else if (message.result == 1 && item.id == 2)
                {
                    if (schoolYearId == "" || level == "")
                    {
                        m.discription = Properties.Langs.Lang.Please_select_a_school_year_and_grade;
                        m.ShowDialog();
                    }
                    else
                    {
                        StudentList student = new StudentList(ping, 4, level);
                        student.schoolYearId = schoolYearId;
                        student.level = level;
                        student.Show();
                    }
                }

                this.Opacity = 1;
            }
            else if (item.id == 5)
            {
                    Studemt_Exam_up_class up_Class = new Studemt_Exam_up_class(schoolYearId, YearSelection);      
                    up_Class.Show();
            }
            
            else if (item.id == 6)
            {
                List_of_repeat_students repeat_Students = new List_of_repeat_students(schoolYearId, YearSelection);
                repeat_Students.Show();
            }
            else if (item.id == 7)
            {
                ListStudentCencel studentCencel = new ListStudentCencel(schoolYearId, YearSelection);
                studentCencel.Show();
            }
            else if (item.id == 9)
            {
                StudenPrintMonthlySemesterResult();
            }
            else if (item.id == 10)
            {
                StudentPrintMonthlySemesterTranscrip();
            }
            else if (item.id == 11)
            {
                StudentPrintHonoraryList();
            }
            else if (item.id == 12)
            {
               if(classId==""||term==""||YearSelection=="")
                {
                    this.Opacity = 0.5;
                    MessageBoxControl messageBox = new MessageBoxControl();
                    messageBox.title = Properties.Langs.Lang.print;
                    messageBox.discription = Properties.Langs.Lang.Please_select_a_class_and_result_type;
                    messageBox.buttonType = 1;
                    messageBox.ShowDialog();
                    this.Opacity = 1;
                }
                else
                {
                    Classification classification = new Classification(classId, term, YearSelection, ping);
                    classification.Show();
                }
            }
            else if (item.id == 13)
            {

                AllSubjectPrint();

            }
            else if(item.id==14)
            {
                if (yearId == "" || resulType == "" || studentMonth == "" || term == "" || studentMonth == "")
                {
                    this.Opacity = 0.5;
                    MessageBoxControl messageBox = new MessageBoxControl();
                    messageBox.title = Properties.Langs.Lang.print;
                    messageBox.discription = Properties.Langs.Lang.Please_select_a_class_and_result_type;
                    messageBox.buttonType = 1;
                    messageBox.ShowDialog();
                    this.Opacity = 1;
                }
                else
                {
                    Sumery_of_Students_Short summary = new Sumery_of_Students_Short(yearId, resulType, DateChange.checkMonthString(studentMonth).ToString(), term, studentMonth);
                    summary.Show();
                }
            }
            else if (item.id == 16)
            {
                if(classId==""||month==""||yearTitle==""||studentClass=="")
                {
                    this.Opacity = 0.5;
                    MessageBoxControl messageBox = new MessageBoxControl();
                    messageBox.title = Properties.Langs.Lang.print;
                    messageBox.discription = Properties.Langs.Lang.Please_select_a_class_and_result_type;
                    messageBox.buttonType = 1;
                    messageBox.ShowDialog();
                    this.Opacity = 1;
                }
                else
                {
                    if (month == "0")
                    {
                        Student_Attendance_Year attendance_Year = new Student_Attendance_Year(classId, month, yearTitle, studentClass);
                        attendance_Year.Show();
                    }
                    else
                    {
                        AttendanceReport attendance = new AttendanceReport(classId, month, yearTitle, studentClass);
                        attendance.Show();
                    }
                }
            }
           
            else if (item.id == 25)
            {
                if (YearSelection == "")
                {
                    this.Opacity = 0.5;
                    MessageBoxControl messageBox = new MessageBoxControl();
                    messageBox.title = Properties.Langs.Lang.print;
                    messageBox.discription = Properties.Langs.Lang.Select_school_year;
                    messageBox.buttonType = 1;
                    messageBox.ShowDialog();
                    this.Opacity = 1;
                }
                else
                {
                    this.Opacity = 0.5;
                    StatisticGrade9 statisticGrade9 = new StatisticGrade9(YearSelection);
                    statisticGrade9.ShowDialog();
                    this.Opacity = 1;
                }
            }
            else if (item.id == 26)
            {
                if (YearSelection == "")
                {
                    this.Opacity = 0.5;
                    MessageBoxControl messageBox = new MessageBoxControl();
                    messageBox.title = Properties.Langs.Lang.print;
                    messageBox.discription = Properties.Langs.Lang.Select_school_year;
                    messageBox.buttonType = 1;
                    messageBox.ShowDialog();
                    this.Opacity = 1;
                }
                else
                {
                    this.Opacity = 0.5;
                    StatisticGrade12 statisticGrade12 = new StatisticGrade12(YearSelection);
                    statisticGrade12.ShowDialog();
                    this.Opacity = 1;
                }
            }
            else
            {
                this.Opacity = 0.5;

                message.title = Properties.Langs.Lang.print;
                message.discription = Properties.Langs.Lang.Function + " " + item.title + " " + Properties.Langs.Lang.Under_construction;

                message.title = Properties.Langs.Lang.print;
                message.discription = Properties.Langs.Lang.Function + " " + item.title + " " + Properties.Langs.Lang.Under_construction;
                message.buttonType = 1;
                message.ShowDialog();
                this.Opacity = 1;
            }
            Console.WriteLine(item.id);
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
        //-----------------cmb choice--------------------------------------
        private void cmbChoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var choice = cmbChoice.SelectedItem as ComboBoxItem;
                ShowListPrint(choice.Tag.ToString(),"student");
            }
            catch { }
        }

        private void cmbChoiceStaff_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var choice = cmbChoiceStaff.SelectedItem as ComboBoxItem;
                ShowListPrint(choice.Tag.ToString(),"staff");
            }
            catch { }
        }

        //---------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------

        private string GetDataDate()
        {
            try
            {
                string respone = "";
                respone = GetStringFromFile("year");
                string[] allRespone = respone.Split('|');
                return allRespone[1].ToString();
            }
            catch
            {
                txtDataDate.Visibility = Visibility.Collapsed;
            }
            return null;
        }

        private List<StudentMonthlyResult> GetData(string month = "", string title = "", string semester = "",string id="")
        {
            string respone = "";
            if(id=="")
            {
                respone = GetStringFromFile(title);
            }
            else
            {
                respone = GetStringFromFile(title,id:id);
            }
            if (title == "semester")
            {
                string[] allRespone = respone.Split('|');
                if (semester == "ឆមាសទី១")
                {
                    var obj = JObject.Parse(allRespone[0].ToString()).ToObject<StudentMonthlyResultData>().data;
                    return obj;
                }
                else if (semester == "ឆមាសទី២")
                {
                    var obj = JObject.Parse(allRespone[1].ToString()).ToObject<StudentMonthlyResultData>().data;
                    return obj;
                }
            }
            else if (title == "year")
            {
                string[] allRespone = respone.Split('|');
                string r = allRespone[0];
                var obj = JObject.Parse(r).ToObject<StudentMonthlyResultData>().data;
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
                        return obj;
                    }
                }
            }
            return null;
        }

        //-----------------------Encryption and Discrption---------------------------------------------
        static public string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes
                  = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue
                  = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }
        string YearSelection = "";
        private void cmbAcademicYearResultPrint_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                cmbGradeResultPrint.ItemsSource = null;
                cmbClassResultPrint.ItemsSource = null;
                cmbMonthResultPrint.ItemsSource = null;
                List<string> grade = new List<string>();
                var item = sender as ComboBox;
                var selection = item.SelectedItem;
                YearSelection = selection.ToString();
                studentYear = selection.ToString();
                yearTitle = selection.ToString();
                var obj = JObject.Parse(Properties.Settings.Default.schoolAcademyYear).ToObject<YearofAcademy>().data.Where(y => y.name.Equals(selection.ToString()));
                foreach (var items in obj)
                {
                    schoolYearId = items.id;

                    foreach (var grades in items.school_system)
                    {
                        foreach (var gradeName in grades.grade.OrderBy(s=>s.sortkey))
                        {
                            grade.Add(gradeName.name);
                        }
                    }
                }
                cmbGradeResultPrint.IsEnabled = true;
                cmbGradeResultPrint.ItemsSource = grade;
                cmbGradeAttReportPrint.IsEnabled = true;
                cmbGradeAttReportPrint.ItemsSource = grade;
            }
            catch { }
        }
        private void cmbGradeResultPrint_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                cmbClassResultPrint.ItemsSource = null;
                cmbMonthResultPrint.ItemsSource = null;
                List<KeyValuePair<string, string>> ClassAndId = new List<KeyValuePair<string, string>>();
                var item = sender as ComboBox;
                var selection = item.SelectedItem;
                var obj = JObject.Parse(Properties.Settings.Default.schoolAcademyYear).ToObject<YearofAcademy>().data.Where(y => y.name.Equals(YearSelection));
                foreach (var items in obj)
                {
                    foreach (var grades in items.school_system)
                    {
                        foreach (var gradeName in grades.grade)
                        {
                            if (gradeName.name.Equals(selection))
                            {
                                yearId = gradeName.id;
                                foreach (var className in gradeName.children)
                                {
                                    ClassAndId.Add(new KeyValuePair<string, string>(className.name,className.id.ToString()));
                                }
                            }
                        }
                    }
                }
                cmbClassResultPrint.ItemsSource = ClassAndId;
                cmbClassResultPrint.DisplayMemberPath = "Key";
                cmbClassResultPrint.SelectedValuePath = "Value";
            }
            catch { }
        }
        private async void cmbClassResultPrint_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                cmbMonthResultPrint.ItemsSource = null;
                string accessUrl = Properties.Settings.Default.acessUrl;
                TimesButton timesButton = new TimesButton();
                var items = sender as ComboBox;
                var selection = (KeyValuePair<string,string>) items.SelectedItem;
                studentClass = selection.Key;
                classId = selection.Value;
                List<KeyValuePair<string, string>> button = new List<KeyValuePair<string, string>>();
                if(internet&&InternetChecker())
                {
                    var respone1 = await RESTApiHelper.GetAll(accessUrl, "/academic/" + selection.Value + "/grade-time-shift", token);
                    timesButton = JObject.Parse(respone1).ToObject<TimesButton>();
                    SaveAcademyMonth(respone1, schoolYearId);
                }
                else
                {
                    timesButton = GetAcademyFromLocal(schoolYearId);
                }
                foreach (var item in timesButton.data)
                {
                    foreach (var month in item.months)
                    {
                        button.Add(new KeyValuePair<string, string>(month.displayMonth,item.semester));
                    }
                    button.Add(new KeyValuePair<string, string>(item.name,item.semester));
                }
                button.Add(new KeyValuePair<string, string>("លទ្ធផលប្រចាំឆ្នាំ", ""));
                cmbMonthResultPrint.IsEnabled = true;
                cmbMonthResultPrint.ItemsSource = button;
                cmbMonthResultPrint.DisplayMemberPath = "Key";
                cmbMonthResultPrint.SelectedValuePath = "Value";
            }
            catch { }
        }
        string term = "",resulType="";
        private void cmbMonthResultPrint_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var items = sender as ComboBox;

                var selection = (KeyValuePair<string, string>)items.SelectedItem;
                studentMonth = selection.Key;
                term = selection.Value;
                if(studentMonth.Equals("ឆមាសទី១")|| studentMonth.Equals("ឆមាសទី២"))
                {
                    resulType = "2";
                }
                else if(studentMonth.Equals("លទ្ធផលប្រចាំឆ្នាំ"))
                {
                    resulType = "3";
                }
                else
                {
                    resulType = "1";
                }
                items.BorderBrush = Brushes.Black;

            }
            catch { }
        }
        static public string DecodeFrom64(string encodedData)
        {
            byte[] encodedDataAsBytes
                = System.Convert.FromBase64String(encodedData);
            string returnValue =
               System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
            return returnValue;
        }
        //------------------------------------------------------------------------

        //--------------------------List Print Student---------------------------------------------
        List<List<CardPrint>> list = new List<List<CardPrint>>();
        private void PrintStudent()
        {
            var obj = JObject.Parse(Properties.Settings.Default.schoolAcademyYear).ToObject<YearofAcademy>().data;           
            List<string> year = new List<string>();
            foreach (var item in obj)
            {
                year.Add(item.name);
            }
            cmbAcademicYearResultPrint.ItemsSource = year;
            cmbAcademicYearStudentListPrint.ItemsSource = year;           
            TitlePrint titlePrint = new TitlePrint();
            int id = 0;

            for (int i = 0; i < titlePrint.titel.Count(); i++)
            {
                List<CardPrint> cards = new List<CardPrint>();
                for (int y = 0; y < titlePrint.titel[i].Count(); y++)
                {
                    id++;
                    cards.Add(new CardPrint { id = id, title = titlePrint.titel[i][y], description = titlePrint.discriptionStudent[i], year = year });
                }
                list.Add(cards);
            }

            StudentListPrint.ItemsSource = list[0];
            ScheduleListPrint.ItemsSource = list[1];
            ResultListPrint.ItemsSource = list[2];
            //CardListPrint.ItemsSource = list[3];
            AttReportListPrint.ItemsSource = list[4];
            //NumOfStuReportListPrint.ItemsSource = list[5];
            //RankReportListPrint.ItemsSource = list[6];
            //SubScoreReportListPrint.ItemsSource = list[7];
            //StatisticResultReportListPrint.ItemsSource = list[8];
            StatisticResultGrade12ReportListPrint.ItemsSource = list[9];
            //StatisticbyClassReportListPrint.ItemsSource = list[10];
            //GradeSummaryReportListPrint.ItemsSource = list[11];
            //StudentbyAgeReportListPrint.ItemsSource = list[12];


        }
        //------------------------------------------------------------------------------------------
        //--------------------------List Print Staff---------------------------------------------
        private void PrintStaff()
        {

            List<List<CardPrint>> list = new List<List<CardPrint>>();
            TitlePrint titlePrint = new TitlePrint();
            int id = 0;

            for (int i = 0; i < titlePrint.titleStaff.Count(); i++)
            {
                List<CardPrint> cards = new List<CardPrint>();
                for (int y = 0; y < titlePrint.titleStaff[i].Count(); y++)
                {
                    id++;
                    cards.Add(new CardPrint { id = id, title = titlePrint.titleStaff[i][y], description = titlePrint.discriptionStaff[0] });
                }
                list.Add(cards);
            }
            var obj = JObject.Parse(Properties.Settings.Default.schoolAcademyYear).ToObject<YearofAcademy>().data;
            List<string> year = new List<string>();
            foreach (var item in obj)
            {
                year.Add(item.name);
            }
            cmbAcademicYearPostionPrint.ItemsSource = year;

            AdminReportListPrint.ItemsSource = list[0];
            AcademicReportListPrint.ItemsSource = list[1];
            PositionReportListPrint.ItemsSource = list[2];
            SchoolInformationReportListPrint.ItemsSource = list[3];
            StatisticReportListPrint.ItemsSource = list[4];
            TeacherShecduleReportListPrint.ItemsSource = list[5];
            TeacherAttReportReportListPrint.ItemsSource = list[6];
        }

        //------------------------------------------------------------------------------------------
        private void ShowListPrint(string name,string type)
        {
            if(type=="student")
            {
                foreach (var item in ListPrintStudent.Children)
                {

                    if (item is StackPanel)
                    {
                        var child = (item as StackPanel);
                        if (name != "0")
                        {
                            if (child.Name.ToString() == "L" + name)
                            {
                                child.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                child.Visibility = Visibility.Collapsed;
                            }
                        }
                        else
                        {
                            child.Visibility = Visibility.Visible;
                        }
                    }
                }
            }
            else
            {
                foreach (var item in ListPrintStaff.Children)
                {

                    if (item is StackPanel)
                    {
                        var child = (item as StackPanel);
                        if (name != "0")
                        {
                            if (child.Name.ToString() == "Ls" + name)
                            {
                                child.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                child.Visibility = Visibility.Collapsed;
                            }
                        }
                        else
                        {
                            child.Visibility = Visibility.Visible;
                        }
                    }
                }
            }
            StackCmbStaff.Visibility = Visibility.Visible;
            StackCmb.Visibility = Visibility.Visible;
        }
        //-------Student Monthly Semester Print----------------------------
        string studentYear = "";
        string studentClass = "";
        //-----------------All Student List Combobox Action------------------------
        string schoolYearId = "";
        private void cmbAcademicYearStudentListPrint_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<string> grade = new List<string>();
            var list = new List<KeyValuePair<string, string>>();
            var item = sender as ComboBox;
            var selection = item.SelectedItem;
            YearSelection = selection.ToString();
            var obj = JObject.Parse(Properties.Settings.Default.schoolAcademyYear).ToObject<YearofAcademy>().data.Where(y => y.name.Equals(selection.ToString()));
            foreach (var items in obj)
            {
                schoolYearId = items.id;
                foreach (var grades in items.school_system)
                {
                    foreach (var gradeName in grades.grade.OrderBy(s=>s.sortkey))
                    {
                        list.Add(new KeyValuePair<string, string>(gradeName.name, gradeName.id));
                    }
                }
            }
            cmbGradeStudentListPrint.ItemsSource = list;
            cmbGradeStudentListPrint.DisplayMemberPath = "Key";
            cmbGradeStudentListPrint.SelectedValuePath = "Value";
            st1.Visibility = Visibility.Visible;
            st3.Visibility = Visibility.Visible;
        }
        //---------------Turn on turn off internet------------------
        bool internet = true;
        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
           if(InternetChecker()==true)
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
                message.title = Properties.Langs.Lang.Internet;
                message.discription = Properties.Langs.Lang.No_internet_connection;
                message.subtxt = Properties.Langs.Lang.please_connect_to_the_internet;
                message.buttonType = 2;
                message.Owner = this;
                message.ShowDialog();
                this.Opacity = 1;
            }
        }
        string gradeId = "",gradName="";
        private void cmbGradeStudentListPrint_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                
                var list = new List<KeyValuePair<string, string>>();
                var select = sender as ComboBox;
                var item = (KeyValuePair<string, string>)select.SelectedItem;
                gradeId = item.Value.ToString();
                gradName = item.Key.ToString();
                var obj = JObject.Parse(Properties.Settings.Default.schoolAcademyYear).ToObject<YearofAcademy>().data.Where(y => y.name.Equals(YearSelection));
                foreach (var items in obj)
                {
                    foreach (var grades in items.school_system)
                    {
                        foreach (var gradeName in grades.grade)
                        {
                            if (gradeName.id.Equals(gradeId))
                            {
                                foreach (var className in gradeName.children)
                                {
                                    list.Add(new KeyValuePair<string, string>(className.name, className.id.ToString()));
                                }
                                break;
                            }
                        }
                    }
                }

                st2.Visibility = Visibility.Visible;
                cmbClassStudentListPrint.ItemsSource = list;
                cmbClassStudentListPrint.DisplayMemberPath = "Key";
                cmbClassStudentListPrint.SelectedValuePath = "Value";

            }
            catch(Exception ex) 
            {
                Console.WriteLine("=---------------------------------------"+ex);
            }
        }
        string level = "";
        private void cmbLevelStudentListPrint_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = cmbLevelStudentListPrint.SelectedItem as ComboBoxItem;
            level = item.Content.ToString();
        }

        private void btnPrintListAdmin_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var item = button.DataContext as CardPrint;
            MessageBoxControl message = new MessageBoxControl();
            MessageBoxControl m = new MessageBoxControl();
            m.title = Properties.Langs.Lang.Data;
            m.buttonType = 1;
            message.Owner = this;
            if (item.id==5)
            {
                this.Opacity = 0.5;
                DistributionTeacher distribution = new DistributionTeacher(yearId,yearName);
                distribution.Show();
                this.Opacity = 1;
            }
            else if(item.id==3)
            {
                this.Opacity = 0.5;
                DistributionStaffAcademy distribution = new DistributionStaffAcademy(yearId, yearName);
                distribution.Show();
                this.Opacity = 1;
            }
            else
            {
                this.Opacity = 0.5;

                message.title = Properties.Langs.Lang.print;
                message.discription = Properties.Langs.Lang.Function + item.title + Properties.Langs.Lang.Under_construction;

                message.title = Properties.Langs.Lang.print;
                message.discription = Properties.Langs.Lang.Function + item.title + Properties.Langs.Lang.Under_construction;

                message.buttonType = 1;
                message.ShowDialog();
                this.Opacity = 1;
            }
            Console.WriteLine("Item : "+item.id);
        }

        //-----------------------------------------------------------
        //---------------------------------------------------------------------------
        string studentMonth = "";
        string id = "", month = "", yearPrint = "";

        //.......................................Select Result.................................
        private void btnResultSemester_Click(object sender, RoutedEventArgs e)
        {
            lbltitleMonth.Content = "លទ្ធផលប្រចាំ " + yearTitle;
            tabStudentResult.SelectedIndex = 2;
        }

        private void btnClassification_Semester_Click(object sender, RoutedEventArgs e)
        {
            lbltitleMonth.Content = "ចំណាត់ថ្នាក់ចំណត់ប្រភេទប្រចាំ​ " + yearTitle;
            tabStudentResult.SelectedIndex = 4;
        }

        private void printClasscify_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            Classification classification = new Classification(classId, term, YearSelection, ping);
            classification.Show();
            this.IsEnabled = true;
        }

        List<StudentMonthlyResult> students = new List<StudentMonthlyResult>();
        private void btnExam_Semester_Click(object sender, RoutedEventArgs e)
        {
            lbltitleMonth.Content = "លទ្ធផលប្រលង " + yearTitle;
            report.data = students;
            startProgram = false;
            tabStudentResult.SelectedIndex = 5;
        }

        private async void btnUnApproved_Click(object sender, RoutedEventArgs e)
        {
            Loading load = new Loading();
            this.IsEnabled = false;
            load.Show();
            string accessUrl = Properties.Settings.Default.acessUrl;
            string token = Properties.Settings.Default.Token;
            MessageBoxControl message = new MessageBoxControl();
            if (Teacher.InternetChecker() == true && internet)
            {
                message.title = Properties.Langs.Lang.Information;
                message.discription = Properties.Langs.Lang.Do_you_really_want_to_submit;
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
                        using (HttpResponseMessage res = client.PostAsJsonAsync(accessUrl + "/unapprove-on-request-learning/" + requestId, new PostRequestApproved { month = month, term = term, type = type }).Result)
                        {
                            using (HttpContent content = res.Content)
                            {
                                string datas = await content.ReadAsStringAsync();
                                var obj = JObject.Parse(datas).ToObject<ApprovedLearningResult>();
                                if (obj.data.is_approved == "1")
                                {
                                    btnUnApproved.Visibility = Visibility.Visible;
                                    btnApproved.Visibility = Visibility.Collapsed;
                                    btnCalculate.Visibility = Visibility.Collapsed;
                                }
                                else
                                {
                                    btnUnApproved.Visibility = Visibility.Collapsed;
                                    btnApproved.Visibility = Visibility.Visible;
                                    btnCalculate.Visibility = Visibility.Visible;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                message.title = Properties.Langs.Lang.Internet;
                message.discription = Properties.Langs.Lang.No_internet_connection;
                message.buttonType = 2;
                message.Owner = this;
                this.Opacity = 0.5;
                message.ShowDialog();
                this.Opacity = 1;
            }
            this.IsEnabled = true;
            load.Close();
        }
        string type = "";
        private async void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            Loading load = new Loading();
            this.IsEnabled = false;
            load.Show();
            string accessUrl = Properties.Settings.Default.acessUrl;
            string token = Properties.Settings.Default.Token;
            string url = "";

            if (type == "1")
            {
                url = accessUrl + "/academic/" + classId + "/generate-montly-result";
            }
            else if (type == "2")
            {
                url = accessUrl + "/academic/" + classId + "/generate-semester-result";

            }
            else if (type == "3")
            {
                url = accessUrl + "/academic/" + classId + "/generate-yearly-result";
            }
            MessageBoxControl message = new MessageBoxControl();
            if (Teacher.InternetChecker() == true && internet)
            {
                message.title = Properties.Langs.Lang.Information;
                message.discription = Properties.Langs.Lang.Do_you_really_want_to_do_the_calculations;
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
                        using (HttpResponseMessage res = client.PostAsJsonAsync(url, new PostCalculate { month = month, semester = term }).Result)
                        {
                            using (HttpContent content = res.Content)
                            {
                                string datas = await content.ReadAsStringAsync();
                                var obj = JObject.Parse(datas).ToObject<ApprovedLearningResult>();
                                if (obj.status == "True")
                                {
                                    await GetMonthlyResultFormApiAsync();
                                }
                                else
                                {
                                    message.title = Properties.Langs.Lang.Information;
                                    message.discription = Properties.Langs.Lang.Calculated_scores_not_successful;
                                    message.buttonType = 2;
                                    message.Owner = this;
                                    this.Opacity = 0.5;
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
                message.title = Properties.Langs.Lang.Internet;
                message.discription = Properties.Langs.Lang.No_internet_connection;
                message.buttonType = 2;
                message.Owner = this;
                this.Opacity = 0.5;
                message.ShowDialog();
                this.Opacity = 1;
            }
            this.IsEnabled = true;
            load.Close();
        }

        private async void btnApproved_Click(object sender, RoutedEventArgs e)
        {
            Loading load = new Loading();
            this.IsEnabled = false;
            load.Show();
            string accessUrl = Properties.Settings.Default.acessUrl;
            string token = Properties.Settings.Default.Token;
            MessageBoxControl message = new MessageBoxControl();
            if (Teacher.InternetChecker() == true && internet)
            {
                message.title = Properties.Langs.Lang.Information;
                message.discription = Properties.Langs.Lang.Do_you_really_want_to_submit;
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
                        using (HttpResponseMessage res = client.PostAsJsonAsync(accessUrl + "/approve-on-request-learning/"+requestId, new PostRequestApproved { month = month, term = term, type = type }).Result)
                        {
                            using (HttpContent content = res.Content)
                            {
                                string datas = await  content.ReadAsStringAsync();
                                var obj = JObject.Parse(datas).ToObject<ApprovedLearningResult>();
                                if (obj.data.is_approved == "1")
                                {
                                    btnUnApproved.Visibility = Visibility.Visible;
                                    btnApproved.Visibility = Visibility.Collapsed;
                                    btnCalculate.Visibility = Visibility.Collapsed;
                                }
                                else
                                {
                                    btnUnApproved.Visibility = Visibility.Collapsed;
                                    btnApproved.Visibility = Visibility.Visible;
                                    btnCalculate.Visibility = Visibility.Visible;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                message.title = Properties.Langs.Lang.Internet;
                message.discription = Properties.Langs.Lang.No_internet_connection;
                message.buttonType = 2;
                message.Owner = this;
                this.Opacity = 0.5;
                message.ShowDialog();
                this.Opacity = 1;
            }
            this.IsEnabled = true;
            load.Close();
        }

        //....................................................................................

       
        private async void StudenPrintMonthlySemesterResult()
        {
            if(classId==""|| studentMonth=="")
            {
                MessageBoxControl messageBox = new MessageBoxControl();
                messageBox.Owner = this;
                messageBox.title = Properties.Langs.Lang.print;
                messageBox.discription = Properties.Langs.Lang.Please_select_a_class_and_result_type;
                messageBox.buttonType = 1;
                this.Opacity = 0.5;
                messageBox.ShowDialog();
                this.Opacity = 1;
            }
            else
            {
                try
                {
                    if (!File.Exists(filePath + "\\" + classId + ".txt"))
                        await GetMonthlyResultFormApiAsync();
                    string title = "";
                    var select = studentMonth;

                    if (select.Equals("ឆមាសទី១") || select.Equals("ឆមាសទី២"))
                    {
                        title = "semester";
                        var data = GetDataForPrint();
                        foreach (var i in data)
                        {
                            if (i.result_semester == null)
                            {
                                i.result_semester = new resultSemester { rank = data.Count(), avg_score = "--" };
                                i.result_semester_exam = new resultSemesterExam { total_score = 0 };
                            }
                        }
                        data = data.OrderBy(s => s.result_semester.rank).ToList();
                        MonthlyResult monthlyResult = new MonthlyResult(data, title,yearTitle);
                        monthlyResult.Show();
                    }
                    else if(select.Equals("លទ្ធផលប្រចាំឆ្នាំ"))
                    {
                        title = "year";
                        var data = GetDataForPrint();
                        foreach (var i in data)
                        {
                            if (i.result_yearly == null)
                            {
                                i.result_yearly = new resultYearly { rank = data.Count(), avg_score = "--" };
                            }
                        }
                       data = data.OrderBy(s => s.result_yearly.rank).ToList();
                        MonthlyResult monthlyResult = new MonthlyResult(data, title, yearTitle);
                        monthlyResult.Show();
                    }
                    else
                    {
                        title = "month";
                        var data = GetDataForPrint();
                        if(data==null)
                        {

                        }
                        else
                        {
                            foreach (var i in data)
                            {
                                if (i.result_monthly == null)
                                {
                                    i.result_monthly = new Library.MonthlyResult { rank = data.Count(), avg_score = "--" };
                                }
                            }
                            data = data.OrderBy(s => s.result_monthly.rank).ToList();
                            MonthlyResult monthlyResult = new MonthlyResult(data, title, yearTitle);
                            monthlyResult.Show();
                        }
                    }
                }
                catch
                {
                    MessageBoxControl messageBox = new MessageBoxControl();
                    messageBox.Owner = this;
                    messageBox.title = Properties.Langs.Lang.print;
                    messageBox.discription = Properties.Langs.Lang.Unsuccessful_printing;
                    messageBox.buttonType = 1;
                    this.Opacity = 0.5;
                    messageBox.ShowDialog();
                    this.Opacity = 1;
                }
            }
        }
        private async void StudentPrintMonthlySemesterTranscrip()
        {
            if(classId=="" || studentMonth == "")
            {
                MessageBoxControl messageBox = new MessageBoxControl();
                messageBox.Owner = this;
                messageBox.title = Properties.Langs.Lang.print;
                messageBox.discription = Properties.Langs.Lang.Please_select_a_class_and_result_type;
                messageBox.buttonType = 1;
                this.Opacity = 0.5;
                messageBox.ShowDialog();
                this.Opacity = 1;
            }
            else
            {
                try
                {
                    if (!File.Exists(filePath + "\\" + classId + ".txt"))
                        await GetMonthlyResultFormApiAsync();
                    string title = "";
                    var data = GetDataForPrint();
                    var select = studentMonth;

                    if (select.Equals("ឆមាសទី១") || select.Equals("ឆមាសទី២"))
                    {
                        title = "semester";
                    }
                    else
                    {
                        title = "month";
                    }

                    if (data != null)
                    {
                        ShowListStudentToPrint show = new ShowListStudentToPrint(data, title, yearTitle: yearTitle);
                        show.Owner = this;
                        show.ShowDialog();
                    }
                }
                catch
                {
                    MessageBoxControl messageBox = new MessageBoxControl();
                    messageBox.Owner = this;
                    messageBox.title = Properties.Langs.Lang.print;
                    messageBox.discription = Properties.Langs.Lang.noresultdata;
                    messageBox.buttonType = 1;
                    this.Opacity = 0.5;
                    messageBox.ShowDialog();
                    this.Opacity = 1;
                }
            }
        }
        private async void AllSubjectPrint()
        {
            if (classId == "" || studentMonth == "")
            {
                MessageBoxControl messageBox = new MessageBoxControl();
                messageBox.Owner = this;
                messageBox.title = Properties.Langs.Lang.print;
                messageBox.discription = Properties.Langs.Lang.Please_select_a_class_and_result_type;
                messageBox.buttonType = 1;
                this.Opacity = 0.5;
                messageBox.ShowDialog();
                this.Opacity = 1;
            }
            else
            {
                try
                {
                    if (!File.Exists(filePath + "\\" + classId + ".txt"))
                        await GetMonthlyResultFormApiAsync();
                    string title = "";
                    var data = GetDataForPrint();
                    
                    var select = studentMonth;

                    foreach(var item in data)
                    {
                        if (item.gender == "1")
                            item.gender = "ប្រុស";
                        else
                            item.gender = "ស្រី";
                    }

                    if (select.Equals("ឆមាសទី១") || select.Equals("ឆមាសទី២"))
                    {
                        data = data.Where(r => r.result_semester != null).ToList();
                        data = data.OrderBy(r => r.result_semester.rank).ToList();
                        title = "semester";
                    }
                    else
                    {
                        data = data.Where(r => r.result_monthly != null).ToList();
                        data = data.OrderBy(r => r.result_monthly.rank).ToList();
                        title = "month";
                    }

                    if (data != null)
                    {
                        AllSubMonthlyResult allsub = new AllSubMonthlyResult(true,data,title,yearTitle);
                        allsub.Show();
                    }
                }
                catch
                {
                    MessageBoxControl messageBox = new MessageBoxControl();
                    messageBox.Owner = this;
                    messageBox.title = Properties.Langs.Lang.print;
                    messageBox.discription = Properties.Langs.Lang.noresultdata;
                    messageBox.buttonType = 1;
                    this.Opacity = 0.5;
                    messageBox.ShowDialog();
                    this.Opacity = 1;
                }
            }
        }

        private void btnImportTeacher_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnExmportTeacher_Click(object sender, RoutedEventArgs e)
        {
            TeacherInformationExcel excel = new TeacherInformationExcel();
            excel.Show();
        }
        string yearId = "",yearName="";

        private void cmbGradeAttReportPrint_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                cmbClassAttReportPrint.ItemsSource = null;
                cmbMonthResultPrint.ItemsSource = null;
                List<KeyValuePair<string, string>> ClassAndId = new List<KeyValuePair<string, string>>();
                var item = sender as ComboBox;
                var selection = item.SelectedItem;
                var obj = JObject.Parse(Properties.Settings.Default.schoolAcademyYear).ToObject<YearofAcademy>().data.Where(y => y.name.Equals(YearSelection));
                foreach (var items in obj)
                {
                    foreach (var grades in items.school_system)
                    {
                        foreach (var gradeName in grades.grade)
                        {
                            if (gradeName.name.Equals(selection))
                            {
                                foreach (var className in gradeName.children)
                                {
                                    ClassAndId.Add(new KeyValuePair<string, string>(className.name, className.id.ToString()));
                                }
                            }
                        }
                    }
                }
                cmbClassAttReportPrint.IsEnabled = true;
                cmbClassAttReportPrint.ItemsSource = ClassAndId;
                cmbClassAttReportPrint.DisplayMemberPath = "Key";
                cmbClassAttReportPrint.SelectedValuePath = "Value";
            }
            catch { }
        }

        private async void cmbClassAttReportPrint_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                cmbTypeAttReportPrint.ItemsSource = null;
                string accessUrl = Properties.Settings.Default.acessUrl;
                TimesButton timesButton = new TimesButton();
                var items = sender as ComboBox;
                var selection = (KeyValuePair<string, string>)items.SelectedItem;
                studentClass = selection.Key;
                classId = selection.Value;
                List<KeyValuePair<string, string>> button = new List<KeyValuePair<string, string>>();
                if (internet && InternetChecker())
                {
                    var respone1 = await RESTApiHelper.GetAll(accessUrl, "/academic/" + selection.Value + "/grade-time-shift", token);
                    timesButton = JObject.Parse(respone1).ToObject<TimesButton>();
                    SaveAcademyMonth(respone1, schoolYearId);
                }
                else
                {
                    timesButton = GetAcademyFromLocal(schoolYearId);
                }
                foreach (var item in timesButton.data)
                {
                    foreach (var month in item.months)
                    {
                        button.Add(new KeyValuePair<string, string>(month.displayMonth, item.semester));
                    }
                }
                button.Add(new KeyValuePair<string, string>("ប្រចាំឆ្នាំ", ""));
                cmbTypeAttReportPrint.IsEnabled = true;
                cmbTypeAttReportPrint.ItemsSource = button;
                cmbTypeAttReportPrint.DisplayMemberPath = "Key";
                cmbTypeAttReportPrint.SelectedValuePath = "Value";
            }
            catch { }
        }

        private void DPStaffAtt_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }
        
        List<StaffAttendanceDaily> staffs = new List<StaffAttendanceDaily>();
        StaffAttendanceDailyList listAttStaff = new StaffAttendanceDailyList();
        List<string> staffName = new List<string>();
        object sender; RoutedEventArgs e;
        private async void btnGetAtt_Click(object sender, RoutedEventArgs e)
        {
            Loading loading = new Loading();
            loading.Owner = this;
            loading.Show();
            this.sender = sender;
            this.e = e;
            var bc = new BrushConverter();
            var accessUrl = Properties.Settings.Default.acessUrl;
            var token = Properties.Settings.Default.Token;
            dateForStaffAtt = DPStaffAtt.SelectedDate.Value.ToString("dd/MM/yyyy");
            if (InternetChecker())
            {
                int i = 1;
                var respone = await RESTApiHelper.GetAll(accessUrl, "/staff-attendance-permission?date=" + dateForStaffAtt, token);
                var respone1 = await RESTApiHelper.GetAll(accessUrl, "/get-daily-staff-attendance-report?date=" + dateForStaffAtt, token);
                var obj = JObject.Parse(respone).ToObject<StaffPermissionList>().data;
                var obj1 = JObject.Parse(respone1).ToObject<StaffAttendanceDailyList>().data;
                listAttStaff = JObject.Parse(respone1).ToObject<StaffAttendanceDailyList>();
                foreach (var item in obj)
                {
                    item.number = i.ToString();
                    if (item.is_approve == null)
                    {
                        item.approved_at = "";
                        item.visble = "Visible";
                    }
                    else
                    {
                        item.visble = "Collapsed";
                    }

                    i++;
                }
                i = 1;
                foreach (var item in listAttStaff.data)
                {
                    requestIdPermission = item.id;
                    staffName.Add(item.name);
                    item.number = i.ToString();
                    if (item.gender == "1")
                        item.gender = "ប្រុស";
                    else
                        item.gender = "ស្រី";
                    if (!item.daily_present.morning.in_time.Equals(""))
                        item.mIn = "Red";
                    if (!item.daily_present.morning.out_time.Equals(""))
                        item.mOut = "Red";
                    if (!item.daily_present.afternoon.out_time.Equals(""))
                        item.aOut = "Red";
                    if (!item.daily_present.afternoon.in_time.Equals(""))
                        item.aIn = "Red";
                    i++;
                }
                DGStaffAtt.ItemsSource = obj;
                DGStaffAtt1.ItemsSource = listAttStaff.data;
                staffs = obj1;
            }
            loading.Close();
        }

        private void btnSearchStaffAtt_Click(object sender, RoutedEventArgs e)
        {
            var staff = staffs.Where(j => j.name.Contains(txtSeachStaffName.Text));
            int i = 1;
            foreach (var item in staff)
            {
                requestIdPermission = item.id;
                item.number = i.ToString();
                if (item.gender == "1")
                    item.gender = "ប្រុស";
                else
                    item.gender = "ស្រី";
                if (!item.daily_present.morning.in_time.Equals(""))
                    item.mIn = "Red";
                if (!item.daily_present.morning.out_time.Equals(""))
                    item.mOut = "Red";
                if (!item.daily_present.afternoon.out_time.Equals(""))
                    item.aOut = "Red";
                if (!item.daily_present.afternoon.in_time.Equals(""))
                    item.aIn = "Red";
                i++;
            }
            DGStaffAtt1.ItemsSource​ = null;
            DGStaffAtt1.ItemsSource = staff;
        }

        private void txtSeachStaffName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(txtSeachStaffName.Text.Equals(""))
            {
                int i = 1;
                foreach (var item in staffs)
                {
                    requestIdPermission = item.id;
                    item.number = i.ToString();
                    if (item.gender == "1")
                        item.gender = "ប្រុស";
                    else
                        item.gender = "ស្រី";
                    if (!item.daily_present.morning.in_time.Equals(""))
                        item.mIn = "Red";
                    if (!item.daily_present.morning.out_time.Equals(""))
                        item.mOut = "Red";
                    if (!item.daily_present.afternoon.out_time.Equals(""))
                        item.aOut = "Red";
                    if (!item.daily_present.afternoon.in_time.Equals(""))
                        item.aIn = "Red";
                    i++;
                }
                DGStaffAtt1.ItemsSource​ = null;
                DGStaffAtt1.ItemsSource = staffs;
            }
            else
            {
                var name = staffName.Where(s => s.Contains(txtSeachStaffName.Text));
                cbSeachStaffName.ItemsSource = name;
                cbSeachStaffName.IsDropDownOpen = true;
            }
        }
        private void cbSeachStaffName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int i = 1;
                var name = cbSeachStaffName.SelectedItem.ToString();
                var staff = staffs.Where(j => j.name.Contains(name));
                foreach (var item in staff)
                {
                    requestIdPermission = item.id;
                    item.number = i.ToString();
                    if (item.gender == "1")
                        item.gender = "ប្រុស";
                    else
                        item.gender = "ស្រី";
                    if (!item.daily_present.morning.in_time.Equals(""))
                        item.mIn = "Red";
                    if (!item.daily_present.morning.out_time.Equals(""))
                        item.mOut = "Red";
                    if (!item.daily_present.afternoon.out_time.Equals(""))
                        item.aOut = "Red";
                    if (!item.daily_present.afternoon.in_time.Equals(""))
                        item.aIn = "Red";
                    i++;
                }
                txtSeachStaffName.Text = cbSeachStaffName.SelectedItem.ToString();
                cbSeachStaffName.Text = "";
                DGStaffAtt1.ItemsSource​ = null;
                DGStaffAtt1.ItemsSource = staff;
            }
            catch { }
        }
        private void btnPrintStaffAtt_Click(object sender, RoutedEventArgs e)
        {
            List_Attendance_staff _Staff = new List_Attendance_staff(dateForStaffAtt,false);
            _Staff.Show();
        }

        private void btnStaffAttPrint_Click(object sender, RoutedEventArgs e)
        {
            List_Attendance_staff _Staff = new List_Attendance_staff(dateForStaffAtt,true);
            _Staff.Show();
        }

        private void btnApprovedStaffAttendance_Click(object sender, RoutedEventArgs e)
        {
            var item = DGStaffAtt.SelectedItem as StaffPermission;
            var requestId = item.id;
            if(internet&&InternetChecker())
            {
                this.Opacity = 0.5;
                StaffAttdanceApprovedPopup popup = new StaffAttdanceApprovedPopup(requestId);
                popup.ShowDialog();
                this.Opacity = 1;
                btnGetAtt_Click(sender,e);
            }
        }
        string requestIdPermission = "";
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Loading load = new Loading();
            this.IsEnabled = false;
            load.Show();
            string accessUrl = Properties.Settings.Default.acessUrl;
            string token = Properties.Settings.Default.Token;
            MessageBoxControl message = new MessageBoxControl();
            if (Teacher.InternetChecker() == true && internet)
            {
                message.title = Properties.Langs.Lang.Information;
                message.discription = Properties.Langs.Lang.Do_you_really_want_to_submit;
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
                        using (HttpResponseMessage res = client.PostAsJsonAsync(accessUrl + "/save-daily-staff-attendance", new StaffAttendanceDailyList { date= dateForStaffAtt, data=listAttStaff.data }).Result)
                        {
                            using (HttpContent content = res.Content)
                            {
                                string datas = await content.ReadAsStringAsync();
                            }
                        }
                    }
                }
            }
            else
            {
                message.title = Properties.Langs.Lang.Internet;
                message.discription = Properties.Langs.Lang.No_internet_connection;
                message.buttonType = 2;
                message.Owner = this;
                this.Opacity = 0.5;
                message.ShowDialog();
                this.Opacity = 1;
            }
            this.IsEnabled = true;
            load.Close();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var item = DGStaffAtt1.SelectedItem as StaffAttendanceDaily;
            if(item.late==true)
            {
                item.leave = false;
                item.permission = false;
                item.absent = false;
            }
            else if(item.leave==true)
            {
                item.late = false;
                item.permission = false;
                item.absent = false;
            }
            else if(item.permission==true)
            {
                item.leave = false;
                item.late = false;
                item.absent = false;
            }
            else if(item.absent==true)
            {
                item.leave = false;
                item.permission = false;
                item.late = false;
            }
        }

        private void MaAttInfo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MaAttInfo.Kind == MaterialDesignThemes.Wpf.PackIconKind.ChevronUp)
            {
                gridBodyAttInfo.Visibility = Visibility.Collapsed;
                MaAttInfo.Kind = MaterialDesignThemes.Wpf.PackIconKind.ChevronDown;
            }
            
            else if(gridBodyAttInfo.Visibility == Visibility.Collapsed)
            {
                gridBodyAttInfo.Visibility = Visibility.Visible;
                MaAttInfo.Kind = MaterialDesignThemes.Wpf.PackIconKind.ChevronUp;
            }
           
        }

        private void MaProRequse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MaProRequse.Kind == MaterialDesignThemes.Wpf.PackIconKind.ChevronUp)
            {
                gridPerRequseBody.Visibility = Visibility.Collapsed;
                MaProRequse.Kind = MaterialDesignThemes.Wpf.PackIconKind.ChevronDown;
            }

            else if (gridPerRequseBody.Visibility == Visibility.Collapsed)
            {
                gridPerRequseBody.Visibility = Visibility.Visible;
                MaProRequse.Kind = MaterialDesignThemes.Wpf.PackIconKind.ChevronUp;
            }
        }

        private void cmbTypeAttReportPrint_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var items = sender as ComboBox;

                var selection = (KeyValuePair<string, string>)items.SelectedItem;
                studentMonth = selection.Key;
                month = DateChange.checkMonthString(selection.Key).ToString();
                items.BorderBrush = Brushes.Black;
            }
            catch { }
        }

        private void cmbAcademicYearStatistic12Print_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                List<string> grade = new List<string>();
                var item = sender as ComboBox;
                var selection = item.SelectedItem;
                YearSelection = selection.ToString();
                studentYear = selection.ToString();
                yearTitle = selection.ToString();
                var obj = JObject.Parse(Properties.Settings.Default.schoolAcademyYear).ToObject<YearofAcademy>().data.Where(y => y.name.Equals(selection.ToString()));
                foreach (var items in obj)
                {
                    schoolYearId = items.id;
                    foreach (var grades in items.school_system)
                    {
                        foreach (var gradeName in grades.grade)
                        {
                            grade.Add(gradeName.name);
                        }
                    }
                }
            }
            catch { }
        }

        private void cmbAcademicYearPostionPrint_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = sender as ComboBox;
            var selection = item.SelectedItem;
            var obj = JObject.Parse(Properties.Settings.Default.schoolAcademyYear).ToObject<YearofAcademy>().data.Where(y => y.name.Equals(selection.ToString()));
            foreach (var items in obj)
            {
                yearId = items.id;
                yearName = items.name;
            }
        }

        private async void StudentPrintHonoraryList()
        {
            if (classId == "" || studentMonth == "")
            {
                MessageBoxControl messageBox = new MessageBoxControl();
                messageBox.Owner = this;
                messageBox.title = Properties.Langs.Lang.print;
                messageBox.discription = Properties.Langs.Lang.Please_select_a_class_and_result_type;
                messageBox.buttonType = 1;
                this.Opacity = 0.5;
                messageBox.ShowDialog();
                this.Opacity = 1;
            }
            else
            {
                try
                {
                    if (!File.Exists(filePath + "\\" + classId + ".txt"))
                        await GetMonthlyResultFormApiAsync();
                    List<StudentMonthlyResult> topStudent = new List<StudentMonthlyResult>();
                    string schoolName = Properties.Settings.Default.schoolName;
                    string teacher = "", title = "";
                    var data = GetDataForPrint();

                    if (data != null)
                    {
                        var select = studentMonth;

                        if (select.Equals("ឆមាសទី១") || select.Equals("ឆមាសទី២"))
                        {
                            foreach (var item in data.OrderBy(r => r.result_semester.rank).Take(5))
                            {
                                teacher = item.instructor.name;
                                topStudent.Add(item);
                            }
                            title = "semester";
                        }
                        else if(select.Equals("លទ្ធផលប្រចាំឆ្នាំ"))
                        {
                            foreach (var item in data.OrderBy(r => r.result_yearly.rank).Take(5))
                            {
                                teacher = item.instructor.name;
                                topStudent.Add(item);
                            }
                            title = "year";
                        }
                        else
                        {
                            foreach (var item in data.OrderBy(r => r.result_monthly.rank).Take(5))
                            {
                                teacher = item.instructor.name;
                                topStudent.Add(item);
                            }
                            title = "month";
                        }

                        HonoraryList honorary = new HonoraryList(topStudent, schoolName, teacher, title,yearTitle);
                        honorary.Owner = this;
                        honorary.ShowDialog();
                    }
                }
                catch
                {
                    MessageBoxControl messageBox = new MessageBoxControl();
                    messageBox.Owner = this;
                    messageBox.title = Properties.Langs.Lang.print;
                    messageBox.discription = Properties.Langs.Lang.noresultdata;
                    messageBox.buttonType = 1;
                    this.Opacity = 0.5;
                    messageBox.ShowDialog();
                    this.Opacity = 1;
                }
            }
        }
        private List<StudentMonthlyResult> GetDataForPrint()
        {
            string title = "";
            if(studentMonth== "ឆមាសទី១"||studentMonth== "ឆមាសទី២")
            {
                month = "";
                title = "semester";
            }
            else if(studentMonth == "លទ្ធផលប្រចាំឆ្នាំ")
            {
                month = "";
                title = "year";
            }
            else
            {
                studentMonth = DateChange.checkMonthString(studentMonth).ToString();
                month = studentMonth;
                title = "";
                studentMonth = "";
            }

            List<StudentMonthlyResult> list = new List<StudentMonthlyResult>();
            
            id = studentClass;
            yearPrint = studentYear;
            if (cmbMonthResultPrint.SelectedIndex < 0)
            {
                cmbMonthResultPrint.BorderBrush = Brushes.Red;
            }
            List<StudentMonthlyResult> studentMonthlyResults = new List<StudentMonthlyResult>();
            var stYear = JObject.Parse(Properties.Settings.Default.schoolAcademyYear).ToObject<YearofAcademy>().data.Where(y => y.name.Equals(yearPrint));
            foreach (var item in stYear)
            {
                foreach (var t in item.school_system)
                {
                    foreach (var g in t.grade)
                    {
                        foreach (var j in g.children)
                        {
                            if (j.name == id)
                            {
                                id = j.id.ToString();
                                break;
                            }
                        }
                    }
                }
            }
            studentMonthlyResults = GetData(month, id: id,title:title,semester:studentMonth);
            if (studentMonthlyResults != null)
            {
                list = studentMonthlyResults;
            }
            else
            {
                MessageBoxControl messageBox = new MessageBoxControl();
                messageBox.Owner = this;
                messageBox.title = Properties.Langs.Lang.Download_data;
                messageBox.discription = Properties.Langs.Lang.noresultdata;
                messageBox.buttonType = 1;
                this.Opacity = 0.5;
                messageBox.ShowDialog();
                this.Opacity = 1;
                return null;
            }
            cmbMonthResultPrint.SelectedIndex = -1;
            return list;
        }
        //-----------------------------------------------------------------
        //-----------Get and Save Academy month----------------------------
        private void SaveAcademyMonth(string respone, string year)
        {
            string saveString = respone;
            using (StreamWriter writer = new StreamWriter(filePath + "\\" + "academyYear " + year + ".txt"))
            {
                writer.WriteLine(saveString);
            }
        }
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
        //-------------------------Get Approved---------------------------
        string requestId = "";
        private async Task GetApprovedAsync(string classId, string month, string type, string term)
        {
            string accessUrl = Properties.Settings.Default.acessUrl;
            string token = Properties.Settings.Default.Token;
            try
            {
                string respone = "";
                if (internet && Teacher.InternetChecker())
                {
                    if (type == "1")
                    {
                        respone = await RESTApiHelper.GetAll(accessUrl, "/academic/" + classId + "/get-approve-learning?type=" + type + "&month=" + month + "&term=" + term, token);
                    }
                    else if (type == "2")
                    {
                        respone = await RESTApiHelper.GetAll(accessUrl, "/academic/" + classId + "/get-approve-learning?type=" + type + "&term=" + term, token);
                    }
                    else if (type == "3")
                    {
                        respone = await RESTApiHelper.GetAll(accessUrl, "/academic/" + classId + "/get-approve-learning?type=" + type, token);
                    }
                    var obj = JObject.Parse(respone).ToObject<ApprovedLearningResult>().data;
                    requestId = obj.id;
                    if (obj.is_approved == "1")
                    {
                        btnCalculate.Visibility = Visibility.Collapsed;
                        btnApproved.Visibility = Visibility.Collapsed;
                        btnUnApproved.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        btnCalculate.Visibility = Visibility.Visible;
                        btnApproved.Visibility = Visibility.Visible;
                        btnUnApproved.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    btnCalculate.Visibility = Visibility.Collapsed;
                    btnApproved.Visibility = Visibility.Collapsed;
                    btnUnApproved.Visibility = Visibility.Collapsed;
                }
            }
            catch
            {
                btnCalculate.Visibility = Visibility.Visible;
                btnApproved.Visibility = Visibility.Collapsed;
                btnUnApproved.Visibility = Visibility.Collapsed;
            }
        }
        //----------------------------------------------------------------
        //---------------------- download Image---------------------------
        int time = 1;
        private async Task downloadPicture(List<StudentMonthlyResult> photo)
        {
            Console.WriteLine("Thread:-----------" + Thread.CurrentThread.ManagedThreadId);
            string id = "";
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
            Console.WriteLine(Properties.Langs.Lang.Download_image_done);
        }
        //----------------------------------------------------------------
        ///////////////////
    }

}
