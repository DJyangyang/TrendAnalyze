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
    public partial class frmPOP : DevComponents.DotNetBar.OfficeForm
    {
        private string sConn = null;
        private OleDbConnection pConn = null;
        private DataTable dt = null;

        private PointPairList list1 = null, list3 = null, list2 = null, list = null;
        public frmPOP()
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
            //string connectionStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data source=" + databasePath;
            //

            //string StrConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
            //StrConn += Application.StartupPath + "\\data\\data.mdb;Persist Security Info=False;Jet OLEDB:Database Password=123456";


            //"Provider = Microsoft.ACE.OLEDB.12.0;Data Source =c:\\users\\xqy\\documents\\visual studio 2013\\Projects\\TrendAnalyze\\TrendAnalyze\\bin\\Debug\\Data\\dqhp.accdb;Jet OLEDB:Database Password=dqhpdata;"


            //sConn = ConfigurationSettings.AppSettings["Provider"];
            //sConn += Application.StartupPath;
            //sConn += ConfigurationSettings.AppSettings["DataSource"];

            //sConn += "Persist Security Info=False;";
            //sConn += ConfigurationSettings.AppSettings["Pwd"];
            sConn = @"Provider = Microsoft.Jet.OLEDB.4.0;Data Source =" + Application.StartupPath + @"\Data\dqhp.mdb;Jet OLEDB:Database Password=dqhpdata";
            if (pConn == null)
                pConn = new OleDbConnection(sConn);
            if (pConn.State == ConnectionState.Closed)
                pConn.Open();
            OleDbCommand cmd = pConn.CreateCommand();
            cmd.CommandText = "Select id,year,pop,apop,napop,citypop From tbTrend";
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

            GraphPane myPane = zedGraphControl1.GraphPane;
            myPane.CurveList.Clear();
            myPane.GraphObjList.Clear();

			// Set up the title and axis labels
            myPane.Title.Text = "人口增长趋势";
			myPane.XAxis.Title.Text = "年份";
			myPane.YAxis.Title.Text = "人口  万人";

            //PointPairList list1 = null, list3 = null, list2 = null, list = null;
			// Make up some data arrays based on the Sine function
			list1 = new PointPairList();
			list3 = new PointPairList();
            list2 = new PointPairList();
            list = new PointPairList();

            //double x = double.Parse(dt.Rows[0]["year"].ToString().Trim());
            //double y1 = double.Parse(dt.Rows[0]["apop"].ToString().Trim());
            //double y3 = double.Parse(dt.Rows[0]["citypop"].ToString().Trim());
            //double y2 = double.Parse(dt.Rows[0]["napop"].ToString().Trim());
            //double y = double.Parse(dt.Rows[0]["pop"].ToString().Trim());
            //list.Add(x, y);
            //list1.Add(x, y1);
            //list2.Add(x, y2);
            //list3.Add(x, y3);

            LineItem myCurve1=null, myCurve2=null, myCurve3=null, myCurve=null;

            // Generate a red curve with diamond
            // symbols, and "apop" in the legend
            myCurve1 = myPane.AddCurve("apop",
                list1, Color.Red, SymbolType.Diamond);

            // Generate a blue curve with circle
            // symbols, and "napop" in the legend
            myCurve2 = myPane.AddCurve("napop",
                list2, Color.Blue, SymbolType.Circle);

            // Generate a blue curve with circle
            // symbols, and "citypop" in the legend
            myCurve3 = myPane.AddCurve("citypop",
                list3, Color.Green, SymbolType.Star);

            // Generate a blue curve with circle
            // symbols, and "pop" in the legend
            myCurve = myPane.AddCurve("pop",
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

        private void CreateMasterPane()
        {
            dgDataSource.DataSource = dt;
            int iRow = dt.Rows.Count;
            MasterPane myMaster = zedGraphControl1.MasterPane;

            // Remove the default GraphPane that comes with ZedGraphControl
            myMaster.PaneList.Clear();

            // Set the masterpane title
            myMaster.Title.Text = "pop 趋势模拟";
            myMaster.Title.IsVisible = true;

            // Fill the masterpane background with a color gradient
            myMaster.Fill = new Fill(Color.White, Color.MediumSlateBlue, 45.0F);

            // Set the margins to 10 points
            myMaster.Margin.All = 10;

            // Enable the masterpane legend
            myMaster.Legend.IsVisible = true;
            myMaster.Legend.Position = LegendPos.TopCenter;


            // Initialize a color and symbol type rotator
            ColorSymbolRotator rotator = new ColorSymbolRotator();

            // Create some new GraphPanes

            //apop - pop 1

            // Create a new graph - rect dimensions do not matter here, since it
            // will be resized by MasterPane.AutoPaneLayout()
            //GraphPane myPane = new GraphPane(new Rectangle(10, 10, 10, 10),
            GraphPane myPane = new GraphPane(new Rectangle(10, 10, 10, 10),
                "黑龙江GDP",
                "年份",
                "GDP,亿元");

            // Fill the GraphPane background with a color gradient
            myPane.Fill = new Fill(Color.White, Color.LightYellow, 45.0F);
            myPane.BaseDimension = 6.0F;

            // Make up some data arrays based on the Sine function
            PointPairList list = new PointPairList();
            for (int i = 0; i < iRow - 1; i++)
            {
                double x = double.Parse(dt.Rows[i]["year"].ToString());
                double y = double.Parse(dt.Rows[i]["gdp1"].ToString());
                list.Add(x, y);
            }

            // Add a curve to the Graph, use the next sequential color and symbol
            LineItem myCurve = myPane.AddCurve("Type 1",
                list, rotator.NextColor, rotator.NextSymbol);
            // Fill the symbols with white to make them opaque
            myCurve.Symbol.Fill = new Fill(Color.White);

            // Add the GraphPane to the MasterPane
            myMaster.Add(myPane);
            using (Graphics g = this.zedGraphControl1.CreateGraphics())
            {
                // Tell ZedGraph to auto layout the new GraphPanes
                myMaster.SetLayout(g, PaneLayout.SquareColPreferred);
                myMaster.AxisChange(g);
                //g.Dispose();
            }
        }
        //加载历史数据
        private void LoadHistoryData()
        {
            //加载数据库的原始数据
            while (list.Count > 1)
            {
                list.RemoveAt(1);
                list1.RemoveAt(1);
                list2.RemoveAt(1);
                list3.RemoveAt(1);
            }
            //修改数据源dt中的值
            //id	year	gdp1	gdp2	gdp3	apop	napop	citypop	gdp	pop

            for (int i = 1; i < 17; i++)
            {
                Thread.Sleep(100);
                double x = double.Parse(dt.Rows[i]["year"].ToString().Trim());
                double y1 = double.Parse(dt.Rows[i]["apop"].ToString().Trim());
                double y3 = double.Parse(dt.Rows[i]["napop"].ToString().Trim());
                double y2 = double.Parse(dt.Rows[i]["citypop"].ToString().Trim());
                double y = double.Parse(dt.Rows[i]["pop"].ToString().Trim());
                list.Add(x, y);
                list1.Add(x, y1);
                list2.Add(x, y2);
                list3.Add(x, y3);
                zedGraphControl1.GraphPane.XAxis.Scale.MaxAuto = true;

                this.zedGraphControl1.AxisChange();
                this.zedGraphControl1.Refresh();
            }
        }
        //加载模拟数据
        private void LoadSimulationData()
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
            //id	year	gdp1	gdp2	gdp3	apop	napop	citypop	gdp	pop
            double rate = 0;
            try
            {
                rate = double.Parse(txtIncreaseRate.Text.Trim()) / 1000;
            }
            catch
            {
                MessageBox.Show("人口增长率必须是数值！");
                return;
            }

            //拐点年份
            Int32 iInflexionYear = (Int32)txtInflexionYear.Value;
            //模拟结束年份
            Int32 iEndYear = (Int32)txtEndYear.Value;
            double dRateTemp = rate / (iInflexionYear - 2012);
            double dRate = rate;

            for (int i = 0; i < iEndYear - 2012 + 1; i++)
            {
                Thread.Sleep(200);
                //产生模拟数据
                dRate = dRate - dRateTemp;
                int iRowMaxIndex = dt.Rows.Count - 1;
                DataRow dr = dt.NewRow();
                dr["id"] = int.Parse(dt.Rows[iRowMaxIndex]["id"].ToString().Trim()) + 1;
                dr["year"] = int.Parse(dt.Rows[iRowMaxIndex]["year"].ToString().Trim()) + 1;
                dr["apop"] = double.Parse(dt.Rows[iRowMaxIndex]["apop"].ToString().Trim()) * (1 + dRate);
                dr["citypop"] = double.Parse(dt.Rows[iRowMaxIndex]["citypop"].ToString().Trim()) * (1 + dRate);
                dr["napop"] = double.Parse(dt.Rows[iRowMaxIndex]["napop"].ToString().Trim()) * (1 + dRate);
                dr["pop"] = double.Parse(dt.Rows[iRowMaxIndex]["pop"].ToString().Trim()) * (1 + dRate);
                dt.Rows.Add(dr);

                //创建曲线图

                double x = double.Parse(dt.Rows[iRowMaxIndex]["year"].ToString().Trim());
                double y1 = double.Parse(dt.Rows[iRowMaxIndex]["apop"].ToString().Trim());
                double y3 = double.Parse(dt.Rows[iRowMaxIndex]["citypop"].ToString().Trim());
                double y2 = double.Parse(dt.Rows[iRowMaxIndex]["napop"].ToString().Trim());
                double y = double.Parse(dt.Rows[iRowMaxIndex]["pop"].ToString().Trim());
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
            LoadHistoryData();
        }

        private void btnSimulate_Click(object sender, EventArgs e)
        {
            LoadSimulationData();
        }
    }
}