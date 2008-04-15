using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace SerialPortClient
{
    public partial class Main : Form
    {
        [DllImport("user32.dll")]
        static extern bool FlashWindow(IntPtr hwnd, bool bInvert);


        public System.IO.Ports.SerialPort serial;
        private string userName;
        private string remoteUserName;

        private byte[] b;
        private Queue<byte> q = new Queue<byte>();
        public bool fileMode = false;
        private string data = "";
        private long fileSIZE;
        Thread dataProcessor;

        private File f;

        private DataLogger.Program dl;

        private bool shift = false;

        private bool micro = false;

        private List<string> imageCodes = new List<string>();

        private Thread fileThread;
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
            if (userName == "microcontroller")
            {
                this.micro = true;
                txtMessage.Text = "";
                txtMessage.Enabled = false;
                btnSend.Enabled = false;
            }
        }

        private void dataHandling()
        {
            if (micro)
            {
                while (q.Count > 0)
                {
                    setText(txtHistory, txtHistory.Text + ((char)(q.Dequeue())).ToString());
                }
            }
            else
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
                        System.Diagnostics.Debug.Print(data);
                        if ((data.Length > 1) && (data.StartsWith("#")) && (data.EndsWith("#")))
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
                                    if ((data.Length > 7) && (data.EndsWith("#EU#")))
                                    {
                                        remoteUserName = data.Substring(data.IndexOf("#U#") + 3, data.LastIndexOf("#EU#") - (data.IndexOf("#U#") + 3));
                                        data = "";
                                    }
                                    break;
                                case "E":
                                    statusLabel.Text = "Connection Closed - By Remote Client";
                                    serial.Close();
                                    Reset();
                                    break;
                                case "M":
                                    if ((data.Length > 7) && (data.EndsWith("#EM#")))
                                    {
                                        string message = data.Substring(data.IndexOf("#M#") + 3, data.LastIndexOf("#EM#") - (data.IndexOf("#M#") + 3));
                                        data = "";
                                        message = IncludeImages(message);
                                        receiveMessage(message);
                                        message = null;
                                    }
                                    break;
                                case "F":
                                    if ((data.Length > 7) && (data.EndsWith("#EF#")))
                                    {
                                        string message = data.Substring(data.IndexOf("#F#") + 3, data.LastIndexOf("#EF#") - (data.IndexOf("#F#") + 3));
                                        data = "";
                                        string fileName = message.Substring(0, message.IndexOf(","));
                                        string size = message.Substring(message.IndexOf(",") + 1);
                                        fileSIZE = long.Parse(size);
                                        if (MessageBox.Show(remoteUserName + " wants to send file " + fileName + "(" + size + " bytes)." + System.Environment.NewLine + "Accept?", "Accept File?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                        {
                                            f = new File(this, false);
                                            f.Text = "Receiving file : " + fileName;
                                            f.fileSize = fileSIZE;
                                            fileMode = true;
                                            fileThread = new Thread(new ThreadStart(newFile));
                                            fileThread.Start();
                                            serial.Write("#FA#");
                                        }
                                        else
                                        {
                                            serial.Write("#FD#");
                                        }
                                    }
                                    break;
                                case "FA":
                                    //f = new File(this, true);
                                    data = "";
                                    f.SendFile();
                                    break;
                                case "FD":
                                    data = "";
                                    fileMode = false;
                                    f.CloseFile();
                                    break;
                            }
                        }
                    }

                    }
            }
        }



        private void Main_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            for (int i = 0; i < imgIcons.Images.Count; i++)
            {
                txtTemp.Clear();
                Image img = imgIcons.Images[i];
                Clipboard.Clear();
                Clipboard.SetImage(img);
                txtTemp.Paste();
                Clipboard.Clear();
                string temp = txtTemp.Rtf;
                temp = temp.Substring(temp.IndexOf("{\\pict\\wmetafile8\\picw338\\pich338\\picwgoal192\\pichgoal192"));
                temp = temp.Substring(0,temp.IndexOf("}") + 1);
                imageCodes.Add(temp);
            }

            imageCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            Image test = new Bitmap("C:\\blah\\test.jpg");
            imageCombo.ImageList = imgIcons;
            imageCombo.Items.Add(new ComboBoxExItem("Grin",0));
            imageCombo.Items.Add(new ComboBoxExItem("???", 1));
            imageCombo.Items.Add(new ComboBoxExItem("Cool", 2));
            imageCombo.Items.Add(new ComboBoxExItem("Shock", 3));
            imageCombo.Items.Add(new ComboBoxExItem("Mad", 4));
            imageCombo.Items.Add(new ComboBoxExItem("Sad", 5));
            imageCombo.Items.Add(new ComboBoxExItem("Smile", 6));
            imageCombo.Items.Add(new ComboBoxExItem("Eek", 7));
            
            b = new byte[1];
            //dataProcessor = new Thread(new ThreadStart(dataHandling));
            //dataProcessor.Start();
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                cboPorts.Items.Add(s);
            }
            
        }

        private void dataReceive()
        {
            while (true)
            {
                try
                {
                    while (serial.BytesToRead > 0)
                    {
                        serial.Read(b, 0, 1);
                        q.Enqueue(b[0]);
                        dataHandling();
                    }
                }
                catch (Exception err)
                {

                }
            }
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            serial = new System.IO.Ports.SerialPort(cboPorts.SelectedItem.ToString());
            serial.Open();
            //dataProcessor = new Thread(new ThreadStart(dataReceive));
            //dataProcessor.Start();
            serial.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(serial_DataReceived);
            statusLabel.Text = "Listening...";
        }

        private void serial_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                while (serial.BytesToRead > 0)
                {
                    serial.Read(b, 0, 1);
                    q.Enqueue(b[0]);
                    dataHandling();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            
            
        }

        private void Reset()
        {
            Exit();
            btnConnect.Enabled = true;
            btnListen.Enabled = true;
            btnSend.Enabled = false;
        }

        delegate void setTextCallback(RichTextBox textBox, string text);
        private void setText(RichTextBox textBox, string text)
        {
            if (textBox.InvokeRequired)
            {
                setTextCallback d = new setTextCallback(setText);
                Invoke(d, new object[] { textBox, text });
            }
            else
            {
                textBox.Text = text;
                textBox.SelectionStart = textBox.Text.Length - 1;
                textBox.ScrollToCaret();
                FlashWindow(this.Handle, true);
            }

        }


        delegate void setTextCallback1(RichTextBox textBox, string text);
        private void setRTF(RichTextBox textBox, string text)
        {
            if (textBox.InvokeRequired)
            {
                setTextCallback1 d = new setTextCallback1(setRTF);
                Invoke(d, new object[] { textBox, text });
            }
            else
            {
                if (textBox.Text.Length > 0)
                {
                    textBox.Select(textBox.Text.Length - 1, 1);
                }
                else
                {
                    textBox.Select(textBox.Text.Length, 1);
                }
                textBox.SelectedRtf = text;
                //textBox.SelectionStart = textBox.Text.Length - 1;
                textBox.ScrollToCaret();
                FlashWindow(this.Handle, true);
            }

        }

        delegate void setTextCallback2(string text);
        private void receiveMessage(string text)
        {
            if ((txtMessage.InvokeRequired) || (txtTemp.InvokeRequired))
            {
                setTextCallback2 d = new setTextCallback2(receiveMessage);
                Invoke(d, new object[] { text });
            }
            else
            {
                txtTemp.Text = remoteUserName + " : ";
                txtTemp.Select(txtTemp.Text.Length, 1);
                txtTemp.SelectedRtf = text;
                setRTF(txtHistory, txtTemp.Rtf);
                FlashWindow(this.Handle, true);
                dl.WriteLine(txtTemp.Text);
                txtTemp.Clear();
            }

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            serial = new System.IO.Ports.SerialPort(cboPorts.SelectedItem.ToString());
            serial.Open();
            //dataProcessor = new Thread(new ThreadStart(dataReceive));
            //dataProcessor.Start();
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
                    //MessageBox.Show("serial port is OPEN");
                    
                    serial.Write("#E#");
                    //MessageBox.Show("wrote EXIT text");
                    serial.Close();
                    //MessageBox.Show("serial port CLOSED");
                }
            }
            dl.CloseFile();
            //dataProcessor.Join();
            //MessageBox.Show("EXIT DONE");
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

        private string StripImages(string data)
        {
            string ret = data;
            for (int i = 0; i < imageCodes.Count; i++)
            {
                if (ret.Contains(imageCodes[i]))
                {
                    ret = ret.Replace(imageCodes[i], "{IMAGE" + i.ToString() + "}");
                }
            }
            return ret;
        }

        private string IncludeImages(string data)
        {
            string ret = data;
            for (int i = 0; i < imageCodes.Count; i++)
            {
                if (ret.Contains("{IMAGE" + i.ToString() + "}"))
                {
                    ret = ret.Replace("{IMAGE" + i.ToString() + "}", imageCodes[i]);
                }
            }
            return ret;
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            
            if (txtMessage.Text != "")
            {
                serial.Write("#M#" + StripImages(txtMessage.Rtf) + "#EM#");
                txtTemp.Clear();
                txtTemp.Text = userName + " : ";
                txtTemp.Select(txtTemp.Text.Length, 1);
                txtTemp.SelectedRtf = txtMessage.Rtf;
                
                setRTF(txtHistory, txtTemp.Rtf);
                //setRTF(txtHistory, txtMessage.Rtf);
                //setRTF(txtHistory, txtHistory.Rtf + userName + " : " + txtMessage.Rtf + System.Environment.NewLine);
                txtMessage.Clear();
                txtTemp.Clear();
                txtMessage.Focus();
                //serial.Write("#M#" + txtMessage.Text + "#EM#");
                //setText(txtHistory, txtHistory.Text + userName + " : " + txtMessage.Text + System.Environment.NewLine);
                //txtMessage.Text = "";
                //txtMessage.Focus();
            }
        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            Process p = new Process();
            p.StartInfo.FileName = "HelpFile.chm";
            p.Start();
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
            fileThread = new Thread(new ThreadStart(newFile));
            f = new File(this, true);
            fileThread.Start();
            //f.ShowDialog();
        }

        private void newFile()
        {
            f.ShowDialog();
        }

        private void txtMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            if ((int)(e.KeyChar) == 13)
            {
                if (shift)
                {
                    string left = txtMessage.Text.Substring(0, txtMessage.SelectionStart);
                    string right = txtMessage.Text.Substring(txtMessage.SelectionStart);
                    txtMessage.Text = left + Environment.NewLine + right;
                    txtMessage.SelectionStart = left.Length;
                }
                else
                {
                    btnSend_Click(sender, e);
                }
            }
        }

        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift)
            {
                shift = true;
            }
        }

        private void txtMessage_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Shift)
            {
                shift = false;
            }
        }

        private void imageCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxExItem item = (ComboBoxExItem)imageCombo.SelectedItem;
            Image i = imgIcons.Images[item.ImageIndex];
            Clipboard.Clear();
            Clipboard.SetImage(i);
            txtMessage.Paste();
            Clipboard.Clear();
        }

    }
}