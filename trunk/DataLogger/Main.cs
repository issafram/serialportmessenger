using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
#region Jon's Code
//using Excel = Microsoft.Office.Interop.Excel;
//using System.Reflection;
#endregion

namespace DataLogger
{
    public partial class Main : Form
    {
        private Program p;
        public Main(Program p)
        {
            InitializeComponent();
            this.p = p;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            rbAutomatic.Checked = p.automatic;
            rbManual.Checked = !p.automatic;
            comboboxSeconds.Enabled = !p.automatic;
            comboboxSeconds.SelectedIndex = comboboxSeconds.Items.IndexOf(p.timerInterval.ToString());

            #region Jon's Code
            //Excel._Workbook objBook;
            //Excel.Workbooks objBooks;
            //Excel._Worksheet objSheet;
            //Excel.Sheets objSheets;
            //Excel.Application excelApp;
            //Excel.Range aRange;
            //excelApp = new Excel.Application();
            //excelApp.Visible = true;
            //objBooks = excelApp.Workbooks;
            //objBook = objBooks.Add(Missing.Value);
            //objSheets = objBook.Worksheets;
            //objSheet = (Excel.Worksheet)objSheets.get_Item(1);
            //objSheet.Name = "Data Logger";
            //aRange = objSheet.get_Range("A1", Missing.Value);
            //aRange.Value2 = DateTime.Now.ToString();
            //aRange.ColumnWidth = 15;
            ////objBook.SaveAs(@"C:\Documents and Settings\Jon Joyce\Desktop\Test.xls", Excel.XlFileFormat.xlWorkbookNormal,
            ////                Missing.Value, Missing.Value, Missing.Value, Missing.Value, Excel.XlSaveAsAccessMode.xlNoChange,
            ////                Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            #endregion
        }

        private void rbAutomatic_CheckedChanged(object sender, EventArgs e)
        {
            Validate();
        }

        private void rbManual_CheckedChanged(object sender, EventArgs e)
        {
            Validate();
        }

        private void Validate()
        {
            comboboxSeconds.Enabled = rbManual.Checked;
            if (rbManual.Checked)
            {
                if (comboboxSeconds.SelectedItem != null)
                {
                    btnSave.Enabled = IsNumeric(comboboxSeconds.SelectedItem.ToString());
                }
                else
                {
                    btnSave.Enabled = false;
                }
            }
            else
            {
                btnSave.Enabled = true;
            }
        }

        public static bool IsNumeric(string Expression)
        {
            bool answer = true;
            if (Expression.Length == 0)
            {
                answer = false;
            }
            else
            {
                for (int i = 0; i < Expression.Length; i++)
                {
                    if (!char.IsNumber(Expression, i))
                    {
                        answer = false;
                    }
                }
            }
            return answer;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            p.automatic = rbAutomatic.Checked;
            if (!p.automatic)
            {
                p.timerInterval = int.Parse(comboboxSeconds.SelectedItem.ToString());
            }
            this.Close();
        }

    }
}