using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using MyPluginEngine;

namespace TrendAnalyze
{
    class frmGDPCmd : MyPluginEngine.ICommand
    {
        private MyPluginEngine.IApplication hk;
        private System.Drawing.Bitmap m_hBitmap;
        private ESRI.ArcGIS.SystemUI.ICommand cmd = null;
        frmGDP pfrmGDP;

        public frmGDPCmd()
        {
            string str = @"..\Data\Image\TrendAnalyze\GDP.png";
            if (System.IO.File.Exists(str))
                m_hBitmap = new Bitmap(str);
            else
                m_hBitmap = null;
        }
        #region ICommand 成员
        public System.Drawing.Bitmap Bitmap
        {
            get { return m_hBitmap; }
        }

        public string Caption
        {
            get { return "GDP增长趋势"; }
        }

        public string Category
        {
            get { return "TrendAnalyzeMenu"; }
        }

        public bool Checked
        {
            get { return false; }
        }

        public bool Enabled
        {
            get { return true; }
        }

        public int HelpContextId
        {
            get { return 0; }
        }

        public string HelpFile
        {
            get { return ""; }
        }

        public string Message
        {
            get { return "GDP增长趋势"; }
        }

        public string Name
        {
            get { return "frmGDP"; }
        }

        public void OnClick()
        {
            //System.Windows.Forms.MessageBox.Show("模块正在开发中！");
            pfrmGDP = new frmGDP();
            pfrmGDP.ShowDialog();
        }

        public void OnCreate(IApplication hook)
        {
            if (hook != null)
            {
                this.hk = hook;
                pfrmGDP = new frmGDP();
                pfrmGDP.Visible = false;
            }
        }

        public string Tooltip
        {
            get { return "GDP增长趋势"; }
        }
        #endregion
    }
}
