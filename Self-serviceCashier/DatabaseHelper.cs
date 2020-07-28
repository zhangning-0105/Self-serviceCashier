/*----------------------------------------------------------------
// Copyright (C) 江苏秀园果科技有限公司
// 版权所有。
//
// 文件名：DatabaseHelper.cs
// 功能描述：
// 
// 创建标识：zhangning   2020/7/20 17:35:47  
// 
// 修改标识：
// 修改描述：
//
// 修改标识：
// 修改描述：
//
//----------------------------------------------------------------*/
using MySql.Data.MySqlClient;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Self_serviceCashier
{
  
    public class DatabaseHelper
    {
        public static string constr = ConfigurationManager.ConnectionStrings["constr"].ToString();

        public static MySqlConnection myCon = new MySqlConnection(constr);

        public static List<object[]> GetProductInformation(string epc)
        {
            try
            {
                myCon.Open();
                string sql = string.Format("select * from ProductInformation where productEPC='{0}'", epc);
                MySqlCommand mySqlCommand = new MySqlCommand(sql, myCon);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                List<object[]> list = new List<object[]>();
                while (mySqlDataReader.Read())
                {
                    if (mySqlDataReader.HasRows)
                    {
                        object[] row =
                        {
                          mySqlDataReader[1].ToString(),
                          mySqlDataReader[2].ToString(),
                            1.ToString(),
                          mySqlDataReader[3].ToString(),
                        
                       };
                        list.Add(row);
                    }
                }
                mySqlDataReader.Close();
                return list;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("查询商品信息出错", ex);
                return new List<object[]>();
            }
            finally
            {
                myCon.Close();
            }
        }
    }
}
