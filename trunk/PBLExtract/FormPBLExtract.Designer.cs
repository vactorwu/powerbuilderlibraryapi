namespace PBLExtract
{
    partial class FormPBLExtract
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPBLExtract));
            this.buttonPickPBL = new System.Windows.Forms.Button();
            this.textBoxPBLFile = new System.Windows.Forms.TextBox();
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.buttonExit = new System.Windows.Forms.Button();
            this.buttonExtract = new System.Windows.Forms.Button();
            this.textBoxFolderPath = new System.Windows.Forms.TextBox();
            this.buttonPickFolder = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // buttonPickPBL
            // 
            this.buttonPickPBL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPickPBL.Location = new System.Drawing.Point(500, 12);
            this.buttonPickPBL.Name = "buttonPickPBL";
            this.buttonPickPBL.Size = new System.Drawing.Size(75, 23);
            this.buttonPickPBL.TabIndex = 0;
            this.buttonPickPBL.Text = "Pick PBL";
            this.buttonPickPBL.UseVisualStyleBackColor = true;
            this.buttonPickPBL.Click += new System.EventHandler(this.buttonPickPBL_Click);
            // 
            // textBoxPBLFile
            // 
            this.textBoxPBLFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPBLFile.Location = new System.Drawing.Point(12, 14);
            this.textBoxPBLFile.Name = "textBoxPBLFile";
            this.textBoxPBLFile.Size = new System.Drawing.Size(482, 20);
            this.textBoxPBLFile.TabIndex = 1;
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutput.BackColor = System.Drawing.Color.Black;
            this.textBoxOutput.ForeColor = System.Drawing.Color.PaleGoldenrod;
            this.textBoxOutput.Location = new System.Drawing.Point(12, 66);
            this.textBoxOutput.MaxLength = 999999;
            this.textBoxOutput.Multiline = true;
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxOutput.Size = new System.Drawing.Size(563, 378);
            this.textBoxOutput.TabIndex = 2;
            this.textBoxOutput.WordWrap = false;
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "pbl";
            this.openFileDialog.Filter = "PowerBuilder Libraries|*.pbl|All Files|*.*";
            this.openFileDialog.Title = "Pick PowerBuilder Library";
            // 
            // buttonExit
            // 
            this.buttonExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExit.Location = new System.Drawing.Point(500, 450);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(75, 23);
            this.buttonExit.TabIndex = 3;
            this.buttonExit.Text = "Exit";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // buttonExtract
            // 
            this.buttonExtract.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExtract.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.buttonExtract.FlatAppearance.BorderColor = System.Drawing.Color.Green;
            this.buttonExtract.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lime;
            this.buttonExtract.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.buttonExtract.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtract.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonExtract.Location = new System.Drawing.Point(399, 450);
            this.buttonExtract.Name = "buttonExtract";
            this.buttonExtract.Size = new System.Drawing.Size(95, 23);
            this.buttonExtract.TabIndex = 4;
            this.buttonExtract.Text = "Extract Files";
            this.buttonExtract.UseVisualStyleBackColor = false;
            this.buttonExtract.Click += new System.EventHandler(this.buttonExtract_Click);
            // 
            // textBoxFolderPath
            // 
            this.textBoxFolderPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFolderPath.Location = new System.Drawing.Point(12, 40);
            this.textBoxFolderPath.Name = "textBoxFolderPath";
            this.textBoxFolderPath.Size = new System.Drawing.Size(482, 20);
            this.textBoxFolderPath.TabIndex = 6;
            // 
            // buttonPickFolder
            // 
            this.buttonPickFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPickFolder.Location = new System.Drawing.Point(500, 38);
            this.buttonPickFolder.Name = "buttonPickFolder";
            this.buttonPickFolder.Size = new System.Drawing.Size(75, 23);
            this.buttonPickFolder.TabIndex = 5;
            this.buttonPickFolder.Text = "Pick Folder";
            this.buttonPickFolder.UseVisualStyleBackColor = true;
            this.buttonPickFolder.Click += new System.EventHandler(this.buttonPickFolder_Click);
            // 
            // FormPBLExtract
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 485);
            this.Controls.Add(this.textBoxFolderPath);
            this.Controls.Add(this.buttonPickFolder);
            this.Controls.Add(this.buttonExtract);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.textBoxOutput);
            this.Controls.Add(this.textBoxPBLFile);
            this.Controls.Add(this.buttonPickPBL);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormPBLExtract";
            this.Text = "Extract PowerBuilder Library Objects";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonPickPBL;
        private System.Windows.Forms.TextBox textBoxPBLFile;
        private System.Windows.Forms.TextBox textBoxOutput;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Button buttonExtract;
        private System.Windows.Forms.TextBox textBoxFolderPath;
        private System.Windows.Forms.Button buttonPickFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}

