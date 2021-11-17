using CamemisOffLine.Component;
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
    /// Interaction logic for schedule_student.xaml
    /// </summary>
    public partial class schedule_student : Window
    {
        string classId = "555",className,yearTitle,instructorName,parentId;
        bool internet,isPrintAll;
        public schedule_student()
        {
            InitializeComponent();
            lbllogoLeft.Content = Properties.Settings.Default.logoNameLeft;
            TitleSchool.Content = Properties.Settings.Default.schoolName;
        }
        public schedule_student(string classId,string className,string yearTitle,string instructorName,string parentId = "",bool internet = false,bool isPrintAll = false)
        {
            InitializeComponent();
            this.classId = classId;
            this.className = className;
            this.yearTitle = yearTitle;
            this.instructorName = instructorName;
            this.parentId = parentId;
            this.internet = internet;
            this.isPrintAll = isPrintAll;

            lbllogoLeft.Content = Properties.Settings.Default.logoNameLeft;
            TitleSchool.Content = Properties.Settings.Default.schoolName;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {

            barRight.Visibility = Visibility.Collapsed;
            this.Hide();
            PrintPopup prints = new PrintPopup();
            if (Properties.Settings.Default.role == "1")
            {

                this.IsEnabled = false;
                this.Opacity = 0.5;

                prints.ShowDialog();

                this.Opacity = 1;
                this.IsEnabled = true;
                titleTeacher.Text = prints.position;
                titleAdmin.Visibility = Visibility.Collapsed;
                barCenter.Visibility = prints.CheckCenter;
                barRight.Visibility = prints.CheckRight;

            }
            else
            {
                this.IsEnabled = false;
                this.Opacity = 0.5;

                prints.ShowDialog();

                this.Opacity = 1;
                this.IsEnabled = true;
                txtPosition.Text = prints.position;
                titleAdmin.Visibility = Visibility.Visible;
                barCenter.Visibility = prints.CheckCenter;
                barRight.Visibility = prints.CheckRight;

            }
            if (prints.isPrint == false)
                this.Close();
            else
            {
                Loading loading = new Loading();
                loading.Show();
                this.Hide();
                txtClassName.Text = "ថ្នាក់ " + className;
                txtYear.Text = yearTitle;
                lblTecher.Text = "គ្រូប្រចាំថ្នាក់: " + instructorName;

                List<ListAllSchedule> obj = new List<ListAllSchedule>();
                List<ListSchedule> obj1 = new List<ListSchedule>();
                var rawData = new List<Schedule_Data>();
                var rawData1 = new List<Schedule>();
                var listClassName = new List<string>();

                string respone = "";

                if (internet && Teacher.InternetChecker())
                {
                    respone = await DataAsync(classId, parentId);
                    obj = JObject.Parse(respone).ToObject<ListAllScheduleData>().data;
                    obj1 = JObject.Parse(respone).ToObject<ListSchduleData>().data;
                }
                else
                {
                    respone = getDataFromLocal(parentId);
                    obj = JObject.Parse(respone).ToObject<ListAllScheduleData>().data;
                    foreach (var item in obj)
                    {
                        if (item.id == classId)
                        {
                            className = item.name;
                            instructorName = item.first_instructor.name;
                            foreach (var schdules in item.schedule_data)
                            {
                                foreach (var schdule in schdules.schedule)
                                {
                                    rawData1.Add(schdule);
                                }
                            }
                        }
                    }
                }

                if (!isPrintAll)
                {
                    if (internet && Teacher.InternetChecker())
                    {
                        foreach (var item in obj1)
                        {
                            foreach (var i in item.schedule)
                            {
                                rawData1.Add(i);
                            }
                        }
                    }

                    txtClassName.Text = "ថ្នាក់ " + className;
                    txtYear.Text = yearTitle;
                    lblTecher.Text = "គ្រូប្រចាំថ្នាក់: " + instructorName;
                    Datagridschedule.ItemsSource = null;
                    Datagridschedule.ItemsSource = rawData1;
                    print();
                    loading.Close();
                    this.Close();
                }
                else
                {
                    foreach (var item in obj)
                    {
                        listClassName.Add(item.name);
                        foreach (var i in item.schedule_data)
                        {
                            rawData.Add(i);
                        }
                    }

                    Document document = new Document(PageSize.A4, 5, 0, 0, 0);
                    PdfWriter.GetInstance(document, new FileStream(filePath + "\\" + "កាលវិភាគសិស្ស" + ".pdf", FileMode.Create));
                    document.Open();
                    GC.Collect();

                    for (int i = 0; i < obj.Count(); i++)
                    {
                        var schdule = new List<Schedule>();

                        try
                        {
                            if (obj[i].schedule_data.Count > 1)
                            {
                                for (int j = 0; j < 2; j++)
                                {
                                    foreach (var item in rawData[j + i].schedule)
                                    {
                                        schdule.Add(item);
                                    }
                                }
                                i++;
                            }
                            else
                            {
                                foreach (var item in rawData[i].schedule)
                                {
                                    schdule.Add(item);
                                }
                            }
                            txtClassName.Text = "" + listClassName[i];
                            txtYear.Text = yearTitle;
                            lblTecher.Text = "គ្រូប្រចាំថ្នាក់: " + obj[i].first_instructor.name;
                            Datagridschedule.ItemsSource = null;
                            Datagridschedule.ItemsSource = schdule;
                            Grid.UpdateLayout();

                            string targetFile = System.IO.Path.GetTempFileName();
                            using (FileStream outStream = new FileStream(targetFile, FileMode.Create))
                            {

                                PngBitmapEncoder enc = new PngBitmapEncoder();
                                var bitmap = new RenderTargetBitmap((int)Grid.ActualWidth * 2, (int)Grid.ActualHeight * 2, 115, 115, PixelFormats.Pbgra32);
                                bitmap.Render(Grid);
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
                        catch { }
                    }

                    GC.Collect();
                    document.Close();
                    GC.Collect();
                    Process.Start(filePath + "\\" + "កាលវិភាគសិស្ស" + ".pdf");
                    loading.Close();
                    this.Close();
                }
            }
            
        }

        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Templates);
        private void print()
        {
            Loading load = new Loading();
            load.Show();

            try
            {

                this.Hide();
                Document document = new Document(PageSize.A4, 5, 0, 0, 0);
                PdfWriter.GetInstance(document, new System.IO.FileStream(filePath + "\\" + "កាលវិភាគសិស្ស" + ".pdf", FileMode.Create));

                document.Open();

                GC.Collect();

                string targetFile = System.IO.Path.GetTempFileName();
                using (FileStream outStream = new FileStream(targetFile, FileMode.Create))
                {

                    PngBitmapEncoder enc = new PngBitmapEncoder();
                    var bitmap = new RenderTargetBitmap((int)Grid.ActualWidth * 2, (int)Grid.ActualHeight * 2, 115, 115, PixelFormats.Pbgra32);
                    bitmap.Render(Grid);
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
                MessageBoxControl message = new MessageBoxControl();
                message.title = Properties.Langs.Lang.Data;
                message.discription = Properties.Langs.Lang.noresultdata;
                message.buttonType = 1;
                message.ShowDialog();
                this.Close();
                load.Close();
            }

        }
        private async Task<string> DataAsync(string classId,string parentId)
        {
            string accessUrl = Properties.Settings.Default.acessUrl;
            string token = Properties.Settings.Default.Token;
            string respone = "";

            if (!isPrintAll)
            {
                respone = await RESTApiHelper.GetAll(accessUrl, "/academic/" + classId + "/schedule-with-format", token);
            }
            else
            {
                respone = await RESTApiHelper.GetAll(accessUrl, "/academic/" + parentId + "/schoolyear-all-schedule-with-format", token);
                File.WriteAllText(filePath + "//schdule" + parentId+".txt", respone);
            }
            return respone;
        }
        private string getDataFromLocal(string parentId)
        {
            string respone = File.ReadAllText(filePath + "//schdule" + parentId + ".txt");
            if(respone!=null)
            {
                return respone;
            }
            return null;
        }
    }
}
