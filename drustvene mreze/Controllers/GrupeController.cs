using drustvene_mreze.Domen;
using drustvene_mreze.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace drustvene_mreze.Controllers
{
    [Route("api/grupe")]
    [ApiController]
    public class GrupeController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Grupe>> GetAll()
        {
            GrupeRepozitorijum grupeRepozitorijum = new GrupeRepozitorijum();
            List<Grupe> grupe = GrupeRepozitorijum.Podaci.Values.ToList();

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

    }

}
