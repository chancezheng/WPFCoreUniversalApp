using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace DesktopUniversalFrame.Entity
{
    public class SqlHelper
    {
        private static string connectionStr = ConfigurationManager.ConnectionStrings["connectionStr"].ConnectionString;

        /// <summary>
        /// 增删改
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int ExcuteModify(string commandText, params MySqlParameter[] parameters)
        {
            try
            {
                using (MySqlConnection sqlConnection = new MySqlConnection(connectionStr))
                {
                    sqlConnection.Open();
                    using (MySqlCommand cmd = new MySqlCommand(commandText, sqlConnection))
                    {
                        cmd.Parameters.AddRange(parameters);
                        int result = cmd.ExecuteNonQuery();
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 查询=>返回单一值
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static object ExcuteScalar(string commandText, params MySqlParameter[] parameters)
        {
            try
            {
                using (MySqlConnection sqlConnection = new MySqlConnection(connectionStr))
                {
                    sqlConnection.Open();
                    using (MySqlCommand cmd = new MySqlCommand(commandText, sqlConnection))
                    {
                        cmd.Parameters.AddRange(parameters);
                        return cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //string commandText = "insert into userinfo(username,password) values (@name,@pwd)";
        //SqlHelper.ExcuteModify(commandText,
        //    new MySqlParameter("@name", name),
        //    new MySqlParameter("@pwd", password));
    }
}
