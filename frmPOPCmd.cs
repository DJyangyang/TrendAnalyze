using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using MyPluginEngine;

namespace TrendAnalyze
{
    class frmPOPCmd : MyPluginEngine.ICommand
    {
        private MyPluginEngine.IApplication hk;
        private System.Drawing.Bitmap m_hBitmap;
        private ESRI.ArcGIS.SystemUI.ICommand cmd = null;
        frmPOP pfrmPOP;

        public frmPOPCmd()
        {
            string str = @"..\Data\Image\TrendAnalyze\People.png";
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
            get { return "人口增长趋势"; }
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
            get { return "人口增长趋势"; }
        }

        public string Name
        {
            get { return "frmPOP"; }
        }

        public void OnClick()
        {
            System.Windows.Forms.MessageBox.Show("正在开发中！");
        }

        public void OnCreate(IApplication hook)
        {
            if (hook != null)
            {
                this.hk = hook;
                pfrmPOP = new frmPOP();
                pfrmPOP.Visible = false;
            }
        }

        public string Tooltip
        {
            get { return "人口增长趋势"; }
        }
        #endregion
    }
}
