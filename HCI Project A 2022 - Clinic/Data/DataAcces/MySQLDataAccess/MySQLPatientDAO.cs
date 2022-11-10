using HCI_Project_A_2022___Clinic.Data.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Project_A_2022___Clinic.Data.DataAcces.MySQLDataAccess
{
    internal class MySQLPatientDAO : IGenericSearchDAO<Patient>
    {
        private static readonly string SELECT_ALL = @"SELECT * FROM `pacijent` p inner join `osoba` o 
                                on p.OSOBA_IdOsobe=o.IdOsobe inner join `mjesto` m on o.MJESTO_IdMjesta=m.IdMjesta
                                where true";
        // private static readonly string SELECT = "SELECT * FROM `mjesto` WHERE IdMjesta=@IdMjesta";
        private static readonly string INSERT = "dodaj_pacijenta";
        private static readonly string UPDATE = "izmijeni_pacijenta";
        // private static readonly string DELETE = "DELETE FROM `mjesto` WHERE IdMjesta=@IdMjesta";

        public int Add(Patient item)
        {
            MySqlConnection conn = null;
            MySqlCommand cmd;
            try
            {
                conn = MySQLUtil.GetConnection();
                cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = INSERT;
                cmd.Parameters.AddWithValue("@jmb", item.Jmb);
                cmd.Parameters["@jmb"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@ime", item.FirstName);
                cmd.Parameters["@ime"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@prezime", item.LastName);
                cmd.Parameters["@prezime"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@datum", item.DateOfBirth);
                cmd.Parameters["@datum"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@email", item.Email);
                cmd.Parameters["@email"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@adresa", item.Address);
                cmd.Parameters["@adresa"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@idMjesta", item.City.CityId);
                cmd.Parameters["@idMjesta"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@brojKartona", item.MedicalCardNumber);
                cmd.Parameters["@brojKartona"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@krvnaGrupa", item.BloodType);
                cmd.Parameters["@krvnaGrupa"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@bracniStatus", item.MarriageStatus);
                cmd.Parameters["@bracniStatus"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@telefon", item.PhoneNumber);
                cmd.Parameters["@telefon"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@idOsobe", MySqlDbType.Int32);
                cmd.Parameters["@idOsobe"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                return (int)cmd.Parameters["@idOsobe"].Value;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in MySqlGroup", ex);
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

        public List<Patient> Get(Patient item)
        {
            string selectQuery = SELECT_ALL;
            if (item.FirstName != null)
                selectQuery += " AND o.Ime LIKE @Ime";
            if (item.LastName != null)
                selectQuery += " AND o.Prezime LIKE @Prezime";
            if (item.Jmb != null)
                selectQuery += " AND o.Jmb LIKE @Jmb";
            List<Patient> result = new List<Patient>();
            MySqlConnection conn = null;
            MySqlCommand cmd;
            MySqlDataReader reader = null;

            try
            {
                conn = MySQLUtil.GetConnection();
                cmd = conn.CreateCommand();
                cmd.CommandText = selectQuery;
                if (item.FirstName != null)
                    cmd.Parameters.AddWithValue("@Ime", item.FirstName + "%");
                if (item.LastName != null)
                    cmd.Parameters.AddWithValue("@Prezime", item.LastName + "%");
                if (item.Jmb != null)
                    cmd.Parameters.AddWithValue("@Jmb", item.Jmb + "%");
                reader = cmd.ExecuteReader();
                while (reader.Read())
                    result.Add(Create(reader));
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in MySqlPatient", ex);
            }
            finally
            {
                MySQLUtil.CloseQuietly(reader, conn);
            }
            return result;
        }

        private Patient Create(MySqlDataReader reader)
        {
            return new Patient()
            {
                MedicalCardNumber = !reader.IsDBNull(0) ? reader.GetString(0) : null,
                BloodType = !reader.IsDBNull(1) ? reader.GetString(1) : null,
                MarriageStatus = !reader.IsDBNull(2) ? reader.GetString(2) : null,
                PhoneNumber = reader.GetString(3),
                PersonId = reader.GetInt32(5),
                Jmb = reader.GetString(6),
                FirstName = reader.GetString(7),
                LastName = reader.GetString(8),
                DateOfBirth = reader.GetDateTime(9),
                Email = !reader.IsDBNull(10) ? reader.GetString(10) : null,
                Address = !reader.IsDBNull(11) ? reader.GetString(11) : null,
                City = new City()
                {
                    CityId = reader.GetInt32(13),
                    CityName = reader.GetString(14),
                    PostCode = reader.GetString(15)
                }
            };
        }

        public List<Patient> GetAll()
        {
            List<Patient> result = new List<Patient>();
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
                throw new Exception("Exception in MySqlPatient", ex);
            }
            finally
            {
                MySQLUtil.CloseQuietly(reader, conn);
            }
            return result;
        }

        public void Update(int id, Patient item)
        {
            MySqlConnection conn = null;
            MySqlCommand cmd;
            try
            {
                conn = MySQLUtil.GetConnection();
                cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = UPDATE;
                cmd.Parameters.AddWithValue("@idOsobe", item.PersonId);
                cmd.Parameters["@idOsobe"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@email", item.Email);
                cmd.Parameters["@email"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@adresa", item.Address);
                cmd.Parameters["@adresa"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@bracniStatus", item.MarriageStatus);
                cmd.Parameters["@bracniStatus"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@telefon", item.PhoneNumber);
                cmd.Parameters["@telefon"].Direction = ParameterDirection.Input;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in MySqlGroup", ex);
            }
            finally
            {
                MySQLUtil.CloseQuietly(conn);
            }
        }
    }
}
