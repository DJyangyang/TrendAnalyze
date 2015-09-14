using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZedGraph;
using System.Data.OleDb;
using System.Configuration;
using System.Threading;

namespace TrendAnalyze
{
    public partial class frmGDP : DevComponents.DotNetBar.OfficeForm
    {
        private string sConn = null;
        private OleDbConnection pConn = null;
        private DataTable dt = null;

        private PointPairList list1 = null, list3 = null, list2 = null, list = null;
        public frmGDP()
        {
            InitializeComponent();
            this.dgDataSource.ReadOnly = true;
            this.dgDataSource.AllowUserToAddRows = false;
            //禁用Glass主题
            this.EnableGlass = false;
            //不显示最大化最小化按钮
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            //
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            //去除图标
            this.ShowIcon = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
            CreateMutiLineChart();

            btnSimulate.Enabled = false;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (pConn.State == ConnectionState.Open)
                pConn.Close();
            if (pConn != null)
                pConn =null;
            
        }

        private void LoadData()
        {
            //sConn = ConfigurationSettings.AppSettings["Provider"];
            //sConn += Application.StartupPath;
            //sConn += ConfigurationSettings.AppSettings["DataSource"];
            //sConn += ConfigurationSettings.AppSettings["Pwd"];
            sConn = @"Provider = Microsoft.Jet.OLEDB.4.0;Data Source =" + Application.StartupPath + @"\Data\dqhp.mdb;Jet OLEDB:Database Password=dqhpdata";
            if (pConn == null)
                pConn = new OleDbConnection(sConn);
            if (pConn.State == ConnectionState.Closed)
                pConn.Open();
            OleDbCommand cmd = pConn.CreateCommand();
            cmd.CommandText = "Select id,year,gdp1,gdp2,gdp3,gdp From tbTrend";
            OleDbDataAdapter oda = new OleDbDataAdapter(cmd);
            DataSet ds = new DataSet();
            oda.Fill(ds, "dtTrend");
            if (dt == null)
                dt = new DataTable();
            dt = ds.Tables["dtTrend"];
        }
        private void CreateMutiLineChart()
        {
            dgDataSource.DataSource = dt;

            int iRow = dt.Rows.Count;

            GraphPane myPane = zedGraphControl1.GraphPane;
            myPane.CurveList.Clear();
            myPane.GraphObjList.Clear();
			// Set up the title and axis labels
            myPane.Title.Text = "GDP 趋势模拟";
			myPane.XAxis.Title.Text = "年份";
			myPane.YAxis.Title.Text = "GDP 亿元";

            //PointPairList list1 = null, list3 = null, list2 = null, list = null;
			// Make up some data arrays based on the Sine function
			list1 = new PointPairList();
			list3 = new PointPairList();
            list2 = new PointPairList();
            list = new PointPairList();
			
            //double x = double.Parse(dt.Rows[0]["year"].ToString().Trim());
            //double y1 = double.Parse(dt.Rows[0]["gdp1"].ToString().Trim());
            //double y3 = double.Parse(dt.Rows[0]["gdp3"].ToString().Trim());
            //double y2 = double.Parse(dt.Rows[0]["gdp2"].ToString().Trim());
            //double y = double.Parse(dt.Rows[0]["gdp"].ToString().Trim());
            //list.Add(x, y);
            //list1.Add( x, y1);
            //list2.Add( x, y2 );
            //list3.Add(x, y3);

            LineItem myCurve1=null, myCurve2=null, myCurve3=null, myCurve=null;

            // Generate a red curve with diamond
            // symbols, and "GDP1" in the legend
            myCurve1 = myPane.AddCurve("GDP1",
                list1, Color.Red, SymbolType.Diamond);

            // Generate a blue curve with circle
            // symbols, and "GDP2" in the legend
            myCurve2 = myPane.AddCurve("GDP2",
                list2, Color.Blue, SymbolType.Circle);

            // Generate a blue curve with circle
            // symbols, and "GDP3" in the legend
            myCurve3 = myPane.AddCurve("GDP3",
                list3, Color.Green, SymbolType.Star);

            // Generate a blue curve with circle
            // symbols, and "GDP" in the legend
            myCurve = myPane.AddCurve("GDP",
                list, Color.Orange, SymbolType.Square);


            // Change the color of the title
            myPane.Title.FontSpec.FontColor = Color.Green;

            // Add gridlines to the plot, and make them gray
            myPane.XAxis.MajorGrid.IsVisible = true;
            myPane.YAxis.MajorGrid.IsVisible = true;
            myPane.XAxis.MajorGrid.Color = Color.LightGray;
            myPane.YAxis.MajorGrid.Color = Color.LightGray;

            // Move the legend location
            myPane.Legend.Position = ZedGraph.LegendPos.Bottom;

            // Make both curves thicker
            myCurve1.Line.Width = 1.0F;
            myCurve2.Line.Width = 1.0F;
            myCurve3.Line.Width = 1.0F;
            myCurve.Line.Width = 1.0F;

            // Fill the area under the curves

            //myCurve1.Line.Fill = new Fill(Color.White, Color.Red, 45F );
            //myCurve2.Line.Fill = new Fill( Color.White, Color.Blue, 45F );
            //myCurve3.Line.Fill = new Fill(Color.White, Color.Green, 45F);

            // Increase the symbol sizes, and fill them with solid white
            myCurve1.Symbol.Size = 2.0F;
            myCurve2.Symbol.Size = 2.0F;
            myCurve3.Symbol.Size = 2.0F;
            myCurve.Symbol.Size = 2.0F;

            myCurve1.Symbol.Fill = new Fill(Color.White);
            myCurve2.Symbol.Fill = new Fill(Color.White);
            myCurve3.Symbol.Fill = new Fill(Color.White);
            myCurve.Symbol.Fill = new Fill(Color.White);

            // Add a background gradient fill to the axis frame
            myPane.Chart.Fill = new Fill(Color.White,
                Color.FromArgb(255, 255, 210), -45F);

            // Add a caption and an arrow
            TextObj myText = new TextObj("Interesting\nPoint", 230F, 70F);
            myText.FontSpec.FontColor = Color.Red;
            myText.Location.AlignH = AlignH.Center;
            myText.Location.AlignV = AlignV.Top;
            myPane.GraphObjList.Add(myText);
            ArrowObj myArrow = new ArrowObj(Color.Red, 12F, 230F, 70F, 280F, 55F);
            myPane.GraphObjList.Add(myArrow);

            myPane.AxisChange();
            zedGraphControl1.Refresh();
        }

