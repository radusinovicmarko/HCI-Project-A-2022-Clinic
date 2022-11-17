using HCI_Project_A_2022___Clinic.Data.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Project_A_2022___Clinic.Data.DataAcces.MySQLDataAccess
{
    internal class MySQLPrescriptionDAO : IGenericSearchDAO<Prescription>
    {
        private static readonly string SELECT_ALL = @"SELECT * FROM `recept` r INNER JOIN `pregled` p
                            ON r.PREGLED_IdPregleda=p.IdPregleda WHERE true";
        private static readonly string INSERT = @"INSERT INTO `recept`(DatumPropisivanja, NacinUpotrebe,
                            LIJEK_IdLijeka, PREGLED_IdPregleda) VALUES ( @datum, @nacin, @idLijeka, @idPregleda)";
        public int Add(Prescription item)
        {
            MySqlConnection conn = null;
            MySqlCommand cmd;
            try
            {
                conn = MySQLUtil.GetConnection();
                cmd = conn.CreateCommand();
                cmd.CommandText = INSERT;
                cmd.Parameters.AddWithValue("@datum", item.Date);
                cmd.Parameters.AddWithValue("@nacin", item.Instruction);
                cmd.Parameters.AddWithValue("@idLijeka", item.Medication.MedicationId);
                cmd.Parameters.AddWithValue("@idPregleda", item.Exam.ExamId);
                cmd.ExecuteNonQuery();
                item.PrescriptionId = (int)cmd.LastInsertedId;
                return item.PrescriptionId;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in MySqlExam", ex);
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

        public List<Prescription> Get(Prescription item)
        {
            string selectQuery = SELECT_ALL;
            if (item.Date != null)
                selectQuery += " AND r.DatumPropisivanja=@datum";
            if (item.Exam != null)
            {
                selectQuery += " AND r.PREGLED_IdPregleda=@idPregleda";
                if (item.Exam.Patient != null)
                    selectQuery += " AND r.PACIJENT_OSOBA_IdOsobe=@idPacijenta";
                if (item.Exam.Doctor != null)
                    selectQuery += " AND r.LJEKAR_ZAPOSLENI_OSOBA_IdOsobe=@idLjekara";
            }
            List<Prescription> result = new List<Prescription>();
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
                throw new Exception("Exception in MySqlExam", ex);
            }
            finally
            {
                MySQLUtil.CloseQuietly(reader, conn);
            }
            return result;
        }

        public List<Prescription> GetAll()
        {
            List<Prescription> result = new List<Prescription>();
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
                throw new Exception("Exception in MySqlExam", ex);
            }
            finally
            {
                MySQLUtil.CloseQuietly(reader, conn);
            }
            return result;
        }

        private Prescription Create(MySqlDataReader reader)
        {
            return new Prescription()
            {
                PrescriptionId = reader.GetInt32(0),
                Date = reader.GetDateTime(1),
                Instruction = reader.GetString(2),
                Medication = new MySQLMedicationDAO().Get(new Medication() { MedicationId = reader.GetInt32(3) })[0],
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

        public void Update(int id, Prescription item)
        {
            throw new NotImplementedException();
        }
    }
}
