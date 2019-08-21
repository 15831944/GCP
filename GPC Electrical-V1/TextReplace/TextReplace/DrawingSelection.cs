using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TextReplace
{
    public partial class DrawingSelection : Form
    {
        public string DWGFolderPath { get; set; }
        public string CompletedPath { get; set; }
        public string ErrorPath { get; set; }
        public string DataPath { get; set; }
        public string FilePath { get; set; }

        public ListBox.ObjectCollection lbo { get; set; }
        public ListBox.ObjectCollection tlbo { get; set; }


        public DrawingSelection()
        {
            InitializeComponent();
        }

        private void Select_Text_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog2 = new OpenFileDialog();
                openFileDialog2.Title = " Select the Data file";
                openFileDialog2.Filter = "Text Files(.txt)|*.txt";
                openFileDialog2.Multiselect = false;
                System.Windows.Forms.DialogResult result = openFileDialog2.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    TextFilePath.Text = openFileDialog2.FileName;
                    string[] lines = System.IO.File.ReadAllLines(openFileDialog2.FileName);

                    foreach (string fname in lines)
                    {
                        if (listBox1.FindStringExact(fname, 0) == -1)
                        {
                            listBox1.Items.Add(fname);
                        }
                    }

                }
            }
            catch (Exception)
            {

            }

        }

        private void OK_But_Click(object sender, EventArgs e)
        {
            try
            {
                DWGFolderPath = FolderPath.Text;
                FilePath = TextFilePath.Text;
                tlbo = listBox1.Items; //Text strings
                lbo = listBox2.Items; //Drawings
            }
            catch (Exception)
            {

            }

        }

        private void deleteSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                while (listBox1.SelectedItem != null)
                {
                    listBox1.Items.Remove(listBox1.SelectedItem);

                }
            }
            catch (Exception)
            {

            }

        }

        private void addTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                AddText AText = new AddText();
                System.Windows.Forms.DialogResult dsResult = AText.ShowDialog();
                if (dsResult == System.Windows.Forms.DialogResult.OK)
                {
                    if (AText.ATBstring != string.Empty)
                    {
                        listBox1.Items.Add(AText.ATBstring);
                        AText.Close();
                    }
                }
                else
                {
                    AText.Close();
                }
            }
            catch (Exception)
            {

            }


        }

        private void Save_Button_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0)
            {

                TextWriter tw = null;
                saveFileDialog1.Title = "Select Folder and File Name";
                saveFileDialog1.Filter = "Text Files|*.txt";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {

                    tw = new StreamWriter(saveFileDialog1.FileName, false);
                    foreach (object li in listBox1.Items)
                    {
                        tw.WriteLine((string)li);
                    }
                    tw.Flush();
                    tw.Close();
                }
            }
            else
            {
                MessageBox.Show("The ListBox is empty");
            }

        }

        private void CancelBut_Click(object sender, EventArgs e)
        {

        }

        private void Select_Folder_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
                folderBrowserDialog1.Description = "Select the folder containing the drawings to be processed";
                System.Windows.Forms.DialogResult result = folderBrowserDialog1.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    FolderPath.Text = folderBrowserDialog1.SelectedPath;

                    FillListBox(FolderPath.Text);
                }
            }
            catch (Exception)
            {

            }

        }

        private void FillListBox(string path)
        {
            try
            {
                listBox2.Items.Clear();
                foreach (string s in Directory.GetFiles(path, "*.dwg"))
                {
                    listBox2.BeginUpdate();
                    listBox2.Items.Add(s.Substring(s.LastIndexOf(@"\") + 1));
                    listBox2.EndUpdate();
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
