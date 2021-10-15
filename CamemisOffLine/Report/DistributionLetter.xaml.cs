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
    /// Interaction logic for DistributionLetter.xaml
    /// </summary>
    public partial class DistributionLetter : Window
    {
        public DistributionLetter()
        {
            InitializeComponent();
        }


        public DistributionLetter(List<StudentMonthlyResult> topStudent, string schoolName)
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
                PdfWriter.GetInstance(document, new System.IO.FileStream(filePath + "\\" + "លិខិតបង្គាប់ការ" + ".pdf", FileMode.Create));

                document.Open();

                GC.Collect();

                string targetFile = System.IO.Path.GetTempFileName();
                using (FileStream outStream = new FileStream(targetFile, FileMode.Create))
                {

                    PngBitmapEncoder enc = new PngBitmapEncoder();
                    var bitmap = new RenderTargetBitmap((int)Grid.ActualWidth * 2, (int)Grid.ActualHeight * 2, 120, 110, PixelFormats.Pbgra32);
                    bitmap.Render(Grid);
                    enc.Frames.Add(BitmapFrame.Create(bitmap));
                    enc.Save(outStream);
                    bitmap = null;

                    outStream.Dispose();
                }
                using (FileStream fs = new FileStream(targetFile, FileMode.Open))
                {
                    iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromStream(fs), System.Drawing.Imaging.ImageFormat.Png);
                    png.ScalePercent(50f);
                    document.Add(png);
                }

                GC.Collect();
                document.Close();
                GC.Collect();
                Process.Start(filePath + "\\" + "លិខិតបង្គាប់ការ" + ".pdf");
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
            barRight.Visibility = Visibility.Collapsed;
            string SchoolYearId = "11000317333438883781";
            this.Hide();
            List<Instructors> l1 = new List<Instructors>();
            List<Instructors> l2 = new List<Instructors>();

            string accessUrl = Properties.Settings.Default.acessUrl;
            string token = Properties.Settings.Default.Token;
            var respone = await RESTApiHelper.GetAll(accessUrl, "/list-all-instructor/" + SchoolYearId, token);
            var obj = JObject.Parse(respone).ToObject<ListInstructor>().data;
            NumberList(obj);

            foreach (var item in obj)
            {
                if (item.number <= obj.Count() / 2)
                    l1.Add(item);
                else
                    l2.Add(item);
            }
            DatagridDistrition.ItemsSource = l1;
            DatagridDistrition1.ItemsSource = l2;
            Grid.UpdateLayout();
            print();
          
            this.Close();

        }
        int i = 1;
        private void NumberList(List<Instructors> obj)
        {

            foreach (var item in obj)
            {
                item.number = i;
                i++;
            }
        }
    }
}
