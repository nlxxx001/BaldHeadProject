using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.Helper
{
    public class ExcelHelper
    {
        /// <summary>
        /// 转execl到list（execl的第一行需要严格对应到class的第一个属性，速度较慢，数据量大请误用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <param name="sheetIndex">默认第一页</param>
        /// <returns></returns>
        public static List<T> ExeclToList<T>(Stream file, int sheetIndex = 0) where T : class, new()
        {
            //var fs = new FileStream(file, FileMode.Open, FileAccess.Read);
            var workbook = new XSSFWorkbook(file);
            var sheet = workbook.GetSheetAt(sheetIndex);

            var t = new T();
            var properties = t.GetType().GetProperties();
            var fields = properties.Select(x => x.Name).ToArray();

            IList<T> list = new List<T>();

            //遍历每一行数据
            for (int i = sheet.FirstRowNum + 1, len = sheet.LastRowNum + 1; i < len; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null) break;
                t = new T();
                for (int j = 0, len2 = fields.Length; j < len2; j++)
                {
                    var cell = row.GetCell(j);
                    if (cell == null) continue;
                    if (cell.CellType == CellType.Numeric && DateUtil.IsCellDateFormatted(cell))
                    {
                        var cellvalue = cell.DateCellValue.ToString("yyyy-MM-dd HH:mm:ss");
                        typeof(T).GetProperty(fields[j])?.SetValue(t, cellvalue, null);
                        continue;
                    }
                    else if (cell.CellType != CellType.String)
                    {
                        cell.SetCellType(CellType.String);
                    }
                    if (cell.StringCellValue != null)
                    {
                        //stuUser.setPhone(row.getCell(0).getStringCellValue());
                        var cellValue = cell.StringCellValue.Trim();
                        typeof(T).GetProperty(fields[j])?.SetValue(t, cellValue, null);
                    }
                }
                list.Add(t);
            }
            return list.ToList();
        }

        /// <summary>
        /// 转list到execl
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static NpoiMemoryStream ListToExecl<T>(IEnumerable<T> list) where T : class, new()
        {
            var workbook = new XSSFWorkbook();
            workbook.CreateSheet("Sheet1");
            var sheetOne = workbook.GetSheet("Sheet1"); //获取名称为Sheet1的工作表

            var sheetRow0 = sheetOne.CreateRow(0);  //获取Sheet1工作表的首行
            var properties0 = new T().GetType().GetProperties();
            for (var j = 0; j < properties0.Length; j++)
            {
                var col = sheetRow0.CreateCell(j);
                col.SetCellValue(properties0[j].Name);
                sheetRow0.Cells.Add(col);
            }
            var count = list.Count();
            for (var i = 0; i < count; i++)
            {
                var item = list.ElementAt(i);
                var sheetRow = sheetOne.CreateRow(i + 1);
                var properties = item.GetType().GetProperties();
                for (var j = 0; j < properties.Length; j++)
                {
                    var col = sheetRow.CreateCell(j, CellType.String);
                    col.SetCellValue(properties[j].GetValue(item, null)?.ToString() ?? "");
                    sheetRow.Cells.Add(col);
                }
            }
            var ms = new NpoiMemoryStream
            {
                AllowClose = false
            };
            workbook.Write(ms);
            return ms;
        }
    }

    /// <summary>
    /// 支持Npoi到内存流的直接操作
    /// </summary>
    public class NpoiMemoryStream : MemoryStream
    {
        /// <summary>
        /// 
        /// </summary>
        public NpoiMemoryStream()
        {
            AllowClose = true;
        }

        /// <summary>
        /// 总是关闭的
        /// </summary>
        public bool AllowClose { get; set; }

        /// <summary>
        /// 手动关闭
        /// </summary>
        public override void Close()
        {
            if (AllowClose)
                base.Close();
        }
    }
}
