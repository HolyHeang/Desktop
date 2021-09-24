using CamemisOffLine.Component;
using CamemisOffLine.Windows;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Library;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace CamemisOffLine.Report
{
    /// <summary>
    /// Interaction logic for Student_Attendance_Year.xaml
    /// </summary>
    public partial class Student_Attendance_Year : Window
    {
        string classId = "518", month, titleYear, className;
        public Student_Attendance_Year()
        {
            InitializeComponent();
            lbllogoLeft.Content = Properties.Settings.Default.logoNameLeft;
            TitleSchool.Content = Properties.Settings.Default.schoolName;
        }
        public Student_Attendance_Year(string classId, string month, string titleYear, string className)
        {
            InitializeComponent();
            this.classId = classId;
            this.month = month;
            this.titleYear = titleYear;
            this.className = className;
            lbllogoLeft.Content = Properties.Settings.Default.logoNameLeft;
            TitleSchool.Content = Properties.Settings.Default.schoolName;
        }
        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Templates);
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Loading load = new Loading();
            load.Show();
            try
            {
                TitleSchool.Content = Properties.Settings.Default.schoolName;
                txtClass.Content = "ថ្នាក់​ :" + className;
                lblMonth.Text = "ចំនួនអត្តមានសិស្សក្នុង ឆ្នាំសិក្សា " + titleYear;


                this.Hide();
                var respone = "";
                if (Teacher.InternetChecker())
                {
                    respone = await GetDataAsync(classId);
                }
                else
                {
                    respone = File.ReadAllText(filePath + "\\" + "YearAttendance" + classId + ".txt");
                }
                int numb = 1;
                var obj = JObject.Parse(respone).ToObject<ListStudentYearlyAtt>().data;
                var ab = new List<SubListStudentAtt>();
                foreach (var item in obj)
                {
                    var a = new SubListStudentAtt();
                    int k = 1;
                    foreach(var i in item.all_attendance)
                    {
                        a.name = item.name;
                        a.number = numb.ToString();
                        a.student_school_id = item.student_school_id;
                        if(k==1)
                        {
                            txtMonth1.Content = i.name;
                            a.absent1 = i.total_absent;
                            a.per1 = i.total_permission;
                        }
                        else if(k==2)
                        {
                            txtMonth2.Content = i.name;
                            a.absent2 = i.total_absent;
                            a.per2 = i.total_permission;
                        }
                        else if (k==3)
                        {
                            txtMonth3.Content = i.name;
                            a.absent3 = i.total_absent;
                            a.per3 = i.total_permission;
                        }
                        else if (k==4)
                        {
                            txtMonth4.Content = i.name;
                            a.absent4 = i.total_absent;
                            a.per4 = i.total_permission;
                        }
                        else if (k==5)
                        {
                            txtMonth5.Content = i.name;
                            a.absent5 = i.total_absent;
                            a.per5 = i.total_permission;
                        }
                        else if (k==6)
                        {
                            txtMonth6.Content = i.name;
                            a.semersterAtt1 = i.total_absent;
                            a.semesterPer1 = i.total_permission;
                        }
                        else if (k==7)
                        {
                            txtMonth6.Content = i.name;
                            a.absent6 = i.total_absent;
                            a.per6 = i.total_permission;
                           
                        }
                        else if (k==8)
                        {
                            txtMonth7.Content = i.name;
                            a.absent7 = i.total_absent;
                            a.per7 = i.total_permission;
                           
                        }
                        else if (k==9)
                        {
                            txtMonth8.Content = i.name;
                            a.absent8 = i.total_absent;
                            a.per8 = i.total_permission;
                           
                        }
                        else if (k==10)
                        {
                            txtMonth9.Content = i.name;
                            a.absent9 = i.total_absent;
                            a.per9 = i.total_permission;
                            
                        }
                        else if(k==11)
                        {
                            txtMonth10.Content = i.name;
                            a.absent10 = i.total_absent;
                            a.per10 = i.total_permission;
                        }
                        else if(k==12)
                        {
                            a.semester2 = i.name;
                            a.semersterAtt2 = i.total_absent;
                            a.semesterPer2 = i.total_permission;
                        }
                        else if(k==13)
                        {
                            a.yearly = i.name;
                            a.yearlyAtt = i.total_absent;
                            a.yearlyPer = i.total_permission;
                            a.yearlyTotal = i.total;
                        }
                        k++;
                    }
                    ab.Add(a);
                    numb++;
                }

                
                int startIndex = 0, endIndex = 18;
                Document document = new Document(PageSize.A4.Rotate(), -15, 0, 5, 0);
                PdfWriter.GetInstance(document, new FileStream(filePath + "\\" + "ResultTemplate" + ".pdf", FileMode.Create));
                document.Open();
                GC.Collect();

                List<SubListStudentAtt> copyResult = new List<SubListStudentAtt>();
                if (ab.Count <= 18)
                {
                    if (ab.Count <= 15)
                    {
                        Grid.Dispatcher.Invoke(() =>
                        {
                            showData(ab);
                            Grid.UpdateLayout();
                        });
                        PrintList(document);
                    }
                    else
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            if (i == 0)
                            {
                                Footer.Visibility = Visibility.Collapsed;
                                Grid.Dispatcher.Invoke(() =>
                                {
                                    showData(ab);
                                    Grid.UpdateLayout();
                                });
                                PrintList(document);
                            }
                            else if (i == 1)
                            {
                                Footer.Visibility = Visibility.Visible;
                                Header.Visibility = Visibility.Collapsed;

                                Grid.Dispatcher.Invoke(() =>
                                {
                                    showData(ab);
                                    Grid.UpdateLayout();
                                });
                                PrintList(document);
                            }
                        }
                    }
                }
                else
                {
                    bool footerAvaliable = false;
                    while (true)
                    {
                        copyResult.Clear();
                        for (int i = startIndex; i < endIndex; i++)
                        {
                            if (ab[i] != null)
                            {
                                copyResult.Add(ab[i]);
                            }
                        }
                        if (!footerAvaliable)
                            Footer.Visibility = Visibility.Collapsed;

                        Grid.Dispatcher.Invoke(() =>
                        {
                            showData(copyResult);
                            Grid.UpdateLayout();
                        });
                        PrintList(document);
                        if (endIndex == ab.Count())
                        {
                            if (!footerAvaliable)
                            {
                                Header.Visibility = Visibility.Collapsed;
                                title.Visibility = Visibility.Collapsed;
                                Body.Visibility = Visibility.Collapsed;
                                Footer.Visibility = Visibility.Visible;
                                PrintList(document);
                            }

                            break;
                        }

                        startIndex = endIndex;

                        if (ab.Count() - endIndex > 22)
                        {
                            endIndex = startIndex + 25;
                            if (endIndex > ab.Count)
                                endIndex = ab.Count();
                            Header.Visibility = Visibility.Collapsed;
                            title.Visibility = Visibility.Collapsed;
                            Footer.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            endIndex = ab.ToList().Count();
                            Header.Visibility = Visibility.Collapsed;
                            if (ab.ToList().Count() - startIndex <= 25)
                            {
                                Header.Visibility = Visibility.Collapsed;
                                title.Visibility = Visibility.Collapsed;
                                Footer.Visibility = Visibility.Visible;
                                footerAvaliable = true;
                            }

                        }

                    }
                }
                document.Close();
                Process.Start(filePath + "\\" + "ResultTemplate" + ".pdf");
                this.Close();
            }
            catch
            {
                MessageBoxControl message = new MessageBoxControl();
                message.buttonType = 1;
                message.title = Properties.Langs.Lang.print;
                message.discription = Properties.Langs.Lang.Unsuccessful_printing;
                message.ShowDialog();
            }
            load.Close();
        }

        private void Data_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async Task<string> GetDataAsync(string classId)
        {
            try
            {
                string accessUrl = Properties.Settings.Default.acessUrl;
                string token = Properties.Settings.Default.Token;
                string respone = "";

                respone = await RESTApiHelper.GetAll(accessUrl, "/student-attendance-yearly/" + classId, token);

                using (StreamWriter writer = new StreamWriter(filePath + "\\" + "YearAttendance" + classId + ".txt"))
                {
                    writer.WriteLine(respone);
                }

                return respone;
            }
            catch
            {
                return null;
            }
        }
        private void showData(List<SubListStudentAtt> obj)
        {
            Data.ItemsSource = null;
            
            
            Data.ItemsSource = obj;
        }
        void PrintList(Document document)
        {
            string targetFile = System.IO.Path.GetTempFileName();
            using (FileStream outStream = new FileStream(targetFile, FileMode.Create))
            {

                PngBitmapEncoder enc = new PngBitmapEncoder();
                var bitmap = new RenderTargetBitmap((int)Grid.ActualWidth * 2, (int)Grid.ActualHeight * 2, 120, 120, PixelFormats.Pbgra32);
                bitmap.Render(Grid);
                enc.Frames.Add(BitmapFrame.Create(bitmap));
                enc.Save(outStream);
                bitmap = null;

                outStream.Dispose();
            }
            using (FileStream fs = new FileStream(targetFile, FileMode.Open))
            {
                iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromStream(fs), System.Drawing.Imaging.ImageFormat.Png);
                png.ScalePercent(40f);
                document.Add(png);
            }

        }

    }
}
