using drustvene_mreze.Domen;
using drustvene_mreze.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace drustvene_mreze.Controllers
{
    [Route("api/grupe")]
    [ApiController]
    public class GrupeController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Grupe>> GetAll()
        {
            List<Grupe> grupe = GetAllFromDatabase();

            return Ok(grupe);
        }

        [HttpPost]
        public ActionResult<Grupe> Create([FromBody] Grupe novaGrupa)
        {
            if (novaGrupa == null || string.IsNullOrWhiteSpace(novaGrupa.naziv))
            {
                return BadRequest("Invalid group data.");
            }
            GrupeRepozitorijum grupeRepozitorijum = new GrupeRepozitorijum();
            int noviId = GrupeRepozitorijum.Podaci.Keys.Max() + 1;
            novaGrupa.id = noviId;
            GrupeRepozitorijum.Podaci.Add(noviId, novaGrupa);
            grupeRepozitorijum.Sacuvaj();
            return Ok(novaGrupa);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            GrupeRepozitorijum grupeRepozitorijum = new GrupeRepozitorijum();
            if (!GrupeRepozitorijum.Podaci.ContainsKey(id))
            {
                return NotFound("Group not found.");
            }
            GrupeRepozitorijum.Podaci.Remove(id);
            grupeRepozitorijum.Sacuvaj();
            return NoContent();
        }


        private static List<Grupe> GetAllFromDatabase()
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
    }

}
