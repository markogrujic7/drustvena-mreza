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
        GroupDbRepository groupDbRepository = new GroupDbRepository();

        [HttpGet]
        public ActionResult<List<Grupe>> GetAll()
        {
            List<Grupe> grupe = groupDbRepository.GetAll();

            return Ok(grupe);
        }

        [HttpGet("{id}")]
        public ActionResult <Grupe> GetById(int id)
        {
            Grupe grupa = groupDbRepository.GetById(id);

            if (grupa == null)
            {
                return NotFound();
            }

            return Ok(grupa);
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

    }

}
