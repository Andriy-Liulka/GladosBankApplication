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
    public class CurrencyController : Controller
    {
        public CurrencyController(ILogger<CurrencyController> logger, AccountService service)
        {
            _logger = logger;
            _service = service;
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
                _logger.LogWarning(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

        }

        private readonly ILogger<CurrencyController> _logger;
        private readonly AccountService _service;
    }
}
