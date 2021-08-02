using iTextSharp.text;
using iTextSharp.text.pdf;
using Library;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Printing;
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

namespace CamemisOffLine.Windows
{
    /// <summary>
    /// Interaction logic for HonoraryList.xaml
    /// </summary>
    public partial class HonoraryList : Window
    {
        public HonoraryList()
        {
            InitializeComponent();
        }
        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Templates);
        public HonoraryList(List<StudentMonthlyResult> topStudent, string schoolName,string teacherName,string title)
        {
            InitializeComponent();
            lblSchoolName.Content = schoolName;
            lblTeacherName.Content = teacherName;
            for(int i = 0;i<topStudent.Count;i++)
            {
                lblClass.Content = topStudent[0].class_name;
                if(title=="month")
                {
                    lblMonth.Content = DateChange.checkMonth(int.Parse(topStudent[0].all_subject_result[0].month));
                    if (i == 0)
                    {
                        lblNum1.Content = "លេខ " + DateChange.Num(topStudent[i].result_monthly.rank);
                        lblNameNum1.Content = topStudent[i].name;
                        imgProfile1.Source = new BitmapImage(new Uri(topStudent[i].localProfileLink));
                    }
                    else if (i == 1)
                    {
                        lblNum2.Content = "លេខ " + DateChange.Num(topStudent[i].result_monthly.rank);
                        lblNameNum2.Content = topStudent[i].name;
                        imgProfile2.Source = new BitmapImage(new Uri(topStudent[i].localProfileLink));
                    }
                    else if (i == 2)
                    {
                        lblNum3.Content = "លេខ " + DateChange.Num(topStudent[i].result_monthly.rank);
                        lblNameNum3.Content = topStudent[i].name;
                        imgProfile3.Source = new BitmapImage(new Uri(topStudent[i].localProfileLink));
                    }
                    else if (i == 3)
                    {
                        lblNum4.Content = "លេខ " + DateChange.Num(topStudent[i].result_monthly.rank);
                        lblNameNum4.Content = topStudent[i].name;
                        imgProfile4.Source = new BitmapImage(new Uri(topStudent[i].localProfileLink));
                    }
                    else if (i == 4)
                    {
                        lblNum5.Content = "លេខ " + DateChange.Num(topStudent[i].result_monthly.rank);
                        lblNameNum5.Content = topStudent[i].name;
                        imgProfile5.Source = new BitmapImage(new Uri(topStudent[i].localProfileLink));
                    }
                }
                else if(title=="semester")
                {
                    if (topStudent[0].result_semester.term == "FIRST_SEMESTER")
                        lblMonth.Content = "ប្រចាំឆមាសលើកទី១";
                    else
                        lblMonth.Content = "ប្រចាំឆមាសលើកទី២";

                    if (i == 0)
                    {
                        lblNum1.Content = "លេខ " + DateChange.Num(topStudent[i].result_semester.rank);
                        lblNameNum1.Content = topStudent[i].name;
                        imgProfile1.Source = new BitmapImage(new Uri(topStudent[i].localProfileLink));
                    }
                    else if (i == 1)
                    {
                        lblNum2.Content = "លេខ " + DateChange.Num(topStudent[i].result_semester.rank);
                        lblNameNum2.Content = topStudent[i].name;
                        imgProfile2.Source = new BitmapImage(new Uri(topStudent[i].localProfileLink));
                    }
                    else if (i == 2)
                    {
                        lblNum3.Content = "លេខ " + DateChange.Num(topStudent[i].result_semester.rank);
                        lblNameNum3.Content = topStudent[i].name;
                        imgProfile3.Source = new BitmapImage(new Uri(topStudent[i].localProfileLink));
                    }
                    else if (i == 3)
                    {
                        lblNum4.Content = "លេខ " + DateChange.Num(topStudent[i].result_semester.rank);
                        lblNameNum4.Content = topStudent[i].name;
                        imgProfile4.Source = new BitmapImage(new Uri(topStudent[i].localProfileLink));
                    }
                    else if (i == 4)
                    {
                        lblNum5.Content = "លេខ " + DateChange.Num(topStudent[i].result_semester.rank);
                        lblNameNum5.Content = topStudent[i].name;
                        imgProfile5.Source = new BitmapImage(new Uri(topStudent[i].localProfileLink));
                    }
                }
            }
        }
        private void print()
        {
            try
            {

                this.Hide();
                Document document = new Document(PageSize.A4, 0, 0, 0, 0);
                PdfWriter.GetInstance(document, new System.IO.FileStream(filePath + "\\" + "HonoraryList" + ".pdf", FileMode.Create));

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
                Process.Start(filePath + "\\" + "HonoraryList" + ".pdf");
                this.Close();
            }
            catch
            {
                MessageBox.Show("ការបោះពុម្ភរបស់អ្នកមិនទទូលបានជោគជ័យ", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Loading loading = new Loading();
            loading.Show();
            print();
            loading.Close();
        }
    }
}
