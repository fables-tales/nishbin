namespace libnish.Debug
{
    partial class NetWatcher
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPushGeneral = new System.Windows.Forms.TabPage();
            this.tabBlacklist = new System.Windows.Forms.TabPage();
            this.tabPushComms = new System.Windows.Forms.TabPage();
            this.tabTrapdoorComms = new System.Windows.Forms.TabPage();
            this.tabTrapdoorGeneral = new System.Windows.Forms.TabPage();
            this.tabInMemChunkCache = new System.Windows.Forms.TabPage();
            this.tabOnDiskChunkCache = new System.Windows.Forms.TabPage();
            this.tabOverview = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabOverview.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabOverview);
            this.tabControl1.Controls.Add(this.tabBlacklist);
            this.tabControl1.Controls.Add(this.tabPushGeneral);
            this.tabControl1.Controls.Add(this.tabPushComms);
            this.tabControl1.Controls.Add(this.tabTrapdoorGeneral);
            this.tabControl1.Controls.Add(this.tabTrapdoorComms);
            this.tabControl1.Controls.Add(this.tabInMemChunkCache);
            this.tabControl1.Controls.Add(this.tabOnDiskChunkCache);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(756, 413);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPushGeneral
            // 
            this.tabPushGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabPushGeneral.Name = "tabPushGeneral";
            this.tabPushGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPushGeneral.Size = new System.Drawing.Size(677, 475);
            this.tabPushGeneral.TabIndex = 0;
            this.tabPushGeneral.Text = "Push (General)";
            this.tabPushGeneral.UseVisualStyleBackColor = true;
            // 
            // tabBlacklist
            // 
            this.tabBlacklist.Location = new System.Drawing.Point(4, 22);
            this.tabBlacklist.Name = "tabBlacklist";
            this.tabBlacklist.Padding = new System.Windows.Forms.Padding(3);
            this.tabBlacklist.Size = new System.Drawing.Size(748, 387);
            this.tabBlacklist.TabIndex = 1;
            this.tabBlacklist.Text = "Blacklist";
            this.tabBlacklist.UseVisualStyleBackColor = true;
            // 
            // tabPushComms
            // 
            this.tabPushComms.Location = new System.Drawing.Point(4, 22);
            this.tabPushComms.Name = "tabPushComms";
            this.tabPushComms.Size = new System.Drawing.Size(677, 475);
            this.tabPushComms.TabIndex = 2;
            this.tabPushComms.Text = "Push Comms";
            this.tabPushComms.UseVisualStyleBackColor = true;
            // 
            // tabTrapdoorComms
            // 
            this.tabTrapdoorComms.Location = new System.Drawing.Point(4, 22);
            this.tabTrapdoorComms.Name = "tabTrapdoorComms";
            this.tabTrapdoorComms.Size = new System.Drawing.Size(677, 475);
            this.tabTrapdoorComms.TabIndex = 3;
            this.tabTrapdoorComms.Text = "Trapdoor Comms";
            this.tabTrapdoorComms.UseVisualStyleBackColor = true;
            // 
            // tabTrapdoorGeneral
            // 
            this.tabTrapdoorGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabTrapdoorGeneral.Name = "tabTrapdoorGeneral";
            this.tabTrapdoorGeneral.Size = new System.Drawing.Size(677, 475);
            this.tabTrapdoorGeneral.TabIndex = 4;
            this.tabTrapdoorGeneral.Text = "Trapdoor (General)";
            this.tabTrapdoorGeneral.UseVisualStyleBackColor = true;
            // 
            // tabInMemChunkCache
            // 
            this.tabInMemChunkCache.Location = new System.Drawing.Point(4, 22);
            this.tabInMemChunkCache.Name = "tabInMemChunkCache";
            this.tabInMemChunkCache.Size = new System.Drawing.Size(677, 475);
            this.tabInMemChunkCache.TabIndex = 5;
            this.tabInMemChunkCache.Text = "In-Mem Chunk Cache";
            this.tabInMemChunkCache.UseVisualStyleBackColor = true;
            // 
            // tabOnDiskChunkCache
            // 
            this.tabOnDiskChunkCache.Location = new System.Drawing.Point(4, 22);
            this.tabOnDiskChunkCache.Name = "tabOnDiskChunkCache";
            this.tabOnDiskChunkCache.Size = new System.Drawing.Size(677, 475);
            this.tabOnDiskChunkCache.TabIndex = 6;
            this.tabOnDiskChunkCache.Text = "On-Disk Chunk Cache";
            this.tabOnDiskChunkCache.UseVisualStyleBackColor = true;
            // 
            // tabOverview
            // 
            this.tabOverview.Controls.Add(this.label1);
            this.tabOverview.Location = new System.Drawing.Point(4, 22);
            this.tabOverview.Name = "tabOverview";
            this.tabOverview.Size = new System.Drawing.Size(748, 387);
            this.tabOverview.TabIndex = 7;
            this.tabOverview.Text = "Overview";
            this.tabOverview.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(492, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "This is the super secret (like, super squirrel super secret) debug view for the l" +
                "ib.  This is NOT a program.";
            // 
            // NetWatcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 437);
            this.Controls.Add(this.tabControl1);
            this.Name = "NetWatcher";
            this.Text = "NetWatcher";
            this.tabControl1.ResumeLayout(false);
            this.tabOverview.ResumeLayout(false);
            this.tabOverview.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPushGeneral;
        private System.Windows.Forms.TabPage tabBlacklist;
        private System.Windows.Forms.TabPage tabPushComms;
        private System.Windows.Forms.TabPage tabTrapdoorComms;
        private System.Windows.Forms.TabPage tabTrapdoorGeneral;
        private System.Windows.Forms.TabPage tabOverview;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabInMemChunkCache;
        private System.Windows.Forms.TabPage tabOnDiskChunkCache;
    }
}