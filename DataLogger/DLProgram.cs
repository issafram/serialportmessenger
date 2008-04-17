using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace DataLogger
{
    //DataLogger main class
    public class DLProgram
    {
        private StreamWriter sw;
        private Main m;
        private bool _automatic = true;
        private int _timerInterval = 5;
        private Timer timer = new Timer();
        private Queue<string> q;

        //Overloaded initializer. File to save to.
        public DLProgram(string fileName)
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
            m = new Main(this);
            q = new Queue<string>();
            timer.Tick += new EventHandler(timer_Tick);
        }

        //Timer ticks - Write to file
        void timer_Tick(object sender, EventArgs e)
        {
            while (q.Count > 0)
            {
                WriteToFile(q.Dequeue());
            }
        }

        //Write data to file
        private void WriteToFile(string data)
        {
            sw.WriteLine(data);
        }

        //Write data to queue or file
        public void WriteLine(string data)
        {
            if (automatic)
            {
                WriteToFile(DateTime.Now.ToLongTimeString() + "," + ParseForCSV(data));
            }
            else
            {
                q.Enqueue(DateTime.Now.ToLongTimeString() + "," + ParseForCSV(data));
            }
        }

        //Make sure data is good for CSV format
        private string ParseForCSV(string data)
        {
            string ret = data;
            ret = ret.Replace(',', ' ');
            ret = ret.Replace(Environment.NewLine, Environment.NewLine + ",");
            return ret;
        }

        //Close file
        public void CloseFile()
        {
            timer.Stop();
            while (q.Count > 0)
            {
                WriteToFile(q.Dequeue());
            }
            sw.Close();
        }

        //Show the options
        public void ShowOptions()
        {
            m.ShowDialog();
        }

        //automatic property
        public bool automatic
        {
            get { return _automatic; }
            set
            {
                _automatic = value;
                if (automatic)
                {
                    timer.Stop();
                }
                else
                {
                    timer.Start();
                }
            }
        }

        //Timer Interval property
        public int timerInterval
        {
            get { return _timerInterval; }
            set
            {
                _timerInterval = value;
                timer.Interval = _timerInterval * 1000;
                if (automatic)
                {
                    timer.Stop();
                }
                else
                {
                    timer.Start();
                }
            }
        }

        
    }
}