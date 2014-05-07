using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Proj3020_Ash
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 dgaTest = new Form2();
            dgaTest.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 transformerform = new Form3();
            //if (transformerform.Visible) transformerform.Hide();
            //else 
            transformerform.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form4 AddRemTrans = new Form4();
            AddRemTrans.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form6 spreadsheetform = new Form6();
            spreadsheetform.Show();
        }
    }
}
