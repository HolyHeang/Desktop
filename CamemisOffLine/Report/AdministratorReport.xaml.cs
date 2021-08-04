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
    /// Interaction logic for AdministratorReport.xaml
    /// </summary>
    public partial class AdministratorReport : Window
    {
        public AdministratorReport()
        {
            InitializeComponent();
        }
        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Templates);
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lbllogoLeft.Text = Properties.Settings.Default.logoNameLeft;
            txtSchoolname.Text = Properties.Settings.Default.schoolName;

            this.Hide();
            var Date = DateTime.Now.ToShortDateString();
            string[] split = Date.Split('/');
            txtDate.Text = "រាជធានីភ្នំពេញ ថ្ងៃទី"+DateChange.Num(int.Parse(split[1]))+" ខែ"+ DateChange.Num(int.Parse(split[0]))+" ឆ្នាំ"+ DateChange.Num(int.Parse(split[2]));
            Grid.UpdateLayout();
            Document document = new Document(PageSize.A4, 0, 0, 0, 0);
            PdfWriter.GetInstance(document, new System.IO.FileStream(filePath + "\\" + "HonoraryList" + ".pdf", FileMode.Create));

            document.Open();

            string targetFile = System.IO.Path.GetTempFileName();
            using (FileStream outStream = new FileStream(targetFile, FileMode.Create))
            {

                PngBitmapEncoder enc = new PngBitmapEncoder();
                var bitmap = new RenderTargetBitmap((int)Grid.ActualWidth * 4, (int)Grid.ActualHeight * 4, 120, 120, PixelFormats.Pbgra32);
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
            Process.Start(filePath + "\\" + "HonoraryList" + ".pdf");
            this.Close();
        }
    }
}
