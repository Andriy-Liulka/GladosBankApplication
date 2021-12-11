using GladosBank.Api.Models.Args.UserControllerArgs;
using GladosBank.Services;
using GladosBank.Services.Exceptions;
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
    public class OperationHistoryController : Controller
    {
        public OperationHistoryController(ILogger<AccountController> logger, AccountService service)
        {
            _logger = logger;
            _service = service;
        }
        [Authorize(Roles = "Worker")]
        [HttpGet(nameof(GetTransactionHistoryElements))]
        public IActionResult GetTransactionHistoryElements([FromQuery] PaginatedArgs args, int customerId)
        {
            try
            {
                var historyElements = _service.GetTransactionHistoryElementService(args.pageIndex, args.pageSize, customerId);
                return Ok(historyElements);
            }
            catch (BusinessLogicException ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        private readonly ILogger<AccountController> _logger;
        private readonly AccountService _service;
    }
}
