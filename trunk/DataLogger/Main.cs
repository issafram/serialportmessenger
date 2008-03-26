using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace DataLogger
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Excel._Workbook objBook;
            Excel.Workbooks objBooks;
            Excel._Worksheet objSheet;
            Excel.Sheets objSheets;
            Excel.Application excelApp;
            Excel.Range aRange;
            excelApp = new Excel.Application();
            excelApp.Visible = true;
            objBooks = excelApp.Workbooks;
            objBook = objBooks.Add(Missing.Value);
            objSheets = objBook.Worksheets;
            objSheet = (Excel.Worksheet)objSheets.get_Item(1);
            objSheet.Name = "Data Logger";
            aRange = objSheet.get_Range("A1", Missing.Value);
            aRange.Value2 = DateTime.Now.ToString();
            aRange.ColumnWidth = 15;
            //objBook.SaveAs(@"C:\Documents and Settings\Jon Joyce\Desktop\Test.xls", Excel.XlFileFormat.xlWorkbookNormal,
            //                Missing.Value, Missing.Value, Missing.Value, Missing.Value, Excel.XlSaveAsAccessMode.xlNoChange,
            //                Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

        }
    }
}