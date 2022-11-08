using HCI_Project_A_2022___Clinic.Data.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Project_A_2022___Clinic.Data.DataAcces.MySQLDataAccess
{
    internal class MySQLMedicationDAO : IGenericDAO<Medication>
    {
        private static readonly string SELECT_ALL = @"SELECT * FROM `lijek`";
        // private static readonly string SELECT = "SELECT * FROM `mjesto` WHERE IdLijeka=@IdLijeka";
        private static readonly string INSERT = @"INSERT INTO `lijek`(GenerickiNazivLijeka, TvornickiNazivLijeka) 
                            VALUES (@GenerickiNaziv, @TvornickiNaziv)";
        private static readonly string UPDATE = @"UPDATE `lijek` SET GenerickiNazivLijeka=@GenerickiNaziv, 
                            TvornickiNazivLijeka=@TvornickiNaziv WHERE IdLijeka=@IdLijeka";
        private static readonly string DELETE = "DELETE FROM `lijek` WHERE IdLijeka=@IdLijeka";

        public int Add(Medication item)
        {
            MySqlConnection conn = null;
            MySqlCommand cmd;
            try
            {
                conn = MySQLUtil.GetConnection();
                cmd = conn.CreateCommand();
                cmd.CommandText = INSERT;
                cmd.Parameters.AddWithValue("@GenerickiNaziv", item.GenericName);
                cmd.Parameters.AddWithValue("@TvornickiNaziv", item.FactoryName);
                cmd.ExecuteNonQuery();
                item.MedicationId = (int)cmd.LastInsertedId;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in MySqlMedication", ex);
            }
            finally
            {
                MySQLUtil.CloseQuietly(conn);
            }
            return item.MedicationId;
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
                cmd.Parameters.AddWithValue("@IdLijeka", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in MySqlMedication", ex);
            }
            finally
            {
                MySQLUtil.CloseQuietly(conn);
            }
        }

        public List<Medication> GetAll()
        {
            List<Medication> result = new List<Medication>();
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
                    result.Add(new Medication()
                    {
                        MedicationId = reader.GetInt32(0),
                        GenericName = reader.GetString(1),
                        FactoryName = reader.GetString(2)
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in MySqlMedication", ex);
            }
            finally
            {
                MySQLUtil.CloseQuietly(reader, conn);
            }
            return result;
        }

        public void Update(int id, Medication item)
        {
            MySqlConnection conn = null;
            MySqlCommand cmd;
            try
            {
                conn = MySQLUtil.GetConnection();
                cmd = conn.CreateCommand();
                cmd.CommandText = UPDATE;
                cmd.Parameters.AddWithValue("@GenerickiNaziv", item.GenericName);
                cmd.Parameters.AddWithValue("@TvornickiNaziv", item.FactoryName);
                cmd.Parameters.AddWithValue("@IdLijeka", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in MySqlMedication", ex);
            }
            finally
            {
                MySQLUtil.CloseQuietly(conn);
            }
        }
    }
}
