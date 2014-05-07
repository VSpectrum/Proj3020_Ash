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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        System.Data.SqlServerCe.SqlCeConnection con;
        System.Data.SqlServerCe.SqlCeDataAdapter daTrans, daTransDetails;
        DataSet dsTrans, dsTransDetails; //data from sql table stored here
        DataRow drow;
        List<string> TransformerNames = new List<string>();


        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Calculate and store to database procedure
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "")
            {
                double H2 = Convert.ToDouble(textBox1.Text);
                double CH4 = Convert.ToDouble(textBox2.Text);
                double C2H2 = Convert.ToDouble(textBox3.Text);
                double C2H4 = Convert.ToDouble(textBox4.Text);
                double C2H6 = Convert.ToDouble(textBox5.Text);

                double CH4toH2 = CH4 / H2;
                double C2H2toC2H4 = C2H2 / C2H4;
                double C2H2toCH4 = C2H2 / CH4;
                double C2H6toC2H2 = C2H6 / C2H2;
                double C2H6toCH4 = C2H6 / CH4;
                double C2H4toC2H6 = C2H4 / C2H6;

                string selectedTransformer = comboBox1.Text;

                drow = dsTransDetails.Tables["TransformerDetails"].NewRow();

                drow[1] = selectedTransformer;
                drow[2] = (float)H2;
                drow[3] = (float)CH4;
                drow[4] = (float)C2H2;
                drow[5] = (float)C2H4;
                drow[6] = (float)C2H6;
                drow[7] = (float)CH4toH2;
                drow[8] = (float)C2H2toC2H4;
                drow[9] = (float)C2H2toCH4;
                drow[10] = (float)C2H6toC2H2;
                drow[11] = (float)C2H6toCH4;
                drow[12] = (float)C2H4toC2H6;
                drow[13] = dateTimePicker1.Value;

                dsTransDetails.Tables["TransformerDetails"].Rows.Add(drow);

                //connect to database, create dataset and datarow, update datarow, update dataset.
                con.Open();
                System.Data.SqlServerCe.SqlCeCommandBuilder cb;
                cb = new System.Data.SqlServerCe.SqlCeCommandBuilder(daTransDetails);
                cb.DataAdapter.Update(dsTransDetails.Tables["TransformerDetails"]);
                con.Close();
                MessageBox.Show("Database Updated.");
                this.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            con = new System.Data.SqlServerCe.SqlCeConnection();

            dsTrans = new DataSet();
            dsTransDetails = new DataSet();

            con.ConnectionString = "Data Source=TransformerData.sdf";
            con.Open();
            daTrans = new System.Data.SqlServerCe.SqlCeDataAdapter("SELECT * From TransformerModel", con);
            daTrans.Fill(dsTrans, "TransformerModel");
            
            daTransDetails = new System.Data.SqlServerCe.SqlCeDataAdapter("SELECT * From TransformerDetails", con);
            daTransDetails.Fill(dsTransDetails, "TransformerDetails");

            for (int i = 0; i < dsTrans.Tables["TransformerModel"].Rows.Count; ++i)
            {
                drow = dsTrans.Tables["TransformerModel"].Rows[i]; //filling our lists with the products
                TransformerNames.Add(drow[1].ToString());
            }
            comboBox1.DataSource = TransformerNames;
            con.Close();
        }
    }
}
