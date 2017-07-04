using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using DataBaseTool.Dao.imp;
using MySql.Data.MySqlClient;
using DataBaseTool.Model;
using System.Data;
using System.IO;
using System.Windows.Threading;
using DataBaseTool.Service.imp;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Win32;

namespace DataBaseTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        #region 定义初始变量
        private SqlHelper sqlhelper;
        private List<Root> reportList;
        private List<CustomerModel> customerList;
        private DBModel dataBaseModel;

        private bool isFirst = false;//是否首次加载
        private bool isRun = false;//线程是否正在执行

        private string getTableRows = "select count({0}) as number from {1}";
        private string getDatas = "select {0} from {1} where {2} limit {3},{4}";
        private string currentSqlStr = "";//当前正在使用的Sql
        private int limitCount = 15;//限制显示条数
        private int pageSize = 0;//页数
        private int tableRows = 0;//总行数
        private int currentPage = 1;//当前页

        #endregion


        public MainWindow()
        {
            sqlhelper = new SqlHelper();
            dataBaseModel = new DBModel();
            reportList = new List<Root>();
            customerList = new List<CustomerModel>();
            InitializeComponent();
            Dispatchtimer();
            Logout.WriteLog("初始化程序", LogType.INFO, false);
        }

        private void UIControlEnabled(bool isEnabled)
        {
            btnSelect.IsEnabled = isEnabled;
            btnAll.IsEnabled = isEnabled;
            btnExport.IsEnabled = isEnabled;
        }

        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConn_Click(object sender, RoutedEventArgs e)
        {
            sqlconn();
        }
        #region 数据库操作

        /// <summary>
        /// 连接数据库
        /// </summary>
        private void sqlconn()
        {
            string server = txtServer.Text.Trim();
            string dataBase = txtDataBase.Text.Trim();
            string user = txtUser.Text.Trim();
            string password = txtPwd.Password.Trim();
            if (string.IsNullOrWhiteSpace(server) || string.IsNullOrWhiteSpace(dataBase) || string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("请填写完整!", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            sqlhelper = new SqlHelper(server, user, password, dataBase);
            try
            {
                bool msg=false;
                msg = sqlhelper.CheckConn();
                if (!msg)
                {
                    publishMessage(string.Format("连接数据库'{0}'失败", dataBase));
                    MessageBox.Show("连接失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                publishMessage(string.Format("连接数据库'{0}'成功", dataBase));
                isFirst = true;
                UIControlEnabled(isFirst);
                initialPageSet(isFirst);
                if (isFirst)
                {
                    writeConConfig(server, dataBase, user, password);
                }
                ClearUIControl();
                string sql = string.Format(getDatas, "info", "access_report_data", "id in(select id from jsoninfo)", 0, limitCount); //"select acd.info from access_report_data acd where acd.info is not NULL and acd.info LIKE '%true%'";
                sumPage(getDatas, 1, "info", "access_report_data", "id in(select id from jsoninfo)");// and info is not NULL and info LIKE '%true%'

                getContactList(sql);
            }
            catch (Exception ex)
            {
                publishMessage("连接数据库出错:" + ex.Message);
            }
        }

        /// <summary>
        /// 连接字符串存入配置文件
        /// </summary>
        /// <param name="datasource"></param>
        /// <param name="database"></param>
        /// <param name="userid"></param>
        /// <param name="password"></param>
        private void writeConConfig(string datasource, string database, string userid, string password)
        {
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cfa.AppSettings.Settings["DataSource"].Value = datasource;
            cfa.AppSettings.Settings["DataBase"].Value = database;
            cfa.AppSettings.Settings["User"].Value = userid;
            cfa.AppSettings.Settings["Pwd"].Value = password;
            cfa.Save();
        }

        /// <summary>
        /// 读取连接字符串
        /// </summary>
        /// <returns></returns>
        private DBModel readConConfig()
        {
            string datasource = ConfigurationManager.AppSettings["DataSource"];
            string database = ConfigurationManager.AppSettings["DataBase"];
            string userid = ConfigurationManager.AppSettings["User"];
            string password = ConfigurationManager.AppSettings["Pwd"];
            DBModel db = new DBModel()
            {
                DataSource = datasource,
                DataBase = database,
                User = userid,
                Pwd = password
            };
            txtServer.Text = db.DataSource;
            txtDataBase.Text = db.DataBase;
            txtUser.Text = db.User;
            txtPwd.Password = db.Pwd;
            return db;
        }

        #endregion

        /// <summary>
        /// 打印信息窗数据
        /// </summary>
        /// <param name="msg"></param>
        private void publishMessage(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                txtPublish.Dispatcher.Invoke(new Action(() => { txtPublish.Text += string.Format("{0}\r\n", msg); }));

            }
            //滚动到最后一行
            txtPublish.Dispatcher.Invoke(new Action(() => { txtPublish.ScrollToEnd(); }));
        }

        #region 表单分页

        /// <summary>
        /// 初始化分页设置
        /// </summary>
        /// <param name="isEnabled"></param>
        private void initialPageSet(bool isEnabled)
        {
            if (isEnabled)
            {
                isRun = false;
                currentSqlStr = "";//当前正在使用的Sql
                limitCount = 15;//限制显示条数
                pageSize = 0;//页数
                tableRows = 0;//总行数
                currentPage = 1;//当前页
            }
        }

        /// <summary>
        /// 获取总页
        /// </summary>
        /// <param name="dataTableRows"></param>
        /// <returns></returns>
        private int getPageSize(int dataTableRows)
        {
            int pages = (dataTableRows / limitCount) + (dataTableRows % limitCount > 0 ? 1 : 0);
            pageSize = pages;
            pageView();
            return pages;
        }

        /// <summary>
        /// 表单数据统计
        /// </summary>
        private void pageView()
        {

            tbkCurrentsize.Dispatcher.BeginInvoke(new Action(() => { tbkCurrentsize.Text = currentPage.ToString(); }), null);
            tbkTotal.Dispatcher.BeginInvoke(new Action(() => { tbkTotal.Text = pageSize.ToString(); }), null);
            txtCount.Dispatcher.BeginInvoke(new Action(() =>
            {
                txtCount.Text = string.Format("从第{0}条到第{1}条数据(共{2}条数据)", ((limitCount * currentPage) - limitCount + 1), (currentPage == pageSize ? tableRows : (limitCount * currentPage)), tableRows);
            }), null);

        }

        /// <summary>
        /// 分页统计入口
        /// </summary>
        /// <param name="sqlStr">sql脚本</param>
        /// <param name="sqlType">1:select {0} from {1} where {2} limit {3},{4};2:自定义Sql</param>
        /// <param name="tableName">表名</param>
        private void sumPage(string sqlStr, int sqlType, string column = "*", string tableName = "", string where = "1=1")
        {
            switch (sqlType)
            {
                case 1:
                    sqlStr = string.Format(sqlStr.Trim().Replace("limit", "").Replace(",", ""), column, tableName, where, "", "");
                    sumPageBySqltype(sqlStr);
                    break;
                case 2:
                    sumPageBySqltype(sqlStr);
                    break;
                default:
                    break;
            }
        }

        private void sumPageBySqltype(string sqlStr)
        {
            //分页统计
            currentSqlStr = sqlStr;
            if (isFirst)
            {
                tableRows = sqlhelper.GetTableCount(string.Format(getTableRows, "id", "jsoninfo"));
                isFirst = false;

            }
            pageSize = getPageSize(tableRows);
        }
        /// <summary>
        /// 分页值绑定数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPageSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isRun)
            {
                return;
            }
            if (cmbPageSize.SelectedIndex != -1)
            {
                int index = cmbPageSize.SelectedIndex;
                switch (index)
                {
                    case 0:
                        limitCount = 15;
                        break;
                    case 1:
                        limitCount = 25;
                        break;
                    case 2:
                        limitCount = 50;
                        break;
                    case 3:
                        limitCount = 100;
                        break;
                    default:
                        limitCount = 15;
                        break;
                }
                if (tableRows > 0)
                {
                    //重新统计页数
                    currentPage = 1;
                    pageSize = getPageSize(tableRows);
                    changePage(currentPage, limitCount);
                }
            }

        }

        /// <summary>
        /// 下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPageNext_Click(object sender, RoutedEventArgs e)
        {
            if (!isRun)
            {
                if (tableRows > 0 && currentPage >= 1 && currentPage < pageSize)
                {
                    currentPage++;
                    tbkCurrentsize.Dispatcher.BeginInvoke(new Action(() => { tbkCurrentsize.Text = currentPage.ToString(); }), null);
                    changePage(currentPage, limitCount);
                }
            }
            
        }

        /// <summary>
        /// 上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPageUp_Click(object sender, RoutedEventArgs e)
        {
            if (!isRun)
            {
                if (tableRows > 0 && currentPage > 1 && currentPage <= pageSize)
                {
                    currentPage--;
                    tbkCurrentsize.Dispatcher.BeginInvoke(new Action(() => { tbkCurrentsize.Text = currentPage.ToString(); }), null);
                    changePage(currentPage, limitCount);
                }
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="currentpage"></param>
        /// <param name="limitcount"></param>
        private void changePage(int currentpage, int limitcount)
        {
            string sqlStr = currentSqlStr.Trim();
            if (!sqlStr.Contains("limit"))
            {
                sqlStr += " limit {0},{1}";
            }

            currentPage = currentpage;
            sqlStr = string.Format(sqlStr, ((currentpage - 1) * limitcount), limitcount);
            if (!string.IsNullOrEmpty(sqlStr))
            {
                Task.Factory.StartNew(() =>
                {
                    isRun = true;
                    getContactList(sqlStr);
                    pageView();
                });
            }
        }

        /// <summary>
        /// 分页跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPageGo_Click(object sender, RoutedEventArgs e)
        {
            if (isRun)
            {
                return;
            }
            string pageNum = tbxPageNum.Text.Trim();
            int pageGo = 0;
            if (!int.TryParse(pageNum, out pageGo))
            {
                MessageBox.Show("请输入数字!","错误",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(pageNum) || pageGo == currentPage || pageGo > pageSize || pageGo <= 0)
            {
                return;
            }
            changePage(pageGo, limitCount);
        }
        #endregion

        #region 系统时间显示

        private void Dispatchtimer()
        {
            DispatcherTimer time = new DispatcherTimer();
            time.Interval = TimeSpan.FromSeconds(1);
            time.Tick += new EventHandler(ShowTime);
            time.Start();
        }

        public void ShowTime(object sender, EventArgs e)
        {
            tblDateNow.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }
        #endregion


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (MessageBox.Show("是否退出?", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    bool? isDebug = chbIsDebug.IsChecked;
                    if (isDebug==true)
                    {
                        Logout.WriteLog("保存输出打印信息!",LogType.INFO,false);
                        savePublishContent(string.Format("{0}\r\n[结束程序]",txtPublish.Text.Trim()));
                    }
                    
                    System.Windows.Application.Current.Shutdown();
                    System.Environment.Exit(0);
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存打印信息
        /// </summary>
        private void savePublishContent(string publish)
        {
            string filePath = "";
            string publishText = publish;
            try
            {
                if (!string.IsNullOrWhiteSpace(publishText))
                {
                    filePath = string.Format(@".\Log\Publish{0}.txt", DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));//文件名
                    if (!Directory.Exists(@".\Log"))//若文件夹不存在则新建文件夹
                    {
                        Directory.CreateDirectory(@".\Log"); //新建文件夹
                    }
                    while (File.Exists(filePath))
                    {
                        filePath = string.Format(@".\Log\Publish{0}.txt", DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
                    }
                    filePath = AppDomain.CurrentDomain.BaseDirectory + filePath.TrimStart(new char[] { '.', '\\' });
                    StreamWriter sw = new StreamWriter(filePath);
                    sw.WriteLine(publishText);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                Logout.WriteLog(string.Format("保存打印文件错误:{0}", ex.Message), LogType.ERROR, false);
            }
        }


        #region 导出Excel
        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            string selectCus = txtSelectedUser.Text.Trim();
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "xls files(*.xls)|*.xls";
            saveFile.FileName = string.Format("{0}通话记录", selectCus == "" ? "客户" : selectCus);
            saveFile.DefaultExt = "xls";
            saveFile.AddExtension = true;

            saveFile.RestoreDirectory = true;
            bool? result = saveFile.ShowDialog();
            if (result == true)
            {
                string savePath = saveFile.FileName.ToString();

                //获取文件名，不带路径
                string fileNameExt = savePath.Substring(savePath.LastIndexOf("\\") + 1);

                ////获取文件路径，不带文件名
                string filePath = savePath.Substring(0, savePath.LastIndexOf("\\"));
                exportTopath(fileNameExt, filePath);
                ////给文件名前加上时间
                //string newFileName = fileNameExt + "_" + DateTime.Now.ToString("yyyyMMdd");
                //newFileName = FilePath + "\\" + newFileName;

                //在文件名里加字符
                //saveFileDialog.FileName.Insert(1,"dameng");
                //为用户使用 SaveFileDialog 选定的文件名创建读/写文件流。
                //System.IO.File.WriteAllText(newFileName, wholestring); //这里的文件名其实是含有路径的。
            }

        }

        private void exportTopath(string fileName, string filePath)
        {
            try
            {
                List<Contact_listItem> phoneList = this.dgMeg.ItemsSource as List<Contact_listItem>;
                if (phoneList != null)
                {
                    DataTable dt = ConvertService.ToDataTable<Contact_listItem>(phoneList);
                    new ExportService().exportResult(dt, fileName, filePath);
                    publishMessage("导出路径：" + filePath + "\\" + fileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "异常错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        } 
        #endregion

        #region 自动删除文件夹内文件
        /// <summary>
        /// 清理文件
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <param name="num">清理条数(需总文件数>清理条数)</param>
        private void cleanWordList(string path, int num)
        {
            try
            {
                string[] files = Directory.GetFiles(path);
                if (files.Length < num)
                {
                    return;
                }
                for (int i = 0; i < num; i++)
                {
                    File.Delete(files[i]);
                    txtPublish.Dispatcher.Invoke(new Action(() =>
                    {
                        publishMessage(string.Format("已删除：{0}/{1} {2}", i + 1, files.Length, files[i].Replace(AppDomain.CurrentDomain.BaseDirectory, "")));
                    }));
                }
            }
            catch (Exception ex)
            {
                Logout.WriteLog(string.Format("自动清理文件错误:{0}", ex.Message), LogType.ERROR, false);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dataBaseModel = readConConfig();
            UIControlEnabled(isFirst);
            //string path = AppDomain.CurrentDomain.BaseDirectory + "WordList";
            //cleanWordList(path, 5);
        }
        #endregion

        #region 获取通话记录

        /// <summary>
        /// 获取用户和通话记录
        /// </summary>
        private void getContactList(string sql)
        {
            if (reportList != null)
            {
                reportList = new List<Root>();
                customerList = new List<CustomerModel>();
            }
                Task.Factory.StartNew(() =>
                {
                    getPhone(sql);
                });
        }

        private void getPhone(string sql)
        {
            publishMessage(string.Format("正在获取客户信息..."));
            try
            {
                MySqlDataReader reader = sqlhelper.ReadDatas(sql, 600);
                if (reader == null)
                {
                    publishMessage("数据无效!\r\nFinish!");
                    return;
                }
                reportList = new List<Root>();
                if (reader.HasRows)
                {
                    string s = reader["info"].ToString();
                    if (s.Contains("true"))
                    {
                        JsonResult(s, reportList);
                    }
                }
                else
                {
                    publishMessage("Finish!");
                    return;
                }
                while (reader.Read())
                {
                    try
                    {
                        string s = reader["info"].ToString();
                        if (s.Contains("true"))
                        {
                            JsonResult(s, reportList);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logout.WriteLog(string.Format("获取数据异常:{0}", ex.Message), LogType.ERROR, false);
                    }
                }
                publishMessage(string.Format("Finish!"));
            }
            catch (Exception ex)
            {
                Logout.WriteLog(string.Format("获取数据异常:{0}", ex.Message), LogType.ERROR, false);
                publishMessage("获取数据异常,请检查Sql语句");
            }
            finally
            {

                if (reportList != null && reportList.Count > 0)
                {
                    bindReortList(reportList);
                }
                isRun = false;
            }
        }
        /// <summary>
        /// Json解析
        /// </summary>
        /// <param name="JsonText">传入的Json字符串</param>
        /// <returns>返回集合List</returns>
        public void JsonResult(string JsonText, List<Root> rootList)
        {
            //JObject jo = (JObject)JsonConvert.DeserializeObject(JsonText);

            try
            {

                Root report = JsonConvert.DeserializeObject<Root>(JsonText);

                string token = report.note;
                string success = report.success;
                if (!success.Equals("true"))
                {
                    return;
                }
                rootList.Add(report);
                string userName = "";
                string id_card = "";
                List<Application_checkItem> app = report.report_data.application_check;
                userName = app[0].check_points.key_value;
                id_card = app[1].check_points.key_value;
                publishMessage("用户名:" + userName+"，身份证："+id_card);
            }
            catch (Exception ex)
            {
                Logout.WriteLog(string.Format("Json解析异常:{0}", ex.Message), LogType.ERROR, false);
            }
        }
        /// <summary>
        /// 绑定获取json数据集到Grid
        /// </summary>
        /// <param name="list"></param>
        private void bindReortList(List<Root> list)
        {
            //List<Contact_listItem> app = new List<Contact_listItem>();
            //app = list[0].report_data.contact_list;
            //app.Sort(delegate(Contact_listItem x, Contact_listItem y) { return Convert.ToInt32(y.call_cnt).CompareTo(Convert.ToInt32(x.call_cnt)); });
            customerList = new List<CustomerModel>();

            try
            {
                foreach (var item in list)
                {
                    CustomerModel customer = new CustomerModel();
                    string rpt_id = item.report_data.report.rpt_id;
                    string name = item.report_data.application_check[0].check_points.key_value;
                    string phone = item.report_data.cell_behavior[0].phone_num;
                    string updateTime = item.report_data.report.update_time;
                    string id_card = item.report_data.application_check[1].check_points.key_value;

                    customer.Rpt_id = rpt_id == "" ? "未知" : rpt_id;
                    customer.UserName = name == "" ? "未知" : name;
                    customer.Phone_num = phone == "" ? "未知" : phone;
                    customer.Update_time = updateTime == "" ? "未知" : updateTime;
                    customer.Id_card = id_card == "" ? "未知" : id_card;
                    customerList.Add(customer);
                }
                dgCustomers.Dispatcher.BeginInvoke(new Action(() => { dgCustomers.ItemsSource = customerList; dgCustomers.SelectedIndex = 0; }));
                //dgMeg.Dispatcher.BeginInvoke(new Action(() => { dgMeg.ItemsSource = list[0].report_data.contact_list; }));
                //txtSelectedUser.Dispatcher.BeginInvoke(new Action(() => { txtSelectedUser.Text = customerList[0].UserName; }));
                //txtUpdateTime.Dispatcher.BeginInvoke(new Action(() => { txtUpdateTime.Text = customerList[0].Update_time.ToLower().Replace("z"," ").Replace("t"," "); }));
                BindUIControl();
            }
            catch (Exception ex)
            {
                Logout.WriteLog(string.Format("绑定数据集异常:{0}", ex.Message), LogType.ERROR, false);
            }
        }

        #endregion

        private void dgCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgCustomers.Items.Count == 0)
            {
                return;
            }
            CustomerModel customerItem = new CustomerModel();
            List<Root> selected_root = new List<Root>();
            List<CustomerModel> cusList = new List<CustomerModel>();
            selected_root = reportList;
            if (cu_root!=null && cu_root.Count>0)
            {
                selected_root = cu_root;
            }
            try
            {
                customerItem = dgCustomers.SelectedItem as CustomerModel;
                if (customerItem != null)
                {
                    string rpt_id = customerItem.Rpt_id;
                    txtSelectedUser.Text = customerItem.UserName;
                    txtUpdateTime.Text = customerItem.Update_time.ToLower().Replace("z", " ").Replace("t", " ");
                    if (!string.IsNullOrWhiteSpace(rpt_id))
                    {
                        foreach (var item in selected_root)
                        {
                            string id = item.report_data.report.rpt_id;
                            if (rpt_id.Equals(id))
                            {
                                List<Contact_listItem> app = new List<Contact_listItem>();
                                app = item.report_data.contact_list;
                                app.Sort(delegate(Contact_listItem x, Contact_listItem y) { return Convert.ToInt32(y.call_cnt).CompareTo(Convert.ToInt32(x.call_cnt)); });

                                dgMeg.ItemsSource = null;
                                dgMeg.ItemsSource = app;
                            }
                        }
                        BindUIControl();
                    }
                }
                
            }
            catch (Exception ex)
            {
                Logout.WriteLog(string.Format("获取客户详细异常:{0}", ex.Message), LogType.ERROR, false);
            }
        }

        private void BindUIControl()
        {
            tblCurrentCount.Dispatcher.BeginInvoke(new Action(() => { tblCurrentCount.Text = string.Format("当前{0}条数据", dgCustomers.Items.Count); }));
        }

        /// <summary>
        /// 恢复控件默认值
        /// </summary>
        private void ClearUIControl()
        {

            dgMeg.Dispatcher.BeginInvoke(new Action(() => { dgMeg.ItemsSource = null; }));
            dgCustomers.Dispatcher.BeginInvoke(new Action(() => { dgCustomers.ItemsSource = null; }));
            txtUpdateTime.Dispatcher.BeginInvoke(new Action(() => { txtUpdateTime.Text = "NULL"; }));
            txtSelectedUser.Dispatcher.BeginInvoke(new Action(() => { txtSelectedUser.Text = "NULL"; }));
            //tblCurrentCount.Dispatcher.BeginInvoke(new Action(() => { tblCurrentCount.Text = "当前0条数据"; }));
            //tbkTotal.Dispatcher.BeginInvoke(new Action(() => { tbkTotal.Text = "0"; }));
            //tbkCurrentsize.Dispatcher.BeginInvoke(new Action(() => { tbkCurrentsize.Text = "0"; }));
            tbxPageNum.Dispatcher.BeginInvoke(new Action(() => { tbxPageNum.Text = ""; }));
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            string selectStr = txtUserName.Text.Trim().ToLower();
            List<CustomerModel> cusList = new List<CustomerModel>();
            if (!string.IsNullOrWhiteSpace(selectStr))
            {
                if (!ConvertService.ConvertId_card(selectStr))
                {
                    publishMessage("身份证输入错误!");
                    txtUserName.Text = "";
                    return;
                }
                
                publishMessage(string.Format("正在查询客户通话记录,通过身份证{0}", selectStr));

                string sql = "select acd.info from access_report_data acd where acd.id in(select id from jsoninfo acd where acd.id_no='{0}')";
                sql = string.Format(sql, selectStr);
                if (!isRun)
                {
                    Task.Factory.StartNew(() =>
                    {
                        isRun = true;
                        selectById(sql, 180);
                    });
                }
                
            }
        }

        private List<Root> cu_root;

        /// <summary>
        /// 通过身份证条件查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="timeOut"></param>
        private void selectById(string sql, int timeOut)
        {
            cu_root = new List<Root>();
            try
            {

                MySqlDataReader reader = sqlhelper.ReadDatas(sql, timeOut);//超时3分钟
                if (reader == null)
                {
                    publishMessage("数据无效!\r\nFinish!");
                    return;
                }
                if (reader.HasRows)
                {
                    ClearUIControl();
                    string s = reader["info"].ToString();
                    if (s.Contains("true"))
                    {
                        JsonResult(s, cu_root);
                    }
                }
                else
                {
                    publishMessage("Finish!");
                    return;
                }
                while (reader.Read())
                {
                    try
                    {
                        string s = reader["info"].ToString();
                        if (s.Contains("true"))
                        {
                            JsonResult(s, cu_root);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logout.WriteLog(string.Format("循环读取数据异常:{0}", ex.Message), LogType.ERROR, false);
                    }
                }
                publishMessage("Finish!");
            }
            catch (Exception ex)
            {
                Logout.WriteLog(string.Format("查询数据异常:{0}", ex.Message), LogType.ERROR, false);
                publishMessage("查询失败");
            }
            finally
            {
                if (cu_root != null && cu_root.Count > 0)
                {
                    bindReortList(cu_root);
                }
                else
                {
                    publishMessage("查询失败");
                }
                isRun = false;
            }

        }

        private void btnAll_Click(object sender, RoutedEventArgs e)
        {
            if (reportList != null && reportList.Count > 0)
            {
                bindReortList(reportList);
                cu_root = new List<Root>();
            }
            
        }
    }
}