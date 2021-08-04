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
    /// Interaction logic for ShowListStudentToPrint.xaml
    /// </summary>
    public partial class ShowListStudentToPrint : Window
    {
        List<StudentMonthlyResult> obj = new List<StudentMonthlyResult>();
        string title = "",yearTitle;
        public ShowListStudentToPrint()
        {
            InitializeComponent();
        }
        public ShowListStudentToPrint(List<StudentMonthlyResult> obj,string title,string yearTitle)
        {
            InitializeComponent();
            this.obj = obj;
            this.title = title;
            this.yearTitle = yearTitle;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            clearSelection();
            DGStudentName.ItemsSource = obj;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            clearSelection();
            checkAll.IsChecked = false;
            DGStudentName.ItemsSource = null;
            DGStudentName.ItemsSource = obj;
            txtNotification.Text = obj.Where(s => s.print == true).Count<StudentMonthlyResult>().ToString();
        }

        private void txtStudentName_TextChanged(object sender, TextChangedEventArgs e)
        {
            DGStudentName.ItemsSource = obj.Where(s=>s.name.Contains(txtStudentName.Text));
            if(txtStudentName.Text=="")
            {
                iconClose.Visibility = Visibility.Collapsed;
            }
            else
            {
                iconClose.Visibility = Visibility.Visible;
            }
        }

        private void btnNamePrint_Click(object sender, RoutedEventArgs e)
        {
            DGStudentName.ItemsSource = obj.Where(s => s.print == true);
            List<StudentMonthlyResult> student = new List<StudentMonthlyResult>();
            try
            {
                this.Hide();
                foreach (var item in obj.Where(s => s.print == true))
                {
                    student.Add(item as StudentMonthlyResult);
                }
                if (student.Count>0)
                {
                    Transcript transcript = new Transcript(null, student, false, false, "", "", true,title:title, yearTitle: yearTitle);
                    transcript.Show();
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                    if(transcript.printOrNot=='1')
                    {
                        this.Close();
                    }
                    else if(transcript.printOrNot == '3')
                    {
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("សូមធ្វើការជ្រើសរើសសិស្សជាមុនសិន", "សារបញ្ចាក់", MessageBoxButton.OK, MessageBoxImage.Error);
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                    DGStudentName.ItemsSource = obj;
                }
            }
            catch
            {
                MessageBox.Show("សូមធ្វើការជ្រើសរើសសិស្សជាមុនសិន", "សារបញ្ចាក់", MessageBoxButton.OK, MessageBoxImage.Error);
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                DGStudentName.ItemsSource = obj;
            }
            
            
        }

        void clearSelection()
        {
            foreach(var item in obj)
            {
                if (item.print)
                {
                    item.print = false;
                }
            }
        }

        private void checkStu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = DGStudentName.SelectedItem as StudentMonthlyResult;

                if (item.print)
                {
                    item.print = false;
                }
                else
                {
                    item.print = true;
                }
                txtNotification.Text = obj.Where(s => s.print == true).Count<StudentMonthlyResult>().ToString();
            }
            catch { }
        }

        private void checkAll_Click(object sender, RoutedEventArgs e)
        {
            
           if(checkAll.IsChecked==true)
            {
                foreach (var item in obj)
                {
                    item.print = true;
                }
            }
            else
            {
                foreach (var item in obj)
                {
                    item.print = false;
                }
            }
            txtNotification.Text = obj.Where(s=>s.print==true).Count<StudentMonthlyResult>().ToString();
            DGStudentName.ItemsSource=null;
            DGStudentName.ItemsSource = obj;
        }

        private void txtStudentName_MouseEnter(object sender, MouseEventArgs e)
        {
            txtStudentName.BorderBrush = Brushes.Black;
        }

        private void txtStudentName_MouseLeave(object sender, MouseEventArgs e)
        {
            txtStudentName.BorderBrush = Brushes.Gray;
        }

        private void txtStudentName_GotFocus(object sender, RoutedEventArgs e)
        {
            txtStudentName.BorderBrush = Brushes.LightBlue;
        }

        private void txtStudentName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtStudentName.BorderBrush = Brushes.Gray;
        }

        private void iconClose_MouseUp(object sender, MouseButtonEventArgs e)
        {
            txtStudentName.Clear();
            iconClose.Visibility = Visibility.Collapsed;
        }

        private void txtStudentName_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtStudentName.BorderBrush = Brushes.LightBlue;
        }
    }
}
