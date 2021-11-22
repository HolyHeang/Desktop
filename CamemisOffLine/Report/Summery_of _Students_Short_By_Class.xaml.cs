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
    /// Interaction logic for Sumery_of__Students_Short.xaml
    /// </summary>
    public partial class Sumery_of_Students_Short_By_Class : Window
    {

        public Sumery_of_Students_Short_By_Class()
        {
            InitializeComponent();
            lbllogoLeft.Content = Properties.Settings.Default.logoNameLeft;
            TitleSchool.Content = Properties.Settings.Default.schoolName;
        }

        string yearId = "508", type = "1", month = "2", semester = "FIRST_SEMESTER", monthName = "";
        public Sumery_of_Students_Short_By_Class(string yearId, string type, string month, string semester, string monthName)
        {
            InitializeComponent();
            lbllogoLeft.Content = Properties.Settings.Default.logoNameLeft;
            TitleSchool.Content = Properties.Settings.Default.schoolName;
            this.yearId = yearId;
            this.type = type;
            this.month = month;
            this.semester = semester;
            this.monthName = monthName;
        }

        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Templates);

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

            if (prints.isPrint == false)
                this.Close();
            else
            {
                Loading load = new Loading();
                load.Show();
                string reponse = "";
                if (Teacher.InternetChecker())
                {
                    reponse = await GetDataAsync();
                }
                else
                {
                    MessageBoxControl message = new MessageBoxControl();
                    message.title = Properties.Langs.Lang.Internet;
                    message.discription = Properties.Langs.Lang.No_internet_connection;
                    message.buttonType = 1;
                    message.ShowDialog();
                    load.Close();
                    this.Close();
                }
                var obj = JObject.Parse(reponse).ToObject<SummarySubjectGradingScal>();

                if (obj.data.subjects.All(l => l.grading_system.Count > 4))
                {
                    Summary_of_Students_By_Class students = new Summary_of_Students_By_Class(yearId, type, month, semester, monthName);
                    students.Show();
                    load.Close();
                    this.Close();
                }
                else
                {
                    Console.WriteLine("----------------------4---------------------");
                }

                try
                {
                    var sum = new List<SummarySubjectGradingScal>();
                    sum.Add(obj);
                    Document document = new Document(PageSize.A4.Rotate(), 5, 0, 0, 0);
                    PdfWriter.GetInstance(document, new FileStream(filePath + "\\" + "ResultTemplate" + ".pdf", FileMode.Create));
                    document.Open();
                    showData(sum);
                    Grid.UpdateLayout();
                    GC.Collect();
                    string targetFile = System.IO.Path.GetTempFileName();
                    using (FileStream outStream = new FileStream(targetFile, FileMode.Create))
                    {

                        PngBitmapEncoder enc = new PngBitmapEncoder();
                        var bitmap = new RenderTargetBitmap((int)Grid.ActualWidth * 2, (int)Grid.ActualHeight * 2, 145, 100, PixelFormats.Pbgra32);
                        bitmap.Render(Grid);
                        enc.Frames.Add(BitmapFrame.Create(bitmap));
                        enc.Save(outStream);
                        bitmap = null;
                        outStream.Dispose();
                    }
                    using (FileStream fs = new FileStream(targetFile, FileMode.Open))
                    {
                        iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromStream(fs), System.Drawing.Imaging.ImageFormat.Png);
                        png.ScalePercent(42f);
                        document.Add(png);
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
        }
        private async Task<string> GetDataAsync()
        {
            try
            {
                string accessUrl = Properties.Settings.Default.acessUrl;
                string token = Properties.Settings.Default.Token;

                var respone = await RESTApiHelper.GetAll(accessUrl, "/get-class-summary-subject-gradingscale/" + yearId + "?type=" + type + "&month=" + month + "&semester=" + semester, token);

                return respone;
            }
            catch
            {
                return null;
            }
        }
        public void showData(List<SummarySubjectGradingScal> obj)
        {
            lblMonth.Text = "តារាងសង្ខេបនិន្ទេសសិស្សតាមមុខវិជ្ជាគោល​​ " + monthName;
            foreach(var item in obj)
            {
                foreach (var i in item.data.subjects)
                {
                    foreach (var j in i.grading_system)
                    {
                        if (j.letter_grade == "A")
                        {
                            i.girlA = j.student.female;
                            i.totalA = j.student.total;
                        }
                        else if (j.letter_grade == "B")
                        {
                            i.girlB = j.student.female;
                            i.totalB = j.student.total;
                        }
                        else if (j.letter_grade == "C")
                        {
                            i.girlC = j.student.female;
                            i.totalC = j.student.total;
                        }
                        else if (j.letter_grade == "D")
                        {
                            i.girlD = j.student.female;
                            i.totalD = j.student.total;
                        }
                    }
                }
            }
            DG.ItemsSource = null;
            DG.ItemsSource = obj;
        }
        void PrintList(Document document)
        {
            string targetFile = System.IO.Path.GetTempFileName();
            using (FileStream outStream = new FileStream(targetFile, FileMode.Create))
            {

                PngBitmapEncoder enc = new PngBitmapEncoder();
                var bitmap = new RenderTargetBitmap((int)Grid.ActualWidth * 2, (int)Grid.ActualHeight * 2, 145, 100, PixelFormats.Pbgra32);
                bitmap.Render(Grid);
                enc.Frames.Add(BitmapFrame.Create(bitmap));
                enc.Save(outStream);
                bitmap = null;
                outStream.Dispose();
            }
            using (FileStream fs = new FileStream(targetFile, FileMode.Open))
            {
                iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromStream(fs), System.Drawing.Imaging.ImageFormat.Png);
                png.ScalePercent(42f);
                document.Add(png);
            }

        }
    }
}
