using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Proj3020_Ash
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            comboBox2.Items.Add("H2");
            comboBox2.Items.Add("CH4");
            comboBox2.Items.Add("C2H2");
            comboBox2.Items.Add("C2H4");
            comboBox2.Items.Add("C2H6");
            comboBox2.Items.Add("CH4 to H2");
            comboBox2.Items.Add("C2H2 to C2H4");
            comboBox2.Items.Add("C2H2 to CH4");
            comboBox2.Items.Add("C2H4 to C2H6");
            comboBox2.Items.Add("C2H6 to C2H2");
            comboBox2.Items.Add("C2H6 to CH4");

            comboBox4.Items.Add("Duval Triangle 1");
            comboBox4.Items.Add("Duval Triangle 2a");
            comboBox4.Items.Add("Duval Triangle 2b");
            comboBox4.Items.Add("Duval Triangle 2c");
            comboBox4.Items.Add("Duval Triangle 2d");
            comboBox4.Items.Add("Duval Triangle 2e");
            comboBox4.Items.Add("Duval Triangle 4");
            comboBox4.Items.Add("Duval Triangle 5");

            Fieldarr["H2"] = 2; //index of column 'H2'
            Fieldarr["CH4"] = 3;
            Fieldarr["C2H2"] = 4;
            Fieldarr["C2H4"] = 5;
            Fieldarr["C2H6"] = 6;
            Fieldarr["CH4 to H2"] = 7;
            Fieldarr["C2H2 to C2H4"] = 8;
            Fieldarr["C2H2 to CH4"] = 9;
            Fieldarr["C2H6 to C2H2"] = 10;
            Fieldarr["C2H6 to CH4"] = 11;
            Fieldarr["C2H4 to C2H6"] = 12;
        }

        Dictionary<String, int> Fieldarr = new Dictionary<String, int>(); //fields into indexes

        System.Data.SqlServerCe.SqlCeConnection con;
        System.Data.SqlServerCe.SqlCeDataAdapter daTrans;
        DataSet dsTrans; //data from sql table stored here
        DataRow drow;
        List<string> TransformerNames = new List<string>();
        Dictionary<DateTime, double[] > DateInfMap = new Dictionary<DateTime, double[] >();


        private void Form3_Load(object sender, EventArgs e)
        {
            con = new System.Data.SqlServerCe.SqlCeConnection();
            dsTrans = new DataSet();
            con.ConnectionString = "Data Source=TransformerData.sdf";
            
            con.Open();
            daTrans = new System.Data.SqlServerCe.SqlCeDataAdapter("SELECT * From TransformerModel", con);
            daTrans.Fill(dsTrans, "TransformerModel");
            con.Close();

            for (int i = 0; i < dsTrans.Tables["TransformerModel"].Rows.Count; ++i)
            {
                drow = dsTrans.Tables["TransformerModel"].Rows[i]; //filling our lists with the products
                TransformerNames.Add(drow[1].ToString());
            }
            comboBox1.DataSource = TransformerNames;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.DataSource = null;
            comboBox3.Items.Clear();
            DateInfMap.Clear();
            textBox1.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox11.Text = "";
            textBox10.Text = "";
            textBox9.Text = "";
            textBox8.Text = "";
            textBox15.Text = "";
            textBox14.Text = "";
            textBox12.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";

            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
            checkBox8.Checked = false;

            con = new System.Data.SqlServerCe.SqlCeConnection();
            dsTrans = new DataSet();
            con.ConnectionString = "Data Source=TransformerData.sdf";

            con.Open();
            daTrans = new System.Data.SqlServerCe.SqlCeDataAdapter("SELECT * From TransformerDetails", con);
            daTrans.Fill(dsTrans, "TransformerDetails");
            con.Close();

            for (int i = 0; i < dsTrans.Tables["TransformerDetails"].Rows.Count; ++i)
            {
                drow = dsTrans.Tables["TransformerDetails"].Rows[i]; //filling our lists with the products
                if (drow[1].ToString() == comboBox1.Text.ToString() )
                {
                    double[] arr = { System.Convert.ToDouble(drow[7]), System.Convert.ToDouble(drow[8]), System.Convert.ToDouble(drow[9]), System.Convert.ToDouble(drow[10]), System.Convert.ToDouble(drow[11]), System.Convert.ToDouble(drow[12]), System.Convert.ToDouble(drow[3]), System.Convert.ToDouble(drow[4]), System.Convert.ToDouble(drow[5]), System.Convert.ToDouble(drow[2]), System.Convert.ToDouble(drow[6])};
                    //CH4toH2 - C2H4toC2H6, CH4, C2H2, C2H4, H2, C2H6
                    DateInfMap[System.Convert.ToDateTime(drow[13])] = arr;
                }
            }
            comboBox3.DataSource = DateInfMap.Keys.ToList();

            comboBox2.SelectedIndex = 1;
            comboBox2.SelectedIndex = 0; //toggles change refreshing data
            comboBox4.SelectedIndex = 0;

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.Text!= "")
                if (DateInfMap.ContainsKey(Convert.ToDateTime(comboBox3.SelectedValue) )) //avoid preloaded key from previous dictionary from causing bugs
                {
                    double[] Inf = DateInfMap[Convert.ToDateTime(comboBox3.SelectedValue)];
                    textBox1.Text = System.Convert.ToString(Inf[0]);
                    textBox5.Text = System.Convert.ToString(Inf[1]);
                    textBox6.Text = System.Convert.ToString(Inf[2]);
                    textBox7.Text = System.Convert.ToString(Inf[3]);

                    textBox11.Text = System.Convert.ToString(Inf[0]);
                    textBox10.Text = System.Convert.ToString(Inf[1]);
                    textBox9.Text = System.Convert.ToString(Inf[5]);
                    textBox8.Text = System.Convert.ToString(Inf[4]);

                    textBox15.Text = System.Convert.ToString(Inf[0]);
                    textBox14.Text = System.Convert.ToString(Inf[1]);
                    textBox12.Text = System.Convert.ToString(Inf[5]);

                    textBox2.Text = System.Convert.ToString(Inf[6]);
                    textBox3.Text = System.Convert.ToString(Inf[7]);
                    textBox4.Text = System.Convert.ToString(Inf[8]);

                    if (Inf[0] > 1)
                    {
                        if (Inf[1] < 1)
                        {
                            if (Inf[5] < 1) checkBox6.Checked = true;
                               
                            else if (Inf[5] > 3) checkBox8.Checked = true;
                             
                            else checkBox7.Checked = true;
                            panel1.Refresh();///new refresh for checkbox problem
                        }
                        panel1.Refresh();///new refresh for checkbox problem
                    }
                    else if (Inf[0] < 0.1)
                    {
                        if(Inf[5] < 0.1)
                        {
                            if (Inf[1] < 0.1) checkBox4.Checked = true;
                                
                            else if (Inf[1] <= 3) checkBox5.Checked = true;
                       
                        }
                        panel1.Refresh();///new refresh for checkbox problem
                    }
                    else
                    {
                        if (Inf[5] > 3) 
                            if (Inf[1] >= 0.1 && Inf[1] <= 3) checkBox3.Checked = true;
                                
                        else if (Inf[5] > 1) if (Inf[1] > 0.1) checkBox2.Checked = true;
                        
                    }
    
                    panel1.Refresh();
                }
           
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            // plan to create new window showing larger version of chart
        }

        List<double> datapointsy = new List<double>(); //global lists for values plotting on chart
        List<DateTime> datapointsx = new List<DateTime>();

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            

            datapointsy.Clear(); // everytime combobox value is changed, the lists used to plot points are cleared.
            datapointsx.Clear();

            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }

            for (int i = 0; i < dsTrans.Tables["TransformerDetails"].Rows.Count; ++i)
            {
                drow = dsTrans.Tables["TransformerDetails"].Rows[i]; //filling our lists with the products
                if (drow[1].ToString() == comboBox1.Text.ToString())
                {
                    double point = (double)drow[Fieldarr[comboBox2.Text]];
                    if (!double.IsNaN(point) && !double.IsInfinity(point)) //check if the textbox has a value that is not NaN or Infinity
                    {
                        datapointsy.Add(point); //creating list of y values for plotting
                        datapointsx.Add(System.Convert.ToDateTime(drow[13]));
                    }//x axis is the corresponding date values
                }
            }

            chart1.Series["Series1"].MarkerStyle = MarkerStyle.Circle;
            chart1.Series["Series1"].LegendText = comboBox2.Text;
            chart1.Series["Series1"].XValueType = ChartValueType.DateTime;
            chart1.Series["Series1"].YValueType = ChartValueType.Double;

            DateTime minDate = DateTime.MaxValue;
            DateTime maxDate = DateTime.MinValue;
            foreach (DateTime date in datapointsx)
            {
                if (date < minDate)
                    minDate = date;
                if (date > maxDate)
                    maxDate = date;
            }

            chart1.ChartAreas[0].AxisX.Minimum = minDate.ToOADate();
            chart1.ChartAreas[0].AxisX.Maximum = maxDate.ToOADate();

            for (int i = 0; i < datapointsy.Count; i++)
            {
                chart1.Series["Series1"].Points.AddXY(datapointsx[i], datapointsy[i]);
            }
        }

        private void chart1_Click_1(object sender, EventArgs e)
        {
            //open chart in new, larger form
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Pen myPen = new Pen(Color.Blue, 1);
            SolidBrush redbrush = new SolidBrush(Color.PaleVioletRed);
            SolidBrush bluebrush = new SolidBrush(Color.LightBlue);
            SolidBrush greenbrush = new SolidBrush(Color.LightGreen);
            SolidBrush seagreenbrush = new SolidBrush(Color.LightSeaGreen);
            SolidBrush orangebrush = new SolidBrush(Color.Orange);
            SolidBrush brownbrush = new SolidBrush(Color.RosyBrown);
            SolidBrush crimsonbrush = new SolidBrush(Color.Crimson);
            SolidBrush pinkbrush = new SolidBrush(Color.LightPink);


            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            myPen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;

            if (comboBox4.Text == "Duval Triangle 1")
            {
                e.Graphics.FillPolygon(greenbrush, new PointF[] { new PointF(150, 0), new PointF(0, 259), new PointF(300, 259) });
                e.Graphics.FillPolygon(pinkbrush, new PointF[] { new PointF(0, 259), new PointF(131, 34), new PointF(166, 94), new PointF(71, 259) });
                e.Graphics.FillPolygon(crimsonbrush, new PointF[] { new PointF(71, 259), new PointF(166, 94), new PointF(188, 133), new PointF(163, 174), new PointF(213, 259) });
                e.Graphics.FillPolygon(brownbrush, new PointF[] { new PointF(213, 259), new PointF(163, 174), new PointF(188, 133), new PointF(131, 34), new PointF(144, 11), new PointF(219, 141), new PointF(203, 168), new PointF(255, 259) });
                e.Graphics.FillPolygon(orangebrush, new PointF[] { new PointF(147, 6), new PointF(150, 0), new PointF(153, 5) });
                e.Graphics.FillPolygon(seagreenbrush, new PointF[] { new PointF(153, 5), new PointF(147, 6), new PointF(144, 11), new PointF(174, 64), new PointF(181, 53) });
                e.Graphics.FillPolygon(orangebrush, new PointF[] { new PointF(181, 53), new PointF(174, 64), new PointF(219, 141), new PointF(225, 130) });
                e.Graphics.FillPolygon(orangebrush, new PointF[] { new PointF(225, 130), new PointF(203, 168), new PointF(255, 259), new PointF(300, 259) });
           
            }

            else if (comboBox4.Text == "Duval Triangle 2a")
            {
                e.Graphics.FillPolygon(greenbrush, new PointF[] { new PointF(150, 0), new PointF(0, 259), new PointF(300, 259) });
                e.Graphics.FillPolygon(pinkbrush, new PointF[] { new PointF(150, 0), new PointF(30, 209), new PointF(98, 209), new PointF(184, 60), new PointF(150, 0) });
                e.Graphics.FillPolygon(brownbrush, new PointF[] { new PointF(30, 209), new PointF(0, 259), new PointF(68, 259), new PointF(72, 251), new PointF(22, 251), new PointF(48, 209) });
                e.Graphics.FillPolygon(crimsonbrush, new PointF[] { new PointF(72, 251), new PointF(22, 251), new PointF(48, 209), new PointF(97, 209) });
                e.Graphics.FillPolygon(seagreenbrush, new PointF[] { new PointF(254, 259), new PointF(299, 259), new PointF(283, 232), new PointF(239, 233) });
                e.Graphics.FillPolygon(bluebrush, new PointF[] { new PointF(223, 233), new PointF(206, 206), new PointF(269, 206), new PointF(285, 233), new PointF(223, 233) });
                e.Graphics.FillPolygon(redbrush, new PointF[] { new PointF(269, 206), new PointF(224, 206), new PointF(201, 166), new PointF(224, 130), new PointF(269, 206) });
                e.Graphics.FillPolygon(orangebrush, new PointF[] { new PointF(161, 99), new PointF(184, 60), new PointF(224, 130), new PointF(201, 167), new PointF(161, 98) });
            }

            else if (comboBox4.Text == "Duval Triangle 2b")
            {
                e.Graphics.FillPolygon(greenbrush, new PointF[] { new PointF(150, 0), new PointF(0, 259), new PointF(300, 259) });
                e.Graphics.FillPolygon(pinkbrush, new PointF[] { new PointF(150,0), new PointF(30,209), new PointF(99,209), new PointF(184,63)});
                e.Graphics.FillPolygon(crimsonbrush, new PointF[] { new PointF(46,209), new PointF(22,251), new PointF(74,251), new PointF(99,209)});
                e.Graphics.FillPolygon(brownbrush, new PointF[] { new PointF(30,209), new PointF(48,209), new PointF(22,251), new PointF(74,251), new PointF(69,259), new PointF(0,259)});
                e.Graphics.FillPolygon(orangebrush, new PointF[] { new PointF(184,63), new PointF(163,98), new PointF(171,112), new PointF(180,96), new PointF(206,96)});
                e.Graphics.FillPolygon(seagreenbrush, new PointF[] { new PointF(180,96), new PointF(165,124), new PointF(177,146), new PointF(199,146), new PointF(218,117), new PointF(206,96)});
                e.Graphics.FillPolygon(bluebrush, new PointF[] { new PointF(192,146), new PointF(203,167), new PointF(225,130), new PointF(218,117), new PointF(199,146)});
                e.Graphics.FillPolygon(redbrush, new PointF[] { new PointF(203,167), new PointF(257,259), new PointF(300,259), new PointF(225,130)});
            }

            else if (comboBox4.Text == "Duval Triangle 2c")
            {
                e.Graphics.FillPolygon(greenbrush, new PointF[] { new PointF(150, 0), new PointF(0, 259), new PointF(300, 259) });
                e.Graphics.FillPolygon(pinkbrush, new PointF[] { new PointF(150, 0), new PointF(30, 209), new PointF(99, 209), new PointF(159,106), new PointF(153,99), new PointF(180,54) });
                e.Graphics.FillPolygon(crimsonbrush, new PointF[] { new PointF(46, 209), new PointF(22, 251), new PointF(74, 251), new PointF(99, 209) });
                e.Graphics.FillPolygon(brownbrush, new PointF[] { new PointF(30, 209), new PointF(48, 209), new PointF(22, 251), new PointF(74, 251), new PointF(69, 259), new PointF(0, 259) });
                e.Graphics.FillPolygon(orangebrush, new PointF[] { new PointF(180, 54), new PointF(153, 99), new PointF(175, 135), new PointF(202, 90) });
                e.Graphics.FillPolygon(seagreenbrush, new PointF[] { new PointF(181, 126), new PointF(204, 166), new PointF(226, 130), new PointF(202, 90) });
                e.Graphics.FillPolygon(bluebrush, new PointF[] { new PointF(203, 167), new PointF(257, 259), new PointF(300, 259), new PointF(226, 130) });
            }

            else if (comboBox4.Text == "Duval Triangle 2d") 
            {
                e.Graphics.FillPolygon(greenbrush, new PointF[] { new PointF(150, 0), new PointF(0, 259), new PointF(300, 259) });
                e.Graphics.FillPolygon(pinkbrush, new PointF[] { new PointF(150, 0), new PointF(30, 209), new PointF(99, 209), new PointF(184, 63) });
                e.Graphics.FillPolygon(crimsonbrush, new PointF[] { new PointF(46, 209), new PointF(22, 251), new PointF(74, 251), new PointF(99, 209) });
                e.Graphics.FillPolygon(brownbrush, new PointF[] { new PointF(30, 209), new PointF(48, 209), new PointF(22, 251), new PointF(74, 251), new PointF(69, 259), new PointF(0, 259) });
                e.Graphics.FillPolygon(orangebrush, new PointF[] { new PointF(87, 140), new PointF(48, 209), new PointF(90, 209), new PointF(131, 140) });
                e.Graphics.FillPolygon(seagreenbrush, new PointF[] { new PointF(184, 62), new PointF(163, 98), new PointF(203, 167), new PointF(226, 130) });
                e.Graphics.FillPolygon(bluebrush, new PointF[] { new PointF(203, 167), new PointF(257, 259), new PointF(300, 259), new PointF(226, 130) });
            }

            else if (comboBox4.Text == "Duval Triangle 2e")
            {
                e.Graphics.FillPolygon(greenbrush, new PointF[] { new PointF(150, 0), new PointF(0, 259), new PointF(300, 259) });
                e.Graphics.FillPolygon(pinkbrush, new PointF[] { new PointF(150, 0), new PointF(30, 209), new PointF(99, 209), new PointF(184, 63) });
                e.Graphics.FillPolygon(crimsonbrush, new PointF[] { new PointF(46, 209), new PointF(22, 251), new PointF(74, 251), new PointF(99, 209) });
                e.Graphics.FillPolygon(brownbrush, new PointF[] { new PointF(30, 209), new PointF(48, 209), new PointF(22, 251), new PointF(74, 251), new PointF(69, 259), new PointF(0, 259) });
                e.Graphics.FillPolygon(orangebrush, new PointF[] { new PointF(99, 209), new PointF(74, 251), new PointF(192, 251), new PointF(167, 209) });
                e.Graphics.FillPolygon(seagreenbrush, new PointF[] { new PointF(184, 62), new PointF(163, 98), new PointF(203, 167), new PointF(226, 130) });
                e.Graphics.FillPolygon(bluebrush, new PointF[] { new PointF(203, 167), new PointF(257, 259), new PointF(300, 259), new PointF(226, 130) });
            }

            else if (comboBox4.Text == "Duval Triangle 4") 
            { 
                e.Graphics.FillPolygon(greenbrush, new PointF[] { new PointF(150, 0), new PointF(0, 259), new PointF(300, 259) });
                e.Graphics.FillPolygon(pinkbrush, new PointF[] { new PointF(153, 7), new PointF(149, 12), new PointF(170, 46), new PointF(173, 41) });
                e.Graphics.FillPolygon(crimsonbrush, new PointF[] { new PointF(81, 120), new PointF(11, 243), new PointF(154, 243) });
                e.Graphics.FillPolygon(brownbrush, new PointF[] { new PointF(11, 243), new PointF(0, 259), new PointF(207, 259), new PointF(196, 243) });
                //e.Graphics.FillPolygon(orangebrush, new PointF[] { new PointF(99, 209), new PointF(74, 251), new PointF(192, 251), new PointF(167, 209) });
                e.Graphics.FillPolygon(seagreenbrush, new PointF[] { new PointF(207, 259), new PointF(196, 243), new PointF(185, 227), new PointF(206, 227), new PointF(167,162), new PointF(204,97), new PointF(300,259) });
                //e.Graphics.FillPolygon(bluebrush, new PointF[] { new PointF(203, 167), new PointF(257, 259), new PointF(300, 259), new PointF(226, 130) });
            }

            else if (comboBox4.Text == "Duval Triangle 5")
            {
                e.Graphics.FillPolygon(greenbrush, new PointF[] { new PointF(150, 0), new PointF(0, 259), new PointF(300, 259) });
                e.Graphics.FillPolygon(pinkbrush, new PointF[] { new PointF(0, 259), new PointF(69, 142), new PointF(84, 168), new PointF(33, 259) });
                e.Graphics.FillPolygon(crimsonbrush, new PointF[] { new PointF(84, 168), new PointF(69, 142), new PointF(127, 39), new PointF(143, 65) });
                e.Graphics.FillPolygon(brownbrush, new PointF[] { new PointF(126, 39), new PointF(130, 42), new PointF(150, 10), new PointF(147, 5) });
                e.Graphics.FillPolygon(orangebrush, new PointF[] { new PointF(147, 5), new PointF(150, 0), new PointF(165, 26), new PointF(147, 57),new PointF(143, 64),new PointF(130, 42),new PointF(130, 42), new PointF(150,10) });
                e.Graphics.FillPolygon(seagreenbrush, new PointF[] { new PointF(165, 26), new PointF(147, 57), new PointF(185, 122), new PointF(201, 93) });
                e.Graphics.FillPolygon(bluebrush, new PointF[] { new PointF(201, 93), new PointF(185, 122), new PointF(202, 153), new PointF(199, 160),new PointF(234, 218),new PointF(209, 259),new PointF(300, 259) });
                e.Graphics.FillPolygon(greenbrush, new PointF[] { new PointF(209, 259), new PointF(234, 218), new PointF(199, 160), new PointF(202, 153), new PointF(148, 57), new PointF(121, 103) });
                e.Graphics.FillPolygon(brownbrush, new PointF[] { new PointF(209, 259), new PointF(158, 170), new PointF(107, 259) });
                e.Graphics.FillPolygon(redbrush, new PointF[] { new PointF(107, 259), new PointF(158, 170), new PointF(121, 103), new PointF(32, 259) });
            }

            double ch4 = System.Convert.ToDouble(textBox2.Text);
            double c2h2 = System.Convert.ToDouble(textBox3.Text);
            double c2h4 = System.Convert.ToDouble(textBox4.Text);
            double total = ch4 + c2h2 + c2h4;

            ch4 = 300 * ch4 / total; //300 is length of side in px
            c2h2 = 300 * c2h2 / total;
            c2h4 = 300 * c2h4 / total;

            PointF ch4line = new PointF((float)(ch4 * Math.Cos(1.04719755)), (float)(259.807 - (ch4 * Math.Sin(1.04719755))));
            PointF ch4line1 = new PointF((float)(ch4 * Math.Cos(1.04719755) + 2 * (300 - ch4) * Math.Cos(1.04719755)), (float)(259.807 - (ch4 * Math.Sin(1.04719755))));

            PointF c2h4line = new PointF((float)( 300 - ( (300-c2h4) * Math.Cos(1.04719755)) ), (float)(259.807 - ((300-c2h4) * Math.Sin(1.04719755))));
            PointF c2h4line1 = new PointF((float)(300 - (2*(300 - c2h4) * Math.Cos(1.04719755))), (float)(259.807));

            PointF c2h2line = new PointF((float)(300 - c2h2), (float)(259.807) );
            PointF c2h2line1 = new PointF((float)((300-c2h2) * Math.Cos(1.04719755)), (float)(259.807 - ((300-c2h2) * Math.Sin(1.04719755))));


            Pen calcPen = new Pen(Color.Black, 2);
            calcPen.DashPattern = new float[] { 4.0F, 2.0F };


            e.Graphics.DrawLine(calcPen, ch4line, ch4line1);
            e.Graphics.DrawLine(calcPen, c2h4line, c2h4line1);
            e.Graphics.DrawLine(calcPen, c2h2line, c2h2line1);
            calcPen.Dispose();
        }

        //private void button2_Click(object sender, EventArgs e)
        

        private void comboBox4_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            panel1.Refresh();
        }

    }
}
