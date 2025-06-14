using drustvene_mreze.Domen;
using drustvene_mreze.Repository;
using Microsoft.AspNetCore.Mvc;

namespace drustvene_mreze.Controllers
{
    [ApiController]
    [Route("posts")]
    public class PostController : ControllerBase
    {
        private readonly PostDbRepository _postRepository;

        public PostController(IConfiguration configuration)
        {
            _postRepository = new PostDbRepository(configuration);
        }

        [HttpGet("posts")]
        public IActionResult GetAll()
        {
            var posts = _postRepository.GetAll();
            return Ok(posts);
        }
    }

}
