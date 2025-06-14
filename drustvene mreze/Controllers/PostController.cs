using drustvene_mreze.Domen;
using drustvene_mreze.Repository;
using Microsoft.AspNetCore.Mvc;

namespace drustvene_mreze.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostController : ControllerBase
    {
        private readonly PostDbRepository _postRepository;
        private readonly UserDbRepository _userRepository;


        public PostController(IConfiguration configuration)
        {
            _postRepository = new PostDbRepository(configuration);
            _userRepository = new UserDbRepository(configuration);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var posts = _postRepository.GetAll();
            return Ok(posts);
        }

        [HttpPost("/users/{userId}/posts")]
        public IActionResult CreatePost(int userId, [FromBody] Post post)
        {
            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                return NotFound($"Korisnik sa ID {userId} ne postoji.");
            }

            if (string.IsNullOrWhiteSpace(post.Content))
            {
                return BadRequest("Sadržaj objave je obavezan.");
            }

            post.UserId = userId;
            post.Date = DateTime.Now;

            Post uspeh = _postRepository.Create(post);
            if (uspeh == null)
            {
                return StatusCode(500, "Greška prilikom dodavanja objave.");
            }

            return Ok(post);
        }
    }
}
