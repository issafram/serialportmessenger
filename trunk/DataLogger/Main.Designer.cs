namespace DataLogger
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
            this.rbAutomatic = new System.Windows.Forms.RadioButton();
            this.rbManual = new System.Windows.Forms.RadioButton();
            this.comboboxSeconds = new System.Windows.Forms.ComboBox();
            this.grpboxInterval = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpboxInterval.SuspendLayout();
            this.SuspendLayout();
            // 
            // rbAutomatic
            // 
            this.rbAutomatic.AutoSize = true;
            this.rbAutomatic.Location = new System.Drawing.Point(16, 34);
            this.rbAutomatic.Name = "rbAutomatic";
            this.rbAutomatic.Size = new System.Drawing.Size(91, 21);
            this.rbAutomatic.TabIndex = 0;
            this.rbAutomatic.TabStop = true;
            this.rbAutomatic.Text = "Automatic";
            this.rbAutomatic.UseVisualStyleBackColor = true;
            this.rbAutomatic.CheckedChanged += new System.EventHandler(this.rbAutomatic_CheckedChanged);
            // 
            // rbManual
            // 
            this.rbManual.AutoSize = true;
            this.rbManual.Location = new System.Drawing.Point(16, 61);
            this.rbManual.Name = "rbManual";
            this.rbManual.Size = new System.Drawing.Size(194, 21);
            this.rbManual.TabIndex = 1;
            this.rbManual.TabStop = true;
            this.rbManual.Text = "Manual (Every x Seconds)";
            this.rbManual.UseVisualStyleBackColor = true;
            this.rbManual.CheckedChanged += new System.EventHandler(this.rbManual_CheckedChanged);
            // 
            // comboboxSeconds
            // 
            this.comboboxSeconds.FormattingEnabled = true;
            this.comboboxSeconds.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.comboboxSeconds.Location = new System.Drawing.Point(39, 88);
            this.comboboxSeconds.Name = "comboboxSeconds";
            this.comboboxSeconds.Size = new System.Drawing.Size(121, 24);
            this.comboboxSeconds.TabIndex = 2;
            // 
            // grpboxInterval
            // 
            this.grpboxInterval.Controls.Add(this.btnCancel);
            this.grpboxInterval.Controls.Add(this.btnSave);
            this.grpboxInterval.Controls.Add(this.rbManual);
            this.grpboxInterval.Controls.Add(this.comboboxSeconds);
            this.grpboxInterval.Controls.Add(this.rbAutomatic);
            this.grpboxInterval.Location = new System.Drawing.Point(12, 12);
            this.grpboxInterval.Name = "grpboxInterval";
            this.grpboxInterval.Size = new System.Drawing.Size(217, 172);
            this.grpboxInterval.TabIndex = 3;
            this.grpboxInterval.TabStop = false;
            this.grpboxInterval.Text = "Save Method";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(55, 134);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(136, 134);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(243, 204);
            this.Controls.Add(this.grpboxInterval);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Main";
            this.Text = "DataLogger Options";
            this.Load += new System.EventHandler(this.Main_Load);
            this.grpboxInterval.ResumeLayout(false);
            this.grpboxInterval.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rbAutomatic;
        private System.Windows.Forms.RadioButton rbManual;
        private System.Windows.Forms.ComboBox comboboxSeconds;
        private System.Windows.Forms.GroupBox grpboxInterval;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
    }
}

