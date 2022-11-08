using HCI_Project_A_2022___Clinic.Data.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Project_A_2022___Clinic.Data.DataAcces.MySQLDataAccess
{
    internal class MySQLAppointmentDAO : IGenericSearchDAO<Appointment>
    {
        private static readonly string SELECT_ALL = @"SELECT * FROM `narucenje` n WHERE true";
        private static readonly string INSERT = @"INSERT INTO `narucenje`(DatumVrijemePregleda, Razlog,
                            PACIJENT_OSOBA_IdOsobe, LJEKAR_ZAPOSLENI_OSOBA_IdOsobe) 
                            VALUES (@datum, @razlog, @idPacijenta, @idLjekara)";
        public int Add(Appointment item)
        {
            MySqlConnection conn = null;
            MySqlCommand cmd;
            try
            {
                conn = MySQLUtil.GetConnection();
                cmd = conn.CreateCommand();
                cmd.CommandText = INSERT;
                cmd.Parameters.AddWithValue("@datum", item.DateTime);
                cmd.Parameters.AddWithValue("@razlog", item.Reason);
                cmd.Parameters.AddWithValue("@idPacijenta", item.Patient.PersonId);
                cmd.Parameters.AddWithValue("@idLjekara", item.Doctor.PersonId);
                cmd.ExecuteNonQuery();
                item.AppointmentId = (int)cmd.LastInsertedId;
                return item.AppointmentId;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in MySqlAppointment", ex);
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

        public List<Appointment> Get(Appointment item)
        {
            string selectQuery = SELECT_ALL;
            if (item.DateTime != null)
                selectQuery += " AND n.DatumVrijemePregleda=@datum";
            if (item.Patient != null)
                selectQuery += " AND n.PACIJENT_OSOBA_IdOsobe=@idPacijenta";
            if (item.Doctor != null)
                selectQuery += " AND n.LJEKAR_ZAPOSLENI_OSOBA_IdOsobe=@idLjekara";
            List<Appointment> result = new List<Appointment>();
            MySqlConnection conn = null;
            MySqlCommand cmd;
            MySqlDataReader reader = null;

            try
            {
                conn = MySQLUtil.GetConnection();
                cmd = conn.CreateCommand();
                cmd.CommandText = selectQuery;
                if (item.DateTime != null)
                    cmd.Parameters.AddWithValue("@datum", item.DateTime);
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
                throw new Exception("Exception in MySqlAppointment", ex);
            }
            finally
            {
                MySQLUtil.CloseQuietly(reader, conn);
            }
            return result;
        }

        public List<Appointment> GetAll()
        {
            List<Appointment> result = new List<Appointment>();
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
                throw new Exception("Exception in MySqlAppointment", ex);
            }
            finally
            {
                MySQLUtil.CloseQuietly(reader, conn);
            }
            return result;
        }

        private Appointment Create(MySqlDataReader reader)
        {
            return new Appointment()
            {
                AppointmentId = reader.GetInt32(0),
                DateTime = reader.GetDateTime(1),
                Reason = reader.GetString(2),
                Patient = new Patient()
                {
                    PersonId = reader.GetInt32(3)
                },
                Doctor = new Doctor()
                {
                    PersonId = reader.GetInt32(4)
                }
            };
        }

        public void Update(int id, Appointment item)
        {
            throw new NotImplementedException();
        }
    }
}
