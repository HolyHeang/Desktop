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
    /// Interaction logic for Classification.xaml
    /// </summary>
    public partial class Classification : Window
    {

        public Classification(string classId,string term,string year,int ping)
        {
            InitializeComponent();

            lbllogoLeft.Content = Properties.Settings.Default.logoNameLeft;
            TitleSchool.Content = Properties.Settings.Default.schoolName;

            txtYear.Text = year;
            this.ping = ping;
            this.term = term;
            this.classId = classId;
            if(term== "FIRST_SEMESTER")
            {
                lblMonth.Text = "ចំណាត់-ថ្នាក់ចំណាត់ប្រភេទប្រចាំឆមាសលើកទី១";
            }
            else if(term=="")
            {
                lblMonth.Text = "ចំណាត់-ថ្នាក់ចំណាត់ប្រភេទប្រចាំឆ្នាំ";
            }
            else
            {
                lblMonth.Text = "ចំណាត់-ថ្នាក់ចំណាត់ប្រភេទប្រចាំឆមាសលើកទី២";
            }
        }
        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Templates);
        string classId="",term="";
        int ping = 0;
        
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            Loading loading = new Loading();
            try
            {
                loading.Show();
                this.Hide();
                string str = "";
                string[] data;
                List<StudentMonthlyResult> obj = new List<StudentMonthlyResult>();
                List<Morality> mol = new List<Morality>();
                TitleSchool.Content = Properties.Settings.Default.schoolName;
                if (term != "")
                {
                    if (ping <= 150 && !File.Exists(filePath + "\\" + "semester" + classId + ".txt"))
                    {
                        str = await DataAsync(classId, "1");
                        data = str.Split('|');
                    }
                    else
                    {
                        str = DecodeFrom64(File.ReadAllText(filePath + "\\" + "semester" + classId + ".txt"));
                        data = str.Split('|');
                    }
                    if (term == "FIRST_SEMESTER")
                    {
                        obj = JObject.Parse(data[0]).ToObject<StudentMonthlyResultData>().data.OrderBy(s => s.result_semester.rank).ToList();
                    }
                    else
                    {
                        obj = JObject.Parse(data[1]).ToObject<StudentMonthlyResultData>().data.OrderBy(s => s.result_semester.rank).ToList();
                    }
                    foreach (var item in obj)
                    {
                        txtClassName.Text = "ថ្នាក់ទី " + item.class_name + " ";
                        if (item.result_semester.avg_score == "0")
                        {
                            item.result_semester.avg_score = "មិនចាត់ថ្នាក់";
                            item.result_semester.color = "Red";
                        }
                        if (item.result_semester.morality == null)
                            item.result_semester.morality = "--";
                        if (item.result_semester.bangkeun_phal == null)
                            item.result_semester.bangkeun_phal = "--";
                        if (item.result_semester.health == null)
                            item.result_semester.health = "--";
                    }
                    NumberList(obj.OrderBy(s => s.result_semester.rank).ToList());
                    foreach (var item in obj)
                    {
                        mol.Add(new Morality
                        {
                            avg_score = item.result_semester.avg_score,
                            bangkeun_phal = item.result_semester.bangkeun_phal,
                            gender = item.gender,
                            grading = item.result_semester.grading,
                            id = item.student_school_id,
                            health = item.result_semester.health,
                            morality = item.result_semester.morality,
                            name = item.name,
                            number = item.numbers,
                            profile = item.localProfileLink,
                            rank = item.result_semester.rank
                        });
                    }

                }
                else
                {
                    if (ping <= 150 && !File.Exists(filePath + "\\" + "Year" + classId + ".txt"))
                    {
                        str = await DataAsync(classId, "2");
                        data = str.Split('|');
                    }
                    else
                    {
                        str = DecodeFrom64(File.ReadAllText(filePath + "\\" + "Year" + classId + ".txt"));
                        data = str.Split('|');
                    }
                    obj = JObject.Parse(data[0]).ToObject<StudentMonthlyResultData>().data.OrderBy(s => s.result_yearly.rank).ToList();

                    foreach (var item in obj)
                    {
                        txtClassName.Text = "ថ្នាក់ទី " + item.class_name + " ";
                        if (item.result_yearly.avg_score == "0")
                        {
                            item.result_yearly.avg_score = "មិនចាត់ថ្នាក់";
                            item.result_yearly.color = "Red";
                        }
                        if (item.result_yearly.morality == null)
                            item.result_yearly.morality = "--";
                        if (item.result_yearly.bangkeun_phal == null)
                            item.result_yearly.bangkeun_phal = "--";
                        if (item.result_yearly.health == null)
                            item.result_yearly.health = "--";
                    }
                    NumberList(obj.OrderBy(s => s.result_yearly.rank).ToList());
                    foreach (var item in obj)
                    {
                        mol.Add(new Morality
                        {
                            avg_score = item.result_yearly.avg_score,
                            bangkeun_phal = item.result_yearly.bangkeun_phal,
                            gender = item.gender,
                            grading = item.result_yearly.grading,
                            id = item.student_school_id,
                            health = item.result_yearly.health,
                            morality = item.result_yearly.morality,
                            name = item.name,
                            number = item.numbers,
                            profile = item.localProfileLink,
                            rank = item.result_yearly.rank
                        });
                    }
                }
               
                List<Morality> copyResult = new List<Morality>();
                int startIndex = 0, endIndex = 26;
                Document document = new Document(PageSize.A4, 20, 0, 0, 0);
                PdfWriter.GetInstance(document, new FileStream(filePath + "\\" + "ResultTemplate" + ".pdf", FileMode.Create));
                document.Open();
                GC.Collect();

                if (obj.Count <= 26)
                {
                    if (obj.Count <= 20)
                    {
                        Grid.Dispatcher.Invoke(() =>
                        {
                            showData(mol);
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
                                    showData(mol);
                                    Grid.UpdateLayout();
                                });
                                PrintList(document);
                            }
                            else if (i == 1)
                            {
                                Footer.Visibility = Visibility.Visible;
                                Header.Visibility = Visibility.Collapsed;
                                title.Visibility = Visibility.Collapsed;
                                DGResult.Visibility = Visibility.Collapsed;
                                Grid.Dispatcher.Invoke(() =>
                                {
                                    showData(mol);
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
                            if (mol[i] != null)
                            {
                                copyResult.Add(mol[i]);
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
                                title.Visibility = Visibility.Collapsed;
                                DGResult.Visibility = Visibility.Collapsed;
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
                            title.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            endIndex = obj.ToList().Count();
                            if (obj.ToList().Count() - startIndex <= 30)
                            {
                                Header.Visibility = Visibility.Collapsed;
                                title.Visibility = Visibility.Collapsed;
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
                MessageBoxControl message = new MessageBoxControl();
                message.title = "ទិន្នន័យ";
                message.discription = "មិនមានទិន្នន័យ";
                message.buttonType = 1;
                message.ShowDialog();
                this.Close();
                loading.Close();
            }
        }
        private async Task<string> DataAsync(string classId,string task)
        {
            string accessUrl = Properties.Settings.Default.acessUrl;
            string token = Properties.Settings.Default.Token;
            string respone = "";
            if(task=="1")
            {
                for (int i = 0; i < 2; i++)
                {
                    if (i == 0)
                    {
                        respone = await RESTApiHelper.GetAll(accessUrl, "/academic/" + classId + "/semester-result?term=FIRST_SEMESTER", token);
                    }
                    else if (i == 1)
                    {
                        respone += "|";
                        respone += await RESTApiHelper.GetAll(accessUrl, "/academic/" + classId + "/semester-result?term=SECOND_SEMESTER", token);
                    }
                }

                using (StreamWriter writer = new StreamWriter(filePath + "\\" + "semester" + classId + ".txt"))
                {
                    writer.WriteLine(EncodeTo64(respone));
                }
            }
            else
            {
                respone = await RESTApiHelper.GetAll(accessUrl, "/academic/" + classId + "/yearly-result", token) + "|" + DateTime.Now.ToString("dd/MM/yyyy hh:mm");
                using (StreamWriter writer = new StreamWriter(filePath + "\\" + "Year" + classId + ".txt"))
                {
                    writer.WriteLine(respone);
                }
            }

            return respone;
        }
        private void NumberList(List<StudentMonthlyResult> obj)
        {
            int i = 1;
            foreach (var item in obj)
            {
                if (item.gender == "1")
                    item.gender = "ប្រុស";
                else
                    item.gender = "ស្រី";
                item.numbers = DateChange.Num(i);
                i++;
            }
        }
        static public string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes
                  = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue
                  = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }
        static public string DecodeFrom64(string encodedData)
        {
            byte[] encodedDataAsBytes
                = System.Convert.FromBase64String(encodedData);
            string returnValue =
               System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
            return returnValue;
        }
        private void showData(List<Morality> obj)
        {
            DGResult.ItemsSource = obj.OrderBy(s => s.rank);
        }

        private void DGResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        void PrintList(Document document)
        {
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
                png.ScalePercent(45f);
                document.Add(png);
            }

        }
    }
}
