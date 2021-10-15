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
    /// Interaction logic for ListStudentCencel.xaml
    /// </summary>
    public partial class ListStudentCencel : Window
    {
        string yearId = "90457379570337304433", titleYear="";
        public ListStudentCencel()
        {
            InitializeComponent();
            lbllogoLeft.Content = Properties.Settings.Default.logoNameLeft;
            TitleSchool.Content = Properties.Settings.Default.schoolName;
        }
        public ListStudentCencel(string yearId,string titleYear)
        {
            InitializeComponent();
            this.titleYear = titleYear;
            this.yearId = yearId;
            lbllogoLeft.Content = Properties.Settings.Default.logoNameLeft;
            TitleSchool.Content = Properties.Settings.Default.schoolName;
        }
        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Templates);
        private int number = 1, gril = 0;

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            barRight.Visibility = Visibility.Collapsed;

            this.Hide();

            PrintPopup prints = new PrintPopup();
            this.IsEnabled = false;
            this.Opacity = 0.5;

            prints.ShowDialog();

            this.Opacity = 1;
            this.IsEnabled = true;
            txtPosition.Content = prints.position;

            barCenter.Visibility = prints.CheckCenter;
            barRight.Visibility = prints.CheckRight;

            if(prints.isPrint==false)
            {
                this.Close();
            }
            else
            {
                Loading load = new Loading();
                load.Show();
                try
                {
                    TitleSchool.Content = Properties.Settings.Default.schoolName;

                    lblMonth.Text = "បញ្ជីរាយនាមសិស្សបោះបង់ការសិក្សា " + titleYear;


                    this.Hide();
                    var respone = "";
                    if (Teacher.InternetChecker())
                    {
                        respone = await GetDataAsync(yearId);
                    }
                    else
                    {
                        respone = File.ReadAllText(filePath + "\\" + "GiveUpStudent" + yearId + ".txt");
                    }
                    var obj = JObject.Parse(respone).ToObject<AllStudentList>().data;



                    foreach (var item in obj)
                    {
                        item.number = number.ToString();
                        if (item.gender == "ស្រី")
                            gril++;
                        number++;
                    }

                    int startIndex = 0, endIndex = 25;
                    Document document = new Document(PageSize.A4, 0, 0, 0, 0);
                    PdfWriter.GetInstance(document, new FileStream(filePath + "\\" + "ResultTemplate" + ".pdf", FileMode.Create));
                    document.Open();
                    GC.Collect();

                    List<StuedntofTheYear> copyResult = new List<StuedntofTheYear>();
                    if (obj.Count <= 25)
                    {
                        if (obj.Count <= 18)
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

                            if (obj.Count() - endIndex > 30)
                            {
                                endIndex = startIndex + 32;
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
                                if (obj.ToList().Count() - startIndex <= 25)
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
                    message.title = Properties.Langs.Lang.print;
                    message.discription = Properties.Langs.Lang.Unsuccessful_printing;
                    message.ShowDialog();
                }
                load.Close();
            }

           
        }
        private async Task<string> GetDataAsync(string yearId)
        {
            try
            {
                string accessUrl = Properties.Settings.Default.acessUrl;
                string token = Properties.Settings.Default.Token;
                string respone = "";

                respone = await RESTApiHelper.GetAll(accessUrl, "/academic/"+yearId+"/print-list-student-status?give_up=1", token);

                using (StreamWriter writer = new StreamWriter(filePath + "\\" + "GiveUpStudent" + yearId + ".txt"))
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
        private void showData(List<StuedntofTheYear> obj)
        {
            Data.ItemsSource = null;
            txtTotalStudent.Text = "បញ្ឈប់បញ្ជីត្រឹមចំនួន " + DateChange.Num(obj.Count) + "នាក់ ស្រី " + DateChange.Num(gril) + "នាក់";
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
                png.ScalePercent(49f);
                document.Add(png);
            }

        }
    }
}
