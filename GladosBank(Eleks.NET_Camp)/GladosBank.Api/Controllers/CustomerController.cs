using AutoMapper;
using GladosBank.Api.Config.Athentication;
using GladosBank.Api.Models.Args.UserControllerArgs;
using GladosBank.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GladosBank.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        public CustomerController(ILogger<CustomerController> logger, UserService service)
        {
            _service = service;
            _logger = logger;
        }
        [Authorize(Roles = "Worker")]
        [HttpGet(nameof(GetPaginatedListOfCustomers))]
        public IActionResult GetPaginatedListOfCustomers([FromQuery] PaginatedArgs args)
        {
            try
            {
                var users = _service.GetPaginatedUsersListOfCustomers(args.pageIndex, args.pageSize);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        private readonly UserService _service;
        private readonly ILogger<CustomerController> _logger;
    }
}
