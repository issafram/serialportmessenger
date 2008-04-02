using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace DataLogger
{
    public class Program
    {
        private StreamWriter sw;

        public Program(string fileName)
        {
            if (File.Exists(fileName + ".csv"))
            {
                sw = File.AppendText(fileName + ".csv");
                //throw new Exception("File already exists");
            }
            else
            {
                sw = File.CreateText(fileName + ".csv");
            }
        }

        public void WriteLine(string data)
        {
            sw.WriteLine(DateTime.Now.ToLongTimeString() + "," + data);
        }

        public void CloseFile()
        {
            sw.Close();
        }

        
        ///// <summary>
        ///// The main entry point for the application.
        ///// </summary>
        //[STAThread]
        //static void Main()
        //{
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    Application.Run(new Main());
        //}
    }
}