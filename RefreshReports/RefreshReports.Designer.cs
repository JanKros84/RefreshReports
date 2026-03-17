namespace RefreshReports
{
    partial class RefreshReports
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RefreshReports));
            this.btnStartRefresh = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.btnStopRefresh = new System.Windows.Forms.Button();
            this.lblVersion = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnStartRefresh
            // 
            this.btnStartRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnStartRefresh.Image")));
            this.btnStartRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStartRefresh.Location = new System.Drawing.Point(43, 55);
            this.btnStartRefresh.Name = "btnStartRefresh";
            this.btnStartRefresh.Size = new System.Drawing.Size(91, 23);
            this.btnStartRefresh.TabIndex = 0;
            this.btnStartRefresh.Tag = "1";
            this.btnStartRefresh.Text = "Štart refresh";
            this.btnStartRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnStartRefresh.UseVisualStyleBackColor = false;
            this.btnStartRefresh.Click += new System.EventHandler(this.btnStartRefresh_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.Location = new System.Drawing.Point(13, 8);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(267, 41);
            this.lblInfo.TabIndex = 1;
            this.lblInfo.Text = "Začnite stlačením tlačidla \"Start refresh\"";
            // 
            // btnStopRefresh
            // 
            this.btnStopRefresh.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.btnStopRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnStopRefresh.Image")));
            this.btnStopRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStopRefresh.Location = new System.Drawing.Point(140, 55);
            this.btnStopRefresh.Name = "btnStopRefresh";
            this.btnStopRefresh.Size = new System.Drawing.Size(91, 23);
            this.btnStopRefresh.TabIndex = 2;
            this.btnStopRefresh.Tag = "2";
            this.btnStopRefresh.Text = "Stop refresh";
            this.btnStopRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnStopRefresh.UseVisualStyleBackColor = false;
            this.btnStopRefresh.Click += new System.EventHandler(this.btnStopRefresh_Click);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblVersion.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblVersion.Location = new System.Drawing.Point(101, 80);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(60, 12);
            this.lblVersion.TabIndex = 3;
            this.lblVersion.Text = "1.0.0.0KROS";
            // 
            // RefreshReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 94);
            this.Controls.Add(this.btnStartRefresh);
            this.Controls.Add(this.btnStopRefresh);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RefreshReports";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Refresh reportov";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RefreshReports_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartRefresh;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Button btnStopRefresh;
        private System.Windows.Forms.Label lblVersion;
    }
}

