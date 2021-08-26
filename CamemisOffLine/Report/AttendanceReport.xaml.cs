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
using System.Threading;
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
    /// Interaction logic for AttendanceReport.xaml
    /// </summary>
    public partial class AttendanceReport : Window
    {
        string classId, month,titleYear,className;
        public AttendanceReport()
        {
            InitializeComponent();
        }
        public AttendanceReport(string classId,string month,string titleYear,string className)
        {
            InitializeComponent();
            this.classId = classId;
            this.month = month;
            this.titleYear = titleYear;
            this.className = className;
        }
        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Templates);

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Loading load = new Loading();
            load.Show();
            try
            {
                txtSchoolName.Text = Properties.Settings.Default.schoolName;
                txtMonthName.Text = DateChange.checkMonth(int.Parse(month));
                txtClass.Text = "ថ្នាក់​ :"+className;
                txtYear.Text = "ឆ្នាំសិក្សា "+titleYear;
               
                   
                this.Hide();
                var respone = "";
                if (Teacher.InternetChecker())
                {
                    respone = await GetDataAsync(classId, month);
                }
                else
                {
                    respone = File.ReadAllText(filePath + "\\" + "Attendance" + classId + "month" + month + ".txt");
                }

                var obj = JObject.Parse(respone).ToObject<ListAttendanceData>().data;
                var ab = new List<SubAttendanceClass>();
                int j = 1, boy = 0, girl = 0;
                foreach (var item in obj)
                {
                    item.number = j.ToString();
                    if (item.gender_name == "ប្រុស")
                        boy++;
                    else
                        girl++;
                    j++;
                    var a = new SubAttendanceClass();
                    foreach (var i in item.month_attendance)
                    {
                        if (i.absent.value == "1")
                        {
                            i.absent.value = "P";
                        }
                        else if (i.absent.value == "2")
                        {
                            i.absent.value = "A";
                        }
                        else
                        {
                            i.absent.value = " ";
                        }
                        if (i.day_number == "1")
                        {
                            a.day1 = "A";
                        }
                        else if (i.day_number == "2")
                        {
                            a.day2 = i.absent.value;
                        }
                        else if (i.day_number == "3")
                        {
                            a.day3 = i.absent.value;
                        }
                        else if (i.day_number == "4")
                        {
                            a.day4 = i.absent.value;
                        }
                        else if (i.day_number == "5")
                        {
                            a.day5 = i.absent.value;
                        }
                        else if (i.day_number == "6")
                        {
                            a.day6 = i.absent.value;
                        }
                        else if (i.day_number == "7")
                        {
                            a.day7 = i.absent.value;
                        }
                        else if (i.day_number == "8")
                        {
                            a.day8 = i.absent.value;
                        }
                        else if (i.day_number == "9")
                        {
                            a.day9 = i.absent.value;
                        }
                        else if (i.day_number == "10")
                        {
                            a.day10 = i.absent.value;
                        }
                        else if (i.day_number == "11")
                        {
                            a.day11 = i.absent.value;
                        }
                        else if (i.day_number == "12")
                        {
                            a.day12 = i.absent.value;
                        }
                        else if (i.day_number == "13")
                        {
                            a.day13 = i.absent.value;
                        }
                        else if (i.day_number == "14")
                        {
                            a.day14 = i.absent.value;
                        }
                        else if (i.day_number == "15")
                        {
                            a.day15 = i.absent.value;
                        }
                        else if (i.day_number == "16")
                        {
                            a.day16 = i.absent.value;
                        }
                        else if (i.day_number == "17")
                        {
                            a.day17 = i.absent.value;
                        }
                        else if (i.day_number == "18")
                        {
                            a.day18 = i.absent.value;
                        }
                        else if (i.day_number == "19")
                        {
                            a.day19 = i.absent.value;
                        }
                        else if (i.day_number == "20")
                        {
                            a.day20 = i.absent.value;
                        }
                        else if (i.day_number == "21")
                        {
                            a.day21 = i.absent.value;
                        }
                        else if (i.day_number == "22")
                        {
                            a.day22 = i.absent.value;
                        }
                        else if (i.day_number == "23")
                        {
                            a.day23 = i.absent.value;
                        }
                        else if (i.day_number == "24")
                        {
                            a.day24 = i.absent.value;
                        }
                        else if (i.day_number == "25")
                        {
                            a.day25 = i.absent.value;
                        }
                        else if (i.day_number == "26")
                        {
                            a.day26 = i.absent.value;
                        }
                        else if (i.day_number == "27")
                        {
                            a.day27 = i.absent.value;
                        }
                        else if (i.day_number == "28")
                        {
                            a.day28 = i.absent.value;
                        }
                        else if (i.day_number == "29")
                        {
                            a.day29 = i.absent.value;
                        }
                        else if (i.day_number == "30")
                        {
                            a.day30 = i.absent.value;
                        }
                        else if (i.day_number == "31")
                        {
                            a.day31 = i.absent.value;
                        }
                    }
                    ab.Add(a);
                }

                txtMan.Text = boy.ToString();
                txtWoman.Text = girl.ToString();
                txtTotal.Text = obj.Count().ToString();
                int startIndex = 0, endIndex = 32;
                Document document = new Document(PageSize.A4.Rotate(), 10, 0, 10, 0);
                PdfWriter.GetInstance(document, new FileStream(filePath + "\\" + "ResultTemplate" + ".pdf", FileMode.Create));
                document.Open();
                GC.Collect();

                List<StudentAttendance> copyResult = new List<StudentAttendance>();
                List<SubAttendanceClass> copyResult1 = new List<SubAttendanceClass>();
                if (obj.Count <= 30)
                {
                    if (obj.Count <= 20)
                    {
                        Grid.Dispatcher.Invoke(() =>
                        {
                            showData(obj, ab);
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
                                    showData(obj, ab);
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
                                    showData(obj, ab);
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
                        copyResult1.Clear();
                        for (int i = startIndex; i < endIndex; i++)
                        {
                            if (obj[i] != null)
                            {
                                copyResult.Add(obj[i]);
                                copyResult1.Add(ab[i]);
                            }
                        }
                        if (!footerAvaliable)
                            Footer.Visibility = Visibility.Collapsed;

                        Grid.Dispatcher.Invoke(() =>
                        {
                            showData(copyResult, copyResult1);
                            Grid.UpdateLayout();
                        });
                        PrintList(document);
                        if (endIndex == obj.ToList().Count())
                        {
                            if (!footerAvaliable)
                            {
                                Header.Visibility = Visibility.Collapsed;
                                body.Visibility = Visibility.Collapsed;
                                Footer.Visibility = Visibility.Visible;
                                PrintList(document);
                            }

                            break;
                        }

                        startIndex = endIndex;

                        if (obj.ToList().Count() - endIndex > 30)
                        {
                            endIndex = startIndex + 33;
                            Header.Visibility = Visibility.Collapsed;
                            Footer.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            endIndex = obj.ToList().Count();
                            Header.Visibility = Visibility.Collapsed;
                            if (obj.ToList().Count() - startIndex <= 29)
                            {
                                Header.Visibility = Visibility.Collapsed;
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
                message.title = "បោះពុម្ភ";
                message.discription = "បោះពុម្ភមិនបានជោគជ័យ";
                message.ShowDialog();
            }
            load.Close();
        }
        private async Task<string> GetDataAsync(string classId,string month)
        {
            try
            {
                string accessUrl = Properties.Settings.Default.acessUrl;
                string token = Properties.Settings.Default.Token;
                string respone = "";

                respone = await RESTApiHelper.GetAll(accessUrl, "/student-attendance-month/" + classId + "/" + month, token);

                using (StreamWriter writer = new StreamWriter(filePath + "\\" + "Attendance" + classId + "month" + month +".txt"))
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
        private void showData(List<StudentAttendance> obj,List<SubAttendanceClass> ab)
        {
            Data.ItemsSource = null;
            Data1.ItemsSource = null;
            Data2.ItemsSource = null;
            Data.ItemsSource = obj;
            Data1.ItemsSource = ab;
            Data2.ItemsSource = obj;
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
                png.ScalePercent(30f);
                document.Add(png);
            }

        }
    }
}
