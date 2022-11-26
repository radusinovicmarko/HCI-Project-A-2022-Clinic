using HCI_Project_A_2022___Clinic.Data.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Project_A_2022___Clinic.Data.DataAcces.MySQLDataAccess
{
    internal class MySQLIllnessDAO : IGenericDAO<Illness>
    {
        private static readonly string SELECT_ALL = @"SELECT * FROM `bolest`";
        private static readonly string INSERT = @"INSERT INTO `bolest`(NazivBolesti, SifraBolesti) VALUES (@Naziv, @Sifra)";
        private static readonly string UPDATE = @"UPDATE `bolest` SET Naziv=@Naziv, Sifra=@Sifra WHERE IdBolesti=@IdBolesti";
        private static readonly string DELETE = "DELETE FROM `bolest` WHERE IdBolesti=@IdBolesti";

        public int Add(Illness item)
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
                item.IllnessId = (int)cmd.LastInsertedId;
            }
            catch (Exception ex)
            {
                throw new Exception(Properties.Resources.DBError, ex);
            }
            finally
            {
                MySQLUtil.CloseQuietly(conn);
            }
            return item.IllnessId;
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
                cmd.Parameters.AddWithValue("@IdBolesti", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(Properties.Resources.DBError, ex);
            }
            finally
            {
                MySQLUtil.CloseQuietly(conn);
            }
        }

        public List<Illness> GetAll()
        {
            List<Illness> result = new List<Illness>();
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
                    result.Add(new Illness()
                    {
                        IllnessId = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Code = reader.GetString(2)
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(Properties.Resources.DBError, ex);
            }
            finally
            {
                MySQLUtil.CloseQuietly(reader, conn);
            }
            return result;
        }

        public void Update(int id, Illness item)
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
                cmd.Parameters.AddWithValue("@IdBolesti", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(Properties.Resources.DBError, ex);
            }
            finally
            {
                MySQLUtil.CloseQuietly(conn);
            }
        }
    }
}
