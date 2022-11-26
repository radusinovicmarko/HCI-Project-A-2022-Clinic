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
    internal class MySQLEmployeeDAO : IGenericSearchDAO<Employee>
    {
        private static readonly string SELECT_ALL = @"SELECT * FROM `zaposleni` z inner join `osoba` o 
                                on z.OSOBA_IdOsobe=o.IdOsobe inner join `mjesto` m on o.MJESTO_IdMjesta=m.IdMjesta
                                where true";
        private static readonly string INSERT = "dodaj_zaposlenog";
        private static readonly string UPDATE = "izmijeni_zaposlenog";
        public int Add(Employee item)
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

        public List<Employee> Get(Employee item)
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
            List<Employee> result = new List<Employee>();
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

        public List<Employee> GetAll()
        {
            List<Employee> result = new List<Employee>();
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

        private Employee Create(MySqlDataReader reader)
        {
            return new Employee()
            {
                Salary = reader.GetDecimal(0),
                HireDate = reader.GetDateTime(1),
                Qualification = reader.GetString(2),
                Username = reader.GetString(3),
                Password = reader.GetString(4),
                Employed = reader.GetInt32(5) == 1,
                Role = (EmployeeRole) Enum.Parse(typeof(EmployeeRole), reader.GetString(6)),
                PersonId = reader.GetInt32(8),
                Jmb = reader.GetString(9),
                FirstName = reader.GetString(10),
                LastName = reader.GetString(11),
                DateOfBirth = reader.GetDateTime(12),
                Email = !reader.IsDBNull(13) ? reader.GetString(13) : null,
                Address = !reader.IsDBNull(14) ? reader.GetString(14) : null,
                City = new City()
                {
                    CityId = reader.GetInt32(16),
                    CityName = reader.GetString(17),
                    PostCode = reader.GetString(18)
                }
            };
        }

        public void Update(int id, Employee item)
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
                cmd.Parameters.AddWithValue("@datumZap", item.HireDate);
                cmd.Parameters["@datumZap"].Direction = ParameterDirection.Input;
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
