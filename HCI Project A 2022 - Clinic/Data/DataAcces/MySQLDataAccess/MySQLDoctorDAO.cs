using HCI_Project_A_2022___Clinic.Data.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Project_A_2022___Clinic.Data.DataAcces.MySQLDataAccess
{
    internal class MySQLDoctorDAO : IGenericSearchDAO<Doctor>
    { 
        private static readonly string SELECT_ALL = @"SELECT * FROM `ljekar` l inner join `zaposleni` z
                                on l.ZAPOSLENI_OSOBA_IdOsobe=z.OSOBA_IdOsobe inner join `osoba` o 
                                on z.OSOBA_IdOsobe=o.IdOsobe inner join `mjesto` m on o.MJESTO_IdMjesta=m.IdMjesta
                                where true";
        private static readonly string INSERT = "dodaj_ljekara";
        public int Add(Doctor item)
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
                cmd.Parameters.AddWithValue("@plata", item.Salary);
                cmd.Parameters["@plata"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@datumZaposlenja", item.HireDate);
                cmd.Parameters["@datumZaposlenja"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@sprema", item.Qualification);
                cmd.Parameters["@sprema"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@korisnickoIme", item.Username);
                cmd.Parameters["@korisnickoIme"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@lozinka", item.Password);
                cmd.Parameters["@lozinka"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@zaposlen", item.Employed);
                cmd.Parameters["@zaposlen"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@uloga", item.Role);
                cmd.Parameters["@uloga"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@zvanje", item.Title);
                cmd.Parameters["@zvanje"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@idOsobe", MySqlDbType.Int32);
                cmd.Parameters["@idOsobe"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                return (int)cmd.Parameters["@idOsobe"].Value;
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

        public List<Doctor> Get(Doctor item)
        {
            string selectQuery = SELECT_ALL;
            if (item.PersonId != null)
                selectQuery += " AND o.IdOsobe = @id";
            if (item.FirstName != null)
                selectQuery += " AND o.Ime LIKE @ime";
            if (item.LastName != null)
                selectQuery += " AND o.Prezime LIKE @prezime";
            if (item.Jmb != null)
                selectQuery += " AND o.Jmb LIKE @jmb";
            if (item.Role != null)
                selectQuery += " AND z.Uloga=@uloga";
            if (item.Title != null)
                selectQuery += " AND l.Zvanje LIKE @zvanje";
            List<Doctor> result = new List<Doctor>();
            MySqlConnection conn = null;
            MySqlCommand cmd;
            MySqlDataReader reader = null;

            try
            {
                conn = MySQLUtil.GetConnection();
                cmd = conn.CreateCommand();
                cmd.CommandText = selectQuery;
                if (item.PersonId != null)
                    cmd.Parameters.AddWithValue("@id", item.PersonId);
                if (item.FirstName != null)
                    cmd.Parameters.AddWithValue("@ime", item.FirstName + "%");
                if (item.LastName != null)
                    cmd.Parameters.AddWithValue("@prezime", item.LastName + "%");
                if (item.Jmb != null)
                    cmd.Parameters.AddWithValue("@jmb", item.Jmb + "%");
                if (item.Role != null)
                    cmd.Parameters.AddWithValue("@uloga", item.Role.ToString());
                if (item.Title != null)
                    cmd.Parameters.AddWithValue("@zvanje", item.Title + "%");
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

        public List<Doctor> GetAll()
        {
            List<Doctor> result = new List<Doctor>();
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

        private Doctor Create(MySqlDataReader reader)
        {
            return new Doctor()
            {
                Title = reader.GetString(1),
                Salary = reader.GetDecimal(2),
                HireDate = reader.GetDateTime(3),
                Qualification = reader.GetString(4),
                Username = reader.GetString(5),
                Password = reader.GetString(6),
                Employed = reader.GetInt32(7) == 1,
                Role = (EmployeeRole)Enum.Parse(typeof(EmployeeRole), reader.GetString(8)),
                PersonId = reader.GetInt32(10),
                Jmb = reader.GetString(11),
                FirstName = reader.GetString(12),
                LastName = reader.GetString(13),
                DateOfBirth = reader.GetDateTime(14),
                Email = !reader.IsDBNull(15) ? reader.GetString(15) : null,
                Address = !reader.IsDBNull(16) ? reader.GetString(16) : null,
                City = new City()
                {
                    CityId = reader.GetInt32(18),
                    CityName = reader.GetString(19),
                    PostCode = reader.GetString(20)
                }
            };
        }

        public void Update(int id, Doctor item)
        {
            new MySQLEmployeeDAO().Update(id, item);
        }
    }
}
