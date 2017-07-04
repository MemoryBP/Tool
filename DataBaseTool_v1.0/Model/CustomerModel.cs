using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBaseTool.Model
{
    public class CustomerModel
    {
        /// <summary>
        /// 报告编号
        /// </summary>
        public string Rpt_id { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string Id_card { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public string Age { get; set; }
        /// <summary>
        /// 区县
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone_num { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public string Update_time { get; set; }
    }
}
