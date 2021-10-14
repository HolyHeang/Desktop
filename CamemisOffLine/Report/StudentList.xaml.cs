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
using System.Text.RegularExpressions;
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
    /// Interaction logic for StudentList.xaml
    /// </summary>
    public partial class StudentList : Window
    {
        public string schoolYearId { get; set; }
        public string classId { get; set; }
        public string gradeId { get; set; }
        public string level { get; set; }
        string titleYear = "";
        public StudentList(int ping,int type,string titleYear)
        {
            InitializeComponent();
            pings = ping;
            this.type = type;
            this.titleYear = titleYear;
        }

        public string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Templates);
        string encryptionString = "";
        int pings,type;

        private void print(string stringRespone,List<StuedntofTheYear> datas)
        {
            TitleSchool.Content = Properties.Settings.Default.schoolName;
            lbllogoLeft.Content = Properties.Settings.Default.logoNameLeft;
            List<string> fileArray = new List<string>();
            try
            {
                var reponse = "";
                List<StuedntofTheYear> data = new List<StuedntofTheYear>();
                if (stringRespone==null)
                {
                   reponse = GetString(schoolYearId);
                }
                else
                {
                    reponse = stringRespone;
                }
                if(datas==null)
                {
                   data  = JObject.Parse(reponse).ToObject<AllStudentList>().data;
                }
                else
                {
                    data = datas; ;
                }
                
                List<StuedntofTheYear> copyResult = new List<StuedntofTheYear>();
                NumberList(data);
                bool footerAvaliable = false;
                int total = 0, pageNumber = 0, itemCount = 509;
                double time = data.Count / 473;
                int startIndex = 0, endIndex = 36;
                for (int j = 0; j <= time; j++)
                {
                    Header.Visibility = Visibility.Visible;
                    title.Visibility = Visibility.Visible;
                    DataStudentList.Visibility = Visibility.Visible;
                    Document document = new Document(PageSize.A4, 5, 0, 0, 0);
                    PdfWriter.GetInstance(document, new System.IO.FileStream(filePath + "\\" + "សិស្ស" + j + ".pdf", FileMode.Create));
                    document.Open();
                    GC.Collect();
                    var obj = new List<StuedntofTheYear>();
                    if (data.Count >= 509)
                    {
                        obj = data.GetRange(total, itemCount);
                    }
                    else
                    {
                        obj = data;
                    }
                    if (j > 0)
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
                            showData(copyResult, pageNumber);
                            Grid.UpdateLayout();
                            GC.Collect();
                        });
                        PrintList(document);
                        if (endIndex == obj.ToList().Count())
                        {
                            if (!footerAvaliable && j == time)
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
                        total += 509;
                    }
                    if (data.Count <= (total + 473))
                    {
                        itemCount = data.Count - total;
                    }
                    else
                    {
                        itemCount = 473;
                    }
                    document.Close();
                    string path = filePath + "\\" + "សិស្ស" + j + ".pdf";
                    fileArray.Add(path);
                }
                //MessageBoxControl message = new MessageBoxControl();
                //message.Owner = this;
                //message.title = Properties.Langs.Lang.print;
                //message.discription = Properties.Langs.Lang.print_successful;
                //message.buttonType = 2;
                //this.Opacity = 0.5;
                //message.ShowDialog();
                //this.Opacity = 1;
                MergePDF(fileArray.ToArray());
                this.Close();
            }
            catch (Exception e)
            {
                MessageBoxControl message = new MessageBoxControl();
                message.title = Properties.Langs.Lang.Data;
                message.discription = Properties.Langs.Lang.noresultdata;
                message.buttonType = 2;
                this.Opacity = 0.5;
                message.ShowDialog();
                this.Opacity = 1;
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
            this.Opacity = 0.5;
            barRight.Visibility = Visibility.Collapsed;
            this.Hide();

            PrintPopup prints = new PrintPopup();
            prints.ShowDialog();
            txtPosition.Content = prints.position;

            barCenter.Visibility = prints.CheckCenter;
            barRight.Visibility = prints.CheckRight;
            this.Opacity = 1;

            Loading loading = new Loading();
            loading.Show();
            var respone = "";
            var obj = new List<StuedntofTheYear>();
            if (pings<=150 && Teacher.InternetChecker())
            {
                string accessUrl = Properties.Settings.Default.acessUrl;
                string token = Properties.Settings.Default.Token;
               
                if(type==1&&schoolYearId!="")
                {
                    lblMonth.Text = "បញ្ជីរាយនាមសិស្សគ្រប់កម្រិតថ្នាក់ ឆ្នាំសិក្សា " + titleYear;
                    respone = await RESTApiHelper.GetAll(accessUrl, "/student-schoolyear-search/" + schoolYearId, token);
                    encryptionString = Teacher.EncodeTo64(respone);
                    SaveString(schoolYearId);
                }
                else if(type==2&&classId!="")
                {
                    lblMonth.Text = "បញ្ជីរាយនាមសិស្សថ្នាក់ទី " + titleYear;
                    respone = await RESTApiHelper.GetAll(accessUrl, "/student-schoolyear-search/" + schoolYearId+ "?classId="+classId, token);
                }
                else if(type==3&&gradeId!="")
                {
                    lblMonth.Text = "បញ្ជីរាយនាមសិស្ស " + titleYear;
                    respone = await RESTApiHelper.GetAll(accessUrl, "/student-schoolyear-search/" + schoolYearId + "?gradeId=" + gradeId, token);
                }
                else if (type == 4&&level!="")
                {
                    lblMonth.Text = "បញ្ជីរាយនាមសិស្សថ្នាក់ទី " + titleYear;
                    respone = await RESTApiHelper.GetAll(accessUrl, "/student-schoolyear-search/" + schoolYearId + "?level=" + level, token);
                }
                else
                {
                    loading.Close();
                }
                obj = null;
            }
            else
            {
                respone = GetString(schoolYearId);
                var data = JObject.Parse(respone).ToObject<AllStudentList>().data;

                if(type==1)
                {
                    lblMonth.Text = "បញ្ជីរាយនាមសិស្សគ្រប់កម្រិតថ្នាក់ ឆ្នាំសិក្សា " + titleYear;
                    obj = data;
                }
                else if(type==2)
                {
                    lblMonth.Text = "បញ្ជីរាយនាមសិស្សថ្នាក់ទី " + titleYear;
                    obj = data.Where(s => s.current_class_id == classId).ToList();
                }
                else if(type==3)
                {
                    lblMonth.Text = "បញ្ជីរាយនាមសិស្ស " + titleYear;
                    obj = data.Where(s => s.grade_id == gradeId).ToList();
                }
                else if(type==4)
                {
                    lblMonth.Text = "បញ្ជីរាយនាមសិស្សថ្នាក់ទី " + titleYear;
                    obj = data.Where(s => s.level == level).ToList();
                }
                respone = null;
            }
            MessageBoxControl message = new MessageBoxControl();
            message.title = Properties.Langs.Lang.print;
            message.discription = Properties.Langs.Lang.print_successful;
            message.buttonType = 1;
            print(respone,obj);
            loading.Close();
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
                if (data.Count == 0)
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

        private void showData(List<StuedntofTheYear> obj, int pageNumber)
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

        private static void MergePDF(string[] fileArray)
        {
            PdfReader reader = null;
            Document sourceDocument = null;
            PdfCopy pdfCopyProvider = null;
            PdfImportedPage importedPage;
            string outputPdfPath = Environment.GetFolderPath(Environment.SpecialFolder.Templates) + "\\" + "new" + ".pdf";

            sourceDocument = new Document();
            pdfCopyProvider = new PdfCopy(sourceDocument, new System.IO.FileStream(outputPdfPath, System.IO.FileMode.Create));

            //output file Open  
            sourceDocument.Open();


            //files list wise Loop  
            for (int f = 0; f < fileArray.Length; f++)
            {
                int pages = TotalPageCount(fileArray[f]);

                reader = new PdfReader(fileArray[f]);
                //Add pages in new file  
                for (int i = 1; i <= pages; i++)
                {
                    importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                    pdfCopyProvider.AddPage(importedPage);
                }

                reader.Close();
            }
            //save the output file  
            sourceDocument.Close();
            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Templates) + "\\" + "new" + ".pdf");
        }

        private static int TotalPageCount(string file)
        {
            using (StreamReader sr = new StreamReader(System.IO.File.OpenRead(file)))
            {
                Regex regex = new Regex(@"/Type\s*/Page[^s]");
                MatchCollection matches = regex.Matches(sr.ReadToEnd());

                return matches.Count;
            }
        }
    }
}
