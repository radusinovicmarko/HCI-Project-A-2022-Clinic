using HCI_Project_A_2022___Clinic.Data.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Project_A_2022___Clinic.Data.DataAcces.MySQLDataAccess
{
    internal class MySQLReferralDAO : IGenericSearchDAO<Referral>
    {
        private static readonly string SELECT_ALL = @"SELECT * FROM `uputnica` u INNER JOIN `pregled` p
                            ON u.PREGLED_IdPregleda=p.IdPregleda WHERE true";
        private static readonly string INSERT = @"INSERT INTO `uputnica`(SifraUstanove, NazivOdredisneUstanove,
                            VrstaUputnice, PREGLED_IdPregleda) VALUES (@sifra, @naziv, @vrsta, @idPregleda)";
        public int Add(Referral item)
        {
            MySqlConnection conn = null;
            MySqlCommand cmd;
            try
            {
                conn = MySQLUtil.GetConnection();
                cmd = conn.CreateCommand();
                cmd.CommandText = INSERT;
                cmd.Parameters.AddWithValue("@sifra", item.InstitutionCode);
                cmd.Parameters.AddWithValue("@naziv", item.InstitutionName);
                cmd.Parameters.AddWithValue("@vrsta", item.Type);
                cmd.Parameters.AddWithValue("@idPregleda", item.Exam.ExamId);
                cmd.ExecuteNonQuery();
                item.ReferralId = (int)cmd.LastInsertedId;
                return item.ReferralId;
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

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Referral> Get(Referral item)
        {
            string selectQuery = SELECT_ALL;
            if (item.Exam != null)
            {
                selectQuery += " AND p.IdPregleda=@idPregleda";
                if (item.Exam.Patient != null)
                    selectQuery += " AND p.PACIJENT_OSOBA_IdOsobe=@idPacijenta";
                if (item.Exam.Doctor != null)
                    selectQuery += " AND p.LJEKAR_ZAPOSLENI_OSOBA_IdOsobe=@idLjekara";
            }
            List<Referral> result = new List<Referral>();
            MySqlConnection conn = null;
            MySqlCommand cmd;
            MySqlDataReader reader = null;

            try
            {
                conn = MySQLUtil.GetConnection();
                cmd = conn.CreateCommand();
                cmd.CommandText = selectQuery;
                if (item.Exam != null)
                {
                    cmd.Parameters.AddWithValue("@idPregleda", item.Exam.ExamId);
                    if (item.Exam.Patient != null)
                        cmd.Parameters.AddWithValue("@idPacijenta", item.Exam.Patient.PersonId);
                    if (item.Exam.Doctor != null)
                        cmd.Parameters.AddWithValue("@idLjekara", item.Exam.Doctor.PersonId);
                }
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

        public List<Referral> GetAll()
        {
            List<Referral> result = new List<Referral>();
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

        private Referral Create(MySqlDataReader reader)
        {
            return new Referral()
            {
                ReferralId = reader.GetInt32(0),
                InstitutionCode = reader.GetString(1),
                InstitutionName = reader.GetString(2),
                Type = reader.GetString(3),
                Exam = new Exam()
                {
                    ExamId = reader.GetInt32(5),
                    DateTime = reader.GetDateTime(6),
                    DiagnosisCode = reader.GetString(7),
                    Report = reader.GetString(8),
                    ExamType = new ExamType() { ExamTypeId = reader.GetInt32(9) },
                    Patient = new Patient() { PersonId = reader.GetInt32(10) },
                    Doctor = new Doctor() { PersonId = reader.GetInt32(11) }
                }

            };
        }

        public void Update(int id, Referral item)
        {
            throw new NotImplementedException();
        }
    }
}
