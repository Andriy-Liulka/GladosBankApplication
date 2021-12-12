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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GladosBank.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        public AccountController(ILogger<AccountController> logger,AccountService service, IMapper mapper, ClaimReader dataService)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
            _dataService = dataService;
        }

        [Authorize(Roles ="Customer")]
        [HttpPost(nameof(Create))]
        public IActionResult Create(CreateAccountArgs args)
        {
            try
            {
                IEnumerable<Claim> claims = this.Request.HttpContext.User.Claims;
                var userLogin =_dataService.GetLogin(claims);

                var localAccount = _mapper.Map<Account>(args.Account);

                localAccount.DateOfCreating = DateTime.Now;
                localAccount.CustomerId = _service.GetCustomerIdFromLogin(userLogin);
            
                _service.CreateAccount(localAccount);
                _logger.LogInformation($"Account with id->{localAccount.Id} was created successfully");
                return Ok(localAccount.Id);
            }
            catch (BusinessLogicException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

        }

        [Authorize(Roles = "Customer")]
        [HttpGet(nameof(Get))]
        [AllowAnonymous]
        public  IActionResult Get()
        {
            try
            {
                IEnumerable<Claim> claims = this.Request.HttpContext.User.Claims;
                var currentLogin = _dataService.GetLogin(claims);

                if (currentLogin==null)
                {
                    _logger.LogInformation("Unauthorized");
                    return BadRequest("Unauthorized");
                }

                IEnumerable<Account> accounts = default;

                accounts = _service.GetAllUserAccounts(currentLogin);
                _logger.LogInformation("Have got all accounts");
                return Ok(accounts);
            }
            catch (BusinessLogicException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Customer")]
        [HttpGet(nameof(GetAccountsFromCurrencyCode))]
        public IActionResult GetAccountsFromCurrencyCode([FromQuery]GetAccountsFromCurrencyArgs args)
        {
            try
            {
                var accounts = _service.GetAllAccountsForCurrencyCode(args.CurrencyCode, args.Login);

                _logger.LogInformation("You have got all currencies !");
                return Ok(accounts);
            }
            catch (BusinessLogicException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Customer")]
        [HttpPost(nameof(Delete))]
        public IActionResult Delete(DeleteAccountArgs account)
        {
            try
            {
                var deletedAccountId=_service.DeleteAccount(account.Id);
                _logger.LogInformation("Account was deleted successfully !");
                return Ok(deletedAccountId);
            }
            catch (BusinessLogicException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }


        }

        [Authorize(Roles = "Customer")]
        [HttpPost(nameof(Replenish))]
        public IActionResult Replenish(ReplenishAccountArgs account)
        {
            try
            {
                var replenishedAccountId = _service.ReplenishAccount(account.Id,account.Amount);
                _logger.LogInformation($"Account with Id->{account.Id} was replenished on {account.Amount} !");
                return Ok(replenishedAccountId);
            }
            catch (BusinessLogicException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Customer")]
        [HttpPost(nameof(TransactMoney))]
        public IActionResult TransactMoney(TransferMoneyArgs args)
        {
            try
            {
                if(!_dataService.IsYourAccount(Request.HttpContext.User.Claims, args.sourceId))
                {
                    throw new NotYourAccountException(args.sourceId);
                }
                var source = _service.GetAccountFromId(args.sourceId);
                var destination = _service.GetAccountFromId(args.destinationId);

                bool finishedSuccessfully= _service.TransferMoneySaver(args.Amount, source, destination);

                if (finishedSuccessfully)
                {
                    return Ok($"Updated successfully from {args.sourceId} to {args.destinationId}");
                }
                return BadRequest("Operation failed !");
            }
            catch (BusinessLogicException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _service;
        private readonly ClaimReader _dataService;
        private readonly IMapper _mapper;

    }
}
