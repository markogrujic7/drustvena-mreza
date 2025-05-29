using System.Data;
using drustvene_mreze.Domen;
using drustvene_mreze.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace drustvene_mreze.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly UserRepozitorijum _repo = new();

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = GetAllFromDatabase();
            return Ok(users);
        }

        private List<User> GetAllFromDatabase()
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

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = _repo.GetById(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            _repo.Add(user);
            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, User user)
        {
            if (id != user.Id) return BadRequest();
            _repo.Update(user);
            return NoContent();
        }

    }

}
