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
        GroupDbRepository groupDbRepository;

        public GrupeController(IConfiguration configuration)
        {
            groupDbRepository = new GroupDbRepository(configuration);
        }

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
            Grupe grupa = groupDbRepository.Create(novaGrupa);

            return Ok(grupa);
        }


        [HttpPut("{id}")]

        public ActionResult<Grupe> Update(int id, [FromBody] Grupe azuriranaGrupa)
        {
            if (azuriranaGrupa == null || string.IsNullOrWhiteSpace(azuriranaGrupa.naziv))
            {
                return BadRequest("Invalid group data.");
            }

            azuriranaGrupa.id = id;

            bool Izvrseno = groupDbRepository.Update(azuriranaGrupa);

            if (!Izvrseno)
            {
                return NotFound("Group not found.");
            }

            return Ok(azuriranaGrupa);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            bool Izvrseno = groupDbRepository.Delete(id);

            if (!Izvrseno)
            {
                return NotFound("Group not found.");
            }

            return NoContent();
        }

    }

}
