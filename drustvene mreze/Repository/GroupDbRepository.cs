using drustvene_mreze.Domen;
using Microsoft.Data.Sqlite;

namespace drustvene_mreze.Repository
{
    public class GroupDbRepository
    {
        public GroupDbRepository()
        {
        }

        public List<Grupe> GetAll()
        {
            List<Grupe> grupe = new List<Grupe>();
            try
            {
                string path = "database/database.db";
                using SqliteConnection connection = new SqliteConnection($"Data Source={path}");
                connection.Open();

                string query = "SELECT * FROM Groups";
                using SqliteCommand command = new SqliteCommand(query, connection);
                using SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string naziv = reader.GetString(1);
                    DateTime datumOsnivanja = reader.GetDateTime(2);
                    Grupe grupa = new Grupe(id, naziv, datumOsnivanja);
                    grupe.Add(grupa);
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

            return grupe; // Ensure a value is returned in all code paths
        }

        public Grupe GetById(int id)
        {
            try
            {

                string path = "database/database.db";
                using SqliteConnection connection = new SqliteConnection($"Data Source={path}");
                connection.Open();

                string query = "SELECT * FROM Groups WHERE Id = @Id";
                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@Id", id);
                using SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int Id = reader.GetInt32(0);
                    string naziv = reader.GetString(1);
                    DateTime datumOsnivanja = reader.GetDateTime(2);
                    Grupe grupa = new Grupe(Id, naziv, datumOsnivanja);
                    return grupa;
                }
                return null; // Dešava se ako ne postoji korisnik sa datim id
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

            return null; // Ensure a value is returned in all code paths
        }

    }
}
