﻿using System.Data;
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
        UserDbRepository usersDbRepository;

        public UsersController(IConfiguration configuration)
        {
            usersDbRepository = new UserDbRepository(configuration);
        }

        [HttpGet]
        public IActionResult GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest("Stranica i veličina stranice moraju biti pozitivni brojevi.");

            try
            {
                List<User> users = usersDbRepository.GetAll(page, pageSize);
                int total = usersDbRepository.CountAll();

                var result = new
                {
                    Count = total,
                    Page = page,
                    PageSize = pageSize,
                    Users = users
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška na serveru: {ex.Message}");
            }
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
