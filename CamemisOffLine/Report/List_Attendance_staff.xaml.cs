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
        public List_Attendance_staff()
        {
            InitializeComponent();
            lbllogoLeft.Content = Properties.Settings.Default.logoNameLeft;
            TitleSchool.Content = Properties.Settings.Default.schoolName;
        }

        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Templates);

        private void print()
        {

            try
            {

                this.Hide();
                Document document = new Document(PageSize.A4, 5, 0, 0, 0);
                PdfWriter.GetInstance(document, new System.IO.FileStream(filePath + "\\" + "បញ្ជីស្រង់វត្តមានបុគ្កលិកសិក្សា" + ".pdf", FileMode.Create));

                document.Open();

                GC.Collect();

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

                GC.Collect();
                document.Close();
                GC.Collect();
                Process.Start(filePath + "\\" + "បញ្ជីស្រង់វត្តមានបុគ្កលិកសិក្សា" + ".pdf");
                this.Close();
            }
            catch
            {
                MessageBox.Show("ការបោះពុម្ភរបស់អ្នកមិនទទូលបានជោគជ័យ", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var accessUrl = Properties.Settings.Default.acessUrl;
            var token = Properties.Settings.Default.Token;
            if(Teacher.InternetChecker())
            {
                int i = 1;
                var respone = await RESTApiHelper.GetAll(accessUrl, "/get-daily-staff-attendance-report?date=03/09/2021", token);
                var obj = JObject.Parse(respone).ToObject<StaffAttendanceDailyList>().data;

                foreach (var item in obj)
                {
                    item.number = i.ToString();
                    if (item.gender == "1")
                        item.gender = "ប្រុស";
                    else
                        item.gender = "ស្រី";
                    i++;
                }
                Data.ItemsSource = obj;
            }
        }
    }
}