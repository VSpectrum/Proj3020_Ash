using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlServerCe;

namespace Proj3020_Ash
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        System.Data.SqlServerCe.SqlCeConnection con;
        System.Data.SqlServerCe.SqlCeDataAdapter daTrans;
        DataSet dsTrans; //data from sql table stored here
        DataRow drow;
        List<string> TransformerNames = new List<string>();

        private void Form4_Load(object sender, EventArgs e)
        {
            con = new System.Data.SqlServerCe.SqlCeConnection();
            dsTrans = new DataSet();
            con.ConnectionString = "Data Source=TransformerData.sdf";
            con.Open();
            daTrans = new System.Data.SqlServerCe.SqlCeDataAdapter("SELECT * From TransformerModel", con);
            daTrans.Fill(dsTrans, "TransformerModel");
            for (int i = 0; i < dsTrans.Tables["TransformerModel"].Rows.Count; ++i)
            {
                drow = dsTrans.Tables["TransformerModel"].Rows[i]; //filling our lists with the products
                TransformerNames.Add(drow[1].ToString());
            }
            TransformerNames.Add("All Transformers");
            comboBox1.DataSource = TransformerNames;
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                drow = dsTrans.Tables["TransformerModel"].NewRow();
                drow[1] = textBox1.Text;

                dsTrans.Tables["TransformerModel"].Rows.Add(drow);

                con.Open();
                System.Data.SqlServerCe.SqlCeCommandBuilder cb;
                cb = new System.Data.SqlServerCe.SqlCeCommandBuilder(daTrans);
                cb.DataAdapter.Update(dsTrans.Tables["TransformerModel"]);
                con.Close();

                TransformerNames.Clear();
                comboBox1.DataSource = null;
                comboBox1.Items.Clear();

                for (int i = 0; i < dsTrans.Tables["TransformerModel"].Rows.Count; ++i)
                {
                    drow = dsTrans.Tables["TransformerModel"].Rows[i];
                    TransformerNames.Add(drow[1].ToString());
                }

                comboBox1.DataSource = TransformerNames;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("Do you really want to delete the chosen transformers?", "Confirm transformer deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                string selectedTransformer = comboBox1.Text;

                con.Open();
                SqlCeCommand cmd = con.CreateCommand();
                if (selectedTransformer == "All Transformers")
                {
                    cmd.CommandText = "DELETE FROM TransformerModel";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "DELETE FROM TransformerDetails";
                    cmd.ExecuteNonQuery();
                }

                else
                {
                    cmd.CommandText = "DELETE FROM TransformerModel WHERE TransName = '" + selectedTransformer + "'";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "DELETE FROM TransformerDetails WHERE TransformerName = '" + selectedTransformer + "'";
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}