using CamemisOffLine.Component;
using CamemisOffLine.Windows;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Library;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
    /// Interaction logic for DistributionStaffAcademy.xaml
    /// </summary>
    public partial class DistributionStaffAcademy : Window
    {
        public DistributionStaffAcademy(string yearid, string year)
        {
            InitializeComponent();
            lbllogoLeft.Content = Properties.Settings.Default.logoNameLeft;
            TitleSchool.Content = Properties.Settings.Default.schoolName;
            lblMonth.Text = "បំណែងចែកភារកិច្ចបុគ្គលិកសិក្សា " + year;
            this.yearid = yearid;
        }
        public DistributionStaffAcademy()
        {
            InitializeComponent();
            lbllogoLeft.Content = Properties.Settings.Default.logoNameLeft;
            TitleSchool.Content = Properties.Settings.Default.schoolName;
            this.yearid = "11000317333438883781";
        }
        string yearid = "";
        int number = 1;
        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Templates);
        private void showData(List<SubClassMapteacher> obj)
        {
            Data.ItemsSource = null;
            Data.ItemsSource = obj;
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            barRight.Visibility = Visibility.Collapsed;
            this.Hide();
            MessageBoxControl message = new MessageBoxControl();
            if (yearid == "")
            {
                message.Owner = this;
                this.Opacity = 0.5;
                message.title = Properties.Langs.Lang.print;
                message.discription = Properties.Langs.Lang.Select_school_year;
                message.buttonType = 1;
                message.ShowDialog();
                this.Opacity = 1;
                this.Close();
            }
            else
            {
                Loading loading = new Loading();
                
                try
                {
                    loading.Show();
                    int j = 1;
                    List<MapteacherCode> obj = new List<MapteacherCode>();
                    List<SubClassMapteacher> subs = new List<SubClassMapteacher>();
                    string repsone = "";
                    if (!File.Exists(filePath + "\\" + "Mapteacher" + yearid + ".txt") && Teacher.InternetChecker())
                    {
                        repsone = await GetDataAsync(yearid);
                    }
                    else
                    {
                        repsone = File.ReadAllText(filePath + "\\" + "Mapteacher" + yearid + ".txt");
                    }
                    obj = JObject.Parse(repsone).ToObject<ListMapteacherCode>().data;
                    
                    foreach (var item in obj)
                    {
                        foreach (var num in item.mapTeacherSubject.OrderBy(s=>s.code.Length).ThenBy(s=>s.code).ToList())
                        {
                            if(num.teacher!=null)
                            {
                                num.number = number.ToString();
                                number++;
                                subs.Add(new SubClassMapteacher
                                {
                                    gender = num.teacher.gender_name,
                                    insructorClass = num.instructor_class_name,
                                    name = num.teacher.name,
                                    number = num.number,
                                    Phone = num.teacher.phone,
                                    subject = num.subject_name,
                                    sub_code = num.code,
                                    teach_class = num.teach_class
                                });
                            }
                        }
                    }
                    List<SubClassMapteacher> copyResult = new List<SubClassMapteacher>();
                    int startIndex = 0, endIndex = 24;
                    Document document = new Document(PageSize.A4, 15, 0, 20, 0);
                    PdfWriter.GetInstance(document, new FileStream(filePath + "\\" + "ResultTemplate" + ".pdf", FileMode.Create));
                    document.Open();
                    GC.Collect();
                    var sub = subs;
                    if (sub.Count <= 24)
                    {
                        if (sub.Count <= 20)
                        {
                            Grid.Dispatcher.Invoke(() =>
                            {
                                showData(sub);
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
                                        showData(sub);
                                        Grid.UpdateLayout();
                                    });
                                    PrintList(document);
                                }
                                else if (i == 1)
                                {
                                    Footer.Visibility = Visibility.Visible;
                                    Header.Visibility = Visibility.Collapsed;
                                    lblMonth.Visibility = Visibility.Collapsed;
                                    Data.Visibility = Visibility.Collapsed;
                                    Grid.Dispatcher.Invoke(() =>
                                    {
                                        showData(sub);
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
                                if (sub[i] != null)
                                {
                                    copyResult.Add(sub[i]);
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
                            if (endIndex == sub.ToList().Count())
                            {
                                if (!footerAvaliable)
                                {
                                    Header.Visibility = Visibility.Collapsed;
                                    lblMonth.Visibility = Visibility.Collapsed;
                                    Body.Visibility = Visibility.Collapsed;
                                    Footer.Visibility = Visibility.Visible;
                                    PrintList(document);
                                }

                                break;
                            }

                            startIndex = endIndex;

                            if (sub.ToList().Count() - endIndex > 28)
                            {
                                endIndex = startIndex + 29;
                                Header.Visibility = Visibility.Collapsed;
                                lblMonth.Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                endIndex = sub.ToList().Count();
                                if (sub.ToList().Count() - startIndex <= 30)
                                {
                                    Header.Visibility = Visibility.Collapsed;
                                    lblMonth.Visibility = Visibility.Collapsed;
                                    Footer.Visibility = Visibility.Visible;
                                    footerAvaliable = true;
                                }

                            }

                        }
                    }

                    document.Close();
                    Process.Start(filePath + "\\" + "ResultTemplate" + ".pdf");
                    loading.Close();
                    this.Close();
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error : "+ex.Message.ToString());
                    message.Owner = this;
                    this.Opacity = 0.5;
                    message.title = Properties.Langs.Lang.print;
                    message.discription = Properties.Langs.Lang.Unsuccessful_printing;
                    message.buttonType = 1;
                    message.ShowDialog();
                    this.Opacity = 1;
                    loading.Close();
                    this.Close();
                }
            }
            this.Close();
        }
        private async Task<string> GetDataAsync(string yearid = "")
        {
            try
            {
                string accessUrl = Properties.Settings.Default.acessUrl;
                string token = Properties.Settings.Default.Token;
                string respone = "";

                respone = await RESTApiHelper.GetAll(accessUrl, "/map-subject-teacher-code/" + yearid, token);

                using (StreamWriter writer = new StreamWriter(filePath + "\\" + "Mapteacher" + yearid + ".txt"))
                {
                    writer.WriteLine(respone);
                }

                return respone;
            }
            catch
            {
                return null;
            }

            return null;
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
                png.ScalePercent(45f);
                document.Add(png);
            }

        }
    }
}
