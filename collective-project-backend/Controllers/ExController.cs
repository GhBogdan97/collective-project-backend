using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("Ex")]
    [ApiController]
    public class ExController : ControllerBase
    {
        [HttpGet]
        [Route("Jucarii")]
        public IActionResult GetJucarii()
        {
            return Ok();
        }
    }
}