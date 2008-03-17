using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SerialPortClient
{
    public partial class Main : Form
    {
        private System.IO.Ports.SerialPort serial;
        private About about; 
        public Main()
        {
            InitializeComponent();
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

        void serial_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            txtHistory.Text = txtHistory.Text + serial.ReadLine() + System.Environment.NewLine;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            serial = new System.IO.Ports.SerialPort(cboPorts.SelectedItem.ToString());
            serial.Open();
            serial.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(serial_DataReceived);
            statusLabel.Text = "Connecting...";
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
                serial.Close();
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
            serial.WriteLine(txtMessage.Text);
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




    }
}