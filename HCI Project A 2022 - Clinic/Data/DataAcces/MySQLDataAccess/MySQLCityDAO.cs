using HCI_Project_A_2022___Clinic.Data.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HCI_Project_A_2022___Clinic.Data.DataAcces.MySQLDataAccess
{
    internal class MySQLCityDAO : IGenericDAO<City>
    {
        private static readonly string SELECT_ALL = @"SELECT * FROM `mjesto`";
        // private static readonly string SELECT = "SELECT * FROM `mjesto` WHERE IdMjesta=@IdMjesta";
        private static readonly string INSERT = @"INSERT INTO `mjesto`(Naziv, BrojPoste) VALUES (@Naziv, @BrojPoste)";
        private static readonly string UPDATE = @"UPDATE `mjesto` SET Naziv=@Naziv, BrojPoste=@BrojPoste WHERE IdMjesta=@IdMjesta";
        private static readonly string DELETE = "DELETE FROM `mjesto` WHERE IdMjesta=@IdMjesta";

        public int Add(City item)
        {
            MySqlConnection conn = null;
            MySqlCommand cmd;
            try
            {
                conn = MySQLUtil.GetConnection();
                cmd = conn.CreateCommand();
                cmd.CommandText = INSERT;
                cmd.Parameters.AddWithValue("@Naziv", item.CityName);
                cmd.Parameters.AddWithValue("@BrojPoste", item.PostCode);
                cmd.ExecuteNonQuery();
                item.CityId = (int)cmd.LastInsertedId;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in MySqlCity", ex);
            }
            finally
            {
                MySQLUtil.CloseQuietly(conn);
            }
            return item.CityId;
        }

        public void Delete(int id)
        {
            MySqlConnection conn = null;
            MySqlCommand cmd;
            try
            {
                conn = MySQLUtil.GetConnection();
                cmd = conn.CreateCommand();
                cmd.CommandText = DELETE;
                cmd.Parameters.AddWithValue("@IdMjesta", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in MySqlCity", ex);
            }
            finally
            {
                MySQLUtil.CloseQuietly(conn);
            }
        }

        public List<City> GetAll()
        {
            List<City> result = new List<City>();
            MySqlConnection conn = null;
            MySqlCommand cmd;
            MySqlDataReader reader = null;

            try
            {
                conn = MySQLUtil.GetConnection();
                cmd = conn.CreateCommand();
                cmd.CommandText = SELECT_ALL;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new City()
                    {
                        CityId = reader.GetInt32(0),
                        CityName = reader.GetString(1),
                        PostCode = reader.GetString(2)
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in MySqlCity", ex);
            }
            finally
            {
                MySQLUtil.CloseQuietly(reader, conn);
            }
            return result;
        }

        public void Update(int id, City item)
        {
            MySqlConnection conn = null;
            MySqlCommand cmd;
            try
            {
                conn = MySQLUtil.GetConnection();
                cmd = conn.CreateCommand();
                cmd.CommandText = UPDATE;
                cmd.Parameters.AddWithValue("@Naziv", item.CityName);
                cmd.Parameters.AddWithValue("@BrojPoste", item.PostCode);
                cmd.Parameters.AddWithValue("@IdMjesta", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in MySqlCity", ex);
            }
            finally
            {
                MySQLUtil.CloseQuietly(conn);
            }
        }
    }
}
