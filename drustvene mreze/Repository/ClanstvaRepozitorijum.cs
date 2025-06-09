using Microsoft.Data.Sqlite;

namespace drustvene_mreze.Repository
{
    public class ClanstvoRepozitorijum
    {
        private readonly string filePath = "data/clanstva.csv";
        // Assuming you might want to use a database connection string in the future
        private readonly string connectionString;

        public ClanstvoRepozitorijum(IConfiguration configuration)
        {
            // Constructor can be used to initialize configuration if needed
            connectionString = configuration["ConnectionString:SQLiteConnection"];
        }

        public void Create(int userId, int groupId)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "INSERT INTO GrupeClanstva (IdUser, IdGrupa) VALUES (@IdUser, @IdGrupa);";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@IdUser", userId);
                command.Parameters.AddWithValue("@IdGrupa", groupId);

                int affectedRows = command.ExecuteNonQuery(); // ⬅️ Koristi ExecuteNonQuery za INSERT

                if (affectedRows > 0)
                {
                    Console.WriteLine("Članstvo uspešno dodato!");
                }
                else
                {
                    Console.WriteLine("Nije dodato članstvo.");
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
        }

        public void Remove(int userId, int groupId)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "DELETE FROM GrupeClanstva WHERE IdUser = @IdUser AND IdGrupa = @IdGrupa;";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@IdUser", userId);
                command.Parameters.AddWithValue("@IdGrupa", groupId);

                int affectedRows = command.ExecuteNonQuery();

                if (affectedRows > 0)
                {
                    Console.WriteLine("Članstvo uspešno obrisano!");
                }
                else
                {
                    Console.WriteLine("Nije obrisano članstvo.");
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
        }

        public void Sacuvaj(List<(int UserId, int GroupId)> clanstva)
        {
            var lines = clanstva.Select(clanstvo => $"{clanstvo.UserId},{clanstvo.GroupId}");
            File.WriteAllLines(filePath, lines);
        }
        public void Add(int userId, int groupId)
        {
            var lines = new List<string>();
            if (File.Exists(filePath))
            {
                lines.AddRange(File.ReadAllLines(filePath));
            }
            lines.Add($"{userId},{groupId}");
            File.WriteAllLines(filePath, lines);
        }

        public List<(int UserId, int GroupId)> GetAll()
        {
            if (!File.Exists(filePath)) return new List<(int, int)>();

            return File.ReadAllLines(filePath)
                .Select(line => line.Split(','))
                .Where(parts => parts.Length == 2)
                .Select(parts => (int.Parse(parts[0]), int.Parse(parts[1])))
                .ToList();
        }

        public List<int> GetUserIdsByGroupId(int groupId)
        {
            return GetAll()
                .Where(pair => pair.GroupId == groupId)
                .Select(pair => pair.UserId)
                .ToList();
        }
    }

}
