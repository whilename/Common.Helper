using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Common.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NPOI.SS.Util;
using System.IO;
using NPOI.XSSF.UserModel;

namespace Common.Utility
{
    /// <summary>NPOI Utils</summary>
    public class PoiUtils
    {
        private ISheet _sheet;
        /// <summary></summary>
        public ISheet WorkSheet
        {
            get { return _sheet; }
            set { _sheet = value; }
        }

        private IWorkbook _workbook;
        /// <summary></summary>
        public IWorkbook Workbook
        {
            get { return _workbook; }
            set { _workbook = value; }
        }

        private int _currentrow;
        /// <summary>当前行</summary>
        public int CurrentRow
        {
            get { return _currentrow; }
            set { _currentrow = value; }
        }
        private int _currentcol;
        /// <summary>当前列</summary>
        public int CurrentCol
        {
            get { return _currentcol; }
            set { _currentcol = value; }
        }
        private IFormulaEvaluator _evaluator;
        private bool is_xlsx = false;

        /// <summary>NPOI加载Excel</summary>
        /// <param name="stream"></param>
        public PoiUtils(string filepath)
        {
            FileInfo fi = new FileInfo(filepath);
            //读取指定的Excel做编辑
            using (FileStream stream = fi.OpenRead())
            {
                if (fi.Extension.Equals(".xlsx", StringComparison.CurrentCultureIgnoreCase))
                {
                    _workbook = new XSSFWorkbook(stream);
                    is_xlsx = true;
                }
                else { _workbook = new HSSFWorkbook(stream); }
                stream.Close();
            }
            _sheet = _workbook.GetSheetAt(0);
        }

        /// <summary>NPOI加载数据流</summary>
        /// <param name="stream"></param>
        public PoiUtils(Stream stream)
        {
            _workbook = new HSSFWorkbook(stream);
            _sheet = _workbook.GetSheetAt(0);
        }

        /// <summary>NPOI帮助类</summary>
        public PoiUtils(IWorkbook workbook = null, ISheet sheet = null)
        {
            _workbook = workbook ?? new HSSFWorkbook();
            _sheet = sheet ?? _workbook.CreateSheet("Sheet1");
            _sheet.ForceFormulaRecalculation = true;
        }

        /// <summary>设置sheet的名称</summary>
        /// <param name="sheetName"></param>
        public void SetSheetName(string sheetName)
        {
            _workbook.SetSheetName(_workbook.GetSheetIndex(_sheet), sheetName);
        }

        /// <summary>sheet克隆</summary>
        /// <param name="sheetName"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public PoiUtils CloneSheet(string sheetName, PoiUtils source)
        {
            ISheet sheet = source.Workbook.CloneSheet(source.Workbook.GetSheetIndex(source.WorkSheet));
            source.Workbook.SetSheetName(source.Workbook.GetSheetIndex(sheet), sheetName);
            return new PoiUtils(source.Workbook, sheet);
        }

        /// <summary>获取当前操作workbook数据流</summary>
        /// <returns>数据字节流</returns>
        public byte[] GetBuffer()
        {
            byte[] bits = new byte[] { };
            using (MemoryStream stream = new MemoryStream())
            {
                this._workbook.Write(stream);
                bits = stream.GetBuffer();
                stream.Close();
            }
            return bits;
        }

        /// <summary>
        /// save excel file
        /// </summary>
        /// <returns>file name</returns>
        public void SaveFile(string filename)
        {
            using (FileStream fsWrite = new FileStream(filename, FileMode.Append))
            {
                this._workbook.Write(fsWrite);
            }
        }

        #region 取得单元格

        /// <summary>取得单元格</summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public ICell GetCell(int row, int col)
        {
            CurrentRow = row;
            CurrentCol = col;
            IRow rowObj = _sheet.GetRow(row - 1);
            if (rowObj == null)
            {
                CreateRow(row);
                rowObj = _sheet.GetRow(row - 1);
            }
            ICell cellObj = rowObj.GetCell(col - 1);
            if (cellObj == null)
            {
                rowObj.CreateCell(col - 1);
                cellObj = rowObj.GetCell(col - 1);
            }
            return cellObj;
        }

        /// <summary>取得单元格</summary>
        /// <param name="row"></param>
        /// <param name="colName">列的字母名称</param>
        /// <returns></returns>
        public ICell GetCell(int row, string colName) { return GetCell(row, ToNumber(colName)); }

        /// <summary>取得单元格内容</summary>
        /// <param name="row">行号，从1开始</param>
        /// <param name="colName">列序号(A,B,C...)</param>
        /// <returns></returns>
        public string GetCellText(int row, string colName)
        {
            int col = ToNumber(colName);
            return GetCellText(row, col);
        }

