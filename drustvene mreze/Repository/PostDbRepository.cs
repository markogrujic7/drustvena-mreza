using System.Globalization;
using drustvene_mreze.Domen;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Hosting;

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
                        Date = DateTime.Parse(reader["Date"].ToString()),
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

        public Post? GetById(int id)
        {
            try
            {
                using var connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT Id, UserId, Content, Date FROM Posts WHERE Id = @Id";
                using var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Post
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        Content = reader["Content"].ToString(),
                        Date = Convert.ToDateTime(reader["Date"])
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška pri čitanju objave: {ex.Message}");
            }

            return null;
        }


        public Post Create(Post post)
        {
            try
            {
                using var connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"
                INSERT INTO Posts (UserId, Content, Date)
                VALUES (@UserId, @Content, @Date);
            ";

                using var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", post.UserId);
                command.Parameters.AddWithValue("@Content", post.Content);
                command.Parameters.AddWithValue("@Date", post.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

                int rowsAffected = command.ExecuteNonQuery();

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

            return post;
        }

        public bool Delete(int id)
        {
            try
            {
                using var connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "DELETE FROM Posts WHERE Id = @Id";
                using var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška pri brisanju objave: {ex.Message}");
                return false;
            }
        }



    }
}
