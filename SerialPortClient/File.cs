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

        public File(Main m, bool sendFile)
        {
            InitializeComponent();
            this.m = m;
            this.sendFile = sendFile;
            Start();
        }

        public FileInfo fileInfo
        {
            get { return _fileInfo; }
            set { _fileInfo = value; }
        }



        public void WriteByte(byte b)
        {
            sw.WriteByte(b);

        }

        public void CloseFile()
        {
            sw.Close();
        }

        public void SendFile()
        {
            byte[] b = new byte[1];
            while (sw.Position < sw.Length)
            {
                sw.Read(b, 0, 1);
                m.serial.Write(b, 0, 1);
            }
            m.fileMode = false;
            CloseFile();
        }

        //private void File_Load(object sender, EventArgs e)
        //{
        [STAThreadAttribute]
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
                }
            }
            
        }
    }
}