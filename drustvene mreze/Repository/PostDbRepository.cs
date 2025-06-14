using drustvene_mreze.Domen;
using Microsoft.Data.Sqlite;

namespace drustvene_mreze.Repository
{
    public class PostDbRepository
    {
        private readonly string connectionString;

        public PostDbRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString:SQLiteConnection"];
        }

        public List<Post> GetAll()
        {
            var posts = new List<Post>();

            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"
                    SELECT 
                        Posts.Id,
                        Posts.Content,
                        Posts.Date,
                        Posts.UserId,
                        Users.Name || ' ' || Users.Surname AS UserName
                    FROM Posts
                    LEFT JOIN Users ON Users.Id = Posts.UserId
                ";

                using SqliteCommand command = new SqliteCommand(query, connection);
                using SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var post = new Post
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Content = reader["Content"].ToString(),
                        Date = reader["Date"].ToString(),
                        UserId = reader.IsDBNull(3) ? null : Convert.ToInt32(reader["UserId"]),
                        UserName = reader.IsDBNull(4) ? "Nepoznat korisnik" : reader["UserName"].ToString()
                    };

                    posts.Add(post);
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri izvršavanju SQL upita: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Neispravna konekcija: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
            }

            return posts;
        }
    }
}
