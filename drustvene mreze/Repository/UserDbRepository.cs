using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using drustvene_mreze.Domen;
using Microsoft.Data.Sqlite;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace drustvene_mreze.Repository
{
    public class UserDbRepository
    {
        public List<User> GetAll()
        {
            var users = new List<User>();
            try
            {
                using SqliteConnection connection = new SqliteConnection("Data Source=database/database.db");
                connection.Open();

                string query = "SELECT Id, Username, Name, Surname, Birthday FROM Users";
                using SqliteCommand command = new SqliteCommand(query, connection); // Objekat koji omogućava izvršavanje upita
                using SqliteDataReader reader = command.ExecuteReader(); // Izvršavamo upit i dobijamo rezultat

                while (reader.Read()) // Čitanje rezultata
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    string username = reader["Username"].ToString();
                    string name = reader["Name"].ToString();
                    string surname = reader["Surname"].ToString();
                    DateTime date = Convert.ToDateTime(reader["Birthday"]);

                    var user = new User
                    {
                        Id = id,
                        KorisnickoIme = username,
                        Ime = name,
                        Prezime = surname,
                        DatumRodjenja = date
                    };

                    users.Add(user);
                 }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
            }
            catch (Exception ex) 
            { 
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
            }
            return users;
        }

        public User? GetById(int id)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection("Data Source=database/database.db");
                connection.Open();

                string query = "SELECT Id, Username, Name, Surname, Birthday FROM Users WHERE Id = @id";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                using SqliteDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new User
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        KorisnickoIme = reader["Username"].ToString(),
                        Ime = reader["Name"].ToString(),
                        Prezime = reader["Surname"].ToString(),
                        DatumRodjenja = Convert.ToDateTime(reader["Birthday"])
                    };
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju SQL upita: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
            }

            return null;
        }

    }
}
