using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using drustvene_mreze.Domen;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace drustvene_mreze.Repository
{
    public class UserDbRepository
    {

        private readonly string connectionString;

        public UserDbRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString:SQLiteConnection"];
        }

        public List<User> GetAll(int page, int pageSize)
        {
            var users = new List<User>();
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"SELECT Id, Username, Name, Surname, Birthday FROM Users LIMIT @limit OFFSET @offset";
                using SqliteCommand command = new SqliteCommand(query, connection); // Objekat koji omogućava izvršavanje upita
                command.Parameters.AddWithValue("@limit", pageSize);
                command.Parameters.AddWithValue("@offset", (page - 1) * pageSize);
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
                using SqliteConnection connection = new SqliteConnection(connectionString);
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

        public User? Create(User user)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"
                    INSERT INTO Users (Username, Name, Surname, Birthday) 
                    VALUES (@username, @name, @surname, @birthday);
                    SELECT last_insert_rowid();";

                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@username", user.KorisnickoIme);
                command.Parameters.AddWithValue("@name", user.Ime);
                command.Parameters.AddWithValue("@surname", user.Prezime);
                command.Parameters.AddWithValue("@birthday", user.DatumRodjenja);

                user.Id = Convert.ToInt32(command.ExecuteScalar());
                Console.WriteLine(user.Id);

                return user;
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

        public bool Update(User user)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"
                    UPDATE Users
                    SET Username = @username, Name = @name, Surname = @surname, Birthday = @birthday
                    WHERE Id = @id";

                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@username", user.KorisnickoIme);
                command.Parameters.AddWithValue("@name", user.Ime);
                command.Parameters.AddWithValue("@surname", user.Prezime);
                command.Parameters.AddWithValue("@birthday", user.DatumRodjenja);
                command.Parameters.AddWithValue("@id", user.Id);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
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

            return false;
        }

        public bool Delete(int id)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "DELETE FROM Users WHERE Id = @id";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
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

            return false;
        }

        public int CountAll()
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT COUNT(*) FROM Users";
                using var command = new SqliteCommand(query, connection);
                return Convert.ToInt32(command.ExecuteScalar());
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

            return -1;
        }
    }
}
