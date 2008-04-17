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
    //Main class
    public partial class Main : Form
    {
        [DllImport("user32.dll")]
        static extern bool FlashWindow(IntPtr hwnd, bool bInvert);


        public System.IO.Ports.SerialPort serial;
        private string userName;
        private string remoteUserName;
        private string currentDirectory;

        private byte[] b;
        private string hString;
        private string hCSV = null;
        private Queue<byte> q = new Queue<byte>();
        public bool fileMode = false;
        private string data = "";
        private long fileSIZE;

        public bool sendingReceivingFile = false;

        private File f;

        private DataLogger.DLProgram dl;

        private bool shift = false;

        private bool micro = false;

        private List<string> imageCodes = new List<string>();

        public Queue<string> messages = new Queue<string>();
        private Thread fileThread;
        
        //Initializer
        public Main()
        {
            InitializeComponent();
            remoteUserName = "";
        }

        //Overloaded initializer used
        public Main(string userName, string currDirectory, DataLogger.DLProgram dl)
        {
            this.userName = userName;
            this.dl = dl;
            InitializeComponent();
            remoteUserName = "";
            //Sets current directory for HelpFile.chm
            currentDirectory = currDirectory;
            if (userName == "microcontroller")
            {
                this.micro = true;
                txtMessage.Text = "";
                txtMessage.Enabled = false;
                btnSend.Enabled = false;
            }
        }

        //DataHandling method handles incoming data
        private void dataHandling()
        {
            if (micro)
            {
                while (q.Count > 0)
                {
                    //hString = ((q.Dequeue())).ToString();
                    hString = String.Format("{0:x2}", (UInt16)System.Convert.ToByte(((q.Dequeue())).ToString()));
                    if (hString.ToUpper() == "7E")
                    {
                        dl.WriteLine(data);
                        data += hString;
                        setText(txtHistory, txtHistory.Text + data.Substring(0,data.Length - 2) + Environment.NewLine);
                        data = "";
                    }
                    else
                    {
                        data += hString + " ";
                    }
                    //setText(txtHistory, txtHistory.Text + ((char)(q.Dequeue())).ToString());
                }
            }
            else
            {
                if ((q.Count > 0) && (fileMode))
                {
                    if (q.Count >= f.bufferSize)
                    {
                        byte[] buffer = new byte[f.bufferSize];
                        for (int i = 0; i < f.bufferSize; i++)
                        {
                            buffer[i] = q.Dequeue();
                        }
                        f.WriteBytes(buffer);
                        fileSIZE = fileSIZE - f.bufferSize;
                        fileMode = false;
                        if (fileSIZE <= 0)
                        {
                            f.CloseFile();
                            fileMode = false;
                            sendingReceivingFile = false;
                            if (File.GetChecksum(f.fileInfo.FullName) == f.checksum)
                            {
                                f.lblStatus.Text = "File Successfully Received";
                                serial.Write("#FS#");
                            }
                            else
                            {
                                f.lblStatus.Text = "File Error (Checksum Is Not Equal)";
                                serial.Write("#FE#");
                            }
                        }
                        if (messages.Count > 0)
                        {
                            while (messages.Count > 0)
                            {
                                serial.Write(messages.Dequeue());
                            }
                        }
                        if (fileSIZE > 0)
                        {
                            serial.Write("#N#");
                        }
                    }
                }
                while ((q.Count > 0) && (fileMode == false))
                {
                
                    data += ((char)(q.Dequeue())).ToString();
                    if ((data.Length > 1) && (data.StartsWith("#")) && (data.EndsWith("#")))
                    {
                        string command = data.Substring(data.IndexOf("#") + 1, data.IndexOf("#", data.IndexOf("#") + 1) - 1);
                        switch (command)
                        {
                            case "C":
                                data = "";
                                sendFileToolStripMenuItem.Enabled = true;
                                btnSend.Enabled = true;
                                serial.Write("#CC#");
                                statusLabel.Text = "Connected";
                                serial.Write("#U#" + userName + "#EU#");
                                break;
                            case "CC":
                                data = "";
                                sendFileToolStripMenuItem.Enabled = true;
                                btnSend.Enabled = true;
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
                                    sendFileToolStripMenuItem.Enabled = false;
                                    string message = data.Substring(data.IndexOf("#F#") + 3, data.LastIndexOf("#EF#") - (data.IndexOf("#F#") + 3));
                                    data = "";
                                    string fileName = message.Substring(0, message.IndexOf(","));
                                    string size = message.Substring(message.IndexOf(",") + 1, message.LastIndexOf(",") - message.IndexOf(",") - 1);
                                    string checksum = message.Substring(message.LastIndexOf(",") + 1);
                                    fileSIZE = long.Parse(size);
                                    if (MessageBox.Show(remoteUserName + " wants to send file " + fileName + "(" + size + " bytes)." + System.Environment.NewLine + "Accept?", "Accept File?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                        f = new File(this, false);
                                        f.Text = "Receiving file : " + fileName;
                                        f.fileSize = fileSIZE;
                                        f.checksum = checksum;
                                        fileThread = new Thread(new ThreadStart(newFile));
                                        fileThread.Start();
                                        sendingReceivingFile = true;
                                        serial.Write("#FA#");
                                    }
                                    else
                                    {
                                        serial.Write("#FD#");
                                        sendFileToolStripMenuItem.Enabled = true;
                                    }
                                }
                                break;
                            case "FA":
                                data = "";
                                f.SendFile();
                                break;
                            case "FD":
                                data = "";
                                fileMode = false;
                                f.CloseFile();
                                f.Text = "Sending Cancelled";
                                f.lblStatus.Text = "Remote user cancelled file transfer!";
                                break;
                            case "B":
                                if ((data.Length > 7) && (data.EndsWith("#EB#")))
                                {
                                    string message = data.Substring(data.IndexOf("#B#") + 3, data.LastIndexOf("#EB#") - (data.IndexOf("#B#") + 3));
                                    data = "";
                                    f.bufferSize = long.Parse(message);
                                    fileMode = true;
                                }
                                break;
                            case "N":
                                data = "";
                                f.SendFile();
                                break;
                            case "FE":
                                data = "";
                                f.lblStatus.Text = "File Error (Checksum Is Not Equal)";
                                break;
                            case "FS":
                                data = "";
                                f.lblStatus.Text = "File Successfully Sent";
                                break;
                        }
                    }
                }
            }
        }


        //When form loads
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
                temp = temp.Substring(temp.IndexOf("{\\pict\\wmetafile8\\"));//picw338\\pich338\\picwgoal192\\pichgoal192"));
                temp = temp.Substring(0,temp.IndexOf("}") + 1);
                imageCodes.Add(temp);
            }

            imageCombo.DropDownStyle = ComboBoxStyle.DropDownList;
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
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                cboPorts.Items.Add(s);
            }
            
        }

        
        //Clicked on listening button. port opens
        private void btnListen_Click(object sender, EventArgs e)
        {
            serial = new System.IO.Ports.SerialPort(cboPorts.SelectedItem.ToString());
            serial.Open();
            serial.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(serial_DataReceived);
            statusLabel.Text = "Listening...";
        }

        //Seperate thread when serial port receives data
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
                
            }
            
            
        }

        //Reset form
        private void Reset()
        {
            Exit();
            btnConnect.Enabled = true;
            btnListen.Enabled = true;
            btnSend.Enabled = false;
            sendFileToolStripMenuItem.Enabled = false;
        }


        //Making changing text thread safe
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


        //Changing RTF thread safe
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
                textBox.ScrollToCaret();
                FlashWindow(this.Handle, true);
            }

        }


        //Making changing text when receiving message thread safe.
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
                //Sets the username color to Blue for a received message
                txtTemp.ForeColor = System.Drawing.Color.Blue;
                txtTemp.Select(txtTemp.Text.Length, 1);
                txtTemp.SelectedRtf = text;
                setRTF(txtHistory, txtTemp.Rtf);
                FlashWindow(this.Handle, true);
                dl.WriteLine(txtTemp.Text);
                txtTemp.Clear();
            }

        }

        //Open port and attempt to connect
        private void btnConnect_Click(object sender, EventArgs e)
        {
            serial = new System.IO.Ports.SerialPort(cboPorts.SelectedItem.ToString());
            serial.Open();
            serial.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(serial_DataReceived);
            statusLabel.Text = "Connecting...";
            serial.Write("#C#");
        }

        //Exit
        private void mnuExit_Click(object sender, EventArgs e)
        {
            Exit();
            Close();
        }

        //Exit procedure
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
            dl.CloseFile();
        }

        //Selected a port
        private void cboPorts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboPorts.SelectedIndex >= 0)
            {
                if (micro == false)
                {
                    btnConnect.Enabled = true;
                }
                btnListen.Enabled = true;
            }
            else
            {
                btnConnect.Enabled = false;
                btnListen.Enabled = false;
            }
        }

        //Strip image from RTF
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

        //Put images back in data
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

        //Send a message
        private void btnSend_Click(object sender, EventArgs e)
        {
            
            if (txtMessage.Text != "")
            {
                if (sendingReceivingFile)
                {
                    messages.Enqueue("#M#" + StripImages(txtMessage.Rtf) + "#EM#");
                }
                else
                {
                    serial.Write("#M#" + StripImages(txtMessage.Rtf) + "#EM#");
                }
                txtTemp.Clear();
                txtTemp.Text = userName + " : ";
                //Sets the username color to Red for a sent message
                txtTemp.ForeColor = System.Drawing.Color.Red;
                txtTemp.Select(txtTemp.Text.Length, 1);
                txtTemp.SelectedRtf = txtMessage.Rtf;
                
                setRTF(txtHistory, txtTemp.Rtf);
                txtMessage.Clear();
                txtTemp.Clear();
                txtMessage.Focus();
                
            }
        }

        //Press Help button
        private void mnuAbout_Click(object sender, EventArgs e)
        {
            //Opens HelpFile.chm
            Process p = new Process();
            p.StartInfo.FileName = currentDirectory + "/HelpFile.chm";
            p.Start();
        }

        //Form is closing
        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Exit();
        }

        //Text changed in message box
        private void txtMessage_TextChanged(object sender, EventArgs e)
        {
            btnSend.Enabled = CommonFunctions.NotFormOfBlank(txtMessage.Text);
        }

        //DataLogger options
        private void dataLoggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dl.ShowOptions();
        }

        //Send file
        private void sendFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sendFileToolStripMenuItem.Enabled = false;
            fileThread = new Thread(new ThreadStart(newFile));
            f = new File(this, true);
            fileThread.Start();
        }

        //Start new thread for file
        private void newFile()
        {
            f.ShowDialog();
        }

        //Key is pressed in message box
        private void txtMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            if ((int)(e.KeyChar) == 13) //Enter is pressed
            {
                if (shift) //Shift is pressed
                {
                    txtMessage.SelectedText = "";
                    shift = false;
                }
                else
                {
                    txtMessage.Select(txtMessage.Text.Length - 1, 1);
                    txtMessage.SelectedText = "";
                    btnSend_Click(sender, e);
                }
            }
        }

        //Key is pressed down
        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift)
            {
                shift = true;
            }
        }

        //Key is released
        private void txtMessage_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Shift)
            {
                shift = false;
            }
        }

        //Selected emoticon
        private void imageCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxExItem item = (ComboBoxExItem)imageCombo.SelectedItem;
            Image i = imgIcons.Images[item.ImageIndex];
            Clipboard.Clear();
            Clipboard.SetImage(i);
            txtMessage.Paste();
            txtMessage.Focus();
            Clipboard.Clear();
        }

    }
}