using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace SerialPortClient
{
    //File class
    public partial class File : Form
    {
        private Main m;
        private bool sendFile;
        private SaveFileDialog sFileDialog = new SaveFileDialog();
        private FileInfo _fileInfo;
        private FileStream sw;
        private long _fileSize;
        private string _checksum;
        public long bufferSize;


        //Checksum property
        public string checksum
        {
            get { return _checksum; }
            set { _checksum = value; }
        }

        //Overloaded initializer
        public File(Main m, bool sendFile)
        {
            InitializeComponent();
            this.m = m;
            this.sendFile = sendFile;
            Start();
        }

        //Get checksum of a file
        public static string GetChecksum(string file)
        {
            using (FileStream stream = System.IO.File.OpenRead(file))
            {
                SHA256Managed sha = new SHA256Managed();
                byte[] checksum = sha.ComputeHash(stream);
                return BitConverter.ToString(checksum).Replace("-", String.Empty);
            }
        }

        //Filesize property
        public long fileSize
        {
            get { return _fileSize; }
            set
            {
                _fileSize = value;
                if (fileSize >= m.serial.WriteBufferSize)
                {
                    bufferSize = m.serial.WriteBufferSize;
                }
                else
                {
                    bufferSize = fileSize;
                }
            }
        }

        //Fileinfo property
        public FileInfo fileInfo
        {
            get { return _fileInfo; }
            set { _fileInfo = value; }
        }


        //Write a byte to a file
        public void WriteByte(byte b)
        {
            sw.WriteByte(b);
            pbStatus.Value = (int)(((double)sw.Position / fileSize) * 100);
            pbStatus.Refresh();
        }

        //Write many bytes to a file
        public void WriteBytes(byte[] b)
        {
            sw.Write(b, 0, (int)bufferSize);
            pbStatus.Value = (int)(((double)sw.Position / fileSize) * 100);
            lblStatus.Text = pbStatus.Value.ToString() + "% Done - " + sw.Position.ToString() + " of " + fileSize.ToString() + " byte(s) received.";
            pbStatus.Refresh();
        }

        //Close the file
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
            m.sendFileToolStripMenuItem.Enabled = true;
        }

        //Send a packet of bytes
        public void SendFile()
        {
            m.sendingReceivingFile = true;
            this.Text = "Sending File " + fileInfo.Name;
            btnClose.Enabled = false;
            this.Visible = true;
            this.Refresh();
            if ((sw.Length - sw.Position) < bufferSize)
            {
                bufferSize = sw.Length - sw.Position;
            }
            while (m.messages.Count > 0)
            {
                m.serial.Write(m.messages.Dequeue());
            }
            byte[] b = new byte[bufferSize];
            sw.Read(b, 0, (int)bufferSize);
            m.serial.Write("#B#" + bufferSize + "#EB#");
            m.serial.Write(b, 0, (int)bufferSize);
            
            if ((((double)sw.Position / sw.Length) - ((double)pbStatus.Value / pbStatus.Maximum)) >= 0.01)
            {
                pbStatus.Value = (int)(Math.Floor(((double)sw.Position / sw.Length) * 100));
                pbStatus.Refresh();
                lblStatus.Text = pbStatus.Value.ToString() + "% Done - " + sw.Position.ToString() + " of " + sw.Length.ToString() + " byte(s) sent.";
                this.Refresh();
            }
            if (sw.Position >= sw.Length)
            {
                m.fileMode = false;
                CloseFile();
                btnClose.Enabled = true;
                m.sendingReceivingFile = false;
            }
        }

        //Ask user for file to send/save
        private void Start(){
            
            if (sendFile)
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.ShowDialog();
                if (System.IO.File.Exists(fileDialog.FileName))
                {
                    checksum = GetChecksum(fileDialog.FileName);
                    fileInfo = new FileInfo(fileDialog.FileName);
                    if (fileInfo.Length >= m.serial.WriteBufferSize)
                    {
                        bufferSize = m.serial.WriteBufferSize;
                    }
                    else
                    {
                        bufferSize = fileInfo.Length;
                    }
                    sw = new FileStream(fileInfo.FullName,FileMode.Open);
                    m.serial.Write("#F#" + fileInfo.Name + "," + fileInfo.Length.ToString() + "," + checksum + "#EF#");
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
                    }
                    

                    sw = new FileStream(fileInfo.FullName, FileMode.Create);


                }
            }
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }
    }
}