using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataBaseTool.Dao
{
    public interface ISqlHelper
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        /// <param name="str">连接字符串</param>
        /// <returns></returns>
        MySqlConnection Sqlconnection(String str);


        bool ExecuteQuery(MySqlConnection conn,string sql);

        MySqlDataReader ExecuteReader(string sql);
    }
}
