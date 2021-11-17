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
        public UserController(ILogger<UserController> logger, UserService service,IMapper mapper, JwtGenerator jwtGenerator)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
            _jwtGenerator = jwtGenerator;
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
            }
            catch (AddingExistUserException ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (InvalidRoleException ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
            catch(ExistingUserLoginException ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }

            _logger.LogInformation("User was created sucessfuly");
            return Ok(newUserId);
        }
        [AllowAnonymous]
        [HttpPost(nameof(Login))]
        public IActionResult Login(LoginUserArgs args)
        {
            User existingUser=default;
            try
            {
                existingUser = _service.GetUserByLogin(args.Login);
            }
            catch (InvalidUserLoginException ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
            if (!args.PasswordHash.Equals(existingUser.PasswordHash))
            {
                return BadRequest("Incorrect password or login !");
            }
            string Role = default;
            try
            {
                Role = _service.GetRole(args.Login);
            }
            catch (DonotHaveRoleException ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
            
            var token = _jwtGenerator.CreateJwtToken(existingUser,Role);
            var result = new {Login= args.Login, JwtToken =token };

            return Ok(result);
        }

        [Authorize(Roles ="Admin")]
        [HttpPost(nameof(Delete))]
        public IActionResult Delete(DeleteUserArgs user)
        {
            try
            {
                _service.DeleteUser(user.UserId);
            }
            catch (InvalidUserIdException ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
            _logger.LogInformation("User was deleted sucessfuly");
            return Ok(user.UserId);
        }
        [Authorize]
        [HttpPost(nameof(Update))]
        public IActionResult Update(UpdateUserArgs user)
        {

            IEnumerable<Claim> claims=this.Request.HttpContext.User.Claims;
            Claim claim=claims.FirstOrDefault(us => us.Type.Equals(ClaimTypes.Name));
            string currentLogin = claim.Value;


            foreach (var item in claims)
            {
                var t=item.Value;
            }  

            //current login must be finded on it's own
            User currentUser = default;
            try
            {
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
            }
            catch (InvalidUserLoginException ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (InvalidUserIdException ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
            _logger.LogInformation("User was updated sucessfuly");
            return Ok(currentUser?.Id);
        }
        [Authorize]
        [HttpGet(nameof(Get))]
        public  IActionResult Get()
        {
            IEnumerable<User> users = _service.GetAllUsers();
            return Ok(users);
        }

        [Authorize]
        [HttpGet(nameof(GetUserDataFromJwt))]
        public IActionResult GetUserDataFromJwt(string JwtToken)
        {
            var token = JwtToken;
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);

            string name=jwtSecurityToken.Claims.FirstOrDefault(us => us.Type == "unique_name").Value;
            string email = jwtSecurityToken.Claims.FirstOrDefault(us=>us.Type=="email").Value;
            string role = jwtSecurityToken.Claims.FirstOrDefault(us => us.Type == "role").Value;

            var returnedData = new {Name=name,Email=email,Role=role };
            return Ok(returnedData);
        }
        private readonly IMapper _mapper;
        private readonly JwtGenerator _jwtGenerator;
        private readonly UserService _service;
        private readonly ILogger<UserController> _logger;
    }
}
