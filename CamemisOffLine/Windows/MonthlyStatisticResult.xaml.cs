using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CamemisOffLine.Windows
{
    /// <summary>
    /// Interaction logic for MonthlyStatisticResult.xaml
    /// </summary>
    public partial class MonthlyStatisticResult : Window
    {
        //--------------Local veriable-------------
        List<StudentMonthlyResult> Results = new List<StudentMonthlyResult>();
        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Templates);

        //-----------------------------------------
        public MonthlyStatisticResult()
        {
            InitializeComponent();
        }
        public MonthlyStatisticResult(List<StudentMonthlyResult> results)
        {
            InitializeComponent();
            Results = results;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            showData();
            loadTotalStudent();
        }
        private void btngoto_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void showData()
        {
            foreach (var item in Results.Take(1))
            {
                txtTitle.Text = "ស្ថិតិថ្នាក់ទី " + item.class_name;
                txtName.Text = item.instructor.name;
                txtDoB.Text = item.instructor.dob;
                txtGender.Text = item.instructor.gender_name;
                txtId.Text = item.instructor.id;
                txtNameEng.Text = item.instructor.name_en;
                txtPhone.Text = item.instructor.phone;
                try
                {
                    imgProfile.Source = new BitmapImage(new Uri(filePath + "\\" + item.instructor.profileMedia.file_name + ".jpg"));
                }
                catch
                {
                    //No image profile
                }
            }
        }
        private void loadTotalStudent()
        {
            int girl = 0, boy = 0, excellentG = 0, goodG = 0, normalG = 0, poorG = 0,
                excellentB = 0, goodB = 0, normalB = 0, poorB = 0;
            foreach (var item in Results)
            {
                if (item.gender == "ប្រុស")
                {
                    boy++;
                    if (item.result_monthly.grading_en == "Good")
                        excellentB++;
                    else if (item.result_monthly.grading_en == "Weak")
                        poorB++;
                    else if (item.result_monthly.grading_en == "Average")
                        normalB++;
                    else
                        goodB++;
                }
                else
                {
                    girl++;
                    if (item.result_monthly.grading_en == "Good")
                        excellentG++;
                    else if (item.result_monthly.grading_en == "Weak")
                        poorG++;
                    else if (item.result_monthly.grading_en == "Average")
                        normalG++;
                    else
                        goodG++;
                }
            }
            //--------------------------------totalStudentChart----------------------------------------
            ((PieSeries)totalStudentChart.Series[0]).ItemsSource = new KeyValuePair<string, int>[] {
                new KeyValuePair<string, int>("ប្រុស",boy),
                new KeyValuePair<string, int>("ស្រី",girl)
            };
            totalStudentChart.Title = "សិស្សសរុប " + Results.Count();
            //-----------------------------------------------------------------------------------------
            //-------------------------------StudentResultChart----------------------------------------
            ((ColumnSeries)StudentResultChart.Series[0]).ItemsSource = new KeyValuePair<string, int>[] {
            new KeyValuePair<string, int>("ល្អ",excellentB),
            new KeyValuePair<string, int>("ល្អបង្គួរ",goodB),
            new KeyValuePair<string, int>("មធ្យម",normalB),
            new KeyValuePair<string, int>("ខ្សោយ",poorB)
            };
            ((ColumnSeries)StudentResultChart.Series[1]).ItemsSource = new KeyValuePair<string, int>[] {
            new KeyValuePair<string, int>("ល្អ",excellentG),
            new KeyValuePair<string, int>("ល្អបង្គួរ",goodG),
            new KeyValuePair<string, int>("មធ្យម",normalG),
            new KeyValuePair<string, int>("ខ្សោយ",poorG)
            };
            //-----------------------------------------------------------------------------------------
        }
    }
}
