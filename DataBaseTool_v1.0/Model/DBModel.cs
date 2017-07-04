using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBaseTool.Model
{
    public class DBModel
    {
        private string dataSource;
        /// <summary>
        /// 数据源
        /// </summary>
        public string DataSource
        {
            get { return dataSource; }
            set { dataSource = value; }
        }
        private string dataBase;
        /// <summary>
        /// 数据库
        /// </summary>
        public string DataBase
        {
            get { return dataBase; }
            set { dataBase = value; }
        }
        private string user;
        /// <summary>
        /// 用户名
        /// </summary>
        public string User
        {
            get { return user; }
            set { user = value; }
        }
        private string pwd;
        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd
        {
            get { return pwd; }
            set { pwd = value; }
        }
    }
}
