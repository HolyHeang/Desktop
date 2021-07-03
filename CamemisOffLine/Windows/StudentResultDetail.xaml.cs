using Library;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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

namespace CamemisOffLine.Windows
{
    /// <summary>
    /// Interaction logic for StudentResultDetail.xaml
    /// </summary>
    public partial class StudentResultDetail : Window
    {
        public StudentResultDetail()
        {
            InitializeComponent();
        }
        
      
        List<StudentMonthlyResult> results = new List<StudentMonthlyResult>();
        int state = 0,total=0;
        StudentMonthlyResult items;
        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Templates),title="";
        List<string> studentName = new List<string>();
        public StudentResultDetail(StudentMonthlyResult result,List<string> studentName,List<StudentMonthlyResult> list,string title)
        {
            InitializeComponent();
            results = list;
            this.studentName = studentName;
            if (title == "month")
            {
                state = result.result_monthly.rank;
            }
            else if(title== "semester")
            {
                state = result.result_semester.rank;
            }
            this.title = title;
            total = list.Count;
            showData(state);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        private void btnPrivous_Click(object sender, RoutedEventArgs e)
        {
           
            state--;
            txtStudentName.Text = "";
            if(state<=0)
            {
                state = total;
                showData(state);
            }
            else
            {
                showData(state);
            }
           

        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            state++;
            txtStudentName.Text = "";
            if (state>total)
            {
                state = 1;
                showData(state);
            }
            else
            {
                showData(state);
            }
           
        }
        string name=""; string month = "";
        void showData(int state)
        {
            try
            {
                StudentMonthlyResult result = null;
                if(title=="month")
                {
                    foreach (var item in results)
                    {
                        if (item.result_monthly.rank == state && item.name != name)
                        {
                            result = item;
                            break;
                        }
                    }

                    month = DateChange.checkMonth(int.Parse(result.result_monthly.month));

                    txtStuId.Text = result.student_schoolyear_id;
                    txtName.Text = result.name;
                    name = txtName.Text;
                    txtClass.Text = result.class_name;
                    txtDoB.Text = result.date_of_birth;
                    txtTitle.Text = "ព័ត៌មានលទ្ធផលម្អិតប្រចាំ " + month;
                    txtStudentGender.Text = result.gender;

                    txtTotalScore.Text = result.result_monthly.total_score.ToString();
                    txtRank.Text = result.result_monthly.rank.ToString();
                    txtAvg.Text = result.result_monthly.avg_score;
                    txtGrade.Text = result.result_monthly.grading;

                    txtTotalAbsent.Text = result.result_monthly.absence_total;
                    txtWithPermission.Text = result.result_monthly.absence_with_permission;
                    txtNoPermission.Text = result.result_monthly.absence_without_permission;

                    if (result.phone == "")
                    {
                        txtPhone.Text = "គ្មានទិន្នន័យ";
                    }
                    else
                    {
                        txtPhone.Text = result.phone;
                    }
                    if (result.name_en.Length <= 5)
                    {
                        txtNameEng.Text = "គ្មានទិន្នន័យ";
                    }
                    else
                    {
                        txtNameEng.Text = result.name_en;
                    }
                    if (result.result_monthly.absence_exam == 1)
                    {
                        txtAvg.Text = "មិនចាត់ថ្នាក់";
                    }
                    else
                    {
                        txtAvg.Text = double.Parse(result.result_monthly.avg_score).ToString("#.##");
                    }
                    DGMonthlyResultDetail.ItemsSource = result.all_subject_result;
                    items = result;

                    imgProfile.Source = new BitmapImage(new Uri(filePath + "\\" + items.student_schoolyear_id + ".jpg"));
                }
                else if(title== "semester")
                {
                    foreach (var item in results)
                    {
                        if (item.result_semester.rank == state && item.name != name)
                        {
                            result = item;
                            break;
                        }
                    }

                   if(result.result_semester.term== "FIRST_SEMESTER")
                    {
                        month = "ឆមាសទី១";
                    }
                    else
                    {
                        month = "ឆមាសទី២";
                    }

                    txtStuId.Text = result.student_schoolyear_id;
                    txtName.Text = result.name;
                    name = txtName.Text;
                    txtClass.Text = result.class_name;
                    txtDoB.Text = result.date_of_birth;
                    txtTitle.Text = "ព័ត៌មានលទ្ធផលម្អិតប្រចាំ " + month;
                    txtStudentGender.Text = result.gender;

                    txtTotalScore.Text = result.result_semester_exam.total_score.ToString();
                    txtRank.Text = result.result_semester.rank.ToString();
                    txtAvg.Text = result.result_semester.avg_score;
                    txtGrade.Text = result.result_semester.grading;

                    txtTotalAbsent.Text = result.result_semester.absence_total;
                    txtWithPermission.Text = result.result_semester.absence_with_permission;
                    txtNoPermission.Text = result.result_semester.absence_without_permission;

                    if (result.phone == "")
                    {
                        txtPhone.Text = "គ្មានទិន្នន័យ";
                    }
                    else
                    {
                        txtPhone.Text = result.phone;
                    }
                    if (result.name_en.Length <= 5)
                    {
                        txtNameEng.Text = "គ្មានទិន្នន័យ";
                    }
                    else
                    {
                        txtNameEng.Text = result.name_en;
                    }
                    DGMonthlyResultDetail.ItemsSource = result.all_subject_semester_exam_result;
                    items = result;

                    imgProfile.Source = new BitmapImage(new Uri(filePath + "\\" + items.student_schoolyear_id + ".jpg"));
                }
               
            }
            catch
            {
                state--;
                showData(state);
            }
            
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            Transcript transcript = new Transcript(items);
            transcript.ShowDialog();
        }

        void shortName(string stName)
        {
            try
            {
                StudentMonthlyResult result = null;
                if (title == "month")
                {
                    foreach (var item in results)
                    {
                        if (item.name == stName)
                        {
                            result = item;
                        }
                    }

                    month = DateChange.checkMonth(int.Parse(result.result_monthly.month));

                    txtStuId.Text = result.student_schoolyear_id;
                    txtName.Text = result.name;
                    name = txtName.Text;
                    txtClass.Text = result.class_name;
                    txtDoB.Text = result.date_of_birth;
                    txtTitle.Text = "ព័ត៌មានលទ្ធផលម្អិតប្រចាំ " + month;
                    txtStudentGender.Text = result.gender;

                    txtTotalScore.Text = result.result_monthly.total_score.ToString();
                    txtRank.Text = result.result_monthly.rank.ToString();
                    txtAvg.Text = result.result_monthly.avg_score;
                    txtGrade.Text = result.result_monthly.grading;

                    txtTotalAbsent.Text = result.result_monthly.absence_total;
                    txtWithPermission.Text = result.result_monthly.absence_with_permission;
                    txtNoPermission.Text = result.result_monthly.absence_without_permission;

                    if (result.phone == "")
                    {
                        txtPhone.Text = "គ្មានទិន្នន័យ";
                    }
                    else
                    {
                        txtPhone.Text = result.phone;
                    }
                    if (result.name_en.Length <= 5)
                    {
                        txtNameEng.Text = "គ្មានទិន្នន័យ";
                    }
                    else
                    {
                        txtNameEng.Text = result.name_en;
                    }
                    if (result.result_monthly.absence_exam == 1)
                    {
                        txtAvg.Text = "មិនចាត់ថ្នាក់";
                    }
                    else
                    {
                        txtAvg.Text = double.Parse(result.result_monthly.avg_score).ToString("#.##");
                    }
                    DGMonthlyResultDetail.ItemsSource = result.all_subject_result;
                    items = result;

                    imgProfile.Source = new BitmapImage(new Uri(filePath + "\\" + items.student_schoolyear_id + ".jpg"));
                    state = result.result_monthly.rank;
                }
                else if (title == "semester")
                {
                    foreach (var item in results)
                    {
                        if (item.name == stName)
                        {
                            result = item;
                        }
                    }

                    month = result.result_semester.term;

                    txtStuId.Text = result.student_schoolyear_id;
                    txtName.Text = result.name;
                    name = txtName.Text;
                    txtClass.Text = result.class_name;
                    txtDoB.Text = result.date_of_birth;
                    txtTitle.Text = "ព័ត៌មានលទ្ធផលម្អិតប្រចាំ " + month;
                    txtStudentGender.Text = result.gender;

                    txtTotalScore.Text = result.result_semester_exam.total_score.ToString();
                    txtRank.Text = result.result_semester.rank.ToString();
                    txtAvg.Text = result.result_semester.avg_score;
                    txtGrade.Text = result.result_semester.grading;

                    txtTotalAbsent.Text = result.result_semester.absence_total;
                    txtWithPermission.Text = result.result_semester.absence_with_permission;
                    txtNoPermission.Text = result.result_semester.absence_without_permission;

                    if (result.phone == "")
                    {
                        txtPhone.Text = "គ្មានទិន្នន័យ";
                    }
                    else
                    {
                        txtPhone.Text = result.phone;
                    }
                    if (result.name_en.Length <= 5)
                    {
                        txtNameEng.Text = "គ្មានទិន្នន័យ";
                    }
                    else
                    {
                        txtNameEng.Text = result.name_en;
                    }
                    DGMonthlyResultDetail.ItemsSource = result.all_subject_semester_exam_result;
                    items = result;

                    imgProfile.Source = new BitmapImage(new Uri(filePath + "\\" + items.student_schoolyear_id + ".jpg"));
                    state = result.result_semester.rank;
                }
                state = result.result_monthly.rank;
            }
            catch
            {

            }
        }

        private void cmbStudentName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtStudentName.Text = cmbStudentName.SelectedValue.ToString();
            shortName(cmbStudentName.SelectedValue.ToString());
        }

        private void txtStudentName_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                txtStudentName.Text = cmbStudentName.SelectedValue.ToString();
                txtStudentName.Focusable = true;
                txtStudentName.Select(0, txtStudentName.Text.Length);
                cmbStudentName.Text = "";
                cmbStudentName.IsDropDownOpen = true;
            }
            catch { }
        }
        private void printTranscript_Click(object sender, RoutedEventArgs e)
        {
            Transcript transcript = new Transcript(items,title:title);
            transcript.Show();
        }

        private void txtStudentName_TextChanged(object sender, TextChangedEventArgs e)
        {
            cmbStudentName.ItemsSource = studentName.Where(n => n.Contains(txtStudentName.Text));
            cmbStudentName.IsDropDownOpen = true;
        }
    }
}
