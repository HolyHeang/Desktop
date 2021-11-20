using CamemisOffLine.Component;
using Library;
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
    /// Interaction logic for ShowListStudentToPrintCard.xaml
    /// </summary>
    public partial class ShowListStudentToPrintCard : Window
    {
       public List<StuedntofTheYear> obj = new List<StuedntofTheYear>();
        public bool isClose;
        public ShowListStudentToPrintCard()
        {
            InitializeComponent();
        }
        public ShowListStudentToPrintCard(List<StuedntofTheYear> obj)
        {
            InitializeComponent();
            this.obj = obj;
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
            txtNotification.Text = obj.Where(s => s.print == true).Count<StuedntofTheYear>().ToString();
        }

        private void txtStudentName_TextChanged(object sender, TextChangedEventArgs e)
        {
            DGStudentName.ItemsSource = obj.Where(s => s.name.Contains(txtStudentName.Text));
            if (txtStudentName.Text == "")
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
            List<StuedntofTheYear> student = new List<StuedntofTheYear>();
            try
            {
                this.Hide();
                foreach (var item in obj.Where(s => s.print == true))
                {
                    student.Add(item as StuedntofTheYear);
                }
                if (student.Count > 0)
                {
                    obj = student;
                }
                else
                {
                    this.Opacity = 0.5;
                    MessageBoxControl messageBox = new MessageBoxControl();
                    messageBox.title = Properties.Langs.Lang.print;
                    messageBox.discription = Properties.Langs.Lang.Please_select_students_first;
                    messageBox.buttonType = 1;
                    messageBox.ShowDialog();
                    isClose = true;
                    this.Opacity = 1;
                    //MessageBox.Show("សូមធ្វើការជ្រើសរើសសិស្សជាមុនសិន", "សារបញ្ចាក់", MessageBoxButton.OK, MessageBoxImage.Error);
                    //Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                    //DGStudentName.ItemsSource = obj;
                }
            }
            catch
            {
                this.Opacity = 0.5;
                MessageBoxControl messageBox = new MessageBoxControl();
                messageBox.title = Properties.Langs.Lang.print;
                messageBox.discription = Properties.Langs.Lang.Please_select_students_first;
                messageBox.buttonType = 1;
                messageBox.ShowDialog();
                isClose = true;
                this.Opacity = 1;
                //MessageBox.Show("សូមធ្វើការជ្រើសរើសសិស្សជាមុនសិន", "សារបញ្ចាក់", MessageBoxButton.OK, MessageBoxImage.Error);
                //Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                //DGStudentName.ItemsSource = obj;
            }


        }

        void clearSelection()
        {
            foreach (var item in obj)
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
                var item = DGStudentName.SelectedItem as StuedntofTheYear;

                if (item.print)
                {
                    item.print = false;
                }
                else
                {
                    item.print = true;
                }
                txtNotification.Text = obj.Where(s => s.print == true).Count<StuedntofTheYear>().ToString();
            }
            catch { }
        }

        private void checkAll_Click(object sender, RoutedEventArgs e)
        {

            if (checkAll.IsChecked == true)
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
            txtNotification.Text = obj.Where(s => s.print == true).Count<StuedntofTheYear>().ToString();
            DGStudentName.ItemsSource = null;
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
            txtStudentName.BorderBrush = Brushes.Black;
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

        private void gridExit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isClose = true;
            this.Close();

        }

        private void gridExit_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid g = (Grid)sender;
            g.Background = Brushes.Red;
        }

        private void gridExit_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid g = (Grid)sender;
            g.Background = Brushes.Transparent;
        }

        private void txtStudentName_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtStudentName.BorderBrush = Brushes.LightBlue;
        }
    }
}
