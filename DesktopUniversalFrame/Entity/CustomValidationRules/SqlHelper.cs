using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
        /// 查询=>返回第一行第一列
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

        /// <summary>
        /// 返回DataTable(Select) <返回结果比较少的>
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string sql, params MySqlParameter[] parameters)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionStr))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(parameters);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataSet dataset = new DataSet();
                    adapter.Fill(dataset);
                    return dataset.Tables[0];
                }
            }
        }

        /// <summary>
        /// 基于序号的查询(Select)
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static List<string> ExecuteDataReader(string sql, string columnName, params MySqlParameter[] parameters)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionStr))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(parameters);

                    List<string> list = new List<string>();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader.GetString(columnName));
                        }
                        return list;
                    }
                }
            }
        }
    }
}
