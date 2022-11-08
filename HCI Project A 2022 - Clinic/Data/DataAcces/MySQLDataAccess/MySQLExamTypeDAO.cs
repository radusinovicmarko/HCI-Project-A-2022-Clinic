using HCI_Project_A_2022___Clinic.Data.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Project_A_2022___Clinic.Data.DataAcces.MySQLDataAccess
{
    internal class MySQLExamTypeDAO : IGenericDAO<ExamType>
    {
        private static readonly string SELECT_ALL = @"SELECT * FROM `vrsta_pregleda`";
        // private static readonly string SELECT = "SELECT * FROM `vrsta_pregleda` WHERE IdVrstePregleda=@IdVrstePregleda";
        private static readonly string INSERT = @"INSERT INTO `vrsta_pregleda`(SifraPregleda, NazivPregleda) VALUES (@Sifra, @Naziv)";
        private static readonly string UPDATE = @"UPDATE `vrsta_pregleda` SET SifraPregleda=@Sifra, NazivPregleda=@Naziv WHERE IdVrstePregleda=@IdVrstePregleda";
        private static readonly string DELETE = "DELETE FROM `vrsta_pregleda` WHERE IdVrstePregleda=@IdVrstePregleda";

        public int Add(ExamType item)
        {
            MySqlConnection conn = null;
            MySqlCommand cmd;
            try
            {
                conn = MySQLUtil.GetConnection();
                cmd = conn.CreateCommand();
                cmd.CommandText = INSERT;
                cmd.Parameters.AddWithValue("@Naziv", item.Name);
                cmd.Parameters.AddWithValue("@Sifra", item.Code);
                cmd.ExecuteNonQuery();
                item.ExamTypeId = (int)cmd.LastInsertedId;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in MySqlExamType", ex);
            }
            finally
            {
                MySQLUtil.CloseQuietly(conn);
            }
            return item.ExamTypeId;
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
                cmd.Parameters.AddWithValue("@IdVrstePregleda", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in MySqlExamType", ex);
            }
            finally
            {
                MySQLUtil.CloseQuietly(conn);
            }
        }

        public List<ExamType> GetAll()
        {
            List<ExamType> result = new List<ExamType>();
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
                    result.Add(new ExamType()
                    {
                        ExamTypeId = reader.GetInt32(0),
                        Code = reader.GetString(1),
                        Name = reader.GetString(2)
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in MySqlExamType", ex);
            }
            finally
            {
                MySQLUtil.CloseQuietly(reader, conn);
            }
            return result;
        }

        public void Update(int id, ExamType item)
        {
            MySqlConnection conn = null;
            MySqlCommand cmd;
            try
            {
                conn = MySQLUtil.GetConnection();
                cmd = conn.CreateCommand();
                cmd.CommandText = UPDATE;
                cmd.Parameters.AddWithValue("@Naziv", item.Name);
                cmd.Parameters.AddWithValue("@Sifra", item.Code);
                cmd.Parameters.AddWithValue("@IdVrstePregleda", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in MySqlExamType", ex);
            }
            finally
            {
                MySQLUtil.CloseQuietly(conn);
            }
        }
    }
}
