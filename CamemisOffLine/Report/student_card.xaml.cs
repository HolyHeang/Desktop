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
    /// Interaction logic for student_card.xaml
    /// </summary>
    public partial class student_card : Window
    {
        string schoolYearId,yearTitle;
        int type;
        bool internet;
        public student_card()
        {
            InitializeComponent();
        }
        public student_card(string schoolYearId,string yearTitle,string classId,string gradeId,string level,int type,bool internet)
        {
            InitializeComponent();
            this.schoolYearId = schoolYearId;
            this.yearTitle = yearTitle;
            this.classId = classId;
            this.gradeId = gradeId;
            this.level = level;
            this.type = type;
            this.internet = internet;
        }
        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Templates);
        string classId;
        string titleYear;
        string gradeId;
        string level;
        string encryptionString;

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var dateTime = DateTime.Now.ToString("dd/M/yyyy");
            var dateParse = dateTime.Split('/');
            var date = "ថ្ងៃទី "+DateChange.Num(int.Parse(dateParse[0]))+"​ "+DateChange.checkMonth(int.Parse(dateParse[1]))+" ឆ្នាំ"+ DateChange.Num(int.Parse(dateParse[2]));
            var exipireDate = "ថ្ងៃផុតកំណត់​ ថ្ងៃ" + DateChange.Num(int.Parse(dateParse[0])) + "​ " + DateChange.checkMonth(int.Parse(dateParse[1])) + " ឆ្នាំ" + DateChange.Num(int.Parse(dateParse[2])+1);
            var yearParse = "";

            try
            {
                var year = yearTitle.Split('-');
                yearParse = DateChange.Num(int.Parse(year[0])).ToString() + "~" + DateChange.Num(int.Parse(year[1])).ToString();
            }
            catch
            {
                yearParse = yearTitle;
            }
            this.Hide();
            Loading loading = new Loading();
            loading.Show();
            var respone = "";
            var obj = new List<StuedntofTheYear>();
            if (Teacher.InternetChecker()&&internet)
            {
                string accessUrl = Properties.Settings.Default.acessUrl;
                string token = Properties.Settings.Default.Token;

                if (type == 1 && schoolYearId != "")
                {
                    respone = await RESTApiHelper.GetAll(accessUrl, "/student-schoolyear-search/" + schoolYearId, token);
                    encryptionString = Teacher.EncodeTo64(respone);
                    SaveString(schoolYearId);
                }
                else if (type == 2 && classId != "")
                {
                    respone = await RESTApiHelper.GetAll(accessUrl, "/student-schoolyear-search/" + schoolYearId + "?classId=" + classId, token);
                }
                else if (type == 3 && gradeId != "")
                {
                    respone = await RESTApiHelper.GetAll(accessUrl, "/student-schoolyear-search/" + schoolYearId + "?gradeId=" + gradeId, token);
                }
                else if (type == 4 && level != "")
                {
                    respone = await RESTApiHelper.GetAll(accessUrl, "/student-schoolyear-search/" + schoolYearId + "?level=" + level, token);
                }
                else
                {
                    loading.Close();
                }
                obj = JObject.Parse(respone).ToObject<AllStudentList>().data;
            }
            else
            {
                respone = GetString(schoolYearId);
                var data = JObject.Parse(respone).ToObject<AllStudentList>().data;

                if (type == 1)
                {
                    
                    obj = data;
                }
                else if (type == 2)
                {   
                    obj = data.Where(s => s.current_class_id == classId).ToList();
                }
                else if (type == 3)
                {  
                    obj = data.Where(s => s.grade_id == gradeId).ToList();
                }
                else if (type == 4)
                {
                    obj = data.Where(s => s.level == level).ToList();
                }
                respone = null;
            }

            if (type==2)
            {
                ShowListStudentToPrintCard printCard = new ShowListStudentToPrintCard(obj);
                printCard.ShowDialog();
                obj = printCard.obj;
            }

            try
            {
                int startIndex = 0;
                Document document = new Document(PageSize.A4.Rotate(), 5, 0, 5, 0);
                PdfWriter.GetInstance(document, new FileStream(filePath + "\\" + "ResultTemplate" + ".pdf", FileMode.Create));
                document.Open();
                GC.Collect();
                if(obj.Count>8)
                {
                    int numberOfData = 8;
                    for (int i = 0; i <Convert.ToInt32(obj.Count / 8)+1; i++)
                    {
                        if(obj.Count-startIndex<8)
                        {
                            numberOfData = obj.Count-startIndex;
                        }
                        var copyData = obj.GetRange(startIndex, numberOfData);
                        foreach (var item in copyData)
                        {
                            item.current_class = "ថ្នាក់​ " + item.current_class;
                            item.year = "ឆ្នាំសិក្សា " + yearParse;
                            item.schoolName = Properties.Settings.Default.schoolName;
                            item.day = date;
                            item.expireDate = exipireDate;
                            item.logoNameLeft = Properties.Settings.Default.logoNameLeft;
                        }
                        showData(copyData);
                        PrintList(document);
                        startIndex += 8;
                        GC.Collect();
                    }
                }
                else
                {
                    var copyData = obj.GetRange(startIndex, obj.Count);
                    foreach (var item in copyData)
                    {
                        item.current_class = "ថ្នាក់​ " + item.current_class;
                        item.year = "ឆ្នាំសិក្សា " + yearParse;
                        item.schoolName = Properties.Settings.Default.schoolName;
                        item.day = date;
                        item.expireDate = exipireDate;
                        item.logoNameLeft = Properties.Settings.Default.logoNameLeft;
                    }
                    showData(copyData);
                    PrintList(document);
                    startIndex += 8;
                    GC.Collect();
                }

                document.Close();
                Process.Start(filePath + "\\" + "ResultTemplate" + ".pdf");
                loading.Close();
                this.Close();
            }
            catch
            {

            }

          
        }

        private void SaveString(string schoolId)
        {

            using (StreamWriter writer = new StreamWriter(filePath + "\\" + schoolId + ".txt"))
            {
                writer.WriteLine(encryptionString);
            }
        }
        private string GetString(string schoolId)
        {
            string readText = "";
            readText = File.ReadAllText(filePath + "\\" + schoolId + ".txt");
            return Teacher.DecodeFrom64(readText);
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

        private void showData(List<StuedntofTheYear> data)
        {
            StudentCard.ItemsSource = null;
            StudentCard.ItemsSource = data;
            StudentCard.UpdateLayout();
        }
        void PrintList(Document document)
        {
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

        }
    }
}
