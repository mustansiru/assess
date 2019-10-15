using System;
using assess.Models;
using assess.Services.Intefaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace assess.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            var user = _service.Authenticate(model.Username, model.Password);

            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            return Ok(user);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _service.GetAll();
            return Ok(users);
        }
    }
}
