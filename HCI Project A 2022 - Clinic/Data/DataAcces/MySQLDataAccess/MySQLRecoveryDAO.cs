using HCI_Project_A_2022___Clinic.Data.Model;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Project_A_2022___Clinic.Data.DataAcces.MySQLDataAccess
{
    internal class MySQLRecoveryDAO : IGenericSearchDAO<Recovery>
    {
        private static readonly string SELECT_ALL = @"SELECT * FROM `preboljenje` p inner join `bolest` b
                            on p.BOLEST_IdBolesti=b.IdBolesti WHERE true";
        private static readonly string INSERT = @"INSERT INTO `preboljenje`(DatumPreboljenja, BOLEST_IdBolesti,
                            PACIJENT_OSOBA_IdOsobe, LJEKAR_ZAPOSLENI_OSOBA_IdOsobe) 
                            VALUES (@datum, @idBolesti, @idPacijenta, @idLjekara)";
        public int Add(Recovery item)
        {
            MySqlConnection conn = null;
            MySqlCommand cmd;
            try
            {
                conn = MySQLUtil.GetConnection();
                cmd = conn.CreateCommand();
                cmd.CommandText = INSERT;
                cmd.Parameters.AddWithValue("@datum", item.Date);
                cmd.Parameters.AddWithValue("@idBolesti", item.Illness.IllnessId);
                cmd.Parameters.AddWithValue("@idPacijenta", item.Patient.PersonId);
                cmd.Parameters.AddWithValue("@idLjekara", item.Doctor.PersonId);
                cmd.ExecuteNonQuery();
                item.RecoveryId = (int)cmd.LastInsertedId;
                return item.RecoveryId;
            }
            catch (Exception ex)
            {
                throw new Exception(Properties.Resources.DBError + " " + ex.Message, ex);
            }
            finally
            {
                MySQLUtil.CloseQuietly(conn);
            }
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Recovery> Get(Recovery item)
        {
            string selectQuery = SELECT_ALL;
            if (item.Date != null)
                selectQuery += " AND p.DatumPreboljenja=@datum";
            if (item.Patient != null)
                selectQuery += " AND p.PACIJENT_OSOBA_IdOsobe=@idPacijenta";
            if (item.Doctor != null)
                selectQuery += " AND p.LJEKAR_ZAPOSLENI_OSOBA_IdOsobe=@idLjekara";
            List<Recovery> result = new List<Recovery>();
            MySqlConnection conn = null;
            MySqlCommand cmd;
            MySqlDataReader reader = null;

            try
            {
                conn = MySQLUtil.GetConnection();
                cmd = conn.CreateCommand();
                cmd.CommandText = selectQuery;
                if (item.Date != null)
                    cmd.Parameters.AddWithValue("@datum", item.Date);
                if (item.Patient != null)
                    cmd.Parameters.AddWithValue("@idPacijenta", item.Patient.PersonId);
                if (item.Doctor != null)
                    cmd.Parameters.AddWithValue("@idLjekara", item.Doctor.PersonId);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                    result.Add(Create(reader));
            }
            catch (Exception ex)
            {
                throw new Exception(Properties.Resources.DBError + " " + ex.Message, ex);
            }
            finally
            {
                MySQLUtil.CloseQuietly(reader, conn);
            }
            return result;
        }

        public List<Recovery> GetAll()
        {
            List<Recovery> result = new List<Recovery>();
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
                    result.Add(Create(reader));
            }
            catch (Exception ex)
            {
                throw new Exception(Properties.Resources.DBError + " " + ex.Message, ex);
            }
            finally
            {
                MySQLUtil.CloseQuietly(reader, conn);
            }
            return result;
        }

        private Recovery Create(MySqlDataReader reader)
        {
            return new Recovery()
            {
                RecoveryId = reader.GetInt32(0),
                Date = reader.GetDateTime(1),
                Illness = new Illness() 
                { 
                    IllnessId = reader.GetInt32(2),
                    Name = reader.GetString(6),
                    Code = reader.GetString(7)
                },
                Patient = new MySQLPatientDAO().Get(new Patient() { PersonId = reader.GetInt32(3) })[0],
                Doctor = new MySQLDoctorDAO().Get(new Doctor() { PersonId = reader.GetInt32(4) })[0]
            };
        }

        public void Update(int id, Recovery item)
        {
            throw new NotImplementedException();
        }
    }
}
