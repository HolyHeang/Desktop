using CamemisOffLine.Windows;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Library;
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
    /// Interaction logic for MonthlySubjectResult.xaml
    /// </summary>
    public partial class MonthlySubjectResult : Window
    {
        public MonthlySubjectResult(List<StudentInformation> obj, string title,string label,string year)
        {
            InitializeComponent();
            this.obj = obj;
            this.title = title;
            this.label = label;
            this.year = year;
        }
        string title = "",label="",year="";
        List<StudentInformation> obj = new List<StudentInformation>();
        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Templates);
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Loading loading = new Loading();
                loading.Show();
                this.Hide();

                List<StudentInformation> copyResult = new List<StudentInformation>();
                int startIndex = 0, endIndex = 50;
                Document document = new Document(PageSize.A4, 0, 0, 0, 0);
                PdfWriter.GetInstance(document, new FileStream(filePath + "\\" + "ResultTemplate" + ".pdf", FileMode.Create));
                document.Open();
                GC.Collect();

                if (obj.Count <= 50)
                {
                    if (obj.Count <= 45)
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
                                DatagridResult.Visibility = Visibility.Collapsed;
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
                                DatagridResult.Visibility = Visibility.Collapsed;
                                Footer.Visibility = Visibility.Visible;
                                PrintList(document);
                            }

                            break;
                        }

                        startIndex = endIndex;

                        if (obj.ToList().Count() - endIndex > 60)
                        {
                            endIndex = startIndex + 60;
                            Header.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            endIndex = obj.ToList().Count();
                            if (obj.ToList().Count() - startIndex <= 60)
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
                loading.Close();
            }
            catch
            {
                this.Close();
            }
        }
        int num = 0;
        private void showData(List<StudentInformation> obj)
        {
            lblSchoolName.Content = Properties.Settings.Default.schoolName;
            lblLogoNameLeft.Content = Properties.Settings.Default.logoNameLeft;
            if (title == "month")
            {
                
                foreach(var item in obj)
                {
                    lblSubject.Text = "លទ្ធផលលើមុខវិជ្ជា: "+item.subject_name;
                    lblClass.Text = "ថ្នាក់ទី "+item.class_name;
                    if (item.score==null)
                    {
                        item.score = "--";
                    }
                    if(item.rank==null)
                    {
                        item.rank = "--";
                    }
                }
                lblMonth.Text = " លទ្ធផលប្រចាំខែ " + label + " " + year;
            }
            else if (title == "semester")
            {
                foreach (var item in obj)
                {
                    lblSubject.Text = "លទ្ធផលលើមុខវិជ្ជា: " + item.subject_name;
                    lblClass.Text = "ថ្នាក់ទី " + item.class_name;
                    if (item.score == null)
                    {
                        item.score = "--";
                    }
                    if (item.rank == null)
                    {
                        item.rank = "--";
                    }
                }
               
                lblMonth.Text = " លទ្ធផលប្រចាំ "+label+" "+year;
            }
            DatagridResult.ItemsSource = obj;
        }
        void PrintList(Document document)
        {
            string targetFile = System.IO.Path.GetTempFileName();
            using (FileStream outStream = new FileStream(targetFile, FileMode.Create))
            {

                PngBitmapEncoder enc = new PngBitmapEncoder();
                var bitmap = new RenderTargetBitmap((int)Grid.ActualWidth * 2, (int)Grid.ActualHeight * 2, 192, 200, PixelFormats.Pbgra32);
                bitmap.Render(Grid);
                enc.Frames.Add(BitmapFrame.Create(bitmap));
                enc.Save(outStream);
                bitmap = null;

                outStream.Dispose();
            }
            using (FileStream fs = new FileStream(targetFile, FileMode.Open))
            {
                iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromStream(fs), System.Drawing.Imaging.ImageFormat.Png);
                png.ScalePercent(28.4f);
                document.Add(png);
            }

        }
    }
}
