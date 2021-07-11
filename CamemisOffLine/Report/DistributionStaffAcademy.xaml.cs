using CamemisOffLine.Windows;
using iTextSharp.text;
using iTextSharp.text.pdf;
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
    /// Interaction logic for DistributionStaffAcademy.xaml
    /// </summary>
    public partial class DistributionStaffAcademy : Window
    {
        public DistributionStaffAcademy()
        {
            InitializeComponent();
        }

       


        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Templates);
        private void print()
        {

            try
            {

                this.Hide();
                Document document = new Document(PageSize.A4, 5, 0, 0, 0);
                PdfWriter.GetInstance(document, new System.IO.FileStream(filePath + "\\" + "បំណែងចែកភារកិច្ចបុគ្គលិកសិក្សា" + ".pdf", FileMode.Create));

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
                    png.ScalePercent(60f);
                    document.Add(png);
                }

                GC.Collect();
                document.Close();
                GC.Collect();
                Process.Start(filePath + "\\" + "បំណែងចែកភារកិច្ចបុគ្គលិកសិក្សា" + ".pdf");
                this.Close();
            }
            catch
            {
                MessageBox.Show("ការបោះពុម្ភរបស់អ្នកមិនទទូលបានជោគជ័យ", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

       

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            print();
        }
    }
}
