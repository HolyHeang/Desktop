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
    /// Interaction logic for student_card.xaml
    /// </summary>
    public partial class student_card : Window
    {
        public student_card()
        {
            InitializeComponent();
        }
        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Templates);
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> s = new List<string>();
             for(int i = 0;i<=7;i++)
            {
                s.Add(i.ToString());
            }
            StudentCard.ItemsSource = s;
            print();
        }

        private void print()
        {

            Loading load = new Loading();
            load.Show();

            try
            {

                this.Hide();
                Document document = new Document(PageSize.A4.Rotate(), 5, 0, 5, 0);
                PdfWriter.GetInstance(document, new System.IO.FileStream(filePath + "\\" + "កាលវិភាគសិស្ស" + ".pdf", FileMode.Create));

                document.Open();

                GC.Collect();

                string targetFile = System.IO.Path.GetTempFileName();
                using (FileStream outStream = new FileStream(targetFile, FileMode.Create))
                {

                    PngBitmapEncoder enc = new PngBitmapEncoder();
                    var bitmap = new RenderTargetBitmap((int)StudentCard.ActualWidth * 2, (int)StudentCard.ActualHeight * 2, 154, 154, PixelFormats.Pbgra32);
                    bitmap.Render(StudentCard);
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
                Process.Start(filePath + "\\" + "កាលវិភាគសិស្ស" + ".pdf");
                this.Close();
                load.Close();
            }
            catch
            {
                MessageBox.Show("ការបោះពុម្ភរបស់អ្នកមិនទទូលបានជោគជ័យ", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }
    }
}
