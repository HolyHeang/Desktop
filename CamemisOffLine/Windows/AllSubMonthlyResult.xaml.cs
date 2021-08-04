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
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using CamemisOffLine.Component;

namespace CamemisOffLine.Windows
{
    /// <summary>
    /// Interaction logic for AllSubMonthlyResult.xaml
    /// </summary>
    public partial class AllSubMonthlyResult : Microsoft.Office.Interop.Excel.Window
    {
        List<StudentMonthlyResult> obj = new List<StudentMonthlyResult>();
        string title = "",yearTitle="";
        public AllSubMonthlyResult(bool print, List<StudentMonthlyResult> obj, string title = "",string yearTitle="")
        {
            InitializeComponent();
            this.print = print;
            this.obj = obj;
            this.title = title;
            this.yearTitle = yearTitle;
        }
        public AllSubMonthlyResult()
        {
            InitializeComponent();
        }

        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Templates);
        int startIndex = 0;
        int endIndex = 15;
        int currentCell = 0;
        bool print = true;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (print)
            {
                Loading loading = new Loading();
                this.Hide();
               
                //--------------Formula-------------------------------------
                //data = 42
                //if(data < 10)
                //firstIndex = 0
                //endIndex = data.lenth
                //if(data > 10)
                //firstIndex = 0
                //endIndex = 10
                //for{
                // print
                // firstIndex = endIndex
                // if(data.length - 10 >= 20)
                //endIndex = firstIndex + 20
                // else
                //endIndex = fistIndex + data.length - 10
                //}
                // p1 = 15 c1
                // p2-> = >20 c2
                //--------------end Formula-------------------------------------
                MessageBoxControl message = new MessageBoxControl();
                message.Owner = this;
                message.title = "បោះពុម្ភ";
                message.discription = "តើអ្នកចង់បោះពុម្ភមែនទេ?";
                message.result = 0;
                this.Opacity = 0.5;
                message.ShowDialog();
                loading.Show();
                if (message.result==1)
                {
                    //this.Hide();
                    //var for copy item
                    List<StudentMonthlyResult> copyResult = new List<StudentMonthlyResult>();
                    Document document = new Document(PageSize.A4.Rotate(),-5,-5,5,0);
                    PdfWriter.GetInstance(document, new FileStream(filePath + "\\" + "allSubjectTemplate" + ".pdf", FileMode.Create));
                    document.Open();
                    GC.Collect();

                    if (obj.Count() < 15)
                    {
                        endIndex = obj.Count();
                    }
                    if (obj.Count() <= 10)
                    {
                        Grid.Dispatcher.Invoke(() =>
                        {
                            showData(obj.ToList());
                            Grid.UpdateLayout();
                        });
                        PrintList(document);
                        //Print
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

                            //print
                            PrintList(document);

                            //check is it finish
                            if (endIndex == obj.ToList().Count())
                            {
                                if (!footerAvaliable)
                                {
                                    Header.Visibility = Visibility.Collapsed;
                                    Data.Visibility = Visibility.Collapsed;
                                    Footer.Visibility = Visibility.Visible;
                                    PrintList(document);
                                }

                                break;
                            }

                            startIndex = endIndex;

                            if (obj.ToList().Count() - endIndex > 20)
                            {
                                endIndex = startIndex + 20;
                                Header.Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                endIndex = obj.ToList().Count();
                                if (obj.ToList().Count() - startIndex <= 15)
                                {
                                    Header.Visibility = Visibility.Collapsed;
                                    Footer.Visibility = Visibility.Visible;
                                    footerAvaliable = true;
                                }

                            }
                        }
                        GC.Collect();
                    }
                    document.Close();
                    loading.Close();
                    this.Close();
                    Process.Start(filePath + "\\" + "allSubjectTemplate" + ".pdf");
                }
                else
                {
                    loading.Close();
                    this.Close();
                }
            }
            else
            {
                this.Hide();
                Loading loading = new Loading(true);
                loading.Show();
                Grid.Dispatcher.Invoke(() =>
                {
                    showData(obj);
                    Grid.UpdateLayout();
                    ExportDataToExcel();
                });
                loading.Close();
                this.Close();
            }
        }

        void PrintList(Document document)
        {
            string targetFile = System.IO.Path.GetTempFileName();
            using (FileStream outStream = new FileStream(targetFile, FileMode.Create))
            {

                PngBitmapEncoder enc = new PngBitmapEncoder();
                var bitmap = new RenderTargetBitmap((int)Grid.ActualWidth * 4, (int)Grid.ActualHeight * 4, 200, 210, PixelFormats.Pbgra32);
                bitmap.Render(Grid);
                enc.Frames.Add(BitmapFrame.Create(bitmap));
                enc.Save(outStream);
                bitmap = null;

                outStream.Dispose();
            }
            using (FileStream fs = new FileStream(targetFile, FileMode.Open))
            {
                iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromStream(fs), System.Drawing.Imaging.ImageFormat.Png);
                png.ScalePercent(24.2f);
                document.Add(png);
            }
            GC.Collect();
        }

        private void NumberList(int start, List<DIYSubject> obj)
        {
            foreach (var item in obj)
            {
                start++;
                item.number = DateChange.Num(start);
            }
        }
        private void showData(List<StudentMonthlyResult> obj)
        {
            var result = new List<DIYSubject>();
            var result1 = new List<DIYSubject>();
            int i = 1, y = 1, itemCount = 1;

            lblSchoolName.Content = Properties.Settings.Default.schoolName;
            lbllogoLeft.Content = Properties.Settings.Default.logoNameLeft;
            if (title== "month")
            {
                foreach (var item in obj)
                {
                    lblClass.Content = item.class_name+" ("+ yearTitle +")";
                    lblTeacherName.Content = item.instructor.name;
                    lblMonth.Content = DateChange.checkMonth(int.Parse(item.all_subject_result[0].month));
                    result.Add(new DIYSubject
                    {
                        id = item.student_school_id,
                        number = i.ToString(),
                        name = item.name,
                        avg_score = item.result_monthly.avg_score,
                        dob = item.date_of_birth,
                        gender = item.gender,
                        grading = item.result_monthly.grading,
                        rank = item.result_monthly.rank,
                        color = item.result_monthly.color,
                        total_score = item.result_monthly.total_score,
                        student_schoolyear_id = item.student_school_id
                    });
                    var value = result.FirstOrDefault(s => s.id == item.student_school_id);
                    foreach (var k in item.all_subject_result)
                    {
                        if (y == 1)
                        {
                            Data.Columns[5].Header = k.subject_name;
                            value.sub1 = k.subject_name;
                            value.score1 = k.score;
                        }
                        else if (y == 2)
                        {
                            Data.Columns[6].Header = k.subject_name;
                            value.sub2 = k.subject_name;
                            value.score2 = k.score;
                        }
                        else if (y == 3)
                        {
                            Data.Columns[7].Header = k.subject_name;
                            value.sub3 = k.subject_name;
                            value.score3 = k.score;
                        }
                        else if (y == 4)
                        {
                            Data.Columns[8].Header = k.subject_name;
                            value.sub4 = k.subject_name;
                            value.score4 = k.score;
                        }
                        else if (y == 5)
                        {
                            Data.Columns[9].Header = k.subject_name;
                            value.sub5 = k.subject_name;
                            value.score5 = k.score;
                        }
                        else if (y == 6)
                        {
                            Data.Columns[10].Header = k.subject_name;
                            value.sub6 = k.subject_name;
                            value.score6 = k.score;
                        }
                        else if (y == 7)
                        {
                            Data.Columns[11].Header = k.subject_name;
                            value.sub7 = k.subject_name;
                            value.score7 = k.score;
                        }
                        else if (y == 8)
                        {
                            Data.Columns[12].Header = k.subject_name;
                            value.sub8 = k.subject_name;
                            value.score8 = k.score;
                        }
                        else if (y == 9)
                        {
                            Data.Columns[13].Header = k.subject_name;
                            value.sub9 = k.subject_name;
                            value.score9 = k.score;
                        }
                        else if (y == 10)
                        {
                            Data.Columns[14].Header = k.subject_name;
                            value.sub10 = k.subject_name;
                            value.score10 = k.score;
                        }
                        else if (y == 11)
                        {
                            Data.Columns[15].Header = k.subject_name;
                            value.sub11 = k.subject_name;
                            value.score11 = k.score;
                        }
                        else if (y == 12)
                        {
                            Data.Columns[16].Header = k.subject_name;
                            value.sub12 = k.subject_name;
                            value.score12 = k.score;
                        }
                        else if (y == 13)
                        {
                            Data.Columns[17].Header = k.subject_name;
                            value.sub13 = k.subject_name;
                            value.score13 = k.score;
                        }
                        else if (y == 14)
                        {
                            Data.Columns[18].Header = k.subject_name;
                            value.sub14 = k.subject_name;
                            value.score14 = k.score;
                        }
                        else if (y == 15)
                        {
                            Data.Columns[19].Header = k.subject_name;
                            value.sub15 = k.subject_name;
                            value.score15 = k.score;
                        }
                        else if (y == 16)
                        {
                            Data.Columns[20].Header = k.subject_name;
                            value.sub16 = k.subject_name;
                            value.score16 = k.score;
                        }
                        else if (y == 17)
                        {
                            Data.Columns[21].Header = k.subject_name;
                            value.sub17 = k.subject_name;
                            value.score17 = k.score;
                        }
                        else if (y == 18)
                        {
                            Data.Columns[22].Header = k.subject_name;
                            value.sub18 = k.subject_name;
                            value.score18 = k.score;
                        }
                        else if (y == 19)
                        {
                            Data.Columns[23].Header = k.subject_name;
                            value.sub19 = k.subject_name;
                            value.score19 = k.score;
                        }
                        else if (y == 20)
                        {
                            Data.Columns[24].Header = k.subject_name;
                            value.sub20 = k.subject_name;
                            value.score20 = k.score;
                        }
                        y++;
                    }
                    i++;
                    itemCount = y;
                    y = 1;
                }

            }
            else if(title== "semester")
            {
                foreach (var item in obj)
                {
                    lblClass.Content = item.class_name;
                    lblTeacherName.Content = item.instructor.name;
                    if(item.result_semester.term== "FIRST_SEMESTER")
                    {
                        lblMonth.Content = "ឆមាសទី១";
                    }
                    else
                    {
                        lblMonth.Content = "ឆមាសទី២";
                    }
                    result.Add(new DIYSubject
                    {
                        id = item.student_school_id,
                        color="Blue",
                        number = i.ToString(),
                        name = item.name,
                        avg_score = item.result_semester.avg_score,
                        dob = item.date_of_birth,
                        gender = item.gender,
                        grading = item.result_semester.grading,
                        rank = item.result_semester.rank,
                        total_score = item.result_semester_exam.total_score,
                        student_schoolyear_id = item.student_school_id
                    });
                    var value = result.FirstOrDefault(s => s.id == item.student_school_id);
                    foreach (var k in item.all_subject_semester_exam_result)
                    {
                        if (y == 1)
                        {
                            Data.Columns[5].Header = k.subject_name;
                            value.sub1 = k.subject_name;
                            value.score1 = k.score;
                        }
                        else if (y == 2)
                        {
                            Data.Columns[6].Header = k.subject_name;
                            value.sub2 = k.subject_name;
                            value.score2 = k.score;
                        }
                        else if (y == 3)
                        {
                            Data.Columns[7].Header = k.subject_name;
                            value.sub3 = k.subject_name;
                            value.score3 = k.score;
                        }
                        else if (y == 4)
                        {
                            Data.Columns[8].Header = k.subject_name;
                            value.sub4 = k.subject_name;
                            value.score4 = k.score;
                        }
                        else if (y == 5)
                        {
                            Data.Columns[9].Header = k.subject_name;
                            value.sub5 = k.subject_name;
                            value.score5 = k.score;
                        }
                        else if (y == 6)
                        {
                            Data.Columns[10].Header = k.subject_name;
                            value.sub6 = k.subject_name;
                            value.score6 = k.score;
                        }
                        else if (y == 7)
                        {
                            Data.Columns[11].Header = k.subject_name;
                            value.sub7 = k.subject_name;
                            value.score7 = k.score;
                        }
                        else if (y == 8)
                        {
                            Data.Columns[12].Header = k.subject_name;
                            value.sub8 = k.subject_name;
                            value.score8 = k.score;
                        }
                        else if (y == 9)
                        {
                            Data.Columns[13].Header = k.subject_name;
                            value.sub9 = k.subject_name;
                            value.score9 = k.score;
                        }
                        else if (y == 10)
                        {
                            Data.Columns[14].Header = k.subject_name;
                            value.sub10 = k.subject_name;
                            value.score10 = k.score;
                        }
                        else if (y == 11)
                        {
                            Data.Columns[15].Header = k.subject_name;
                            value.sub11 = k.subject_name;
                            value.score11 = k.score;
                        }
                        else if (y == 12)
                        {
                            Data.Columns[16].Header = k.subject_name;
                            value.sub12 = k.subject_name;
                            value.score12 = k.score;
                        }
                        else if (y == 13)
                        {
                            Data.Columns[17].Header = k.subject_name;
                            value.sub13 = k.subject_name;
                            value.score13 = k.score;
                        }
                        else if (y == 14)
                        {
                            Data.Columns[18].Header = k.subject_name;
                            value.sub14 = k.subject_name;
                            value.score14 = k.score;
                        }
                        else if (y == 15)
                        {
                            Data.Columns[19].Header = k.subject_name;
                            value.sub15 = k.subject_name;
                            value.score15 = k.score;
                        }
                        else if (y == 16)
                        {
                            Data.Columns[20].Header = k.subject_name;
                            value.sub16 = k.subject_name;
                            value.score16 = k.score;
                        }
                        else if (y == 17)
                        {
                            Data.Columns[21].Header = k.subject_name;
                            value.sub17 = k.subject_name;
                            value.score17 = k.score;
                        }
                        else if (y == 18)
                        {
                            Data.Columns[22].Header = k.subject_name;
                            value.sub18 = k.subject_name;
                            value.score18 = k.score;
                        }
                        else if (y == 19)
                        {
                            Data.Columns[23].Header = k.subject_name;
                            value.sub19 = k.subject_name;
                            value.score19 = k.score;
                        }
                        else if (y == 20)
                        {
                            Data.Columns[24].Header = k.subject_name;
                            value.sub20 = k.subject_name;
                            value.score20 = k.score;
                        }
                        y++;
                    }
                    i++;
                    itemCount = y;
                    y = 1;
                }
            }
            for (int l = itemCount + 4; l <= 24; l++)
            {
                Data.Columns[l].Visibility = Visibility.Collapsed;
            }
           
            NumberList(startIndex,result.OrderBy(r => r.rank).ToList());
            Data.ItemsSource = result.OrderBy(r => r.rank);
        }

        private void DataGridTemplateColumn_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }

        private void ExportDataToExcel()
        {
            Excel.Application excel = new Excel.Application();
            excel.Visible = false;
            Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            Worksheet sheet1 = (Worksheet)workbook.Sheets[1];

            for (int j = 0; j < Data.Columns.Count; j++)
            {
                if (Data.Columns[j].Header != null)
                {
                    currentCell++;
                    if (Data.Columns[j].Header.Equals("លេខ"))
                    {
                        Range myRange = (Range)sheet1.Cells[2, currentCell];
                        sheet1.Cells[2, currentCell].Font.Bold = true;
                        sheet1.Cells[2, currentCell].Font.Name = "Khmer OS Siemreap";
                        sheet1.Cells[2, currentCell].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        sheet1.Columns[currentCell].ColumnWidth = 10;
                        sheet1.Rows[2].rowheight = 150;
                        myRange.Value2 = Data.Columns[j].Header;
                    }
                    else if (Data.Columns[j].Header.Equals("អត្តលេខ"))
                    {

                        Range myRange = (Range)sheet1.Cells[2, currentCell];
                        sheet1.Cells[2, currentCell].Font.Bold = true;
                        sheet1.Cells[2, currentCell].Font.Name = "Khmer OS Siemreap";
                        sheet1.Cells[2, currentCell].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        myRange.Value2 = Data.Columns[j].Header;
                    }
                    else if (Data.Columns[j].Header.Equals("គោត្តនាម នាម"))
                    {

                        Range myRange = (Range)sheet1.Cells[2, currentCell];
                        sheet1.Cells[2, currentCell].Font.Bold = true;
                        sheet1.Cells[2, currentCell].Font.Name = "Khmer OS Siemreap";
                        sheet1.Cells[2, currentCell].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        sheet1.Columns[currentCell].ColumnWidth = 25;
                        sheet1.Rows[2].rowheight = 30;
                        myRange.Value2 = Data.Columns[j].Header;
                    }
                    else if (Data.Columns[j].Header.Equals("ភេទ"))
                    {

                        Range myRange = (Range)sheet1.Cells[2, currentCell];
                        sheet1.Cells[2, currentCell].Font.Bold = true;
                        sheet1.Cells[2, currentCell].Font.Name = "Khmer OS Siemreap";
                        sheet1.Cells[2, currentCell].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        myRange.Value2 = Data.Columns[j].Header;
                    }
                    else if (Data.Columns[j].Header.Equals("ថ្ងៃខែឆ្នាំកំណើត"))
                    {

                        Range myRange = (Range)sheet1.Cells[2, currentCell];
                        sheet1.Cells[2, currentCell].Font.Bold = true;
                        sheet1.Cells[2, currentCell].Font.Name = "Khmer OS Siemreap";
                        sheet1.Cells[2, currentCell].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        sheet1.Columns[currentCell].ColumnWidth = 20;
                        myRange.Value2 = Data.Columns[j].Header;
                        sheet1.Cells[2, currentCell].Style.Orientation = Excel.XlOrientation.xlUpward;
                    }
                    else if (Data.Columns[j].Header.Equals("ពិន្ទុសរុប"))
                    {
                        sheet1.Cells[2, currentCell].Style.Orientation = Excel.XlOrientation.xlHorizontal;
                        Range myRange = (Range)sheet1.Cells[2, currentCell];
                        sheet1.Cells[2, currentCell].Font.Bold = true;
                        sheet1.Cells[2, currentCell].Font.Name = "Khmer OS Siemreap";
                        sheet1.Cells[2, currentCell].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        sheet1.Columns[currentCell].ColumnWidth = 15;
                        sheet1.Cells[2, currentCell].Style.Orientation = Excel.XlOrientation.xlHorizontal;
                        myRange.Value2 = Data.Columns[j].Header;
                    }
                    else if (Data.Columns[j].Header.Equals("មធ្យមភាគ"))
                    {

                        Range myRange = (Range)sheet1.Cells[2, currentCell];
                        sheet1.Cells[2, currentCell].Font.Bold = true;
                        sheet1.Cells[2, currentCell].Font.Name = "Khmer OS Siemreap";
                        sheet1.Cells[2, currentCell].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        sheet1.Columns[currentCell].ColumnWidth = 15;
                        myRange.Value2 = Data.Columns[j].Header;
                    }
                    else if (Data.Columns[j].Header.Equals("ចំណាត់ថ្នាក់"))
                    {

                        Range myRange = (Range)sheet1.Cells[2, currentCell];
                        sheet1.Cells[2, currentCell].Font.Bold = true;
                        sheet1.Cells[2, currentCell].Font.Name = "Khmer OS Siemreap";
                        sheet1.Cells[2, currentCell].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        sheet1.Columns[currentCell].ColumnWidth = 15;
                        myRange.Value2 = Data.Columns[j].Header;
                    }
                    else if (Data.Columns[j].Header.Equals("និទេ្ទស"))
                    {

                        Range myRange = (Range)sheet1.Cells[2, currentCell];
                        sheet1.Cells[2, currentCell].Font.Bold = true;
                        sheet1.Cells[2, currentCell].Font.Name = "Khmer OS Siemreap";
                        sheet1.Cells[2, currentCell].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        sheet1.Columns[currentCell].ColumnWidth = 15;
                        sheet1.Cells[2, currentCell].Style.Orientation = Excel.XlOrientation.xlHorizontal;
                        myRange.Value2 = Data.Columns[j].Header;
                    }
                    else
                    {
                        Range myRange = (Range)sheet1.Cells[2, currentCell];
                        sheet1.Cells[2, currentCell].Font.Bold = true;
                        sheet1.Cells[2, currentCell].Font.Size = 10;
                        sheet1.Cells[2, currentCell].Font.Name = "Khmer OS Siemreap";
                        sheet1.Cells[2, currentCell].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        sheet1.Columns[currentCell].ColumnWidth = 8;
                        sheet1.Rows[2].rowheight = 150;
                        sheet1.Cells[2, currentCell].Style.Orientation = Excel.XlOrientation.xlUpward;
                        myRange.Value2 = Data.Columns[j].Header;
                    }
                }
            }
            var range = sheet1.Range[sheet1.Cells[1, 1], sheet1.Cells[1, currentCell]];
            int totalColumn = currentCell;
            sheet1.Range[sheet1.Cells[2, 1], sheet1.Cells[2, totalColumn]].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightBlue);
            sheet1.Range[sheet1.Cells[2, 1], sheet1.Cells[Data.Items.Count + 2, totalColumn]].Borders.LineStyle = XlLineStyle.xlContinuous;

            currentCell = 0;
            for (int i = 0; i < Data.Columns.Count; i++)
            {
                if (Data.Columns[i].Header != null)
                {
                    currentCell++;
                    for (int j = 0; j < Data.Items.Count; j++)
                    {
                        try
                        {
                            System.Windows.Controls.TextBlock b = Data.Columns[i].GetCellContent(Data.Items[j]) as System.Windows.Controls.TextBlock;
                            Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 3, currentCell];
                            if (b.Text != null)
                            {
                                myRange.Value2 = b.Text;
                                myRange.Font.Name = "Khmer OS Siemreap";
                                if (i == 2)
                                {
                                    myRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                                }
                                else if (i == 26)
                                {
                                    if (b.Text == "មិនចាត់ថ្នាក់")
                                    {
                                        sheet1.Range[sheet1.Cells[j + 3, 1], sheet1.Cells[j + 3, totalColumn]].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                        myRange.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                        myRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                                    }
                                    else
                                    {
                                        myRange.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
                                        myRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                                    }
                                }
                                else if (i == 27)
                                {
                                    myRange.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    myRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                                }
                                else
                                    myRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            }
            range.Merge();
            sheet1.Cells[1, 1] = "លទ្ធផលប្រចាំ " + lblMonth.Content + "\n" + "ថ្នាក់ទី" + lblClass.Content;
            sheet1.Cells[1, 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenterAcrossSelection;
            sheet1.Cells[1, 1].Font.Name = "Khmer OS Muol Light";
            sheet1.Cells[1, 1].Font.Size = 18;
            sheet1.Cells[1, 1].rowheight = 160;

            excel.Visible = true;
        }
        dynamic Excel.Window.Activate()
        {
            throw new NotImplementedException();
        }

        public dynamic ActivateNext()
        {
            throw new NotImplementedException();
        }

        public dynamic ActivatePrevious()
        {
            throw new NotImplementedException();
        }

        public bool Close(object SaveChanges, object Filename, object RouteWorkbook)
        {
            throw new NotImplementedException();
        }

        public dynamic LargeScroll(object Down, object Up, object ToRight, object ToLeft)
        {
            throw new NotImplementedException();
        }

        public Excel.Window NewWindow()
        {
            throw new NotImplementedException();
        }

        public dynamic _PrintOut(object From, object To, object Copies, object Preview, object ActivePrinter, object PrintToFile, object Collate, object PrToFileName)
        {
            throw new NotImplementedException();
        }

        public dynamic PrintPreview(object EnableChanges)
        {
            throw new NotImplementedException();
        }

        public dynamic ScrollWorkbookTabs(object Sheets, object Position)
        {
            throw new NotImplementedException();
        }

        public dynamic SmallScroll(object Down, object Up, object ToRight, object ToLeft)
        {
            throw new NotImplementedException();
        }

        public int PointsToScreenPixelsX(int Points)
        {
            throw new NotImplementedException();
        }

        public int PointsToScreenPixelsY(int Points)
        {
            throw new NotImplementedException();
        }

        public dynamic RangeFromPoint(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void ScrollIntoView(int Left, int Top, int Width, int Height, object Start)
        {
            throw new NotImplementedException();
        }

        public dynamic PrintOut(object From, object To, object Copies, object Preview, object ActivePrinter, object PrintToFile, object Collate, object PrToFileName)
        {
            throw new NotImplementedException();
        }

        public Excel.Application Application => throw new NotImplementedException();

        public XlCreator Creator => throw new NotImplementedException();

        dynamic Excel.Window.Parent => throw new NotImplementedException();

        public Range ActiveCell => throw new NotImplementedException();

        public Chart ActiveChart => throw new NotImplementedException();

        public Pane ActivePane => throw new NotImplementedException();

        public dynamic ActiveSheet => throw new NotImplementedException();

        public dynamic Caption { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayFormulas { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayGridlines { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayHeadings { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayHorizontalScrollBar { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayOutline { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool _DisplayRightToLeft { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayVerticalScrollBar { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayWorkbookTabs { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayZeros { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool EnableResize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool FreezePanes { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int GridlineColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public XlColorIndex GridlineColorIndex { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Index => throw new NotImplementedException();

        public string OnWindow { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Panes Panes => throw new NotImplementedException();

        public Range RangeSelection => throw new NotImplementedException();

        public int ScrollColumn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int ScrollRow { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Sheets SelectedSheets => throw new NotImplementedException();

        public dynamic Selection => throw new NotImplementedException();

        public bool Split { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int SplitColumn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double SplitHorizontal { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int SplitRow { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double SplitVertical { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double TabRatio { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public XlWindowType Type => throw new NotImplementedException();

        public double UsableHeight => throw new NotImplementedException();

        public double UsableWidth => throw new NotImplementedException();

        public bool Visible { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Range VisibleRange => throw new NotImplementedException();

        public int WindowNumber => throw new NotImplementedException();

        XlWindowState Excel.Window.WindowState { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public dynamic Zoom { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public XlWindowView View { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayRightToLeft { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public SheetViews SheetViews => throw new NotImplementedException();

        public dynamic ActiveSheetView => throw new NotImplementedException();

        public bool DisplayRuler { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool AutoFilterDateGrouping { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayWhitespace { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Hwnd => throw new NotImplementedException();
    }
}
