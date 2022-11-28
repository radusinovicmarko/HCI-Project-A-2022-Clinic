using HCI_Project_A_2022___Clinic.Data.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Project_A_2022___Clinic.Data.DataAcces.MySQLDataAccess
{
    internal class MySQLExamDAO : IGenericSearchDAO<Exam>
    {
        private static readonly string SELECT_ALL = @"SELECT * FROM `pregled` p inner join `vrsta_pregleda` v 
                            on p.VRSTA_PREGLEDA_IdVrstePregleda=v.IdVrstePregleda WHERE true";
        private static readonly string INSERT = @"INSERT INTO `pregled`(DatumVrijemePregleda, SifraDijagnoze, Nalaz,
                            VRSTA_PREGLEDA_IdVrstePregleda, PACIJENT_OSOBA_IdOsobe, LJEKAR_ZAPOSLENI_OSOBA_IdOsobe) 
                            VALUES (@datum, @sifra, @nalaz, @idVrstePregleda, @idPacijenta, @idLjekara)";
        public int Add(Exam item)
        {
            MySqlConnection conn = null;
            MySqlCommand cmd;
            try
            {
                conn = MySQLUtil.GetConnection();
                cmd = conn.CreateCommand();
                cmd.CommandText = INSERT;
                cmd.Parameters.AddWithValue("@datum", item.DateTime);
                cmd.Parameters.AddWithValue("@sifra", item.DiagnosisCode);
                cmd.Parameters.AddWithValue("@nalaz", item.Report);
                cmd.Parameters.AddWithValue("@idVrstePregleda", item.ExamType.ExamTypeId);
                cmd.Parameters.AddWithValue("@idPacijenta", item.Patient.PersonId);
                cmd.Parameters.AddWithValue("@idLjekara", item.Doctor.PersonId);
                cmd.ExecuteNonQuery();
                item.ExamId = (int)cmd.LastInsertedId;
                return item.ExamId;
            }
            catch (Exception ex)
            {
                throw new Exception(Properties.Resources.DBError , ex);
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

        public List<Exam> Get(Exam item)
        {
            string selectQuery = SELECT_ALL;
            if (item.DateTime != null)
                selectQuery += " AND p.DatumVrijemePregleda>=@datumOd AND p.DatumVrijemePregleda<=@datumDo";
            if (item.Patient != null)
                selectQuery += " AND p.PACIJENT_OSOBA_IdOsobe=@idPacijenta";
            if (item.Doctor != null)
                selectQuery += " AND p.LJEKAR_ZAPOSLENI_OSOBA_IdOsobe=@idLjekara";
            List<Exam> result = new List<Exam>();
            MySqlConnection conn = null;
            MySqlCommand cmd;
            MySqlDataReader reader = null;

            try
            {
                conn = MySQLUtil.GetConnection();
                cmd = conn.CreateCommand();
                cmd.CommandText = selectQuery;
                if (item.DateTime != null)
                {
                    cmd.Parameters.AddWithValue("@datumOd", item.DateTime);
                    cmd.Parameters.AddWithValue("@datumDo", new DateTime(item.DateTime.Value.Year, item.DateTime.Value.Month, item.DateTime.Value.Day, 23, 59, 59));
                }
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
                throw new Exception(Properties.Resources.DBError, ex);
            }
            finally
            {
                MySQLUtil.CloseQuietly(reader, conn);
            }
            return result;
        }

        public List<Exam> GetAll()
        {
            List<Exam> result = new List<Exam>();
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
                throw new Exception(Properties.Resources.DBError, ex);
            }
            finally
            {
                MySQLUtil.CloseQuietly(reader, conn);
            }
            return result;
        }

        private Exam Create(MySqlDataReader reader)
        {
            return new Exam()
            {
                ExamId = reader.GetInt32(0),
                DateTime = reader.GetDateTime(1),
                DiagnosisCode = reader.GetString(2),
                Report = reader.GetString(3),
                ExamType = new ExamType()
                {
                    ExamTypeId = reader.GetInt32(4),
                    Code = reader.GetString(8),
                    Name = reader.GetString(9),
                },
                Patient = new MySQLPatientDAO().Get(new Patient() { PersonId = reader.GetInt32(5) })[0],
                Doctor = new MySQLDoctorDAO().Get(new Doctor() { PersonId = reader.GetInt32(6) })[0]
            };
        }

        public void Update(int id, Exam item)
        {
            throw new NotImplementedException();
        }
    }
}
