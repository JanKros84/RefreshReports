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
            this.SuspendLayout();
            // 
            // btnStartRefresh
            // 
            this.btnStartRefresh.Location = new System.Drawing.Point(101, 59);
            this.btnStartRefresh.Name = "btnStartRefresh";
            this.btnStartRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnStartRefresh.TabIndex = 0;
            this.btnStartRefresh.Text = "Štart refresh";
            this.btnStartRefresh.UseVisualStyleBackColor = true;
            this.btnStartRefresh.Click += new System.EventHandler(this.btnStartRefresh_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.Location = new System.Drawing.Point(13, 13);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(260, 41);
            this.lblInfo.TabIndex = 1;
            this.lblInfo.Text = "Začnite stlačením tlačidla \"Start refresh\"";
            // 
            // RefreshReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(276, 92);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.btnStartRefresh);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RefreshReports";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Refresh reportov";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStartRefresh;
        private System.Windows.Forms.Label lblInfo;
    }
}