        private void loadHistoryData()
        {
            //加载数据库的原始数据
            while (list.Count > 1)
            {
                list.RemoveAt(1);
                list1.RemoveAt(1);
                list2.RemoveAt(1);
                list3.RemoveAt(1);
            }
            

            for (int i = 0; i < 17; i++)
            {
                Thread.Sleep(100);
               
                //创建曲线图

                double x = double.Parse(dt.Rows[i]["year"].ToString().Trim());
                double y1 = double.Parse(dt.Rows[i]["gdp1"].ToString().Trim());
                double y3 = double.Parse(dt.Rows[i]["gdp3"].ToString().Trim());
                double y2 = double.Parse(dt.Rows[i]["gdp2"].ToString().Trim());
                double y = double.Parse(dt.Rows[i]["gdp"].ToString().Trim());
                list.Add(x, y);
                list1.Add(x, y1);
                list2.Add(x, y2);
                list3.Add(x, y3);
                zedGraphControl1.GraphPane.XAxis.Scale.MaxAuto = true;

                this.zedGraphControl1.AxisChange();
                this.zedGraphControl1.Refresh();
            }
        }

        private void loadSimulationData()
        {
            //加载数据库的原始数据
            //id=1-17	year=1996-2012
            int iRowCount = dt.Rows.Count - 1;
            if (iRowCount > 17)
            {
                for (int i = iRowCount; i >= 17; i--)
                {
                    dt.Rows.RemoveAt(i);
                    list.RemoveAt(i);
                    list1.RemoveAt(i);
                    list2.RemoveAt(i);
                    list3.RemoveAt(i);
                }
            }
            //修改数据源dt中的值
            //id	year	gdp1	gdp2	gdp3	pop1	pop2	pop3	gdp	pop
            double dRate = 0;
            try
            {
                dRate = double.Parse(txtIncreaseRate.Text.Trim()) / 100;
            }
            catch
            {
                MessageBox.Show("增长率必须是数值！");
                return;
            }

            //模拟结束年份
            Int32 iEndYear = (Int32)txtEndYear.Value;

            for (int i = 0; i < iEndYear - 2012 + 1; i++)
            {
                Thread.Sleep(200);
                //产生模拟数据
                int iRowMaxIndex = dt.Rows.Count - 1;
                DataRow dr = dt.NewRow();
                dr["id"] = int.Parse(dt.Rows[iRowMaxIndex]["id"].ToString().Trim()) + 1;
                dr["year"] = int.Parse(dt.Rows[iRowMaxIndex]["year"].ToString().Trim()) + 1;
                dr["gdp1"] = double.Parse(dt.Rows[iRowMaxIndex]["gdp1"].ToString().Trim()) * (1 + dRate);
                dr["gdp3"] = double.Parse(dt.Rows[iRowMaxIndex]["gdp3"].ToString().Trim()) * (1 + dRate);
                dr["gdp2"] = double.Parse(dt.Rows[iRowMaxIndex]["gdp2"].ToString().Trim()) * (1 + dRate);
                dr["gdp"] = double.Parse(dt.Rows[iRowMaxIndex]["gdp"].ToString().Trim()) * (1 + dRate);
                dt.Rows.Add(dr);
                //创建曲线图

                double x = double.Parse(dt.Rows[iRowMaxIndex]["year"].ToString().Trim());
                double y1 = double.Parse(dt.Rows[iRowMaxIndex]["gdp1"].ToString().Trim());
                double y3 = double.Parse(dt.Rows[iRowMaxIndex]["gdp3"].ToString().Trim());
                double y2 = double.Parse(dt.Rows[iRowMaxIndex]["gdp2"].ToString().Trim());
                double y = double.Parse(dt.Rows[iRowMaxIndex]["gdp"].ToString().Trim());
                list.Add(x, y);
                list1.Add(x, y1);
                list2.Add(x, y2);
                list3.Add(x, y3);
                zedGraphControl1.GraphPane.XAxis.Scale.MaxAuto = true;

                this.zedGraphControl1.AxisChange();
                this.zedGraphControl1.Refresh();

                this.dgDataSource.DataSource = dt;
                this.dgDataSource.Refresh();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            loadHistoryData();
            btnSimulate.Enabled = true;
        }

        private void btnSimulate_Click(object sender, EventArgs e)
        {
            loadSimulationData();
            btnSimulate.Enabled = false;
        }
    }
}