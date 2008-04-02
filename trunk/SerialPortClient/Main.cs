using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace SerialPortClient
{
    public partial class Main : Form
    {
        private System.IO.Ports.SerialPort serial;
        private About about;
        private string userName;
        private string remoteUserName;

        private DataLogger.Program dl;
        public Main()
        {
            InitializeComponent();
            remoteUserName = "";
        }

        public Main(string userName, DataLogger.Program dl)
        {
            this.userName = userName;
            this.dl = dl;
            InitializeComponent();
            remoteUserName = "";
        }

        private void Main_Load(object sender, EventArgs e)
        {
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                cboPorts.Items.Add(s);
            }
            
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            serial = new System.IO.Ports.SerialPort(cboPorts.SelectedItem.ToString());
            serial.Open();
            serial.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(serial_DataReceived);
            statusLabel.Text = "Listening...";
        }

        private void serial_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string data = serial.ReadLine();
            string command = data.Substring(data.IndexOf("#") + 1, data.IndexOf("#", data.IndexOf("#") + 1) - 1);
            switch (command)
            {
                case "C":
                    serial.WriteLine("#CC#");
                    statusLabel.Text = "Connected";
                    serial.WriteLine("#U#" + userName + "#EU#");
                    break;
                case "CC":
                    statusLabel.Text = "Connected";
                    serial.WriteLine("#U#" + userName + "#EU#");
                    break;
                case "U":
                    remoteUserName = data.Substring(data.IndexOf("#U#") + 3, data.LastIndexOf("#EU#") - (data.IndexOf("#U#") + 3));
                    break;
                case "E":
                    statusLabel.Text = "Connection Closed - By Remote Client";
                    Reset();
                    break;
                case "M":
                    string message = data.Substring(data.IndexOf("#M#") + 3, data.LastIndexOf("#EM#") - (data.IndexOf("#M#") + 3));
                    setText(txtHistory, txtHistory.Text + remoteUserName + " : " +  message + System.Environment.NewLine);
                    dl.WriteLine(message);
                    message = null;
                    break;
            }
        }

        private void Reset()
        {
            Exit();
            btnConnect.Enabled = true;
            btnListen.Enabled = true;
            btnSend.Enabled = false;
        }

        delegate void setTextCallback(TextBox textBox, string text);
        private void setText(TextBox textBox, string text)
        {
            if (textBox.InvokeRequired)
            {
                setTextCallback d = new setTextCallback(setText);
                Invoke(d, new object[] {textBox, text });
            }
            else
            {
                textBox.Text = text;
            }
            
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            serial = new System.IO.Ports.SerialPort(cboPorts.SelectedItem.ToString());
            serial.Open();
            serial.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(serial_DataReceived);
            statusLabel.Text = "Connecting...";
            serial.WriteLine("#C#");
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Exit();
            Close();
        }

        public void Exit()
        {
            if (serial != null)
            {
                if (serial.IsOpen)
                {
                    serial.WriteLine("#E#");
                    serial.Close();
                }
            }
        }

        private void cboPorts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboPorts.SelectedIndex >= 0)
            {
                btnConnect.Enabled = true;
                btnListen.Enabled = true;
            }
            else
            {
                btnConnect.Enabled = false;
                btnListen.Enabled = false;
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (txtMessage.Text != "")
            {
                serial.WriteLine("#M#" + txtMessage.Text + "#EM#");
                setText(txtHistory, txtHistory.Text + userName + " : " + txtMessage.Text + System.Environment.NewLine);
                txtMessage.Text = "";
                txtMessage.Focus();
            }
        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            if (about == null)
            {
                about = new About();
            }
            else
            {
                if (about.IsDisposed)
                {
                    about = new About();
                }
            }
            about.Show();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Exit();
        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {
            btnSend.Enabled = CommonFunctions.NotFormOfBlank(txtMessage.Text);
        }

    }
}