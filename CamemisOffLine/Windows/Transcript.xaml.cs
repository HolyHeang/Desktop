using CamemisOffLine.Component;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Library;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CamemisOffLine.Windows
{
    /// <summary>
    /// Interaction logic for Transcript.xaml
    /// </summary>
    public partial class Transcript : Window
    {
        //-----------Gobal Veriable----------------
        string month = "", _class = "", yearTitle="";
        string pathName = "";
        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Templates),title="";
        bool printAll, saveInOne,printCheck;
        StudentMonthlyResult result;
        List<StudentMonthlyResult> results;
        Loading loading = new Loading(true);
        public char printOrNot { get; set; }
        //----------- end Gobal Veriable----------------

        //-----------Constructor------------------------
        public Transcript()
        {
            InitializeComponent();
        }

        public Transcript(StudentMonthlyResult result, List<StudentMonthlyResult> results = null, bool printAll = false, bool saveInOne = false, string month = "", string _class = "",bool print=false,string title="",string yearTitle="")
        {
            InitializeComponent();
            
            this.printAll = printAll;
            this.result = result;
            this.saveInOne = saveInOne;
            this.month = month;
            this._class = _class;
            this.results = results;
            this.printCheck = print;
            this.title = title;
            this.yearTitle = yearTitle;
        }

        //----------- end Constructor------------------------

        //------------event---------------------------------- 
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            btnPrint.Visibility = Visibility.Visible;
            gridButton.Visibility = Visibility.Visible;
            PrintDialog printDialog = new PrintDialog();
            PageMediaSize pageSize = null;
            if (printDialog.ShowDialog() == true)
            {
                btnPrint.Visibility = Visibility.Collapsed;
                gridButton.Visibility = Visibility.Collapsed;
                pageSize = new PageMediaSize(PageMediaSizeName.ISOA4);
                printDialog.PrintTicket.PageMediaSize = pageSize;
                //this.WindowState = WindowState.Normal;
                printDialog.PrintTicket.PageOrientation = PageOrientation.Portrait;
                printDialog.PrintVisual(Grid, "ព្រឹត្តិប័ត្រពិន្ទុសិស្សឈ្មោះ(" + txtName.Text + ")");
                //this.WindowState = WindowState.Maximized;
            }
            this.Close();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.role == "1")
            {
                titleTeacher.Content = "នាយកសាលា";
                titleAdmin.Visibility = Visibility.Collapsed;
                lblTeacherName.Visibility = Visibility.Collapsed;
            }
            if (printCheck)
            {
                this.Hide();
                MessageBoxControl message = new MessageBoxControl();
                message.Owner = this;
                message.title = "បោះពុម្ភ";
                message.discription = "តើអ្នកចង់បោះពុម្ភមែនទេ?";
                message.result = 0;
                this.Opacity = 0.5;
                message.ShowDialog();
                this.Opacity = 1;
                if (message.result==1)
                {
                    btnPrint.Visibility = Visibility.Collapsed;
                    gridButton.Visibility = Visibility.Collapsed;
                    loading.Show();
                    SaveFileInOne(results);
                    loading.Close();
                    printOrNot = '1';
                    this.Close();
                }
                else
                {
                    printOrNot = '2';
                    this.Close();
                }
            }
            else
            {
                if (printAll)
                {
                    this.Hide();
                    System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
                    var resultFile = openFileDlg.ShowDialog();
                    if (resultFile.ToString() == "OK")
                    {
                        btnPrint.Visibility = Visibility.Collapsed;
                        gridButton.Visibility = Visibility.Collapsed;
                        pathName = openFileDlg.SelectedPath;
                        if (saveInOne)
                        {
                            this.Hide();
                            loading.Show();
                            SaveFileInOne(_class, month, results);
                            Process.Start(pathName);
                            loading.Close();
                        }
                        else
                        {
                            print(results);
                        }
                    }
                }
                else
                {
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                    btnPrint.Visibility = Visibility.Collapsed;
                    gridButton.Visibility = Visibility.Collapsed;
                    this.Hide();
                    MessageBoxControl message = new MessageBoxControl();
                    message.Owner = this;
                    message.title = "បោះពុម្ភ";
                    message.discription = "តើអ្នកចង់បោះពុម្ភមែនទេ?";
                    message.result = 0;
                    this.Opacity = 0.5;
                    message.ShowDialog();
                    if (message.result == 1)
                    {
                        this.Hide();
                        saveData(result);
                        this.Close();
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
        }
        private void DGMonthlyResultDetail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //------------end event---------------------------------- 

        //---------------Fuction---------------------------------
        void print(List<StudentMonthlyResult> results = null, string name = "Untitle")
        {
            btnPrint.Visibility = Visibility.Visible;
            gridButton.Visibility = Visibility.Visible;
            PrintDialog printDialog = new PrintDialog();
            PageMediaSize pageSize = null;

            btnPrint.Visibility = Visibility.Collapsed;
            gridButton.Visibility = Visibility.Collapsed;
            pageSize = new PageMediaSize(PageMediaSizeName.ISOA4);
            printDialog.PrintTicket.PageMediaSize = pageSize;
            printDialog.PrintTicket.PageOrientation = PageOrientation.Portrait;
            List<StudentMonthlyResult> obj = new List<StudentMonthlyResult>();


            obj = results;

            foreach (var item in obj)
            {
                showData(item);
                Grid.UpdateLayout();
                GC.Collect();
                SaveFile(item.name);
            }
            GC.Collect();
            this.Close();
        }
        private void SaveFile(string name)
        {

            Document document = new Document(PageSize.A4, 10, 10, 10, 10);

            PdfWriter.GetInstance(document, new System.IO.FileStream(pathName + "\\" + name + ".pdf", FileMode.Create));

            document.Open();
            // document.HtmlStyleClass = "{width:100%; height:100%; object-fit:cover;}";

            // render the fixed document to a WPF Visual object

            // create a temporary file for the bitmap image
            string targetFile = System.IO.Path.GetTempFileName();
            // convert XPS file to an image
            using (FileStream outStream = new FileStream(targetFile, FileMode.Create))
            {
                PngBitmapEncoder enc = new PngBitmapEncoder();
                var bitmap = new RenderTargetBitmap((int)Grid.ActualWidth * 2, (int)Grid.ActualHeight * 2, 130, 130, PixelFormats.Pbgra32);
                bitmap.Render(Grid);
                enc.Frames.Add(BitmapFrame.Create(bitmap));
                enc.Save(outStream);
                bitmap = null;
                outStream.Dispose();
            }

            // add the image to the iTextSharp PDF document
            using (FileStream fs = new FileStream(targetFile, FileMode.Open))
            {
                iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromStream(fs), System.Drawing.Imaging.ImageFormat.Png);
                png.ScalePercent(55f);
                document.Add(png);
                png = null;
                fs.Dispose();
            }
            document.Dispose();
            document.Close();
            GC.Collect();
        }
        void showData(object obj)
        {
            StudentMonthlyResult result = obj as StudentMonthlyResult;
            lblYearTitle.Content = "ឆ្នាំសិក្សា​ (" + yearTitle + ")";
            string id = "";
            if (result.profileMedia.id == null)
                id = result.student_id;
            else
                id = result.profileMedia.id;
            result.localProfileLink = filePath + "\\" + id + ".jpg";
            if (title=="month")
            {
                try
                {
                    lblMonth.Text = "លទ្ធផលប្រចាំ " + DateChange.checkMonth(int.Parse(result.result_monthly.month));
                    lblSchoolName.Content = Properties.Settings.Default.schoolName;
                    lbllogoLeft.Content = Properties.Settings.Default.logoNameLeft;
                    txtStudentId.Text = "អត្តលេខសិស្ស : " + result.student_school_id;
                    txtName.Text = "ឈ្មោះ : " + result.name;
                    txtGender.Text = "ភេទ : " + result.gender;
                    lblClass.Content = result.class_name;
                    _class = result.class_name;
                    txtTotalScore.Text = result.result_monthly.total_score.ToString();
                    txtRank.Text = result.result_monthly.rank.ToString();
                    txtGrade.Text = result.result_monthly.grading;

                    txtTotalAbsent.Text = result.result_monthly.absence_total;
                    txtWithPermission.Text = result.result_monthly.absence_with_permission;
                    txtNoPermission.Text = result.result_monthly.absence_without_permission;

                    txtTeacherRecommend.Text = result.result_monthly.recommendation;
                    txtTeacherCommend.Text = result.result_monthly.teacher_comment;
                    txtBehavoiur.Text = result.result_monthly.behavior;
                    lblTeacherName.Content = result.instructor.name;
                    if (result.gender == "1")
                    {
                        txtGender.Text = "ភេទ : ប្រុស";
                    }
                    else
                    {
                        txtGender.Text = "ភេទ : ស្រី";
                    }
                    if (result.result_monthly.absence_exam == 1)
                    {
                        txtAvg.Text = "មិនចាត់ថ្នាក់";
                    }
                    else
                    {
                        txtAvg.Text = double.Parse(result.result_monthly.avg_score).ToString("#.##");
                    }
                    DGMonthlyResultDetail.ItemsSource = result.all_subject_result.ToList();

                    Grid.UpdateLayout();
                }
                catch { }

                try
                {
                    imgProfile.Source = new BitmapImage(new Uri(result.localProfileLink));
                }
                catch { }

                
            }
            else if(title== "semester")
            {
                try
                {
                    if (result.result_semester.term == "FIRST_SEMESTER")
                    {
                        lblMonth.Text = "លទ្ធផលប្រចាំ ឆមាសទី១";
                    }
                    else
                    {
                        lblMonth.Text = "លទ្ធផលប្រចាំ ឆមាសទី២";
                    }
                    lblSchoolName.Content = Properties.Settings.Default.schoolName;
                    txtStudentId.Text = "អត្តលេខសិស្ស : " + result.student_school_id;
                    txtName.Text = "ឈ្មោះ : " + result.name;
                    txtGender.Text = "ភេទ : " + result.gender;
                    lblClass.Content = result.class_name;
                    _class = result.class_name;
                    txtTotalScore.Text = result.result_semester_exam.total_score.ToString();
                    txtRank.Text = result.result_semester.rank.ToString();
                    txtGrade.Text = result.result_semester.grading;

                    imgProfile.Source = new BitmapImage(new Uri(result.localProfileLink));

                    txtTotalAbsent.Text = result.result_semester.absence_total;
                    txtWithPermission.Text = result.result_semester.absence_with_permission;
                    txtNoPermission.Text = result.result_semester.absence_without_permission;

                    txtTeacherRecommend.Text = result.result_semester.recommendation;
                    txtTeacherCommend.Text = result.result_semester_exam.teacher_comment;
                    txtBehavoiur.Text = result.result_semester.behavior;
                    lblTeacherName.Content = result.instructor.name;
                    txtAvg.Text = result.result_semester.avg_score;
                    if (result.gender == "ប្រុស")
                    {
                        txtGender.Text = "ភេទ : ប្រុស";
                    }
                    else
                    {
                        txtGender.Text = "ភេទ : ស្រី";
                    }
                    DGMonthlyResultDetail.ItemsSource = result.all_subject_semester_exam_result.ToList();

                    Grid.UpdateLayout();
                }
                catch { }
            }
        }
        private void SaveFileInOne(string _class,string month,List<StudentMonthlyResult> results)
        {
            // convert XPS file to an image

            Document document = new Document(PageSize.A4, 10, 20, 10, 10);
            PdfWriter.GetInstance(document, new FileStream(pathName + "\\" + "ព្រឹត្តិប័ត្រពិន្ទុសម្រាប់សិស្សថ្នាក់ទី" + _class + "ប្រចាំ" + month + ".pdf", FileMode.Create));

            document.Open();
            foreach (var item in results.OrderBy(s => s.name))
            {

                showData(item);

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
            }
            document.Close();
            GC.Collect();
        }

        private void SaveFileInOne(List<StudentMonthlyResult> results)
        {
            // convert XPS file to an image
            try
            {


                Document document = new Document(PageSize.A4, 10, 20, 10, 10);
                PdfWriter.GetInstance(document, new FileStream(filePath + "\\" + "ព្រឹត្តិប័ត្រពិន្ទុសម្រាប់សិស្សថ្នាក់ទី" + ".pdf", FileMode.Create));

                document.Open();
                foreach (var item in results.OrderBy(s => s.name))
                {

                    showData(item);

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
                }
                document.Close();
                GC.Collect();
                Process.Start(filePath + "\\" + "ព្រឹត្តិប័ត្រពិន្ទុសម្រាប់សិស្សថ្នាក់ទី" + ".pdf");
            }
            catch
            {
                MessageBoxControl message = new MessageBoxControl();
                message.Owner = this;
                message.title = "បោះពុម្ភ";
                message.discription = "ការបោះពុម្ភរបស់អ្នកមិនទទូលបានជោគជ័យ";
                message.result = 2;
                this.Opacity = 0.5;
                message.ShowDialog();
                
                this.Close();
            }
        }
        private void saveData(StudentMonthlyResult item)
        {
            try
            {
                lblYearTitle.Content = "ឆ្នាំសិក្សា​ (" + yearTitle + ")";
                lbllogoLeft.Content = Properties.Settings.Default.logoNameLeft;
                Document document = new Document(PageSize.A4, 10, 20, 10, 10);
                PdfWriter.GetInstance(document, new FileStream(filePath + "\\" + "ព្រឹត្តិប័ត្រពិន្ទុសម្រាប់សិស្សថ្នាក់ទី" + ".pdf", FileMode.Create));

                document.Open();
                showData(item);

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
                Process.Start(filePath + "\\" + "ព្រឹត្តិប័ត្រពិន្ទុសម្រាប់សិស្សថ្នាក់ទី" + ".pdf");
            }
            catch
            {
                MessageBoxControl message = new MessageBoxControl();
                message.Owner = this;
                message.title = "បោះពុម្ភ";
                message.discription = "ការបោះពុម្ភរបស់អ្នកមិនទទូលបានជោគជ័យ";
                message.result = 2;
                this.Opacity = 0.5;
                message.ShowDialog();
                this.Close();
            }
        }
        //---------------end Fuction---------------------------------
    }
}
