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
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using CamemisOffLine.Component;
using Window = Microsoft.Office.Interop.Excel.Window;

namespace CamemisOffLine.Report
{
    /// <summary>
    /// Interaction logic for TeacherInformationExcel.xaml
    /// </summary>
    public partial class TeacherInformationExcel : Window
    {
        public TeacherInformationExcel()
        {
            InitializeComponent();
        }
        int currentCell;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Hide();
            ExportDataToExcel();
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
                    Range myRange = (Range)sheet1.Cells[2, currentCell];
                    sheet1.Cells[2, currentCell].Font.Bold = true;
                    sheet1.Cells[2, currentCell].Font.Name = "Khmer OS Siemreap";
                    sheet1.Cells[2, currentCell].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    sheet1.Columns[currentCell].ColumnWidth = 20;
                    sheet1.Rows[2].rowheight = 50;
                    myRange.Value2 = Data.Columns[j].Header;
                }
            }
            var range = sheet1.Range[sheet1.Cells[1, 1], sheet1.Cells[1, currentCell]];
            int totalColumn = currentCell;
            sheet1.Range[sheet1.Cells[2, 1], sheet1.Cells[2, totalColumn]].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightBlue);
            sheet1.Range[sheet1.Cells[2, 1], sheet1.Cells[Data.Items.Count + 2, totalColumn]].Borders.LineStyle = XlLineStyle.xlContinuous;

            currentCell = 0;
            //for (int i = 0; i < Data.Columns.Count; i++)
            //{
            //    if (Data.Columns[i].Header != null)
            //    {
            //        currentCell++;
            //        for (int j = 0; j < Data.Items.Count; j++)
            //        {
            //            try
            //            {
            //                System.Windows.Controls.TextBlock b = Data.Columns[i].GetCellContent(Data.Items[j]) as System.Windows.Controls.TextBlock;
            //                Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 3, currentCell];
            //                if (b.Text != null)
            //                {
            //                    myRange.Value2 = b.Text;
            //                    myRange.Font.Name = "Khmer OS Siemreap";
            //                    if (i == 2)
            //                    {
            //                        myRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            //                    }
            //                    else if (i == 26)
            //                    {
            //                        if (b.Text == "មិនចាត់ថ្នាក់")
            //                        {
            //                            sheet1.Range[sheet1.Cells[j + 3, 1], sheet1.Cells[j + 3, totalColumn]].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
            //                            myRange.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
            //                            myRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            //                        }
            //                        else
            //                        {
            //                            myRange.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
            //                            myRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            //                        }
            //                    }
            //                    else if (i == 27)
            //                    {
            //                        myRange.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
            //                        myRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            //                    }
            //                    else
            //                        myRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            //                }
            //            }
            //            catch
            //            {

            //            }
            //        }
            //    }
            //}
            range.Merge();
            sheet1.Cells[1, 1] = "បញ្ចីរាយនាមបុគ្គិកសិក្សា ឆ្នាំសិក្សា​ ២០២០-២០២១";
            sheet1.Cells[1, 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenterAcrossSelection;
            sheet1.Cells[1, 1].Font.Name = "Khmer OS Muol Light";
            sheet1.Cells[1, 1].Font.Size = 18;
            sheet1.Cells[1, 1].rowheight = 160;

            excel.Visible = true;
        }

        dynamic Window.Activate()
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

        public Window NewWindow()
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

        dynamic Window.Parent => throw new NotImplementedException();

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

        XlWindowState Window.WindowState { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
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
