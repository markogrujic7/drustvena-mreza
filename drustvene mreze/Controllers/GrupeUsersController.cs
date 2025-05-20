using drustvene_mreze.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace drustvene_mreze.Controllers
{
    [Route("api/GrupeUsers")]
    [ApiController]
    public class GrupeUsersController : ControllerBase
    {
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

        [HttpPut("add/{groupId}/{userId}")]
        public IActionResult AddUserToGroup(int groupId, int userId)
        {
            var clanstvoRepo = new ClanstvoRepozitorijum();
            var userRepo = new UserRepozitorijum();
            var user = userRepo.GetById(userId);

            if (user == null)
            {
               return NotFound("User not found.");
            }

            clanstvoRepo.Add(userId,groupId);
            return Ok("User added to group successfully.");
        }

        [HttpDelete("remove/{groupId}/{userId}")]

        public IActionResult RemoveUserFromGroup(int groupId, int userId)
        {
            var clanstvoRepo = new ClanstvoRepozitorijum();
            var userRepo = new UserRepozitorijum();
            var user = userRepo.GetById(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            var clanstva = clanstvoRepo.GetAll();
            var clanstvoZaBrisanje = clanstva.FirstOrDefault(c => c.UserId == userId && c.GroupId == groupId);
            if (clanstvoZaBrisanje != default)
            {
                clanstva.Remove(clanstvoZaBrisanje);
                clanstvoRepo.Sacuvaj(clanstva);
                return Ok("User removed from group successfully.");
            }
            return NotFound("Membership not found.");
        }
    }
}
