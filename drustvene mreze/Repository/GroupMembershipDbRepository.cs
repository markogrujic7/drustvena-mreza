using drustvene_mreze.Domen;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace drustvene_mreze.Repository
{
    public class GroupMembershipDbRepository
    {
        private readonly string connectionString;


        public GroupMembershipDbRepository(IConfiguration configuration)
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
    }
}
