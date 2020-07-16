using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WebApplication1.Models
{
    public class jiance
    {
        static string pattern_guanli = @"\A[a-zA-Z0-9]{4,18}\z";
        static string pattern = @"\A[a-zA-Z0-9]{6,18}\z";
        static string pattern1 = @"\A[a-zA-Z0-9]{10}\z";
        static string sqlinjection = @"\A[\u4E00-\u9FA5a-zA-Z0-9]{3,10}\z";
        public static bool login_guanli(string str)
        {
            if(Regex.IsMatch(str, pattern_guanli))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool login(string str)
        {
            if(Regex.IsMatch(str, pattern))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool chongzhi(string str)
        {
            if(Regex.IsMatch(str, pattern1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool sql(string str)
        {
            if(Regex.IsMatch(str, sqlinjection))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool isshangjia(string cookie)
        {
            Express ex = new Express();
            string sql = "SELECT state FROM users WHERE cookie = '" + cookie + "'";
            DataSet ds = ex.Query(sql);

            if (ds.Tables[0].Rows[0]["state"].ToString() == "3")
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}