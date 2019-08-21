using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AcadUtils
{
    public partial class InsertBorderForm : Form
    {
        public string BorderName = string.Empty;

        public InsertBorderForm()
        {
            InitializeComponent();
            BorderName = "A3_Border";
            Boolean Filled = FillListBox();
            if (!Filled)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            BorderName = listBox1.SelectedItem.ToString();
        }

        private Boolean FillListBox()
        {
            Boolean ListBoxFilled = false;
            try
            {
                
                listBox1.Items.Clear();
                if (DwgUtils.Borders != null)
                {
                    foreach (string s in DwgUtils.Borders)
                    {
                        listBox1.Items.Add(s);
                        listBox1.EndUpdate();
                        ListBoxFilled = true;
                    }
                }
                else
                {
                    Helper.InfoMessageBox("There was an error retreiving the border names. The program cannot continue.");
                    throw new Exception("There was an error retreiving the border names. The program cannot continue.");
                }
                return ListBoxFilled;
            }
            catch (Exception)
            {
                return ListBoxFilled;
            }
        }

        private void CancelButton1_Click(object sender, EventArgs e)
        {

        }
    }
}
