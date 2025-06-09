using drustvene_mreze.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace drustvene_mreze.Controllers
{
    [Route("api/GrupeUsers")]
    [ApiController]
    public class GrupeUsersController : ControllerBase
    {
        ClanstvoRepozitorijum clanstvoRepozitorijum;

        public GrupeUsersController(IConfiguration configuration)
        {
            clanstvoRepozitorijum = new ClanstvoRepozitorijum(configuration);
        }

        [HttpGet("group/{groupId}")]
        public IActionResult GetUsersByGroup(int groupId)
        {
            var userRepo = new UserRepozitorijum();

            var userIds = clanstvoRepozitorijum.GetUserIdsByGroupId(groupId);
            var sviKorisnici = userRepo.GetAll();

            var korisniciUGrupi = sviKorisnici.Where(u => userIds.Contains(u.Id)).ToList();

            return Ok(korisniciUGrupi);
        }

        [HttpPut("add/{groupId}/{userId}")]
        public IActionResult AddUserToGroup(int groupId, int userId)
        {
            clanstvoRepozitorijum.Create(userId,groupId);
            return Ok("User added to group successfully.");
        }

        [HttpDelete("remove/{groupId}/{userId}")]

        public IActionResult RemoveUserFromGroup(int groupId, int userId)
        {
            clanstvoRepozitorijum.Remove(userId, groupId);
            return NoContent();
        }
    }
}
