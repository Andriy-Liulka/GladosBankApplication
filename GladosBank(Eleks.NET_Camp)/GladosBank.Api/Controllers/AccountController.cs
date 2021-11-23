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
        public AccountController(ILogger<AccountController> logger,AccountService service, IMapper mapper, DataService dataService)
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
        [HttpGet(nameof(Get))]
        [AllowAnonymous]
        public async  Task<IActionResult> Get()
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

                accounts = await _service.GetAllAccounts(currentLogin);
                _logger.LogInformation("Have got all accounts");
                return Ok(accounts);
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
        [HttpGet(nameof(GetAllCurrencies))]
        public IActionResult GetAllCurrencies()
        {
            try
            {
                var currenciesList = _service.GetAllCurrenciesService();
                _logger.LogInformation("You have got all currencies !");
                return Ok(currenciesList);
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
        [HttpGet(nameof(GetCurrencyCodeFromAccountId))]
        public IActionResult GetCurrencyCodeFromAccountId(int id)
        {
            try
            {
                var currency = _service.GetCurrencyFromId(id);
                _logger.LogInformation("You have got all currencies !");
                return Ok(currency);
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
        [HttpGet(nameof(GetAccountsForCurrencyCode))]
        public IActionResult GetAccountsForCurrencyCode([FromQuery]GetAccountsForCurrencyArgs args)
        {
            try
            {
                var accounts = _service.GetAllAccountsForCurrencyCode(args.CurrencyCode, args.Login);

                _logger.LogInformation("You have got all currencies !");
                return Ok(accounts);
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
        [HttpPost(nameof(TransactMoney))]
        public IActionResult TransactMoney(TransferMoneyArgs args)
        {
            try
            {
                if(!_dataService.IsYourAccount(Request.HttpContext.User.Claims, args.sourceId))
                {
                    throw new NotYourAccountException(args.sourceId);
                }
                (int,int) resultIds=_service.TransferMoney(args.Amount,args.sourceId,args.destinationId);
                return Ok($"Updated successfully from {resultIds.Item1} to {resultIds.Item2}");
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
        private readonly DataService _dataService;
        private readonly IMapper _mapper;

    }
}
