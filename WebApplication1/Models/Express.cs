using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace WebApplication1.Models
{
    public class Express
    {
        public string connString = "Data Source = DESKTOP-TOTCTSB;Initial Catalog = test; User ID = sa; Pwd = Li114326";

        public SqlConnection conn;
        
        public DataSet Query(String sql)
        {
            conn = new SqlConnection(connString);
            DataSet ds = new DataSet();

            try
            {
                conn.Open();    //打开数据库
                SqlDataAdapter adp = new SqlDataAdapter(sql, conn);
                adp.Fill(ds);
            }
            catch
            {

            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();   //关闭数据库
            }
            return ds;
        }

        public int Execute(string sql)
        {
            conn = new SqlConnection(connString);
            int a = -1;
            try
            {
                conn.Open();    //打开数据库
                SqlCommand cmd = new SqlCommand(sql, conn);
                a = cmd.ExecuteNonQuery();
            }
            catch
            {

            }
            finally
            {
                if(conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return a;
        }
    }
}