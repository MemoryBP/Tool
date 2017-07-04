using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBaseTool.Model
{
    public class Check_black_info
    {
        /// <summary>
        /// 
        /// </summary>
        public string contacts_class1_blacklist_cnt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string contacts_router_ratio { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string contacts_class2_blacklist_cnt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string contacts_router_cnt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string contacts_class1_cnt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string phone_gray_score { get; set; }
    }

    public class Check_search_info
    {
        /// <summary>
        /// 
        /// </summary>
        public List<string> arised_open_web { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> phone_with_other_idcards { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> idcard_with_other_phones { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string register_org_cnt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> idcard_with_other_names { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> searched_org_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string searched_org_cnt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> register_org_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> phone_with_other_names { get; set; }
    }

    public class User_info_check
    {
        /// <summary>
        /// 
        /// </summary>
        public Check_black_info check_black_info { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Check_search_info check_search_info { get; set; }
    }

    public class Service_detailsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string interact_cnt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string interact_mth { get; set; }
    }

    public class Main_serviceItem
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Service_detailsItem> service_details { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string total_service_cnt { get; set; }
        /// <summary>
        /// 银行
        /// </summary>
        public string company_type { get; set; }
        /// <summary>
        /// 中信信用卡
        /// </summary>
        public string company_name { get; set; }
    }

    public class Behavior_checkItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string check_point { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string score { get; set; }
        /// <summary>
        /// 朋友圈在哪里
        /// </summary>
        public string check_point_cn { get; set; }
        /// <summary>
        /// 无数据
        /// </summary>
        public string result { get; set; }
        /// <summary>
        /// 未提供居住地址
        /// </summary>
        public string evidence { get; set; }
    }

    public class Contact_listItem
    {
        /// <summary>
        /// 号码
        /// </summary>
        public string phone_num { get; set; }
        /// <summary>
        /// 通话次数
        /// </summary>
        public int call_cnt { get; set; }
        /// <summary>
        /// 通话时长
        /// </summary>
        public double call_len { get; set; }
        /// <summary>
        /// 号码标注
        /// </summary>
        public string contact_name { get; set; }
        /// <summary>
        /// 需求类别
        /// </summary>
        public string needs_type { get; set; }
        /// <summary>
        /// 号码归属地
        /// </summary>
        public string phone_num_loc { get; set; }
        /// <summary>
        /// 呼出次数
        /// </summary>
        public int call_out_cnt { get; set; }
        /// <summary>
        /// 呼出时间
        /// </summary>
        public string call_out_len { get; set; }
        /// <summary>
        /// 呼入次数
        /// </summary>
        public int call_in_cnt { get; set; }
        /// <summary>
        /// 呼入时间
        /// </summary>
        public double call_in_len { get; set; }
        /// <summary>
        /// 关系推测
        /// </summary>
        public string p_relation { get; set; }
        /// <summary>
        /// 最近一月联系次数
        /// </summary>
        public int contact_1m { get; set; }
        /// <summary>
        /// 最近一周联系次数
        /// </summary>
        public int contact_1w { get; set; }
        /// <summary>
        /// 最近三月联系次数
        /// </summary>
        public int contact_3m { get; set; }
        /// <summary>
        /// 三个月以上联系次数
        /// </summary>
        public int contact_3m_plus { get; set; }
        /// <summary>
        /// 周中联系次数
        /// </summary>
        public int contact_weekday { get; set; }
        /// <summary>
        /// 凌晨联系次数
        /// </summary>
        public int contact_early_morning { get; set; }
        /// <summary>
        /// 上午联系次数
        /// </summary>
        public int contact_morning { get; set; }
        /// <summary>
        /// 中午联系次数
        /// </summary>
        public int contact_noon { get; set; }
        /// <summary>
        /// 下午联系次数
        /// </summary>
        public int contact_afternoon { get; set; }
        /// <summary>
        /// 晚上联系次数
        /// </summary>
        public int contact_night { get; set; }
        /// <summary>
        /// 周末联系次数
        /// </summary>
        public int contact_weekend { get; set; }
        /// <summary>
        /// 节假日联系次数
        /// </summary>
        public int contact_holiday { get; set; }
        /// <summary>
        /// 是否全天联系
        /// </summary>
        public string contact_all_day { get; set; }
    }

    public class Contact_regionItem
    {
        /// <summary>
        /// 上海
        /// </summary>
        public string region_loc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string region_uniq_num_cnt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double region_call_out_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double region_avg_call_in_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double region_call_in_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string region_call_out_cnt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double region_avg_call_out_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double region_call_in_cnt_pct { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double region_call_in_time_pct { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string region_call_in_cnt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double region_call_out_time_pct { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double region_call_out_cnt_pct { get; set; }
    }

    public class Check_points
    {
        /// <summary>
        /// 客户名
        /// </summary>
        public string key_value { get; set; }
    }

    public class Application_checkItem
    {
        /// <summary>
        /// 
        /// </summary>
        public Check_points check_points { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string app_point { get; set; }
    }

    public class Report
    {
        /// <summary>
        /// 
        /// </summary>
        public string rpt_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string token { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string version { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string update_time { get; set; }
    }

    public class BehaviorItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string sms_cnt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string cell_phone_num { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double net_flow { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double total_amount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double call_out_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string cell_mth { get; set; }
        /// <summary>
        /// 四川
        /// </summary>
        public string cell_loc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string call_cnt { get; set; }
        /// <summary>
        /// 四川移动
        /// </summary>
        public string cell_operator_zh { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string call_out_cnt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string cell_operator { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double call_in_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string call_in_cnt { get; set; }
    }

    public class Cell_behaviorItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string phone_num { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<BehaviorItem> behavior { get; set; }
    }

    public class Report_data
    {
        /// <summary>
        /// 
        /// </summary>
        public User_info_check user_info_check { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Main_serviceItem> main_service { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Behavior_checkItem> behavior_check { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> collection_contact { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Contact_listItem> contact_list { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> ebusiness_expense { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Contact_regionItem> contact_region { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Application_checkItem> application_check { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> deliver_address { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Report report { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Trip_infoItem> trip_info { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string _id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Cell_behaviorItem> cell_behavior { get; set; }
    }

    public class Trip_infoItem
    {
        /// <summary>
        /// 重庆
        /// </summary>
        public string trip_dest { get; set; }
        /// <summary>
        /// 四川
        /// </summary>
        public string trip_leave { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string trip_end_time { get; set; }
        /// <summary>
        /// 工作日
        /// </summary>
        public string trip_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string trip_start_time { get; set; }
    }

    public class Root
    {
        /// <summary>
        /// 获取报告成功
        /// </summary>
        public string note { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Report_data report_data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string success { get; set; }
    }
}