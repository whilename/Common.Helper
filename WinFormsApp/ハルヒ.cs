using Common.Utility;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinFormsApp
{
    /// <summary></summary>
    public partial class ハルヒ : Form
    {
        /// <summary></summary>
        public ハルヒ()
        {
            InitializeComponent();
        }

        /// <summary>填充数据至指定的Excel模板中</summary>
        public void FillData()
        {
            string sp = Application.StartupPath + "\\template01.xls";
            //读取指定的Excel做编辑
            using (FileStream fs = new FileStream(sp, FileMode.Open, FileAccess.Read))
            {
                this.workbook = new HSSFWorkbook(fs);
                //根据sheet名称获取索引
                int index = this.workbook.GetSheetIndex("Sheet1");
                //克隆一个sheet做编辑操作
                //this._sheet = this.workbook.CloneSheet(index);
                this._sheet = this.workbook.GetSheetAt(index);
                this._poi = new PoiUtils(this.workbook, this._sheet);
                //移除原sheet
                //this.workbook.RemoveSheetAt(index);
                this.workbook.GetSheetAt(index).ForceFormulaRecalculation = true;
                fs.Close();
            }

            for (int i = 1; i <= 20; i++)
            {
                this._poi.SetCellText(i, 1, "NPOI");
                this._poi.SetCellText(i, 3, "浪里个浪");
                this._poi.SetCellText(i, 5, "哈哈哈");
            }
            //保存更改后的Excel
            using (FileStream fs = new FileStream(sp, FileMode.Create))
            {
                //this.workbook.SetSheetName(0, "new");
                this.workbook.Write(fs);
                fs.Close();
            }
        }

        /// <summary>写入数据至指定的Excel中</summary>
        public void WriteData()
        {
            string sp = Application.StartupPath + "\\template02.xls";

            this.workbook = new HSSFWorkbook();
            this._sheet = this.workbook.CreateSheet("Sheet1");
            this._poi = new PoiUtils(this.workbook, this._sheet);
            this.workbook.GetSheetAt(this.workbook.GetSheetIndex("Sheet1")).ForceFormulaRecalculation = true;

            for (int i = 1; i <= 20; i++)
            {
                this._poi.SetCellText(i, 1, "NPOI");
                this._poi.SetCellText(i, 3, "浪里个浪");
                this._poi.SetCellText(i, 5, "哈哈哈");
            }
            //保存更改后的Excel
            using (FileStream fs = new FileStream(sp, FileMode.Create))
            {
                //this.workbook.SetSheetName(0, "new");
                this.workbook.Write(fs);
                fs.Close();
            }
        }

        /// <summary></summary>
        private void ハルヒ_Load(object sender, EventArgs e)
        {
            //this.FillData();
            //this.WriteData();
            //ImageAnimator.Animate(WinFormsApp.Properties.Resources.ハルヒ,OnFrameChanged);
        }

        /// <summary></summary>
        private void OnFrameChanged(object sender, EventArgs e)
        {
            ImageAnimator.UpdateFrames();
            this.Invalidate();
        }

        /// <summary></summary>
        private void ハルヒ_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }
        }

    }
}
