using GladosBank.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GladosBank.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }
        
        [HttpPost]
        [ActionName("GetUsers")]
        public IActionResult Get()
        {
            var user = new
            {
                Login="LoginEntered",
                Password="PasswordEntered"
            };
            return Ok(user);

        }

        private readonly ILogger<UserController> _logger;
    }
}
