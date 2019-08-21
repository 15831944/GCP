﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Upload2SP
{
    public partial class Banner_Form : Form
    {

        public ListBox.ObjectCollection lbo { get; set; }


        public Banner_Form()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string OpenMessage = "Select drawing files to process";
                Autodesk.AutoCAD.Windows.OpenFileDialog ofd = new Autodesk.AutoCAD.Windows.OpenFileDialog(OpenMessage, null, "dwg", "FileToProcess", Autodesk.AutoCAD.Windows.OpenFileDialog.OpenFileDialogFlags.AllowMultiple);
                System.Windows.Forms.DialogResult dr = ofd.ShowDialog();

                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    string[] DrawingList = ofd.GetFilenames();
                    foreach (string fname in DrawingList)
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

        private void LoadFileBtn_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog2 = new OpenFileDialog();
                openFileDialog2.Title = " Select the Data file";
                openFileDialog2.Filter = "Text Files(.txt)|*.txt|DSD Files(.dsd)|*.dsd";
                openFileDialog2.Multiselect = false;
                System.Windows.Forms.DialogResult result = openFileDialog2.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    string[] lines = System.IO.File.ReadAllLines(openFileDialog2.FileName);
                    if (Path.GetExtension(openFileDialog2.FileName) == ".dsd")
                    {
                        foreach (string fname in lines)
                        {
                            if (fname.Substring(0, 4) == "DWG=")
                            {
                                if (listBox1.FindStringExact(fname.Substring(4), 0) == -1)
                                {
                                    listBox1.Items.Add(fname.Substring(4));
                                }
                            }
                        }
                    }
                    else
                    {

                        foreach (string fname in lines)
                        {
                            if (listBox1.FindStringExact(fname, 0) == -1)
                            {
                                listBox1.Items.Add(fname);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

        }

        private void SaveFileBtn_Click(object sender, EventArgs e)
        {
            TextWriter tw = null;
            try
            {
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
            catch (Exception)
            {

            }


        }


        private void deleteDrawingToolStripMenuItem_Click(object sender, EventArgs e)
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


        private void OK_But_Click(object sender, EventArgs e)
        {
            try
            {
                lbo = listBox1.Items;

            }
            catch (Exception)
            {

            }

        }
    }
}
