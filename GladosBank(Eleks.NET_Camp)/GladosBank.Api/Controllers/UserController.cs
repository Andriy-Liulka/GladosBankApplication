using GladosBank.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GladosBank.Services;

namespace GladosBank.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        public UserController(ILogger<UserController> logger, UserService service)
        {
            this._service = service;
            _logger = logger;
        }
        
        [HttpPost]
        [ActionName(nameof(Create))]
        public IActionResult Create(User user)
        {
            
            int newUserId = -_service.CreateUser(user);
            if (newUserId==-1)
            {
                return BadRequest("User can't be created !");
            }
            _logger.LogInformation("User was created sucessfuly");
            return Ok(newUserId);
        }

        [HttpGet]
        [ActionName(nameof(Get))]
        public  IActionResult Get()
        {
            User[] users = _service.GetAllUsers();
            return Ok(users);
        }


        private readonly UserService _service;
        private readonly ILogger<UserController> _logger;
    }
}
