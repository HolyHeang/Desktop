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
    /// Interaction logic for Summary_of_Students.xaml
    /// </summary>
    public partial class Summary_of_Students : Window
    {
        public Summary_of_Students()
        {
            InitializeComponent();
        }

        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Templates);

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var reponse = await GetDataAsync();
            var obj = JObject.Parse(reponse).ToObject<SummarySubjectGradingList>().data;
            foreach(var item in obj)
            {
                foreach(var i in item.subjects)
                {
                    foreach (var j in i.grading_system)
                    {
                        if(j.letter_grade=="A")
                        {
                            i.girlA.Add(j.student.female);
                            i.totalA.Add(j.student.total);
                        }
                        else if (j.letter_grade == "B")
                        {
                            i.girlB.Add(j.student.female);
                            i.totalB.Add(j.student.total);
                        }
                        else if (j.letter_grade == "C")
                        {
                            i.girlC.Add(j.student.female);
                            i.totalC.Add(j.student.total);
                        }
                        else if (j.letter_grade == "D")
                        {
                            i.girlD.Add(j.student.female);
                            i.totalD.Add(j.student.total);
                        }
                        else if (j.letter_grade == "E")
                        {
                            i.girlE.Add(j.student.female);
                            i.totalE.Add(j.student.total);
                        }
                        else if (j.letter_grade == "F")
                        {
                            i.girlF.Add(j.student.female);
                            i.totalF.Add(j.student.total);
                        }
                    }
                    
                }
            }
            DG.ItemsSource = obj;
        }
        private async Task<string> GetDataAsync()
        {
            try
            {
                string accessUrl = Properties.Settings.Default.acessUrl;
                string token = Properties.Settings.Default.Token;

                var respone = await RESTApiHelper.GetAll(accessUrl, "/get-summary-subject-gradingscale/508?type=1&month=3&semester=FIRST_SEMESTER", token);
                var obj = JObject.Parse(respone).ToObject<StaffAttendanceDailyList>().data;
               
                return respone;
            }
            catch
            {
                return null;
            }
        }
    }
}
