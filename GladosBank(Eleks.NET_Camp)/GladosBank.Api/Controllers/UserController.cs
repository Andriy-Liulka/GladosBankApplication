using GladosBank.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GladosBank.Services;
using GladosBank.Services.Exceptions;

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
            int newUserId = default;
            try
            {
                newUserId = -_service.CreateUser(user);
            }
            catch (AddingExistUserException ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }

            _logger.LogInformation("User was created sucessfuly");
            return Ok(newUserId);
        }
        [HttpPost]
        [ActionName(nameof(Delete))]
        public IActionResult Delete(int UserId)
        {
            try
            {
                _service.DeleteUser(UserId);
            }
            catch (InvalidUserIdException ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
            _logger.LogInformation("User was deleted sucessfuly");
            return Ok(UserId);
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
