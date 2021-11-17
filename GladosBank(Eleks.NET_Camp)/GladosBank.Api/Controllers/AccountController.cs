using AutoMapper;
using GladosBank.Api.Models.Args.AccountControllerArgs;
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
    public class AccountController : Controller
    {
        public AccountController(ILogger<AccountController> logger,AccountService service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }
        [Authorize(Roles ="Customer")]
        [HttpPost(nameof(Create))]
        public IActionResult Create(CreateAccountArgs args)
        {
            IEnumerable<Claim> claims = this.Request.HttpContext.User.Claims;
            Claim claim = claims.FirstOrDefault(us => us.Type.Equals(ClaimTypes.Name));
            var userLogin = claim.Value;

            var localAccount = _mapper.Map<Account>(args.Account);

            localAccount.DateOfCreating = DateTime.Now;
            localAccount.CustomerId = _service.GetCustomerIdFromLogin(userLogin);


            try
            {
                _service.CreateAccount(localAccount);
            }
            catch (ExistingAccountException ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (InvalidCustomerException ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
            
            return Ok(localAccount.Id);
        }

        [Authorize(Roles = "Customer")]
        [HttpGet(nameof(Get))]
        [AllowAnonymous]
        public IActionResult Get()
        {

            IEnumerable<Claim> claims = this.Request.HttpContext.User.Claims;
            Claim claim = claims.FirstOrDefault(us => us.Type.Equals(ClaimTypes.Name));
            string currentLogin = claim?.Value;

            if (currentLogin==null)
            {
                _logger.LogInformation("Unauthorized");
                return BadRequest("Unauthorized");
            }

            IEnumerable<Account> accounts = default;
            try
            {
                accounts = _service.GetAllAccounts(currentLogin);
            }
            catch (IsntCustomerException ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
            return Ok(accounts);

        }

        [Authorize(Roles = "Customer")]
        [HttpGet(nameof(GetAllCurrencies))]
        public IActionResult GetAllCurrencies()
        {
            var currenciesList=_service.GetAllCurrencies();
            return Ok(currenciesList);
        }

        private readonly ILogger<AccountController> _logger;
        private readonly AccountService _service;
        private readonly IMapper _mapper;

    }
}
