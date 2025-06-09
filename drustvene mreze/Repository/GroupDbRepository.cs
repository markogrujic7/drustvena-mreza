using drustvene_mreze.Domen;
using Microsoft.Data.Sqlite;

namespace drustvene_mreze.Repository
{
    public class GroupDbRepository
    {
        private readonly string connectionString;

        public GroupDbRepository(IConfiguration configuration)
        {
            // Constructor can be used to initialize configuration if needed
            connectionString = configuration["ConnectionString:SQLiteConnection"];
        }

        public List<Grupe> GetPaged(int page , int pageSize)
        {
            List<Grupe> grupe = new List<Grupe>();
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT * FROM Groups LIMIT @PageSize OFFSET @Offset";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@PageSize", pageSize); // Example page size
                command.Parameters.AddWithValue("@Offset", pageSize * (page - 1)); // Example offset for pagination

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
                Grupe grupa = null;
                List<User> korisnici = new List<User>();

                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"
                    SELECT g.Id, g.Name, g.CreationDate, u.Id as UserId, u.Name, u.Surname, u.Username, u.Birthday
                    FROM Groups g
                    LEFT JOIN GrupeClanstva gc ON g.Id = gc.IdGrupa
                    LEFT JOIN Users u ON gc.IdUser = u.Id
                    WHERE g.Id = @Id";
                using SqliteCommand command = new SqliteCommand(query, connection);


                command.Parameters.AddWithValue("@Id", id);
                using SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (grupa == null)
                    {
                        int groupId = reader.GetInt32(0);
                        string naziv = reader.GetString(1);
                        DateTime datumOsnivanja = reader.GetDateTime(2);
                        grupa = new Grupe(groupId, naziv, datumOsnivanja);
                    }

                    if (reader["UserId"] != DBNull.Value)
                    {
                        User user = new User
                        {
                            Id = reader.GetInt32(3),
                            Ime = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Prezime = reader.IsDBNull(5) ? null : reader.GetString(5),
                            KorisnickoIme = reader.IsDBNull(6) ? null : reader.GetString(6),
                            DatumRodjenja = reader.IsDBNull(7) ? DateTime.MinValue : reader.GetDateTime(7)
                        };
                        korisnici.Add(user);
                    }

                    if (grupa != null)
                    {
                        grupa.clanovi = korisnici;
                    }
                }
                return grupa;
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

        public Grupe Create(Grupe novaGrupa)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "INSERT INTO Groups (Name, CreationDate) VALUES (@Name, @CreationDate); SELECT LAST_INSERT_ROWID();";
                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@Name", novaGrupa.naziv);
                command.Parameters.AddWithValue("@CreationDate", novaGrupa.datumOsnivanja);

                novaGrupa.id = Convert.ToInt32(command.ExecuteScalar());
                Console.WriteLine(novaGrupa.id);

                return novaGrupa;
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

        public Boolean Update(Grupe NovaGrupa)
        {
            try
            {
                string path = "database/database.db";
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "UPDATE Groups SET Name = @Column1, CreationDate = @Column2 WHERE Id = @Id";


                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@Column1", NovaGrupa.naziv);
                command.Parameters.AddWithValue("@Column2", NovaGrupa.datumOsnivanja);
                command.Parameters.AddWithValue("@Id", NovaGrupa.id);

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
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
            return false; // Ensure a value is returned in all code paths

        }

        public bool Delete(int id)
        {
            try
            {
                string path = "database/database.db";
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();


                string query = "DELETE FROM Groups WHERE Id = @Id";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);


                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
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
            return false; // Ensure a value is returned in all code paths
        }

        public int CountAll()
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT COUNT(*) FROM Groups;";
                using SqliteCommand command = new SqliteCommand(query, connection);


                int brojGrupa = Convert.ToInt32(command.ExecuteScalar());

                return brojGrupa;
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
            return 0; // Ensure a value is returned in all code paths
        }
    }
}
