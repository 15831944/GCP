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
    public partial class Addreference : Form
    {
        public string DwgNumbers = string.Empty;

        public Addreference()
        {
            InitializeComponent();
        }

        private void Addreference_Load(object sender, EventArgs e)
        {

        }

        private void button1_OK_Click(object sender, EventArgs e)
        {
            DwgNumbers = textBox1.Text;

         }

        private void button2_Cancel_Click(object sender, EventArgs e)
        {

        }
    }
}
