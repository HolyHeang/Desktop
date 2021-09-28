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
    /// Interaction logic for DistributionTeacher.xaml
    /// </summary>
    public partial class DistributionTeacher : Window
    {
        string yearid = "", term = "";
        public DistributionTeacher(string yearid, string year)
        {
            InitializeComponent();
            lbllogoLeft.Content = Properties.Settings.Default.logoNameLeft;
            TitleSchool.Content = Properties.Settings.Default.schoolName;
            txtYear.Content = year;
            this.yearid = yearid;
        }
        public DistributionTeacher()
        {
            InitializeComponent();
            lbllogoLeft.Content = Properties.Settings.Default.logoNameLeft;
            TitleSchool.Content = Properties.Settings.Default.schoolName;
        }
        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Templates);
        private void showData(List<ClassificationTeacer> obj)
        {
            Data.ItemsSource = null;
            Data.ItemsSource = obj;
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            barRight.Visibility = Visibility.Collapsed;
            this.Hide();
            MessageBoxControl message = new MessageBoxControl();
            if (yearid=="")
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
                    List<ClassificationTeacer> obj = new List<ClassificationTeacer>();
                    string repsone = "";
                    if (!File.Exists(filePath + "\\" + "teacher" + yearid + ".txt") && Teacher.InternetChecker())
                    {
                        repsone = await GetDataAsync(yearid);
                    }
                    else
                    {
                        repsone = File.ReadAllText(filePath + "\\" + "teacher" + yearid + ".txt");
                    }
                    obj = JObject.Parse(repsone).ToObject<ClassificationListTeacher>().data;
                    foreach (var item in obj)
                    {
                        item.number = j.ToString();
                        j++;
                    }
                    List<ClassificationTeacer> copyResult = new List<ClassificationTeacer>();
                    int startIndex = 0, endIndex = 24;
                    Document document = new Document(PageSize.A4, 20, 0, 0, 0);
                    PdfWriter.GetInstance(document, new FileStream(filePath + "\\" + "ResultTemplate" + ".pdf", FileMode.Create));
                    document.Open();
                    GC.Collect();

                    if (obj.Count <= 24)
                    {
                        if (obj.Count <= 20)
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
                                    year.Visibility = Visibility.Collapsed;
                                    lblMonth.Visibility = Visibility.Collapsed;
                                    Data.Visibility = Visibility.Collapsed;
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
                            if (endIndex == obj.ToList().Count())
                            {
                                if (!footerAvaliable)
                                {
                                    Header.Visibility = Visibility.Collapsed;
                                    year.Visibility = Visibility.Collapsed;
                                    lblMonth.Visibility = Visibility.Collapsed;
                                    Body.Visibility = Visibility.Collapsed;
                                    Footer.Visibility = Visibility.Visible;
                                    PrintList(document);
                                }

                                break;
                            }

                            startIndex = endIndex;

                            if (obj.ToList().Count() - endIndex > 28)
                            {
                                endIndex = startIndex + 29;
                                Header.Visibility = Visibility.Collapsed;
                                year.Visibility = Visibility.Collapsed;
                                lblMonth.Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                endIndex = obj.ToList().Count();
                                if (obj.ToList().Count() - startIndex <= 30)
                                {
                                    Header.Visibility = Visibility.Collapsed;
                                    year.Visibility = Visibility.Collapsed;
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
                catch
                {
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
            
            
        }
        private async Task<string> GetDataAsync(string yearid="")
        {
            try
            {
                string accessUrl = Properties.Settings.Default.acessUrl;
                string token = Properties.Settings.Default.Token;
                string respone = "";

                respone = await RESTApiHelper.GetAll(accessUrl, "/list-all-instructor/" + yearid, token);

                using (StreamWriter writer = new StreamWriter(filePath + "\\" + "teacher" + yearid + ".txt"))
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
