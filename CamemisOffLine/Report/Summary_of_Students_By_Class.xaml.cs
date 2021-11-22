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
    /// Interaction logic for Summary_of_Students.xaml
    /// </summary>
    public partial class Summary_of_Students_By_Class : Window
    {
        string yearId = "", type = "", month = "", semester = "",monthName="";
        public Summary_of_Students_By_Class(string yearId,string type,string month,string semester,string monthName)
        {
            InitializeComponent();
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

            Loading load = new Loading();
            load.Show();
            string reponse = "";
            if(Teacher.InternetChecker())
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
                    var bitmap = new RenderTargetBitmap((int)Grid.ActualWidth * 2, (int)Grid.ActualHeight * 2, 145, 110, PixelFormats.Pbgra32);
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
                message.title = Properties.Langs.Lang.print;
                message.discription = Properties.Langs.Lang.Unsuccessful_printing;
                message.ShowDialog();
            }
            load.Close();


        }
        private async Task<string> GetDataAsync()
        {
            try
            {
                string accessUrl = Properties.Settings.Default.acessUrl;
                string token = Properties.Settings.Default.Token;

                var respone = await RESTApiHelper.GetAll(accessUrl, "/get-class-summary-subject-gradingscale/"+yearId+"?type="+type+"&month="+month+"&semester="+semester, token);
                var obj = JObject.Parse(respone).ToObject<StaffAttendanceDailyList>().data;
               
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
            foreach (var item in obj)
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
                        else if (j.letter_grade == "E")
                        {
                            i.girlE = j.student.female;
                            i.totalE = j.student.total;
                        }
                        else if (j.letter_grade == "F")
                        {
                            i.girlF = j.student.female;
                            i.totalF = j.student.total;
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
                var bitmap = new RenderTargetBitmap((int)Grid.ActualWidth * 2, (int)Grid.ActualHeight * 2, 140, 120, PixelFormats.Pbgra32);
                bitmap.Render(Grid);
                enc.Frames.Add(BitmapFrame.Create(bitmap));
                enc.Save(outStream);
                bitmap = null;
                outStream.Dispose();
            }
            using (FileStream fs = new FileStream(targetFile, FileMode.Open))
            {
                iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromStream(fs), System.Drawing.Imaging.ImageFormat.Png);
                png.ScalePercent(36f);
                document.Add(png);
            }

        }
    }
}
