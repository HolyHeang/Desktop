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
    /// Interaction logic for StudentList.xaml
    /// </summary>
    public partial class StudentList : Window
    {
        public string schoolYearId { get; set; }
        public StudentList()
        {
            InitializeComponent();
        }

        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Templates);
        string encryptionString = "";

        private void print()
        {
            try
            {
                var reponse = GetString(schoolYearId);
                var data = JObject.Parse(reponse).ToObject<AllStudentList>().data;
                List<StuedntofTheYear> copyResult = new List<StuedntofTheYear>();
                NumberList(data);
                this.Hide();
                bool footerAvaliable = false;
                int total = 0, pageNumber = 0,itemCount = 469;
                double time = data.Count / 473;
                int startIndex = 0, endIndex = 39;
                for (int j = 0;j<=time;j++)
                {
                    Header.Visibility = Visibility.Visible;
                    title.Visibility = Visibility.Visible;
                    DataStudentList.Visibility = Visibility.Visible;
                    Document document = new Document(PageSize.A4, 5, 0, 0, 0);
                    PdfWriter.GetInstance(document, new System.IO.FileStream(filePath + "\\" + "សិស្ស"+ j + ".pdf", FileMode.Create));
                    document.Open();
                    GC.Collect();
                    var obj = data.GetRange(total, itemCount);
                    if(j>0)
                    {
                        startIndex = 0;
                        endIndex = 43; 
                    }
                    while (true)
                    {
                        pageNumber++;
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
                        if (j > 0)
                        {
                            Header.Visibility = Visibility.Collapsed;
                            title.Visibility = Visibility.Collapsed;
                        }
                            
                        Grid.Dispatcher.Invoke(() =>
                        {
                            showData(copyResult,pageNumber);
                            Grid.UpdateLayout();
                            GC.Collect();
                        });
                        PrintList(document);
                        if (endIndex == obj.ToList().Count())
                        {
                            if (!footerAvaliable&&j==time)
                            {
                                Header.Visibility = Visibility.Collapsed;
                                title.Visibility = Visibility.Collapsed;
                                DataStudentList.Visibility = Visibility.Collapsed;
                                Footer.Visibility = Visibility.Visible;
                                PrintList(document);
                            }

                            break;
                        }

                        startIndex = endIndex;

                        if (obj.ToList().Count() - endIndex > 43)
                        {
                            endIndex = startIndex + 43;
                            Header.Visibility = Visibility.Collapsed;
                            title.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            endIndex = obj.ToList().Count();
                            if (obj.ToList().Count() - startIndex <= 42)
                            {
                                Header.Visibility = Visibility.Collapsed;
                                title.Visibility = Visibility.Collapsed;
                                Footer.Visibility = Visibility.Visible;
                                footerAvaliable = true;
                            }

                        }

                    }
                    if (j >= 1)
                    {
                        total += 473;
                    }
                    else
                    {
                        total += 469;
                    }
                    if (data.Count <=(total+473))
                    {
                        itemCount = data.Count - total;
                    }
                    else
                    {
                        itemCount = 473;
                    }
                    document.Close();
                    Process.Start(filePath + "\\" + "សិស្ស" + j + ".pdf");
                }
                MessageBoxControl message = new MessageBoxControl();
                message.title = "ដំណឹង";
                message.discription = "បោះពុម្ភបានជោគជ័យ";
                message.buttonType = 1;
                message.ShowDialog();
                this.Close();
            }
            catch (Exception e)
            {
                MessageBoxControl message = new MessageBoxControl();
                message.title = "មានបញ្ហាទិន្នន័យ";
                message.discription = "មិនមានទិន្នន័យ";
                message.buttonType = 1;
                message.ShowDialog();
            }
        }
        void PrintList(Document document)
        {
            string targetFile = System.IO.Path.GetTempFileName();
            using (FileStream outStream = new FileStream(targetFile, FileMode.Create))
            {

                PngBitmapEncoder enc = new PngBitmapEncoder();
                var bitmap = new RenderTargetBitmap((int)Grid.ActualWidth * 4, (int)Grid.ActualHeight * 4, 130, 110, PixelFormats.Pbgra32);
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
            GC.Collect();
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {           
           if(!checkFile(schoolYearId)&&!Teacher.InternetChecker())
            {
                string accessUrl = Properties.Settings.Default.acessUrl;
                string token = Properties.Settings.Default.Token;
                var respone = await RESTApiHelper.GetAll(accessUrl, "/student-schoolyear-search/"+schoolYearId, token);
                var obj = JObject.Parse(respone).ToObject<AllStudentList>().data;
                encryptionString = Teacher.EncodeTo64(respone);
                SaveString(schoolYearId);
            }
            print();
        }
        private void SaveString(string schoolId)
        {
            
            using (StreamWriter writer = new StreamWriter(filePath + "\\" + schoolId + ".txt"))
            {
                writer.WriteLine(encryptionString);
            }
        }
        private string GetString(string schoolId)
        {
            string readText = "";           
            readText = File.ReadAllText(filePath + "\\" + schoolId + ".txt");
            return Teacher.DecodeFrom64(readText);
        }
        private bool checkFile(string schoolId)
        {
            string readText = "";
            try
            {
                readText = File.ReadAllText(filePath + "\\" + schoolId + ".txt");
                readText = Teacher.DecodeFrom64(readText);
                var data = JObject.Parse(readText).ToObject<AllStudentList>().data;
                if (data.Count==0)
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }
            
        }
        private void DataStudentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void showData(List<StuedntofTheYear> obj,int pageNumber)
        {
            DataStudentList.ItemsSource = null;
            DataStudentList.ItemsSource = obj;
            txtPageNumber.Text = pageNumber.ToString();
            GC.Collect();
            Grid.UpdateLayout();
        }
        int i = 1;
        private void NumberList(List<StuedntofTheYear> obj)
        {
            
            foreach (var item in obj)
            {
                item.number = i.ToString();
                i++;
            }
        }
    }
}
