using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using org.in2bits.MyXls;
using System.IO;

namespace DataBaseTool.Service.imp
{
    public class ExportService
    {
        private string filePath = string.Empty;
        /// <summary>
        /// 导出结果到Excel
        /// </summary>
        /// <param name="dtt">DataTable</param>
        /// <param name="path">路径</param>
        public void exportResult(DataTable dtt, string fileName,string path)
        {
            //path = string.Empty;
            if (dtt == null)
            {
                return;
            }
            try
            {
                DataTable dt = dtt;
                DataRow row = dt.NewRow();
                XlsDocument xls = new XlsDocument();
                Worksheet sheet = xls.Workbook.Worksheets.AddNamed(fileName);
                ColumnInfo cinfo = new ColumnInfo(xls, sheet);
                cinfo.ColumnIndexStart = 0;
                cinfo.ColumnIndexEnd = 24;
                cinfo.Width = 60 * 80;
                sheet.AddColumnInfo(cinfo);
                Cells cells = sheet.Cells;

                // 列标题样式
                XF columnTitleXF = xls.NewXF(); // 为xls生成一个XF实例，XF是单元格格式对象
                columnTitleXF.HorizontalAlignment = HorizontalAlignments.Centered; // 设定文字居中
                columnTitleXF.VerticalAlignment = VerticalAlignments.Centered; // 垂直居中
                columnTitleXF.UseBorder = true; // 使用边框 
                columnTitleXF.TopLineStyle = 1; // 上边框样式
                columnTitleXF.TopLineColor = Colors.Black; // 上边框颜色
                columnTitleXF.BottomLineStyle = 1; // 下边框样式
                columnTitleXF.BottomLineColor = Colors.Black; // 下边框颜色
                columnTitleXF.LeftLineStyle = 1; // 左边框样式
                columnTitleXF.LeftLineColor = Colors.Black; // 左边框颜色
                columnTitleXF.Pattern = 1; // 单元格填充风格。如果设定为0，则是纯色填充(无色)，1代表没有间隙的实色 
                columnTitleXF.PatternBackgroundColor = Colors.Red; // 填充的底色 
                columnTitleXF.PatternColor = Colors.Default27; // 填充背景色
                columnTitleXF.Font.Bold = true;//加粗
                columnTitleXF.Font.ColorIndex = 1;//白色字体


                // 数据单元格样式
                XF dataXF = xls.NewXF(); // 为xls生成一个XF实例，XF是单元格格式对象
                dataXF.HorizontalAlignment = HorizontalAlignments.Centered; // 设定文字居中
                dataXF.VerticalAlignment = VerticalAlignments.Centered; // 垂直居中
                dataXF.UseBorder = true; // 使用边框 
                dataXF.LeftLineStyle = 1; // 左边框样式
                dataXF.LeftLineColor = Colors.Black; // 左边框颜色
                dataXF.BottomLineStyle = 1;  // 下边框样式
                dataXF.BottomLineColor = Colors.Black;  // 下边框颜色
                dataXF.Font.FontName = "宋体";
                dataXF.Font.Height = 9 * 20; // 设定字大小（字体大小是以 1/20 point 为单位的）
                dataXF.Pattern = 1; // 单元格填充风格。如果设定为0，则是纯色填充(无色)，1代表没有间隙的实色 
                dataXF.UseProtection = false; // 默认的就是受保护的，导出后需要启用编辑才可修改
                dataXF.TextWrapRight = true; // 自动换行

                //生成Excel中列头名称
                foreach (DataColumn col in dt.Columns)
                {
                    sheet.Cells.Add(1, col.Ordinal + 1, headerDictionary(col.ColumnName), columnTitleXF);
                }

                //填充内容   
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if ((i + 2) % 2 == 0)
                        {
                            dataXF.PatternColor = Colors.Default2C;
                        }
                        else
                        {
                            dataXF.PatternColor = Colors.Default29;
                        }
                        Cell cell;
                        cell = sheet.Cells.Add(i + 2, j + 1, dt.Rows[i][j], dataXF);
                    }
                }

                string file = path + "\\" + fileName;
                if (File.Exists(file))
                {
                    File.Delete(file);
                }

                xls.FileName = fileName;
                xls.Save(path);
            }
            catch (Exception ex)
            {
                Logout.WriteLog(string.Format("导出Excel查询结果文件错误:{0}", ex.Message), LogType.ERROR);
                throw ex;
            }
        }

        /// <summary>
        /// 标题字典
        /// </summary>
        /// <param name="headerName"></param>
        /// <returns></returns>
        private string headerDictionary(string headerName)
        {
            string name = headerName;
            if (!string.IsNullOrWhiteSpace(headerName))
            {
                switch (headerName)
                {
                    case "contact_noon":
                        name = "中午联系次数";
                        break;
                    case "phone_num_loc":
                        name = "号码归属地";
                        break;
                    case "contact_3m":
                        name = "最近三月联系次数";
                        break;
                    case "contact_1m":
                        name = "最近一月联系次数";
                        break;
                    case "contact_1w":
                        name = "最近一周联系次数";
                        break;
                    case "p_relation":
                        name = "关系推测";
                        break;
                    case "phone_num":
                        name = "号码";
                        break;
                    case "contact_name":
                        name = "号码标注";
                        break;
                    case "call_in_cnt":
                        name = "呼入次数";
                        break;
                    case "call_out_cnt":
                        name = "呼出次数";
                        break;
                    case "call_out_len":
                        name = "呼出时间";
                        break;
                    case "contact_holiday":
                        name = "节假日联系次数";
                        break;
                    case "needs_type":
                        name = "需求类别";
                        break;
                    case "contact_weekday":
                        name = "周中联系次数";
                        break;
                    case "contact_afternoon":
                        name = "下午联系次数";
                        break;
                    case "call_len":
                        name = "通话时长";
                        break;
                    case "contact_early_morning":
                        name = "凌晨联系次数";
                        break;
                    case "contact_night":
                        name = "晚上联系次数";
                        break;
                    case "contact_3m_plus":
                        name = "三个月以上联系次数";
                        break;
                    case "call_cnt":
                        name = "通话次数";
                        break;
                    case "call_in_len":
                        name = "呼入时间";
                        break;
                    case "contact_all_day":
                        name = "是否全天联系";
                        break;
                    case "contact_morning":
                        name = "上午联系次数";
                        break;
                    case "contact_weekend":
                        name = "周末联系次数";
                        break;
                    default:
                        name = headerName;
                        break;
                }
            }
            else
            {
                return headerName;
            }
            return name;
        }

    }
}
