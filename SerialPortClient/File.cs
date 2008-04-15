using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SerialPortClient
{
    public partial class File : Form
    {
        private Main m;
        private bool sendFile;
        private SaveFileDialog sFileDialog = new SaveFileDialog();
        private FileInfo _fileInfo;
        private FileStream sw;
        private long _fileSize;

        public File(Main m, bool sendFile)
        {
            InitializeComponent();
            this.m = m;
            this.sendFile = sendFile;
            Start();
        }

        public long fileSize
        {
            get { return _fileSize; }
            set { _fileSize = value; }
        }

        public FileInfo fileInfo
        {
            get { return _fileInfo; }
            set { _fileInfo = value; }
        }


        private int temp = 0;
        public void WriteByte(byte b)
        {
            sw.WriteByte(b);
            pbStatus.Value = (int)(((double)sw.Position / fileSize) * 100);
            pbStatus.Refresh();
        }

        public void CloseFile()
        {
            if (sendFile)
            {
                this.Text = "Sending File Complete!";
            }
            else
            {
                this.Text = "Receiving File Complete!";
            }
            sw.Close();
            btnClose.Enabled = true;
        }

        public void SendFile()
        {

            //this.Show();
            this.Text = "Sending File " + fileInfo.Name;
            pbStatus.Maximum = 100;
            pbStatus.Value = 0;
            btnClose.Enabled = false;
            this.Visible = true;
            this.Refresh();
            byte[] b = new byte[1];
            while (sw.Position < sw.Length)
            {
                sw.Read(b, 0, 1);
                m.serial.Write(b, 0, 1);
                if ((((double)sw.Position / sw.Length) - ((double)pbStatus.Value / pbStatus.Maximum)) >= 0.01)
                {
                    pbStatus.Value = (int)(Math.Floor(((double)sw.Position / sw.Length) * 100));
                    pbStatus.Refresh();
                    this.Refresh();
                }
            }
            m.fileMode = false;
            CloseFile();
            btnClose.Enabled = true;
        }


        private void Start(){
            
            if (sendFile)
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.ShowDialog();
                if (System.IO.File.Exists(fileDialog.FileName))
                {
                    fileInfo = new FileInfo(fileDialog.FileName);
                    sw = new FileStream(fileInfo.FullName,FileMode.Open);
                    m.serial.Write("#F#" + fileInfo.Name + "," + fileInfo.Length.ToString() + "#EF#");
                }
            }
            else
            {
                if (sFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (System.IO.File.Exists(sFileDialog.FileName))
                    {
                        fileInfo = new FileInfo(sFileDialog.FileName);
                    }
                    else
                    {
                        sw = new FileStream(sFileDialog.FileName,FileMode.Create);
                        sw.Close();
                        fileInfo = new FileInfo(sFileDialog.FileName);
                        //StreamWriter s = new StreamWriter(sFileDialog.FileName);
                        //s.Close();
                        //fileInfo = new FileInfo(sFileDialog.FileName);
                    }

                    sw = new FileStream(fileInfo.FullName, FileMode.Create);

                    //this.Show();
                    //this.Text = "Receiving File " + fileInfo.Name;
                    //pbStatus.Maximum = 100;
                    //pbStatus.Value = 0;
                    //btnClose.Enabled = false;
                    //this.Visible = true;

                }
            }
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }
    }
}