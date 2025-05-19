using Microsoft.AspNetCore.Mvc;

namespace drustvene_mreze.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly UserRepozitorijum _repo = new();

        [HttpGet]
        public IActionResult GetAll() => Ok(_repo.GetAll());

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

        [HttpGet("group/{groupId}")]
        public IActionResult GetUsersByGroup(int groupId)
        {
            var clanstvoRepo = new ClanstvoRepozitorijum();
            var userRepo = new UserRepozitorijum();

            var userIds = clanstvoRepo.GetUserIdsByGroupId(groupId);
            var sviKorisnici = userRepo.GetAll();

            var korisniciUGrupi = sviKorisnici.Where(u => userIds.Contains(u.Id)).ToList();

            return Ok(korisniciUGrupi);
        }

    }

}
