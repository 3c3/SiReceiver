namespace SiReceiver
{
    partial class Form1
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
            this.portCb = new System.Windows.Forms.ComboBox();
            this.openCloseBtn = new System.Windows.Forms.Button();
            this.startBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // portCb
            // 
            this.portCb.FormattingEnabled = true;
            this.portCb.Location = new System.Drawing.Point(12, 24);
            this.portCb.Name = "portCb";
            this.portCb.Size = new System.Drawing.Size(114, 21);
            this.portCb.TabIndex = 0;
            this.portCb.DropDown += new System.EventHandler(this.portCb_DropDown);
            // 
            // openCloseBtn
            // 
            this.openCloseBtn.Location = new System.Drawing.Point(132, 22);
            this.openCloseBtn.Name = "openCloseBtn";
            this.openCloseBtn.Size = new System.Drawing.Size(75, 23);
            this.openCloseBtn.TabIndex = 1;
            this.openCloseBtn.Text = "Open";
            this.openCloseBtn.UseVisualStyleBackColor = true;
            this.openCloseBtn.Click += new System.EventHandler(this.openCloseBtn_Click);
            // 
            // startBtn
            // 
            this.startBtn.Location = new System.Drawing.Point(213, 22);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(75, 23);
            this.startBtn.TabIndex = 2;
            this.startBtn.Text = "Start";
            this.startBtn.UseVisualStyleBackColor = true;
            this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 65);
            this.Controls.Add(this.startBtn);
            this.Controls.Add(this.openCloseBtn);
            this.Controls.Add(this.portCb);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox portCb;
        private System.Windows.Forms.Button openCloseBtn;
        private System.Windows.Forms.Button startBtn;
    }
}

