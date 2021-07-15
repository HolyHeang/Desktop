using iTextSharp.text;
using iTextSharp.text.pdf;
using Library;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CamemisOffLine.Windows
{
    /// <summary>
    /// Interaction logic for MonthlyResult.xaml
    /// </summary>
    public partial class MonthlyResult : Window
    {
        public MonthlyResult(List<StudentMonthlyResult> obj,string title)
        {
            InitializeComponent();
            this.obj = obj;
            this.title = title;
        }
        string title = "";
        List<StudentMonthlyResult> obj = new List<StudentMonthlyResult>();
        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Templates);
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Loading loading = new Loading();
            loading.Show();
            this.Hide();

            List<StudentMonthlyResult> copyResult = new List<StudentMonthlyResult>();
            int startIndex = 0, endIndex = 66;
            Document document = new Document(PageSize.A4, 0, 0, 0, 0);
            PdfWriter.GetInstance(document, new FileStream(filePath + "\\" + "ResultTemplate" + ".pdf", FileMode.Create));
            document.Open();
            GC.Collect();

            if(obj.Count<=66)
            {
                if(obj.Count<=62)
                {
                    Grid.Dispatcher.Invoke(() =>
                    {
                        showData(obj);
                        Grid.UpdateLayout();
                    });
                    PrintList(document);
                }
                else
                {
                    for(int i = 0;i<2;i++)
                    {
                        if(i==0)
                        {
                            Footer.Visibility = Visibility.Collapsed;
                            Grid.Dispatcher.Invoke(() =>
                            {
                                showData(obj);
                                Grid.UpdateLayout();
                            });
                            PrintList(document);
                        }
                        else if(i==1)
                        {
                            Footer.Visibility = Visibility.Visible;
                            Header.Visibility = Visibility.Collapsed;
                            DatagridResult.Visibility = Visibility.Collapsed;
                            DatagridResult1.Visibility = Visibility.Collapsed;
                            Grid.Dispatcher.Invoke(() =>
                            {
                                showData(obj);
                                Grid.UpdateLayout();
                            });
                            PrintList(document);
                        }
                    }
                }
            }
            else
            {
                bool footerAvaliable = false;
                while(true)
                {
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

                    Grid.Dispatcher.Invoke(() =>
                    {
                        showData(copyResult);
                        Grid.UpdateLayout();
                    });
                    PrintList(document);
                    if (endIndex == obj.ToList().Count())
                    {
                        if (!footerAvaliable)
                        {
                            Header.Visibility = Visibility.Collapsed;
                            DatagridResult.Visibility = Visibility.Collapsed;
                            DatagridResult1.Visibility = Visibility.Collapsed;
                            Footer.Visibility = Visibility.Visible;
                            PrintList(document);
                        }

                        break;
                    }

                    startIndex = endIndex;

                    if (obj.ToList().Count() - endIndex > 70)
                    {
                        endIndex = startIndex + 70;
                        Header.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        endIndex = obj.ToList().Count();
                        if (obj.ToList().Count() - startIndex <= 66)
                        {
                            Header.Visibility = Visibility.Collapsed;
                            Footer.Visibility = Visibility.Visible;
                            footerAvaliable = true;
                        }

                    }

                }
            }
            
            document.Close();
            Process.Start(filePath + "\\" + "ResultTemplate" + ".pdf");
            this.Close();
            loading.Close();
        }
        int num = 0;
        private void showData(List<StudentMonthlyResult> obj)
        {
            List<StudentResultForCrystalReport> L1 = new List<StudentResultForCrystalReport>();
            List<StudentResultForCrystalReport> L2 = new List<StudentResultForCrystalReport>();
            lblSchoolName.Content = Properties.Settings.Default.schoolName;
            int startIndex = 0;
            string avg = "", colors = "";

            if(title== "month")
            {
                //--------------------L1-----------------------------------
                for (int i = startIndex; i < (obj.Count() / 2); i++)
                {
                    num++;
                    lblTeacherName.Content = obj[i].instructor.name;
                    lblMonth.Text = "លទ្ធផលប្រចាំ " + DateChange.checkMonth(int.Parse(obj[i].result_monthly.month));
                    if (obj[i].result_monthly.absence_exam.Equals(1))
                    {
                        avg = "មិនចាត់ថ្នាក់";
                        colors = "Red";
                    }
                    else
                    {
                        var changeAvg = obj.FirstOrDefault(s => s.result_monthly.avg_score == obj[i].result_monthly.avg_score);
                        changeAvg.result_monthly.avg_score = double.Parse(obj[i].result_monthly.avg_score).ToString("#.##");
                        avg = changeAvg.result_monthly.avg_score;
                        colors = "Blue";
                    }
                    L1.Add(new StudentResultForCrystalReport
                    {
                        number = DateChange.Num(num),
                        avg = avg,
                        rank = obj[i].result_monthly.rank,
                        color = colors,
                        totalScore = obj[i].result_monthly.total_score.ToString(),
                        studentId = obj[i].student_schoolyear_id,
                        studentName = obj[i].name

                    });
                }
                //-------------------- end L1---------------------

                //--------------------L2-----------------------------------
                for (int i = (obj.Count() / 2); i < obj.Count(); i++)
                {
                    num++;
                    if (obj[i].result_monthly.absence_exam.Equals(1))
                    {
                        avg = "មិនចាត់ថ្នាក់";
                        colors = "Red";
                    }
                    else
                    {
                        var changeAvg = obj.FirstOrDefault(s => s.result_monthly.avg_score == obj[i].result_monthly.avg_score);
                        changeAvg.result_monthly.avg_score = double.Parse(obj[i].result_monthly.avg_score).ToString("#.##");
                        avg = changeAvg.result_monthly.avg_score;
                        colors = "Blue";
                    }
                    L2.Add(new StudentResultForCrystalReport
                    {
                        number = DateChange.Num(num),
                        avg = avg,
                        rank = obj[i].result_monthly.rank,
                        color = colors,
                        totalScore = obj[i].result_monthly.total_score.ToString(),
                        studentId = obj[i].student_schoolyear_id,
                        studentName = obj[i].name
                    });
                }
                //--------------------end L2-----------------------------------
            }
            else if(title== "semester")
            {
                obj.OrderBy(s => s.result_semester_exam.total_score);
                //--------------------L1-----------------------------------
                for (int i = startIndex; i < (obj.Count() / 2); i++)
                {
                    num++;
                    if(num==2)
                    {
                        lblTeacherName.Content = obj[i].instructor.name;

                        if (obj[i].result_semester.term == "FIRST_SEMESTER")
                        {
                            lblMonth.Text = "លទ្ធផលប្រចាំ ឆមាសទី១";
                        }
                        else
                        {
                            lblMonth.Text = "លទ្ធផលប្រចាំ ឆមាសទី២";
                        }
                    }
                    L1.Add(new StudentResultForCrystalReport
                    {
                        number = DateChange.Num(num),
                        avg = obj[i].result_semester.avg_score,
                        rank = obj[i].result_semester.rank,
                        color = colors,
                        totalScore = obj[i].result_semester_exam.total_score.ToString(),
                        studentId = obj[i].student_schoolyear_id,
                        studentName = obj[i].name

                    });
                }
                //-------------------- end L1---------------------

                //--------------------L2-----------------------------------
                for (int i = (obj.Count() / 2); i < obj.Count(); i++)
                {
                    num++;
                    L2.Add(new StudentResultForCrystalReport
                    {
                        number = DateChange.Num(num),
                        avg = obj[i].result_semester.avg_score,
                        rank = obj[i].result_semester.rank,
                        color = colors,
                        totalScore = obj[i].result_semester_exam.total_score.ToString(),
                        studentId = obj[i].student_schoolyear_id,
                        studentName = obj[i].name
                    });
                }
                //--------------------end L2-----------------------------------
            }
            DatagridResult.ItemsSource = L1.OrderBy(r=>r.rank);
            DatagridResult1.ItemsSource = L2.OrderBy(r => r.rank);
        }
        void PrintList(Document document)
        {
            string targetFile = System.IO.Path.GetTempFileName();
            using (FileStream outStream = new FileStream(targetFile, FileMode.Create))
            {

                PngBitmapEncoder enc = new PngBitmapEncoder();
                var bitmap = new RenderTargetBitmap((int)Grid.ActualWidth * 2, (int)Grid.ActualHeight * 2, 192, 200, PixelFormats.Pbgra32);
                bitmap.Render(Grid);
                enc.Frames.Add(BitmapFrame.Create(bitmap));
                enc.Save(outStream);
                bitmap = null;

                outStream.Dispose();
            }
            using (FileStream fs = new FileStream(targetFile, FileMode.Open))
            {
                iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromStream(fs), System.Drawing.Imaging.ImageFormat.Png);
                png.ScalePercent(28.4f);
                document.Add(png);
            }
           
        }
    }
}
