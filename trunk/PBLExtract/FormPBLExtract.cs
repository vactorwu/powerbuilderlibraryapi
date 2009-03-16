using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using PowerBuilder;

namespace PBLExtract
{
    public partial class FormPBLExtract : Form
    {
        public FormPBLExtract()
        {
            InitializeComponent();
        }

        private void buttonPickPBL_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.textBoxPBLFile.Text = openFileDialog.FileName;
            }
        }

        private void buttonPickFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                this.textBoxFolderPath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void buttonExtract_Click(object sender, EventArgs e)
        {
            if (textBoxPBLFile.Text.Trim().Length > 0)
            {
                    ExtractPBL(new FileInfo(this.textBoxPBLFile.Text));
            }
            else if (textBoxFolderPath.Text.Trim().Length > 0)
            {
                DirectoryInfo di = new DirectoryInfo(textBoxFolderPath.Text);
                ExtractPBL(di);
            }
        }

        private void Write(string txt)
        {
            this.textBoxOutput.AppendText(txt);
            this.textBoxOutput.Refresh();
        }

        private void WriteLine(string txt)
        {
            this.textBoxOutput.AppendText(txt);
            this.textBoxOutput.AppendText(Environment.NewLine);

            this.textBoxOutput.Refresh();
            Application.DoEvents();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void ExtractPBL(DirectoryInfo parent)
        {
            foreach (FileInfo fi in parent.GetFiles("*.pbl"))
            {
                ExtractPBL(fi);
            }
            foreach (DirectoryInfo childFolder in parent.GetDirectories())
            {
                ExtractPBL(childFolder);
            }
        }

        public void ExtractPBL(FileInfo fi)
        {
            try
            {
                if (fi.Exists)
                {
                    string newDirName = fi.Directory.FullName + "\\" + fi.Name.Substring(0, (fi.Name.Length - fi.Extension.Length));

                    DirectoryInfo saveDir;
                    if (Directory.Exists(newDirName))
                        saveDir = new DirectoryInfo(newDirName);
                    else
                        saveDir = Directory.CreateDirectory(newDirName);

                    PBLFile pblFile = PBLParser.Parse(fi.FullName);

                    WriteLine("File: " + fi.Name);
                    WriteLine("Description: " + pblFile.Header.Description);
                    WriteLine("PBL Format Version: " + pblFile.Header.Version);
                    WriteLine("Is Unicode? " + pblFile.IsUnicode);

                    foreach (PBLNodeEntry entry in pblFile.Node.Entries.Values)
                    {
                        WriteLine("------------------------------------");
                        WriteLine("Object: " + entry.ObjectName);
                        WriteLine("Comment: " + entry.Comment);

                        string fname;
                        if (entry.ObjectName.IndexOf(".sr") != -1)
                        {
                            Encoding enc = pblFile.IsUnicode ? Encoding.Unicode : Encoding.ASCII;

                            fname = saveDir.FullName + "\\" + entry.ObjectName + ".txt";

                            File.WriteAllText(
                                fname,
                                enc.GetString(entry.RawData), enc);
                        }
                        else
                        {
                            fname = saveDir.FullName + "\\" + entry.ObjectName + ".bin";

                            File.WriteAllBytes(
                                fname,
                                entry.RawData);
                        }

                        WriteLine("Saving object to file: " + fname);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
                WriteLine(ex.StackTrace);
            }
        }
    }
}
