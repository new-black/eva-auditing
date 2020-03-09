namespace AuditingApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbFicalArchiveURL = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbPublicKeyURL = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnLoad = new System.Windows.Forms.Button();
            this.tbProgress = new System.Windows.Forms.TextBox();
            this.lblProgress = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSaveAndClose = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbFicalArchiveURL
            // 
            this.tbFicalArchiveURL.Location = new System.Drawing.Point(119, 6);
            this.tbFicalArchiveURL.Name = "tbFicalArchiveURL";
            this.tbFicalArchiveURL.Size = new System.Drawing.Size(447, 23);
            this.tbFicalArchiveURL.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Archive fiscal URL";
            // 
            // tbPublicKeyURL
            // 
            this.tbPublicKeyURL.Location = new System.Drawing.Point(119, 35);
            this.tbPublicKeyURL.Name = "tbPublicKeyURL";
            this.tbPublicKeyURL.Size = new System.Drawing.Size(447, 23);
            this.tbPublicKeyURL.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Public key url";
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(453, 64);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(113, 23);
            this.btnLoad.TabIndex = 3;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbProgress
            // 
            this.tbProgress.HideSelection = false;
            this.tbProgress.Location = new System.Drawing.Point(13, 138);
            this.tbProgress.Multiline = true;
            this.tbProgress.Name = "tbProgress";
            this.tbProgress.ReadOnly = true;
            this.tbProgress.ShortcutsEnabled = false;
            this.tbProgress.Size = new System.Drawing.Size(553, 146);
            this.tbProgress.TabIndex = 0;
            this.tbProgress.TabStop = false;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(13, 109);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(52, 15);
            this.lblProgress.TabIndex = 0;
            this.lblProgress.Text = "Progress";
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(302, 290);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSaveAndClose
            // 
            this.btnSaveAndClose.Enabled = false;
            this.btnSaveAndClose.Location = new System.Drawing.Point(383, 290);
            this.btnSaveAndClose.Name = "btnSaveAndClose";
            this.btnSaveAndClose.Size = new System.Drawing.Size(102, 23);
            this.btnSaveAndClose.TabIndex = 5;
            this.btnSaveAndClose.Text = "Save and close";
            this.btnSaveAndClose.UseVisualStyleBackColor = true;
            this.btnSaveAndClose.Click += new System.EventHandler(this.btnSaveAndClose_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(491, 290);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 322);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnSaveAndClose);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbPublicKeyURL);
            this.Controls.Add(this.tbFicalArchiveURL);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.tbProgress);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbFicalArchiveURL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPublicKeyURL;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.TextBox tbProgress;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnSaveAndClose;
        private System.Windows.Forms.Button btnClose;
    }
}

