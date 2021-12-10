using GladosBank.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GladosBank.Services;
using GladosBank.Services.Exceptions;
using GladosBank.Api.Models.Args.UserControllerArgs;
using AutoMapper;
using GladosBank.Domain.Models_DTO;
using Microsoft.AspNetCore.Authorization;
using GladosBank.Api.Config.Athentication;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using GladosBank.Api.Models.Args.AccountControllerArgs;

namespace GladosBank.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public sealed class UserController : ControllerBase
    {
        //TO DO fix bug with adding service of IMapper
        public UserController(ILogger<UserController> logger, UserService service, IMapper mapper, JwtGenerator jwtGenerator, ClaimReader dataService)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
            _jwtGenerator = jwtGenerator;
            _dataService = dataService;
        }

        [AllowAnonymous]
        [HttpPost(nameof(Create))]
        public IActionResult Create(CreateUserArgs user)
        {
            int newUserId = default;

            var localUser = _mapper.Map<User>(user.MyUser);

            try
            {
                newUserId = _service.CreateUser(localUser, user.Role);
                if (newUserId == 0)
                {
                    throw new InvalidUserSavingException("Exception occurs during a saving procedure");
                }
                _logger.LogInformation("User was created sucessfuly");
                return Ok(newUserId);
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
        [AllowAnonymous]
        [HttpPost(nameof(Login))]
        public IActionResult Login(LoginUserArgs args)
        {
            try
            {
                User existingUser = existingUser = _service.GetUserByLogin(args.Login);
                if (!args.PasswordHash.Equals(existingUser.PasswordHash))
                {
                    return BadRequest("Incorrect password or login !");
                }

                _service.IsActive(args.Login);

                var Role = _service.GetRole(args.Login);

                var token = _jwtGenerator.CreateJwtToken(existingUser, Role);
                var result = new { Login = args.Login, JwtToken = token };

                return Ok(result);
            }
            catch (BusinessLogicException ex)
            {
                switch (ex)
                {
                    case InvalidUserLoginException:
                        {
                            _logger.LogInformation("Incorrect login or password");
                            return BadRequest("Incorrect login or password");
                        }
                    case UserWasBannedException:
                        {
                            _logger.LogInformation("Your account was banned");
                            return BadRequest("Your account was banned");
                        }

                }
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(nameof(Delete))]
        public IActionResult Delete(DeleteUserArgs user)
        {
            try
            {
                _service.DeleteUser(user.UserId);
                _logger.LogInformation("User was deleted sucessfuly");
                return Ok(user.UserId);
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

        [Authorize]
        [HttpPost(nameof(Update))]
        public IActionResult Update(UpdateUserArgs user)
        {
            try
            {
                IEnumerable<Claim> claims = this.Request.HttpContext.User.Claims;
                string currentLogin = _dataService.GetLogin(claims);
                User currentUser = default;

                currentUser = _service.GetUserByLogin(currentLogin);
                if (!string.IsNullOrWhiteSpace(user.Login))
                {
                    currentUser.Login = user.Login;
                }
                if (!string.IsNullOrWhiteSpace(user.Phone))
                {
                    currentUser.Phone = user.Phone;
                }
                if (!string.IsNullOrWhiteSpace(user.Email))
                {
                    currentUser.Email = user.Email;
                }
                _service.UpdateUser(currentUser.Id, currentUser);


                var token = _jwtGenerator.CreateJwtToken(currentUser, user.Role);

                var returnData = new { Id = currentUser?.Id, Token = token };


                _logger.LogInformation("User was updated sucessfuly");
                return Ok(returnData);
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

        [Authorize(Roles = "Admin")]
        [HttpGet(nameof(Get))]
        public IActionResult Get()
        {
            try
            {
                var users = _service.GetAllUsers();
                _logger.LogInformation("You have got all users !");
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet(nameof(GetPaginatedList))]
        public IActionResult GetPaginatedList([FromQuery]PaginatedArgs args)
        {
            try
            {
                var users = _service.GetPaginatedUsersList(args.pageIndex, args.pageSize);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Worker")]
        [HttpGet(nameof(GetPaginatedListOfCustomers))]
        public  IActionResult GetPaginatedListOfCustomers([FromQuery] PaginatedArgs args)
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

        [Authorize]
        [HttpGet(nameof(GetUserData))]
        public IActionResult GetUserData()
        {
            try
            {
                var claims = this.Request.HttpContext.User.Claims;

                var userLogin = _dataService.GetLogin(claims);

                var userEmail = _dataService.GetEmail(claims);

                var userRole = _dataService.GetRole(claims);

                var userPhone = _dataService.GetPhone(claims);

                var returnedData = new {Phone= userPhone, Name = userLogin, Email = userEmail, Role = userRole };

                _logger.LogInformation("You got all user data !");
                return Ok(returnedData);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }

        }

        [Authorize(Roles ="Admin")]
        [HttpPost(nameof(BlockUnblockUser))]
        public IActionResult BlockUnblockUser([FromQuery]int UserId)
        {
            try
            {
                var existingUserId = _service.BlockUnblockUser(UserId);
                return Ok(existingUserId);
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
                int customerId=_dataService.GetCustomerId(claims);

                OperationsHistory newHistory = _mapper.Map<OperationsHistory>(args);
                newHistory.DateTime = DateTime.UtcNow;
                newHistory.CustomerId = customerId;

                var savedElementId=_service.KeepHistoryElementOfOperation(newHistory);

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
        private readonly IMapper _mapper;
        private readonly JwtGenerator _jwtGenerator;
        private readonly UserService _service;
        private readonly ClaimReader _dataService;
        private readonly ILogger<UserController> _logger;
    }
}
