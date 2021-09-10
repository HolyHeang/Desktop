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
    /// Interaction logic for List_Attendance_staff.xaml
    /// </summary>
    public partial class List_Attendance_staff : Window
    {
        public List_Attendance_staff(string date,bool show)
        {
            InitializeComponent();
            lbllogoLeft.Content = Properties.Settings.Default.logoNameLeft;
            TitleSchool.Content = Properties.Settings.Default.schoolName;
            this.date = date;
            this.show = show;
        }

        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Templates);
        string date = "";
        int number = 1;
        bool show = false;

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Loading load = new Loading();
            load.Show();
            var trimeDate = date.Split('/');
            var day = new DateTime(int.Parse(trimeDate[2]), int.Parse(trimeDate[1]), int.Parse(trimeDate[0]));
            string dayofTheWeek = DateChange.checkDay(day.DayOfWeek.ToString());
            lblDate.Content = dayofTheWeek+" ទី" + trimeDate[0]+ " " + DateChange.checkMonth(int.Parse(trimeDate[1])) + " ឆ្នាំ" + trimeDate[2];
            try
            {
                TitleSchool.Content = Properties.Settings.Default.schoolName;

                this.Hide();
                var respone = "";
                if (Teacher.InternetChecker())
                {
                    respone = await GetDataAsync(date);
                }
                var obj = JObject.Parse(respone).ToObject<StaffAttendanceDailyList>().data;



                foreach (var item in obj)
                {
                    item.number = number.ToString();
                    if (item.gender == "2")
                        item.gender = "ស្រី";
                    else
                        item.gender = "ប្រុស";
                    if (show)
                    {
                        item.show = "Visible";
                    }
                    else
                    {
                        item.daily_present.morning.in_time = "";
                        item.daily_present.morning.out_time = "";
                        item.daily_present.afternoon.in_time = "";
                        item.daily_present.afternoon.out_time = "";
                    }
                    number++;
                }

                int startIndex = 0, endIndex = 11;
                Document document = new Document(PageSize.A4.Rotate(), 0, 0, 0, 0);
                PdfWriter.GetInstance(document, new FileStream(filePath + "\\" + "ResultTemplate" + ".pdf", FileMode.Create));
                document.Open();
                GC.Collect();

                List<StaffAttendanceDaily> copyResult = new List<StaffAttendanceDaily>();
                if (obj.Count <= 8)
                {
                    if (obj.Count <= 5)
                    {
                        Grid.Dispatcher.Invoke(() =>
                        {
                            showData(obj);
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
                                    showData(obj);
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
                                    showData(obj);
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
                            if (obj[i] != null)
                            {
                                copyResult.Add(obj[i]);
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
                        if (endIndex == obj.Count())
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

                        if (obj.Count() - endIndex > 16)
                        {
                            endIndex = startIndex + 17;
                            if (endIndex > obj.Count)
                                endIndex = obj.Count();
                            Header.Visibility = Visibility.Collapsed;
                            title.Visibility = Visibility.Collapsed;
                            Footer.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            endIndex = obj.ToList().Count();
                            Header.Visibility = Visibility.Collapsed;
                            title.Visibility = Visibility.Collapsed;
                            if (obj.ToList().Count() - startIndex <= 17)
                            {
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

        private async Task<string> GetDataAsync(string yearId)
        {
            try
            {
                string accessUrl = Properties.Settings.Default.acessUrl;
                string token = Properties.Settings.Default.Token;

                var respone = await RESTApiHelper.GetAll(accessUrl, "/get-daily-staff-attendance-report?date="+date, token);
                var obj = JObject.Parse(respone).ToObject<StaffAttendanceDailyList>().data;
/*
                using (StreamWriter writer = new StreamWriter(filePath + "\\" + "GiveUpStudent" + yearId + ".txt"))
                {
                    writer.WriteLine(respone);
                }*/

                return respone;
            }
            catch
            {
                return null;
            }
        }
        private void showData(List<StaffAttendanceDaily> obj)
        {
            DG.ItemsSource = null;
            DG.ItemsSource = obj;
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
                png.ScalePercent(49f);
                document.Add(png);
            }

        }
    }
}