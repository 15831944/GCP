using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace TextReplace
{
    public partial class AddText : Form
    {
        public string ATBstring { get; set; }

        public AddText()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Regex myexp = new Regex("([A-Za-z0-9][:][A-Za-z0-9])", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection MatchList = myexp.Matches(AddTextBox1.Text);
                if (MatchList.Count == 1)
                {
                    ATBstring = AddTextBox1.Text;
                }
                else
                {
                    MessageBox.Show("The text string '" + AddTextBox1.Text + "' is invalid");
                    ATBstring = string.Empty;
                }
            }
            catch (Exception)
            {

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
