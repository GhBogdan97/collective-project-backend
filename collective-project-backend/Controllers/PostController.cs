using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Services;


namespace API.Controllers
{
    [Route("posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;

        public PostController(PostService postService)
        {
            _postService = postService;
        }

        [HttpPost]
        public IActionResult SavePost(Post post)
        {
            try
            {
                _postService.SavePost(post);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }
    }
}