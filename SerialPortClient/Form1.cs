using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GUI
{
    public partial class Form1 : Form
    {
        public System.IO.Ports.SerialPort serial;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                cboPorts.Items.Add(s);
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //listen
            serial = new System.IO.Ports.SerialPort(cboPorts.SelectedItem.ToString());
            serial.Open();
            statusLabel.Text = "Listening...";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //connect
            serial = new System.IO.Ports.SerialPort(cboPorts.SelectedItem.ToString());
            serial.Open();
            statusLabel.Text = "Connecting...";
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (serial != null){
                serial.Close();
            }
            Close();
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
    }
}