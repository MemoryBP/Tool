using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace DataBaseTool.Service.imp
{
    public static class ConvertService
    {
        /// <summary>
        /// 将List转换成DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dt = new DataTable();
            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];
                dt.Columns.Add(property.Name, property.PropertyType);
            }
            object[] values = new object[properties.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);
                }
                dt.Rows.Add(values);
            }
            return dt;
        }
        /// <summary>
        /// 验证身份证
        /// </summary>
        /// <param name="id_card">身份证号</param>
        /// <returns></returns>
        public static bool ConvertId_card(string id_card)
        {
            //\d{17}[\d|x]|\d{15}
            Regex re = new Regex(@"\d{17}[\d|x]|\d{15}");
            return re.Match(id_card).Success;
        }
    }
}
