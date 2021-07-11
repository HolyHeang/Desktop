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
using System.Printing;
using Library;
using System.Net;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Web.Script.Serialization;
using System.Drawing.Imaging;
using System.IO;
using MonthlyResult = CamemisOffLine.Windows.MonthlyResult;
using CamemisOffLine.Asset;
using CamemisOffLine.Component;
using CamemisOffLine.Report;
using System.Net.NetworkInformation;

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
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(UpdateTimer_Tick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
            DispatcherTimer Internet = new DispatcherTimer();
            Internet.Tick += Internet_Tick;
            Internet.Interval = TimeSpan.FromSeconds(1);
            Internet.Start();

        }

        private void Internet_Tick(object sender, EventArgs e)
        {

            Ping myPing = new Ping();
            int ping = 0;
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
                            this.Opacity = 0.5;
                            MessageBoxControl message = new MessageBoxControl();
                            message.title = "ដំណឹង";
                            message.discription = "សេវាអ៊ីនធឺណែតខ្សោយ!! សូមត្រួតពីនិត្យអ៊ីនធឺណែតរបស់អ្នកម្តងទៀត";
                            message.buttonType = 2;
                            message.ShowDialog();
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
                wifiIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WifiOff;
            }
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
        public string token = Properties.Settings.Default.Token;
        ObservableCollection<GradeTimeButton> DataButton = new ObservableCollection<GradeTimeButton>();
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
         
            OptionStaffAtt.Visibility = Visibility.Collapsed;

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
            StaffAttRe.SelectedIndex = 2;
            //defult home page//
            tabMenu.SelectedIndex = 1;
            //END.........//
            var bc = new BrushConverter();
            btnStaffAttendanceReport.Background = Brushes.White;
            MateriaStaffAtt.Foreground = (Brush)bc.ConvertFrom("#1183CA");
            lblStaffAtt.Foreground = (Brush)bc.ConvertFrom("#1183CA");
            //Tiltle.Content = Properties.Langs.Lang.Staff_Att_Re;
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

                    this.FontFamily = new FontFamily("Khmer OS Siemreab");

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



                    Date.SelectedDate = DateTime.Now;
                    var dateValue = Date.SelectedDate.Value.Date;
                    ////////////////////////////////////////////////////////////////
                    ///dateTime StaffAttendanceRepore Admin

                    lblday.Content = DateChange.checkDay(dateValue.DayOfWeek.ToString());
                    lblMonth.Content = DateChange.checkMonth(int.Parse(dateValue.Month.ToString()));
                    lblyear.Content = DateChange.Num(int.Parse(dateValue.Year.ToString()));
                    lbldayNum.Content = DateChange.Num(int.Parse(dateValue.Day.ToString()));
                    //////////////////////////////////////////////////////////////////////////
                    /////dateTime StanAttendanceRepore Acard
                    ///
                    lbldayAcard.Content = DateChange.checkDay(dateValue.DayOfWeek.ToString());
                    lblMonthAcard.Content = DateChange.checkMonth(int.Parse(dateValue.Month.ToString()));
                    lblyearAcard.Content = DateChange.Num(int.Parse(dateValue.Year.ToString()));
                    lbldayNumAcard.Content = DateChange.Num(int.Parse(dateValue.Day.ToString()));


                    todayDate = DateChange.checkDay(dateValue.DayOfWeek.ToString()) +
                        " ទី" + DateChange.Num(int.Parse(dateValue.Day.ToString())) +
                        " " + DateChange.checkMonth(int.Parse(dateValue.Month.ToString())) +
                        " ឆ្នាំ" + DateChange.Num(int.Parse(dateValue.Year.ToString()));
                    lbldate.Content = todayDate;

                    List<StaffAttendance> at = new List<StaffAttendance>();
                    for (int i = 1; i <= 10; i++)
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
                    DGStudentAtt.ItemsSource = at;

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
        bool checkButtonClick = true;
        private void btnStaffAttendanceReport_Click(object sender, RoutedEventArgs e)
        {
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
            tabMenu.SelectedIndex = 1;

            //if (slideLeft.Width == 320)
            //{
            //DoubleAnimation animation = new DoubleAnimation();
            //animation.From = 320;
            //animation.To = 45;
            //animation.Duration = TimeSpan.FromMilliseconds(150);
            //slideLeft.BeginAnimation(WidthProperty, animation);
            //checkButtonClick = false;
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


            check = false;
            //btnHome.IsEnabled = false;
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

                btnTeacher.Visibility = Visibility.Visible;
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
                btnAbout.Visibility = Visibility.Visible;
                btnColor.Visibility = Visibility.Visible;
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


            Properties.Settings.Default.Save();
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void English_Click_1(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Language = "en-US";
            Properties.Settings.Default.Save();
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();

        }

        private void Chinese_Click_1(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Language = "zh-Hans";

            Properties.Settings.Default.Save();
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();

        }

        private void Vietnam_Click_1(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Language = "vi-VN";

            Properties.Settings.Default.Save();
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();

        }


        //stype Combobox top bar.........
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
            if (WindowState == System.Windows.WindowState.Normal)
            {
                WindowState = System.Windows.WindowState.Maximized;
            }
            else
            {
                WindowState = System.Windows.WindowState.Normal;
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
            MessageBoxResult result = MessageBox.Show(Properties.Langs.Lang.QuseLogout, Properties.Langs.Lang.LogoutWarning, MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
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

        private void txtSub_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        //button tabcontrol in button staff attendent report
        private void AdminStaff_Click(object sender, RoutedEventArgs e)
        {
            GridCursor.Margin = new Thickness(10, 30, 0, 0);
            GridCursor.Width = 170;
            StaffAttRe.SelectedIndex = 2;
        }

        private void AcardmeStaff_Click(object sender, RoutedEventArgs e)
        {
            GridCursor.Margin = new Thickness(180, 30, 0, 0);
            GridCursor.Width = 150;
            StaffAttRe.SelectedIndex = 2;
        }
        //..........................................................


        //button Print in staff attendent Report
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (StaffAttRe.SelectedIndex == 0)
            {/*
                Attendance_Admin report = new Attendance_Admin();
                report.SetSource(at);
                report.setParamet(0, lbldayNum.Content);
                report.setParamet(1, lblMonth.Content);
                report.setParamet(2, txttotalAttno.Text);
                report.setParamet(3, lblday.Content);
                report.setParamet(4, lblyear.Content);
                report.setParamet(5, txttotalAtthave.Text);
                report.setParamet(6, txtotherAdmin.Text);
                report.setParamet(7, lblSchoolAdmin.Content);
                report.setParamet(8, txtAdmintitle.Text);
                report.ShowDialog();*/
            }
            else if (StaffAttRe.SelectedIndex == 1)
            {
                /*Attendance_Academy report = new Attendance_Academy();
                report.SetSource(at1);
                report.setParamet(0, lbldayNumAcard.Content);
                report.setParamet(1, lblMonthAcard.Content);
                report.setParamet(2, txttotalAttAcard.Text);
                report.setParamet(3, lbldayAcard.Content);
                report.setParamet(4, lblyearAcard.Content);
                report.setParamet(5, txtAtthaveAcrad.Text);
                report.setParamet(6, txtotherAcard.Text);
                report.setParamet(7, lblSchoolAcard.Content);
                report.setParamet(8, txttitleAcard.Text);
                report.setParamet(9, txtsalaryAcard.Text);
                report.ShowDialog();*/

            }

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
                AllSubMonthlyResult result = new AllSubMonthlyResult(true, report.data, title);
                loading.Show();
                this.IsEnabled = false;
                result.Show();
                loading.Close();
                this.IsEnabled = true;
            }

            else if (tabStudentResult.SelectedIndex == 2)
            {
                title = "semester";
                AllSubMonthlyResult result = new AllSubMonthlyResult(true, report.data.OrderBy(r => r.result_semester.rank).ToList(), title);
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
                    results = report.data.OrderBy(r => r.result_monthly.rank).ToList();
                }
                else if (tabStudentResult.SelectedIndex == 2)
                {
                    results = report.data.OrderBy(r => r.result_semester.rank).ToList();
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
                MonthlyResult monthlyResult = new MonthlyResult(results, title);
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

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StaffAttRe.SelectedIndex == 0)
            {
                /////////Admin
                var dateValue = Date.SelectedDate.Value.Date;
                lblday.Content = DateChange.checkDay(dateValue.DayOfWeek.ToString());
                lblMonth.Content = DateChange.checkMonth(int.Parse(dateValue.Month.ToString()));
                lblyear.Content = DateChange.Num(int.Parse(dateValue.Year.ToString()));
                lbldayNum.Content = DateChange.Num(int.Parse(dateValue.Day.ToString()));

                todayDate = DateChange.checkDay(dateValue.DayOfWeek.ToString()) +
                   " ទី" + DateChange.Num(int.Parse(dateValue.Day.ToString())) +
                   "  ខែ" + DateChange.checkMonth(int.Parse(dateValue.Month.ToString())) +
                   " ឆ្នាំ" + DateChange.Num(int.Parse(dateValue.Year.ToString()));
                lbldate.Content = todayDate;
            }
            else if (StaffAttRe.SelectedIndex == 1)
            {
                ////////////Acard
                var dateValue = Date.SelectedDate.Value.Date;
                lbldayAcard.Content = DateChange.checkDay(dateValue.DayOfWeek.ToString());
                lblMonthAcard.Content = DateChange.checkMonth(int.Parse(dateValue.Month.ToString()));
                lblyearAcard.Content = DateChange.Num(int.Parse(dateValue.Year.ToString()));
                lbldayNumAcard.Content = DateChange.Num(int.Parse(dateValue.Day.ToString()));

                todayDate = DateChange.checkDay(dateValue.DayOfWeek.ToString()) +
                   " ទី" + DateChange.Num(int.Parse(dateValue.Day.ToString())) +
                   "  ខែ" + DateChange.checkMonth(int.Parse(dateValue.Month.ToString())) +
                   " ឆ្នាំ" + DateChange.Num(int.Parse(dateValue.Year.ToString()));
                lbldate.Content = todayDate;
            }

        }
        //Internte Checker
        static public bool InternetChecker()
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
                        Properties.Settings.Default.monthofTheAcademyYear = respone;
                        Properties.Settings.Default.Save();
                        getAllData = true;
                        Task<string> task = GetMonthlyResultFormApiAsync();

                    }
                    else
                    {
                        getAllData = true;
                        DataButton = JObject.Parse(Properties.Settings.Default.monthofTheAcademyYear).ToObject<TimesButton>().data;

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
            LabelTitle.Content = Properties.Langs.Lang.Message_Box_Stu_Result_Title_select_year;

            tvAcademy.ItemsSource = null;
            var cb = cbAcademyYear.SelectedValue;
            var obj = JObject.Parse(Properties.Settings.Default.schoolAcademyYear).ToObject<YearofAcademy>().data.Where(y => y.name.Equals(cb.ToString()));
            tvAcademy.ItemsSource = obj;
        }

        string classId = "", monthName = "", className = "", yearTitle = "";
        List<string> studentName = new List<string>();

        //-------Get Monthly and Semester Reslut-------------------------------
        private async void btnSemester_Click(object sender, RoutedEventArgs e)
        {
            txtDataDate.Content = GetDataDate();
            txtResult.Visibility = Visibility.Visible;
            cmbResult.Visibility = Visibility.Visible;
            lbltitleMonth.Visibility = Visibility.Visible;
            btnStatistic.Visibility = Visibility.Visible;
            DGMonthlyResult.Visibility = Visibility.Visible;
            checkStart = true;
            int girlTotal = 0;
            int number = 1;
            var _tag = e.Source as Button;
            var button = sender as Button;
            string term = _tag.Tag.ToString(), monthName = button.Content.ToString();
            yearTitle = monthName;
            var month = DateChange.checkMonthString(button.Content.ToString());
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
                    item.localProfileLink = filePath + "\\" + item.student_schoolyear_id + ".jpg";
                    if (item.result_semester.is_fail.Equals("1"))
                    {
                        item.result_semester.is_fail = "ធ្លាក់";
                    }
                    else
                    {
                        item.result_semester.is_fail = "ជាប់";
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

                if (startProgram == true)
                {
                    NumberList(obj.OrderBy(s => s.result_semester.rank).ToList());
                    DGSemester.ItemsSource = null;
                    DGSemester.ItemsSource = obj.OrderBy(s => s.result_semester.rank);
                }
                else
                {
                    NumberList(obj.OrderBy(s => s.result_semester.rank).ToList());
                    DGSemester.ItemsSource = obj.OrderBy(s => s.result_semester.rank);
                }
                report.data = obj.OrderBy(s => s.result_semester_exam.total_score).ToList();
                lblTitleTotalStudent.Content = "សិស្សសរុប : " + DGSemester.Items.Count.ToString() + " នាក់" + " ស្រី : " + girlTotal.ToString() + " នាក់";
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
        private async void btnMonths_Click(object sender, RoutedEventArgs e)
        {
            txtDataDate.Content = GetDataDate();
            txtResult.Visibility = Visibility.Collapsed;
            cmbResult.Visibility = Visibility.Collapsed;
            lbltitleMonth.Visibility = Visibility.Visible;
            btnStatistic.Visibility = Visibility.Visible;
            DGMonthlyResult.Visibility = Visibility.Visible;
            checkStart = true;
            int girlTotal = 0;
            int number = 1;
            var _tag = e.Source as Button;
            var button = sender as Button;
            string term = _tag.Tag.ToString(), monthName = button.Content.ToString();
            var month = DateChange.checkMonthString(button.Content.ToString());
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
                    monthName = DateChange.checkMonth(int.Parse(item.result_monthly.month));
                    studentName.Add(item.name);
                    item.localProfileLink = filePath + "\\" + item.student_schoolyear_id + ".jpg";
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
                lblTitleTotalStudent.Content = "សិស្សសរុប : " + DGMonthlyResult.Items.Count.ToString() + " នាក់" + " ស្រី : " + girlTotal.ToString() + " នាក់";
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
                tvAcademy.ItemsSource = obj;
                Month.Visibility = Visibility.Collapsed;
                lbltitleGrade.Content = "ឆ្នាំសិក្សា" + cbAcademyYear.SelectedValue.ToString();
                changeAcademyYear = true;
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
            Transcript transcript = new Transcript(null, null, true, true);
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
            gridMonth.Visibility = Visibility.Visible;
            tabStudentResult.Visibility = Visibility.Collapsed;
            gridStudentResult.Margin = new Thickness(0, -42, 0, 0);
            gridHeadernumstu.Visibility = Visibility.Collapsed;
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
                foreach (var item in report.data.OrderBy(r => r.result_monthly.rank))
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
                foreach (var item in report.data.OrderBy(r => r.result_semester.rank))
                {
                    if (item.result_semester.rank <= 5)
                    {
                        topStudent.Add(item);
                        teacher = item.instructor.name;
                    }
                }
                title = "semester";
            }
            HonoraryList honorary = new HonoraryList(topStudent, schoolName, teacher, title);
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
            var student = DGMonthlyResult.SelectedItem as StudentMonthlyResult;
            Transcript transcript = new Transcript(student);
            transcript.ShowDialog();
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
                    }
                    DGSemester.ItemsSource = null;
                    DGSemester.ItemsSource = result;
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
            Transcript transcript = new Transcript(null, report.data.ToList(), true, true, monthName, className, title: title);
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
            Transcript transcript = new Transcript(null, report.data.ToList(), true, false, title: title);
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
            ShowListStudentToPrint show = new ShowListStudentToPrint(report.data.ToList(), title);
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
            Transcript transcript = new Transcript(null, report.data.ToList(), false, false, "", "", true, title: title);
            transcript.Show();
            this.Opacity = 1;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }
        StudentMonthlyResultData report = new StudentMonthlyResultData();

        private void MenuExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            string title = "";
            if (tabStudentResult.SelectedIndex == 1)
                title = "month";
            else if (tabStudentResult.SelectedIndex == 2)
                title = "semester";
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            AllSubMonthlyResult result = new AllSubMonthlyResult(false, report.data, title);
            result.Show();
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (InternetChecker()&&internet)
            {
                getAllData = true;
                GetMonthlyResultFormApiAsync();
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
                    foreach (var Class in Grade.grade)
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
        private void cmbResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var value = cmbResult.SelectedItem as ComboBoxItem;
                if (value.Tag.ToString() == "1")
                {
                    lbltitleMonth.Content = value.Content + yearTitle;
                    DGSemester.Columns[5].Visibility = Visibility.Collapsed;
                    DGSemester.Columns[8].Visibility = Visibility.Collapsed;
                    DGSemester.Columns[9].Visibility = Visibility.Collapsed;
                    DGSemester.Columns[10].Visibility = Visibility.Collapsed;
                }
                else
                {
                    lbltitleMonth.Content = value.Content + yearTitle;
                    DGSemester.Columns[5].Visibility = Visibility.Visible;
                    DGSemester.Columns[8].Visibility = Visibility.Visible;
                    DGSemester.Columns[9].Visibility = Visibility.Visible;
                    DGSemester.Columns[10].Visibility = Visibility.Visible;
                }
            }
            catch
            {

            }
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
        private async Task<string> GetMonthlyResultFormApiAsync()
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
                id = i.student_schoolyear_id;
            }

            if (IsImageHave(id))
            {
                MessageBoxResult result = MessageBox.Show("Do you want to update image?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    foreach (var item in photo)
                    {
                        if (time == 1)
                        {
                            SaveImage(item.instructor.profileMedia.file_name, ImageFormat.Jpeg, item.instructor.profileMedia.file_show);
                            time++;
                        }
                        SaveImage(item.student_schoolyear_id, ImageFormat.Jpeg, item.profileMedia.file_show);
                        GC.Collect();
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
                    SaveImage(item.student_schoolyear_id, ImageFormat.Jpeg, item.profileMedia.file_show);
                    GC.Collect();
                }
            }
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

        private void btnPrintList_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var item = button.DataContext as CardPrint;
            MessageBoxControl message = new MessageBoxControl();
            message.Owner = this;
            if(item.id>=1&&item.id<=4)
            {
                this.Opacity = 0.5;
                message.title = "បោះពុម្ភ";
                message.discription = "តើអ្នកចង់ប្រើមុខងារបោះពុម្ភ" + item.title + "?";
                message.ShowDialog();
                this.Opacity = 1;
                if (message.result == 1&&item.id==1)
                {
                    
                    StudentList student = new StudentList();
                    student.schoolYearId = schoolYearId;
                    student.Show();
                }
            }
            else if(item.id==6)
            {
                StudenPrintMonthlySemesterResult();
            }
            else if(item.id==7)
            {
                StudentPrintMonthlySemesterTranscrip();
            }
            else if(item.id==8)
            {
                StudentPrintHonoraryList();
            }
            else
            {
                this.Opacity = 0.5;
                message.title = "បោះពុម្ភ";
                message.discription = "មុខងារ"+ item.title +"កំពុងសាងសង់";
                message.buttonType = 1;
                message.ShowDialog();
                this.Opacity = 1;
            }
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
                respone = GetStringFromFile("Year");
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
                List<string> grade = new List<string>();
                var item = sender as ComboBox;
                var selection = item.SelectedItem;
                YearSelection = selection.ToString();
                studentYear = selection.ToString();
                var obj = JObject.Parse(Properties.Settings.Default.schoolAcademyYear).ToObject<YearofAcademy>().data.Where(y => y.name.Equals(selection.ToString()));
                foreach (var items in obj)
                {
                    foreach (var grades in items.school_system)
                    {
                        foreach (var gradeName in grades.grade)
                        {
                            grade.Add(gradeName.name);
                        }
                    }
                }
                cmbGradeResultPrint.IsEnabled = true;
                cmbGradeResultPrint.ItemsSource = grade;
            }
            catch { }
        }
        private void cmbGradeResultPrint_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                List<string> _class = new List<string>();
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
                                    _class.Add(className.name);
                                }
                                break;
                            }
                        }
                    }
                }

                cmbClassResultPrint.IsEnabled = true;
                cmbClassResultPrint.ItemsSource = _class;
            }
            catch { }
        }
        private void cmbClassResultPrint_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var items = sender as ComboBox;
                var selection = items.SelectedItem;
                studentClass = selection.ToString();
                List<string> button = new List<string>();
                var obj = JObject.Parse(Properties.Settings.Default.monthofTheAcademyYear).ToObject<TimesButton>().data;
                foreach (var item in obj)
                {
                    foreach (var month in item.months)
                    {
                        button.Add(month.displayMonth);
                    }
                    button.Add(item.name);
                }
                cmbMonthResultPrint.IsEnabled = true;
                cmbMonthResultPrint.ItemsSource = button;
            }
            catch { }
        }

        private void cmbMonthResultPrint_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var items = sender as ComboBox;
                var selection = items.SelectedItem;
                studentMonth = selection.ToString();
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
            StatisticbyClassReportListPrint.ItemsSource = list[10];
            //GradeSummaryReportListPrint.ItemsSource = list[11];
            StudentbyAgeReportListPrint.ItemsSource = list[12];


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
            var item = sender as ComboBox;
            var selection = item.SelectedItem;
            var obj = JObject.Parse(Properties.Settings.Default.schoolAcademyYear).ToObject<YearofAcademy>().data.Where(y => y.name.Equals(selection.ToString()));
            foreach(var items in obj)
            {
                schoolYearId = items.id;
            }
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
                message.title = "អ៊ិនធឺណែត";
                message.discription = "មិនមានការភ្ជាប់អ៊ិនធឺណែត";
                message.buttonType = 2;
                message.Owner = this;
                message.ShowDialog();
                this.Opacity = 1;
            }
        }
        //-----------------------------------------------------------
        //---------------------------------------------------------------------------
        string studentMonth = "";
        string id = "", month = "", yearPrint = "";
        private void StudenPrintMonthlySemesterResult()
        {
            try
            {
                var data = GetDataForPrint().OrderByDescending(s => s.result_monthly.avg_score).ToList();
                if (data != null)
                {
                    MonthlyResult monthlyResult = new MonthlyResult(data, "month");
                    monthlyResult.Show();
                }
            }
            catch
            {
                MessageBoxControl messageBox = new MessageBoxControl();
                messageBox.Owner = this;
                messageBox.title = "បោះពុម្ភ";
                messageBox.discription = "មិនមានទិន្នន័យ";
                messageBox.buttonType = 1;
                this.Opacity = 0.5;
                messageBox.ShowDialog();
                this.Opacity = 1;
            }
        }
        private void StudentPrintMonthlySemesterTranscrip()
        {
            try
            {
                var data = GetDataForPrint();

                if (data != null)
                {
                    ShowListStudentToPrint show = new ShowListStudentToPrint(data, "month");
                    show.Owner = this;
                    show.ShowDialog();
                }
            }
            catch
            {
                MessageBoxControl messageBox = new MessageBoxControl();
                messageBox.Owner = this;
                messageBox.title = "បោះពុម្ភ";
                messageBox.discription = "មិនមានទិន្នន័យ";
                messageBox.buttonType = 1;
                this.Opacity = 0.5;
                messageBox.ShowDialog();
                this.Opacity = 1;
            }
        }
        private void StudentPrintHonoraryList()
        {
            try
            {
                List<StudentMonthlyResult> topStudent = new List<StudentMonthlyResult>();
                string schoolName = Properties.Settings.Default.schoolName;
                string teacher = "", title = "";
                var data = GetDataForPrint();
                if (data != null)
                {
                    foreach (var item in data.OrderByDescending(r => r.result_monthly.avg_score).Take(5))
                    {
                        teacher = item.instructor.name;
                        topStudent.Add(item);
                    }
                    title = "month";
                    HonoraryList honorary = new HonoraryList(topStudent, schoolName, teacher, title);
                    honorary.Owner = this;
                    honorary.ShowDialog();
                }
            }
            catch
            {
                MessageBoxControl messageBox = new MessageBoxControl();
                messageBox.Owner = this;
                messageBox.title = "បោះពុម្ភ";
                messageBox.discription = "មិនមានទិន្នន័យ";
                messageBox.buttonType = 1;
                this.Opacity = 0.5;
                messageBox.ShowDialog();
                this.Opacity = 1;
            }
        }
        private List<StudentMonthlyResult> GetDataForPrint()
        {
            studentMonth = DateChange.checkMonthString(studentMonth).ToString();

            List<StudentMonthlyResult> list = new List<StudentMonthlyResult>();
            month = studentMonth;
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
            studentMonthlyResults = GetData(month, id: id);
            if (studentMonthlyResults != null)
            {
                list = studentMonthlyResults;
            }
            else
            {
                MessageBoxControl messageBox = new MessageBoxControl();
                messageBox.Owner = this;
                messageBox.title = "ទាញទិន្នន័យ";
                messageBox.discription = "មិនមានទិន្នន័យ";
                messageBox.result = 1;
                this.Opacity = 0.5;
                messageBox.ShowDialog();
                this.Opacity = 1;
                return null;
            }
            cmbMonthResultPrint.SelectedIndex = -1;
            return list;
        }
        //-----------------------------------------------------------------
         ///////////////////
    }

}