        /// <summary>取得单元格内容</summary>
        /// <param name="row">行数，从1开始</param>
        /// <param name="col">列数，从1开始</param>
        /// <returns></returns>
        /// <remarks> 
        /// 注意事项
        /// 1，当Excel中这个单元格的文字格式是日期时，返回的将是一串数字，
        ///    如果是读取日期值的，请改用GetCellDateValue()方法 
        /// </remarks>
        public string GetCellText(int row, int col)
        {
            //用不惯POI啊。GetRow会返回null
            IRow hssfrow = _sheet.GetRow(row - 1);
            if (hssfrow == null)
            {
                return "";
            }
            ICell cell = hssfrow.GetCell(col - 1);
            if (cell == null)
            {
                return "";
            }
            if (cell.CellType == CellType.Numeric)
            {
                if (DateUtil.IsCellDateFormatted(cell))
                {
                    return cell.DateCellValue.ToString("yyyy-MM-dd");//日期型
                }
                else
                {
                    return cell.NumericCellValue.ToString();//数字型
                }
            }
            else if (cell.CellType == CellType.String)
            {
                return cell.StringCellValue.Trim();
            }
            else if (cell.CellType == CellType.Boolean)
            {
                return cell.BooleanCellValue == true ? "true" : "false";
            }
            else if (cell.CellType == CellType.Formula)
            {
                if (cell.CachedFormulaResultType == CellType.String)
                    return cell.StringCellValue;
                return GetCalcValue(row, col).ToString();
            }
            return "";
        }

        /// <summary>取得单元格的日期值</summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public DateTime? GetCellDateValue(int row, int col)
        {
            IRow hssfrow = _sheet.GetRow(row - 1);
            if (hssfrow == null)
                return null;

            ICell cell = hssfrow.GetCell(col - 1);
            if (cell == null)
                return null;

            if (cell.CellType == CellType.Numeric)
            {
                return cell.DateCellValue;
            }
            return null;
        }

        /// <summary>获取公式计算后的值</summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        /// <remarks>适用场景：用npoi生成Excel时，获取单元格经公式计算后的值时使用</remarks>
        public decimal GetCalcValue(int row, int col)
        {
            if (_evaluator == null && _workbook != null)
            {
                if (is_xlsx) { _evaluator = new XSSFFormulaEvaluator(_workbook); }
                else { _evaluator = new HSSFFormulaEvaluator(_workbook); }
            }
            if (_evaluator == null)
            {
                return 0;
            }
            ICell cell = GetCell(row, col);
            return _evaluator.Evaluate(cell).NumberValue.ToDecimal();
        }

        #endregion

        #region 设置单元格

        /// <summary>设置单元格内容</summary>
        /// <param name="row"></param>
        /// <param name="colName"></param>
        /// <param name="value"></param>
        public void SetCellText(int row, string colName, object value)
        {
            int col = ToNumber(colName);
            SetCellText(row, col, value);
        }

        /// <summary>设置单元格内容</summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="value"></param>
        public void SetCellText(int row, int col, object value)
        {
            if (value == null) return;
            Type t = value.GetType();
            ICell cell = GetCell(row, col);
            if (t == typeof(decimal) || t == typeof(double)
                || t == typeof(Int32) || t == typeof(Int64))
            {
                cell.SetCellValue(value.ToDouble());
            }
            else if (t == typeof(DateTime))
            {
                cell.SetCellValue(Convert.ToDateTime(value));
                cell.CellStyle.DataFormat = CreateDataFormat().GetFormat("yyyy-MM-dd HH:mm:ss");
            }
            else if (t == typeof(bool))
            {
                cell.SetCellValue(Convert.ToBoolean(value) ? "是" : "否");
            }
            else
            {
                cell.SetCellValue(Convert.ToString(value));
            }
        }

        /// <summary>设置单元格内容及样式</summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="value"></param>
        /// <param name="cellStyle"></param>
        public void SetCellTextStyle(int row, int col, object value, HSSFCellStyle cellStyle)
        {
            SetCellText(row, col, value);
            SetCellStyle(row, col, cellStyle);
        }

        /// <summary>设置单元格公式</summary>
        /// <param name="row"></param>
        /// <param name="colName"></param>
        /// <param name="formula"></param>
        public void SetCellFormula(int row, string colName, string formula)
        {
            int col = ToNumber(colName);
            SetCellFormula(row, col, formula);
        }

        /// <summary></summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="formula"></param>
        public void SetCellFormula(int row, int col, string formula) { GetCell(row, col).CellFormula = formula; }

        /// <summary>设置单元格公式及样式</summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="formula"></param>
        /// <param name="cellStyle"></param>
        public void SetCellFormulaStyle(int row, int col, string formula, HSSFCellStyle cellStyle)
        {
            SetCellFormula(row, col, formula);
            SetCellStyle(row, col, cellStyle);
        }

