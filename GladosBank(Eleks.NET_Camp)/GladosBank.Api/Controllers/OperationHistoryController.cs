using AutoMapper;
using GladosBank.Api.Models.Args.AccountControllerArgs;
using GladosBank.Api.Models.Args.UserControllerArgs;
using GladosBank.Domain;
using GladosBank.Services;
using GladosBank.Services.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GladosBank.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OperationHistoryController : Controller
    {
        public OperationHistoryController(ILogger<OperationHistoryController> logger, AccountService service, ClaimReader claimReader, IMapper mapper,UserService uservice)
        {
            _logger = logger;
            _uservice = uservice;
            _service = service;
            _mapper = mapper;
            _claimReader = claimReader;
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
        [Authorize(Roles = "Customer")]
        [HttpPost(nameof(KeepHistoryOfOperation))]
        public IActionResult KeepHistoryOfOperation(KeepHistoryOfOperationArgs args)
        {
            try
            {
                IEnumerable<Claim> claims = Request.HttpContext.User.Claims;
                int customerId = _claimReader.GetCustomerId(claims);

                OperationsHistory newHistory = _mapper.Map<OperationsHistory>(args);
                newHistory.DateTime = DateTime.UtcNow;
                newHistory.CustomerId = customerId;

                var savedElementId = _uservice.KeepHistoryElementOfOperation(newHistory);

                return Ok(savedElementId);
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
        private readonly ILogger<OperationHistoryController> _logger;
        private readonly AccountService _service;
        private readonly UserService _uservice;
        private readonly ClaimReader _claimReader;
        private readonly IMapper _mapper;
    }
}
