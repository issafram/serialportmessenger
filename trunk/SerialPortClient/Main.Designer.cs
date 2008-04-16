namespace SerialPortClient
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.btnListen = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.cboPorts = new System.Windows.Forms.ComboBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fIleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.baudRateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.baudRateToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.dataLoggerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.status = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.txtHistory = new System.Windows.Forms.RichTextBox();
            this.txtMessage = new System.Windows.Forms.RichTextBox();
            this.imgIcons = new System.Windows.Forms.ImageList(this.components);
            this.txtTemp = new System.Windows.Forms.RichTextBox();
            this.imageCombo = new SerialPortClient.ComboBoxEx();
            this.menuStrip1.SuspendLayout();
            this.status.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnListen
            // 
            this.btnListen.Enabled = false;
            this.btnListen.Location = new System.Drawing.Point(192, 52);
            this.btnListen.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnListen.Name = "btnListen";
            this.btnListen.Size = new System.Drawing.Size(56, 19);
            this.btnListen.TabIndex = 0;
            this.btnListen.Text = "Listen";
            this.btnListen.UseVisualStyleBackColor = true;
            this.btnListen.Click += new System.EventHandler(this.btnListen_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Enabled = false;
            this.btnConnect.Location = new System.Drawing.Point(24, 52);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(56, 19);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // cboPorts
            // 
            this.cboPorts.FormattingEnabled = true;
            this.cboPorts.Location = new System.Drawing.Point(94, 25);
            this.cboPorts.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cboPorts.Name = "cboPorts";
            this.cboPorts.Size = new System.Drawing.Size(92, 21);
            this.cboPorts.TabIndex = 2;
            this.cboPorts.Text = "Select Port";
            this.cboPorts.SelectedIndexChanged += new System.EventHandler(this.cboPorts_SelectedIndexChanged);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(192, 341);
            this.btnSend.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(56, 19);
            this.btnSend.TabIndex = 4;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fIleToolStripMenuItem,
            this.baudRateToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(280, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fIleToolStripMenuItem
            // 
            this.fIleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sendFileToolStripMenuItem,
            this.mnuExit});
            this.fIleToolStripMenuItem.Name = "fIleToolStripMenuItem";
            this.fIleToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fIleToolStripMenuItem.Text = "File";
            // 
            // sendFileToolStripMenuItem
            // 
            this.sendFileToolStripMenuItem.Name = "sendFileToolStripMenuItem";
            this.sendFileToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.sendFileToolStripMenuItem.Text = "Send File";
            this.sendFileToolStripMenuItem.Click += new System.EventHandler(this.sendFileToolStripMenuItem_Click);
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(128, 22);
            this.mnuExit.Text = "Exit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // baudRateToolStripMenuItem
            // 
            this.baudRateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.baudRateToolStripMenuItem1,
            this.dataLoggerToolStripMenuItem});
            this.baudRateToolStripMenuItem.Name = "baudRateToolStripMenuItem";
            this.baudRateToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.baudRateToolStripMenuItem.Text = "Options";
            // 
            // baudRateToolStripMenuItem1
            // 
            this.baudRateToolStripMenuItem1.Name = "baudRateToolStripMenuItem1";
            this.baudRateToolStripMenuItem1.Size = new System.Drawing.Size(144, 22);
            this.baudRateToolStripMenuItem1.Text = "Baud Rate";
            // 
            // dataLoggerToolStripMenuItem
            // 
            this.dataLoggerToolStripMenuItem.Name = "dataLoggerToolStripMenuItem";
            this.dataLoggerToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.dataLoggerToolStripMenuItem.Text = "Data Logger";
            this.dataLoggerToolStripMenuItem.Click += new System.EventHandler(this.dataLoggerToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuHelp});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // mnuHelp
            // 
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(152, 22);
            this.mnuHelp.Text = "SPM Help";
            this.mnuHelp.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // status
            // 
            this.status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.status.Location = new System.Drawing.Point(0, 372);
            this.status.Name = "status";
            this.status.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.status.Size = new System.Drawing.Size(280, 22);
            this.status.TabIndex = 8;
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(25, 17);
            this.statusLabel.Text = "Idle";
            // 
            // txtHistory
            // 
            this.txtHistory.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtHistory.Location = new System.Drawing.Point(16, 83);
            this.txtHistory.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtHistory.Name = "txtHistory";
            this.txtHistory.ReadOnly = true;
            this.txtHistory.Size = new System.Drawing.Size(234, 161);
            this.txtHistory.TabIndex = 10;
            this.txtHistory.Text = "";
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(16, 268);
            this.txtMessage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(234, 73);
            this.txtMessage.TabIndex = 11;
            this.txtMessage.Text = "";
            this.txtMessage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMessage_KeyDown);
            this.txtMessage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMessage_KeyPress);
            this.txtMessage.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtMessage_KeyUp);
            // 
            // imgIcons
            // 
            this.imgIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgIcons.ImageStream")));
            this.imgIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.imgIcons.Images.SetKeyName(0, "icon_biggrin.gif");
            this.imgIcons.Images.SetKeyName(1, "icon_confused.gif");
            this.imgIcons.Images.SetKeyName(2, "icon_cool.gif");
            this.imgIcons.Images.SetKeyName(3, "icon_eek.gif");
            this.imgIcons.Images.SetKeyName(4, "icon_mad.gif");
            this.imgIcons.Images.SetKeyName(5, "icon_sad.gif");
            this.imgIcons.Images.SetKeyName(6, "icon_smile.gif");
            this.imgIcons.Images.SetKeyName(7, "icon_surprised.gif");
            // 
            // txtTemp
            // 
            this.txtTemp.Location = new System.Drawing.Point(192, 25);
            this.txtTemp.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtTemp.Name = "txtTemp";
            this.txtTemp.Size = new System.Drawing.Size(43, 24);
            this.txtTemp.TabIndex = 14;
            this.txtTemp.Text = "";
            this.txtTemp.Visible = false;
            // 
            // imageCombo
            // 
            this.imageCombo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.imageCombo.DropDownHeight = 200;
            this.imageCombo.FormattingEnabled = true;
            this.imageCombo.ImageList = this.imgIcons;
            this.imageCombo.IntegralHeight = false;
            this.imageCombo.Location = new System.Drawing.Point(158, 248);
            this.imageCombo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.imageCombo.Name = "imageCombo";
            this.imageCombo.Size = new System.Drawing.Size(92, 21);
            this.imageCombo.TabIndex = 13;
            this.imageCombo.Text = "Emoticons";
            this.imageCombo.SelectedIndexChanged += new System.EventHandler(this.imageCombo_SelectedIndexChanged);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 394);
            this.Controls.Add(this.txtTemp);
            this.Controls.Add(this.imageCombo);
            this.Controls.Add(this.status);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.cboPorts);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnListen);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.txtHistory);
            this.Controls.Add(this.txtMessage);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Main";
            this.Text = "Serial Port Data Transfer";
            this.Load += new System.EventHandler(this.Main_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnListen;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ComboBox cboPorts;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fIleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.ToolStripMenuItem baudRateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem baudRateToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem dataLoggerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuHelp;
        private System.Windows.Forms.StatusStrip status;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.RichTextBox txtHistory;
        private System.Windows.Forms.RichTextBox txtMessage;
        private ComboBoxEx imageCombo;
        private System.Windows.Forms.ImageList imgIcons;
        private System.Windows.Forms.RichTextBox txtTemp;
        public System.Windows.Forms.ToolStripMenuItem sendFileToolStripMenuItem;
    }
}