        /// <summary>创建 用于自定义样式</summary>
        /// <returns></returns>
        public IDataFormat CreateDataFormat() { return _workbook.CreateDataFormat(); }

        /// <summary>创建单元格样式</summary>
        /// <returns></returns>
        public ICellStyle CreateCellStyle() { return _workbook.CreateCellStyle(); }

        /// <summary>创建字体样式</summary>
        /// <returns></returns>
        public IFont CreateFont() { return _workbook.CreateFont(); }

        /// <summary>克隆单元格样式</summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public ICellStyle CloneCellStyle(ICellStyle source)
        {
            ICellStyle cellstyle = CreateCellStyle();
            cellstyle.CloneStyleFrom(source);
            return cellstyle;
        }

        /// <summary></summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public ICellStyle GetCellStyle(int row, int col)
        {
            int iRow = CurrentRow;
            int iCol = CurrentCol;
            ICellStyle cellStyle = GetCell(row, col).CellStyle;
            CurrentRow = iRow;
            CurrentCol = iCol;
            return cellStyle;
        }

        /// <summary></summary>
        /// <param name="fromCellStyle"></param>
        public void SetCellStyle(ICellStyle fromCellStyle) { SetCellStyle(CurrentRow, CurrentCol, fromCellStyle); }

        /// <summary></summary>
        /// <param name="fromRow"></param>
        /// <param name="fromCol"></param>
        /// <param name="toRow"></param>
        /// <param name="toCol"></param>
        public void SetCellStyle(int fromRow, int fromCol, int toRow, int toCol) { SetCellStyle(toRow, toCol, GetCellStyle(fromRow, fromCol)); }

        /// <summary></summary>
        /// <param name="toRow"></param>
        /// <param name="toCol"></param>
        /// <param name="fromCellStyle"></param>
        public void SetCellStyle(int toRow, int toCol, ICellStyle fromCellStyle) { GetCell(toRow, toCol).CellStyle = fromCellStyle; }

        /// <summary></summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="cellstyle"></param>
        public void SetCellStyle(int row, string col, HSSFCellStyle cellstyle) { SetCellStyle(row, ToNumber(col), cellstyle); }

        /// <summary>设置单元格样式</summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="cellstyle"></param>
        public void SetCellStyle(int row, int col, HSSFCellStyle cellstyle) { GetCell(row, col).CellStyle = cellstyle; }

        /// <summary>隐藏列</summary>
        /// <param name="col"></param>
        public void HideColumn(int col) { _sheet.SetColumnHidden(col - 1, true); }

        /// <summary>合并单元格</summary>
        /// <param name="startRow">起始行</param>
        /// <param name="startCol">起始列</param>
        /// <param name="endRow">结束行</param>
        /// <param name="endCol">结束列</param>
        public void MergedRegion(int startRow, int startCol, int endRow, int endCol)
        {
            _sheet.AddMergedRegion(new CellRangeAddress(startRow - 1, endRow - 1, startCol - 1, endCol - 1));
        }

        /// <summary>设置列宽</summary>
        /// <param name="col"></param>
        /// <param name="width"></param>
        /// <remarks>poi设置后的列宽要比给定的width要小0.71，所以适当放大</remarks>
        public void SetColumnWidth(string col, int width) { SetColumnWidth(ToNumber(col), width); }

        /// <summary>设置列宽</summary>
        /// <param name="col"></param>
        /// <param name="width"></param>
        public void SetColumnWidth(int col, int width) { _sheet.SetColumnWidth(col - 1, width * 256); }

        /// <summary>设置行高</summary>
        /// <param name="row"></param>
        /// <param name="height"></param>
        public void SetRowHeight(int row, int height) { _sheet.SetColumnWidth(row - 1, height * 256); }

        #endregion

        #region CreateRow 新增一行

        /// <summary>在当前行插入一行（也就是把当前行移到下一行）</summary>
        /// <returns></returns>
        public void InsertRow()
        {
            _sheet.ShiftRows(CurrentRow - 1, _sheet.LastRowNum, 1);
            IRow _row = _sheet.GetRow(CurrentRow - 1);
            for (int i = 0; i < _row.LastCellNum; i++)
            {
                _row.CreateCell(i);
            }
        }

        /// <summary>添加新行</summary>
        /// <returns></returns>
        public IRow CreateRow() { return CreateRow(++CurrentRow); }

        /// <summary>添加新行</summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public IRow CreateRow(int row) { return CreateRow(row, null); }

