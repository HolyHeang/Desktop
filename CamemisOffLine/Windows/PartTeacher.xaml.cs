using CamemisOffLine.Component;
using Library;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for PartTeacher.xaml
    /// </summary>
    public partial class PartTeacher : Window
    {
        public string YearSelection { get; private set; }
        string classId = "";

        private string studentYear;

        public PartTeacher()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
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
            btnInputScore.Background = Brushes.White;
            lblTitle.Content = Properties.Langs.Lang.score;

            //.............Slide Left..............
            slidLeft.Width = 45;
            gridAcc.Visibility = Visibility.Collapsed;
            lblnameCompany.Visibility = Visibility.Collapsed;
            titleSchool.Visibility = Visibility.Collapsed;


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
            if (Teacher.InternetChecker()==false)
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
            }
            else
            {
                WindowState = System.Windows.WindowState.Normal;
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
            tabcontrolScore.SelectedIndex = 0;
        }

        private void btnInputScore_Click(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            btnLearningResult.Background = (Brush)bc.ConvertFrom("#66D3D3D3");
            btnInputScore.Background = Brushes.White;
            tabcontrolResulandInput.SelectedIndex = 0;
            tabcontrolLearn1.SelectedIndex = 1;
        }

        //......................Hover Slide Left......................
        private void slidLeft_MouseEnter(object sender, MouseEventArgs e)
        {
            slidLeft.Width = 320;
            gridAcc.Visibility = Visibility.Visible;
            lblnameCompany.Visibility = Visibility.Visible;
            titleSchool.Visibility = Visibility.Visible;
            MateriaSettingDrop.Visibility = Visibility.Visible;
            //btnHome.IsEnabled = true;
            MateriaLangDrop.Visibility = Visibility.Visible;
            MateriaSettingDrop.Visibility = Visibility.Visible;
        }

        private void slidLeft_MouseLeave(object sender, MouseEventArgs e)
        {
            slidLeft.Width = 45;
           
            titleSchool.Visibility = Visibility.Collapsed;
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
                tvAcademy.ItemsSource = obj;
                changeAcademyYear = true;
                year = cb.ToString();
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
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void English_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Language = "en-US";


            Properties.Settings.Default.Save();
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void Chinese_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Language = "zh-Hans";


            Properties.Settings.Default.Save();
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private async void tvAcademy_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
               
                //checkMonthButton = true;
                var item = tvAcademy.SelectedItem as TeachingClass;
                //treeViewItemChange(item.id);
                classId = item.id;
                LabelTitle.Content = Properties.Langs.Lang.Message_Box_Stu_Result_Title_select_month;
                
                if (Teacher.InternetChecker()==false)
                {
                    
                    string accessUrl = Properties.Settings.Default.acessUrl;
                    string token = Properties.Settings.Default.Token;
                    var respone = await RESTApiHelper.GetAll(accessUrl, "/get-teaching-subject-class/"+item.id, token);
                    Properties.Settings.Default.teachingSubject = respone;
                    Properties.Settings.Default.Save();
                    var obj = JObject.Parse(respone).ToObject<GetTeachingSubjectClass>().data;
                    treeViewItemChange(item.id);
                   
                    cbSelectSubject.ItemsSource = obj;
                    cbSelectSubject.DisplayMemberPath = "name";
                    cbSelectSubject.SelectedValuePath = "id";
                    //Task<string> task = GetMonthlyResultFormApiAsync();
                }
                else
                {
                    //DataButton = JObject.Parse(Properties.Settings.Default.monthofTheAcademyYear).ToObject<TimesButton>().data;
                }
            }
            catch
            {
                //checkMonthButton = false;
                LabelTitle.Content = Properties.Langs.Lang.Message_Box_Stu_Result_Title;
            }
        }

        private void Vietnam_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Language = "vi-VN";


            Properties.Settings.Default.Save();
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
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

        string SubjectId="";
        private async void cbSelectSubject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var subject = cbSelectSubject.SelectedItem as TeachingSubject;
                SubjectId = subject.id;
                string accessUrl = Properties.Settings.Default.acessUrl;
                string token = Properties.Settings.Default.Token;
                var respone = await RESTApiHelper.GetAll(accessUrl, "/academic/" + classId + "/grade-time-shift", token);
                Properties.Settings.Default.monthofTheAcademyYear = respone;
                Properties.Settings.Default.Save();
                var obj = JObject.Parse(respone).ToObject<TimesButton>().data;
                lButton.ItemsSource = obj;
                lButton1.ItemsSource = obj;
                btnResultofTheYear.Visibility = Visibility.Visible;
                btnResultofTheYear1.Visibility = Visibility.Visible;
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
            tvAcademy.ItemsSource = null;
            tvAcademy.ItemsSource = obj.Where(y => y.name.Equals(year));
        }

        private async void btnMonths_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var month = DateChange.checkMonthString(button.Content.ToString());
            
            MessageBoxControl message = new MessageBoxControl();
            Loading loading = new Loading();
            message.title = "ដំណឹង";
            message.discription = "ទាញទិន្នន័យបានជោគជ័យ";
            message.buttonType = 1;
            loading.Show();
            string accessUrl = Properties.Settings.Default.acessUrl;
            string token = Properties.Settings.Default.Token;
            var respone = await RESTApiHelper.GetAll(accessUrl, "/academic/" + classId + "/monthly-subject-result?month=" + month + "&subject_id=" + SubjectId, token);
            Properties.Settings.Default.monthofTheAcademyYear = respone;
            Properties.Settings.Default.Save();

            var obj = JObject.Parse(respone).ToObject<InputScore>().data;
            NumberList(obj);
            DGScoreMonth.ItemsSource = null;
            DGScoreMonth.ItemsSource = obj;
            loading.Close();
            this.Opacity = 0.5;
            message.ShowDialog();
            this.Opacity = 1;
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
                i++;
            }
        }
        //--------------------------------------------------------
    }
}