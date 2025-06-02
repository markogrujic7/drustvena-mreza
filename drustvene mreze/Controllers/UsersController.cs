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
        private readonly UserDbRepository usersDbRepository = new();

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = usersDbRepository.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            User user = usersDbRepository.GetById(id);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            User createdUser = usersDbRepository.Create(user);
            if (createdUser == null)
                return StatusCode(500, "Greška pri kreiranju korisnika.");

            return Ok(createdUser);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, User user)
        {
            if (id != user.Id)
                return BadRequest("ID u URL i objektu se ne poklapaju.");

            bool success = usersDbRepository.Update(user);
            if (!success)
                return NotFound($"Korisnik sa ID {id} nije pronađen.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool success = usersDbRepository.Delete(id);
            if (!success)
                return NotFound($"Korisnik sa ID {id} nije pronađen ili nije obrisan.");

            return NoContent();
        }

    }

}
