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
        public IActionResult Create()
        {

            return Ok();

        }

        //[HttpGet]
        //[ActionName(nameof(Get))]
        //public async Task<IEnumerable<User>> Get()
        //{
        //    return await _service;
        //}


        private readonly UserService _service;
        private readonly ILogger<UserController> _logger;
    }
}
