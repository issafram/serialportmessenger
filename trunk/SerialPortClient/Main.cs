using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;

namespace SerialPortClient
{
    public partial class Main : Form
    {
        [DllImport("user32.dll")]
        static extern bool FlashWindow(IntPtr hwnd, bool bInvert);


        public System.IO.Ports.SerialPort serial;
        private About about;
        private string userName;
        private string remoteUserName;

        private byte[] b;
        private Queue<byte> q = new Queue<byte>();
        private bool fileMode = false;
        private string data = "";
        private long fileSIZE;
        Thread dataProcessor;

        private File f;

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

        private void dataHandling()
        {
            while (q.Count > 0)
            {
                if (fileMode)
                {
                    f.WriteByte(q.Dequeue());
                    fileSIZE = fileSIZE - 1;
                    if (fileSIZE <= 0)
                    {
                        //MessageBox.Show("File RECEIVED");
                        f.CloseFile();
                        fileMode = false;
                    }
                }
                else
                {
                    data += ((char)(q.Dequeue())).ToString();
                    if ((data.Length > 1) & (data.StartsWith("#")) & (data.EndsWith("#")))
                    {
                        string command = data.Substring(data.IndexOf("#") + 1, data.IndexOf("#", data.IndexOf("#") + 1) - 1);
                        switch (command)
                        {
                            case "C":
                                data = "";
                                serial.Write("#CC#");
                                statusLabel.Text = "Connected";
                                serial.Write("#U#" + userName + "#EU#");
                                break;
                            case "CC":
                                data = "";
                                statusLabel.Text = "Connected";
                                serial.Write("#U#" + userName + "#EU#");
                                break;
                            case "U":
                                if ((data.Length > 7) & (data.EndsWith("#EU#")))
                                {
                                    remoteUserName = data.Substring(data.IndexOf("#U#") + 3, data.LastIndexOf("#EU#") - (data.IndexOf("#U#") + 3));
                                    data = "";
                                }
                                break;
                            case "E":
                                statusLabel.Text = "Connection Closed - By Remote Client";
                                Reset();
                                break;
                            case "M":
                                if ((data.Length > 7) & (data.EndsWith("#EM#")))
                                {
                                    string message = data.Substring(data.IndexOf("#M#") + 3, data.LastIndexOf("#EM#") - (data.IndexOf("#M#") + 3));
                                    data = "";
                                    setText(txtHistory, txtHistory.Text + remoteUserName + " : " + message + System.Environment.NewLine);
                                    dl.WriteLine(message);
                                    message = null;
                                }
                                break;
                            case "F":
                                if ((data.Length > 7) & (data.EndsWith("#EF#")))
                                {
                                    string message = data.Substring(data.IndexOf("#F#") + 3, data.LastIndexOf("#EF#") - (data.IndexOf("#F#") + 3));
                                    data = "";
                                    string fileName = message.Substring(0, message.IndexOf(","));
                                    string size = message.Substring(message.IndexOf(",") + 1);
                                    fileSIZE = long.Parse(size);
                                    if (MessageBox.Show(remoteUserName + " wants to send file " + fileName + "(" + size + ")." + System.Environment.NewLine + "Accept?") == DialogResult.OK)
                                    {
                                        f = new File(this,false);
                                        fileMode = true;
                                        serial.Write("#FA#");
                                    }
                                }
                                break;
                            case "FA":
                                //f = new File(this, true);
                                f.SendFile();
                                fileMode = false;
                                break;
                        }
                    }

                }
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            //Control.CheckForIllegalCrossThreadCalls = false;
            b = new byte[1];
            //dataProcessor = new Thread(new ThreadStart(dataHandling));
            //dataProcessor.Start();
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
            while (serial.BytesToRead > 0)
            {
                serial.Read(b, 0, 1);
                q.Enqueue(b[0]);
                dataHandling();
            }
            //dataHandling();
            
            
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
                textBox.SelectionStart = textBox.Text.Length - 1;
                textBox.ScrollToCaret();
                FlashWindow(this.Handle, true);
            }
            
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            serial = new System.IO.Ports.SerialPort(cboPorts.SelectedItem.ToString());
            serial.Open();
            serial.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(serial_DataReceived);
            statusLabel.Text = "Connecting...";
            serial.Write("#C#");
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
                    serial.Write("#E#");
                    serial.Close();
                }
            }
            //if (dataProcessor.ThreadState == ThreadState.Running)
            //{
                //while (dataProcessor.ThreadState != ThreadState.Suspended)
                //{
                    //dataProcessor.Suspend();
                    ////dataProcessor.Abort();
                //}
            //}
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
                serial.Write("#M#" + txtMessage.Text + "#EM#");
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

        private void dataLoggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dl.ShowOptions();
        }

        private void sendFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f = new File(this, true);
            //f.ShowDialog();
        }

    }
}