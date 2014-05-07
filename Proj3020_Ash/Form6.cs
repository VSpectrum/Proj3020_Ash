using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlServerCe;

namespace Proj3020_Ash
{
    public partial class Form6 : Form
    {
        System.Data.SqlServerCe.SqlCeConnection con;
        System.Data.SqlServerCe.SqlCeDataAdapter daTrans;
        System.Data.SqlServerCe.SqlCeDataAdapter daModel;
        DataSet dsTrans; //data from TransformerNames will be stored here
        DataSet dsModel; //likewise for TransformerModel
        public Form6()
        {
            InitializeComponent();
            con = new System.Data.SqlServerCe.SqlCeConnection();
            dsTrans = new DataSet();
            dsModel = new DataSet();
            con.ConnectionString = "Data Source=TransformerData.sdf";

            con.Open();
            daTrans = new System.Data.SqlServerCe.SqlCeDataAdapter("SELECT * From TransformerDetails", con);
            daModel = new System.Data.SqlServerCe.SqlCeDataAdapter("SELECT * From TransformerModel", con);
            daTrans.Fill(dsTrans, "TransformerDetails");
            daModel.Fill(dsModel, "TransformerModel");
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog toLoadXML = new OpenFileDialog();
            toLoadXML.InitialDirectory = "";
            toLoadXML.Filter = "txt files (*.xls)|*.xls|(*.xml)|*xml";
            toLoadXML.FilterIndex = 2;
            toLoadXML.RestoreDirectory = true;
            String path = "";

            if (toLoadXML.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = toLoadXML.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            path = toLoadXML.FileName;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }

            // check columns of spreadsheet if it matches db to ensure 'good' data is being added -check unimplemented


            // add to dataset 
            if (path != "")
            {
                con.Open();

                //clear database before updating it with new values
                SqlCeCommand cmd = con.CreateCommand();
                cmd.CommandText = "DELETE FROM TransformerDetails";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM TransformerModel";
                cmd.ExecuteNonQuery();

                System.Data.SqlServerCe.SqlCeCommandBuilder cb;
                dsTrans.ReadXml(path);
                // update db from dataset obtained from xml file
                cb = new System.Data.SqlServerCe.SqlCeCommandBuilder(daTrans);
                cb.DataAdapter.Update(dsTrans.Tables["TransformerDetails"]);


                //below is to update the TransformerModel table with a unique list of names of transf
                //dsModel.Clear();
                HashSet<String> TransformerNameList = new HashSet<String>();
                foreach (DataRow dr in dsTrans.Tables["TransformerDetails"].Rows)
                {
                    TransformerNameList.Add(dr["TransformerName"].ToString());
                }


                DataRow drow;

                foreach (String TransformerName in TransformerNameList)
                {
                    drow = dsModel.Tables["TransformerModel"].NewRow();
                    drow[1] = TransformerName;
                    dsModel.Tables["TransformerModel"].Rows.Add(drow);
                    //MessageBox.Show(TransformerName);
                }

                System.Data.SqlServerCe.SqlCeCommandBuilder cb1;
                cb1 = new System.Data.SqlServerCe.SqlCeCommandBuilder(daModel);
                cb1.DataAdapter.Update(dsModel.Tables["TransformerModel"]);

                MessageBox.Show("Database Updated.");

                con.Close();
            }
            else MessageBox.Show("Database not updated.");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // export dataset dsTrans to excel sheet
            dsTrans.WriteXml("test.xml");
            MessageBox.Show("Database Exported to XML file. XML file can be opened and edited with MS Excel!");
        }
    }
}