        /// <summary>添加新行，并拷贝指定行的样式到新行上</summary>
        /// <param name="row"></param>
        /// <param name="copyRowStyleFrom"></param>
        /// <returns></returns>
        public IRow CreateRow(int row, int copyRowStyleFrom)
        {
            IRow hssfRow = CreateRow(row);
            CurrentRow = row;
            IRow _row = _sheet.GetRow(copyRowStyleFrom - 1);
            for (int i = 0; i < _row.LastCellNum; i++)
            {
                hssfRow.GetCell(i).CellStyle = _row.GetCell(i).CellStyle;
            }
            return hssfRow;
        }

        /// <summary>添加新行，并指定新行的样式</summary>
        /// <param name="row"></param>
        /// <param name="cellstyle"></param>
        /// <returns></returns>
        public IRow CreateRow(int row, ICellStyle cellstyle)
        {
            IRow _row = _sheet.CreateRow(row - 1);
            CurrentRow = row;
            for (int i = 0; i < _row.LastCellNum; i++)
            {
                _row.CreateCell(i);
                if (cellstyle != null) { GetCell(row, i + 1).CellStyle = cellstyle; }
            }
            return _row;
        }

        #endregion

        #region 静态方法

        /// <summary>取得某一列多个行的合计公式</summary>
        /// <param name="rowList">参与合计的行的集合</param>
        /// <returns></returns>
        public static string GetTotalFormula(List<int> rowList)
        {
            string ret = "";
            for (int i = 0; i < rowList.Count; i++)
            {
                if (ret.Length > 0) { ret += "+"; }
                //{0}是列的占位符
                ret += "{0}" + rowList[i].ToString();
            }
            return ret;
        }

        /// <summary>
        /// HSSFRow Copy Command, Inserts a existing row into a new row, will automatically push down
        /// any existing rows.  Copy is done cell by cell and supports, and the
        /// command tries to copy all properties available (style, merged cells, values, etc...)
        /// </summary>
        /// <param name="poiTo">目标PoiUtils对象</param>
        /// <param name="poiFrom">源PoiUtils对象</param>
        /// <param name="sourceRowNum">源样式的行数</param>
        /// <param name="destinationRowNum">目标行</param>
        public static void CopyRow(PoiUtils poiTo, PoiUtils poiFrom, int sourceRowNum, int destinationRowNum)
        {
            // Get the source / new row
            IRow newRow = poiTo.WorkSheet.GetRow(destinationRowNum - 1);
            IRow sourceRow = poiFrom.WorkSheet.GetRow(sourceRowNum - 1);
            // Loop through source columns to add to new row
            for (int i = 0; i < sourceRow.LastCellNum; i++)
            {
                // Grab a copy of the old/new cell
                ICell oldCell = sourceRow.GetCell(i);
                //ICell newCell = newRow.CreateCell(i);
                ICell newCell = newRow.GetCell(i);
                // If the old cell is null jump to next cell
                if (oldCell == null) { newCell = null; continue; }
                //模板cell有，目标cell没有
                if (newCell == null) { newCell = newRow.CreateCell(i); }
                // 这个不适合生成内容较多的表单（poi只能生成4000个样式）
                // Copy style from old cell and apply to new cell
                //ICellStyle newCellStyle = poiTo.CreateCellStyle();
                //newCellStyle.CloneStyleFrom(oldCell.CellStyle); ;
                //newCell.CellStyle = newCellStyle;
                newCell.CellStyle = oldCell.CellStyle;
                // If there is a cell comment, copy
                if (newCell.CellComment != null) newCell.CellComment = oldCell.CellComment;
                // If there is a cell hyperlink, copy
                if (oldCell.Hyperlink != null) newCell.Hyperlink = oldCell.Hyperlink;
                // Set the cell data type
                newCell.SetCellType(oldCell.CellType);
            }
            // If there are are any merged regions in the source row, copy to new row
            for (int i = 0; i < poiFrom.WorkSheet.NumMergedRegions; i++)
            {
                CellRangeAddress cellRangeAddress = poiFrom.WorkSheet.GetMergedRegion(i);
                if (cellRangeAddress.FirstRow == sourceRow.RowNum)
                {
                    CellRangeAddress newCellRangeAddress = new CellRangeAddress(newRow.RowNum,
                        (newRow.RowNum + (cellRangeAddress.LastRow - cellRangeAddress.FirstRow)),
                        cellRangeAddress.FirstColumn, cellRangeAddress.LastColumn);
                    poiTo.WorkSheet.AddMergedRegion(newCellRangeAddress);
                }
            }
        }

        /// <summary>用于excel表格中列号字转成数字，返回的列号索引从1开始</summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static int ToNumber(string columnName)
        {
            if (!Regex.IsMatch(columnName.ToUpper(), @"[A-Z]+"))
                throw new Exception("Invalid parameter");
            int index = 0;
            char[] chars = columnName.ToUpper().ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                index += ((int)chars[i] - (int)'A' + 1) * (int)Math.Pow(26, chars.Length - i - 1);
            }
            return index;
        }
        #endregion
    }
}
