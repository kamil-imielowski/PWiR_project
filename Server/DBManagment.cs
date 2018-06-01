using SharedLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class DBManagment : IContract
    {
        public List<Brand> GetBrands()
        {
            List<Brand> results = new List<Brand>();

            //zdefiniowanie polaczenia do db (conection string w app.config o nazwie dbstr)
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbstr"].ToString());
            //otworzenia polaczenia z baza
            conn.Open();
            // inizjalizacja komendy sql z polaczeniem do db
            SqlCommand cmd = new SqlCommand("Select * from Brand", conn);
            //Wyslanie CommandText do Connection i kompilacje SqlDataReader.
            //SqlDataReader umozliwia odczyt strumiena rekordow z bazy (tylko do przodu)
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Brand n = new Brand();
                n.ID = reader.GetInt32(0);
                n.Name = reader.GetString(1);
                results.Add(n);
            }
            reader.Close();
            conn.Close();

            return results;
        }

        public List<Phone> GetPhones()
        {
            List<Phone> results = new List<Phone>();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbstr"].ToString());
            conn.Open();
            SqlCommand cmd = new SqlCommand("Select * from Phone INNER JOIN Brand ON Phone.BrandID = Brand.ID;", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Phone p = new Phone();
                Brand b = new Brand();
                b.ID = (int)reader.GetValue(reader.GetOrdinal("BrandID"));
                b.Name = (string)reader.GetValue(reader.GetOrdinal("Name"));
                p.ID = (int)reader.GetValue(reader.GetOrdinal("ID"));
                p.Brand = b;
                p.Model = (string)reader.GetValue(reader.GetOrdinal("Model"));
                p.Processor = (string)reader.GetValue(reader.GetOrdinal("Processor"));
                p.Ram = (string)reader.GetValue(reader.GetOrdinal("Ram"));
                p.Memory = (string)reader.GetValue(reader.GetOrdinal("Memory"));
                p.Graphic = (string)reader.GetValue(reader.GetOrdinal("Graphic"));
                p.Description = (string)reader.GetValue(reader.GetOrdinal("Description"));
                p.Price = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Price")));
                p.Premiere = (DateTime)reader.GetValue(reader.GetOrdinal("Premiere"));
                results.Add(p);
            }
            reader.Close();
            conn.Close();
            return results;
        }

        public bool AddBrand(string name)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbstr"].ToString());
            conn.Open();
            SqlCommand cmd = new SqlCommand($"INSERT INTO Brand(Name) VALUES('{name}')", conn);
            if (cmd.ExecuteNonQuery() != 1)
            {
                return false;
            }
            return true;
        }

        public bool AddPhone(Phone phone)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbstr"].ToString());
            conn.Open();
            SqlCommand cmd = new SqlCommand(
                $"INSERT INTO Phone(BrandID, Model, Processor, Ram, Memory, Graphic, Description, Price, Premiere)"+
                $"VALUES ({phone.Brand.ID},'{phone.Model}','{phone.Processor}','{phone.Ram}','{phone.Memory}','{phone.Graphic}','{phone.Description}',{phone.Price},'{phone.Premiere.ToString("yyyy-MM-dd")}')"
                , conn);
            if (cmd.ExecuteNonQuery() != 1)
            {
                return false;
            }
            return true;
        }
    }
}
