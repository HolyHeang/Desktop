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

namespace CamemisOffLine.Component
{
    /// <summary>
    /// Interaction logic for PrintCardStyle.xaml
    /// </summary>
    public partial class PrintCardStyle : Window
    {
        public PrintCardStyle()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<CardPrint> card = new List<CardPrint>();

            for(int i = 1;i<7;i++)
            {
                card.Add(new CardPrint
                {
                    title = "Title\nTitle " + i.ToString(),
                    id = i,
                    description = "A car (or automobile) is a wheeled motor vehicle used for transportation. Most definitions of cars say that they run primarily on roads, " +
                    "seat one to eight people, have four wheels, and mainly transport people rather than goods."
                });
            }
            ListTest.ItemsSource = card;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var item = button.DataContext as CardPrint;
            MessageBox.Show(item.title);
        }
    }
}
