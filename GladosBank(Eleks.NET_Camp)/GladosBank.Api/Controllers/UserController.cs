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

namespace GladosBank.Api.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public sealed class UserController : ControllerBase
    {
        //TO DO fix bug with adding service of IMapper
        public UserController(ILogger<UserController> logger, UserService service,IMapper mapper, JwtGenerator jwtGenerator, DataService dataService)
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
                if (newUserId==0)
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

                var Role = _service.GetRole(args.Login);

                var token = _jwtGenerator.CreateJwtToken(existingUser, Role);
                var result = new { Login = args.Login, JwtToken = token };

                return Ok(result);
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

        [Authorize(Roles ="Admin")]
        [HttpPost(nameof(Delete))]
        public IActionResult Delete(DeleteUserArgs user)
        {
            try
            {
                _service.DeleteUser(user.UserId);
                _logger.LogInformation("User was deleted sucessfuly");
                return Ok(user.UserId);
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
        }
        [Authorize]
        [HttpPost(nameof(Update))]
        public IActionResult Update(UpdateUserArgs user)
        {
            try
            {
                IEnumerable<Claim> claims=this.Request.HttpContext.User.Claims;
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
                _logger.LogInformation("User was updated sucessfuly");

                return Ok(currentUser?.Id);
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
        [HttpGet(nameof(Get))]
        public  IActionResult Get()
        {
            IEnumerable<User> users = _service.GetAllUsers();
            _logger.LogInformation("You have got all users !");
            return Ok(users);
        }

        [Authorize]
        [HttpGet(nameof(GetUserData))]
        public IActionResult GetUserData()
        {
            IEnumerable<Claim> claims = this.Request.HttpContext.User.Claims;

            var userLogin = _dataService.GetLogin(claims);

            var userEmail = _dataService.GetEmail(claims);

            var userRole = _dataService.GetRole(claims);

            var returnedData = new {Name= userLogin, Email= userEmail, Role= userRole };
            _logger.LogInformation("You got all user data !");
            return Ok(returnedData);
        }
        private readonly IMapper _mapper;
        private readonly JwtGenerator _jwtGenerator;
        private readonly UserService _service;
        private readonly DataService _dataService;
        private readonly ILogger<UserController> _logger;
    }
}
