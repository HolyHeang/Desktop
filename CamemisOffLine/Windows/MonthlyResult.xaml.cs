﻿using CamemisOffLine.Component;
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
        public MonthlyResult(List<StudentMonthlyResult> obj, string title,string year)
        {
            InitializeComponent();
            this.obj = obj;
            this.title = title;
            this.year = year;
        }
        string title = "",year="";
        List<StudentMonthlyResult> obj = new List<StudentMonthlyResult>();
        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Templates);
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            barRight.Visibility = Visibility.Collapsed;
            this.Hide();
            PrintPopup prints = new PrintPopup();
            if (Properties.Settings.Default.role == "1")
            {
                this.Opacity = 0.5;
                this.IsEnabled = false;
            

                prints.ShowDialog();

                
                this.IsEnabled = true;
                titleTeacher.Content = prints.position;
                titleAdmin.Visibility = Visibility.Collapsed;
                lblTeacherName.Visibility = Visibility.Collapsed;

                barCenter.Visibility = prints.CheckCenter;
                barRight.Visibility = prints.CheckRight;
                this.Opacity = 1;
            }
            else
            {
                this.Opacity = 0.5;
                this.IsEnabled = false;
               

                prints.ShowDialog();

               
                this.IsEnabled = true;
                txtPosition.Text = prints.position;
                titleAdmin.Visibility = Visibility.Visible;
                lblTeacherName.Visibility = Visibility.Visible;

                barCenter.Visibility = prints.CheckCenter;
                barRight.Visibility = prints.CheckRight;
                this.Opacity = 1;
            }

            if (prints.isPrint == false)
                this.Close();
            else
            {
                Loading loading = new Loading();
                try
                {
                    loading.Show();
                    this.Hide();

                    List<StudentMonthlyResult> copyResult = new List<StudentMonthlyResult>();
                    int startIndex = 0, endIndex = 27;
                    Document document = new Document(PageSize.A4, 0, 0, 0, 0);
                    PdfWriter.GetInstance(document, new FileStream(filePath + "\\" + "ResultTemplate" + ".pdf", FileMode.Create));
                    document.Open();
                    GC.Collect();

                    if (obj.Count <= 26)
                    {
                        if (obj.Count <= 20)
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
                            for (int i = 0; i < 2; i++)
                            {
                                if (i == 0)
                                {
                                    Footer.Visibility = Visibility.Collapsed;
                                    Grid.Dispatcher.Invoke(() =>
                                    {
                                        showData(obj);
                                        Grid.UpdateLayout();
                                    });
                                    PrintList(document);
                                }
                                else if (i == 1)
                                {
                                    Footer.Visibility = Visibility.Visible;
                                    Header.Visibility = Visibility.Collapsed;

                                    DatagridResult.Visibility = Visibility.Collapsed;
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
                        while (true)
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
                                    Footer.Visibility = Visibility.Visible;
                                    PrintList(document);
                                }

                                break;
                            }

                            startIndex = endIndex;

                            if (obj.ToList().Count() - endIndex > 26)
                            {
                                endIndex = startIndex + 30;
                                Header.Visibility = Visibility.Collapsed;

                            }
                            else
                            {
                                endIndex = obj.ToList().Count();
                                if (obj.ToList().Count() - startIndex <= 30)
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
                    loading.Close();
                    this.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    MessageBoxControl message = new MessageBoxControl();
                    message.title = Properties.Langs.Lang.Data;
                    message.discription = Properties.Langs.Lang.noresultdata;
                    message.buttonType = 1;
                    message.ShowDialog();
                    this.Close();
                    loading.Close();
                }
            }

           
        }
        int num = 0;
        private void showData(List<StudentMonthlyResult> obj)
        {
            List<StudentResultForCrystalReport> L1 = new List<StudentResultForCrystalReport>();
           
            lblSchoolName.Content = Properties.Settings.Default.schoolName;
            lblLogoleft.Content = Properties.Settings.Default.logoNameLeft;
            
            int startIndex = 0;
            string avg = "", colors = "";

           
            if (title == "month")
            {
                foreach(var i in obj)
                {
                    if(i.result_monthly==null)
                    {
                        i.result_monthly = new Library.MonthlyResult { rank = obj.Count(), avg_score = "--" };
                    }
                }
                obj.OrderBy(s => !s.result_monthly.Equals(null)).ThenBy(s => s.result_monthly.total_score);
                //--------------------L1-----------------------------------
                for (int i = startIndex; i < (obj.Count() / 1); i++)
                {
                    num++;
                    try
                    {
                        lblTeacherName.Content = obj[i].instructor.name;
                        lblclass.Text = "ថ្នាក់ទី " + obj[i].class_name + " (" + year + ")";
                        lblMonth.Text = "លទ្ធផលប្រចាំ " + DateChange.checkMonth(int.Parse(obj[i].result_monthly.month));
                    }
                    catch { }

                    if (obj[i].gender == "1")
                        obj[i].gender = "ប្រុស";
                    else if(obj[i].gender == "2")
                        obj[i].gender = "ស្រី";
                    if (obj[i].result_monthly.absence_exam.Equals(1))
                    {
                        avg = "មិនចាត់ថ្នាក់";
                        colors = "Red";
                    }
                    else if (obj[i].result_monthly.absence_exam.Equals(2))
                    {
                        avg = "--";
                        colors = "Red";
                        obj[i].result_monthly.visbleTotalScore = "Collapsed";
                        obj[i].result_monthly.visbleRank = "Collapsed";
                    }
                    else
                    {
                        var changeAvg = obj.FirstOrDefault(s => s.result_monthly.avg_score == obj[i].result_monthly.avg_score);
                        if(changeAvg.result_monthly.avg_score!="--")
                        {
                            changeAvg.result_monthly.avg_score = double.Parse(obj[i].result_monthly.avg_score).ToString("#.##");
                        }
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
                        studentId = obj[i].student_school_id,
                        date = obj[i].date_of_birth,
                        gender = obj[i].gender,
                        studentName = obj[i].name,
                        grade = obj[i].result_monthly.grading,
                        visbleRank = obj[i].result_monthly.visbleRank,
                        visbleTotalScore = obj[i].result_monthly.visbleTotalScore

                    });
                }
                //-------------------- end L1---------------------
            }
            else if (title == "semester")
            {
                foreach (var i in obj)
                {
                    if (i.result_semester == null)
                    {
                        i.result_semester = new resultSemester { rank = obj.Count(), avg_score = "--" };
                        i.result_semester_exam = new resultSemesterExam { total_score = 0 };
                    }
                }
                obj.OrderBy(s => s.result_semester_exam!=null).ThenBy(s => s.result_semester_exam.total_score);
                
                //--------------------L1-----------------------------------
                for (int i = startIndex; i < (obj.Count() / 1); i++)
                {
                    lblclass.Text = "ថ្នាក់ទី " + obj[i].class_name + " (" + year + ")";
                    num++;
                    if (num == 2)
                    {
                        lblTeacherName.Content = obj[i].instructor.name;

                        try
                        {
                            if (obj[i].result_semester.term == "FIRST_SEMESTER")
                            {
                                lblMonth.Text = "លទ្ធផលប្រចាំ ឆមាសទី១";
                            }
                            else
                            {
                                lblMonth.Text = "លទ្ធផលប្រចាំ ឆមាសទី២";
                            }
                        }
                        catch { }
                    }
                    if (obj[i].gender == "1")
                        obj[i].gender = "ប្រុស";
                    else
                        obj[i].gender = "ស្រី";
                    L1.Add(new StudentResultForCrystalReport
                    {
                        number = DateChange.Num(num),
                        avg = obj[i].result_semester.avg_score,
                        rank = obj[i].result_semester.rank,
                        color = colors,
                        totalScore = obj[i].result_semester_exam.total_score.ToString(),
                        studentId = obj[i].student_school_id,
                        date = obj[i].date_of_birth,
                        gender = obj[i].gender,
                        studentName = obj[i].name,
                        grade = obj[i].result_semester.grading,


                    });
                }
                //-------------------- end L1---------------------

               
            }
            else if (title == "exam")
            {
                foreach (var i in obj)
                {
                    if (i.result_semester_exam == null)
                    {
                        i.result_semester_exam = new resultSemesterExam { rank = obj.Count().ToString(), avg_score = "--",total_score=0 };
                    }
                }
                obj.OrderBy(s => s.result_semester_exam!=null).ThenBy(s => s.result_semester_exam.total_score);
                for (int i = startIndex; i < (obj.Count() / 1); i++)
                {
                    lblclass.Text = "ថ្នាក់ទី " + obj[i].class_name + " (" + year + ")";
                    num++;
                    if (num == 2)
                    {
                        lblTeacherName.Content = obj[i].instructor.name;

                        if (obj[i].result_semester_exam.term == "FIRST_SEMESTER")
                        {
                            lblMonth.Text = "លទ្ធផលប្រឡង ឆមាសទី១";
                        }
                        else
                        {
                            lblMonth.Text = "លទ្ធផលប្រឡង ឆមាសទី២";
                        }
                    }
                    if (obj[i].gender == "1")
                        obj[i].gender = "ប្រុស";
                    else
                        obj[i].gender = "ស្រី";
                    L1.Add(new StudentResultForCrystalReport
                    {
                        number = DateChange.Num(num),
                        avg = obj[i].result_semester_exam.avg_score,
                        rank = int.Parse(obj[i].result_semester_exam.rank),
                        color = colors,
                        totalScore = obj[i].result_semester_exam.total_score.ToString(),
                        studentId = obj[i].student_school_id,
                        date = obj[i].date_of_birth,
                        gender = obj[i].gender,
                        studentName = obj[i].name,
                        grade = obj[i].result_semester_exam.grading,

                    });
                }
               
            }
            else if (title == "year")
            {
                foreach (var i in obj)
                {
                    if (i.result_yearly == null)
                    {
                        i.result_yearly = new resultYearly { rank = obj.Count(), avg_score = "--" };
                    }
                }
                obj.OrderBy(s => s.result_yearly!=null).ThenBy(s => s.result_yearly.rank);
                for (int i = startIndex; i < (obj.Count() / 1); i++)
                {
                    lblclass.Text = "ថ្នាក់ទី " + obj[i].class_name + " (" + year + ")";
                    num++;
                    if (num == 2)
                    {
                        lblTeacherName.Content = obj[i].instructor.name;

                        lblMonth.Text = "លទ្ធផលប្រចាំឆ្នាំ";
                    }
                    if (obj[i].gender == "1")
                        obj[i].gender = "ប្រុស";
                    else
                        obj[i].gender = "ស្រី";
                    L1.Add(new StudentResultForCrystalReport
                    {
                        number = DateChange.Num(num),
                        avg = obj[i].result_yearly.avg_score,
                        rank = obj[i].result_yearly.rank,
                        color = colors,
                        studentId = obj[i].student_school_id,
                        date = obj[i].date_of_birth,
                        gender = obj[i].gender,
                        studentName = obj[i].name,
                        grade = obj[i].result_yearly.grading,
                    });
                }
                DatagridResult.Columns[5].Visibility = Visibility.Collapsed;
               
            }
            DatagridResult.ItemsSource = L1.OrderBy(r=>r.rank);
           
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
