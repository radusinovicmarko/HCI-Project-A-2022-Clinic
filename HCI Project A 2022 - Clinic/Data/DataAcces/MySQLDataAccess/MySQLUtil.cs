using HCI_Project_A_2022___Clinic.Data.Model;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Data;

namespace HCI_Project_A_2022___Clinic.Data.DataAcces.MySQLDataAccess
{
    internal class MySQLUtil
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["MySqlClinicDB"].ConnectionString;
        private static readonly string LOGIN = "prijava";
        public static MySqlConnection GetConnection()
        {
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            return conn;
        }

        public static void CloseQuietly(MySqlConnection conn)
        {
            try
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            catch { }
        }

        public static void CloseQuietly(MySqlDataReader reader)
        {
            try
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            catch { }
        }

        public static void CloseQuietly(MySqlDataReader reader, MySqlConnection conn)
        {
            CloseQuietly(reader);
            CloseQuietly(conn);
        }

        public static bool Login(string username, string password)
        {
            MySqlConnection conn = null;
            MySqlCommand cmd;
            try
            {
                conn = GetConnection();
                cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = LOGIN;
                cmd.Parameters.AddWithValue("@korisnickoIme", username);
                cmd.Parameters["@korisnickoIme"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@lozinka", password);
                cmd.Parameters["@lozinka"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@prijava", MySqlDbType.Int32);
                cmd.Parameters["@prijava"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@idOsobe", MySqlDbType.Int32);
                cmd.Parameters["@idOsobe"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                return !((int) cmd.Parameters["@prijava"].Value == 0);
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in MySqlGroup", ex);
            }
            finally
            {
                CloseQuietly(conn);
            }
        }
    }
}
