using AutoMapper;
using GladosBank.Api.Config.Athentication;
using GladosBank.Api.Models.Args.UserControllerArgs;
using GladosBank.Domain.Models.Enums;
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
        public CustomerController(ILogger<CustomerController> logger, CustomerService service)
        {
            _service = service;
            _logger = logger;
        }
        [Authorize(Roles = RolesEnum.Worker)]
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
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        private readonly ICustomerService _service;
        private readonly ILogger<CustomerController> _logger;
    }
}
