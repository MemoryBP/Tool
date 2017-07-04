using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;


namespace DataBaseTool.Dao.imp
{
    public class SqlHelper
    {
        private MySqlConnection mysqlconn;
        public SqlHelper()
        {    
        }

        public SqlHelper(string datasource, string userid, string password, string database)
        {
            string conStr = "data source=" + datasource + ";user id=" + userid + ";password=" + password + ";database=" + database + ";charset=utf8" + ";";
            //mysql = new MySqlConnection("server=localhost;user id=root;password=root;database=dali;charset=utf8");
            mysqlconn = new MySqlConnection(conStr);
        }

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>受影响的行数</returns>
        public int ExecuteNonQuery(string sql)
        {

            try
            {
                    MySqlConnection conn = mysqlconn;
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    MySqlDataAdapter sda = new MySqlDataAdapter(sql,conn);
                    DataSet ds = new DataSet();
                    sda.Fill(ds, "ds");
                    return ds.Tables[0].Rows.Count;
            }
            catch (Exception ex)
            {
                Logout.WriteLog(string.Format("执行SQL错误:{0}", ex.Message), LogType.ERROR, false);
                return -1;
            }
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="cmdTimeOut"></param>
        /// <returns></returns>
        public MySqlDataReader ReadDatas(string sql,int cmdTimeOut)
        {
            try
            {
                MySqlConnection conn = mysqlconn;
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandTimeout = cmdTimeOut;
                cmd.CommandText = sql;
                MySqlDataReader odr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (odr.Read())
                {
                    return odr;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logout.WriteLog(string.Format("读取数据错误:{0}", ex.Message), LogType.ERROR, false);
                return null;
            }

        }

        /// <summary>
        /// count查询
        /// </summary>
        /// <param name="sql">count查询</param>
        /// <returns></returns>
        public int GetTableCount(string sql)
        {
            try
            {
                MySqlConnection conn = mysqlconn;
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                DataTable dt = new DataTable();
                MySqlDataAdapter sda = new MySqlDataAdapter(sql, conn);
                sda.Fill(dt);
                return int.Parse(dt.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                Logout.WriteLog(string.Format("执行SQL错误:{0}", ex.Message), LogType.ERROR, false);
                return -1;
            }
        }

        /// <summary>
        /// 获取表单数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql)
        {
            try
            {
                MySqlConnection conn = mysqlconn;
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Open();
                MySqlDataAdapter sda = new MySqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {

                Logout.WriteLog(string.Format("获取表单数据错误:{0}", ex.Message), LogType.ERROR, false);
                return null;
            }
        }

        /// <summary>
        /// 运行存储过程
        /// </summary>
        /// <param name="ProcedureName">存储过程名称</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int RunProcedure(MySqlConnection con, string ProcedureName, params MySqlParameter[] parameters)
        {
            try
            {
                using (MySqlConnection conn = mysqlconn)
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(ProcedureName, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(parameters);
                        int i = cmd.ExecuteNonQuery();
                        conn.Close();
                        return i;
                    }
                }
            }
            catch (Exception ex)
            {

                Logout.WriteLog(string.Format("运行存储过程错误:{0}", ex.Message), LogType.ERROR, false);
                return -1;
            }
        }
        /// <summary>
        /// 检查连接
        /// </summary>
        public bool CheckConn()
        {
            bool flag = false;
            try
            {
                using (MySqlConnection conn = mysqlconn)
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        return flag = true;
                    }
                }

            }
            catch (Exception ex)
            {
                Logout.WriteLog(string.Format("检查连接错误:{0}", ex.Message), LogType.ERROR, false);
            }
            return flag;
        }
    }
}
